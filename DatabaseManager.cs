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
                ExactTime = time,
                X = x_mm / 1000.0f,
                Y = y_mm / 1000.0f,
                Z = z_mm / 1000.0f,
                Depth = depth_m,
                Reflectivity = (byte)reflex,
                Tag = (byte)tag
            });
        }

        // 新增：根据时间范围查询点云
        public List<PointData> GetPointsInRange(DateTime endTime, double secondsBack)
        {
            List<PointData> points = new List<PointData>();
            DateTime startTime = endTime.AddSeconds(-secondsBack);

            using (MySqlConnection conn = new MySqlConnection(_connString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT x, y, z, depth, reflectivity, tag FROM point_cloud " +
                                 "WHERE collect_time BETWEEN @start AND @end";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@start", startTime);
                    cmd.Parameters.AddWithValue("@end", endTime);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            points.Add(new PointData
                            {
                                X = reader.GetFloat(0),
                                Y = reader.GetFloat(1),
                                Z = reader.GetFloat(2),
                                Depth = reader.GetFloat(3),
                                Reflectivity = reader.GetByte(4),
                                Tag = reader.GetByte(5)
                            });
                        }
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Query Error: " + ex.Message); }
            }
            return points;
        }

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
                    using (StreamWriter sw = new StreamWriter(tempFile, false, Encoding.UTF8))
                    {
                        foreach (var p in batch)
                        {
                            sw.WriteLine($"{p.ExactTime.ToString("yyyy-MM-dd HH:mm:ss.fff")},{p.X},{p.Y},{p.Z},{p.Depth},{p.Reflectivity},{p.Tag}");
                        }
                    }
                    BulkLoadFromFile(tempFile);
                    try { File.Delete(tempFile); } catch { }
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
                bulk.Columns.AddRange(new[] { "collect_time", "x", "y", "z", "depth", "reflectivity", "tag" });
                bulk.Load();
            }
        }
    }
}