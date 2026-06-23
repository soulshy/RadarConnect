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
            _renderer.ResetCamera();
            vtkCamera camera = _renderer.GetActiveCamera();

            // 2. 获取当前点云的真实物理中心点，避免偏心观察
            double[] bounds = _currentActor.GetBounds();
            double centerX = (bounds[0] + bounds[1]) / 2.0;
            double centerY = (bounds[2] + bounds[3]) / 2.0;
            double centerZ = (bounds[4] + bounds[5]) / 2.0;

            // 3. 严格设置相机参数，看向真实中心
            camera.SetFocalPoint(centerX, centerY, centerZ);
            // 将相机放在 X 轴负方向，距离中心一定距离 (形成正交前视图)
            camera.SetPosition(centerX - 100, centerY, centerZ);
            // 强制 Z 轴朝上 (如果你的雷达数据是 Y 轴朝上，请改为 0, 1, 0)
            camera.SetViewUp(0, 0, 1);

            // 4. 正交化视点向量，消除浮点计算可能导致的微量画面旋转
            camera.OrthogonalizeViewUp();

            // 5. 开启正交投影 (Parallel Projection)
            // 彻底消除“近大远小”的透视畸变，保证画面中的墙壁等平行结构绝对垂直/水平！
            camera.ParallelProjectionOn();

            // 6. 重新调整裁剪平面并自动缩放，确保所有点都在画面内
            _renderer.ResetCameraClippingRange();
            _renderer.ResetCamera();

            // 可以根据需要微调缩放比例
            camera.Zoom(1.2);

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

