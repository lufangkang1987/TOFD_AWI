using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewInspect.UI
{
    /// <summary>
    /// 连接仪器对话框 — CTSPA22S 相控阵超声主机
    /// 双 TCP 通道：命令通道(51007/JSON) + 数据通道(51005/1024B二进制)
    /// </summary>
    public class FrmConnect : Form
    {
        #region 字段

        // 历史 IP 记录文件路径
        private static readonly string HistoryFilePath =
            Path.Combine(Application.StartupPath, "ip_history.txt");

        // 默认参数
        private const string DefaultIp = "192.168.0.51";
        private const int CommandPort = 51007;
        private const int DataPort = 51005;
        private const int ConnectTimeoutMs = 3000;

        // 连接状态
        private bool _isConnected;
        private bool _isConnecting;
        private bool _closingBySuccess;   // 标记是否因连接成功而自动关闭
        private CancellationTokenSource _cts;

        // 选中的模式
        public int ConnectState { get; private set; } = 1; // 1=普通 2=CL 3=TFM 4=C扫

        #endregion

        #region 控件声明

        // 顶部状态
        private Panel _pnlStatus;
        private Label _lblStatusDot;
        private Label _lblStatusText;

        // IP 输入区
        private Label _lblIp;
        private TextBox _txtIp;
        private ComboBox _cboHistory;

        // 双通道状态卡片
        private Panel _pnlCmdChannel;
        private Label _lblCmdTitle;
        private Label _lblCmdDot;
        private Label _lblCmdPort;
        private Label _lblCmdDesc;

        private Panel _pnlDataChannel;
        private Label _lblDataTitle;
        private Label _lblDataDot;
        private Label _lblDataPort;
        private Label _lblDataDesc;

        // 模式选择
        private Label _lblMode;
        private RadioButton _rdoNormal;
        private RadioButton _rdoCl;
        private RadioButton _rdoTfm;
        private RadioButton _rdoCscan;

        // 连接进度
        private Panel _pnlProgress;
        private Label _lblProgressTitle;
        private Label _lblProgressIp;
        private ProgressBar _progConnect;
        private Label _lblStepCmd;
        private Label _lblStepData;

        // 底部按钮
        private Button _btnPowerOff;
        private Button _btnWifi;
        private Button _btnConnect;

        #endregion

        #region 构造

        public FrmConnect()
        {
            InitializeComponent();
            LoadIpHistory();
            // 订阅全局连接状态变化（网络断开时自动刷新UI）
            AppState.ConnectionStateChanged += OnAppStateChanged;
            // 根据全局状态初始化UI：如果已经连上，直接显示已连接状态
            if (AppState.IsInstrumentConnected)
            {
                _isConnected = true;
                _txtIp.Text = AppState.CurrentIp;
                SetUiConnected();
                SetChannelDot(_pnlCmdChannel, CommChannelType.Command, true);
                SetChannelDot(_pnlDataChannel, CommChannelType.Data, true);
            }
        }

        private void OnAppStateChanged(object sender, bool connected)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnAppStateChanged(sender, connected)));
                return;
            }

            if (connected)
            {
                _isConnected = true;
                _txtIp.Text = AppState.CurrentIp;
                SetUiConnected();
                SetChannelDot(_pnlCmdChannel, CommChannelType.Command, true);
                SetChannelDot(_pnlDataChannel, CommChannelType.Data, true);
            }
            else
            {
                _isConnected = false;
                _isConnecting = false;
                SetUiDisconnected();
                _btnConnect.Enabled = true;
            }
        }

        private void InitializeComponent()
        {
            // ====== Form ======
            Text = "连接仪器";
            Size = new Size(500, 420);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Microsoft YaHei UI", 9F);
            BackColor = Color.White;

            // ====== 顶部状态指示 ======
            _pnlStatus = new Panel
            {
                Location = new Point(12, 14),
                Size = new Size(460, 28),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            _lblStatusDot = new Label
            {
                Location = new Point(0, 5),
                Size = new Size(10, 10),
                Text = "●",
                ForeColor = Color.Gray,
                Font = new Font("Microsoft YaHei UI", 7F)
            };

            _lblStatusText = new Label
            {
                Location = new Point(16, 2),
                Size = new Size(100, 20),
                Text = "未连接",
                ForeColor = Color.Gray,
                Font = new Font("Microsoft YaHei UI", 9F)
            };

            var lblTitle = new Label
            {
                Location = new Point(0, 0),
                Size = new Size(400, 22),
                Text = "CTSPA22S 相控阵超声检测仪",
                Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 44, 44)
            };

            _pnlStatus.Controls.AddRange(new Control[]
            {
                lblTitle,
                _lblStatusDot,
                _lblStatusText
            });
            // dot和text放在右侧
            _lblStatusDot.Location = new Point(380, 4);
            _lblStatusText.Location = new Point(396, 0);

            // ====== IP 地址输入 ======
            _lblIp = new Label
            {
                Location = new Point(12, 52),
                Size = new Size(460, 18),
                Text = "仪器 IP 地址",
                ForeColor = Color.FromArgb(100, 100, 100),
                Font = new Font("Microsoft YaHei UI", 8F)
            };

            _txtIp = new TextBox
            {
                Location = new Point(12, 72),
                Size = new Size(340, 26),
                Text = DefaultIp,
                Font = new Font("Consolas", 11F),
                BorderStyle = BorderStyle.FixedSingle
            };

            _cboHistory = new ComboBox
            {
                Location = new Point(360, 72),
                Size = new Size(112, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Microsoft YaHei UI", 8F),
                Text = "历史"
            };
            _cboHistory.SelectedIndexChanged += (s, e) =>
            {
                if (_cboHistory.SelectedItem != null)
                    _txtIp.Text = _cboHistory.SelectedItem.ToString();
            };

            // ====== 双通道状态卡片 ======
            _pnlCmdChannel = CreateChannelCard(12, 110, "命令通道", "TCP :51007", "JSON 指令收发");
            _pnlDataChannel = CreateChannelCard(248, 110, "数据通道", "TCP :51005", "1024B 波形数据流");

            // ====== 模式选择 ======
            _lblMode = new Label
            {
                Location = new Point(12, 190),
                Size = new Size(460, 18),
                Text = "启动模式",
                ForeColor = Color.FromArgb(100, 100, 100),
                Font = new Font("Microsoft YaHei UI", 8F)
            };

            int radioY = 210;
            _rdoNormal = CreateModeRadio(12, radioY, "普通模式", checked_: true);
            _rdoCl = CreateModeRadio(130, radioY, "CL 扫查");
            _rdoTfm = CreateModeRadio(248, radioY, "TFM 成像");
            _rdoCscan = CreateModeRadio(366, radioY, "C 扫描");

            // 目前仅普通模式可用：保持按钮外观一致，但禁止切换并给出提示
            _rdoCl.AutoCheck = false;
            _rdoTfm.AutoCheck = false;
            _rdoCscan.AutoCheck = false;
            // 把鼠标样式改为默认，避免看起来可点击
            _rdoCl.Cursor = Cursors.Default;
            _rdoTfm.Cursor = Cursors.Default;
            _rdoCscan.Cursor = Cursors.Default;
            // 点击时提示用户当前仅普通模式可用
            _rdoCl.Click += (s, e) => MessageBox.Show("当前仅支持普通模式。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _rdoTfm.Click += (s, e) => MessageBox.Show("当前仅支持普通模式。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _rdoCscan.Click += (s, e) => MessageBox.Show("当前仅支持普通模式。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _rdoNormal.CheckedChanged += (s, e) => { if (_rdoNormal.Checked) ConnectState = 1; };
            _rdoCl.CheckedChanged += (s, e) => { if (_rdoCl.Checked) ConnectState = 2; };
            _rdoTfm.CheckedChanged += (s, e) => { if (_rdoTfm.Checked) ConnectState = 3; };
            _rdoCscan.CheckedChanged += (s, e) => { if (_rdoCscan.Checked) ConnectState = 4; };

            // ====== 连接进度 ======
            _pnlProgress = new Panel
            {
                Location = new Point(12, 248),
                Size = new Size(460, 60),
                Visible = false,
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.None
            };

            _lblProgressTitle = new Label
            {
                Location = new Point(12, 8),
                Size = new Size(60, 16),
                Text = "正在连接",
                Font = new Font("Microsoft YaHei UI", 9F),
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            _lblProgressIp = new Label
            {
                Location = new Point(76, 8),
                Size = new Size(200, 16),
                Text = "",
                Font = new Font("Consolas", 9F),
                ForeColor = Color.FromArgb(44, 44, 44)
            };

            _progConnect = new ProgressBar
            {
                Location = new Point(12, 30),
                Size = new Size(436, 6),
                Style = ProgressBarStyle.Marquee,
                Visible = false
            };

            _lblStepCmd = new Label
            {
                Location = new Point(12, 40),
                Size = new Size(220, 16),
                Text = "",
                Font = new Font("Microsoft YaHei UI", 8F),
                ForeColor = Color.Gray
            };

            _lblStepData = new Label
            {
                Location = new Point(240, 40),
                Size = new Size(230, 16),
                Text = "",
                TextAlign = ContentAlignment.TopRight,
                Font = new Font("Microsoft YaHei UI", 8F),
                ForeColor = Color.Gray
            };

            _pnlProgress.Controls.AddRange(new Control[]
            {
                _lblProgressTitle, _lblProgressIp, _progConnect,
                _lblStepCmd, _lblStepData
            });

            // ====== 分隔线 ======
            var separator = new Label
            {
                Location = new Point(12, 320),
                Size = new Size(460, 1),
                BorderStyle = BorderStyle.Fixed3D,
                BackColor = Color.FromArgb(220, 220, 220)
            };

            // ====== 底部按钮 ======
            int btnY = 335;
            _btnPowerOff = new Button
            {
                Location = new Point(12, btnY),
                Size = new Size(90, 32),
                Text = "关闭仪器",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 9F),
                ForeColor = Color.FromArgb(210, 60, 50),
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            _btnPowerOff.FlatAppearance.BorderColor = Color.FromArgb(210, 60, 50);
            _btnPowerOff.Click += BtnPowerOff_Click;

            _btnWifi = new Button
            {
                Location = new Point(280, btnY),
                Size = new Size(90, 32),
                Text = "WiFi 设置",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 9F),
                ForeColor = Color.FromArgb(80, 80, 80),
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            _btnWifi.FlatAppearance.BorderColor = Color.FromArgb(180, 180, 180);
            _btnWifi.Click += BtnWifi_Click;

            _btnConnect = new Button
            {
                Location = new Point(378, btnY),
                Size = new Size(94, 36),
                Text = "连接仪器",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(24, 120, 200),
                BackColor = Color.FromArgb(230, 240, 250),
                Cursor = Cursors.Hand
            };
            _btnConnect.FlatAppearance.BorderColor = Color.FromArgb(24, 120, 200);
            _btnConnect.Click += BtnConnect_Click;

            // ====== 组装 ======
            Controls.AddRange(new Control[]
            {
                _pnlStatus,
                _lblIp, _txtIp, _cboHistory,
                _pnlCmdChannel, _pnlDataChannel,
                _lblMode,
                _rdoNormal, _rdoCl, _rdoTfm, _rdoCscan,
                _pnlProgress,
                separator,
                _btnPowerOff, _btnWifi, _btnConnect
            });

            FormClosing += FrmConnect_FormClosing;
        }

        #endregion

        #region 控件创建辅助

        private Panel CreateChannelCard(int x, int y, string title, string port, string desc)
        {
            var pnl = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(228, 68),
                BackColor = Color.FromArgb(248, 248, 248),
                BorderStyle = BorderStyle.None
            };

            var dot = new Label
            {
                Location = new Point(10, 10),
                Size = new Size(8, 8),
                Text = "●",
                ForeColor = Color.Gray,
                Font = new Font("Microsoft YaHei UI", 6F)
            };

            var lblTitle = new Label
            {
                Location = new Point(24, 6),
                Size = new Size(80, 16),
                Text = title,
                Font = new Font("Microsoft YaHei UI", 8F),
                ForeColor = Color.FromArgb(120, 120, 120)
            };

            var lblPort = new Label
            {
                Location = new Point(10, 26),
                Size = new Size(200, 20),
                Text = port,
                Font = new Font("Consolas", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 44, 44)
            };

            var lblDesc = new Label
            {
                Location = new Point(10, 46),
                Size = new Size(200, 16),
                Text = desc,
                Font = new Font("Microsoft YaHei UI", 7F),
                ForeColor = Color.FromArgb(160, 160, 160)
            };

            pnl.Controls.AddRange(new Control[] { dot, lblTitle, lblPort, lblDesc });
            return pnl;
        }

        private RadioButton CreateModeRadio(int x, int y, string text, bool checked_ = false)
        {
            return new RadioButton
            {
                Location = new Point(x, y),
                Size = new Size(110, 28),
                Text = text,
                Appearance = Appearance.Button,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft YaHei UI", 9F),
                TextAlign = ContentAlignment.MiddleCenter,
                Checked = checked_,
                Cursor = Cursors.Hand
            };
        }

        #endregion

        #region 事件处理

        private async void BtnConnect_Click(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                // 已连接状态：按钮显示"进入系统"，点击直接关闭对话框返回 OK
                _closingBySuccess = true;
                DialogResult = DialogResult.OK;
                Close();
                return;
            }
            if (_isConnecting) return;

            await ConnectAsync();
        }

        private void BtnWifi_Click(object sender, EventArgs e)
        {
            // 打开 WiFi 设置对话框
            using (var dlg = new FrmSettings(tabIndex: 0))
                dlg.ShowDialog(this);
        }

        private void BtnPowerOff_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "确定要关闭仪器电源吗？",
                "确认关机",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                DialogResult = DialogResult.Abort;
                Close();
            }
        }

        private void FrmConnect_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppState.ConnectionStateChanged -= OnAppStateChanged;
            // 只有非成功关闭（用户点 X 或取消）时才断开连接
            if (_isConnected && !_closingBySuccess)
                Disconnect();
            _cts?.Cancel();
        }

        #endregion

        #region 连接逻辑

        private async Task ConnectAsync()
        {
            _isConnecting = true;
            var ip = _txtIp.Text.Trim();

            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("请输入仪器 IP 地址。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _isConnecting = false;
                return;
            }

            // UI: 进入连接状态
            SetUiConnecting(ip);

            _cts = new CancellationTokenSource();
            bool cmdOk = false;
            bool dataOk = false;

            try
            {
                // Step 1: 连接命令通道
                UpdateStep(_lblStepCmd, "命令通道 51007...", Color.FromArgb(24, 120, 200));
                cmdOk = await Task.Run(() => ConnectChannel(ip, CommandPort, _cts.Token));

                if (!cmdOk)
                {
                    UpdateStep(_lblStepCmd, "命令通道 连接失败", Color.FromArgb(210, 60, 50));
                    SetChannelDot(_pnlCmdChannel, CommChannelType.Command, false);
                    _isConnecting = false;
                    SetUiDisconnected();
                    MessageBox.Show("命令通道连接失败，请检查 IP 地址和仪器状态。",
                        "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                UpdateStep(_lblStepCmd, "命令通道 已连接", Color.FromArgb(40, 160, 40));
                SetChannelDot(_pnlCmdChannel, CommChannelType.Command, true);
                AppState.SetChannelState(CommChannelType.Command, true);

                // Step 2: 连接数据通道
                UpdateStep(_lblStepData, "数据通道 51005...", Color.FromArgb(24, 120, 200));
                dataOk = await Task.Run(() => ConnectChannel(ip, DataPort, _cts.Token));

                if (!dataOk)
                {
                    UpdateStep(_lblStepData, "数据通道 连接失败", Color.FromArgb(210, 60, 50));
                    SetChannelDot(_pnlDataChannel, CommChannelType.Data, false);
                    _isConnecting = false;
                    SetUiDisconnected();
                    MessageBox.Show("数据通道连接失败，命令通道已连接。请检查仪器状态。",
                        "部分连接", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                UpdateStep(_lblStepData, "数据通道 已连接", Color.FromArgb(40, 160, 40));
                SetChannelDot(_pnlDataChannel, CommChannelType.Data, true);
                AppState.SetChannelState(CommChannelType.Data, true);

                // 全部连接成功
                _isConnected = true;
                _isConnecting = false;
                SetUiConnected();
                SaveIpToHistory(ip);

                // ★ 写入全局状态，主界面和再次打开此对话框都能读到
                AppState.SetConnected(ip);

                // 短暂显示成功状态后关闭对话框
                await Task.Delay(800);
                _closingBySuccess = true;   // 标记为成功关闭，避免 FormClosing 中断开
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (OperationCanceledException)
            {
                SetUiDisconnected();
                _isConnecting = false;
                AppState.SetDisconnected();
            }
        }

        private bool ConnectChannel(string ip, int port, CancellationToken ct)
        {
            try
            {
                using (var client = new System.Net.Sockets.TcpClient())
                {
                    client.SendTimeout = ConnectTimeoutMs;
                    client.ReceiveTimeout = ConnectTimeoutMs;
                    var task = client.ConnectAsync(ip, port);

                    if (Task.WaitAny(task, Task.Delay(ConnectTimeoutMs, ct)) == 0)
                    {
                        task.Wait(ct);
                        return client.Connected;
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private void Disconnect()
        {
            _cts?.Cancel();
            _isConnected = false;
            _isConnecting = false;
            SetUiDisconnected();
            // ★ 写入全局状态，主界面顶栏同步更新
            AppState.SetDisconnected();
        }

        #endregion

        #region UI 状态更新

        private void SetUiConnecting(string ip)
        {
            _btnConnect.Text = "连接中...";
            _btnConnect.Enabled = false;
            _txtIp.Enabled = false;
            _cboHistory.Enabled = false;

            _pnlProgress.Visible = true;
            _lblProgressIp.Text = ip;
            _lblStepCmd.Text = "";
            _lblStepData.Text = "";
            _progConnect.Visible = true;

            SetStatusIndicator(Color.FromArgb(240, 180, 0), "连接中...");
        }

        private void SetUiConnected()
        {
            _btnConnect.Text = "进入系统";
            _btnConnect.Enabled = true;
            _btnConnect.ForeColor = Color.FromArgb(40, 160, 40);
            _btnConnect.BackColor = Color.FromArgb(220, 245, 220);
            _btnConnect.FlatAppearance.BorderColor = Color.FromArgb(40, 160, 40);

            _progConnect.Visible = false;
            SetStatusIndicator(Color.FromArgb(40, 160, 40), "已连接");
        }

        private void SetUiDisconnected()
        {
            _btnConnect.Text = "连接仪器";
            _btnConnect.Enabled = true;
            _btnConnect.ForeColor = Color.FromArgb(24, 120, 200);
            _btnConnect.BackColor = Color.FromArgb(230, 240, 250);
            _btnConnect.FlatAppearance.BorderColor = Color.FromArgb(24, 120, 200);
            _txtIp.Enabled = true;
            _cboHistory.Enabled = true;

            _pnlProgress.Visible = false;
            _progConnect.Visible = false;

            SetStatusIndicator(Color.Gray, "未连接");
            SetChannelDot(_pnlCmdChannel, CommChannelType.Command, false);
            SetChannelDot(_pnlDataChannel, CommChannelType.Data, false);
        }

        private void SetStatusIndicator(Color color, string text)
        {
            _lblStatusDot.ForeColor = color;
            _lblStatusText.Text = text;
            _lblStatusText.ForeColor = color;
        }

        private void SetChannelDot(Panel channelPanel, CommChannelType channel, bool connected)
        {
            // 面板背景与圆点颜色：命令通道 → 淡青色，数据通道 → 淡绿色
            Color bgColor, dotColor;
            if (connected)
            {
                if (channel == CommChannelType.Command)
                {
                    bgColor = Color.FromArgb(224, 242, 254);  // 淡青色背景
                    dotColor = Color.FromArgb(14, 165, 233);   // 青色圆点
                }
                else
                {
                    bgColor = Color.FromArgb(232, 245, 233);  // 淡绿色背景
                    dotColor = Color.FromArgb(34, 197, 94);   // 绿色圆点
                }
            }
            else
            {
                bgColor = Color.FromArgb(248, 248, 248);      // 浅灰色背景
                dotColor = Color.Gray;
            }

            channelPanel.BackColor = bgColor;

            foreach (Control ctrl in channelPanel.Controls)
            {
                if (ctrl is Label lbl && lbl.Text == "●")
                {
                    lbl.ForeColor = dotColor;
                    break;
                }
            }
        }

        private void UpdateStep(Label lbl, string text, Color color)
        {
            if (lbl.InvokeRequired)
                lbl.Invoke(new Action(() => { lbl.Text = text; lbl.ForeColor = color; }));
            else
            {
                lbl.Text = text;
                lbl.ForeColor = color;
            }
        }

        #endregion

        #region IP 历史记录

        private void LoadIpHistory()
        {
            try
            {
                if (File.Exists(HistoryFilePath))
                {
                    var lines = File.ReadAllLines(HistoryFilePath)
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .Take(10);

                    _cboHistory.Items.Clear();
                    _cboHistory.Items.Add("历史");
                    foreach (var line in lines)
                        _cboHistory.Items.Add(line.Trim());

                    if (_cboHistory.Items.Count > 1)
                        _cboHistory.SelectedIndex = 1;
                }
                else
                {
                    _cboHistory.Items.Add(DefaultIp);
                }
            }
            catch
            {
                _cboHistory.Items.Add(DefaultIp);
            }
        }

        private void SaveIpToHistory(string ip)
        {
            try
            {
                var list = new List<string> { ip };
                if (File.Exists(HistoryFilePath))
                {
                    var existing = File.ReadAllLines(HistoryFilePath)
                        .Where(l => !string.IsNullOrWhiteSpace(l) && l.Trim() != ip)
                        .Take(9);
                    list.AddRange(existing);
                }
                File.WriteAllLines(HistoryFilePath, list);
            }
            catch
            {
                // 静默失败，不影响主流程
            }
        }

        #endregion
    }
}
