using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using OpenCvSharp;

namespace RadarConnect
{
    /// <summary>
    /// Writes samples in the format consumed by Diffusion/finetune_avia.py.
    /// Images are undistorted PNG files and point clouds are NumPy float32 Nx4 arrays.
    /// </summary>
    public sealed class AviaDatasetExporter
    {
        public const int TargetPointCount = 24000;
        public const int MinimumPointCount = 4096;

        private const int ExpectedImageWidth = 1920;
        private const int ExpectedImageHeight = 1080;

        private const double Fx = 1878.4;
        private const double Fy = 1878.0;
        private const double Cx = 980.4468;
        private const double Cy = 550.5467;

        private static readonly double[] Distortion =
            { -0.0889, -0.0206, 0.0, 0.0, 0.0 };

        private static readonly object ManifestLock = new object();
        private static readonly DateTime UnixEpochUtc =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly string _root;
        private readonly string _split;
        private readonly string _session;
        private readonly string _imageDirectory;
        private readonly string _pointDirectory;
        private readonly string _manifestPath;
        private int _nextFrameIndex;

        public AviaDatasetExporter(string root, string split, string session)
        {
            if (string.IsNullOrWhiteSpace(root))
                throw new ArgumentException("数据集根目录不能为空。", nameof(root));

            _split = NormalizeSplit(split);
            _session = NormalizeSession(session);
            _root = Path.GetFullPath(root);
            _imageDirectory = Path.Combine(_root, _split, _session, "images");
            _pointDirectory = Path.Combine(_root, _split, _session, "points");
            _manifestPath = Path.Combine(_root, "manifest.jsonl");

            Directory.CreateDirectory(_imageDirectory);
            Directory.CreateDirectory(_pointDirectory);
            EnsureCalibrationFile();
            _nextFrameIndex = FindNextFrameIndex();
        }

        public string Root => _root;
        public string Split => _split;
        public string Session => _session;

        public AviaExportResult Export(
            byte[] encodedImage,
            IReadOnlyList<PointData> sourcePoints,
            DateTime imageTimeUtc,
            DateTime cloudTimeUtc,
            double syncDeltaMs)
        {
            if (encodedImage == null || encodedImage.Length == 0)
                throw new ArgumentException("图像数据为空。", nameof(encodedImage));
            if (sourcePoints == null)
                throw new ArgumentNullException(nameof(sourcePoints));

            List<PointData> validPoints = sourcePoints
                .Where(IsValidPoint)
                .ToList();
            if (validPoints.Count < MinimumPointCount)
            {
                throw new InvalidOperationException(
                    $"有效点数 {validPoints.Count} 少于模型最低要求 {MinimumPointCount}，样本已丢弃。");
            }

            int frameIndex = _nextFrameIndex;
            string frameName = frameIndex.ToString("D6", CultureInfo.InvariantCulture);
            string imagePath = Path.Combine(_imageDirectory, frameName + ".png");
            string pointPath = Path.Combine(_pointDirectory, frameName + ".npy");

            if (File.Exists(imagePath) || File.Exists(pointPath))
                throw new IOException("目标样本编号已存在，请重新开始数据集场次。");

            try
            {
                WriteUndistortedPng(encodedImage, imagePath);
                PointData[] sampled = SamplePoints(
                    validPoints,
                    TargetPointCount,
                    unchecked((int)imageTimeUtc.Ticks));
                WriteNpyFloat32Nx4(pointPath, sampled);

                string relativeImage = ToManifestPath(
                    Path.Combine(_split, _session, "images", frameName + ".png"));
                string relativePoints = ToManifestPath(
                    Path.Combine(_split, _session, "points", frameName + ".npy"));
                string id = _session + "_" + frameName;
                string manifestLine = BuildManifestLine(
                    id,
                    relativeImage,
                    relativePoints,
                    sampled.Length,
                    imageTimeUtc,
                    cloudTimeUtc,
                    syncDeltaMs);

                lock (ManifestLock)
                {
                    File.AppendAllText(
                        _manifestPath,
                        manifestLine + Environment.NewLine,
                        new UTF8Encoding(false));
                }

                _nextFrameIndex++;
                return new AviaExportResult
                {
                    Id = id,
                    FrameIndex = frameIndex,
                    ImagePath = imagePath,
                    PointPath = pointPath,
                    PointCount = sampled.Length,
                    SyncDeltaMs = syncDeltaMs
                };
            }
            catch
            {
                TryDelete(imagePath);
                TryDelete(pointPath);
                throw;
            }
        }

        private void EnsureCalibrationFile()
        {
            string calibrationPath = Path.Combine(_root, "calibration.json");
            string expected = BuildCalibrationJson();
            if (File.Exists(calibrationPath))
            {
                string existing = File.ReadAllText(calibrationPath, Encoding.UTF8);
                if (!NormalizeNewLines(existing).Equals(
                        NormalizeNewLines(expected),
                        StringComparison.Ordinal))
                {
                    throw new InvalidOperationException(
                        "数据集中的 calibration.json 与当前 RadarConnect 标定参数不一致。" +
                        "请更换数据集目录，避免混用不同标定数据。");
                }
                return;
            }

            Directory.CreateDirectory(_root);
            File.WriteAllText(calibrationPath, expected, new UTF8Encoding(false));
        }

        private int FindNextFrameIndex()
        {
            int maximum = -1;
            foreach (string path in Directory.EnumerateFiles(_imageDirectory, "*.png"))
            {
                int value;
                if (int.TryParse(
                        Path.GetFileNameWithoutExtension(path),
                        NumberStyles.None,
                        CultureInfo.InvariantCulture,
                        out value))
                {
                    maximum = Math.Max(maximum, value);
                }
            }
            return maximum + 1;
        }

        private static void WriteUndistortedPng(byte[] encodedImage, string outputPath)
        {
            using (Mat source = Cv2.ImDecode(encodedImage, ImreadModes.Color))
            {
                if (source.Empty())
                    throw new InvalidDataException("相机返回的数据无法解码为图像。");
                if (source.Width != ExpectedImageWidth || source.Height != ExpectedImageHeight)
                {
                    throw new InvalidDataException(
                        $"相机图像为 {source.Width}x{source.Height}，但标定参数要求 " +
                        $"{ExpectedImageWidth}x{ExpectedImageHeight}。");
                }

                using (Mat cameraMatrix = new Mat(3, 3, MatType.CV_64FC1))
                using (Mat distortion = new Mat(1, 5, MatType.CV_64FC1))
                using (Mat undistorted = new Mat())
                {
                    cameraMatrix.Set(0, 0, Fx);
                    cameraMatrix.Set(0, 1, 0.0);
                    cameraMatrix.Set(0, 2, Cx);
                    cameraMatrix.Set(1, 0, 0.0);
                    cameraMatrix.Set(1, 1, Fy);
                    cameraMatrix.Set(1, 2, Cy);
                    cameraMatrix.Set(2, 0, 0.0);
                    cameraMatrix.Set(2, 1, 0.0);
                    cameraMatrix.Set(2, 2, 1.0);
                    for (int i = 0; i < Distortion.Length; i++)
                        distortion.Set(0, i, Distortion[i]);

                    // Keep the same camera matrix, so calibration.json remains exact.
                    Cv2.Undistort(source, undistorted, cameraMatrix, distortion, cameraMatrix);
                    if (!Cv2.ImWrite(outputPath, undistorted))
                        throw new IOException("去畸变 PNG 保存失败。");
                }
            }
        }

        private static PointData[] SamplePoints(
            IReadOnlyList<PointData> points,
            int targetCount,
            int seed)
        {
            Random random = new Random(seed);
            int outputCount = Math.Min(points.Count, targetCount);
            PointData[] output = new PointData[outputCount];

            for (int i = 0; i < outputCount; i++)
                output[i] = points[i];

            if (points.Count > targetCount)
            {
                // Reservoir sampling avoids copying a potentially large 100 ms point window.
                for (int i = targetCount; i < points.Count; i++)
                {
                    int replacement = random.Next(i + 1);
                    if (replacement < targetCount)
                        output[replacement] = points[i];
                }
            }

            return output;
        }

        private static void WriteNpyFloat32Nx4(string path, IReadOnlyList<PointData> points)
        {
            string dictionary = string.Format(
                CultureInfo.InvariantCulture,
                "{{'descr': '<f4', 'fortran_order': False, 'shape': ({0}, 4), }}",
                points.Count);

            // NPY v1.0 requires the complete preamble + header to be divisible by 16.
            int preambleLength = 10;
            int padding = 16 - ((preambleLength + dictionary.Length + 1) % 16);
            if (padding == 16) padding = 0;
            string header = dictionary + new string(' ', padding) + "\n";
            byte[] headerBytes = Encoding.ASCII.GetBytes(header);
            if (headerBytes.Length > ushort.MaxValue)
                throw new InvalidDataException("NPY 头部过长。");

            using (FileStream stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII))
            {
                writer.Write((byte)0x93);
                writer.Write(Encoding.ASCII.GetBytes("NUMPY"));
                writer.Write((byte)1);
                writer.Write((byte)0);
                writer.Write((ushort)headerBytes.Length);
                writer.Write(headerBytes);

                foreach (PointData point in points)
                {
                    writer.Write(point.X);
                    writer.Write(point.Y);
                    writer.Write(point.Z);
                    writer.Write((float)point.Reflectivity);
                }
            }
        }

        private string BuildManifestLine(
            string id,
            string relativeImage,
            string relativePoints,
            int pointCount,
            DateTime imageTimeUtc,
            DateTime cloudTimeUtc,
            double syncDeltaMs)
        {
            string body = string.Format(
                CultureInfo.InvariantCulture,
                "{{\"id\":\"{0}\",\"split\":\"{1}\",\"sequence\":\"{2}\"," +
                "\"image\":\"{3}\",\"points\":\"{4}\",\"num_points\":{5}," +
                "\"image_record_timestamp_ns\":{6},\"cloud_record_timestamp_ns\":{7}," +
                "\"sync_delta_ms\":{8:F6}",
                EscapeJson(id),
                EscapeJson(_split),
                EscapeJson(_session),
                EscapeJson(relativeImage),
                EscapeJson(relativePoints),
                pointCount,
                ToUnixNanoseconds(imageTimeUtc),
                ToUnixNanoseconds(cloudTimeUtc),
                syncDeltaMs);
            return body + "}";
        }

        private static string BuildCalibrationJson()
        {
            return @"{
  ""image_size"": [1920, 1080],
  ""images_undistorted"": true,
  ""camera_matrix"": [
    [1878.4, 0.0, 980.4468],
    [0.0, 1878.0, 550.5467],
    [0.0, 0.0, 1.0]
  ],
  ""distortion"": [0.0, 0.0, 0.0, 0.0, 0.0],
  ""original_camera_matrix"": [
    [1878.4, 0.0, 980.4468],
    [0.0, 1878.0, 550.5467],
    [0.0, 0.0, 1.0]
  ],
  ""original_distortion"": [-0.0889, -0.0206, 0.0, 0.0, 0.0],
  ""T_lidar_to_camera"": [
    [0.0277, -0.9996, 0.0039, 0.1269],
    [-0.0390, -0.0050, -0.9992, 0.1474],
    [0.9989, 0.0275, -0.0391, 0.0530],
    [0.0, 0.0, 0.0, 1.0]
  ],
  ""rectification_matrix"": [
    [1.0, 0.0, 0.0, 0.0],
    [0.0, 1.0, 0.0, 0.0],
    [0.0, 0.0, 1.0, 0.0],
    [0.0, 0.0, 0.0, 1.0]
  ],
  ""point_format"": ""NumPy float32 Nx4: x, y, z, intensity"",
  ""sync_basis"": ""HTTP request midpoint and centered 100 ms LiDAR window""
}
";
        }

        private static string NormalizeSplit(string split)
        {
            string value = (split ?? string.Empty).Trim().ToLowerInvariant();
            if (value != "train" && value != "val" && value != "test")
                throw new ArgumentException("数据集划分必须是 train、val 或 test。", nameof(split));
            return value;
        }

        public static string NormalizeSession(string session)
        {
            string value = (session ?? string.Empty).Trim();
            if (value.Length == 0)
                throw new ArgumentException("场次名称不能为空。", nameof(session));

            StringBuilder output = new StringBuilder(value.Length);
            foreach (char character in value)
            {
                if (char.IsLetterOrDigit(character) || character == '_' || character == '-')
                    output.Append(character);
                else
                    output.Append('_');
            }
            return output.ToString();
        }

        private static bool IsValidPoint(PointData point)
        {
            return IsFinite(point.X) && IsFinite(point.Y) && IsFinite(point.Z);
        }

        private static bool IsFinite(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }

        private static long ToUnixNanoseconds(DateTime time)
        {
            DateTime utc = time.Kind == DateTimeKind.Utc ? time : time.ToUniversalTime();
            return checked((utc.Ticks - UnixEpochUtc.Ticks) * 100L);
        }

        private static string ToManifestPath(string path)
        {
            return path.Replace('\\', '/');
        }

        private static string EscapeJson(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static string NormalizeNewLines(string value)
        {
            return value.Replace("\r\n", "\n").Trim();
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);
            }
            catch
            {
            }
        }
    }

    public sealed class AviaExportResult
    {
        public string Id { get; set; }
        public int FrameIndex { get; set; }
        public string ImagePath { get; set; }
        public string PointPath { get; set; }
        public int PointCount { get; set; }
        public double SyncDeltaMs { get; set; }
    }
}
