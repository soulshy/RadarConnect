using System;
using System.IO;
using System.Threading.Tasks;
using OpenCvSharp;

namespace RadarConnect
{
    /// <summary>
    /// 图像预处理器：负责对抽帧图像进行增强、去噪，直接在内存中输出 Mat
    /// </summary>
    public class ImageProcessor
    {
        public async Task<Mat> ProcessImageAsync(string sourceImagePath, double brightness = 15, double contrast = 1.2, bool enableGray = false)
        {
            return await Task.Run(() =>
            {
                // 1. 读取图像
                Mat src = new Mat(sourceImagePath, ImreadModes.Color);
                if (src.Empty()) throw new Exception("OpenCV 无法读取源图像文件。");

                Mat dst = new Mat();
                // 2. 调整对比度和亮度
                src.ConvertTo(dst, MatType.CV_8UC3, contrast, brightness);

                // 3. 去噪处理
                Cv2.GaussianBlur(dst, dst, new Size(3, 3), 0);
                Cv2.MedianBlur(dst, dst, 3);

                // 4. 直方图均衡化
                ApplyAutoContrast(dst);

                // 5. 灰度化处理
                if (enableGray)
                {
                    Cv2.CvtColor(dst, dst, ColorConversionCodes.BGR2GRAY);
                    //将单通道灰度图转回三通道 BGR，否则后续无法绘制彩色点云
                    Cv2.CvtColor(dst, dst, ColorConversionCodes.GRAY2BGR);
                }

                src.Dispose();
                return dst; // 直接返回内存中的 Mat 对象
            });
        }

        private void ApplyAutoContrast(Mat img)
        {
            using (Mat ycrcb = new Mat())
            {
                Cv2.CvtColor(img, ycrcb, ColorConversionCodes.BGR2YCrCb);
                Mat[] channels = Cv2.Split(ycrcb);
                Cv2.EqualizeHist(channels[0], channels[0]);
                Cv2.Merge(channels, ycrcb);
                Cv2.CvtColor(ycrcb, img, ColorConversionCodes.YCrCb2BGR);
                foreach (var c in channels) c.Dispose();
            }
        }
    }
}