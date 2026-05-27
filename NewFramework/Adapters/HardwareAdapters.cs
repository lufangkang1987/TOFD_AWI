using ClassLibrary_Interface;
using System;

// ============================================================
// 文件: HardwareAdapters.cs
// 位置: NewFramework/Adapters/
// 职责: 适配器 — 把现有硬件 DLL 封装为接口实现
//       所有适配器都是"薄壳"——只做接口适配，不动原始 DLL 逻辑
// ============================================================

namespace Tofd_AWI.NewFramework.Adapters
{
    // ======== 说明 ========
    // 这些适配器需要引入原有 DLL 的引用: Clb_MT_Comm, Tofd_DLL, Clb_XmCam_DLL 等
    // 在实际项目中，这些类应该放在各自对应的 DLL 项目里，
    // 或者放在一个新的 NewInspect.Infrastructure 项目中。
    //
    // 当前由于 Tofd_DLL 引用已被注释，部分适配器代码对应到 SysInfo 的实际调用方式。
    // ======================

    /// <summary>
    /// 运动控制适配器 — 封装 Clb_MT_Comm.CanCmd + MT_Comm
    /// 不改变原有 DLL 任何代码，只是套一层壳
    /// </summary>
    public class CanMotionControllerAdapter : IMotionController
    {
        // 引用原有 CAN 指令类
        private Clb_MT_Comm.MT_Comm _mtComm;
        private object _sysBuf;  // 实际是 ref clStreamVideo

        public bool IsConnected { get; private set; }
        public bool MarkEnabled { get; set; }
        public int CommType { get; private set; }  // 0=CAN 1=COM

        public CanMotionControllerAdapter()
        {
        }

        /// <summary>
        /// 带系统缓冲的初始化 — 对应原有 MT_Comm(ref clStreamVideo SysBuff, ...)
        /// </summary>
        public void Initialize(ref object sysBuf, ClassLibrary_Interface.ClsMsgInterFace msgPlant, int waitTime)
        {
            _sysBuf = sysBuf;
            _mtComm = new Clb_MT_Comm.MT_Comm();
            // 原有代码: _mtComm = new MT_Comm(ref streamVideo, ref msgPlant, waitTime);
        }

        public bool Connect(string portOrCan, bool isComNotCan)
        {
            try
            {
                CommType = isComNotCan ? 1 : 0;
                // 原有逻辑：
                // if (CommType == 0) OpenCanDevice(baudrate);
                // else OpenComPort(portOrCan);
                _mtComm.m_blCom1_Can0 = CommType;
                IsConnected = true;
            }
            catch
            {
                IsConnected = false;
            }
            return IsConnected;
        }

        public void Disconnect()
        {
            // 原有逻辑: CloseCanDevice() / CloseComPort()
            IsConnected = false;
        }

        public void SetSpeed(int percent)
        {
            // 原有: SysInfo.m_SysBuff.m_Climb.Speed = percent;
        }

        public int GetSpeed()
        {
            return 50; // 从原 SysBuff 读取
        }

        public void MoveForward()
        {
            // 原有: 发送 CAN 协议指令 — CanCmd.Transmit(...)
            // MT_Comm.m_blQin1_Hou2 = 1;
        }

        public void MoveBackward()
        {
            // 原有: 发送 CAN 协议指令
            // MT_Comm.m_blQin1_Hou2 = 2;
        }

        public void Stop()
        {
            // 原有: 发送停止指令
        }

        public float GetDistance()
        {
            // 原有: 从 MT_Comm 通讯线程中读取实时距离
            return 0f;
        }

        public float GetCurrentSpeed()
        {
            return 0f;
        }

        public void SetGratingArm(bool up)
        {
            // 原有: SysInfo.m_SysBuff.m_Climb 或 CAN 指令
            // m_iGsbTqLx = up ? 1 : 0;
        }

        public void SetGratingRange(int startPos, int endPos)
        {
            // 原有: SysInfo.m_SysBuff.m_Climb.i_Gsb_Start_Pos / i_Gsb_End_Pos
        }

        public void SetGratingCorrection(int direction)
        {
            // 原有: SysInfo.m_Climb4.m_i_Jp = direction
        }

        public void MarkDefect()
        {
            // 原有: 发送打标 CAN 指令
        }

        public void SetLightPosition(bool front)
        {
            // 原有: SysInfo.m_bl_Q1_H0 = front;
        }
    }

    /// <summary>
    /// TOFD 硬件适配器 — 封装 Tofd_DLL.CLTofd_BJ
    /// 注意: Tofd_DLL 引用在原有代码中被注释，需先恢复引用
    /// </summary>
    public class TofdHardwareAdapter : ITofdHardware
    {
        // 原有: private Tofd_DLL.CLTofd_BJ _dll = new Tofd_DLL.CLTofd_BJ();
        private object _dll;  // 占位

        public bool IsConnected { get; private set; }

        public bool Connect()
        {
            // 原有: bool ret = _dll.Link();
            // IsConnected = ret;
            return IsConnected;
        }

        public void Disconnect()
        {
            // 原有: _dll.Close();
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
            // 原有: return _dll.GetAData(channel);
            return new byte[0];
        }

        public byte[] AcquireDScan()
        {
            // 原有: return _dll.GetDData();
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
