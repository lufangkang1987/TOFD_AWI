using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tofd_AWI.From.NewInspect
{
    // ============================================================
    // 文件: UcRightPanel.cs
    // 职责: 右侧控制面板
    //       增益快捷控制 + 闸门读数 + 报警状态 + 缺陷记录 + 通信监控
    // ============================================================

    public partial class UcRightPanel : UserControl
    {
        private static readonly Color BG_PANEL   = Color.FromArgb(30, 41, 59);
        private static readonly Color BG_INPUT   = Color.FromArgb(40, 52, 72);
        private static readonly Color CLR_TEXT   = Color.FromArgb(226, 232, 240);
        private static readonly Color CLR_MUTED  = Color.FromArgb(148, 163, 184);
        private static readonly Color CLR_BLUE   = Color.FromArgb(59, 130, 246);
        private static readonly Color CLR_GREEN  = Color.FromArgb(34, 197, 94);
        private static readonly Color CLR_RED    = Color.FromArgb(239, 68, 68);
        private static readonly Color CLR_AMBER  = Color.FromArgb(245, 158, 11);
        private static readonly Color CLR_BORDER = Color.FromArgb(51, 65, 85);

        private int _gainValue = 40;
        private int _defectCount = 0;

        // 控件引用
        private Label _lblGain;
        private TrackBar _trackGain;
        private Label _lblG1Peak, _lblG1Pos;
        private Label _lblG2Peak, _lblG2Pos;
        private Label _lblAlarmG1, _lblAlarmG2, _lblAlarmLoss;
        private ListBox _lstDefects;
        private Label _lblTxRate, _lblRxRate, _lblLatency;

        public UcRightPanel()
        {
            InitializeComponent();
            BuildContent();
        }

        private void BuildContent()
        {
            var mainPanel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = BG_PANEL };
            int y = 4;

            // ====== 增益控制 ======
            AddSection(mainPanel, ref y, "增益控制");

            _lblGain = new Label
            {
                Text = "40 dB",
                Font = new Font("Consolas", 16F, FontStyle.Bold),
                ForeColor = CLR_GREEN,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(8, y),
                Size = new Size(170, 30)
            };
            mainPanel.Controls.Add(_lblGain);
            y += 34;

            _trackGain = new TrackBar
            {
                Minimum = 0, Maximum = 80, Value = 40,
                TickFrequency = 10,
                Location = new Point(8, y),
                Size = new Size(170, 30),
                BackColor = BG_PANEL
            };
            _trackGain.ValueChanged += (s, e) =>
            {
                _gainValue = _trackGain.Value;
                _lblGain.Text = $"{_gainValue} dB";
            };
            mainPanel.Controls.Add(_trackGain);
            y += 36;

            // +/- 按钮
            var gains = new[] { "-6", "-1", "+1", "+6" };
            var deltas = new[] { -6, -1, 1, 6 };
            for (int i = 0; i < 4; i++)
            {
                var btn = new Button
                {
                    Text = gains[i],
                    FlatStyle = FlatStyle.Flat,
                    BackColor = BG_INPUT,
                    ForeColor = (deltas[i] > 0) ? CLR_GREEN : CLR_AMBER,
                    Font = new Font("Consolas", 9F, FontStyle.Bold),
                    Location = new Point(8 + i * 44, y),
                    Size = new Size(40, 24)
                };
                btn.FlatAppearance.BorderColor = CLR_BORDER;
                int delta = deltas[i];
                btn.Click += (s, e) =>
                {
                    _gainValue = Math.Max(0, Math.Min(80, _gainValue + delta));
                    _trackGain.Value = _gainValue;
                };
                mainPanel.Controls.Add(btn);
            }
            y += 32;

            // ====== 闸门读数 ======
            AddSection(mainPanel, ref y, "闸门读数");

            _lblG1Peak = AddReadout(mainPanel, ref y, "G1 峰值", "0 %");
            _lblG1Pos  = AddReadout(mainPanel, ref y, "G1 位置", "0.0 mm");
            _lblG2Peak = AddReadout(mainPanel, ref y, "G2 峰值", "0 %");
            _lblG2Pos  = AddReadout(mainPanel, ref y, "G2 位置", "0.0 mm");

            // ====== 报警状态 ======
            AddSection(mainPanel, ref y, "报警状态");

            _lblAlarmG1 = new Label { Text = "G1 超阈值", ForeColor = CLR_MUTED, Font = new Font("微软雅黑", 8F), Location = new Point(8, y), AutoSize = true };
            _lblAlarmG2 = new Label { Text = "G2 超阈值", ForeColor = CLR_MUTED, Font = new Font("微软雅黑", 8F), Location = new Point(8, y + 18), AutoSize = true };
            _lblAlarmLoss = new Label { Text = "丢失波", ForeColor = CLR_MUTED, Font = new Font("微软雅黑", 8F), Location = new Point(8, y + 36), AutoSize = true };
            mainPanel.Controls.Add(_lblAlarmG1);
            mainPanel.Controls.Add(_lblAlarmG2);
            mainPanel.Controls.Add(_lblAlarmLoss);
            y += 56;

            // ====== 缺陷记录 ======
            AddSection(mainPanel, ref y, "缺陷记录");

            var btnMark = new Button
            {
                Text = "标记缺陷",
                FlatStyle = FlatStyle.Flat,
                BackColor = CLR_RED,
                ForeColor = Color.White,
                Font = new Font("微软雅黑", 8.5F, FontStyle.Bold),
                Location = new Point(8, y),
                Size = new Size(80, 24)
            };
            btnMark.Click += (s, e) =>
            {
                _defectCount++;
                _lstDefects.Items.Add($"#{_defectCount}  X:0.0  L:0.0");
            };
            mainPanel.Controls.Add(btnMark);

            var btnClear = new Button
            {
                Text = "清除",
                FlatStyle = FlatStyle.Flat,
                BackColor = BG_INPUT,
                ForeColor = CLR_MUTED,
                Font = new Font("微软雅黑", 8F),
                Location = new Point(92, y),
                Size = new Size(48, 24)
            };
            btnClear.FlatAppearance.BorderColor = CLR_BORDER;
            btnClear.Click += (s, e) => { _lstDefects.Items.Clear(); _defectCount = 0; };
            mainPanel.Controls.Add(btnClear);
            y += 28;

            _lstDefects = new ListBox
            {
                Location = new Point(8, y),
                Size = new Size(170, 80),
                BackColor = BG_INPUT,
                ForeColor = CLR_TEXT,
                Font = new Font("Consolas", 8F),
                BorderStyle = BorderStyle.FixedSingle
            };
            mainPanel.Controls.Add(_lstDefects);
            y += 84;

            // ====== 通信监控 ======
            AddSection(mainPanel, ref y, "通信监控");

            _lblTxRate = AddReadout(mainPanel, ref y, "TX", "0 pkt/s");
            _lblRxRate = AddReadout(mainPanel, ref y, "RX", "0 pkt/s");
            _lblLatency = AddReadout(mainPanel, ref y, "延迟", "0 ms");

            this.Controls.Add(mainPanel);
        }

        private void AddSection(Panel parent, ref int y, string title)
        {
            var lbl = new Label
            {
                Text = title,
                ForeColor = CLR_BLUE,
                Font = new Font("微软雅黑", 8.5F, FontStyle.Bold),
                Location = new Point(4, y),
                AutoSize = true
            };
            var line = new Panel
            {
                BackColor = CLR_BORDER,
                Location = new Point(4, y + 16),
                Size = new Size(180, 1)
            };
            parent.Controls.Add(lbl);
            parent.Controls.Add(line);
            y += 22;
        }

        private Label AddReadout(Panel parent, ref int y, string label, string value)
        {
            var lbl = new Label
            {
                Text = label,
                ForeColor = CLR_MUTED,
                Font = new Font("微软雅黑", 8F),
                Location = new Point(8, y + 2),
                AutoSize = true
            };
            var val = new Label
            {
                Text = value,
                ForeColor = CLR_TEXT,
                Font = new Font("Consolas", 9F),
                Location = new Point(70, y + 2),
                AutoSize = true
            };
            parent.Controls.Add(lbl);
            parent.Controls.Add(val);
            y += 20;
            return val;
        }
    }
}
