using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Kitware.VTK;

namespace RadarConnect
{
    public partial class VtkPointCloudForm : Form
    {
        private RenderWindowControl _renderControl;
        private vtkRenderer _renderer;

        private vtkPoints _vtkPoints;
        private vtkCellArray _vertices;
        private vtkFloatArray _scalars;
        private vtkPolyData _polyData;
        private vtkPolyDataMapper _mapper;
        private vtkActor _actor;
        private vtkLookupTable _lut;
        private vtkOrientationMarkerWidget _axesWidget;

        private LinkedList<List<PointData>> _batchBuffer = new LinkedList<List<PointData>>();
        private readonly object _lock = new object();

        private double _accumulationTime = 0.5;

        private Timer _renderTimer;

        public VtkPointCloudForm()
        {
            this.Text = "实时点云";
            this.Size = new Size(1000, 800);

            _renderControl = new RenderWindowControl();
            _renderControl.Dock = DockStyle.Fill;
            _renderControl.AddTestActors = false;
            this.Controls.Add(_renderControl);

            _renderControl.Load += OnRenderControlLoad;

            // 提高刷新率到 30 FPS (33ms)
            _renderTimer = new Timer();
            _renderTimer.Interval = 33;
            _renderTimer.Tick += (s, e) => UpdateVtkData();
            _renderTimer.Start();
        }

        private void OnRenderControlLoad(object sender, EventArgs e)
        {
            try
            {
                _renderer = _renderControl.RenderWindow.GetRenderers().GetFirstRenderer();
                _renderer.SetBackground(0.0, 0.0, 0.0);

                // 1. VTK 管道
                _vtkPoints = vtkPoints.New();
                _vertices = vtkCellArray.New();
                _scalars = vtkFloatArray.New();
                _scalars.SetNumberOfComponents(1);

                _polyData = vtkPolyData.New();
                _polyData.SetPoints(_vtkPoints);
                _polyData.SetVerts(_vertices);
                _polyData.GetPointData().SetScalars(_scalars);

                // 2. 颜色表：亮蓝 -> 红
                _lut = vtkLookupTable.New();
                _lut.SetTableRange(0, 255);
                _lut.SetHueRange(0.667, 0.0);
                _lut.SetSaturationRange(1.0, 1.0);
                _lut.SetValueRange(1.0, 1.0);
                _lut.Build();

                _mapper = vtkPolyDataMapper.New();
                _mapper.SetInput(_polyData); // ActiViz 5.8
                _mapper.SetLookupTable(_lut);
                _mapper.SetScalarRange(0, 255);

                _actor = vtkActor.New();
                _actor.SetMapper(_mapper);
                _actor.GetProperty().SetPointSize(2);
                _renderer.AddActor(_actor);

                // 3. 坐标轴
                var axesActor = vtkAxesActor.New();
                axesActor.SetTotalLength(1.0, 1.0, 1.0);
                _axesWidget = vtkOrientationMarkerWidget.New();
                _axesWidget.SetOrientationMarker(axesActor);
                _axesWidget.SetInteractor(_renderControl.RenderWindow.GetInteractor());
                _axesWidget.SetViewport(0.0, 0.0, 0.15, 0.15); // 左下角
                _axesWidget.SetEnabled(1);
                _axesWidget.InteractiveOff();

                // 4. 网格
                CreateGrid();

                _renderer.ResetCamera();
                _renderer.GetActiveCamera().Elevation(30);
            }
            catch (Exception ex) { MessageBox.Show("VTK Error: " + ex.Message); }
        }

        private void CreateGrid()
        {
            for (int i = -10; i <= 10; i += 5)
            {
                CreateLine(i, -10, 0, i, 10, 0);
                CreateLine(-10, i, 0, 10, i, 0);
            }
        }

        private void CreateLine(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            var source = vtkLineSource.New();
            source.SetPoint1(x1, y1, z1);
            source.SetPoint2(x2, y2, z2);
            var mapper = vtkPolyDataMapper.New();
            mapper.SetInput(source.GetOutput());
            var actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.GetProperty().SetColor(0.2, 0.2, 0.2);
            _renderer.AddActor(actor);
        }

        public void AddPoints(List<PointData> newBatch)
        {
            if (newBatch == null || newBatch.Count == 0) return;
            lock (_lock)
            {
                _batchBuffer.AddLast(newBatch);
                //时间窗口：0.5s
                if (_batchBuffer.Count > 0)
                {
                    DateTime threshold = _batchBuffer.Last.Value.Last().ExactTime.AddSeconds(-_accumulationTime);
                    while (_batchBuffer.First != null)
                    {
                        var firstBatch = _batchBuffer.First.Value;
                        if (firstBatch.Count > 0 && firstBatch.Last().ExactTime < threshold)
                            _batchBuffer.RemoveFirst();
                        else
                            break;
                    }
                }
            }
        }

        private void UpdateVtkData()
        {
            if (_vtkPoints == null) return;
            List<List<PointData>> snapshot;
            lock (_lock) { snapshot = _batchBuffer.ToList(); }

            _vtkPoints.Reset();
            _vertices.Reset();
            _scalars.Reset();

            long ptId = 0;
            // 简单距离过滤
            float minDistSq = 0.1f * 0.1f;

            foreach (var batch in snapshot)
            {
                foreach (var p in batch)
                {
                    if (p.X * p.X + p.Y * p.Y + p.Z * p.Z < minDistSq) continue;

                    _vtkPoints.InsertNextPoint(p.X, p.Y, p.Z);
                    _vertices.InsertNextCell(1);
                    _vertices.InsertCellPoint(ptId++);
                    _scalars.InsertNextValue(p.Reflectivity);
                }
            }

            _vtkPoints.Modified();
            _vertices.Modified();
            _scalars.Modified();
            _polyData.Modified();
            _renderControl.RenderWindow.Render();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _renderTimer.Stop();
            if (_renderControl != null) _renderControl.Dispose();
            base.OnFormClosing(e);
        }
    }
}