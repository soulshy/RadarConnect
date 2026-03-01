using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RadarConnect
{
    /// <summary>
    /// 传感器融合器：处理 Lidar 点云与 Camera 图像的空间映射
    /// </summary>
    public class SensorFusion
    {
        // 相机内参 (Camera Intrinsics)
        public float Fx { get; set; } = 1200f;
        public float Fy { get; set; } = 1200f;
        public float Cx { get; set; } = 960f;
        public float Cy { get; set; } = 540f;

        /// <summary>
        /// 将 3D 点云通过针孔相机模型和外参投影到 2D 图像上
        /// </summary>
        public Image ProjectPointCloudToImage(string imagePath, List<PointData> points, float maxDepth = 30.0f)
        {
            Bitmap bmp;
            // 使用 FileStream 加载避免文件长期锁定
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                using (Image tempImg = Image.FromStream(fs))
                {
                    // 【修复核心】创建一个新的非索引格式 Bitmap (Format32bppArgb)
                    // 这样即使源图是 OpenCV 生成的灰度图（索引格式），也能创建 Graphics 对象进行绘制
                    bmp = new Bitmap(tempImg.Width, tempImg.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    using (Graphics gTemp = Graphics.FromImage(bmp))
                    {
                        gTemp.DrawImage(tempImg, 0, 0);
                    }
                }
            }

            // 现在可以安全地在 bmp 上创建 Graphics 对象
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // 设置高品质绘图参数
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                foreach (var p in points)
                {
                    // --- 外参变换 (Extrinsics [R|T]) ---
                    // 将 Lidar 坐标系转换到 Camera 坐标系
                    float X_c = -p.Y;
                    float Y_c = -p.Z;
                    float Z_c = p.X;

                    // 剔除位于相机背后的点云
                    if (Z_c <= 0.2f) continue;

                    // --- 内参投影 (Intrinsics K) ---
                    int u = (int)(Fx * (X_c / Z_c) + Cx);
                    int v = (int)(Fy * (Y_c / Z_c) + Cy);

                    // 判断是否在画面内并绘制
                    if (u >= 0 && u < bmp.Width && v >= 0 && v < bmp.Height)
                    {
                        Color ptColor = GetColorByDepth(p.Depth, maxDepth);
                        using (SolidBrush brush = new SolidBrush(ptColor))
                        {
                            // 绘制点云，圆点大小设为 3x3
                            g.FillEllipse(brush, u - 1, v - 1, 3, 3);
                        }
                    }
                }
            }
            return bmp;
        }

        /// <summary>
        /// 深度伪彩色映射 (Jet Colormap)
        /// </summary>
        private Color GetColorByDepth(float depth, float maxDepth)
        {
            float v = Math.Max(0, Math.Min(depth / maxDepth, 1.0f));
            int r = 0, g = 0, b = 0;

            if (v < 0.5f)
            {
                b = (int)(255 * (1.0f - 2 * v));
                g = (int)(255 * 2 * v);
            }
            else
            {
                g = (int)(255 * (1.0f - 2 * (v - 0.5f)));
                r = (int)(255 * 2 * (v - 0.5f));
            }

            return Color.FromArgb(255, r, g, b);
        }
    }
}