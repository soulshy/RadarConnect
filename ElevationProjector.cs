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

            // 1. 获取物理边界，这次使用 Y(左右宽度) 和 Z(上下高度) 作为画板的 XY 轴
            float minY = float.MaxValue, maxY = float.MinValue;
            float minZ = float.MaxValue, maxZ = float.MinValue;

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
            }

            if (maxY - minY < 0.1f) maxY = minY + 1.0f;
            if (maxZ - minZ < 0.1f) maxZ = minZ + 1.0f;

            float rangeY = maxY - minY;
            float rangeZ = maxZ - minZ;

            // 2. 计算缩放系数，保持真实物理比例 (门不会被拉宽或压扁)
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

                            // 保留灰度反射率逻辑
                            byte intensity = Math.Max((byte)80, p.Reflectivity);

                            ptr[idx] = intensity;
                            ptr[idx + 1] = intensity;
                            ptr[idx + 2] = intensity;
                        }
                    }
                }
            }
            bmp.UnlockBits(bmpData);

            return bmp;
        }
    }
}