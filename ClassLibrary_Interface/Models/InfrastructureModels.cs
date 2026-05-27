using System;
using System.Collections.Generic;
using System.Diagnostics;

// ============================================================
// 文件: InfrastructureModels.cs
// 位置: ClassLibrary_Interface/Models/
// 命名空间: ClassLibrary_Interface.Models
// 职责: 从 SysInfo.cs 迁移的小型数据模型和辅助类
//       - ClDog (加密狗心跳监控)
//       - Cl_Mark (打标位置记录)
//       - Cl_Print_Item / Cl_Print_Record (打印管理)
//       - XjGetData (寻迹数据)
//       - StartKeyBoard (系统键盘助手)
// ============================================================

namespace ClassLibrary_Interface.Models
{
    // ========================================================
    // ClDog — 加密狗/看门狗心跳监控
    // 替代: SysInfo.ClDog
    // ========================================================
    /// <summary>
    /// 加密狗心跳监控 — 替代 SysInfo.ClDog
    /// 监控 USB 加密狗通讯是否正常，超过门限时间没有心跳则报警
    /// </summary>
    public class ClDog
    {
        /// <summary>心跳计数</summary>
        public int AddCount { get; set; } = 0;

        /// <summary>上次计数</summary>
        public int OldCount { get; set; } = -1;

        /// <summary>异常状态: -1=初始 0=正常 1=有异常</summary>
        public int AlarmState { get; set; } = -1;

        /// <summary>异常发现起始时间</summary>
        public DateTime AlarmStartTime { get; set; } = DateTime.Now;

        /// <summary>异常持续时长门限 (秒)</summary>
        public int AlarmDurationThreshold { get; set; } = 4;

        /// <summary>是否触发加密狗异常报警</summary>
        public bool IsDogAlarm { get; set; } = false;
    }

    // ========================================================
    // Cl_Mark — 打标位置记录
    // 替代: SysInfo.Cl_Mark
    // ========================================================
    /// <summary>
    /// 打标位置记录 — 替代 SysInfo.Cl_Mark
    /// </summary>
    public class ClMark
    {
        /// <summary>X 轴打标位置 (mm)</summary>
        public float PositionX { get; set; } = 0;

        /// <summary>是否已在此位置打标</summary>
        public bool IsMarked { get; set; } = false;
    }

    // ========================================================
    // Cl_Print_Item / Cl_Print_Record — 打印管理
    // 替代: SysInfo.Cl_Print_Item / SysInfo.Cl_Print_Record
    // ========================================================
    /// <summary>
    /// 打印项目 — 替代 SysInfo.Cl_Print_Item
    /// </summary>
    public class ClPrintItem
    {
        /// <summary>项目名称</summary>
        public string ItemName { get; set; } = "";

        /// <summary>文件路径</summary>
        public string ItemPath { get; set; } = "";

        /// <summary>项目对应的焊缝检测记录</summary>
        public List<ClPrintRecord> Records { get; set; } = new List<ClPrintRecord>();
    }

    /// <summary>
    /// 打印记录 — 替代 SysInfo.Cl_Print_Record
    /// </summary>
    public class ClPrintRecord
    {
        /// <summary>是否选中当前记录</summary>
        public bool IsChecked { get; set; } = true;

        /// <summary>当前焊缝记录文件名</summary>
        public string RecordName { get; set; } = "";

        /// <summary>文件路径</summary>
        public string RecordPath { get; set; } = "";
    }

    // ========================================================
    // XjGetData — 寻迹数据
    // 替代: SysInfo.Class_Xj_GetData
    // ========================================================
    /// <summary>
    /// 寻迹数据 — 替代 SysInfo.Class_Xj_GetData
    /// 用于存储激光寻迹返回的原始数据和计算结果
    /// </summary>
    public class XjGetData
    {
        /// <summary>原始数据数组 (500点)</summary>
        public double[] DataArray { get; set; } = new double[500];

        /// <summary>深度值</summary>
        public double Depth { get; set; } = 0;

        /// <summary>中心像素坐标</summary>
        public int Center { get; set; } = 0;

        /// <summary>起始像素坐标</summary>
        public int Start { get; set; } = 0;

        /// <summary>结束像素坐标</summary>
        public int End { get; set; } = 0;

        /// <summary>串口数据帧</summary>
        public string Frame { get; set; } = "";
    }

    // ========================================================
    // StartKeyBoard — 系统键盘启动助手
    // 替代: SysInfo.StartKeyBoard
    // ========================================================
    /// <summary>
    /// 系统键盘助手 — 替代 SysInfo.StartKeyBoard
    /// </summary>
    public static class StartKeyBoard
    {
        /// <summary>键盘是否已显示</summary>
        public static bool IsShowing { get; set; } = false;

        /// <summary>启动自定义键盘</summary>
        public static void Show()
        {
            foreach (Process p in Process.GetProcessesByName("StartKeyBoard"))
            {
                p.Kill();
            }
            Process kbpr = new Process();
            kbpr.StartInfo.FileName = System.Windows.Forms.Application.StartupPath + "\\StartKeyBoard.exe";
            kbpr.Start();
            IsShowing = true;
        }

        /// <summary>关闭自定义键盘</summary>
        public static void Hide()
        {
            foreach (Process p in Process.GetProcessesByName("StartKeyBoard"))
            {
                p.Kill();
            }
            IsShowing = false;
        }
    }
}
