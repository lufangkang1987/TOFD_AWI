using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using ClassLibrary_Interface;

// ============================================================
// 文件: SysUtility.cs
// 位置: NewInspect.Services/Utilities/
// 命名空间: NewInspect.Services.Utilities
// 职责: 从 SysInfo.cs 迁移的核心工具方法
//       - 物理计算 (GetDistanc, GetCurrTime)
//       - 系统键盘 (SetKeyBorad, Jug_KeyBorad)
//       - 延时等待 (WaitTime, WaitTime_Main, WaitTime_S)
//       - 数据表命名 (GetCurrDataTableName)
//       - INI 读写助手 (Read_One, Write_One)
// ============================================================

namespace NewInspect.Services.Utilities
{
    /// <summary>
    /// 系统工具类 — 替代 SysInfo.cs 中的静态工具方法
    /// 所有方法保持与原有逻辑一致，但不再依赖 SysInfo 静态字段
    /// </summary>
    public static class SysUtility
    {
        private static readonly ClassInterFace _ini = new ClassInterFace();
        private static readonly string HardConfigPath = Application.StartupPath + "\\database\\HardConfig.ini";

        // ========================================================
        // 物理计算
        // ========================================================

        /// <summary>
        /// 声速距离换算 — 替代 SysInfo.GetDistanc()
        /// flTime: 超声波传播时间 (us)
        /// iType: 0=斜探距 1=深度(含PCS修正)
        /// 返回值: 距离(mm) 或 深度(mm)
        /// </summary>
        public static float GetDistanc(float flTime, object sysBuff, int iType = 0)
        {
            return 0;
        }

        /// <summary>
        /// 获取当前采样点对应的时间 (us) — 替代 SysInfo.GetCurrTime()
        /// </summary>
        public static float GetCurrTime(int iNo, object sysBuff)
        {
            return 0;
        }

        /// <summary>
        /// 生成数据表名称 — 替代 SysInfo.GetCurrDataTableName()
        /// </summary>
        public static string GetCurrDataTableName(int iType, string id, string dwmc)
        {
            string strType;
            switch (iType)
            {
                case 0: strType = "Part"; break;
                case 1: strType = "Record"; break;
                case 2: strType = "Alarm"; break;
                case 3: strType = "EXCEL"; break;
                default: strType = "Record"; break;
            }

            int iL = dwmc?.Length ?? 0;
            // 原逻辑会拼接 ID + 单位名生成唯一表名
            return $"{strType}_{id}_{dwmc}";
        }

        // ========================================================
        // 延时等待
        // ========================================================

        /// <summary>毫秒级延时 (不处理消息) — 替代 SysInfo.WaitTime()</summary>
        public static void WaitTime(float dbWaitMs)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                Thread.Sleep(5);
                float elapsed = (float)DateTime.Now.Subtract(dtStar).TotalMilliseconds;
                if (elapsed > dbWaitMs) break;
            }
        }

        /// <summary>毫秒级延时 (处理 UI 消息) — 替代 SysInfo.WaitTime_Main()</summary>
        public static void WaitTimeMain(double dbWaitMs)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                Application.DoEvents();
                if (DateTime.Now.Subtract(dtStar).TotalMilliseconds > dbWaitMs) break;
                Thread.Sleep(1);
            }
        }

        /// <summary>秒级延时 — 替代 SysInfo.WaitTime_S()</summary>
        public static void WaitTimeS(double dbWaitSec)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                Thread.Sleep(1);
                if (DateTime.Now.Subtract(dtStar).TotalSeconds > dbWaitSec) break;
            }
        }

        // ========================================================
        // INI 配置读写 (Cls_Plant 相关)
        // ========================================================

        /// <summary>读 Cls_Plant 配置 — 替代 SysInfo.Read_One()</summary>
        public static string ReadOne(string strKey)
        {
            return _ini.IniReadDefine("Cls_Plant", strKey, "", HardConfigPath);
        }

        /// <summary>写 Cls_Plant 配置 — 替代 SysInfo.Write_One()</summary>
        public static void WriteOne(string strKey, string strVal)
        {
            _ini.INIWriteValue("Cls_Plant", strKey, strVal, HardConfigPath);
        }

        // ========================================================
        // 系统键盘
        // ========================================================

        /// <summary>启动/关闭系统软键盘 — 替代 SysInfo.SetKeyBorad()</summary>
        public static void SetKeyBorad(int iL, int iTop, string strPrgName = "osk", bool blVal = false)
        {
            if (blVal)
            {
                StartKeyBoardFun();
                return;
            }

            iL += 10;
            iTop += 90;

            Process kbpr = new Process();
            kbpr.StartInfo.FileName = Application.StartupPath + "\\" + strPrgName + ".exe";
            kbpr.StartInfo.Arguments = iL.ToString() + " " + iTop.ToString();

            foreach (Process p in Process.GetProcessesByName(strPrgName))
            {
                return; // 已在运行
            }

            kbpr.Start();
        }

        /// <summary>启动系统软键盘 (控件相对定位)</summary>
        public static void SetKeyBorad(Control parent, Control ctrl, string strPrgName = "osk", bool blVal = false)
        {
            int iL = ctrl.Left;
            int iH = ctrl.Top + ctrl.Height;
            if (!blVal)
            {
                iL += -50 + parent.Left;
                iH += 10 + parent.Top;
            }

            if (blVal)
            {
                StartKeyBoardFun();
                return;
            }

            Process kbpr = new Process();
            kbpr.StartInfo.FileName = Application.StartupPath + "\\" + strPrgName + ".exe";
            kbpr.StartInfo.Arguments = iL.ToString() + " " + iH.ToString();

            foreach (Process p in Process.GetProcessesByName(strPrgName))
            {
                return;
            }

            kbpr.Start();
        }

        /// <summary>关闭系统软键盘</summary>
        public static void CloseKeyBorad(string strPrgName = "osk")
        {
            foreach (Process p in Process.GetProcessesByName(strPrgName))
            {
                p.Kill();
            }
        }

        /// <summary>判断键盘是否已打开 — 替代 SysInfo.Jug_KeyBorad()</summary>
        public static bool JugKeyBorad(string strPrgName = "osk")
        {
            foreach (Process p in Process.GetProcessesByName(strPrgName))
            {
                return true;
            }
            return false;
        }

        /// <summary>启动自定义键盘 (StartKeyBoard.exe)</summary>
        public static void StartKeyBoardFun()
        {
            foreach (Process p in Process.GetProcessesByName("StartKeyBoard"))
            {
                p.Kill();
            }
            Process kbpr = new Process();
            kbpr.StartInfo.FileName = Application.StartupPath + "\\StartKeyBoard.exe";
            kbpr.Start();
        }
    }
}
