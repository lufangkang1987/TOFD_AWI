namespace Tofd_AWI.From.NewInspect
{
    partial class Frm_NewInspect
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this._topBar = new System.Windows.Forms.Panel();
            this._tabBar = new System.Windows.Forms.Panel();
            this._lblTitle = new System.Windows.Forms.Label();
            this._panelComStatus = new System.Windows.Forms.Panel();
            this._lblJsonPort = new System.Windows.Forms.Label();
            this._lblMtldPort = new System.Windows.Forms.Label();
            this._lblCanBus = new System.Windows.Forms.Label();
            this._splitLeftRight = new System.Windows.Forms.SplitContainer();
            this._paramPanel = new System.Windows.Forms.Panel();
            this._splitCenterRight = new System.Windows.Forms.SplitContainer();
            this._waveformView = new Tofd_AWI.From.NewInspect.UcWaveformView();
            this._rightPanel = new Tofd_AWI.From.NewInspect.UcRightPanel();
            this._bottomBar = new Tofd_AWI.From.NewInspect.UcMotionBar();
            this._toolStrip = new System.Windows.Forms.Panel();
            this._btnLayout2x2 = new System.Windows.Forms.Button();
            this._btnLayout1p1 = new System.Windows.Forms.Button();
            this._btnLayoutFull = new System.Windows.Forms.Button();
            this._btnFreeze = new System.Windows.Forms.Button();
            this._btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // ==========================================
            // 顶栏 (40px)
            // ==========================================
            this._topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this._topBar.Location = new System.Drawing.Point(0, 0);
            this._topBar.Name = "_topBar";
            this._topBar.Size = new System.Drawing.Size(1280, 40);
            this._topBar.BackColor = System.Drawing.Color.FromArgb(22, 28, 38);
            this._topBar.Controls.Add(this._lblTitle);
            this._topBar.Controls.Add(this._tabBar);
            this._topBar.Controls.Add(this._panelComStatus);

            // 标题
            this._lblTitle.Text = "NewInspect";
            this._lblTitle.Font = new System.Drawing.Font("Consolas", 13F, System.Drawing.FontStyle.Bold);
            this._lblTitle.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this._lblTitle.Location = new System.Drawing.Point(12, 8);
            this._lblTitle.Size = new System.Drawing.Size(140, 24);
            this._lblTitle.BackColor = System.Drawing.Color.Transparent;

            // 工作台标签栏
            this._tabBar.Location = new System.Drawing.Point(160, 4);
            this._tabBar.Size = new System.Drawing.Size(600, 32);
            this._tabBar.BackColor = System.Drawing.Color.Transparent;
            var tabNames = new[] { "检测", "设置", "分析", "运动", "报告", "数据" };
            var tabTags  = new[] { "inspect", "settings", "analysis", "motion", "report", "data" };
            for (int i = 0; i < tabNames.Length; i++)
            {
                var btn = new System.Windows.Forms.Button();
                btn.Text = tabNames[i];
                btn.Tag = tabTags[i];
                btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new System.Drawing.Font("微软雅黑", 9.5F);
                btn.Size = new System.Drawing.Size(72, 28);
                btn.Location = new System.Drawing.Point(i * 78, 2);
                btn.BackColor = (i == 0) ? System.Drawing.Color.FromArgb(59, 130, 246) : System.Drawing.Color.FromArgb(22, 28, 38);
                btn.ForeColor = (i == 0) ? System.Drawing.Color.White : System.Drawing.Color.FromArgb(148, 163, 184);
                btn.Click += new System.EventHandler(this.TabBtn_Click);
                this._tabBar.Controls.Add(btn);
            }

            // 通信状态面板(右上角)
            this._panelComStatus.Location = new System.Drawing.Point(1000, 6);
            this._panelComStatus.Size = new System.Drawing.Size(270, 28);
            this._panelComStatus.BackColor = System.Drawing.Color.Transparent;

            this._lblJsonPort = new System.Windows.Forms.Label();
            this._lblJsonPort.Text = "51007 JSON";
            this._lblJsonPort.Font = new System.Drawing.Font("Consolas", 8.5F);
            this._lblJsonPort.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this._lblJsonPort.Location = new System.Drawing.Point(0, 4);
            this._lblJsonPort.Size = new System.Drawing.Size(85, 20);
            this._lblJsonPort.BackColor = System.Drawing.Color.Transparent;

            this._lblMtldPort = new System.Windows.Forms.Label();
            this._lblMtldPort.Text = "51005 MTLD";
            this._lblMtldPort.Font = new System.Drawing.Font("Consolas", 8.5F);
            this._lblMtldPort.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this._lblMtldPort.Location = new System.Drawing.Point(90, 4);
            this._lblMtldPort.Size = new System.Drawing.Size(90, 20);
            this._lblMtldPort.BackColor = System.Drawing.Color.Transparent;

            this._lblCanBus = new System.Windows.Forms.Label();
            this._lblCanBus.Text = "CAN BUS";
            this._lblCanBus.Font = new System.Drawing.Font("Consolas", 8.5F);
            this._lblCanBus.ForeColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this._lblCanBus.Location = new System.Drawing.Point(186, 4);
            this._lblCanBus.Size = new System.Drawing.Size(80, 20);
            this._lblCanBus.BackColor = System.Drawing.Color.Transparent;

            this._panelComStatus.Controls.Add(this._lblJsonPort);
            this._panelComStatus.Controls.Add(this._lblMtldPort);
            this._panelComStatus.Controls.Add(this._lblCanBus);

            // ==========================================
            // 工具栏 (32px) — 布局切换 + 冻结/开始
            // ==========================================
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this._toolStrip.Location = new System.Drawing.Point(0, 40);
            this._toolStrip.Size = new System.Drawing.Size(1280, 32);
            this._toolStrip.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._toolStrip.Padding = new System.Windows.Forms.Padding(8, 2, 8, 2);

            this._btnLayout2x2 = new System.Windows.Forms.Button();
            this._btnLayout2x2.Text = "2x2";
            this._btnLayout2x2.Tag = "2x2";
            this._btnLayout2x2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnLayout2x2.FlatAppearance.BorderSize = 1;
            this._btnLayout2x2.Font = new System.Drawing.Font("Consolas", 8.5F);
            this._btnLayout2x2.Size = new System.Drawing.Size(40, 24);
            this._btnLayout2x2.Location = new System.Drawing.Point(8, 4);
            this._btnLayout2x2.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this._btnLayout2x2.ForeColor = System.Drawing.Color.White;
            this._btnLayout2x2.Click += (s, e) => _waveformView.SetLayout("2x2");

            this._btnLayout1p1 = new System.Windows.Forms.Button();
            this._btnLayout1p1.Text = "1+1";
            this._btnLayout1p1.Tag = "1p1";
            this._btnLayout1p1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnLayout1p1.FlatAppearance.BorderSize = 1;
            this._btnLayout1p1.Font = new System.Drawing.Font("Consolas", 8.5F);
            this._btnLayout1p1.Size = new System.Drawing.Size(40, 24);
            this._btnLayout1p1.Location = new System.Drawing.Point(52, 4);
            this._btnLayout1p1.BackColor = System.Drawing.Color.FromArgb(40, 52, 72);
            this._btnLayout1p1.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this._btnLayout1p1.Click += (s, e) => _waveformView.SetLayout("1p1");

            this._btnLayoutFull = new System.Windows.Forms.Button();
            this._btnLayoutFull.Text = "Full";
            this._btnLayoutFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnLayoutFull.FlatAppearance.BorderSize = 1;
            this._btnLayoutFull.Font = new System.Drawing.Font("Consolas", 8.5F);
            this._btnLayoutFull.Size = new System.Drawing.Size(40, 24);
            this._btnLayoutFull.Location = new System.Drawing.Point(96, 4);
            this._btnLayoutFull.BackColor = System.Drawing.Color.FromArgb(40, 52, 72);
            this._btnLayoutFull.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this._btnLayoutFull.Click += (s, e) => _waveformView.SetLayout("full");

            this._btnFreeze = new System.Windows.Forms.Button();
            this._btnFreeze.Text = "冻结";
            this._btnFreeze.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnFreeze.FlatAppearance.BorderSize = 1;
            this._btnFreeze.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this._btnFreeze.Size = new System.Drawing.Size(52, 24);
            this._btnFreeze.Location = new System.Drawing.Point(160, 4);
            this._btnFreeze.BackColor = System.Drawing.Color.FromArgb(40, 52, 72);
            this._btnFreeze.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);

            this._btnStart = new System.Windows.Forms.Button();
            this._btnStart.Text = "开始";
            this._btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnStart.FlatAppearance.BorderSize = 1;
            this._btnStart.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this._btnStart.Size = new System.Drawing.Size(52, 24);
            this._btnStart.Location = new System.Drawing.Point(216, 4);
            this._btnStart.BackColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this._btnStart.ForeColor = System.Drawing.Color.White;

            this._toolStrip.Controls.Add(this._btnLayout2x2);
            this._toolStrip.Controls.Add(this._btnLayout1p1);
            this._toolStrip.Controls.Add(this._btnLayoutFull);
            this._toolStrip.Controls.Add(this._btnFreeze);
            this._toolStrip.Controls.Add(this._btnStart);

            // ==========================================
            // 底部运动状态栏 (36px)
            // ==========================================
            this._bottomBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomBar.Location = new System.Drawing.Point(0, 724);
            this._bottomBar.Size = new System.Drawing.Size(1280, 36);
            this._bottomBar.Name = "_bottomBar";

            // ==========================================
            // 主内容区 (三栏 SplitContainer)
            // ==========================================

            // 外层: 左面板 | (中+右)
            this._splitLeftRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitLeftRight.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this._splitLeftRight.Location = new System.Drawing.Point(0, 72);
            this._splitLeftRight.Name = "_splitLeftRight";
            this._splitLeftRight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this._splitLeftRight.SplitterDistance = 190;
            this._splitLeftRight.SplitterWidth = 4;
            this._splitLeftRight.Panel1.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._splitLeftRight.Panel1MinSize = 140;
            this._splitLeftRight.Panel2.BackColor = System.Drawing.Color.FromArgb(15, 18, 25);

            // 左侧参数面板
            this._paramPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._paramPanel.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._paramPanel.Padding = new System.Windows.Forms.Padding(4);
            this._splitLeftRight.Panel1.Controls.Add(this._paramPanel);

            // 内层: 中间视图 | 右侧面板
            this._splitCenterRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitCenterRight.BackColor = System.Drawing.Color.FromArgb(51, 65, 85);
            this._splitCenterRight.Name = "_splitCenterRight";
            this._splitCenterRight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this._splitCenterRight.SplitterDistance = 780;
            this._splitCenterRight.SplitterWidth = 3;
            this._splitCenterRight.Panel1.BackColor = System.Drawing.Color.FromArgb(15, 18, 25);
            this._splitCenterRight.Panel2.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);

            // 中间波形视图
            this._waveformView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._waveformView.BackColor = System.Drawing.Color.FromArgb(15, 18, 25);
            this._splitCenterRight.Panel1.Controls.Add(this._waveformView);

            // 右侧控制面板
            this._rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._rightPanel.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this._splitCenterRight.Panel2.Controls.Add(this._rightPanel);

            this._splitLeftRight.Panel2.Controls.Add(this._splitCenterRight);

            // ==========================================
            // 窗体属性
            // ==========================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 760);
            this.Controls.Add(this._splitLeftRight);
            this.Controls.Add(this._bottomBar);
            this.Controls.Add(this._toolStrip);
            this.Controls.Add(this._topBar);
            this.MinimumSize = new System.Drawing.Size(1024, 600);
            this.Name = "Frm_NewInspect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewInspect - 无损检测系统";
            this.BackColor = System.Drawing.Color.FromArgb(15, 18, 25);
            this.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.ResumeLayout(false);
        }

        #endregion

        // 顶栏
        private System.Windows.Forms.Panel _topBar;
        private System.Windows.Forms.Panel _tabBar;
        private System.Windows.Forms.Label _lblTitle;
        private System.Windows.Forms.Panel _panelComStatus;
        private System.Windows.Forms.Label _lblJsonPort;
        private System.Windows.Forms.Label _lblMtldPort;
        private System.Windows.Forms.Label _lblCanBus;

        // 工具栏
        private System.Windows.Forms.Panel _toolStrip;
        private System.Windows.Forms.Button _btnLayout2x2;
        private System.Windows.Forms.Button _btnLayout1p1;
        private System.Windows.Forms.Button _btnLayoutFull;
        private System.Windows.Forms.Button _btnFreeze;
        private System.Windows.Forms.Button _btnStart;

        // 三栏主体
        private System.Windows.Forms.SplitContainer _splitLeftRight;
        private System.Windows.Forms.SplitContainer _splitCenterRight;
        private System.Windows.Forms.Panel _paramPanel;
        private UcWaveformView _waveformView;
        private UcRightPanel _rightPanel;

        // 底栏
        private UcMotionBar _bottomBar;
    }
}
