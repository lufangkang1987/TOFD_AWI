using System;
using System.Drawing;
using System.Windows.Forms;

// ============================================================
// 文件: VideoDisplayService.cs
// 位置: NewInspect.Services/Services/
// 职责: 两路视频显示管理 — 检测摄像头 + 操作员摄像头
//       封装 PictureBox 绑定、帧渲染、叠加文字、录像状态指示
// ============================================================

namespace NewInspect.Services
{
    /// <summary>
    /// 视频显示组件 — 管理两个视频显示 PictureBox
    /// 组件1: 检测摄像头 (前视/焊缝画面)
    /// 组件2: 操作员摄像头 (检定员/环境监控)
    /// </summary>
    public class VideoDisplayService : IDisposable
    {
        // ========== 两路视频显示控件 ==========

        /// <summary>组件1: 检测摄像头显示区</summary>
        public PictureBox InspectVideoBox { get; private set; }

        /// <summary>组件2: 操作员摄像头显示区</summary>
        public PictureBox OperatorVideoBox { get; private set; }

        // ========== 显示状态 ==========

        /// <summary>当前检测摄像头的帧</summary>
        public Bitmap InspectFrame { get; private set; }

        /// <summary>当前操作员摄像头的帧</summary>
        public Bitmap OperatorFrame { get; private set; }

        /// <summary>检测摄像头是否正在显示</summary>
        public bool IsInspectVideoActive { get; set; } = true;

        /// <summary>操作员摄像头是否正在显示</summary>
        public bool IsOperatorVideoActive { get; set; }

        /// <summary>是否录制中</summary>
        public bool IsRecording { get; set; }

        /// <summary>叠加文字颜色</summary>
        public Color OverlayTextColor { get; set; } = Color.Yellow;

        /// <summary>叠加文字: 检测画面</summary>
        public string InspectOverlayText { get; set; } = "";

        /// <summary>叠加文字: 操作员画面</summary>
        public string OperatorOverlayText { get; set; } = "";

        /// <summary>录制指示器闪烁颜色</summary>
        public Color RecordingIndicatorColor { get; set; } = Color.Red;

        // ========== 构造函数 ==========

        /// <summary>
        /// 创建视频显示服务
        /// </summary>
        /// <param name="inspectBox">检测摄像头 PictureBox (可由外部传入或 null 自动创建)</param>
        /// <param name="operatorBox">操作员摄像头 PictureBox (可由外部传入或 null 自动创建)</param>
        public VideoDisplayService(PictureBox inspectBox = null, PictureBox operatorBox = null)
        {
            InspectVideoBox = inspectBox ?? CreateDefaultVideoBox("InspectVideo");
            OperatorVideoBox = operatorBox ?? CreateDefaultVideoBox("OperatorVideo");
        }

        // ========== 帧更新 ==========

        /// <summary>
        /// 更新检测摄像头画面
        /// </summary>
        /// <param name="frame">摄像头采集到的帧</param>
        /// <param name="overlayText">叠加文字 (距离、时间等)</param>
        public void UpdateInspectFrame(Bitmap frame, string overlayText = null)
        {
            if (!IsInspectVideoActive || InspectVideoBox == null) return;

            if (overlayText != null)
                InspectOverlayText = overlayText;

            RenderFrame(InspectVideoBox, frame, InspectOverlayText);
            InspectFrame = frame;
        }

        /// <summary>
        /// 更新操作员摄像头画面
        /// </summary>
        /// <param name="frame">摄像头采集到的帧</param>
        /// <param name="overlayText">叠加文字</param>
        public void UpdateOperatorFrame(Bitmap frame, string overlayText = null)
        {
            if (!IsOperatorVideoActive || OperatorVideoBox == null) return;

            if (overlayText != null)
                OperatorOverlayText = overlayText;

            RenderFrame(OperatorVideoBox, frame, OperatorOverlayText);
            OperatorFrame = frame;
        }

        /// <summary>
        /// 将帧渲染到指定 PictureBox，带叠加文字和录制指示器
        /// </summary>
        private void RenderFrame(PictureBox box, Bitmap frame, string overlayText)
        {
            if (box == null || frame == null) return;

            try
            {
                var oldImage = box.Image;
                Bitmap canvas;

                // 如果帧尺寸与控件不一致，创建适配画布
                if (frame.Width != box.Width || frame.Height != box.Height)
                {
                    canvas = new Bitmap(box.Width, box.Height);
                    using (var g = Graphics.FromImage(canvas))
                    {
                        g.Clear(Color.Black);
                        // 保持比例缩放居中
                        float ratio = Math.Min(
                            (float)box.Width / frame.Width,
                            (float)box.Height / frame.Height);
                        int drawW = (int)(frame.Width * ratio);
                        int drawH = (int)(frame.Height * ratio);
                        int x = (box.Width - drawW) / 2;
                        int y = (box.Height - drawH) / 2;
                        g.DrawImage(frame, x, y, drawW, drawH);

                        // 叠加文字
                        DrawOverlay(g, box.Width, box.Height, overlayText);
                    }
                }
                else
                {
                    canvas = new Bitmap(frame);
                    using (var g = Graphics.FromImage(canvas))
                    {
                        DrawOverlay(g, canvas.Width, canvas.Height, overlayText);
                    }
                }

                box.Image = canvas;
                oldImage?.Dispose();
            }
            catch
            {
                // 渲染失败时静默处理
            }
        }

        /// <summary>
        /// 绘制叠加层: 录制指示器 + 文字信息
        /// </summary>
        private void DrawOverlay(Graphics g, int width, int height, string text)
        {
            // 录制指示器 (左上角红色圆点)
            if (IsRecording)
            {
                using (var brush = new SolidBrush(RecordingIndicatorColor))
                {
                    g.FillEllipse(brush, 10, 10, 12, 12);
                }
                using (var font = new Font("Arial", 9, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                {
                    g.DrawString("REC", font, brush, 26, 8);
                }
            }

            // 叠加文字 (左下角)
            if (!string.IsNullOrEmpty(text))
            {
                using (var font = new Font("Consolas", 10, FontStyle.Regular))
                using (var brush = new SolidBrush(OverlayTextColor))
                using (var bgBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0)))
                {
                    var textSize = g.MeasureString(text, font);
                    // 半透明背景
                    g.FillRectangle(bgBrush, 4, height - textSize.Height - 8,
                        textSize.Width + 8, textSize.Height + 4);
                    g.DrawString(text, font, brush, 8, height - textSize.Height - 6);
                }
            }
        }

        // ========== 画面切换 ==========

        /// <summary>切换检测画面显示/隐藏</summary>
        public void ToggleInspectVideo()
        {
            IsInspectVideoActive = !IsInspectVideoActive;
            if (InspectVideoBox != null)
            {
                InspectVideoBox.Visible = IsInspectVideoActive;
                if (!IsInspectVideoActive)
                {
                    InspectVideoBox.Image?.Dispose();
                    InspectVideoBox.Image = null;
                }
            }
        }

        /// <summary>切换操作员画面显示/隐藏</summary>
        public void ToggleOperatorVideo()
        {
            IsOperatorVideoActive = !IsOperatorVideoActive;
            if (OperatorVideoBox != null)
            {
                OperatorVideoBox.Visible = IsOperatorVideoActive;
                if (!IsOperatorVideoActive)
                {
                    OperatorVideoBox.Image?.Dispose();
                    OperatorVideoBox.Image = null;
                }
            }
        }

        /// <summary>大/小画面切换</summary>
        public void SwapVideoFeeds()
        {
            if (InspectVideoBox == null || OperatorVideoBox == null) return;

            // 交换位置和尺寸
            var tempLoc = InspectVideoBox.Location;
            var tempSize = InspectVideoBox.Size;
            InspectVideoBox.Location = OperatorVideoBox.Location;
            InspectVideoBox.Size = OperatorVideoBox.Size;
            OperatorVideoBox.Location = tempLoc;
            OperatorVideoBox.Size = tempSize;

            // 交换帧
            var tempFrame = InspectFrame;
            InspectFrame = OperatorFrame;
            OperatorFrame = tempFrame;
            InspectVideoBox.Image = InspectFrame;
            OperatorVideoBox.Image = OperatorFrame;
        }

        // ========== 截图 ==========

        /// <summary>抓拍检测画面当前帧</summary>
        public Bitmap CaptureInspectFrame()
        {
            return InspectFrame != null ? new Bitmap(InspectFrame) : null;
        }

        /// <summary>抓拍操作员画面当前帧</summary>
        public Bitmap CaptureOperatorFrame()
        {
            return OperatorFrame != null ? new Bitmap(OperatorFrame) : null;
        }

        // ========== 工具方法 ==========

        /// <summary>创建默认样式的视频 PictureBox</summary>
        private static PictureBox CreateDefaultVideoBox(string name)
        {
            return new PictureBox
            {
                Name = name,
                BackColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Visible = false // 默认隐藏，等有画面时再显示
            };
        }

        /// <summary>清除所有画面</summary>
        public void ClearAll()
        {
            ClearBox(InspectVideoBox);
            ClearBox(OperatorVideoBox);
            InspectFrame?.Dispose();
            InspectFrame = null;
            OperatorFrame?.Dispose();
            OperatorFrame = null;
        }

        private static void ClearBox(PictureBox box)
        {
            if (box?.Image != null)
            {
                box.Image.Dispose();
                box.Image = null;
            }
        }

        /// <summary>释放资源 (PictureBox 由窗体设计器管理，此处仅清理图像缓存)</summary>
        public void Dispose()
        {
            ClearAll();
        }
    }
}
