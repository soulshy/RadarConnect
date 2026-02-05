using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace RadarConnect
{
    public partial class Form1 : Form
    {
        // ==========================================
        // 配置区域
        // ==========================================
        private const string FIXED_LOCAL_IP = "192.168.1.230";
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

        private VtkPointCloudForm _cloudViewer = null;
        private PointCloudProcessor _processor = new PointCloudProcessor();

        // 队列现在存储的是“点云列表(Batch)”，而不是单个点
        private ConcurrentQueue<List<PointData>> _uiDataQueue = new ConcurrentQueue<List<PointData>>();

        private System.Windows.Forms.Timer _uiRefreshTimer;

        private ulong _lastRadarTimestamp = 0;
        private System.Windows.Forms.Timer _heartbeatTimer;

        private bool _isTimeSynced = false;
        private long _basePcTicks = 0;
        private ulong _baseRadarTime = 0;

        public Form1()
        {
            InitializeComponent();
            InitGui();

            _udpClient = new UdpCommunication();
            _dbManager = new DatabaseManager();

            _heartbeatTimer = new System.Windows.Forms.Timer();
            _heartbeatTimer.Interval = 1000;
            _heartbeatTimer.Tick += OnHeartbeatTimerTick;

            // UI 刷新定时器
            _uiRefreshTimer = new System.Windows.Forms.Timer();
            _uiRefreshTimer.Interval = 100;
            _uiRefreshTimer.Tick += OnUiRefreshTimerTick;
            _uiRefreshTimer.Start();

            _udpClient.OnBroadcastReceived += HandleBroadcast;
            _udpClient.OnCmdAckReceived += HandleAck;
            _udpClient.OnDataReceived += ProcessPointCloud;
            _udpClient.OnError += (msg) => this.BeginInvoke((MethodInvoker)delegate { AddLog($"错误: {msg}"); });
        }

        private void InitGui()
        {
            cbx_WorkMode.Items.AddRange(new object[] { "正常模式", "省电模式", "待机模式" });
            cbx_WorkMode.SelectedIndex = 0;
            cbx_Coordinate.Items.AddRange(new object[] { "直角坐标", "球坐标" });
            cbx_Coordinate.SelectedIndex = 0;

            Button btn_ShowCloud = new Button();
            btn_ShowCloud.Text = "显示点云图";
            btn_ShowCloud.Location = new System.Drawing.Point(17, 280);
            btn_ShowCloud.Size = new System.Drawing.Size(160, 30);
            btn_ShowCloud.UseVisualStyleBackColor = true;
            btn_ShowCloud.Click += Btn_ShowCloud_Click;

            this.groupBox2.Controls.Add(btn_ShowCloud);
            if (this.groupBox2.Height < 330) 
                this.groupBox2.Height = 330;

            _processor.MinDistance = 1.5f;
            _processor.MinReflectivity = 0; 
            _processor.DownsampleFactor = 1;
        }

        private void Btn_ShowCloud_Click(object sender, EventArgs e)
        {
            if (_cloudViewer == null || _cloudViewer.IsDisposed)
            {
                // 实例化新的 VTK 窗口
                _cloudViewer = new VtkPointCloudForm();
                _cloudViewer.Show();
            }
            else
            {
                _cloudViewer.BringToFront();
                if (_cloudViewer.WindowState == FormWindowState.Minimized)
                    _cloudViewer.WindowState = FormWindowState.Normal;
            }
        }

        //批量取出数据
        private void OnUiRefreshTimerTick(object sender, EventArgs e)
        {
            if (_cloudViewer == null || _cloudViewer.IsDisposed || _uiDataQueue.IsEmpty)
            {
                if ((_cloudViewer == null || _cloudViewer.IsDisposed) && !_uiDataQueue.IsEmpty)
                {
                    while (_uiDataQueue.TryDequeue(out _)) { } // 清理积压
                }
                return;
            }

            // 准备一个大列表，容纳这 100ms 内收到的所有点
            List<PointData> aggregatedBatch = new List<PointData>(15000);

            // 一次性取出队列中所有的“包”
            while (_uiDataQueue.TryDequeue(out var batch))
            {
                aggregatedBatch.AddRange(batch);
            }

            // 如果有数据，一次性发给 UI
            if (aggregatedBatch.Count > 0)
            {
                _cloudViewer.AddPoints(aggregatedBatch);
            }
        }

        private void btn_StartListen_Click(object sender, EventArgs e) { 
            _udpClient.Start(FIXED_LOCAL_IP, LOCAL_CMD_PORT, LOCAL_DATA_PORT); 
            btn_StartListen.Enabled = false; 
            btn_StopListen.Enabled = true; 
            AddLog($"服务启动 | 命令端口:{LOCAL_CMD_PORT} 数据端口:{LOCAL_DATA_PORT}");
        }
        private void btn_StopListen_Click(object sender, EventArgs e) {
            StopAllWork(); _udpClient.Stop(); 
            btn_StartListen.Enabled = true; 
            btn_StopListen.Enabled = false; AddLog("服务停止");
        }
        private void btn_HandShake_Click(object sender, EventArgs e)
        {
            if (listView_Devices.SelectedItems.Count == 0) return;
            _currentDeviceIp = listView_Devices.SelectedItems[0].SubItems[1].Text;
            HandshakeRequest req = new HandshakeRequest();
            string localIpStr = FIXED_LOCAL_IP;
            try { var host = Dns.GetHostEntry(Dns.GetHostName()); foreach (var ip in host.AddressList) { if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString().StartsWith("192.168")) { localIpStr = ip.ToString(); break; } } } catch { }
            req.user_ip = IPAddress.Parse(localIpStr).GetAddressBytes();
            req.data_port = LOCAL_DATA_PORT; req.cmd_port = LOCAL_CMD_PORT; req.imu_port = LOCAL_DATA_PORT;
            SendControlCommand(CmdSet.General, (byte)GeneralCmdId.Handshake, ProtocolUtils.StructToBytes(req), 0x01);
            AddLog($"发送握手 -> {_currentDeviceIp} (Port:{TARGET_LIDAR_PORT})");
        }
        private void btn_StartSample_Click_1(object sender, EventArgs e)
        {
            SendControlCommand(CmdSet.General, 0x04, new byte[] { 0x01 });
            _isSaving = true;
            _dbManager.StartSaving(); AddLog("已发送 [开始采样] 指令，并启动入库。");
            btn_StartSample.Enabled = false;
            btn_StopSample.Enabled = true;
        }
        private void btn_StopSample_Click_1(object sender, EventArgs e)
        {
            SendControlCommand(CmdSet.General, 0x04, new byte[] { 0x00 });
            _isSaving = false; _dbManager.StopSaving();
            _isTimeSynced = false;
            AddLog("已发送 [停止采样] 指令，停止入库。");
            btn_StartSample.Enabled = true;
            btn_StopSample.Enabled = false;
        }
        private void btn_Disconnect_Click_1(object sender, EventArgs e)
        {
            SendControlCommand(CmdSet.General, 0x06, null);
            StopAllWork();
            AddLog(">>> 已发送断开请求");
        }
        private void StopAllWork()
        {
            _heartbeatTimer.Stop();
            _isSaving = false; _dbManager.StopSaving();
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
            pkt[2] = lenB[0]; pkt[3] = lenB[1]; byte[] header = pkt.GetRange(0, 7).ToArray();
            ushort c16 = ProtocolUtils.Crc16(header, 7); byte[] c16B = BitConverter.GetBytes(c16);
            pkt[7] = c16B[0]; pkt[8] = c16B[1];
            byte[] finalWithoutCrc32 = pkt.ToArray();
            uint c32 = ProtocolUtils.Crc32(finalWithoutCrc32, finalWithoutCrc32.Length);
            pkt.AddRange(BitConverter.GetBytes(c32));
            _udpClient.SendCommand(pkt.ToArray(), _currentDeviceIp, TARGET_LIDAR_PORT);
        }
        private void HandleBroadcast(byte[] data, IPEndPoint remote)
        {
            if (data.Length < 10) return;
            if (data[9] == 0 && data[10] == 0)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    bool exists = false; foreach (ListViewItem item in listView_Devices.Items)
                    {
                        if (item.SubItems[1].Text == remote.Address.ToString()) { exists = true; break; }
                    }
                    if (!exists)
                    {
                        var item = new ListViewItem(DateTime.Now.ToLongTimeString());
                        item.SubItems.Add(remote.Address.ToString()); item.SubItems.Add("Livox Lidar");
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
                    AddLog($"握手成功! 雷达已连接 (Target: {remote.Address})");
                    if (listView_Devices.SelectedItems.Count > 0) listView_Devices.SelectedItems[0].SubItems[3].Text = "已连接"; SyncRadarTime(); AddLog("提示: 请点击 '开始采样' 获取点云"); if (!_heartbeatTimer.Enabled)
                    {
                        _heartbeatTimer.Start(); AddLog(">>> 心跳机制已启动");
                    }
                }
                else if (cmdSet == 0 && cmdId == 4)
                {
                    byte retCode = (data.Length > 11) ? data[11] : (byte)255;
                    if (retCode == 0) AddLog("收到 [开始采样] 成功确认。等待 UDP 数据流...");
                    else AddLog($"[开始采样] 失败! RetCode: {retCode}");
                }
                else if (cmdSet == 0 && cmdId == 3)
                {
                    if (data.Length >= 12)
                    {
                        byte state = (data.Length > 12) ? data[12] : (byte)0;
                        string stateStr = "未知"; switch (state)
                        {
                            case 0:
                                stateStr = "初始化";
                                break;
                            case 1: stateStr = "正常"; break;
                            case 2: stateStr = "省电"; break;
                            case 3: stateStr = "待机"; break;
                            case 4: stateStr = "错误"; break;
                        }
                        AddLog($"收到心跳 ACK | 状态: {stateStr}");
                    }
                }
                else if (cmdSet == 0 && cmdId == 6) { AddLog($"收到断开连接确认。雷达已断开。"); if (listView_Devices.SelectedItems.Count > 0) listView_Devices.SelectedItems[0].SubItems[3].Text = "未连接"; StopAllWork(); btn_StartSample.Enabled = true; btn_StopSample.Enabled = true; } else if (cmdSet == 0 && cmdId == 10) { byte retCode = (data.Length > 11) ? data[11] : (byte)255; if (retCode == 0) AddLog(">>> 时间同步成功！"); else AddLog($">>> 时间同步失败，RetCode: {retCode}"); } else if (cmdSet == (byte)CmdSet.Lidar && cmdId == (byte)LidarCmdId.SetMode) { byte retCode = (data.Length > 11) ? data[11] : (byte)255; if (retCode == 0) AddLog(">>> 工作模式设置成功！"); else AddLog($">>> 工作模式设置失败，错误码: {retCode}"); } else if (cmdSet == (byte)CmdSet.General && cmdId == (byte)GeneralCmdId.CoordinateSystem) { byte retCode = (data.Length > 11) ? data[11] : (byte)255; if (retCode == 0) AddLog(">>> 坐标系切换成功！下一次采样生效。"); else if (retCode == 3) AddLog(">>> 设备不支持切换坐标系指令 (RetCode: NotSupported)。"); else AddLog($">>> 坐标系切换失败，错误码: {retCode}"); }
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
            req.microsecond = (uint)(ticksInHour / 10); byte[] payload = ProtocolUtils.StructToBytes(req);
            SendControlCommand(CmdSet.Lidar, (byte)LidarCmdId.UpdateUtcTime, payload);
            AddLog($">>> [同步发送] 标准UTC: {nowUtc.ToString("HH: mm:ss")}.{req.microsecond}");
        }
        private void AddLog(string msg)
        {
            if (listBox_Log.InvokeRequired) listBox_Log.BeginInvoke((MethodInvoker)
                delegate { 
                    AddLog(msg);
                });
            else
            {
                listBox_Log.Items.Add($"[{DateTime.Now.ToString("HH: mm:ss")}] {msg}");
                listBox_Log.TopIndex = listBox_Log.Items.Count - 1;
            }
        }
        private void btn_SetMode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentDeviceIp))
            {
                MessageBox.Show("请先选择并连接设备！");
                return;
            }
            byte modeVal = (byte)(cbx_WorkMode.SelectedIndex + 1);
            if (!Enum.IsDefined(typeof(LidarMode), modeVal))
                modeVal = (byte)LidarMode.Normal;
            AddLog($"正在设置工作模式为: {((LidarMode)modeVal)}...");
            SendControlCommand(CmdSet.Lidar, (byte)LidarCmdId.SetMode, new byte[] { modeVal });
        }
        private void btn_SetCoordinate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentDeviceIp)) {
                MessageBox.Show("请先选择并连接设备！"); 
                return; 
                    }
            byte coordVal = (byte)cbx_Coordinate.SelectedIndex;
            string coordName = (coordVal == 0) ? "直角坐标 (Cartesian)" : "球坐标 (Spherical)"; AddLog($"正在请求切换坐标系为: {coordName}..."); SendControlCommand(CmdSet.General, (byte)GeneralCmdId.CoordinateSystem, new byte[] { coordVal });
        }

        private void ProcessPointCloud(byte[] data)
        {
            int protocolHeaderSize = 11; int livoxEthHeaderSize = 18;
            if (data.Length < protocolHeaderSize + livoxEthHeaderSize && data.Length < 100)
                return;
            int ptr = protocolHeaderSize;
            byte timestamp_type = data[ptr + 8];
            byte data_type = data[ptr + 9];
            ulong radarTimestamp = BitConverter.ToUInt64(data, ptr + 10);
            DateTime frameTime;
            if (timestamp_type == 1 || timestamp_type == 3)
            {
                long ticks = (long)(radarTimestamp / 100);
                frameTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddTicks(ticks);
                _isTimeSynced = true;
            }
            else
            {
                if (_isTimeSynced && _lastRadarTimestamp != 0)
                {
                    long diffAbs = Math.Abs((long)((long)radarTimestamp - (long)_lastRadarTimestamp));
                    if (diffAbs > 1000000000) _isTimeSynced = false;
                }
                _lastRadarTimestamp = radarTimestamp;
                if (!_isTimeSynced)
                {
                    _basePcTicks = DateTime.UtcNow.Ticks;
                    _baseRadarTime = radarTimestamp;
                    _isTimeSynced = true;
                }
                long diffNs = unchecked((long)(radarTimestamp - _baseRadarTime));
                long diffTicks = diffNs / 100;
                frameTime = new DateTime(_basePcTicks + diffTicks, DateTimeKind.Utc);
            }

            int dataOffset = protocolHeaderSize + livoxEthHeaderSize; int pSize = 0;
            switch (data_type)
            {
                case 0: pSize = 13; break;
                case 1: pSize = 9; break;
                case 2: pSize = 14; break;
                case 3: pSize = 10; break;
                default: return;
            }
            int pointsDataLen = data.Length - dataOffset; if (pSize == 0) return; int count = pointsDataLen / pSize; double pointIntervalTicks = 100.0;

            List<PointData> rawBatch = new List<PointData>();
            bool isViewerOpen = (_cloudViewer != null && !_cloudViewer.IsDisposed);

            for (int i = 0; i < count; i++)
            {
                int currentOffset = dataOffset + (i * pSize);
                if (currentOffset + pSize > data.Length) break;
                DateTime exactPointTime = frameTime.AddTicks((long)(i * pointIntervalTicks));
                int x = 0, y = 0, z = 0; byte reflectivity = 0; byte tag = 0;
                if (data_type == 0 || data_type == 2)
                {
                    x = BitConverter.ToInt32(data, currentOffset);
                    y = BitConverter.ToInt32(data, currentOffset + 4);
                    z = BitConverter.ToInt32(data, currentOffset + 8);
                    reflectivity = data[currentOffset + 12];
                    if (data_type == 2)
                        tag = data[currentOffset + 13];
                }
                else if (data_type == 1 || data_type == 3)
                {
                    uint depth = BitConverter.ToUInt32(data, currentOffset);
                    ushort theta = BitConverter.ToUInt16(data, currentOffset + 4);
                    ushort phi = BitConverter.ToUInt16(data, currentOffset + 6);
                    reflectivity = data[currentOffset + 8]; if (data_type == 3) tag = data[currentOffset + 9];
                    if (depth > 0)
                    {
                        double d = depth; double t = theta * 0.01 * Math.PI / 180.0;
                        double p = phi * 0.01 * Math.PI / 180.0;
                        x = (int)(d * Math.Sin(t) * Math.Cos(p));
                        y = (int)(d * Math.Sin(t) * Math.Sin(p));
                        z = (int)(d * Math.Cos(t));
                    }
                }

                if (x == 0 && y == 0 && z == 0) continue;
                _dbManager.EnqueuePoint(exactPointTime, x, y, z, reflectivity, tag);

                if (isViewerOpen)
                {
                    rawBatch.Add(new PointData
                    {
                        ExactTime = exactPointTime,
                        X = x / 1000.0f,
                        Y = y / 1000.0f,
                        Z = z / 1000.0f,
                        Reflectivity = reflectivity,
                        Tag = tag
                    });
                }
            }

            //
            // 将整个 List 放入队列，而不是逐个放入点
            if (rawBatch.Count > 0 && isViewerOpen)
            {
                List<PointData> filteredBatch = _processor.ApplyFilters(rawBatch);
                // 只有非空才放
                if (filteredBatch.Count > 0)
                {
                    _uiDataQueue.Enqueue(filteredBatch);
                }
            }
        }
    }
}