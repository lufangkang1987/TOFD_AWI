using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tofd_AWI.From.NewInspect
{
    // ============================================================
    // 文件: UcMotionBar.cs
    // 职责: 底部运动状态栏
    //       X/Y/Z 轴位置 + 速度 + JOG按钮 + 回零 + 扫描进度条
    // ============================================================

    public partial class UcMotionBar : UserControl
    {
        private static readonly Color BG_BAR    = Color.FromArgb(12, 16, 28);   // #0c101c 更暗
        private static readonly Color BG_INPUT   = Color.FromArgb(22, 30, 48);   // #161e30
        private static readonly Color CLR_TEXT   = Color.FromArgb(200, 210, 225); // #c8d2e1
        private static readonly Color CLR_MUTED  = Color.FromArgb(100, 115, 140); // #64738c
        private static readonly Color CLR_GREEN  = Color.FromArgb(34, 197, 94);   // #22c55e
        private static readonly Color CLR_RED    = Color.FromArgb(239, 68, 68);   // #ef4444
        private static readonly Color CLR_BLUE   = Color.FromArgb(56, 130, 246);  // #3882f6
        private static readonly Color CLR_AMBER  = Color.FromArgb(245, 158, 11);  // #f59e0b
        private static readonly Color CLR_BORDER = Color.FromArgb(30, 40, 60);    // #1e283c

        private double _posX = 0.0, _posY = 0.0, _posZ = 0.0;
        private int _scanProgress = 0;

        public UcMotionBar()
        {
            InitializeComponent();
            BackColor = BG_BAR;
            ForeColor = CLR_TEXT;
            Height = 36;
            Dock = DockStyle.Bottom;
            BuildBar();
        }

        private void BuildBar()
        {
            // X 轴位置
            var lblX = MakeAxisLabel("X:");
            lblX.Location = new Point(8, 8);
            this.Controls.Add(lblX);

            _lblXPos = MakeValueLabel("0.00");
            _lblXPos.Location = new Point(30, 8);
            _lblXPos.ForeColor = CLR_GREEN;
            this.Controls.Add(_lblXPos);

            // X+ / X- 按钮
            _btnXPlus = MakeJogButton("X+");
            _btnXPlus.Location = new Point(82, 4);
            _btnXPlus.Click += (s, e) => { _posX += 0.5; UpdateAxisDisplay(); };
            this.Controls.Add(_btnXPlus);

            _btnXMinus = MakeJogButton("X-");
            _btnXMinus.Location = new Point(118, 4);
            _btnXMinus.Click += (s, e) => { _posX -= 0.5; UpdateAxisDisplay(); };
            this.Controls.Add(_btnXMinus);

            // Y 轴位置
            var lblY = MakeAxisLabel("Y:");
            lblY.Location = new Point(158, 8);
            this.Controls.Add(lblY);

            _lblYPos = MakeValueLabel("0.00");
            _lblYPos.Location = new Point(180, 8);
            _lblYPos.ForeColor = CLR_GREEN;
            this.Controls.Add(_lblYPos);

            _btnYPlus = MakeJogButton("Y+");
            _btnYPlus.Location = new Point(232, 4);
            _btnYPlus.Click += (s, e) => { _posY += 0.5; UpdateAxisDisplay(); };
            this.Controls.Add(_btnYPlus);

            _btnYMinus = MakeJogButton("Y-");
            _btnYMinus.Location = new Point(268, 4);
            _btnYMinus.Click += (s, e) => { _posY -= 0.5; UpdateAxisDisplay(); };
            this.Controls.Add(_btnYMinus);

            // Z 轴位置
            var lblZ = MakeAxisLabel("Z:");
            lblZ.Location = new Point(308, 8);
            this.Controls.Add(lblZ);

            _lblZPos = MakeValueLabel("0.00");
            _lblZPos.Location = new Point(330, 8);
            _lblZPos.ForeColor = CLR_GREEN;
            this.Controls.Add(_lblZPos);

            _btnZPlus = MakeJogButton("Z+");
            _btnZPlus.Location = new Point(382, 4);
            _btnZPlus.Click += (s, e) => { _posZ += 0.5; UpdateAxisDisplay(); };
            this.Controls.Add(_btnZPlus);

            _btnZMinus = MakeJogButton("Z-");
            _btnZMinus.Location = new Point(418, 4);
            _btnZMinus.Click += (s, e) => { _posZ -= 0.5; UpdateAxisDisplay(); };
            this.Controls.Add(_btnZMinus);

            // 分隔线
            var sep1 = new Panel { BackColor = CLR_BORDER, Location = new Point(458, 4), Size = new Size(1, 28) };
            this.Controls.Add(sep1);

            // 速度显示
            var lblSpeed = new Label { Text = "速度:", ForeColor = CLR_MUTED, Font = new Font("Consolas", 8.5F), Location = new Point(468, 8), AutoSize = true, BackColor = Color.Transparent };
            this.Controls.Add(lblSpeed);

            _lblSpeed = new Label { Text = "0%", ForeColor = CLR_AMBER, Font = new Font("Consolas", 9F, FontStyle.Bold), Location = new Point(508, 8), AutoSize = true, BackColor = Color.Transparent };
            this.Controls.Add(_lblSpeed);

            // 分隔线
            var sep2 = new Panel { BackColor = CLR_BORDER, Location = new Point(548, 4), Size = new Size(1, 28) };
            this.Controls.Add(sep2);

            // 回零按钮
            _btnGoHome = new Button { Text = "回零", FlatStyle = FlatStyle.Flat, BackColor = CLR_BLUE, ForeColor = Color.White, Font = new Font("微软雅黑", 8.5F, FontStyle.Bold), Size = new Size(52, 24), Location = new Point(558, 6) };
            _btnGoHome.FlatAppearance.BorderSize = 0;
            _btnGoHome.Click += (s, e) => { _posX = 0; _posY = 0; _posZ = 0; UpdateAxisDisplay(); };
            this.Controls.Add(_btnGoHome);

            // JOG 模式按钮
            _btnJogMode = new Button { Text = "JOG", FlatStyle = FlatStyle.Flat, BackColor = BG_INPUT, ForeColor = CLR_MUTED, Font = new Font("Consolas", 8.5F), Size = new Size(44, 24), Location = new Point(616, 6) };
            _btnJogMode.FlatAppearance.BorderColor = CLR_BORDER;
            _btnJogMode.FlatAppearance.BorderSize = 1;
            this.Controls.Add(_btnJogMode);

            // 分隔线
            var sep3 = new Panel { BackColor = CLR_BORDER, Location = new Point(668, 4), Size = new Size(1, 28) };
            this.Controls.Add(sep3);

            // 扫描进度条
            _progressScan = new ProgressBar { Location = new Point(678, 8), Size = new Size(280, 20), Style = ProgressBarStyle.Continuous, BackColor = BG_INPUT, ForeColor = CLR_BLUE };
            this.Controls.Add(_progressScan);

            _lblProgressPct = new Label { Text = "0%", ForeColor = CLR_TEXT, Font = new Font("Consolas", 8.5F), Location = new Point(964, 8), AutoSize = true, BackColor = Color.Transparent };
            this.Controls.Add(_lblProgressPct);

            // 分隔线
            var sep4 = new Panel { BackColor = CLR_BORDER, Location = new Point(1000, 4), Size = new Size(1, 28) };
            this.Controls.Add(sep4);

            // 状态指示灯
            _pnlStatus = new Panel { Location = new Point(1010, 8), Size = new Size(10, 10), BackColor = CLR_GREEN };
            this.Controls.Add(_pnlStatus);

            _lblStatusText = new Label { Text = "就绪", ForeColor = CLR_GREEN, Font = new Font("微软雅黑", 8.5F), Location = new Point(1026, 8), AutoSize = true, BackColor = Color.Transparent };
            this.Controls.Add(_lblStatusText);
        }

        private Label MakeAxisLabel(string text)
        {
            return new Label { Text = text, ForeColor = CLR_MUTED, Font = new Font("Consolas", 9F), AutoSize = true, BackColor = Color.Transparent };
        }

        private Label MakeValueLabel(string text)
        {
            return new Label { Text = text, ForeColor = CLR_GREEN, Font = new Font("Consolas", 9F, FontStyle.Bold), AutoSize = true, BackColor = Color.Transparent };
        }

        private Button MakeJogButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                BackColor = BG_INPUT,
                ForeColor = CLR_TEXT,
                Font = new Font("Consolas", 8F),
                Size = new Size(32, 24)
            };
            btn.FlatAppearance.BorderColor = CLR_BORDER;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.MouseOverBackColor = CLR_BLUE;
            return btn;
        }

        private void UpdateAxisDisplay()
        {
            _lblXPos.Text = $"{_posX:F2}";
            _lblYPos.Text = $"{_posY:F2}";
            _lblZPos.Text = $"{_posZ:F2}";
        }

        // =========================================
        // 公共方法 (供外部 Service 调用)
        // =========================================
        public void SetAxisPosition(double x, double y, double z)
        {
            _posX = x; _posY = y; _posZ = z;
            UpdateAxisDisplay();
        }

        public void SetSpeed(int percent)
        {
            _lblSpeed.Text = $"{percent}%";
        }

        public void SetScanProgress(int percent)
        {
            _scanProgress = Math.Max(0, Math.Min(100, percent));
            _progressScan.Value = _scanProgress;
            _lblProgressPct.Text = $"{_scanProgress}%";
        }

        public void SetStatus(bool isRunning, string text = "")
        {
            _pnlStatus.BackColor = isRunning ? CLR_GREEN : CLR_RED;
            _lblStatusText.Text = string.IsNullOrEmpty(text) ? (isRunning ? "运行中" : "就绪") : text;
            _lblStatusText.ForeColor = isRunning ? CLR_GREEN : CLR_RED;
        }

        // 控件字段
        private Label _lblXPos, _lblYPos, _lblZPos;
        private Label _lblSpeed, _lblProgressPct;
        private Label _lblStatusText;
        private Button _btnXPlus, _btnXMinus;
        private Button _btnYPlus, _btnYMinus;
        private Button _btnZPlus, _btnZMinus;
        private Button _btnGoHome, _btnJogMode;
        private ProgressBar _progressScan;
        private Panel _pnlStatus;
    }
}
