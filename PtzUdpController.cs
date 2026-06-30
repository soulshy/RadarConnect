using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RadarConnect
{
    public sealed class PtzUdpController : IDisposable
    {
        private UdpClient _client;
        private Thread _receiveThread;
        private volatile bool _running;

        public event Action<PtzPacket, IPEndPoint> PacketReceived;
        public event Action<string> ErrorReceived;

        public bool IsOpen => _running && _client != null;

        public void Open(string localIp, int localPort)
        {
            Close();

            IPAddress bindIp = IPAddress.Any;
            if (!string.IsNullOrWhiteSpace(localIp) && localIp.Trim() != "0.0.0.0")
            {
                bindIp = IPAddress.Parse(localIp.Trim());
            }

            _client = new UdpClient();
            _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _client.Client.Bind(new IPEndPoint(bindIp, localPort));

            _running = true;
            _receiveThread = new Thread(ReceiveLoop) { IsBackground = true, Name = "PTZ UDP Receiver" };
            _receiveThread.Start();
        }

        public void Close()
        {
            _running = false;

            try { _client?.Close(); } catch { }
            _client = null;
        }

        public void Send(byte[] data, string targetIp, int targetPort)
        {
            if (_client == null)
                throw new InvalidOperationException("云台 UDP 尚未打开。");

            if (data == null || data.Length == 0)
                throw new ArgumentException("发送数据不能为空。", nameof(data));

            IPEndPoint remote = new IPEndPoint(IPAddress.Parse(targetIp), targetPort);
            _client.Send(data, data.Length, remote);
        }

        private void ReceiveLoop()
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            while (_running)
            {
                try
                {
                    UdpClient client = _client;
                    if (client == null) break;

                    byte[] data = client.Receive(ref remote);
                    if (data == null || data.Length == 0) continue;

                    if (data.Length % PtzProtocol.PacketLength == 0)
                    {
                        for (int i = 0; i < data.Length; i += PtzProtocol.PacketLength)
                        {
                            PtzPacket packet;
                            if (PtzPacket.TryParse(data, i, out packet))
                                PacketReceived?.Invoke(packet, remote);
                            else
                                ErrorReceived?.Invoke("收到无效云台回包: " + PtzProtocol.ToHex(data));
                        }
                    }
                    else
                    {
                        PtzPacket packet;
                        if (PtzPacket.TryParse(data, 0, out packet))
                            PacketReceived?.Invoke(packet, remote);
                        else
                            ErrorReceived?.Invoke("收到非 7 字节云台回包: " + PtzProtocol.ToHex(data));
                    }
                }
                catch (SocketException ex)
                {
                    if (_running)
                        ErrorReceived?.Invoke("云台 UDP 接收异常: " + ex.Message);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    if (_running)
                        ErrorReceived?.Invoke("云台 UDP 接收异常: " + ex.Message);
                }
            }
        }

        public void Dispose()
        {
            Close();
        }
    }

    public sealed class PtzPacket
    {
        public byte Address { get; private set; }
        public byte Data1 { get; private set; }
        public byte Data2 { get; private set; }
        public byte Data3 { get; private set; }
        public byte Data4 { get; private set; }
        public byte Checksum { get; private set; }

        public bool IsChecksumValid
        {
            get { return Checksum == PtzProtocol.Checksum(Address, Data1, Data2, Data3, Data4); }
        }

        public byte[] ToArray()
        {
            return new[] { PtzProtocol.Header, Address, Data1, Data2, Data3, Data4, Checksum };
        }

        public string ToHex()
        {
            return PtzProtocol.ToHex(ToArray());
        }

        public static bool TryParse(byte[] data, int offset, out PtzPacket packet)
        {
            packet = null;
            if (data == null || offset < 0 || data.Length - offset < PtzProtocol.PacketLength)
                return false;

            if (data[offset] != PtzProtocol.Header)
                return false;

            packet = new PtzPacket
            {
                Address = data[offset + 1],
                Data1 = data[offset + 2],
                Data2 = data[offset + 3],
                Data3 = data[offset + 4],
                Data4 = data[offset + 5],
                Checksum = data[offset + 6]
            };

            return packet.IsChecksumValid;
        }
    }

    public static class PtzProtocol
    {
        public const int PacketLength = 7;
        public const byte Header = 0xff;

        public static byte[] Build(byte address, byte data1, byte data2, byte data3, byte data4)
        {
            return new[]
            {
                Header,
                address,
                data1,
                data2,
                data3,
                data4,
                Checksum(address, data1, data2, data3, data4)
            };
        }

        public static byte Checksum(byte address, byte data1, byte data2, byte data3, byte data4)
        {
            return (byte)((address + data1 + data2 + data3 + data4) & 0xff);
        }

        public static string ToHex(byte[] data)
        {
            if (data == null) return string.Empty;

            StringBuilder builder = new StringBuilder(data.Length * 3);
            for (int i = 0; i < data.Length; i++)
            {
                if (i > 0) builder.Append(' ');
                builder.Append(data[i].ToString("X2", CultureInfo.InvariantCulture));
            }
            return builder.ToString();
        }

        public static byte[] ParseHex(string hexText)
        {
            if (string.IsNullOrWhiteSpace(hexText))
                throw new ArgumentException("请输入十六进制指令。");

            string normalized = hexText
                .Replace("0x", string.Empty)
                .Replace("0X", string.Empty)
                .Replace(",", " ")
                .Replace("-", " ")
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Trim();

            string[] parts = normalized.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] data;
            if (parts.Length == 1 && parts[0].Length > 2)
            {
                string compact = parts[0];
                if (compact.Length % 2 != 0)
                    throw new FormatException("紧凑十六进制字符串长度必须为偶数。");

                data = new byte[compact.Length / 2];
                for (int i = 0; i < data.Length; i++)
                    data[i] = byte.Parse(compact.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            else
            {
                data = new byte[parts.Length];
                for (int i = 0; i < parts.Length; i++)
                    data[i] = byte.Parse(parts[i], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }

        public static byte EncodeSpeed10(decimal speedRpm)
        {
            int value = (int)Math.Round(speedRpm * 10m, MidpointRounding.AwayFromZero);
            if (value < 0) value = 0;
            if (value > 255) value = 255;
            return (byte)value;
        }

        public static ushort EncodeAngle100(decimal angle)
        {
            int value = (int)Math.Round(angle * 100m, MidpointRounding.AwayFromZero);
            if (value < short.MinValue || value > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(angle), "角度放大 100 倍后必须在 -32768 到 65535 范围内。");

            if (value < 0)
                return unchecked((ushort)(short)value);

            return (ushort)value;
        }

        public static ushort EncodeUInt16(int value, string name)
        {
            if (value < 0 || value > 65535)
                throw new ArgumentOutOfRangeException(name, "数值必须在 0 到 65535 范围内。");

            return (ushort)value;
        }

        public static byte HighByte(ushort value)
        {
            return (byte)((value >> 8) & 0xff);
        }

        public static byte LowByte(ushort value)
        {
            return (byte)(value & 0xff);
        }

        public static decimal DecodeUnsigned100(byte high, byte low)
        {
            ushort raw = (ushort)((high << 8) | low);
            return raw / 100m;
        }

        public static decimal DecodeSigned100(byte high, byte low)
        {
            short raw = unchecked((short)((high << 8) | low));
            return raw / 100m;
        }

        public static decimal DecodeUnsignedSpeed100(byte high, byte low)
        {
            ushort raw = (ushort)((high << 8) | low);
            return raw / 100m;
        }

        public static string DescribePacket(PtzPacket packet)
        {
            if (packet == null) return string.Empty;

            if (packet.Data1 == 0xff && packet.Data2 == 0xff && packet.Data3 == 0xff && packet.Data4 == 0xff)
                return "指令错误或校验失败";

            if (packet.Data1 == 0xee && packet.Data2 == 0xee && packet.Data3 == 0xee && packet.Data4 == 0xee)
                return "指令正确但执行失败";

            if (packet.Data1 == 0x00 && packet.Data2 == 0x59)
                return "水平角度回传: " + DecodeUnsigned100(packet.Data3, packet.Data4).ToString("F2", CultureInfo.InvariantCulture) + "°";

            if (packet.Data1 == 0x00 && packet.Data2 == 0x5b)
                return "垂直角度回传: " + DecodeSigned100(packet.Data3, packet.Data4).ToString("F2", CultureInfo.InvariantCulture) + "°";

            if (packet.Data1 == 0xe0)
                return "工作模式: " + WorkModeText(packet.Data2) + $" (Num1={packet.Data3}, Num2={packet.Data4})";

            if (packet.Data1 == 0xd6)
                return "温度: " + DecodeSigned100(packet.Data2, packet.Data3).ToString("F2", CultureInfo.InvariantCulture) + " °C";

            if (packet.Data1 == 0xcd)
                return "电压: " + DecodeUnsigned100(packet.Data2, packet.Data3).ToString("F2", CultureInfo.InvariantCulture) + " V";

            if (packet.Data1 == 0xc8)
                return "电流: " + DecodeUnsigned100(packet.Data2, packet.Data3).ToString("F2", CultureInfo.InvariantCulture) + " A";

            if (packet.Data1 == 0xd0 && packet.Data2 == 0x03)
                return "水平转速: " + DecodeUnsignedSpeed100(packet.Data3, packet.Data4).ToString("F2", CultureInfo.InvariantCulture) + " r/min";

            if (packet.Data1 == 0xd0 && packet.Data2 == 0x04)
                return "垂直转速: " + DecodeUnsignedSpeed100(packet.Data3, packet.Data4).ToString("F2", CultureInfo.InvariantCulture) + " r/min";

            if (packet.Data1 == 0x21)
                return $"水平电机: {(packet.Data2 == 0 ? "正常" : "故障")}, 方向={HorizontalDirectionText(packet.Data3)}, 转动={(packet.Data4 == 1 ? "是" : "否")}";

            if (packet.Data1 == 0x22)
                return "水平霍尔: " + (packet.Data2 == 0 ? "正常" : "故障");

            if (packet.Data1 == 0x23)
                return "水平光电位置更新: " + (packet.Data2 == 0 ? "正常" : "出错");

            if (packet.Data1 == 0x24)
                return $"垂直电机: {(packet.Data2 == 0 ? "正常" : "故障")}, 方向={VerticalDirectionText(packet.Data3)}, 转动={(packet.Data4 == 1 ? "是" : "否")}";

            if (packet.Data1 == 0x25)
                return "垂直霍尔: " + (packet.Data2 == 0 ? "正常" : "故障");

            if (packet.Data1 == 0x26)
                return $"垂直光电位置更新: SW1={packet.Data2}, SW2={packet.Data3}";

            if (packet.Data1 == 0x27)
                return $"温度状态: {(packet.Data2 == 0 ? "正常" : "高温故障")}, 温度={DecodeSigned100(packet.Data3, packet.Data4):F2} °C";

            if (packet.Data1 == 0x28)
                return $"电压状态: {(packet.Data2 == 0 ? "正常" : "异常")}, 电压={DecodeUnsigned100(packet.Data3, packet.Data4):F2} V";

            if (packet.Data1 == 0x29)
                return $"顶部电源: 电源1={(packet.Data2 == 1 ? "开" : "关")}, 电源2={(packet.Data3 == 1 ? "开" : "关")}";

            if (packet.Data1 == 0x2a)
                return $"电流状态: {(packet.Data2 == 0 ? "正常" : "异常")}, 电流={DecodeUnsigned100(packet.Data3, packet.Data4):F2} A";

            if (packet.Data1 >= 0x0b && packet.Data1 <= 0x14)
                return DescribeAreaConfig(packet);

            if (packet.Data1 == 0xc4)
                return DescribeAreaReturn(packet);

            if (packet.Data1 == 0x9f)
                return DescribePresetReturn(packet);

            if (packet.Data1 == 0xc5)
                return DescribeLocateReturn(packet);

            if (packet.Data1 == 0xdf)
                return packet.Data3 == 0x01 ? "指令回复已开启" : "指令回复已关闭";

            if (packet.Data1 == 0xde)
                return "云台复位重启指令应答";

            if (packet.Data1 == 0xce)
                return "云台全范围自检指令应答";

            return "回包/应答";
        }

        private static string DescribeAreaConfig(PtzPacket packet)
        {
            switch (packet.Data1)
            {
                case 0x0b: return $"区域 {packet.Data2} 水平起点: {DecodeUnsigned100(packet.Data3, packet.Data4):F2}°";
                case 0x0c: return $"区域 {packet.Data2} 水平终点: {DecodeUnsigned100(packet.Data3, packet.Data4):F2}°";
                case 0x0d: return $"区域 {packet.Data2} 垂直起点: {DecodeSigned100(packet.Data3, packet.Data4):F2}°";
                case 0x0e: return $"区域 {packet.Data2} 垂直终点: {DecodeSigned100(packet.Data3, packet.Data4):F2}°";
                case 0x0f: return $"区域 {packet.Data2} 水平间隔: {DecodeUnsigned100(packet.Data3, packet.Data4):F2}°";
                case 0x10: return $"区域 {packet.Data2} 垂直间隔: {DecodeUnsigned100(packet.Data3, packet.Data4):F2}°";
                case 0x11: return $"区域 {packet.Data2} 扫描速度: H={packet.Data3 / 10m:F1} r/min, V={packet.Data4 / 10m:F1} r/min";
                case 0x12: return $"区域 {packet.Data2} 停止时间: {((packet.Data3 << 8) | packet.Data4)} ms";
                case 0x13: return $"区域 {packet.Data2} 扫描模式: {(packet.Data3 == 2 ? "单步" : "连续")}";
                case 0x14: return $"区域 {packet.Data2} 使能: {(packet.Data3 == 1 ? "开" : "关")}";
                default: return "区域扫描配置回包";
            }
        }

        private static string DescribeAreaReturn(PtzPacket packet)
        {
            switch (packet.Data2)
            {
                case 0x00: return "区域扫描结束回传已关闭";
                case 0x01: return "区域扫描结束回传已开启";
                case 0x02: return "区域扫描结束回传: 区域 " + packet.Data3;
                case 0x03: return "区域单步到位回传已开启";
                case 0x04: return "区域单步到位回传已关闭";
                case 0x05: return "区域单步水平到位: " + DecodeUnsigned100(packet.Data3, packet.Data4).ToString("F2", CultureInfo.InvariantCulture) + "°";
                case 0x06: return "区域单步垂直到位: " + DecodeSigned100(packet.Data3, packet.Data4).ToString("F2", CultureInfo.InvariantCulture) + "°";
                default: return "区域扫描回传";
            }
        }

        private static string DescribePresetReturn(PtzPacket packet)
        {
            switch (packet.Data2)
            {
                case 0x00: return "预置位扫描结束回传已关闭";
                case 0x01: return "预置位扫描结束回传已开启";
                case 0x02: return "预置位扫描到位回传已开启";
                case 0x03: return "预置位扫描到位回传已关闭";
                case 0x04: return "调用预置位到位回传已开启";
                case 0x05: return "调用预置位到位回传已关闭";
                case 0x06: return $"预置位扫描结束: {packet.Data3} -> {packet.Data4}";
                case 0x07: return "预置位扫描到位: " + packet.Data3;
                case 0x08: return "调用预置位到位: " + packet.Data3;
                default: return "预置位回传";
            }
        }

        private static string DescribeLocateReturn(PtzPacket packet)
        {
            switch (packet.Data2)
            {
                case 0x00: return "角度定位回传已关闭";
                case 0x01: return "角度定位回传已开启";
                case 0x02: return "水平定位到位: " + DecodeUnsigned100(packet.Data3, packet.Data4).ToString("F2", CultureInfo.InvariantCulture) + "°";
                case 0x03: return "垂直定位到位: " + DecodeSigned100(packet.Data3, packet.Data4).ToString("F2", CultureInfo.InvariantCulture) + "°";
                default: return "角度定位回传";
            }
        }

        private static string WorkModeText(byte mode)
        {
            switch (mode)
            {
                case 0x00: return "常规正常模式";
                case 0x01: return "自检中";
                case 0x02: return "区域扫描进行中";
                case 0x03: return "区域扫描暂停中";
                case 0x04: return "区域扫描恢复中";
                case 0x05: return "区域扫描关闭中";
                case 0x06: return "预置位扫描中";
                case 0x07: return "预置位扫描暂停中";
                case 0x08: return "预置位扫描恢复中";
                case 0x09: return "预置位扫描关闭中";
                case 0x10: return "水平电压采集中";
                case 0x11: return "垂直电压采集中";
                case 0x12: return "水平/垂直电压采集中";
                case 0x13: return "水平位置更新出错";
                case 0x14: return "垂直位置更新出错";
                case 0x15: return "水平/垂直位置更新出错";
                case 0x16: return "电机低温无法转动";
                case 0xfa: return "水平电机低温故障";
                case 0xfb: return "垂直电机低温故障";
                case 0xfc: return "水平电机故障无法转动";
                case 0xfd: return "垂直电机故障无法转动";
                case 0xfe: return "所有电机故障无法转动";
                default: return "未知模式 0x" + mode.ToString("X2", CultureInfo.InvariantCulture);
            }
        }

        private static string HorizontalDirectionText(byte value)
        {
            if (value == 3) return "左";
            if (value == 4) return "右";
            return "未知";
        }

        private static string VerticalDirectionText(byte value)
        {
            if (value == 1) return "上";
            if (value == 2) return "下";
            return "未知";
        }
    }
}
