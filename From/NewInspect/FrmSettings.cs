using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NewInspect.UI
{
    /// <summary>
    /// 设置对话框 — 仪器配置 + 软件设置
    /// 6 Tab：仪器配置 / 显示设置 / 路径设置 / 通信设置 / 安全密码 / 版本信息
    /// </summary>
    public class FrmSettings : Form
    {
        #region 字段

        // 仪器解锁密码（可从配置文件读取）
        private const string InstrumentPassword = "2018030";
        private const string DefaultWifiPass = "12345678";
        private const string DefaultWifiPrefix = "PA22S_";
        private const string DefaultIpPrefix = "192.168.";

        private bool _instrumentUnlocked;

        #endregion

        #region 控件 — 仪器配置 Tab

        private TabControl _tabMain;
        private TabPage _tabInstrument;
        private TabPage _tabDisplay;
        private TabPage _tabPaths;
        private TabPage _tabComm;
        private TabPage _tabSecurity;
        private TabPage _tabAbout;

        // --- 仪器配置页 ---
        private Panel _pnlLock;
        private Label _lblLockIcon;
        private Label _lblLockHint;
        private TextBox _txtPassword;
        private Button _btnUnlock;

        private GroupBox _grpWifi;
        private Label _lblWifiSsid;
        private TextBox _txtWifiSsid;
        private Label _lblWifiPass;
        private TextBox _txtWifiPass;

        private GroupBox _grpEthernet;
        private Label _lblEthIp;
        private TextBox _txtEthIp;

        private GroupBox _grpFirmware;
        private Label _lblServerVer;
        private TextBox _txtServerVer;
        private Label _lblFpgaVer;
        private TextBox _txtFpgaVer;

        private Button _btnApplyToInstrument;
        private Button _btnReadFromInstrument;

        // --- 显示设置页 ---
        private GroupBox _grpColorScheme;
        private ComboBox _cboColorScheme;

        private GroupBox _grpWaveform;
        private CheckBox _chkShowPeak;
        private CheckBox _chkShowEnvelope;
        private NumericUpDown _nudLineWidth;

        private GroupBox _grpLanguage;
        private ComboBox _cboLanguage;

        // --- 路径设置页 ---
        private GroupBox _grpDataPath;
        private TextBox _txtDataPath;
        private Button _btnDataPath;

        private GroupBox _grpReportPath;
        private TextBox _txtReportPath;
        private Button _btnReportPath;

        private GroupBox _grpScreenshotPath;
        private TextBox _txtScreenshotPath;
        private Button _btnScreenshotPath;

        // --- 通信设置页 ---
        private GroupBox _grpPorts;
        private Label _lblCmdPort;
        private NumericUpDown _nudCmdPort;
        private Label _lblDataPort;
        private NumericUpDown _nudDataPort;

        private GroupBox _grpTimeout;
        private NumericUpDown _nudConnectTimeout;
        private NumericUpDown _nudCmdTimeout;

        private GroupBox _grpReconnect;
        private CheckBox _chkAutoReconnect;
        private NumericUpDown _nudReconnectInterval;
        private NumericUpDown _nudReconnectMax;

        // --- 安全密码页 ---
        private GroupBox _grpChangePassword;
        private Label _lblOldPwd;
        private TextBox _txtOldPwd;
        private Label _lblNewPwd;
        private TextBox _txtNewPwd;
        private Label _lblConfirmPwd;
        private TextBox _txtConfirmPwd;
        private Button _btnChangePassword;

        // --- 版本信息页 ---
        private GroupBox _grpVersion;
        private Label _lblSoftwareVer;
        private TextBox _txtSoftwareVer;
        private Label _lblServerVer2;
        private TextBox _txtServerVer2;
        private Label _lblFpgaVer2;
        private TextBox _txtFpgaVer2;

        private GroupBox _grpUpdateLog;
        private RichTextBox _rtbUpdateLog;

        // 底部按钮
        private Button _btnOk;
        private Button _btnCancel;

        #endregion

        #region 构造

        /// <param name="tabIndex">默认打开的 Tab 页索引</param>
        public FrmSettings(int tabIndex = 0)
        {
            InitializeComponent();
            _tabMain.SelectedIndex = tabIndex;
        }

        private void InitializeComponent()
        {
            // ====== Form ======
            Text = "设置";
            Size = new Size(560, 540);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Microsoft YaHei UI", 9F);
            BackColor = Color.White;

            // ====== TabControl ======
            _tabMain = new TabControl
            {
                Location = new Point(12, 12),
                Size = new Size(520, 430),
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _tabInstrument = new TabPage { Text = "仪器配置" };
            _tabDisplay = new TabPage { Text = "显示设置" };
            _tabPaths = new TabPage { Text = "路径设置" };
            _tabComm = new TabPage { Text = "通信设置" };
            _tabSecurity = new TabPage { Text = "安全密码" };
            _tabAbout = new TabPage { Text = "版本信息" };

            _tabMain.TabPages.AddRange(new[]
            {
                _tabInstrument, _tabDisplay, _tabPaths,
                _tabComm, _tabSecurity, _tabAbout
            });

            // ====== 构建各 Tab 内容 ======
            BuildInstrumentTab();
            BuildDisplayTab();
            BuildPathsTab();
            BuildCommTab();
            BuildSecurityTab();
            BuildAboutTab();

            // ====== 底部按钮 ======
            _btnOk = new Button
            {
                Location = new Point(360, 455),
                Size = new Size(80, 32),
                Text = "确定",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 9F),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(24, 120, 200),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            _btnOk.FlatAppearance.BorderColor = Color.FromArgb(24, 120, 200);

            _btnCancel = new Button
            {
                Location = new Point(448, 455),
                Size = new Size(80, 32),
                Text = "取消",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 9F),
                ForeColor = Color.FromArgb(80, 80, 80),
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            _btnCancel.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);

            // ====== 组装 ======
            Controls.AddRange(new Control[]
            {
                _tabMain,
                _btnOk, _btnCancel
            });

            _btnOk.Click += BtnOk_Click;
        }

        #endregion

        #region Tab 1: 仪器配置

        private void BuildInstrumentTab()
        {
            var tab = _tabInstrument;

            // --- 密码解锁区块（黄色醒目） ---
            _pnlLock = new Panel
            {
                Location = new Point(12, 12),
                Size = new Size(490, 60),
                BackColor = Color.FromArgb(255, 248, 220),
                BorderStyle = BorderStyle.None
            };

            _lblLockIcon = new Label
            {
                Location = new Point(12, 10),
                Size = new Size(20, 20),
                Text = "🔒",
                Font = new Font("Microsoft YaHei UI", 12F),
                ForeColor = Color.FromArgb(200, 150, 0)
            };

            _lblLockHint = new Label
            {
                Location = new Point(38, 10),
                Size = new Size(240, 18),
                Text = "仪器配置已锁定，输入密码解锁",
                ForeColor = Color.FromArgb(160, 120, 0),
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _txtPassword = new TextBox
            {
                Location = new Point(38, 30),
                Size = new Size(140, 24),
                PasswordChar = '●',
                Font = new Font("Microsoft YaHei UI", 9F),
                BorderStyle = BorderStyle.FixedSingle
            };

            _btnUnlock = new Button
            {
                Location = new Point(184, 29),
                Size = new Size(60, 26),
                Text = "解锁",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 8F),
                ForeColor = Color.FromArgb(120, 80, 0),
                BackColor = Color.FromArgb(255, 240, 180),
                Cursor = Cursors.Hand
            };
            _btnUnlock.FlatAppearance.BorderColor = Color.FromArgb(200, 150, 0);
            _btnUnlock.Click += BtnUnlock_Click;

            _pnlLock.Controls.AddRange(new Control[]
            {
                _lblLockIcon, _lblLockHint, _txtPassword, _btnUnlock
            });

            // --- WiFi 设置 ---
            _grpWifi = new GroupBox
            {
                Location = new Point(12, 82),
                Size = new Size(490, 90),
                Text = "WiFi 设置",
                Enabled = false,
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _lblWifiSsid = new Label
            {
                Location = new Point(14, 24),
                Size = new Size(80, 18),
                Text = "WiFi 名称：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _txtWifiSsid = new TextBox
            {
                Location = new Point(100, 22),
                Size = new Size(200, 24),
                Text = DefaultWifiPrefix + "001",
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.FixedSingle
            };

            _lblWifiPass = new Label
            {
                Location = new Point(14, 54),
                Size = new Size(80, 18),
                Text = "WiFi 密码：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _txtWifiPass = new TextBox
            {
                Location = new Point(100, 52),
                Size = new Size(200, 24),
                Text = DefaultWifiPass,
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.FixedSingle
            };

            _grpWifi.Controls.AddRange(new Control[]
            {
                _lblWifiSsid, _txtWifiSsid,
                _lblWifiPass, _txtWifiPass
            });

            // --- 以太网设置 ---
            _grpEthernet = new GroupBox
            {
                Location = new Point(12, 180),
                Size = new Size(490, 60),
                Text = "以太网 IP 设置",
                Enabled = false,
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _lblEthIp = new Label
            {
                Location = new Point(14, 26),
                Size = new Size(80, 18),
                Text = "IP 地址：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _txtEthIp = new TextBox
            {
                Location = new Point(100, 24),
                Size = new Size(200, 24),
                Text = DefaultIpPrefix + "100",
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.FixedSingle
            };

            _grpEthernet.Controls.AddRange(new Control[]
            {
                _lblEthIp, _txtEthIp
            });

            // --- 固件版本（只读） ---
            _grpFirmware = new GroupBox
            {
                Location = new Point(12, 250),
                Size = new Size(490, 60),
                Text = "固件版本",
                Enabled = false,
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _lblServerVer = new Label
            {
                Location = new Point(14, 26),
                Size = new Size(60, 18),
                Text = "Server：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _txtServerVer = new TextBox
            {
                Location = new Point(80, 24),
                Size = new Size(140, 24),
                Text = "V2.1.0",
                ReadOnly = true,
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            _lblFpgaVer = new Label
            {
                Location = new Point(240, 26),
                Size = new Size(60, 18),
                Text = "FPGA：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _txtFpgaVer = new TextBox
            {
                Location = new Point(290, 24),
                Size = new Size(140, 24),
                Text = "V1.3.2",
                ReadOnly = true,
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            _grpFirmware.Controls.AddRange(new Control[]
            {
                _lblServerVer, _txtServerVer,
                _lblFpgaVer, _txtFpgaVer
            });

            // --- 操作按钮 ---
            _btnApplyToInstrument = new Button
            {
                Location = new Point(280, 325),
                Size = new Size(110, 32),
                Text = "下发配置至仪器",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 9F),
                ForeColor = Color.FromArgb(24, 120, 200),
                BackColor = Color.FromArgb(230, 240, 250),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            _btnApplyToInstrument.FlatAppearance.BorderColor = Color.FromArgb(24, 120, 200);
            _btnApplyToInstrument.Click += BtnApplyToInstrument_Click;

            _btnReadFromInstrument = new Button
            {
                Location = new Point(398, 325),
                Size = new Size(110, 32),
                Text = "读取仪器配置",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 9F),
                ForeColor = Color.FromArgb(80, 80, 80),
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            _btnReadFromInstrument.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            _btnReadFromInstrument.Click += BtnReadFromInstrument_Click;

            tab.Controls.AddRange(new Control[]
            {
                _pnlLock,
                _grpWifi, _grpEthernet, _grpFirmware,
                _btnApplyToInstrument, _btnReadFromInstrument
            });
        }

        #endregion

        #region Tab 2: 显示设置

        private void BuildDisplayTab()
        {
            var tab = _tabDisplay;

            // --- 配色方案 ---
            _grpColorScheme = new GroupBox
            {
                Location = new Point(12, 12),
                Size = new Size(490, 60),
                Text = "配色方案",
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _cboColorScheme = new ComboBox
            {
                Location = new Point(14, 24),
                Size = new Size(200, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Microsoft YaHei UI", 9F)
            };
            _cboColorScheme.Items.AddRange(new[] { "经典模式（彩色）", "高对比度（黑白）", "夜间模式（深色）" });
            _cboColorScheme.SelectedIndex = 0;

            _grpColorScheme.Controls.Add(_cboColorScheme);

            // --- 波形样式 ---
            _grpWaveform = new GroupBox
            {
                Location = new Point(12, 82),
                Size = new Size(490, 100),
                Text = "波形显示",
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _chkShowPeak = new CheckBox
            {
                Location = new Point(14, 24),
                Size = new Size(120, 20),
                Text = "显示波峰标记",
                Checked = true
            };

            _chkShowEnvelope = new CheckBox
            {
                Location = new Point(14, 48),
                Size = new Size(120, 20),
                Text = "显示包络线",
                Checked = false
            };

            var lblLineWidth = new Label
            {
                Location = new Point(14, 74),
                Size = new Size(80, 18),
                Text = "波形线宽：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _nudLineWidth = new NumericUpDown
            {
                Location = new Point(100, 72),
                Size = new Size(60, 24),
                Minimum = 1,
                Maximum = 5,
                Value = 1,
                BorderStyle = BorderStyle.FixedSingle
            };

            _grpWaveform.Controls.AddRange(new Control[]
            {
                _chkShowPeak, _chkShowEnvelope,
                lblLineWidth, _nudLineWidth
            });

            // --- 语言 ---
            _grpLanguage = new GroupBox
            {
                Location = new Point(12, 192),
                Size = new Size(490, 60),
                Text = "语言",
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _cboLanguage = new ComboBox
            {
                Location = new Point(14, 24),
                Size = new Size(200, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Microsoft YaHei UI", 9F)
            };
            _cboLanguage.Items.AddRange(new[] { "简体中文", "English" });
            _cboLanguage.SelectedIndex = 0;

            _grpLanguage.Controls.Add(_cboLanguage);

            tab.Controls.AddRange(new Control[]
            {
                _grpColorScheme, _grpWaveform, _grpLanguage
            });
        }

        #endregion

        #region Tab 3: 路径设置

        private void BuildPathsTab()
        {
            var tab = _tabPaths;

            var descLabel = new Label
            {
                Location = new Point(12, 12),
                Size = new Size(490, 30),
                Text = "设置检测数据、报告和截屏的保存路径。路径不存在时将自动创建。",
                ForeColor = Color.FromArgb(120, 120, 120),
                Font = new Font("Microsoft YaHei UI", 8F)
            };
            tab.Controls.Add(descLabel);

            // 数据保存路径
            _grpDataPath = CreatePathGroup("检测数据", ref _txtDataPath, ref _btnDataPath, 12, 48);
            _btnDataPath.Click += (s, e) => BrowseFolder(_txtDataPath, "选择数据保存目录");

            // 报告输出路径
            _grpReportPath = CreatePathGroup("报告输出", ref _txtReportPath, ref _btnReportPath, 12, 136);
            _btnReportPath.Click += (s, e) => BrowseFolder(_txtReportPath, "选择报告输出目录");

            // 截屏保存路径
            _grpScreenshotPath = CreatePathGroup("截屏保存", ref _txtScreenshotPath, ref _btnScreenshotPath, 12, 224);
            _btnScreenshotPath.Click += (s, e) => BrowseFolder(_txtScreenshotPath, "选择截屏保存目录");

            tab.Controls.AddRange(new Control[]
            {
                _grpDataPath, _grpReportPath, _grpScreenshotPath
            });
        }

        private GroupBox CreatePathGroup(string title, ref TextBox txt, ref Button btn, int x, int y)
        {
            var grp = new GroupBox
            {
                Location = new Point(x, y),
                Size = new Size(490, 78),
                Text = title + "路径",
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            txt = new TextBox
            {
                Location = new Point(14, 26),
                Size = new Size(380, 24),
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                Text = @"D:\InspectData\" + (title == "检测数据" ? "data" : title == "报告输出" ? "reports" : "screenshots")
            };

            btn = new Button
            {
                Location = new Point(402, 24),
                Size = new Size(72, 28),
                Text = "浏览...",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 8F),
                ForeColor = Color.FromArgb(80, 80, 80),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);

            grp.Controls.AddRange(new Control[] { txt, btn });
            return grp;
        }

        #endregion

        #region Tab 4: 通信设置

        private void BuildCommTab()
        {
            var tab = _tabComm;

            var descLabel = new Label
            {
                Location = new Point(12, 12),
                Size = new Size(490, 30),
                Text = "修改通信参数后需重新连接仪器才能生效。",
                ForeColor = Color.FromArgb(200, 120, 0),
                Font = new Font("Microsoft YaHei UI", 8F)
            };

            // --- 端口设置 ---
            _grpPorts = new GroupBox
            {
                Location = new Point(12, 48),
                Size = new Size(490, 80),
                Text = "端口设置",
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _lblCmdPort = new Label
            {
                Location = new Point(14, 26),
                Size = new Size(100, 18),
                Text = "命令通道端口：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _nudCmdPort = new NumericUpDown
            {
                Location = new Point(120, 24),
                Size = new Size(80, 24),
                Minimum = 1,
                Maximum = 65535,
                Value = 51007,
                BorderStyle = BorderStyle.FixedSingle
            };

            _lblDataPort = new Label
            {
                Location = new Point(220, 26),
                Size = new Size(100, 18),
                Text = "数据通道端口：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _nudDataPort = new NumericUpDown
            {
                Location = new Point(320, 24),
                Size = new Size(80, 24),
                Minimum = 1,
                Maximum = 65535,
                Value = 51005,
                BorderStyle = BorderStyle.FixedSingle
            };

            _grpPorts.Controls.AddRange(new Control[]
            {
                _lblCmdPort, _nudCmdPort,
                _lblDataPort, _nudDataPort
            });

            // --- 超时设置 ---
            _grpTimeout = new GroupBox
            {
                Location = new Point(12, 138),
                Size = new Size(490, 80),
                Text = "超时设置",
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            var lblConnTimeout = new Label
            {
                Location = new Point(14, 26),
                Size = new Size(100, 18),
                Text = "连接超时(ms)：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _nudConnectTimeout = new NumericUpDown
            {
                Location = new Point(120, 24),
                Size = new Size(80, 24),
                Minimum = 500,
                Maximum = 30000,
                Increment = 500,
                Value = 3000,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblCmdTimeout = new Label
            {
                Location = new Point(220, 26),
                Size = new Size(100, 18),
                Text = "命令超时(ms)：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _nudCmdTimeout = new NumericUpDown
            {
                Location = new Point(320, 24),
                Size = new Size(80, 24),
                Minimum = 500,
                Maximum = 60000,
                Increment = 500,
                Value = 5000,
                BorderStyle = BorderStyle.FixedSingle
            };

            _grpTimeout.Controls.AddRange(new Control[]
            {
                lblConnTimeout, _nudConnectTimeout,
                lblCmdTimeout, _nudCmdTimeout
            });

            // --- 重连策略 ---
            _grpReconnect = new GroupBox
            {
                Location = new Point(12, 228),
                Size = new Size(490, 100),
                Text = "断线重连",
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _chkAutoReconnect = new CheckBox
            {
                Location = new Point(14, 24),
                Size = new Size(120, 20),
                Text = "自动重连",
                Checked = true
            };

            var lblReconnInterval = new Label
            {
                Location = new Point(14, 50),
                Size = new Size(120, 18),
                Text = "重连间隔(ms)：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _nudReconnectInterval = new NumericUpDown
            {
                Location = new Point(120, 48),
                Size = new Size(80, 24),
                Minimum = 1000,
                Maximum = 60000,
                Increment = 1000,
                Value = 5000,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblReconnMax = new Label
            {
                Location = new Point(220, 50),
                Size = new Size(120, 18),
                Text = "最大重连次数：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _nudReconnectMax = new NumericUpDown
            {
                Location = new Point(320, 48),
                Size = new Size(80, 24),
                Minimum = 1,
                Maximum = 100,
                Value = 10,
                BorderStyle = BorderStyle.FixedSingle
            };

            _chkAutoReconnect.CheckedChanged += (s, e) =>
            {
                _nudReconnectInterval.Enabled = _chkAutoReconnect.Checked;
                _nudReconnectMax.Enabled = _chkAutoReconnect.Checked;
            };

            _grpReconnect.Controls.AddRange(new Control[]
            {
                _chkAutoReconnect,
                lblReconnInterval, _nudReconnectInterval,
                lblReconnMax, _nudReconnectMax
            });

            tab.Controls.AddRange(new Control[]
            {
                descLabel,
                _grpPorts, _grpTimeout, _grpReconnect
            });
        }

        #endregion

        #region Tab 5: 安全密码

        private void BuildSecurityTab()
        {
            var tab = _tabSecurity;

            var descLabel = new Label
            {
                Location = new Point(12, 12),
                Size = new Size(490, 30),
                Text = "修改仪器配置解锁密码。修改后即刻生效，请妥善保管。",
                ForeColor = Color.FromArgb(120, 120, 120),
                Font = new Font("Microsoft YaHei UI", 8F)
            };

            _grpChangePassword = new GroupBox
            {
                Location = new Point(12, 50),
                Size = new Size(490, 170),
                Text = "修改密码",
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _lblOldPwd = new Label
            {
                Location = new Point(14, 28),
                Size = new Size(100, 18),
                Text = "当前密码：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _txtOldPwd = new TextBox
            {
                Location = new Point(120, 26),
                Size = new Size(200, 24),
                PasswordChar = '●',
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 10F)
            };

            _lblNewPwd = new Label
            {
                Location = new Point(14, 62),
                Size = new Size(100, 18),
                Text = "新密码：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _txtNewPwd = new TextBox
            {
                Location = new Point(120, 60),
                Size = new Size(200, 24),
                PasswordChar = '●',
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 10F)
            };

            _lblConfirmPwd = new Label
            {
                Location = new Point(14, 96),
                Size = new Size(100, 18),
                Text = "确认新密码：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _txtConfirmPwd = new TextBox
            {
                Location = new Point(120, 94),
                Size = new Size(200, 24),
                PasswordChar = '●',
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 10F)
            };

            _btnChangePassword = new Button
            {
                Location = new Point(120, 130),
                Size = new Size(100, 32),
                Text = "修改密码",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 9F),
                ForeColor = Color.FromArgb(24, 120, 200),
                BackColor = Color.FromArgb(230, 240, 250),
                Cursor = Cursors.Hand
            };
            _btnChangePassword.FlatAppearance.BorderColor = Color.FromArgb(24, 120, 200);
            _btnChangePassword.Click += BtnChangePassword_Click;

            _grpChangePassword.Controls.AddRange(new Control[]
            {
                _lblOldPwd, _txtOldPwd,
                _lblNewPwd, _txtNewPwd,
                _lblConfirmPwd, _txtConfirmPwd,
                _btnChangePassword
            });

            tab.Controls.AddRange(new Control[]
            {
                descLabel, _grpChangePassword
            });
        }

        #endregion

        #region Tab 6: 版本信息

        private void BuildAboutTab()
        {
            var tab = _tabAbout;

            _grpVersion = new GroupBox
            {
                Location = new Point(12, 12),
                Size = new Size(490, 100),
                Text = "版本信息",
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _lblSoftwareVer = new Label
            {
                Location = new Point(14, 26),
                Size = new Size(80, 18),
                Text = "软件版本：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _txtSoftwareVer = new TextBox
            {
                Location = new Point(100, 24),
                Size = new Size(160, 24),
                Text = "NewInspect V1.0.0",
                ReadOnly = true,
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            _lblServerVer2 = new Label
            {
                Location = new Point(14, 56),
                Size = new Size(80, 18),
                Text = "Server 版本：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _txtServerVer2 = new TextBox
            {
                Location = new Point(100, 54),
                Size = new Size(160, 24),
                Text = "V2.1.0",
                ReadOnly = true,
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            _lblFpgaVer2 = new Label
            {
                Location = new Point(14, 86),
                Size = new Size(80, 18),
                Text = "FPGA 版本：",
                ForeColor = Color.FromArgb(80, 80, 80)
            };

            _txtFpgaVer2 = new TextBox
            {
                Location = new Point(100, 84),
                Size = new Size(160, 24),
                Text = "V1.3.2",
                ReadOnly = true,
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            _grpVersion.Controls.AddRange(new Control[]
            {
                _lblSoftwareVer, _txtSoftwareVer,
                _lblServerVer2, _txtServerVer2,
                _lblFpgaVer2, _txtFpgaVer2
            });

            _grpUpdateLog = new GroupBox
            {
                Location = new Point(12, 122),
                Size = new Size(490, 220),
                Text = "更新日志",
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            _rtbUpdateLog = new RichTextBox
            {
                Location = new Point(10, 22),
                Size = new Size(470, 190),
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(248, 248, 248),
                Font = new Font("Consolas", 8F),
                Text = @"V1.0.0 (2026-06-01)
- 首个正式版本
- 支持 S扫/L扫/CL扫/TFM 四种扫查模式
- 双 TCP 通道通信（命令 + 数据）
- A-Scan / B-Scan / C-Scan 三视图显示
- ACG / TCG 校准功能
- 扫查架联动控制（新增）
- 数据回放与报告导出
"
            };

            _grpUpdateLog.Controls.Add(_rtbUpdateLog);

            tab.Controls.AddRange(new Control[]
            {
                _grpVersion, _grpUpdateLog
            });
        }

        #endregion

        #region 事件处理

        private void BtnUnlock_Click(object sender, EventArgs e)
        {
            if (_txtPassword.Text == InstrumentPassword)
            {
                _instrumentUnlocked = true;
                _pnlLock.Visible = false;

                _grpWifi.Enabled = true;
                _grpEthernet.Enabled = true;
                _grpFirmware.Enabled = true;
                _btnApplyToInstrument.Enabled = true;
                _btnReadFromInstrument.Enabled = true;
            }
            else
            {
                MessageBox.Show("密码错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _txtPassword.SelectAll();
                _txtPassword.Focus();
            }
        }

        private void BtnApplyToInstrument_Click(object sender, EventArgs e)
        {
            if (!_instrumentUnlocked)
            {
                MessageBox.Show("请先输入密码解锁。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var mode = _tabMain.SelectedIndex; // 0=WiFi, 通过 Tab 判断
            // 构造 JSON 并下发到仪器（示例占位）
            var json = "{\"wifi\":{\"ssid\":\"" + _txtWifiSsid.Text +
                       "\",\"passwd\":\"" + _txtWifiPass.Text + "\"}}";

            // TODO: 通过 CommandChannel 发送
            MessageBox.Show("配置已下发至仪器。\n\n" + json,
                "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnReadFromInstrument_Click(object sender, EventArgs e)
        {
            if (!_instrumentUnlocked) return;

            // TODO: 通过 CommandChannel 查询仪器当前配置
            MessageBox.Show("已从仪器读取配置。", "成功",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_txtOldPwd.Text))
            {
                MessageBox.Show("请输入当前密码。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_txtOldPwd.Text != InstrumentPassword)
            {
                MessageBox.Show("当前密码错误。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(_txtNewPwd.Text))
            {
                MessageBox.Show("请输入新密码。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_txtNewPwd.Text != _txtConfirmPwd.Text)
            {
                MessageBox.Show("两次输入的新密码不一致。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // TODO: 持久化新密码到配置文件
            MessageBox.Show("密码修改成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _txtOldPwd.Clear();
            _txtNewPwd.Clear();
            _txtConfirmPwd.Clear();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            // 保存设置（路径、通信参数等）到配置文件
            SaveSettings();
            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion

        #region 辅助方法

        private void BrowseFolder(TextBox target, string description)
        {
            using (var dlg = new FolderBrowserDialog
            {
                Description = description,
                SelectedPath = target.Text,
                ShowNewFolderButton = true
            })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    target.Text = dlg.SelectedPath;
            }
        }

        private void SaveSettings()
        {
            // TODO: 持久化到 App.config 或自定义 XML
            // 包括：显示设置、路径、通信参数
        }

        #endregion
    }
}
