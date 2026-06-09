using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.NetworkInformation;

using ClassLibrary_Interface;
using NewInspect.Services;
using NewInspect.Services.Adapters;
using NewInspect.Services.State;

// ============================================================
// 文件: Program_New.cs
// 位置: Tofd_AWI/ (入口点，参考用 — 逐步替换原 Program.cs)
// 命名空间: Tofd_AWI
// 职责: 新框架的应用入口点 — DI 组装 + 启动
//       对比原来: Application.Run(new Frm_Main_C()) — 无注入
// ============================================================

namespace Tofd_AWI
{
    static class Program
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
            var systemState  = new SystemRuntimeState { Language = config.Language, WorkMode = config.WorkMode };
            var tofdState    = new TofdState { Gain = 5f };
            var cScanState   = new CScanState();
            var motionState  = new MotionState { MarkEnabled = false };
            var projectState = new InspectionProjectState();
            var reportState  = new ReportState();

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
            ITofdHardware     tofdHw    = new TofdHardwareAdapter();
            ICScanHardware    cScanHw   = null;
            IMotionController motionHw = new CanMotionControllerAdapter();

            // ==========================================
            // Step 6: 创建服务层 (替代原来塞在窗体里的业务代码)
            // ==========================================
            var tofdService   = new TofdService(tofdHw, cScanHw, tofdState, cScanState, config);
            var motionService = new MotionService(motionHw, motionState, config);
            var cameraService = new CameraService();

            // ==========================================
            // Step 6b: (已移除 — 显示组件改为在 Frm_Main_C_New 内部用设计器 PictureBox 创建)
            // ==========================================

            // ==========================================
            // Step 7: 启动主窗体 — 临时改为 Frm_NewInspect 预览新界面
            // ==========================================
            try
            {
                // 新界面预览 (CTSPA22S 参数面板 + 三栏布局 + 深色主题)
                var previewForm = new From.NewInspect.MainForm();
                Application.Run(previewForm);

                // 原启动代码 (保留，后续恢复)
                // var mainForm = new Frm_Main_C_New(
                //     config, systemState, tofdService, motionService,
                //     cameraService, tofdState, motionState, projectState, reportState);
                // Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"System error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }       
    }
}
