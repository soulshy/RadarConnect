using System;
using System.Collections.Generic;

namespace RadarConnect
{
    public class DepthCompleter
    {
        /// <summary>
        ///点云深度补全
        /// </summary>
        /// <param name="points">原始稀疏点云</param>
        /// <param name="windowSize">滑动窗口大小 (如 5x5, 7x7)</param>
        /// <param name="canvasSize">映射的2D网格分辨率，越大点云越密</param>
        public static List<PointData> CompleteDepth(List<PointData> points, int windowSize = 5, int canvasSize = 800)
        {
            if (points == null || points.Count == 0) return new List<PointData>();

            float minY = float.MaxValue, maxY = float.MinValue;
            float minZ = float.MaxValue, maxZ = float.MinValue;

            // 1. 获取物理边界
            foreach (var p in points)
            {
                if (float.IsNaN(p.X) || float.IsNaN(p.Y) || float.IsNaN(p.Z) || p.X < 0.1f) continue;
                if (p.Y < minY) minY = p.Y;
                if (p.Y > maxY) maxY = p.Y;
                if (p.Z < minZ) minZ = p.Z;
                if (p.Z > maxZ) maxZ = p.Z;
            }

            if (maxY - minY < 0.1f) maxY = minY + 1.0f;
            if (maxZ - minZ < 0.1f) maxZ = minZ + 1.0f;

            float rangeY = maxY - minY;
            float rangeZ = maxZ - minZ;

            // 计算缩放系数，投影至 2D Canvas
            float scaleY = (float)canvasSize / rangeY;
            float scaleZ = (float)canvasSize / rangeZ;
            float scale = Math.Min(scaleY, scaleZ) * 0.95f;

            float offsetY = (canvasSize - rangeY * scale) / 2.0f;
            float offsetZ = (canvasSize - rangeZ * scale) / 2.0f;

            float[,] depthBuffer = new float[canvasSize, canvasSize];
            byte[,] refBuffer = new byte[canvasSize, canvasSize];

            for (int i = 0; i < canvasSize; i++)
            {
                for (int j = 0; j < canvasSize; j++)
                {
                    depthBuffer[i, j] = float.MaxValue;
                }
            }

            // 2. 投影到 2D 深度网格 (Z-Buffer)
            foreach (var p in points)
            {
                if (float.IsNaN(p.X) || float.IsNaN(p.Y) || float.IsNaN(p.Z) || p.X < 0.1f) continue;

                int u = (int)((maxY - p.Y) * scale + offsetY);
                int v = (int)((maxZ - p.Z) * scale + offsetZ);

                if (u >= 0 && u < canvasSize && v >= 0 && v < canvasSize)
                {
                    // 深度测试：保留最近的点
                    if (p.X < depthBuffer[u, v])
                    {
                        depthBuffer[u, v] = p.X;
                        refBuffer[u, v] = p.Reflectivity;
                    }
                }
            }

            // 3. 执行传统的 IDW (反距离加权) 空间插值填补空洞
            float[,] newDepthBuffer = (float[,])depthBuffer.Clone();
            byte[,] newRefBuffer = (byte[,])refBuffer.Clone();
            int halfWin = windowSize / 2;

            for (int u = 0; u < canvasSize; u++)
            {
                for (int v = 0; v < canvasSize; v++)
                {
                    if (depthBuffer[u, v] == float.MaxValue) // 发现空洞
                    {
                        float sumWeight = 0;
                        float sumDepth = 0;
                        float sumRef = 0;
                        int validNeighbors = 0;

                        // 搜索邻域窗口
                        for (int du = -halfWin; du <= halfWin; du++)
                        {
                            for (int dv = -halfWin; dv <= halfWin; dv++)
                            {
                                int nu = u + du;
                                int nv = v + dv;

                                if (nu >= 0 && nu < canvasSize && nv >= 0 && nv < canvasSize)
                                {
                                    if (depthBuffer[nu, nv] != float.MaxValue)
                                    {
                                        float dist = (float)Math.Sqrt(du * du + dv * dv);
                                        float weight = 1.0f / (dist + 0.0001f);
                                        sumWeight += weight;
                                        sumDepth += depthBuffer[nu, nv] * weight;
                                        sumRef += refBuffer[nu, nv] * weight;
                                        validNeighbors++;
                                    }
                                }
                            }
                        }

                        // 如果周围存在至少2个有效邻居，才进行填补，防止在点云外部边缘凭空生成噪点
                        if (validNeighbors >= 2 && sumWeight > 0)
                        {
                            newDepthBuffer[u, v] = sumDepth / sumWeight;
                            newRefBuffer[u, v] = (byte)(sumRef / sumWeight);
                        }
                    }
                }
            }

            // 4. 反投影，恢复回 3D 点云数据
            List<PointData> completedPoints = new List<PointData>();
            for (int u = 0; u < canvasSize; u++)
            {
                for (int v = 0; v < canvasSize; v++)
                {
                    if (newDepthBuffer[u, v] != float.MaxValue)
                    {
                        float pX = newDepthBuffer[u, v];
                        // 还原物理坐标
                        float pY = maxY - (u - offsetY) / scale;
                        float pZ = maxZ - (v - offsetZ) / scale;

                        completedPoints.Add(new PointData
                        {
                            X = pX,
                            Y = pY,
                            Z = pZ,
                            Depth = (float)Math.Sqrt(pX * pX + pY * pY + pZ * pZ),
                            Reflectivity = newRefBuffer[u, v],
                            ExactTime = DateTime.Now
                        });
                    }
                }
            }

            return completedPoints;
        }
    }
}