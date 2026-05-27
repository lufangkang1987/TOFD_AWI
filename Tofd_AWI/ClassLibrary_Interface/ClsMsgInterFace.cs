using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_Interface
{
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
        public void Fun_UI_WinClose( )
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
        public delegate void OnAdd_Parts(int iType=1);
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
        #endregion 7
    }
    #endregion 消息类
}
