using System;
using System.Windows.Forms;

// ============================================================
// 文件: SystemConfig.cs
// 位置: ClassLibrary_Interface 项目（新增）
// 命名空间: ClassLibrary_Interface
// 职责: 替代 SysInfo 中所有 INI 配置读取逻辑
//       从 SysConfig.ini / HardConfig.ini 一次性加载，不可变
// ============================================================

namespace ClassLibrary_Interface
{
    /// <summary>
    /// 系统配置 — 替代 SysInfo 中散落的配置字段
    /// 设计原则：只读、从 INI 加载一次、不持有运行时状态
    /// </summary>
    public class SystemConfig
    {
        // 依赖现有的 INI 读写工具（ClassInterFace）
        private readonly ClassInterFace _ini = new ClassInterFace();

        // ========== 基础系统配置 ==========

        /// <summary>语言: 0=中文 1=English</summary>
        public int Language { get; private set; } = 0;

        /// <summary>工作模式: 0=TOFD 1=CScan 2=M_UI 3=Coating</summary>
        public int WorkMode { get; private set; } = 0;

        /// <summary>超声探头厂家: 0=武汉中科 1=北京六维远光</summary>
        public int UltrasoundType { get; private set; } = 0;

        /// <summary>是否使用激光寻迹: 1=启用</summary>
        public int LaserTrackingEnabled { get; private set; } = 1;

        /// <summary>北京六维远光采集时间 (us) 默认10</summary>
        public int CScanRangeUs { get; private set; } = 10;

        /// <summary>系统键盘: 1=打开 0=关闭</summary>
        public int OnScreenKeyboard { get; private set; } = 1;

        // ========== NETWORK / REGISTRATION ==========

        /// <summary>网卡 IP 地址</summary>
        public string NetworkIp { get; private set; } = "192.168.1.10";

        /// <summary>注册密码</summary>
        public string RegistrationPass { get; private set; } = "";

        // ========== 运动控制配置 ==========

        /// <summary>光栅臂最小间距 (mm)</summary>
        public int GratingMinDistance { get; private set; } = 5;

        /// <summary>当前速度百分比</summary>
        public int SpeedPercent { get; private set; } = 50;

        /// <summary>光栅臂速度</summary>
        public int GratingSpeed { get; private set; } = 50;

        // ========== 通讯配置 ==========

        /// <summary>通讯方式: 0=CAN 1=COM</summary>
        public int CommMode { get; private set; } = 1;

        /// <summary>串口号</summary>
        public string CommPort { get; private set; } = "COM3";

        /// <summary>移动增量地址</summary>
        public int MoveAddress { get; private set; } = 1;

        // ========== 检测条件配置 ==========

        /// <summary>TOFD 声速 m/s</summary>
        public float SoundVelocity { get; private set; } = 5900;

        /// <summary>探头中心距 mm</summary>
        public float ProbeCenterDistance { get; private set; } = 50;

        /// <summary>探头角度 (度)</summary>
        public float ProbeAngle { get; private set; } = 60;

        // ========== 文件路径 ==========

        /// <summary>系统主配置文件</summary>
        public static string SysConfigPath =>
            Application.StartupPath + "\\database\\SysConfig.ini";

        /// <summary>硬件配置文件</summary>
        public static string HardConfigPath =>
            Application.StartupPath + "\\database\\HardConfig.ini";

        /// <summary>数据库路径</summary>
        public string DatabasePath =>
            Application.StartupPath + "\\database\\";

        // ========================================================
        // 加载方法
        // ========================================================

        /// <summary>从 INI 文件加载系统配置</summary>
        public static SystemConfig Load()
        {
            var cfg = new SystemConfig();
            cfg.Reload();
            return cfg;
        }

        /// <summary>重新从 INI 文件读取（当用户修改配置后调用）</summary>
        public void Reload()
        {
            string sysPath = SysConfigPath;
            string hardPath = HardConfigPath;

            // --- 系统配置 ---
            Language      = ParseInt(_ini.IniReadDefine("System", "m_iLanguage", "0", sysPath));
            WorkMode      = ParseInt(_ini.IniReadDefine("System", "m_i_TOFD_0_Cscan_1", "0", sysPath));
            UltrasoundType = ParseInt(_ini.IniReadDefine("SysInfo", "m_i_UI_Type", "0", sysPath));
            LaserTrackingEnabled = ParseInt(_ini.IniReadDefine("SysInfo", "m_i_Alarm", "1", sysPath));
            CScanRangeUs  = ParseInt(_ini.IniReadDefine("ClassUltrasGate", "m_iChScanRange", "10", sysPath));

            // --- 通讯 ---
            CommMode  = ParseInt(_ini.IniReadDefine("COM_Can", "m_blCom1_Can0", "1", sysPath));
            CommPort  = _ini.IniReadDefine("COM_Can", "COM", "COM3", sysPath);
            MoveAddress = ParseInt(_ini.IniReadDefine("COM_Can", "m_i_Move_Add", "1", sysPath));

            // --- 运动 ---
            SpeedPercent     = ParseInt(_ini.IniReadDefine("SYSinfo", "iSpeed_Xs", "50", sysPath));
            GratingSpeed     = ParseInt(_ini.IniReadDefine("SYSinfo", "i_Para_Speed_Gsb", "50", sysPath));
            GratingMinDistance = ParseInt(_ini.IniReadDefine("SYSinfo", "m_UI_Gsb_Distan_Min", "5", sysPath));

            // --- 网络 ---
            NetworkIp = _ini.IniReadDefine("IP", "IP", "192.168.1.10", sysPath);

            // --- 注册 ---
            RegistrationPass = _ini.IniReadDefine("RegEdidt", "strPass", "", sysPath);

            // --- TOFD 参数 ---
            SoundVelocity = ParseFloat(_ini.IniReadDefine("TOFD", "SoundVelocity", "5900", hardPath));
            ProbeCenterDistance = ParseFloat(_ini.IniReadDefine("TOFD", "PCS", "50", hardPath));
            ProbeAngle = ParseFloat(_ini.IniReadDefine("TOFD", "ProbeAngle", "60", hardPath));
        }

        // ========================================================
        // 保存方法（仅部分可写字段）
        // ========================================================

        /// <summary>保存语言设置</summary>
        public void SaveLanguage(int lang)
        {
            _ini.INIWriteValue("System", "m_iLanguage", lang.ToString(), SysConfigPath);
            Language = lang;
        }

        /// <summary>保存工作模式</summary>
        public void SaveWorkMode(int mode)
        {
            _ini.INIWriteValue("System", "m_i_TOFD_0_Cscan_1", mode.ToString(), SysConfigPath);
            WorkMode = mode;
        }

        /// <summary>保存通讯设置</summary>
        public void SaveCommSettings(int mode, string port)
        {
            _ini.INIWriteValue("COM_Can", "m_blCom1_Can0", mode.ToString(), SysConfigPath);
            _ini.INIWriteValue("COM_Can", "COM", port, SysConfigPath);
            CommMode = mode;
            CommPort = port;
        }

        // ========================================================
        // 工具方法
        // ========================================================

        private static int ParseInt(string s, int fallback = 0)
        {
            return int.TryParse(s, out int v) ? v : fallback;
        }

        private static float ParseFloat(string s, float fallback = 0)
        {
            return float.TryParse(s, out float v) ? v : fallback;
        }
    }
}
