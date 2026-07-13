using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading;
using K4os.Compression.LZ4;
using MySql.Data.MySqlClient;

namespace RadarConnect
{
    public struct PointData
    {
        public DateTime ExactTime;
        public float X;
        public float Y;
        public float Z;
        public float Depth;
        public byte Reflectivity;
        public byte Tag;
    }

    public class DatabaseManager
    {
        private const int BATCH_SIZE = 50000;
        private const int CHUNK_MAGIC = 0x50434C31; // "PCL1"

        private readonly string _connString =
            "Server=localhost;Database=radardata;Uid=root;Pwd=123456;" +
            "Pooling=True;MinimumPoolSize=1;MaximumPoolSize=10;";

        private readonly ConcurrentQueue<PointData> _bufferQueue =
            new ConcurrentQueue<PointData>();
        private readonly AutoResetEvent _queueSignal = new AutoResetEvent(false);

        private volatile bool _isSaving;
        private Thread _saveThread;

        public event Action OnSaveFinished;
        public event Action<string> OnError;

        public void StartSaving()
        {
            if (_isSaving) return;

            _isSaving = true;
            _saveThread = new Thread(SaveLoop)
            {
                IsBackground = true,
                Name = "PointCloudChunkWriter"
            };
            _saveThread.Start();
        }

        public void StopSaving()
        {
            _isSaving = false;
            _queueSignal.Set();
        }

        public void EnqueuePoint(
            DateTime time,
            int x_mm,
            int y_mm,
            int z_mm,
            float depth_m,
            int reflex,
            int tag)
        {
            if (!_isSaving) return;

            _bufferQueue.Enqueue(new PointData
            {
                ExactTime = time,
                X = x_mm / 1000.0f,
                Y = y_mm / 1000.0f,
                Z = z_mm / 1000.0f,
                Depth = depth_m,
                Reflectivity = (byte)reflex,
                Tag = (byte)tag
            });
            _queueSignal.Set();
        }

        public List<PointData> GetPointsInRange(
            DateTime localStartTime,
            double durationSeconds)
        {
            DateTime localEndTime = localStartTime.AddSeconds(durationSeconds);
            DateTime utcStart = localStartTime.ToUniversalTime();
            DateTime utcEnd = localEndTime.ToUniversalTime();

            try
            {
                List<PointData> points = ReadChunks(utcStart, utcEnd);
                if (points.Count > 0)
                    return points;

                // 兼容升级前逐点保存的历史数据。
                return ReadLegacyRows(utcStart, utcEnd);
            }
            catch (Exception ex)
            {
                ReportError("查询点云失败: " + ex.Message);
                return new List<PointData>();
            }
        }

        private void SaveLoop()
        {
            List<PointData> batch = new List<PointData>(BATCH_SIZE);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connString))
                {
                    connection.Open();
                    EnsureChunkTable(connection);

                    while (_isSaving || !_bufferQueue.IsEmpty)
                    {
                        batch.Clear();
                        PointData point;
                        while (batch.Count < BATCH_SIZE &&
                               _bufferQueue.TryDequeue(out point))
                        {
                            batch.Add(point);
                        }

                        if (batch.Count == 0)
                        {
                            _queueSignal.WaitOne(50);
                            continue;
                        }

                        InsertChunkWithRetry(connection, batch);
                    }
                }
            }
            catch (Exception ex)
            {
                ReportError("点云写入线程异常终止: " + ex.Message);
            }
            finally
            {
                _isSaving = false;
                Action handler = OnSaveFinished;
                if (handler != null) handler();
            }
        }

        private void InsertChunkWithRetry(
            MySqlConnection connection,
            List<PointData> batch)
        {
            byte[] payload = SerializeAndCompress(batch);
            DateTime startUtc = ToUtc(batch[0].ExactTime);
            DateTime endUtc = startUtc;

            for (int i = 1; i < batch.Count; i++)
            {
                DateTime time = ToUtc(batch[i].ExactTime);
                if (time < startUtc) startUtc = time;
                if (time > endUtc) endUtc = time;
            }

            Exception lastError = null;
            for (int attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    const string sql =
                        "INSERT INTO point_cloud_chunks " +
                        "(start_time, end_time, point_count, payload) " +
                        "VALUES (@start, @end, @count, @payload)";

                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.Add("@start", MySqlDbType.DateTime).Value = startUtc;
                        command.Parameters.Add("@end", MySqlDbType.DateTime).Value = endUtc;
                        command.Parameters.Add("@count", MySqlDbType.Int32).Value = batch.Count;
                        command.Parameters.Add("@payload", MySqlDbType.LongBlob).Value = payload;
                        command.ExecuteNonQuery();
                    }
                    return;
                }
                catch (Exception ex)
                {
                    lastError = ex;
                    try { connection.Close(); } catch { }
                    Thread.Sleep(attempt * 200);
                }
            }

            // 三次失败后放回队列，避免整批数据静默丢失。
            foreach (PointData point in batch)
                _bufferQueue.Enqueue(point);

            ReportError(
                "压缩点云块写入失败，数据已放回队列，将继续重试。" +
                (lastError == null ? string.Empty : " 原因: " + lastError.Message));
            Thread.Sleep(1000);
        }

        private List<PointData> ReadChunks(DateTime utcStart, DateTime utcEnd)
        {
            List<PointData> result = new List<PointData>();

            using (MySqlConnection connection = new MySqlConnection(_connString))
            {
                connection.Open();
                EnsureChunkTable(connection);

                const string sql =
                    "SELECT payload FROM point_cloud_chunks " +
                    "WHERE end_time >= @start AND start_time < @end " +
                    "ORDER BY start_time ASC";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@start", MySqlDbType.DateTime).Value = utcStart;
                    command.Parameters.Add("@end", MySqlDbType.DateTime).Value = utcEnd;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            byte[] compressed = (byte[])reader[0];
                            List<PointData> chunk = DecompressAndDeserialize(compressed);
                            foreach (PointData point in chunk)
                            {
                                DateTime pointUtc = ToUtc(point.ExactTime);
                                if (pointUtc >= utcStart && pointUtc < utcEnd)
                                {
                                    PointData localPoint = point;
                                    localPoint.ExactTime = pointUtc.ToLocalTime();
                                    result.Add(localPoint);
                                }
                            }
                        }
                    }
                }
            }

            result.Sort((left, right) => left.ExactTime.CompareTo(right.ExactTime));
            return result;
        }

        private List<PointData> ReadLegacyRows(DateTime utcStart, DateTime utcEnd)
        {
            List<PointData> points = new List<PointData>();

            using (MySqlConnection connection = new MySqlConnection(_connString))
            {
                connection.Open();
                const string sql =
                    "SELECT x, y, z, depth, reflectivity, tag, collect_time " +
                    "FROM point_cloud " +
                    "WHERE collect_time >= @start AND collect_time < @end " +
                    "ORDER BY collect_time ASC";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@start", MySqlDbType.DateTime).Value = utcStart;
                    command.Parameters.Add("@end", MySqlDbType.DateTime).Value = utcEnd;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime dbUtc = DateTime.SpecifyKind(
                                reader.GetDateTime("collect_time"),
                                DateTimeKind.Utc);

                            points.Add(new PointData
                            {
                                X = reader.GetFloat("x"),
                                Y = reader.GetFloat("y"),
                                Z = reader.GetFloat("z"),
                                Depth = reader.GetFloat("depth"),
                                Reflectivity = reader.GetByte("reflectivity"),
                                Tag = reader.GetByte("tag"),
                                ExactTime = dbUtc.ToLocalTime()
                            });
                        }
                    }
                }
            }

            return points;
        }

        private static byte[] SerializeAndCompress(List<PointData> points)
        {
            using (MemoryStream stream = new MemoryStream(points.Count * 26 + 8))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(CHUNK_MAGIC);
                writer.Write(points.Count);

                foreach (PointData point in points)
                {
                    writer.Write(ToUtc(point.ExactTime).Ticks);
                    writer.Write(point.X);
                    writer.Write(point.Y);
                    writer.Write(point.Z);
                    writer.Write(point.Depth);
                    writer.Write(point.Reflectivity);
                    writer.Write(point.Tag);
                }

                writer.Flush();
                return LZ4Pickler.Pickle(stream.ToArray());
            }
        }

        private static List<PointData> DecompressAndDeserialize(byte[] compressed)
        {
            byte[] raw = LZ4Pickler.Unpickle(compressed);

            using (MemoryStream stream = new MemoryStream(raw, false))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                if (reader.ReadInt32() != CHUNK_MAGIC)
                    throw new InvalidDataException("未知的点云块格式。");

                int count = reader.ReadInt32();
                if (count < 0 || count > 10000000)
                    throw new InvalidDataException("点云块数量无效。");

                List<PointData> points = new List<PointData>(count);
                for (int i = 0; i < count; i++)
                {
                    points.Add(new PointData
                    {
                        ExactTime = new DateTime(reader.ReadInt64(), DateTimeKind.Utc),
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle(),
                        Depth = reader.ReadSingle(),
                        Reflectivity = reader.ReadByte(),
                        Tag = reader.ReadByte()
                    });
                }
                return points;
            }
        }

        private static void EnsureChunkTable(MySqlConnection connection)
        {
            const string sql = @"
CREATE TABLE IF NOT EXISTS point_cloud_chunks (
    id BIGINT NOT NULL AUTO_INCREMENT,
    start_time DATETIME(6) NOT NULL,
    end_time DATETIME(6) NOT NULL,
    point_count INT NOT NULL,
    payload LONGBLOB NOT NULL,
    PRIMARY KEY (id),
    INDEX idx_point_cloud_chunks_time (start_time, end_time)
) ENGINE=InnoDB;";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
                command.ExecuteNonQuery();
        }

        private static DateTime ToUtc(DateTime time)
        {
            if (time.Kind == DateTimeKind.Utc) return time;
            if (time.Kind == DateTimeKind.Unspecified)
                time = DateTime.SpecifyKind(time, DateTimeKind.Local);
            return time.ToUniversalTime();
        }

        private void ReportError(string message)
        {
            Debug.WriteLine(message);
            Action<string> handler = OnError;
            if (handler != null) handler(message);
        }
    }
}
