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

            // ==================== 视角设置核心逻辑 ====================
            vtkCamera camera = _renderer.GetActiveCamera();

            // 1. 设置相机看向的目标点
            camera.SetFocalPoint(0, 0, 0);

            // 2. 将相机放到 X 轴的负半轴上，从而绕到点云的正前方
            camera.SetPosition(-100, 0, 0);

            // 3. 保持 Z轴 朝上不变
            camera.SetViewUp(0, 0, 1);

            // 4. 让 VTK 自动计算包围盒并缩放距离，保证所有点云刚好显示在画面中央
            _renderer.ResetCamera();
            // ========================================================

            _renderWindow.Render();
        }

        // 截取当前渲染窗口并保存为图片
        public void SaveScreenshot(string filePath)
        {
            if (_renderWindow == null) return;

            // 强制刷新渲染一次，确保画面是最新的
            _renderWindow.Render();

            // 使用 vtkWindowToImageFilter 提取渲染窗口的图像数据
            vtkWindowToImageFilter windowToImageFilter = vtkWindowToImageFilter.New();
            windowToImageFilter.SetInput(_renderWindow);
            windowToImageFilter.SetInputBufferTypeToRGB();
            // 设为 Off 可以从后台缓冲读取，防止被其他操作系统窗口遮挡时截到别的窗口
            windowToImageFilter.ReadFrontBufferOff();
            windowToImageFilter.Update();

            // 使用 vtkPNGWriter 写入为 PNG 文件
            vtkPNGWriter writer = vtkPNGWriter.New();
            writer.SetFileName(filePath);
            writer.SetInputConnection(windowToImageFilter.GetOutputPort());
            writer.Write();

            // 释放不再使用的非托管资源
            writer.Dispose();
            windowToImageFilter.Dispose();
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