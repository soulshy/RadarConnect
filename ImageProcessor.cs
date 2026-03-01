using System;
using System.IO;
using System.Threading.Tasks;
using OpenCvSharp; // 需通过 NuGet 安装 OpenCvSharp4 和 OpenCvSharp4.runtime.win

namespace RadarConnect
{
    /// <summary>
    /// 图像预处理器：负责对抽帧图像进行增强、去噪及格式转换
    /// </summary>
    public class ImageProcessor
    {
        /// <summary>
        /// 异步处理图像
        /// </summary>
        /// <param name="sourceImagePath">原始图片路径</param>
        /// <param name="brightness">亮度增量 (-255 到 255)</param>
        /// <param name="contrast">对比度倍数 (1.0 为原始)</param>
        /// <param name="enableGray">是否转为灰阶（灰暗背景能让彩色点云更醒目）</param>
        /// <returns>处理后的图片路径</returns>
        public async Task<string> ProcessImageAsync(string sourceImagePath, double brightness = 15, double contrast = 1.2, bool enableGray = false)
        {
            return await Task.Run(() =>
            {
                // 创建临时目录
                string tempDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FusionTemp");
                if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);

                string outImagePath = Path.Combine(tempDir, $"processed_{DateTime.Now.Ticks}.jpg");

                // 1. 读取图像
                using (Mat src = new Mat(sourceImagePath, ImreadModes.Color))
                using (Mat dst = new Mat())
                {
                    if (src.Empty()) throw new Exception("OpenCV 无法读取源图像文件。");

                    // 2. 调整对比度和亮度
                    src.ConvertTo(dst, MatType.CV_8UC3, contrast, brightness);

                    // 3. 去噪处理
                    // 高斯滤波：平滑图像，减轻视频压缩产生的噪点
                    Cv2.GaussianBlur(dst, dst, new Size(3, 3), 0);

                    // 中值滤波：进一步去除孤立的杂点（如椒盐噪声）
                    Cv2.MedianBlur(dst, dst, 3);

                    // 4.直方图均衡化 - 自动平衡明暗（如果环境光线复杂建议开启）
                    ApplyAutoContrast(dst);

                    // 5.灰度化处理
                    if (enableGray)
                    {
                        Cv2.CvtColor(dst, dst, ColorConversionCodes.BGR2GRAY);
                    }
                    // 6. 保存处理后的图像
                    dst.SaveImage(outImagePath);
                }

                return outImagePath;
            });
        }

        /// <summary>
        /// 自动对比度增强（针对彩色图像）
        /// </summary>
        private void ApplyAutoContrast(Mat img)
        {
            using (Mat ycrcb = new Mat())
            {
                Cv2.CvtColor(img, ycrcb, ColorConversionCodes.BGR2YCrCb);
                Mat[] channels = Cv2.Split(ycrcb);
                Cv2.EqualizeHist(channels[0], channels[0]); // 仅均衡亮度通道
                Cv2.Merge(channels, ycrcb);
                Cv2.CvtColor(ycrcb, img, ColorConversionCodes.YCrCb2BGR);
            }
        }
    }
}