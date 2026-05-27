using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.NetworkInformation;

using ClassLibrary_Interface;
using Tofd_AWI.NewFramework.Adapters;
using Tofd_AWI.NewFramework.Services;
using Tofd_AWI.NewFramework.State;

// ============================================================
// 文件: Program_New.cs
// 位置: NewFramework/ (参考用，替换原 Program.cs)
// 职责: 新框架的应用入口点 — DI 组装 + 启动
// ============================================================

namespace Tofd_AWI
{
    static class Program_New
    {
        [STAThread]
        static void Main()
        {
            // ==========================================
            // Step 1: 防止重复运行 (保留原有逻辑)
            // ==========================================
            string processName = Process.GetCurrentProcess().ProcessName;
            if (Process.GetProcessesByName(processName).Length > 1)
            {
                MessageBox.Show(processName + " Already running!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ==========================================
            // Step 2: 加载配置 (替代 SysInfo 中散落的 INI 读取)
            // ==========================================
            var config = SystemConfig.Load();

            // ==========================================
            // Step 3: 创建运行时状态对象 (替代 SysInfo 的静态字段)
            // ==========================================
            var systemState   = new SystemRuntimeState { Language = config.Language, WorkMode = config.WorkMode };
            var tofdState     = new TofdState { Gain = config.SoundVelocity > 0 ? 5f : 5f };
            var cScanState    = new CScanState();
            var motionState   = new MotionState { MarkEnabled = false };
            var projectState  = new InspectionProjectState();
            var reportState   = new ReportState();

            // ==========================================
            // Step 4: 注册验证 (保留原有 MAC 地址 + 加密狗逻辑)
            // ==========================================
            // if (!ValidateLicense(config.NetworkIp))
            // {
            //     MessageBox.Show("License validation failed!", "Error");
            //     return;
            // }

            // ==========================================
            // Step 5: 创建硬件适配器 (替代 new Tofd_DLL / new CanCmd)
            // ==========================================
            ITofdHardware     tofdHw     = new TofdHardwareAdapter();
            ICScanHardware    cScanHw    = null;  // 按需创建
            IMotionController motionHw  = new CanMotionControllerAdapter();
            // ICameraDevice     camera     = new CameraAdapter();
            // IPowerMonitor     power      = new PowerMonitorAdapter();

            // ==========================================
            // Step 6: 创建服务层 (替代原来塞在窗体里的业务代码)
            // ==========================================
            var tofdService   = new TofdService(tofdHw, cScanHw, tofdState, cScanState, config);
            var motionService = new MotionService(motionHw, motionState, config);
            var cameraService = new CameraService();
            var dataService   = new DataService(config);

            // ==========================================
            // Step 6b: 创建显示组件 (视频 + 波形)
            // ==========================================
            var videoDisplayService    = new VideoDisplayService();     // 两路视频显示
            var waveformDisplayService = new WaveformDisplayService(); // A/B/C/D 波形显示

            // ==========================================
            // Step 7: 启动主窗体 (注入服务，而非直接访问 SysInfo)
            // ==========================================
            try
            {
                var mainForm = new Frm_Main_C_New(
                    config,
                    systemState,
                    tofdService,
                    motionService,
                    cameraService,
                    dataService,
                    videoDisplayService,
                    waveformDisplayService,
                    tofdState,
                    motionState,
                    projectState,
                    reportState);

                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"System error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // 辅助方法：网卡 MAC 获取 (保留原有逻辑)
        // ==========================================
        private static string GetMacAddress(string ip)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    var ipProps = adapter.GetIPProperties();
                    foreach (var addr in ipProps.UnicastAddresses)
                    {
                        if (addr.Address.ToString() == ip)
                            return adapter.GetPhysicalAddress().ToString();
                    }
                }
            }
            return "";
        }
    }
}
