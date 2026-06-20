using ClassLibrary_Interface;
using NewInspect.Services.State;
using System;
using System.Threading;

// ============================================================
// 文件: TofdService.cs
// 位置: NewInspect.Services/Services/
// 命名空间: NewInspect.Services
// 职责: TOFD / C-Scan 检测业务逻辑
//       替代 Frm_Main_C.cs 和 Frm_TOFD.cs 中的超声采集和参数控制逻辑
// ============================================================

namespace NewInspect.Services
{
    /// <summary>
    /// TOFD 超声检测服务
    /// 原来这些逻辑散落在 Frm_Main_C 和各种按钮事件中
    /// </summary>
    public class TofdService
    {
        private readonly ITofdHardware _hardware;
        private readonly ICScanHardware _cScanHardware;
        private readonly TofdState _tofdState;
        private readonly CScanState _cScanState;
        private readonly SystemConfig _config;

        // 采集线程 (原 Thread_A_Wave, Thread_D_Wave)
        private Thread _aScanThread;
        private Thread _dScanThread;
        private bool _isRunning;
        private bool _isConnected;

        /// <summary>TOFD 设备连接状态</summary>
        public bool IsConnected => _isConnected;

        public TofdService(
            ITofdHardware hardware,
            ICScanHardware cScanHardware,
            TofdState tofdState,
            CScanState cScanState,
            SystemConfig config)
        {
            _hardware = hardware ?? throw new ArgumentNullException(nameof(hardware));
            _cScanHardware = cScanHardware;
            _tofdState = tofdState ?? throw new ArgumentNullException(nameof(tofdState));
            _cScanState = cScanState ?? throw new ArgumentNullException(nameof(cScanState));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        // ========== TOFD 操作 ==========

        /// <summary>连接 TOFD 设备 (对应 Bt_Link_Click)</summary>
        public bool Connect()
        {
            bool ok = _hardware.Connect();
            _tofdState.IsLinked = ok;
            return ok;
        }

        /// <summary>断开 TOFD 设备</summary>
        public void Disconnect()
        {
            _hardware.Disconnect();
            _tofdState.IsLinked = false;
        }

        /// <summary>设置增益 (对应 Track_Gain 等控件事件)</summary>
        public void SetGain(int db)
        {
            _tofdState.Gain = db;
            _hardware.SetGain(0, db);
        }

        /// <summary>设置声速</summary>
        public void SetSoundVelocity(float velocity)
        {
            _hardware.SetSoundVelocity(velocity);
        }

        /// <summary>设置采样范围</summary>
        public void SetScanRange(int rangeUs)
        {
            _tofdState.ExposureTime = rangeUs;
            _hardware.SetScanRange(rangeUs);
        }

        /// <summary>启动 A 扫描连续采集</summary>
        public void StartAScan(Action<int, byte[]> onData)
        {
            if (_isRunning) return;
            _isRunning = true;

            _aScanThread = new Thread(() =>
            {
                while (_isRunning)
                {
                    try
                    {
                        var data = _hardware.AcquireAScan(0);
                        onData?.Invoke(0, data);
                        Thread.Sleep(50);  // 20Hz
                    }
                    catch { break; }
                }
            })
            { IsBackground = true };
            _aScanThread.Start();
        }

        /// <summary>停止 A 扫描</summary>
        public void StopAScan()
        {
            _isRunning = false;
            _hardware.StopAcquisition();
        }

        /// <summary>采集一帧 D 扫描数据</summary>
        public byte[] AcquireDScan()
        {
            return _hardware.AcquireDScan();
        }

        // ========== C-Scan 操作 ==========

        /// <summary>连接 C-Scan 设备</summary>
        public bool ConnectCScan()
        {
            if (_cScanHardware == null) return false;
            return _cScanHardware.Connect();
        }

        /// <summary>C 扫描采集一帧</summary>
        public byte[] AcquireCFrame()
        {
            if (_cScanHardware == null) return null;
            return _cScanHardware.AcquireFrame();
        }

        // ========== 参数计算 (原 Frm_Tofd_Calcu) ==========

        /// <summary>计算 TOFD 探头中心距和深度</summary>
        public (float depth, float surfaceDistance) CalculateDepth(
            float timeOfFlight,
            float soundVelocity,
            float probeCenterDistance)
        {
            float halfPCS = probeCenterDistance / 2f;
            float halfPath = soundVelocity * timeOfFlight / 2f;
            float depth = (float)Math.Sqrt(Math.Max(0, halfPath * halfPath - halfPCS * halfPCS));
            float surfaceDist = (float)Math.Sqrt(Math.Max(0,
                halfPath * halfPath - depth * depth));
            return (depth, surfaceDist);
        }
    }
}
