using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RadarConnect
{
    public class UdpCommunication : IDisposable
    {
        // 通道1: 广播监听 (只收不发)
        private UdpClient _broadcastListener;
        // 通道2: 命令交互 (发指令 / 收ACK) - 绑定到 50001
        private UdpClient _cmdClient;
        // 通道3: 数据接收 (只收点云) - 绑定到 60000
        private UdpClient _dataListener;

        private Thread _broadcastThread;
        private Thread _cmdThread;
        private Thread _dataThread;
        private Thread _dataDispatchThread;

        private sealed class ReceivedDataPacket
        {
            public byte[] Data;
            public DateTime ReceivedUtc;
        }

        private BlockingCollection<ReceivedDataPacket> _dataQueue;
        private long _receivedDataPackets;
        private long _droppedDataPackets;

        private bool _isRunning = false;

        // 事件定义
        public event Action<byte[], IPEndPoint> OnBroadcastReceived;
        public event Action<byte[], IPEndPoint> OnCmdAckReceived; //命令回复
        public event Action<byte[], DateTime> OnDataReceived;
        public event Action<string> OnError;

        public bool IsConnected => _isRunning;
        public long ReceivedDataPackets => Interlocked.Read(ref _receivedDataPackets);
        public long DroppedDataPackets => Interlocked.Read(ref _droppedDataPackets);

        /// <summary>
        /// 启动所有网络服务
        /// </summary>
        /// <param name="localIp">本机IP</param>
        /// <param name="cmdPort">命令端口 (建议 50001)</param>
        /// <param name="dataPort">数据端口 (建议 60000)</param>
        public void Start(string localIp, int cmdPort, int dataPort)
        {
            if (_isRunning) return;
            try
            {
                _isRunning = true;
                IPAddress ip = IPAddress.Parse(localIp);

                // 1. 启动广播监听 (55000)
                _broadcastListener = new UdpClient();
                _broadcastListener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _broadcastListener.Client.Bind(new IPEndPoint(IPAddress.Any, 55000));

                _broadcastThread = new Thread(ReceiveBroadcastLoop) { IsBackground = true };
                _broadcastThread.Start();

                // 2. 启动命令通道 (绑定到 cmdPort)
                _cmdClient = new UdpClient();
                _cmdClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _cmdClient.Client.Bind(new IPEndPoint(ip, cmdPort));

                _cmdThread = new Thread(ReceiveCmdLoop) { IsBackground = true };
                _cmdThread.Start();

                // 3. 启动数据通道 (绑定到 dataPort)
                _dataListener = new UdpClient();
                _dataListener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _dataListener.Client.ReceiveBufferSize = 1024 * 1024 * 100; // 100MB 缓冲区
                _dataListener.Client.Bind(new IPEndPoint(IPAddress.Any, dataPort));

                // 接收线程只负责尽快收包，解析与入库交给独立线程，避免阻塞 UDP Receive。
                var dataQueue = new BlockingCollection<ReceivedDataPacket>(8192);
                _dataQueue = dataQueue;
                _dataDispatchThread = new Thread(() => ProcessDataQueue(dataQueue)) { IsBackground = true };
                _dataDispatchThread.Start();

                _dataThread = new Thread(ReceiveDataLoop) { IsBackground = true };
                _dataThread.Start();
            }
            catch (Exception ex)
            {
                Stop(); // 发生错误清理资源
                OnError?.Invoke($"启动失败: {ex.Message}");
            }
        }

        public void Stop()
        {
            _isRunning = false;
            try { _broadcastListener?.Close(); } catch { }
            try { _cmdClient?.Close(); } catch { }
            try { _dataListener?.Close(); } catch { }
            try { _dataQueue?.CompleteAdding(); } catch { }
        }

        // 发送命令 (通过命令通道发出)
        public void SendCommand(byte[] data, string targetIp, int targetPort)
        {
            try
            {
                if (_cmdClient != null)
                {
                    IPEndPoint remote = new IPEndPoint(IPAddress.Parse(targetIp), targetPort);
                    _cmdClient.Send(data, data.Length, remote);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"发送异常: {ex.Message}");
            }
        }

        // --- 接收循环 ---

        private void ReceiveBroadcastLoop()
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            while (_isRunning)
            {
                try
                {
                    if (_broadcastListener == null) break;
                    byte[] data = _broadcastListener.Receive(ref remote);
                    OnBroadcastReceived?.Invoke(data, remote);
                }
                catch { }
            }
        }

        private void ReceiveCmdLoop()
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            while (_isRunning)
            {
                try
                {
                    if (_cmdClient == null) break;
                    byte[] data = _cmdClient.Receive(ref remote);
                    OnCmdAckReceived?.Invoke(data, remote);
                }
                catch { }
            }
        }

        private void ReceiveDataLoop()
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            while (_isRunning)
            {
                try
                {
                    if (_dataListener == null) break;
                    byte[] data = _dataListener.Receive(ref remote);
                    DateTime receivedUtc = DateTime.UtcNow;
                    Interlocked.Increment(ref _receivedDataPackets);

                    var packet = new ReceivedDataPacket
                    {
                        Data = data,
                        ReceivedUtc = receivedUtc
                    };

                    if (_dataQueue == null || !_dataQueue.TryAdd(packet))
                        Interlocked.Increment(ref _droppedDataPackets);
                }
                catch (Exception ex)
                {
                    // 在输出窗口打印错误，方便调试
                    System.Diagnostics.Debug.WriteLine("接收异常: " + ex.Message);
                }
            }
        }

        private void ProcessDataQueue(BlockingCollection<ReceivedDataPacket> dataQueue)
        {
            try
            {
                foreach (ReceivedDataPacket packet in dataQueue.GetConsumingEnumerable())
                {
                    try
                    {
                        OnDataReceived?.Invoke(packet.Data, packet.ReceivedUtc);
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke("点云处理异常: " + ex.Message);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public void Dispose() => Stop();
    }
}
