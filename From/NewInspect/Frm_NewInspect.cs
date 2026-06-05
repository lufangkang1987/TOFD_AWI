using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewInspect.UI;

namespace Tofd_AWI.From.NewInspect
{
    // ============================================================
    // Frm_NewInspect.cs  — 相控阵超声检测主界面
    //
    // 布局: 顶栏(44) | 左栏(200,Tab参数) | 右栏(72) | 底栏(38) | 中心4象限
    //
    // 顶栏: [连接设备] [设置] | [A扫][B扫][C扫][TFM] | ☑闸门 TC... | 快速应用 | ○未连接
    // 左栏: Tab分类按钮(发射/接收/探头/楔块/材料/孔径/闸门/扫查/编码器/校准/TFM)
    //       + 当前Tab参数面板 + 扫查控制面板(绿色区块,Dock=Bottom)
    // 右栏: 竖排功能按钮
    // 底栏: 坐标 + JOG + 回零/急停 + 进度条 + 状态 + 时间
    // 中心: TableLayoutPanel 2x2 四象限 (A扫/S扫/L扫/C扫)
    // ============================================================

    public partial class Frm_NewInspect : Form
    {
        // 深色主题配色
        private static readonly Color BG_DARK      = Color.FromArgb(10, 14, 26);
        private static readonly Color BG_PANEL     = Color.FromArgb(16, 22, 36);
        private static readonly Color BG_INPUT     = Color.FromArgb(24, 30, 44);
        private static readonly Color CLR_TEXT     = Color.FromArgb(200, 210, 225);
        private static readonly Color CLR_MUTED    = Color.FromArgb(100, 115, 140);
        private static readonly Color CLR_BLUE     = Color.FromArgb(56, 130, 246);
        private static readonly Color CLR_GREEN    = Color.FromArgb(34, 197, 94);
        private static readonly Color CLR_RED      = Color.FromArgb(239, 68, 68);
        private static readonly Color CLR_AMBER    = Color.FromArgb(245, 158, 11);
        private static readonly Color CLR_PURPLE   = Color.FromArgb(168, 85, 247);
        private static readonly Color CLR_PINK     = Color.FromArgb(236, 72, 153);
        private static readonly Color CLR_CYAN     = Color.FromArgb(14, 165, 233);
        private static readonly Color CLR_YELLOW   = Color.FromArgb(251, 191, 36);

        // 当前状态
        private string _activeViewMode = "ascan";
        private string _activeParamTab = "tx";
        private bool _isFrozen = false;
        public bool IsScanning { get; private set; } = false;

        // 定时器
        private System.Windows.Forms.Timer _connMonitorTimer;
        private System.Windows.Forms.Timer _dateTimeTimer;

        public Frm_NewInspect()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            ApplyDarkTheme(this);
            WireUpEvents();
            InitDateTimeTimer();

            // 启动时根据全局状态初始化顶栏
            UpdateComStatus(AppState.IsInstrumentConnected);

            // 断线检测定时器
            _connMonitorTimer = new System.Windows.Forms.Timer();
            _connMonitorTimer.Interval = 3000;
            _connMonitorTimer.Tick += ConnMonitorTimer_Tick;
            _connMonitorTimer.Start();
        }

        // ==========================================
        // 日期时间定时器
        // ==========================================
        private void InitDateTimeTimer()
        {
            _dateTimeTimer = new System.Windows.Forms.Timer();
            _dateTimeTimer.Interval = 1000;
            _dateTimeTimer.Tick += (s, e) =>
            {
                _lblDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            };
            _dateTimeTimer.Start();
            _lblDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // ==========================================
        // 深色主题递归应用
        // ==========================================
        public static void ApplyDarkTheme(Control control)
        {
            if (control == null) return;

            if (control is NumericUpDown nud)
            {
                nud.BackColor = BG_INPUT;
                nud.ForeColor = CLR_TEXT;
            }
            else if (control is ComboBox cb && cb.Tag == null)
            {
                cb.BackColor = BG_INPUT;
                cb.ForeColor = CLR_TEXT;
            }
            else if (control is ProgressBar prg)
            {
                prg.BackColor = BG_INPUT;
            }
            else if (control is TableLayoutPanel tlp)
            {
                tlp.BackColor = Color.FromArgb(8, 10, 18);
            }

            foreach (Control child in control.Controls)
            {
                ApplyDarkTheme(child);
            }
        }

        // ==========================================
        // 事件绑定
        // ==========================================
        #region WireUpEvents

        private void WireUpEvents()
        {
            // ★ 订阅全局连接状态变化事件
            AppState.ConnectionStateChanged += (s, connected) =>
            {
                if (InvokeRequired)
                    Invoke(new Action(() => UpdateComStatus(connected)));
                else
                    UpdateComStatus(connected);
            };

            // "连接" 按钮
            _btnConnect.Click += (s, e) => OpenConnectDialog();

            // "设置" 按钮
            _btnSettings.Click += (s, e) =>
            {
                using (var dlg = new FrmSettings(tabIndex: 0))
                    dlg.ShowDialog(this);
            };

            // 通信状态标签点击
            _lblComStatus.Click += (s, e) => OpenConnectDialog();

            // CheckBox 变更 → 快速应用
            _chkGate.CheckedChanged += QuickApplyChanged;
            _chkTcg.CheckedChanged += QuickApplyChanged;
            _chkEnvelope.CheckedChanged += QuickApplyChanged;
            _chkPeakHold.CheckedChanged += QuickApplyChanged;

            // 底栏事件
            _btnJogLeft.Click       += (s, e) => JogMove(-10);
            _btnJogStepLeft.Click   += (s, e) => JogMove(-1);
            _btnJogStepRight.Click  += (s, e) => JogMove(1);
            _btnJogRight.Click      += (s, e) => JogMove(10);
            _btnHome.Click          += (s, e) => HomeAxis();
            _btnEstop.Click         += (s, e) => EmergencyStop();

            // 窗体调整大小
            Resize += Frm_NewInspect_Resize;
        }

        #endregion

        // ==========================================
        // 连接
        // ==========================================
        #region 连接

        private void OpenConnectDialog()
        {
            using (var dlg = new FrmConnect())
            {
                var result = dlg.ShowDialog(this);
                if (result == DialogResult.OK)
                    UpdateComStatus(true);
                else if (result == DialogResult.Abort)
                    this.Close();
            }
        }

        private void ConnMonitorTimer_Tick(object sender, EventArgs e)
        {
            if (!AppState.IsInstrumentConnected)
                return;

            Task.Run(() =>
            {
                try
                {
                    using (var client = new System.Net.Sockets.TcpClient())
                    {
                        var task = client.ConnectAsync(AppState.CurrentIp, 51007);
                        if (Task.WaitAny(task, Task.Delay(2000)) == 0 && client.Connected)
                            return;
                    }
                }
                catch { }

                AppState.SetDisconnected();
            });
        }

        public void UpdateComStatus(bool connected)
        {
            if (connected)
            {
                _lblComStatus.Text = "● 已连接";
                _lblComStatus.ForeColor = CLR_GREEN;
                _btnConnect.Text = "已连接";
                _btnConnect.BackColor = CLR_GREEN;
            }
            else
            {
                _lblComStatus.Text = "○ 未连接";
                _lblComStatus.ForeColor = CLR_RED;
                _btnConnect.Text = "连接设备";
                _btnConnect.BackColor = CLR_BLUE;
            }

            UpdateStatusBar(connected);

            if (connected)
                AppState.SetConnected(AppState.CurrentIp ?? "192.168.0.51");
            else
                AppState.SetDisconnected();
        }

        private void UpdateStatusBar(bool connected)
        {
            var prefix = connected ? "●已连接" : "○未连接";
            _lblStatus.Text = string.Format("{0}  |  PRF:-- Hz  |  帧率:-- fps  |  增益:-- dB", prefix);
            _lblStatus.ForeColor = connected ? CLR_GREEN : CLR_RED;
        }

        #endregion

        // ==========================================
        // 显示模式切换
        // ==========================================
        #region 显示模式切换

        private void ViewModeBtn_Click(object sender, EventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is string tag))
                return;

            _activeViewMode = tag;

            // 更新按钮高亮
            foreach (Control c in _viewModeBar.Controls)
            {
                if (c is Button b && b.Tag is string t)
                {
                    b.BackColor = (t == tag) ? CLR_BLUE : Color.Transparent;
                    b.ForeColor = (t == tag) ? Color.White : CLR_MUTED;
                }
            }

            // 根据显示模式调整四象限布局
            UpdateQuadrantLayout(tag);
        }

        /// <summary>
        /// 根据显示模式调整4象限面板的RowSpan/ColumnSpan
        /// </summary>
        private void UpdateQuadrantLayout(string mode)
        {
            // 重置所有跨度
            _centerPanel.SetRowSpan(_panelQ1, 1);
            _centerPanel.SetColumnSpan(_panelQ1, 1);
            _centerPanel.SetRowSpan(_panelQ2, 1);
            _centerPanel.SetColumnSpan(_panelQ2, 1);
            _centerPanel.SetRowSpan(_panelQ3, 1);
            _centerPanel.SetColumnSpan(_panelQ3, 1);
            _centerPanel.SetRowSpan(_panelQ4, 1);
            _centerPanel.SetColumnSpan(_panelQ4, 1);

            // 显示/隐藏面板
            _panelQ1.Visible = true;
            _panelQ2.Visible = true;
            _panelQ3.Visible = true;
            _panelQ4.Visible = true;

            switch (mode)
            {
                case "ascan":
                    // A扫: 全屏显示Q1
                    _centerPanel.SetRowSpan(_panelQ1, 2);
                    _centerPanel.SetColumnSpan(_panelQ1, 2);
                    _panelQ2.Visible = false;
                    _panelQ3.Visible = false;
                    _panelQ4.Visible = false;
                    break;

                case "bscan":
                    // B扫: Q1+Q2 水平排列
                    _panelQ2.Visible = false;
                    _panelQ3.Visible = false;
                    _panelQ4.Visible = false;
                    _centerPanel.SetRowSpan(_panelQ1, 2);
                    _centerPanel.SetColumnSpan(_panelQ1, 2);
                    break;

                case "cscan":
                    // C扫: Q4全屏
                    _panelQ1.Visible = false;
                    _panelQ2.Visible = false;
                    _panelQ3.Visible = false;
                    _centerPanel.SetRowSpan(_panelQ4, 2);
                    _centerPanel.SetColumnSpan(_panelQ4, 2);
                    break;

                case "tfm":
                    // TFM: Q2全屏
                    _panelQ1.Visible = false;
                    _panelQ3.Visible = false;
                    _panelQ4.Visible = false;
                    _centerPanel.SetRowSpan(_panelQ2, 2);
                    _centerPanel.SetColumnSpan(_panelQ2, 2);
                    break;
            }

            // 刷新标签
            UpdateQuadrantLabels(mode);
        }

        private void UpdateQuadrantLabels(string mode)
        {
            SetPanelLabel(_panelQ1, "");
            SetPanelLabel(_panelQ2, "");
            SetPanelLabel(_panelQ3, "");
            SetPanelLabel(_panelQ4, "");

            switch (mode)
            {
                case "ascan":
                    SetPanelLabel(_panelQ1, "A扫波形");
                    break;
                case "bscan":
                    SetPanelLabel(_panelQ1, "B扫成像");
                    break;
                case "cscan":
                    SetPanelLabel(_panelQ4, "C扫成像");
                    break;
                case "tfm":
                    SetPanelLabel(_panelQ2, "TFM成像");
                    break;
                default:
                    SetPanelLabel(_panelQ1, "A扫波形");
                    SetPanelLabel(_panelQ2, "S扫 / L扫");
                    SetPanelLabel(_panelQ3, "A扫细节");
                    SetPanelLabel(_panelQ4, "C扫成像");
                    break;
            }
        }

        private void SetPanelLabel(Panel panel, string text)
        {
            foreach (Control c in panel.Controls)
            {
                if (c is Label lbl)
                {
                    lbl.Text = text;
                    break;
                }
            }
        }

        #endregion

        // ==========================================
        // 参数Tab切换 (核心)
        // ==========================================
        #region 参数Tab切换

        private void ParamCategoryBtn_Click(object sender, EventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is string tag))
                return;

            _activeParamTab = tag;

            // 高亮当前选中的分类按钮
            foreach (var b in _catButtons)
            {
                if (b.Tag is string t)
                {
                    b.BackColor = (t == tag) ? CLR_BLUE : BG_INPUT;
                    b.ForeColor = (t == tag) ? Color.White : CLR_MUTED;
                }
            }

            // ★ 切换参数面板: 隐藏所有, 显示当前
            HideAllTabPanels();

            switch (tag)
            {
                case "tx":       _panelTx.Visible = true;       break;
                case "rx":       _panelRx.Visible = true;       break;
                case "probe":    _panelProbe.Visible = true;    break;
                case "wedge":    _panelWedge.Visible = true;    break;
                case "mat":      _panelMaterial.Visible = true; break;
                case "aperture": _panelAperture.Visible = true; break;
                case "gate":     _panelGate.Visible = true;     break;
                case "scan":     _panelScan.Visible = true;     break;
                case "encoder":  _panelEncoder.Visible = true;  break;
                case "cal":      _panelCal.Visible = true;      break;
                case "tfm":      _panelTfm.Visible = true;      break;
            }

            // 同步更新视图模式按钮 (扫查Tab自动切到对应视图)
            SyncViewModeWithTab(tag);
        }

        private void HideAllTabPanels()
        {
            _panelTx.Visible       = false;
            _panelRx.Visible       = false;
            _panelProbe.Visible    = false;
            _panelWedge.Visible    = false;
            _panelMaterial.Visible = false;
            _panelAperture.Visible = false;
            _panelGate.Visible     = false;
            _panelScan.Visible     = false;
            _panelEncoder.Visible  = false;
            _panelCal.Visible      = false;
            _panelTfm.Visible      = false;
        }

        /// <summary>
        /// Tab切换时自动联动视图模式
        /// </summary>
        private void SyncViewModeWithTab(string tabTag)
        {
            string suggestMode = null;
            switch (tabTag)
            {
                case "scan":  suggestMode = "bscan";  break;
                case "tfm":   suggestMode = "tfm";    break;
                case "tx":
                case "rx":
                case "gate":  suggestMode = "ascan";  break;
            }

            if (suggestMode != null && suggestMode != _activeViewMode)
            {
                _activeViewMode = suggestMode;
                UpdateQuadrantLayout(suggestMode);

                // 同步视图按钮高亮
                foreach (Control c in _viewModeBar.Controls)
                {
                    if (c is Button b && b.Tag is string t)
                    {
                        b.BackColor = (t == suggestMode) ? CLR_BLUE : Color.Transparent;
                        b.ForeColor = (t == suggestMode) ? Color.White : CLR_MUTED;
                    }
                }
            }
        }

        #endregion

        // ==========================================
        // 快速应用
        // ==========================================
        #region 快速应用

        private void QuickApplyChanged(object sender, EventArgs e)
        {
            // CheckBox 变更后自动应用到显示
            // TODO: 对接显示引擎
            bool gate     = _chkGate.Checked;
            bool tcg      = _chkTcg.Checked;
            bool envelope = _chkEnvelope.Checked;
            bool peakHold = _chkPeakHold.Checked;
            System.Diagnostics.Debug.WriteLine(
                "QuickApply: gate={0}, tcg={1}, env={2}, peak={3}",
                gate, tcg, envelope, peakHold);
        }

        #endregion

        // ==========================================
        // 右侧功能按钮
        // ==========================================
        #region 右侧功能按钮

        private void RightBtn_Click(object sender, EventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is int idx))
                return;

            switch (idx)
            {
                case 0:  ApplyFocalLaw();        break;
                case 1:  AdjustParam(1);         break;
                case 2:  AdjustParam(-1);        break;
                case 3:  ToggleFreeze(btn);      break;
                case 4:  SaveParams();           break;
                case 5:  SaveData();             break;
                case 6:  LoadParams();           break;
                case 7:  PlaybackData();         break;
                case 8:  CaptureScreen();        break;
                case 9:  StartScan();            break;
                case 10: StopScan();             break;
                case 11: CloseApp();             break;
            }
        }

        private void ApplyFocalLaw()
        {
            MessageBox.Show("应用法则: 将当前参数下发至硬件重新计算聚焦法则", "操作提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AdjustParam(int direction)
        {
            // TODO: 根据当前选中的参数项微调数值
            System.Diagnostics.Debug.WriteLine("AdjustParam: direction={0}, tab={1}", direction, _activeParamTab);
        }

        private void ToggleFreeze(Button btn)
        {
            _isFrozen = !_isFrozen;
            btn.Text = _isFrozen ? "解冻" : "冻结";
            btn.BackColor = _isFrozen ? CLR_RED : BG_INPUT;
            btn.ForeColor = _isFrozen ? Color.White : CLR_AMBER;
        }

        private void SaveParams()
        {
            MessageBox.Show("保存参数: 将当前配置写入参数文件", "操作提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveData()
        {
            MessageBox.Show("保存数据: 将当前采集数据写入检测文件", "操作提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadParams()
        {
            MessageBox.Show("调用参数: 从文件加载配置参数", "操作提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PlaybackData()
        {
            MessageBox.Show("回放数据: 加载并回放历史检测文件", "操作提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CaptureScreen()
        {
            MessageBox.Show("截屏: 保存当前界面截图", "操作提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void StartScan()
        {
            IsScanning = true;
            _scanProgress.Value = 0;
            _lblScanProgress.Text = "0%";
            _scanCtrlProgress.Value = 0;
            MessageBox.Show("扫查启动: 开始扫查架移动 + 超声数据采集", "操作提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void StopScan()
        {
            IsScanning = false;
            MessageBox.Show("扫查停止: 停止扫查架 + 数据采集", "操作提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CloseApp()
        {
            var result = MessageBox.Show("确定要退出程序吗？", "退出确认",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Close();
        }

        #endregion

        // ==========================================
        // 扫查控制面板事件
        // ==========================================
        #region 扫查控制面板

        private void BtnScanStart_Click(object sender, EventArgs e)
        {
            IsScanning = true;
            _scanProgress.Value = 0;
            _lblScanProgress.Text = "0%";
            _scanCtrlProgress.Value = 0;

            // 更新控制面板状态
            foreach (Control c in _scanCtrlPanel.Controls)
            {
                if (c is Label && c.Name == "lblCtrlProg")
                    c.Text = "扫描中...";
            }

            // 联动右侧"扫查启动"按钮
            System.Diagnostics.Debug.WriteLine("ScanStart: dir={0}, speed={1} mm/s",
                _cmbScanDir.SelectedItem, _nudScanSpeed.Value);
        }

        private void BtnScanStop_Click(object sender, EventArgs e)
        {
            IsScanning = false;

            foreach (Control c in _scanCtrlPanel.Controls)
            {
                if (c is Label && c.Name == "lblCtrlProg")
                    c.Text = "已停止";
            }
        }

        #endregion

        // ==========================================
        // 底栏控制
        // ==========================================
        #region 底栏控制

        private void JogMove(double delta)
        {
            try
            {
                double x = double.Parse(_lblPosX.Text.Replace("X: ", "").Replace(" mm", ""));
                double y = double.Parse(_lblPosY.Text.Replace("Y: ", "").Replace(" mm", ""));
                x += delta;
                y += delta * 0.5;
                _lblPosX.Text = string.Format("X: {0:F1} mm", x);
                _lblPosY.Text = string.Format("Y: {0:F1} mm", y);
            }
            catch { }
        }

        private void HomeAxis()
        {
            _lblPosX.Text = "X: 0.0 mm";
            _lblPosY.Text = "Y: 0.0 mm";
            MessageBox.Show("回零: 扫查架返回原点", "操作提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EmergencyStop()
        {
            IsScanning = false;
            MessageBox.Show("急停! 立即停止扫查架运动!", "紧急停止",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #endregion

        // ==========================================
        // 窗体大小调整
        // ==========================================
        #region Resize

        private void Frm_NewInspect_Resize(object sender, EventArgs e)
        {
            int rightEdge = ClientSize.Width;

            // 顶栏右侧通信状态
            _lblComStatus.Location = new Point(rightEdge - 130, 12);
            _lblComStatus.Size = new Size(120, 20);

            // 底栏状态文本 (动态宽度)
            int statusWidth = rightEdge - 800;
            if (statusWidth < 200) statusWidth = 200;
            _lblStatus.Location = new Point(rightEdge - statusWidth - 170, 10);
            _lblStatus.Size = new Size(statusWidth, 18);

            // 底栏时间
            _lblDateTime.Location = new Point(rightEdge - 160, 10);
            _lblDateTime.Size = new Size(150, 18);
        }

        #endregion

        // ==========================================
        // 公开属性（外部可读写）
        // ==========================================
        #region Public API

        public string ActiveViewMode => _activeViewMode;

        public string ActiveParamTab => _activeParamTab;

        public void SetScanProgress(int percent)
        {
            percent = Math.Max(0, Math.Min(100, percent));
            _scanProgress.Value = percent;
            _lblScanProgress.Text = string.Format("{0}%", percent);
            _scanCtrlProgress.Value = percent;

            foreach (Control c in _scanCtrlPanel.Controls)
            {
                if (c is Label && c.Name == "lblCtrlProg")
                    c.Text = string.Format("进度 {0}%", percent);
            }
        }

        public void SetPosition(double x, double y)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetPosition(x, y)));
                return;
            }
            _lblPosX.Text = string.Format("X: {0:F1} mm", x);
            _lblPosY.Text = string.Format("Y: {0:F1} mm", y);
        }

        public void SetStatusInfo(int prf, int fps, double gain, bool gateA)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetStatusInfo(prf, fps, gain, gateA)));
                return;
            }
            var prefix = AppState.IsInstrumentConnected ? "●已连接" : "○未连接";
            var gate = gateA ? "ON" : "OFF";
            _lblStatus.Text = string.Format(
                "{0}  |  PRF:{1} Hz  |  帧率:{2} fps  |  增益:{3:F1} dB  |  门阀A:{4}",
                prefix, prf, fps, gain, gate);
        }

        /// <summary>
        /// 获取当前激活Tab的所有参数快照，用于"应用法则"等功能
        /// </summary>
        public System.Collections.Generic.Dictionary<string, decimal> GetCurrentParams()
        {
            var dict = new System.Collections.Generic.Dictionary<string, decimal>();
            switch (_activeParamTab)
            {
                case "tx":
                    dict["offset"]      = _nudTxOffset.Value;
                    dict["gain"]        = _nudTxGain.Value;
                    dict["range"]       = _nudTxRange.Value;
                    dict["freq"]        = _nudTxFreq.Value;
                    dict["pulseWidth"]  = _nudTxPulseWidth.Value;
                    dict["filter"]      = _nudTxFilter.Value;
                    dict["sampleRate"]  = _nudTxSampleRate.Value;
                    break;
                case "rx":
                    dict["digitalGain"] = _nudRxDigitalGain.Value;
                    dict["analogGain"]  = _nudRxAnalogGain.Value;
                    dict["voltage"]     = _nudRxVoltage.Value;
                    dict["average"]     = _nudRxAverage.Value;
                    dict["damping"]     = _nudRxDamping.Value;
                    break;
                case "scan":
                    dict["startAngle"]  = _nudScanStartAngle.Value;
                    dict["endAngle"]    = _nudScanEndAngle.Value;
                    dict["angleStep"]   = _nudScanAngleStep.Value;
                    dict["depth"]       = _nudScanDepth.Value;
                    break;
                case "encoder":
                    dict["xStep"]       = _nudEncXStep.Value;
                    dict["yStep"]       = _nudEncYStep.Value;
                    dict["interval"]    = _nudEncInterval.Value;
                    break;
                // 可按需扩展其他Tab
            }
            return dict;
        }

        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _connMonitorTimer?.Stop();
            _dateTimeTimer?.Stop();
            base.OnFormClosing(e);
        }
    }
}
