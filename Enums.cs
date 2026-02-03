namespace RadarConnect
{
    public enum DeviceType : byte
    {
        Hub = 0,
        Mid40 = 1,
        Tele15 = 2,
        Horizon = 3,
        Mid70 = 6,
        Avia = 7
    }

    public enum CmdType : byte
    {
        CMD = 0x00, // 请求
        ACK = 0x01, // 应答
        MSG = 0x02  // 消息
    }

    public enum CmdSet : byte
    {
        General = 0x00, // 通用指令
        Lidar = 0x01,   // 雷达指令
        Hub = 0x02      // Hub指令
    }

    public enum GeneralCmdId : byte
    {
        Broadcast = 0x00,
        Handshake = 0x01, // 握手
        DeviceInfo = 0x02,
        Heartbeat = 0x03,
        StartSample = 0x04,
        StopSample = 0x04,
        CoordinateSystem = 0x05,
        Disconnect = 0x06
    }

    /// <summary>
    /// Lidar Command Set (0x01) 的 ID
    /// </summary>
    public enum LidarCmdId : byte
    {
        SetMode = 0x00,
        WriteExtrinsic = 0x01,
        ReadExtrinsic = 0x02,
        RainFogSuppression = 0x03,
        FanControl = 0x04,
        GetFanState = 0x05,
        SetReturnMode = 0x06,
        GetReturnMode = 0x07,
        SetImuFreq = 0x08,
        GetImuFreq = 0x09,
        UpdateUtcTime = 0x0A // 【关键】时间同步指令 ID
    }

    public enum LidarMode : byte
    {
        Normal = 0x01,
        PowerSaving = 0x02,
        Standby = 0x03
    }

    public enum CoordinateType : byte
    {
        Cartesian = 0x00, // 笛卡尔
        Spherical = 0x01  // 球坐标
    }
}