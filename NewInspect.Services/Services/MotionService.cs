using ClassLibrary_Interface;
using NewInspect.Services.State;
using System;
using System.Threading;

// ============================================================
// 文件: MotionService.cs
// 位置: NewInspect.Services/Services/
// 命名空间: NewInspect.Services
// 职责: 运动控制业务逻辑 — 爬行器/4轮车体的前后左右、光栅臂、打标
//       替代 Frm_Move.cs 和 Frm_Main_C.cs 中的运动控制逻辑
// ============================================================

namespace NewInspect.Services
{
    public class MotionService
    {
        private readonly IMotionController _controller;
        private readonly MotionState _state;
        private readonly SystemConfig _config;

        // 通讯轮询线程 (原 Trd_GetCanData)
        private Thread _commThread;
        private bool _isRunning;

        public MotionService(
            IMotionController controller,
            MotionState state,
            SystemConfig config)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        // ========== 连接管理 ==========

        /// <summary>连接车体 (对应 Ck_Com_Can 后的连接操作)</summary>
        public bool Connect()
        {
            bool isCom = _config.CommMode == 1;
            string portOrCan = isCom ? _config.CommPort : "0";

            bool ok = _controller.Connect(portOrCan, isCom);
            _state.IsOnline = ok;

            if (ok)
            {
                StartDistancePolling();
            }
            return ok;
        }

        /// <summary>断开连接</summary>
        public void Disconnect()
        {
            StopDistancePolling();
            _controller.Disconnect();
            _state.IsOnline = false;
        }

        // ========== 运动 ==========

        /// <summary>设置速度 (对应 Track_Sd 事件)</summary>
        public void SetSpeed(int percent)
        {
            _controller.SetSpeed(percent);
            _state.SpeedPercent = percent;
        }

        /// <summary>前进 (对应 Bt_QianJin)，返回是否成功发送指令</summary>
        public bool Forward()
        {
            if (!_state.IsOnline) return false;
            _controller.MoveForward();
            _state.Direction = 1;
            return true;
        }

        /// <summary>后退 (对应 Bt_HouTui)，返回是否成功发送指令</summary>
        public bool Backward()
        {
            if (!_state.IsOnline) return false;
            _controller.MoveBackward();
            _state.Direction = 2;
            return true;
        }

        /// <summary>停止 (对应 Bt_Stop)，返回是否成功发送指令</summary>
        public bool Stop()
        {
            if (!_state.IsOnline) return false;
            _controller.Stop();
            _state.Direction = 0;
            return true;
        }

        // ========== 光栅臂 ==========

        /// <summary>光栅臂抬起/落下 (对应 Ck_Gsb 等)</summary>
        public void SetGratingArm(bool up)
        {
            _controller.SetGratingArm(up);
            _state.GratingArmState = up ? 1 : 0;
        }

        /// <summary>设置光栅臂运行区间</summary>
        public void SetGratingRange(int startPos, int endPos)
        {
            _controller.SetGratingRange(startPos, endPos);
            _state.GratingStartPos = startPos;
            _state.GratingEndPos = endPos;
        }

        /// <summary>光栅臂步进间隔</summary>
        public void SetGratingStep(int interval)
        {
            _state.GratingStepInterval = interval;
        }

        // ========== 打标 ==========

        public void MarkDefect()
        {
            _controller.MarkDefect();
        }

        public void SetMarkEnabled(bool enabled)
        {
            _controller.MarkEnabled = enabled;
            _state.MarkEnabled = enabled;
        }

        // ========== 灯光 ==========

        public void SetLight(bool front)
        {
            _controller.SetLightPosition(front);
        }

        // ========== 状态查询 ==========

        /// <summary>获取当前距离</summary>
        public float GetDistance() => _controller.GetDistance();

        // ========== 内部：距离轮询 ==========

        private void StartDistancePolling()
        {
            _isRunning = true;
            _commThread = new Thread(() =>
            {
                while (_isRunning)
                {
                    try
                    {
                        _state.DistanceX = _controller.GetDistance();
                        Thread.Sleep(100);
                    }
                    catch { break; }
                }
            })
            { IsBackground = true };
            _commThread.Start();
        }

        private void StopDistancePolling()
        {
            _isRunning = false;
            _commThread?.Join(500);
        }
    }
}
