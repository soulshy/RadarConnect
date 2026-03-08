using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace RadarConnect
{
    public class ElevationProjector
    {
        /// <summary>
        /// 生成高保真“正视图/立视图”，完美复刻图一的平视长条走廊效果
        /// </summary>
        public static Bitmap CreateFrontViewImage(List<PointData> points, int canvasWidth = 3840, int canvasHeight = 1080)
        {
            if (points == null || points.Count == 0) return null;

            // 1. 获取物理边界，增加对 X(深度) 的极值获取，用于彩色映射归一化
            float minY = float.MaxValue, maxY = float.MinValue;
            float minZ = float.MaxValue, maxZ = float.MinValue;
            float minX = float.MaxValue, maxX = float.MinValue;

            foreach (var p in points)
            {
                if (float.IsNaN(p.Y) || float.IsNaN(p.Z) || float.IsNaN(p.X)) continue;

                // 剔除雷达原点自身的噪点，以及雷达背后的点（X<0.1表示只看前方）
                if (p.X < 0.1f) continue;
                // 剔除过分极端的飞点，保证主体建筑占满画面
                if (p.Y < -50f || p.Y > 50f || p.Z < -20f || p.Z > 20f) continue;

                if (p.Y < minY) minY = p.Y;
                if (p.Y > maxY) maxY = p.Y;
                if (p.Z < minZ) minZ = p.Z;
                if (p.Z > maxZ) maxZ = p.Z;
                if (p.X < minX) minX = p.X;
                if (p.X > maxX) maxX = p.X;
            }

            if (maxY - minY < 0.1f) maxY = minY + 1.0f;
            if (maxZ - minZ < 0.1f) maxZ = minZ + 1.0f;
            if (maxX - minX < 0.1f) maxX = minX + 1.0f;

            float rangeY = maxY - minY;
            float rangeZ = maxZ - minZ;

            // 2. 计算缩放系数，保持真实物理比例              
            float scaleY = (float)canvasWidth / rangeY;
            float scaleZ = (float)canvasHeight / rangeZ;
            float scale = Math.Min(scaleY, scaleZ) * 0.95f;

            float offsetY = (canvasWidth - rangeY * scale) / 2.0f;
            float offsetZ = (canvasHeight - rangeZ * scale) / 2.0f;

            // 3. 深度缓冲 (Z-Buffer)：这次用 X(前后距离) 决定谁挡住谁
            float[,] depthBuffer = new float[canvasWidth, canvasHeight];
            for (int i = 0; i < canvasWidth; i++)
            {
                for (int j = 0; j < canvasHeight; j++)
                {
                    depthBuffer[i, j] = float.MaxValue;
                }
            }

            Bitmap bmp = new Bitmap(canvasWidth, canvasHeight, PixelFormat.Format24bppRgb);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, canvasWidth, canvasHeight), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;
                int stride = bmpData.Stride;

                foreach (var p in points)
                {
                    if (float.IsNaN(p.Y) || float.IsNaN(p.Z) || float.IsNaN(p.X)) continue;
                    if (p.X < 0.1f || p.Y < -50f || p.Y > 50f || p.Z < -20f || p.Z > 20f) continue;

                    // 4. 翻转 Y 轴消除镜像：用 maxY 减去 p.Y，让左边的物理点画在左侧像素，右侧画在右侧
                    int u = (int)((maxY - p.Y) * scale + offsetY);
                    int v = (int)((maxZ - p.Z) * scale + offsetZ);

                    if (u >= 0 && u < canvasWidth && v >= 0 && v < canvasHeight)
                    {
                        // 5. 深度测试：前面的点遮挡后面的点
                        if (p.X < depthBuffer[u, v])
                        {
                            depthBuffer[u, v] = p.X;

                            int idx = v * stride + u * 3;

                            // 6. 归一化深度 (0.0 ~ 1.0)
                            float ratio = (p.X - minX) / (maxX - minX);
                            ratio = Math.Max(0f, Math.Min(1f, ratio));

                            // Jet Colormap 算法（近红远蓝）
                            float r = ClampColor(1.5f - Math.Abs(4.0f * (1.0f - ratio) - 3.0f));
                            float g = ClampColor(1.5f - Math.Abs(4.0f * (1.0f - ratio) - 2.0f));
                            float b = ClampColor(1.5f - Math.Abs(4.0f * (1.0f - ratio) - 1.0f));

                            // 结合反射率微调亮度 (保留原有质感，反射率越高越亮，最低保持一定亮度防止纯黑)
                            float intensityFactor = Math.Max(0.5f, p.Reflectivity / 255.0f * 1.5f);

                            // 写入像素。注意：Format24bppRgb 在内存中的顺序是 B, G, R
                            ptr[idx] = (byte)Math.Min(255, (b * 255 * intensityFactor));     // Blue
                            ptr[idx + 1] = (byte)Math.Min(255, (g * 255 * intensityFactor)); // Green
                            ptr[idx + 2] = (byte)Math.Min(255, (r * 255 * intensityFactor)); // Red
                        }
                    }
                }
            }
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        // 辅助方法：限制颜色范围在 0~1 之间
        private static float ClampColor(float val)
        {
            if (val < 0f) return 0f;
            if (val > 1f) return 1f;
            return val;
        }
    }
}