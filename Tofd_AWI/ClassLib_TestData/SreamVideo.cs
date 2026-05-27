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
        public float Trip_Com_mm = 0;
        /// <summary>
        /// 车体运行速度
        /// </summary>
        public double Speed = 0;
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
        public int i_Para_Wz = 0;
        /// <summary>
        /// 接收到报文
        /// </summary>
        public string strCmdRev = "";

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
                Inter_BrushBiaoZhu_Posit( i_X,  i_Y);
            }
            catch { }
        }
        #endregion 13
    }
    #endregion 消息类
}
