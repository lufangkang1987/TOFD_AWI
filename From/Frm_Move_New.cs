using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;

using NewInspect.Services;
using NewInspect.Services.State;
using ClassLibrary_Interface;

// ============================================================
// 文件: Frm_Move_New.cs
// 位置: Tofd_AWI/From/
// 命名空间: Tofd_AWI.From
// 职责: 新框架运动控制窗体 — 小车前后左右/光栅臂/打标/灯光
//
// 完整调用链 (以"前进"为例):
//   Btn_Forward_MouseDown
//     → _motionService.Forward()
//       → _controller.MoveForward()              [IMotionController 接口]
//         → _mtComm.SendData(1,1,6,0,"1")        [CanMotionControllerAdapter]
//           → 组帧 43 52 01 00 00 01 00 00       [MT_Comm 协议层]
//             → CAN_ChannelSend(...)              [CanCmd.dll P/Invoke]
//               → USBCAN 卡 → CAN_H/CAN_L 差分信号 → 车体 MCU
//
// 对比旧 Frm_Move.cs:
//   旧: 直接 SysInfo.m_Climb4.SendData(1,1,6,0,"1")
//   新: _motionService.Forward() — 完全通过 DI 注入
// ============================================================

namespace Tofd_AWI.From
{
    public partial class Frm_Move_New : Form
    {
        // ==========================================
        // 依赖注入
        // ==========================================
        private readonly MotionService _motionService;
        private readonly MotionState _motionState;
        private readonly SystemConfig _config;

        /// <summary>无参构造器 — 仅供 WinForms 设计器使用</summary>
        public Frm_Move_New()
        {
            InitializeComponent();
        }

        /// <summary>DI 构造器 — 运行时使用</summary>
        public Frm_Move_New(
            MotionService motionService,
            MotionState motionState,
            SystemConfig config)
        {
            _motionService = motionService ?? throw new ArgumentNullException(nameof(motionService));
            _motionState   = motionState ?? throw new ArgumentNullException(nameof(motionState));
            _config        = config ?? throw new ArgumentNullException(nameof(config));

            InitializeComponent();
            InitializeForm();
        }

        // ==========================================
        // 初始化
        // ==========================================

        private void InitializeForm()
        {
            // 填充 COM 口列表
            PopulateComPorts();

            // 加载配置
            Ck_Com_Can.Checked = _config.CommMode == 1;
            Cmb_Port.Text = _config.CommPort;
            Cmb_Port.Visible = Ck_Com_Can.Checked;

            Track_Speed.Value = _motionState.SpeedPercent;
            Lb_SpeedVal.Text = _motionState.SpeedPercent + "%";

            Ck_MarkEnabled.Checked = _motionState.MarkEnabled;

            // 刷新连接状态显示
            RefreshConnectState();
            RefreshGratingState();

            Timer_Refresh.Start();
        }

        private void PopulateComPorts()
        {
            Cmb_Port.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string p in ports)
                Cmb_Port.Items.Add(p);
            if (Cmb_Port.Items.Count > 0 && string.IsNullOrEmpty(Cmb_Port.Text))
                Cmb_Port.SelectedIndex = 0;
        }

        // ==========================================
        // 连接管理
        // ==========================================

        private void Btn_Link_Click(object sender, EventArgs e)
        {
            Btn_Link.Enabled = false;
            try
            {
                // 注意: CommMode/CommPort 从 HardConfig.ini 加载，不在运行时修改
                // 若需运行时切换，应通过 MotionService.Connect(mode, port) 重载方法
                bool ok = _motionService.Connect();
                RefreshConnectState();

                MessageBox.Show(
                    "车体联机: " + (ok ? "成功" : "失败"),
                    "连接结果",
                    MessageBoxButtons.OK,
                    ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            finally
            {
                Btn_Link.Enabled = true;
            }
        }

        private void Btn_DisLink_Click(object sender, EventArgs e)
        {
            _motionService.Disconnect();
            RefreshConnectState();
        }

        private void Ck_Com_Can_CheckedChanged(object sender, EventArgs e)
        {
            Cmb_Port.Visible = Ck_Com_Can.Checked;
            if (Ck_Com_Can.Checked && Cmb_Port.Items.Count == 0)
                PopulateComPorts();
        }

        // ==========================================
        // 运动方向控制
        // 使用 MouseDown/MouseUp 实现"按住走、松开停"
        // ==========================================

        private void Btn_Forward_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_motionState.IsOnline) { WarnNotConnected(); return; }
            _motionService.Forward();
        }

        private void Btn_Backward_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_motionState.IsOnline) { WarnNotConnected(); return; }
            _motionService.Backward();
        }

        private void Btn_Left_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_motionState.IsOnline) { WarnNotConnected(); return; }
            // 前进左转 — 通过适配器的底层 SendData(1,1,7,0,"4") 实现
            // 暂不暴露独立接口，由适配器内部处理
            //MessageBox.Show("左转功能通过 CAN 帧 0x04 实现，待适配器扩展", "提示");
            _motionService.MoveLeft();
        }

        private void Btn_Right_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_motionState.IsOnline) { WarnNotConnected(); return; }
            // 前进右转 — CAN 帧 0x05
            //MessageBox.Show("右转功能通过 CAN 帧 0x05 实现，待适配器扩展", "提示");
            _motionService.MoveRight();
        }

        private void Btn_Stop_MouseUp(object sender, MouseEventArgs e)
        {
            _motionService.Stop();
        }

        /// <summary>停止按钮 (Click 事件) — 独立于 MouseUp，点停止立即停</summary>
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            _motionService.Stop();
        }

        private void WarnNotConnected()
        {
            MessageBox.Show("请先连接车体！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // ==========================================
        // 速度控制
        // ==========================================

        private void Track_Speed_Scroll(object sender, EventArgs e)
        {
            int speed = Track_Speed.Value;
            _motionService.SetSpeed(speed);
            Lb_SpeedVal.Text = speed + "%";
        }

        private void Btn_SpeedAdd_Click(object sender, EventArgs e)
        {
            if (Track_Speed.Value < Track_Speed.Maximum)
                Track_Speed.Value += 5;
            Track_Speed_Scroll(sender, e);
        }

        private void Btn_SpeedDec_Click(object sender, EventArgs e)
        {
            if (Track_Speed.Value > Track_Speed.Minimum)
                Track_Speed.Value -= 5;
            Track_Speed_Scroll(sender, e);
        }

        // ==========================================
        // 光栅臂控制
        // ==========================================

        private void Btn_GratingUp_Click(object sender, EventArgs e)
        {
            if (!_motionState.IsOnline) { WarnNotConnected(); return; }
            _motionService.SetGratingArm(true);
            RefreshGratingState();
        }

        private void Btn_GratingDown_Click(object sender, EventArgs e)
        {
            if (!_motionState.IsOnline) { WarnNotConnected(); return; }
            _motionService.SetGratingArm(false);
            RefreshGratingState();
        }

        // ==========================================
        // 打标 & 灯光
        // ==========================================

        private void Btn_Mark_Click(object sender, EventArgs e)
        {
            if (!_motionState.IsOnline) { WarnNotConnected(); return; }
            _motionService.MarkDefect();
        }

        private void Ck_MarkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            _motionService.SetMarkEnabled(Ck_MarkEnabled.Checked);
        }

        private void Btn_LightFront_Click(object sender, EventArgs e)
        {
            if (!_motionState.IsOnline) { WarnNotConnected(); return; }
            _motionService.SetLight(true);
        }

        private void Btn_LightBack_Click(object sender, EventArgs e)
        {
            if (!_motionState.IsOnline) { WarnNotConnected(); return; }
            _motionService.SetLight(false);
        }

        // ==========================================
        // 定时刷新状态
        // ==========================================

        private void Timer_Refresh_Tick(object sender, EventArgs e)
        {
            Lb_StatusDistance.Text = $"距离: {_motionState.DistanceX:F1} mm";
            Lb_StatusSpeed.Text = $"速度: {_motionState.SpeedPercent}%";

            string dir;
            switch (_motionState.Direction)
            {
                case 1: dir = "前进"; break;
                case 2: dir = "后退"; break;
                default: dir = "停止"; break;
            }
            Lb_StatusDirection.Text = "方向: " + dir;

            // 连接状态变化时刷新
            RefreshConnectState();
        }

        // ==========================================
        // 界面状态刷新
        // ==========================================

        private void RefreshConnectState()
        {
            if (_motionState.IsOnline)
            {
                Lb_LinkState.Text = "联机: 成功";
                Lb_LinkState.ForeColor = Color.Green;
            }
            else
            {
                Lb_LinkState.Text = "联机: 未连接";
                Lb_LinkState.ForeColor = Color.Red;
            }
        }

        private void RefreshGratingState()
        {
            Lb_GratingState.Text = "状态: " + (_motionState.GratingArmState == 1 ? "抬起" : "落下");
        }

        // ==========================================
        // 窗体关闭
        // ==========================================

        private void Frm_Move_New_FormClosing(object sender, FormClosingEventArgs e)
        {
            Timer_Refresh.Stop();
            // 不在此断开连接 — 连接在主窗体关闭时统一断开
        }
    }
}
