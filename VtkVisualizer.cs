/*using Kitware.VTK;
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
}*/

//利用点的深度信息进行颜色映射，增强视觉效果。
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

            // ==================== 1. 计算深度并获取极值用于归一化 ====================
            float minDepth = float.MaxValue;
            float maxDepth = float.MinValue;
            float[] depths = new float[points.Count];

            for (int i = 0; i < points.Count; i++)
            {
                var p = points[i];
                float depth = (float)Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);

                depths[i] = depth;
                if (depth < minDepth) minDepth = depth;
                if (depth > maxDepth) maxDepth = depth;
            }

            // 防止全零或所有点在同一距离导致的除零异常
            if (maxDepth - minDepth < 0.001f) maxDepth = minDepth + 1.0f;

            // ==================== 2. 应用点坐标与颜色映射 ====================
            for (int i = 0; i < points.Count; i++)
            {
                var p = points[i];
                vtkPts.InsertNextPoint(p.X, p.Y, p.Z);

                // 根据当前点的深度和全局极值获取颜色
                byte[] rgb = GetColorFromDepth(depths[i], minDepth, maxDepth);
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

            double[] bounds = _currentActor.GetBounds();
            double centerX = (bounds[0] + bounds[1]) / 2.0;
            double centerY = (bounds[2] + bounds[3]) / 2.0;
            double centerZ = (bounds[4] + bounds[5]) / 2.0;

            camera.SetFocalPoint(centerX, centerY, centerZ);
            camera.SetPosition(centerX - 100, centerY, centerZ);
            camera.SetViewUp(0, 0, 1);

            camera.OrthogonalizeViewUp();
            camera.ParallelProjectionOn();

            _renderer.ResetCameraClippingRange();
            _renderer.ResetCamera();

            camera.Zoom(1.2);

            _renderWindow.Render();
        }

        public void SaveScreenshot(string filePath)
        {
            if (_renderWindow == null) return;

            _renderWindow.Render();
            vtkWindowToImageFilter windowToImageFilter = vtkWindowToImageFilter.New();
            windowToImageFilter.SetInput(_renderWindow);
            windowToImageFilter.SetInputBufferTypeToRGB();
            windowToImageFilter.ReadFrontBufferOff();
            windowToImageFilter.Update();

            vtkPNGWriter writer = vtkPNGWriter.New();
            writer.SetFileName(filePath);
            writer.SetInputConnection(windowToImageFilter.GetOutputPort());
            writer.Write();

            writer.Dispose();
            windowToImageFilter.Dispose();
        }

        // ==================== 深度转伪彩色 (Jet Colormap) ====================
        /// <summary>
        /// 将深度值映射为经典的 Jet 伪彩色 (蓝 -> 青 -> 绿 -> 黄 -> 红)
        /// </summary>
        private byte[] GetColorFromDepth(float depth, float minDepth, float maxDepth)
        {
            // 将深度归一化到 0.0 ~ 1.0 之间
            float v = (depth - minDepth) / (maxDepth - minDepth);

            // 限制在 0-1 范围，防止溢出
            v = Math.Max(0f, Math.Min(1f, v));

            // Jet Colormap 算法
            float r = ClampColor(1.5f - Math.Abs(4.0f * (1.0f - v) - 3.0f));
            float g = ClampColor(1.5f - Math.Abs(4.0f * (1.0f - v) - 2.0f));
            float b = ClampColor(1.5f - Math.Abs(4.0f * (1.0f - v) - 1.0f));

            // 近处为红色，远处为蓝色。
            return new byte[] { (byte)(r * 255), (byte)(g * 255), (byte)(b * 255) };
        }

        private float ClampColor(float val)
        {
            if (val < 0f) return 0f;
            if (val > 1f) return 1f;
            return val;
        }
    }
}