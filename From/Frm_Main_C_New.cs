using System;
using System.Drawing;
using System.Windows.Forms;

using NewInspect.Services;
using NewInspect.Services.State;
using ClassLibrary_Interface;

// ============================================================
// 文件: Frm_Main_C_New.cs
// 位置: Tofd_AWI/From/
// 命名空间: Tofd_AWI
// 职责: 新框架主窗体 — 只负责 UI 渲染和用户事件转发
//       所有业务逻辑委托给 Service 对象
//
// 对比原来 Frm_Main_C.cs (151KB+):
//   - 原来: 直接操作 SysInfo.m_SysBuff.m_Tofd_DLL.Link()
//   - 新:   _tofdService.Connect()
// ============================================================

namespace Tofd_AWI
{
    public partial class Frm_Main_C_New : Form
    {
        // ==========================================
        // 依赖注入 (构造函数参数 — 不再依赖 SysInfo 静态类)
        // ==========================================
        private readonly SystemConfig _config;
        private readonly SystemRuntimeState _sysState;
        private readonly TofdService _tofdService;
        private readonly MotionService _motionService;
        private readonly CameraService _cameraService;
        private readonly DataService _dataService;
        private VideoDisplayService _videoDisplayService;
        private WaveformDisplayService _waveformDisplayService;
        private readonly TofdState _tofdState;
        private readonly MotionState _motionState;
        private readonly InspectionProjectState _projectState;
        private readonly ReportState _reportState;

        // ==========================================
        // 构造函数
        // ==========================================

        /// <summary>无参构造器 — 仅供 WinForms 设计器使用</summary>
        public Frm_Main_C_New()
        {
            InitializeComponent();
        }

        /// <summary>DI 构造器 — 运行时使用</summary>
        public Frm_Main_C_New(
            SystemConfig config,
            SystemRuntimeState sysState,
            TofdService tofdService,
            MotionService motionService,
            CameraService cameraService,
            DataService dataService,
            TofdState tofdState,
            MotionState motionState,
            InspectionProjectState projectState,
            ReportState reportState)
        {
            _config        = config  ?? throw new ArgumentNullException(nameof(config));
            _sysState      = sysState ?? throw new ArgumentNullException(nameof(sysState));
            _tofdService   = tofdService ?? throw new ArgumentNullException(nameof(tofdService));
            _motionService = motionService ?? throw new ArgumentNullException(nameof(motionService));
            _cameraService = cameraService ?? throw new ArgumentNullException(nameof(cameraService));
            _dataService   = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _tofdState     = tofdState ?? throw new ArgumentNullException(nameof(tofdState));
            _motionState   = motionState ?? throw new ArgumentNullException(nameof(motionState));
            _projectState  = projectState ?? throw new ArgumentNullException(nameof(projectState));
            _reportState   = reportState ?? throw new ArgumentNullException(nameof(reportState));

            InitializeComponent();

            // 用设计器中布局好的 PictureBox 创建显示服务
            _videoDisplayService    = new VideoDisplayService(Pic_InspectVideo, Pic_OperatorVideo);
            _waveformDisplayService = new WaveformDisplayService(Pic_AScan, Pic_BScan, Pic_CScan, Pic_DScan);

            InitializeControls();
        }

        // ==========================================
        // 初始化（替代 Frm_Main_C_Load）
        // ==========================================
        private void InitializeControls()
        {
            this.WindowState = FormWindowState.Maximized;

            ApplyLanguage(_sysState.Language);

            // 创建定时器刷新状态（注册到 components 以随窗体自动释放）
            _statusTimer = new Timer();
            _statusTimer.Interval = 500;
            _statusTimer.Tick += StatusTimer_Tick;
            _statusTimer.Start();
            components?.Add(_statusTimer);

            // 订阅相机帧更新事件
            _cameraService.FrameUpdated += OnCameraFrameUpdated;

            // 初始隐藏检测画面，等连接后显示
            _videoDisplayService.IsInspectVideoActive = false;
        }

        // ==========================================
        // 事件处理 — 按钮
        // ==========================================

        private void Btn_Link_Click(object sender, EventArgs e)
        {
            bool tofdOk = _tofdService.Connect();
            bool motionOk = _motionService.Connect();

            bool allOk = tofdOk && motionOk;
            Lb_LinkState.Text = allOk ? "联机成功" : "联机失败";
            Lb_LinkState.ForeColor = allOk ? Color.Lime : Color.Red;
        }

        /// <summary>
        /// 窗体关闭时断开所有硬件连接
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _tofdService.StopAScan();
            _motionService.Disconnect();
            _cameraService.Dispose();
            _videoDisplayService.Dispose();
            _waveformDisplayService.Dispose();
            _statusTimer?.Stop();
            base.OnFormClosing(e);
        }

        private void Btn_Forward_Click(object sender, EventArgs e)
        {
            if (!_motionState.IsOnline)
            {
                ShowNotConnectedWarning();
                return;
            }
            _motionService.Forward();
        }

        private void Btn_Backward_Click(object sender, EventArgs e)
        {
            if (!_motionState.IsOnline)
            {
                ShowNotConnectedWarning();
                return;
            }
            _motionService.Backward();
        }

        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            if (!_motionState.IsOnline)
            {
                ShowNotConnectedWarning();
                return;
            }
            _motionService.Stop();
        }

        private void Btn_Mark_Click(object sender, EventArgs e)
        {
            _motionService.MarkDefect();
        }

        private void Btn_StartScan_Click(object sender, EventArgs e)
        {
            _tofdService.StartAScan((channel, data) =>
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => UpdateAScan(channel, data)));
                }
            });
        }

        private void Btn_StopScan_Click(object sender, EventArgs e)
        {
            _tofdService.StopAScan();
            _motionService.Stop();
        }

        private void ShowNotConnectedWarning()
        {
            bool isChinese = _sysState.Language == 0;
            string msg = isChinese
                ? "运动控制器未连接，请先点击\"连接\"按钮！"
                : "Motion controller not connected. Please click \"Link\" first!";
            MessageBox.Show(msg, isChinese ? "提示" : "Hint",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // ==========================================
        // 状态刷新
        // ==========================================
        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            Lb_Distance.Text = _motionState.DistanceX.ToString("F1") + " mm";
            Lb_Speed.Text = _motionState.SpeedPercent + "%";
        }

        // ==========================================
        // 相机帧更新
        // ==========================================
        private void OnCameraFrameUpdated(Bitmap frame)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ShowCameraFrame(frame)));
            }
            else
            {
                ShowCameraFrame(frame);
            }
        }

        private void ShowCameraFrame(Bitmap frame)
        {
            // 第一次收到帧时自动显示检测画面
            if (!_videoDisplayService.IsInspectVideoActive)
                _videoDisplayService.IsInspectVideoActive = true;

            // 将相机帧推送到视频显示组件 (检测摄像头)
            _videoDisplayService.UpdateInspectFrame(frame);
        }

        // ==========================================
        // A 扫描渲染
        // ==========================================
        private void UpdateAScan(int channel, byte[] data)
        {
            // 将 A 扫描数据推送到波形显示组件
            _waveformDisplayService.UpdateAScan(data);
        }

        // ==========================================
        // 语言切换
        // ==========================================
        private void ApplyLanguage(int lang)
        {
            bool isChinese = lang == 0;
            Btn_Link.Text      = isChinese ? "连接" : "Link";
            Btn_Forward.Text   = isChinese ? "前进" : "Forward";
            Btn_Backward.Text  = isChinese ? "后退" : "Backward";
            Btn_Stop.Text      = isChinese ? "停止" : "Stop";
            Btn_Mark.Text      = isChinese ? "打标" : "Mark";
            this.Text          = isChinese ? "4轮车体控制系统" : "4-Wheel Crawler Control";
        }
    }
}
