using Frame_Work;
using ClassLib_TestData;
using System.Collections.Generic;

// ============================================================
// 文件: RuntimeState.cs
// 位置: NewInspect.Services/State/
// 命名空间: NewInspect.Services.State
// 职责: 拆分 SysInfo 的运行时状态 — 替代原来 290KB 的静态大杂烩
//       每个状态类职责单一，不依赖窗体，可独立单元测试
//
// 2025-05-26: 补充 SysInfo.cs 遗漏字段 (寻迹、打标、相机、显示配置等)
// ============================================================

namespace NewInspect.Services.State
{
    /// <summary>
    /// TOFD 检测运行状态
    /// 替代: SysInfo 中 m_SysBuff、m_fl_Gain、m_i_UI_Type 等
    /// </summary>
    public class TofdState
    {
        /// <summary>系统缓存 (保留原有 Frame_Work 数据结构)</summary>
        public Frame_Work.ClassTofd_Buff SysBuff { get; } = new Frame_Work.ClassTofd_Buff();

        /// <summary>是否已连接 TOFD 设备</summary>
        public bool IsLinked { get; set; }

        /// <summary>当前增益值</summary>
        public float Gain { get; set; } = 5f;

        /// <summary>曝光时间 (us)</summary>
        public int ExposureTime { get; set; } = 10;

        /// <summary>探头厂家类型: 0=武汉中科 1=北京六维远光</summary>
        public int ProbeType { get; set; } = 0;

        /// <summary>是否 4 路相机模式 (SysInfo.m_bl_Qhzy)</summary>
        public bool FourCameraMode { get; set; } = false;

        /// <summary>A 扫描图像高度 (SysInfo.m_i_Pic_A_Height)</summary>
        public int AScanImageHeight { get; set; } = 0;

        /// <summary>焊缝算法编号 (SysInfo.m_Ck_First)</summary>
        public int WeldAlgorithm { get; set; } = 1;

        /// <summary>通讯帧率 (SysInfo.m_i_Frame_Num)</summary>
        public int FrameRate { get; set; } = 100;

        // ==== 补充: SysInfo 遗漏字段 ====

        /// <summary>是否显示视频 (SysInfo.m_bl_Show_Video)</summary>
        public bool ShowVideo { get; set; } = false;

        /// <summary>打标滞后距离 mm (SysInfo.m_fl_MarkLag)</summary>
        public float MarkLag { get; set; } = 0;

        /// <summary>打标滞后等待时间 s (SysInfo.m_i_MarkLag_WaitTime)</summary>
        public int MarkLagWaitTime { get; set; } = 2;

        /// <summary>打标滞后辅助距离 (SysInfo.m_fl_MarkLag_m)</summary>
        public float MarkLagM { get; set; } = 0;

        /// <summary>打标滞后限值 mm (SysInfo.m_fl_MarkLag_Limit)</summary>
        public float MarkLagLimit { get; set; } = 10;

        /// <summary>色标颜色定义 (SysInfo.m_str_B_Stand_Color)</summary>
        public string StandardColor { get; set; } = "";

        /// <summary>色标颜色定义备用 (SysInfo.m_str_B_Stand_Color_A)</summary>
        public string StandardColorA { get; set; } = "";

        /// <summary>图像显示高度 (SysInfo.m_flPic_Height)</summary>
        public float DisplayHeight { get; set; } = 30;

        /// <summary>是否进行图像计算 (SysInfo.m_bl_Img_Calcu)</summary>
        public bool ImageCalculation { get; set; } = false;

        /// <summary>激光日志开关 (SysInfo.m_i_Lars_RiZhi)</summary>
        public int LaserLogging { get; set; } = 0;
    }

    /// <summary>
    /// C-Scan 检测运行状态
    /// 替代: SysInfo.m_SysBuff_C、m_C_Item_Info 等
    /// </summary>
    public class CScanState
    {
        /// <summary>C 扫描数据缓存</summary>
        public Class_C_Buff DataBuff { get; } = new Class_C_Buff();

        /// <summary>C 扫描项目信息</summary>
        public Class_C_ItemInfor ItemInfo { get; } = new Class_C_ItemInfor();

        /// <summary>采集间隔等待时间</summary>
        public int WaitTimeMs { get; set; } = 0;
    }

    /// <summary>
    /// 运动控制运行状态
    /// 替代: SysInfo 中 m_SysBuff.m_Climb 相关字段
    /// </summary>
    public class MotionState
    {
        /// <summary>车体是否在线</summary>
        public bool IsOnline { get; set; }

        /// <summary>当前速度百分比</summary>
        public int SpeedPercent { get; set; } = 50;

        /// <summary>当前状态: 0=停止 1=前进 2=后退</summary>
        public int Direction { get; set; } = 0;

        /// <summary>当前 X 轴距离 (mm)</summary>
        public float DistanceX { get; set; }

        /// <summary>当前 Y 轴距离 (mm)</summary>
        public float DistanceY { get; set; }

        /// <summary>光栅臂状态: 0=落下 1=抬起</summary>
        public int GratingArmState { get; set; } = 0;

        /// <summary>光栅臂运行起/止位置 (mm)</summary>
        public int GratingStartPos { get; set; }
        public int GratingEndPos { get; set; }

        /// <summary>光栅臂总长 (mm)</summary>
        public int GratingTotalLength { get; set; }

        /// <summary>光栅臂步进间隔 (mm)</summary>
        public int GratingStepInterval { get; set; } = 5;

        /// <summary>光栅臂纠偏: 1=左 2=右 3=停止</summary>
        public int GratingCorrectionDir { get; set; } = 3;

        /// <summary>打标使能</summary>
        public bool MarkEnabled { get; set; }

        /// <summary>涂层/测厚步进</summary>
        public int CoatingStepInterval { get; set; }

        // ==== 补充: SysInfo 遗漏字段 ====

        /// <summary>PC 控制爬行器 (SysInfo.m_bl_ClimbRun_By_PC)</summary>
        public bool ClimbRunByPc { get; set; } = false;

        /// <summary>手柄/自动: 0=手柄 1=自动 (SysInfo.m_i_Climb_Hand0_Auto1)</summary>
        public int ControlMode { get; set; } = 0;

        /// <summary>手柄连续控制标志 (SysInfo.m_i_Hand_Do)</summary>
        public int HandControlActive { get; set; } = 0;

        /// <summary>是否正在打标 (SysInfo.m_bl_Mark)</summary>
        public bool IsMarking { get; set; } = false;

        /// <summary>打标延迟距离 (SysInfo.m_i_Add_MarkDisc)</summary>
        public int MarkDelayDistance { get; set; } = 100;

        /// <summary>编码器类型 (SysInfo.m_SysBuff.m_Climb.iBmq_Type)</summary>
        public int EncoderType { get; set; } = 0;

        /// <summary>按键延时 ms (SysInfo.Time_Key_Down)</summary>
        public int KeyDownTime { get; set; } = 10;

        /// <summary>CAN 刷屏间隔 ms (SysInfo.m_i_Can_BrushTime)</summary>
        public int CanBrushTime { get; set; } = 20;
    }

    /// <summary>
    /// 检测项目状态
    /// 替代: SysInfo 中 m_W_strItemName、m_W_strWeldID 等项目相关字段
    /// </summary>
    public class InspectionProjectState
    {
        /// <summary>项目文件夹名</summary>
        public string ItemName { get; set; } = "";

        /// <summary>上次项目名 (用于切换比较)</summary>
        public string ItemNameOld { get; set; } = "-1";

        /// <summary>焊缝编号</summary>
        public string WeldId { get; set; } = "";

        /// <summary>焊缝 ID 列表</summary>
        public List<string> WeldIdList { get; } = new List<string>();

        /// <summary>是否新项目</summary>
        public bool IsNewItem { get; set; } = false;

        /// <summary>项目标识符</summary>
        public string ItemMark { get; set; } = "_@@";

        // ==== 补充: SysInfo 遗漏字段 ====

        /// <summary>数据文件路径 (SysInfo.m_W_i_FilePath)</summary>
        public string FilePath { get; set; } = System.Windows.Forms.Application.StartupPath + "\\Temp";

        /// <summary>焊口日志文件 (SysInfo.m_Log_Weld)</summary>
        public string WeldLogPath { get; set; } = System.Windows.Forms.Application.StartupPath + "\\m_Log_Weld.ini";

        /// <summary>保存类型: 1=单文件 0=Web (SysInfo.m_W_i_SaveType_1One_0Web)</summary>
        public int SaveType { get; set; } = 0;

        /// <summary>最大记录数 (SysInfo.m_iA_Dellon_MaxNum)</summary>
        public int MaxRecordCount { get; set; } = 11;

        /// <summary>检测开始时间 (SysInfo.m_strStart_Time)</summary>
        public string StartTime { get; set; } = "";

        /// <summary>检测结束时间 (SysInfo.m_strEnd_Time)</summary>
        public string EndTime { get; set; } = "";

        /// <summary>设备编号列表 (SysInfo.m_strArrGjbh)</summary>
        public string DeviceNumbers { get; set; } = "";

        /// <summary>子文件名称 (SysInfo.m_strSaveDataFileName)</summary>
        public string SaveDataFileName { get; set; } = "";
    }

    /// <summary>
    /// 系统全局运行状态
    /// 替代: SysInfo 中 m_iLanguage、m_i_TOFD_0_Cscan_1 等全局字段
    /// </summary>
    public class SystemRuntimeState
    {
        /// <summary>当前界面语言: 0=中文 1=English</summary>
        public int Language { get; set; } = 0;

        /// <summary>当前工作模式: 0=TOFD 1=CScan 2=M_UI 3=Coating</summary>
        public int WorkMode { get; set; } = 0;

        /// <summary>是否已保存数据</summary>
        public bool IsSaved { get; set; } = false;

        /// <summary>录像状态: 1=开始 0=结束</summary>
        public int VideoRecording { get; set; } = 1;

        /// <summary>结论是否通过 (SysInfo.m_bl_JL)</summary>
        public bool Conclusion { get; set; } = false;

        /// <summary>结论信息 (SysInfo.m_str_JL)</summary>
        public string ConclusionInfo { get; set; } = "";

        /// <summary>激光温度</summary>
        public int LaserTemperature { get; set; } = 0;

        /// <summary>寻迹是否自动</summary>
        public bool TrackAuto { get; set; } = false;

        /// <summary>报表类型: 0=新报表 1=自定义 2=厚度列表</summary>
        public int ReportType { get; set; } = 0;

        // ==== 补充: SysInfo 遗漏字段 ====

        /// <summary>旧工作模式 (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2_Old)</summary>
        public int WorkModeOld { get; set; } = 0;

        /// <summary>运行提示信息 (SysInfo.m_strRunTitl)</summary>
        public string RunTip { get; set; } = "";

        /// <summary>A 扫鼠标提示 (SysInfo.m_str_A_Tiltl)</summary>
        public string AScanTooltip { get; set; } = "";

        /// <summary>寻迹最大显示数 (SysInfo.m_i_ShowXj_MaxNum)</summary>
        public int TrackingMaxDisplay { get; set; } = 5;

        /// <summary>寻迹当前序号 (SysInfo.m_i_ShowXj_No)</summary>
        public int TrackingCurrentIndex { get; set; } = 0;

        /// <summary>最大等待次数 (SysInfo.m_i_WaitTimeNum_Max)</summary>
        public int MaxWaitCount { get; set; } = 0;

        /// <summary>背景刷新时间 (SysInfo.m_i_Bg_Time)</summary>
        public int BackgroundTime { get; set; } = 10;

        /// <summary>运行步骤提示 (SysInfo.m_blRunStepTips)</summary>
        public bool ShowRunStepTips { get; set; } = true;

        /// <summary>窗体打开标志 (SysInfo.m_blFrmOpen)</summary>
        public bool[] FormOpenFlags { get; set; } = new bool[10];

        /// <summary>TOFD 窗体打开标志 (SysInfo.m_blFrm_TOFD_Open)</summary>
        public bool[] TofdFormOpenFlags { get; set; } = new bool[2];

        /// <summary>是否显示手机界面 (SysInfo.m_iShowPhone)</summary>
        public int ShowPhone { get; set; } = 0;

        /// <summary>连接信息 (SysInfo.m_strLinkMsg)</summary>
        public string LinkMessage { get; set; } = "";

        /// <summary>镀层/测厚模式 (SysInfo.m_i_Cl0_Cx1)</summary>
        public int CoatingMode { get; set; } = 0;

        /// <summary>灯光位置: true=前 false=后 (SysInfo.m_bl_Q1_H0)</summary>
        public bool LightFront { get; set; } = true;

        /// <summary>电源连接状态</summary>
        public bool IsPowerConnected { get; set; } = true;
    }

    /// <summary>
    /// 打印/报表缓存
    /// 替代: SysInfo 中 m_Lst_Print_Item、m_Report_Para 等
    /// </summary>
    public class ReportState
    {
        /// <summary>打印项目列表</summary>
        public List<object> PrintItems { get; } = new List<object>();

        /// <summary>报表参数</summary>
        public Cls_Report_P ReportParams { get; set; } = new Cls_Report_P();

        /// <summary>报表参数 (显示用)</summary>
        public Cls_Report_P ReportParamsDisplay { get; set; } = new Cls_Report_P();

        /// <summary>报表参数列表</summary>
        public List<Cls_Report_P> ReportParamList { get; } = new List<Cls_Report_P>();
    }

    // ========================================================
    // 新增: 寻迹/跟踪状态
    // 替代: SysInfo 中 m_bl_Larser_*, m_lst_Weld, IP_XunJi_Server 等
    // ========================================================

    /// <summary>
    /// 寻迹/焊缝跟踪运行状态
    /// 替代: SysInfo 中激光器、测高寻迹、焊缝跟踪相关字段
    /// </summary>
    public class TrackingState
    {
        /// <summary>激光器方向: false=靠近车体 true=远离车体 (SysInfo.m_bl_Larser_UP1_Down0)</summary>
        public bool LaserAwayFromBody { get; set; } = false;

        /// <summary>寻迹中心值 (SysInfo.m_fl_Larser_Center)</summary>
        public float LaserCenter { get; set; } = 0;

        /// <summary>寻迹类型: 2=测高 0=色带 1=焊缝 (SysInfo.m_iTrack_Type)</summary>
        public int TrackType { get; set; } = 2;

        /// <summary>测高寻迹报文 (SysInfo.m_strComMsg)</summary>
        public string ComMessage { get; set; } = "";

        /// <summary>焊缝寻迹服务器 IP (SysInfo.IP_XunJi_Server)</summary>
        public string TrackingServerIp { get; set; } = "";

        /// <summary>寻迹数据列表 (SysInfo.m_lst_Weld) — 类型在 ClassLibrary_Interface.Models.XjGetData</summary>
        public List<object> WeldDataList { get; } = new List<object>();

        /// <summary>是否获取激光寻迹数据 (SysInfo.m_bl_Get_JGXJ)</summary>
        public bool GetLaserTrackingData { get; set; } = true;

        /// <summary>寻迹程序位置: false=PC true=远程 (SysInfo.m_blXunJi_Prog_0PC_1YY)</summary>
        public bool TrackingOnRemote { get; set; } = false;
    }

    // ========================================================
    // 新增: 相机状态
    // 替代: SysInfo 中 m_strIP, m_strUser, m_strPwd, iWidth, iHeight
    // ========================================================

    /// <summary>
    /// 相机配置状态
    /// 替代: SysInfo 中相机 IP/用户/密码/分辨率字段
    /// </summary>
    public class CameraState
    {
        /// <summary>相机 IP 地址 (SysInfo.m_strIP)</summary>
        public string IpAddress { get; set; } = "";

        /// <summary>相机用户名 (SysInfo.m_strUser)</summary>
        public string Username { get; set; } = "";

        /// <summary>相机密码 (SysInfo.m_strPwd)</summary>
        public string Password { get; set; } = "";

        /// <summary>视频初始化完成 (SysInfo.m_bl_Video_Init)</summary>
        public bool VideoInitialized { get; set; } = false;

        /// <summary>视频网络连通 (SysInfo.strNetTrue_Video)</summary>
        public string VideoNetworkStatus { get; set; } = "";

        /// <summary>相机画面宽度 (SysInfo.iWidth)</summary>
        public int Width { get; set; } = 0;

        /// <summary>相机画面高度 (SysInfo.iHeight)</summary>
        public int Height { get; set; } = 0;
    }
}
