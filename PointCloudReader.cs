using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RadarConnect
{
    public static class PointCloudReader
    {
        /// <summary>
        /// 读取点云文件（支持 .pcd 和 .las）并转换为内部的 PointData 列表
        /// </summary>
        public static List<PointData> Read(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"文件未找到: {filePath}");

            string extension = Path.GetExtension(filePath).ToLower();

            if (extension == ".pcd")
            {
                return ReadPcd(filePath);
            }
            else if (extension == ".las")
            {
                return ReadLas(filePath);
            }
            else
            {
                throw new NotSupportedException($"不支持的文件格式: {extension}");
            }
        }

        #region LAS 解析逻辑

        /// <summary>
        /// 解析基本的 LAS 文件格式 (适用于 LAS 1.0 - 1.3 规范)
        /// </summary>
        private static List<PointData> ReadLas(string filePath)
        {
            List<PointData> points = new List<PointData>();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                // 1. 验证 LAS 文件头签名
                string signature = new string(br.ReadChars(4));
                if (signature != "LASF")
                    throw new Exception("不是有效的 LAS 文件头 (缺少 LASF 签名)。");

                // 2. 读取关键偏移量和长度信息
                br.BaseStream.Seek(96, SeekOrigin.Begin);
                uint offsetToPointData = br.ReadUInt32(); // 数据起始位置

                br.BaseStream.Seek(105, SeekOrigin.Begin);
                ushort pointDataRecordLength = br.ReadUInt16(); // 单个点数据的字节长度

                uint numberOfPoints = br.ReadUInt32(); // 总点数

                // 3. 读取缩放因子 (Scale Factor)
                br.BaseStream.Seek(131, SeekOrigin.Begin);
                double xScale = br.ReadDouble();
                double yScale = br.ReadDouble();
                double zScale = br.ReadDouble();

                // 4. 读取偏移量 (Offset)
                double xOffset = br.ReadDouble();
                double yOffset = br.ReadDouble();
                double zOffset = br.ReadDouble();

                // 5. 将指针移动到点云数据起始区
                br.BaseStream.Seek(offsetToPointData, SeekOrigin.Begin);

                // 6. 逐个解析点云
                for (uint i = 0; i < numberOfPoints; i++)
                {
                    long currentRecordStart = br.BaseStream.Position;

                    // LAS 标准中，前 12 个字节永远是 X, Y, Z 的 Int32 原始值
                    int xRaw = br.ReadInt32();
                    int yRaw = br.ReadInt32();
                    int zRaw = br.ReadInt32();

                    // 第 13-14 字节是强度 Intensity (ushort)
                    ushort intensityRaw = br.ReadUInt16();

                    // 还原真实的物理坐标：实际坐标 = (原始值 * 缩放因子) + 偏移量
                    float x = (float)(xRaw * xScale + xOffset);
                    float y = (float)(yRaw * yScale + yOffset);
                    float z = (float)(zRaw * zScale + zOffset);

                    // LAS的强度是 uint16 (0-65535)，这里根据你的 PointData 转成 byte (0-255)
                    // 如果你的雷达强度数据本身只用了 0-255 存在 ushort 里，直接强转；
                    // 如果占满了 65535，可能需要右移：(byte)(intensityRaw >> 8)
                    byte refI = intensityRaw > 255 ? (byte)255 : (byte)intensityRaw;

                    points.Add(CreatePointData(x, y, z, refI));

                    // 严格按照 pointDataRecordLength 跳转到下一个点，防止不同版本(Format 0-10)附加数据导致错位
                    br.BaseStream.Seek(currentRecordStart + pointDataRecordLength, SeekOrigin.Begin);
                }
            }

            return points;
        }

        #endregion

        #region PCD 解析逻辑

        private static List<PointData> ReadPcd(string filePath)
        {
            List<PointData> points = new List<PointData>();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                bool isAscii = true;
                int pointsCount = 0;
                int xIndex = -1, yIndex = -1, zIndex = -1, intensityIndex = -1;
                int fieldCount = 0;

                string line;
                while (!string.IsNullOrEmpty(line = ReadLine(br)))
                {
                    if (line.StartsWith("FIELDS"))
                    {
                        string[] fields = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        fieldCount = fields.Length - 1;
                        for (int i = 1; i < fields.Length; i++)
                        {
                            string fieldName = fields[i].ToLower();
                            if (fieldName == "x") xIndex = i - 1;
                            else if (fieldName == "y") yIndex = i - 1;
                            else if (fieldName == "z") zIndex = i - 1;
                            else if (fieldName == "intensity" || fieldName == "reflectivity") intensityIndex = i - 1;
                        }
                    }
                    else if (line.StartsWith("POINTS"))
                    {
                        pointsCount = int.Parse(line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                    }
                    else if (line.StartsWith("DATA"))
                    {
                        isAscii = line.Contains("ascii");
                        break;
                    }
                }

                if (xIndex == -1 || yIndex == -1 || zIndex == -1)
                    throw new Exception("PCD文件中未找到 x, y, z 字段。");

                if (isAscii)
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        for (int i = 0; i < pointsCount; i++)
                        {
                            string dataLine = sr.ReadLine();
                            if (string.IsNullOrWhiteSpace(dataLine)) break;
                            string[] parts = dataLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length < fieldCount) continue;

                            float x = float.Parse(parts[xIndex]);
                            float y = float.Parse(parts[yIndex]);
                            float z = float.Parse(parts[zIndex]);
                            byte refI = intensityIndex != -1 ? (byte)Math.Min(255, float.Parse(parts[intensityIndex])) : (byte)255;

                            points.Add(CreatePointData(x, y, z, refI));
                        }
                    }
                }
                else
                {
                    int bytesPerPoint = fieldCount * 4;
                    for (int i = 0; i < pointsCount; i++)
                    {
                        byte[] pointBytes = br.ReadBytes(bytesPerPoint);
                        if (pointBytes.Length < bytesPerPoint) break;

                        float x = BitConverter.ToSingle(pointBytes, xIndex * 4);
                        float y = BitConverter.ToSingle(pointBytes, yIndex * 4);
                        float z = BitConverter.ToSingle(pointBytes, zIndex * 4);
                        byte refI = 255;

                        if (intensityIndex != -1)
                        {
                            refI = (byte)Math.Min(255, BitConverter.ToSingle(pointBytes, intensityIndex * 4));
                        }

                        points.Add(CreatePointData(x, y, z, refI));
                    }
                }
            }
            return points;
        }

        #endregion

        #region 通用辅助方法

        private static PointData CreatePointData(float x, float y, float z, byte reflectivity)
        {
            return new PointData
            {
                X = x,
                Y = y,
                Z = z,
                Depth = (float)Math.Sqrt(x * x + y * y + z * z),
                Reflectivity = reflectivity,
                ExactTime = DateTime.Now
            };
        }

        private static string ReadLine(BinaryReader br)
        {
            List<byte> bytes = new List<byte>();
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                byte b = br.ReadByte();
                if (b == '\n') break;
                if (b != '\r') bytes.Add(b);
            }
            return Encoding.UTF8.GetString(bytes.ToArray());
        }

        #endregion
    }
}