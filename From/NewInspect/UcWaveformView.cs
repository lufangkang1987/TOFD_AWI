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
        private static readonly Color BG_DARK    = Color.FromArgb(15, 18, 25);
        private static readonly Color CLR_BORDER = Color.FromArgb(51, 65, 85);
        private static readonly Color CLR_LABEL  = Color.FromArgb(148, 163, 184);

        private string _layout = "2x2";

        public UcWaveformView()
        {
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
                    _tableLayout.Controls.Add(MakeScanBox("A-Scan", Color.FromArgb(20, 80, 40)), 0, 0);
                    _tableLayout.Controls.Add(MakeScanBox("S-Scan", Color.FromArgb(20, 40, 80)), 1, 0);
                    _tableLayout.Controls.Add(MakeScanBox("A-Scan 2", Color.FromArgb(80, 40, 20)), 0, 1);
                    _tableLayout.Controls.Add(MakeScanBox("C-Scan", Color.FromArgb(40, 20, 80)), 1, 1);
                    break;

                case "1p1":
                    _tableLayout.ColumnCount = 2;
                    _tableLayout.RowCount = 1;
                    _tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    _tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                    _tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    _tableLayout.Controls.Add(MakeScanBox("A-Scan", Color.FromArgb(20, 80, 40)), 0, 0);
                    _tableLayout.Controls.Add(MakeScanBox("S-Scan", Color.FromArgb(20, 40, 80)), 1, 0);
                    break;

                case "full":
                    _tableLayout.ColumnCount = 1;
                    _tableLayout.RowCount = 1;
                    _tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                    _tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                    _tableLayout.Controls.Add(MakeScanBox("A-Scan (Full)", Color.FromArgb(20, 80, 40)), 0, 0);
                    break;
            }
        }

        private Control MakeScanBox(string title, Color bgColor)
        {
            var panel = new Panel
            {
                BackColor = bgColor,
                Padding = new Padding(2),
                Margin = new Padding(2)
            };

            var lbl = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font("Consolas", 9F),
                ForeColor = CLR_LABEL,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(4, 0, 0, 0)
            };

            var pic = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            panel.Controls.Add(pic);
            panel.Controls.Add(lbl);
            return panel;
        }
    }
}
