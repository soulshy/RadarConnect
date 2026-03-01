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

        // VTK 可视化包装器
        private VtkVisualizer _vtkVisualizer;

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

        public Form1()
        {
            InitializeComponent();
            InitGui();

            _udpClient = new UdpCommunication();
            _dbManager = new DatabaseManager();

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
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // 初始化相机播放器
            InitCameraPlayer();
        }

        private void InitGui()
        {
            if (cbx_WorkMode.Items.Count == 0)
                cbx_WorkMode.Items.AddRange(new object[] { "正常模式", "省电模式", "待机模式" });
            cbx_WorkMode.SelectedIndex = 0;

            if (cbx_Coordinate.Items.Count == 0)
                cbx_Coordinate.Items.AddRange(new object[] { "直角坐标", "球坐标" });
            cbx_Coordinate.SelectedIndex = 0;

            // 初始化时间选择器为当前时间
            dateTimePicker_Query.Value = DateTime.Now;

            // 相机 IP
            txt_CameraIp.Text = "192.168.1.168";
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
                                _mediaPlayer.AspectRatio = $"{panel_Video.Width}:{panel_Video.Height}";
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
                _isCameraPlaying = true;
                btn_PlayCamera.Enabled = true;
                btn_PlayCamera.Text = "停止视频";
                AddLog("4. 视频流连接成功，正在播放！");
            }));
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
                // 恢复工作目录，防止 FFmpeg 由于路径问题找不到文件
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
                DateTime videoStartTime = _videoProcessor.ParseVideoStartTime(Path.GetFileName(videoPath));
                TimeSpan offset = targetTime - videoStartTime;
                if (offset.TotalSeconds < 0)
                {
                    MessageBox.Show("所选时间早于视频的开始录制时间，请重新选择！");
                    return;
                }

                // 2. 调用 FFmpeg 抽帧模块
                AddLog($"[融合] 正在抽帧，视频偏移: {offset.TotalSeconds:F3} 秒...");
                string extractedImagePath = await _videoProcessor.ExtractFrameAsync(videoPath, offset);
                AddLog("[融合] 正在使用 OpenCV 进行图像增强与去噪...");
                string processedPath = await _imageProcessor.ProcessImageAsync(extractedImagePath, 15, 1.2, true);

                if (!File.Exists(extractedImagePath))
                {
                    AddLog("[融合] FFmpeg 抽帧失败。请确认程序目录下存在 ffmpeg.exe。");
                    return;
                }

                // 3. 调用数据库模块获取 1 秒的点云数据
                AddLog("[融合] 正在提取点云数据...");
                List<PointData> rawPoints = await Task.Run(() => _dbManager.GetPointsInRange(targetTime, 1.0));

                if (rawPoints == null || rawPoints.Count == 0)
                {
                    AddLog("[融合] 数据库中该时间段无点云数据。");
                    return;
                }

                // 4. 点云预处理 (过滤/降采样)
                List<PointData> filteredPoints = new List<PointData>();
                _processor.ApplyFilters(rawPoints, filteredPoints);

                // 5. 调用传感器融合模块进行 3D->2D 投影映射
                AddLog($"[融合] 正在将 {filteredPoints.Count} 个点投影至图像...");

                Image fusedImage = await Task.Run(() =>
             _sensorFusion.ProjectPointCloudToImage(processedPath, filteredPoints, 30.0f)); 
                // 6. UI 更新
                if (pictureBox_FusionResult.Image != null) pictureBox_FusionResult.Image.Dispose();
                pictureBox_FusionResult.Image = fusedImage;

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
            SendControlCommand(CmdSet.General, 0x04, new byte[] { 0x01 });
            _isSaving = true;
            _dbManager.StartSaving();
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

        #endregion

        #region 数据处理

        private void ProcessPointCloud(byte[] data)
        {
            int livoxHeaderSize = 18;
            if (data.Length < livoxHeaderSize) return;

            byte data_type = data[9];
            ulong radarTimestamp = BitConverter.ToUInt64(data, 10);

            DateTime frameTime;
            if (!_isTimeSynced || (Math.Abs((long)radarTimestamp - (long)_lastRadarTimestamp) > 1_000_000_000))
            {
                _basePcTicks = DateTime.UtcNow.Ticks;
                _baseRadarTime = radarTimestamp;
                _isTimeSynced = true;
            }
            _lastRadarTimestamp = radarTimestamp;

            long diffNs = unchecked((long)(radarTimestamp - _baseRadarTime));
            long diffTicks = diffNs / 100;
            frameTime = new DateTime(_basePcTicks + diffTicks, DateTimeKind.Utc);

            int pSize = 0;
            int returnCount = 1;

            const double AVIA_FIRING_FREQ = 240000.0;
            const double TICKS_PER_SECOND = 10000000.0;
            double pointIntervalTicks = TICKS_PER_SECOND / AVIA_FIRING_FREQ;

            switch (data_type)
            {
                case 2: pSize = 14; returnCount = 1; break;
                case 3: pSize = 10; returnCount = 1; break;
                case 4: pSize = 28; returnCount = 2; break;
                case 5: pSize = 16; returnCount = 2; break;
                case 7: pSize = 42; returnCount = 3; break;
                case 8: pSize = 22; returnCount = 3; break;
                case 0: pSize = 13; pointIntervalTicks = 100.0; break;
                case 1: pSize = 9; pointIntervalTicks = 100.0; break;
                default: return;
            }

            if (pSize == 0) return;

            int pointsDataLen = data.Length - livoxHeaderSize;
            int packetCount = pointsDataLen / pSize;

            for (int i = 0; i < packetCount; i++)
            {
                int baseOffset = livoxHeaderSize + (i * pSize);
                if (baseOffset + pSize > data.Length) break;

                DateTime firingTime = frameTime.AddTicks((long)(i * pointIntervalTicks));

                if (data_type == 2 || data_type == 4 || data_type == 7)
                {
                    for (int j = 0; j < returnCount; j++)
                    {
                        int ptOffset = baseOffset + (j * 14);
                        int x = BitConverter.ToInt32(data, ptOffset);
                        int y = BitConverter.ToInt32(data, ptOffset + 4);
                        int z = BitConverter.ToInt32(data, ptOffset + 8);
                        byte refI = data[ptOffset + 12];
                        byte tag = data[ptOffset + 13];
                        EnqueueCartesianPoint(firingTime, x, y, z, refI, tag);
                    }
                }
                else if (data_type == 3)
                {
                    uint depth = BitConverter.ToUInt32(data, baseOffset);
                    ushort theta = BitConverter.ToUInt16(data, baseOffset + 4);
                    ushort phi = BitConverter.ToUInt16(data, baseOffset + 6);
                    byte refI = data[baseOffset + 8];
                    byte tag = data[baseOffset + 9];
                    EnqueueSphericalPoint(firingTime, depth, theta, phi, refI, tag);
                }
                else if (data_type == 5 || data_type == 8)
                {
                    ushort theta = BitConverter.ToUInt16(data, baseOffset);
                    ushort phi = BitConverter.ToUInt16(data, baseOffset + 2);
                    for (int j = 0; j < returnCount; j++)
                    {
                        int ptOffset = baseOffset + 4 + (j * 6);
                        uint depth = BitConverter.ToUInt32(data, ptOffset);
                        byte refI = data[ptOffset + 4];
                        byte tag = data[ptOffset + 5];
                        EnqueueSphericalPoint(firingTime, depth, theta, phi, refI, tag);
                    }
                }
                else if (data_type == 0 || data_type == 1)
                {
                    if (data_type == 0)
                    {
                        int x = BitConverter.ToInt32(data, baseOffset);
                        int y = BitConverter.ToInt32(data, baseOffset + 4);
                        int z = BitConverter.ToInt32(data, baseOffset + 8);
                        byte refI = data[baseOffset + 12];
                        EnqueueCartesianPoint(firingTime, x, y, z, refI, 0);
                    }
                    else
                    {
                        uint d = BitConverter.ToUInt32(data, baseOffset);
                        ushort t = BitConverter.ToUInt16(data, baseOffset + 4);
                        ushort p = BitConverter.ToUInt16(data, baseOffset + 6);
                        byte r = data[baseOffset + 8];
                        EnqueueSphericalPoint(firingTime, d, t, p, r, 0);
                    }
                }
            }
        }

        private void EnqueueCartesianPoint(DateTime time, int x, int y, int z, byte reflectivity, byte tag)
        {
            if (x == 0 && y == 0 && z == 0) return;
            double distSq = (double)x * x + (double)y * y + (double)z * z;
            float depth_m = (distSq > 0) ? (float)(Math.Sqrt(distSq) / 1000.0) : 0;
            _dbManager.EnqueuePoint(time, x, y, z, depth_m, reflectivity, tag);
        }

        private void EnqueueSphericalPoint(DateTime time, uint depth_mm, ushort thetaRaw, ushort phiRaw, byte reflectivity, byte tag)
        {
            if (depth_mm == 0)
                return;
            double thetaRad = (thetaRaw * 0.01) * (Math.PI / 180.0);
            double phiRad = (phiRaw * 0.01) * (Math.PI / 180.0);
            double r = (double)depth_mm;
            int x = (int)(r * Math.Sin(thetaRad) * Math.Cos(phiRad));
            int y = (int)(r * Math.Sin(thetaRad) * Math.Sin(phiRad));
            int z = (int)(r * Math.Cos(thetaRad));
            float depth_m = depth_mm / 1000.0f;
            _dbManager.EnqueuePoint(time, x, y, z, depth_m, reflectivity, tag);
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

        /// <summary>
        /// 将日志内容追加到本地文件中
        /// </summary>
        private void WriteLogToFile(string logLine)
        {
            try
            {
                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                // 每天生成一个新的日志文件
                string logFile = Path.Combine(logDir, $"Log_{DateTime.Now:yyyyMMdd}.txt");

                // 使用锁防止多线程读写冲突
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
    }
}