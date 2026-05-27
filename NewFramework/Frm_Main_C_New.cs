using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

using Tofd_AWI.NewFramework.Services;
using Tofd_AWI.NewFramework.State;
using ClassLibrary_Interface;

// ============================================================
// 文件: Frm_Main_C_New.cs
// 位置: NewFramework/ (参考用，展示新主窗体的结构)
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
        private readonly VideoDisplayService _videoDisplayService;
        private readonly WaveformDisplayService _waveformDisplayService;
        private readonly TofdState _tofdState;
        private readonly MotionState _motionState;
        private readonly InspectionProjectState _projectState;
        private readonly ReportState _reportState;

        // ==========================================
        // UI 控件引用 (对应 Designer 生成的控件)
        // ==========================================
        // 标签
        private Label Lb_LinkState;          // 连接状态
        private Label Lb_Distance;           // 距离显示
        private Label Lb_Speed;              // 速度显示
        private Label Lb_Battery;            // 电量显示

        // 按钮
        private Button Btn_Link;
        private Button Btn_Forward;
        private Button Btn_Backward;
        private Button Btn_Stop;
        private Button Btn_Mark;
        private Button Btn_StartScan;
        private Button Btn_StopScan;

        // Timer
        private Timer _statusTimer;

        // ==========================================
        // 构造函数
        // ==========================================
        public Frm_Main_C_New(
            SystemConfig config,
            SystemRuntimeState sysState,
            TofdService tofdService,
            MotionService motionService,
            CameraService cameraService,
            DataService dataService,
            VideoDisplayService videoDisplayService,
            WaveformDisplayService waveformDisplayService,
            TofdState tofdState,
            MotionState motionState,
            InspectionProjectState projectState,
            ReportState reportState)
        {
            _config       = config  ?? throw new ArgumentNullException(nameof(config));
            _sysState     = sysState ?? throw new ArgumentNullException(nameof(sysState));
            _tofdService  = tofdService ?? throw new ArgumentNullException(nameof(tofdService));
            _motionService = motionService ?? throw new ArgumentNullException(nameof(motionService));
            _cameraService = cameraService ?? throw new ArgumentNullException(nameof(cameraService));
            _dataService  = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _videoDisplayService    = videoDisplayService ?? throw new ArgumentNullException(nameof(videoDisplayService));
            _waveformDisplayService = waveformDisplayService ?? throw new ArgumentNullException(nameof(waveformDisplayService));
            _tofdState    = tofdState ?? throw new ArgumentNullException(nameof(tofdState));
            _motionState  = motionState ?? throw new ArgumentNullException(nameof(motionState));
            _projectState = projectState ?? throw new ArgumentNullException(nameof(projectState));
            _reportState  = reportState ?? throw new ArgumentNullException(nameof(reportState));

            InitializeComponent();
            InitializeControls();
        }

        // ==========================================
        // 初始化（替代 Frm_Main_C_Load）
        // ==========================================
        private void InitializeControls()
        {
            this.WindowState = FormWindowState.Maximized;

            ApplyLanguage(_sysState.Language);

            // 创建定时器刷新状态
            _statusTimer = new Timer();
            _statusTimer.Interval = 500;  // 500ms
            _statusTimer.Tick += StatusTimer_Tick;
            _statusTimer.Start();

            // 订阅相机帧 → 视频显示组件
            _cameraService.FrameUpdated += OnCameraFrameUpdated;

            // 初始化视频显示组件 (默认隐藏，等有画面时显示)
            _videoDisplayService.InspectVideoBox.Visible = false;
            _videoDisplayService.OperatorVideoBox.Visible = false;
        }

        // ==========================================
        // 事件处理 — 按钮 (原来这些是 Frm_Main_C.cs 中的 3000+ 行按钮事件)
        // ==========================================

        /// <summary>连接 TOFD 设备</summary>
        private void Btn_Link_Click(object sender, EventArgs e)
        {
            // ====== 原来：======
            // bool _blRet = SysInfo.m_SysBuff.m_Tofd_DLL.Link();
            // ====== 新：======
            bool ok = _tofdService.Connect();
            Lb_LinkState.Text = ok ? "联机成功" : "联机失败";
            Lb_LinkState.ForeColor = ok ? Color.Lime : Color.Red;
        }

        /// <summary>前进</summary>
        private void Btn_Forward_Click(object sender, EventArgs e)
        {
            _motionService.Forward();
        }

        /// <summary>后退</summary>
        private void Btn_Backward_Click(object sender, EventArgs e)
        {
            _motionService.Backward();
        }

        /// <summary>停止</summary>
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            _motionService.Stop();
        }

        /// <summary>缺陷打标</summary>
        private void Btn_Mark_Click(object sender, EventArgs e)
        {
            _motionService.MarkDefect();

            // 保存报警记录
            // _dataService.SaveAlarm(...);
        }

        /// <summary>开始检测</summary>
        private void Btn_StartScan_Click(object sender, EventArgs e)
        {
            // 原来：
            // SysInfo.m_SysBuff.m_C_Ctrl.m_iRun = 1;
            // Thread_A_Wave = new Thread(...);
            // ====== 新：======
            _tofdService.StartAScan((channel, data) =>
            {
                // 数据到达后更新 A 扫描图
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => UpdateAScan(channel, data)));
                }
            });
        }

        /// <summary>停止检测</summary>
        private void Btn_StopScan_Click(object sender, EventArgs e)
        {
            _tofdService.StopAScan();
            _motionService.Stop();
        }

        // ==========================================
        // 状态刷新 (替代原来散落各处的刷新代码)
        // ==========================================
        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            // 距离
            Lb_Distance.Text = _motionState.DistanceX.ToString("F1") + " mm";

            // 速度
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
            // 更新检测摄像头画面 (组件1)
            string overlay = $"距离: {_motionState.DistanceX:F1} mm  |  {DateTime.Now:HH:mm:ss}";
            _videoDisplayService.UpdateInspectFrame(frame, overlay);
        }

        // ==========================================
        // A 扫描渲染 (替代原 Thread_A_Wave 中的绘图代码)
        // ==========================================
        private void UpdateAScan(int channel, byte[] data)
        {
            // 委托给波形显示组件渲染
            _waveformDisplayService.UpdateAScan(data);
        }

        // ==========================================
        // 语言切换 (替代原来重复的 if(m_iLanguage==0) 判断)
        // ==========================================
        private void ApplyLanguage(int lang)
        {
            bool isChinese = lang == 0;
            Btn_Link.Text   = isChinese ? "连接" : "Link";
            Btn_Forward.Text = isChinese ? "前进" : "Forward";
            Btn_Backward.Text = isChinese ? "后退" : "Backward";
            Btn_Stop.Text   = isChinese ? "停止" : "Stop";
            Btn_Mark.Text   = isChinese ? "打标" : "Mark";
            this.Text       = isChinese ? "4轮车体控制系统" : "4-Wheel Crawler Control";
        }

        // ==========================================
        // 释放资源
        // ==========================================
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

        // ==========================================
        // Designer 生成的代码 (简化版)
        // ==========================================
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // ---- 控件布局 (简化示例) ----
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 状态栏
            Lb_LinkState = new Label { Location = new Point(10, 10), Size = new Size(150, 25), Text = "未连接" };
            Lb_Distance  = new Label { Location = new Point(200, 10), Size = new Size(150, 25), Text = "0.0 mm" };
            Lb_Speed     = new Label { Location = new Point(400, 10), Size = new Size(100, 25), Text = "0%" };

            // 按钮
            Btn_Link = new Button { Location = new Point(10, 50), Size = new Size(100, 35), Text = "连接" };
            Btn_Link.Click += Btn_Link_Click;

            Btn_Forward = new Button { Location = new Point(120, 50), Size = new Size(80, 35), Text = "前进" };
            Btn_Forward.Click += Btn_Forward_Click;

            Btn_Backward = new Button { Location = new Point(210, 50), Size = new Size(80, 35), Text = "后退" };
            Btn_Backward.Click += Btn_Backward_Click;

            Btn_Stop = new Button { Location = new Point(300, 50), Size = new Size(80, 35), Text = "停止" };
            Btn_Stop.Click += Btn_Stop_Click;

            Btn_Mark = new Button { Location = new Point(390, 50), Size = new Size(80, 35), Text = "打标" };
            Btn_Mark.Click += Btn_Mark_Click;

            Btn_StartScan = new Button { Location = new Point(500, 50), Size = new Size(100, 35), Text = "开始检测" };
            Btn_StartScan.Click += Btn_StartScan_Click;

            Btn_StopScan = new Button { Location = new Point(610, 50), Size = new Size(100, 35), Text = "停止检测" };
            Btn_StopScan.Click += Btn_StopScan_Click;

            // ==========================================
            // 视频显示组件 (由 VideoDisplayService 管理)
            // ==========================================
            // 组件1: 检测摄像头画面 (主画面，右上区域)
            _videoDisplayService.InspectVideoBox.Location = new Point(930, 100);
            _videoDisplayService.InspectVideoBox.Size = new Size(330, 250);

            // 组件2: 操作员摄像头画面 (右下区域)
            _videoDisplayService.OperatorVideoBox.Location = new Point(930, 360);
            _videoDisplayService.OperatorVideoBox.Size = new Size(330, 250);

            // ==========================================
            // 波形显示组件 (由 WaveformDisplayService 管理)
            // ==========================================
            // A 扫描显示区
            _waveformDisplayService.AScanBox.Location = new Point(10, 100);
            _waveformDisplayService.AScanBox.Size = new Size(450, 300);

            // B 扫描显示区
            _waveformDisplayService.BScanBox.Location = new Point(470, 100);
            _waveformDisplayService.BScanBox.Size = new Size(450, 300);

            // C 扫描显示区
            _waveformDisplayService.CScanBox.Location = new Point(10, 410);
            _waveformDisplayService.CScanBox.Size = new Size(450, 180);

            // D 扫描显示区
            _waveformDisplayService.DScanBox.Location = new Point(470, 410);
            _waveformDisplayService.DScanBox.Size = new Size(450, 180);

            // 添加到窗体
            this.Controls.Add(Lb_LinkState);
            this.Controls.Add(Lb_Distance);
            this.Controls.Add(Lb_Speed);
            this.Controls.Add(Btn_Link);
            this.Controls.Add(Btn_Forward);
            this.Controls.Add(Btn_Backward);
            this.Controls.Add(Btn_Stop);
            this.Controls.Add(Btn_Mark);
            this.Controls.Add(Btn_StartScan);
            this.Controls.Add(Btn_StopScan);

            // 视频显示组件
            this.Controls.Add(_videoDisplayService.InspectVideoBox);
            this.Controls.Add(_videoDisplayService.OperatorVideoBox);

            // 波形显示组件 (A/B/C/D)
            this.Controls.Add(_waveformDisplayService.AScanBox);
            this.Controls.Add(_waveformDisplayService.BScanBox);
            this.Controls.Add(_waveformDisplayService.CScanBox);
            this.Controls.Add(_waveformDisplayService.DScanBox);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
