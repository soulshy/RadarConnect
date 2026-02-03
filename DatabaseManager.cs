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
            while (_bufferQueue.TryDequeue(out _)) { }
            _saveThread = new Thread(SaveLoop) { IsBackground = true };
            _saveThread.Start();
        }

        public void StopSaving() => _isSaving = false;

        public void EnqueuePoint(DateTime time, int x_mm, int y_mm, int z_mm, int reflex, int tag)
        {
            if (!_isSaving) return;
            if (_bufferQueue.Count > 500000) return;

            _bufferQueue.Enqueue(new PointData
            {
                ExactTime = time, // 存入计算好的精确时间
                X = x_mm / 1000.0f,
                Y = y_mm / 1000.0f,
                Z = z_mm / 1000.0f,
                Reflectivity = (byte)reflex,
                Tag = (byte)tag
            });
        }

        private void SaveLoop()
        {
            List<PointData> batch = new List<PointData>(BATCH_SIZE);

            while (_isSaving || !_bufferQueue.IsEmpty)
            {
                try
                {
                    batch.Clear();
                    int currentLimit = (_bufferQueue.Count > BATCH_SIZE * 2) ? BATCH_SIZE * 2 : BATCH_SIZE;

                    while (batch.Count < currentLimit && _bufferQueue.TryDequeue(out var p))
                    {
                        batch.Add(p);
                    }

                    if (batch.Count > 0)
                    {
                        string tempFile = Path.GetTempFileName();
                        using (StreamWriter sw = new StreamWriter(tempFile, false, Encoding.UTF8))
                        {
                            foreach (var p in batch)
                            {
                                //使用点云自带的 ExactTime
                                sw.Write(p.ExactTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                sw.Write(',');
                                sw.Write(p.X); sw.Write(',');
                                sw.Write(p.Y); sw.Write(',');
                                sw.Write(p.Z); sw.Write(',');
                                sw.Write(p.Reflectivity); sw.Write(',');
                                sw.WriteLine(p.Tag);
                            }
                        }

                        BulkLoadFromFile(tempFile);
                        try { File.Delete(tempFile); } catch { }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"DB Error: {ex.Message}");
                    Thread.Sleep(1000);
                }
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
                    LineTerminator = "\r\n",
                    FileName = filePath,
                    NumberOfLinesToSkip = 0,
                    Local = true
                };
                // 确保列名对应
                bulk.Columns.AddRange(new[] { "collect_time", "x", "y", "z", "reflectivity", "tag" });
                bulk.Load();
            }
        }
    }
}