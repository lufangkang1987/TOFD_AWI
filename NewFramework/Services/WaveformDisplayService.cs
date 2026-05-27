using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// ============================================================
// 文件: WaveformDisplayService.cs
// 位置: NewFramework/Services/
// 职责: 波形显示组件 — 封装 A/B/C/D 扫描的 GDI+ 渲染逻辑
//       将波形绘图从窗体中解耦，支持独立测试和复用
// ============================================================

namespace Tofd_AWI.NewFramework.Services
{
    /// <summary>
    /// 波形显示组件 — 管理超声检测波形的渲染
    /// 支持: A 扫描 (时域波形)、B 扫描 (截面图)、C 扫描 (俯视图)、D 扫描 (端视图)
    /// </summary>
    public class WaveformDisplayService : IDisposable
    {
        // ========== 显示控件 ==========

        /// <summary>A 扫描显示区 — 时域信号波形</summary>
        public PictureBox AScanBox { get; private set; }

        /// <summary>B 扫描显示区 — 截面图</summary>
        public PictureBox BScanBox { get; private set; }

        /// <summary>C 扫描显示区 — 俯视图 (彩色深度映射)</summary>
        public PictureBox CScanBox { get; private set; }

        /// <summary>D 扫描显示区 — 端视图</summary>
        public PictureBox DScanBox { get; private set; }

        // ========== 渲染配置 ==========

        /// <summary>背景颜色</summary>
        public Color BackgroundColor { get; set; } = Color.Black;

        /// <summary>波形线条颜色</summary>
        public Color WaveformColor { get; set; } = Color.Lime;

        /// <summary>网格线颜色</summary>
        public Color GridColor { get; set; } = Color.FromArgb(60, 60, 60);

        /// <summary>刻度文字颜色</summary>
        public Color ScaleTextColor { get; set; } = Color.Gray;

        /// <summary>门/闸门颜色</summary>
        public Color GateColor { get; set; } = Color.FromArgb(255, 80, 80);

        /// <summary>波形线宽</summary>
        public float LineWidth { get; set; } = 1.5f;

        /// <summary>是否显示网格</summary>
        public bool ShowGrid { get; set; } = true;

        /// <summary>网格水平分割数</summary>
        public int GridHorizontalDivisions { get; set; } = 10;

        /// <summary>网格垂直分割数</summary>
        public int GridVerticalDivisions { get; set; } = 10;

        // ========== A 扫描参数 ==========

        /// <summary>A 扫描数据缓冲区</summary>
        private byte[] _aScanData;

        /// <summary>A 扫描数据长度</summary>
        public int AScanDataLength => _aScanData?.Length ?? 0;

        // ========== C 扫描颜色映射 ==========

        /// <summary>C 扫描配色表 (0-255 灰度 → 彩色)</summary>
        private static readonly Color[] DefaultColorMap = BuildDefaultColorMap();

        // ========== 构造函数 ==========

        /// <summary>
        /// 创建波形显示服务
        /// </summary>
        /// <param name="aScanBox">A 扫描 PictureBox (可选)</param>
        /// <param name="bScanBox">B 扫描 PictureBox (可选)</param>
        /// <param name="cScanBox">C 扫描 PictureBox (可选)</param>
        /// <param name="dScanBox">D 扫描 PictureBox (可选)</param>
        public WaveformDisplayService(
            PictureBox aScanBox = null,
            PictureBox bScanBox = null,
            PictureBox cScanBox = null,
            PictureBox dScanBox = null)
        {
            AScanBox = aScanBox ?? CreateDefaultScanBox("AScan");
            BScanBox = bScanBox ?? CreateDefaultScanBox("BScan");
            CScanBox = cScanBox ?? CreateDefaultScanBox("CScan");
            DScanBox = dScanBox ?? CreateDefaultScanBox("DScan");
        }

        // ============================================================
        // A 扫描 — 时域波形
        // ============================================================

        /// <summary>
        /// 更新并渲染 A 扫描波形
        /// </summary>
        /// <param name="data">原始超声数据 (0-255 幅值)</param>
        /// <param name="gateStart">闸门起始位置 (采样点)</param>
        /// <param name="gateEnd">闸门结束位置 (采样点)</param>
        public void UpdateAScan(byte[] data, int gateStart = 0, int gateEnd = 0)
        {
            if (AScanBox == null || data == null || data.Length == 0) return;
            _aScanData = data;

            var bmp = new Bitmap(AScanBox.Width, AScanBox.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(BackgroundColor);

                int margin = 40;
                int plotW = bmp.Width - margin * 2;
                int plotH = bmp.Height - margin * 2;

                // 绘制网格
                if (ShowGrid)
                    DrawGrid(g, margin, margin, plotW, plotH);

                // 绘制闸门区域
                if (gateEnd > gateStart && gateStart >= 0)
                {
                    float gateX1 = margin + (float)gateStart / data.Length * plotW;
                    float gateX2 = margin + (float)gateEnd / data.Length * plotW;
                    using (var gateBrush = new SolidBrush(Color.FromArgb(40, GateColor)))
                    {
                        g.FillRectangle(gateBrush, gateX1, margin, gateX2 - gateX1, plotH);
                    }
                    using (var gatePen = new Pen(GateColor, 1) { DashStyle = DashStyle.Dash })
                    {
                        g.DrawLine(gatePen, gateX1, margin, gateX1, margin + plotH);
                        g.DrawLine(gatePen, gateX2, margin, gateX2, margin + plotH);
                    }
                }

                // 绘制波形
                float scaleX = (float)plotW / data.Length;
                using (var pen = new Pen(WaveformColor, LineWidth))
                {
                    PointF[] points = new PointF[data.Length];
                    for (int i = 0; i < data.Length; i++)
                    {
                        float x = margin + i * scaleX;
                        float y = margin + plotH - (data[i] / 255f * plotH);
                        points[i] = new PointF(x, y);
                    }
                    if (points.Length > 1)
                        g.DrawLines(pen, points);
                }

                // 绘制刻度
                DrawAScanScale(g, margin, margin, plotW, plotH, data.Length);
            }

            SetBoxImage(AScanBox, bmp);
        }

        // ============================================================
        // B 扫描 — 截面图 (多帧 A 扫描按位置堆叠)
        // ============================================================

        /// <summary>
        /// 更新并渲染 B 扫描图像
        /// </summary>
        /// <param name="bScanImage">B 扫描位图 (外部生成好的截面图)</param>
        public void UpdateBScan(Bitmap bScanImage)
        {
            if (BScanBox == null || bScanImage == null) return;

            var bmp = new Bitmap(BScanBox.Width, BScanBox.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(BackgroundColor);

                float ratio = Math.Min(
                    (float)BScanBox.Width / bScanImage.Width,
                    (float)BScanBox.Height / bScanImage.Height);
                int w = (int)(bScanImage.Width * ratio);
                int h = (int)(bScanImage.Height * ratio);
                int x = (BScanBox.Width - w) / 2;
                int y = (BScanBox.Height - h) / 2;
                g.DrawImage(bScanImage, x, y, w, h);
            }

            SetBoxImage(BScanBox, bmp);
        }

        // ============================================================
        // C 扫描 — 俯视图 (彩色深度映射)
        // ============================================================

        /// <summary>
        /// 更新并渲染 C 扫描俯视图 (彩色深度映射)
        /// </summary>
        /// <param name="depthData">深度数据二维数组 (值范围 0-255)</param>
        /// <param name="width">数据宽度 (X 轴点数)</param>
        /// <param name="height">数据高度 (Y 轴点数)</param>
        public void UpdateCScan(byte[] depthData, int width, int height)
        {
            if (CScanBox == null || depthData == null || depthData.Length == 0) return;
            if (width * height != depthData.Length) return;

            var bmp = new Bitmap(CScanBox.Width, CScanBox.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(BackgroundColor);

                int margin = 30;
                int plotW = bmp.Width - margin * 2;
                int plotH = bmp.Height - margin * 2;

                float cellW = (float)plotW / width;
                float cellH = (float)plotH / height;
                float cellSize = Math.Max(1, Math.Min(cellW, cellH));

                int offsetX = margin + (plotW - (int)(cellSize * width)) / 2;
                int offsetY = margin + (plotH - (int)(cellSize * height)) / 2;

                for (int row = 0; row < height; row++)
                {
                    for (int col = 0; col < width; col++)
                    {
                        int idx = row * width + col;
                        byte val = depthData[idx];
                        Color color = DefaultColorMap[val];

                        using (var brush = new SolidBrush(color))
                        {
                            g.FillRectangle(brush,
                                offsetX + col * cellSize,
                                offsetY + row * cellSize,
                                cellSize + 0.5f,
                                cellSize + 0.5f);
                        }
                    }
                }
            }

            SetBoxImage(CScanBox, bmp);
        }

        // ============================================================
        // D 扫描 — 端视图
        // ============================================================

        /// <summary>
        /// 更新并渲染 D 扫描端视图
        /// </summary>
        /// <param name="data">D 扫描数据 (幅值 0-255)</param>
        public void UpdateDScan(byte[] data)
        {
            if (DScanBox == null || data == null || data.Length == 0) return;

            var bmp = new Bitmap(DScanBox.Width, DScanBox.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(BackgroundColor);

                int margin = 20;
                int plotW = bmp.Width - margin * 2;
                int plotH = bmp.Height - margin * 2;

                if (ShowGrid)
                    DrawGrid(g, margin, margin, plotW, plotH);

                float scaleX = (float)plotW / data.Length;
                using (var pen = new Pen(WaveformColor, LineWidth))
                {
                    PointF[] points = new PointF[data.Length];
                    for (int i = 0; i < data.Length; i++)
                    {
                        float x = margin + i * scaleX;
                        float y = margin + plotH / 2f - (data[i] - 128) / 128f * plotH / 2f;
                        points[i] = new PointF(x, y);
                    }
                    if (points.Length > 1)
                        g.DrawLines(pen, points);
                }
            }

            SetBoxImage(DScanBox, bmp);
        }

        // ============================================================
        // 网格 & 刻度
        // ============================================================

        /// <summary>绘制网格</summary>
        private void DrawGrid(Graphics g, int x, int y, int w, int h)
        {
            using (var pen = new Pen(GridColor, 0.5f) { DashStyle = DashStyle.Dot })
            {
                // 竖线
                for (int i = 1; i < GridVerticalDivisions; i++)
                {
                    float vx = x + (float)w / GridVerticalDivisions * i;
                    g.DrawLine(pen, vx, y, vx, y + h);
                }
                // 横线
                for (int i = 1; i < GridHorizontalDivisions; i++)
                {
                    float hy = y + (float)h / GridHorizontalDivisions * i;
                    g.DrawLine(pen, x, hy, x + w, hy);
                }
            }
        }

        /// <summary>绘制 A 扫描刻度</summary>
        private void DrawAScanScale(Graphics g, int x, int y, int w, int h, int dataLen)
        {
            using (var font = new Font("Consolas", 8))
            using (var brush = new SolidBrush(ScaleTextColor))
            using (var format = new StringFormat { Alignment = StringAlignment.Center })
            {
                // 水平刻度 (采样点)
                for (int i = 0; i <= GridVerticalDivisions; i++)
                {
                    float vx = x + (float)w / GridVerticalDivisions * i;
                    int sample = (int)((float)dataLen / GridVerticalDivisions * i);
                    g.DrawString(sample.ToString(), font, brush, vx, y + h + 2, format);
                }

                // 垂直刻度 (幅值 %)
                format.Alignment = StringAlignment.Far;
                format.LineAlignment = StringAlignment.Center;
                for (int i = 0; i <= GridHorizontalDivisions; i++)
                {
                    float hy = y + (float)h / GridHorizontalDivisions * i;
                    int pct = 100 - (int)(100f / GridHorizontalDivisions * i);
                    g.DrawString(pct + "%", font, brush, x - 4, hy, format);
                }
            }
        }

        // ============================================================
        // 工具方法
        // ============================================================

        /// <summary>安全设置 PictureBox 图像 (自动 Dispose 旧图)</summary>
        private static void SetBoxImage(PictureBox box, Bitmap newBmp)
        {
            if (box == null) return;
            var old = box.Image;
            box.Image = newBmp;
            old?.Dispose();
        }

        /// <summary>创建默认样式的扫描 PictureBox</summary>
        private static PictureBox CreateDefaultScanBox(string name)
        {
            return new PictureBox
            {
                Name = name,
                BackColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Normal
            };
        }

        /// <summary>清除所有波形</summary>
        public void ClearAll()
        {
            ClearBox(AScanBox);
            ClearBox(BScanBox);
            ClearBox(CScanBox);
            ClearBox(DScanBox);
            _aScanData = null;
        }

        private static void ClearBox(PictureBox box)
        {
            if (box?.Image != null)
            {
                box.Image.Dispose();
                box.Image = null;
            }
        }

        /// <summary>释放资源</summary>
        public void Dispose()
        {
            ClearAll();
            AScanBox?.Dispose();
            BScanBox?.Dispose();
            CScanBox?.Dispose();
            DScanBox?.Dispose();
        }

        // ============================================================
        // 颜色映射表 (灰度 → 热力图彩色)
        // ============================================================
        private static Color[] BuildDefaultColorMap()
        {
            var map = new Color[256];
            for (int i = 0; i < 256; i++)
            {
                // 蓝色(低) → 青色 → 绿色 → 黄色 → 红色(高)
                float t = i / 255f;
                int r, g, b;

                if (t < 0.25f)
                {
                    // 黑 → 蓝
                    r = 0;
                    g = 0;
                    b = (int)(t / 0.25f * 255);
                }
                else if (t < 0.5f)
                {
                    // 蓝 → 青
                    float s = (t - 0.25f) / 0.25f;
                    r = 0;
                    g = (int)(s * 255);
                    b = 255;
                }
                else if (t < 0.75f)
                {
                    // 青 → 黄
                    float s = (t - 0.5f) / 0.25f;
                    r = (int)(s * 255);
                    g = 255;
                    b = (int)((1 - s) * 255);
                }
                else
                {
                    // 黄 → 红
                    float s = (t - 0.75f) / 0.25f;
                    r = 255;
                    g = (int)((1 - s) * 255);
                    b = 0;
                }

                map[i] = Color.FromArgb(
                    Math.Max(0, Math.Min(255, r)),
                    Math.Max(0, Math.Min(255, g)),
                    Math.Max(0, Math.Min(255, b)));
            }
            return map;
        }
    }
}
