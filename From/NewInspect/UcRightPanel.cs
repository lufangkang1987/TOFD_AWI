using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tofd_AWI.From.NewInspect
{
    // ============================================================
    // 文件: UcRightPanel.cs
    // 职责: 右侧控制面板
    //       增益快捷控制 + 闸门读数 + 报警状态 + 缺陷记录
    // ============================================================

    public partial class UcRightPanel : UserControl
    {
        // 截图色系
        private static readonly Color BG_PANEL   = Color.FromArgb(16, 22, 36);   // #101624
        private static readonly Color BG_CELL    = Color.FromArgb(22, 30, 48);   // #161e30
        private static readonly Color BG_INPUT   = Color.FromArgb(30, 40, 62);   // #1e283e
        private static readonly Color CLR_TEXT   = Color.FromArgb(200, 210, 225); // #c8d2e1
        private static readonly Color CLR_MUTED  = Color.FromArgb(100, 115, 140); // #64738c
        private static readonly Color CLR_BLUE   = Color.FromArgb(56, 130, 246);  // #3882f6
        private static readonly Color CLR_GREEN  = Color.FromArgb(34, 197, 94);   // #22c55e
        private static readonly Color CLR_RED    = Color.FromArgb(239, 68, 68);   // #ef4444
        private static readonly Color CLR_AMBER  = Color.FromArgb(245, 158, 11);  // #f59e0b
        private static readonly Color CLR_BORDER = Color.FromArgb(30, 40, 60);    // #1e283c

        private int _gainValue = 40;
        private int _defectCount = 0;

        private Label _lblGain;
        private TrackBar _trackGain;
        private Label _lblG1Peak, _lblG1Pos;
        private Label _lblG2Peak, _lblG2Pos;
        private Panel _pnlAlarmG1, _pnlAlarmG2, _pnlAlarmLoss;
        private ListBox _lstDefects;

        public UcRightPanel()
        {
            InitializeComponent();
            BuildContent();
        }

        private void BuildContent()
        {
            var mainPanel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = BG_PANEL };
            int y = 6;

            // ====== 增益控制 ======
            AddSection(mainPanel, ref y, "增益");

            _lblGain = new Label
            {
                Text = "40.0 dB",
                Font = new Font("Consolas", 20F, FontStyle.Bold),
                ForeColor = CLR_BLUE,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(8, y),
                Size = new Size(180, 36)
            };
            mainPanel.Controls.Add(_lblGain);
            y += 40;

            // 滑块
            _trackGain = new TrackBar
            {
                Minimum = 0, Maximum = 80, Value = 40,
                TickFrequency = 10,
                Location = new Point(8, y),
                Size = new Size(180, 28),
                BackColor = BG_PANEL
            };
            _trackGain.ValueChanged += (s, e) =>
            {
                _gainValue = _trackGain.Value;
                _lblGain.Text = $"{_gainValue}.0 dB";
            };
            mainPanel.Controls.Add(_trackGain);
            y += 32;

            // +/- 按钮 — 紧凑排列
            var gains = new[] { "-6", "-1", "+1", "+6" };
            var deltas = new[] { -6, -1, 1, 6 };
            for (int i = 0; i < 4; i++)
            {
                var btn = new Button
                {
                    Text = gains[i],
                    FlatStyle = FlatStyle.Flat,
                    BackColor = BG_CELL,
                    ForeColor = (deltas[i] > 0) ? CLR_GREEN : CLR_AMBER,
                    Font = new Font("Consolas", 9F, FontStyle.Bold),
                    Location = new Point(8 + i * 46, y),
                    Size = new Size(42, 24)
                };
                btn.FlatAppearance.BorderColor = CLR_BORDER;
                btn.FlatAppearance.BorderSize = 1;
                int delta = deltas[i];
                btn.Click += (s, e) =>
                {
                    _gainValue = Math.Max(0, Math.Min(80, _gainValue + delta));
                    _trackGain.Value = _gainValue;
                };
                mainPanel.Controls.Add(btn);
            }
            y += 32;

            // ====== 闸门读数 — 卡片式 2x2 布局 ======
            AddSection(mainPanel, ref y, "闸门读数");

            // G1 峰值卡片
            var cardG1Peak = MakeReadoutCard("G1 峰值", "68%", CLR_GREEN);
            cardG1Peak.Location = new Point(8, y);
            mainPanel.Controls.Add(cardG1Peak);

            // G1 位置卡片
            var cardG1Pos = MakeReadoutCard("G1 位置", "22.3", CLR_TEXT);
            cardG1Pos.Location = new Point(100, y);
            mainPanel.Controls.Add(cardG1Pos);
            y += 46;

            // G2 峰值卡片
            var cardG2Peak = MakeReadoutCard("G2 峰值", "35%", CLR_RED);
            cardG2Peak.Location = new Point(8, y);
            mainPanel.Controls.Add(cardG2Peak);

            // G2 位置卡片
            var cardG2Pos = MakeReadoutCard("G2 位置", "45.1", CLR_TEXT);
            cardG2Pos.Location = new Point(100, y);
            mainPanel.Controls.Add(cardG2Pos);
            y += 52;

            // ====== 报警状态 — 指示灯样式 ======
            AddSection(mainPanel, ref y, "报警");

            _pnlAlarmG1 = MakeAlarmRow(mainPanel, ref y, "G1 超阈值", true);
            _pnlAlarmG2 = MakeAlarmRow(mainPanel, ref y, "G2 超阈值", true);
            _pnlAlarmLoss = MakeAlarmRow(mainPanel, ref y, "丢失波", false);
            y += 4;

            // ====== 缺陷记录 ======
            AddSection(mainPanel, ref y, "缺陷记录");

            var btnMark = new Button
            {
                Text = "标记缺陷",
                FlatStyle = FlatStyle.Flat,
                BackColor = BG_CELL,
                ForeColor = CLR_TEXT,
                Font = new Font("微软雅黑", 8.5F),
                Location = new Point(8, y),
                Size = new Size(180, 26)
            };
            btnMark.FlatAppearance.BorderColor = CLR_BORDER;
            btnMark.FlatAppearance.BorderSize = 1;
            btnMark.Click += (s, e) =>
            {
                _defectCount++;
                _lstDefects.Items.Add($"#{_defectCount}  X:0.0  L:0.0");
            };
            mainPanel.Controls.Add(btnMark);
            y += 32;

            _lstDefects = new ListBox
            {
                Location = new Point(8, y),
                Size = new Size(180, 60),
                BackColor = BG_CELL,
                ForeColor = CLR_TEXT,
                Font = new Font("Consolas", 8F),
                BorderStyle = BorderStyle.FixedSingle
            };
            mainPanel.Controls.Add(_lstDefects);

            this.Controls.Add(mainPanel);
        }

        /// <summary>
        /// 创建读数卡片（2x2布局中的小卡片）
        /// </summary>
        private Panel MakeReadoutCard(string label, string value, Color valueColor)
        {
            var card = new Panel
            {
                Size = new Size(88, 42),
                BackColor = BG_CELL
            };

            var lblLabel = new Label
            {
                Text = label,
                ForeColor = CLR_MUTED,
                Font = new Font("微软雅黑", 7.5F),
                Location = new Point(4, 2),
                AutoSize = true
            };

            var lblValue = new Label
            {
                Text = value,
                ForeColor = valueColor,
                Font = new Font("Consolas", 11F, FontStyle.Bold),
                Location = new Point(4, 18),
                AutoSize = true
            };

            card.Controls.Add(lblValue);
            card.Controls.Add(lblLabel);
            return card;
        }

        /// <summary>
        /// 创建报警行（文字 + 指示灯）
        /// </summary>
        private Panel MakeAlarmRow(Panel parent, ref int y, string text, bool isAlarm)
        {
            var row = new Panel
            {
                Location = new Point(8, y),
                Size = new Size(180, 22),
                BackColor = Color.Transparent
            };

            var lblText = new Label
            {
                Text = text,
                ForeColor = CLR_MUTED,
                Font = new Font("微软雅黑", 8F),
                Location = new Point(0, 2),
                AutoSize = true
            };

            var indicator = new Panel
            {
                Location = new Point(164, 5),
                Size = new Size(10, 10),
                BackColor = isAlarm ? CLR_RED : CLR_GREEN
            };

            row.Controls.Add(indicator);
            row.Controls.Add(lblText);
            parent.Controls.Add(row);
            y += 22;
            return indicator;
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
                Size = new Size(188, 1)
            };
            parent.Controls.Add(lbl);
            parent.Controls.Add(line);
            y += 22;
        }
    }
}
