using System;
using System.Collections.Generic;
using System.Drawing;

// ============================================================
// 文件: IHardwareInterfaces.cs
// 位置: ClassLibrary_Interface 项目（新增）
// 命名空间: ClassLibrary_Interface
// 职责: 定义所有硬件设备的抽象接口，让上层不直接依赖具体 DLL
// ============================================================

namespace ClassLibrary_Interface
{
    // ============================
    // 1. TOFD 超声波检测硬件接口
    // 对应现有: Tofd_DLL，SysInfo.m_SysBuff.Tofd_Buff
    // ============================
    public interface ITofdHardware
    {
        bool Connect();
        void Disconnect();
        bool IsConnected { get; }

        void SetGain(int channel, int db);
        int GetGain(int channel);

        void SetSoundVelocity(float velocity);
        void SetPulseWidth(int us);
        void SetScanRange(int rangeUs);

        byte[] AcquireAScan(int channel);
        byte[] AcquireDScan();

        void StartAcquisition(int channel, Action<int, byte[]> onData);
        void StopAcquisition();
    }

    // ============================
    // 2. C-Scan 扫描硬件接口
    // 对应现有: Tofd_DLL.Cls_C_Scan
    // ============================
    public interface ICScanHardware
    {
        bool Connect();
        void Disconnect();
        bool IsConnected { get; }

        byte[] AcquireFrame();
        void SetParameters(int gain, int range, int channelCount);
    }

    // ============================
    // 3. 运动控制接口 (爬行器/4轮车体)
    // 对应现有: Clb_MT_Comm.CanCmd + MT_Comm
    // 调用方: Frm_Move.cs, Frm_Main_C.cs
    // ============================
    public interface IMotionController
    {
        bool Connect(string portOrCan, bool isComNotCan);
        void Disconnect();
        bool IsConnected { get; }

        void SetSpeed(int percent);
        int GetSpeed();

        void MoveForward();
        void MoveBackward();
        void Stop();

        float GetDistance();
        float GetCurrentSpeed();

        // ---- 光栅臂控制 ----
        void SetGratingArm(bool up);
        void SetGratingRange(int startPos, int endPos);
        void SetGratingCorrection(int direction);

        // ---- 缺陷打标 ----
        void MarkDefect();
        bool MarkEnabled { get; set; }

        // ---- 灯光控制 ----
        void SetLightPosition(bool front);

        // ---- 通讯类型 ----
        int CommType { get; }
    }

    // ============================
    // 4. 工业相机接口
    // 对应现有: Clb_XmCam_DLL, MVSDK
    // ============================
    public interface ICameraDevice
    {
        bool Open(string ipAddress);
        void Close();
        bool IsOpen { get; }

        Bitmap Capture();
        void StartStream(Action<Bitmap> onFrame);
        void StopStream();
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

        (float[] x, float[] z) GetProfile();
        int GetTemperature();
        void SetExposureTime(int us);
    }

    // ============================
    // 6. 电源/电池监控接口
    // 对应现有: Cmm_PcPower
    // ============================
    public interface IPowerMonitor
    {
        int GetBatteryPercent();
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
