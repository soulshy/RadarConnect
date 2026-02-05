using System;
using System.Collections.Generic;
using System.Linq;

namespace RadarConnect
{
    /// <summary>
    /// 点云处理器
    /// </summary>
    public class PointCloudProcessor
    {
        // 1. 基础过滤
        public float MinDistance = 1.5f;   // 0.1米防遮挡
        public float MaxDistance = 200.0f; // 200米

        // 确保能看到黑色物体/墙面
        public byte MinReflectivity = 0;

        // 2. 降采样因子 (1 = 全显示, 2 = 丢弃一半, 3 = 丢弃2/3...)
        public int DownsampleFactor = 1;

        public List<PointData> ApplyFilters(List<PointData> rawPoints)
        {
            if (rawPoints == null || rawPoints.Count == 0) return new List<PointData>();

            // 预分配内存，避免 GC
            List<PointData> result = new List<PointData>(rawPoints.Count / DownsampleFactor);

            float minDSq = MinDistance * MinDistance;
            float maxDSq = MaxDistance * MaxDistance;

            for (int i = 0; i < rawPoints.Count; i++)
            {
                // 简单的降采样：每隔 N 个点取 1 个
                if (i % DownsampleFactor != 0) continue;

                var p = rawPoints[i];

                // 1. 反射率过滤
                if (p.Reflectivity < MinReflectivity) continue;

                // 2. 距离过滤
                float distSq = p.X * p.X + p.Y * p.Y + p.Z * p.Z;
                if (distSq < minDSq || distSq > maxDSq) continue;

                result.Add(p);
            }

            return result;
        }
    }
}