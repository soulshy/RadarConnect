using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kitware.VTK;

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

        public Form1()
        {
            InitializeComponent();
            InitGui();

            _udpClient = new UdpCommunication();
            _dbManager = new DatabaseManager();

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
        }
        private void Form1_Load(object sender, EventArgs e)
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
            }
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
        }

        #region 数据库查询与还原

        // 点击按钮触发：根据 dateTimePicker 的时间查询并显示
        // 【核心功能】数据库查询与还原
        private async void btn_Reconstruct_Click(object sender, EventArgs e)
        {
            // 1. 获取用户选择的【起始时间】（北京时间）
            DateTime selectedStartTime = dateTimePicker_Query.Value;

            // 设定查询时长为 1 秒
            double durationSeconds = 1.0;

            // 显示日志
            AddLog($"[查询] 时间窗口: {selectedStartTime.ToString("HH:mm:ss")} -> +1s");

            btn_Reconstruct.Enabled = false;

            try
            {
                // 2. 异步后台查询
                List<PointData> pointsToRender = await Task.Run(() =>
                {
                    // 直接调用 DatabaseManager，传入起始时间和时长
                    // 内部会自动处理 UTC 转换和 "不足1s查到最后" 的逻辑
                    List<PointData> rawPoints = _dbManager.GetPointsInRange(selectedStartTime, durationSeconds);

                    if (rawPoints == null || rawPoints.Count == 0) return null;

                    // 3. 数据预处理
                    lock (_displayBuffer)
                    {
                        _processor.ApplyFilters(rawPoints, _displayBuffer);
                        return new List<PointData>(_displayBuffer);
                    }
                });

                // 4. UI 线程渲染
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

        #region 
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
            catch { }

            req.user_ip = IPAddress.Parse(localIpStr).GetAddressBytes();
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
            SendControlCommand(CmdSet.General, 0x06, null);
            StopAllWork();
            AddLog(">>> 请求断开");
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

        #region 

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
            if (depth_mm == 0) return;
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
            if (payload != null) pkt.AddRange(payload);

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
                        if (item.SubItems[1].Text == remote.Address.ToString()) { exists = true; break; }
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
            if (string.IsNullOrEmpty(_currentDeviceIp)) return;
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

        private void AddLog(string msg)
        {
            if (listBox_Log.InvokeRequired)
                listBox_Log.BeginInvoke((MethodInvoker)delegate { AddLog(msg); });
            else
            {
                listBox_Log.Items.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] {msg}");
                listBox_Log.TopIndex = listBox_Log.Items.Count - 1;
            }
        }

        #endregion
    }
}