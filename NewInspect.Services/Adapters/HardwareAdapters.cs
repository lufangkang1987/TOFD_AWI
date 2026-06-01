using ClassLibrary_Interface;
using System;
using System.Threading;

// ============================================================
// 文件: HardwareAdapters.cs
// 位置: NewInspect.Services/Adapters/
// 命名空间: NewInspect.Services.Adapters
// 职责: 适配器 — 把现有硬件 DLL 封装为接口实现
//       所有适配器都是"薄壳"——只做接口适配，不动原始 DLL 逻辑
// ============================================================

namespace NewInspect.Services.Adapters
{
    /// <summary>
    /// 运动控制适配器 — 封装 Clb_MT_Comm.MT_Comm
    ///
    /// 连接生命周期:
    ///   Initialize(...) → Connect(portOrCan, isCom) → [心跳 1.5s] → Disconnect()
    ///
    /// CAN 协议指令速查:
    ///   前进  SendData(1,1,6,0,"1") → 帧 43 52 01 00 00 00 00 00
    ///   后退  SendData(1,1,6,0,"2") → 帧 43 52 02 ...
    ///   停止  SendData(1,1,6,0,"3") → 帧 43 52 03 ...
    ///   心跳  SendData(1,3,0,0,"99") → 向 MCU 索取状态
    /// </summary>
    public class CanMotionControllerAdapter : IMotionController, IDisposable
    {
        private Clb_MT_Comm.MT_Comm _mtComm;
        private Timer _heartbeatTimer;
        private int _speedPercent = 50;
        private bool _disposed;

        public bool IsConnected { get; private set; }
        public bool MarkEnabled { get; set; }
        public int CommType { get; private set; }  // 0=CAN 1=COM

        /// <summary>
        /// 校验适配器是否已初始化并可安全发送指令
        /// </summary>
        private bool CanSend => _mtComm != null && IsConnected;

        // ================================================================
        // 生命周期
        // ================================================================

        /// <summary>
        /// 带系统缓冲的初始化 — 对应原有 MT_Comm(ref clStreamVideo SysBuff, ...)
        /// 目前使用无参构造器；后续如需传递 clStreamVideo 再扩展
        /// </summary>
        public void Initialize(ref object sysBuf, ClassLibrary_Interface.MsgInterFace msgPlant, int waitTime)
        {
            _sysBuf = sysBuf;
            _mtComm = new Clb_MT_Comm.MT_Comm();
            // 原有代码: _mtComm = new MT_Comm(ref streamVideo, ref msgPlant, waitTime);
        }

        /// <summary>
        /// 连接车体控制器
        /// COM 模式: 调用 InitCom(串口号) 打开串口
        /// CAN 模式: 调用 InitCan(波特率) 打开 CAN 设备 + 启动接收
        /// 连接成功后启动心跳定时器 (每 1.5s 索取状态)
        /// </summary>
        public bool Connect(string portOrCan, bool isComNotCan)
        {
            // 懒初始化 — 如果还没创建 MT_Comm 则用无参构造器自动创建
            if (_mtComm == null)
                _mtComm = new Clb_MT_Comm.MT_Comm();

            try
            {
                CommType = isComNotCan ? 1 : 0;
                _mtComm.m_blCom1_Can0 = CommType;

                if (CommType == 1)
                {
                    // COM 串口模式
                    _mtComm.InitCom(portOrCan);
                }
                else
                {
                    // CAN 模式 — 默认 125Kbps, USBCAN_1CH(索引5)
                    _mtComm.InitCan(125, 5);
                }

                IsConnected = _mtComm.m_blLink;

                if (IsConnected)
                    StartHeartbeat();
            }
            catch
            {
                IsConnected = false;
            }
            return IsConnected;
        }

        /// <summary>
        /// 断开连接: 停止心跳 → 关闭 COM/CAN 设备
        /// </summary>
        public void Disconnect()
        {
            StopHeartbeat();

            if (_mtComm != null)
            {
                try { _mtComm.CloseSet(); } catch { }
            }
            IsConnected = false;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            Disconnect();
            _heartbeatTimer?.Dispose();
            _heartbeatTimer = null;
        }

        // ================================================================
        // 基础运动 — P0 核心
        // ================================================================

        /// <summary>手动前进 — SendData(1,1,6,0,"1")</summary>
        public void MoveForward()
        {
            System.Diagnostics.Debug.WriteLine("[MotionAdapter] MoveForward called. CanSend={0}, IsConnected={1}, _mtComm={2}",
                CanSend, IsConnected, _mtComm != null);
            if (!CanSend) return;
            _mtComm.SendData(1, 1, 6, 0, "1");
            Clb_MT_Comm.MT_Comm.m_blQin1_Hou2 = 1;
        }

        /// <summary>手动后退 — SendData(1,1,6,0,"2")</summary>
        public void MoveBackward()
        {
            System.Diagnostics.Debug.WriteLine("[MotionAdapter] MoveBackward called. CanSend={0}", CanSend);
            if (!CanSend) return;
            _mtComm.SendData(1, 1, 6, 0, "2");
            Clb_MT_Comm.MT_Comm.m_blQin1_Hou2 = 2;
        }

        /// <summary>手动左转 — SendData(1,1,6,0,"4")</summary>
        public void MoveLeft()
        {
            System.Diagnostics.Debug.WriteLine("[MotionAdapter] MoveLeft called. CanSend={0}", CanSend);
            if (!CanSend) return;
            _mtComm.SendData(1, 1, 6, 0, "4");
            Clb_MT_Comm.MT_Comm.m_blQin1_Hou2 = 4;
        }
        
        /// <summary>手动右转 — SendData(1,1,6,0,"5")</summary>
        public void MoveRight()
        {
            System.Diagnostics.Debug.WriteLine("[MotionAdapter] MoveRight called. CanSend={0}", CanSend);
            if (!CanSend) return;
            _mtComm.SendData(1, 1, 6, 0, "5");
            Clb_MT_Comm.MT_Comm.m_blQin1_Hou2 = 5;
        }           

        /// <summary>停止 — SendData(1,1,6,0,"3")</summary>
        public void Stop()
        {
            System.Diagnostics.Debug.WriteLine("[MotionAdapter] Stop called. CanSend={0}", CanSend);
            if (!CanSend) return;
            _mtComm.SendData(1, 1, 6, 0, "3");
            Clb_MT_Comm.MT_Comm.m_blQin1_Hou2 = 0;
        }

        // ================================================================
        // 速度控制
        // ================================================================

        /// <summary>
        /// 设置车体+光栅臂速度
        /// 手动模式下 iComType_0=2 (帧头 0x4A 0x56)
        /// strVal 格式: "车体速度,光栅臂速度"
        /// </summary>
        public void SetSpeed(int percent)
        {
            _speedPercent = percent;
            if (!CanSend) return;
            // 手动速度指令: iComType_0=2, iComType_1=2(速度参数)
            // byte[4-5]=车体速度高/低, byte[6-7]=滑台速度高/低
            _mtComm.SendData(2, 2, 0, 0, percent + ",50");
        }

        public int GetSpeed() => _speedPercent;

        // ================================================================
        // 状态查询
        // ================================================================

        public float GetDistance()
        {
            // 距离值由 MT_Comm 内部接收线程自动更新到 m_SysBuf
            // 后续 P1 通过读取回传帧解析编码器位置实现
            return 0f;
        }

        public float GetCurrentSpeed() => _speedPercent;

        // ================================================================
        // 光栅臂控制 (P2)
        // ================================================================

        /// <summary>光栅臂抬起/落下 — SendData(5,1,0,0,"0"提起/"1"落下)</summary>
        public void SetGratingArm(bool up)
        {
            if (!CanSend) return;
            _mtComm.SendData(5, 1, 0, 0, up ? "0" : "1");
        }

        public void SetGratingRange(int startPos, int endPos)
        {
            if (!CanSend) return;
            // 帧头 0x42 0x4C, 扫查模式 byte[3]=0x00, byte[4-7]=起止位置
            string val = startPos + "," + endPos;
            _mtComm.SendData(2, 1, 0, 0, val);
        }

        public void SetGratingCorrection(int direction)
        {
            if (!CanSend) return;
            // 方向: 1=左纠偏 2=右纠偏 3=停止纠偏
            _mtComm.SendData(1, 1, 8, 0, direction.ToString());
        }

        // ================================================================
        // 打标 & 灯光 (P3)
        // ================================================================

        /// <summary>缺陷打标 — SendData(5,2,0,0,"1")</summary>
        public void MarkDefect()
        {
            if (!CanSend) return;
            _mtComm.SendData(5, 2, 0, 0, "1");
        }

        /// <summary>前后灯切换: true=前灯 false=后灯</summary>
        public void SetLightPosition(bool front)
        {
            if (!CanSend) return;
            int lightLevel = front ? 50 : 0;
            _mtComm.SendData(4, 1, front ? 3 : 4, 0, lightLevel.ToString());
        }

        // ================================================================
        // 私有: 心跳 & 字段
        // ================================================================

        private object _sysBuf;

        /// <summary>
        /// 启动心跳定时器 — 每 1.5 秒向车体 MCU 索取在线状态
        /// 不发送心跳的话 MCU 会在 ~3 秒内判定主机断线并自动停车
        /// </summary>
        private void StartHeartbeat()
        {
            StopHeartbeat();
            _heartbeatTimer = new Timer(
                callback: _ => SendHeartbeat(),
                state: null,
                dueTime: 1500,
                period: 1500);
        }

        private void StopHeartbeat()
        {
            _heartbeatTimer?.Dispose();
            _heartbeatTimer = null;
        }

        /// <summary>
        /// 发送心跳帧 — SendData(1,3,0,0,"99")
        /// 对应协议: iComType_0=1(动作类), iComType_1=3(索取状态), byte[3]=0x99
        /// </summary>
        private void SendHeartbeat()
        {
            if (!CanSend) return;
            try { _mtComm.SendData(1, 3, 0, 0, "99"); } catch { }
        }
    }

    /// <summary>
    /// TOFD 硬件适配器 — 封装 Tofd_DLL.CLTofd_BJ
    /// 注意: Tofd_DLL 引用在原有代码中被注释，需先恢复引用
    /// </summary>
    public class TofdHardwareAdapter : ITofdHardware
    {
        private object _dll;

        public bool IsConnected { get; private set; }

        public bool Connect()
        {
            // 原有: bool ret = _dll.Link();
            return IsConnected;
        }

        public void Disconnect()
        {
            IsConnected = false;
        }

        public void SetGain(int channel, int db)
        {
            // 原有: _dll.SetGain(channel, db);
        }

        public int GetGain(int channel)
        {
            return 0;
        }

        public void SetSoundVelocity(float velocity)
        {
            // 原有: _dll.SetSoundVel(velocity);
        }

        public void SetPulseWidth(int us)
        {
            // 原有: _dll.SetPulseWidth(us);
        }

        public void SetScanRange(int rangeUs)
        {
            // 原有: _dll.SetRange(rangeUs);
        }

        public byte[] AcquireAScan(int channel)
        {
            return new byte[0];
        }

        public byte[] AcquireDScan()
        {
            return new byte[0];
        }

        public void StartAcquisition(int channel, Action<int, byte[]> onData)
        {
            // 原有: 启动采集线程 (Thread_A_Wave)
        }

        public void StopAcquisition()
        {
            // 原有: 停止采集线程
        }
    }

    /// <summary>
    /// 相机适配器 — 封装 Clb_XmCam_DLL + MVSDK
    /// </summary>
    public class CameraAdapter : ICameraDevice
    {
        public bool IsOpen { get; private set; }
        public string IpAddress { get; private set; }

        public bool Open(string ipAddress)
        {
            IpAddress = ipAddress;
            // 原有: 通过 Clb_XmCam_DLL 或 CameNet 连接相机
            IsOpen = true;
            return true;
        }

        public void Close()
        {
            IsOpen = false;
        }

        public System.Drawing.Bitmap Capture()
        {
            // 原有: 调用相机 SDK 抓取
            return null;
        }

        public void StartStream(Action<System.Drawing.Bitmap> onFrame)
        {
            // 原有: 启动视频流线程
        }

        public void StopStream()
        {
            // 原有: 停止视频流
        }
    }
}
