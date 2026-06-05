namespace Tofd_AWI.From.NewInspect
{
    partial class Frm_NewInspect
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            _connMonitorTimer?.Stop();
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer

        private void InitializeComponent()
        {
            // ==========================================
            // 配色常量
            // ==========================================
            var cBgDark    = System.Drawing.Color.FromArgb(10, 14, 26);
            var cBgPanel   = System.Drawing.Color.FromArgb(16, 22, 36);
            var cBgInput   = System.Drawing.Color.FromArgb(24, 30, 44);
            var cText      = System.Drawing.Color.FromArgb(200, 210, 225);
            var cMuted     = System.Drawing.Color.FromArgb(100, 115, 140);
            var cBlue      = System.Drawing.Color.FromArgb(56, 130, 246);
            var cGreen     = System.Drawing.Color.FromArgb(34, 197, 94);
            var cRed       = System.Drawing.Color.FromArgb(239, 68, 68);
            var cAmber     = System.Drawing.Color.FromArgb(245, 158, 11);
            var cBorder    = System.Drawing.Color.FromArgb(30, 40, 60);
            var cScanner   = System.Drawing.Color.FromArgb(20, 60, 30);
            var fontSize9  = new System.Drawing.Font("微软雅黑", 9F);
            var fontSize85 = new System.Drawing.Font("微软雅黑", 8.5F);
            var fontSize8  = new System.Drawing.Font("微软雅黑", 8F);
            var fontMono8  = new System.Drawing.Font("Consolas", 8F);
            var fontMono85 = new System.Drawing.Font("Consolas", 8.5F);

            this.SuspendLayout();

            // ==========================================
            // 顶栏 _topBar  (44px)
            // ==========================================
            this._topBar = new System.Windows.Forms.Panel();
            this._topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this._topBar.Height = 44;
            this._topBar.BackColor = cBgDark;

            // "连接设备" 按钮
            this._btnConnect = new System.Windows.Forms.Button();
            this._btnConnect.Text = "连接设备";
            this._btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnConnect.FlatAppearance.BorderSize = 0;
            this._btnConnect.Font = fontSize9;
            this._btnConnect.Size = new System.Drawing.Size(80, 32);
            this._btnConnect.Location = new System.Drawing.Point(6, 6);
            this._btnConnect.BackColor = cBlue;
            this._btnConnect.ForeColor = System.Drawing.Color.White;
            this._btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this._topBar.Controls.Add(this._btnConnect);

            // "设置" 按钮
            this._btnSettings = new System.Windows.Forms.Button();
            this._btnSettings.Text = "设置";
            this._btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnSettings.FlatAppearance.BorderSize = 1;
            this._btnSettings.FlatAppearance.BorderColor = cBorder;
            this._btnSettings.Font = fontSize9;
            this._btnSettings.Size = new System.Drawing.Size(60, 32);
            this._btnSettings.Location = new System.Drawing.Point(92, 6);
            this._btnSettings.BackColor = cBgPanel;
            this._btnSettings.ForeColor = cText;
            this._btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this._topBar.Controls.Add(this._btnSettings);

            // 分隔线1
            var sep1 = new System.Windows.Forms.Label();
            sep1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            sep1.Size = new System.Drawing.Size(2, 24);
            sep1.Location = new System.Drawing.Point(160, 10);
            this._topBar.Controls.Add(sep1);

            // 显示模式按钮组: A扫 / B扫 / C扫 / TFM
            this._viewModeBar = new System.Windows.Forms.Panel();
            this._viewModeBar.Location = new System.Drawing.Point(170, 4);
            this._viewModeBar.Size = new System.Drawing.Size(240, 36);
            this._viewModeBar.BackColor = System.Drawing.Color.Transparent;
            var viewNames = new[] { "A扫", "B扫", "C扫", "TFM" };
            var viewTags  = new[] { "ascan", "bscan", "cscan", "tfm" };
            for (int i = 0; i < viewNames.Length; i++)
            {
                var btn = new System.Windows.Forms.Button();
                btn.Text = viewNames[i];
                btn.Tag = viewTags[i];
                btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = fontSize85;
                btn.Size = new System.Drawing.Size(52, 28);
                btn.Location = new System.Drawing.Point(i * 58, 4);
                btn.BackColor = (i == 0) ? cBlue : cBgDark;
                btn.ForeColor = (i == 0) ? System.Drawing.Color.White : cMuted;
                btn.Cursor = System.Windows.Forms.Cursors.Hand;
                btn.Click += new System.EventHandler(this.ViewModeBtn_Click);
                this._viewModeBar.Controls.Add(btn);
            }
            this._topBar.Controls.Add(this._viewModeBar);

            // 分隔线2
            var sep2 = new System.Windows.Forms.Label();
            sep2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            sep2.Size = new System.Drawing.Size(2, 24);
            sep2.Location = new System.Drawing.Point(418, 10);
            this._topBar.Controls.Add(sep2);

            // 显示切换 CheckBox: 闸门 / TCG / 包络 / 峰值保持 (统一80x20)
            int chkX = 426;
            this._chkGate = MakeTopCheckBox("闸门", true, chkX, 10);
            this._chkTcg = MakeTopCheckBox("TCG", false, chkX + 80, 10);
            this._chkEnvelope = MakeTopCheckBox("包络", false, chkX + 160, 10);
            this._chkPeakHold = MakeTopCheckBox("峰值保持", false, chkX + 240, 10);

            // "快速应用" 标签
            this._lblQuickApply = new System.Windows.Forms.Label();
            this._lblQuickApply.Text = "快速应用";
            this._lblQuickApply.Font = fontSize85;
            this._lblQuickApply.ForeColor = cMuted;
            this._lblQuickApply.Size = new System.Drawing.Size(60, 20);
            this._lblQuickApply.Location = new System.Drawing.Point(chkX + 320, 10);
            this._lblQuickApply.BackColor = System.Drawing.Color.Transparent;
            this._lblQuickApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this._topBar.Controls.Add(this._lblQuickApply);

            // 右侧通信状态
            this._lblComStatus = new System.Windows.Forms.Label();
            this._lblComStatus.Text = "○ 未连接";
            this._lblComStatus.Font = fontSize85;
            this._lblComStatus.ForeColor = cRed;
            this._lblComStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._lblComStatus.BackColor = System.Drawing.Color.Transparent;
            this._lblComStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this._topBar.Controls.Add(this._lblComStatus);

            // ==========================================
            // 底栏 _bottomBar  (38px)
            // ==========================================
            this._bottomBar = new System.Windows.Forms.Panel();
            this._bottomBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomBar.Height = 38;
            this._bottomBar.BackColor = cBgPanel;

            // 位置显示 X/Y
            this._lblPosX = new System.Windows.Forms.Label();
            this._lblPosX.Text = "X: 0.0 mm";
            this._lblPosX.Font = fontMono85;
            this._lblPosX.ForeColor = cGreen;
            this._lblPosX.Size = new System.Drawing.Size(100, 18);
            this._lblPosX.Location = new System.Drawing.Point(8, 2);
            this._lblPosX.BackColor = System.Drawing.Color.Transparent;
            this._bottomBar.Controls.Add(this._lblPosX);

            this._lblPosY = new System.Windows.Forms.Label();
            this._lblPosY.Text = "Y: 0.0 mm";
            this._lblPosY.Font = fontMono85;
            this._lblPosY.ForeColor = cGreen;
            this._lblPosY.Size = new System.Drawing.Size(100, 18);
            this._lblPosY.Location = new System.Drawing.Point(8, 19);
            this._lblPosY.BackColor = System.Drawing.Color.Transparent;
            this._bottomBar.Controls.Add(this._lblPosY);

            // JOG 按钮组
            int jogX = 120;
            this._btnJogLeft       = MakeJogBtn("<<", jogX, 6, cBgInput, cText);
            this._btnJogStepLeft   = MakeJogBtn("<", jogX + 30, 6, cBgInput, cText);
            this._btnJogStepRight  = MakeJogBtn(">", jogX + 60, 6, cBgInput, cText);
            this._btnJogRight      = MakeJogBtn(">>", jogX + 90, 6, cBgInput, cText);

            // 回零
            this._btnHome = new System.Windows.Forms.Button();
            this._btnHome.Text = "回零";
            this._btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnHome.FlatAppearance.BorderSize = 1;
            this._btnHome.FlatAppearance.BorderColor = cBorder;
            this._btnHome.Font = fontSize85;
            this._btnHome.Size = new System.Drawing.Size(44, 26);
            this._btnHome.Location = new System.Drawing.Point(jogX + 128, 6);
            this._btnHome.BackColor = cBgInput;
            this._btnHome.ForeColor = cText;
            this._btnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this._bottomBar.Controls.Add(this._btnHome);

            // 急停
            this._btnEstop = new System.Windows.Forms.Button();
            this._btnEstop.Text = "急停";
            this._btnEstop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnEstop.FlatAppearance.BorderSize = 1;
            this._btnEstop.FlatAppearance.BorderColor = cRed;
            this._btnEstop.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this._btnEstop.Size = new System.Drawing.Size(44, 26);
            this._btnEstop.Location = new System.Drawing.Point(jogX + 178, 6);
            this._btnEstop.BackColor = System.Drawing.Color.FromArgb(60, 20, 20);
            this._btnEstop.ForeColor = cRed;
            this._btnEstop.Cursor = System.Windows.Forms.Cursors.Hand;
            this._bottomBar.Controls.Add(this._btnEstop);

            // 扫查进度条
            this._scanProgress = new System.Windows.Forms.ProgressBar();
            this._scanProgress.Size = new System.Drawing.Size(120, 14);
            this._scanProgress.Location = new System.Drawing.Point(jogX + 235, 12);
            this._scanProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._scanProgress.Value = 0;
            this._bottomBar.Controls.Add(this._scanProgress);

            this._lblScanProgress = new System.Windows.Forms.Label();
            this._lblScanProgress.Text = "0%";
            this._lblScanProgress.Font = fontMono8;
            this._lblScanProgress.ForeColor = cGreen;
            this._lblScanProgress.Size = new System.Drawing.Size(36, 14);
            this._lblScanProgress.Location = new System.Drawing.Point(jogX + 360, 12);
            this._lblScanProgress.BackColor = System.Drawing.Color.Transparent;
            this._bottomBar.Controls.Add(this._lblScanProgress);

            // 状态文本
            this._lblStatus = new System.Windows.Forms.Label();
            this._lblStatus.Text = "○未连接  |  PRF:-- Hz  |  帧率:-- fps  |  增益:-- dB";
            this._lblStatus.Font = fontMono8;
            this._lblStatus.ForeColor = cMuted;
            this._lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._lblStatus.BackColor = System.Drawing.Color.Transparent;
            this._bottomBar.Controls.Add(this._lblStatus);

            // 时间
            this._lblDateTime = new System.Windows.Forms.Label();
            this._lblDateTime.Text = "----/--/-- --:--:--";
            this._lblDateTime.Font = fontMono8;
            this._lblDateTime.ForeColor = cMuted;
            this._lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._lblDateTime.BackColor = System.Drawing.Color.Transparent;
            this._bottomBar.Controls.Add(this._lblDateTime);

            // ==========================================
            // 底部参数面板 _bottomParamPanel (位于中心显示区和底栏之间)
            // ==========================================
            this._bottomParamPanel = new System.Windows.Forms.Panel();
            this._bottomParamPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bottomParamPanel.Height = 180;
            this._bottomParamPanel.BackColor = cBgPanel;
            this._bottomParamPanel.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);

            // --- Tab分类按钮区 (位于参数内容区底部，三行布局) ---
            this._catPanel = new System.Windows.Forms.Panel();
            this._catPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._catPanel.Height = 90;
            this._catPanel.BackColor = System.Drawing.Color.Transparent;

            var catNames = new[] { "扫查控制", "发射", "接收", "探头", "楔块", "材料", "孔径", "闸门", "扫查", "编码器", "校准", "TFM" };
            var catTags = new[] { "scanCtrl", "tx", "rx", "probe", "wedge", "mat", "aperture", "gate", "scan", "encoder", "cal", "tfm" };
            int catBtnW = 72;
            int catBtnH = 24;
            int colsPerRow = 6;
            int gapX = 6;
            int gapY = 4;

            for (int i = 0; i < catNames.Length; i++)
            {
                int row = i / colsPerRow;
                int col = i % colsPerRow;
                int x = 4 + col * (catBtnW + gapX);
                int y = 4 + row * (catBtnH + gapY);
                var btn = MakeCatButton(catNames[i], catTags[i], x, y, catBtnW, catBtnH,
                    (i == 0) ? cBlue : cBgInput, (i == 0) ? System.Drawing.Color.White : cMuted);
                this._catPanel.Controls.Add(btn);
                _catButtons.Add(btn);
            }

            // --- Tab内容区 (Dock=Fill, AutoScroll) ---
            this._tabContentHost = new System.Windows.Forms.Panel();
            this._tabContentHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabContentHost.AutoScroll = true;
            this._tabContentHost.BackColor = cBgPanel;
            this._tabContentHost.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);

            // ===========================================
            // 构建12个Tab参数面板 (放在 _tabContentHost 中，水平排列)
            // ===========================================
            BuildTabPanelScanCtrl_Horizontal(cBgInput, cText, cMuted, cGreen, cRed, fontSize8, fontSize85);
            BuildTabPanelTx_Horizontal(cBgInput, cText, cMuted, cRed, fontSize8, fontSize85);
            BuildTabPanelRx_Horizontal(cBgInput, cText, cMuted, fontSize8, fontSize85);
            BuildTabPanelProbe_Horizontal(cBgInput, cText, cMuted, fontSize8, fontSize85);
            BuildTabPanelWedge_Horizontal(cBgInput, cText, cMuted, fontSize8, fontSize85);
            BuildTabPanelMaterial_Horizontal(cBgInput, cText, cMuted, fontSize8, fontSize85);
            BuildTabPanelAperture_Horizontal(cBgInput, cText, cMuted, fontSize8, fontSize85);
            BuildTabPanelGate_Horizontal(cBgInput, cText, cMuted, fontSize8, fontSize85);
            BuildTabPanelScan_Horizontal(cBgInput, cText, cMuted, fontSize8, fontSize85);
            BuildTabPanelEncoder_Horizontal(cBgInput, cText, cMuted, fontSize8, fontSize85);
            BuildTabPanelCal_Horizontal(cBgInput, cText, cMuted, cGreen, fontSize8, fontSize85);
            BuildTabPanelTfm_Horizontal(cBgInput, cText, cMuted, fontSize8, fontSize85);

            // 默认显示扫查控制面板
            _panelScanCtrl.Visible = true;

            // 添加顺序: Bottom(catPanel) → Fill(tabContent)
            this._bottomParamPanel.Controls.Add(this._catPanel);
            this._bottomParamPanel.Controls.Add(this._tabContentHost);

            // ==========================================
            // 中心显示区 _centerPanel — 2x2 四象限
            // ==========================================
            this._centerPanel = new System.Windows.Forms.TableLayoutPanel();
            this._centerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._centerPanel.BackColor = System.Drawing.Color.FromArgb(8, 10, 18);
            this._centerPanel.ColumnCount = 2;
            this._centerPanel.RowCount = 2;
            this._centerPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._centerPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._centerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._centerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._centerPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this._centerPanel.Padding = new System.Windows.Forms.Padding(1);

            // 四个象限显示面板
            var gridBg = System.Drawing.Color.FromArgb(12, 15, 22);
            this._panelQ1 = MakeQuadPanel("A扫波形", gridBg, cText, fontSize9);
            this._panelQ2 = MakeQuadPanel("S扫 / L扫", gridBg, cText, fontSize9);
            this._panelQ3 = MakeQuadPanel("A扫细节", gridBg, cText, fontSize9);
            this._panelQ4 = MakeQuadPanel("C扫成像", gridBg, cText, fontSize9);

            this._centerPanel.Controls.Add(this._panelQ1, 0, 0);
            this._centerPanel.Controls.Add(this._panelQ2, 1, 0);
            this._centerPanel.Controls.Add(this._panelQ3, 0, 1);
            this._centerPanel.Controls.Add(this._panelQ4, 1, 1);

            // ==========================================
            // 右栏 _rightPanel  (72px)
            // ==========================================
            this._rightPanel = new System.Windows.Forms.Panel();
            this._rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this._rightPanel.Width = 72;
            this._rightPanel.BackColor = cBgPanel;
            this._rightPanel.Padding = new System.Windows.Forms.Padding(2, 6, 2, 4);

            int btnY = 6;
            int btnW = 66;
            int btnH = 38;
            int btnIdx = 0;

            var rightBtns = new (string text, System.Drawing.Color back, System.Drawing.Color fore)[]
            {
                ("应用法则", cBlue, System.Drawing.Color.White),
                ("参数 +",   cBgInput, cText),
                ("参数 -",   cBgInput, cText),
                ("冻结",     cBgInput, cAmber),
                ("保存参数", cBgInput, cText),
                ("保存数据", cBgInput, cMuted),
                ("调用参数", cBgInput, cText),
                ("回放数据", cBgInput, cMuted),
                ("截屏",     cBgInput, cText),
                ("扫查启动", cGreen, System.Drawing.Color.White),
                ("扫查停止", cRed, System.Drawing.Color.White),
                ("退  出",   cBgInput, cRed),
            };

            foreach (var b in rightBtns)
            {
                var btn = new System.Windows.Forms.Button();
                btn.Text = b.text;
                btn.Tag = btnIdx;
                btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new System.Drawing.Font("微软雅黑", 7.5F);
                btn.Size = new System.Drawing.Size(btnW, btnH);
                btn.Location = new System.Drawing.Point(4, btnY);
                btn.BackColor = b.back;
                btn.ForeColor = b.fore;
                btn.Cursor = System.Windows.Forms.Cursors.Hand;
                btn.Click += new System.EventHandler(this.RightBtn_Click);
                this._rightPanel.Controls.Add(btn);
                btnY += btnH + 2;
                btnIdx++;
            }

            // ==========================================
            // 窗体属性
            // ==========================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 760);
            this.BackColor = cBgDark;
            this.ForeColor = cText;
            this.Font = fontSize9;
            this.MinimumSize = new System.Drawing.Size(1024, 600);
            this.Name = "Frm_NewInspect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewInspect - 相控阵超声检测系统";

            // 添加顺序: Top → Right → BottomParam → Bottom(status) → Fill
            // 注意: Dock=Bottom 后添加的会靠近底部边缘，因此 _bottomBar 要最后加才在最底部
            this.Controls.Add(this._topBar);
            this.Controls.Add(this._rightPanel);
            this.Controls.Add(this._bottomParamPanel);
            this.Controls.Add(this._bottomBar);
            this.Controls.Add(this._centerPanel);

            this.ResumeLayout(false);
        }

        // ===== 辅助: 顶栏 CheckBox (统一大小) =====
        private System.Windows.Forms.CheckBox MakeTopCheckBox(string text, bool chk, int x, int y)
        {
            var cb = new System.Windows.Forms.CheckBox();
            cb.Text = text;
            cb.Checked = chk;
            cb.AutoSize = false;
            cb.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            cb.ForeColor = System.Drawing.Color.FromArgb(200, 210, 225);
            cb.BackColor = System.Drawing.Color.Transparent;
            cb.Size = new System.Drawing.Size(80, 20);
            cb.Location = new System.Drawing.Point(x, y);
            cb.Cursor = System.Windows.Forms.Cursors.Hand;
            this._topBar.Controls.Add(cb);
            return cb;
        }

        // ===== 辅助: 分类Tab按钮 =====
        private System.Windows.Forms.Button MakeCatButton(string text, string tag, int x, int y,
            int w, int h, System.Drawing.Color back, System.Drawing.Color fore)
        {
            var btn = new System.Windows.Forms.Button();
            btn.Text = text;
            btn.Tag = tag;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new System.Drawing.Font("微软雅黑", 7.5F);
            btn.Size = new System.Drawing.Size(w, h);
            btn.Location = new System.Drawing.Point(x, y);
            btn.BackColor = back;
            btn.ForeColor = fore;
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            btn.Click += new System.EventHandler(this.ParamCategoryBtn_Click);
            return btn;
        }

        // ===== 辅助: JOG 按钮 =====
        private System.Windows.Forms.Button MakeJogBtn(string text, int x, int y,
            System.Drawing.Color backColor, System.Drawing.Color foreColor)
        {
            var btn = new System.Windows.Forms.Button();
            btn.Text = text;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(30, 40, 60);
            btn.Font = new System.Drawing.Font("Consolas", 9F);
            btn.Size = new System.Drawing.Size(26, 26);
            btn.Location = new System.Drawing.Point(x, y);
            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this._bottomBar.Controls.Add(btn);
            return btn;
        }

        // ===== 辅助: 四象限面板 =====
        private System.Windows.Forms.Panel MakeQuadPanel(string title,
            System.Drawing.Color bg, System.Drawing.Color fg, System.Drawing.Font font)
        {
            var pnl = new System.Windows.Forms.Panel();
            pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            pnl.BackColor = bg;
            var lbl = new System.Windows.Forms.Label();
            lbl.Text = title;
            lbl.Font = font;
            lbl.ForeColor = System.Drawing.Color.FromArgb(60, 70, 90);
            lbl.AutoSize = false;
            lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl.BackColor = System.Drawing.Color.Transparent;
            pnl.Controls.Add(lbl);
            return pnl;
        }

        // ===== 辅助: 参数标签+NumericUpDown =====
        private System.Windows.Forms.Label MakeParamLabel(string text, int x, int y, int w,
            System.Drawing.Color fore, System.Drawing.Font font)
        {
            var lbl = new System.Windows.Forms.Label();
            lbl.Text = text;
            lbl.Font = font;
            lbl.ForeColor = fore;
            lbl.Size = new System.Drawing.Size(w, 20);
            lbl.Location = new System.Drawing.Point(x, y + 2);
            lbl.BackColor = System.Drawing.Color.Transparent;
            return lbl;
        }

        private System.Windows.Forms.NumericUpDown MakeParamNud(decimal min, decimal max,
            decimal val, decimal step, int decimals, int x, int y, int w)
        {
            var nud = new System.Windows.Forms.NumericUpDown();
            nud.Minimum = min;
            nud.Maximum = max;
            nud.Value = val;
            nud.Increment = step;
            nud.DecimalPlaces = decimals;
            nud.Size = new System.Drawing.Size(w, 20);
            nud.Location = new System.Drawing.Point(x, y);
            nud.BackColor = System.Drawing.Color.FromArgb(24, 30, 44);
            nud.ForeColor = System.Drawing.Color.FromArgb(200, 210, 225);
            nud.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            nud.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            return nud;
        }

        private System.Windows.Forms.ComboBox MakeParamCombo(string[] items, int selIdx,
            int x, int y, int w, System.Drawing.Font font)
        {
            var cb = new System.Windows.Forms.ComboBox();
            cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cb.Items.AddRange(items);
            cb.SelectedIndex = selIdx;
            cb.Size = new System.Drawing.Size(w, 20);
            cb.Location = new System.Drawing.Point(x, y);
            cb.BackColor = System.Drawing.Color.FromArgb(24, 30, 44);
            cb.ForeColor = System.Drawing.Color.FromArgb(200, 210, 225);
            cb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cb.Font = font;
            return cb;
        }

        // ============================================================
        // 各Tab参数面板构建方法
        // ============================================================

        // ---- 1. 发射参数 ----
        private void BuildTabPanelTx(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Color cRed,
            System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelTx = new System.Windows.Forms.Panel();
            _panelTx.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelTx.Height = 210;
            _panelTx.BackColor = System.Drawing.Color.Transparent;

            int y = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 发射参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = cRed;
            hdr.Size = new System.Drawing.Size(180, 20);
            hdr.Location = new System.Drawing.Point(4, y);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelTx.Controls.Add(hdr);
            y += 22;

            _panelTx.Controls.Add(MakeParamLabel("零位偏移", 6, y, 56, cMuted, f8));
            _nudTxOffset = MakeParamNud(0, 999, 0.0m, 0.1m, 1, 64, y, 68);
            _panelTx.Controls.Add(_nudTxOffset);
            var lu1 = MakeParamLabel("μs", 134, y, 26, cMuted, f8);
            _panelTx.Controls.Add(lu1);
            y += 24;

            _panelTx.Controls.Add(MakeParamLabel("增益", 6, y, 56, cMuted, f8));
            _nudTxGain = MakeParamNud(0, 120, 26.1m, 0.1m, 1, 64, y, 68);
            _panelTx.Controls.Add(_nudTxGain);
            var lu2 = MakeParamLabel("dB", 134, y, 26, cMuted, f8);
            _panelTx.Controls.Add(lu2);
            y += 24;

            _panelTx.Controls.Add(MakeParamLabel("量程", 6, y, 56, cMuted, f8));
            _nudTxRange = MakeParamNud(0, 9999, 56.0m, 1.0m, 1, 64, y, 68);
            _panelTx.Controls.Add(_nudTxRange);
            var lu3 = MakeParamLabel("μs", 134, y, 26, cMuted, f8);
            _panelTx.Controls.Add(lu3);
            y += 24;

            _panelTx.Controls.Add(MakeParamLabel("激励频率", 6, y, 56, cMuted, f8));
            _nudTxFreq = MakeParamNud(500, 25000, 20000, 500, 0, 64, y, 68);
            _panelTx.Controls.Add(_nudTxFreq);
            var lu4 = MakeParamLabel("Hz", 134, y, 26, cMuted, f8);
            _panelTx.Controls.Add(lu4);
            y += 24;

            _panelTx.Controls.Add(MakeParamLabel("脉冲宽度", 6, y, 56, cMuted, f8));
            _nudTxPulseWidth = MakeParamNud(20, 2000, 100, 10, 0, 64, y, 68);
            _panelTx.Controls.Add(_nudTxPulseWidth);
            var lu5 = MakeParamLabel("ns", 134, y, 26, cMuted, f8);
            _panelTx.Controls.Add(lu5);
            y += 24;

            _panelTx.Controls.Add(MakeParamLabel("接收滤波", 6, y, 56, cMuted, f8));
            _nudTxFilter = MakeParamNud(100, 25000, 3100, 100, 0, 64, y, 68);
            _panelTx.Controls.Add(_nudTxFilter);
            var lu6 = MakeParamLabel("Hz", 134, y, 26, cMuted, f8);
            _panelTx.Controls.Add(lu6);
            y += 24;

            _panelTx.Controls.Add(MakeParamLabel("采样率", 6, y, 56, cMuted, f8));
            _nudTxSampleRate = MakeParamNud(10, 500, 100, 10, 0, 64, y, 68);
            _panelTx.Controls.Add(_nudTxSampleRate);
            var lu7 = MakeParamLabel("MHz", 134, y, 26, cMuted, f8);
            _panelTx.Controls.Add(lu7);

            _panelTx.Visible = false;
            this._tabContentHost.Controls.Add(_panelTx);
        }

        // ---- 2. 接收参数 ----
        private void BuildTabPanelRx(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelRx = new System.Windows.Forms.Panel();
            _panelRx.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelRx.Height = 210;
            _panelRx.BackColor = System.Drawing.Color.Transparent;

            int y = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 接收参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(56, 130, 246);
            hdr.Size = new System.Drawing.Size(180, 20);
            hdr.Location = new System.Drawing.Point(4, y);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelRx.Controls.Add(hdr);
            y += 22;

            _panelRx.Controls.Add(MakeParamLabel("数字增益", 6, y, 56, cMuted, f8));
            _nudRxDigitalGain = MakeParamNud(0, 120, 0.0m, 0.1m, 1, 64, y, 68);
            _panelRx.Controls.Add(_nudRxDigitalGain);
            _panelRx.Controls.Add(MakeParamLabel("dB", 134, y, 26, cMuted, f8));
            y += 24;

            _panelRx.Controls.Add(MakeParamLabel("模拟增益", 6, y, 56, cMuted, f8));
            _nudRxAnalogGain = MakeParamNud(0, 120, 20.5m, 0.1m, 1, 64, y, 68);
            _panelRx.Controls.Add(_nudRxAnalogGain);
            _panelRx.Controls.Add(MakeParamLabel("dB", 134, y, 26, cMuted, f8));
            y += 24;

            _panelRx.Controls.Add(MakeParamLabel("高压", 6, y, 56, cMuted, f8));
            _nudRxVoltage = MakeParamNud(10, 200, 50, 5, 0, 64, y, 68);
            _panelRx.Controls.Add(_nudRxVoltage);
            _panelRx.Controls.Add(MakeParamLabel("V", 134, y, 26, cMuted, f8));
            y += 24;

            _panelRx.Controls.Add(MakeParamLabel("整流方式", 6, y, 56, cMuted, f8));
            _cmbRxRectify = MakeParamCombo(new[] { "全波", "半波", "射频" }, 0, 64, y, 68, f8);
            _panelRx.Controls.Add(_cmbRxRectify);
            y += 24;

            _panelRx.Controls.Add(MakeParamLabel("平均次数", 6, y, 56, cMuted, f8));
            _nudRxAverage = MakeParamNud(1, 64, 1, 1, 0, 64, y, 68);
            _panelRx.Controls.Add(_nudRxAverage);
            y += 24;

            _panelRx.Controls.Add(MakeParamLabel("阻尼", 6, y, 56, cMuted, f8));
            _nudRxDamping = MakeParamNud(25, 1000, 50, 25, 0, 64, y, 68);
            _panelRx.Controls.Add(_nudRxDamping);
            _panelRx.Controls.Add(MakeParamLabel("Ω", 134, y, 26, cMuted, f8));

            _panelRx.Visible = false;
            this._tabContentHost.Controls.Add(_panelRx);
        }

        // ---- 3. 探头参数 ----
        private void BuildTabPanelProbe(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelProbe = new System.Windows.Forms.Panel();
            _panelProbe.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelProbe.Height = 210;
            _panelProbe.BackColor = System.Drawing.Color.Transparent;

            int y = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 探头参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(168, 85, 247);
            hdr.Size = new System.Drawing.Size(180, 20);
            hdr.Location = new System.Drawing.Point(4, y);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelProbe.Controls.Add(hdr);
            y += 22;

            _panelProbe.Controls.Add(MakeParamLabel("探头类型", 6, y, 56, cMuted, f8));
            _cmbProbeType = MakeParamCombo(new[] { "线阵", "面阵", "弧阵" }, 0, 64, y, 68, f8);
            _panelProbe.Controls.Add(_cmbProbeType);
            y += 24;

            _panelProbe.Controls.Add(MakeParamLabel("晶片总数", 6, y, 56, cMuted, f8));
            _nudProbeElements = MakeParamNud(1, 256, 64, 1, 0, 64, y, 68);
            _panelProbe.Controls.Add(_nudProbeElements);
            y += 24;

            _panelProbe.Controls.Add(MakeParamLabel("晶片间距", 6, y, 56, cMuted, f8));
            _nudProbePitch = MakeParamNud(0.01m, 10, 0.6m, 0.01m, 2, 64, y, 68);
            _panelProbe.Controls.Add(_nudProbePitch);
            _panelProbe.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));
            y += 24;

            _panelProbe.Controls.Add(MakeParamLabel("首晶片位", 6, y, 56, cMuted, f8));
            _nudProbeFirstElem = MakeParamNud(0, 255, 0, 1, 0, 64, y, 68);
            _panelProbe.Controls.Add(_nudProbeFirstElem);
            _panelProbe.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));
            y += 24;

            _panelProbe.Controls.Add(MakeParamLabel("晶片频率", 6, y, 56, cMuted, f8));
            _nudProbeFreq = MakeParamNud(0.5m, 25, 5.0m, 0.5m, 1, 64, y, 68);
            _panelProbe.Controls.Add(_nudProbeFreq);
            _panelProbe.Controls.Add(MakeParamLabel("MHz", 134, y, 26, cMuted, f8));
            y += 24;

            _panelProbe.Controls.Add(MakeParamLabel("晶片宽度", 6, y, 56, cMuted, f8));
            _nudProbeWidth = MakeParamNud(0.1m, 10, 0.5m, 0.1m, 2, 64, y, 68);
            _panelProbe.Controls.Add(_nudProbeWidth);
            _panelProbe.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));

            _panelProbe.Visible = false;
            this._tabContentHost.Controls.Add(_panelProbe);
        }

        // ---- 4. 楔块参数 ----
        private void BuildTabPanelWedge(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelWedge = new System.Windows.Forms.Panel();
            _panelWedge.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelWedge.Height = 210;
            _panelWedge.BackColor = System.Drawing.Color.Transparent;

            int y = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 楔块参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            hdr.Size = new System.Drawing.Size(180, 20);
            hdr.Location = new System.Drawing.Point(4, y);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelWedge.Controls.Add(hdr);
            y += 22;

            _panelWedge.Controls.Add(MakeParamLabel("楔块角度", 6, y, 56, cMuted, f8));
            _nudWedgeAngle = MakeParamNud(0, 85, 36.0m, 0.5m, 1, 64, y, 68);
            _panelWedge.Controls.Add(_nudWedgeAngle);
            _panelWedge.Controls.Add(MakeParamLabel("°", 134, y, 26, cMuted, f8));
            y += 24;

            _panelWedge.Controls.Add(MakeParamLabel("楔块声速", 6, y, 56, cMuted, f8));
            _nudWedgeVelocity = MakeParamNud(1000, 10000, 2337, 1, 0, 64, y, 68);
            _panelWedge.Controls.Add(_nudWedgeVelocity);
            _panelWedge.Controls.Add(MakeParamLabel("m/s", 134, y, 26, cMuted, f8));
            y += 24;

            _panelWedge.Controls.Add(MakeParamLabel("楔块高度", 6, y, 56, cMuted, f8));
            _nudWedgeHeight = MakeParamNud(0, 100, 20.0m, 0.5m, 1, 64, y, 68);
            _panelWedge.Controls.Add(_nudWedgeHeight);
            _panelWedge.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));
            y += 24;

            _panelWedge.Controls.Add(MakeParamLabel("前端高度", 6, y, 56, cMuted, f8));
            _nudWedgeFrontH = MakeParamNud(0, 100, 10.0m, 0.5m, 1, 64, y, 68);
            _panelWedge.Controls.Add(_nudWedgeFrontH);
            _panelWedge.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));
            y += 24;

            _panelWedge.Controls.Add(MakeParamLabel("楔块偏移", 6, y, 56, cMuted, f8));
            _nudWedgeOffset = MakeParamNud(-100, 100, 0, 0.5m, 1, 64, y, 68);
            _panelWedge.Controls.Add(_nudWedgeOffset);
            _panelWedge.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));

            _panelWedge.Visible = false;
            this._tabContentHost.Controls.Add(_panelWedge);
        }

        // ---- 5. 材料参数 ----
        private void BuildTabPanelMaterial(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelMaterial = new System.Windows.Forms.Panel();
            _panelMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelMaterial.Height = 160;
            _panelMaterial.BackColor = System.Drawing.Color.Transparent;

            int y = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 材料参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            hdr.Size = new System.Drawing.Size(180, 20);
            hdr.Location = new System.Drawing.Point(4, y);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelMaterial.Controls.Add(hdr);
            y += 22;

            _panelMaterial.Controls.Add(MakeParamLabel("纵波声速", 6, y, 56, cMuted, f8));
            _nudMatLongVel = MakeParamNud(1000, 15000, 5900, 10, 0, 64, y, 68);
            _panelMaterial.Controls.Add(_nudMatLongVel);
            _panelMaterial.Controls.Add(MakeParamLabel("m/s", 134, y, 26, cMuted, f8));
            y += 24;

            _panelMaterial.Controls.Add(MakeParamLabel("横波声速", 6, y, 56, cMuted, f8));
            _nudMatShearVel = MakeParamNud(500, 10000, 3230, 10, 0, 64, y, 68);
            _panelMaterial.Controls.Add(_nudMatShearVel);
            _panelMaterial.Controls.Add(MakeParamLabel("m/s", 134, y, 26, cMuted, f8));
            y += 24;

            _panelMaterial.Controls.Add(MakeParamLabel("材料厚度", 6, y, 56, cMuted, f8));
            _nudMatThickness = MakeParamNud(0.1m, 9999, 50.0m, 0.5m, 1, 64, y, 68);
            _panelMaterial.Controls.Add(_nudMatThickness);
            _panelMaterial.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));
            y += 24;

            _panelMaterial.Controls.Add(MakeParamLabel("衰减系数", 6, y, 56, cMuted, f8));
            _nudMatAtten = MakeParamNud(0, 99, 0, 0.01m, 2, 64, y, 68);
            _panelMaterial.Controls.Add(_nudMatAtten);
            _panelMaterial.Controls.Add(MakeParamLabel("dB/m", 134, y, 26, cMuted, f8));

            _panelMaterial.Visible = false;
            this._tabContentHost.Controls.Add(_panelMaterial);
        }

        // ---- 6. 孔径参数 ----
        private void BuildTabPanelAperture(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelAperture = new System.Windows.Forms.Panel();
            _panelAperture.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelAperture.Height = 210;
            _panelAperture.BackColor = System.Drawing.Color.Transparent;

            int y = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 孔径参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(236, 72, 153);
            hdr.Size = new System.Drawing.Size(180, 20);
            hdr.Location = new System.Drawing.Point(4, y);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelAperture.Controls.Add(hdr);
            y += 22;

            _panelAperture.Controls.Add(MakeParamLabel("起始晶片", 6, y, 56, cMuted, f8));
            _nudApertureStart = MakeParamNud(1, 256, 1, 1, 0, 64, y, 68);
            _panelAperture.Controls.Add(_nudApertureStart);
            y += 24;

            _panelAperture.Controls.Add(MakeParamLabel("晶片数量", 6, y, 56, cMuted, f8));
            _nudApertureCount = MakeParamNud(1, 256, 16, 1, 0, 64, y, 68);
            _panelAperture.Controls.Add(_nudApertureCount);
            y += 24;

            _panelAperture.Controls.Add(MakeParamLabel("聚焦深度", 6, y, 56, cMuted, f8));
            _nudApertureFocus = MakeParamNud(0, 9999, 30.0m, 1.0m, 1, 64, y, 68);
            _panelAperture.Controls.Add(_nudApertureFocus);
            _panelAperture.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));
            y += 24;

            _panelAperture.Controls.Add(MakeParamLabel("孔径类型", 6, y, 56, cMuted, f8));
            _cmbApertureType = MakeParamCombo(new[] { "固定", "动态" }, 0, 64, y, 68, f8);
            _panelAperture.Controls.Add(_cmbApertureType);
            y += 24;

            _panelAperture.Controls.Add(MakeParamLabel("偏转角度", 6, y, 56, cMuted, f8));
            _nudApertureAngle = MakeParamNud(-80, 80, 0, 0.5m, 1, 64, y, 68);
            _panelAperture.Controls.Add(_nudApertureAngle);
            _panelAperture.Controls.Add(MakeParamLabel("°", 134, y, 26, cMuted, f8));

            _panelAperture.Visible = false;
            this._tabContentHost.Controls.Add(_panelAperture);
        }

        // ---- 7. 闸门参数 ----
        private void BuildTabPanelGate(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelGate = new System.Windows.Forms.Panel();
            _panelGate.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelGate.Height = 230;
            _panelGate.BackColor = System.Drawing.Color.Transparent;

            int y = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 闸门参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(251, 191, 36);
            hdr.Size = new System.Drawing.Size(180, 20);
            hdr.Location = new System.Drawing.Point(4, y);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelGate.Controls.Add(hdr);
            y += 22;

            // 闸门A
            var gaHdr = new System.Windows.Forms.Label();
            gaHdr.Text = "▸ 闸门A";
            gaHdr.Font = new System.Drawing.Font("微软雅黑", 8.5F, System.Drawing.FontStyle.Bold);
            gaHdr.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            gaHdr.Size = new System.Drawing.Size(100, 18);
            gaHdr.Location = new System.Drawing.Point(6, y);
            gaHdr.BackColor = System.Drawing.Color.Transparent;
            _panelGate.Controls.Add(gaHdr);
            y += 18;

            _panelGate.Controls.Add(MakeParamLabel("起点", 10, y, 30, cMuted, f8));
            _nudGateAStart = MakeParamNud(0, 9999, 10.0m, 1.0m, 1, 64, y, 56);
            _panelGate.Controls.Add(_nudGateAStart);
            _panelGate.Controls.Add(MakeParamLabel("μs", 122, y, 26, cMuted, f8));
            y += 24;

            _panelGate.Controls.Add(MakeParamLabel("宽度", 10, y, 30, cMuted, f8));
            _nudGateAWidth = MakeParamNud(0, 9999, 50.0m, 1.0m, 1, 64, y, 56);
            _panelGate.Controls.Add(_nudGateAWidth);
            _panelGate.Controls.Add(MakeParamLabel("μs", 122, y, 26, cMuted, f8));
            y += 24;

            _panelGate.Controls.Add(MakeParamLabel("阈值", 10, y, 30, cMuted, f8));
            _nudGateAThld = MakeParamNud(0, 100, 50, 1, 0, 64, y, 56);
            _panelGate.Controls.Add(_nudGateAThld);
            _panelGate.Controls.Add(MakeParamLabel("%", 122, y, 26, cMuted, f8));
            y += 26;

            // 闸门B
            var gbHdr = new System.Windows.Forms.Label();
            gbHdr.Text = "▸ 闸门B";
            gbHdr.Font = new System.Drawing.Font("微软雅黑", 8.5F, System.Drawing.FontStyle.Bold);
            gbHdr.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            gbHdr.Size = new System.Drawing.Size(100, 18);
            gbHdr.Location = new System.Drawing.Point(6, y);
            gbHdr.BackColor = System.Drawing.Color.Transparent;
            _panelGate.Controls.Add(gbHdr);
            y += 18;

            _panelGate.Controls.Add(MakeParamLabel("起点", 10, y, 30, cMuted, f8));
            _nudGateBStart = MakeParamNud(0, 9999, 20.0m, 1.0m, 1, 64, y, 56);
            _panelGate.Controls.Add(_nudGateBStart);
            _panelGate.Controls.Add(MakeParamLabel("μs", 122, y, 26, cMuted, f8));
            y += 24;

            _panelGate.Controls.Add(MakeParamLabel("宽度", 10, y, 30, cMuted, f8));
            _nudGateBWidth = MakeParamNud(0, 9999, 40.0m, 1.0m, 1, 64, y, 56);
            _panelGate.Controls.Add(_nudGateBWidth);
            _panelGate.Controls.Add(MakeParamLabel("μs", 122, y, 26, cMuted, f8));
            y += 24;

            _panelGate.Controls.Add(MakeParamLabel("阈值", 10, y, 30, cMuted, f8));
            _nudGateBThld = MakeParamNud(0, 100, 50, 1, 0, 64, y, 56);
            _panelGate.Controls.Add(_nudGateBThld);
            _panelGate.Controls.Add(MakeParamLabel("%", 122, y, 26, cMuted, f8));

            _panelGate.Visible = false;
            this._tabContentHost.Controls.Add(_panelGate);
        }

        // ---- 8. 扫查参数 ----
        private void BuildTabPanelScan(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelScan = new System.Windows.Forms.Panel();
            _panelScan.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelScan.Height = 210;
            _panelScan.BackColor = System.Drawing.Color.Transparent;

            int y = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 扫查参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            hdr.Size = new System.Drawing.Size(180, 20);
            hdr.Location = new System.Drawing.Point(4, y);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelScan.Controls.Add(hdr);
            y += 22;

            _panelScan.Controls.Add(MakeParamLabel("扫查类型", 6, y, 56, cMuted, f8));
            _cmbScanType = MakeParamCombo(new[] { "S扫", "L扫", "CL扫" }, 0, 64, y, 68, f8);
            _panelScan.Controls.Add(_cmbScanType);
            y += 24;

            _panelScan.Controls.Add(MakeParamLabel("起始角度", 6, y, 56, cMuted, f8));
            _nudScanStartAngle = MakeParamNud(-90, 90, -45, 1.0m, 1, 64, y, 68);
            _panelScan.Controls.Add(_nudScanStartAngle);
            _panelScan.Controls.Add(MakeParamLabel("°", 134, y, 26, cMuted, f8));
            y += 24;

            _panelScan.Controls.Add(MakeParamLabel("终止角度", 6, y, 56, cMuted, f8));
            _nudScanEndAngle = MakeParamNud(-90, 90, 45, 1.0m, 1, 64, y, 68);
            _panelScan.Controls.Add(_nudScanEndAngle);
            _panelScan.Controls.Add(MakeParamLabel("°", 134, y, 26, cMuted, f8));
            y += 24;

            _panelScan.Controls.Add(MakeParamLabel("角度步进", 6, y, 56, cMuted, f8));
            _nudScanAngleStep = MakeParamNud(0.1m, 10, 1.0m, 0.1m, 1, 64, y, 68);
            _panelScan.Controls.Add(_nudScanAngleStep);
            _panelScan.Controls.Add(MakeParamLabel("°", 134, y, 26, cMuted, f8));
            y += 24;

            _panelScan.Controls.Add(MakeParamLabel("深度范围", 6, y, 56, cMuted, f8));
            _nudScanDepth = MakeParamNud(1, 9999, 200.0m, 10.0m, 1, 64, y, 68);
            _panelScan.Controls.Add(_nudScanDepth);
            _panelScan.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));

            _panelScan.Visible = false;
            this._tabContentHost.Controls.Add(_panelScan);
        }

        // ---- 9. 编码器参数 ----
        private void BuildTabPanelEncoder(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelEncoder = new System.Windows.Forms.Panel();
            _panelEncoder.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelEncoder.Height = 210;
            _panelEncoder.BackColor = System.Drawing.Color.Transparent;

            int y = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 编码器参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(14, 165, 233);
            hdr.Size = new System.Drawing.Size(180, 20);
            hdr.Location = new System.Drawing.Point(4, y);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelEncoder.Controls.Add(hdr);
            y += 22;

            _panelEncoder.Controls.Add(MakeParamLabel("编码器分辨率", 6, y, 76, cMuted, f8));
            _nudEncRes = MakeParamNud(1, 5000, 100, 1, 0, 84, y, 68);
            _panelEncoder.Controls.Add(_nudEncRes);
            _panelEncoder.Controls.Add(MakeParamLabel("P/mm", 154, y, 36, cMuted, f8));
            y += 24;

            _panelEncoder.Controls.Add(MakeParamLabel("触发方式", 6, y, 56, cMuted, f8));
            _cmbEncTrig = MakeParamCombo(new[] { "等距触发", "等时触发" }, 0, 64, y, 68, f8);
            _panelEncoder.Controls.Add(_cmbEncTrig);
            y += 24;

            _panelEncoder.Controls.Add(MakeParamLabel("触发间距", 6, y, 56, cMuted, f8));
            _nudEncInterval = MakeParamNud(0.1m, 100, 1.0m, 0.1m, 1, 64, y, 68);
            _panelEncoder.Controls.Add(_nudEncInterval);
            _panelEncoder.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));
            y += 24;

            _panelEncoder.Controls.Add(MakeParamLabel("X步进", 6, y, 56, cMuted, f8));
            _nudEncXStep = MakeParamNud(0.1m, 100, 7.8m, 0.1m, 1, 64, y, 68);
            _panelEncoder.Controls.Add(_nudEncXStep);
            _panelEncoder.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));
            y += 24;

            _panelEncoder.Controls.Add(MakeParamLabel("Y步进", 6, y, 56, cMuted, f8));
            _nudEncYStep = MakeParamNud(0.1m, 100, 1.6m, 0.1m, 1, 64, y, 68);
            _panelEncoder.Controls.Add(_nudEncYStep);
            _panelEncoder.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));

            _panelEncoder.Visible = false;
            this._tabContentHost.Controls.Add(_panelEncoder);
        }

        // ---- 10. 校准参数 ----
        private void BuildTabPanelCal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Color cGreen,
            System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelCal = new System.Windows.Forms.Panel();
            _panelCal.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelCal.Height = 210;
            _panelCal.BackColor = System.Drawing.Color.Transparent;

            int y = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 校准参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            hdr.Size = new System.Drawing.Size(180, 20);
            hdr.Location = new System.Drawing.Point(4, y);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelCal.Controls.Add(hdr);
            y += 22;

            _panelCal.Controls.Add(MakeParamLabel("声速校准", 6, y, 56, cMuted, f8));
            _nudCalVelocity = MakeParamNud(500, 15000, 5900, 10, 0, 64, y, 68);
            _panelCal.Controls.Add(_nudCalVelocity);
            _panelCal.Controls.Add(MakeParamLabel("m/s", 134, y, 26, cMuted, f8));
            y += 24;

            _panelCal.Controls.Add(MakeParamLabel("零位校准", 6, y, 56, cMuted, f8));
            _nudCalZero = MakeParamNud(0, 9999, 0, 0.1m, 1, 64, y, 68);
            _panelCal.Controls.Add(_nudCalZero);
            _panelCal.Controls.Add(MakeParamLabel("μs", 134, y, 26, cMuted, f8));
            y += 26;

            // ACG
            this._chkACG = new System.Windows.Forms.CheckBox();
            this._chkACG.Text = "ACG 角度增益补偿";
            this._chkACG.Checked = false;
            this._chkACG.AutoSize = false;
            this._chkACG.Font = f85;
            this._chkACG.ForeColor = cGreen;
            this._chkACG.BackColor = System.Drawing.Color.Transparent;
            this._chkACG.Size = new System.Drawing.Size(160, 20);
            this._chkACG.Location = new System.Drawing.Point(6, y);
            this._chkACG.Cursor = System.Windows.Forms.Cursors.Hand;
            _panelCal.Controls.Add(this._chkACG);
            y += 22;

            // TCG
            this._chkTCG = new System.Windows.Forms.CheckBox();
            this._chkTCG.Text = "TCG 时间增益补偿";
            this._chkTCG.Checked = false;
            this._chkTCG.AutoSize = false;
            this._chkTCG.Font = f85;
            this._chkTCG.ForeColor = cGreen;
            this._chkTCG.BackColor = System.Drawing.Color.Transparent;
            this._chkTCG.Size = new System.Drawing.Size(160, 20);
            this._chkTCG.Location = new System.Drawing.Point(6, y);
            this._chkTCG.Cursor = System.Windows.Forms.Cursors.Hand;
            _panelCal.Controls.Add(this._chkTCG);
            y += 24;

            // TCG 增益补偿值
            _panelCal.Controls.Add(MakeParamLabel("TCG增益", 6, y, 56, cMuted, f8));
            _nudCalTCGGain = MakeParamNud(0, 80, 0, 0.1m, 1, 64, y, 68);
            _panelCal.Controls.Add(_nudCalTCGGain);
            _panelCal.Controls.Add(MakeParamLabel("dB", 134, y, 26, cMuted, f8));

            _panelCal.Visible = false;
            this._tabContentHost.Controls.Add(_panelCal);
        }

        // ---- 11. TFM参数 ----
        private void BuildTabPanelTfm(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelTfm = new System.Windows.Forms.Panel();
            _panelTfm.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelTfm.Height = 210;
            _panelTfm.BackColor = System.Drawing.Color.Transparent;

            int y = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● TFM参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(168, 85, 247);
            hdr.Size = new System.Drawing.Size(180, 20);
            hdr.Location = new System.Drawing.Point(4, y);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelTfm.Controls.Add(hdr);
            y += 22;

            _panelTfm.Controls.Add(MakeParamLabel("TFM模式", 6, y, 56, cMuted, f8));
            _cmbTfmMode = MakeParamCombo(new[] { "全聚焦", "半聚焦" }, 0, 64, y, 68, f8);
            _panelTfm.Controls.Add(_cmbTfmMode);
            y += 24;

            _panelTfm.Controls.Add(MakeParamLabel("网格分辨率", 6, y, 56, cMuted, f8));
            _nudTfmGridRes = MakeParamNud(0.1m, 5, 0.5m, 0.1m, 1, 64, y, 68);
            _panelTfm.Controls.Add(_nudTfmGridRes);
            _panelTfm.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));
            y += 24;

            _panelTfm.Controls.Add(MakeParamLabel("成像范围X", 6, y, 56, cMuted, f8));
            _nudTfmRangeX = MakeParamNud(1, 999, 50.0m, 1.0m, 1, 64, y, 68);
            _panelTfm.Controls.Add(_nudTfmRangeX);
            _panelTfm.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));
            y += 24;

            _panelTfm.Controls.Add(MakeParamLabel("成像范围Z", 6, y, 56, cMuted, f8));
            _nudTfmRangeZ = MakeParamNud(1, 999, 50.0m, 1.0m, 1, 64, y, 68);
            _panelTfm.Controls.Add(_nudTfmRangeZ);
            _panelTfm.Controls.Add(MakeParamLabel("mm", 134, y, 26, cMuted, f8));
            y += 24;

            _panelTfm.Controls.Add(MakeParamLabel("重构模式", 6, y, 56, cMuted, f8));
            _cmbTfmReconMode = MakeParamCombo(new[] { "直接", "全矩阵捕获" }, 0, 64, y, 68, f8);
            _panelTfm.Controls.Add(_cmbTfmReconMode);

            _panelTfm.Visible = false;
            this._tabContentHost.Controls.Add(_panelTfm);
        }

        // ============================================================
        // 各Tab参数面板构建方法 - 水平布局版本
        // ============================================================

        // ---- 1. 发射参数 (水平) ----
        // ============================================================
        // 11个Tab参数面板 — 水平布局版 (列间距180px, 消除遮挡)
        // 每个参数单元: label(56) + nud/combo(64~70) + unit(26~36) ≈ 160px
        // ============================================================

        // ---- 0. 扫查控制 (水平) ----
        private void BuildTabPanelScanCtrl_Horizontal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Color cGreen, System.Drawing.Color cRed,
            System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelScanCtrl = new System.Windows.Forms.Panel();
            _panelScanCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelScanCtrl.BackColor = System.Drawing.Color.Transparent;

            int rowY = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 扫查控制";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = cGreen;
            hdr.Size = new System.Drawing.Size(100, 20);
            hdr.Location = new System.Drawing.Point(4, rowY);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelScanCtrl.Controls.Add(hdr);

            // 4列布局, 间距180px
            int col1X = 4, col2X = 184, col3X = 364, col4X = 544;
            rowY += 24;

            // 编码器使能
            _chkEncoderEnable = new System.Windows.Forms.CheckBox();
            _chkEncoderEnable.Text = "编码器使能";
            _chkEncoderEnable.Checked = true;
            _chkEncoderEnable.AutoSize = false;
            _chkEncoderEnable.Font = f85;
            _chkEncoderEnable.ForeColor = cGreen;
            _chkEncoderEnable.BackColor = System.Drawing.Color.Transparent;
            _chkEncoderEnable.Size = new System.Drawing.Size(100, 20);
            _chkEncoderEnable.Location = new System.Drawing.Point(col1X, rowY);
            _chkEncoderEnable.Cursor = System.Windows.Forms.Cursors.Hand;
            _panelScanCtrl.Controls.Add(_chkEncoderEnable);

            // 扫描方向
            _panelScanCtrl.Controls.Add(MakeParamLabel("扫描方向", col2X, rowY, 56, cMuted, f8));
            _cmbScanDir = MakeParamCombo(new[] { "正向(→)", "反向(←)", "往复(↔)" }, 0, col2X + 60, rowY, 86, f8);
            _panelScanCtrl.Controls.Add(_cmbScanDir);

            // 扫描速度
            _panelScanCtrl.Controls.Add(MakeParamLabel("扫描速度", col3X, rowY, 56, cMuted, f8));
            _nudScanSpeed = MakeParamNud(1, 500, 50, 5, 0, col3X + 60, rowY, 60);
            _panelScanCtrl.Controls.Add(_nudScanSpeed);
            _panelScanCtrl.Controls.Add(MakeParamLabel("mm/s", col3X + 122, rowY, 36, cMuted, f8));

            // 扫描进度条
            _scanCtrlProgress = new System.Windows.Forms.ProgressBar();
            _scanCtrlProgress.Size = new System.Drawing.Size(150, 12);
            _scanCtrlProgress.Location = new System.Drawing.Point(col4X, rowY + 4);
            _scanCtrlProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            _scanCtrlProgress.Value = 0;
            _panelScanCtrl.Controls.Add(_scanCtrlProgress);

            rowY += 26;

            // 启动/停止按钮
            _btnScanStart = new System.Windows.Forms.Button();
            _btnScanStart.Text = "▶ 启动";
            _btnScanStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnScanStart.FlatAppearance.BorderSize = 0;
            _btnScanStart.Font = new System.Drawing.Font("微软雅黑", 8F);
            _btnScanStart.Size = new System.Drawing.Size(72, 22);
            _btnScanStart.Location = new System.Drawing.Point(col1X, rowY);
            _btnScanStart.BackColor = cGreen;
            _btnScanStart.ForeColor = System.Drawing.Color.White;
            _btnScanStart.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnScanStart.Click += new System.EventHandler(this.BtnScanStart_Click);
            _panelScanCtrl.Controls.Add(_btnScanStart);

            _btnScanStop = new System.Windows.Forms.Button();
            _btnScanStop.Text = "■ 停止";
            _btnScanStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnScanStop.FlatAppearance.BorderSize = 0;
            _btnScanStop.Font = new System.Drawing.Font("微软雅黑", 8F);
            _btnScanStop.Size = new System.Drawing.Size(72, 22);
            _btnScanStop.Location = new System.Drawing.Point(col1X + 78, rowY);
            _btnScanStop.BackColor = cRed;
            _btnScanStop.ForeColor = System.Drawing.Color.White;
            _btnScanStop.Cursor = System.Windows.Forms.Cursors.Hand;
            _btnScanStop.Click += new System.EventHandler(this.BtnScanStop_Click);
            _panelScanCtrl.Controls.Add(_btnScanStop);

            _panelScanCtrl.Visible = false;
            this._tabContentHost.Controls.Add(_panelScanCtrl);
        }

        // ---- 1. 发射参数 (水平) ----
        private void BuildTabPanelTx_Horizontal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Color cRed,
            System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelTx = new System.Windows.Forms.Panel();
            _panelTx.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelTx.BackColor = System.Drawing.Color.Transparent;

            int rowY = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 发射参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = cRed;
            hdr.Size = new System.Drawing.Size(100, 20);
            hdr.Location = new System.Drawing.Point(4, rowY);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelTx.Controls.Add(hdr);

            // 列间距180px: col1=4, col2=184, col3=364, col4=544
            int col1X = 4, col2X = 184, col3X = 364, col4X = 544;
            rowY += 24;

            // ---- 第1行 ----
            _panelTx.Controls.Add(MakeParamLabel("零位偏移", col1X, rowY, 56, cMuted, f8));
            _nudTxOffset = MakeParamNud(0, 999, 0.0m, 0.1m, 1, col1X + 60, rowY, 70);
            _panelTx.Controls.Add(_nudTxOffset);
            _panelTx.Controls.Add(MakeParamLabel("μs", col1X + 132, rowY, 26, cMuted, f8));

            _panelTx.Controls.Add(MakeParamLabel("增益", col2X, rowY, 56, cMuted, f8));
            _nudTxGain = MakeParamNud(0, 120, 26.1m, 0.1m, 1, col2X + 60, rowY, 70);
            _panelTx.Controls.Add(_nudTxGain);
            _panelTx.Controls.Add(MakeParamLabel("dB", col2X + 132, rowY, 26, cMuted, f8));

            _panelTx.Controls.Add(MakeParamLabel("量程", col3X, rowY, 56, cMuted, f8));
            _nudTxRange = MakeParamNud(0, 9999, 56.0m, 1.0m, 1, col3X + 60, rowY, 70);
            _panelTx.Controls.Add(_nudTxRange);
            _panelTx.Controls.Add(MakeParamLabel("μs", col3X + 132, rowY, 26, cMuted, f8));

            _panelTx.Controls.Add(MakeParamLabel("激励频率", col4X, rowY, 56, cMuted, f8));
            _nudTxFreq = MakeParamNud(500, 25000, 20000, 500, 0, col4X + 60, rowY, 70);
            _panelTx.Controls.Add(_nudTxFreq);
            _panelTx.Controls.Add(MakeParamLabel("kHz", col4X + 132, rowY, 36, cMuted, f8));

            rowY += 26;

            // ---- 第2行 ----
            _panelTx.Controls.Add(MakeParamLabel("脉冲宽度", col1X, rowY, 56, cMuted, f8));
            _nudTxPulseWidth = MakeParamNud(20, 2000, 100, 10, 0, col1X + 60, rowY, 70);
            _panelTx.Controls.Add(_nudTxPulseWidth);
            _panelTx.Controls.Add(MakeParamLabel("ns", col1X + 132, rowY, 26, cMuted, f8));

            _panelTx.Controls.Add(MakeParamLabel("接收滤波", col2X, rowY, 56, cMuted, f8));
            _nudTxFilter = MakeParamNud(100, 25000, 3100, 100, 0, col2X + 60, rowY, 70);
            _panelTx.Controls.Add(_nudTxFilter);
            _panelTx.Controls.Add(MakeParamLabel("kHz", col2X + 132, rowY, 36, cMuted, f8));

            _panelTx.Controls.Add(MakeParamLabel("采样率", col3X, rowY, 56, cMuted, f8));
            _nudTxSampleRate = MakeParamNud(10, 500, 100, 10, 0, col3X + 60, rowY, 70);
            _panelTx.Controls.Add(_nudTxSampleRate);
            _panelTx.Controls.Add(MakeParamLabel("MHz", col3X + 132, rowY, 36, cMuted, f8));

            _panelTx.Visible = false;
            this._tabContentHost.Controls.Add(_panelTx);
        }

        // ---- 2. 接收参数 (水平) ----
        private void BuildTabPanelRx_Horizontal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelRx = new System.Windows.Forms.Panel();
            _panelRx.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelRx.BackColor = System.Drawing.Color.Transparent;

            int rowY = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 接收参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(56, 130, 246);
            hdr.Size = new System.Drawing.Size(100, 20);
            hdr.Location = new System.Drawing.Point(4, rowY);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelRx.Controls.Add(hdr);

            // 6列, 间距180px: col1=4, col2=184, col3=364, col4=544, col5=724, col6=904
            int col1X = 4, col2X = 184, col3X = 364, col4X = 544, col5X = 724, col6X = 904;
            rowY += 24;

            _panelRx.Controls.Add(MakeParamLabel("数字增益", col1X, rowY, 56, cMuted, f8));
            _nudRxDigitalGain = MakeParamNud(0, 120, 0.0m, 0.1m, 1, col1X + 60, rowY, 64);
            _panelRx.Controls.Add(_nudRxDigitalGain);
            _panelRx.Controls.Add(MakeParamLabel("dB", col1X + 126, rowY, 26, cMuted, f8));

            _panelRx.Controls.Add(MakeParamLabel("模拟增益", col2X, rowY, 56, cMuted, f8));
            _nudRxAnalogGain = MakeParamNud(0, 120, 20.5m, 0.1m, 1, col2X + 60, rowY, 64);
            _panelRx.Controls.Add(_nudRxAnalogGain);
            _panelRx.Controls.Add(MakeParamLabel("dB", col2X + 126, rowY, 26, cMuted, f8));

            _panelRx.Controls.Add(MakeParamLabel("高压", col3X, rowY, 56, cMuted, f8));
            _nudRxVoltage = MakeParamNud(10, 200, 50, 5, 0, col3X + 60, rowY, 64);
            _panelRx.Controls.Add(_nudRxVoltage);
            _panelRx.Controls.Add(MakeParamLabel("V", col3X + 126, rowY, 26, cMuted, f8));

            _panelRx.Controls.Add(MakeParamLabel("整流方式", col4X, rowY, 56, cMuted, f8));
            _cmbRxRectify = MakeParamCombo(new[] { "全波", "半波", "射频" }, 0, col4X + 60, rowY, 64, f8);
            _panelRx.Controls.Add(_cmbRxRectify);

            _panelRx.Controls.Add(MakeParamLabel("平均次数", col5X, rowY, 56, cMuted, f8));
            _nudRxAverage = MakeParamNud(1, 64, 1, 1, 0, col5X + 60, rowY, 64);
            _panelRx.Controls.Add(_nudRxAverage);

            _panelRx.Controls.Add(MakeParamLabel("阻尼", col6X, rowY, 56, cMuted, f8));
            _nudRxDamping = MakeParamNud(25, 1000, 50, 25, 0, col6X + 60, rowY, 64);
            _panelRx.Controls.Add(_nudRxDamping);
            _panelRx.Controls.Add(MakeParamLabel("Ω", col6X + 126, rowY, 26, cMuted, f8));

            _panelRx.Visible = false;
            this._tabContentHost.Controls.Add(_panelRx);
        }

        // ---- 3. 探头参数 (水平) ----
        private void BuildTabPanelProbe_Horizontal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelProbe = new System.Windows.Forms.Panel();
            _panelProbe.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelProbe.BackColor = System.Drawing.Color.Transparent;

            int rowY = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 探头参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(168, 85, 247);
            hdr.Size = new System.Drawing.Size(100, 20);
            hdr.Location = new System.Drawing.Point(4, rowY);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelProbe.Controls.Add(hdr);

            // 6列, 间距180px
            int col1X = 4, col2X = 184, col3X = 364, col4X = 544, col5X = 724, col6X = 904;
            rowY += 24;

            _panelProbe.Controls.Add(MakeParamLabel("探头类型", col1X, rowY, 56, cMuted, f8));
            _cmbProbeType = MakeParamCombo(new[] { "线阵", "面阵", "弧阵" }, 0, col1X + 60, rowY, 64, f8);
            _panelProbe.Controls.Add(_cmbProbeType);

            _panelProbe.Controls.Add(MakeParamLabel("晶片总数", col2X, rowY, 56, cMuted, f8));
            _nudProbeElements = MakeParamNud(1, 256, 64, 1, 0, col2X + 60, rowY, 64);
            _panelProbe.Controls.Add(_nudProbeElements);

            _panelProbe.Controls.Add(MakeParamLabel("晶片间距", col3X, rowY, 56, cMuted, f8));
            _nudProbePitch = MakeParamNud(0.01m, 10, 0.6m, 0.01m, 2, col3X + 60, rowY, 64);
            _panelProbe.Controls.Add(_nudProbePitch);
            _panelProbe.Controls.Add(MakeParamLabel("mm", col3X + 126, rowY, 26, cMuted, f8));

            _panelProbe.Controls.Add(MakeParamLabel("首晶片位", col4X, rowY, 56, cMuted, f8));
            _nudProbeFirstElem = MakeParamNud(0, 255, 0, 1, 0, col4X + 60, rowY, 64);
            _panelProbe.Controls.Add(_nudProbeFirstElem);

            _panelProbe.Controls.Add(MakeParamLabel("晶片频率", col5X, rowY, 56, cMuted, f8));
            _nudProbeFreq = MakeParamNud(0.5m, 25, 5.0m, 0.5m, 1, col5X + 60, rowY, 64);
            _panelProbe.Controls.Add(_nudProbeFreq);
            _panelProbe.Controls.Add(MakeParamLabel("MHz", col5X + 126, rowY, 36, cMuted, f8));

            _panelProbe.Controls.Add(MakeParamLabel("晶片宽度", col6X, rowY, 56, cMuted, f8));
            _nudProbeWidth = MakeParamNud(0.1m, 10, 0.5m, 0.1m, 2, col6X + 60, rowY, 64);
            _panelProbe.Controls.Add(_nudProbeWidth);
            _panelProbe.Controls.Add(MakeParamLabel("mm", col6X + 126, rowY, 26, cMuted, f8));

            _panelProbe.Visible = false;
            this._tabContentHost.Controls.Add(_panelProbe);
        }

        // ---- 4. 楔块参数 (水平) ----
        private void BuildTabPanelWedge_Horizontal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelWedge = new System.Windows.Forms.Panel();
            _panelWedge.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelWedge.BackColor = System.Drawing.Color.Transparent;

            int rowY = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 楔块参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            hdr.Size = new System.Drawing.Size(100, 20);
            hdr.Location = new System.Drawing.Point(4, rowY);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelWedge.Controls.Add(hdr);

            // 5列, 间距180px (nud=70)
            int col1X = 4, col2X = 184, col3X = 364, col4X = 544, col5X = 724;
            rowY += 24;

            _panelWedge.Controls.Add(MakeParamLabel("楔块角度", col1X, rowY, 56, cMuted, f8));
            _nudWedgeAngle = MakeParamNud(0, 85, 36.0m, 0.5m, 1, col1X + 60, rowY, 70);
            _panelWedge.Controls.Add(_nudWedgeAngle);
            _panelWedge.Controls.Add(MakeParamLabel("°", col1X + 132, rowY, 26, cMuted, f8));

            _panelWedge.Controls.Add(MakeParamLabel("楔块声速", col2X, rowY, 56, cMuted, f8));
            _nudWedgeVelocity = MakeParamNud(1000, 10000, 2337, 1, 0, col2X + 60, rowY, 70);
            _panelWedge.Controls.Add(_nudWedgeVelocity);
            _panelWedge.Controls.Add(MakeParamLabel("m/s", col2X + 132, rowY, 36, cMuted, f8));

            _panelWedge.Controls.Add(MakeParamLabel("楔块高度", col3X, rowY, 56, cMuted, f8));
            _nudWedgeHeight = MakeParamNud(0, 100, 20.0m, 0.5m, 1, col3X + 60, rowY, 70);
            _panelWedge.Controls.Add(_nudWedgeHeight);
            _panelWedge.Controls.Add(MakeParamLabel("mm", col3X + 132, rowY, 26, cMuted, f8));

            _panelWedge.Controls.Add(MakeParamLabel("前端高度", col4X, rowY, 56, cMuted, f8));
            _nudWedgeFrontH = MakeParamNud(0, 100, 10.0m, 0.5m, 1, col4X + 60, rowY, 70);
            _panelWedge.Controls.Add(_nudWedgeFrontH);
            _panelWedge.Controls.Add(MakeParamLabel("mm", col4X + 132, rowY, 26, cMuted, f8));

            _panelWedge.Controls.Add(MakeParamLabel("楔块偏移", col5X, rowY, 56, cMuted, f8));
            _nudWedgeOffset = MakeParamNud(-100, 100, 0, 0.5m, 1, col5X + 60, rowY, 70);
            _panelWedge.Controls.Add(_nudWedgeOffset);
            _panelWedge.Controls.Add(MakeParamLabel("mm", col5X + 132, rowY, 26, cMuted, f8));

            _panelWedge.Visible = false;
            this._tabContentHost.Controls.Add(_panelWedge);
        }

        // ---- 5. 材料参数 (水平) ----
        private void BuildTabPanelMaterial_Horizontal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelMaterial = new System.Windows.Forms.Panel();
            _panelMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelMaterial.BackColor = System.Drawing.Color.Transparent;

            int rowY = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 材料参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            hdr.Size = new System.Drawing.Size(100, 20);
            hdr.Location = new System.Drawing.Point(4, rowY);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelMaterial.Controls.Add(hdr);

            // 4列, 间距180px
            int col1X = 4, col2X = 184, col3X = 364, col4X = 544;
            rowY += 24;

            _panelMaterial.Controls.Add(MakeParamLabel("纵波声速", col1X, rowY, 56, cMuted, f8));
            _nudMatLongVel = MakeParamNud(1000, 15000, 5900, 10, 0, col1X + 60, rowY, 70);
            _panelMaterial.Controls.Add(_nudMatLongVel);
            _panelMaterial.Controls.Add(MakeParamLabel("m/s", col1X + 132, rowY, 36, cMuted, f8));

            _panelMaterial.Controls.Add(MakeParamLabel("横波声速", col2X, rowY, 56, cMuted, f8));
            _nudMatShearVel = MakeParamNud(500, 10000, 3230, 10, 0, col2X + 60, rowY, 70);
            _panelMaterial.Controls.Add(_nudMatShearVel);
            _panelMaterial.Controls.Add(MakeParamLabel("m/s", col2X + 132, rowY, 36, cMuted, f8));

            _panelMaterial.Controls.Add(MakeParamLabel("材料厚度", col3X, rowY, 56, cMuted, f8));
            _nudMatThickness = MakeParamNud(0.1m, 9999, 50.0m, 0.5m, 1, col3X + 60, rowY, 70);
            _panelMaterial.Controls.Add(_nudMatThickness);
            _panelMaterial.Controls.Add(MakeParamLabel("mm", col3X + 132, rowY, 26, cMuted, f8));

            _panelMaterial.Controls.Add(MakeParamLabel("衰减系数", col4X, rowY, 56, cMuted, f8));
            _nudMatAtten = MakeParamNud(0, 99, 0, 0.01m, 2, col4X + 60, rowY, 70);
            _panelMaterial.Controls.Add(_nudMatAtten);
            _panelMaterial.Controls.Add(MakeParamLabel("dB/m", col4X + 132, rowY, 36, cMuted, f8));

            _panelMaterial.Visible = false;
            this._tabContentHost.Controls.Add(_panelMaterial);
        }

        // ---- 6. 孔径参数 (水平) ----
        private void BuildTabPanelAperture_Horizontal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelAperture = new System.Windows.Forms.Panel();
            _panelAperture.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelAperture.BackColor = System.Drawing.Color.Transparent;

            int rowY = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 孔径参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(236, 72, 153);
            hdr.Size = new System.Drawing.Size(100, 20);
            hdr.Location = new System.Drawing.Point(4, rowY);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelAperture.Controls.Add(hdr);

            // 5列, 间距180px
            int col1X = 4, col2X = 184, col3X = 364, col4X = 544, col5X = 724;
            rowY += 24;

            _panelAperture.Controls.Add(MakeParamLabel("起始晶片", col1X, rowY, 56, cMuted, f8));
            _nudApertureStart = MakeParamNud(1, 256, 1, 1, 0, col1X + 60, rowY, 64);
            _panelAperture.Controls.Add(_nudApertureStart);

            _panelAperture.Controls.Add(MakeParamLabel("晶片数量", col2X, rowY, 56, cMuted, f8));
            _nudApertureCount = MakeParamNud(1, 256, 16, 1, 0, col2X + 60, rowY, 64);
            _panelAperture.Controls.Add(_nudApertureCount);

            _panelAperture.Controls.Add(MakeParamLabel("聚焦深度", col3X, rowY, 56, cMuted, f8));
            _nudApertureFocus = MakeParamNud(0, 9999, 30.0m, 1.0m, 1, col3X + 60, rowY, 70);
            _panelAperture.Controls.Add(_nudApertureFocus);
            _panelAperture.Controls.Add(MakeParamLabel("mm", col3X + 132, rowY, 26, cMuted, f8));

            _panelAperture.Controls.Add(MakeParamLabel("孔径类型", col4X, rowY, 56, cMuted, f8));
            _cmbApertureType = MakeParamCombo(new[] { "固定", "动态" }, 0, col4X + 60, rowY, 64, f8);
            _panelAperture.Controls.Add(_cmbApertureType);

            _panelAperture.Controls.Add(MakeParamLabel("偏转角度", col5X, rowY, 56, cMuted, f8));
            _nudApertureAngle = MakeParamNud(-80, 80, 0, 0.5m, 1, col5X + 60, rowY, 70);
            _panelAperture.Controls.Add(_nudApertureAngle);
            _panelAperture.Controls.Add(MakeParamLabel("°", col5X + 132, rowY, 26, cMuted, f8));

            _panelAperture.Visible = false;
            this._tabContentHost.Controls.Add(_panelAperture);
        }

        // ---- 7. 闸门参数 (水平) ----
        private void BuildTabPanelGate_Horizontal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelGate = new System.Windows.Forms.Panel();
            _panelGate.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelGate.BackColor = System.Drawing.Color.Transparent;

            int rowY = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 闸门参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(251, 191, 36);
            hdr.Size = new System.Drawing.Size(100, 20);
            hdr.Location = new System.Drawing.Point(4, rowY);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelGate.Controls.Add(hdr);

            // 闸门A 左半边, 闸门B 右半边, 间距400px
            int gaX = 4, gbX = 420;
            rowY += 24;

            var gaHdr = new System.Windows.Forms.Label();
            gaHdr.Text = "▸ 闸门A";
            gaHdr.Font = new System.Drawing.Font("微软雅黑", 8.5F, System.Drawing.FontStyle.Bold);
            gaHdr.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            gaHdr.Size = new System.Drawing.Size(60, 18);
            gaHdr.Location = new System.Drawing.Point(gaX, rowY);
            gaHdr.BackColor = System.Drawing.Color.Transparent;
            _panelGate.Controls.Add(gaHdr);

            var gbHdr = new System.Windows.Forms.Label();
            gbHdr.Text = "▸ 闸门B";
            gbHdr.Font = new System.Drawing.Font("微软雅黑", 8.5F, System.Drawing.FontStyle.Bold);
            gbHdr.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            gbHdr.Size = new System.Drawing.Size(60, 18);
            gbHdr.Location = new System.Drawing.Point(gbX, rowY);
            gbHdr.BackColor = System.Drawing.Color.Transparent;
            _panelGate.Controls.Add(gbHdr);

            rowY += 20;

            // ---- 闸门A: 起点 / 宽度 / 阈值 ----
            _panelGate.Controls.Add(MakeParamLabel("起点", gaX, rowY, 30, cMuted, f8));
            _nudGateAStart = MakeParamNud(0, 9999, 10.0m, 1.0m, 1, gaX + 36, rowY, 56);
            _panelGate.Controls.Add(_nudGateAStart);
            _panelGate.Controls.Add(MakeParamLabel("μs", gaX + 94, rowY, 26, cMuted, f8));

            _panelGate.Controls.Add(MakeParamLabel("宽度", gaX + 130, rowY, 30, cMuted, f8));
            _nudGateAWidth = MakeParamNud(0, 9999, 50.0m, 1.0m, 1, gaX + 166, rowY, 56);
            _panelGate.Controls.Add(_nudGateAWidth);
            _panelGate.Controls.Add(MakeParamLabel("μs", gaX + 224, rowY, 26, cMuted, f8));

            _panelGate.Controls.Add(MakeParamLabel("阈值", gaX + 260, rowY, 30, cMuted, f8));
            _nudGateAThld = MakeParamNud(0, 100, 50, 1, 0, gaX + 296, rowY, 56);
            _panelGate.Controls.Add(_nudGateAThld);
            _panelGate.Controls.Add(MakeParamLabel("%", gaX + 354, rowY, 26, cMuted, f8));

            // ---- 闸门B: 起点 / 宽度 / 阈值 ----
            _panelGate.Controls.Add(MakeParamLabel("起点", gbX, rowY, 30, cMuted, f8));
            _nudGateBStart = MakeParamNud(0, 9999, 20.0m, 1.0m, 1, gbX + 36, rowY, 56);
            _panelGate.Controls.Add(_nudGateBStart);
            _panelGate.Controls.Add(MakeParamLabel("μs", gbX + 94, rowY, 26, cMuted, f8));

            _panelGate.Controls.Add(MakeParamLabel("宽度", gbX + 130, rowY, 30, cMuted, f8));
            _nudGateBWidth = MakeParamNud(0, 9999, 40.0m, 1.0m, 1, gbX + 166, rowY, 56);
            _panelGate.Controls.Add(_nudGateBWidth);
            _panelGate.Controls.Add(MakeParamLabel("μs", gbX + 224, rowY, 26, cMuted, f8));

            _panelGate.Controls.Add(MakeParamLabel("阈值", gbX + 260, rowY, 30, cMuted, f8));
            _nudGateBThld = MakeParamNud(0, 100, 50, 1, 0, gbX + 296, rowY, 56);
            _panelGate.Controls.Add(_nudGateBThld);
            _panelGate.Controls.Add(MakeParamLabel("%", gbX + 354, rowY, 26, cMuted, f8));

            _panelGate.Visible = false;
            this._tabContentHost.Controls.Add(_panelGate);
        }

        // ---- 8. 扫查参数 (水平) ----
        private void BuildTabPanelScan_Horizontal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelScan = new System.Windows.Forms.Panel();
            _panelScan.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelScan.BackColor = System.Drawing.Color.Transparent;

            int rowY = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 扫查参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            hdr.Size = new System.Drawing.Size(100, 20);
            hdr.Location = new System.Drawing.Point(4, rowY);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelScan.Controls.Add(hdr);

            // 5列, 间距180px
            int col1X = 4, col2X = 184, col3X = 364, col4X = 544, col5X = 724;
            rowY += 24;

            _panelScan.Controls.Add(MakeParamLabel("扫查类型", col1X, rowY, 56, cMuted, f8));
            _cmbScanType = MakeParamCombo(new[] { "S扫", "L扫", "CL扫" }, 0, col1X + 60, rowY, 64, f8);
            _panelScan.Controls.Add(_cmbScanType);

            _panelScan.Controls.Add(MakeParamLabel("起始角度", col2X, rowY, 56, cMuted, f8));
            _nudScanStartAngle = MakeParamNud(-90, 90, -45, 1.0m, 1, col2X + 60, rowY, 64);
            _panelScan.Controls.Add(_nudScanStartAngle);
            _panelScan.Controls.Add(MakeParamLabel("°", col2X + 126, rowY, 26, cMuted, f8));

            _panelScan.Controls.Add(MakeParamLabel("终止角度", col3X, rowY, 56, cMuted, f8));
            _nudScanEndAngle = MakeParamNud(-90, 90, 45, 1.0m, 1, col3X + 60, rowY, 64);
            _panelScan.Controls.Add(_nudScanEndAngle);
            _panelScan.Controls.Add(MakeParamLabel("°", col3X + 126, rowY, 26, cMuted, f8));

            _panelScan.Controls.Add(MakeParamLabel("角度步进", col4X, rowY, 56, cMuted, f8));
            _nudScanAngleStep = MakeParamNud(0.1m, 10, 1.0m, 0.1m, 1, col4X + 60, rowY, 64);
            _panelScan.Controls.Add(_nudScanAngleStep);
            _panelScan.Controls.Add(MakeParamLabel("°", col4X + 126, rowY, 26, cMuted, f8));

            _panelScan.Controls.Add(MakeParamLabel("深度范围", col5X, rowY, 56, cMuted, f8));
            _nudScanDepth = MakeParamNud(1, 9999, 200.0m, 10.0m, 1, col5X + 60, rowY, 70);
            _panelScan.Controls.Add(_nudScanDepth);
            _panelScan.Controls.Add(MakeParamLabel("mm", col5X + 132, rowY, 26, cMuted, f8));

            _panelScan.Visible = false;
            this._tabContentHost.Controls.Add(_panelScan);
        }

        // ---- 9. 编码器参数 (水平) ----
        private void BuildTabPanelEncoder_Horizontal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelEncoder = new System.Windows.Forms.Panel();
            _panelEncoder.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelEncoder.BackColor = System.Drawing.Color.Transparent;

            int rowY = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 编码器参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(14, 165, 233);
            hdr.Size = new System.Drawing.Size(100, 20);
            hdr.Location = new System.Drawing.Point(4, rowY);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelEncoder.Controls.Add(hdr);

            // col1宽标签(76px), 5列: col1=4, col2=200, col3=360, col4=520, col5=680
            int col1X = 4, col2X = 200, col3X = 360, col4X = 520, col5X = 680;
            rowY += 24;

            _panelEncoder.Controls.Add(MakeParamLabel("编码器分辨率", col1X, rowY, 76, cMuted, f8));
            _nudEncRes = MakeParamNud(1, 5000, 100, 1, 0, col1X + 80, rowY, 70);
            _panelEncoder.Controls.Add(_nudEncRes);
            _panelEncoder.Controls.Add(MakeParamLabel("P/mm", col1X + 152, rowY, 36, cMuted, f8));

            _panelEncoder.Controls.Add(MakeParamLabel("触发方式", col2X, rowY, 56, cMuted, f8));
            _cmbEncTrig = MakeParamCombo(new[] { "等距触发", "等时触发" }, 0, col2X + 60, rowY, 70, f8);
            _panelEncoder.Controls.Add(_cmbEncTrig);

            _panelEncoder.Controls.Add(MakeParamLabel("触发间距", col3X, rowY, 56, cMuted, f8));
            _nudEncInterval = MakeParamNud(0.1m, 100, 1.0m, 0.1m, 1, col3X + 60, rowY, 64);
            _panelEncoder.Controls.Add(_nudEncInterval);
            _panelEncoder.Controls.Add(MakeParamLabel("mm", col3X + 126, rowY, 26, cMuted, f8));

            _panelEncoder.Controls.Add(MakeParamLabel("X步进", col4X, rowY, 56, cMuted, f8));
            _nudEncXStep = MakeParamNud(0.1m, 100, 7.8m, 0.1m, 1, col4X + 60, rowY, 64);
            _panelEncoder.Controls.Add(_nudEncXStep);
            _panelEncoder.Controls.Add(MakeParamLabel("mm", col4X + 126, rowY, 26, cMuted, f8));

            _panelEncoder.Controls.Add(MakeParamLabel("Y步进", col5X, rowY, 56, cMuted, f8));
            _nudEncYStep = MakeParamNud(0.1m, 100, 1.6m, 0.1m, 1, col5X + 60, rowY, 64);
            _panelEncoder.Controls.Add(_nudEncYStep);
            _panelEncoder.Controls.Add(MakeParamLabel("mm", col5X + 126, rowY, 26, cMuted, f8));

            _panelEncoder.Visible = false;
            this._tabContentHost.Controls.Add(_panelEncoder);
        }

        // ---- 10. 校准参数 (水平) ----
        private void BuildTabPanelCal_Horizontal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Color cGreen,
            System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelCal = new System.Windows.Forms.Panel();
            _panelCal.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelCal.BackColor = System.Drawing.Color.Transparent;

            int rowY = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● 校准参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            hdr.Size = new System.Drawing.Size(100, 20);
            hdr.Location = new System.Drawing.Point(4, rowY);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelCal.Controls.Add(hdr);

            // 4列, 间距180px
            int col1X = 4, col2X = 184, col3X = 364, col4X = 544;
            rowY += 24;

            _panelCal.Controls.Add(MakeParamLabel("声速校准", col1X, rowY, 56, cMuted, f8));
            _nudCalVelocity = MakeParamNud(500, 15000, 5900, 10, 0, col1X + 60, rowY, 70);
            _panelCal.Controls.Add(_nudCalVelocity);
            _panelCal.Controls.Add(MakeParamLabel("m/s", col1X + 132, rowY, 36, cMuted, f8));

            _panelCal.Controls.Add(MakeParamLabel("零位校准", col2X, rowY, 56, cMuted, f8));
            _nudCalZero = MakeParamNud(0, 9999, 0, 0.1m, 1, col2X + 60, rowY, 70);
            _panelCal.Controls.Add(_nudCalZero);
            _panelCal.Controls.Add(MakeParamLabel("μs", col2X + 132, rowY, 26, cMuted, f8));

            this._chkACG = new System.Windows.Forms.CheckBox();
            this._chkACG.Text = "ACG 角度增益补偿";
            this._chkACG.Checked = false;
            this._chkACG.AutoSize = false;
            this._chkACG.Font = f85;
            this._chkACG.ForeColor = cGreen;
            this._chkACG.BackColor = System.Drawing.Color.Transparent;
            this._chkACG.Size = new System.Drawing.Size(160, 20);
            this._chkACG.Location = new System.Drawing.Point(col3X, rowY);
            this._chkACG.Cursor = System.Windows.Forms.Cursors.Hand;
            _panelCal.Controls.Add(this._chkACG);

            this._chkTCG = new System.Windows.Forms.CheckBox();
            this._chkTCG.Text = "TCG 时间增益补偿";
            this._chkTCG.Checked = false;
            this._chkTCG.AutoSize = false;
            this._chkTCG.Font = f85;
            this._chkTCG.ForeColor = cGreen;
            this._chkTCG.BackColor = System.Drawing.Color.Transparent;
            this._chkTCG.Size = new System.Drawing.Size(160, 20);
            this._chkTCG.Location = new System.Drawing.Point(col4X, rowY);
            this._chkTCG.Cursor = System.Windows.Forms.Cursors.Hand;
            _panelCal.Controls.Add(this._chkTCG);

            rowY += 26;

            _panelCal.Controls.Add(MakeParamLabel("TCG增益", col1X, rowY, 56, cMuted, f8));
            _nudCalTCGGain = MakeParamNud(0, 80, 0, 0.1m, 1, col1X + 60, rowY, 64);
            _panelCal.Controls.Add(_nudCalTCGGain);
            _panelCal.Controls.Add(MakeParamLabel("dB", col1X + 126, rowY, 26, cMuted, f8));

            _panelCal.Visible = false;
            this._tabContentHost.Controls.Add(_panelCal);
        }

        // ---- 11. TFM参数 (水平) ----
        private void BuildTabPanelTfm_Horizontal(System.Drawing.Color cBg, System.Drawing.Color cText,
            System.Drawing.Color cMuted, System.Drawing.Font f8, System.Drawing.Font f85)
        {
            _panelTfm = new System.Windows.Forms.Panel();
            _panelTfm.Dock = System.Windows.Forms.DockStyle.Fill;
            _panelTfm.BackColor = System.Drawing.Color.Transparent;

            int rowY = 4;
            var hdr = new System.Windows.Forms.Label();
            hdr.Text = "● TFM参数";
            hdr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            hdr.ForeColor = System.Drawing.Color.FromArgb(168, 85, 247);
            hdr.Size = new System.Drawing.Size(100, 20);
            hdr.Location = new System.Drawing.Point(4, rowY);
            hdr.BackColor = System.Drawing.Color.Transparent;
            _panelTfm.Controls.Add(hdr);

            // 5列, 间距180px
            int col1X = 4, col2X = 184, col3X = 364, col4X = 544, col5X = 724;
            rowY += 24;

            _panelTfm.Controls.Add(MakeParamLabel("TFM模式", col1X, rowY, 56, cMuted, f8));
            _cmbTfmMode = MakeParamCombo(new[] { "全聚焦", "半聚焦" }, 0, col1X + 60, rowY, 64, f8);
            _panelTfm.Controls.Add(_cmbTfmMode);

            _panelTfm.Controls.Add(MakeParamLabel("网格分辨率", col2X, rowY, 56, cMuted, f8));
            _nudTfmGridRes = MakeParamNud(0.1m, 5, 0.5m, 0.1m, 1, col2X + 60, rowY, 64);
            _panelTfm.Controls.Add(_nudTfmGridRes);
            _panelTfm.Controls.Add(MakeParamLabel("mm", col2X + 126, rowY, 26, cMuted, f8));

            _panelTfm.Controls.Add(MakeParamLabel("成像范围X", col3X, rowY, 56, cMuted, f8));
            _nudTfmRangeX = MakeParamNud(1, 999, 50.0m, 1.0m, 1, col3X + 60, rowY, 64);
            _panelTfm.Controls.Add(_nudTfmRangeX);
            _panelTfm.Controls.Add(MakeParamLabel("mm", col3X + 126, rowY, 26, cMuted, f8));

            _panelTfm.Controls.Add(MakeParamLabel("成像范围Z", col4X, rowY, 56, cMuted, f8));
            _nudTfmRangeZ = MakeParamNud(1, 999, 50.0m, 1.0m, 1, col4X + 60, rowY, 64);
            _panelTfm.Controls.Add(_nudTfmRangeZ);
            _panelTfm.Controls.Add(MakeParamLabel("mm", col4X + 126, rowY, 26, cMuted, f8));

            _panelTfm.Controls.Add(MakeParamLabel("重构模式", col5X, rowY, 56, cMuted, f8));
            _cmbTfmReconMode = MakeParamCombo(new[] { "直接", "全矩阵捕获" }, 0, col5X + 60, rowY, 64, f8);
            _panelTfm.Controls.Add(_cmbTfmReconMode);

            _panelTfm.Visible = false;
            this._tabContentHost.Controls.Add(_panelTfm);
        }

        #endregion

        // ======== 控件字段 ========

        // 顶栏
        private System.Windows.Forms.Panel _topBar;
        private System.Windows.Forms.Button _btnConnect;
        private System.Windows.Forms.Button _btnSettings;
        private System.Windows.Forms.Panel _viewModeBar;
        private System.Windows.Forms.CheckBox _chkGate;
        private System.Windows.Forms.CheckBox _chkTcg;
        private System.Windows.Forms.CheckBox _chkEnvelope;
        private System.Windows.Forms.CheckBox _chkPeakHold;
        private System.Windows.Forms.Label _lblQuickApply;
        private System.Windows.Forms.Label _lblComStatus;

        // 底栏
        private System.Windows.Forms.Panel _bottomBar;
        private System.Windows.Forms.Label _lblPosX;
        private System.Windows.Forms.Label _lblPosY;
        private System.Windows.Forms.Button _btnJogLeft;
        private System.Windows.Forms.Button _btnJogStepLeft;
        private System.Windows.Forms.Button _btnJogStepRight;
        private System.Windows.Forms.Button _btnJogRight;
        private System.Windows.Forms.Button _btnHome;
        private System.Windows.Forms.Button _btnEstop;
        private System.Windows.Forms.ProgressBar _scanProgress;
        private System.Windows.Forms.Label _lblScanProgress;
        private System.Windows.Forms.Label _lblStatus;
        private System.Windows.Forms.Label _lblDateTime;

        // 底部参数面板
        private System.Windows.Forms.Panel _bottomParamPanel;
        private System.Windows.Forms.Panel _catPanel;
        private System.Windows.Forms.Panel _tabContentHost;
        private System.Windows.Forms.Panel _panelScanCtrl;
        private System.Windows.Forms.CheckBox _chkEncoderEnable;
        private System.Windows.Forms.ComboBox _cmbScanDir;
        private System.Windows.Forms.NumericUpDown _nudScanSpeed;
        private System.Windows.Forms.ProgressBar _scanCtrlProgress;
        private System.Windows.Forms.Button _btnScanStart;
        private System.Windows.Forms.Button _btnScanStop;
        private System.Collections.Generic.List<System.Windows.Forms.Button> _catButtons
            = new System.Collections.Generic.List<System.Windows.Forms.Button>();

        // 12个Tab参数面板
        private System.Windows.Forms.Panel _panelTx;
        private System.Windows.Forms.NumericUpDown _nudTxOffset, _nudTxGain, _nudTxRange;
        private System.Windows.Forms.NumericUpDown _nudTxFreq, _nudTxPulseWidth, _nudTxFilter, _nudTxSampleRate;

        private System.Windows.Forms.Panel _panelRx;
        private System.Windows.Forms.NumericUpDown _nudRxDigitalGain, _nudRxAnalogGain, _nudRxVoltage;
        private System.Windows.Forms.NumericUpDown _nudRxAverage, _nudRxDamping;
        private System.Windows.Forms.ComboBox _cmbRxRectify;

        private System.Windows.Forms.Panel _panelProbe;
        private System.Windows.Forms.ComboBox _cmbProbeType;
        private System.Windows.Forms.NumericUpDown _nudProbeElements, _nudProbePitch, _nudProbeFirstElem;
        private System.Windows.Forms.NumericUpDown _nudProbeFreq, _nudProbeWidth;

        private System.Windows.Forms.Panel _panelWedge;
        private System.Windows.Forms.NumericUpDown _nudWedgeAngle, _nudWedgeVelocity, _nudWedgeHeight;
        private System.Windows.Forms.NumericUpDown _nudWedgeFrontH, _nudWedgeOffset;

        private System.Windows.Forms.Panel _panelMaterial;
        private System.Windows.Forms.NumericUpDown _nudMatLongVel, _nudMatShearVel, _nudMatThickness, _nudMatAtten;

        private System.Windows.Forms.Panel _panelAperture;
        private System.Windows.Forms.NumericUpDown _nudApertureStart, _nudApertureCount, _nudApertureFocus, _nudApertureAngle;
        private System.Windows.Forms.ComboBox _cmbApertureType;

        private System.Windows.Forms.Panel _panelGate;
        private System.Windows.Forms.NumericUpDown _nudGateAStart, _nudGateAWidth, _nudGateAThld;
        private System.Windows.Forms.NumericUpDown _nudGateBStart, _nudGateBWidth, _nudGateBThld;

        private System.Windows.Forms.Panel _panelScan;
        private System.Windows.Forms.ComboBox _cmbScanType;
        private System.Windows.Forms.NumericUpDown _nudScanStartAngle, _nudScanEndAngle;
        private System.Windows.Forms.NumericUpDown _nudScanAngleStep, _nudScanDepth;

        private System.Windows.Forms.Panel _panelEncoder;
        private System.Windows.Forms.NumericUpDown _nudEncRes, _nudEncInterval, _nudEncXStep, _nudEncYStep;
        private System.Windows.Forms.ComboBox _cmbEncTrig;

        private System.Windows.Forms.Panel _panelCal;
        private System.Windows.Forms.CheckBox _chkACG;
        private System.Windows.Forms.CheckBox _chkTCG;
        private System.Windows.Forms.NumericUpDown _nudCalVelocity, _nudCalZero, _nudCalTCGGain;

        private System.Windows.Forms.Panel _panelTfm;
        private System.Windows.Forms.ComboBox _cmbTfmMode, _cmbTfmReconMode;
        private System.Windows.Forms.NumericUpDown _nudTfmGridRes, _nudTfmRangeX, _nudTfmRangeZ;

        // 右栏
        private System.Windows.Forms.Panel _rightPanel;

        // 中心四象限
        private System.Windows.Forms.TableLayoutPanel _centerPanel;
        private System.Windows.Forms.Panel _panelQ1;
        private System.Windows.Forms.Panel _panelQ2;
        private System.Windows.Forms.Panel _panelQ3;
        private System.Windows.Forms.Panel _panelQ4;
    }
}
