using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClassLibrary_Interface;
using Clb_MT_Comm;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

using System.Reflection;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using ClassLib_TestData;

using Cmm_PcPower;//电池
using System.IO;//文件操作引用
using Frame_Work;
//using Tofd_DLL;
//using ReportDLL;
using System.Diagnostics;

namespace Tofd_AWI.Class
{
    /// <summary>
    /// 系统资源
    /// </summary>
    public class SysInfo
    {
        /// <summary>
        /// 0：中文 1：英文
        /// </summary>
        public static int m_iLanguage = 0;
        /// <summary>
        /// 当前数据是否保存
        /// </summary>
        public static bool m_bl_Save = false;

        /// <summary>
        ///  True: 4个相机， false: 老的两个窗口循环  是否前后左右4个相机，以第4个相机是否有IP为依据
        /// </summary>
        public static bool m_bl_Qhzy = false;

      //  public static   Tofd_AWI.From.Frm_Stop m_frm_Stop;
        /// <summary>
        /// 结果选择 true：false
        /// </summary>
        public static bool m_bl_JL = false;
        /// <summary>
        /// 结论传递信息
        /// </summary>
        public static string m_str_JL = "";
        /// <summary>
        /// 超声探头厂家类型 0：武汉中科  1：北京六维远光
        /// </summary>
        public static int m_i_UI_Type = 0;
        /// <summary>
        /// 北京六维远光采集时间  默认 10us
        /// </summary>
        public static int  m_iChScanRange=10;
        /// <summary>
        /// TOFD:0  Cscan:1   M_Ui:2    Coat: 3
        /// </summary>
        public static int m_i_TOFD_0_Cscan_1_Mui_2 = 0;
        /// <summary>
        /// 保存老的测量方式，如果不是TOFD，可以切换
        /// </summary>
        public static int m_i_TOFD_0_Cscan_1_Mui_2_Old = 0;
        /// <summary>
        /// 录像状态：1:开始  0:结束
        /// </summary>
        public static int m_i_Video = 1;

        /// <summary>
        /// 北京六维远光探头 佟先生 15611883261  Mail:tongabcd@yeah.net
        /// </summary>
    //    public static Tofd_DLL.CLTofd_BJ m_Tofd_BJ;
        /// <summary>
        /// C扫描超声DLL
        /// </summary>
     //   public static Tofd_DLL.Cls_C_Scan m_Tofd_C_Scan;
        /// <summary>
        /// 打印报表类型 0：新报表  1：自定义的报表格式 2: 厚度列表
        /// </summary>
        public static int m_i_ReportType=0;
        /// <summary>
        /// 是否使用激光寻迹
        /// </summary>
        public static int m_i_Alarm = 1;

        #region 打印数据缓存

        /// <summary>
        /// 报表输出
        /// </summary>
  //      public static ClsReport m_Report = new ClsReport();
        /// <summary>
        /// 打印：项目名称
        /// </summary>
        public static List<Cl_Print_Item> m_Lst_Print_Item = new List<Cl_Print_Item>();
        #endregion 打印缓存
        /// <summary>
        /// 运行提示信息
        /// </summary>
        public static string m_strRunTitl = "";
        /// <summary>
        /// 鼠标移动
        /// </summary>
        public static string m_str_A_Tiltl = "";

        /// <summary>
        /// 寻迹是否打开
        /// </summary>
        public static bool m_bl_Track_Auto = false;
        /// <summary>
        /// 系统缓存
        /// </summary>
        public static Frame_Work.ClassTofd_Buff m_SysBuff = new Frame_Work.ClassTofd_Buff();
        /// <summary>
        /// C扫描数据类
        /// </summary>
        public static  Class_C_Buff m_SysBuff_C = new Class_C_Buff();
        /// <summary>
        /// C扫描项目信息
        /// </summary>
        public static Class_C_ItemInfor m_C_Item_Info = new Class_C_ItemInfor();
        /// <summary>
        /// 新项目
        /// </summary>
        public static bool  m_bl_New_Item=false ;

        public static ClDog m_Dog = new ClDog();


        /// <summary>
        /// 灯光位置控制：前/后
        /// </summary>
        public static bool m_bl_Q1_H0 = true;
        /// <summary>
        /// 视频通讯服务器
        /// </summary>
        public static    Class_Server_UI m_ServerUI = new Class_Server_UI();
        /// <summary>
        /// 距离信息通讯
        /// </summary>
        public static Class_Server_Mul_Distanc m_Server_MulDistan = new Class_Server_Mul_Distanc();
        //--
        /// <summary>
        /// A扫描图
        /// </summary>
        public static Thread Thread_A_Wave = null;
     
        /// <summary>
        /// D扫描图
        /// </summary>
        public static Thread Thread_D_Wave = null;
        /// <summary>
        /// 寻迹信息显示次数：累计够5次显示一次
        /// </summary>
        public static int m_i_ShowXj_MaxNum = 5;
        /// <summary>
        /// 累计次数
        /// </summary>
        public static int m_i_ShowXj_No = 0;
        /// <summary>
        /// 激光器温度
        /// </summary>
        public static int m_i_LasersTempr = 0;
        /// <summary>
        /// 系统键盘打开：1  关闭：0
        /// </summary>
        public static int m_i_Osk = 1;
        /// <summary>
        /// A扫描图像高度
        /// </summary>
        public static int m_i_Pic_A_Height = 0;
        /// <summary>
        /// 给车体发送循迹命令等待总时间间隔
        /// </summary>
        public static int m_i_WaitTimeNum_Max = 0;
        /// <summary>
        /// 曝光时间
        /// </summary>
        public static int m_i_Bg_Time = 10;
        /// <summary>
        /// 增益值
        /// </summary>
        public static float m_fl_Gain = 5;
        /// <summary>
        /// 激光器写通讯日志
        /// </summary>
        public static int m_i_Lars_RiZhi = 0;
        /// <summary>
        /// 焊缝算法
        /// </summary>
        public static int m_Ck_First = 1;
        /// <summary>
        /// 通讯频率
        /// </summary>
        public static int m_i_Frame_Num = 100;

        #region 增加输入窗口
        /// <summary>
        /// 项目文件夹名字
        /// </summary>
        public static string m_W_strItemName = "";
        /// <summary>
        /// 项目标识
        /// </summary>
        public static string m_ItemMark = "_@@";
        /// <summary>
        /// 老项目
        /// </summary>
        public static string m_W_strItemName_Old = "-1";
        /// <summary>
        /// 焊缝编号
        /// </summary>
        public static string m_W_strWeldID = "";
        /// <summary>
        /// 焊缝ID列表
        /// </summary>
        public static List<string> m_W_Lst_WeldID = new List<string>();
        /// <summary>
        /// 报表参数
        /// </summary>
        public static Cls_Report_P m_Report_Para = new Cls_Report_P();
        /// <summary>
        /// 用于显示报表参数
        /// </summary>
        public static Cls_Report_P m_Report_Para_CS = new Cls_Report_P();
        /// <summary>
        /// 报表显示用缓存
        /// </summary>
        public static List<Class_Test_AlarmArea> m_Lst_Mark_Record_CS = new List<Class_Test_AlarmArea>();

        public static List < Cls_Report_P> m_LstReport_Para = new List<Cls_Report_P>();
        #endregion 

        /// <summary>
        /// 是否使用PC控制车体
        /// </summary>
        public static bool m_bl_ClimbRun_By_PC = false;
    
        /// <summary>
        /// 存储路径
        /// </summary>
        public static string m_W_i_FilePath = Application.StartupPath + "\\Temp";
        /// <summary>
        /// 调用图片计算
        /// </summary>
        public static bool m_bl_Img_Calcu = false;
        /// <summary>
        /// 激光文件
        /// </summary>
        public static string m_Log_Weld = Application.StartupPath + "\\m_Log_Weld.ini";
        /// <summary>
        /// 寻迹程序是否在研杨板子上，随车体安装 
        /// true: 在研杨板子上 
        /// false: 在PC上
        /// </summary>
        public static bool m_blXunJi_Prog_0PC_1YY = false;
        /// <summary>
        /// 存储数据类型 1：一条焊缝对应一条独立文件  0：使用网络数据库界面
        /// </summary>
        public static int m_W_i_SaveType_1One_0Web = 0;

        public static int m_iA_Dellon_MaxNum = 11;
        /// <summary>
        /// 是否显示视频窗口
        /// </summary>
        public static bool m_bl_Show_Video = false;
        /// <summary>
        /// 接收激光寻迹数据
        /// </summary>
        public static bool m_bl_Get_JGXJ = true;
        /// <summary>
        /// 速度类型 0：手动  1：自动
        /// </summary>
        public static int m_i_Climb_Hand0_Auto1 = 0;
        /// <summary>
        /// 减薄量对应颜色
        /// </summary>
        public static string m_str_B_Stand_Color = "";

        /// <summary>
        /// 增厚量对应颜色
        /// </summary>
        public static string m_str_B_Stand_Color_A = "";
        /// <summary>
        /// 打标器滞后探头距离mm
        /// </summary>
        public static float m_fl_MarkLag = 0;
        /// <summary>
        ///自动打标延时时间 s
        /// </summary>
        public static int m_i_MarkLag_WaitTime = 2;

        /// <summary>
        /// 打标器滞后探头距离mm
        /// </summary>
        public static float m_fl_MarkLag_m = 0;
        /// <summary>
        /// 实际距离与要求打标位置误差mm
        /// </summary>
        public static float m_fl_MarkLag_Limit = 10;
        /// <summary>
        /// 打标记录
        /// </summary>
        public static List<Cl_Mark> m_Lst_Mark = new List<Cl_Mark>();
     
        /// <summary>
        /// 程序运行名称
        /// </summary>
        public static string m_strProName = "TOFD自动焊缝检测系统";
        /// <summary>
        /// 前后视频连接状态 00 01 10 11  0:联机成功  1：2号失败 2:1号失败  3：都失败
        /// </summary>
        public static string  strNetTrue_Video ="";
        /// <summary>
        /// 1：开始 0：停止 2: 检定完成  10:退出程序
        /// </summary>
        //public static int m_iRun = 0;

        /// <summary>
        /// 窗体是否打开0：Tofd 1:项目管理 2：焊缝3：车体控制  4: 标注  5 缺陷样板 6: 报告参数 7 文件拷贝 8 打印 9 多层厚度
        /// </summary>
        public static bool[] m_blFrmOpen = new bool[10];
        /// <summary>
        /// 报表参数是否输入
        /// </summary>
        public static   bool m_bl_HaveGetReportPara = false;
        /// <summary>
        /// TOFD俩个子窗口
        /// </summary>
        public static bool[] m_blFrm_TOFD_Open = new bool[2];
        /// <summary>
        ///1：前视  中视  2 前视 后视  3 前视  4 中视  5 后视
        /// </summary>
        public static int m_iShowPhone = 0;
        /// <summary>
        /// 联机信息
        /// </summary>
        public static string m_strLinkMsg = "";

        #region 检测数据
        ///// <summary>
        ///// 视频通讯服务器
        ///// </summary>
        //public static Class_Server_UI m_ServerUI = new Class_Server_UI();
        /// <summary>
        /// 当前测量0：查询状态：1
        /// </summary>
        public static int m_i_Cl0_Cx1 = 0;
        /// <summary>
        /// 子文件名称，方便共享
        /// </summary>
        public static string m_strSaveDataFileName = "";//子文件名称
        /// <summary>
        /// 步骤是否提示
        /// </summary>
        public static bool m_blRunStepTips = true;
        /// <summary>
        /// 项目检测记录
        /// </summary>
        public static Class_Test_Item m_Test_Item = new Class_Test_Item();
        /// <summary>
        /// 检测焊缝
        /// </summary>
        public static Class_Test_Parts m_Test_Parts = new Class_Test_Parts();
  
        /// <summary>
        /// 异常点记录
        /// </summary>
        public static Class_Test_AlarmArea m_Test_Alarm = new Class_Test_AlarmArea();

        /// <summary>
        /// 标注数据参数
        /// </summary>
        public static ClBiaoZhu m_BiaoZhu = new ClBiaoZhu();
        /// <summary>
        /// 标注序号 
        /// </summary>
        public static CL_BiaoZ_S m_BiaoZhu_No = new CL_BiaoZ_S();
        #endregion 检测数据
        /// <summary>
        /// 电池
        /// </summary>
        public static ClasPcPower m_PcPower;
        /// <summary>
        /// 是否有电池监视
        /// </summary>
        public static int m_i_Have_m_Power = 0;
        /// <summary>
        /// 采集数据
        /// </summary>
        public static bool blCollection = true;
        /// <summary>
        /// 初始化相机
        /// </summary>
        public static bool m_bl_Video_Init = false;
        /// <summary>
        /// 文件读写
        /// </summary>
        public static ClassInterFace csInter = new ClassInterFace();
        public static string HardFileName = Application.StartupPath + "\\database\\SysConfig.ini";

        public static string m_Log_Main = Application.StartupPath + "\\datalog\\Main_Log.ini";

        /// <summary>
        /// 消息接口
        /// </summary>
        public static ClassLib_TestData.MsgInterFace g_Msg_InterFace = ClassLib_TestData.MsgInterFace.GetInstance();
       /// <summary>
       /// 涂层车体一些参数命令，不能在自动开始时发送，容易引起硬件flash出错
       /// </summary>
        public static void  Coat_SentCmd_Black()
        {
            SysInfo.m_Climb4.SendData(2, 4, 0, 1, SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval.ToString() + "," +
                                                 SysInfo.m_SysBuff.m_Climb.i_Coat_Interval.ToString());

      //      SysInfo.m_Climb4.SendData(2, 4, 0, 1, SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval.ToString());//写单步间距
            SysInfo.SetSpeed();
        }

        /// <summary>
        /// 武汉中科
        /// </summary>
        public static   void Whzk()
        {
            if (SysInfo.csInter.IniReadDefine("TOFD", "Have", "0", SysInfo.HardFileName) != "1") return;

            //if (SysInfo.m_Tofd_C_Scan == null || SysInfo.m_Tofd_C_Scan != null &&
            //    SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0 && SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type == 1 ||
            //    SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 && SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type == 1)
                SysInfo.m_SysBuff.m_Climb.iBmq_Type = 1;
        }
        /// <summary>
        /// 距离清零
        /// </summary>
        public static void  Clear_Dis()
        {
            if (SysInfo.m_SysBuff.m_Climb.iBmq_Type == 1)
                SysInfo.m_Climb4.SendData(2, 3, 0, 0, "0");
            else
                SysInfo.m_SysBuff.m_Tofd_DLL.Init_Encoder();
        }
        /// <summary>
        /// 当前序号对应的总时间
        /// </summary>
        /// <param name="iNo"></param>
        /// <returns></returns>
        public static float GetCurrTime(int iNo)
        {
            if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam == null) return 0;
            float _Time_One = 0;
       //     if (SysInfo.m_i_UI_Type == 1)
       //         _Time_One = SysInfo.m_Tofd_BJ.Cal_Thick(SysInfo.m_Plant.m_i_X_No, false) + (float)SysInfo.m_Tofd_BJ.m_ChScanStart;
       //     else
       //     {
       //         _Time_One = SysInfo.m_SysBuff.m_Tofd_DLL.m_fl_Time_JG * iNo;    // iNo * 5 / 1000f;//每个点 乘以 5ns
       //         float _fl_ParalletTime = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iParallelTime / 100f;
       //         _Time_One += _fl_ParalletTime;// SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iParallelTime / 100f;


       ////         _Time_One = Tofd.m_pTimeBuf[iNo] /200 + _fl_ParalletTime;

       //         // _Time_One = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iRange * 2.0f /
       //         //         (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_dSpeed / 1000f * Tofd.UTS_DATA_WIDTH);//单位us
       //         //_Time_One = iNo * _Time_One;// - SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].T0;
       //     }
            if (_Time_One < 0) _Time_One = 0;
            return _Time_One;
        }

        #region 车体信息
        /// <summary>
        /// 接收服务器数据
        /// </summary>
        public static TcpClient_UI m_Client;
        /// <summary>
        /// 激光器线靠近车体0  远离车体：1
        /// </summary>
        public static bool m_bl_Larser_UP1_Down0 = false ;
        /// <summary>
        /// 寻迹中心值
        /// </summary>
        public static float m_fl_Larser_Center = 0;
        /// <summary>
        /// 寻迹类型 2：测高 0: 色带 1：焊缝
        /// </summary>
        public static int m_iTrack_Type = 2;
        /// <summary>
        /// 接收到的测高寻迹报文
        /// </summary>
        public static string m_strComMsg = "";
        /// <summary>
        /// 焊缝寻迹服务器IP
        /// </summary>
        public static string IP_XunJi_Server = "";
        /// <summary>
        /// 接收到的测高寻迹数据
        /// </summary>
        public static List<Class_Xj_GetData> m_lst_Weld = new List<Class_Xj_GetData>();
        /// <summary>
        /// 手柄是否连续控制车体运行
        /// </summary>
        public static int  m_i_Hand_Do=0 ;
        /// <summary>
        /// 4轮控制
        /// </summary>
        public static Clb_MT_Comm.MT_Comm m_Climb4;
        /// <summary>
        /// 记录放电打标间隔距离
        /// </summary>
        public static int m_i_Add_MarkDisc = 100;

        public static byte[] m_bt_Arr = new byte[4];
        /// <summary>
        /// 放开按钮后，等待次数10次后发送停止命令
        /// </summary>
        public static int Time_Key_Down = 10;
        ///// <summary>
        ///// 距离信息
        ///// </summary>
        //public static ClassLib_TestData.clStreamVideo m_Climb = new ClassLib_TestData.clStreamVideo();

        /// <summary>
        /// Can刷新时间间隔
        /// </summary>
        public static int m_i_Can_BrushTime = 20;
        /// <summary>
        /// false: 喷雾打标  true: 蜡笔打标
        /// </summary>
        public static bool m_bl_Mark = false;
        public static void DaBiao(int iRun=0)
        {
          if(iRun==1)     SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");

            //1 打标机构降
            m_Climb4.SendData(5,4, 2);//打标机构降
            SysInfo.WaitTime(1000);

            //2 前进
            if (iRun == 1)
            {
                if (SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Qj1_Ht0 == 1)
                    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "1");
                else
                    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "2");
                float _iOld_x = 0;
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0)

                    _iOld_x = SysInfo.m_SysBuff.m_Tofd_DLL.m_flDistanc_X;

                else

                    _iOld_x = SysInfo.m_Climb4.m_SysBuf.Trip_Com_mm;
                DateTime dtStar = DateTime.Now;
                while (true)
                {
                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > 3) break;

                    //监视距离变化10mm
                    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0)
                    {
                        if (Math.Abs(_iOld_x - SysInfo.m_SysBuff.m_Tofd_DLL.m_flDistanc_X) > 0.01)
                            break;
                    }
                    else
                    {
                        if (Math.Abs(_iOld_x - SysInfo.m_Climb4.m_SysBuf.Trip_Com_mm) > 10) break;
                    }
                }
           //     SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
            }
            //3 回升打标机构
            m_Climb4.SendData(5,4, 3);//打标机构升
            SysInfo.WaitTime(1000f);
            m_Climb4.SendData(5,4, 4);//打标机构停止升降
        }
        #endregion 车体信息

        #region 相机
        /// <summary>
        /// 图像高度比例
        /// </summary>
        public static float m_flPic_Height = 30;
        /// <summary>
        /// 相机IP
        /// </summary>
        public static string m_strIP = "";
        /// <summary>
        /// 用户名
        /// </summary>
        public static string m_strUser = "";
        /// <summary>
        /// 密码
        /// </summary>
        public static string m_strPwd = "";
        /// <summary>
        /// 相机分辨率 宽度
        /// </summary>
        public static int iWidth = 0;
        /// <summary>
        /// 相机分辨率 高度
        /// </summary>
        public static int iHeight = 0;
        #endregion 相机

        #region 文件备份
        public static string  File_Source()
        {
            //1 选择
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";//ShowNewFolderButton 

            string _strIniPath = "";
            if (SysInfo.m_W_strItemName == "")
                _strIniPath = SysInfo.m_W_i_FilePath + "\\";
            else
            {
                SysInfo.m_W_strItemName = SysInfo.m_W_strItemName.Replace(SysInfo.m_ItemMark, "");
                _strIniPath = SysInfo.m_W_i_FilePath + "\\" + SysInfo.m_W_strItemName + SysInfo.m_ItemMark;
            }
            dilog.SelectedPath = _strIniPath;
            dilog.ShowNewFolderButton = false;//不显示创建文件夹

            string _strPath = "";

            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                _strPath = dilog.SelectedPath;//目录文件是单选
            }
            return _strPath;
        }
        public static string File_Desti()
        {
            //1 选择
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";//ShowNewFolderButton 

            string _strIniPath = "";
            if (SysInfo.m_W_strItemName == "")
                _strIniPath = SysInfo.m_W_i_FilePath + "\\";
            else
            {
                SysInfo.m_W_strItemName = SysInfo.m_W_strItemName.Replace(SysInfo.m_ItemMark, "");
                _strIniPath = SysInfo.m_W_i_FilePath + "\\" + SysInfo.m_W_strItemName + SysInfo.m_ItemMark;
            }
            dilog.SelectedPath = _strIniPath;
            dilog.ShowNewFolderButton = true;//显示创建新文件加

            string _strPath = "";

            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                _strPath = dilog.SelectedPath;//目录文件是单选
            }
            return _strPath;
        }
        #endregion 文件备份

        #region 系统画图

        public static Cls_Plant m_Plant = new Cls_Plant();


        /// <summary>
        /// 计算厚度色标限
        /// </summary>
        /// <param name="strNum"></param>
        /// <param name="P_1"></param>
        /// <param name="P_Md"></param>
        /// <param name="P_2"></param>
        /// <param name="flMd_Bl"></param>
        public static void InitLimitPic(string _str_B_Stand_Color, int _iNum, PictureBox P_1, PictureBox P_Md, PictureBox P_2, GroupBox Grp_Color,
               float fl_1_BL = 0.4F, float fl_2_BL = 0.3f, float fl_3_BL = 0.3F)
        {
          if (_iNum == 0|| _iNum> 200) 
                _iNum = 200;
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0)
             SysInfo.m_SysBuff.m_Tofd_DLL.m_Co_StandColor = new Color[_iNum];
            //else
            //    Cls_Plant_C.m_Co_StandColor = new Color[_iNum];

            P_Md.Width = (int)(Grp_Color.Width * fl_2_BL);
            P_1.Width = (int)(Grp_Color.Width * fl_1_BL);
            P_2.Width = (int)(Grp_Color.Width * fl_3_BL);

            Bitmap Rr_Bitmap_1 = new Bitmap(P_1.ClientSize.Width, P_1.ClientSize.Height);
            Bitmap Rr_Bitmap_2 = new Bitmap(P_2.ClientSize.Width, P_2.ClientSize.Height);
            Bitmap Rr_Bitmap_Md = new Bitmap(P_Md.ClientSize.Width, P_Md.ClientSize.Height);
            float flJG = 0;

            #region 多色，正常测试
            P_1.Left = 1;
            P_Md.Left = P_1.Left + P_1.Width;
            P_2.Left = P_Md.Left + P_Md.Width;

            P_1.Visible = true; P_2.Visible = true; P_Md.Visible = true;
            flJG = (float)((Grp_Color.Width) / _iNum);

            //图片2点数据量：由最小刻度倍数和第一图片间隔获得第二图片最小间隔，然后获得图片2点数量
            int iP_1_NumJg = (int)(((P_1.Width / (float)(Grp_Color.Width - 2)) * _iNum));
            int iP_2_NumJg = (int)((P_2.Width / (float)(Grp_Color.Width - 2)) * _iNum);
            int iP_Md_NumJg = (int)((P_Md.Width / (float)(Grp_Color.Width - 2)) * _iNum);
            // 画色标图
            if (_str_B_Stand_Color == "")
            {
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || m_i_TOFD_0_Cscan_1_Mui_2==2)
                    _str_B_Stand_Color = "0/0/255|154/205/50|255/255/0|255/0/0";
                else
                {
                    _str_B_Stand_Color = "255/255/255" + "|";//白色
                    _str_B_Stand_Color += "121/112/184" + "|";//中间开始
                    _str_B_Stand_Color += "199/179/97" + "|";//中间结束
                    _str_B_Stand_Color += "220/108/75";//最终颜色
                }
                m_str_B_Stand_Color = _str_B_Stand_Color;
                m_str_B_Stand_Color_A = _str_B_Stand_Color;
                csInter.INIWriteValue("TOFD", "m_str_B_Stand_Color", m_str_B_Stand_Color, HardFileName);
                csInter.INIWriteValue("TOFD", "m_str_B_Stand_Color_A", m_str_B_Stand_Color_A, HardFileName);
            }
            Color[] _ArrColor = SysInfo.GetStantCol(_str_B_Stand_Color);

            float flStart = 0;
            float fl_JG = m_i_TOFD_0_Cscan_1_Mui_2 == 0 ? 1 : 0.01f; // float.Parse(SysInfo.m_SysInfo.g_Gate.strWc_Fbl);//  0.01f;// float.Parse(Txt_Wc_fbl.Text);// Ck_By_Gchd.Checked ? _flScale_Low : 0.01f;
            PlanScheme_ColorLimit_JG(_ArrColor, P_1, iP_1_NumJg, ref Rr_Bitmap_1, 0, 0, fl_JG, ref flStart);
            PlanScheme_ColorLimit_JG(_ArrColor, P_Md, iP_Md_NumJg, ref Rr_Bitmap_Md, 3, 0, fl_JG, ref flStart);
            PlanScheme_ColorLimit_JG(_ArrColor, P_2, iP_2_NumJg, ref Rr_Bitmap_2, 1, 0, fl_JG, ref flStart);

            #region 1

            float _flLess =  0.01f;// (SysInfo.m_SysInfo.g_ScreenPlant.iRad_Dw == 0 ? float.Parse(SysInfo.m_SysInfo.g_Gate.strWc_Fbl) : 0f);
            for (int i = 0; i < iP_1_NumJg; i++)//flJG -_flLess
            {
                #region   1 获得色度值
                Color _color = Rr_Bitmap_1.GetPixel((int)(i ), 0);
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0)
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_Co_StandColor[i] = _color;
                #endregion 1
            }
            #endregion 1

            #region 中间过度色
            for (int i = 0; i < iP_Md_NumJg; i++)
            {
                #region   1 获得色度值flJG -* ( _flLess)
                Color _color = Rr_Bitmap_Md.GetPixel((int)(i ), 0);
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0)
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_Co_StandColor[iP_1_NumJg + i] = _color;
                //else
                //    Cls_Plant_C.m_Co_StandColor[iP_1_NumJg + i] = _color;
                #endregion 1
            }
            #endregion 中间过度色

            #region 2
            for (int i = 0; i < iP_2_NumJg; i++)
            {
                try
                {
                    #region   1 获得色度值flJG - * ( _flLess)
                    Color _color = Rr_Bitmap_2.GetPixel((int)(i), 0); //(iMove_1 + i * iJG_Height, 0);
                    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0)
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_Co_StandColor[iP_1_NumJg + iP_Md_NumJg + i] = _color;
                    #endregion 1
                }
                catch { }
            }
            #endregion 2
            #endregion 多色
        }
        /// <summary>
        /// 涂层颜色对应厚度误差
        /// </summary>
        /// <param name="Pic_S">开始颜色</param>
        /// <param name="Pic_E">结束颜色</param>
        /// <param name="Pic_Out">输出图</param>
        /// <param name="iNum">等份个数</param>
        /// <param name="_flScale_Low">等份厚度值</param>
        /// <param name="iType">0：减薄 1：常规 2：增厚</param>
        public static void InitLimitPic_Coat(PictureBox Pic_S,PictureBox Pic_E,ref PictureBox Pic_Out,int iNum,float _flScale_Low, float flStart = 0,int iType=0)
        {
            Color[] _ArrColor = new Color[2];
            _ArrColor[0] = Pic_S.BackColor;
            _ArrColor[1] = Pic_E.BackColor;

            Bitmap Rr_Bitmap = new Bitmap(Pic_E.ClientSize.Width, Pic_E.ClientSize.Height);
            float _fl_In_Start = flStart;
            PlanScheme_ColorLimit_JG(_ArrColor, Pic_Out, iNum, ref Rr_Bitmap, 0, 0, _flScale_Low, ref flStart);

            float _flLess = 0.01f;
            float flJG = Pic_S.Width*1.0f /iNum ;

            switch (iType )
            {
                case 0:
                    Cls_Plant_C.m_Co_Coat_Jb.Clear();
                    break;
                case 1:
                    Cls_Plant_C.m_Co_Coat_Md.Clear();
                    break;
                case 2:
                    Cls_Plant_C.m_Co_Coat_Zh.Clear();
                    break;
            }
            float _flLimit = _fl_In_Start;
            int _iD = 0;
            for (int i = 0; i < iNum; i++)
            {
                #region   1 获得色度值
                _iD = (int)(i * (flJG - _flLess));
                if (_iD > Rr_Bitmap.Width) continue;
                Color _color = Rr_Bitmap.GetPixel(_iD, 0);
                #endregion 1
             
                #region 2 将误差限数据添加进系统
                ColorRange _clorRg = new ColorRange();
                _clorRg.R = _color.R;
                _clorRg.G = _color.G;
                _clorRg.B = _color.B;
                _clorRg.Max_Limit = _flLimit;
                _flLimit += _flScale_Low;

                #endregion 2
                switch (iType)
                {
                    case 0://减薄
                        Cls_Plant_C.m_Co_Coat_Jb.Add(_clorRg);
                        break;
                    case 1://常规
                        Cls_Plant_C.m_Co_Coat_Md.Add(_clorRg);
                        break;
                    case 2://增厚
                        Cls_Plant_C.m_Co_Coat_Zh.Add(_clorRg);
                        break;
                }
            }
        }
        /// <summary>
        /// 解析标准颜色
        /// </summary>
        /// <returns></returns>
        public static Color[] GetStantCol( string _str_B_Stand_Color)
        {
            System.Drawing.Color[] _RetArrColor = new System.Drawing.Color[4];

            string[] _sPara = "".Split('|');
            string[] _sPara_Sub = "".Split('|');
            int iR = 0, iG = 0, iB = 0;
            if (_str_B_Stand_Color != "")
                _sPara = _str_B_Stand_Color.Split('|');
            if (_sPara.Length >= 4)
            {
                //    Ck_MyColor.Checked = _sPara[0] == "1" ? true : false;//0 自定义
                #region 1 开始色
                _sPara_Sub = _sPara[0].Split('/');
                if (_sPara_Sub.Length == 3)
                {
                    if (_sPara_Sub[0] != "" && _sPara_Sub[1] != "" && _sPara_Sub[2] != "")
                    {
                        iR = int.Parse(_sPara_Sub[0]);
                        iG = int.Parse(_sPara_Sub[1]);
                        iB = int.Parse(_sPara_Sub[2]);
                        _RetArrColor[0] = System.Drawing.Color.FromArgb((byte)iR, (byte)iG, (byte)iB);
                    }
                }
                #endregion 1
                #region 2 中间开始色
                _sPara_Sub = _sPara[1].Split('/');
                if (_sPara_Sub.Length == 3)
                {
                    if (_sPara_Sub[0] != "" && _sPara_Sub[1] != "" && _sPara_Sub[2] != "")
                    {
                        iR = int.Parse(_sPara_Sub[0]);
                        iG = int.Parse(_sPara_Sub[1]);
                        iB = int.Parse(_sPara_Sub[2]);
                        _RetArrColor[1] = System.Drawing.Color.FromArgb((byte)iR, (byte)iG, (byte)iB);
                    }
                }
                #endregion 2
                #region 3 中间开始色
                _sPara_Sub = _sPara[2].Split('/');
                if (_sPara_Sub.Length == 3)
                {
                    if (_sPara_Sub[0] != "" && _sPara_Sub[1] != "" && _sPara_Sub[2] != "")
                    {
                        iR = int.Parse(_sPara_Sub[0]);
                        iG = int.Parse(_sPara_Sub[1]);
                        iB = int.Parse(_sPara_Sub[2]);
                        _RetArrColor[2] = System.Drawing.Color.FromArgb((byte)iR, (byte)iG, (byte)iB);
                    }
                }
                #endregion 3
                #region 4 结束色
                _sPara_Sub = _sPara[3].Split('/');
                if (_sPara_Sub.Length == 3)
                {
                    if (_sPara_Sub[0] != "" && _sPara_Sub[1] != "" && _sPara_Sub[2] != "")
                    {
                        iR = int.Parse(_sPara_Sub[0]);
                        iG = int.Parse(_sPara_Sub[1]);
                        iB = int.Parse(_sPara_Sub[2]);
                        _RetArrColor[3] = System.Drawing.Color.FromArgb((byte)iR, (byte)iG, (byte)iB);
                    }
                }
                #endregion 4
            }

            return _RetArrColor;
        }
        private static float PlanScheme_ColorLimit_JG(Color[] ArrColor, System.Windows.Forms.PictureBox PicArea, int iJG, ref Bitmap Rr_Bitmap, int iType,
                        int iHorizontal_0, float fl_JG_JL, ref float flStart)
        {
            int _iRet = 0;//画图返回
            try
            {
                float flPic_W = PicArea.Width;//画布宽度
                float flPic_H = PicArea.Height;//画布高度

                #region 1.2 准备画布
                // 初始化画板，在内存中建立一块虚拟画布
                Bitmap image = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                // 获取背景层
                Bitmap bg = (Bitmap)PicArea.BackgroundImage;
                // 初始化整个画布
                Bitmap canvas = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
                // 初始化图形面板，获取这块内存画布的Graphics的引用
                Graphics g = Graphics.FromImage(image);
                Graphics gb = Graphics.FromImage(canvas);

                #endregion  准备画布
                g.Clear(Color.White);// g.Clear(Color.Teal);// CadetBlue);
                Rectangle rect = new Rectangle(0, 0, PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                rect.Location = new Point(0, 0);
                LinearGradientBrush b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Horizontal);
                if (ArrColor[0].R == 0 && ArrColor[0].G == 0 && ArrColor[0].B == 0)
                {
                    ArrColor[0] = Color.Blue; ArrColor[1] = Color.YellowGreen;
                    ArrColor[2] = Color.Yellow; ArrColor[3] = Color.Red;
                }
                if (iHorizontal_0 == 0)
                {

                    switch (iType)
                    {
                        case 0:
                            b3 = new LinearGradientBrush(rect, ArrColor[0], ArrColor[1], LinearGradientMode.Horizontal); //Color.Blue, Color.YellowGreen, 
                            break;
                        case 2:
                            b3 = new LinearGradientBrush(rect, ArrColor[0], ArrColor[1], LinearGradientMode.Horizontal);//Color.Blue, Color.White,
                            break;
                        case 1:
                            b3 = new LinearGradientBrush(rect, ArrColor[2], ArrColor[3], LinearGradientMode.Horizontal);// Color.Yellow, Color.Red,
                            break;
                        case 3:
                            b3 = new LinearGradientBrush(rect, ArrColor[1], ArrColor[2], LinearGradientMode.Horizontal);// Color.YellowGreen, Color.Yellow,
                            break;
                    }
                }
                else
                    switch (iType)
                    {
                        case 0:
                            b3 = new LinearGradientBrush(rect, ArrColor[0], ArrColor[1], LinearGradientMode.Vertical); //Color.Blue, Color.YellowGreen, 
                            break;
                        case 2:
                            b3 = new LinearGradientBrush(rect, ArrColor[0], ArrColor[1], LinearGradientMode.Vertical);//Color.Blue, Color.White,
                            break;
                        case 1:
                            b3 = new LinearGradientBrush(rect, ArrColor[2], ArrColor[3], LinearGradientMode.Vertical);// Color.Yellow, Color.Red,
                            break;
                        case 3:
                            b3 = new LinearGradientBrush(rect, ArrColor[1], ArrColor[2], LinearGradientMode.Vertical);// Color.YellowGreen, Color.Yellow,
                            break;

                    }

                g.FillRectangle(b3, rect);

                //  if (iHorizontal_0 == 1)
                //  {
                Pen p = new Pen(Brushes.Green);//刻度笔
                int X, Y, iBzNum, iH, iXcale = 6;//位置
                string strT = "";
                Font drawFont = new Font("Arial", 8);//刻度字体定义
                SolidBrush drawBrush = new SolidBrush(iType != 0 ? Color.Black : Color.Black);//刻度刷子

                //switch (iType)
                //{
                //    case 0:
                if (iHorizontal_0 == 0)//横轴
                {
                    iBzNum = (int)((PicArea.Width) / iJG);
                    if (iBzNum == 0) { iBzNum = PicArea.Width; }
                }
                else
                {
                    iBzNum = (int)((PicArea.Height) / iJG);
                    if (iBzNum == 0) { iBzNum = PicArea.Height; }
                }
                //   iJG = 1;

                if (iHorizontal_0 == 0)//横轴
                {
                    X = 0;
                    Y = PicArea.Height - 3;
                }
                else
                {
                    X = 2;
                    Y = 0;
                }
                int Y_Titl = PicArea.Height - 15;
                float _flJg = 0f;
                //;//实际值/百分值
                StringFormat StrF = new StringFormat();
                //StrF.FormatFlags = StringFormatFlags. //DirectionVertical; //StringFormatFlags.DirectionVertical; StringFormatFlags.DirectionVertical; // 竖排
                float fl_ScaleLeast = 0.01f;

                float _fl_Jg = (float)PicArea.Width / iJG;
                if (iHorizontal_0 == 1)//横轴
                    _fl_Jg = (float)PicArea.Height / iJG;
                int iPic_Jg = (int)_fl_Jg;
                float _fl_Add = _fl_Jg - iPic_Jg;
                int iRad_Dw = 0;
                int i10 = iJG > 500 ? 100 : (iRad_Dw == 0 ? (iJG > 6 ? 5 : 20) : (iJG > 190 ? 20 : (iJG > 3 ? 4 : 2)));
                string _strDw = "";// iRad_Dw == 0 ? "mm" : "in.";
                string _strXsw = m_i_TOFD_0_Cscan_1_Mui_2 == 0? "0":"0.01";// iRad_Dw == 0 ? "0.00" : "0.00000"; || m_i_TOFD_0_Cscan_1_Mui_2 == 3 
                for (int i = 1; i <= iJG; i++)//iBzNum
                {
                    //1 定位置和取颜色  画色标
                    if (iHorizontal_0 == 0)//横轴
                        X = (int)(i * (iPic_Jg + _fl_Add));
                    else
                        Y = (int)(i * (iPic_Jg + _fl_Add));

                    #region 1 画标准色标和刻度线
                    try
                    {
                        p = new Pen(iType != 0 ? Brushes.Black : Brushes.Black);//    Brushes.White);//刻度笔
                        //2 刻度
                        if (iHorizontal_0 == 0)//横轴
                            g.DrawLine(p, new Point(X, (i % 5 == 0 ? Y - 6 : Y - 2)), new Point(X, PicArea.Height));
                        else//竖轴
                            g.DrawLine(p, new Point(0, Y), new Point((i % 5 == 0 ? 4 : 2), Y));

                        #region 值
                        _flJg = flStart + (i) * fl_JG_JL;

                        strT = _flJg.ToString(_strXsw) + (iType == 0 && i == 10 ? _strDw : "") + (PicArea.Name == "P_1" ? (i == 10 ? " " : "") : "");

                        #endregion
                        if (i % i10 == 0)
                        {
                            if (iHorizontal_0 == 0)//横轴
                                g.DrawString(strT, drawFont, drawBrush, X - 7, (Y - 25), StrF);// g.DrawString(strT, drawFont, drawBrush, X - 7, (iType == 0 && i == 10 ? 26 : 26), StrF);
                            else//竖轴
                                if ((i / i10) % 2 == 1)
                                g.DrawString(strT, drawFont, drawBrush, 4, Y - 10, StrF);
                        }
                    }
                    catch { }
                }
                g.DrawLine(p, new Point(0, Y + 2), new Point((int)flPic_W, Y + 2));
                flStart = _flJg + fl_ScaleLeast;
                #endregion 1
                #region  4 刷新
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (bg != null)
                    gb.DrawImage(bg, _Rect);// 先绘制背景层
                gb.DrawImage(image, _Rect); // 再绘制绘画层

                PicArea.BackgroundImage = (Bitmap)canvas.Clone();// (Bitmap)canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Rr_Bitmap = (Bitmap)canvas.Clone();

                g.Dispose(); g = null;
                image.Dispose();
                gb.Dispose();
                canvas.Dispose();

                PicArea.Refresh();
                #endregion 刷新

                canvas.Dispose();
                image.Dispose();
            }
            catch { }
            //     flStart --;
            return flStart;
        }

        public int PlanScheme_ColorLimit_MyColor(System.Windows.Forms.PictureBox PicArea, int iJG, ref Bitmap Rr_Bitmap, int iType, int iHorizontal_0, Color[] ArrColor)
        {

            int _iRet = 0;//画图返回
            try
            {
                float flPic_W = PicArea.Width;//画布宽度
                float flPic_H = PicArea.Height;//画布高度

                #region 1.2 准备画布
                // 初始化画板，在内存中建立一块虚拟画布
                Bitmap image = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                // 获取背景层
                Bitmap bg = (Bitmap)PicArea.BackgroundImage;
                // 初始化整个画布
                Bitmap canvas = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
                // 初始化图形面板，获取这块内存画布的Graphics的引用
                Graphics g = Graphics.FromImage(image);
                Graphics gb = Graphics.FromImage(canvas);

                #endregion  准备画布
                g.Clear(Color.White);// g.Clear(Color.Teal);// CadetBlue);

                Rectangle rect = new Rectangle(0, 0, PicArea.ClientSize.Width, PicArea.ClientSize.Height);


                rect.Location = new Point(0, 0);
                LinearGradientBrush b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Horizontal);

                if (iHorizontal_0 == 0)
                {
                    if (ArrColor.Length == 4)
                    {
                        switch (iType)
                        {
                            case 0:
                                b3 = new LinearGradientBrush(rect, ArrColor[0], ArrColor[1], LinearGradientMode.Horizontal);//WhiteSmoke
                                break;
                            case 2:
                                b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Horizontal);
                                break;
                            case 1:
                                b3 = new LinearGradientBrush(rect, ArrColor[2], ArrColor[3], LinearGradientMode.Horizontal);
                                break;
                            case 3:
                                b3 = new LinearGradientBrush(rect, ArrColor[1], ArrColor[2], LinearGradientMode.Horizontal);
                                break;
                        }
                    }
                    else
                    {
                        switch (iType)
                        {
                            case 0:

                                b3 = new LinearGradientBrush(rect, Color.Blue, Color.YellowGreen, LinearGradientMode.Horizontal);//WhiteSmoke
                                break;
                            case 2:
                                b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Horizontal);
                                break;
                            case 1:
                                b3 = new LinearGradientBrush(rect, Color.Yellow, Color.Red, LinearGradientMode.Horizontal);
                                break;
                            case 3:
                                b3 = new LinearGradientBrush(rect, Color.YellowGreen, Color.Yellow, LinearGradientMode.Horizontal);
                                break;
                        }
                    }
                }
                else
                    switch (iType)
                    {
                        case 0:

                            b3 = new LinearGradientBrush(rect, Color.YellowGreen, Color.Blue, LinearGradientMode.Vertical);//WhiteSmoke
                            break;
                        case 2:
                            b3 = new LinearGradientBrush(rect, Color.Blue, Color.White, LinearGradientMode.Vertical);
                            break;
                        case 1:
                            b3 = new LinearGradientBrush(rect, Color.Red, Color.Yellow, LinearGradientMode.Vertical);
                            break;

                    }

                g.FillRectangle(b3, rect);

                if (iHorizontal_0 == 1)
                {
                    Pen p = new Pen(Brushes.Green);//刻度笔
                    int X, Y, iBzNum, iH, iXcale = 6;//位置
                    string strT = "";
                    Font drawFont = new Font("Arial", 8);//刻度字体定义
                    SolidBrush drawBrush = new SolidBrush(Color.Green);//刻度刷子

                    switch (iType)
                    {
                        case 0:
                            iBzNum = (PicArea.Height - 6) / iJG;
                            X = 0;
                            #region 1 画标准色标和刻度线
                            for (int i = 1; i <= iBzNum; i++)
                            {
                                //1 定位置和取颜色  画色标
                                Y = PicArea.Height - i * iJG;
                                try
                                {
                                    //if (i < iYsPort)
                                    p = new Pen(Brushes.White);//刻度笔

                                    //2 刻度
                                    g.DrawLine(p, new Point(X, Y), new Point(iXcale, Y));
                                    //3 画箭头
                                    //g.DrawLine(p, new Point(iW / 2 + 6, Y), new Point(iW / 2 + 6, Y + 6));
                                    //g.DrawLine(p, new Point(iW / 2 + 6 - 3, Y + 3), new Point(iW / 2 + 6, Y + 6));
                                    //g.DrawLine(p, new Point(iW / 2 + 6, Y + 6), new Point(iW / 2 + 6 + 3, Y + 3));
                                }
                                catch { }
                            }
                            #endregion 1
                            #region 2 画刻度值
                            X = iXcale + 2;
                            float _flJg = 0.0f;
                            for (int i = 1; i <= iBzNum; i += 3)
                            {
                                Y = PicArea.Height - i * iJG - 4; //(iBzNum - i) * iJG;
                                PointF drawPoint = new PointF(X, Y);

                                _flJg = (i - 1) * 0.1f;
                                strT = _flJg.ToString("0.0");
                                drawBrush = new SolidBrush(Color.White);//刻度刷子
                                g.DrawString(strT, drawFont, drawBrush, drawPoint);
                            }
                            // _iRet = _flJg;
                            #endregion 2
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                    }

                }

                #region  4 刷新
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (bg != null)
                    gb.DrawImage(bg, _Rect);// 先绘制背景层
                gb.DrawImage(image, _Rect); // 再绘制绘画层

                PicArea.BackgroundImage = (Bitmap)canvas.Clone();// (Bitmap)canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Rr_Bitmap = (Bitmap)canvas.Clone();

                g.Dispose(); g = null;
                image.Dispose();
                gb.Dispose();
                canvas.Dispose();

                PicArea.Refresh();
                #endregion 刷新

                canvas.Dispose();
                image.Dispose();


            }
            catch { }

            return _iRet;
        }



        #endregion 系统画图
        #region 光栅臂模式设定
        /// <summary>
        /// 开始时间
        /// </summary>
        public static string m_strStart_Time = "";
        /// <summary>
        /// 结束时间
        /// </summary>
        public static string m_strEnd_Time = "";
        /// <summary>
        /// 工件编号集合
        /// </summary>
        public static string m_strArrGjbh = "";

        /// <summary>
        /// 实时随动放大镜功能
        /// </summary>
        /// <param name="i_x">鼠标的X坐标</param>
        /// <param name="i_y">鼠标的Y坐标</param>
        /// <param name="offset">放大镜的偏移量</param>
        /// <param name="select_shape">选择方形显示还是圆形显示：0:圆形 1:方形</param>
        /// <param name="pic_original">需要放大的图形</param>
        /// <param name="pic_display">需要显示的图层</param>
        public static void Zoom_Tool(int i_x, int i_y, bool blShowUp, int offset, int select_shape, PictureBox pic_original, PictureBox pic_display, bool m_bl_Zoom = true,bool bl_Sz=true )
        {
            try
            {
                if (pic_original.BackgroundImage == null) return;//chendawei 190731 
              //  int originalWidth = pic_original.Width;
              //  int originalHeight = pic_original.Height;
                int originalWidth = pic_original.BackgroundImage.Width;
                int originalHeight = pic_original.BackgroundImage.Height;

                PropertyInfo rectangleProperty = pic_original.GetType().GetProperty("ImageRectangle", BindingFlags.Instance | BindingFlags.NonPublic);
                Rectangle rectangle = (Rectangle)rectangleProperty.GetValue(pic_original, null);

                int currentWidth = pic_display.Width;
                int currentHeight = pic_display.Height;

                double rate = (double)currentHeight / (double)originalHeight;    //图片缩放比率
                double rate1 = (double)currentWidth / (double)originalWidth;

                double original_x = (double)i_x / rate1;  //鼠标在缩放图片中的坐标
                double original_y = (double)i_y / rate;


                pic_original.Refresh();
                if (m_bl_Zoom)
                {
                    Graphics graphics = pic_display.CreateGraphics();     //实例化pictureBox1控件的Graphics类
                    if (blShowUp)
                    {
                        //string _strT = pic_display.Name.ToUpper();
                        //if (_strT.IndexOf("R") > 0)
                        //    i_x = 10;
                        //else
                        //    i_x = pic_display.Width - 120;
                        //i_y = 120;
                    }
                    else//显示图像上放大器和字符
                    {
                        //转换成控件在屏幕上的坐标
                        //  var screenPoint = pic_display.PointToScreen(new Point(pic_display.Width, pic_display.Height));
                        if (i_x > pic_display.Width - 120)
                            i_x -= 120;
                        //if (i_x < 120)
                        //    i_x = i_x ;

                        if (i_y < 120)
                            i_y += 120;
                    }

                    Point p = new Point(i_x, i_y);
                    int _iShow_X = (int)i_x + 10;
                    int _iShow_Y = (int)i_y - 40;
                    if (_iShow_X + 80 > pic_display.Width) _iShow_X = i_x - 80;
                    if (_iShow_Y + 80 > pic_display.Height) _iShow_Y = i_y - 80;
                    p = new Point(_iShow_X, _iShow_Y);

                    if (select_shape == 0)   //如果需要圆形画布，需要现在创建一个圆形区域，后续会再此区域中绘制
                    {
                        GraphicsPath gpath = new GraphicsPath();  //
                        gpath.AddEllipse(p.X - 60, p.Y - 60, 110, 110);//添加一个圆形区域
                        Region rg = new Region(gpath);
                        graphics.Clip = rg;  //设定绘制的区域，以后的绘图都在这个区域内
                    }

                    //声明两个Rectangle对象，分别用来指定要放大的区域和放大后的区域
                    Rectangle sourceRectangle = new Rectangle(Convert.ToInt32(i_x) - 20, Convert.ToInt32(i_y) - 20, 40, 40);  //要放大的区域 
                    Rectangle destRectangle = new Rectangle(0, 0, 80, 80);// Rectangle(p.X - 240, p.Y - 240, 240, 240);
                                                                              //调用DrawImage方法对选定区域进行重新绘制，以放大该部分
                    graphics.DrawImage(pic_original.BackgroundImage, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                    if (bl_Sz)
                    {
                        int _i_C_X = p.X + 40;
                        int _i_C_Y = p.Y + 40;
                        /// <summary>
                        /// 画点笔
                        /// </summary>
                        Pen m_p_xy = new Pen(Brushes.Red);
                        int _iLen = 5;
                        graphics.DrawLine(m_p_xy, new Point(_i_C_X - _iLen, _i_C_Y), new Point(_i_C_X + _iLen, _i_C_Y));
                        graphics.DrawLine(m_p_xy, new Point(_i_C_X, _i_C_Y - _iLen), new Point(_i_C_X, _i_C_Y + _iLen));

                        //Brush bush = new SolidBrush(Color.Green);//填充的颜色
                        //graphics.FillEllipse(bush, 10, 10, 100, 100);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50

                        //Pen pen = Pens.Yellow;  //描边，画轮廓
                        //graphics.DrawEllipse(pen, p.X - 2, p.Y - 2, 4, 4);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 当前区域产生的文件名
        /// </summary>
        public static   void Get_C_FileName(string str1="",string str2="",string strGjbh="")
        {
            if (SysInfo.m_C_Item_Info.i_C_AllRows > 0)
            {
                string _strMsg = "";
                if (SysInfo.m_W_strItemName == "")
                    _strMsg += (_strMsg == "" ? "" : ",") + "工程名称";
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 2)
                {
                    if (SysInfo.m_C_Item_Info.strGjbh == "")//strGjmc
                        _strMsg += (_strMsg == "" ? "" : ",") + "工件编号";
                }
                if (SysInfo.m_C_Item_Info.strJcwz == "")
                    _strMsg += (_strMsg == "" ? "" : ",") + "检测位置";
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 2)
                {
                    if (SysInfo.m_C_Item_Info.ID == "")
                        _strMsg += (_strMsg == "" ? "" : ",") + "检测日期";
                }
               
                if (_strMsg != "")
                {
                    MessageBox.Show(_strMsg + " 不能为空。");
                    return;
                }
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
                    _strMsg = SysInfo.m_W_strItemName + "_" + SysInfo.m_C_Item_Info.strGjbh + "_" +
                                                        SysInfo.m_C_Item_Info.strJcwz + "_" + SysInfo.m_C_Item_Info.ID + ".tdf";
                else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)
                {
                    
                    _strMsg =  SysInfo.m_W_strItemName + "_" +
                                                        SysInfo.m_C_Item_Info.strJcwz +"_("+ strGjbh+")";// + "_";
                    if (str2 != "")
                    {
                        string[] _sPara = str2.Split(' ');
                        str2 = _sPara[1];
                    }
                    string _str1 = str1 + (str2!=""?("_" +str2):"");
                    _strMsg = _str1 + "_" + _strMsg +".tdf";
                }
                else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                {
                    _strMsg = SysInfo.m_W_strItemName + "_" + SysInfo.m_C_Item_Info.strGjmc+"_"+ SysInfo.m_C_Item_Info.strGjbh;// + "_";
                   
                }
                //  if(SysInfo.m_Report_Para.m_Save_FileName=="")
                SysInfo.m_Report_Para.m_Save_FileName = _strMsg;
                SysInfo.csInter.INIWriteValue("System", "m_Save_FileName", SysInfo.m_Report_Para.m_Save_FileName, SysInfo.HardFileName);
                SysInfo.m_Report_Para.m_Save_FilePath = SysInfo.m_W_i_FilePath + "\\" + SysInfo.m_W_strItemName + SysInfo.m_ItemMark + "\\";
            }
        }
        public  string  Get_Time_S_E_2(string strS,string strE)
        {
            string strR = "";

            return strR;
        }
        public static void Set_C_Pos()
        {
            Set_L_R(SysInfo.m_SysBuff.m_Climb.i_Gsb_Start_Pos, SysInfo.m_SysBuff.m_Climb.i_Gsb_End_Pos);
        }
        public static void Set_L_R(int i_Gsb_Left, int i_Gsb_Right)
        {
            //如果是光栅臂直行，可以直接移动光栅臂
            // if (SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 == 0)
            SysInfo.m_SysBuff.m_Climb.i_Gsb_Start_Pos = i_Gsb_Left;
            SysInfo.m_SysBuff.m_Climb.i_Gsb_End_Pos = i_Gsb_Right;


            float _fl_T = 1.0f * SysInfo.m_SysBuff_C.m_Plant_C.iGsb_Len / SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotHeightmm_Y;
            SysInfo.m_SysBuff.m_Climb.i_Gsb_Show_All_Num = (int)_fl_T;
            
            _fl_T = 1.0f * i_Gsb_Left / SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotHeightmm_Y;
            SysInfo.m_SysBuff.m_Climb.i_Gsb_Show_LeftBoundary= (int)_fl_T;

            _fl_T = 1.0f * i_Gsb_Right / SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotHeightmm_Y;
            SysInfo.m_SysBuff.m_Climb.i_Gsb_Show_RightBoundary  = (int)_fl_T-1;

            SysInfo.Set_4_Gsb_Wz(SysInfo.m_SysBuff.m_Climb.i_Gsb_Start_Pos, SysInfo.m_SysBuff.m_Climb.i_Gsb_End_Pos);
        }
        /// <summary>
        /// 启动
        /// </summary>
        public static void Set_Mode_Start(int iStat=1)
        {
            if (iStat == 1)
            {
                //if(SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 3 ||
                //    SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 && SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have)
                //SysInfo.m_Climb4.SendData(1, 1, 3, 0, "2");// 自动模式关闭

                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)//C扫描
                {
                    if (SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Qj1_Ht0 == 0)
                        SysInfo.m_Climb4.SendData(1, 1, 3, 0, SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ? "3" : "7");//倒车
                    else
                        SysInfo.m_Climb4.SendData(1, 1, 3, 0, SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ? "1" : "6");//前进
                }
                else
                {
                    //if ( SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 && SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have)//樊彦伟有涂层和超声测厚
                    //{
                    //    if (SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Qj1_Ht0 == 1)
                    //        SysInfo.m_Climb4.SendData(1, 1, (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 ? 3 : 6), 0,
                    //                                  SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Only_Coat ?"16": "14");
                    //    else
                    //        SysInfo.m_Climb4.SendData(1, 1, (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 ? 3 : 6), 0, 
                    //                                  SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Only_Coat ? "17" : "15");
                    //}
                    //else
                    {
                        bool _blOk = true;
                        //if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                        //{
                        //    if (SysInfo.m_Tofd_C_Scan.m_Coat_DLL.m_i_AutoCmd == 0)//张萌的三峡项目，可以手柄控制
                        //    {
                        //        _blOk = false;
                        //    }
                        //}

                        if (_blOk)//常规
                        {
                            string _strT = SysInfo.csInter.IniReadDefine("ClassUltrasGate", "Txt_Mul_Select", "", System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini");

                            //if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 && SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Only_Coat &&
                            //    SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum > 0 && _strT.IndexOf("1") > -1)
                            //{
                            //    if (SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Qj1_Ht0 == 1)
                            //        SysInfo.m_Climb4.SendData(1, 1, 3, 0, "14");
                            //    else
                            //        SysInfo.m_Climb4.SendData(1, 1, 3, 0, "15");
                            //}
                            //else
                            {
                                if (SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Qj1_Ht0 == 1)
                                    SysInfo.m_Climb4.SendData(1, 1, (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 ? 3 : 6), 0, "1");
                                else
                                {
                                    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                                        SysInfo.m_Climb4.SendData(1, 1, 3, 0, "3");
                                    else
                                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "2");
                                }
                            }
                            //01：前进    02：后退      03：停止 04：前进左转 
                            //05：前进右转 06：后退左转 07：后退右转)
                        }
                    }
                }
            }
            else
            {
               
                {
                    SysInfo.m_Climb4.SendData(1, 1, 3, 0, "2");
                    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "1");
                    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
                }
            }
        }
        /// <summary>
        /// 车体联机
        /// </summary>
        public static void  Link_Climb_Com(int iType, string strCom)
        {
            SysInfo.m_Climb4.m_blLink = false;
            SysInfo.m_Climb4.m_blCom1_Can0 = iType;
            if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 0) return;

            SysInfo.m_Climb4.CloseSet();

            SysInfo.csInter.INIWriteValue("COM_Can", "m_blCom1_Can0", SysInfo.m_Climb4.m_blCom1_Can0.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue("COM_Can", "COM", strCom, SysInfo.HardFileName);

            if (SysInfo.m_Climb4.m_blCom1_Can0 == 0)
                SysInfo.m_Climb4.InitCan();
            else
            {
                SysInfo.m_Climb4.InitCom(strCom, SysInfo.m_Climb4.strBtl);

                DateTime dtStar = DateTime.Now;
                while (true)
                {
                    try
                    {
                        if (SysInfo.m_Climb4.m_blLink) break;
                        Application.DoEvents();
                        if (DateTime.Now.Subtract(dtStar).TotalSeconds > 6) break;
                        System.Threading.Thread.Sleep(10);
                    }
                    catch (Exception EE)
                    { break; }
                }
            }
        }
        public static void Set_4_Gsb_Wz(int i_Gsb_Left, int i_Gsb_Right)
        {
            try
            {
                if (i_Gsb_Left < 0 || i_Gsb_Left >= SysInfo.m_SysBuff.m_Climb.iGsb_Len)
                {
                    i_Gsb_Left = 0;
                }
            }
            catch { i_Gsb_Left = 0; }

            string _strT = SysInfo.csInter.IniReadDefine("ClassUltrasGate", "Txt_Mul_Select", "", System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini");

         
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 3)
            {
                if (SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 == 1)
                    SysInfo.m_Climb4.SendData(2, 1, 0, 0, i_Gsb_Left.ToString() + "," + i_Gsb_Right.ToString());
                else
                    SysInfo.m_Climb4.SendData(2, 1, 1, 0, i_Gsb_Left.ToString() + "," + i_Gsb_Left.ToString());
            }
            SysInfo.m_SysBuff.m_Climb.i_Gsb_Start_Pos = i_Gsb_Left;
            SysInfo.m_SysBuff.m_Climb.i_Gsb_End_Pos = i_Gsb_Right;
        }
        public static void SetSpeed()
        {
            //bool _blSendSpeed = false;
            //if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || m_i_TOFD_0_Cscan_1_Mui_2 == 2)
            //    _blSendSpeed = true;
            //else
            //    _blSendSpeed = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1;
            //     if (_blSendSpeed)

            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 3 || (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 && SysInfo.m_i_Climb_Hand0_Auto1 == 0))
            {
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                    SysInfo.m_Climb4.SendData((SysInfo.m_i_Climb_Hand0_Auto1 == 1 ? 2 : 4), 2,//4自动 2手动SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_BScanMode == 1
                              0, 0, (SysInfo.m_SysBuff.m_Climb.Speed.ToString() + "," + SysInfo.m_SysBuff.m_Climb.i_Para_Speed_Gsb.ToString()));

                else
                    SysInfo.m_Climb4.SendData((SysInfo.m_i_Climb_Hand0_Auto1 == 1 ? 4 : 2), 2,//4自动 2手动SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_BScanMode == 1
                                                  0, 0, (SysInfo.m_SysBuff.m_Climb.Speed.ToString() + "," + SysInfo.m_SysBuff.m_Climb.i_Para_Speed_Gsb.ToString()));
            }
        }
        #endregion 模式设定
        /// <summary>
        /// 添加探头补充参数
        /// </summary>
        public static  void Get_UI()
        {
            int _iCurr = SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan;
            SysInfo.m_Report_Para.Txt_PL = SysInfo.csInter.INIReadValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "Txt_PL", "5", SysInfo.HardFileName);
            SysInfo.m_Report_Para.Txt_Jpcc = SysInfo.csInter.INIReadValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "Txt_Jpcc", "6", SysInfo.HardFileName);
            SysInfo.m_Report_Para.Txt_Xkjd = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurr].m_fPcsAngle.ToString();
            SysInfo.m_Report_Para.Txt_PCS = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurr].m_fPcsLen.ToString();
            SysInfo.m_Report_Para.Txt_Scbj = SysInfo.m_SysBuff.m_Tofd_DLL.m_i_1_mm.ToString();
            SysInfo.m_Report_Para.Txt_Scfs = SysInfo.csInter.INIReadValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "Txt_Scfs", "非平衡扫查", SysInfo.HardFileName);
        }
        public static int Jug_SaveData( )
        {
            int _iRet = 0;
           if(SysInfo.m_Report_Para.m_Save_FilePath=="") Get_C_FileName(m_strStart_Time ,m_strEnd_Time ,m_strArrGjbh );

            string _strTitl = "", _strMsg = "";

            if (SysInfo.m_iLanguage == 0)
            {
             
                _strTitl = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 2 ? "区域多行数据保存" : "区域多行数据保存，文件名定义方法 : 开始/结束时间_项目名称_位置_多个设备编号";

                _strMsg = "1 本区域：" + (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3? 
                            SysInfo.m_C_Item_Info .strGjmc :
                            SysInfo.m_C_Item_Info.strJcwz )+ "\r\n";
                _strMsg += "2 总共检测行数：" + SysInfo.m_C_Item_Info.i_C_AllRows + "行" + "\r\n";
                _strMsg += "3 将保存到一个文件夹:" + SysInfo.m_Report_Para.m_Save_FilePath + "\r\n";
                _strMsg += "4 文件名：" + SysInfo.m_Report_Para.m_Save_FileName + "\r\n\r\n";

                _strMsg += "   要     保     存     数     据     吗？\r\n\r\n       保存：Yes\r\n\r\n       不保存，本区域还没有全部检测完成：No";
            }
            else
            {
                _strTitl = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 2 ? "Area multi-line data saving" : "Area multi-line data saving, file name definition method: start/end time _ project name _ location _ multiple device numbers";
                _strMsg = "1 Local detection area：" + (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 ?
                            SysInfo.m_C_Item_Info.strGjmc :
                            SysInfo.m_C_Item_Info.strJcwz) + "\r\n";

              //  _strMsg = "1 Local detection area：" + SysInfo.m_C_Item_Info.strJcwz + "\r\n";
                _strMsg += "2 Total number of detected rows：" + SysInfo.m_C_Item_Info.i_C_AllRows + "行" + "\r\n";
                _strMsg += "3 Will save to a folder:" + SysInfo.m_Report_Para.m_Save_FilePath + "\r\n";
                _strMsg += "4 Filename：" + SysInfo.m_Report_Para.m_Save_FileName + "\r\n\r\n";

                _strMsg += "Do you want to save data?\r\n\r\n       Save：Yes\r\n\r\n       No, this area is not fully checked：No";

            }
            SysInfo.m_bl_JL = false;
            SysInfo.m_str_JL = _strMsg;
            //if (SysInfo.m_frm_Stop!=null )
            //SysInfo.m_frm_Stop.ShowDialog();

            if (SysInfo.m_bl_JL==false )

       //         DialogResult dr = MessageBox.Show(_strMsg, _strTitl, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
       //     if (dr == DialogResult.No)
                return _iRet;
            _iRet = 1;
            SysInfo.m_C_Item_Info.i_S1_E2_N0 = 2;

            return _iRet;
        }
        public static int Jug_SaveData_Num(int iAll_RecordNum)
        {
            int _iRet = 0;

            string _strTitl = "区域多行数据保存";
            string _strMsg = "1 本区域：" + SysInfo.m_C_Item_Info.strJcwz + "\r\n";
            _strMsg += "2 总共检测行数：" + iAll_RecordNum + "行" + "\r\n";
            _strMsg += "3 将保存到一个文件夹:" + SysInfo.m_Report_Para.m_Save_FilePath + "\r\n";
            _strMsg += "4 文件名：" + SysInfo.m_Report_Para.m_Save_FileName + "\r\n\r\n";

            _strMsg += "5 要保存数据吗？\r\n\r\n保存：Yes\r\n不保存，本区域还没有全部检测完成：No";

            DialogResult dr = MessageBox.Show(_strMsg, _strTitl, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dr == DialogResult.No)
                return _iRet;
            _iRet = 1;
            SysInfo.m_C_Item_Info.i_S1_E2_N0 = 2;

            return _iRet;
        }
        #region 列表函数

        /// <summary>
        /// 列表界面初始化 0:项目  1：焊缝 2：报警 3: 打印的项目名称  4:项目检测记录  5:项目检测记录的内容   6:缺陷记录
        /// </summary>
        /// <param name="Dg_TestItem"></param>
        /// <param name="iType">0:项目  1：焊缝 2：报警 3: 打印的项目名称  4:项目检测记录  5:项目检测记录的内容   6:缺陷记录
        /// <returns></returns>
        public static float Grd_IniGrid_ShowZdName(DataGridView Dg_TestItem, int iType)
        {
            float _flRet = 0;
            try
            {
                string strSection = "";//搜寻配置文件主键
                string strTmp = "";
                //1 读要求显示字段个数
                int iMax = 0;
                string[] Para = null;
                int iNo = 0;//字段序号
                string[] _ArrTitl = null;  //  #region 1 获得显示字段配置主键  字段名|标题名|文本框或下拉框(0-文本框,1-下拉框)|是否显示(0-不显示,1-显示)|列宽度
                switch (iType)
                {
                    case 0://项目记录    
                        #region
                        strSection = "IniGrid_Item";
                        iNo = 0;
                        iMax = Int32.Parse(csInter.INIReadValue(strSection, "sum", "0", HardFileName));
                        if (iMax == 0)
                        {
                            //  csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Row|行号|0|1|60", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_ID|ID|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_Dwmc|单位名称|0|1|150", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_ItemName|项目名称|0|1|120", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_Sbbh|设备编号|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_Testblock|试块名称|0|1|80", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_Stand|检测依据|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_Jyy|检验员|0|1|90", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "sum", iNo.ToString(), HardFileName);
                            iMax = iNo;
                        }
                        #endregion
                        break;
                    case 1://焊缝记录
                        #region
                        iNo = 0;
                        strSection = "IniGrid_Weld";
                        iMax = Int32.Parse(csInter.INIReadValue(strSection, "sum", "0", HardFileName));
                        if (iMax == 0)
                        {
                            //  csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Row|行号|0|1|60", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Sub_ID|焊缝ID|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Part_No|焊缝编号|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "DetectionSite|外壁/内壁|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flThicknise|厚度|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "ProbeSpacing|探头间距|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "ReMark|备注|0|1|190", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "sum", iNo.ToString(), HardFileName);
                            iMax = iNo;
                        }
                        #endregion
                        break;
                    #region 删除
                    //case 2://异常数据
                    //    #region
                    //    iNo = 0;
                    //    strSection = "IniGrid_Alarm";
                    //    iMax = Int32.Parse(csInter.INIReadValue(strSection, "sum", "0", HardFileName));
                    //    if (iMax == 0)
                    //    {
                    //    //    csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Row|行号|0|1|60", HardFileName); iNo++;

                    //        csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Sub_ID|焊缝ID|0|1|120", HardFileName); iNo++;
                    //        csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Part_No|焊缝编号|0|1|120", HardFileName); iNo++;
                    //        csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "strType|类型|0|1|120", HardFileName); iNo++;
                    //        csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flDistancX_Start|开始距离|0|1|90", HardFileName); iNo++;
                    //        csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flDistancX_End|结束距离|0|1|90", HardFileName); iNo++;

                    //        csInter.INIWriteValue(strSection, "sum", iNo.ToString(), HardFileName);
                    //        iMax = iNo;
                    //    }
                    //    #endregion
                    //    break;
                    #endregion 删除
                    case 2://异常数据标注
                        #region
                        iNo = 0;
                        strSection = "IniGrid_TOFD_Bz";
                        iMax = Int32.Parse(csInter.INIReadValue(strSection, "sum", "0", HardFileName));
                        if (iMax == 0)
                        {
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "ID|项目ID|0|1|1", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Sub_ID|焊缝ID|0|1|1", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Part_No|焊缝编号|0|1|1", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "strType|缺陷类型|0|1|120", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flLen|长度|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flLen_S|开始距离|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flLen_E|结束距离|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "iX_No|X轴No|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "iY_No|Y轴No|0|1|90", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flHeight|高度|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flHeight_S|开始时间|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flHeight_E|结束时间|0|1|90", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flDepth|深度|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flDepth_S|深度开始时间|0|1|90", HardFileName); iNo++;
                          
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Zldj|质量等级|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Remark|备注|0|1|120", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "sum", iNo.ToString(), HardFileName);
                            iMax = iNo;
                        }
                        #endregion
                        break;
                    case 3://打印的项目名称 
                        #region
                        strSection = "IniGrid_Print_Item";
                        iNo = 0;
                        iMax = Int32.Parse(csInter.INIReadValue(strSection, "sum", "0", HardFileName));
                   //     if (iMax == 0)
                        {
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "strItemName|项目名称|0|1|350", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "sum", iNo.ToString(), HardFileName);
                            iMax = iNo;
                        }
                        #endregion
                        break;
                    case 4://项目检测记录 
                        #region
                        strSection = "IniGrid_Print_Record";
                        iNo = 0;
                        iMax = Int32.Parse(csInter.INIReadValue(strSection, "sum", "0", HardFileName));
                   //     if (iMax == 0)
                        {
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "strRecordName|焊缝记录文件|0|1|350", HardFileName); iNo++;
                         
                            csInter.INIWriteValue(strSection, "sum", iNo.ToString(), HardFileName);
                            iMax = iNo;
                        }
                        #endregion
                        break;
                    case 5://项目检测记录的内容
                        #region
                        strSection = "IniGrid_Print_RecordCont";
                        iNo = 0;
                        iMax = Int32.Parse(csInter.INIReadValue(strSection, "sum", "0", HardFileName));
                        //if (iMax == 0)
                        {
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Wtdw|委托单位|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Gcmc|工程名称|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Gjmc|工件名称|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Gjbh|工件编号|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jcff|检测方法|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jcrq|检测日期|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jcjgmc|检测机构名称|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jcjgdz|检测机构地址|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Yb|邮编|0|1|120", HardFileName); iNo++;
                          
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Bg_Bgbh|报告编号|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Bz|备注|0|1|120", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Bg_Jlbh|记录编号|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jcr|检测人|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Shr|审核人|0|1|120", HardFileName); iNo++;
                          
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Gg|规格|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_CL|材料|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Hjff|焊接方法|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Pkxs|坡口型式|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Rclzt|热处理状态|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Bmzt|表面状态|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jcbw|检测部位|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jcsj|检测时机|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Bmwd|表面温度|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Cysblb|承压设备类别|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jcbl|检测比例|0|1|120", HardFileName); iNo++;
                        
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jcbz|检测标准|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Hgjb|合格级别|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jsdj|技术等级|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_CzZdsbh|操作指导书编号|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Yqmc|仪器名称|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Yqxh|仪器型号|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Yqbh|仪器编号|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Sczz|扫查装置|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Sk|试块|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Ohj|耦合剂|0|1|120", HardFileName); iNo++;
                     
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Wd|检测温度|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jcm|检测面|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jcqy|检测区域|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Tt_Td|通道|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Tt_Xh|探头型号|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Tt_Bh|探头编号|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Tt_Lmd|灵敏度设置|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Tt_Sjck|时间窗口设置|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "m_Save_FilePath|路径|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "m_Save_FileName|文件名|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_PL|频率|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Jpcc|镜片尺寸|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Xkjd|楔块角度|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_PCS|PCS|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Scbj|扫查步进|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Txt_Scfs|扫查方式|0|1|120", HardFileName); iNo++;
                    
                            csInter.INIWriteValue(strSection, "sum", iNo.ToString(), HardFileName);
                            iMax = iNo;
                        }
                        #endregion
                        break;
                    case 6://缺陷记录
                        #region
                        strSection = "IniGrid_Print_Alarm";
                        iNo = 0;
                        iMax = Int32.Parse(csInter.INIReadValue(strSection, "sum", "0", HardFileName));
                  //      if (iMax == 0)
                        {
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Hfbh|焊缝编号|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Jccd|检测长度|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Qxbh|缺陷编号|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Qxwz|缺陷位置x|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Cd|长度|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Sd|深度|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Gd|高度|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Lx|缺陷类型|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Zldj|质量等级|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Remark|备注|0|1|120", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "sum", iNo.ToString(), HardFileName);
                            iMax = iNo;
                        }
                        #endregion
                        break;
                    default:
                        break;
                }
                // #endregion

                Dg_TestItem.RowCount = 2;//总行数
                Dg_TestItem.ColumnCount = 1;

                //1 列号  4轮C扫查显示当前数据是第几排  ，下面行为光栅臂的位置
                Dg_TestItem[0, 0].Value = "NO";
                Dg_TestItem[0, 0].Style.BackColor = System.Drawing.Color.SeaShell;
                Dg_TestItem[0, 0].ReadOnly = true;//定义第二列是选择列

                if (iType == 4)//添加单选列
                {
                    DataGridViewCheckBoxColumn txtCol_Ck = new DataGridViewCheckBoxColumn();
                    txtCol_Ck.Name = "CKE";//字段名称

                    txtCol_Ck.HeaderText = "选中";//标题头名称

                    // txtCol_Ck.ReadOnly = false;
                    txtCol_Ck.TrueValue = true;
                    txtCol_Ck.FalseValue = false;
                    txtCol_Ck.Width = 30;
                    txtCol_Ck.Resizable = DataGridViewTriState.False;
                    //  txtCol_Ck.DataPropertyName = "CKE";
                    Dg_TestItem.Columns.Add(txtCol_Ck);//列表增加此列
                }
                #region 2 画列表头
                try
                {
                    for (int i = 0; i < iMax; i++)
                    {
                        //        Application.DoEvents();
                        //2 读当前字段显示值：  0：字段名|       1：标题|      :2：文本框或下拉框(0-文本框,1-下拉框)|     :3： 此字段是否显示(0-不显示,1-显示)|     :4： 列宽度
                        //if (iType == 1)
                        //    strTmp = _ArrTitl[i];
                        //else
                        strTmp = csInter.INIReadValue(strSection, "P" + i.ToString(), "", HardFileName);
                        Para = strTmp.Split('|');
                        //3 要求显示此字段
                        if (Para.Length > 3)
                            if (Para[3].Replace(" ", "") == "1")
                            {
                                try
                                {
                                    //4 添加此列数据
                                    DataGridViewTextBoxColumn txtCol = new DataGridViewTextBoxColumn();
                                    txtCol.Name = Para[0].ToUpper();//字段名称

                                    txtCol.HeaderText = Para[1];//标题头名称
                                    if (Para.Length == 5)
                                    {
                                        txtCol.Width = int.Parse(Para[4]);//列宽度
                                    }
                                    else
                                        txtCol.Width = 80;//列宽度
                                    txtCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                    Dg_TestItem.Columns.Add(txtCol);//列表增加此列

                                    Dg_TestItem[Para[0], 0].Value = Para[1];//标题头名称
                                    Dg_TestItem[Para[0], 0].Style.BackColor = System.Drawing.Color.SeaShell;
                                    Dg_TestItem[Para[0], 0].ReadOnly = true;
                                    Dg_TestItem[Para[0], 0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    Dg_TestItem[Para[0], 0].Tag = (object)Para[2];//记录下此列显示 文本/下拉框，供点击时判断使用

                                    //      _strZdNameList += ((_strZdNameList == "" ? "" : ",") + txtCol.Name);//获得配置文件中要求显示的方案表字段名称
                                }
                                catch (Exception e)
                                { }
                            }
                    }
                }
                catch (Exception e)
                {

                }
                #endregion 2
                Dg_TestItem.Rows[0].Frozen = true;//列头行固定
                Dg_TestItem.Columns[0].Frozen = true;//列头第1列固定
                Dg_TestItem.Columns[0].Width = 50;
               
                switch (iType)
                {//1: 全部数据  2: 报警数据  3: 查询指定数据 4: 伤面积数据  5: 视频数据 6:单位名牌信息 7:波形原始数据 8:伤点统计列表</param>
                    case 1:
                        Dg_TestItem.Columns[1].Frozen = true;//列头第1列固定
                        break;

                    default:
                        if (iType == 2)
                        {
                            Dg_TestItem.Columns[0].Frozen = true;//列头第1列固定
                            Dg_TestItem.Columns[3].Frozen = true;

                        }
                        else if (iType < 5)
                        {
                            Dg_TestItem.Columns[1].Frozen = true;//列头第1列固定
                            Dg_TestItem.Columns[2].Frozen = true;
                        }
                        break;
                }

                Dg_TestItem.ColumnHeadersVisible = false;// 列标题不显示
                Dg_TestItem.RowHeadersVisible = false;
            }
            catch (Exception Ex)
            {
                /// m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
            }
            return _flRet;
        }
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="Dg_TestItem"></param>
        /// <param name="iR">0-N</param>
        public static bool Grd_AddData(DataGridView Dg_TestItem, int iR, Class_Test_Item Test_Item)
        {
            bool _blRet = false;
            try
            {
                #region 如果当前行大于列表行，添加新行
                int iRow = iR + 1;//1-N
                int iCol = 1;
                //if (iRow + 1 > Dg_TestItem.RowCount)
                //    Dg_TestItem.RowCount = iRow + 1;
                #endregion
                if (iRow + 1 > Dg_TestItem.RowCount) return _blRet;
                if (iCol + 1 > Dg_TestItem.ColumnCount)
                    return _blRet;
                if (iCol < 0 || iRow < 1) return _blRet;
                try
                {
                    for (int i = 0; i < Dg_TestItem.ColumnCount; i++)
                        Dg_TestItem[i, iRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch { }
                Dg_TestItem[iCol++, iRow].Value = iRow.ToString();

                Dg_TestItem[iCol++, iRow].Value = Test_Item.ID;
                Dg_TestItem[iCol++, iRow].Value = Test_Item.Dwmc;
                Dg_TestItem[iCol++, iRow].Value = Test_Item.ItemName;
                Dg_TestItem[iCol++, iRow].Value = Test_Item.Sbbh;
                Dg_TestItem[iCol++, iRow].Value = Test_Item.Testblock;
                Dg_TestItem[iCol++, iRow].Value = Test_Item.Testing_Standard;
                Dg_TestItem[iCol++, iRow].Value = Test_Item.strJyy;

                Dg_TestItem.FirstDisplayedScrollingRowIndex = iRow;
                _blRet = true;
            }
            catch (Exception e3)
            {

            }//MessageBox.Show("Grd_AddData：" + e3.Message + e3.StackTrace); }
            return _blRet;
        }

        /// <summary>
        /// 焊缝列表
        /// </summary>
        /// <param name="Dg_TestItem"></param>
        /// <param name="iR">0-N</param>
        public static bool Grd_AddData(DataGridView Dg_TestItem, int iR, Class_Test_Parts Test_Part)
        {
            bool _blRet = false;
            try
            {
                #region 如果当前行大于列表行，添加新行
                int iRow = iR + 1;//1-N
                int iCol = 0;
                //if (iRow + 1 > Dg_TestItem.RowCount)
                //    Dg_TestItem.RowCount = iRow + 1;
                #endregion
                if (iRow + 1 > Dg_TestItem.RowCount) return _blRet;
                if (iCol + 1 > Dg_TestItem.ColumnCount)
                    return _blRet;
                if (iCol < 0 || iRow < 1) return _blRet;
                try
                {
                    for (int i = 0; i < Dg_TestItem.ColumnCount; i++)
                        Dg_TestItem[i, iRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch { }
                Dg_TestItem[iCol, iRow].Value = iRow.ToString(); iCol++;
                //     Dg_TestItem[iCol++, iRow].Value = Test_Part.ID;
                Dg_TestItem[iCol, iRow].Value = Test_Part.Sub_ID; iCol++;
                Dg_TestItem[iCol, iRow].Value = Test_Part.Part_No; iCol++;
                Dg_TestItem[iCol, iRow].Value = Test_Part.DetectionSite == 0 ? "外壁" : "内壁"; iCol++;
                Dg_TestItem[iCol, iRow].Value = Test_Part.flThicknise; iCol++;
                Dg_TestItem[iCol, iRow].Value = Test_Part.ProbeSpacing; iCol++;
                Dg_TestItem[iCol, iRow].Value = Test_Part.ReMark;

                //        Dg_TestItem.FirstDisplayedScrollingRowIndex = iRow;
                _blRet = true;
            }
            catch (Exception e3)
            {
                //csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Sub_ID|焊缝ID|0|1|90", HardFileName); iNo++;
                //csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Part_No|焊缝编号|0|1|90", HardFileName); iNo++;
                //csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "DetectionSite|外壁/内壁|0|1|90", HardFileName); iNo++;
                //csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flThicknise|厚度|0|1|90", HardFileName); iNo++;
                //csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "ProbeSpacing|探头间距|0|1|90", HardFileName); iNo++;
                //csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "ReMark|备注|0|1|190", HardFileName); iNo++;

            }//MessageBox.Show("Grd_AddData：" + e3.Message + e3.StackTrace); }
            return _blRet;
        }
        /// <summary>
        /// 报警列表
        /// </summary>
        /// <param name="Dg_TestItem"></param>
        /// <param name="iR">0-N</param>
        public static bool Grd_AddData(DataGridView Dg_TestItem, int iR, Class_Test_AlarmArea Test_Alarm)
        {
            bool _blRet = false;
            try
            {
                #region 如果当前行大于列表行，添加新行
                int iRow = iR + 1;//1-N
                int iCol = 0;

                #endregion
                if (iRow + 1 > Dg_TestItem.RowCount) return _blRet;
                if (iCol + 1 > Dg_TestItem.ColumnCount)
                    return _blRet;
                if (iCol < 0 || iRow < 1) return _blRet;
                try
                {
                    for (int i = 0; i < Dg_TestItem.ColumnCount; i++)
                        Dg_TestItem[i, iRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch { }
                Dg_TestItem[iCol++, iRow].Value = iRow.ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.ID;//项目ID
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.Sub_ID;//焊缝ID



                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.Part_No + "/" + iRow;//缺陷编号
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.strType.ToString();//缺陷类型
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flLen.ToString();//长度

                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flLen_S.ToString();//开始距离
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flLen_E;//结束距离

                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.iX_No  .ToString();//X轴No
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.iY_No .ToString();//Y轴No

                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flHeight.ToString();//高度
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flHeight_S.ToString ();//开始时间
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flHeight_E.ToString ();////结束时间

                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flDepth.ToString();//深度
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flDepth_S;//深度开始时间

                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.strZldj.ToString();//质量等级
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.Remark.ToString();//备注



                //Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flLen_S.ToString();//开始距离
                //Dg_TestItem[iCol++, iRow].Value = Test_Alarm.Part_No + "/" + iRow;//缺陷编号
                //Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flLen_S.ToString();//缺陷位置x

                //Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flDepth.ToString();//深度
                //Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flHeight.ToString();//高度

                //Dg_TestItem[iCol++, iRow].Value = Test_Alarm.strZldj.ToString();//质量等级
                //Dg_TestItem[iCol++, iRow].Value = Test_Alarm.Remark.ToString();//备注


                Dg_TestItem.FirstDisplayedScrollingRowIndex = iRow;
                _blRet = true;
            }
            catch (Exception e3)
            {

            }//MessageBox.Show("Grd_AddData：" + e3.Message + e3.StackTrace); }
            return _blRet;
        }
        /// <summary>
        /// 打印：项目名称
        /// </summary>
        /// <param name="Dg_TestItem"></param>
        /// <param name="iR"></param>
        /// <param name="Test_Data"></param>
        /// <returns></returns>
        public static bool Grd_AddData(DataGridView Dg_TestItem, int iR, Cl_Print_Item Test_Data)
        {
            bool _blRet = false;
            try
            {
                #region 如果当前行大于列表行，添加新行
                int iRow = iR + 1;//1-N
                int iCol = 0;

                #endregion
                if (iRow  > Dg_TestItem.RowCount) return _blRet;
                if (iCol + 1 > Dg_TestItem.ColumnCount)
                    return _blRet;
                if (iCol < 0 || iRow < 1) return _blRet;
                try
                {
                    for (int i = 0; i < Dg_TestItem.ColumnCount; i++)
                        Dg_TestItem[i, iRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch { }
                Dg_TestItem[iCol++, iRow].Value = iRow.ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Data.strItemName;

                Dg_TestItem.FirstDisplayedScrollingRowIndex = iRow;
                _blRet = true;
            }
            catch (Exception e3)
            {

            }//MessageBox.Show("Grd_AddData：" + e3.Message + e3.StackTrace); }
            return _blRet;
        }
        public static bool Grd_AddData_Print(DataGridView Dg_TestItem, int iR, Class_Test_AlarmArea Test_Alarm)
        {
            bool _blRet = false;
            try
            {
                #region 如果当前行大于列表行，添加新行
                int iRow = iR + 1;//1-N
                int iCol = 0;

                #endregion
                if (iRow + 1 > Dg_TestItem.RowCount) return _blRet;
                if (iCol + 1 > Dg_TestItem.ColumnCount)
                    return _blRet;
                if (iCol < 0 || iRow < 1) return _blRet;
                try
                {
                    for (int i = 0; i < Dg_TestItem.ColumnCount; i++)
                        Dg_TestItem[i, iRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch { }
                Dg_TestItem[iCol++, iRow].Value = iRow.ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.ID;//项目ID
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.Sub_ID;//焊缝ID
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.Part_No;//焊缝编号
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.strType.ToString();//缺陷类型

                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flLen.ToString();//长度
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flLen_S.ToString();//检测长度
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flLen_E.ToString();//开始距离
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.iX_No.ToString();//X轴No
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.iY_No.ToString();//Y轴No

                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flHeight.ToString();//高度
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flHeight_S.ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flHeight_E.ToString();

                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flDepth.ToString();//深度
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flDepth_S.ToString();//深度开始时间

                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.strZldj.ToString();//质量等级
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.Remark.ToString();//备注

                Dg_TestItem.FirstDisplayedScrollingRowIndex = iRow;
                _blRet = true;
            }
            catch (Exception e3)
            {

            }//MessageBox.Show("Grd_AddData：" + e3.Message + e3.StackTrace); }
            return _blRet;
        }

        /// <summary>
        /// 打印：项目检测记录
        /// </summary>
        /// <param name="Dg_TestItem"></param>
        /// <param name="iR"></param>
        /// <param name="Test_Data"></param>
        /// <returns></returns>
        public static bool Grd_AddData(DataGridView Dg_TestItem, int iR, Cl_Print_Record Test_Data)
        {
            bool _blRet = false;
            try
            {
                #region 如果当前行大于列表行，添加新行
                int iRow = iR + 1;//1-N
                int iCol = 0;

                #endregion
                if (iRow + 1 > Dg_TestItem.RowCount)
                    return _blRet;
                if (iCol + 1 > Dg_TestItem.ColumnCount)
                    return _blRet;
                if (iCol < 0 || iRow < 1) return _blRet;
                try
                {
                    for (int i = 0; i < Dg_TestItem.ColumnCount; i++)
                        Dg_TestItem[i, iRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch { }
                Dg_TestItem[iCol++, iRow].Value = iRow.ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Data.blCheck;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.strRecordName;

                Dg_TestItem.FirstDisplayedScrollingRowIndex = iRow;
                _blRet = true;
            }
            catch (Exception e3)
            {

            }//MessageBox.Show("Grd_AddData：" + e3.Message + e3.StackTrace); }
            return _blRet;
        }
        /// <summary>
        /// 打印：当前项目检测记录的内容
        /// </summary>
        /// <param name="Dg_TestItem"></param>
        /// <param name="iR"></param>
        /// <param name="Test_Data"></param>
        /// <returns></returns>
        public static bool Grd_AddData(DataGridView Dg_TestItem, int iR, Cls_Report_P Test_Data)
        {
            bool _blRet = false;
            try
            {
                #region 如果当前行大于列表行，添加新行
                int iRow = iR + 1;//1-N
                int iCol = 0;

                #endregion
                if (iRow + 1 > Dg_TestItem.RowCount) return _blRet;
                if (iCol + 1 > Dg_TestItem.ColumnCount)
                    return _blRet;
                if (iCol < 0 || iRow < 1) return _blRet;
                try
                {
                    for (int i = 0; i < Dg_TestItem.ColumnCount; i++)
                        Dg_TestItem[i, iRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch { }
                Dg_TestItem[iCol++, iRow].Value = iRow.ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Wtdw;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Gcmc;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Gjmc;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Gjbh;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jcff;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jcrq;

                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jcjgmc;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jcjgdz;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Yb;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Bg_Bgbh;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Bz;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Bg_Jlbh;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jcr;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Shr;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Gg;

                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_CL;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Hjff;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Pkxs;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Rclzt;

                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Bmzt;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jcbw;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jcsj;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Bmwd;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Cysblb;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jcbl;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jcbz;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Hgjb;

                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jsdj;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_CzZdsbh;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Yqmc;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Yqxh;

                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Yqbh;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Sczz;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Sk;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Ohj;

                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Wd;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jcm;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jcqy;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Tt_Td;

                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Tt_Xh;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Tt_Bh;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Tt_Lmd;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Tt_Sjck;

                Dg_TestItem[iCol++, iRow].Value = Test_Data.m_Save_FilePath;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.m_Save_FileName;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_PL;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Jpcc;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Xkjd;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_PCS;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Scbj;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Txt_Scfs;
              
                Dg_TestItem.FirstDisplayedScrollingRowIndex = iRow;
                _blRet = true;
            }
            catch (Exception e3)
            {

            }//MessageBox.Show("Grd_AddData：" + e3.Message + e3.StackTrace); }
            return _blRet;
        }

        /// <summary>
        /// 打印：项目检测记录
        /// </summary>
        /// <param name="Dg_TestItem"></param>
        /// <param name="iR"></param>
        /// <param name="Test_Data"></param>
        /// <returns></returns>
        public static bool Grd_AddData(DataGridView Dg_TestItem, int iR,int iNo, Class_Test_AlarmArea Test_Data)
        {
            bool _blRet = false;
            try
            {
                #region 如果当前行大于列表行，添加新行
                int iRow = iR + 1;//1-N
                int iCol = 0;

                #endregion
                if (iRow + 1 > Dg_TestItem.RowCount) return _blRet;
                if (iCol + 1 > Dg_TestItem.ColumnCount)
                    return _blRet;
                if (iCol < 0 || iRow < 1) return _blRet;
                try
                {
                    for (int i = 0; i < Dg_TestItem.ColumnCount; i++)
                        Dg_TestItem[i, iRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch { }
                Dg_TestItem[iCol++, iRow].Value = iRow.ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Sub_ID;//
                Dg_TestItem[iCol++, iRow].Value = Test_Data.flLen_S;
                Dg_TestItem[iCol++, iRow].Value = "";//缺陷编号
                Dg_TestItem[iCol++, iRow].Value = Test_Data.flLen_S;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.flLen;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.flDepth;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.flHeight;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.strType;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.strZldj;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.strFileName;
                Dg_TestItem[iCol++, iRow].Value = Test_Data.Remark;

                Dg_TestItem.FirstDisplayedScrollingRowIndex = iRow;
                _blRet = true;
            }
            catch (Exception e3)
            {

            }//MessageBox.Show("Grd_AddData：" + e3.Message + e3.StackTrace); }
            return _blRet;
        }
        #endregion  列表函数

        /// <summary>
        /// 高度计算
        /// </summary>
        /// <param name="flTime"></param>
        /// <returns></returns>
        public static float GetDistanc(float flTime, int iType = 0)
        {
            double _d = 0; int _iN0 = SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan;
            float _f1 = 0, _fl_Distan = 0;

            _f1 = flTime * SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iN0].m_dSpeed / 1000f;// 512f;
            _f1 /= 2.0f;
            _fl_Distan = _f1;
            _f1 *= _f1;

            float _f2 = 0;// SysInfo.m_BiaoZhu.flTLW_mm;/// 2.0f;// (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iN0].m_fPcsLen) / 2;// -24;
            
            if(_f2==0)
            {
                _f2 = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iN0].m_fPcsLen / 2.0f;
            }
            _f2 *= _f2;
            _d = Math.Sqrt((double)(_f1 - _f2));// *2;
            if (_f1 < _f2) _d = 0;
            return iType == 0 ? _fl_Distan : (float)_d;
        }

        /// <summary>
        /// 获得当前数据表名称  0: 焊缝 1:原始记录 2:异常数据
        /// </summary>
        /// <param name="iType">0: 焊缝 1:原始记录 2:异常数据</param>
        /// <returns></returns>
        public static string GetCurrDataTableName(int iType, string ID, string Dwmc)
        {
            string strType = "";
            string _DataTableName = "";
            switch (iType)
            {
                case 0://焊缝
                    strType = "Part";
                    break;
                case 1://原始记录
                    strType = "Record";
                    break;
                case 2://异常数据
                    strType = "Alarm";
                    break;
                case 3:
                    strType = "EXCEL";
                    break;
            }


            int _iL = Dwmc.Length;
            //  int _iL2 = ItemName.Length;
            _DataTableName = strType + "_" + ID + "_" + Dwmc.Substring(0, (_iL > 5 ? 5 : _iL));// + "_" +
                                                                                               //       SysInfo.m_Test_Item.ItemName.Substring(0, (_iL2 > 5 ? 5 : _iL2));
            return _DataTableName;
        }
        /// <summary>
        /// 系统参数读写
        /// </summary>
        /// <param name="iType"></param>
        public static void Init(int iType = 0)
        {
            if (iType == 0)
            {
                #region 读 
                SysInfo.m_i_Have_m_Power = int.Parse(SysInfo.csInter.IniReadDefine("System", "m_i_Have_m_Power", "1", SysInfo.HardFileName));
                SysInfo. m_SysBuff.m_Climb.iBmq_Type = int.Parse(SysInfo.csInter.IniReadDefine("System", "iBmq_Type", "0", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.m_Data_Type = int.Parse(SysInfo.csInter.IniReadDefine("C_Scan_Whzk", "m_Data_Type", "1", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_Gate_S = int.Parse(SysInfo.csInter.IniReadDefine("C_Scan_Whzk", "1_i_Gate_S", "10", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_Gate_E = int.Parse(SysInfo.csInter.IniReadDefine("C_Scan_Whzk", "1_i_Gate_E", "40", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_Gate_S = int.Parse(SysInfo.csInter.IniReadDefine("C_Scan_Whzk", "2_i_Gate_S", "60", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_Gate_E = int.Parse(SysInfo.csInter.IniReadDefine("C_Scan_Whzk", "2_i_Gate_E", "90", SysInfo.HardFileName));

                SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_S_X = int.Parse(SysInfo.csInter.IniReadDefine("C_Scan_Whzk", "1_i_S_X", "10", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_E_X = int.Parse(SysInfo.csInter.IniReadDefine("C_Scan_Whzk", "1_i_E_X", "40", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_Y = int.Parse(SysInfo.csInter.IniReadDefine("C_Scan_Whzk", "1_i_Y", "60", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_S_X = int.Parse(SysInfo.csInter.IniReadDefine("C_Scan_Whzk", "2_i_S_X", "60", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_E_X = int.Parse(SysInfo.csInter.IniReadDefine("C_Scan_Whzk", "2_i_E_X", "90", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_Y = int.Parse(SysInfo.csInter.IniReadDefine("C_Scan_Whzk", "2_i_Y", "60", SysInfo.HardFileName));
               
                //      SysInfo.m_iLanguage = int.Parse(SysInfo.csInter.IniReadDefine("System", "m_iLanguage", "0", SysInfo.HardFileName));
                m_bl_Mark = int.Parse(SysInfo.csInter.IniReadDefine("System", "m_bl_Mark", "0", SysInfo.HardFileName)) == 1;
                m_i_MarkLag_WaitTime = int.Parse(SysInfo.csInter.IniReadDefine("System", "m_i_MarkLag_WaitTime", "0", SysInfo.HardFileName));
                SysInfo.m_i_Alarm = int.Parse(SysInfo.csInter.IniReadDefine("System", "m_i_Alarm", "1", SysInfo.HardFileName));
                SysInfo.m_i_ReportType = int.Parse(SysInfo.csInter.IniReadDefine("System", "m_i_ReportType", "0", SysInfo.HardFileName));

                SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval = int.Parse(SysInfo.csInter.IniReadDefine("GSB", "i_Gsb_Interval", "5", SysInfo.HardFileName));
                if (SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval == 0)
                    SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval = 1;


                SysInfo.m_SysBuff.m_Climb.i_Coat_Interval = int.Parse(SysInfo.csInter.IniReadDefine("GSB", "i_Coat_Interval", "5", SysInfo.HardFileName));
                SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotHeightmm_Y_Coat = SysInfo.m_SysBuff.m_Climb.i_Coat_Interval;


                SysInfo.m_SysBuff.m_Climb.i_Gsb_Start_Pos = int.Parse(SysInfo.csInter.IniReadDefine("GSB", "i_Gsb_Start_Pos", "0", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Climb.i_Gsb_End_Pos = int.Parse(SysInfo.csInter.IniReadDefine("GSB", "i_Gsb_End_Pos", "300", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 = int.Parse(SysInfo.csInter.IniReadDefine("GSB", "iRun_Gsb_Sc1_Zx0", "1", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Qj1_Ht0 = 1;// int.Parse(SysInfo.csInter.IniReadDefine("GSB", "iRun_Gsb_Qj1_Ht0", "1", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Climb.iGsb_Len = int.Parse(SysInfo.csInter.IniReadDefine("GSB", "iGsb_Len", "300", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Climb.Speed = int.Parse(SysInfo.csInter.IniReadDefine("GSB", "i_Speed", "5", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Climb.i_Para_Speed_Gsb = int.Parse(SysInfo.csInter.IniReadDefine("GSB", "i_Gsb_Speed", "5", SysInfo.HardFileName));

                SysInfo.m_SysBuff.m_Climb.i_R1_L0 = int.Parse(SysInfo.csInter.IniReadDefine("GSB", "i_R1_L0", "1", SysInfo.HardFileName));
                SysInfo.m_SysBuff.m_Climb.strSpeed = SysInfo.csInter.IniReadDefine("GSB", "strSpeed", "3200", SysInfo.HardFileName);

                m_ItemMark = csInter.IniReadDefine("System", "m_ItemMark", "_@@", HardFileName);
                m_iChScanRange = int.Parse(csInter.IniReadDefine("TOFD_BJ", "m_iChScanRange", "10", HardFileName));

                m_i_UI_Type = int.Parse(csInter.IniReadDefine("TOFD", "m_i_UI_Type", "0", HardFileName));
                if (m_i_UI_Type < 0 || m_i_UI_Type > 1) m_i_UI_Type = 1;
                m_i_Bg_Time = int.Parse(csInter.IniReadDefine("System", "m_i_Bg_Time", "10", HardFileName));
                m_i_Frame_Num = int.Parse(csInter.IniReadDefine("System", "m_i_Frame_Num", "100", HardFileName));
                m_i_WaitTimeNum_Max = int.Parse(csInter.IniReadDefine("System", "m_i_WaitTimeNum_Max", "4", HardFileName));

                m_i_Hand_Do = int.Parse(csInter.IniReadDefine("System", "m_i_Hand_Do", "0", HardFileName));
            //    Tofd.m_iArrLen = int.Parse(csInter.IniReadDefine("TOFD", "m_iArrLen", "100000", HardFileName));
                m_i_ShowXj_MaxNum = int.Parse(csInter.IniReadDefine("System", "m_i_ShowXj_MaxNum", "5", HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.g_iWaitTime = int.Parse(csInter.IniReadDefine("TOFD", "g_iWaitTime", "15", HardFileName));
                m_bl_Get_JGXJ = csInter.IniReadDefine("System", "m_bl_Get_JGXJ", "1", SysInfo.HardFileName) == "1";
                m_bl_ClimbRun_By_PC = csInter.IniReadDefine("System", "m_bl_ClimbRun_By_PC", "1", SysInfo.HardFileName) == "1";
                m_i_Climb_Hand0_Auto1 = int.Parse(csInter.IniReadDefine("System", "m_i_Climb_Hand0_Auto1", "0", SysInfo.HardFileName));
                SysInfo.m_iA_Dellon_MaxNum = int.Parse(csInter.IniReadDefine("System", "m_iA_Dellon_MaxNum", "11", SysInfo.HardFileName));

                m_W_i_FilePath = Application.StartupPath + (m_i_TOFD_0_Cscan_1_Mui_2 == 0 ? "\\Temp" : 
                                                           (m_i_TOFD_0_Cscan_1_Mui_2 == 1 ? "\\Data_C_Scan" : 
                                                           (m_i_TOFD_0_Cscan_1_Mui_2 == 2?"\\Data_Mul_UI":"Data_Coat" )));
                //if (SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type == 2)
                //{
                //    m_W_i_FilePath = Application.StartupPath + "\\Data_C_Scan_ECT";
                //}
                SysInfo.m_W_i_FilePath = csInter.IniReadDefine("System", SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0 ? "m_W_i_FilePath"
                                : (m_i_TOFD_0_Cscan_1_Mui_2 == 1 ? ("m_W_i_FilePath_Cscan") :
                                "m_W_i_FilePath_M_UI"), SysInfo.m_W_i_FilePath, SysInfo.HardFileName);


                SysInfo.m_W_i_SaveType_1One_0Web = int.Parse(csInter.IniReadDefine("System", "m_W_i_SaveType_1One_0Web", "1", SysInfo.HardFileName));

                m_str_B_Stand_Color = csInter.IniReadDefine("Cls_Plant", "m_str_B_Stand_Color", "0/0/255|154/205/50|255/255/0|255/0/0", HardFileName);
                m_str_B_Stand_Color_A = csInter.IniReadDefine("Cls_Plant", "m_str_B_Stand_Color_A", m_str_B_Stand_Color, HardFileName);


                m_fl_MarkLag = float.Parse(csInter.IniReadDefine("TOFD", "m_fl_MarkLag", "300", HardFileName));
                if (m_fl_MarkLag < 0) m_fl_MarkLag = 0;
                m_fl_MarkLag_Limit = float.Parse(csInter.IniReadDefine("TOFD", "m_fl_MarkLag_Limit", "5", HardFileName));
                m_fl_MarkLag_m = m_fl_MarkLag / 1000;


                m_strProName = csInter.IniReadDefine("Cls_Plant", "m_strProName", "DAUT200自动化TOFD焊缝检测系统", HardFileName);
                m_i_Can_BrushTime = int.Parse(csInter.IniReadDefine("Cls_Plant", "m_i_Can_BrushTime", "20", HardFileName));
                m_flPic_Height = int.Parse(csInter.IniReadDefine("Cls_Plant", "m_flPic_Height", "30", HardFileName));

                m_strIP = csInter.IniReadDefine("Cls_Plant", "m_strIP", "192.168.1.2", HardFileName);
                m_strUser = csInter.IniReadDefine("Cls_Plant", "m_strUser", "admin", HardFileName);
                m_strPwd = csInter.IniReadDefine("Cls_Plant", "m_strPwd", "123", HardFileName);

                IP_XunJi_Server = csInter.IniReadDefine("Cls_Plant", "IP_XunJi_Server", "", HardFileName);

                //    m_Tofd_DLL.flWaitTime = float.Parse(csInter.IniReadDefine("TOFD", "flWaitTime", "10", HardFileName));

                SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam = new SEmatChanParam[Tofd.CHAN_OF_CLIENT];
                SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real = new Emat_Real[Tofd.CHAN_OF_CLIENT];

                SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection = "m_Tofd_DLL";
                #region 工艺文件
                string _strT = csInter.IniReadDefine("Tofd_Craft", "CurrCraft_Name", "", HardFileName);
                int _iT = int.Parse(csInter.IniReadDefine("Tofd_Craft", "Num", "0", HardFileName));
                if (_strT != "" && _iT > 0)
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection = _strT;
                #endregion

                SysInfo.m_SysBuff.m_Tofd_DLL.m_1_2 = int.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_1_2", "2", HardFileName));

                // SysInfo.m_SysBuff.m_Tofd_DLL.CHAN_OF_CLIENT = SysInfo.m_SysBuff.m_Tofd_DLL.m_1_2 == 1 ? 1 : 2;
                SysInfo.m_SysBuff.m_Tofd_DLL.Scree_iDotWithmm_X = SysInfo.m_Plant.Scree_iDotWithmm_X;
                SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan = int.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_icurChan", "0", HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_AddDataByHis = int.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_bl_AddDataByHis", "0", HardFileName))==1;

                for (int i = 0; i < Tofd.CHAN_OF_CLIENT; i++)
                {
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_fEnStep = SysInfo.m_SysBuff.m_Tofd_DLL.Scree_iDotWithmm_X;

                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_fEnRatio = new float[2];
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[i].m_iEnPul = new int[2];
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[i].m_fEnReal = new float[2];

                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_idB = int.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_idB", "300", HardFileName));// 300;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iRange = int.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iRange", "200", HardFileName));//200;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iZeroTime = int.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iZeroTime", "0", HardFileName));// 0;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iParallelTime = int.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iParallelTime", "0", HardFileName));//0;

                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iPulWidthCode = ushort.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iPulWidthCode", "80", HardFileName));//80;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iDemodulation_Flag = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iDemodulation_Flag", "2", HardFileName));// 3;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iRepeatFreq = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iRepeatFreq", "2", HardFileName));//2;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iWorkMode = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iWorkMode", "0", HardFileName));//0;

                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iSecBandWidthF = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iSecBandWidthF", "0", HardFileName));// 0;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iImpedanceF = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iImpedanceF", "1", HardFileName));//1;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iVolt = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iVolt", "0", HardFileName));//0;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_dSpeed = int.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_dSpeed", "5900", HardFileName));//3240;

                    SysInfo.m_SysBuff.m_Tofd_DLL.m_iMinRange[i] = int.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iMinRange", "10", HardFileName));//10;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_iMAxRange[i] = int.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iMAxRange", "1000", HardFileName));//1000;

                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iForword = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iForword", "0", HardFileName));//0;

                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iPulEnable = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iPulEnable", "1", HardFileName));//0;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iVoltEnable = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iVoltEnable", "1", HardFileName));//0;
                    //下面2021-10-10增加
                    //  if (i == 0)
                    {
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iPcsType = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iPcsType", "0", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iPcsMode = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iPcsMode", "0", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_BScanMode = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_BScanMode", "0", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iCurEn = byte.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iCurEn", "0", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iEnPos = 1;// byte.Parse(csInter.IniReadDefine(SysInfo .m_SysBuff .m_Tofd_DLL.m_strSection, "m_iEnPos", "0", HardFileName));

                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_fPcsLen = float.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsLen", "0", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_fPcsArc = float.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsArc", "0", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_fPcsChord = float.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsChord", "0", HardFileName));

                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_fPcsDia = float.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsDia", "300", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_fPcsAngle = float.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsAngle", "60", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_fPcsStart = float.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsStart", "0", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_fPcsEnd = float.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsEnd", "20", HardFileName));

                        //    SysInfo .m_SysBuff .m_Tofd_DLL.m_pSparam[i].m_fEnStep = float.Parse(csInter.IniReadDefine(SysInfo .m_SysBuff .m_Tofd_DLL.m_strSection, "m_fEnStep", "0", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_fEnRatio[0] = float.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fEnRatio_0", "0.04", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_fEnRatio[1] = float.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fEnRatio_1", "0.04", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.fl4Car_JzXs = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_fEnRatio[0];
                    //    SysInfo.m_SysBuff.m_Climb.fl4Car_JzXs = SysInfo.m_SysBuff.m_Tofd_DLL.fl4Car_JzXs;
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[i].T0 = float.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "T0", "0", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[i].L0 = float.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "L0", "0", HardFileName));
                        SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[i].L0_Distan = float.Parse(csInter.IniReadDefine(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "L0_Distan", "0", HardFileName));
                    }
                }
                try
                {
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_fl_Time_JG = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iRange * 2.0f /
                            (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_dSpeed / 1000f)
                            / Tofd.UTS_DATA_WIDTH;
                }
                catch { }

                SysInfo.m_SysBuff.m_Tofd_DLL.m_i_Alarm_Left_S = int.Parse(csInter.IniReadDefine("TOFD", "m_i_Alarm_Left_S", "10", HardFileName));
                if (SysInfo.m_SysBuff.m_Tofd_DLL.m_i_Alarm_Left_S < 10) SysInfo.m_SysBuff.m_Tofd_DLL.m_i_Alarm_Left_S = 10;
                SysInfo.m_SysBuff.m_Tofd_DLL.m_i_Alarm_Left_E = int.Parse(csInter.IniReadDefine("TOFD", "m_i_Alarm_Left_E", "500", HardFileName));
                SysInfo.m_SysBuff.m_Tofd_DLL.m_i_Alarm_Limit = int.Parse(csInter.IniReadDefine("TOFD", "m_i_Alarm_Limit", "20", HardFileName));
                #endregion 读

                #region C扫描信息类
                m_C_Item_Info.strWtdw = csInter.IniReadDefine("C_INFOR", "strWtdw", "", HardFileName);
                m_C_Item_Info.StrGcmc = csInter.IniReadDefine("C_INFOR", "StrGcmc", "", HardFileName);
                m_C_Item_Info.strGjmc = csInter.IniReadDefine("C_INFOR", "strGjmc", "", HardFileName);
                m_C_Item_Info.strJcff = csInter.IniReadDefine("C_INFOR", "strJcff", "", HardFileName);
                m_C_Item_Info.strJcjg = csInter.IniReadDefine("C_INFOR", "strJcjg", "", HardFileName);
                m_C_Item_Info.strJgdz = csInter.IniReadDefine("C_INFOR", "strJgdz", "", HardFileName);
                m_C_Item_Info.strYb = csInter.IniReadDefine("C_INFOR", "strYb", "", HardFileName);
                m_C_Item_Info.strJcry = csInter.IniReadDefine("C_INFOR", "strJcry", "", HardFileName);
                m_C_Item_Info.strSpry = csInter.IniReadDefine("C_INFOR", "strSpry", "", HardFileName);
                m_C_Item_Info.strJyrq = csInter.IniReadDefine("C_INFOR", "strJyrq", "", HardFileName);


                m_C_Item_Info.strBgbh = csInter.IniReadDefine("C_INFOR", "strBgbh", "", HardFileName);
                m_C_Item_Info.strJlbh = csInter.IniReadDefine("C_INFOR", "strJlbh", "", HardFileName);
                m_C_Item_Info.strJcbj = csInter.IniReadDefine("C_INFOR", "strJcbj", "", HardFileName);
                m_C_Item_Info.strRclzt = csInter.IniReadDefine("C_INFOR", "strRclzt", "", HardFileName);
                m_C_Item_Info.strBmzt = csInter.IniReadDefine("C_INFOR", "strBmzt", "", HardFileName);
                m_C_Item_Info.strSblb = csInter.IniReadDefine("C_INFOR", "strSblb", "", HardFileName);
                m_C_Item_Info.strJcbl = csInter.IniReadDefine("C_INFOR", "strJcbl", "", HardFileName);
                m_C_Item_Info.strJcbz = csInter.IniReadDefine("C_INFOR", "strJcbz", "", HardFileName);
                m_C_Item_Info.strHgjb = csInter.IniReadDefine("C_INFOR", "strHgjb", "", HardFileName);
                m_C_Item_Info.strJsdj = csInter.IniReadDefine("C_INFOR", "strJsdj", "", HardFileName);
                m_C_Item_Info.strYqmc = csInter.IniReadDefine("C_INFOR", "strYqmc", "", HardFileName);
                m_C_Item_Info.strYqxh = csInter.IniReadDefine("C_INFOR", "strYqxh", "", HardFileName);
                m_C_Item_Info.strYqbh = csInter.IniReadDefine("C_INFOR", "strYqbh", "", HardFileName);
                m_C_Item_Info.strYxq = csInter.IniReadDefine("C_INFOR", "strYxq", "", HardFileName);
                m_C_Item_Info.strYqjd = csInter.IniReadDefine("C_INFOR", "strYqjd", "", HardFileName);

                m_C_Item_Info.strCaiZhi = csInter.IniReadDefine("C_INFOR", "strCaiZhi", "", HardFileName);
                m_C_Item_Info.strGjbh = csInter.IniReadDefine("C_INFOR", "strGjbh", "", HardFileName);
                m_C_Item_Info.strSbxs = csInter.IniReadDefine("C_INFOR", "strSbxs", "", HardFileName);
                m_C_Item_Info.strGg = csInter.IniReadDefine("C_INFOR", "strGg", "", HardFileName);
                m_C_Item_Info.strJcm = csInter.IniReadDefine("C_INFOR", "strJcm", "", HardFileName);
                m_C_Item_Info.strJcwz = csInter.IniReadDefine("C_INFOR", "strJcwz", "", HardFileName);

                #endregion C
            }
            else
            {
                SysInfo.csInter.INIWriteValue("System", "iBmq_Type", SysInfo.m_SysBuff.m_Climb.iBmq_Type.ToString(), SysInfo.HardFileName);
                #region 写
                #region C扫描
                csInter.INIWriteValue("C_INFOR", "strWtdw", m_C_Item_Info.strWtdw, HardFileName);
                csInter.INIWriteValue("C_INFOR", "StrGcmc", m_C_Item_Info.StrGcmc, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strGjmc", m_C_Item_Info.strGjmc, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strJcff", m_C_Item_Info.strJcff, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strJcjg", m_C_Item_Info.strJcjg, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strJgdz", m_C_Item_Info.strJgdz, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strYb", m_C_Item_Info.strYb, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strJcry", m_C_Item_Info.strJcry, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strSpry", m_C_Item_Info.strSpry, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strJyrq", m_C_Item_Info.strJyrq, HardFileName);

                csInter.INIWriteValue("C_INFOR", "strBgbh", m_C_Item_Info.strBgbh, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strJlbh", m_C_Item_Info.strJlbh, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strJcbj", m_C_Item_Info.strJcbj, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strRclzt", m_C_Item_Info.strRclzt, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strBmzt", m_C_Item_Info.strBmzt, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strSblb", m_C_Item_Info.strSblb, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strJcbl", m_C_Item_Info.strJcbl, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strJcbz", m_C_Item_Info.strJcbz, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strHgjb", m_C_Item_Info.strHgjb, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strJsdj", m_C_Item_Info.strJsdj, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strYqmc", m_C_Item_Info.strYqmc, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strYqxh", m_C_Item_Info.strYqxh, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strYqbh", m_C_Item_Info.strYqbh, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strYxq", m_C_Item_Info.strYxq, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strYqjd", m_C_Item_Info.strYqjd, HardFileName);

                csInter.INIWriteValue("C_INFOR", "strCaiZhi", m_C_Item_Info.strCaiZhi, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strGjbh", m_C_Item_Info.strGjbh, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strSbxs", m_C_Item_Info.strSbxs, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strGg", m_C_Item_Info.strGg, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strJcm", m_C_Item_Info.strJcm, HardFileName);
                csInter.INIWriteValue("C_INFOR", "strJcwz", m_C_Item_Info.strJcwz, HardFileName);
                #endregion C

                //   m_i_MarkLag_WaitTime
                csInter.INIWriteValue("System", "m_i_MarkLag_WaitTime", m_i_MarkLag_WaitTime.ToString(), HardFileName);
                csInter.INIWriteValue("System", "m_i_Alarm", m_i_Alarm.ToString(), HardFileName);
                csInter.INIWriteValue("TOFD", "m_i_UI_Type", m_i_UI_Type.ToString(), HardFileName);

                csInter.INIWriteValue("TOFD", "m_fl_MarkLag", m_fl_MarkLag.ToString(), HardFileName);
                csInter.INIWriteValue("Cls_Plant", "m_str_B_Stand_Color", m_str_B_Stand_Color, HardFileName);
                csInter.INIWriteValue("Cls_Plant", "m_str_B_Stand_Color_A", m_str_B_Stand_Color_A, HardFileName);

                csInter.INIWriteValue("Cls_Plant", "m_i_Can_BrushTime", m_i_Can_BrushTime.ToString(), HardFileName);
                csInter.INIWriteValue("Cls_Plant", "m_flPic_Height", m_flPic_Height.ToString(), HardFileName);
                csInter.INIWriteValue("Cls_Plant", "m_strIP", m_strIP, HardFileName);
                csInter.INIWriteValue("Cls_Plant", "m_strPwd", m_strPwd, HardFileName);

                csInter.INIWriteValue("Cls_Plant", "IP_XunJi_Server", IP_XunJi_Server, HardFileName);

                // csInter.INIWriteValue("TOFD", "g_iWaitTime", SysInfo .m_SysBuff .m_Tofd_DLL.g_iWaitTime.ToString(), HardFileName);

                for (int i = 0; i < 1; i++)
                {
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[0].m_fEnStep = SysInfo.m_SysBuff.m_Tofd_DLL.Scree_iDotWithmm_X;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_idB", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_idB.ToString(), HardFileName);// 300;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iRange", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iRange.ToString(), HardFileName);//200;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iZeroTime", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iZeroTime.ToString(), HardFileName);// 0;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iParallelTime", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iParallelTime.ToString(), HardFileName);//0;

                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iPulWidthCode", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iPulWidthCode.ToString(), HardFileName);//80;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iDemodulation_Flag", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iDemodulation_Flag.ToString(), HardFileName);// 3;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iRepeatFreq", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iRepeatFreq.ToString(), HardFileName);//2;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iWorkMode", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iWorkMode.ToString(), HardFileName);//0;

                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iSecBandWidthF", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iSecBandWidthF.ToString(), HardFileName);// 0;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iImpedanceF", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iImpedanceF.ToString(), HardFileName);//1;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iVolt", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iVolt.ToString(), HardFileName);//0;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_dSpeed", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_dSpeed.ToString(), HardFileName);//3240;

                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iMinRange", SysInfo.m_SysBuff.m_Tofd_DLL.m_iMinRange[i].ToString(), HardFileName);//10;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iMAxRange", SysInfo.m_SysBuff.m_Tofd_DLL.m_iMAxRange[i].ToString(), HardFileName);//1000;

                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iForword", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iForword.ToString(), HardFileName);//0;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "T0", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[i].T0.ToString(), HardFileName);
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "L0", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[i].L0.ToString(), HardFileName);
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "L0", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[i].L0_Distan.ToString(), HardFileName);

                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iPulEnable", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iPulEnable.ToString(), HardFileName);//0;
                    csInter.INIWriteValue(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iVoltEnable", SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iVoltEnable.ToString(), HardFileName);

                }
                #endregion 写
            }
        }

        /// <summary>
        /// 文件名称，不带路径和后缀
        /// </summary>
        /// <param name="strFileNane"></param>
        public static void Language(Control cons, string strFileNane = "UI_Language")
        {
            SysInfo.m_iLanguage = int.Parse(SysInfo.csInter.IniReadDefine("Language", "m_iLanguage", "0", SysInfo.HardFileName));
            //  return;
            strFileNane = Application.StartupPath + "\\database\\Translate\\" + strFileNane + ".ini";

            String[] _LgPara = strFileNane.Split('_');
            string Section = "Language_";//语言
            if (SysInfo.m_iLanguage > 0)
                Section += SysInfo.m_iLanguage;
            else
                Section += 1;

            string strVal = "";//临时变量
            string strT = "";//临时

            string[] _sPara = "".Split('$');
            string[] _sPara_JianJu = "".Split('#');//控件左右上下位置调整参数 ","间隔
            string[] _sPara_ZySx = "".Split(',');//左右上下位置调整参数 ","间隔
            int iNum = int.Parse(SysInfo.csInter.IniReadDefine(Section, "Num", "0", strFileNane));
            string strName = "";//控件名称

            System.Windows.Forms.Control con;
            int iNo = 1;
            try
            {
                for (iNo = 1; iNo <= iNum; iNo++)
                {
                    try
                    {
                        strVal = SysInfo.csInter.INIReadValue(Section, "P_" + iNo.ToString(), "", strFileNane);//根据控件名称读对应翻译语言

                        if (strVal != "")
                        {
                            _sPara = strVal.Split('=');
                            if (_sPara.Length == 2)
                            {
                                strName = _sPara[0];
                                if (strName != "")
                                {
                                    if (strName == "Rad_Coat")
                                    {

                                    }
                                    if (strName == "Lb_Dg_All_Data_0")
                                    {
                                        strVal = "Lb_Dg_All_Data_0=数据区域\r\n\r\n1 磁爬联机：点击<磁爬快捷键>中的Com字符，程序会刷新COM口;\r\n选择对应COM口，点击<联接>\r\n\r\n2 右下方的1、2、3 对应：B扫图、多行B扫图、厚度列表\r\n  点击B扫图的左边、右边、上中、下中部，可以数据翻页\r\n\r\n3 B扫图上移动鼠标可查看详细数据，\r\n\r\n4 红色线是公称厚度上下的误差告警限（设置->误差限->2中设置）\r\n\r\n5 厚度列表：双击鼠标，右翻页（停止运行时）\r\n\r\n6 扫查模式每次开始检测时，车速默认恒速，磁爬快捷键右下方可取消恒速\r\n\r\n7 开始检测，磁爬距离将清零，图形左右坐标从0和上行最终距离开始绘制\r\n\r\n点击我：提示隐藏&&Data area\r\n\r\n1Magnetic crawl online: Click the Com character in <Magnetic shortcut key>, the program will refresh the COM port; \r\n select the corresponding COM port,click <Connection>\r\n\r\n2 1, 2, 3 on the lower right corresponds to: B scan, multi-line B scan, thickness list \r\n Click B scan left, right, upper middle, lower middle, can flip data\r\n \r\n3 Move the mouse on the B-scan to view the detailed data, \r\n\r\n4 The red line is the error alarm limit above the nominal thickness (Settings -> Error Limit -> 2 settings) \r\n\r\n5 Thickness list: Double click the mouse, right page (stop running)\r\n\r\n6 Scan mode Every time user start measuring, the speed defaults to constant speed, and the magnetic crawl shortcut can cancel the constant speed at the bottom right. r\n\r\n7 Start detection, the magnetic creep distance will be cleared, the left and right coordinates of the graph will be drawn from 0 and the final distance of the distance\r\n\r\nClick me: prompt to hide&";
                                        _sPara = strVal.Split('=');
                                    }
                                    if (strName == "Txt_UI_Help")
                                    {
                                        // strVal = "Txt_UI_Help=波形图区域\r\n\r\n1 图右侧下方<键盘>：\r\n  用于波形参数设置,\r\n  是开关键;\r\n\r\n2 探头显示--：点键盘重设声速等\r\n  测量前探头应先校准\r\n\r\n3 选项键盘区域的1、2对应：波形图、超限数据列表\r\n\r\n4  本界面只显示重连信息\r\n\r\n5  探头提力度高时数据会抖动\r\n\r\n6 正确联机超声探头方法：\r\n  1 先打开PC\r\n  2 联机电源盒和磁爬连线，此时电源盒不能上电\r\n  3 联机电源盒与PC连线（此时，探头与PC开始通讯联接）\r\n  &&Waveform plot\r\n\r\n1Lower <keypad> on the right side of the picture:\r\nFor waveform parameter setting, \r\nis the key to open;\r\n\r\n2 Probe display --: point keyboard reset sound speed etc.\r\nThe probe should be calibrated before measurement\r\n\r\n3 Option 1,2 for the keyboard area: Waveform, out-of-limit data list\r\n\r\n4 this interface only displays reconnect information\r\n\r\n5 The data jitter when the probe is raised with high force\r\n\r\n  6 method of correctly connecting ultrasonic probe: \r\n  1 First open PC\r\n  2 connect the power box and magnetic climbing wire, at this time the power box cannot be powered on \r\n  3 Connect the power box to PC (at this time, the probe and PC start to connect \r\n & ";
                                        strVal = "Txt_UI_Help=超声监控区域\r\n\r\n1 本界面显示超声探头联接信息\r\n\r\n2  超声监控区的右下三角按钮：显示/隐藏按键\r\n\r\n3 参数：设置超声详细参数\r\n\r\n4 探头提力度控制在2mm内，提力越高测厚值越抖动&&Ultrasonic monitoring area\r\n\r\n1  This interface displays the connection information of ultrasonic probe\r\n\r\n2.  right lower triangle button in ultrasonic monitoring area: show/hide button\r\n\r\n3.  Parameters: set detailed ultrasonic parameters\r\n\r\n4. The lifting force of the probe should be controlled within 2mm&";
                                        _sPara = strVal.Split('=');
                                    }
                                    try
                                    {
                                        con = Get_Control(cons, strName);
                                        _sPara = _sPara[1].Split('&');
                                        if (strName == "Txt_Test_Help")
                                            _sPara =
    "数据区域\r\n\r\n1 磁爬联机：双击<控制台>将显示Com字符，点击Com，程序会刷新COM口;\r\n选择对应COM口，点击 < 联接 >\r\n\r\n2 右下方的选择对应：查看图对应波形、B扫图、多行C扫图、厚度列表\r\n 停止运行时选择对应选项，显示翻页按钮，可以数据翻页\r\n\r\n3 B扫图上移动鼠标可查看详细数据，\r\n\r\n4 红色线是公称厚度上下的误差告警限（设置->误差限->2中设置）\r\n\r\n5 厚度列表：双击鼠标，右翻页（停止运行时）\r\n\r\n6 扫查模式每次开始检测时，车速默认恒速，磁爬快捷键右下方可取消恒速\r\n\r\n7 开始检测，磁爬距离将清零，图形左右坐标从0和上行最终距离开始绘制 && Data area\r\n\r\n1Magnetic crawl online: Click the Com character in < Magnetic shortcut key>, the program will refresh the COM port; \r\n select the corresponding COM port, click < Connection >\r\n\r\n2 1, 2, 3 on the lower right corresponds to: B scan, multi-line C scan, thickness list \r\n Click B scan left, right, upper middle, lower middle, can flip data\r\n \r\n3 Move the mouse on the B - scan to view the detailed data, \r\n\r\n4 The red line is the error alarm limit above the nominal thickness(Settings->Error Limit-> 2 settings) \r\n\r\n5 Thickness list: Double click the mouse, right page(stop running)\r\n\r\n6 Scan mode Every time user start measuring, the speed defaults to constant speed, and the magnetic crawl shortcut can cancel the constant speed at the bottom right. r\n\r\n7 Start detection, the magnetic creep distance will be cleared, the left and right coordinates of the graph will be drawn from 0 and the final distance of the distance\r\n\r\nClick me: prompt to hide &"
          .Split('&');
                                        #region 解析赋值
                                        if (_sPara.Length == 4)//单名称控件
                                        {
                                            if (SysInfo.m_iLanguage == 0)
                                            {
                                                if (strName.IndexOf("strJyrq") == -1)//检定日期，用lable来显示数据，不允许编辑
                                                    if (_sPara[0] != "")
                                                    {
                                                        #region 赋值和位置调整  字符标题 & 位置调整 左右,上下
                                                        _sPara_JianJu = _sPara[0].Split('#');
                                                        con.Text = _sPara_JianJu[0].Replace("\\r\\n", "\r\n");//标题
                                                        if (_sPara_JianJu.Length == 2)
                                                        {
                                                            _sPara_ZySx = _sPara_JianJu[1].Split(',');
                                                            if (_sPara_ZySx.Length == 2)
                                                            {
                                                                try
                                                                {
                                                                    con.Left += int.Parse(_sPara_ZySx[0]);
                                                                }
                                                                catch { }
                                                                try
                                                                {
                                                                    con.Top += int.Parse(_sPara_ZySx[1]);
                                                                }
                                                                catch { }
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                if (_sPara[1] != "")//注释
                                                {
                                                    _sPara[1] = _sPara[1].Replace("\\r\\n", "\r\n");
                                                    SysInfo.Tool_Tip(con, _sPara[1]);
                                                }
                                            }
                                            else
                                            {
                                                _sPara[2] = _sPara[2].Replace("\\r\\n", "\r\n");
                                                if (strName.IndexOf("strJyrq") == -1)//检定日期，用lable来显示数据，不允许编辑
                                                    if (_sPara[2] != "")
                                                    {
                                                        //  con.Text = _sPara[2];//外文 标题

                                                        #region 赋值和位置调整  字符标题 & 位置调整 左右,上下
                                                        _sPara_JianJu = _sPara[2].Split('#');
                                                        strT = _sPara_JianJu[0].Replace("\\r\\n", "\r\n");//标题
                                                        try
                                                        {
                                                            if (strT != "")
                                                                con.Text = strT;//标题
                                                        }
                                                        catch { }
                                                        //if (con.Text == "dddd")//英文色标区间=公称厚度 * 20% 将此等号用dddd代替
                                                        //    con.Text = "=";

                                                        if (_sPara_JianJu.Length == 2)
                                                        {
                                                            _sPara_ZySx = _sPara_JianJu[1].Split(',');
                                                            if (_sPara_ZySx.Length > 0)
                                                            {
                                                                try
                                                                {
                                                                    con.Left += int.Parse(_sPara_ZySx[0]);
                                                                }
                                                                catch { }
                                                                try
                                                                {
                                                                    con.Top += int.Parse(_sPara_ZySx[1]);
                                                                }
                                                                catch { }
                                                            }
                                                        }
                                                        #endregion
                                                    }

                                                if (_sPara[3] != "")//注释
                                                {
                                                    _sPara[3] = _sPara[3].Replace("\\r\\n", "\r\n");
                                                    SysInfo.Tool_Tip(con, _sPara[3]);
                                                }
                                            }
                                        }
                                        else if (_sPara.Length == 5)//多项内容的控件
                                        {
                                            if (_sPara[0].ToLower().IndexOf("box") > 0)//combobox
                                            {
                                                ComboBox Com = con as ComboBox;
                                                strVal = _sPara[SysInfo.m_iLanguage == 0 ? 2 : 4];
                                                if (strVal != "")//注释
                                                {
                                                    strVal = strVal.Replace("\\r\\n", "\r\n");
                                                    SysInfo.Tool_Tip(Com, strVal);
                                                }
                                                _sPara = _sPara[SysInfo.m_iLanguage == 0 ? 1 : 3].Split('|');
                                                Com.Items.Clear();
                                                for (int i = 0; i < _sPara.Length; i++)
                                                    Com.Items.Add(_sPara[i].ToString());
                                            }
                                            else if (_sPara[0].ToLower().IndexOf("tabc") > -1)//tabcontrol
                                            {
                                                TabControl Tab = con as TabControl;
                                                _sPara = _sPara[SysInfo.m_iLanguage == 0 ? 1 : 3].Split('|');
                                                if (_sPara.Length == Tab.TabPages.Count)
                                                {
                                                    for (int i = 0; i < _sPara.Length; i++)
                                                    {
                                                        try
                                                        {
                                                            Tab.TabPages[i].Text = _sPara[i].ToString();
                                                        }
                                                        catch (Exception et)
                                                        { }
                                                    }
                                                }
                                                else
                                                {
                                                    if (strName == "tab_Wave_Set")
                                                    {
                                                        if (Tab.TabPages.Count == 2)
                                                        {
                                                            Tab.TabPages[0].Text = _sPara[3].ToString();
                                                            Tab.TabPages[1].Text = _sPara[4].ToString();
                                                        }
                                                        else
                                                        {
                                                            Tab.TabPages[0].Text = _sPara[1].ToString();
                                                            Tab.TabPages[1].Text = _sPara[3].ToString();
                                                            Tab.TabPages[2].Text = _sPara[4].ToString();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    catch(Exception exl) { }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    { }
                }
            }
            catch (Exception e)
            { MessageBox.Show("行号：" + iNo + " " + e.Message + " " + e.StackTrace); }
            return;
        }
        //---
        public static Control Get_Control(Control cons, string name)
        {
            object o = cons.GetType().GetField(name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(cons);

            return ((Control)o);
        }
        public static void Tool_Tip(System.Windows.Forms.Control ctTiTl, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(ctTiTl, strVal);
        }
        public static void Tool_Tip(System.Windows.Forms.NumericUpDown nmbTiTl, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(nmbTiTl, strVal);
        }

        public static void Tool_Tip(System.Windows.Forms.ComboBox cmbTiTl, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(cmbTiTl, strVal);
        }
        public static void Tool_Tip(System.Windows.Forms.CheckBox ckTiTl, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(ckTiTl, strVal);
        }
        public static void Tool_Tip(System.Windows.Forms.ProgressBar pgTiTl, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(pgTiTl, strVal);
        }
        public static void Tool_Tip(System.Windows.Forms.TextBox TxTiTl, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(TxTiTl, strVal);
        }
        public static void Tool_Tip(System.Windows.Forms.RadioButton Rd, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(Rd, strVal);
        }
        public static void Tool_Tip(System.Windows.Forms.Label lbTiTl, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(lbTiTl, strVal);
        }
        public static void Tool_Tip(System.Windows.Forms.Button BtTi, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(BtTi, strVal);
        }
        public static void Tool_Tip(System.Windows.Forms.PictureBox BtTi, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(BtTi, strVal);
        }
        //---
        /// <summary>
        /// 参数计算改变参数保存
        /// </summary>
        /// <param name="iType"></param>
        public static void IniIt_Calcu()
        {
            int _iCurrChan =  SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan;

            #region  写数据
            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iPcsType",  SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsType.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iPcsMode",  SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsMode.ToString(), SysInfo.HardFileName);

            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_BScanMode",  SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_BScanMode.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_iCurEn",  SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn.ToString(), SysInfo.HardFileName);

            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsLen",  SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsLen.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsArc",  SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsArc.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsChord",  SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsChord.ToString(), SysInfo.HardFileName);

            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsDia",  SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsDia.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsAngle",  SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsAngle.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsStart",  SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsStart.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fPcsEnd",  SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsEnd.ToString(), SysInfo.HardFileName);

            if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn == 0)
                SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fEnRatio_0",
                     SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fEnRatio[0].ToString(), SysInfo.HardFileName);
            else
                SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_fEnRatio_1",
                     SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fEnRatio[1].ToString(), SysInfo.HardFileName);
            #endregion
        }
        /// <summary>
        /// 校准参数保存
        /// </summary>
        public static void Init_Jz()
        {
             SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[ SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iZeroTime = (int)( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[ SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].T0 * 100f);
            Tofd.SendCmdZeroTime( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[ SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iZeroTime);

            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "T0",
                   SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[ SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].T0.ToString(), SysInfo.HardFileName);

            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "L0",
                         SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[ SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].L0.ToString(), SysInfo.HardFileName);

            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "L0_Distan",
                         SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[ SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].L0_Distan.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue( SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, "m_dSpeed",
                         SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[ SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_dSpeed.ToString(), SysInfo.HardFileName);
        }
        /// <summary>
        /// 写配置
        /// </summary>
        /// <param name="strKey">主键</param>
        /// <returns></returns>
        public string Read_One(string strKey)
        {
            return csInter.IniReadDefine("Cls_Plant", strKey, "", HardFileName);
        }
        /// <summary>
        /// 写配置
        /// </summary>
        /// <param name="strKey">主键</param>
        /// <param name="strVal">对应值</param>
        public void Write_One(string strKey, string strVal)
        {
            csInter.INIWriteValue("Cls_Plant", strKey, strVal, HardFileName);
        }
        /// <summary>
        /// 以毫秒为单位延时
        /// </summary>
        /// <param name="dbWait">延时时间：ms</param>
        public static void WaitTime(float dbWait)
        {
            DateTime dtStar = DateTime.Now;
            float _flD = 0;
            while (true)
            {  
                System.Threading.Thread.Sleep(5);
            //   Application.DoEvents();
                _flD = DateTime.Now.Subtract(dtStar).Milliseconds;
                if (_flD > dbWait) break;
             
            }
        }
        public static void WaitTime_Main(float dbWait)
        {
            DateTime dtStar = DateTime.Now;
            float _flD = 0;
            while (true)
            {
                Application.DoEvents();
                if (DateTime.Now.Subtract(dtStar).TotalMilliseconds > dbWait) break;
                System.Threading.Thread.Sleep(1);

            }
        }
        public static void WaitTime_S(float dbWait)
        {
            DateTime dtStar = DateTime.Now;
            double  _flD = 0;
            while (true)
            {
                System.Threading.Thread.Sleep(1);
                //    Application.DoEvents();
                _flD = DateTime.Now.Subtract(dtStar).TotalSeconds  ;
                if (_flD > dbWait) break;

            }
        }
        #region 系统键盘
        public static void SetKeyBorad(int iL, int iTop, string strPrgName = "osk", bool blVal = false)
        {
            if (blVal == false)
            {
                iL += 10;
                iTop += 90;
            }
            if (blVal)
            {
                StartKeyBoard.StartKeyBoardFun();
                return;

            }
            else
            {
                System.Diagnostics.Process kbpr = new System.Diagnostics.Process();
                kbpr.StartInfo.FileName = blVal ? strPrgName + ".exe" : Application.StartupPath + "\\" + strPrgName + ".exe";
                kbpr.StartInfo.Arguments = iL.ToString() + " " + iTop.ToString();
                //  Process.Start(@C:\WINDOWS\system32\osk.exe);
                foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName(strPrgName))
                {
                    return;
                    p.Kill();
                }

                kbpr.Start();
            }

        }
        public static void SetKeyBorad(Control _Para, Control _MyCtrl, string strPrgName = "osk", bool blVal = false)
        {
            int iL = _MyCtrl.Left;
            int iH = _MyCtrl.Top + _MyCtrl.Height;
            if (blVal == false)
            {
                iL +=- 50 + _Para.Left;
                iH += 10 + _Para.Top;
            }
            if (blVal)
            {
                StartKeyBoard.StartKeyBoardFun();
                return;

            }
            else
            {
                System.Diagnostics.Process kbpr = new System.Diagnostics.Process();
                kbpr.StartInfo.FileName = blVal ? strPrgName + ".exe" : Application.StartupPath + "\\" + strPrgName + ".exe";
                kbpr.StartInfo.Arguments = iL.ToString() + " " + iH.ToString();
                //  Process.Start(@C:\WINDOWS\system32\osk.exe);
                foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName(strPrgName))
                {
                    return;
                    p.Kill();
                }

                kbpr.Start();
            }

        }
        public static void SetKeyBorad(string strPrgName = "osk", bool blVal = false)
        {
            System.Diagnostics.Process kbpr = new System.Diagnostics.Process();
            kbpr.StartInfo.FileName = blVal ? strPrgName + ".exe" : Application.StartupPath + "\\" + strPrgName + ".exe";
            //  Process.Start(@C:\WINDOWS\system32\osk.exe);
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName(strPrgName))
            {
                // return;
                p.Kill();
            }
        }
        /// <summary>
        /// onoff值为0是关闭键盘，为1打开键盘
        /// </summary>
        /// <param name="onoff"></param>
        public static void SetKeyBorad(int onoff, string strPrgName = "osk")
        {
            System.Diagnostics.Process kbpr = new System.Diagnostics.Process();
            kbpr.StartInfo.FileName = strPrgName + ".exe";
            //  Process.Start(@C:\WINDOWS\system32\osk.exe);
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName(strPrgName))
            {
                return ;
                p.Kill();
            }
            SysInfo.m_i_Osk = 1;

            //       if (onoff == 1)
            {
                kbpr.Start();
                SysInfo.m_i_Osk = 0;
            }
            //      else
            {

            }
        }
        /// <summary>
        /// 判断键盘是否已经显示 
        /// </summary>
        /// <returns>false:没打开 true: 已经打开</returns>
        public static bool Jug_KeyBorad(string strPrgName= "osk")
        {
            bool _blRet = false;
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName(strPrgName))
            {
                _blRet = true; break;
            }
            return _blRet;
        }
        #endregion 系统键盘
    }


    /// <summary>
    /// 画图类
    /// </summary>
    public class Cls_Plant
    {
        /// <summary>
        /// 画图类初始化
        /// </summary>
        public   Cls_Plant()
        {
            Init();
        }
        #region 变量

        /// <summary>
        /// 是否查看历史数据对应的波形 true:查看历史数据  false:实时数据
        /// </summary>
        public bool m_bl_Ck_Wave = false;
        /// <summary>
        /// 当前屏幕数据是历史查询得到的，true:依据屏幕序号从记录中拿，false:否则依据距离从数据库拿
        /// </summary>
        public bool m_bl_His_Data = false;
        /// <summary>
        /// 默认显示指针，离开后不显示
        /// </summary>
        public bool m_bl_Hid_Sz = false;
        /// <summary>
        /// A扫描图备份
        /// </summary>
      //  public Bitmap m_image;
        /// <summary>
        /// 标准颜色
        /// </summary>
        public List<ColorRange> m_StandcolorRange;
        /// <summary>
        /// 单位制式：0：公制单位mm  1：英制单位in
        /// </summary>
        public int iRad_Dw = 0;
        /// <summary>
        /// 文件读写
        /// </summary>
        ClassInterFace csInter = new ClassInterFace();
        string HardFileName = Application.StartupPath + "\\database\\SysConfig.ini";


        /// <summary>
        /// 图形坐标：横轴开始像素位置 15
        /// </summary>
        public int Chart_Ruler_X_Start = 15;
        /// <summary>
        /// C扫图像左上角距离图像顶部高度 18
        /// </summary>
        public int Chart_Ruler_Y_Start = 18;
        /// <summary>
        /// 一个点宽度 5
        /// </summary>
        public int Scree_iDotWith_X = 5;
        /// <summary>
        /// 一个点高度 5
        /// </summary>
        public float  Scree_iDotHeight = 5;
        ///// <summary>
        ///// A扫描图一个间隔的像素宽度
        ///// </summary>
        //public int m_iKd_W_512Num = 0;
        /// <summary>
        /// 画图显示当前位置
        /// </summary>
        public float m_fl_Distance = 0;
        /// <summary>
        /// A图像X位置
        /// </summary>
        public int m_i_X_No = 0;
        /// <summary>
        /// 图像X位置
        /// </summary>
        public int m_i_X_A = 0;
        /// <summary>
        /// 图像Y位置
        /// </summary>
        public int m_i_Y_A = 0;

        /// <summary>
        /// 图像X位置
        /// </summary>
        public int m_i_X_D = 0;
        /// <summary>
        /// 图像Y位置
        /// </summary>
        public int m_i_Y_D = 0;
        /// <summary>
        /// 横轴间隔个数
        /// </summary>
        public int m_iJgNum = 10;
        /// <summary>
        /// D扫描行数，这个由TOFD数据个数决定
        /// </summary>
        public int Scree_iAllRows = 0;
        /// <summary>
        /// 屏幕刻度（以起点为原点的左右刻度是相同的，屏幕序号以各自区间独立判断）
        /// </summary>
        public List<Class_Screen_Kd> lst_Screenkd = new List<Class_Screen_Kd>();
        /// <summary>
        /// 一个点代表距离  默认10毫米  单位m
        /// </summary>
        public float Scree_iDotWithmm_X = 10;
        /// <summary>
        /// 英寸最小单位
        /// </summary>
        public float m_fl_In_MinLimit = 1 / 32f;
        /// <summary>
        /// <summary>
        /// 屏幕总列数
        /// </summary>
        public int Scree_iAllCols = 0;
        /// <summary>
        /// 横轴纵轴刻度字下降量
        /// </summary>
        public int Chart_Ruler_Word_H = 2;
        /// <summary>
        /// 最远距离，画图默认1000米-----
        /// </summary>
        public float flMaxDistance = 10000;

        /// <summary>
        /// 当前距离对应的屏幕序号
        /// </summary>
        public int g_iCurrDistanc_Calcu_ScreenNo = -1;
        /// <summary>
        /// 屏幕序号统计
        /// </summary>
        public int g_iCurrDistanc_Calcu_All_ScreenNo = -1;
        /// <summary>
        /// 当前屏幕使用的序号
        /// </summary>
        public int g_iCurrUse_ScreenNo = 0;

        /// <summary>
        /// C图像变量
        /// </summary>
        public Struct_G m_G_C = new Struct_G();
        /// <summary>
        /// 当前运行位置
        /// </summary>
        public int m_iCurr_Run_Position = 0;


        /// <summary>
        /// 屏幕信息对照表
        /// </summary>
        public Class_Screen_Info[] m_Arr_Screen;


        #endregion 变量
        #region 操作配置文件
        /// <summary>
        /// 配置文件读写 0：读 1：写
        /// </summary>
        /// <param name="iType">0：读 1：写</param>
        /// <returns></returns>
        public bool Init(int iType = 0)
        {
            bool _blRet = false;

            try
            {
                if (iType == 0)
                {
                    #region 读
                    Chart_Ruler_X_Start = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_X_Start", "15", HardFileName));
                    Chart_Ruler_Y_Start = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_Y_Start", "18", HardFileName));
                    Scree_iDotWith_X = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotWith_X", "5", HardFileName));
                    Scree_iDotHeight = float .Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotHeight", "15", HardFileName));
                    try
                    {
                        Scree_iDotWithmm_X = int.Parse(csInter.IniReadDefine("Cls_Plant", "Scree_iDotWithmm_X", "1", HardFileName));
                        if (Scree_iDotWithmm_X == 0)
                        {
                            Scree_iDotWithmm_X = 1;
                            csInter.INIWriteValue("Cls_Plant", "Scree_iDotWithmm_X", "1", HardFileName);
                        }
                    }
                    catch { Scree_iDotWithmm_X = 10; }
                    flMaxDistance = int.Parse(csInter.IniReadDefine("Class_Plant", "flMaxDistance", "1000000", HardFileName));
                    m_fl_In_MinLimit = float.Parse(csInter.IniReadDefine("Class_Plant", "m_fl_In_MinLimit", "0.03125", HardFileName));

                    if (iRad_Dw == 0)
                    {
                        Scree_iDotWithmm_X *= 0.001F;
                        Scree_iDotWithmm_X = float.Parse(Scree_iDotWithmm_X.ToString("f3"));
                    }
                    else
                    {
                        if (Scree_iDotWithmm_X == 0) Scree_iDotWithmm_X = m_fl_In_MinLimit;
                    }
      
                    #endregion 读
                }
                else
                {
                    #region 写
                    /*
                    csInter.INIWriteValue("Cls_Plant", "fl_Onel_Height_Time", fl_Onel_Height_Time.ToString(), HardFileName);
                    csInter.INIWriteValue("Cls_Plant", "fl_A_PCS", fl_A_PCS.ToString(), HardFileName);

                    csInter.INIWriteValue("Cls_Plant", "i_A_Onepixel_With", i_A_Onepixel_With.ToString(), HardFileName);

                    csInter.INIWriteValue("Cls_Plant", "i_C_Onepixel_Height", i_C_Onepixel_Height.ToString(), HardFileName);
                    csInter.INIWriteValue("Cls_Plant", "Scree_C_iDotWith_X", Scree_C_iDotWith_X.ToString(), HardFileName);
                    csInter.INIWriteValue("Cls_Plant", "i_One_With_mm", i_One_With_mm.ToString(), HardFileName);
                    */
                    #endregion
                }
                _blRet = true;
            }
            catch { }
            return _blRet;
        }
        /// <summary>
        /// 写配置
        /// </summary>
        /// <param name="strKey">主键</param>
        /// <returns></returns>
        public string Read_One(string strKey)
        {
            return csInter.IniReadDefine("Cls_Plant", strKey, "", HardFileName);
        }
        /// <summary>
        /// 写配置
        /// </summary>
        /// <param name="strKey">主键</param>
        /// <param name="strVal">对应值</param>
        public void Write_One(string strKey, string strVal)
        {
            csInter.INIWriteValue("Cls_Plant", strKey, strVal, HardFileName);
        }
        #endregion 配置文件


        /// <summary>
        /// 图像初坐标
        /// </summary>
        /// <param name="iTotalTime">总时长</param>
        /// <param name="iDataNum">数据个数</param>
        /// <param name="iWith">图像宽度</param>
        /// <param name="iHeight">图像高度</param>
        public void Screen_Init_A(int iTotalTime, int iDataNum, int iWith, int iHeight)
        {
            #region 1 计算纵横刻度
            //i_Arr_Num = iDataNum;
            //fl_Onel_Height_Time = 1.0f * iTotalTime / iDataNum;//一个点代表时间长度

            //i_A_Onepixel_With = (iWith - i_A_Screen_Left) / iDataNum;//一个像素点宽度
            //i_A_Height = iHeight - i_A_Screen_Bottom;
            //i_A_Onepixel_Height = i_A_Height / 255;//一个像素点高度
            #endregion 1
        }
        /// <summary>
        /// 获得灰度图内存数据：黑-白变化
        /// </summary>
        /// <param name="PicArea">画图板</param>
        /// <param name="Rr_Bitmap">图像内存</param>
        /// <param name="iHorizontal_0">0:横向 1：竖向</param>
        /// <returns></returns>
        public int PlanScheme_ColorLimit(System.Windows.Forms.PictureBox PicArea, ref Bitmap Rr_Bitmap, int iHorizontal_0)
        {
            int _iRet = 0;//画图返回
            try
            {
                float flPic_W = PicArea.Width;//画布宽度
                float flPic_H = PicArea.Height;//画布高度

                #region 1.2 准备画布
                // 初始化画板，在内存中建立一块虚拟画布
                Bitmap image = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                // 获取背景层
                Bitmap bg = (Bitmap)PicArea.BackgroundImage;
                // 初始化整个画布
                Bitmap canvas = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
                // 初始化图形面板，获取这块内存画布的Graphics的引用
                Graphics g = Graphics.FromImage(image);
                Graphics gb = Graphics.FromImage(canvas);
                #endregion  准备画布

                g.Clear(Color.White);// g.Clear(Color.Teal);// CadetBlue);

                Rectangle rect = new Rectangle(0, 0, PicArea.ClientSize.Width, PicArea.ClientSize.Height);
                rect.Location = new Point(0, 0);
                LinearGradientBrush b3 = new LinearGradientBrush(rect, Color.Black, Color.White,
                                             iHorizontal_0 == 0 ? LinearGradientMode.Horizontal : LinearGradientMode.Vertical);//WhiteSmoke
                g.FillRectangle(b3, rect);

                #region  4 刷新
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (bg != null)
                    gb.DrawImage(bg, _Rect);// 先绘制背景层
                gb.DrawImage(image, _Rect); // 再绘制绘画层

                PicArea.BackgroundImage = (Bitmap)canvas.Clone();// (Bitmap)canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Rr_Bitmap = (Bitmap)canvas.Clone();

                g.Dispose(); g = null;
                image.Dispose();
                gb.Dispose();
                canvas.Dispose();

                PicArea.Refresh();
                #endregion 刷新

                canvas.Dispose();
                image.Dispose();
            }
            catch { }

            return _iRet;
        }

        /// <summary>
        /// 保存标准颜色
        /// </summary>
        /// <param name="Rr_Bitmap"></param>
        /// <param name="iWithJgNum"></param>
        public void SaveStandColor(Bitmap Rr_Bitmap, int iWithJgNum)
        {
            float _flJG = Rr_Bitmap.Width / iWithJgNum;
            // int _iHeght = Rr_Bitmap.Height / 2;

            m_StandcolorRange = new List<ColorRange>();

            for (int i = 0; i < iWithJgNum; i++)
            {
                ColorRange _Col = new ColorRange();
                _Col.Col = Rr_Bitmap.GetPixel((int)(i * (_flJG)), 0);
                _Col.strColor = _Col.Col.R.ToString() + "," + _Col.Col.G.ToString() + "," + _Col.Col.B.ToString();

                m_StandcolorRange.Add(_Col);
            }
        }
        public struct ColorRange
        {
            /// <summary>
            /// R,G,B
            /// </summary>
            public string strColor;
            /// <summary>
            /// 最大厚度误差 方案中存储方式：从小到大排列
            /// </summary>
            public float Max_Limit;
            /// <summary>
            /// 颜色
            /// </summary>
            public Color Col;

        }

        /// <summary>
        /// 画A扫波形
        /// </summary>
        /// <param name="PicArea">画布</param>
        /// <param name="ArrData_A">A扫数据</param>
        public void Plant_A(System.Windows.Forms.PictureBox PicArea, Tofd Tofd_Data)
        {
            try
            {
                #region 1 初始化
                #region 1.1 画布
                float flPic_W = PicArea.Width;//画布宽度  965
                float flPic_H = PicArea.Height;//画布高度   364
                bool _blSp = Tofd_Data.m_pSparam[Tofd_Data.m_icurChan].m_iDemodulation_Flag == 2;//射频信号

                int iKd_W_Num = int.Parse((flPic_W / m_iJgNum).ToString("f0"));//横轴10个数据一个刻度
                int iKd_H_Num = int.Parse((flPic_H / m_iJgNum).ToString("f0"));//纵轴10个数据一个刻度
                int i_X = 0, i_Y = 0;//刻度

                // 初始化画板，在内存中建立一块虚拟画布
                Bitmap image = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                // 获取背景层
                Bitmap bg = (Bitmap)PicArea.BackgroundImage;
                // 初始化整个画布
                Bitmap canvas = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
                // 初始化图形面板，获取这块内存画布的Graphics的引用
                Graphics g = Graphics.FromImage(image);
                Graphics gb = Graphics.FromImage(canvas);
                g.Clear(Color.Black);//

                g.SmoothingMode = SmoothingMode.AntiAlias;
                #endregion 1.1

                #region 1.2 刻度画笔
                //1 坐标笔
                PointF Zb_X = new PointF(0, 10);
                Pen p_Zb = new Pen(Brushes.White);//坐标笔
                Font Font_Zb = new Font("黑体", 18);
                SolidBrush Color_Zb = new SolidBrush(Color.White);
                //2 中心线条
                Color _ColorL = Color.FromArgb(255, 0, 98, 0);
                Pen p_xy = new Pen(_ColorL);// Brushes.LimeGreen CadetBlue);
                p_xy.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                p_xy.Width = 1;
                // g.DrawLine(p_xy, new Point(0, i_A_Height / 2), new Point((int)flPic_W, i_A_Height / 2));
                #endregion 1.2
                #endregion 1

                #region 2 画横、纵轴分别10格的间隔线
                for (int i = 1; i < m_iJgNum; i++)
                {
                    i_X = i * iKd_W_Num;//竖线
                    g.DrawLine(p_xy, new Point(i_X, 0), new Point(i_X, (int)flPic_H));
                    i_Y = i * iKd_H_Num;//横线
                    g.DrawLine(p_xy, new Point(0, i_Y), new Point((int)flPic_W, i_Y));
                }
                #endregion 2
           
                #region 3 画波形
                Pen mysum = new Pen(Color.GreenYellow, 1);
                g.DrawLines(mysum, _blSp ? Tofd_Data.m_points_2 : Tofd_Data.m_points);
                #endregion 3
                #region 3.2 提示信息
                Font drawFont_Thick = new Font("黑体", 14);
                SolidBrush drawBrush_TiTl = new SolidBrush(Color.White);
                float flTiTl_x = 7.5f * iKd_W_Num;
                PointF drawPoint_W = new PointF(flTiTl_x, 2);
                int _iN0 = SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan;
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0)
                {
                    if (SysInfo.m_i_UI_Type == 0)
                    {
                        if (Tofd_Data.m_iRun == 1)
                            SysInfo.m_strRunTitl = "距离: " + SysInfo.m_SysBuff.m_Tofd_DLL.m_flDistanc_X.ToString("f3") + "m 时长:" + Tofd_Data.m_iHaveRunTime + "s";
                        else
                            SysInfo.m_strRunTitl = "距离: " + (SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_Ck_Wave ? SysInfo.m_Plant.m_fl_Distance : SysInfo.m_SysBuff.m_Tofd_DLL.m_flDistanc_X).ToString("f3") + "m ";
                    }
                    else if (SysInfo.m_i_UI_Type == 1)
                    {
                        SysInfo.m_strRunTitl = "距离: " + SysInfo.m_SysBuff.m_Tofd_DLL.m_flDistanc_X.ToString("f3");// + "m 时长:" + Tofd_Data.m_iHaveRunTime + "s";
                    }

                    SysInfo.m_strRunTitl += "\r\n" + ("焊缝中心: " + SysInfo.m_fl_Larser_Center.ToString("f1"));
                }
                //else if (SysInfo.m_Tofd_C_Scan != null)
                //{
                //    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 && SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type == 1)
                //    {
                //        Pen Ruler_p_Limit_G = new Pen(Brushes.Red);
                //        Ruler_p_Limit_G.Width = 6;
                //        g.DrawLine(Ruler_p_Limit_G, new Point(SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_S_X, SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_Y),
                //                                    new Point(SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_E_X, SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_Y));

                //        if (SysInfo.m_SysBuff.m_Tofd_DLL.m_Data_Type == 2)
                //        {
                //            Ruler_p_Limit_G = new Pen(Brushes.Green);
                //            Ruler_p_Limit_G.Width = 6;
                //            g.DrawLine(Ruler_p_Limit_G, new Point(SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_S_X, SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_Y),
                //                                        new Point(SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_E_X, SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_Y));
                //        }
                //        #region 厚度值
                //        drawPoint_W = new PointF(150, 20);
                //        drawFont_Thick = new Font("黑体", 160);
                //        SolidBrush drawBrush_TiTl_A = new SolidBrush(Color.White);
                //        g.DrawString(SysInfo.m_SysBuff.m_Tofd_DLL.m_flThick.ToString("F2"), drawFont_Thick,
                //                       drawBrush_TiTl_A, drawPoint_W);

                //        #endregion
                //    }
                //}

                drawPoint_W = new PointF(flTiTl_x, 20);

                if (SysInfo.m_i_UI_Type == 0)
                {
                    SysInfo.m_strRunTitl += "\r\n" + "S:" + SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iN0].m_dSpeed.ToString("f0") + "m/s";
                    SysInfo.m_strRunTitl += "\r\n" + "PCS: " + SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iN0].m_fPcsLen.ToString ("f2") + "mm";

                    SysInfo.m_strRunTitl += "\r\n" + "分层:起/止/角度: "+ SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iN0].m_fPcsStart.ToString()+"/"+ 
                                                                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iN0].m_fPcsEnd.ToString()+"mm/"+
                                                                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iN0].m_fPcsAngle.ToString()+"°";
                    //       SysInfo.m_strRunTitl += " 2T0:" + SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[_iN0].T0.ToString("f0") + "us";
                }
                drawPoint_W = new PointF(flTiTl_x, 52);
                #region 计算高度
                Pen g_Sz = new Pen(Brushes.Red);
                g_Sz.Width = 4;
                g.DrawLine(g_Sz, new Point(SysInfo.m_Plant.m_i_X_A - 5, SysInfo.m_Plant.m_i_Y_A),
                                                  new Point(SysInfo.m_Plant.m_i_X_A + 5, SysInfo.m_Plant.m_i_Y_A));
                g_Sz.Width = 1;
                g.DrawLine(g_Sz, new Point(SysInfo.m_Plant.m_i_X_A, 0),
                                                     new Point(SysInfo.m_Plant.m_i_X_A, (int)flPic_H));
                float _Time = SysInfo.GetCurrTime(SysInfo.m_Plant.m_i_X_No);//  Tofd.m_pTimeBuf[SysInfo.m_Plant.m_i_X_No]);/// Tofd.m_pTimeBuf[SysInfo.m_Plant.m_i_X_No] * 0.01f;
              //  float _f1 = _Time * SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iN0].m_dSpeed / 1000f;// * 0.5f;// *
              ////      (_Time - SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[_iN0].T0);
              //  _f1 *= _f1;
              //  float _f2 = SysInfo.m_BiaoZhu.flTLW_mm;// SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iN0].m_fPcsLen;///2f;
              //  _f2 *= _f2;
                double _d = SysInfo.GetDistanc(_Time, 1);// Math.Sqrt((double)(_f1 - _f2));

        

                #endregion 计算高度
                if (SysInfo.m_i_UI_Type == 0)
                {
                    SysInfo.m_strRunTitl += "\r\n" + "X:" + SysInfo.m_Plant.m_i_X_No.ToString("f0") + " Time:" + _Time.ToString("f1") + " H:" + _d.ToString("f1");
                    drawPoint_W = new PointF(flTiTl_x, 98);
                    SysInfo.m_strRunTitl += "\r\n" + "P/V:" + Tofd.m_pChannelBuf[SysInfo.m_Plant.m_i_X_No] + "/" +
                                               Tofd.m_pValueBuf[SysInfo.m_Plant.m_i_X_No];// + "/" +
                                         //      Tofd.m_pTimeBuf[SysInfo.m_Plant.m_i_X_No];
                }
              
                    #endregion
                #region  4 刷新
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (bg != null)
                    gb.DrawImage(bg, _Rect);// 先绘制背景层
                gb.DrawImage(image, _Rect); // 再绘制绘画层

                PicArea.BackgroundImage = (Bitmap)canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布

                //        m_image = (Bitmap)image.Clone();
                g.Dispose(); g = null;
                image.Dispose();
                gb.Dispose();
                canvas.Dispose();

                PicArea.Refresh();

                p_Zb.Dispose();
                //      canvas.Dispose();
                mysum.Dispose();
                //       image.Dispose();
                #endregion 刷新
            }
            catch (Exception ee)
            { }
        }
        public int Get_Scree_No(float flDistanc_X, int iType = 0)
        {
     
            float _flDat = 0;
            if (iType == 0)
                _flDat = ((flDistanc_X) / Scree_iDotWithmm_X);//Chart_Run_flStart_Distance
            else
            {      
                int iScreenNo = SysInfo.m_Plant.Get_No(flDistanc_X);//lst_Screenkd[iScreenNo].Chart_Run_flStart_Distance
                _flDat = ((flDistanc_X - lst_Screenkd[iScreenNo].Chart_Run_flStart_Distance) / Scree_iDotWithmm_X);
            }
            int i_X = (int)_flDat;//由实际点计算屏幕开始序号
            if (_flDat - i_X >= 0.5) i_X++;
            return i_X;
        }
        /// <summary>
        /// 画TOFD的D灰度图
        /// </summary>
        /// <param name="PicArea">画板</param>
        /// <param name="iScreenNo">屏幕序号</param>
        /// <param name="Tofd_Data">数据</param>
        /// <param name="iType">0：实时数据 1：历史数据</param>
        public void Plant_D(PictureBox PicArea, int iScreenNo, Tofd Tofd_Data, float m_flDistanc_X, byte [] m_ArrWave, int iType = 0, bool blBrush = true)
        {
            //1 获得距离对应图像X轴位置
            //float _flDat = ((m_flDistanc_X - lst_Screenkd[iScreenNo].Chart_Run_flStart_Distance) / Scree_iDotWithmm_X);//Chart_Run_flStart_Distance
            //int i_X = (int)_flDat;//由实际点计算屏幕开始序号
            //if (_flDat - i_X >= 0.5) i_X++;

            int i_X = Get_Scree_No(m_flDistanc_X,1);

            i_X = (int)(Chart_Ruler_X_Start + i_X * Scree_iDotWith_X);

            //2 由帧数据对应D图
            int i_Y2 = 0,  iDat = 0;
            float i_Y = 0f;
            Color _CurColor = Color.White;
         
            m_iCurr_Run_Position = i_X;

            int _iData_Len = 0;// (SysInfo.m_i_UI_Type == 0 ? Tofd.UTS_DATA_WIDTH : SysInfo.m_Tofd_BJ.dataLength);

            for (int j = 0; j < _iData_Len; ++j)
            {
                //2.1 帧数据
                if (iType == 0)
                    iDat = Tofd.m_pChannelBuf[j];
                else
                    iDat = m_ArrWave[j];
                
                iDat = Tofd.UTS_DATA_HEIGHT - iDat;

                //数据值与颜色转换
                if (iDat > -1 && iDat < Tofd.UTS_DATA_HEIGHT)
                {
                    #region 颜色转换
                    if ( Tofd_Data.m_i_Plant_B1_D0 == 0)
                        _CurColor = Color.FromArgb(255, iDat, iDat, iDat);// m_StandcolorRange[iDat].Col  ;
                    else
                    {
                        //if (SysInfo.m_i_UI_Type ==0&&  Tofd_Data.m_pSparam[Tofd_Data.m_icurChan].m_iDemodulation_Flag != 2 ||
                        //    SysInfo.m_i_UI_Type==1 && SysInfo . m_Tofd_BJ.m_i_Cmb_Bx_Select !=3)//不是射频
                        //    _CurColor = Tofd_Data.m_Co_StandColor[iDat];
                        //else//射频
                        //{
                        //    try
                        //    {
                        //        if (iDat >= 127)
                        //            iDat -= 127;
                        //        else
                        //            iDat = 127 - iDat;

                        //        try
                        //        {
                        //            _CurColor = Tofd_Data.m_Co_StandColor[iDat];
                        //        }
                        //        catch (Exception e2)
                        //        { }
                        //    }
                        //    catch { }
                        //}
                    }
                    #endregion 
                }
                else
                    _CurColor = Color.White;

                //2.2 Y轴位置
                i_Y = Chart_Ruler_Y_Start*1.0f+ j * Scree_iDotHeight + 1.0f;

                //2.3 画图
                InitColor(m_G_C.g, i_X, i_Y, Scree_iDotWith_X, Scree_iDotHeight, _CurColor);// System.Drawing.Color.FromArgb((byte)iDat, (byte)iDat, (byte)iDat));

                #region 删除
                //2.4 射频时不清楚如何画:武汉中科的周工说，D图不用m_pValueBuf数据,它只用画波形
                //if (Tofd_Data.m_pSparam[Tofd_Data.m_icurChan].m_iDemodulation_Flag == 2)
                //{  i_Y2 = i_Y;
                //    iDat = Tofd.m_pValueBuf[ j];
                //    //数据值与颜色转换
                //    iDat = iDat;
                //    InitColor(m_G_C.g, i_X, i_Y2, Scree_iDotWith_X, Scree_iDotHeight, System.Drawing.Color.FromArgb((byte)iDat, (byte)iDat, (byte)iDat));
                //}
                #endregion 
            }
            if (blBrush)
            {
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (m_G_C.bg != null)
                    m_G_C.gb.DrawImage(m_G_C.bg, _Rect);// 先绘制背景层

                m_G_C.gb.DrawImage(m_G_C.image, _Rect); // 再绘制绘画层
                PicArea.BackgroundImage = (Bitmap)m_G_C.canvas.Clone();
            }
        }
        public void Plant_D(PictureBox PicArea, int iScreenNo, Tofd Tofd_Data, float m_flDistanc_X, string [] m_ArrWave, int iType = 0, bool blBrush = true)
        {
            //1 获得距离对应图像X轴位置
            float _flDat = ((m_flDistanc_X - lst_Screenkd[iScreenNo].Chart_Run_flStart_Distance) / Scree_iDotWithmm_X);//Chart_Run_flStart_Distance
            int i_X = (int)_flDat;//由实际点计算屏幕开始序号
            if (_flDat - i_X >= 0.5) i_X++;

            i_X = (int)(Chart_Ruler_X_Start + i_X * Scree_iDotWith_X);

            //2 由帧数据对应D图
            int i_Y2 = 0,  iDat = 0;
            float i_Y = 0;
            Color _CurColor = Color.White;
            m_iCurr_Run_Position = i_X;
            for (int j = 0; j < Tofd.UTS_DATA_WIDTH; ++j)
            {
                //2.1 帧数据
                if (iType == 0)
                    iDat = Tofd.m_pChannelBuf[j];
                else if(m_ArrWave[j]!=null )
                    iDat =int.Parse ( m_ArrWave[j]);
                iDat = Tofd.UTS_DATA_HEIGHT - iDat;

                //数据值与颜色转换
                if (iDat > -1 && iDat < Tofd.UTS_DATA_HEIGHT)
                {
                    #region 颜色转换
                    if (Tofd_Data.m_i_Plant_B1_D0 == 0)
                        _CurColor = Color.FromArgb(255, iDat, iDat, iDat);// m_StandcolorRange[iDat].Col  ;
                    else
                    {
                        if (Tofd_Data.m_pSparam[Tofd_Data.m_icurChan].m_iDemodulation_Flag != 2)//不是射频
                            _CurColor = Tofd_Data.m_Co_StandColor[iDat];
                        else//射频
                        {
                            try
                            {
                                if (iDat >= 127)
                                    iDat -= 127;
                                else
                                    iDat = 127 - iDat;

                                try
                                {
                                    _CurColor = Tofd_Data.m_Co_StandColor[iDat];
                                }
                                catch (Exception e2)
                                { }
                            }
                            catch { }
                        }
                    }
                    #endregion 
                }
                else
                    _CurColor = Color.White;

                //2.2 Y轴位置
                i_Y = Chart_Ruler_Y_Start + j * Scree_iDotHeight + 1;

                //2.3 画图
                InitColor(m_G_C.g, i_X, i_Y, Scree_iDotWith_X, Scree_iDotHeight, _CurColor);// System.Drawing.Color.FromArgb((byte)iDat, (byte)iDat, (byte)iDat));
            }
            if (blBrush)
            {
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (m_G_C.bg != null)
                    m_G_C.gb.DrawImage(m_G_C.bg, _Rect);// 先绘制背景层

                m_G_C.gb.DrawImage(m_G_C.image, _Rect); // 再绘制绘画层
                PicArea.BackgroundImage = (Bitmap)m_G_C.canvas.Clone();
            }
        }

        /// <summary>
        /// 计算屏幕行列个数
        /// </summary>
        /// <param name="PicArea"></param>
        public void GetRulerPara_C(System.Windows.Forms.PictureBox PicArea, int iRowNum)
        {
            if (PicArea == null) return;


            int iScreen_With = PicArea.Width - Chart_Ruler_X_Start;
            int iScreen_Height = PicArea.Height - Chart_Ruler_Y_Start;

            Scree_iAllCols = iScreen_With / Scree_iDotWith_X;

            #region Y轴光栅臂对应图像点宽度以及数据行数
            Scree_iAllRows = iRowNum;
            Scree_iDotHeight = float .Parse((iScreen_Height * 1.0f / iRowNum*1.0f).ToString());
          //  if (Scree_iDotHeight == 0) Scree_iDotHeight = 1;
            #endregion 行数
            //   Scree_Stant_Distance = Scree_iAllCols * Scree_iDotWithmm_X;
        }
        /// <summary>
        /// 画刻度 ：初始化、右边出图、满屏时操作
        /// </summary>
        public void Plant_Ruler_C(System.Windows.Forms.PictureBox PicArea, int iScreenNo)
        {
            if (PicArea == null) return;
            int i_X = 0, i_Y = 0;

            string strT = "";
            //1 画图工具清零
            Chart_Clear_C();
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;

            #region 画刻度尺
            Pen Ruler_Pen = new Pen(Brushes.Black);//黑刻度
            Font drawFont = new Font("黑体", (float)7.9, FontStyle.Bold);
            PointF drawPoint;
            //   SolidBrush RulerStr_Brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 0, 0, 0));
            SolidBrush RulerStr_Brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 21, 139, 207));

            float _flStart_X = lst_Screenkd[iScreenNo].Chart_Run_flStart_Distance;//X轴起点
            #region 1 画布准备:初始化、右边出图、满屏时
            m_G_C.image = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
            // 获取背景层
            m_G_C.bg = (Bitmap)PicArea.BackgroundImage;
            // 初始化整个画布
            m_G_C.canvas = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            m_G_C.g = Graphics.FromImage(m_G_C.image);
            m_G_C.gb = Graphics.FromImage(m_G_C.canvas);
            m_G_C.g.Clear(Color.White);
            m_G_C.Buff = new PointF[0];

            int iTop = PicArea.Top;

            int i_S_H = 0, i_S_L = 0;
            float _flEndKd = 0f;

            int _iGs = iRad_Dw == 0 ? 10 : 16;
            #region 1.2 画横轴刻度
            i_S_H = Chart_Ruler_Y_Start - 6; i_S_L = Chart_Ruler_Y_Start - 3;
            for (int i = 1; i <= Scree_iAllCols; i++)
            {
                //1 计算刻度位置和刻度值
                i_X = (int)(Chart_Ruler_X_Start + i * Scree_iDotWith_X);//刻度位置
                _flEndKd = _flStart_X + (i * Scree_iDotWithmm_X);//刻度值
                //2 画刻度线
                if (iRad_Dw == 0 && i % 5 != 0 || iRad_Dw == 1 && i % 4 != 0)
                    m_G_C.g.DrawLine(Ruler_Pen, new Point(i_X, i_S_L), new Point(i_X, Chart_Ruler_Y_Start));
                else
                    m_G_C.g.DrawLine(Ruler_Pen, new Point(i_X, i_S_H), new Point(i_X, Chart_Ruler_Y_Start));
                //3 画刻度值
                if (i % _iGs == 0)
                {
                    #region 标记横轴刻度值
                    if (iRad_Dw == 0)
                    {
                        _flEndKd *= 1000;
                        strT = _flEndKd.ToString("f0") + (i == _iGs ? "mm" : "");
                    }
                    else
                        strT = _flEndKd.ToString("f4") + (i == _iGs ? "in." : "");

                    drawPoint = new PointF(i_X - 15, Chart_Ruler_Word_H);
                    m_G_C.g.DrawString(strT, drawFont, RulerStr_Brush, drawPoint);
                    #endregion 标记横轴刻度值
                }

                if (i_X > PicArea.ClientSize.Width) break;
            }
            m_G_C.g.DrawLine(Ruler_Pen, Chart_Ruler_X_Start, Chart_Ruler_Y_Start, (int)(Chart_Ruler_X_Start + Scree_iAllCols * Scree_iDotWith_X), Chart_Ruler_Y_Start);
            Font drawFont_t = new Font("Arial", (float)8);
            Font drawFont_Y = new Font("黑体", (float)7.9, FontStyle.Bold);//"Arial
            m_G_C.g.DrawString("Dist.", drawFont_Y, RulerStr_Brush, -2, 1);
            m_G_C.g.DrawLine(Ruler_Pen, new Point(Chart_Ruler_X_Start, Chart_Ruler_Y_Start), new Point(Chart_Ruler_X_Start, PicArea.Height));

            #endregion 1.2 画横轴刻度

            Rectangle _Rect_Kd = new Rectangle(0, 0, PicArea.Width, PicArea.Height);
            if (m_G_C.bg != null)
            {
                try
                {
                    m_G_C.gb.DrawImage(m_G_C.bg, _Rect_Kd);// 先绘制背景层
                }
                catch (Exception de)
                { }
            }
            m_G_C.gb.DrawImage(m_G_C.image, _Rect_Kd); // 再绘制绘画层
            try
            {
                PicArea.BackgroundImage = (Bitmap)m_G_C.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                PicArea.Refresh();
            }
            catch { }
            #endregion 1 画布准备
            #endregion 刻度尺

            Application.DoEvents();
        }
        /// <summary>
        /// 获得距离对应的序号
        /// </summary>
        /// <param name="flDistanc"></param>
        /// <returns></returns>
        public int GetDistan_No(float flDistanc_X) 
        {
            int _iRet = -1;
            float _fmm = (Scree_iDotWithmm_X * 1000);
            int _imm = int.Parse(_fmm.ToString("f0"));

            float _fX = (flDistanc_X * 1000);
            int _iX = int.Parse(_fX.ToString("f0"));
            _fX = _iX / _imm;
            _iRet = int.Parse(_fX.ToString("f0"));
            return _iRet;
        }
        /// <summary>
        /// 屏幕起止刻度初始化:图像间隔改变时调用此函数,距离范围 0--N
        /// </summary>
        public void Init_ScreenKd(int PicArea_Width, int PicArea_Height)
        {
            //距离以最远距离为准
            if (lst_Screenkd.Count > 0) lst_Screenkd.Clear();
            int _iNo = 0;
            string strDw = iRad_Dw == 0 ? "f3" : "f5";
            //2 计算图像X轴Y轴间隔
            int iScreen_With = PicArea_Width - Chart_Ruler_X_Start;
            int iScreen_Height = PicArea_Height - Chart_Ruler_Y_Start;
            if (Scree_iDotWithmm_X == 0) Scree_iDotWithmm_X = 0.01f;
            //  Scree_iAllCols = iScreen_With / Scree_C_iDotWith_X;
            float Scree_Stant_Distance = Scree_iAllCols * Scree_iDotWithmm_X;

            Scree_Stant_Distance = Scree_iAllCols * Scree_iDotWithmm_X;
            //3 计算屏幕刻度
            while (true)
            {
                //1 拿数据
                Class_Screen_Kd _kd = new Class_Screen_Kd();

                if (_iNo == 0)
                {
                    _kd.Chart_Run_flStart_Distance = float.Parse((_iNo * (Scree_Stant_Distance + Scree_iDotWithmm_X)).ToString(strDw));
                    _kd.Chart_Run_flEnd_Distance = float.Parse((_iNo * (Scree_Stant_Distance + Scree_iDotWithmm_X) + Scree_Stant_Distance).ToString(strDw));
                }
                else
                {
                    _kd.Chart_Run_flStart_Distance = lst_Screenkd[_iNo - 1].Chart_Run_flEnd_Distance;
                    _kd.Chart_Run_flEnd_Distance = float.Parse((_iNo * (Scree_Stant_Distance) + Scree_Stant_Distance).ToString(strDw));
                }
                //2 添加当前数据
                lst_Screenkd.Add(_kd);
                if (_kd.Chart_Run_flEnd_Distance > flMaxDistance) return;
                //3 下一个数据
                _iNo++;
            }

        }

        /// <summary>
        /// 依据距离获得对应屏幕序号
        /// </summary>
        /// <param name="flDistance"></param>
        /// <returns></returns>
        public int Get_No(float flDistance)
        {
            int iNo = 0;
            for (int i = 0; i < lst_Screenkd.Count; i++)
            {
                if (flDistance >= lst_Screenkd[i].Chart_Run_flStart_Distance && (flDistance < lst_Screenkd[i].Chart_Run_flEnd_Distance))//|| flDistance <= m_flArr_Rul_S[i+1]))
                {
                    iNo = i;
                    //   Chart_Run_flStart_Distance = i;
                    break;
                }
            }
            return iNo;
        }
        /// <summary>
        /// 图形清零
        /// </summary>
        private void Chart_Clear_C()
        {
            if (m_G_C.g != null)
            {
                m_G_C.g.Dispose(); m_G_C.g = null;
                m_G_C.image.Dispose();
                m_G_C.gb.Dispose();
                m_G_C.canvas.Dispose();
                Application.DoEvents();
            }
        }

        /// <summary>
        /// 画色块
        /// </summary>
        /// <param name="G">画布</param>
        /// <param name="intStar_X">起点X</param>
        /// <param name="intStar_Y">起点Y</param>
        /// <param name="iWith">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <param name="color">颜色</param>
        public void InitColor(Graphics G, int intStar_X, int intStar_Y, int iWith, int iHeight, Color color)
        {
            SolidBrush mybrush = new SolidBrush(color);
            try
            {
                if (G != null)
                    G.FillRectangle(mybrush, intStar_X, intStar_Y, iWith, iHeight);
            }
            catch (Exception e)
            { }
        }
        public void InitColor(Graphics G, float  intStar_X, float intStar_Y, float iWith, float iHeight, Color color)
        {
            SolidBrush mybrush = new SolidBrush(color);
            try
            {
                if (G != null)
                    G.FillRectangle(mybrush, intStar_X, intStar_Y, iWith, iHeight);
            }
            catch (Exception e)
            { }
        }
    }
    public struct Struct_G
    {
        public Bitmap image;
        // 获取背景层
        public Bitmap bg;
        // 初始化整个画布
        public Bitmap canvas;
        // 初始化图形面板，获取这块内存画布的Graphics的引用
        public Graphics g;
        public Graphics gb;

        public int iCs_Num;
        /// <summary>
        /// 数据缓存：根据X、Y填写测量点数据
        /// </summary>
        public PointF[] Buff;
    }

    /// <summary>
    /// 当前屏幕鼠标位置的信息
    /// 距离
    /// 报文
    /// 是否有伤
    /// 伤最上距离
    /// 伤最下距离
    /// </summary>
    public class Class_Screen_Info
    {
        /// <summary>
        /// 当前距离
        /// </summary>
        public float fl_CurrentDistance = 0;
        /// <summary>
        /// 通讯报文
        /// </summary>
        public int[] btData;
        /// <summary>
        /// 是否有伤点
        /// </summary>
        public bool blMarking = false;

        /// <summary>
        /// 缺陷深度
        /// </summary>
        public int i_Defectdepth = 0;
        /// <summary>
        /// 缺陷长度
        /// </summary>
        public int i_DefectLengt = 0;
    }
    public class TcpClient_UI
    {

        //刷新视频
        // PictureBox m_Pho_Video;
        #region tcp通讯属性

        public  bool m_blOut = false;
        /// <summary>
        /// 客户端监听线程
        /// </summary>
        private Thread tcpClientThread = null;

        /// <summary>
        /// 连接服务器的客户端
        /// </summary>
        public TcpClient tcpClient = null;
        /// <summary>
        /// 连接服务器
        /// </summary>
        public bool m_blLinkServe = false;
        /// <summary>
        /// 是否成功接收数据0:失败 1：成功
        /// </summary>
        public int m_iRecevo = 0;
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string m_strServIp = "";
        /// <summary>
        /// 服务器端口号
        /// </summary>
        public string m_strPort = "";
        #endregion

        #region 数据帧格式
        /// <summary>
        /// 接收客户端数据帧头<clie>
        /// </summary>
        public string C_Head = "<clie>";
        /// <summary>
        /// 接收客户端数据帧尾</clie>
        /// </summary>
        public string C_Tail = "</clie>";
        /// <summary>
        /// 服务器发送帧头<main>
        /// </summary>
        public string S_Head = "<main>";
        /// <summary>
        /// 服务器发送帧尾</main>
        /// </summary>
        public string S_Tail = "</main>";
        #endregion
        public TcpClient_UI()//PictureBox _Pic)
        {
            // m_Pho_Video = _Pic;
            tcpClient = null;
        }
        //获取端口
        public string GetLocalEndPoint()
        {

            //this.tcpClient.Client.LocalEndPoint.ToString() = "48000";

            return tcpClient.Client.LocalEndPoint.ToString();
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="strServIp">服务器IP</param>
        /// <param name="strPort">服务器端口</param>
        /// <returns></returns>
        public bool ConnectToServer(string strServIp, string strPort)
        {
            //   csInterface csinfo = new csInterface();
            m_blLinkServe = false;

            if (strServIp == "") 
                strServIp = "127.0.0.1";
            if (strPort == "") strPort = "48100";
            try
            {
                m_strServIp = strServIp;
                m_strPort = strPort;

                if (tcpClient != null) tcpClient.Close();
                tcpClient = new TcpClient(strServIp, int.Parse(strPort));

                //延时操作
                if (tcpClient != null)
                {
                    if (tcpClientThread != null)
                    {
                        if (tcpClientThread.IsAlive)
                        {
                            //关闭线程
                            tcpClientThread.Abort();
                        }
                    }
                    tcpClientThread = new Thread(new ParameterizedThreadStart(ReceiveData));
                    int ID = 0;
                    tcpClientThread.Start((object)ID);
                    tcpClientThread.IsBackground = true;
                }
            }
            catch (Exception ex)
            {
                // throw;
                //  MessageBox.Show("集中控制器没有连接上！"+ex.Message+"请重新连接服务器，确保IP端口正确");
            }
            return m_blLinkServe;
        }


        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="Obj"></param>
        private void ReceiveData(object Obj)
        {
            int iTcp = (int)Obj;
            int num = 0, iCout = 0, iType = -1, i_StartVideo = 1 + 38 + 26;
            string[] _sPara = "".Split(',');
            byte[] _dd = new byte[10];
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_dd, 0);
            try
            {
                if (tcpClient.Connected)
                {
                    NetworkStream ns = tcpClient.GetStream();
                    while (tcpClient.Connected)
                    {
                        m_blLinkServe = true;

                        //从网络接收的可供读取的数据字节数据
                        num = tcpClient.Available;

                        byte[] Data = new byte[num];
                        if (num > 1)
                        {
                            iCout = ns.Read(Data, 0, num);
                            ns.Flush();
                        }
                        //  Application.DoEvents();
                        iCout = Data.Length - 1;
                        if (iCout > 0)//数据帧合理就处理
                        {
                            try
                            {
                                //1 解压数据   www
                                // GZip.GZIPDecompress(ref Data);//
                                //2 数据类型
                                iType = Data[0];//

                                byte[] _ArrEnd = new byte[3];
                                string _sT = "";
                                byte[] _GetData;

                                switch (iType)
                                {
                                    case 88://苏州博智惠达激光器
                                            //解析高度和对应属性值
                                            //2个字节表示一个数据，低位在前高温在后SysInfo.m_bl_Show_Video &&
                                        SysInfo.m_i_ShowXj_No++;
                                        if (SysInfo.m_i_Alarm==1 &&  SysInfo.m_i_ShowXj_No >= SysInfo.m_i_ShowXj_MaxNum)
                                        {
                                            SysInfo.m_i_ShowXj_No = 0;
                                            _ArrEnd[0] = Data[iCout - 1];//结尾有3字节FE
                                            _ArrEnd[1] = Data[iCout - 2];
                                            _ArrEnd[2] = Data[iCout];
                                            SysInfo.m_i_LasersTempr = int.Parse(_ArrEnd[1].ToString("X2"));
                                            if (!(_ArrEnd[0] == 0xFE && _ArrEnd[2] == 0xFE)) break;
                                            int _iType_050_1100 = Data[1];//激光器类型 0:050W  1: 100W

                                            int _iArrNum = _iType_050_1100 == 0 ? 1000 : 1320;
                                            byte[] _Arr_Hight = new byte[_iArrNum];//波形数据
                                            byte[] _Arr_Attrib = new byte[8];//高度属性数据

                                            IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_Hight, 0);
                                            Marshal.Copy(Data, 2, IntPtArr, _iArrNum);
                                            IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_Attrib, 0);
                                            Marshal.Copy(Data, 2 + _iArrNum, IntPtArr, 8);

                                            #region 解析数据
                                            Class_Xj_GetData _ClData = new Class_Xj_GetData();
                                            //1 解析高度
                                            byte[] _btArr = new byte[2];
                                            int _iNo = 0;
                                            if (_iType_050_1100 == 0)
                                                _ClData.dbArrData = new double[500];
                                            else
                                                _ClData.dbArrData = new double[660];
                                            string _strMsg = "";
                                          
                                            //2 解析属性 1001-1008 余高 中心点  焊缝开始  焊缝结束
                                            //余高 *100
                                            _iNo = 0;
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.dbDepth = double.Parse(byteToHexStr(_btArr)) / 100;
                                            //中心点 
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.i_Cent = int.Parse(byteToHexStr(_btArr));
                                            //焊缝开始
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.i_Start = int.Parse(byteToHexStr(_btArr));
                                            //焊缝结束
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.i_End = int.Parse(byteToHexStr(_btArr));

                                            //串口数据 1009-1034
                                            _GetData = new byte[26];//串口数据
                                            IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_GetData, 0);
                                            Marshal.Copy(Data, _iArrNum + 2 + 8, IntPtArr, 26);
                                            _sT = byteToHexStr(_GetData);
                                            _ClData.strFrame = _sT;
                                            #endregion
                                            SysInfo.m_lst_Weld.Add(_ClData);
                                       
                                            double _D = 0f;
                                            _iNo = 0;
                                            for (int i = 0; i < _iArrNum; i += 2)//1-1000 波形数据
                                            {
                                                try
                                                {
                                                    _btArr[0] = _Arr_Hight[i];
                                                    _btArr[1] = _Arr_Hight[i + 1];
                                                    _D = double.Parse(byteToHexStr(_btArr)) / 100f;
                                                    _ClData.dbArrData[_iNo++] = _D;
                                                    if (_ClData.i_Start == i)
                                                        _strMsg += "开始:";
                                                    else if (_ClData.i_End == i)
                                                        _strMsg += "结束:";
                                                    _strMsg += (i == 0 ? "" : ", ") + _D.ToString("f3");
                                                }
                                                catch (Exception e)
                                                { }
                                            }
                                            _strMsg += "余高: " + _ClData.dbDepth.ToString();
                                            _strMsg += "中心点: " + _ClData.i_Cent;
                                            _strMsg += "焊缝开始: " + _ClData.i_Start;
                                            _strMsg += "焊缝结束: " + _ClData.i_End;

                                            #region 激光中心显示值
                                            try
                                            {
                                              //  int m_i50_100 = _ClData.dbArrData.Length == 660 ? 1 : 0;
                                                if (_iType_050_1100 == 1)
                                                    SysInfo.m_fl_Larser_Center = (100f * _ClData.i_Cent / 660f);
                                                else
                                                    SysInfo.m_fl_Larser_Center = (_ClData.i_Cent / 10f);

                                                //if (SysInfo.m_bl_Larser_UP1_Down0 == false)
                                                //    SysInfo.m_fl_Larser_Center = (_iType_050_1100 == 1 ? 100 : 50) - SysInfo.m_fl_Larser_Center;
                                            }
                                            catch (Exception Elaser)
                                            {
                                                MessageBox.Show(Elaser.Message);
                                            }
                                            //    MessageBox.Show("焊缝："+SysInfo.m_fl_Larser_Center.ToString());
                                            #endregion 激光中心显示值

                                            //if (SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_Xj_Write)
                                            //    SysInfo.csInter.WriteErrorLog(SysInfo.m_Log_Weld, (SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun == 1 ? "开始测量：" : "") + _strMsg);


                                            SysInfo.m_Dog.iAddNum_Old = SysInfo.m_Dog.iAddNum;
                                            SysInfo.g_Msg_InterFace.Fun_GetServe_Data1();


                                            /*
                                             *  byte[] _Arr_Hight = new byte[1000];//高度数据
                                              byte[] _Arr_Attrib = new byte[8];//属性数据
                                            IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_Hight, 0);
                                            Marshal.Copy(Data, 1, IntPtArr, 1000);
                                            IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_Attrib, 0);
                                            Marshal.Copy(Data, 1001, IntPtArr, 8);
                                            #region 解析数据
                                            Class_Xj_GetData _ClData = new Class_Xj_GetData();
                                            //1 解析高度
                                            byte[] _btArr = new byte[2];
                                            int _iNo = 0;
                                            for (int i = 0; i < 1000; i += 2)//1-1000 测高数据
                                            {
                                                _btArr[0] = _Arr_Hight[i];
                                                _btArr[1] = _Arr_Hight[i + 1];
                                                _ClData.dbArrData[_iNo++] = double.Parse(byteToHexStr(_btArr)) / 100;
                                            }
                                            //2 解析属性 1001-1008 余高 中心点  焊缝开始  焊缝结束
                                            //余高 *100
                                            _iNo = 0;
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.dbDepth = double.Parse(byteToHexStr(_btArr)) / 100;
                                            //中心点 
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.i_Cent = int.Parse(byteToHexStr(_btArr));
                                            //焊缝开始
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.i_Start = int.Parse(byteToHexStr(_btArr));
                                            //焊缝结束
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.i_End = int.Parse(byteToHexStr(_btArr));

                                            //串口数据 1009-1034
                                            _GetData = new byte[26];//串口数据
                                            IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_GetData, 0);
                                            Marshal.Copy(Data, 1009, IntPtArr, 26);
                                            _sT = byteToHexStr(_GetData);
                                            _ClData.strFrame = _sT;
                                            #endregion
                                            SysInfo.m_lst_Weld.Add(_ClData);
                                            SysInfo.g_Msg_InterFace.Fun_GetServe_Data1();
                                            
                                           

                                            SysInfo.m_Dog.iAddNum_Old = SysInfo.m_Dog.iAddNum;
                                             */
                                        }
                                        break;
                                    case 99://视频数据
                                        /*   _ArrEnd[0] = Data[iCout - 1];
                                           _ArrEnd[1] = Data[iCout - 2];
                                           _ArrEnd[2] = Data[iCout];
                                           if (!(_ArrEnd[0] == 0xFE && _ArrEnd[1] == 0xFE && _ArrEnd[2] == 0xFE)) break;

                                           iCout = iCout - i_StartVideo;// Data.Length帧标识 + 38控制数据-26串口报文  + 视频数据
                                           SysInfo_Xj.m_Data_JC = new byte[iCout];//视频数据大小
                                           IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(SysInfo_Xj.m_Data_JC, 0);
                                           Marshal.Copy(Data, i_StartVideo, IntPtArr, iCout);

                                           //1 控制数据
                                            _GetData = new byte[38];
                                           IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_GetData, 0);
                                           Marshal.Copy(Data, 1, IntPtArr, 38);
                                           string _strT = Encoding.Default.GetString(_GetData, 0, _GetData.Length);
                                           SysInfo_Xj.m_i_Xj_Col = int.Parse(_strT.Split(',')[6]);

                                           if (SysInfo_Xj.m_blShowXunJi == false) break;//窗体没加载不显示

                                           SysInfo_Xj.m_lst_Buf_CtrDat.Add(_strT);
                                           //2 串口数据
                                           _GetData = new byte[26];
                                           IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_GetData, 0);
                                           Marshal.Copy(Data, 39, IntPtArr, 26);
                                            _sT = byteToHexStr(_GetData);// Encoding.Default.GetString(_GetData, 0, _GetData.Length);
                                           SysInfo_Xj.m_lst_Buf_COM.Add(_sT);


                                           #region 3 视频显示
                                           if (SysInfo_Xj.m_Data_JC != null)
                                           {
                                               BitmapImage bi = new BitmapImage();
                                               bi.BeginInit();
                                               bi.StreamSource = new System.IO.MemoryStream(SysInfo_Xj.m_Data_JC);
                                               bi.EndInit();
                                               try
                                               {
                                                   SysInfo_Xj.m_lst_Image.Add(bi);//  Image.FromStream(stream));
                                               }
                                               catch (Exception e)
                                               {
                                               }
                                           }
                                           #endregion 
                                           SysInfo_Xj.g_Msg_InterFace.Fun_GetServe_Data_2();
                                         */
                                        break;

                                }
                                if (m_blOut) break;
                            }
                            catch (Exception jx)
                            {
                                if (m_blOut) return;
                            }

                        }
                        Thread.Sleep(20);
                    }
                }

            }
            catch (Exception ex)
            {
                //Application.Exit();
                //this.Close();
            }
            Close();
            //this.Close();
            //Application.Exit();
        }
        private void ReceiveData_____(object Obj)
        {
            int iTcp = (int)Obj;
            int num = 0, iCout = 0, iType = -1, i_StartVideo = 1 + 38 + 26;
            string[] _sPara = "".Split(',');
            byte[] _dd = new byte[10];
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_dd, 0);
            try
            {
                if (tcpClient.Connected)
                {
                    SysInfo.csInter.WriteErrorLog(SysInfo.m_Log_Main, "循迹连接成功!");
                    NetworkStream ns = tcpClient.GetStream();
                    while (tcpClient.Connected)
                    {
                        m_blLinkServe = true;
                       
                        //从网络接收的可供读取的数据字节数据
                        num = tcpClient.Available;

                        byte[] Data = new byte[num];
                        if (num > 1)
                        {
                            iCout = ns.Read(Data, 0, num);
                            ns.Flush();
                        }
                        //  Application.DoEvents();
                        iCout = Data.Length - 1;
                        if (iCout > 0)//数据帧合理就处理
                        {
                            try
                            {
                                //1 解压数据   www
                                // GZip.GZIPDecompress(ref Data);//
                                //2 数据类型
                                iType = Data[0];//

                                byte[] _ArrEnd = new byte[3];
                                string _sT = "";
                                byte[] _GetData;

                                switch (iType)
                                {
                                    case 88://苏州博智惠达激光器
                                            //解析高度和对应属性值
                                            //2个字节表示一个数据，低位在前高温在后
                                        SysInfo.m_i_ShowXj_No++;
                                        if (SysInfo.m_bl_Show_Video && SysInfo.m_i_ShowXj_No >= SysInfo.m_i_ShowXj_MaxNum)
                                        {
                                            SysInfo.m_i_ShowXj_No = 0;
                                            _ArrEnd[0] = Data[iCout - 1];//结尾有3字节FE
                                            _ArrEnd[1] = Data[iCout - 2];
                                            _ArrEnd[2] = Data[iCout];
                                            if (!(_ArrEnd[0] == 0xFE && _ArrEnd[1] == 0xFE && _ArrEnd[2] == 0xFE)) break;
                                            int _iType_050_1100 = Data[1];//激光器类型 0:050W  1: 100W

                                            int _iArrNum = _iType_050_1100 == 0 ? 1000 : 1320;
                                            byte[] _Arr_Hight = new byte[_iArrNum];//波形数据
                                            byte[] _Arr_Attrib = new byte[8];//高度属性数据

                                            IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_Hight, 0);
                                            Marshal.Copy(Data, 2, IntPtArr, _iArrNum);
                                            IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_Attrib, 0);
                                            Marshal.Copy(Data, 2+ _iArrNum, IntPtArr, 8);
                                         
                                            #region 解析数据
                                            Class_Xj_GetData _ClData = new Class_Xj_GetData();
                                            //1 解析高度
                                            byte[] _btArr = new byte[2];
                                            int _iNo = 0;
                                            if (_iType_050_1100 == 0)
                                                _ClData.dbArrData = new double[500];
                                            else
                                                _ClData.dbArrData = new double[660];
                                            for (int i = 0; i < _iArrNum; i += 2)//1-1000 波形数据
                                            {
                                                _btArr[0] = _Arr_Hight[i];
                                                _btArr[1] = _Arr_Hight[i + 1];
                                                _ClData.dbArrData[_iNo++] = double.Parse(byteToHexStr(_btArr)) / 100;
                                            }
                                            //2 解析属性 1001-1008 余高 中心点  焊缝开始  焊缝结束
                                            //余高 *100
                                            _iNo = 0;
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.dbDepth = double.Parse(byteToHexStr(_btArr)) / 100;
                                            //中心点 
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.i_Cent = int.Parse(byteToHexStr(_btArr));
                                            //焊缝开始
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.i_Start = int.Parse(byteToHexStr(_btArr));
                                            //焊缝结束
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.i_End = int.Parse(byteToHexStr(_btArr));

                                            //串口数据 1009-1034
                                            _GetData = new byte[26];//串口数据
                                            IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_GetData, 0);
                                            Marshal.Copy(Data, _iArrNum+2+8, IntPtArr, 26);
                                            _sT = byteToHexStr(_GetData);
                                            _ClData.strFrame = _sT;
                                            #endregion
                                            SysInfo.m_lst_Weld.Add(_ClData);
                                            SysInfo.g_Msg_InterFace.Fun_GetServe_Data1();
                                            

                                            SysInfo.m_Dog.iAddNum_Old = SysInfo.m_Dog.iAddNum;

                                            /*
                                             *  byte[] _Arr_Hight = new byte[1000];//高度数据
                                              byte[] _Arr_Attrib = new byte[8];//属性数据
                                            IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_Hight, 0);
                                            Marshal.Copy(Data, 1, IntPtArr, 1000);
                                            IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_Attrib, 0);
                                            Marshal.Copy(Data, 1001, IntPtArr, 8);
                                            #region 解析数据
                                            Class_Xj_GetData _ClData = new Class_Xj_GetData();
                                            //1 解析高度
                                            byte[] _btArr = new byte[2];
                                            int _iNo = 0;
                                            for (int i = 0; i < 1000; i += 2)//1-1000 测高数据
                                            {
                                                _btArr[0] = _Arr_Hight[i];
                                                _btArr[1] = _Arr_Hight[i + 1];
                                                _ClData.dbArrData[_iNo++] = double.Parse(byteToHexStr(_btArr)) / 100;
                                            }
                                            //2 解析属性 1001-1008 余高 中心点  焊缝开始  焊缝结束
                                            //余高 *100
                                            _iNo = 0;
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.dbDepth = double.Parse(byteToHexStr(_btArr)) / 100;
                                            //中心点 
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.i_Cent = int.Parse(byteToHexStr(_btArr));
                                            //焊缝开始
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.i_Start = int.Parse(byteToHexStr(_btArr));
                                            //焊缝结束
                                            _btArr[0] = _Arr_Attrib[_iNo++];
                                            _btArr[1] = _Arr_Attrib[_iNo++];
                                            _ClData.i_End = int.Parse(byteToHexStr(_btArr));

                                            //串口数据 1009-1034
                                            _GetData = new byte[26];//串口数据
                                            IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_GetData, 0);
                                            Marshal.Copy(Data, 1009, IntPtArr, 26);
                                            _sT = byteToHexStr(_GetData);
                                            _ClData.strFrame = _sT;
                                            #endregion
                                            SysInfo.m_lst_Weld.Add(_ClData);
                                            SysInfo.g_Msg_InterFace.Fun_GetServe_Data1();
                                            
                                           

                                            SysInfo.m_Dog.iAddNum_Old = SysInfo.m_Dog.iAddNum;
                                             */
                                        }
                                        break;
                                    case 99://视频数据
                                        /*   _ArrEnd[0] = Data[iCout - 1];
                                           _ArrEnd[1] = Data[iCout - 2];
                                           _ArrEnd[2] = Data[iCout];
                                           if (!(_ArrEnd[0] == 0xFE && _ArrEnd[1] == 0xFE && _ArrEnd[2] == 0xFE)) break;

                                           iCout = iCout - i_StartVideo;// Data.Length帧标识 + 38控制数据-26串口报文  + 视频数据
                                           SysInfo_Xj.m_Data_JC = new byte[iCout];//视频数据大小
                                           IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(SysInfo_Xj.m_Data_JC, 0);
                                           Marshal.Copy(Data, i_StartVideo, IntPtArr, iCout);

                                           //1 控制数据
                                            _GetData = new byte[38];
                                           IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_GetData, 0);
                                           Marshal.Copy(Data, 1, IntPtArr, 38);
                                           string _strT = Encoding.Default.GetString(_GetData, 0, _GetData.Length);
                                           SysInfo_Xj.m_i_Xj_Col = int.Parse(_strT.Split(',')[6]);

                                           if (SysInfo_Xj.m_blShowXunJi == false) break;//窗体没加载不显示

                                           SysInfo_Xj.m_lst_Buf_CtrDat.Add(_strT);
                                           //2 串口数据
                                           _GetData = new byte[26];
                                           IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_GetData, 0);
                                           Marshal.Copy(Data, 39, IntPtArr, 26);
                                            _sT = byteToHexStr(_GetData);// Encoding.Default.GetString(_GetData, 0, _GetData.Length);
                                           SysInfo_Xj.m_lst_Buf_COM.Add(_sT);


                                           #region 3 视频显示
                                           if (SysInfo_Xj.m_Data_JC != null)
                                           {
                                               BitmapImage bi = new BitmapImage();
                                               bi.BeginInit();
                                               bi.StreamSource = new System.IO.MemoryStream(SysInfo_Xj.m_Data_JC);
                                               bi.EndInit();
                                               try
                                               {
                                                   SysInfo_Xj.m_lst_Image.Add(bi);//  Image.FromStream(stream));
                                               }
                                               catch (Exception e)
                                               {
                                               }
                                           }
                                           #endregion 
                                           SysInfo_Xj.g_Msg_InterFace.Fun_GetServe_Data_2();
                                         */
                                        break;

                                }
                                if (m_blOut) break;
                            }
                            catch (Exception jx)
                            {
                                if (m_blOut) return;
                            }

                        }
                        Thread.Sleep(20);
                    }
                }

            }
            catch (Exception ex)
            {
                //Application.Exit();
                //this.Close();
            }
            Close();
            //this.Close();
            //Application.Exit();
        }
        #region 字节型转十六进制字符串
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        #endregion


        private void WaitTimess(double dbWait)
        {
            DateTime dtStart = DateTime.Now;
            while (true)
            {
                double dbTime = DateTime.Now.Subtract(dtStart).TotalSeconds;
                if (dbTime >= dbWait) break;
                Thread.Sleep(200);
                Application.DoEvents();
            }
        }

        /// <summary>
        /// 服务器锁
        /// </summary>
        public static object m_lockSend = new object();

        public void SendDatToServe(string strKey, string strDat)
        {
            lock (m_lockSend)
            {
                try
                {

                    string _strSendDat = strKey + "/" + strDat;//发送数据:信息类型/数据
                    m_iRecevo = 0;

                    //2 准备发送
                    if (SendDataToServer(GetSendZhen(_strSendDat)) == false)
                        SendDataToServer(GetSendZhen(_strSendDat));
                }
                catch (Exception e)
                {
                }
            }
        }
        /// <summary>
        /// 将发送字符串组帧压缩
        /// </summary>
        /// <param name="strDat">发送字符串</param>
        /// <returns>压缩后数据</returns>
        public byte[] GetSendZhen(string strDat)
        {
            string strSend = C_Head + "[" + strDat + "[" + C_Tail;
            byte[] sDat = Encoding.Default.GetBytes(strSend);
            //   GZip.GZIPCompress(ref sDat);

            return sDat;
        }
        /// <summary>
        /// 发送控制信息
        /// </summary>
        /// <param name="iType">
        /// 数据类型数据范围：1：PC发送：标准色行号，标准色列号
        ///2：PC接收到：检测标准数据（6）： 视频宽度，视频高度， 中心点Y, 中心点X ,斜率，耗时
        ///3：PC发送：退出系统并断电</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public bool SendData_C(int iType, string strMsg)
        {

            bool _blRet = false;
            if (m_blLinkServe == false) return _blRet;

            strMsg = iType.ToString() + strMsg;
            byte[] Data = Encoding.Default.GetBytes(strMsg);
            //2 压缩
            //  GZip.GZIPCompress(ref Data);
            //3 发送
            SendDataToServer(Data);
            return _blRet;
        }
        /// <summary>
        ///给服务器发送数据
        ///btDat 要写入的数据：字节格式
        /// </summary>
        /// <param name="btSendDat">要写入的数据：字节格式</param>
        public bool SendDataToServer(byte[] btSendDat)
        {
            lock (m_lockSend)
            {
                bool blRet = false;
                try
                {
                    if (tcpClient != null)
                    {
                        if (tcpClient.Connected == true)
                        {
                            NetworkStream ns = tcpClient.GetStream();
                            ns.Write(btSendDat, 0, btSendDat.Length);
                            ns.Flush();
                            blRet = true;
                        }
                        else
                        {
                            ConnectToServer(m_strServIp, m_strPort);

                        }
                    }
                    else
                    {
                        ConnectToServer(m_strServIp, m_strPort);

                    }
                    return blRet;
                }
                catch (Exception e)
                {
                    return blRet;
                }
            }
        }


        /// <summary>
        /// 等待台体返回信息
        /// </summary>
        /// <param name="dbWait">等待时间，单位：秒</param>
        /// <param name="strKey">查询信息类型</param>
        public void WaitTime(double dbWait, string strKey)
        {
            DateTime dtStart = DateTime.Now;
            while (true)
            {
                if (tcpClient != null)//确认设备正常发送出去了
                {
                    if (tcpClient.Connected == true)
                    {
                        if (DateTime.Now.Subtract(dtStart).TotalSeconds > dbWait)//dbWait)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                System.Windows.Forms.Application.DoEvents();
            }
        }
        /// <summary>
        /// 关闭线程和监听
        /// </summary>
        public void Close()
        {
            try
            {
                m_blOut = true;
                try
                {
                    //       Application.ExitThread();
                    if (tcpClientThread != null)
                        if (tcpClientThread.IsAlive) tcpClientThread.Abort();//关闭线程 tcpClientThread
                    tcpClientThread = null;
                }
                catch (Exception e1)
                { }
                try
                {
                    if (tcpClient != null) tcpClient.Close();//关闭与服务器连接
                }
                catch (Exception e2)
                { }

            }
            catch (Exception e)
            {
                //   this.Close();
                Application.Exit();
                if (tcpClient != null) tcpClient.Close();//关闭与服务器连接
            }
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
            //    HLSysInfo.Close();
            try
            {
                System.Environment.Exit(0);
            }
            catch { }
            //  this.Close();
            Application.Exit();
        }
    }
    /// <summary>
    /// 接收测高寻迹数据
    /// </summary>
    public class Class_Xj_GetData
    {
        /// <summary>
        /// 测高数据
        /// </summary>
        public double[] dbArrData = new double[500];
        /// <summary>
        /// 余高
        /// </summary>
        public double dbDepth = 0;
        /// <summary>
        /// 中心点
        /// </summary>
        public int i_Cent = 0;
        /// <summary>
        /// 焊缝开始
        /// </summary>
        public int i_Start = 0;
        /// <summary>
        /// 焊缝结束
        /// </summary>
        public int i_End = 0;
        /// <summary>
        /// 串口数据
        /// </summary>
        public string strFrame = "";
    }
    public class ClassInterFace
    {
        #region  1 INI文件操作方法
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        private static object LockFile = new object();

        /// <summary>
        /// 日志文件
        /// </summary>
        public string strErrFileName = Application.StartupPath + "\\datalog\\Err.ini";
        /// <summary>
        /// void EraseSection        删除指定[Section]的全部内容
        /// string Section           [Section]
        /// string strFileName       配置文件名称
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="strFileName"></param>
        public void EraseSection(string Section, string strFileName)
        {
            WritePrivateProfileString(Section, null, null, strFileName);
        }
        /// <summary>
        /// void EraseSectionOneItem    删除指定[Section]的Key值内容
        /// string Section           [Section]
        /// string strKey            strKey=?
        /// string strFileName       配置文件名称
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="strKey"></param>
        /// <param name="strFileName"></param>
        public void EraseSectionOneItem(string Section, string strKey, string strFileName)
        {
            WritePrivateProfileString(Section, strKey, null, strFileName);
        }
        /// <summary>
        /// INIReadValue           从INI文件里读数据
        /// string Section     配置文件[]里的值
        /// string strKey         []下标记名
        /// string strValue       标记名的默认值
        /// string strFileName    配置文件名称
        /// </summary>
        public string INIReadValue(string Section, string strKey, string strValue, string strFileName)
        {   //从ini配置文件读取-----------
            StringBuilder sbTemp = new StringBuilder(1024);
            int i = GetPrivateProfileString(Section, strKey, strValue, sbTemp, 1024, strFileName);
            string strTmp = "";
            strTmp = sbTemp.ToString().Trim();
            if (strTmp.Length == 0) strTmp = strValue;//给出默认值
            return strTmp;
        }
        /// <summary>
        /// INIWriteValue         从INI文件里写数据
        /// string Section     配置文件[]里的值
        /// string strKey         []下标记名
        /// string strValue       标记名的值
        /// string strFileName    配置文件名称
        /// </summary>
        public void INIWriteValue(string Section, string strKey, string strValue, string strFileName)
        {   //写入ini配置文件-----------
            string strTmp = "";
            if (strValue != null)
            {
                strTmp = strValue.Trim();
                strValue.Replace("/n", "");		//替代回车换行
                long n = WritePrivateProfileString(Section, strKey, strTmp, strFileName);
            }
        }
        /// <summary>
        /// 给配置初始化默认参数
        /// string strValue就是默认参数
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public string IniReadDefine(string Section, string strKey, string strValue, string strFileName)
        {
            string strTmp = INIReadValue(Section, strKey, "", strFileName);
            if (strTmp.Length == 0)
            {
                strTmp = strValue;
                INIWriteValue(Section, strKey, strTmp, strFileName);
            }
            return strTmp;
        }

        /// <summary>
        /// 文件操作
        /// </summary>
        /// <param name="strPathFileName">包含路径的文件名称</param>
        /// <param name="strMsg">写入信息</param>
        /// <param name="blSendOrRec">true：发送 false:返回</param>
        public void WriteErrorLog(string strPathFileName, string strMsg, bool blSendOrRec=true )
        {
            lock (LockFile)
            {
                strMsg = strMsg.Trim();
                if (strMsg == "") return;

                strMsg += "\r\n";
                System.IO.StreamWriter swTxt = null;

                //1 判断路径是否存在
                string strPath = strPathFileName;
                if (strPathFileName.IndexOf("datalog") > 0)
                {
                    strPath = System.Windows.Forms.Application.StartupPath + "\\datalog\\";
                }
                else if (strPathFileName.IndexOf("Log") > 0)
                {
                    strPath = System.Windows.Forms.Application.StartupPath + "\\log\\";
                }

                if (System.IO.Directory.Exists(strPath) == false)
                {
                    System.IO.Directory.CreateDirectory(strPath);
                }
                //2 创建文件
                try
                {
                    swTxt = System.IO.File.AppendText(strPathFileName);
                }
                catch (Exception e)
                {

                }
                //3 写入数据
                string strValue = "";
                strValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + (blSendOrRec ? "->" : "<-") + strMsg;
                //strValue = srTxt.ReadLine();
                try
                {
                    swTxt.WriteLine(strValue);
                    swTxt.Flush();
                    swTxt.Close();
                }
                catch (Exception e)
                {
                }
                swTxt = null;
            }
        }

        /// <summary>
        /// 16进制字节变字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string HexToStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    if (i == 0)
                        returnStr += int.Parse(bytes[i].ToString("X2")).ToString();
                    else
                        returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        /// <summary>
        /// 创建当前ID文件夹
        /// </summary>
        /// <param name="ID"></param>
        public void CreatCurrDir(string ID)
        {
            //1 判断路径是否存在
            //判断是否有Temp
            string strPath = SysInfo.m_W_i_FilePath + "\\";
            if (System.IO.Directory.Exists(strPath) == false)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(strPath);
                }
                catch { }
            }
            strPath = strPath + ID;// + "\\";
            if (System.IO.Directory.Exists(strPath) == false)
            {
                System.IO.Directory.CreateDirectory(strPath);
            }
        }

       public   FileStream m_fi = null;
        /// <summary>
        /// 当前文件对应数据个数
        /// </summary>
        public int m_iNum_FileData=0;
   

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="_TofdData"></param>
        /// <param name="iType"></param>
        /// <returns></returns>
        public int FileReadByte(ref ProgressBar Prg_Print_Bar  ,string filename, ref SEmatChanParam _TofdData,
                                           int iType = 0, int iStart = 0, int iEnd = 0)
        {
                return 0;
        }

      
       
        public int FileReadByte_GetDat(string filename)
        {

            return 0;
        }
        /// <summary>
        /// 字符串转字节数组
        /// </summary>
        /// <param name="strHex"></param>
        /// <returns></returns>
        public static byte[] StrToHex(string strHex)
        {
            //清空格
            strHex = strHex.Replace(" ", "");
            if ((strHex.Length % 2) != 0)
                strHex = strHex.Insert(0, "0");
            //数组
            byte[] returnBytes = new byte[strHex.Length / 2];
            //转换
            try
            {
                for (int i = 0; i < returnBytes.Length; i++)
                    returnBytes[i] = Convert.ToByte(strHex.Substring(i * 2, 2), 16);
            }
            catch (Exception e)
            {
                return new byte[strHex.Length / 2];
                //throw e;
            }
            return returnBytes;
        }
        #endregion 文件操作

        /// <summary>
        /// 等待指定时间（秒）
        /// </summary>
        /// <param name="dbWait"></param>
        public void WaitTime(double dbWait, ref bool m_blApp)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                try
                {
                    if (m_blApp) break;
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > dbWait) break;
                    Thread.Sleep(10);
                }
                catch { break; }
            }
        }
        /// <summary>
        /// 等待制定时间
        /// </summary>
        /// <param name="dbWait">秒</param>
        public void WaitTime(double dbWait)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                try
                {

                    Application.DoEvents();
                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > dbWait) break;
                    Thread.Sleep(5);
                }
                catch { break; }
            }
        }

        public bool IsNum(string strDat)
        {
            bool _blRet = false;

            try
            {
                float _flDat = float.Parse(strDat);
                _blRet = true;

            }
            catch { }

            return _blRet;
        }

        /*
         1，C#追加文件
　　　　StreamWriter sw = File.AppendText(Server.MapPath(".")+"\\myText.txt");
　　　　sw.WriteLine("追逐理想");
　　　　sw.WriteLine("kzlll");
　　　　sw.WriteLine(".NET笔记");
　　　　sw.Flush();
　　　　sw.Close();,
2，C#拷贝文件
　　　　string OrignFile,NewFile;
　　　　OrignFile = Server.MapPath(".")+"\\myText.txt";
　　　　NewFile = Server.MapPath(".")+"\\myTextCopy.txt";
　　　　File.Copy(OrignFile,NewFile,true);
3，C#删除文件
　　　　string delFile = Server.MapPath(".")+"\\myTextCopy.txt";
　　　　File.Delete(delFile);
4，C#移动文件
　　　　string OrignFile,NewFile;
　　　　OrignFile = Server.MapPath(".")+"\\myText.txt";
　　　　NewFile = Server.MapPath(".")+"\\myTextCopy.txt";
　　　　File.Move(OrignFile,NewFile);
5，C#创建目录
// 创建目录c:\sixAge
　　　　DirectoryInfo d=Directory.CreateDirectory("c:\\sixAge");
// d1指向c:\sixAge\sixAge1
　　　　DirectoryInfo d1=d.CreateSubdirectory("sixAge1");
// d2指向c:\sixAge\sixAge1\sixAge1_1
　　　　DirectoryInfo d2=d1.CreateSubdirectory("sixAge1_1");
// 将当前目录设为c:\sixAge
　　　　Directory.SetCurrentDirectory("c:\\sixAge");
// 创建目录c:\sixAge\sixAge2
　　　　Directory.CreateDirectory("sixAge2");
// 创建目录c:\sixAge\sixAge2\sixAge2_1
　　　　Directory.CreateDirectory("sixAge2\\sixAge2_1");
         */
        /// <summary>
        /// 文件拷贝
        /// </summary>
        /// <param name="OldPathFile"></param>
        /// <param name="NewPathFile"></param>
        /// <returns></returns>
        public bool FileCopy(string OldPathFile, string NewPathFile)
        {
            bool blRet = false;
            try
            {
                System.IO.File.Copy(OldPathFile, NewPathFile, true);
                blRet = System.IO.File.Exists(NewPathFile);
            }
            catch (Exception e)
            { }
            return blRet;
        }
        /// <summary>
        /// 修改文件名字
        /// </summary>
        /// <param name="sourceFileName">带路径的老文件名</param>
        /// <param name="destFileName">带路径的新文件名</param>
        public bool Change_FileName(string sourceFileName, string destFileName)
        {
            bool _blRet = false;
            //string[] strDirs_S = System.IO.Directory.GetDirectories(sourceFileName);
            //if (System.IO.Directory.Exists(sourceFileName))
            //{
            //    string[] strDirs_d = System.IO.Directory.GetDirectories(destFileName);
            //    if (strDirs_S[0] == strDirs_d[0])
            //    {
            try
            {
                System.IO.File.Move(sourceFileName, destFileName); _blRet = true;
            }
            catch (Exception e)
            { }
            //    }
            //}
            return _blRet;
        }
        public string[] GetLatestFiles(string Path, int count)
        {
            string[] strArr = new string[1];

            string strPath = Application.StartupPath;// AppDomain.CurrentDomain.BaseDirectory;
            Path = strPath + "\\" + Path;
            if (System.IO.Directory.Exists(Path))
            {
                var query = (from f in System.IO.Directory.GetFiles(Path, "*.mdb")
                             let fi = new System.IO.FileInfo(f)
                             orderby fi.CreationTime descending
                             select fi.FullName).Take(count);
                strArr = query.ToArray();
                // return query.ToArray();
            }
            if (strArr != null)
            {
                if (strArr.Count() > 0)
                {
                    string[] sPara = "".Split(',');

                    for (int i = 0; i < strArr.Count(); i++)
                    {
                        if (strArr[i] != "")
                            sPara = strArr[i].Split('\\');
                        if (sPara.Count() > 0)
                            strArr[i] = sPara[sPara.Count() - 1];
                    }
                }
            }
            return strArr;
        }

        /// <summary>
        /// 清空文件夹下
        /// </summary>
        /// <param name="strDir">目录地址:文件夹名字</param>
        public void DeleteFiles(string strDir)
        {
            try
            {
                string strPath = Application.StartupPath;// AppDomain.CurrentDomain.BaseDirectory;
                strDir = strPath + "\\" + strDir;
                if (System.IO.Directory.Exists(strDir))
                {
                    string[] strDirs = System.IO.Directory.GetDirectories(strDir);
                    string[] strFiles = System.IO.Directory.GetFiles(strDir);
                    foreach (string strFile in strFiles)
                    {
                        System.IO.File.Delete(strFile);
                    }

                    //foreach (string strdir in strDirs)
                    //{
                    //    Directory.Delete(strdir, true);
                    //}
                    Console.WriteLine("删除成功！");
                }
                else
                {
                    Console.WriteLine("此目录不存在！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除文件夹保存：" + ex.Message);
            }
        }

        /// <summary>
        /// 删除当前路径下文件夹下某文件(例如：\\datalog\\GWJMRunMsg.ini)或者当前路径下文件  陈大伟 
        /// </summary>
        /// <param name="strPathFile">格式：\\datalog\\GWJMRunMsg.ini</param>
        public bool DeleFile(string strPathFile, int iType = 1)
        {
            bool _blRet = true;
            try
            {
                if (strPathFile == "") return _blRet;
                string strPath = "";
                int iS = strPathFile.IndexOf('\\');
                if (iS == 0)
                    strPath = Application.StartupPath;// AppDomain.CurrentDomain.BaseDirectory;
                else
                    strPath = AppDomain.CurrentDomain.BaseDirectory;

                string _strPathFile = strPath + strPathFile;
                if (iType == 1 && strPathFile != "")
                    _strPathFile = strPathFile;
                if (System.IO.File.Exists(_strPathFile))
                    System.IO.File.Delete(_strPathFile);
            }
            catch (Exception Err)
            {
                _blRet = false;
                //       MessageBox.Show("删除文件：" + strPathFile + "出错！" + Err.Message);
            }
            return _blRet;
        }

        /// <summary>
        /// 复制文件夹及文件：复制并没有包括原文件的根目录名称(要复制的文件除了根目录文件夹以外其他的都原封不动地搬到了目的地),
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public int CopyFolder(string sourceFolder, string destFolder)
        {
            try
            {
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }
                //得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    System.IO.File.Copy(file, dest);//复制文件
                }
                //得到原文件根目录下的所有文件夹
                string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    CopyFolder(folder, dest);//构建目标路径,递归复制文件
                }
                return 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;
            }

        }

        /// <summary>
        /// 复制文件夹及文件：复制包括了原文件的根目录名称(要复制的文件原封不动的搬到目的地),
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public int CopyFolder2(string sourceFolder, string destFolder)
        {
            try
            {
                string folderName = System.IO.Path.GetFileName(sourceFolder);
                string destfolderdir = System.IO.Path.Combine(destFolder, folderName);//包含根目录名称
                string[] filenames = System.IO.Directory.GetFileSystemEntries(sourceFolder);
                foreach (string file in filenames)// 遍历所有的文件和目录
                {
                    if (System.IO.Directory.Exists(file))
                    {
                        string currentdir = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(currentdir))
                        {
                            System.IO.Directory.CreateDirectory(currentdir);
                        }
                        CopyFolder2(file, destfolderdir);
                    }
                    else
                    {
                        string srcfileName = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(destfolderdir))
                        {
                            System.IO.Directory.CreateDirectory(destfolderdir);
                        }
                        try
                        {
                            System.IO.File.Copy(file, srcfileName);
                        }
                        catch(Exception e) { MessageBox.Show(e.Message); }
                    }
                }

                return 1;
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
                return 0;
            }

        }

        
    }


    /// <summary>
    /// 设置GridView双缓冲
    /// </summary>
    public static class DublGrid
    {
        /// <summary>
        /// 将给定的DataGridView设置双缓冲
        /// </summary>
        /// <param name="dgv">给定的DataGridView</param>
        /// <param name="b">设置为ture即打开双缓冲</param>
        public static void SetDoubleBuffered(this DataGridView dgv, bool b)
        {
            var dgvType = dgv.GetType();
            var pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, b, null);
        }
    }
    public class Cl_Mark
    {
        /// <summary>
        /// 记录需要打标位置
        /// </summary>
        public float fl_X = 0;
        /// <summary>
        /// 是否已经打标
        /// </summary>
        public bool blHaveMark = false;
    }

    #region 钩子处理
    class KeyboardHook
    {
        public event System.Windows.Forms.KeyEventHandler KeyDownEvent;
        //public event KeyPressEventHandler KeyPressEvent;
        public event System.Windows.Forms.KeyEventHandler KeyUpEvent;

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        static int hKeyboardHook = 0; //声明键盘钩子处理的初始值
        public const int WH_KEYBOARD_LL = 13;   //线程键盘钩子监听鼠标消息设为2，全局键盘监听鼠标消息设为13
        HookProc KeyboardHookProcedure; //声明KeyboardHookProcedure作为HookProc类型
        //键盘结构
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode;  //定一个虚拟键码。该代码必须有一个价值的范围1至254
            public int scanCode; // 指定的硬件扫描码的关键
            public int flags;  // 键标志
            public int time; // 指定的时间戳记的这个讯息
            public int dwExtraInfo; // 指定额外信息相关的信息
        }
        //使用此功能，安装了一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);


        //调用此函数卸载钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);


        //使用此功能，通过信息钩子继续下一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        // 取得当前线程编号（线程钩子需要用到）
        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();

        //使用WINDOWS API函数代替获取当前实例的函数,防止钩子失效
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        public void Start()
        {
            // 安装键盘钩子
            if (hKeyboardHook == 0)
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
                //hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                //************************************
                //键盘线程钩子
                //SetWindowsHookEx( 2,KeyboardHookProcedure, IntPtr.Zero, GetCurrentThreadId());//指定要监听的线程idGetCurrentThreadId(),
                //键盘全局钩子,需要引用空间(using System.Reflection;)
                //SetWindowsHookEx( 13,MouseHookProcedure,Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),0);
                //
                //关于SetWindowsHookEx (int idHook, HookProc lpfn, IntPtr hInstance, int threadId)函数将钩子加入到钩子链表中，说明一下四个参数：
                //idHook 钩子类型，即确定钩子监听何种消息，上面的代码中设为2，即监听键盘消息并且是线程钩子，如果是全局钩子监听键盘消息应设为13，
                //线程钩子监听鼠标消息设为7，全局钩子监听鼠标消息设为14。lpfn 钩子子程的地址指针。如果dwThreadId参数为0 或是一个由别的进程创建的
                //线程的标识，lpfn必须指向DLL中的钩子子程。 除此以外，lpfn可以指向当前进程的一段钩子子程代码。钩子函数的入口地址，当钩子钩到任何
                //消息后便调用这个函数。hInstance应用程序实例的句柄。标识包含lpfn所指的子程的DLL。如果threadId 标识当前进程创建的一个线程，而且子
                //程代码位于当前进程，hInstance必须为NULL。可以很简单的设定其为本应用程序的实例句柄。threaded 与安装的钩子子程相关联的线程的标识符
                //如果为0，钩子子程与所有的线程关联，即为全局钩子
                //************************************
                //如果SetWindowsHookEx失败
                if (hKeyboardHook == 0)
                {
                    Stop();
                    throw new Exception("安装键盘钩子失败");
                }
            }
        }
        public void Stop()
        {
            bool retKeyboard = true;


            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }

            if (!(retKeyboard)) throw new Exception("卸载钩子失败！");
        }
        //ToAscii职能的转换指定的虚拟键码和键盘状态的相应字符或字符
        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, //[in] 指定虚拟关键代码进行翻译。
                                         int uScanCode, // [in] 指定的硬件扫描码的关键须翻译成英文。高阶位的这个值设定的关键，如果是（不压）
                                         byte[] lpbKeyState, // [in] 指针，以256字节数组，包含当前键盘的状态。每个元素（字节）的数组包含状态的一个关键。如果高阶位的字节是一套，关键是下跌（按下）。在低比特，如果设置表明，关键是对切换。在此功能，只有肘位的CAPS LOCK键是相关的。在切换状态的NUM个锁和滚动锁定键被忽略。
                                         byte[] lpwTransKey, // [out] 指针的缓冲区收到翻译字符或字符。
                                         int fuState); // [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise.

        //获取按键的状态
        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        private const int WM_KEYDOWN = 0x100;//KEYDOWN
        private const int WM_KEYUP = 0x101;//KEYUP
        private const int WM_SYSKEYDOWN = 0x104;//SYSKEYDOWN
        private const int WM_SYSKEYUP = 0x105;//SYSKEYUP

        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            // 侦听键盘事件
            if ((nCode >= 0) && (KeyDownEvent != null || KeyUpEvent != null))
            {
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                //键盘按下
                if (KeyDownEvent != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    System.Windows.Forms.Keys keyData = (System.Windows.Forms.Keys)MyKeyboardHookStruct.vkCode;
                    System.Windows.Forms.KeyEventArgs e = new System.Windows.Forms.KeyEventArgs(keyData);
                    KeyDownEvent(this, e);
                }

                // 键盘抬起
                if (KeyUpEvent != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    System.Windows.Forms.Keys keyData = (System.Windows.Forms.Keys)MyKeyboardHookStruct.vkCode;
                    System.Windows.Forms.KeyEventArgs e = new System.Windows.Forms.KeyEventArgs(keyData);
                    KeyUpEvent(this, e);
                }
            }
            //如果返回1，则结束消息，这个消息到此为止，不再传递。
            //如果返回0或调用CallNextHookEx函数则消息出了这个钩子继续往下传递，也就是传给消息真正的接受者
            return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }

        ~KeyboardHook()
        {
            Stop();
        }
    }

    class KeyboardHook_M
    {
        public event System.Windows.Forms.KeyEventHandler KeyDownEvent;
        //public event KeyPressEventHandler KeyPressEvent;
        public event System.Windows.Forms.MouseEventHandler KeyUpEvent;

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        static int hMouseHook = 0; //声明键盘钩子处理的初始值

        public const int WH_MOUSE_LL = 14;   //线程键盘钩子监听鼠标消息设为2，全局键盘监听鼠标消息设为13  鼠标全局为14
        HookProc MouseHookProcedure; //声明KeyboardHookProcedure作为HookProc类型

        //ToAscii职能的转换指定的虚拟键码和键盘状态的相应字符或字符
        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, //[in] 指定虚拟关键代码进行翻译。
                                         int uScanCode, // [in] 指定的硬件扫描码的关键须翻译成英文。高阶位的这个值设定的关键，如果是（不压）
                                         byte[] lpbKeyState, // [in] 指针，以256字节数组，包含当前键盘的状态。每个元素（字节）的数组包含状态的一个关键。如果高阶位的字节是一套，关键是下跌（按下）。在低比特，如果设置表明，关键是对切换。在此功能，只有肘位的CAPS LOCK键是相关的。在切换状态的NUM个锁和滚动锁定键被忽略。
                                         byte[] lpwTransKey, // [out] 指针的缓冲区收到翻译字符或字符。
                                         int fuState); // [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise.

        public event System.Windows.Forms.MouseEventHandler OnMouseActivity;
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hWnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }
        //   private static extern short GetKeyState(int vKey);

        private const int WM_LBUTTONDOWN = 0x0201;//
        private const int WM_RBUTTONDOWN = 0x0204;//
        private const int WM_SYSKEYDOWN = 0x104;//SYSKEYDOWN
        private const int WM_SYSKEYUP = 0x105;//SYSKEYUP
        private const int WM_LBUTTONDBLCLK = 0x0203;
        private const int WM_RBUTTONDBLCLK = 0x0206;
        /*
           #define WM_MOUSEFIRST                   0x0200
            #define WM_MOUSEMOVE                    0x0200
            #define WM_LBUTTONDOWN                  0x0201
            #define WM_LBUTTONUP                    0x0202
            #define WM_LBUTTONDBLCLK                0x0203
            #define WM_RBUTTONDOWN                  0x0204
            #define WM_RBUTTONUP                    0x0205
            #define WM_RBUTTONDBLCLK                0x0206
            #define WM_MBUTTONDOWN                  0x0207
            #define WM_MBUTTONUP                    0x0208
            #define WM_MBUTTONDBLCLK                0x0209  
         */

        //键盘结构
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode;  //定一个虚拟键码。该代码必须有一个价值的范围1至254
            public int scanCode; // 指定的硬件扫描码的关键
            public int flags;  // 键标志
            public int time; // 指定的时间戳记的这个讯息
            public int dwExtraInfo; // 指定额外信息相关的信息
        }



        //使用此功能，安装了一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);


        //调用此函数卸载钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);


        //使用此功能，通过信息钩子继续下一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        // 取得当前线程编号（线程钩子需要用到）
        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();

        //使用WINDOWS API函数代替获取当前实例的函数,防止钩子失效
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        public void Start()
        {
            // 安装键盘钩子
            if (hMouseHook == 0)
            {
                MouseHookProcedure = new HookProc(MouseHookProc);
                hMouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, GetModuleHandle("user32"), 0);// System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
                //hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                //************************************
                //键盘线程钩子
                //SetWindowsHookEx( 2,KeyboardHookProcedure, IntPtr.Zero, GetCurrentThreadId());//指定要监听的线程idGetCurrentThreadId(),
                //键盘全局钩子,需要引用空间(using System.Reflection;)
                //SetWindowsHookEx( 13,MouseHookProcedure,Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),0);
                //
                //关于SetWindowsHookEx (int idHook, HookProc lpfn, IntPtr hInstance, int threadId)函数将钩子加入到钩子链表中，说明一下四个参数：
                //idHook 钩子类型，即确定钩子监听何种消息，上面的代码中设为2，即监听键盘消息并且是线程钩子，如果是全局钩子监听键盘消息应设为13，
                //线程钩子监听鼠标消息设为7，全局钩子监听鼠标消息设为14。lpfn 钩子子程的地址指针。如果dwThreadId参数为0 或是一个由别的进程创建的
                //线程的标识，lpfn必须指向DLL中的钩子子程。 除此以外，lpfn可以指向当前进程的一段钩子子程代码。钩子函数的入口地址，当钩子钩到任何
                //消息后便调用这个函数。hInstance应用程序实例的句柄。标识包含lpfn所指的子程的DLL。如果threadId 标识当前进程创建的一个线程，而且子
                //程代码位于当前进程，hInstance必须为NULL。可以很简单的设定其为本应用程序的实例句柄。threaded 与安装的钩子子程相关联的线程的标识符
                //如果为0，钩子子程与所有的线程关联，即为全局钩子
                //************************************
                //如果SetWindowsHookEx失败
                if (hMouseHook == 0)
                {
                    Stop();
                    throw new Exception("安装鼠标钩子失败");
                }
            }
        }
        public void Stop()
        {
            bool retKeyboard = true;


            if (hMouseHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hMouseHook);
                hMouseHook = 0;
            }

            if (!(retKeyboard)) throw new Exception("卸载钩子失败！");
        }
        private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            // 侦听键盘事件
            if ((nCode >= 0) && (OnMouseActivity != null))//&& (KeyDownEvent != null || KeyUpEvent != null))
            {
                int clickCount = 0;
                MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct)); System.Windows.Forms.MouseButtons button = System.Windows.Forms.MouseButtons.None;
                //鼠标左键
                if ((wParam == WM_LBUTTONDOWN))//||KeyDownEvent != null &&  wParam == WM_SYSKEYDOWN))
                {
                    button = System.Windows.Forms.MouseButtons.Left;
                    clickCount = 1;

                    //System.Windows.Forms.Keys keyData = (System.Windows.Forms.Keys)MyKeyboardHookStruct.vkCode;
                    //System.Windows.Forms.KeyEventArgs e = new System.Windows.Forms.KeyEventArgs(keyData);
                    //KeyDownEvent(this, e);
                }

                // 鼠标右键
                //if (KeyUpEvent != null && (wParam == WM_RBUTTONDOWN || wParam == WM_SYSKEYUP))
                //{
                //    System.Windows.Forms.Keys keyData = (System.Windows.Forms.Keys)MyKeyboardHookStruct.vkCode;
                //    System.Windows.Forms.KeyEventArgs e = new System.Windows.Forms.KeyEventArgs(keyData);
                //    KeyUpEvent(this, e);
                //}



                if (button != System.Windows.Forms.MouseButtons.None)

                    if (wParam == WM_LBUTTONDBLCLK || wParam == WM_RBUTTONDBLCLK) clickCount = 2;

                    else clickCount = 1;
                System.Windows.Forms.MouseEventArgs e = new
                    System.Windows.Forms.MouseEventArgs(
                                                   button,
                                                   clickCount,
                                                   MyMouseHookStruct.pt.x,
                                                   MyMouseHookStruct.pt.y,
                                                   0);

                //raise it  

                OnMouseActivity(this, e);
            }
            //如果返回1，则结束消息，这个消息到此为止，不再传递。
            //如果返回0或调用CallNextHookEx函数则消息出了这个钩子继续往下传递，也就是传给消息真正的接受者
            return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }

        ~KeyboardHook_M()
        {
            Stop();
        }
    }
    #endregion 钩子

    public class ClDog
    {
        /// <summary>
        /// 心跳
        /// </summary>
        public int iAddNum = 0;
        /// <summary>
        /// 上次数值
        /// </summary>
        public int iAddNum_Old = -1;
        /// <summary>
        /// 是否有异常现象 -1:初始  0：运行 1：有异常 
        /// </summary>
        public int  ilAlarm = -1;
        /// <summary>
        /// 异常发现起始时间
        /// </summary>
        public DateTime dtStar = DateTime.Now;
        /// <summary>
        /// 异常持续时长
        /// </summary>
        public int iWaitLen = 4;
        /// <summary>
        /// 异常持续时长超过门限3秒
        /// </summary>
        public bool bl_Dog = false;

    }
    /// <summary>
    /// 打印：项目名称
    /// </summary>
    public class Cl_Print_Item
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string strItemName = "";
        /// <summary>
        /// 文件路径
        /// </summary>
        public string strItemName_Path = "";
        /// <summary>
        /// 项目名称对应的项目记录，实际是项目的焊缝检测记录
        /// </summary>
        public List<Cl_Print_Record> m_LstRecord = new List<Cl_Print_Record>();
    }
    /// <summary>
    /// 打印：项目检测记录
    /// </summary>
    public class Cl_Print_Record
    {
        /// <summary>
        /// 是否选中当前记录，默认：没有选中
        /// </summary>
        public bool blCheck = true  ;
        /// <summary>
        /// 当前焊缝记录文件
        /// </summary>
        public string strRecordName = "";
        /// <summary>
        /// 文件路径
        /// </summary>
        public string strRecordName_Path = "";
    }



    /// <summary>
    /// 距离传递服务器
    /// </summary>
    public class Class_Server_Mul_Distanc
    {
        /// <summary>
        /// 联机上客户端
        /// </summary>
        public bool m_blLink = false;
        /// <summary>
        /// 是否发送数据
        /// </summary>
        public bool m_blSend = false;
        /// <summary>
        /// 接收到数据
        /// </summary>
        public bool m_blRecev = false;

        #region 数据帧格式
        /// <summary>
        /// 接收客户端数据帧头<clie>
        /// </summary>
        public string C_Head = "<clie>";
        /// <summary>
        /// 接收客户端数据帧尾 </clie>
        /// </summary>
        public string C_Tail = "</clie>";
        /// <summary>
        /// 服务器发送帧头<main>
        /// </summary>
        public string S_Head = "<main>";
        /// <summary>
        /// 服务器发送帧尾</main>
        /// </summary>
        public string S_Tail = "</main>";
        #endregion
        /// <summary>
        /// 服务器
        /// </summary>
        public TcpListener m_TcpServer = null;
        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient m_TcpC = null;
        /// <summary>
        /// 多个客户端
        /// </summary>
        public TcpClient[] m_TcpArr = new TcpClient[2];
        /// <summary>
        /// 通讯线程
        /// </summary>
        Thread m_trdServer = null;

        Thread[] m_treaArr = new Thread[2];
        /// <summary>
        /// 解析网络数据
        /// </summary>
        Thread m_trd_Parsing = null;
        /// <summary>
        /// 终端服务器监听线程
        /// </summary>
        private Thread TreadTcp_Server;

        ///// <summary>
        ///// 启动涡流程序
        ///// </summary>
        //public void Run_Client()
        //{
        //    Thread tdS = new Thread(new ThreadStart(LoadClientProgram));
        //    tdS.Start();
        //    tdS.IsBackground = true;
        //}
        /// <summary>
        /// 关闭客户端
        /// </summary>
        public void End_Client()
        {
            SendData("", "99");
        }
        //private void LoadClientProgram()
        //{
        //    System.Diagnostics.Process pAppInterface = new System.Diagnostics.Process();
        //    pAppInterface.StartInfo.FileName = Application.StartupPath + "\\Auto_Pulsed_Eddy.exe";
        //    pAppInterface.Start();
        //    pAppInterface.Close();
        //}

        /// <summary>
        /// 启动服务器监听的方式
        /// </summary>
        /// <param name="strServIp">127.0.0.1</param>
        /// <param name="Port">48100</param>
        public void Start(string strServIp = "127.0.0.1", int Port = 48111)
        {
            try
            {
                if (TreadTcp_Server != null) TreadTcp_Server.Abort();
                if (m_TcpServer != null) m_TcpServer.Stop();

                m_TcpServer = new TcpListener(System.Net.IPAddress.Parse(strServIp == "" ? "127.0.0.1" : strServIp), Port);
                TreadTcp_Server = new Thread(new ThreadStart(StartListen));
                TreadTcp_Server.Start();
                TreadTcp_Server.IsBackground = true;
            }
            catch (Exception ex)
            {
            }
        }
        private void StartListen()
        {
            /*
            Thread myThread = new Thread(new ParameterizedThreadStart(printReceiveMsg));
            myThread.Start(client);
             */
            try
            {
                m_TcpServer.Start();
                SysInfo.csInter.WriteErrorLog(SysInfo.m_Log_Main, "距离服务器启动...");
                //             Run_Client();m_ServerUI
                while (true && SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun != 10)//
                {
                    if (m_TcpServer.Pending())
                    {
                        //if (m_TcpC != null)
                        //    m_TcpC.Close();
                        TcpClient _TcpC = null;
                        m_TcpC = m_TcpServer.AcceptTcpClient();
                        //MessageBox.Show("客户端收到");
                        // if (m_trdServer != null) m_trdServer.Abort();
                        Thread _trdServer = new Thread(new ParameterizedThreadStart(AcceptClientMsg));
                        _trdServer.Start(m_TcpC);
                        _trdServer.IsBackground = true;

                        Thread.Sleep(100);
                        Application.DoEvents();
                        //   break;
                    }
                }
            }
            catch (Exception Err)
            {
            }
        }

        private void StartListen_Mul()
        {
            try
            {
                m_TcpServer.Start();
                //             Run_Client();m_ServerUI
                while (true)//
                {
                    if (m_TcpServer.Pending())
                    {
                      //  TcpClient _TcpC = null;

                        m_TcpC = null;
                        m_TcpC = m_TcpServer.AcceptTcpClient();
                        //  m_TcpArr.Add(_TcpC);
                        //MessageBox.Show("客户端收到");
                        if (m_trdServer != null) m_trdServer.Abort();
                        m_trdServer = new Thread(new ParameterizedThreadStart(AcceptClientMsg));
                        m_trdServer.Start(m_TcpC);
                        m_trdServer.IsBackground = true;

                        Thread.Sleep(100);
                        Application.DoEvents();
                        break;
                    }
                }
            }
            catch (Exception Err)
            {
            }
        }
        private void NetParsing(string strMsg)//陈大伟WWW
        {
            if (m_trd_Parsing != null) m_trd_Parsing.Abort();
            m_trd_Parsing = new Thread(new ParameterizedThreadStart(Parsing));
            m_trd_Parsing.Start(strMsg);
            m_trd_Parsing.IsBackground = true;
        }
        private void Parsing(object ObjMsg)
        {
            string _strMsg = (string)ObjMsg;
            ExplainClientMsg(_strMsg);
        }
        private void AcceptClientMsg(object arg)
        {
            TcpClient TcpC = (TcpClient)arg;
            m_blLink = false;
            string strMsg = "";
            string _strRet = "";
            if (TcpC != null)
            {
                NetworkStream ns = TcpC.GetStream();
                while (TcpC.Connected == true && SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun != 10)
                {
                    try
                    {
                        Thread.Sleep(10);
                        m_blLink = true;
                        int num = TcpC.Available;
                        if (num > 0)
                        {
                            //1 接收
                            byte[] Msg = new byte[num];
                            int Count = TcpC.Client.Receive(Msg);

                            if (Msg != null && Msg.Length > 0)
                            {
                                //2 拿返回数据
                                strMsg = Encoding.Default.GetString(Msg, 0, Msg.Length);
                                if (strMsg.Length > 0)
                                {
                                    //  NetParsing(strMsg);//
                                    _strRet = ExplainClientMsg(strMsg);
                                    if (strMsg.IndexOf("19.0") > 0)
                                    {
                                        switch (_strRet)
                                        {
                                            case "1"://视频客户端
                                                SysInfo.csInter.WriteErrorLog(SysInfo.m_Log_Main, "收到相机客户端连接确认消息");
                                                m_TcpArr[0] = new TcpClient();
                                                m_TcpArr[0] = TcpC;
                                                break;
                                            case "2"://tofd波形客户端
                                                SysInfo.csInter.WriteErrorLog(SysInfo.m_Log_Main, "收到TOFD客户端连接确认消息");
                                                m_TcpArr[1] = new TcpClient();
                                                m_TcpArr[1] = TcpC;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {

                        //WriteErrorLog(ee.Message);
                        // return;
                    }
                    Thread.Sleep(100);
                    Application.DoEvents();
                }
            }
        }
        /// <summary>
        /// 数据解析
        /// </summary>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        private string ExplainClientMsg(string strRetDat)
        {
            string strRet = "";

            #region 依据帧头 帧尾 截取有效数据
            int _iT = strRetDat.IndexOf(C_Head);

            strRetDat = strRetDat.Substring(_iT);
            _iT = strRetDat.IndexOf(C_Tail);
            if (_iT > -1)
                strRetDat = strRetDat.Substring(0, _iT + C_Tail.Length);
            #endregion 

            string[] sPara = strRetDat.Split('[');
            if (sPara.Length > 3)
            {
                int _iS = strRetDat.IndexOf("/main>");
                strRetDat = strRetDat.Substring(0, _iS + 6);
                sPara = strRetDat.Split('[');
            }
            //数据格式：<main/clie> + "[" + 数据 + "[" + </main/clie>  数据格式：数据类型 / 数据内容
            //例如：<clie> + "[" + 1.0/235432 + "[" + </clie>
            if (sPara.Length == 3)
            {
                //2 确认包
                if (sPara[0].ToLower() == C_Head && sPara[2].ToLower() == C_Tail)
                {
                    string[] _sPara = sPara[1].Split('/'); //数据格式：数据类型 / 数据内容
                    if (_sPara.Length == 2)
                    {
                        if (_sPara[0] == "4.0")//数据类型 1.0：数据格式： 视频帧号标志 / 视频帧号
                        {
                            try
                            {
                                //SysInfo.m_SysInfo.g_Climb.m_iVideo_No = int.Parse(_sPara[1]);
                                //SysInfo.g_Msg_InterFace.Fun_RunInfo("3#" + SysInfo.m_SysInfo.g_Climb.m_iVideo_No.ToString());
                                //if (SysInfo.m_SysInfo.g_Video.blNetTrue == false)
                                //    SysInfo.m_SysInfo.g_Video.blNetTrue = true;
                                //  SysInfo.blNetTrue_Video = true;
                                m_blRecev = true;
                            }
                            catch (Exception ee
                            )
                            { m_blRecev = false; }
                        }
                        if (_sPara[0] == "8.0")
                        {
                            //重新启动
                            SysInfo.g_Msg_InterFace.Fun_ReLink_Videl();//SysInfo.m_SysInfo.g_Video.dt_GetDataTime = new DateTime();
                        }
                        if (_sPara[0] == "10.0")//数据类型 1.0：数据格式： 视频帧号标志 / 视频帧号
                        {
                            try
                            {
                                //  SysInfo.m_SysInfo.g_Climb.m_iVideo_No = int.Parse(_sPara[1]);

                                //    SysInfo.g_Msg_InterFace.Fun_RunInfo("3#" + _sPara[1]);
                                try
                                {
                                    SysInfo.strNetTrue_Video = _sPara[1];
                                    SysInfo.m_ServerUI.SendData_M(0, "", "7.0");
                                }
                                catch { }


                                m_blRecev = true;
                            }
                            catch (Exception ee
                            )
                            { m_blRecev = false; }
                        }
                        if (_sPara[0] == "18.0")//
                        {
                            //SysInfo.m_SysInfo.g_Video.dt_GetDataTime = DateTime.Now;
                            //SysInfo.m_SysInfo.g_Video.m_iRunning = 2;
                        }
                        if (_sPara[0] == "19.0") //客户端类型
                        {
                            if (_sPara.Length == 2)
                            {
                                switch (int.Parse(_sPara[1]))
                                {
                                    case 1://视频客户端
                                        strRet = "1";
                                        break;
                                    case 2://TOFD波形客户端
                                        strRet = "2";
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            return strRet;
        }
        /// <summary>
        /// 数据解析
        /// </summary>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        private string ExplainClientMsg_Old(string strRetDat)
        {
            string strRet = "";

            #region 依据帧头 帧尾 截取有效数据
            int _iT = strRetDat.IndexOf(C_Head);

            strRetDat = strRetDat.Substring(_iT);
            _iT = strRetDat.IndexOf(C_Tail);
            if (_iT > -1)
                strRetDat = strRetDat.Substring(0, _iT + C_Tail.Length);
            #endregion 

            string[] sPara = strRetDat.Split('[');
            //数据格式：<main/clie> + "[" + 数据 + "[" + </main/clie>  数据格式：数据类型 / 数据内容
            //例如：<clie> + "[" + 1.0/235432 + "[" + </clie>
            if (sPara.Length == 3)
            {
                //2 确认包
                if (sPara[0].ToLower() == C_Head && sPara[2].ToLower() == C_Tail)
                {
                    string[] _sPara = sPara[1].Split('/'); //数据格式：数据类型 / 数据内容
                    if (_sPara.Length == 2)
                    {
                        if (_sPara[0] == "4.0")//数据类型 1.0：数据格式： 视频帧号标志 / 视频帧号
                        {
                            try
                            {
                                //SysInfo.m_SysInfo.g_Climb.m_iVideo_No = int.Parse(_sPara[1]);
                                //SysInfo.g_Msg_InterFace.Fun_RunInfo("3#" + SysInfo.m_SysInfo.g_Climb.m_iVideo_No.ToString());
                                //if (SysInfo.m_SysInfo.g_Video.blNetTrue == false || m_blRecev == false)
                                //    SysInfo.m_SysInfo.g_Video.blNetTrue = true;
                                // SysInfo.blNetTrue_Video = true;
                                m_blRecev = true;
                            }
                            catch (Exception ee
                            )
                            { m_blRecev = false; }
                        }
                    }
                }
            }
            return strRet;
        }
        /// <summary>
        /// 发送数据   拍照数据格式  2.0  / 文件名 | 行号  | 位置 | 检验员 | 1（开始拍照）   录像数据格式  3.0  / 文件名 | 录像开始1/接收0
        /// </summary>
        /// <param name="strKey">1.0: 系统信息 2.0:拍照 3.0:录像 99：退出程序</param>
        /// <param name="strDat">拍照数据格式  2.0  / 文件名 | 行号  | 位置 | 检验员 | 1（开始拍照）   录像数据格式  3.0  / 文件名 | 录像开始1/接收0 99: 关闭程序</param>
        public void SendData(string strDat, string strKey = "1")
        {
            string strSendMsg = strKey + ":" + strDat; //发送数据: 信息类型 : 数据
            string strSend = S_Head + "[" + strSendMsg + "[" + S_Tail;
            //  <main>[1:201[</main>
            TcpClient tcpC = (TcpClient)m_TcpC;
            if (tcpC != null)
            {
                if (tcpC.Connected)
                {
                    NetworkStream ns = tcpC.GetStream();
                    byte[] SendMsg = Encoding.Default.GetBytes(strSend);
                    try
                    {
                        ns.Write(SendMsg, 0, SendMsg.Length);
                        ns.Flush();
                    }
                    catch (Exception e)
                    { }
                }
            }
        }
        public void SendData_M(int iNo, string strDat, string strKey = "1.0")
        {
            if (m_TcpArr != null && iNo < m_TcpArr.Length)
            {
                if (m_TcpArr[iNo] == null) return;

                string strSendMsg = strKey + "/" + strDat; //发送数据: 信息类型 / 数据
                string strSend = S_Head + "[" + strSendMsg + "[" + S_Tail;
                TcpClient tcpC = (TcpClient)m_TcpArr[iNo];
                if (tcpC != null)
                {
                    if (tcpC.Connected)
                    {
                        NetworkStream ns = tcpC.GetStream();
                        byte[] SendMsg = Encoding.Default.GetBytes(strSend);
                        try
                        {
                            ns.Write(SendMsg, 0, SendMsg.Length);
                            ns.Flush();
                        }
                        catch (Exception e)
                        { }
                    }
                }
            }
            else
            {
            }
        }
        /// <summary>
        /// 发送波形数据
        /// </summary>
        /// <param name="iTcpNo">1:固定TOFD的A扫描成像客户端  0：视频客户端</param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public bool SendData_M(int iTcpNo, byte[] Data, int iDataLen)
        {
            bool _blRet = false;
            int _iLen = 1 + iDataLen + 3;//头 1 +  尾部3
            byte[] _SendDat = new byte[_iLen];
            //1 组帧
            _SendDat[0] = 89;//数据

            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_SendDat, 1);

            Marshal.Copy(Data, 0, IntPtArr, iDataLen);
            _SendDat[_iLen - 1] = 0xFE;
            _SendDat[_iLen - 2] = 0xFE;
            _SendDat[_iLen - 3] = 0xFE;

            int _i8 = _SendDat[_iLen - 7];
            int _i7 = _SendDat[_iLen - 6];
            int _i6 = _SendDat[_iLen - 5];
            int _i5 = _SendDat[_iLen - 4];

            //2 压缩
            //  GZip.GZIPCompress(ref _SendDat);
            //3 发送
            SendData(iTcpNo, _SendDat);
            return _blRet;
        }
        /// <summary>
        /// 发送字节数据：波形数据
        /// </summary>
        /// <param name="iTcpNo"></param>
        /// <param name="SendMsg"></param>
        private void SendData(int iTcpNo, byte[] SendMsg)
        {
            if (m_TcpArr[iTcpNo] == null) return;
            TcpClient tcpC = (TcpClient)m_TcpArr[iTcpNo];
            if (tcpC != null)
            {
                int _iLen = SendMsg.Length;
                int _i8 = SendMsg[_iLen - 7];
                int _i7 = SendMsg[_iLen - 6];
                int _i6 = SendMsg[_iLen - 5];
                int _i5 = SendMsg[_iLen - 4];
                int _i4 = SendMsg[_iLen - 1];
                if (tcpC.Connected)
                {
                    NetworkStream ns = tcpC.GetStream();
                    try
                    {
                        ns.Write(SendMsg, 0, SendMsg.Length);
                        ns.Flush();
                    }
                    catch (Exception e)
                    { }
                }
            }
        }

        /// <summary>
        /// 关闭网络
        /// </summary>
        public void Close()
        {
            if (TreadTcp_Server != null) TreadTcp_Server.Abort();
            if (m_TcpServer != null) m_TcpServer.Stop();
            m_blLink = false;

            if (m_TcpC != null)
            {
                if (m_TcpC.Connected)
                {
                    m_TcpC.Close();
                }
            }
            //3 关闭线程
            if (m_trdServer != null) m_trdServer.Abort();
            m_trdServer = null;
        }
    }
    /// <summary>
    /// 打印：当前项目检测记录的内容
    /// </summary>
    //public class CL_Print_RecordCont
    //{
    //    /// <summary>
    //    /// 委托单位
    //    /// </summary>
    //    public string Txt_Wtdw = "";
    //    /// <summary>
    //    /// 工程名称:
    //    /// </summary>
    //    public string Txt_Gcmc = "";
    //    /// <summary>
    //    /// 工件名称:
    //    /// </summary>
    //    public string Txt_Gjmc = "";
    //    /// <summary>
    //    /// 检测方法:
    //    /// </summary>
    //    public string Txt_Jcff = "";
    //    /// <summary>
    //    /// 检测日期:
    //    /// </summary>
    //    public string Txt_Jcrq = "";
    //    /// <summary>
    //    /// 检测机构名称:
    //    /// </summary>
    //    public string Txt_Jcjgmc = "";
    //    /// <summary>
    //    /// 检测机构地址
    //    /// </summary>
    //    public string Txt_Jcjgdz = "";
    //    /// <summary>
    //    /// 邮编:
    //    /// </summary>
    //    public string Txt_Yb = "";


    //    /// <summary>
    //    /// 报告编号:
    //    /// </summary>
    //    public string Txt_Bg_Bgbh = "";
    //    /// <summary>
    //    /// 记录编号:
    //    /// </summary>
    //    public string Txt_Bg_Jlbh = "";
    //    /// <summary>
    //    /// 检测人:
    //    /// </summary>
    //    public string Txt_Jcr = "";
    //    /// <summary>
    //    /// 审核人:
    //    /// </summary>
    //    public string Txt_Shr = "";

    //    //---------工作参数
    //    /// <summary>
    //    /// 规格::
    //    /// </summary>
    //    public string Txt_Gg = "";
    //    /// <summary>
    //    /// 材料:
    //    /// </summary>
    //    public string Txt_CL = "";
    //    /// <summary>
    //    /// 焊接方法:
    //    /// </summary>
    //    public string Txt_Hjff = "";
    //    /// <summary>
    //    /// 坡口型式:
    //    /// </summary>
    //    public string Txt_Pkxs = "";
    //    /// <summary>
    //    /// 热处理状态
    //    /// </summary>
    //    public string Txt_Rclzt = "";
    //    /// <summary>
    //    /// 表面状态
    //    /// </summary>
    //    public string Txt_Bmzt = "";
    //    /// <summary>
    //    /// 检测部位:
    //    /// </summary>
    //    public string Txt_Jcbw = "";
    //    /// <summary>
    //    /// 检测时机:
    //    /// </summary>
    //    public string Txt_Jcsj = "";
    //    /// <summary>
    //    /// 表面温度:
    //    /// </summary>
    //    public string Txt_Bmwd = "";
    //    /// <summary>
    //    /// 承压设备类别:
    //    /// </summary>
    //    public string Txt_Cysblb = "";
    //    /// <summary>
    //    /// 检测比例:
    //    /// </summary>
    //    public string Txt_Jcbl = "";

    //    /// <summary>
    //    /// 检测标准:
    //    /// </summary>
    //    public string Txt_Jcbz = "";
    //    /// <summary>
    //    /// 合格级别:
    //    /// </summary>
    //    public string Txt_Hgjb = "";
    //    /// <summary>
    //    /// 技术等级:
    //    /// </summary>
    //    public string Txt_Jsdj = "";
    //    /// <summary>
    //    /// 操作指导书编号
    //    /// </summary>
    //    public string Txt_CzZdsbh = "";
    //    /// <summary>
    //    /// 仪器名称
    //    /// </summary>
    //    public string Txt_Yqmc = "";
    //    /// <summary>
    //    /// 仪器型号
    //    /// </summary>
    //    public string Txt_Yqxh = "";
    //    /// <summary>
    //    /// 仪器编号
    //    /// </summary>
    //    public string Txt_Yqbh = "";
    //    /// <summary>
    //    /// 扫查装置
    //    /// </summary>
    //    public string Txt_Sczz = "";
    //    /// <summary>
    //    /// 试块
    //    /// </summary>
    //    public string Txt_Sk = "";
    //    /// <summary>
    //    /// 耦合剂
    //    /// </summary>
    //    public string Txt_Ohj = "";
    //    /// <summary>
    //    /// 检测温度
    //    /// </summary>
    //    public string Txt_Wd = "";
    //    /// <summary>
    //    /// 检测面
    //    /// </summary>
    //    public string Txt_Jcm = "";
    //    /// <summary>
    //    /// 检测区域
    //    /// </summary>
    //    public string Txt_Jcqy = "";
    //    /// <summary>
    //    /// 通道
    //    /// </summary>
    //    public string Txt_Tt_Td = "";
    //    /// <summary>
    //    /// 探头型号
    //    /// </summary>
    //    public string Txt_Tt_Xh = "";
    //    /// <summary>
    //    /// 探头编号
    //    /// </summary>
    //    public string Txt_Tt_Bh = "";
    //    /// <summary>
    //    /// 灵敏度设置
    //    /// </summary>
    //    public string Txt_Tt_Lmd = "";
    //    /// <summary>
    //    /// 时间窗口设置:
    //    /// </summary>
    //    public string Txt_Tt_Sjck = "";
    //    /// <summary>
    //    /// 频率
    //    /// </summary>
    //    public string Txt_PL = "";
    //    /// <summary>
    //    /// 镜片尺寸
    //    /// </summary>
    //    public string Txt_Jpcc = "";
    //    /// <summary>
    //    /// 楔块角度
    //    /// </summary>
    //    public string Txt_Xkjd = "";//SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsAngle
    //    /// <summary>
    //    /// PCS
    //    /// </summary>
    //    public string Txt_PCS = "";//SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsLen
    //    /// <summary>
    //    /// 扫查步进
    //    /// </summary>
    //    public string Txt_Scbj = "";//SysInfo.m_SysBuff.m_Tofd_DLL.m_i_1_mm 
    //    /// <summary>
    //    /// 扫查方式
    //    /// </summary>
    //    public string Txt_Scfs = "";
    //}

    public class StartKeyBoard
    {
        public static bool isShowNumBoard = false;
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(ref IntPtr ptr);
        public static void StartKeyBoardFun()
        {
            Process[] pro = Process.GetProcessesByName("osk");
            if (pro != null && pro.Length > 0)
                return;
            IntPtr ptr = new IntPtr();
            bool iswow64FsRedirectionDisabled = Wow64DisableWow64FsRedirection(ref ptr);
            if (iswow64FsRedirectionDisabled)
            {
                Process.Start(@"C:\WINDOWS\SYSTEM32\OSK.EXE");
                //  bool iswow64FsRedirectionReverted = Wow64RevertWow64FsRedirection(ptr);

            }
        }
    }
}
