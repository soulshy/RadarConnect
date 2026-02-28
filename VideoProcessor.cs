using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RadarConnect
{
    /// <summary>
    /// 视频处理器：负责解析视频时间和调用 FFmpeg 进行精确抽帧
    /// </summary>
    public class VideoProcessor
    {
        /// <summary>
        /// 解析视频文件名以获取其开始录制的时间
        /// </summary>
        public DateTime ParseVideoStartTime(string fileName)
        {
            Match match = Regex.Match(fileName, @"Record_(\d{8}_\d{6})");
            if (match.Success)
            {
                return DateTime.ParseExact(match.Groups[1].Value, "yyyyMMdd_HHmmss", null);
            }
            throw new Exception("无法从视频文件名中解析出录制时间。文件名格式应包含 Record_yyyyMMdd_HHmmss");
        }

        /// <summary>
        /// 异步调用 FFmpeg 抽取指定时间偏移量的一帧图像
        /// </summary>
        public async Task<string> ExtractFrameAsync(string videoPath, TimeSpan offset)
        {
            return await Task.Run(() =>
            {
                string tempDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FusionTemp");
                if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);

                string outImagePath = Path.Combine(tempDir, $"frame_{DateTime.Now.Ticks}.jpg");

                // FFmpeg 参数: -ss 定位时间, -i 输入, -vframes 1 提取一帧, -q:v 2 高画质输出
                string arguments = $"-ss {offset.TotalSeconds:F3} -i \"{videoPath}\" -vframes 1 -q:v 2 -y \"{outImagePath}\"";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "ffmpeg", // 确保 ffmpeg.exe 在程序目录或环境变量中
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit(5000); // 超时保护
                }

                return outImagePath;
            });
        }
    }
}
}