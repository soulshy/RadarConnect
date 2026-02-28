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

                string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = false
                };

                using (Process process = Process.Start(psi))
                {
                    string ffmpegLog = process.StandardError.ReadToEnd();

                    process.WaitForExit(5000);

                    if (!File.Exists(outImagePath))
                    {
                        throw new Exception($"FFmpeg 未生成图片！\n偏移时间: {offset.TotalSeconds}秒\nFFmpeg底层日志: {ffmpegLog}");
                    }
                }

                return outImagePath;
            });
        }
    }
}
