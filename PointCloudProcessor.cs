using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices; // inline optimization
using System.Text;

namespace RadarConnect
{
    /// <summary>
    /// 点云处理器
    /// </summary>
    public class PointCloudProcessor
    {
        // ==========================================
        // 1. 过滤参数
        // ==========================================
        // 距离阈值 (平方值缓存，避免循环内开方)
        private float _minDistSq = 1.5f * 1.5f;
        private float _maxDistSq = 200.0f * 200.0f;

        private float _minDist = 1.5f;
        public float MinDistance
        {
            get => _minDist;
            set { _minDist = value; _minDistSq = value * value; }
        }

        private float _maxDist = 200.0f;
        public float MaxDistance
        {
            get => _maxDist;
            set { _maxDist = value; _maxDistSq = value * value; }
        }

        // 反射率阈值
        public byte MinReflectivity = 0;

        // 降采样因子 (必须 >= 1)
        private int _downsampleFactor = 1;
        public int DownsampleFactor
        {
            get => _downsampleFactor;
            set => _downsampleFactor = Math.Max(1, value); // 防止设为0导致死循环
        }

        // ==========================================
        // 2. ROI (Region of Interest)
        // ==========================================
        // 用于过滤地面(Z > -1.5) 或 天花板(Z < 3.0)
        public bool EnableRoiFilter = false;
        public float MinZ = -500f;
        public float MaxZ = 500f;
        public float MinX = -500f;
        public float MaxX = 500f;
        public float MinY = -500f;
        public float MaxY = 500f;

        /// <summary>
        /// 处理点云
        /// </summary>
        /// <param name="rawPoints">原始数据源</param>
        /// <param name="outputBuffer">输出缓冲区 (传入已有的List以复用内存)</param>
        public void ApplyFilters(List<PointData> rawPoints, List<PointData> outputBuffer)
        {
            // 1. 基础检查
            if (rawPoints == null || rawPoints.Count == 0) return;
            if (outputBuffer == null) throw new ArgumentNullException(nameof(outputBuffer));

            // 2. 清空输出缓冲区 (不释放内存，只重置计数，极快)
            outputBuffer.Clear();

            // 缓存局部变量，减少属性访问开销
            float minDSq = _minDistSq;
            float maxDSq = _maxDistSq;
            byte minRef = MinReflectivity;
            int step = DownsampleFactor;
            int count = rawPoints.Count;

            // 缓存 ROI 变量
            bool useRoi = EnableRoiFilter;
            float minZ = MinZ, maxZ = MaxZ;
            float minX = MinX, maxX = MaxX;
            float minY = MinY, maxY = MaxY;

            // 3. 循环优化
            for (int i = 0; i < count; i += step)
            {
                // 获取结构体
                PointData p = rawPoints[i];

                // --- 第一层：极速过滤 (反射率) ---
                if (p.Reflectivity < minRef) continue;

                // --- 第二层：有效性检查 (防止 NaN 导致程序崩溃) ---
                if (float.IsNaN(p.X) || float.IsNaN(p.Y) || float.IsNaN(p.Z)) continue;

                // --- 第三层：距离过滤 (球体) ---
                float distSq = p.X * p.X + p.Y * p.Y + p.Z * p.Z;
                if (distSq < minDSq || distSq > maxDSq) continue;

                // --- 第四层：ROI 过滤 (立方体) ---
                if (useRoi)
                {
                    if (p.Z < minZ || p.Z > maxZ) continue;
                    if (p.X < minX || p.X > maxX) continue;
                    if (p.Y < minY || p.Y > maxY) continue;
                }

                // 加入结果
                outputBuffer.Add(p);
            }
        }

        /// <summary>
        /// Export point cloud data to an ASCII PCD file. PointData coordinates are already stored in meters.
        /// </summary>
        public static int ExportToPcd(IEnumerable<PointData> points, string filePath, bool includeTag = true)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("Export path cannot be empty.", nameof(filePath));

            List<PointData> validPoints = new List<PointData>();
            foreach (PointData p in points)
            {
                if (!IsValidCoordinate(p.X) || !IsValidCoordinate(p.Y) || !IsValidCoordinate(p.Z)) continue;
                validPoints.Add(p);
            }

            string directory = Path.GetDirectoryName(Path.GetFullPath(filePath));
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter writer = new StreamWriter(filePath, false, new UTF8Encoding(false), 64 * 1024))
            {
                WritePcdHeader(writer, validPoints.Count, includeTag);

                IFormatProvider culture = CultureInfo.InvariantCulture;
                for (int i = 0; i < validPoints.Count; i++)
                {
                    PointData p = validPoints[i];
                    if (includeTag)
                    {
                        writer.WriteLine(
                            string.Format(
                                culture,
                                "{0:R} {1:R} {2:R} {3} {4}",
                                p.X,
                                p.Y,
                                p.Z,
                                p.Reflectivity,
                                p.Tag));
                    }
                    else
                    {
                        writer.WriteLine(
                            string.Format(
                                culture,
                                "{0:R} {1:R} {2:R} {3}",
                                p.X,
                                p.Y,
                                p.Z,
                                p.Reflectivity));
                    }
                }
            }

            return validPoints.Count;
        }

        private static void WritePcdHeader(StreamWriter writer, int pointCount, bool includeTag)
        {
            writer.WriteLine("# .PCD v0.7 - Point Cloud Data file format");
            writer.WriteLine("VERSION 0.7");
            writer.WriteLine(includeTag ? "FIELDS x y z intensity tag" : "FIELDS x y z intensity");
            writer.WriteLine(includeTag ? "SIZE 4 4 4 4 1" : "SIZE 4 4 4 4");
            writer.WriteLine(includeTag ? "TYPE F F F F U" : "TYPE F F F F");
            writer.WriteLine(includeTag ? "COUNT 1 1 1 1 1" : "COUNT 1 1 1 1");
            writer.WriteLine("WIDTH " + pointCount.ToString(CultureInfo.InvariantCulture));
            writer.WriteLine("HEIGHT 1");
            writer.WriteLine("VIEWPOINT 0 0 0 1 0 0 0");
            writer.WriteLine("POINTS " + pointCount.ToString(CultureInfo.InvariantCulture));
            writer.WriteLine("DATA ascii");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidCoordinate(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }
    }
}

