using System;
using System.Collections.Generic;
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

        // 雷达的目标端口 (协议规定是 65000)
        private const int TARGET_LIDAR_PORT = 65000;

        // 本机开放的端口
        private const int LOCAL_CMD_PORT = 50001;  // 发令/收ACK
        private const int LOCAL_DATA_PORT = 60000; // 收点云

        // ==========================================
        // 成员变量
        // ==========================================
        private UdpCommunication _udpClient;
        private DatabaseManager _dbManager;
        private string _currentDeviceIp;
        private bool _isSaving = false;

        // 定义一个类成员变量，记录上一次的雷达时间，用于检测跳变
        private ulong _lastRadarTimestamp = 0;

        // 定时器与计数器
        private System.Windows.Forms.Timer _heartbeatTimer;
        private int _uiUpdateCounter = 0;
        private int _logDisplayCounter = 0;
        private int _logFrequencyCounter = 0; // 用于控制点云日志频率

        // --- 时间同步相关变量 ---
        private bool _isTimeSynced = false;
        private long _basePcTicks = 0;       // 基准电脑时间 (Ticks)
        private ulong _baseRadarTime = 0;    // 基准雷达时间戳 (ns)

        // ==========================================
        // 初始化
        // ==========================================
        public Form1()
        {
            InitializeComponent();
            InitGui();

            _udpClient = new UdpCommunication();
            _dbManager = new DatabaseManager();

            // 初始化心跳定时器
            _heartbeatTimer = new System.Windows.Forms.Timer();
            _heartbeatTimer.Interval = 1000; // 1秒一次
            _heartbeatTimer.Tick += OnHeartbeatTimerTick;

            // 绑定网络事件
            _udpClient.OnBroadcastReceived += HandleBroadcast; // 处理广播
            _udpClient.OnCmdAckReceived += HandleAck;         // 处理握手/命令回复
            _udpClient.OnDataReceived += ProcessPointCloud;    // 处理点云
            _udpClient.OnError += (msg) => this.BeginInvoke((MethodInvoker)delegate { AddLog($"错误: {msg}"); });
        }

        private void InitGui()
        {
            cbx_WorkMode.Items.AddRange(new object[] { "正常模式", "省电模式","待机模式" });
            cbx_WorkMode.SelectedIndex = 0;
            cbx_Coordinate.Items.AddRange(new object[] { "直角坐标", "球坐标" });
            cbx_Coordinate.SelectedIndex = 0;
        }

        // ==========================================
        // 按钮事件处理
        // ==========================================

        // 1. 启动监听
        private void btn_StartListen_Click(object sender, EventArgs e)
        {
            // 启动三路通道
            _udpClient.Start(FIXED_LOCAL_IP, LOCAL_CMD_PORT, LOCAL_DATA_PORT);

            btn_StartListen.Enabled = false;
            btn_StopListen.Enabled = true;
            AddLog($"服务启动 | 命令端口:{LOCAL_CMD_PORT} 数据端口:{LOCAL_DATA_PORT}");
        }

        // 2. 停止监听
        private void btn_StopListen_Click(object sender, EventArgs e)
        {
            StopAllWork(); // 封装停止逻辑

            _udpClient.Stop();
            btn_StartListen.Enabled = true;
            btn_StopListen.Enabled = false;
            AddLog("服务停止");
        }

        // 3. 发送握手
        private void btn_HandShake_Click(object sender, EventArgs e)
        {
            if (listView_Devices.SelectedItems.Count == 0) return;
            _currentDeviceIp = listView_Devices.SelectedItems[0].SubItems[1].Text;

            HandshakeRequest req = new HandshakeRequest();
            string localIpStr = string.Empty;

            // 自动寻找本机 IP
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

            if (string.IsNullOrEmpty(localIpStr))
            {
                localIpStr = FIXED_LOCAL_IP;
                AddLog($"警告: 未找到自动 IP，使用默认 {localIpStr}");
            }
            else
            {
                AddLog($"自动检测本机 IP: {localIpStr}");
            }

            req.user_ip = IPAddress.Parse(localIpStr).GetAddressBytes();
            req.data_port = LOCAL_DATA_PORT;
            req.cmd_port = LOCAL_CMD_PORT;
            req.imu_port = LOCAL_DATA_PORT;

            // 发送握手指令 (CmdSet:0, CmdId:1)
            SendControlCommand(CmdSet.General, (byte)GeneralCmdId.Handshake, ProtocolUtils.StructToBytes(req), 0x01);
            AddLog($"发送握手 -> {_currentDeviceIp} (Port:{TARGET_LIDAR_PORT})");
        }

        // 4. 开始采样
        private void btn_StartSample_Click_1(object sender, EventArgs e)
        {
            // 发送开始采样指令 (CmdSet:0, CmdId:4, Payload:0x01)
            SendControlCommand(CmdSet.General, 0x04, new byte[] { 0x01 });

            // 开启软件接收/存储开关
            _isSaving = true;
            _dbManager.StartSaving(); // 启动数据库线程

            AddLog("已发送 [开始采样] 指令，并启动入库。");
            btn_StartSample.Enabled = false;
            btn_StopSample.Enabled = true;
        }

        // 5. 停止采样
        private void btn_StopSample_Click_1(object sender, EventArgs e)
        {
            // 发送停止采样指令 (CmdSet:0, CmdId:4, Payload:0x00)
            SendControlCommand(CmdSet.General, 0x04, new byte[] { 0x00 });

            _isSaving = false;
            _dbManager.StopSaving();
            _isTimeSynced = false; // 重置时间同步

            AddLog("已发送 [停止采样] 指令，停止入库。");
            btn_StartSample.Enabled = true;
            btn_StopSample.Enabled = false;
        }

        // 6. 断开连接
        private void btn_Disconnect_Click_1(object sender, EventArgs e)
        {
            // 发送断开指令 (CmdSet:0, CmdId:6)
            SendControlCommand(CmdSet.General, 0x06, null);

            StopAllWork();
            AddLog(">>> 已发送断开请求");
        }

        // 辅助方法：停止所有工作（心跳、存储、同步）
        private void StopAllWork()
        {
            _heartbeatTimer.Stop();
            _isSaving = false;
            _dbManager.StopSaving();
            _isTimeSynced = false;
        }

        // ==========================================
        // 核心逻辑：心跳与发送
        // ==========================================

        private void OnHeartbeatTimerTick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentDeviceIp) || !_udpClient.IsConnected)
                return;

            // 发送心跳指令 (CmdSet:0, CmdId:3)
            SendControlCommand(CmdSet.General, (byte)GeneralCmdId.Heartbeat, null);
            // AddLog("发送心跳..."); // 日志可选，避免刷屏
        }

        private void SendControlCommand(CmdSet set, byte cmdId, byte[] payload, byte packetType = 0)
        {
            if (string.IsNullOrEmpty(_currentDeviceIp)) return;

            // 组包逻辑 (Header + Payload + CRC)
            List<byte> pkt = new List<byte>();
            pkt.Add(0xAA);             // SOF
            pkt.Add(0x01);             // Version
            pkt.Add(0); pkt.Add(0);    // Length (占位)
            pkt.Add(packetType);       // Packet Type
            pkt.AddRange(BitConverter.GetBytes(ProtocolUtils.GetSeqNum())); // Seq
            pkt.Add(0); pkt.Add(0);    // Preamble CRC (占位)
            pkt.Add((byte)set);        // CmdSet
            pkt.Add(cmdId);            // CmdID
            if (payload != null) pkt.AddRange(payload);

            // 填回长度
            ushort len = (ushort)(pkt.Count + 4);
            byte[] lenB = BitConverter.GetBytes(len);
            pkt[2] = lenB[0]; pkt[3] = lenB[1];

            // 计算 CRC16
            byte[] header = pkt.GetRange(0, 7).ToArray();
            ushort c16 = ProtocolUtils.Crc16(header, 7);
            byte[] c16B = BitConverter.GetBytes(c16);
            pkt[7] = c16B[0]; pkt[8] = c16B[1];

            // 计算 CRC32
            byte[] finalWithoutCrc32 = pkt.ToArray();
            uint c32 = ProtocolUtils.Crc32(finalWithoutCrc32, finalWithoutCrc32.Length);
            pkt.AddRange(BitConverter.GetBytes(c32));

            _udpClient.SendCommand(pkt.ToArray(), _currentDeviceIp, TARGET_LIDAR_PORT);
        }

        // ==========================================
        // 接收处理：广播、ACK、点云
        // ==========================================

        private void HandleBroadcast(byte[] data, IPEndPoint remote)
        {
            // 简单解析广播包，发现新雷达
            if (data.Length < 10) return;
            // 广播包通常是 CmdSet=0, CmdID=0
            if (data[9] == 0 && data[10] == 0)
            {
                this.BeginInvoke((MethodInvoker)delegate {
                    bool exists = false;
                    foreach (ListViewItem item in listView_Devices.Items)
                    {
                        if (item.SubItems[1].Text == remote.Address.ToString()) { exists = true; break; }
                    }
                    if (!exists)
                    {
                        var item = new ListViewItem(DateTime.Now.ToLongTimeString());
                        item.SubItems.Add(remote.Address.ToString());
                        item.SubItems.Add("Livox Lidar");
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

            this.BeginInvoke((MethodInvoker)delegate {

                // 1. 握手成功 (CmdSet:0, ID:1)
                if (cmdType == 1 && cmdSet == 0 && cmdId == 1)
                {
                    AddLog($"握手成功! 雷达已连接 (Target: {remote.Address})");
                    if (listView_Devices.SelectedItems.Count > 0)
                        listView_Devices.SelectedItems[0].SubItems[3].Text = "已连接";
                    SyncRadarTime();
                    AddLog("提示: 请点击 '开始采样' 获取点云");


                    // 启动心跳
                    if (!_heartbeatTimer.Enabled)
                    {
                        _heartbeatTimer.Start();
                        AddLog(">>> 心跳机制已启动");
                    }
                }

                // 2. 开始采样回复 (CmdSet:0, ID:4)
                else if (cmdSet == 0 && cmdId == 4)
                {
                    byte retCode = (data.Length > 11) ? data[11] : (byte)255;
                    if (retCode == 0) AddLog("收到 [开始采样] 成功确认。等待 UDP 数据流...");
                    else AddLog($"[开始采样] 失败! RetCode: {retCode}");
                }

                // 3. 心跳回复 (CmdSet:0, ID:3)
                else if (cmdSet == 0 && cmdId == 3)
                {
                    // 心跳包通常22字节
                    if (data.Length >= 12)
                    {
                        byte state = (data.Length > 12) ? data[12] : (byte)0;
                        string stateStr = "未知";
                        switch (state)
                        {
                            case 0: stateStr = "初始化"; break;
                            case 1: stateStr = "正常"; break;
                            case 2: stateStr = "省电"; break;
                            case 3: stateStr = "待机"; break;
                            case 4: stateStr = "错误"; break;
                        }
                        // 直接打印，不限制频率 (1秒1次)
                        AddLog($"收到心跳 ACK | 状态: {stateStr}");
                    }
                }

                // 4. 断开连接回复 (CmdSet:0, ID:6)
                else if (cmdSet == 0 && cmdId == 6)
                {
                    AddLog($"收到断开连接确认。雷达已断开。");
                    if (listView_Devices.SelectedItems.Count > 0)
                        listView_Devices.SelectedItems[0].SubItems[3].Text = "未连接";

                    StopAllWork();
                    btn_StartSample.Enabled = true;
                    btn_StopSample.Enabled = true;
                }
                else if (cmdSet == 0 && cmdId == 10)
                {
                    byte retCode = (data.Length > 11) ? data[11] : (byte)255;
                    if (retCode == 0) AddLog(">>> 时间同步成功！");
                    else AddLog($">>> 时间同步失败，RetCode: {retCode}");
                }
                //处理设置工作模式的回复 (CmdSet:1, CmdId:0)
                else if (cmdSet == (byte)CmdSet.Lidar && cmdId == (byte)LidarCmdId.SetMode)
                {
                    byte retCode = (data.Length > 11) ? data[11] : (byte)255;
                    if (retCode == 0) AddLog(">>> 工作模式设置成功！");
                    else AddLog($">>> 工作模式设置失败，错误码: {retCode}");
                }

                //处理设置坐标系的回复 (CmdSet:0, CmdId:5)
                else if (cmdSet == (byte)CmdSet.General && cmdId == (byte)GeneralCmdId.CoordinateSystem)
                {
                    byte retCode = (data.Length > 11) ? data[11] : (byte)255;
                    if (retCode == 0) AddLog(">>> 坐标系切换成功！下一次采样生效。");
                    else if (retCode == 3) AddLog(">>> 设备不支持切换坐标系指令 (RetCode: NotSupported)。");
                    else AddLog($">>> 坐标系切换失败，错误码: {retCode}");
                }
            });
        }


        private void ProcessPointCloud(byte[] data)
        {
            // 1. 长度校验
            int protocolHeaderSize = 11;
            int livoxEthHeaderSize = 18;
            if (data.Length < protocolHeaderSize + livoxEthHeaderSize&& data.Length< 100) return;

            // 2. 解析头部
            int ptr = protocolHeaderSize;
            byte timestamp_type = data[ptr + 8];
            byte data_type = data[ptr + 9];
            ulong radarTimestamp = BitConverter.ToUInt64(data, ptr + 10);

            DateTime frameTime;

            // Type 1/3: 硬件授时 (GPS/PTP)
            if (timestamp_type == 1 || timestamp_type == 3)
            {
                long ticks = (long)(radarTimestamp / 100);
                frameTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddTicks(ticks);
                _isTimeSynced = true;
            }
            // Type 0: 内部软同步 
            else
            {

                if (_isTimeSynced && _lastRadarTimestamp != 0)
                {
                    long diffAbs = Math.Abs((long)((long)radarTimestamp - (long)_lastRadarTimestamp));
                    // 阈值：1秒 (10^9 纳秒)
                    if (diffAbs > 1000000000)
                    {
                        _isTimeSynced = false;
                    }
                }
                _lastRadarTimestamp = radarTimestamp;

                // 执行同步
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

            // ==========================================
            // 【点云解析】
            // ==========================================
            int dataOffset = protocolHeaderSize + livoxEthHeaderSize;
            int pSize = 0;

            switch (data_type)
            {
                case 0: pSize = 13; break;
                case 1: pSize = 9; break;
                case 2: pSize = 14; break;
                case 3: pSize = 10; break;
                default: return;
            }

            int pointsDataLen = data.Length - dataOffset;
            if (pSize == 0) return;
            int count = pointsDataLen / pSize;
            double pointIntervalTicks = 100.0;

            for (int i = 0; i < count; i++)
            {
                int currentOffset = dataOffset + (i * pSize);
                if (currentOffset + pSize > data.Length) break;

                DateTime exactPointTime = frameTime.AddTicks((long)(i * pointIntervalTicks));

                int x = 0, y = 0, z = 0;
                byte reflectivity = 0;
                byte tag = 0;

                if (data_type == 0 || data_type == 2)
                {
                    x = BitConverter.ToInt32(data, currentOffset);
                    y = BitConverter.ToInt32(data, currentOffset + 4);
                    z = BitConverter.ToInt32(data, currentOffset + 8);
                    reflectivity = data[currentOffset + 12];
                    if (data_type == 2) tag = data[currentOffset + 13];
                }
                else if (data_type == 1 || data_type == 3)
                {
                    uint depth = BitConverter.ToUInt32(data, currentOffset);
                    ushort theta = BitConverter.ToUInt16(data, currentOffset + 4);
                    ushort phi = BitConverter.ToUInt16(data, currentOffset + 6);
                    reflectivity = data[currentOffset + 8];
                    if (data_type == 3) tag = data[currentOffset + 9];

                    if (depth > 0)
                    {
                        double d = depth;
                        double t = theta * 0.01 * Math.PI / 180.0;
                        double p = phi * 0.01 * Math.PI / 180.0;
                        x = (int)(d * Math.Sin(t) * Math.Cos(p));
                        y = (int)(d * Math.Sin(t) * Math.Sin(p));
                        z = (int)(d * Math.Cos(t));
                    }
                }

                if (x == 0 && y == 0 && z == 0) continue;
                _dbManager.EnqueuePoint(exactPointTime, x, y, z, reflectivity, tag);
            }
        }
        // 发送时间同步指令
        private void SyncRadarTime()
        {
            if (string.IsNullOrEmpty(_currentDeviceIp)) return;

            // 直接取标准 UTC 时间
            DateTime nowUtc = DateTime.UtcNow;

            LidarSetUtcSyncTimeRequest req = new LidarSetUtcSyncTimeRequest();

            // 协议要求：Year 0 = 2000年
            req.year = (byte)(nowUtc.Year - 2000);
            req.month = (byte)nowUtc.Month;
            req.day = (byte)nowUtc.Day;
            req.hour = (byte)nowUtc.Hour;

            // 协议要求：微秒数 (0 ~ 3,600,000,000)
            // 1秒 = 10,000,000 Ticks -> 1 Tick = 0.1 us
            // 取当前小时内的 Ticks 余数，再除以 10 得到微秒
            long ticksInHour = nowUtc.Ticks % 36000000000;
            req.microsecond = (uint)(ticksInHour / 10);

            byte[] payload = ProtocolUtils.StructToBytes(req);

            // 发送指令 (CmdSet: 1 [Lidar], CmdId: 0x0A [10])
            SendControlCommand(CmdSet.Lidar, (byte)LidarCmdId.UpdateUtcTime, payload);

            AddLog($">>> [同步发送] 标准UTC: {nowUtc:HH:mm:ss}.{req.microsecond}");
        }

        private void AddLog(string msg)
        {
            if (listBox_Log.InvokeRequired) listBox_Log.BeginInvoke((MethodInvoker)delegate { AddLog(msg); });
            else
            {
                listBox_Log.Items.Add($"[{DateTime.Now:HH:mm:ss}] {msg}");
                // 自动滚动到底部
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

            // 获取下拉框选择的模式 (假设下拉框顺序：0-正常, 1-省电, 2-待机)
            // 对应 LidarMode 枚举: Normal=1, PowerSaving=2, Standby=3
            byte modeVal = (byte)(cbx_WorkMode.SelectedIndex + 1);

            // 简单的校验，防止越界
            if (!Enum.IsDefined(typeof(LidarMode), modeVal))
            {
                modeVal = (byte)LidarMode.Normal;
            }

            AddLog($"正在设置工作模式为: {((LidarMode)modeVal)}...");

            // 发送指令: CmdSet=1 (Lidar), CmdId=0 (SetMode), Payload=[mode]
            SendControlCommand(CmdSet.Lidar, (byte)LidarCmdId.SetMode, new byte[] { modeVal });
        }

        private void btn_SetCoordinate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentDeviceIp))
            {
                MessageBox.Show("请先选择并连接设备！");
                return;
            }

            // 获取下拉框选择 (0: 直角坐标, 1: 球坐标)
            byte coordVal = (byte)cbx_Coordinate.SelectedIndex;

            string coordName = (coordVal == 0) ? "直角坐标 (Cartesian)" : "球坐标 (Spherical)";
            AddLog($"正在请求切换坐标系为: {coordName}...");

            SendControlCommand(CmdSet.General, (byte)GeneralCmdId.CoordinateSystem, new byte[] { coordVal });
        }
    }
}