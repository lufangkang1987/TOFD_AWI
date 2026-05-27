using System;
using System.Collections.Generic;
using System.Drawing;

// ============================================================
// 文件: IHardwareInterfaces.cs
// 位置: ClassLibrary_Interface 项目（新增）
// 职责: 定义所有硬件设备的抽象接口，让上层不直接依赖具体 DLL
// 说明: 第3步的核心产出，每个接口对应一个现有硬件模块
// ============================================================

namespace ClassLibrary_Interface
{
    // ============================
    // 1. TOFD 超声波检测硬件接口
    // 对应现有: Tofd_DLL (已注释)，SysInfo.m_SysBuff.Tofd_Buff
    // ============================
    public interface ITofdHardware
    {
        /// <summary>连接 TOFD 设备</summary>
        bool Connect();
        /// <summary>断开连接</summary>
        void Disconnect();
        /// <summary>是否已连接</summary>
        bool IsConnected { get; }

        /// <summary>设置增益 (dB)</summary>
        void SetGain(int channel, int db);
        /// <summary>获取当前增益</summary>
        int GetGain(int channel);

        /// <summary>设置声速 (m/s)</summary>
        void SetSoundVelocity(float velocity);
        /// <summary>设置脉冲宽度 (us)</summary>
        void SetPulseWidth(int us);
        /// <summary>设置采样范围 (us)</summary>
        void SetScanRange(int rangeUs);

        /// <summary>采集一帧 A 扫描数据 (通道号, 原始数据)</summary>
        byte[] AcquireAScan(int channel);
        /// <summary>采集一帧 D 扫描原始数据</summary>
        byte[] AcquireDScan();

        /// <summary>开始连续采集 (回调模式)</summary>
        void StartAcquisition(int channel, Action<int, byte[]> onData);
        /// <summary>停止连续采集</summary>
        void StopAcquisition();
    }

    // ============================
    // 2. C-Scan 扫描硬件接口
    // 对应现有: Tofd_DLL.Cls_C_Scan (已注释)
    // ============================
    public interface ICScanHardware
    {
        bool Connect();
        void Disconnect();
        bool IsConnected { get; }

        /// <summary>采集 C 扫描一帧数据</summary>
        byte[] AcquireFrame();
        /// <summary>设置扫描参数</summary>
        void SetParameters(int gain, int range, int channelCount);
    }

    // ============================
    // 3. 运动控制接口 (爬行器/4轮车体)
    // 对应现有: Clb_MT_Comm.CanCmd + MT_Comm
    // 调用方: Frm_Move.cs, Frm_Main_C.cs
    // ============================
    public interface IMotionController
    {
        /// <summary>连接 (COM 串口 或 CAN 总线)</summary>
        bool Connect(string portOrCan, bool isComNotCan);
        /// <summary>断开连接</summary>
        void Disconnect();
        /// <summary>是否已连接</summary>
        bool IsConnected { get; }

        /// <summary>设置速度 (百分比 0~100)</summary>
        void SetSpeed(int percent);
        /// <summary>获取当前速度</summary>
        int GetSpeed();

        /// <summary>前进</summary>
        void MoveForward();
        /// <summary>后退</summary>
        void MoveBackward();
        /// <summary>停止</summary>
        void Stop();

        /// <summary>获取当前距离 (mm)</summary>
        float GetDistance();
        /// <summary>获取实时速度</summary>
        float GetCurrentSpeed();

        // ---- 光栅臂控制 ----
        /// <summary>光栅臂抬起/落下 (true:抬起 false:落下)</summary>
        void SetGratingArm(bool up);
        /// <summary>设置光栅臂运行区间 (起/止 mm)</summary>
        void SetGratingRange(int startPos, int endPos);
        /// <summary>光栅臂纠偏: 0:左 1:右 2:停止</summary>
        void SetGratingCorrection(int direction);

        // ---- 缺陷打标 ----
        /// <summary>执行打标动作</summary>
        void MarkDefect();
        /// <summary>打标使能</summary>
        bool MarkEnabled { get; set; }

        // ---- 灯光控制 ----
        /// <summary>灯光位置: true=前 false=后</summary>
        void SetLightPosition(bool front);

        // ---- 通讯类型 ----
        /// <summary>当前通讯类型: 0=CAN  1=COM</summary>
        int CommType { get; }
    }

    // ============================
    // 4. 工业相机接口
    // 对应现有: Clb_XmCam_DLL, MVSDK
    // ============================
    public interface ICameraDevice
    {
        /// <summary>根据 IP 打开相机</summary>
        bool Open(string ipAddress);
        /// <summary>关闭相机</summary>
        void Close();
        /// <summary>是否已打开</summary>
        bool IsOpen { get; }

        /// <summary>抓取一帧</summary>
        Bitmap Capture();
        /// <summary>开始视频流 (回调)</summary>
        void StartStream(Action<Bitmap> onFrame);
        /// <summary>停止视频流</summary>
        void StopStream();
        /// <summary>获取相机 IP</summary>
        string IpAddress { get; }
    }

    // ============================
    // 5. 激光轮廓仪接口 (寻迹)
    // 对应现有: HD850_64, C_ProfileDll64
    // ============================
    public interface ILaserProfiler
    {
        bool Connect(string port);
        void Disconnect();
        bool IsConnected { get; }

        /// <summary>获取轮廓数据 (X[], Z[])</summary>
        (float[] x, float[] z) GetProfile();
        /// <summary>获取当前温度</summary>
        int GetTemperature();
        /// <summary>设置曝光时间</summary>
        void SetExposureTime(int us);
    }

    // ============================
    // 6. 电源/电池监控接口
    // 对应现有: Cmm_PcPower
    // ============================
    public interface IPowerMonitor
    {
        /// <summary>获取电池电量百分比</summary>
        int GetBatteryPercent();
        /// <summary>是否正在充电</summary>
        bool IsCharging { get; }
    }

    // ============================
    // 7. 涡流检测接口
    // 对应现有: ECT_DLL
    // ============================
    public interface IEddyCurrentDevice
    {
        bool Connect();
        void Disconnect();
        float[] ReadSignal();
        void StartScan(Action<float[]> onData);
        void StopScan();
    }
}
