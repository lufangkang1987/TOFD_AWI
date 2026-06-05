using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tofd_AWI.From.NewInspect
{
    // ============================================================
    // 文件: UcWaveformView.cs
    // 职责: 波形视图区域 — 支持 2x2 / 1+1 / 全屏 三种布局
    //       四个 PictureBox: A-Scan / S-Scan / A-Scan2 / C-Scan
    // ============================================================

    public partial class UcWaveformView : UserControl
    {
        private static readonly Color BG_DARK     = Color.FromArgb(10, 14, 26);   // #0a0e1a 更深
        private static readonly Color BG_CELL     = Color.FromArgb(12, 18, 30);   // #0c121e 单元格背景
        private static readonly Color CLR_BORDER  = Color.FromArgb(30, 40, 60);   // #1e283c 边框
        private static readonly Color CLR_TITLE   = Color.FromArgb(59, 130, 246); // #3b82f6 标题蓝
        private static readonly Color CLR_TEXT    = Color.FromArgb(148, 163, 184); // #94a3b8 文字
        private static readonly Color CLR_GREEN   = Color.FromArgb(34, 197, 94);   // #22c55e 绿色
        private static readonly Color CLR_RED     = Color.FromArgb(239, 68, 68);   // #ef4444 红色

        private string _layout = "2x2";

        public UcWaveformView()
        {
            BackColor = BG_DARK;
            InitializeComponent();
            SetLayout("2x2");
        }

        public void SetLayout(string layout)
        {
            _layout = layout;
            _tableLayout.Controls.Clear();
            _tableLayout.ColumnStyles.Clear();
            _tableLayout.RowStyles.Clear();
            _tableLayout.ColumnCount = 1;
            _tableLayout.RowCount = 1;

            switch (layout)
            {
                case "2x2":
                    _tableLayout.ColumnCount = 2;
                    _tableLayout.RowCount = 2;
                    _tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    _tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    _tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                    _tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                    _tableLayout.Controls.Add(MakeScanBox("A-Scan [F]1", "G2", CLR_RED, Color.FromArgb(10, 40, 20)), 0, 0);
                    _tableLayout.Controls.Add(MakeScanBox("S-Scan", null, Color.Empty, Color.FromArgb(10, 20, 40)), 1, 0);
                    _tableLayout.Controls.Add(MakeScanBox("A-Scan CH8", null, Color.Empty, Color.FromArgb(10, 40, 20)), 0, 1);
                    _tableLayout.Controls.Add(MakeScanBox("C-Scan", null, Color.Empty, Color.FromArgb(40, 10, 20)), 1, 1);
                    break;

                case "1p1":
                    _tableLayout.ColumnCount = 2;
                    _tableLayout.RowCount = 1;
                    _tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    _tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    _tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    _tableLayout.Controls.Add(MakeScanBox("A-Scan", "G2", CLR_RED, Color.FromArgb(10, 40, 20)), 0, 0);
                    _tableLayout.Controls.Add(MakeScanBox("S-Scan", null, Color.Empty, Color.FromArgb(10, 20, 40)), 1, 0);
                    break;

                case "full":
                    _tableLayout.ColumnCount = 1;
                    _tableLayout.RowCount = 1;
                    _tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                    _tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    _tableLayout.Controls.Add(MakeScanBox("A-Scan (Full)", "G2", CLR_RED, Color.FromArgb(10, 40, 20)), 0, 0);
                    break;
            }
        }

        /// <summary>
        /// 根据顶栏显示模式切换视图
        /// </summary>
        public void SetViewMode(string mode)
        {
            switch (mode)
            {
                case "ascan":
                    SetLayout("full");
                    break;
                case "bscan":
                    SetLayout("2x2");
                    break;
                case "cscan":
                    SetLayout("1p1");
                    break;
                case "tfm":
                    SetLayout("full");
                    break;
                default:
                    SetLayout("2x2");
                    break;
            }
        }

        private Control MakeScanBox(string title, string tag, Color tagColor, Color bgColor)
        {
            // 外层容器：带边框
            var outer = new Panel
            {
                BackColor = CLR_BORDER,
                Padding = new Padding(1),
                Margin = new Padding(2),
                Dock = DockStyle.Fill
            };

            // 内容面板
            var panel = new Panel
            {
                BackColor = BG_CELL,
                Dock = DockStyle.Fill
            };

            // 顶部标题栏
            var titlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 22,
                BackColor = BG_CELL
            };

            var lblTitle = new Label
            {
                Text = title,
                Dock = DockStyle.Left,
                Width = 120,
                Font = new Font("Consolas", 9F, FontStyle.Bold),
                ForeColor = CLR_TITLE,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(6, 0, 0, 0)
            };
            titlePanel.Controls.Add(lblTitle);

            // 标签（如 G2）
            if (!string.IsNullOrEmpty(tag))
            {
                var lblTag = new Label
                {
                    Text = tag,
                    Dock = DockStyle.Right,
                    Width = 30,
                    Font = new Font("Consolas", 9F, FontStyle.Bold),
                    ForeColor = tagColor,
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleRight,
                    Padding = new Padding(0, 0, 6, 0)
                };
                titlePanel.Controls.Add(lblTag);
            }

            // 波形绘制区域（黑色背景 + 模拟波形）
            var pic = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black
            };
            pic.Paint += (s, e) => DrawWaveform(e.Graphics, pic.ClientSize, title);

            panel.Controls.Add(pic);
            panel.Controls.Add(titlePanel);

            outer.Controls.Add(panel);
            return outer;
        }

        /// <summary>
        /// 在 PictureBox 上绘制模拟波形
        /// </summary>
        private void DrawWaveform(Graphics g, Size size, string title)
        {
            if (size.Width <= 0 || size.Height <= 0) return;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int w = size.Width;
            int h = size.Height;
            int midY = h / 2;

            // 绘制网格线（暗色虚线）
            using (var gridPen = new Pen(Color.FromArgb(30, 40, 60), 1))
            {
                gridPen.DashPattern = new float[] { 2, 6 };
                for (int x = 0; x < w; x += 40)
                    g.DrawLine(gridPen, x, 0, x, h);
                for (int y = midY; y < h; y += 30)
                    g.DrawLine(gridPen, 0, y, w, y);
            }

            // 中心线
            using (var centerPen = new Pen(Color.FromArgb(40, 60, 90), 1))
            {
                g.DrawLine(centerPen, 0, midY, w, midY);
            }

            // 根据标题绘制不同的波形
            if (title.Contains("C-Scan"))
            {
                DrawCScan(g, w, h);
            }
            else if (title.Contains("S-Scan"))
            {
                DrawSScan(g, w, h, midY);
            }
            else
            {
                DrawAScan(g, w, h, midY, title.Contains("CH8"));
            }
        }

        private void DrawAScan(Graphics g, int w, int h, int midY, bool isCh8)
        {
            // 模拟 A-Scan 波形（绿色脉冲）
            var points = new System.Collections.Generic.List<PointF>();
            var rand = new Random(42);
            float prevY = midY;

            for (int x = 0; x < w; x += 2)
            {
                float t = (float)x / w;
                float y = midY;

                if (!isCh8)
                {
                    // CH1: 多个脉冲
                    if (t > 0.15f && t < 0.2f) y -= 80 * (float)Math.Sin((t - 0.15f) * 20 * Math.PI);
                    if (t > 0.35f && t < 0.42f) y -= 60 * (float)Math.Sin((t - 0.35f) * 15 * Math.PI);
                    if (t > 0.65f && t < 0.72f) y -= 45 * (float)Math.Sin((t - 0.65f) * 18 * Math.PI);
                }
                else
                {
                    // CH8: 不同波形
                    if (t > 0.2f && t < 0.25f) y -= 70 * (float)Math.Sin((t - 0.2f) * 25 * Math.PI);
                    if (t > 0.5f && t < 0.58f) y -= 55 * (float)Math.Sin((t - 0.5f) * 12 * Math.PI);
                    if (t > 0.8f && t < 0.85f) y -= 40 * (float)Math.Sin((t - 0.8f) * 20 * Math.PI);
                }

                // 添加噪声
                y += (float)(rand.NextDouble() - 0.5) * 6;
                points.Add(new PointF(x, y));
                prevY = y;
            }

            using (var wavePen = new Pen(CLR_GREEN, 1.5f))
            {
                if (points.Count > 1)
                    g.DrawCurve(wavePen, points.ToArray());
            }

            // 脉冲填充（半透明）
            if (points.Count > 1)
            {
                using (var fillBrush = new SolidBrush(Color.FromArgb(20, 34, 197, 94)))
                {
                    var fillPoints = new System.Collections.Generic.List<PointF>(points);
                    fillPoints.Add(new PointF(w, midY));
                    fillPoints.Add(new PointF(0, midY));
                    g.FillPolygon(fillBrush, fillPoints.ToArray());
                }
            }
        }

        private void DrawSScan(Graphics g, int w, int h, int midY)
        {
            // 模拟 S-Scan 扇形图
            var center = new PointF(w / 2f, h * 0.85f);
            float radius = Math.Min(w, h) * 0.35f;

            // 扇形区域
            using (var sectorBrush = new SolidBrush(Color.FromArgb(20, 34, 197, 94)))
            {
                g.FillPie(sectorBrush, center.X - radius, center.Y - radius,
                    radius * 2, radius * 2, -70, -40);
            }

            using (var sectorPen = new Pen(Color.FromArgb(60, 34, 197, 94), 2))
            {
                g.DrawArc(sectorPen, center.X - radius, center.Y - radius,
                    radius * 2, radius * 2, -70, -40);
            }

            // 内部波形线
            using (var wavePen = new Pen(CLR_GREEN, 1.5f))
            {
                var pts = new PointF[50];
                for (int i = 0; i < 50; i++)
                {
                    float angle = -70 + (-40) * (i / 49f);
                    float r = radius * (0.3f + 0.4f * (float)Math.Sin(i * 0.3));
                    float rad = angle * (float)Math.PI / 180;
                    pts[i] = new PointF(center.X + r * (float)Math.Cos(rad),
                                       center.Y + r * (float)Math.Sin(rad));
                }
                g.DrawCurve(wavePen, pts);
            }
        }

        private void DrawCScan(Graphics g, int w, int h)
        {
            // 模拟 C-Scan 二维图像（色块矩阵）
            int cols = 8, rows = 4;
            int cellW = w / cols;
            int cellH = h / rows;
            var rand = new Random(123);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int cx = c * cellW + 1;
                    int cy = r * cellH + 1;
                    int cw = cellW - 2;
                    int ch = cellH - 2;

                    // 随机缺陷
                    int defect = rand.Next(0, 100);
                    Color cellColor;
                    if (defect > 85) cellColor = Color.FromArgb(200, 239, 68, 68);  // 严重缺陷 红
                    else if (defect > 65) cellColor = Color.FromArgb(180, 245, 158, 11); // 中等 黄
                    else if (defect > 40) cellColor = Color.FromArgb(120, 34, 197, 94); // 轻微 绿
                    else cellColor = Color.FromArgb(30, 40, 60); // 正常 深灰

                    using (var brush = new SolidBrush(cellColor))
                    {
                        g.FillRectangle(brush, cx, cy, cw, ch);
                    }
                }
            }
        }
    }
}
