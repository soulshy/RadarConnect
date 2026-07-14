using Kitware.VTK;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using Org.BouncyCastle.Utilities.Net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace RadarConnect
{
    public partial class Form1 : Form
    {
        // ==========================================
        // 配置区域
        // ==========================================
        private const string FIXED_LOCAL_IP = "192.168.1.230"; // 本机 IP
        private const int TARGET_LIDAR_PORT = 65000;
        private const int LOCAL_CMD_PORT = 50001;
        private const int LOCAL_DATA_PORT = 60000;

        // ==========================================
        // 成员变量
        // ==========================================
        private UdpCommunication _udpClient;
        private DatabaseManager _dbManager;
        private string _currentDeviceIp;
        private bool _isSaving = false;

        private PointCloudProcessor _processor = new PointCloudProcessor();

        // 队列
        private ConcurrentQueue<List<PointData>> _uiDataQueue = new ConcurrentQueue<List<PointData>>();

        private System.Windows.Forms.Timer _heartbeatTimer;

        // 时间同步
        private ulong _lastRadarTimestamp = 0;
        private bool _isTimeSynced = false;
        private long _basePcTicks = 0;
        private ulong _baseRadarTime = 0;
        private byte _lastRadarTimestampType = byte.MaxValue;
        private byte _lastTimeSyncStatus = byte.MaxValue;
        private long _packetsSinceLastStats = 0;
        private long _pointsSinceLastStats = 0;
        private int _statsElapsedSeconds = 0;
        private long _lastDroppedDataPackets = 0;

        private static readonly DateTime UnixEpochUtc =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // VTK 可视化包装器
        private VtkVisualizer _vtkVisualizer;
        private VtkVisualizer _vtkVisualizer2;

        // 预处理复用缓冲区
        private List<PointData> _displayBuffer = new List<PointData>(300000);

        private VideoProcessor _videoProcessor = new VideoProcessor();
        private SensorFusion _sensorFusion = new SensorFusion();
        private ImageProcessor _imageProcessor = new ImageProcessor();

        // ==========================================
        // 相机视频播放器变量
        // ==========================================
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private VideoView _videoView;
        private bool _isCameraPlaying = false;
        private Size _videoPanelMaximumSize;
        private Point _videoPanelTopRight;

        // ==========================================
        // UDP 云台控制
        // ==========================================
        private PtzUdpController _ptzController;

        public Form1()
        {
            InitializeComponent();
            // 保存设计器中为视频预留的最大区域。后续根据实际码流比例动态调整。
            _videoPanelMaximumSize = panel_Video.ClientSize;
            _videoPanelTopRight = new Point(panel_Video.Right, panel_Video.Top);
            InitPtzPage();

            _udpClient = new UdpCommunication();
            _dbManager = new DatabaseManager();
            _dbManager.OnError += (message) =>
            {
                AddLog("[数据库] " + message);
            };

            // 订阅数据库多线程安全结束事件
            _dbManager.OnSaveFinished += () =>
            {
                this.BeginInvoke((MethodInvoker)delegate {
                    AddLog(">>> 后台剩余点云已全部安全入库，写入线程已彻底结束。");
                });
            };

            _heartbeatTimer = new System.Windows.Forms.Timer();
            _heartbeatTimer.Interval = 1000;
            _heartbeatTimer.Tick += OnHeartbeatTimerTick;

            _udpClient.OnBroadcastReceived += HandleBroadcast;
            _udpClient.OnCmdAckReceived += HandleAck;
            _udpClient.OnDataReceived += ProcessPointCloud;
            _udpClient.OnError += (msg) => this.BeginInvoke((MethodInvoker)delegate { AddLog($"错误: {msg}"); });

            // 配置预处理器参数
            _processor.DownsampleFactor = 1;
            _processor.MinReflectivity = 10;
            _processor.EnableRoiFilter = true;
            _processor.MinZ = -2.0f;
            _processor.MaxZ = 50.0f;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 初始化相机播放器
            InitCameraPlayer();
        }

        #region 相机视频流控制

        private void InitCameraPlayer()
        {
            try
            {
                // 初始化 LibVLC 核心
                Core.Initialize();

                _libVLC = new LibVLC(
                          "--no-osd",
                          "--rtsp-tcp",
                          "--network-caching=300",
                          "--live-caching=300",
                          "--rtsp-frame-buffer-size=2000000",
                          "--drop-late-frames",
                          "--skip-frames",
                          "--avcodec-hw=dxva2",
                          "--vout=direct3d9"
                        );
                _mediaPlayer = new MediaPlayer(_libVLC);

                // 动态创建 VideoView 并填充到 panel_Video 容器中
                _videoView = new VideoView
                {
                    MediaPlayer = _mediaPlayer,
                    Dock = DockStyle.Fill,
                    BackColor = Color.Black
                };

                panel_Video.Controls.Add(_videoView);
                AddLog("视频播放器初始化成功。");
            }
            catch (Exception ex)
            {
                AddLog($"视频播放器初始化失败: {ex.Message}");
            }
        }

        private async void btn_PlayCamera_Click(object sender, EventArgs e)
        {
            if (_libVLC == null)
            {
                MessageBox.Show("播放器未正确初始化！");
                return;
            }

            if (!_isCameraPlaying)
            {
                string inputIp = txt_CameraIp.Text.Trim();
                if (string.IsNullOrEmpty(inputIp))
                {
                    MessageBox.Show("请输入相机IP地址！");
                    return;
                }

                string ipAddress = inputIp.Contains(":") ? inputIp.Split(':')[0] : inputIp;

                btn_PlayCamera.Enabled = false;
                btn_PlayCamera.Text = "正在请求接口...";
                AddLog("1. 正在调用 HTTP 接口验证相机状态...");

                try
                {
                    string user = "admin";
                    string pwdMd5 = "e10adc3949ba59abbe56e057f20f883e";
                    string apiUrl = $"http://{ipAddress}/action/cgi_action?user={user}&pwd={pwdMd5}&action=getRtspConf";
                    await SyncCameraTimeAsync(ipAddress, user, pwdMd5);
                    using (HttpClient client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(3);

                        HttpResponseMessage response = await client.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            AddLog("2. 接口返回成功，校验配置中...");

                            if (jsonResponse.Contains("\"code\": 0") || jsonResponse.Contains("\"code\":0"))
                            {
                                int rtspPort = 554; // 默认端口
                                Match match = Regex.Match(jsonResponse, "\"rtsp_port\":\\s*(\\d+)");
                                if (match.Success)
                                {
                                    rtspPort = int.Parse(match.Groups[1].Value);
                                }

                                string rtspUrl = $"rtsp://{ipAddress}:{rtspPort}/stream_0";
                                var media = new Media(_libVLC, rtspUrl, FromType.FromLocation);
                                media.AddOption(":rtsp-tcp");
                                media.AddOption(":rtsp-frame-buffer-size=2000000");
                                media.AddOption(":network-caching=500");
                                media.AddOption(":live-caching=500");
                                media.AddOption(":no-sout-audio");

                                string recordFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CameraRecords");
                                if (!System.IO.Directory.Exists(recordFolder))
                                {
                                    System.IO.Directory.CreateDirectory(recordFolder);
                                }
                                string fileName = $"Record_{DateTime.Now:yyyyMMdd_HHmmss}.mp4";
                                string savePath = System.IO.Path.Combine(recordFolder, fileName).Replace("\\", "/");
                                string soutOption = $":sout=#duplicate{{dst=display,dst=std{{access=file,mux=mp4,dst='{savePath}'}}}}";

                                media.AddOption(soutOption);

                                AddLog($"视频将同步录制到: {savePath}");
                                _mediaPlayer.Playing += MediaPlayer_Playing;
                                _mediaPlayer.EncounteredError += MediaPlayer_EncounteredError;

                                AddLog("3. 配置检查OK，准备连接 RTSP 视频流...");
                                btn_PlayCamera.Text = "连接 RTSP...";

                                _mediaPlayer.Play(media);

                                // 使用码流自身的宽高比进行等比缩放，避免按 panel_Video
                                // 的尺寸强制拉伸画面。容器比例不一致时由 VLC 自动留黑边。
                                _mediaPlayer.AspectRatio = null;
                            }
                            else
                            {
                                MessageBox.Show("相机返回错误代码，请检查配置！");
                                btn_PlayCamera.Text = "播放相机";
                                btn_PlayCamera.Enabled = true;
                            }
                        }
                        else
                        {
                            AddLog($"接口 HTTP 请求失败，状态码: {response.StatusCode}");
                            btn_PlayCamera.Text = "播放相机";
                            btn_PlayCamera.Enabled = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    AddLog($"连接异常: {ex.Message}");
                    btn_PlayCamera.Text = "播放相机";
                    btn_PlayCamera.Enabled = true;
                }
            }
            else
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Playing -= MediaPlayer_Playing;
                _mediaPlayer.EncounteredError -= MediaPlayer_EncounteredError;

                _isCameraPlaying = false;
                btn_PlayCamera.Text = "播放相机";
                AddLog("已断开视频流。");
            }
        }

        private void MediaPlayer_Playing(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                ResizeVideoPanelToStream();
                _isCameraPlaying = true;
                btn_PlayCamera.Enabled = true;
                btn_PlayCamera.Text = "停止视频";
                AddLog("4. 视频流连接成功，正在播放！");
            }));
        }

        /// <summary>
        /// 按实际码流宽高比，把视频面板调整为预留区域内的最大尺寸。
        /// 面板与码流比例一致，因此无需拉伸、裁剪或用黑边补齐。
        /// </summary>
        private void ResizeVideoPanelToStream()
        {
            if (_mediaPlayer == null || panel_Video == null)
                return;

            uint streamWidth = 0;
            uint streamHeight = 0;
            if (!_mediaPlayer.Size(0, ref streamWidth, ref streamHeight) ||
                streamWidth == 0 || streamHeight == 0)
            {
                AddLog("暂未读取到码流尺寸，保留当前视频区域大小。");
                return;
            }

            double streamRatio = (double)streamWidth / streamHeight;
            int maxWidth = _videoPanelMaximumSize.Width;
            int maxHeight = _videoPanelMaximumSize.Height;

            int targetWidth = maxWidth;
            int targetHeight = (int)Math.Round(targetWidth / streamRatio);

            // 如果按最大宽度计算后高度超出，则改为用最大高度计算宽度。
            if (targetHeight > maxHeight)
            {
                targetHeight = maxHeight;
                targetWidth = (int)Math.Round(targetHeight * streamRatio);
            }

            // BorderStyle.FixedSingle 会占用边框，设置 ClientSize 可保证内部播放区比例正确。
            panel_Video.ClientSize = new Size(targetWidth, targetHeight);
            panel_Video.Location = new Point(
                _videoPanelTopRight.X - panel_Video.Width,
                _videoPanelTopRight.Y);

            _videoView.Dock = DockStyle.Fill;
            _mediaPlayer.AspectRatio = null;

            AddLog($"码流尺寸：{streamWidth}×{streamHeight}，视频区域已调整为：" +
                   $"{panel_Video.ClientSize.Width}×{panel_Video.ClientSize.Height}。");
        }

        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                _isCameraPlaying = false;
                btn_PlayCamera.Enabled = true;
                btn_PlayCamera.Text = "播放相机";
                AddLog("视频流连接失败或已断开！");
                MessageBox.Show("获取 RTSP 视频流失败，请检查网络或相机端口配置！");
            }));
        }

        private async Task SyncCameraTimeAsync(string ipAddress, string user, string pwdMd5)
        {
            AddLog("配置相机 NTP 服务中...");
            try
            {
                string jsonParam = $"{{\"ntpServer\":\"{FIXED_LOCAL_IP}\", \"timeInterval\":30, \"timeZone\":26, \"ntp_enable\":1}}";
                string encodedJson = Uri.EscapeDataString(jsonParam);
                string apiUrl = $"http://{ipAddress}/action/cgi_action?user={user}&pwd={pwdMd5}&action=setTime&json={encodedJson}";

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        if (jsonResponse.Contains("\"code\": 0") || jsonResponse.Contains("\"code\":0"))
                        {
                            AddLog("相机 NTP 配置成功");
                        }
                        else
                        {
                            AddLog($"相机 NTP 配置失败，接口返回: {jsonResponse}");
                        }
                    }
                    else
                    {
                        AddLog($"配置 NTP HTTP 请求失败，状态码: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog($"配置相机 NTP 异常: {ex.Message}");
            }
        }

        private void btn_SelectVideo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "视频文件|*.mp4;*.avi;*.mkv|所有文件|*.*";
                //防止 FFmpeg 由于路径问题找不到文件
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_VideoPath.Text = ofd.FileName;
                }
            }
        }

        private async void btn_ExecuteFusion_Click(object sender, EventArgs e)
        {
            string videoPath = txt_VideoPath.Text.Trim();
            if (!File.Exists(videoPath))
            {
                MessageBox.Show("请先选择有效的视频文件！");
                return;
            }

            DateTime targetTime = dateTimePicker_Fusion.Value;
            btn_ExecuteFusion.Enabled = false;
            AddLog($"[融合] 开始处理目标时间: {targetTime:yyyy-MM-dd HH:mm:ss}");

            try
            {
                // 1. 视频时间轴计算
                DateTime videoStartTime = _videoProcessor.ParseVideoStartTime(System.IO.Path.GetFileName(videoPath));
                TimeSpan offset = targetTime - videoStartTime;
                if (offset.TotalSeconds < 0)
                {
                    MessageBox.Show("所选时间早于视频的开始录制时间，请重新选择！");
                    return;
                }

                // 2. 调用 FFmpeg 抽帧模块
                AddLog($"[融合] 正在抽帧，视频偏移: {offset.TotalSeconds:F3} 秒...");
                string extractedImagePath = await _videoProcessor.ExtractFrameAsync(videoPath, offset);
                if (!File.Exists(extractedImagePath))
                {
                    AddLog("[融合] FFmpeg 抽帧失败。请确认程序目录下存在 ffmpeg.exe。");
                    return;
                }

                // 3. 调用 OpenCV 处理获取内存矩阵 Mat
                AddLog("[融合] 正在使用 OpenCV 进行图像增强与去噪...");
                OpenCvSharp.Mat processedMat = await _imageProcessor.ProcessImageAsync(extractedImagePath, 15, 1.2, true);

                // 4. 调用数据库模块获取 1 秒的点云数据
                AddLog("[融合] 正在提取点云数据...");
                List<PointData> rawPoints = await Task.Run(() =>
                    _dbManager.GetPointsCenteredAt(targetTime, 1.0));

                if (rawPoints == null || rawPoints.Count == 0)
                {
                    AddLog("[融合] 数据库中该时间段无点云数据。");
                    processedMat.Dispose();
                    return;
                }

                // 5.不使用过滤器，直接提取有效的原始点云
                List<PointData> validRawPoints = new List<PointData>(rawPoints.Count);
                foreach (var p in rawPoints)
                {
                    // 剔除 NaN 无效坐标，防止引发 OpenCV 绘图崩溃
                    if (!float.IsNaN(p.X) && !float.IsNaN(p.Y) && !float.IsNaN(p.Z))
                    {
                        validRawPoints.Add(p);
                    }
                }

                // 6. 调用传感器融合模块进行 3D->2D 投影映射 (不再需要硬编码 30.0f)
                AddLog($"[融合] 正在将 {validRawPoints.Count} 个原始点投影至图像...");
                OpenCvSharp.Mat fusedMat = await Task.Run(() => _sensorFusion.ProjectPointCloudToImage(processedMat, validRawPoints));

                OpenCvSharp.Cv2.ImEncode(".bmp", fusedMat, out byte[] imgBuf);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(imgBuf))
                {
                    System.Drawing.Image fusedImage = System.Drawing.Image.FromStream(ms);
                    if (pictureBox_FusionResult.Image != null) pictureBox_FusionResult.Image.Dispose();
                    pictureBox_FusionResult.Image = fusedImage;
                }

                // 8. 释放非托管内存，防止内存泄漏
                fusedMat.Dispose();
                System.IO.File.Delete(extractedImagePath); // 清理抽帧的临时图片

                AddLog("[融合] 融合完成！");
            }
            catch (Exception ex)
            {
                AddLog($"[融合] 发生异常: {ex.Message}");
            }
            finally
            {
                btn_ExecuteFusion.Enabled = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 停止所有的后台工作
            StopAllWork();
            _ptzController?.Dispose();

            // 释放 VLC 资源，防止内存泄漏或进程卡死
            if (_mediaPlayer != null)
            {
                if (_mediaPlayer.IsPlaying)
                {
                    _mediaPlayer.Stop();
                }
                _mediaPlayer.Dispose();
            }
            _libVLC?.Dispose();
        }

        #endregion

        #region 数据库查询与还原

        private async void btn_Reconstruct_Click(object sender, EventArgs e)
        {
            if (_vtkVisualizer == null)
            {
                try
                {
                    _vtkVisualizer = new VtkVisualizer(renderWindowControl1);
                    AddLog("VTK 可视化组件初始化成功。");
                }
                catch (Exception ex)
                {
                    AddLog($"VTK 初始化失败: {ex.Message}");
                    MessageBox.Show("VTK 初始化失败，请确保已正确安装 Activiz.NET 库。\n" + ex.Message);
                    return;
                }
            }
            DateTime selectedStartTime = dateTimePicker_Query.Value;
            double durationSeconds = 1.0;

            AddLog($"[查询] 时间窗口: {selectedStartTime.ToString("HH:mm:ss")} -> +1s");
            btn_Reconstruct.Enabled = false;

            try
            {
                List<PointData> pointsToRender = await Task.Run(() =>
                {
                    List<PointData> rawPoints = _dbManager.GetPointsInRange(selectedStartTime, durationSeconds);
                    if (rawPoints == null || rawPoints.Count == 0) return null;

                    lock (_displayBuffer)
                    {
                        _processor.ApplyFilters(rawPoints, _displayBuffer);
                        return new List<PointData>(_displayBuffer);
                    }
                });

                if (pointsToRender != null && pointsToRender.Count > 0)
                {
                    AddLog($"[还原] 成功加载点数: {pointsToRender.Count}");
                    _vtkVisualizer.Render(pointsToRender);
                }
                else
                {
                    AddLog("[警告] 该时间段内无数据！");
                    MessageBox.Show($"未找到 {selectedStartTime.ToString("HH:mm:ss")} 这一秒内的数据。\n请确认：\n1. 数据库中是否有该时段记录？\n2. 是否存在 8小时 时差问题？");
                }
            }
            catch (Exception ex)
            {
                AddLog($"[错误] {ex.Message}");
            }
            finally
            {
                btn_Reconstruct.Enabled = true;
            }
        }

        private void btn_SaveImage_Click(object sender, EventArgs e)
        {
            if (_vtkVisualizer == null)
            {
                MessageBox.Show("请先执行【查询并还原点云】后再保存图片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "保存点云截图";
                sfd.Filter = "PNG 图片|*.png|所有文件|*.*";
                sfd.FileName = $"PointCloud_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _vtkVisualizer.SaveScreenshot(sfd.FileName);
                        AddLog($"[截图] 点云图片已成功保存至: {sfd.FileName}");
                        MessageBox.Show("图片保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        AddLog($"[截图] 保存失败: {ex.Message}");
                        MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #endregion

        #region 控制与网络

        private void btn_StartListen_Click(object sender, EventArgs e)
        {
            _udpClient.Start(FIXED_LOCAL_IP, LOCAL_CMD_PORT, LOCAL_DATA_PORT);
            btn_StartListen.Enabled = false;
            btn_StopListen.Enabled = true;
            AddLog($"服务启动 | 命令:{LOCAL_CMD_PORT} 数据:{LOCAL_DATA_PORT}");
        }

        private void btn_StopListen_Click(object sender, EventArgs e)
        {
            StopAllWork();
            _udpClient.Stop();
            btn_StartListen.Enabled = true;
            btn_StopListen.Enabled = false;
            AddLog("服务停止");
        }

        private void btn_HandShake_Click(object sender, EventArgs e)
        {
            if (listView_Devices.SelectedItems.Count == 0) return;
            _currentDeviceIp = listView_Devices.SelectedItems[0].SubItems[1].Text;

            HandshakeRequest req = new HandshakeRequest();
            string localIpStr = FIXED_LOCAL_IP;

            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString().StartsWith("192.168"))
                    {
                        localIpStr = ip.ToString();
                        break;
                    }
                }
            }
            catch
            {
            }
            req.user_ip = System.Net.IPAddress.Parse(localIpStr).GetAddressBytes();
            req.data_port = LOCAL_DATA_PORT;
            req.cmd_port = LOCAL_CMD_PORT;
            req.imu_port = LOCAL_DATA_PORT;

            SendControlCommand(CmdSet.General, (byte)GeneralCmdId.Handshake, ProtocolUtils.StructToBytes(req), 0x01);
            AddLog($"发送握手 -> {_currentDeviceIp}");
        }

        private void btn_StartSample_Click_1(object sender, EventArgs e)
        {
            _isSaving = true;
            _dbManager.StartSaving();
            SendControlCommand(CmdSet.General, 0x04, new byte[] { 0x01 });
            AddLog("开始采样并入库...");
            btn_StartSample.Enabled = false;
            btn_StopSample.Enabled = true;
        }

        private void btn_StopSample_Click_1(object sender, EventArgs e)
        {
            SendControlCommand(CmdSet.General, 0x04, new byte[] { 0x00 });
            _isSaving = false;
            _dbManager.StopSaving();
            _isTimeSynced = false;
            _lastRadarTimestampType = byte.MaxValue;
            _lastTimeSyncStatus = byte.MaxValue;
            _lastRadarTimestampType = byte.MaxValue;
            _lastTimeSyncStatus = byte.MaxValue;
            AddLog("停止采样");
            btn_StartSample.Enabled = true;
            btn_StopSample.Enabled = false;
        }

        private void btn_Disconnect_Click_1(object sender, EventArgs e)
        {
            if (listView_Devices.SelectedItems.Count == 0)
            {
                MessageBox.Show("未选中设备！", "提示");
                AddLog(">>> 操作失败：未选中设备");
                return;
            }
            SendControlCommand(CmdSet.General, 0x06, null);
            StopAllWork();
            AddLog(">>> 连接断开");
            listView_Devices.SelectedItems[0].SubItems[3].Text = "未连接";
        }

        private void btn_SetMode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentDeviceIp)) return;
            byte modeVal = (byte)(cbx_WorkMode.SelectedIndex + 1);
            SendControlCommand(CmdSet.Lidar, (byte)LidarCmdId.SetMode, new byte[] { modeVal });
        }

        private void btn_SetCoordinate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentDeviceIp)) return;
            if (btn_StartSample.Enabled == false)
            {
                MessageBox.Show("请先停止采样！");
                return;
            }
            byte coordVal = (byte)cbx_Coordinate.SelectedIndex;
            SendControlCommand(CmdSet.General, (byte)GeneralCmdId.CoordinateSystem, new byte[] { coordVal });
        }

        // ==========================================
        // 扫描模式指令
        // ==========================================
        private void btn_SetScanPattern_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentDeviceIp))
            {
                MessageBox.Show("请先握手连接并选中雷达设备！");
                return;
            }

            // 获取下拉框选择的模式 (0 = NoneRepetitive, 1 = Repetitive)
            byte patternVal = (byte)cbx_ScanPattern.SelectedIndex;

            // 构造 KeyValueParam 载荷 (总长度 5 字节)
            byte[] payload = new byte[5];

            // Key = 2 (kKeyScanPattern)，占 2 字节 (小端序)
            payload[0] = 0x02;
            payload[1] = 0x00;

            // Length = 1 (Value的长度)，占 2 字节 (小端序)
            payload[2] = 0x01;
            payload[3] = 0x00;

            // Value = 模式值
            payload[4] = patternVal;

            // 下发通用指令集(0x00)，命令ID为 0x0B (设置设备参数)
            SendControlCommand(CmdSet.General, 0x0B, payload);

            AddLog($"已发送切换扫描模式指令 -> {(patternVal == 0 ? "非重复扫描" : "重复扫描")},机器重启中，请稍后进行采样");
        }

        #endregion

        #region 数据处理与回环

        private void ProcessPointCloud(byte[] data, DateTime receivedUtc)
        {
            const int livoxHeaderSize = 18;
            if (data == null || data.Length < livoxHeaderSize) return;

            byte timestampType = data[8];
            byte dataType = data[9];
            uint statusCode = BitConverter.ToUInt32(data, 4);
            byte timeSyncStatus = (byte)((statusCode >> 14) & 0x07);
            DateTime frameTimeUtc = ResolvePacketTimeUtc(
                data,
                timestampType,
                timeSyncStatus,
                receivedUtc);

            int pointSize = 0;
            int returnCount = 1;

            const double AVIA_FIRING_FREQ = 240000.0;
            const double TICKS_PER_SECOND = TimeSpan.TicksPerSecond;
            double pointIntervalTicks = TICKS_PER_SECOND / AVIA_FIRING_FREQ;

            switch (dataType)
            {
                case 2: pointSize = 14; returnCount = 1; break;
                case 3: pointSize = 10; returnCount = 1; break;
                case 4: pointSize = 28; returnCount = 2; break;
                case 5: pointSize = 16; returnCount = 2; break;
                case 7: pointSize = 42; returnCount = 3; break;
                case 8: pointSize = 22; returnCount = 3; break;
                case 0: pointSize = 13; pointIntervalTicks = 100.0; break;
                case 1: pointSize = 9; pointIntervalTicks = 100.0; break;
                default: return; // data type 6 is IMU, not point cloud
            }

            int pointsDataLength = data.Length - livoxHeaderSize;
            int sampleCount = pointsDataLength / pointSize;
            var packetPoints = new List<PointData>(sampleCount * returnCount);

            for (int i = 0; i < sampleCount; i++)
            {
                int baseOffset = livoxHeaderSize + (i * pointSize);
                if (baseOffset + pointSize > data.Length) break;

                DateTime firingTimeUtc = frameTimeUtc.AddTicks((long)(i * pointIntervalTicks));

                if (dataType == 2 || dataType == 4 || dataType == 7)
                {
                    for (int j = 0; j < returnCount; j++)
                    {
                        int pointOffset = baseOffset + (j * 14);
                        PointData point;
                        if (TryCreateCartesianPoint(
                            firingTimeUtc,
                            BitConverter.ToInt32(data, pointOffset),
                            BitConverter.ToInt32(data, pointOffset + 4),
                            BitConverter.ToInt32(data, pointOffset + 8),
                            data[pointOffset + 12],
                            data[pointOffset + 13],
                            out point))
                        {
                            packetPoints.Add(point);
                        }
                    }
                }
                else if (dataType == 3)
                {
                    PointData point;
                    if (TryCreateSphericalPoint(
                        firingTimeUtc,
                        BitConverter.ToUInt32(data, baseOffset),
                        BitConverter.ToUInt16(data, baseOffset + 4),
                        BitConverter.ToUInt16(data, baseOffset + 6),
                        data[baseOffset + 8],
                        data[baseOffset + 9],
                        out point))
                    {
                        packetPoints.Add(point);
                    }
                }
                else if (dataType == 5 || dataType == 8)
                {
                    ushort theta = BitConverter.ToUInt16(data, baseOffset);
                    ushort phi = BitConverter.ToUInt16(data, baseOffset + 2);
                    for (int j = 0; j < returnCount; j++)
                    {
                        int pointOffset = baseOffset + 4 + (j * 6);
                        PointData point;
                        if (TryCreateSphericalPoint(
                            firingTimeUtc,
                            BitConverter.ToUInt32(data, pointOffset),
                            theta,
                            phi,
                            data[pointOffset + 4],
                            data[pointOffset + 5],
                            out point))
                        {
                            packetPoints.Add(point);
                        }
                    }
                }
                else if (dataType == 0)
                {
                    PointData point;
                    if (TryCreateCartesianPoint(
                        firingTimeUtc,
                        BitConverter.ToInt32(data, baseOffset),
                        BitConverter.ToInt32(data, baseOffset + 4),
                        BitConverter.ToInt32(data, baseOffset + 8),
                        data[baseOffset + 12],
                        0,
                        out point))
                    {
                        packetPoints.Add(point);
                    }
                }
                else if (dataType == 1)
                {
                    PointData point;
                    if (TryCreateSphericalPoint(
                        firingTimeUtc,
                        BitConverter.ToUInt32(data, baseOffset),
                        BitConverter.ToUInt16(data, baseOffset + 4),
                        BitConverter.ToUInt16(data, baseOffset + 6),
                        data[baseOffset + 8],
                        0,
                        out point))
                    {
                        packetPoints.Add(point);
                    }
                }
            }

            _dbManager.EnqueuePoints(packetPoints);
            Interlocked.Increment(ref _packetsSinceLastStats);
            Interlocked.Add(ref _pointsSinceLastStats, packetPoints.Count);
        }

        private DateTime ResolvePacketTimeUtc(
            byte[] data,
            byte timestampType,
            byte timeSyncStatus,
            DateTime receivedUtc)
        {
            DateTime absoluteUtc;
            if (timestampType == 3 && TryDecodeGpsUtc(data, out absoluteUtc))
            {
                UpdateTimestampStatus(timestampType, timeSyncStatus);
                return absoluteUtc;
            }

            ulong rawTimestamp = BitConverter.ToUInt64(data, 10);
            if (timestampType == 1 && TryDecodePtpUtc(rawTimestamp, out absoluteUtc))
            {
                _lastRadarTimestamp = rawTimestamp;
                UpdateTimestampStatus(timestampType, timeSyncStatus);
                return absoluteUtc;
            }

            bool timestampWentBackwards =
                _lastRadarTimestampType == timestampType && rawTimestamp < _lastRadarTimestamp;
            bool timestampJumped =
                _lastRadarTimestampType == timestampType &&
                rawTimestamp >= _lastRadarTimestamp &&
                rawTimestamp - _lastRadarTimestamp > 1_000_000_000UL;

            if (!_isTimeSynced ||
                timestampType != _lastRadarTimestampType ||
                timestampWentBackwards ||
                timestampJumped)
            {
                _basePcTicks = receivedUtc.Ticks;
                _baseRadarTime = rawTimestamp;
                _isTimeSynced = true;
            }

            _lastRadarTimestamp = rawTimestamp;
            UpdateTimestampStatus(timestampType, timeSyncStatus);

            ulong diffNs = rawTimestamp >= _baseRadarTime
                ? rawTimestamp - _baseRadarTime
                : 0UL;
            long diffTicks = (long)(diffNs / 100UL);
            return new DateTime(_basePcTicks + diffTicks, DateTimeKind.Utc);
        }

        private static bool TryDecodePtpUtc(ulong nanoseconds, out DateTime utc)
        {
            utc = default(DateTime);
            try
            {
                ulong ticksFromEpoch = nanoseconds / 100UL;
                if (ticksFromEpoch > (ulong)(DateTime.MaxValue.Ticks - UnixEpochUtc.Ticks))
                    return false;

                DateTime candidate = new DateTime(
                    UnixEpochUtc.Ticks + (long)ticksFromEpoch,
                    DateTimeKind.Utc);

                // 防止非 Unix 纪元的主时钟数据被误当成绝对时间。
                if (candidate.Year < 2000 || candidate.Year > 2100)
                    return false;

                utc = candidate;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryDecodeGpsUtc(byte[] data, out DateTime utc)
        {
            utc = default(DateTime);
            try
            {
                int year = 2000 + data[10];
                int month = data[11];
                int day = data[12];
                int hour = data[13];
                uint microsecondsWithinHour = BitConverter.ToUInt32(data, 14);
                if (microsecondsWithinHour >= 3_600_000_000U) return false;

                utc = new DateTime(year, month, day, hour, 0, 0, DateTimeKind.Utc)
                    .AddTicks((long)microsecondsWithinHour * 10L);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void UpdateTimestampStatus(byte timestampType, byte timeSyncStatus)
        {
            if (timestampType != _lastRadarTimestampType ||
                timeSyncStatus != _lastTimeSyncStatus)
            {
                _lastRadarTimestampType = timestampType;
                _lastTimeSyncStatus = timeSyncStatus;
                AddLog($"[时间] 雷达 timestamp_type={timestampType}, sync_status={timeSyncStatus}");
            }
        }

        private static bool TryCreateCartesianPoint(
            DateTime timeUtc,
            int xMm,
            int yMm,
            int zMm,
            byte reflectivity,
            byte tag,
            out PointData point)
        {
            point = default(PointData);
            if (xMm == 0 && yMm == 0 && zMm == 0) return false;

            double distanceSquared =
                (double)xMm * xMm + (double)yMm * yMm + (double)zMm * zMm;
            point = new PointData
            {
                ExactTime = timeUtc,
                X = xMm / 1000.0f,
                Y = yMm / 1000.0f,
                Z = zMm / 1000.0f,
                Depth = (float)(Math.Sqrt(distanceSquared) / 1000.0),
                Reflectivity = reflectivity,
                Tag = tag
            };
            return true;
        }

        private static bool TryCreateSphericalPoint(
            DateTime timeUtc,
            uint depthMm,
            ushort thetaRaw,
            ushort phiRaw,
            byte reflectivity,
            byte tag,
            out PointData point)
        {
            point = default(PointData);
            if (depthMm == 0) return false;

            double thetaRad = thetaRaw * 0.01 * (Math.PI / 180.0);
            double phiRad = phiRaw * 0.01 * (Math.PI / 180.0);
            double radiusM = depthMm / 1000.0;
            point = new PointData
            {
                ExactTime = timeUtc,
                X = (float)(radiusM * Math.Sin(thetaRad) * Math.Cos(phiRad)),
                Y = (float)(radiusM * Math.Sin(thetaRad) * Math.Sin(phiRad)),
                Z = (float)(radiusM * Math.Cos(thetaRad)),
                Depth = (float)radiusM,
                Reflectivity = reflectivity,
                Tag = tag
            };
            return true;
        }

        private void StopAllWork()
        {
            _heartbeatTimer.Stop();
            _isSaving = false;
            _dbManager.StopSaving();
            _isTimeSynced = false;
        }

        private void OnHeartbeatTimerTick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentDeviceIp) && _udpClient.IsConnected)
                SendControlCommand(CmdSet.General, (byte)GeneralCmdId.Heartbeat, null);

            _statsElapsedSeconds++;
            if (_statsElapsedSeconds >= 5)
            {
                long packetCount = Interlocked.Exchange(ref _packetsSinceLastStats, 0);
                long pointCount = Interlocked.Exchange(ref _pointsSinceLastStats, 0);
                long droppedTotal = _udpClient.DroppedDataPackets;
                long droppedInInterval = droppedTotal - _lastDroppedDataPackets;
                _lastDroppedDataPackets = droppedTotal;

                AddLog(
                    $"[采集统计] {pointCount / (double)_statsElapsedSeconds:F0} 点/秒, " +
                    $"{packetCount / (double)_statsElapsedSeconds:F0} 包/秒, " +
                    $"处理队列丢包={droppedInInterval}");
                _statsElapsedSeconds = 0;
            }
        }

        private void SendControlCommand(CmdSet set, byte cmdId, byte[] payload, byte packetType = 0)
        {
            if (string.IsNullOrEmpty(_currentDeviceIp)) return;

            List<byte> pkt = new List<byte> { 0xAA, 0x01, 0, 0, packetType };
            pkt.AddRange(BitConverter.GetBytes(ProtocolUtils.GetSeqNum()));
            pkt.Add(0); pkt.Add(0);
            pkt.Add((byte)set);
            pkt.Add(cmdId);
            if (payload != null)
                pkt.AddRange(payload);

            ushort len = (ushort)(pkt.Count + 4);
            byte[] lenB = BitConverter.GetBytes(len);
            pkt[2] = lenB[0]; pkt[3] = lenB[1];

            byte[] header = pkt.GetRange(0, 7).ToArray();
            ushort c16 = ProtocolUtils.Crc16(header, 7);
            byte[] c16B = BitConverter.GetBytes(c16);
            pkt[7] = c16B[0]; pkt[8] = c16B[1];

            byte[] finalWithoutCrc32 = pkt.ToArray();
            uint c32 = ProtocolUtils.Crc32(finalWithoutCrc32, finalWithoutCrc32.Length);
            pkt.AddRange(BitConverter.GetBytes(c32));

            _udpClient.SendCommand(pkt.ToArray(), _currentDeviceIp, TARGET_LIDAR_PORT);
        }

        private void HandleBroadcast(byte[] data, IPEndPoint remote)
        {
            if (data.Length < 10) return;
            if (data.Length > 10)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    bool exists = false;
                    foreach (ListViewItem item in listView_Devices.Items)
                    {
                        if (item.SubItems[1].Text == remote.Address.ToString())
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        var item = new ListViewItem(DateTime.Now.ToLongTimeString());
                        item.SubItems.Add(remote.Address.ToString());
                        item.SubItems.Add("Livox Device");
                        item.SubItems.Add("未连接");
                        listView_Devices.Items.Add(item);
                    }
                });
            }
        }

        private void HandleAck(byte[] data, IPEndPoint remote)
        {
            if (data.Length < 11) return;
            byte cmdType = data[4];
            byte cmdSet = data[9];
            byte cmdId = data[10];

            this.BeginInvoke((MethodInvoker)delegate
            {
                if (cmdType == 1 && cmdSet == 0 && cmdId == 1)
                {
                    AddLog($"握手成功! 雷达已连接 ({remote.Address})");
                    if (listView_Devices.SelectedItems.Count > 0)
                        listView_Devices.SelectedItems[0].SubItems[3].Text = "已连接";
                    SyncRadarTime();
                    if (!_heartbeatTimer.Enabled) _heartbeatTimer.Start();
                }
                else if (cmdSet == 0 && cmdId == 4)
                {
                    byte retCode = (data.Length > 11) ? data[11] : (byte)255;
                    AddLog(retCode == 0 ? ">>> 采样状态变更成功" : $">>> 采样状态变更失败: {retCode}");
                }
                else if (cmdSet == (byte)CmdSet.General && cmdId == (byte)GeneralCmdId.CoordinateSystem)
                {
                    byte retCode = (data.Length > 11) ? data[11] : (byte)255;
                    if (retCode == 0) AddLog(">>> 坐标系切换成功！请重新开始采样。");
                    else AddLog($">>> 坐标系切换失败，错误码: {retCode}");
                }
                else if (cmdSet == (byte)CmdSet.Lidar && cmdId == (byte)LidarCmdId.SetMode)
                {
                    AddLog(">>> 工作模式设置指令已返回");
                }
                else if (cmdSet == (byte)CmdSet.General && cmdId == 0x0B)
                {
                    byte retCode = (data.Length > 11) ? data[11] : (byte)255;
                    if (retCode == 0)
                        AddLog(">>> 扫描模式切换成功");
                    else
                        AddLog($">>> 扫描模式切换失败，硬件返回错误码: {retCode}");
                }
            });
        }

        private void SyncRadarTime()
        {
            if (string.IsNullOrEmpty(_currentDeviceIp))
                return;
            DateTime nowUtc = DateTime.UtcNow;
            LidarSetUtcSyncTimeRequest req = new LidarSetUtcSyncTimeRequest();
            req.year = (byte)(nowUtc.Year - 2000);
            req.month = (byte)nowUtc.Month;
            req.day = (byte)nowUtc.Day;
            req.hour = (byte)nowUtc.Hour;
            long ticksInHour = nowUtc.Ticks % 36000000000;
            req.microsecond = (uint)(ticksInHour / 10);
            byte[] payload = ProtocolUtils.StructToBytes(req);
            SendControlCommand(CmdSet.Lidar, (byte)LidarCmdId.UpdateUtcTime, payload);
        }

        // ==========================================
        // 日志系统
        // ==========================================
        private static readonly object _logLock = new object();

        private void AddLog(string msg)
        {
            if (listBox_Log.InvokeRequired)
                listBox_Log.BeginInvoke((MethodInvoker)delegate { AddLog(msg); });
            else
            {
                // 1. 格式化日志内容
                string logLine = $"[{DateTime.Now.ToString("HH:mm:ss")}] {msg}";

                // 2. 更新 UI 界面
                listBox_Log.Items.Add(logLine);
                listBox_Log.TopIndex = listBox_Log.Items.Count - 1;

                // 3. 异步写入到文件，不阻塞界面
                Task.Run(() => WriteLogToFile(logLine));
            }
        }

        private void WriteLogToFile(string logLine)
        {
            try
            {
                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                string logFile = Path.Combine(logDir, $"Log_{DateTime.Now:yyyyMMdd}.txt");

                lock (_logLock)
                {
                    File.AppendAllText(logFile, logLine + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"写日志文件失败: {ex.Message}");
            }
        }
        #endregion

        #region 焦距增加或减小


        private async Task SendPtzCommandAsync(int ptzCmd, int speed = 50)
        {
            string inputIp = txt_CameraIp.Text.Trim();
            if (string.IsNullOrEmpty(inputIp))
            {
                MessageBox.Show("请输入相机IP地址！");
                return;
            }

            // 处理带端口的 IP
            string ipAddress = inputIp.Contains(":") ? inputIp.Split(':')[0] : inputIp;

            // 默认账户及密码，与 InitCameraPlayer 保持一致
            string user = "admin";
            string pwdMd5 = "e10adc3949ba59abbe56e057f20f883e";

            // 构造 JSON 参数，文档规定 channel 固定为 0
            string jsonParam = $"{{\"speed_h\": {speed}, \"speed_v\": {speed}, \"channel\": 0, \"ptz_cmd\": {ptzCmd}}}";
            string encodedJson = Uri.EscapeDataString(jsonParam);
            string apiUrl = $"http://{ipAddress}/action/cgi_action?user={user}&pwd={pwdMd5}&action=setPtzControl&json={encodedJson}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        if (jsonResponse.Contains("\"code\": 0") || jsonResponse.Contains("\"code\":0"))
                        {
                            AddLog($"[PTZ] 指令发送成功: Cmd={ptzCmd}");
                        }
                        else
                        {
                            AddLog($"[PTZ] 指令失败，返回: {jsonResponse}");
                        }
                    }
                    else
                    {
                        AddLog($"[PTZ] HTTP 请求失败，状态码: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog($"[PTZ] 控制异常: {ex.Message}");
            }
        }
        #endregion

        #region OSD 水印恢复

        /// <summary>
        /// 恢复摄像头画面上的时间和名称水印
        /// </summary>
        private async Task EnableCameraOsdAsync()
        {
            string inputIp = txt_CameraIp.Text.Trim();
            if (string.IsNullOrEmpty(inputIp))
            {
                MessageBox.Show("请输入相机IP地址！");
                return;
            }

            string ipAddress = inputIp.Contains(":") ? inputIp.Split(':')[0] : inputIp;
            string user = "admin";
            string pwdMd5 = "e10adc3949ba59abbe56e057f20f883e";

            // 构造 JSON 参数：将所有开关设为 1
            // show_date: 1 开启日期, show_time: 1 开启时间
            string jsonParam = "{" +
                "\"show_date\": 1, " +
                "\"show_time\": 1, " +
                "\"show_week\": 1, " +
                "\"font_size\": 1, " +
                "\"title_list\": [" +
                    "{\"show_title\": 1, \"title\": \"IPCamera\", \"title_pos_x\": 556, \"title_pos_y\": 546}, " +
                    "{\"show_title\": 0, \"title\": \" \", \"title_pos_x\": 556, \"title_pos_y\": 506}, " +
                    "{\"show_title\": 0, \"title\": \" \", \"title_pos_x\": 556, \"title_pos_y\": 466}, " +
                    "{\"show_title\": 0, \"title\": \" \", \"title_pos_x\": 556, \"title_pos_y\": 426}" +
                "]" +
            "}";

            string encodedJson = Uri.EscapeDataString(jsonParam);
            string apiUrl = $"http://{ipAddress}/action/cgi_action?user={user}&pwd={pwdMd5}&action=setOsdConf&json={encodedJson}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);
                    AddLog("[OSD] 正在恢复水印显示...");

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        if (jsonResponse.Contains("\"code\": 0") || jsonResponse.Contains("\"code\":0"))
                        {
                            AddLog("[OSD] 水印已恢复！");
                        }
                        else
                        {
                            AddLog($"[OSD] 恢复失败: {jsonResponse}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog($"[OSD] 异常: {ex.Message}");
            }
        }
        #endregion

        #region OSD 水印去除

        /// <summary>
        /// 关闭摄像头画面上的时间和名称水印
        /// </summary>
        private async Task DisableCameraOsdAsync()
        {
            string inputIp = txt_CameraIp.Text.Trim();
            if (string.IsNullOrEmpty(inputIp))
            {
                MessageBox.Show("请输入相机IP地址！", "提示");
                return;
            }

            string ipAddress = inputIp.Contains(":") ? inputIp.Split(':')[0] : inputIp;
            string user = "admin";
            string pwdMd5 = "e10adc3949ba59abbe56e057f20f883e";

            // 构造 JSON 参数，根据文档：
            // show_date: 0 关闭日期, show_time: 0 关闭时间, show_week: 0 关闭星期
            // title_list: 包含4个标题配置，将它们的 show_title 全设为 0 (关闭显示)
            string jsonParam = "{" +
                "\"show_date\": 0, " +
                "\"show_time\": 0, " +
                "\"show_week\": 0, " +
                "\"title_list\": [" +
                    "{\"show_title\": 0}, " +
                    "{\"show_title\": 0}, " +
                    "{\"show_title\": 0}, " +
                    "{\"show_title\": 0}" +
                "]" +
            "}";

            string encodedJson = Uri.EscapeDataString(jsonParam);
            string apiUrl = $"http://{ipAddress}/action/cgi_action?user={user}&pwd={pwdMd5}&action=setOsdConf&json={encodedJson}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);
                    AddLog("[OSD] 正在发送关闭水印指令...");

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        if (jsonResponse.Contains("\"code\": 0") || jsonResponse.Contains("\"code\":0"))
                        {
                            AddLog("[OSD] 水印已成功关闭！现在抓拍的图片将是纯净画面。");
                        }
                        else
                        {
                            AddLog($"[OSD] 关闭水印失败，返回: {jsonResponse}");
                        }
                    }
                    else
                    {
                        AddLog($"[OSD] HTTP 请求失败，状态码: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog($"[OSD] 控制异常: {ex.Message}");
            }
        }

        #endregion

        private async void btn_ShowRaw_Click(object sender, EventArgs e)
        {
            // 1. 确保 VTK 已经初始化 
            if (_vtkVisualizer == null)
            {
                try
                {
                    _vtkVisualizer = new VtkVisualizer(renderWindowControl1);
                    AddLog("VTK 可视化组件初始化成功。");
                }
                catch (Exception ex)
                {
                    AddLog($"VTK 初始化失败: {ex.Message}");
                    MessageBox.Show("VTK 初始化失败，请确保已正确安装 Activiz.NET 库。\n" + ex.Message);
                    return;
                }
            }

            DateTime selectedStartTime = dateTimePicker_Query.Value;
            double durationSeconds = 1.0;

            AddLog($"[查询原始数据] 时间窗口: {selectedStartTime.ToString("HH:mm:ss")} -> +1s");

            // 禁用当前触发事件的按钮，防止重复点击
            if (sender is System.Windows.Forms.Button btn)
            {
                btn.Enabled = false;
            }

            try
            {
                // 2. 异步获取数据，跳过 ROI 和降采样，但必须剔除 NaN
                List<PointData> rawPointsToRender = await Task.Run(() =>
                {
                    // 从数据库拉取这一秒内的所有原始点
                    List<PointData> rawPoints = _dbManager.GetPointsInRange(selectedStartTime, durationSeconds);
                    if (rawPoints == null || rawPoints.Count == 0) return new List<PointData>();


                    // 这一步是为了防止 VTK 渲染器在计算深度着色时因为 NaN 而崩溃
                    List<PointData> safeRawPoints = new List<PointData>(rawPoints.Count);
                    for (int i = 0; i < rawPoints.Count; i++)
                    {
                        var p = rawPoints[i];
                        // 剔除无效坐标点
                        if (float.IsNaN(p.X) || float.IsNaN(p.Y) || float.IsNaN(p.Z)) continue;

                        safeRawPoints.Add(p);
                    }

                    return safeRawPoints;
                });

                // 3. 渲染原始数据 (只要传入 Render，就会自动应用深度着色)
                if (rawPointsToRender != null && rawPointsToRender.Count > 0)
                {
                    AddLog($"[原始还原] 成功加载原始点数: {rawPointsToRender.Count}");
                    _vtkVisualizer.Render(rawPointsToRender);
                }
                else
                {
                    AddLog("[警告] 该时间段内无有效的原始数据！");
                    MessageBox.Show($"未找到 {selectedStartTime.ToString("HH:mm:ss")} 这一秒内的有效数据。\n请确认：\n1. 数据库中是否有该时段记录？\n2. 是否存在 8小时 时差问题？");
                }
            }
            catch (Exception ex)
            {
                AddLog($"[错误] {ex.Message}");
            }
            finally
            {
                btn_ShowRaw.Enabled = true;
            }
        }

        private async void btn_ExportPcd_Click(object sender, EventArgs e)
        {
            DateTime selectedCenterTimeLocal = dateTimePicker_Query.Value;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "\u5bfc\u51fa\u539f\u59cb\u70b9\u4e91\u4e3a PCD";
                sfd.Filter = "PCD \u70b9\u4e91\u6587\u4ef6|*.pcd|\u6240\u6709\u6587\u4ef6|*.*";
                sfd.FileName = $"PointCloud_{selectedCenterTimeLocal:yyyyMMdd_HHmmss}.pcd";
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() != DialogResult.OK) return;

                Button exportButton = sender as Button;
                if (exportButton != null) exportButton.Enabled = false;

                try
                {
                    AddLog($"[PCD Export] Loading a centered 1-second window around {selectedCenterTimeLocal:HH:mm:ss}...");

                    List<PointData> rawPoints = await Task.Run(() =>
                    {
                        return _dbManager.GetPointsCenteredAt(selectedCenterTimeLocal, 1.0);
                    });

                    if (rawPoints == null || rawPoints.Count == 0)
                    {
                        MessageBox.Show("No raw point cloud data exists in the selected time range.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int exportedCount = await Task.Run(() =>
                    {
                        return PointCloudProcessor.ExportToPcd(rawPoints, sfd.FileName, true);
                    });

                    AddLog($"[PCD Export] Exported {exportedCount} valid points to: {sfd.FileName}");
                    MessageBox.Show($"PCD export completed!\nValid points: {exportedCount}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    AddLog($"[PCD Export] Failed: {ex.Message}");
                    MessageBox.Show($"Export failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (exportButton != null) exportButton.Enabled = true;
                }
            }
        }

        private async void btn_SaveBEV_Click(object sender, EventArgs e)
        {
            // 获取当前面板上选择的时间
            DateTime selectedStartTime = dateTimePicker_Query.Value;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "保存高保真原始点云正视图";
                sfd.Filter = "PNG 图片|*.png|所有文件|*.*";
                sfd.FileName = $"PointCloud_RawFrontView_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // 临时禁用按钮防止重复点击
                    if (sender is Button btn) btn.Enabled = false;

                    try
                    {
                        AddLog($"[转换] 正在从数据库提取 {selectedStartTime:HH:mm:ss} 的原始点云，请稍候...");

                        // 1. 异步从数据库拉取 1 秒内的绝对原始点云数据
                        List<PointData> rawPoints = await Task.Run(() =>
                        {
                            return _dbManager.GetPointsInRange(selectedStartTime, 1.0);
                        });

                        if (rawPoints == null || rawPoints.Count == 0)
                        {
                            MessageBox.Show("当前时间段没有原始点云数据！请确认时间是否正确。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        AddLog($"[转换] 成功提取 {rawPoints.Count} 个原始点，正在生成 1920x1080 投影图...");

                        // 2. 异步执行投影绘图（传入原始数据 rawPoints，而不是 _displayBuffer）
                        using (Bitmap flattenedBmp = await Task.Run(() => ElevationProjector.CreateFrontViewImage(rawPoints, 1920, 1080)))
                        {
                            if (flattenedBmp != null)
                            {
                                flattenedBmp.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                                AddLog($"[截图] 原始点云投影图已成功保存至: {sfd.FileName}");
                                MessageBox.Show("原始点云投影图保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("生成投影图失败：有效数据不足或存在极端异常坐标。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AddLog($"[截图] 保存失败: {ex.Message}");
                        MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        // 恢复按钮状态
                        if (sender is Button btn_SaveBEVn)
                            btn_SaveBEV.Enabled = true;
                    }
                }
            }
        }
        private async void btn_LoadPcd_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                // 1. 修改 Filter，使其同时支持 .pcd 和 .las 文件
                ofd.Filter = "点云文件 (*.pcd;*.las)|*.pcd;*.las|PCD 文件 (*.pcd)|*.pcd|LAS 文件 (*.las)|*.las|所有文件 (*.*)|*.*";
                ofd.Title = "选择离线点云文件进行投影";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // 禁用按钮防止重复点击
                    btn_LoadPcd.Enabled = false;

                    try
                    {
                        string fileName = System.IO.Path.GetFileName(ofd.FileName);
                        AddLog($"[离线投影] 正在解析点云文件: {fileName}");

                        // 2. 调用修改后的 PointCloudReader，变量名也相应改为 cloudPoints
                        List<PointData> cloudPoints = await Task.Run(() => PointCloudReader.Read(ofd.FileName));

                        if (cloudPoints == null || cloudPoints.Count == 0)
                        {
                            MessageBox.Show("该文件中没有解析到有效的点云数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        AddLog($"[离线投影] 解析成功，共 {cloudPoints.Count} 个点，正在生成正视图...");

                        // 3. 异步投影为图像，传入解析出的点云列表
                        // 默认生成 1920x1080 尺寸的图像，可根据需要调整
                        using (System.Drawing.Bitmap projectedImg = await Task.Run(() => ElevationProjector.CreateFrontViewImage(cloudPoints, 1920, 1080)))
                        {
                            if (projectedImg != null)
                            {
                                // 将结果输出到 tabPage3 的 pictureBox_FusionResult 中展示
                                if (pictureBox_FusionResult.Image != null)
                                {
                                    pictureBox_FusionResult.Image.Dispose();
                                }
                                pictureBox_FusionResult.Image = new System.Drawing.Bitmap(projectedImg);

                                // 自动保存投影结果到与原点云文件相同的目录下
                                string savePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(ofd.FileName), $"Projected_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                                projectedImg.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);

                                AddLog($"[离线投影] 投影完成！图片已自动保存至: {savePath}");

                                // 自动切换到“点云CCD融合”选项卡以展示图片
                                tabControl1.SelectedTab = tabPage3;
                            }
                            else
                            {
                                AddLog("[离线投影] 生成图像失败：有效数据不足或由于阈值过滤导致无坐标点。");
                                MessageBox.Show("生成投影图失败，点云范围异常或超出预设视野。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AddLog($"[错误] 处理点云文件发生异常: {ex.Message}");
                        MessageBox.Show($"解析或投影失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        // 恢复按钮状态
                        btn_LoadPcd.Enabled = true;
                    }
                }
            }
        }

        private async void btn_ZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            await SendPtzCommandAsync(9, 50);
        }

        private async void btn_ZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            await SendPtzCommandAsync(10, 50);
        }

        private async void btn_ZoomIn_MouseUp(object sender, MouseEventArgs e)
        {
            await SendPtzCommandAsync(21, 50);
        }

        private async void btn_ZoomOut_MouseUp(object sender, MouseEventArgs e)
        {
            await SendPtzCommandAsync(21, 50);
        }

        private async void btn_Snapshot_Click(object sender, EventArgs e)
        {
            string inputIp = txt_CameraIp.Text.Trim();
            if (string.IsNullOrEmpty(inputIp))
            {
                MessageBox.Show("请输入相机IP地址！", "提示");
                return;
            }

            // 提取纯IP
            string ipAddress = inputIp.Contains(":") ? inputIp.Split(':')[0] : inputIp;

            // 构造抓拍接口 URL。根据文档，不指定 fmt 参数时默认返回 jpg。
            string apiUrl = $"http://{ipAddress}/action/cgi_images";
            Button snapshotButton = sender as Button;
            if (snapshotButton != null) snapshotButton.Enabled = false;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);

                    AddLog("[抓拍] 正在请求抓拍...");

                    // 相机接口没有返回曝光时间时，以请求往返时间中点作为最佳估计。
                    DateTime requestStartUtc = DateTime.UtcNow;
                    using (HttpResponseMessage response = await client.GetAsync(apiUrl))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                            DateTime requestEndUtc = DateTime.UtcNow;
                            DateTime imageTimeUtc = requestStartUtc.AddTicks(
                                (requestEndUtc - requestStartUtc).Ticks / 2L);

                            if (imageBytes != null && imageBytes.Length > 0)
                            {
                                string saveFolder = Path.Combine(Application.StartupPath, "Snapshots");
                                Directory.CreateDirectory(saveFolder);

                                DateTime imageTimeLocal = imageTimeUtc.ToLocalTime();
                                string sampleId = $"Sample_{imageTimeLocal:yyyyMMdd_HHmmss_fff}";
                                string imagePath = Path.Combine(saveFolder, sampleId + ".jpg");
                                string pcdPath = Path.Combine(saveFolder, sampleId + ".pcd");
                                string metadataPath = Path.Combine(saveFolder, sampleId + ".txt");

                                File.WriteAllBytes(imagePath, imageBytes);
                                AddLog(
                                    $"[抓拍] RGB 已保存，估计曝光时刻: " +
                                    $"{imageTimeLocal:yyyy-MM-dd HH:mm:ss.fff}");

                                const double pointWindowSeconds = 1.0;
                                List<PointData> points = await GetCenteredPointWindowWithRetryAsync(
                                    imageTimeUtc,
                                    pointWindowSeconds);

                                int exportedCount = 0;
                                if (points != null && points.Count > 0)
                                {
                                    exportedCount = await Task.Run(() =>
                                        PointCloudProcessor.ExportToPcd(points, pcdPath, true));
                                    AddLog($"[抓拍] 同步 PCD 已保存，共 {exportedCount} 个点。");
                                }
                                else
                                {
                                    AddLog("[抓拍] 警告：图像对应的一秒窗口内没有查到点云。");
                                }

                                DateTime windowStartUtc = imageTimeUtc.AddSeconds(-0.5);
                                DateTime windowEndUtc = imageTimeUtc.AddSeconds(0.5);
                                double halfRoundTripMs =
                                    (requestEndUtc - requestStartUtc).TotalMilliseconds / 2.0;

                                File.WriteAllLines(metadataPath, new[]
                                {
                                    "sample_id=" + sampleId,
                                    "image_time_source=http_request_midpoint_estimate",
                                    "image_time_utc=" + imageTimeUtc.ToString("O"),
                                    "image_time_local=" + imageTimeLocal.ToString("O"),
                                    "http_half_round_trip_ms=" + halfRoundTripMs.ToString("F3"),
                                    "point_window_start_utc=" + windowStartUtc.ToString("O"),
                                    "point_window_end_utc=" + windowEndUtc.ToString("O"),
                                    "point_count=" + exportedCount,
                                    "radar_timestamp_type=" + _lastRadarTimestampType,
                                    "radar_time_sync_status=" + _lastTimeSyncStatus,
                                    "image_file=" + Path.GetFileName(imagePath),
                                    "point_cloud_file=" + (exportedCount > 0 ? Path.GetFileName(pcdPath) : string.Empty)
                                });

                                MessageBox.Show(
                                    exportedCount > 0
                                        ? $"RGB 与 PCD 配对保存成功！\n样本：{sampleId}\n点数：{exportedCount}"
                                        : $"RGB 已保存，但没有找到配对点云。\n样本：{sampleId}",
                                    "抓拍完成",
                                    MessageBoxButtons.OK,
                                    exportedCount > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                            }
                            else
                            {
                                AddLog("[抓拍] 失败：返回的图片数据为空。");
                            }
                        }
                        else
                        {
                            AddLog($"[抓拍] HTTP 请求失败，状态码: {response.StatusCode}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog($"[抓拍] 发生异常: {ex.Message}");
            }
            finally
            {
                if (snapshotButton != null) snapshotButton.Enabled = true;
            }
        }

        private async Task<List<PointData>> GetCenteredPointWindowWithRetryAsync(
            DateTime centerUtc,
            double durationSeconds)
        {
            DateTime windowEndUtc = centerUtc.AddSeconds(durationSeconds / 2.0);
            TimeSpan waitForWindow = windowEndUtc.AddMilliseconds(300) - DateTime.UtcNow;
            if (waitForWindow > TimeSpan.Zero)
                await Task.Delay(waitForWindow);

            List<PointData> latestResult = new List<PointData>();
            for (int attempt = 0; attempt < 5; attempt++)
            {
                latestResult = await Task.Run(() =>
                    _dbManager.GetPointsCenteredAtUtc(centerUtc, durationSeconds));

                if (latestResult.Count > 0)
                {
                    DateTime latestPointUtc = latestResult[latestResult.Count - 1]
                        .ExactTime.ToUniversalTime();
                    if (latestPointUtc >= windowEndUtc.AddMilliseconds(-20))
                        return latestResult;
                }

                await Task.Delay(250);
            }

            return latestResult;
        }

        private async void btn_RemoveOsd_Click(object sender, EventArgs e)
        {
            await DisableCameraOsdAsync();
        }

        private async void btn_EnableOsd_Click_1(object sender, EventArgs e)
        {
            await EnableCameraOsdAsync();
        }

        private async void btn_CompleteDepth_Click(object sender, EventArgs e)
        {
            // 初始化右侧的独立 VTK 渲染实例
            if (_vtkVisualizer2 == null)
            {
                try
                {
                    _vtkVisualizer2 = new VtkVisualizer(renderWindowControl2);
                    AddLog("右侧 VTK 可视化组件初始化成功。");
                }
                catch (Exception ex)
                {
                    AddLog($"右侧 VTK 初始化失败: {ex.Message}");
                    MessageBox.Show("VTK 初始化失败，请确保已正确安装 Activiz.NET 库。\n" + ex.Message);
                    return;
                }
            }

            DateTime selectedStartTime = dateTimePicker_Query.Value;

            Button currentBtn = sender as Button;
            if (currentBtn != null) currentBtn.Enabled = false;

            AddLog($"[深度补全] 正在提取 {selectedStartTime:HH:mm:ss} 的点云并进行深度补全计算...");

            try
            {
                List<PointData> completedPointsToRender = await Task.Run(() =>
                {
                    List<PointData> rawPoints = _dbManager.GetPointsInRange(selectedStartTime, 1.0);
                    if (rawPoints == null || rawPoints.Count == 0) return new List<PointData>();

                    List<PointData> safePoints = new List<PointData>(rawPoints.Count);
                    foreach (var p in rawPoints)
                    {
                        if (!float.IsNaN(p.X) && !float.IsNaN(p.Y) && !float.IsNaN(p.Z) && p.X > 0.1f)
                        {
                            safePoints.Add(p);
                        }
                    }
                    return DepthCompleter.CompleteDepth(safePoints);
                });

                if (completedPointsToRender != null && completedPointsToRender.Count > 0)
                {
                    AddLog($"[深度补全] 重建出 {completedPointsToRender.Count} 个致密点，正在交由右侧 VTK 屏幕渲染...");
                    // 调用右侧的渲染器进行显示，保留左侧的原始画面
                    _vtkVisualizer2.Render(completedPointsToRender);
                }
                else
                {
                    AddLog("[警告] 该时间段内无有效的点云数据，补全失败！");
                    MessageBox.Show($"未找到 {selectedStartTime:HH:mm:ss} 的点云数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                AddLog($"[错误] 深度补全发生异常: {ex.Message}");
            }
            finally
            {
                // 恢复按钮状态
                if (currentBtn != null) currentBtn.Enabled = true;
            }
        }

        #region UDP 云台控制

        private void InitPtzPage()
        {
            if (_ptzController == null)
            {
                _ptzController = new PtzUdpController();
                _ptzController.PacketReceived += HandlePtzPacketReceived;
                _ptzController.ErrorReceived += (msg) => PtzLog("[错误] " + msg);
            }

            BuildSinglePagePtzLayout();
            PtzLog("云台 UDP 页面已初始化");
        }

        /// <summary>
        /// 单页紧凑布局：全部云台功能一次显示，不使用二级导航或滚动条。
        /// </summary>
        private void BuildSinglePagePtzLayout()
        {
            if (tabPage4 == null || tabPage4.Tag as string == "single-page-layout")
                return;

            tabPage4.Tag = "single-page-layout";
            tabPage4.SuspendLayout();
            tabPage4.Controls.Clear();
            tabPage4.AutoScroll = false;

            // 左上：通信与状态。
            SetPtzGroupBounds(gbxPtzNet, 10, 10, 430, 120);
            SetPtzGroupBounds(gbxPtzQuery, 10, 140, 430, 200);

            // 上层中部：方向控制和角度定位。
            SetPtzGroupBounds(gbxPtzDirect, 450, 10, 430, 330);
            ArrangePtzDirectionPad();
            SetPtzGroupBounds(gbxPtzLocate, 890, 10, 500, 330);
            ArrangePtzLocatePanel();

            // 右上：设备配置与查询回传并排显示。
            SetPtzGroupBounds(gbxPtzExtBasic, 1400, 10, 300, 330);
            gbxPtzExtBasic.Text = "设备基础配置";
            SetPtzGroupBounds(gbxPtzExtQuery, 1710, 10, 325, 330);
            gbxPtzExtQuery.Text = "查询与回传";

            // 下层：充分使用页面下方空间。
            SetPtzGroupBounds(gbxPtzPreset, 10, 350, 430, 170);
            SetPtzGroupBounds(gbxPtzRaw, 10, 530, 430, 70);
            SetPtzGroupBounds(gbxPtzExtMaintenance, 10, 610, 430, 170);
            gbxPtzExtMaintenance.Text = "零位、校准与维护";

            SetPtzGroupBounds(gbxPtzArea, 450, 350, 430, 400);
            gbxPtzArea.Text = "区域扫描参数";

            SetPtzGroupBounds(gbxPtzExtPreset, 890, 350, 500, 350);
            gbxPtzExtPreset.Text = "预置位高级操作";

            SetPtzGroupBounds(gbxPtzExtArea, 1400, 350, 635, 400);
            gbxPtzExtArea.Text = "区域扫描操作";

            Control[] groups =
            {
                gbxPtzNet, gbxPtzQuery, gbxPtzPreset, gbxPtzRaw,
                gbxPtzDirect, gbxPtzArea, gbxPtzLocate,
                gbxPtzExtPreset, gbxPtzExtArea, gbxPtzExtMaintenance,
                gbxPtzExtBasic, gbxPtzExtQuery
            };

            foreach (Control group in groups)
            {
                tabPage4.Controls.Add(group);
                ConfigurePtzTextLayout(group);
            }

            CompactPtzCommandButtons(gbxPtzExtPreset, 160);
            CompactPtzCommandButtons(gbxPtzExtArea, 145);
            CompactPtzCommandButtons(gbxPtzExtMaintenance, 125);
            CompactPtzCommandButtons(gbxPtzExtBasic, 130);
            CompactPtzCommandButtons(gbxPtzExtQuery, 140);

            tabPage4.ResumeLayout(true);
        }

        private static void SetPtzGroupBounds(
            Control control, int x, int y, int width, int height)
        {
            control.Location = new Point(x, y);
            control.Size = new Size(width, height);
            control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }

        private static void CompactPtzCommandButtons(Control root, int buttonWidth)
        {
            foreach (Control control in root.Controls)
            {
                FlowLayoutPanel panel = control as FlowLayoutPanel;
                if (panel != null)
                {
                    panel.AutoScroll = false;
                    panel.WrapContents = true;
                    panel.Padding = new Padding(5);
                }

                Button button = control as Button;
                if (button != null)
                {
                    button.AutoSize = false;
                    button.Dock = DockStyle.None;
                    button.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    button.Size = new Size(buttonWidth, 28);
                    button.Margin = new Padding(3);
                }

                if (control.HasChildren)
                    CompactPtzCommandButtons(control, buttonWidth);
            }
        }

        /// <summary>
        /// 参考官方云台程序的信息架构重组界面：
        /// 首页显示状态、九宫格方向和角度定位，其余功能按类别分页。
        /// </summary>
        private void BuildOfficialStylePtzPage()
        {
            if (tabPage4 == null || tabPage4.Tag as string == "official-layout")
                return;

            tabPage4.Tag = "official-layout";
            tabPage4.SuspendLayout();

            // 先解除原有父子关系，再将现有控件重新分配到功能页面。
            tabPage4.Controls.Clear();

            TabControl navigation = new TabControl
            {
                Name = "tabPtzNavigation",
                Dock = DockStyle.Fill,
                Font = this.Font,
                Padding = new Point(18, 6)
            };

            TabPage homePage = CreatePtzPage("首页");
            TabPage presetPage = CreatePtzPage("预置位");
            TabPage configPage = CreatePtzPage("配置信息");
            TabPage areaPage = CreatePtzPage("区域扫描");
            TabPage zeroPage = CreatePtzPage("设置0位");

            navigation.TabPages.Add(homePage);
            navigation.TabPages.Add(presetPage);
            navigation.TabPages.Add(configPage);
            navigation.TabPages.Add(areaPage);
            navigation.TabPages.Add(zeroPage);
            tabPage4.Controls.Add(navigation);

            BuildPtzHomePage(homePage);
            BuildPtzPresetPage(presetPage);
            BuildPtzConfigPage(configPage);
            BuildPtzAreaPage(areaPage);
            BuildPtzZeroPage(zeroPage);
            ConfigurePtzTextLayout(navigation);

            tabPage4.ResumeLayout(true);
        }

        /// <summary>
        /// 让标签根据文字自动扩展，避免字体或 DPI 缩放后文字被固定尺寸裁切。
        /// </summary>
        private static void ConfigurePtzTextLayout(Control root)
        {
            foreach (Control control in root.Controls)
            {
                Label label = control as Label;
                if (label != null)
                    label.AutoSize = true;

                FlowLayoutPanel flowPanel = control as FlowLayoutPanel;
                if (flowPanel != null)
                {
                    flowPanel.AutoScroll = false;
                    flowPanel.WrapContents = true;
                }

                if (control.HasChildren)
                    ConfigurePtzTextLayout(control);
            }
        }

        private static TabPage CreatePtzPage(string title)
        {
            return new TabPage
            {
                Text = title,
                BackColor = Color.WhiteSmoke,
                AutoScroll = false,
                Padding = new Padding(10)
            };
        }

        private void BuildPtzHomePage(TabPage page)
        {
            // 左栏：通信配置与状态查询。
            gbxPtzNet.Location = new Point(10, 10);
            gbxPtzNet.Size = new Size(430, 120);
            gbxPtzQuery.Location = new Point(10, 140);
            gbxPtzQuery.Size = new Size(430, 200);

            // 中栏：官方风格九宫格方向控制。
            gbxPtzDirect.Location = new Point(450, 10);
            gbxPtzDirect.Size = new Size(390, 330);
            ArrangePtzDirectionPad();

            // 右栏：角度定位。
            gbxPtzLocate.Location = new Point(850, 10);
            gbxPtzLocate.Size = new Size(430, 330);
            ArrangePtzLocatePanel();

            page.Controls.Add(gbxPtzNet);
            page.Controls.Add(gbxPtzQuery);
            page.Controls.Add(gbxPtzDirect);
            page.Controls.Add(gbxPtzLocate);
        }

        private void ArrangePtzDirectionPad()
        {
            // 速度参数位于顶部。
            lblHS.Location = new Point(15, 28);
            nud_PtzHSpeed.Location = new Point(95, 25);
            lblVS.Location = new Point(205, 28);
            nud_PtzVSpeed.Location = new Point(285, 25);

            Button[] directionButtons =
            {
                btnPtzSupDirLeftUp, btnPtzUp, btnPtzSupDirRightUp,
                btnPtzLeft, btnPtzStop, btnPtzRight,
                btnPtzSupDirLeftDown, btnPtzDown, btnPtzSupDirRightDown
            };

            string[] texts =
            {
                "左上", "上", "右上",
                "左", "■ 停止", "右",
                "左下", "下", "右下"
            };

            for (int index = 0; index < directionButtons.Length; index++)
            {
                Button button = directionButtons[index];
                button.Text = texts[index];
                button.Size = new Size(95, 42);
                button.Location = new Point(
                    35 + (index % 3) * 110,
                    85 + (index / 3) * 55);
                gbxPtzDirect.Controls.Add(button);
            }
        }

        private void ArrangePtzLocatePanel()
        {
            lblHA.Location = new Point(25, 55);
            nud_PtzHAngle.Location = new Point(130, 52);
            lblVA.Location = new Point(25, 105);
            nud_PtzVAngle.Location = new Point(130, 102);
            chk_PtzUseSpeedLocate.Location = new Point(25, 155);
            btnPtzLocate.Location = new Point(25, 205);
            btnPtzLocate.Size = new Size(370, 42);
        }

        private void BuildPtzPresetPage(TabPage page)
        {
            gbxPtzPreset.Location = new Point(10, 10);
            gbxPtzPreset.Size = new Size(430, 170);
            gbxPtzExtPreset.Location = new Point(450, 10);
            gbxPtzExtPreset.Size = new Size(550, 340);

            page.Controls.Add(gbxPtzPreset);
            page.Controls.Add(gbxPtzExtPreset);
        }

        private void BuildPtzAreaPage(TabPage page)
        {
            gbxPtzArea.Location = new Point(10, 10);
            gbxPtzArea.Size = new Size(430, 260);
            gbxPtzExtArea.Location = new Point(450, 10);
            gbxPtzExtArea.Size = new Size(550, 440);
            gbxPtzArea.Text = "区域扫描参数";
            gbxPtzExtArea.Text = "区域扫描操作";

            page.Controls.Add(gbxPtzArea);
            page.Controls.Add(gbxPtzExtArea);
        }

        private void BuildPtzConfigPage(TabPage page)
        {
            gbxPtzRaw.Location = new Point(10, 10);
            gbxPtzRaw.Size = new Size(430, 70);
            gbxPtzExtBasic.Location = new Point(10, 90);
            gbxPtzExtBasic.Size = new Size(550, 280);
            gbxPtzExtBasic.Text = "设备基础配置";
            gbxPtzExtQuery.Location = new Point(570, 10);
            gbxPtzExtQuery.Size = new Size(550, 330);
            gbxPtzExtQuery.Text = "配置查询与回传";

            page.Controls.Add(gbxPtzRaw);
            page.Controls.Add(gbxPtzExtBasic);
            page.Controls.Add(gbxPtzExtQuery);
        }

        private void BuildPtzZeroPage(TabPage page)
        {
            gbxPtzExtMaintenance.Location = new Point(10, 10);
            gbxPtzExtMaintenance.Size = new Size(700, 240);
            gbxPtzExtMaintenance.Text = "零位设置与维护操作";

            page.Controls.Add(gbxPtzExtMaintenance);
        }

        private string GetPtzSupplementCommandKey(object sender)
        {
            Button button = sender as Button;
            return button?.Tag as string;
        }

        private void PtzSupplementButton_Click(object sender, EventArgs e)
        {
            string key = GetPtzSupplementCommandKey(sender);
            if (string.IsNullOrEmpty(key))
                return;

            ExecutePtzSupplementCommand(key);
        }

        private void PtzSupplementDirectionButton_MouseDown(object sender, MouseEventArgs e)
        {
            string key = GetPtzSupplementCommandKey(sender);
            if (string.IsNullOrEmpty(key))
                return;

            ExecutePtzSupplementCommand(key);
        }

        private void PtzSupplementDirectionButton_MouseUp(object sender, MouseEventArgs e)
        {
            PtzSendStop();
        }

        private void PtzSupplementDirectionButton_MouseLeave(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Capture)
                PtzSendStop();
        }

        private void btn_PtzOpen_Click(object sender, EventArgs e)
        {
            OpenPtzUdp();
        }

        private void btn_PtzClose_Click(object sender, EventArgs e)
        {
            _ptzController?.Close();
            lbl_PtzStatus.Text = "状态: 已关闭";
            PtzLog("UDP 已关闭。");
        }

        private void btn_PtzRawSend_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] input = PtzProtocol.ParseHex(txt_PtzRawHex.Text);
                byte[] data;

                if (input.Length == 4)
                {
                    data = PtzProtocol.Build(GetPtzAddress(), input[0], input[1], input[2], input[3]);
                }
                else if (input.Length == 5)
                {
                    data = PtzProtocol.Build(input[0], input[1], input[2], input[3], input[4]);
                }
                else if (input.Length == 6 && input[0] == PtzProtocol.Header)
                {
                    data = PtzProtocol.Build(input[1], input[2], input[3], input[4], input[5]);
                }
                else if (input.Length == 7)
                {
                    data = input;
                    PtzPacket packet;
                    if (!PtzPacket.TryParse(data, 0, out packet))
                    {
                        MessageBox.Show("完整帧校验失败，请检查校验码。", "云台指令", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("高级指令长度必须是 4、5、6 或 7 字节。", "云台指令", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PtzSendBytes("高级指令", data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("高级指令格式错误: " + ex.Message, "云台指令", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool OpenPtzUdp()
        {
            try
            {
                string localIp = txt_PtzLocalIp.Text.Trim();
                int localPort = (int)nud_PtzLocalPort.Value;
                _ptzController.Open(localIp, localPort);
                lbl_PtzStatus.Text = localPort == 0 ? "状态: 已打开(随机本地端口)" : $"状态: 已打开({localPort})";
                PtzLog($"UDP 已打开，本地 {localIp}:{localPort}。");
                return true;
            }
            catch (Exception ex)
            {
                lbl_PtzStatus.Text = "状态: 打开失败";
                MessageBox.Show("打开云台 UDP 失败: " + ex.Message, "云台UDP", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PtzLog("[错误] 打开 UDP 失败: " + ex.Message);
                return false;
            }
        }

        private bool EnsurePtzUdpOpen()
        {
            if (_ptzController != null && _ptzController.IsOpen)
                return true;

            return OpenPtzUdp();
        }

        private byte GetPtzAddress()
        {
            return (byte)nud_PtzAddress.Value;
        }

        private byte GetPtzHSpeedByte()
        {
            return PtzProtocol.EncodeSpeed10(nud_PtzHSpeed.Value);
        }

        private byte GetPtzVSpeedByte()
        {
            return PtzProtocol.EncodeSpeed10(nud_PtzVSpeed.Value);
        }

        private void PtzSendCommand(string label, byte data1, byte data2, byte data3, byte data4)
        {
            byte[] packet = PtzProtocol.Build(GetPtzAddress(), data1, data2, data3, data4);
            PtzSendBytes(label, packet);
        }

        private void PtzSendBytes(string label, byte[] packet)
        {
            try
            {
                if (!EnsurePtzUdpOpen())
                    return;

                string ip = txt_PtzIp.Text.Trim();
                int port = (int)nud_PtzPort.Value;
                _ptzController.Send(packet, ip, port);
                PtzLog($"发送[{label}] -> {ip}:{port}  {PtzProtocol.ToHex(packet)}");
            }
            catch (Exception ex)
            {
                PtzLog("[错误] 发送失败: " + ex.Message);
                MessageBox.Show("发送云台指令失败: " + ex.Message, "云台UDP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PtzSendStop()
        {
            PtzSendCommand("停止转动", 0x00, 0x00, 0x00, 0x00);
        }

        private void PtzSendDirection(byte command, bool useHSpeed, bool useVSpeed)
        {
            byte hSpeed = useHSpeed ? GetPtzHSpeedByte() : (byte)0x00;
            byte vSpeed = useVSpeed ? GetPtzVSpeedByte() : (byte)0x00;
            PtzSendCommand("方向控制", 0x00, command, hSpeed, vSpeed);
        }

        private void PtzSendLocate(bool horizontal)
        {
            decimal angle = horizontal ? nud_PtzHAngle.Value : nud_PtzVAngle.Value;
            ushort encoded = PtzProtocol.EncodeAngle100(angle);
            byte high = PtzProtocol.HighByte(encoded);
            byte low = PtzProtocol.LowByte(encoded);

            if (chk_PtzUseSpeedLocate.Checked)
            {
                if (horizontal)
                    PtzSendCommand("水平速度定位", 0x4b, GetPtzHSpeedByte(), high, low);
                else
                    PtzSendCommand("垂直速度定位", 0x4d, GetPtzVSpeedByte(), high, low);
            }
            else
            {
                if (horizontal)
                    PtzSendCommand("水平角度定位", 0x00, 0x4b, high, low);
                else
                    PtzSendCommand("垂直角度定位", 0x00, 0x4d, high, low);
            }
        }

        private void PtzSendPreset(byte command, string label)
        {
            PtzSendCommand(label, 0x00, command, 0x00, (byte)nud_PtzPreset.Value);
        }

        private void PtzSendAngleDataCommand(string label, byte data1, byte data2, decimal angle)
        {
            ushort encoded = PtzProtocol.EncodeAngle100(angle);
            PtzSendCommand(label, data1, data2, PtzProtocol.HighByte(encoded), PtzProtocol.LowByte(encoded));
        }

        private void PtzSendUInt16Command(string label, byte data1, byte data2, int value)
        {
            ushort encoded = PtzProtocol.EncodeUInt16(value, label);
            PtzSendCommand(label, data1, data2, PtzProtocol.HighByte(encoded), PtzProtocol.LowByte(encoded));
        }

        private void PtzSendAreaBoundariesByAngle()
        {
            byte area = (byte)nud_PtzArea.Value;
            PtzSendAngleDataCommand("区域HA角度边界", 0xf7, area, nud_PtzAreaHStart.Value);
            PtzSendAngleDataCommand("区域HB角度边界", 0xf8, area, nud_PtzAreaHEnd.Value);
            PtzSendAngleDataCommand("区域VA角度边界", 0xf9, area, nud_PtzAreaVStart.Value);
            PtzSendAngleDataCommand("区域VB角度边界", 0xfa, area, nud_PtzAreaVEnd.Value);
        }

        private void PtzSendAreaIntervals()
        {
            byte area = (byte)nud_PtzArea.Value;
            PtzSendAngleDataCommand("区域水平间隔", 0xfb, area, nud_PtzAreaHInterval.Value);
            PtzSendAngleDataCommand("区域垂直间隔", 0xfc, area, nud_PtzAreaVInterval.Value);
        }

        private void PtzSendZeroByAngle(bool horizontal)
        {
            decimal angle = horizontal ? nud_PtzHAngle.Value : nud_PtzVAngle.Value;
            ushort encoded = PtzProtocol.EncodeAngle100(angle);
            PtzSendCommand(horizontal ? "按水平角设置0位" : "按垂直角设置0位",
                0xe3,
                horizontal ? (byte)0x04 : (byte)0x05,
                PtzProtocol.HighByte(encoded),
                PtzProtocol.LowByte(encoded));
        }

        private void PtzSendReturnZero()
        {
            const byte zeroHigh = 0x00;
            const byte zeroLow = 0x00;

            if (chk_PtzUseSpeedLocate.Checked)
            {
                PtzSendCommand("水平回零(带速度)", 0x4b, GetPtzHSpeedByte(), zeroHigh, zeroLow);
                Thread.Sleep(50);
                PtzSendCommand("垂直回零(带速度)", 0x4d, GetPtzVSpeedByte(), zeroHigh, zeroLow);
            }
            else
            {
                PtzSendCommand("水平回零", 0x00, 0x4b, zeroHigh, zeroLow);
                Thread.Sleep(50);
                PtzSendCommand("垂直回零", 0x00, 0x4d, zeroHigh, zeroLow);
            }
        }

        private byte GetPtzArea()
        {
            return (byte)nud_PtzArea.Value;
        }

        private byte GetPtzPreset()
        {
            return (byte)nud_PtzPreset.Value;
        }

        private void PtzSendPresetByAngle()
        {
            byte preset = GetPtzPreset();
            ushort hAngle = PtzProtocol.EncodeAngle100(nud_PtzHAngle.Value);
            ushort vAngle = PtzProtocol.EncodeAngle100(nud_PtzVAngle.Value);

            PtzSendCommand("按水平角设置预置位", 0xe4, preset, PtzProtocol.HighByte(hAngle), PtzProtocol.LowByte(hAngle));
            Thread.Sleep(50);
            PtzSendCommand("按垂直角设置预置位", 0xe5, preset, PtzProtocol.HighByte(vAngle), PtzProtocol.LowByte(vAngle));
        }

        private bool ConfirmPtzDangerousCommand(string commandName)
        {
            DialogResult result = MessageBox.Show(
                $"确认发送“{commandName}”指令？",
                "云台指令确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            return result == DialogResult.Yes;
        }

        private void ExecutePtzSupplementCommand(string key)
        {
            byte area = GetPtzArea();
            byte preset = GetPtzPreset();

            switch (key)
            {
                case "dir_up":
                    PtzSendDirection(0x08, false, true);
                    break;
                case "dir_down":
                    PtzSendDirection(0x10, false, true);
                    break;
                case "dir_left":
                    PtzSendDirection(0x04, true, false);
                    break;
                case "dir_right":
                    PtzSendDirection(0x02, true, false);
                    break;
                case "dir_left_up":
                    PtzSendDirection(0x0c, true, true);
                    break;
                case "dir_right_up":
                    PtzSendDirection(0x0a, true, true);
                    break;
                case "dir_left_down":
                    PtzSendDirection(0x14, true, true);
                    break;
                case "dir_right_down":
                    PtzSendDirection(0x12, true, true);
                    break;
                case "stop":
                    PtzSendStop();
                    break;
                case "locate_h":
                    PtzSendLocate(true);
                    break;
                case "locate_v":
                    PtzSendLocate(false);
                    break;
                case "query_h_angle":
                    PtzSendCommand("查询水平角度", 0x00, 0x51, 0x00, 0x00);
                    break;
                case "query_v_angle":
                    PtzSendCommand("查询垂直角度", 0x00, 0x53, 0x00, 0x00);
                    break;
                case "power_1_on":
                    PtzSendCommand("电源1打开", 0x00, 0x09, 0x00, 0x03);
                    break;
                case "power_2_on":
                    PtzSendCommand("电源2打开", 0x00, 0x09, 0x00, 0x04);
                    break;
                case "power_1_off":
                    PtzSendCommand("电源1关闭", 0x00, 0x0b, 0x00, 0x03);
                    break;
                case "power_2_off":
                    PtzSendCommand("电源2关闭", 0x00, 0x0b, 0x00, 0x04);
                    break;

                case "area_angle_ha":
                    PtzSendAngleDataCommand("区域HA角度边界", 0xf7, area, nud_PtzAreaHStart.Value);
                    break;
                case "area_angle_hb":
                    PtzSendAngleDataCommand("区域HB角度边界", 0xf8, area, nud_PtzAreaHEnd.Value);
                    break;
                case "area_angle_va":
                    PtzSendAngleDataCommand("区域VA角度边界", 0xf9, area, nud_PtzAreaVStart.Value);
                    break;
                case "area_angle_vb":
                    PtzSendAngleDataCommand("区域VB角度边界", 0xfa, area, nud_PtzAreaVEnd.Value);
                    break;
                case "area_video_ha":
                    PtzSendCommand("当前水平写入HA边界", 0xe6, area, 0x00, 0x00);
                    break;
                case "area_video_hb":
                    PtzSendCommand("当前水平写入HB边界", 0xe7, area, 0x00, 0x00);
                    break;
                case "area_video_va":
                    PtzSendCommand("当前垂直写入VA边界", 0xe8, area, 0x00, 0x00);
                    break;
                case "area_video_vb":
                    PtzSendCommand("当前垂直写入VB边界", 0xe9, area, 0x00, 0x00);
                    break;
                case "area_interval_h":
                    PtzSendAngleDataCommand("区域水平间隔", 0xfb, area, nud_PtzAreaHInterval.Value);
                    break;
                case "area_interval_v":
                    PtzSendAngleDataCommand("区域垂直间隔", 0xfc, area, nud_PtzAreaVInterval.Value);
                    break;
                case "area_set_speed":
                    PtzSendCommand("配置区域扫描转速", 0xfd, area, GetPtzHSpeedByte(), GetPtzVSpeedByte());
                    break;
                case "area_set_time":
                    PtzSendUInt16Command("配置区域停止时间", 0xf6, area, (int)nud_PtzAreaTime.Value);
                    break;
                case "area_enable":
                    PtzSendCommand("使能当前区域", 0xf4, area, 0x01, 0x00);
                    break;
                case "area_disable":
                    PtzSendCommand("禁用当前区域", 0xf4, area, 0x00, 0x00);
                    break;
                case "area_start_single":
                    PtzSendCommand("开启单区域扫描", 0xf5, 0x01, area, 0x00);
                    break;
                case "area_start_multi":
                    PtzSendCommand("开启区域/多区扫描", 0xf5, 0x02, (byte)nud_PtzAreaStart.Value, (byte)nud_PtzAreaEnd.Value);
                    break;
                case "area_pause":
                    PtzSendCommand("暂停区域扫描", 0xf5, 0x03, 0x00, 0x00);
                    break;
                case "area_continue":
                    PtzSendCommand("恢复区域扫描", 0xf5, 0x04, 0x00, 0x00);
                    break;
                case "area_close":
                    PtzSendCommand("彻底关闭区域扫描", 0xf5, 0x05, 0x00, 0x00);
                    break;
                case "area_mode_step":
                    PtzSendCommand("设置单步扫描模式", 0xf5, 0x06, area, 0x02);
                    break;
                case "area_mode_continuous":
                    PtzSendCommand("设置连续扫描模式", 0xf5, 0x06, area, 0x01);
                    break;
                case "area_save":
                    PtzSendCommand("保存区域扫描数据", 0xf3, 0x00, 0x00, 0x00);
                    break;
                case "area_query":
                    PtzSendCommand("查询当前区域配置", 0xca, area, 0x00, 0x00);
                    break;
                case "area_end_return_on":
                    PtzSendCommand("开启区域结束回传", 0xc4, 0x01, 0x00, 0x00);
                    break;
                case "area_end_return_off":
                    PtzSendCommand("关闭区域结束回传", 0xc4, 0x00, 0x00, 0x00);
                    break;
                case "area_step_return_on":
                    PtzSendCommand("开启单步到位回传", 0xc4, 0x03, 0x00, 0x00);
                    break;
                case "area_step_return_off":
                    PtzSendCommand("关闭单步到位回传", 0xc4, 0x04, 0x00, 0x00);
                    break;

                case "preset_standard_set":
                    PtzSendPreset(0x03, "设置预置位");
                    break;
                case "preset_standard_call":
                    PtzSendPreset(0x07, "调用预置位");
                    break;
                case "preset_standard_delete":
                    PtzSendPreset(0x05, "删除预置位");
                    break;
                case "preset_set_by_angle":
                    PtzSendPresetByAngle();
                    break;
                case "preset_set_h_angle":
                    PtzSendAngleDataCommand("按水平角设置预置位", 0xe4, preset, nud_PtzHAngle.Value);
                    break;
                case "preset_set_v_angle":
                    PtzSendAngleDataCommand("按垂直角设置预置位", 0xe5, preset, nud_PtzVAngle.Value);
                    break;
                case "preset_set_time":
                    PtzSendUInt16Command("设置预置位驻留时间", 0xf1, preset, (int)nud_PtzPresetTime.Value);
                    break;
                case "preset_set_speed":
                    PtzSendCommand("设置预置位扫描速度", 0xf2, preset, GetPtzHSpeedByte(), GetPtzVSpeedByte());
                    break;
                case "preset_start":
                    PtzSendCommand("开启预置巡航", 0xf0, 0x01, (byte)nud_PtzPresetStart.Value, (byte)nud_PtzPresetEnd.Value);
                    break;
                case "preset_pause":
                    PtzSendCommand("暂停预置位扫描", 0xf0, 0x02, 0x00, 0x00);
                    break;
                case "preset_continue":
                    PtzSendCommand("恢复预置位扫描", 0xf0, 0x03, 0x00, 0x00);
                    break;
                case "preset_close":
                    PtzSendCommand("彻底关闭巡航", 0xf0, 0x04, 0x00, 0x00);
                    break;
                case "preset_end_return_on":
                    PtzSendCommand("开启预置扫描结束回传", 0x9f, 0x01, 0x00, 0x00);
                    break;
                case "preset_end_return_off":
                    PtzSendCommand("关闭预置扫描结束回传", 0x9f, 0x00, 0x00, 0x00);
                    break;
                case "preset_arrive_return_on":
                    PtzSendCommand("开启预置扫描到位回传", 0x9f, 0x02, 0x00, 0x00);
                    break;
                case "preset_arrive_return_off":
                    PtzSendCommand("关闭预置扫描到位回传", 0x9f, 0x03, 0x00, 0x00);
                    break;
                case "preset_call_return_on":
                    PtzSendCommand("开启调用预置位到位回传", 0x9f, 0x04, 0x00, 0x00);
                    break;
                case "preset_call_return_off":
                    PtzSendCommand("关闭调用预置位到位回传", 0x9f, 0x05, 0x00, 0x00);
                    break;

                case "ack_on":
                    PtzSendCommand("开启指令回复", 0xdf, 0x00, 0x01, 0x00);
                    break;
                case "ack_off":
                    PtzSendCommand("关闭指令回复", 0xdf, 0x00, 0x00, 0x00);
                    break;
                case "query_mode":
                    PtzSendCommand("查询工作模式", 0xe0, 0x00, 0x00, 0x00);
                    break;
                case "query_status":
                    PtzSendCommand("查询工作状态", 0xdd, 0x00, 0x00, 0x00);
                    break;
                case "zero_h_current":
                    PtzSendCommand("当前水平设为0位", 0xe3, 0x01, 0x00, 0x00);
                    break;
                case "zero_v_current":
                    PtzSendCommand("当前垂直设为0位", 0xe3, 0x02, 0x00, 0x00);
                    break;
                case "zero_hv_current":
                    PtzSendCommand("当前水平/垂直设为0位", 0xe3, 0x03, 0x00, 0x00);
                    break;
                case "zero_h_angle":
                    PtzSendZeroByAngle(true);
                    break;
                case "zero_v_angle":
                    PtzSendZeroByAngle(false);
                    break;
                case "zero_delete":
                    PtzSendCommand("删除水平/垂直0位", 0xe3, 0x06, 0x00, 0x00);
                    break;
                case "return_zero":
                    PtzSendReturnZero();
                    break;
                case "query_temperature":
                    PtzSendCommand("查询工作温度", 0xd6, 0x00, 0x00, 0x00);
                    break;
                case "query_voltage":
                    PtzSendCommand("查询工作电压", 0xcd, 0x00, 0x00, 0x00);
                    break;
                case "query_current":
                    PtzSendCommand("查询工作电流", 0xc8, 0x00, 0x00, 0x00);
                    break;
                case "angle_realtime_on":
                    PtzSendUInt16Command("开启连续回传", 0xe1, 0x01, (int)nud_PtzRealtimeInterval.Value);
                    break;
                case "angle_realtime_off":
                    PtzSendCommand("关闭回传", 0xe1, 0x02, 0x00, 0x00);
                    break;
                case "query_h_speed":
                    PtzSendCommand("查询水平转速", 0xd0, 0x01, 0x00, 0x00);
                    break;
                case "query_v_speed":
                    PtzSendCommand("查询垂直转速", 0xd0, 0x02, 0x00, 0x00);
                    break;
                case "query_all_speed":
                    PtzSendCommand("查询全部转速", 0xd0, 0x00, 0x00, 0x00);
                    break;
                case "speed_realtime_on":
                    PtzSendUInt16Command("开启转速实时回传", 0xdc, 0x01, (int)nud_PtzRealtimeInterval.Value);
                    break;
                case "speed_realtime_off":
                    PtzSendCommand("关闭转速实时回传", 0xdc, 0x02, 0x00, 0x00);
                    break;
                case "reboot":
                    if (ConfirmPtzDangerousCommand("云台复位重启"))
                        PtzSendCommand("云台复位重启", 0xde, 0x00, 0x00, 0x00);
                    break;
                case "self_check":
                    if (ConfirmPtzDangerousCommand("云台全范围自检"))
                        PtzSendCommand("云台全范围自检", 0xce, 0x00, 0x00, 0x00);
                    break;
                case "locate_return_on":
                    PtzSendCommand("开启角度定位回传", 0xc5, 0x01, 0x00, 0x00);
                    break;
                case "locate_return_off":
                    PtzSendCommand("关闭角度定位回传", 0xc5, 0x00, 0x00, 0x00);
                    break;
                default:
                    MessageBox.Show("未识别的补充指令。", "云台指令", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private void HandlePtzPacketReceived(PtzPacket packet, IPEndPoint remote)
        {
            if (IsDisposed) return;

            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { HandlePtzPacketReceived(packet, remote); });
                return;
            }

            string description = PtzProtocol.DescribePacket(packet);
            PtzLog($"接收[{remote.Address}:{remote.Port}] {packet.ToHex()}  {description}");

            if (packet.Data1 == 0x00 && packet.Data2 == 0x59)
                lbl_PtzHAngle.Text = $"水平角度: {PtzProtocol.DecodeUnsigned100(packet.Data3, packet.Data4):F2}";
            else if (packet.Data1 == 0x00 && packet.Data2 == 0x5b)
                lbl_PtzVAngle.Text = $"垂直角度: {PtzProtocol.DecodeSigned100(packet.Data3, packet.Data4):F2}";
            else if (packet.Data1 == 0xe0)
                lbl_PtzStatus.Text = "状态: " + description;
        }

        private void PtzLog(string message)
        {
            // 云台与雷达、相机共用第一页的主日志框，并保留来源标识。
            AddLog("[云台] " + message);
        }

        #endregion
        private void btnPtzOpen_Click(object sender, EventArgs e)
        {
            OpenPtzUdp();
        }

        private void btnPtzClose_Click(object sender, EventArgs e)
        {
            _ptzController?.Close();
            lbl_PtzStatus.Text = "状态: 已关闭";
            PtzLog("UDP 已关闭。");
        }

        private void btnPtzSendRaw_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] input = PtzProtocol.ParseHex(txt_PtzRawHex.Text);
                byte[] data;

                if (input.Length == 4)
                {
                    data = PtzProtocol.Build(GetPtzAddress(), input[0], input[1], input[2], input[3]);
                }
                else if (input.Length == 5)
                {
                    data = PtzProtocol.Build(input[0], input[1], input[2], input[3], input[4]);
                }
                else if (input.Length == 6 && input[0] == PtzProtocol.Header)
                {
                    data = PtzProtocol.Build(input[1], input[2], input[3], input[4], input[5]);
                }
                else if (input.Length == 7)
                {
                    data = input;
                    PtzPacket packet;
                    if (!PtzPacket.TryParse(data, 0, out packet))
                    {
                        MessageBox.Show("完整帧校验失败，请检查校验码。", "云台指令", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("高级指令长度必须是 4、5、6 或 7 字节。", "云台指令", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PtzSendBytes("高级指令", data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("高级指令格式错误: " + ex.Message, "云台指令", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- 方向控制 ---

        private void btnPtzUp_MouseDown(object sender, MouseEventArgs e)
        {
            PtzSendDirection(0x08, false, true); // 0x08 向上
        }

        private void btnPtzDown_MouseDown(object sender, MouseEventArgs e)
        {
            PtzSendDirection(0x10, false, true); // 0x10 向下
        }

        private void btnPtzLeft_MouseDown(object sender, MouseEventArgs e)
        {
            PtzSendDirection(0x04, true, false); // 0x04 向左
        }

        private void btnPtzRight_MouseDown(object sender, MouseEventArgs e)
        {
            PtzSendDirection(0x02, true, false); // 0x02 向右
        }

        private void btnPtzDirection_MouseUp(object sender, MouseEventArgs e)
        {
            PtzSendStop(); // 鼠标释放发送停止
        }

        private void btnPtzStop_Click(object sender, EventArgs e)
        {
            PtzSendStop(); // 点击停止按钮
        }

        // --- 角度定位 ---

        private void btnPtzLocate_Click(object sender, EventArgs e)
        {
            // 依次发送水平和垂直定位指令
            PtzSendLocate(true);
            System.Threading.Thread.Sleep(50); // 略微延时防止 UDP 乱序或云台处理不过来
            PtzSendLocate(false);
        }

        // --- 预置位控制 ---

        private void btnPtzPresetSet_Click(object sender, EventArgs e)
        {
            PtzSendPreset(0x03, "设置预置位");
        }

        private void btnPtzPresetCall_Click(object sender, EventArgs e)
        {
            PtzSendPreset(0x07, "调用预置位");
        }

        private void btnPtzPresetDel_Click(object sender, EventArgs e)
        {
            PtzSendPreset(0x05, "删除预置位");
        }

        private void btnPtzPresetScanStart_Click(object sender, EventArgs e)
        {
            // Start 预置位扫描 0xf0 0x01
            PtzSendCommand("开启预置巡航", 0xf0, 0x01, (byte)nud_PtzPresetStart.Value, (byte)nud_PtzPresetEnd.Value);
        }

        private void btnPtzPresetScanStop_Click(object sender, EventArgs e)
        {
            // Close 彻底关闭扫描 0xf0 0x04
            PtzSendCommand("彻底关闭巡航", 0xf0, 0x04, 0x00, 0x00);
        }

        // --- 区域扫描控制---

        private void btnPtzAreaSetBound_Click(object sender, EventArgs e)
        {
            PtzSendAreaBoundariesByAngle();
        }

        private void btnPtzAreaSetInterval_Click(object sender, EventArgs e)
        {
            PtzSendAreaIntervals();
        }

        private void btnPtzAreaScanStart_Click(object sender, EventArgs e)
        {
            // 开启多区域扫描 Start_M: 0xf5 0x02
            byte startArea = (byte)nud_PtzAreaStart.Value;
            byte endArea = (byte)nud_PtzAreaEnd.Value;
            PtzSendCommand("开启区域/多区扫描", 0xf5, 0x02, startArea, endArea);
        }

        private void btnPtzAreaScanStop_Click(object sender, EventArgs e)
        {
            //  彻底关闭区域扫描 Close: 0xf5 0x05
            PtzSendCommand("彻底关闭区域扫描", 0xf5, 0x05, 0x00, 0x00);
        }

        // --- 查询与实时回传 ---

        private void btnPtzQueryStatus_Click(object sender, EventArgs e)
        {
            // 查询云台工作状态 0xdd 0 0 0
            PtzSendCommand("查询工作状态", 0xdd, 0x00, 0x00, 0x00);
        }

        private void btnPtzQueryMode_Click(object sender, EventArgs e)
        {
            //查询云台工作模式 0xe0 0 0 0
            PtzSendCommand("查询工作模式", 0xe0, 0x00, 0x00, 0x00);
        }

        private void btnPtzRealtimeAngleOn_Click(object sender, EventArgs e)
        {
            // 角度实时回传打开 0xe1 0x01 H_Time L_Time
            PtzSendUInt16Command("开启连续回传", 0xe1, 0x01, (int)nud_PtzRealtimeInterval.Value);
        }

        private void btnPtzRealtimeAngleOff_Click(object sender, EventArgs e)
        {
            //角度实时回传关闭 0xe1 0x02 0 0
            PtzSendCommand("关闭回传", 0xe1, 0x02, 0x00, 0x00);
        }
    }
}
