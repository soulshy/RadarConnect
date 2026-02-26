using System.Runtime.InteropServices;

namespace RadarConnect
{
    // ==========================================
    // 协议模型定义 
    // ==========================================

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HandshakeRequest
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] user_ip; // 主机IP (4字节)
        public ushort data_port;
        public ushort cmd_port;
        public ushort imu_port;
    }

    //时间同步请求结构体
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LidarSetUtcSyncTimeRequest
    {
        public byte year;         // 年份 (20xx, 比如 25 表示 2025)
        public byte month;        // 月
        public byte day;          // 日
        public byte hour;         // 时
        public uint microsecond;  // 当前小时内的微秒数 (0 ~ 3,600,000,000)
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SampleFrameHeader
    {
        public byte version;
        public byte slot_id;
        public byte lidar_id;
        public byte reserved;
        public uint status_code;
        public byte timestamp_type;
        public byte data_type; // 0:直角, 1:球, 2:扩展直角, 3:扩展球
        public ulong timestamp;
    }

    // Type 0: 标准直角坐标 (13字节)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxRawPoint
    {
        public int x; public int y; public int z;
        public byte reflectivity;
    }

    // Type 1: 标准球坐标 (9字节)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxSpherPoint
    {
        public uint depth;
        public ushort theta;
        public ushort phi;
        public byte reflectivity;
    }

    // Type 2: 扩展直角坐标 (14字节)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxExtendRawPoint
    {
        public int x; public int y; public int z;
        public byte reflectivity;
        public byte tag; // 扩展信息
    }

    // Type 3: 扩展球坐标 (10字节)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxExtendSpherPoint
    {
        public uint depth;
        public ushort theta;
        public ushort phi;
        public byte reflectivity;
        public byte tag; // 扩展信息
    }
}