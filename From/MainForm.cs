using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewInspect.UI;

namespace Tofd_AWI.From.NewInspect
{
    public partial class MainForm : Form
    {
        // ---------------------------------------------------------------
        // 配色常量 (与 Frm_NewInspect 保持一致)
        // ---------------------------------------------------------------
        private static readonly Color CLR_BLUE  = Color.FromArgb(56, 130, 246);
        private static readonly Color CLR_GREEN = Color.FromArgb(34, 197, 94);
        private static readonly Color CLR_RED   = Color.FromArgb(239, 68, 68);
        private static readonly Color CLR_CYAN  = Color.FromArgb(14, 165, 233);
        private static readonly Color CLR_MUTED = Color.FromArgb(100, 115, 140);

        // 定时器
        private System.Windows.Forms.Timer _connMonitorTimer;
        private System.Windows.Forms.Timer _dateTimeTimer;

        public MainForm()
        {
            InitializeComponent();
            WireUpEvents();
            WindowState = FormWindowState.Maximized;

            InitDateTimeTimer();

            // 启动断线检测
            _connMonitorTimer = new System.Windows.Forms.Timer();
            _connMonitorTimer.Interval = 3000;
            _connMonitorTimer.Tick += ConnMonitorTimer_Tick;
            _connMonitorTimer.Start();
        }

        // ==========================================
        // 事件绑定
        // ==========================================
        private void WireUpEvents()
        {
            // ★ 订阅通道级别状态变化事件 —— 命令通道/数据通道独立刷新颜色
            AppState.ChannelStateChanged += (s, args) =>
            {
                if (InvokeRequired)
                    Invoke(new Action(() => UpdateChannelStatus(args.Channel, args.Connected)));
                else
                    UpdateChannelStatus(args.Channel, args.Connected);
            };

            // "连接" 按钮
            _btnConnect.Click += (s, e) => OpenConnectDialog();

            // "设置" 按钮
            _btnSettings.Click += (s, e) =>
            {
                using (var dlg = new FrmSettings(tabIndex: 0))
                    dlg.ShowDialog(this);
            };

            // 通信状态标签点击 —— 两个通道标签都打开连接对话框
            _lblCmdStatus.Click += (s, e) => OpenConnectDialog();
            _lblDataStatus.Click += (s, e) => OpenConnectDialog();
        }

        // ==========================================
        // 日期时间
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
        // 连接
        // ==========================================
        private void OpenConnectDialog()
        {
            using (var dlg = new FrmConnect())
            {
                var result = dlg.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    // 对话框内部已通过 AppState 设置通道状态并触发事件，
                    // 此处兜底同步 UI，确保颜色正确
                    UpdateChannelStatus(CommChannelType.Command, AppState.IsCommandChannelConnected);
                    UpdateChannelStatus(CommChannelType.Data, AppState.IsDataChannelConnected);
                }
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

        /// <summary>
        /// 根据指定通道类型更新顶栏状态标签颜色
        /// 命令通道 → 青色，数据通道 → 绿色
        /// </summary>
        private void UpdateChannelStatus(CommChannelType channel, bool connected)
        {
            switch (channel)
            {
                case CommChannelType.Command:
                    _lblCmdStatus.Text = connected ? "● 命令" : "○ 命令";
                    _lblCmdStatus.ForeColor = connected ? CLR_CYAN : CLR_MUTED;
                    break;
                case CommChannelType.Data:
                    _lblDataStatus.Text = connected ? "● 数据" : "○ 数据";
                    _lblDataStatus.ForeColor = connected ? CLR_GREEN : CLR_MUTED;
                    break;
            }

            // 同步更新按钮（基于整体连接状态）
            bool overall = AppState.IsInstrumentConnected;
            _btnConnect.Text = overall ? "已连接" : "连接";
            _btnConnect.BackColor = overall ? CLR_GREEN : CLR_BLUE;
        }

        /// <summary>
        /// 兜底刷新：外部代码需要同步整体UI时调用
        /// </summary>
        public void UpdateComStatus(bool connected)
        {
            _btnConnect.Text = connected ? "已连接" : "连接";
            _btnConnect.BackColor = connected ? CLR_GREEN : CLR_BLUE;
        }
    }
}
