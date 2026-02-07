using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
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
        private string _connString = "Server=localhost;Database=radardata;Uid=root;Pwd=123456;AllowLoadLocalInfile=True;";
        private ConcurrentQueue<PointData> _bufferQueue = new ConcurrentQueue<PointData>();
        private bool _isSaving = false;
        private Thread _saveThread;
        private const int BATCH_SIZE = 10000;

        public void StartSaving()
        {
            if (_isSaving) return;
            _isSaving = true;
            _saveThread = new Thread(SaveLoop) { IsBackground = true };
            _saveThread.Start();
        }

        public void StopSaving() => _isSaving = false;

        public void EnqueuePoint(DateTime time, int x_mm, int y_mm, int z_mm, float depth_m, int reflex, int tag)
        {
            if (!_isSaving) return;
            _bufferQueue.Enqueue(new PointData
            {
                ExactTime = time, // 这里传入的通常已经是 UTC 
                X = x_mm / 1000.0f,
                Y = y_mm / 1000.0f,
                Z = z_mm / 1000.0f,
                Depth = depth_m,
                Reflectivity = (byte)reflex,
                Tag = (byte)tag
            });
        }

        // =============================================================
        // 查询方法：自动处理时区，支持任意时间段查询
        // =============================================================
        public List<PointData> GetPointsInRange(DateTime localStartTime, double durationSeconds)
        {
            List<PointData> points = new List<PointData>();

            // 1. 计算查询的时间窗口 (本地时间)
            DateTime localEndTime = localStartTime.AddSeconds(durationSeconds);

            // 2. 转换为 UTC 时间 (因为数据库存的是 UTC)
            // .ToUniversalTime() 会自动减去本地时区偏移
            DateTime utcStart = localStartTime.ToUniversalTime();
            DateTime utcEnd = localEndTime.ToUniversalTime();

            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                try
                {
                    conn.Open();
                    // 3. 执行查询
                    string sql = "SELECT x, y, z, depth, reflectivity, tag, collect_time FROM point_cloud " +
                                 "WHERE collect_time BETWEEN @start AND @end ORDER BY collect_time ASC";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@start", utcStart);
                        cmd.Parameters.AddWithValue("@end", utcEnd);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 读取并还原为 PointData
                                float x = reader.GetFloat("x");
                                float y = reader.GetFloat("y");
                                float z = reader.GetFloat("z");
                                float depth = reader.GetFloat("depth");
                                byte refl = reader.GetByte("reflectivity");
                                byte tag = reader.GetByte("tag");

                                // 读出来的 collect_time 是 UTC
                                DateTime dbTimeUtc = reader.GetDateTime("collect_time");
                                // 转换回本地时间，方便调试或显示
                                DateTime dbTimeLocal = dbTimeUtc.ToLocalTime();

                                points.Add(new PointData
                                {
                                    X = x,
                                    Y = y,
                                    Z = z,
                                    Depth = depth,
                                    Reflectivity = refl,
                                    Tag = tag,
                                    ExactTime = dbTimeLocal
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Query Error: " + ex.Message);
                }
            }
            return points;
        }

        // 后台保存逻辑
        private void SaveLoop()
        {
            List<PointData> batch = new List<PointData>(BATCH_SIZE);
            while (_isSaving || !_bufferQueue.IsEmpty)
            {
                batch.Clear();
                while (batch.Count < BATCH_SIZE && _bufferQueue.TryDequeue(out var p)) batch.Add(p);

                if (batch.Count > 0)
                {
                    string tempFile = Path.GetTempFileName();
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(tempFile, false, Encoding.UTF8))
                        {
                            foreach (var p in batch)
                            {
                                // 存入 CSV 时使用 UTC 时间格式化
                                sw.WriteLine($"{p.ExactTime.ToString("yyyy-MM-dd HH:mm:ss.fff")},{p.X},{p.Y},{p.Z},{p.Reflectivity},{p.Tag},{p.Depth}");
                            }
                        }
                        BulkLoadFromFile(tempFile);
                    }
                    catch { }
                    finally { try { File.Delete(tempFile); } catch { } }
                }
                else Thread.Sleep(10);
            }
        }

        private void BulkLoadFromFile(string filePath)
        {
            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                conn.Open();
                var bulk = new MySqlBulkLoader(conn)
                {
                    TableName = "point_cloud",
                    FieldTerminator = ",",
                    LineTerminator = "\n",
                    FileName = filePath,
                    Local = true
                };
                bulk.Columns.AddRange(new[] { "collect_time", "x", "y", "z","reflectivity", "tag","depth"});
                bulk.Load();
            }
        }
    }
}