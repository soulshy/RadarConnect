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

        //用于在线程真正结束时通知主界面
        public event Action OnSaveFinished;

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
                ExactTime = time,
                X = x_mm / 1000.0f,
                Y = y_mm / 1000.0f,
                Z = z_mm / 1000.0f,
                Depth = depth_m,
                Reflectivity = (byte)reflex,
                Tag = (byte)tag
            });
        }

        public List<PointData> GetPointsInRange(DateTime localStartTime, double durationSeconds)
        {
            List<PointData> points = new List<PointData>();

            DateTime localEndTime = localStartTime.AddSeconds(durationSeconds);
            DateTime utcStart = localStartTime.ToUniversalTime();
            DateTime utcEnd = localEndTime.ToUniversalTime();

            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                try
                {
                    conn.Open();
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
                                float x = reader.GetFloat("x");
                                float y = reader.GetFloat("y");
                                float z = reader.GetFloat("z");
                                float depth = reader.GetFloat("depth");
                                byte refl = reader.GetByte("reflectivity");
                                byte tag = reader.GetByte("tag");

                                DateTime dbTimeUtc = reader.GetDateTime("collect_time");
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
            try
            {
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
            finally
            {
                // 无论是否发生异常，在退出前彻底停止保存并通知事件
                StopSaving();
                OnSaveFinished?.Invoke();
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
                bulk.Columns.AddRange(new[] { "collect_time", "x", "y", "z", "reflectivity", "tag", "depth" });
                bulk.Load();
            }
        }
    }
}