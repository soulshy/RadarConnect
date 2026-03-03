using System;
using System.Collections.Generic;
using OpenCvSharp;

namespace RadarConnect
{
    public class SensorFusion
    {
        public float Fx { get; set; } = 1200f;
        public float Fy { get; set; } = 1200f;
        public float Cx { get; set; } = 960f;
        public float Cy { get; set; } = 540f;

        // 畸变系数：k1, k2, p1, p2, k3
        public double[] DistCoeffs { get; set; } = new double[] { 0, 0, 0, 0, 0 };

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
        /// 直接在内存中的 OpenCV Mat 对象上投影 3D 点云
        /// </summary>
        public Mat ProjectPointCloudToImage(Mat img, List<PointData> points, float maxDepth = 30.0f)
        {
            double k1 = DistCoeffs[0], k2 = DistCoeffs[1], p1 = DistCoeffs[2], p2 = DistCoeffs[3], k3 = DistCoeffs[4];

            foreach (var p in points)
            {
                float X_c = -p.Y;
                float Y_c = -p.Z;
                float Z_c = p.X;

                if (Z_c <= 0.2f) continue;

                double x = X_c / Z_c;
                double y = Y_c / Z_c;

                double r2 = x * x + y * y;
                double r4 = r2 * r2;
                double r6 = r2 * r4;

                double x_distorted = x * (1 + k1 * r2 + k2 * r4 + k3 * r6) + 2 * p1 * x * y + p2 * (r2 + 2 * x * x);
                double y_distorted = y * (1 + k1 * r2 + k2 * r4 + k3 * r6) + p1 * (r2 + 2 * y * y) + 2 * p2 * x * y;

                int u = (int)(Fx * x_distorted + Cx);
                int v = (int)(Fy * y_distorted + Cy);

                if (u >= 0 && u < img.Width && v >= 0 && v < img.Height)
                {
                    float depthRatio = Math.Max(0, Math.Min(p.Depth / maxDepth, 1.0f));
                    int lutIndex = (int)(depthRatio * 255);
                    Scalar ptColor = _colorLut[lutIndex];
                    Cv2.Circle(img, new OpenCvSharp.Point(u, v), 1, ptColor, -1, LineTypes.AntiAlias);
                }
            }
            return img;
        }
    }
}