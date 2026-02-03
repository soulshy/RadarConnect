using System;
using System.Runtime.InteropServices;

namespace RadarConnect
{
    public static class ProtocolUtils
    {
        private static ushort _seqNum = 0;
        private static readonly object _seqLock = new object();

        public static ushort GetSeqNum()
        {
            lock (_seqLock) { _seqNum++; return _seqNum; }
        }

        public static byte[] StructToBytes<T>(T str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try { Marshal.StructureToPtr(str, ptr, true); Marshal.Copy(ptr, arr, 0, size); }
            finally { Marshal.FreeHGlobal(ptr); }
            return arr;
        }

        public static T BytesToStruct<T>(byte[] bytes)
        {
            int size = Marshal.SizeOf(typeof(T));
            if (bytes.Length < size) return default(T);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try { Marshal.Copy(bytes, 0, ptr, size); return (T)Marshal.PtrToStructure(ptr, typeof(T)); }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        // ---  Livox CRC 算法 ---

        // CRC16: MCRF4XX (Poly=0x1021 Reflected -> 0x8408, Init=0x4C49)
        public static ushort Crc16(byte[] data, int length)
        {
            ushort crc = 0x4C49; // 官方 SDK 种子
            ushort poly = 0x8408;

            for (int i = 0; i < length; i++)
            {
                crc ^= data[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 1) != 0)
                        crc = (ushort)((crc >> 1) ^ poly);
                    else
                        crc = (ushort)(crc >> 1);
                }
            }
            return crc;
        }

        // CRC32: Ethernet (Poly=0x04C11DB7 Reflected -> 0xEDB88320, Init=0x564F580A)
        // 修正后的 CRC32 方法
        public static uint Crc32(byte[] data, int length)
        {
            uint crc = 0x564F580A; // 官方 SDK 种子
            uint poly = 0xEDB88320;

            crc ^= 0xFFFFFFFF;

            for (int i = 0; i < length; i++)
            {
                crc ^= data[i];
                for (int j = 0; j < 8; j++)
                    crc = (uint)((crc & 1) != 0 ? (crc >> 1) ^ poly : crc >> 1);
            }

            return crc ^ 0xFFFFFFFF;
        }
    }
}