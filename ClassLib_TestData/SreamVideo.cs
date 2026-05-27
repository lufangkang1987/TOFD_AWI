using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib_TestData
{
    /// <summary>
    /// 视频记录文件
    /// </summary>
    public class clStreamVideo
    {
        /// <summary>
        /// 发送命令时间
        /// </summary>
        public DateTime dtSendStar  = new DateTime();
        /// <summary>
        /// 显示通讯参数的窗体是否打开
        /// </summary>
        public bool blWinOpen = false;
        /// <summary>
        /// 数据索引
        /// </summary>
        public int iDatIndex = 0;
        /// <summary>
        /// 视频帧序号  暂时只有磁粉进行录像，监控不录像
        /// </summary>
        public int iFrameNo = 0;
        /// <summary>
        /// 车体行程 m
        /// </summary>
        public double Trip = 0;
        /// <summary>
        /// 0：TOFD自带编码器 1:4轮车体编码器
        /// </summary>
        public int iBmq_Type = 0;
        /// <summary>
        /// 车体行程 mm
        /// </summary>
        public float  Trip_mm = 0;
        /// <summary>
        /// 车体固有编码器 mm
        /// </summary>
        public int Trip_Com_mm = 0;
        /// <summary>
        /// 校准系数
        /// </summary>
        public float fl4Car_JzXs = 1;
        /// <summary>
        /// 放电
        /// </summary>
        public bool  bl_Discharge = false ;
        /// <summary>
        /// 放电
        /// </summary>
        public bool bl_Discharge_Txt = false;
        /// <summary>
        /// 放电位置
        /// </summary>
        public int i_Discharge_Txt_X = 0;

        /// <summary>
        /// 放电位置
        /// </summary>
        public int i_Discharge_Txt_Y = 0;
        /// <summary>
        /// 放电时间
        /// </summary>
        public string  str_Discharge_Txt_Time = "";

        /// <summary>
        /// 是否显示啦
        /// </summary>
        public bool bl_Discharge_Show = false;
        /// <summary>
        /// 放电去打标
        /// </summary>
        public bool bl_Discharge_To_Mark = false;
       
        /// <summary>
        /// 打标
        /// </summary>
        public bool bl_Discharge_To_Mark_Txt = false;
        /// <summary>
        /// 打标位置
        /// </summary>
        public int i_Discharge_To_Mark_Txt_X = 0;
        /// <summary>
        /// 打标位置
        /// </summary>
        public int i_Discharge_To_Mark_Txt_Y = 0;
        /// <summary>
        /// 打标
        /// </summary>
        public string  str_Discharge_To_Mark_Txt_Time = "";

        /// <summary>
        /// 是否显示啦
        /// </summary>
        public bool bl_Discharge_Show_To_Mark = false;

        /// <summary>
        /// 统计距离信息
        /// </summary>
        public string strTrip_mm = "";
        /// <summary>
        /// 初始位置
        /// </summary>
        public int Trip_Com_Init_mm = 0;
        /// <summary>
        /// 测量最远距离后停车
        /// </summary>
        public int Trip_Max_Distanc = 0;
        /// <summary>
        /// 车体运行速度
        /// </summary>
        public double Speed = 0;
        /// <summary>
        /// 车体设定速度
        /// </summary>
        public int i_Speed = 0;
        /// <summary>
        /// 光栅臂设定速度
        /// </summary>
        public int i_Gsb_Speed = 0;
        /// <summary>
        /// 光栅臂扫查 ：1    直行：0
        /// </summary>
        public int iRun_Gsb_Sc1_Zx0 = 1;
        /// <summary>
        /// 光栅臂前进后退 1：前进 0：后退
        /// </summary>
        public int iRun_Gsb_Qj1_Ht0 = 1;
        /// <summary>
        /// 光栅臂起点位置
        /// </summary>
        public int i_Gsb_Start_Pos = 0;

        /// <summary>
        /// 光栅臂总显示缓存数量
        /// </summary>
        public int i_Gsb_Show_All_Num = 0;
        /// <summary>
        /// 光栅臂显示缓存左边界
        /// </summary>
        public int i_Gsb_Show_LeftBoundary = 0;
        /// <summary>
        /// 光栅臂显示缓存有边界
        /// </summary>
        public int i_Gsb_Show_RightBoundary = 0;

        /// <summary>
        /// 增加
        /// </summary>
        public int i_Distan_Add = 3;
        /// <summary>
        /// 光栅臂终点位置
        /// </summary>
        public int i_Gsb_End_Pos = 300;
        /// <summary>
        /// 光栅臂总长度
        /// </summary>
        public int iGsb_Len = 300;
        /// <summary>
        /// 延时距离
        /// </summary>
        public int m_UI_Gsb_Distan_Min = 0;

        /// <summary>
        /// 测量数据大于此百份值，不再计算
        /// </summary>
        public float fl_Max_Limit = 30;

        /// <summary>
        /// 是否使用不显示大误差
        /// </summary>
        public int i_Alarm = 0;
        /// <summary>
        /// 每次光栅臂走完一行，然后车体开始移动（前进/后退）位置，这个位置就是步进距离mm
        /// </summary>
        public int i_Gsb_Interval = 5;
        /// <summary>
        /// 涂层沿着光栅臂的方向，间隔扫查，间隔的距离
        /// </summary>
        public int i_Coat_Interval = 5;
        /// <summary>
        /// 后续检测在当前位置的  左边：0 右边：1
        /// </summary>
        public int i_R1_L0 = 1;
        /// <summary>
        /// 公称厚度
        /// </summary>
        public float flstrThickAlarm = 10;
        /// <summary>
        /// 告警门限
        /// </summary>
        public string strWc_Bfz = "20";
        /// <summary>
        /// 声速值
        /// </summary>
        public string strSpeed = "3200";
        /// <summary>
        /// 运动方向，单向0，双向1
        /// </summary>
        public int i_Dx0_Sx1 = 0;

        /// <summary>
        /// 开始检测时，探头架子是否落下
        /// </summary>
        public int i_Begin_Down = 1;
        /// <summary>
        /// 光栅臂0：抬起1：落下
        /// </summary>
        public int iGsbTtLx = 0;
        /// <summary>
        /// 缺陷打标MK 0：抬起1：落下 
        /// </summary>
        public int iAlarmMark = 0;

        // 第三字节 01：自动模式启动 
        /// <summary>
        /// 自动模式01：自动模式启动 02：自动模式关闭
        /// </summary>
        public int i_Auto_Run = 2;
        /// <summary>
        /// 01：磨头旋转  02：旋转停止
        /// </summary>
        public int i_Xz = 2;
        /// <summary>
        /// 01：升降压下 02：升降抬起
        /// </summary>
        public int i_Sj_Up = 2;
        /// <summary>
        ///  01：前进
        ///      02：后退
        ///	  03：停止
        ///	  04：前进左转
        ///	  05：前进右转
        ///      06：后退左转
        ///     07：后退右转
        /// </summary>
        public int i_Run = 3;
        /// <summary>
        /// 光栅臂 01: 横扫启动 02：横扫停止 03：左扫
        /// 04：右扫 05：满行程左扫 06：满行程右扫
        /// </summary>
        /*
         01: 横扫启动
	          02：横扫停止
			  03：左扫
			  04：右扫
              05：满行程左扫（用于设定横扫区间值）
              06：满行程右扫（用于设定横扫区间值）
    */
        public int i_Gsb_Hs = 2;
        /// <summary>
        /// 01：左纠偏   02：右纠偏   03：停止纠偏
        /// </summary>
        public int i_Jp = 3;

        /// <summary>
        /// 左限位
        /// </summary>
        public int i_Para_Zxw = 0;
        /// <summary>
        /// 右限位
        /// </summary>
        public int i_Para_Yxw = 0;
        /// <summary>
        /// 光栅臂速度
        /// </summary>
        public int i_Para_Speed_Gsb = 0;
        /// <summary>
        /// 单步间隔
        /// </summary>
        public int i_Para_Dbjg = 0;
        /// <summary>
        /// 纠偏参数
        /// </summary>
        public int i_Para_Jp = 0;
        /// <summary>
        /// 滑台实时位置
        /// </summary>
        public int i_Para_Wz = -1;
        /// <summary>
        /// 接收到报文
        /// </summary>
        public string strCmdRev = "";
        /// <summary>
        /// 光栅臂从左往右 true:从左往右  false: 从右往左
        /// </summary>
        public bool blGsb_RunFx_L_to_R = true;

        /// <summary>
        /// 测试采集光栅臂
        /// </summary>
        public bool m_bl_Cs_Y = true ;
        /// <summary>
        /// 测量类型
        /// </summary>
        public int m_i_TOFD_0_Cscan_1_Mui_2 = 0;
        /// <summary>
        /// 是否有超声仪器
        /// </summary>
        public bool m_bl_Have = false;

        public bool blGsb_L_to_R = true;
        /// <summary>
        /// 历史信息：光栅臂从左往右 true:从左往右  false: 从右往左
        /// </summary>
        public bool blGsb_RunFx_L_to_R_Old = true;

        #region 陀螺仪数据 
        /*
1、陀螺仪数据 gyroscopic
字节	1	2	3	4	5	6	7	8
 指令示例：	0x47	0x59	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
			偏航角（z轴）*100	俯仰角（y轴）*100	滚转角（x轴）*100
前两字节是指令的针头，用于识别指令的类型；	 
	  第三四字节：偏航角（z轴）*100的高位至低位
	  第三四字节：俯仰角（y轴）*100的高位至低位
第三四字节：滚转角（x轴）*100的高位至低位
         */
        /// <summary>
        /// 偏航角（z轴）*100的高位至低位
        /// </summary>
        public string Z = "";
        /// <summary>
        /// 俯仰角（y轴）*100的高位至低位
        /// </summary>
        public string Y = "";
        /// <summary>
        /// 滚转角（x轴）*100的高位至低位
        /// </summary>
        public string X = "";

        /// <summary>
        /// 校正角度
        /// </summary>
        public float flAnle = 0;
        /// <summary>
        /// 17：已下压到位状态   18：已抬起到位状态
        /// </summary>
        public int iUpDownState = -1;
        #endregion
    }
    #region 消息类
    /// <summary>
    /// 消息
    /// </summary>
    public class TestDataMsg : EventArgs
    {
        public TestDataMsg()
        {
        }
        private string _strKey = "";
        /// <summary>
        ///1 命令类型
        /// </summary>
        public string strKey
        {
            get { return _strKey; }
            set { _strKey = value; }

        }

        private string _strMsg = "";
        /// <summary>
        ///2  数据信息
        /// </summary>
        public string strMsg
        {
            get { return _strMsg; }
            set { _strMsg = value; }
        }
    }

    /// <summary>
    /// 界面信息调用接口
    /// </summary>
    public class MsgInterFace
    {
        private TestDataMsg _MsgDat = null;
        // 实例化类对象
        private static MsgInterFace interTestface = null;

        private MsgInterFace()
        { }
        /// <summary>
        /// 单例实例化
        /// </summary>
        /// <returns>消息器实例</returns>
        public static MsgInterFace GetInstance()
        {
            if (interTestface == null)
                interTestface = new MsgInterFace();
            return interTestface;
        }
        #region 1 显示监控视频
        /// <summary>
        /// 1 代理函数
        /// </summary>
        /// <param name="strRetCmd"></param>
        public delegate void OnShow_Monit(Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> WBitmap);
        /// <summary>
        /// 1 显示悬浮窗体视频
        /// </summary>
        public event OnShow_Monit Inter_Show_Monit;
        /// <summary>
        /// 1 显示悬浮窗体视频
        /// </summary>
        /// <param name="WBitmap">显示图片数据</param>
        public void Fun_Show_Monit(Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> WBitmap)
        {
            if (Inter_Show_Monit != null)
                Inter_Show_Monit(WBitmap);
        }
        #endregion 1 
        #region 2 显示车体上传PC的报文
        /// <summary>
        ///  代理函数
        /// </summary>
        public delegate void OnClimb_UI_ClimbUpData(string strMsg);
        /// <summary>
        ///  车体上传信息请求应用层显示
        /// </summary>
        public event OnClimb_UI_ClimbUpData Inter_UI_ClimbUpData;

        /// <summary>
        /// 外部调用函数:车体上传信息请求应用层显示请求发送
        /// </summary>
        /// <param name="strMsg">接收到的报文</param>
        public void Fun_UI_ClimbUpData(string strMsg)
        {
            try
            {
                if(Inter_UI_ClimbUpData!=null )
                Inter_UI_ClimbUpData(strMsg);
            }
            catch { }
        }
        #endregion 2


        #region 3 关机
        /// <summary>
        ///  代理函数
        /// </summary>
        public delegate void OnDataWin_UI_Close();
        /// <summary>
        ///  关机
        /// </summary>
        public event OnDataWin_UI_Close Inter_UI_WinClose;

        /// <summary>
        /// 外部调用函数:接收关机命令
        /// </summary>
        public void Fun_UI_WinClose()
        {
            try
            {
                if(Inter_UI_WinClose!=null )
                Inter_UI_WinClose();
            }
            catch { }
        }
        #endregion 3

        #region 4
        /// <summary>
        ///  界面控制键盘显示/隐藏
        /// </summary>
        public delegate void OnBtshow();
        /// <summary>
        ///  浮动按钮显示
        /// </summary>
        public event OnBtshow Inter_Bt_Show;

        /// <summary>
        /// 外部调用函数:接收关机命令
        /// </summary>
        public void Fun_Bt_Show()
        {
            try
            {
                if(Inter_Bt_Show!=null )
                Inter_Bt_Show();
            }
            catch { }
        }
        #endregion 4

        #region 5
        /// <summary>
        ///  增加检测部件
        /// </summary>
        public delegate void OnAdd_Parts(int iType = 1);
        /// <summary>
        ///  浮动按钮显示
        /// </summary>
        public event OnAdd_Parts Inter_Add_Parts;

        /// <summary>
        /// 外部调用函数:接收关机命令
        /// </summary>
        public void Fun_Add_Parts(int iType = 1)
        {
            try
            {
                if (Inter_Add_Parts != null)
                    Inter_Add_Parts(iType);
            }
            catch { }
        }
        #endregion 5

        #region 6
        /// <summary>
        ///  删除照片
        /// </summary>
        public delegate void OnBrush_PhoneLstBox();
        /// <summary>
        ///  删除照片
        /// </summary>
        public event OnBrush_PhoneLstBox Inter_Brush_PhoneLstBox;

        /// <summary>
        /// 外部调用函数:接收关机命令
        /// </summary>
        public void Fun_Brush_PhoneLstBox()
        {
            try
            {
                Inter_Brush_PhoneLstBox();
            }
            catch { }
        }
        #endregion 5

        #region 6 显示服务器中控制参数
        /// <summary>
        /// 1 代理函数:接收服务器控制参数
        /// </summary>
        /// <param name="strRetCmd"></param>
        public delegate void OnGetServe_Data_2();
        /// <summary>
        ///
        /// </summary>
        public event OnGetServe_Data_2 Inter_GetData_2;
        /// <summary>
        /// 1 接收服务器控制参数
        /// </summary>
        public void Fun_GetServe_Data_2()
        {
            if (Inter_GetData_2 != null)
                Inter_GetData_2();
        }
        #endregion 6 

        //---
        #region 7 拍照
        /// <summary>
        ///  代理函数
        /// </summary>
        public delegate void OnData_Photo();
        /// <summary>
        ///  拍照
        /// </summary>
        public event OnData_Photo Inter_Photo;

        /// <summary>
        /// 外部调用函数:接收拍照命令
        /// </summary>
        public void Fun_Photo()
        {
            try
            {
                if(Inter_Photo!=null )
                Inter_Photo();
            }
            catch { }
        }
        #endregion 7
        #region 8 录像
        /// <summary>
        ///  代理函数 1开始录像 2停止录像
        /// </summary>
        public delegate void OnData_Video(int iData);
        /// <summary>
        ///  录像 1开始录像 2停止录像
        /// </summary>
        public event OnData_Video Inter_Video;

        /// <summary>
        /// 外部调用函数:接收录像命令  1开始录像 2停止录像
        /// </summary>
        public void Fun_Video(int iData)
        {
            try
            {
                if (Inter_Video != null)
                    Inter_Video(iData);
            }
            catch { }
        }
        #endregion 7
        //--


        #region 9
        /// <summary>
        ///  测高寻迹
        /// </summary>
        public delegate void OnGetCgXjServe_Data();
        /// <summary>
        ///  测高寻迹
        /// </summary>
        public event OnGetCgXjServe_Data Inter_GetServe_CgXj;

        /// <summary>
        /// 测高寻迹
        /// </summary>
        public void Fun_GetServe_Data1()
        {
            try
            {
                if(Inter_GetServe_CgXj!=null )
                Inter_GetServe_CgXj();
            }
            catch { }
        }
        #endregion 9

        #region 10 刷新当前焊缝数据
        /// <summary>
        ///  0:提醒项目名称 1：刷新焊缝数据
        /// </summary>
        public delegate void OnBrushCurrWeld(int iData);
        /// <summary>
        ///  0:提醒项目名称 1：刷新焊缝数据
        /// </summary>
        public event OnBrushCurrWeld Inter_BrushCurrWeld;

        /// <summary>
        /// 0:提醒项目名称 1：刷新焊缝数据
        /// </summary>
        public void Fun_BrushCurrWeld(int iData)
        {
            try
            {
                if (Inter_BrushCurrWeld != null)
                    Inter_BrushCurrWeld(iData);
            }
            catch { }
        }
        #endregion 10

        #region 11 D图初始化
        /// <summary>
        /// D图初始化
        /// </summary>
        public delegate void OnBrush_D(int iType);
        /// <summary>
        /// D图初始化
        /// </summary>
        public event OnBrush_D Inter_Brush_D;

        /// <summary>
        /// D图初始化
        /// </summary>
        public void Fun_Brush_D(int iType)
        {
            try
            {
                if (Inter_Brush_D != null)
                    Inter_Brush_D(iType);
            }
            catch { }
        }
        #endregion 11

        #region 12  D图标注
        /// <summary>
        /// D图标注
        /// </summary>
        public delegate void OnBrush_BiaoZhu();
        /// <summary>
        /// D图标注
        /// </summary>
        public event OnBrush_BiaoZhu Inter_Brush_BiaoZhu;

        /// <summary>
        /// D图标注
        /// </summary>
        public void Fun_Brush_BiaoZhu( )
        {
            try
            {
             if(Inter_Brush_BiaoZhu!=null )     Inter_Brush_BiaoZhu();
            }
            catch { }
        }
        #endregion 12

        #region 13 伤点图标定位
        /// <summary>
        /// 伤点图标定位
        /// </summary>
        public delegate void OnBrushBiaoZhu_Posit(int i_X,int i_Y);
        /// <summary>
        /// 伤点图标定位
        /// </summary>
        public event OnBrushBiaoZhu_Posit Inter_BrushBiaoZhu_Posit;

        /// <summary>
        /// 伤点图标定位
        /// </summary>
        public void Fun_BrushBiaoZhu_Posit(int i_X, int i_Y)
        {
            try
            {
                if(Inter_BrushBiaoZhu_Posit != null)
                Inter_BrushBiaoZhu_Posit( i_X,  i_Y);
            }
            catch { }
        }
        #endregion 13

        #region 14 重新连接视频
        /// <summary>
        /// 重新连接视频
        /// </summary>
        public delegate void OnReLink_Video();
        /// <summary>
        /// 重新连接视频
        /// </summary>
        public event OnReLink_Video Inter_ReLink_Video;

        /// <summary>
        /// 重新连接视频
        /// </summary>
        public void Fun_ReLink_Videl()
        {
            try
            {
                if (Inter_ReLink_Video != null)
                    Inter_ReLink_Video();
            }
            catch { }
        }
        #endregion 14

        #region 15 重新连接寻迹
        /// <summary>
        /// 重新连接寻迹
        /// </summary>
        public delegate void OnReLink_XunJi();
        /// <summary>
        /// 重新连接寻迹
        /// </summary>
        public event OnReLink_XunJi Inter_ReLink_XunJi;

        /// <summary>
        /// 重新连接寻迹
        /// </summary>
        public void Fun_ReLink_XunJi()
        {
            try
            {
                if (Inter_ReLink_XunJi != null)
                    Inter_ReLink_XunJi();
            }
            catch { }
        }
        #endregion 14

        #region 16  相机切换 0：前置 1：后置 2：寻迹  3: 打标视频
        /// <summary>
        /// 相机切换
        /// </summary>
        public delegate void OnChangeVideo(int iType);
        /// <summary>
        /// 相机切换
        /// </summary>
        public event OnChangeVideo Inter_ChangeVideo;

        /// <summary>
        /// 相机切换 0：前置 1：后置 2：寻迹  3: 打标视频
        /// </summary>
        public void Fun_ChangeVideo(int iType)
        {
            try
            {
                if (Inter_ChangeVideo != null)
                    Inter_ChangeVideo(iType);
            }
            catch { }
        }
        #endregion 11



        #region 17 标准色修改了
        /// <summary>
        /// 2 代理函数
        /// </summary>
        public delegate void OnMody_Color();
        /// <summary>
        /// 标准色修改了
        /// </summary>
        public event OnMody_Color Inter_ModyColor;

        /// <summary>
        ///  标准色修改了
        /// </summary>
        public void Fun_ModyColor()
        {
            if (Inter_ModyColor != null)
                Inter_ModyColor();
        }
        #endregion 17

        #region 18 保存修改数据
        /// <summary>
        /// 2 代理函数
        /// </summary>
        public delegate void OnSave_Mark();
        /// <summary>
        /// 标准色修改了
        /// </summary>
        public event OnSave_Mark Inter_SaveData;

        /// <summary>
        ///  保存修改数据
        /// </summary>
        public void Fun_SaveData()
        {
            if (Inter_SaveData != null)
                Inter_SaveData();
        }
        #endregion 18



        #region 19
        /// <summary>
        /// C扫描运行
        /// </summary>
        public delegate void OnRun_C(int iType);
        /// <summary>
        /// C扫描运行
        /// </summary>
        public event OnRun_C Inter_Run_C;

        /// <summary>
        /// C扫描运行
        /// </summary>
        public void Fun_Run_C(int iType)
        {
            try
            {
                if(Inter_Run_C!=null )
                Inter_Run_C(iType);
            }
            catch { }
        }
        #endregion 19


        #region 20
        /// <summary>
        /// TOFD参数变化
        /// </summary>
        public delegate void OnTofd_Para(int iType);
        /// <summary>
        /// TOFD参数变化
        /// </summary>
        public event OnTofd_Para Inter_Tofd_Para;

        /// <summary>
        /// TOFD参数变化
        /// </summary>
        public void Fun_Tofd_Para(int iType)
        {
            try
            {
                if(Inter_Tofd_Para!=null )
                Inter_Tofd_Para(iType);
            }
            catch (Exception e)
            { }
        }
        #endregion 20

        #region 21 涡流发送数据后，接收到数据返回
        /// <summary>
        ///  涡流发送数据后，接收到数据返回
        /// </summary>
        public delegate void OnETC_GetEtcReceive(int iType, int iData = 0);
        /// <summary>
        ///  涡流发送数据后，接收到数据返回
        /// </summary>
        public event OnETC_GetEtcReceive Inter_GetEtcReceive;
        /// <summary>
        /// 涡流发送数据后，接收到数据返回
        /// </summary>
        public void Fun_GetEtcReceive(int iType, int iData = 0)
        {
            if (Inter_GetEtcReceive != null)
                Inter_GetEtcReceive(iType, iData);
        }
        #endregion 21 

        #region 22
        /// <summary>
        /// 点测显示C扫描数据
        /// </summary>
        public delegate void OnShow_Coat_C(bool blHaveData);
        /// <summary>
        /// 点测显示C扫描数据
        /// </summary>
        public event OnShow_Coat_C Inter_ShowCoat_C;

        /// <summary>
        /// 点测显示C扫描数据
        /// </summary>
        public void Fun_ShowCoat_C(bool blHaveData)
        {
            try
            {
                if (Inter_ShowCoat_C != null)
                    Inter_ShowCoat_C(blHaveData);
            }
            catch { }
        }
        #endregion 22

        #region 22
        /// <summary>
        /// 点测显示C扫描数据
        /// </summary>
        public delegate void OnShow_C();
        /// <summary>
        /// 点测显示C扫描数据
        /// </summary>
        public event OnShow_C Inter_Show_C;

        /// <summary>
        /// 点测显示C扫描数据
        /// </summary>
        public void Fun_Show_C()
        {
            try
            {
                if (Inter_Show_C != null)
                    Inter_Show_C();
            }
            catch { }
        }
        #endregion 22
    }


    #endregion 消息类
}
