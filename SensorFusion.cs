using System;
using System.Collections.Generic;
using OpenCvSharp;

namespace RadarConnect
{
    public class SensorFusion
    {
        // --- 1. 相机内参 (Camera Intrinsics) ---
        // 焦距 (Focal Length)
        public float Fx { get; set; } = 1878.4f;
        public float Fy { get; set; } = 1878.0f;
        // 主点 (Principal Point)
        public float Cx { get; set; } = 980.4468f;
        public float Cy { get; set; } = 550.5467f;

        // --- 2. 相机畸变系数 (Distortion Coefficients) ---
        // k1, k2, p1, p2, k3 (图片中未提供 k3，默认为 0)
        public double[] DistCoeffs { get; set; } = new double[] { -0.0889, -0.0206, 0.0, 0.0, 0.0 };

        // --- 3. 真实的相机外参 (Camera Extrinsics) ---
        // 3x3 旋转矩阵 (Rotation Matrix) 代表雷达坐标系相对于相机坐标系的旋转姿态
        public double[,] R { get; set; } = new double[3, 3] {
            { 0.0277, -0.9996,  0.0039 },
            { -0.0390, -0.0050, -0.9992 },
            { 0.9989,  0.0275, -0.0391 }
        };

        // 3x1 平移向量 (Translation Vector) 
        public double[] T { get; set; } = new double[3] { 0.1269, 0.1474, 0.0530 };

        private readonly Scalar[] _colorLut = new Scalar[256];

        public SensorFusion()
        {
            InitializeColorLut();
        }

        private void InitializeColorLut()
        {
            for (int i = 0; i < 256; i++)
            {
                float v = i / 255.0f;
                int r = 0, g = 0, b = 0;
                if (v < 0.5f) { b = (int)(255 * (1.0f - 2 * v)); g = (int)(255 * 2 * v); }
                else { g = (int)(255 * (1.0f - 2 * (v - 0.5f))); r = (int)(255 * 2 * (v - 0.5f)); }
                _colorLut[i] = new Scalar(b, g, r);
            }
        }

        /// <summary>
        ///  OpenCV Mat 对象上投影 3D 点云
        /// </summary>
        /// <summary>
        ///  OpenCV Mat 对象上投影 3D 点云 (动态深度着色)
        /// </summary>
        public Mat ProjectPointCloudToImage(Mat img, List<PointData> points)
        {
            double k1 = DistCoeffs[0], k2 = DistCoeffs[1], p1 = DistCoeffs[2], p2 = DistCoeffs[3], k3 = DistCoeffs[4];

            // ==============================================================
            // 1. 获取当前帧真实的深度极值，用于动态颜色映射，彻底解决全蓝问题
            // ==============================================================
            float minDepth = float.MaxValue;
            float maxDepth = float.MinValue;
            foreach (var p in points)
            {
                if (p.Depth < minDepth) minDepth = p.Depth;
                if (p.Depth > maxDepth) maxDepth = p.Depth;
            }
            // 防止画面里只有一个点导致除零异常
            if (maxDepth - minDepth < 0.1f) maxDepth = minDepth + 1.0f;

            foreach (var p in points)
            {
                // ==========================================
                // 真正的外参矩阵运算 ( P_c = R * P_l + T )
                // ==========================================
                double X_c = R[0, 0] * p.X + R[0, 1] * p.Y + R[0, 2] * p.Z + T[0];
                double Y_c = R[1, 0] * p.X + R[1, 1] * p.Y + R[1, 2] * p.Z + T[1];
                double Z_c = R[2, 0] * p.X + R[2, 1] * p.Y + R[2, 2] * p.Z + T[2];

                // 剔除位于相机背后的点云
                if (Z_c <= 0.2f) continue;

                // --- 归一化相机平面 ---
                double x = X_c / Z_c;
                double y = Y_c / Z_c;

                // --- 畸变校正 (Distortion) ---
                double r2 = x * x + y * y;
                double r4 = r2 * r2;
                double r6 = r2 * r4;

                double x_distorted = x * (1 + k1 * r2 + k2 * r4 + k3 * r6) + 2 * p1 * x * y + p2 * (r2 + 2 * x * x);
                double y_distorted = y * (1 + k1 * r2 + k2 * r4 + k3 * r6) + p1 * (r2 + 2 * y * y) + 2 * p2 * x * y;

                // --- 内参投影映射到像素坐标系 ---
                int u = (int)(Fx * x_distorted + Cx);
                int v = (int)(Fy * y_distorted + Cy);

                // 判断是否在画面内并绘制
                if (u >= 0 && u < img.Width && v >= 0 && v < img.Height)
                {
                    // ================== Jet Colormap 彩色映射 ==================
                    // 将深度归一化到 0.0 ~ 1.0 之间
                    float ratio = (p.Depth - minDepth) / (maxDepth - minDepth);
                    ratio = Math.Max(0f, Math.Min(1f, ratio));

                    // 近红远蓝 公式计算
                    float r = ClampColor(1.5f - Math.Abs(4.0f * (1.0f - ratio) - 3.0f));
                    float g = ClampColor(1.5f - Math.Abs(4.0f * (1.0f - ratio) - 2.0f));
                    float b = ClampColor(1.5f - Math.Abs(4.0f * (1.0f - ratio) - 1.0f));

                    // OpenCV中颜色结构体 Scalar 的顺序是 (Blue, Green, Red)
                    Scalar ptColor = new Scalar(b * 255, g * 255, r * 255);

                    Cv2.Circle(img, new OpenCvSharp.Point(u, v), 1, ptColor, -1, LineTypes.AntiAlias);
                }
            }
            return img;
        }

        // 辅助函数：将颜色限制在合法范围内
        private float ClampColor(float val)
        {
            if (val < 0f) return 0f;
            if (val > 1f) return 1f;
            return val;
        }
    }
}