using Frame_Work;
using ClassLib_TestData;
using ClassLibrary_Interface.Models;
using System.Collections.Generic;

// ============================================================
// 文件: RuntimeState.cs
// 位置: NewInspect.Services/State/
// 命名空间: NewInspect.Services.State
// 职责: 拆分 SysInfo 的运行时状态 — 替代原来 290KB 的静态大杂烩
//       每个状态类职责单一，不依赖窗体，可独立单元测试
// 迁移自: NewFramework/State/RuntimeState.cs
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

        /// <summary>是否 4 路相机模式</summary>
        public bool FourCameraMode { get; set; } = false;

        /// <summary>A 扫描图像高度</summary>
        public int AScanImageHeight { get; set; } = 0;

        /// <summary>焊缝算法编号</summary>
        public int WeldAlgorithm { get; set; } = 1;

        /// <summary>通讯帧率</summary>
        public int FrameRate { get; set; } = 100;
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

        /// <summary>结论是否通过</summary>
        public bool Conclusion { get; set; } = false;

        /// <summary>结论信息</summary>
        public string ConclusionInfo { get; set; } = "";

        /// <summary>激光温度</summary>
        public int LaserTemperature { get; set; } = 0;

        /// <summary>寻迹是否自动</summary>
        public bool TrackAuto { get; set; } = false;

        /// <summary>报表类型: 0=新报表 1=自定义 2=厚度列表</summary>
        public int ReportType { get; set; } = 0;

        /// <summary>电源是否连接</summary>
        public bool IsPowerConnected { get; set; } = false;
    }

    /// <summary>
    /// 打印/报表缓存
    /// 替代: SysInfo 中 m_Lst_Print_Item、m_Report_Para 等
    /// </summary>
    public class ReportState
    {
        /// <summary>打印项目列表</summary>
        public List<ClPrintItem> PrintItems { get; } = new List<ClPrintItem>();

        /// <summary>报表参数</summary>
        public Cls_Report_P ReportParams { get; set; } = new Cls_Report_P();

        /// <summary>报表参数 (显示用)</summary>
        public Cls_Report_P ReportParamsDisplay { get; set; } = new Cls_Report_P();

        /// <summary>报表参数列表</summary>
        public List<Cls_Report_P> ReportParamList { get; } = new List<Cls_Report_P>();
    }
}
