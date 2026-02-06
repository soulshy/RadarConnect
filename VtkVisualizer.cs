using Kitware.VTK;
using System;
using System.Collections.Generic;

namespace RadarConnect
{
    public class VtkVisualizer
    {
        private vtkRenderer _renderer;
        private vtkRenderWindow _renderWindow;
        private vtkActor _currentActor;

        public VtkVisualizer(RenderWindowControl renderWindowControl)
        {
            if (renderWindowControl == null) return;
            _renderWindow = renderWindowControl.RenderWindow;
            _renderer = _renderWindow.GetRenderers().GetFirstRenderer();

            if (_renderer == null)
            {
                _renderer = vtkRenderer.New();
                _renderer.SetBackground(0.0, 0.0, 0.0);
                _renderWindow.AddRenderer(_renderer);
            }
            _renderer.ResetCamera();
        }

        public void Render(List<PointData> points)
        {
            if (points == null || points.Count == 0) return;

            if (_currentActor != null)
            {
                _renderer.RemoveActor(_currentActor);
                _currentActor.Dispose();
                _currentActor = null;
            }

            vtkPoints vtkPts = vtkPoints.New();
            vtkUnsignedCharArray colors = vtkUnsignedCharArray.New();
            colors.SetNumberOfComponents(3);
            colors.SetName("Colors");

            foreach (var p in points)
            {
                vtkPts.InsertNextPoint(p.X, p.Y, p.Z);
                byte[] rgb = GetColorFromIntensity(p.Reflectivity);
                colors.InsertNextTuple3(rgb[0], rgb[1], rgb[2]);
            }

            vtkPolyData polyData = vtkPolyData.New();
            polyData.SetPoints(vtkPts);
            polyData.GetPointData().SetScalars(colors);

            vtkVertexGlyphFilter glyphFilter = vtkVertexGlyphFilter.New();
            glyphFilter.SetInput(polyData);
            glyphFilter.Update();

            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInputConnection(glyphFilter.GetOutputPort());

            _currentActor = vtkActor.New();
            _currentActor.SetMapper(mapper);
            _currentActor.GetProperty().SetPointSize(2);

            _renderer.AddActor(_currentActor);
            _renderer.ResetCamera();
            _renderWindow.Render();
        }

        private byte[] GetColorFromIntensity(byte val)
        {
            byte r = 0, g = 0, b = 0;
            if (val < 30) { r = 0; g = 0; b = 255; }
            else if (val < 100) { r = 0; g = 255; b = 0; }
            else { r = 255; g = 0; b = 0; }
            return new byte[] { r, g, b };
        }
    }
}