/*
 * Copyright(C) 2 2019 郑州金润高科电子有限公司
 * 文件名: Frm_Main
 * 文件功能描述: 视频显示
 * 目的：视频显示
 * 创建标识: 陈大伟 2019-1-7
 * 修改标识: 
 * 修改描述:
 * 2020-4-22:  视频采用C++DLL,必须是64位
 * 版本：V1.0
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Net;
using System.Threading;
//using HL;//恒力SDK
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using  Emgu.CV;

using System.Drawing.Drawing2D;//图像缩放
using System.Reflection;

using Clb_XmCam_DLL;//使用独立通讯的雄迈相机

using MVSDK;//使用SDK接口
using CameraHandle = System.Int32;
using MvApi = MVSDK.MvApi;
using System.Drawing.Imaging;

namespace ClimbVideo
{
    public partial class Frm_Video : Form
    {
        string m_Net_V = Application.StartupPath + "\\database\\HardConfig.ini";
        #region variable
        protected IntPtr m_Grabber = IntPtr.Zero;
        protected CameraHandle m_hCamera = 0;
        protected tSdkCameraDevInfo m_DevInfo;
        protected ColorPalette m_GrayPal;
        protected pfnCameraGrabberFrameCallback m_FrameCallback;
        #endregion

        /// <summary>
        /// 看门狗
        /// </summary>
        ClDog m_Dog = new ClDog();
      
 
        /// <summary>
        /// 0:2轮 1：4轮 2:过山车
        /// </summary>
        int m_iClimbType = 0;
        /// <summary>
        /// 录像保存
        /// </summary>
        bool m_blSave = false;
        /// <summary>
        /// 图像列表
        /// </summary>
        List<  Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> > m_LstImgDat=new List<Image<Bgr, byte>> ();
        #region 变量联结
        int[] m_iArrOut_1 = new int[1];
        int[] m_iArrOut_2 = new int[1];
        /// <summary>
        /// 开机网络信息
        /// </summary>
        string m_strServerMsg = "";
        bool m_blGetImge_1 = false;
        bool m_blGetImge_2 = false;
        bool m_blAddD3D = false;
        int m_iAdd = 2;
        int m_i_Curr_X = 0;
        int m_i_Curr_Y = 0;
        /// <summary>
        /// 是否重新
        /// </summary>
        bool m_blReLink = false;
        /// <summary>
        /// 发送数据间隔
        /// </summary>
        int m_iFrameNum = 0;
        /// <summary>
        /// 是否重新
        /// </summary>
        bool m_blReLink_1 = false;
        /// <summary>
        /// 是否重新
        /// </summary>
        bool m_blReLink_2 = false;
        /// <summary>
        /// 准备联结
        /// </summary>
        bool m_blRepLink_1 = false;
        int m_iErr = 0;
        int m_iErr_2 = 0;
        /// <summary>
        /// 刷新视频
        /// </summary>
        /// <param name="Value"></param>
        delegate void Delg_ShowVideo(int iNo, byte[] Value, PictureBox PicBox);// ,int iTitl_Type);
        /// <summary>
        /// 刷新报警
        /// </summary>
        delegate void Delg_RunAlarm();
        private static object Lock_Video = new object();
        /// <summary>
        /// 相机显示提示符号
        /// </summary>
        string m_strMsg = "";
        int[] m_ArrOut = new int[3];
        SolidBrush m_drawBrush = new SolidBrush(Color.Blue);

        Font m_font = new Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Millimeter);
        /// <summary>
        /// 显示频率
        /// </summary>
        int m_iShowPh = 0;
        DateTime m_dtStar = DateTime.Now;
        Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> m_ImgDat_Other;
        Task Task_SaveVideo = null;
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="Value"></param>
        delegate void Delg_ShowTiTl(int iType, string Value);
        /// <summary>
        /// 1号重新联结
        /// </summary>
        delegate void Delg_ReLink(int iType);
        /// <summary>
        /// 2号重新联结
        /// </summary>
        delegate void Delg_ReLink_2(int iType);
        /// <summary>
        /// 相机缓存大小
        /// </summary>
        int m_iCam_Len = 0;
        /// <summary>
        /// 显示录像闪烁
        /// </summary>
        public int m_iShowVideo = 0;
        /// <summary>
        /// 视频显示
        /// </summary>
        public Thread Thread_ShowVideo = null;
        /// <summary>
        /// 视频运行
        /// </summary>
        public Thread Thread_RunVideo_1 = null;

        /// <summary>
        /// 视频运行
        /// </summary>
        public Thread Thread_RunVideo_2 = null;
        /// <summary>
        /// 视频运行
        /// </summary>
        public Thread Thr_RunVideo_1 = null;
        /// <summary>
        /// 录像
        /// </summary>
        public Thread Thr_LuXiang = null;

        /// <summary>
        /// 循环获得相机1图像
        /// </summary>
        public Thread Thread_Tcp_Send = null;
        /// <summary>
        /// 循环获得相机1图像
        /// </summary>
        public Thread Thread_WhileGetVideo_1 = null;
        /// <summary>
        /// 循环获得相机2图像
        /// </summary>
        public Thread Thread_WhileGetVideo_2 = null;
        /// <summary>
        /// 视频运行
        /// </summary>
        public Thread Thr_RunVideo_2 = null;
        /// <summary>
        /// 视频运行
        /// </summary>
        public Thread Thread_RunVideo_CS_1 = null;

        /// <summary>
        /// 视频运行
        /// </summary>
        public Thread Thread_RunVideo_CS_2 = null;

        public Thread Thread_RunVideo_CS_3 = null;
        #region 调用视频D3D控件
        /// <summary>
        /// 使用Direct3D刷新视频 默认true
        /// </summary>
        bool m_bl_D3D = false ;
        /// <summary>
        /// 设备初始化
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lWidth"></param>
        /// <param name="lHeight"></param>
        /// <param name="blBgra1_Yuv420"></param>
        /// <returns></returns>//Dll_D3D_Video
        [DllImport("DLL_D3D.dll", EntryPoint = "InitD3D", CallingConvention = CallingConvention.Cdecl)]
        static extern int InitD3D(IntPtr hwnd, bool blBgra1_Yuv420, long lWidth, long lHeight);//设备初始化

        /// <summary>
        /// 输入视频数据显示
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        [DllImport("DLL_D3D.dll", EntryPoint = "Render", CallingConvention = CallingConvention.Cdecl)]
        static extern bool Render(byte[] buffer);//渲染

        /// <summary>
        /// 退出程序资源释放
        /// </summary>
        [DllImport("DLL_D3D.dll", EntryPoint = "Cleanup", CallingConvention = CallingConvention.Cdecl)]
        static extern void Cleanup();//资源释放

        /// <summary>
        /// 中途改变界面尺寸时
        /// </summary>
        /// <param name="hwnd"></param>
        [DllImport("DLL_D3D.dll", EntryPoint = "GetClient_Rec", CallingConvention = CallingConvention.Cdecl)]
        static extern void GetClient_Rec(IntPtr hwnd);//刷新客户区域大小

        #endregion  调用D3D

        #region 调用DLL

        /// <summary>
        /// 相机视频
        /// </summary>
        public Thread Thread_Run_Video = null;
        /// <summary>
        /// 左相机缓存
        /// </summary>
        byte[] m_Img_L;//
        /// <summary>
        /// 右相机缓存
        /// </summary>
        byte[] m_Img_R;//
        /// <summary>
        /// 抓取视频数据
        /// </summary>
        int m_iVideo = 0;
        /// <summary>
        /// 左图像
        /// </summary>
        //   Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> originalImg_L; 


        /// <summary>
        /// 连接是否成功
        /// </summary>
        private int m_iLink = 0;

        //1 设置运行路径
        [DllImport("V_DLL.dll", EntryPoint = "SetPath", CallingConvention = CallingConvention.Cdecl)]
        static extern void SetPath(string strPath);
        //2 相机初始化
        [DllImport("V_DLL.dll", EntryPoint = "pair_image_capture", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_capture(int camera_w, int camare_h, string Str_In_Key_Value, string Ip, string Ip2, string iPort, int[] _iOutArr);

        [DllImport("V_DLL.dll", EntryPoint = "pair_image_1", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_1(int camera_w, int camare_h, string Str_In_Key_Value, string Ip1, string iPort, int[] _iOutArr);

        [DllImport("V_DLL.dll", EntryPoint = "pair_image_2", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_2(int camera_w, int camare_h, string Str_In_Key_Value, string Ip2, string iPort, int[] _iOutArr);





        [DllImport("V_DLL.dll", EntryPoint = "pair_image_capture_1", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_capture_1(int camera_w, int camare_h, string Str_In_Key_Value, string Ip, string Ip2, string iPort, int[] _iOutArr,
                                         byte[] ImageBuffer_1, byte[] ImageBuffer_2);
        /// <summary>
        /// 打开1号相机
        /// </summary>
        /// <param name="camera_w"></param>
        /// <param name="camare_h"></param>
        /// <param name="Ip"></param>
        /// <param name="iPort"></param>
        /// <param name="_iOutArr"></param>
        /// <param name="ImageBuffer_1"></param>
        /// <returns></returns>
        [DllImport("V_DLL.dll", EntryPoint = "pair_image_capture_And_Video_3", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_capture_And_Video_3(int camera_w, int camare_h, string Str_In_Key_Value,
                               string Ip, string Ip2, string iPort, int[] _iOutArr, byte[] ImageBuffer_1, byte[] ImageBuffer_2);


        /// <summary>
        /// 打开2号相机
        /// </summary>
        /// <param name="Ip2"></param>
        /// <param name="iPort"></param>
        /// <param name="_iOutArr"></param>
        /// <param name="ImageBuffer_2"></param>
        /// <returns></returns>
        [DllImport("V_DLL.dll", EntryPoint = "pair_image_capture_And_Video_2", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_capture_And_Video_2(int camera_w, int camare_h, byte[] ImageBuffer_2);


        [DllImport("V_DLL.dll", EntryPoint = "Show_FFMpeg", CallingConvention = CallingConvention.Cdecl)]
        static extern int Show_FFMpeg( string Ip_strPort);// "tcp://192.168.1.2:34567"  "tcp://172.25.11.129:2222"



        //3 视频
        [DllImport("V_DLL.dll", EntryPoint = "Video", CallingConvention = CallingConvention.Cdecl)]//SetLastError = false)
        static extern int Video(byte[] ImageBuffer_1, byte[] ImageBuffer_2, int iNo); //static extern int Video(byte[] ImageBuffer_1, int iNo);//, byte[] ImageBuffer_2h);

        [DllImport("V_DLL.dll", EntryPoint = "Video_1", CallingConvention = CallingConvention.Cdecl)]//SetLastError = false)
        static extern int Video_1(byte[] ImageBuffer); //static extern int Video(byte[] ImageBuffer_1, int iNo);//, byte[] ImageBuffer_2h);
        [DllImport("V_DLL.dll", EntryPoint = "Video_2", CallingConvention = CallingConvention.Cdecl)]//SetLastError = false)
        static extern int Video_2(byte[] ImageBuffer); //static extern int Video(byte[] ImageBuffer_1, int iNo);//, byte[] ImageBuffer_2h);


        //4 关闭
        [DllImport("V_DLL.dll", EntryPoint = "Set_Flag", CallingConvention = CallingConvention.Cdecl)]
        static extern void Set_Flag(int iType);  //2   0：视频、 1：关闭相机   2：拍照



        [DllImport("V_DLL.dll", EntryPoint = "Set_Flag_1", CallingConvention = CallingConvention.Cdecl)]
        static extern void Set_Flag_1();  // 1：关闭1相机
        [DllImport("V_DLL.dll", EntryPoint = "Set_Flag_2", CallingConvention = CallingConvention.Cdecl)]

        static extern void Set_Flag_2();  //关闭2相机

        [DllImport("V_DLL.dll", EntryPoint = "While_Video_1", CallingConvention = CallingConvention.Cdecl)]//SetLastError = false)
        static extern void  While_Video_1(byte[] ImageBuffer, int[] iOutArr); //static extern int Video(byte[] ImageBuffer_1, int iNo);//, byte[] ImageBuffer_2h);
        [DllImport("V_DLL.dll", EntryPoint = "While_Video_2", CallingConvention = CallingConvention.Cdecl)]//SetLastError = false)
        static extern void While_Video_2(byte[] ImageBuffer, int[] iOutArr); //static extern int Video(byte[] ImageBuffer_1, int iNo);//, byte[] ImageBuffer_2h);

        [DllImport("V_DLL.dll", EntryPoint = "Get_Video_1", CallingConvention = CallingConvention.Cdecl)]//SetLastError = false)
        static extern void Get_Video_1(byte[] ImageBuffer); //static extern int Video(byte[] ImageBuffer_1, int iNo);//, byte[] ImageBuffer_2h);
        [DllImport("V_DLL.dll", EntryPoint = "Get_Video_2", CallingConvention = CallingConvention.Cdecl)]//SetLastError = false)
        static extern void Get_Video_2(byte[] ImageBuffer); //static extern int Video(byte[] ImageBuffer_1, int iNo);//, byte[] ImageBuffer_2h);


        #endregion 调用DLL
        int m_iLanguage = 0;
        #region SDK
        Bitmap bmp;
        /// <summary>
        /// 参数传递时的排序方式
        /// </summary>
        List<string> ShutterItem_1 = new List<string> { "1/25", "1/50", "1/60", "1/80", "1/100", "1/120", "1/125", "1/150", "1/180", "1/200", "1/250", "1/500", "1/1k", "1/2k", "1/4k", "1/10k", "1/30", "1/100k", "1/15", "1/12", "1/10", "1/8", "1/6", "1/5", "1/4", "1/3", "1/2", "1" };
        /// <summary>
        /// 方便显示重新排线
        /// </summary>
        List<string> ShutterItem_2 = new List<string> { "1/100k", "1/10k", "1/4k", "1/2k", "1/1k", "1/500", "1/250", "1/200", "1/180", "1/150", "1/125", "1/120", "1/100", "1/80", "1/60", "1/50", "1/30", "1/25", "1/15", "1/12", "1/10", "1/8", "1/6", "1/5", "1/4", "1/3", "1/2", "1" };
//        NetClient.ITS_TTimeRangeParam _lpBuf = new NetClient.ITS_TTimeRangeParam();
        #endregion SDK

        /// <summary>
        /// 视频字体颜色
        /// </summary>
        private int m_iVideo_Color = 0;

        /// <summary>
        /// 接收服务器数据
        /// </summary>
        private TcpClient_UI m_Client = new TcpClient_UI();
        /// <summary>
        /// 是否启动
        /// </summary>
        private bool m_blActive = false;

        /// <summary>
        /// 显示界面:用于开机
        /// </summary>
        private bool m_blShowFrm = false;
        /// <summary>
        /// 是否拍照
        /// </summary>
        private bool m_blPhoto = false;
        public int m_iBuff_iRows = 0;
        public float m_flDistance_X = 0;
        public string m_strJyy = "";
        /// <summary>
        /// 是否录像
        /// </summary>
        private bool m_blVideo = false;
        /// <summary>
        /// 录像开始还是结束  1:开始  0：结束
        /// </summary>
        private string m_strVideo_S_E = "0";
        private bool m_HaveRun = false;
        /// <summary>
        /// 是否退出程序
        /// </summary>
        private bool m_blstrOut = false;
        /// <summary>
        /// 图像大小  true:大  false:小
        /// </summary>
        private bool m_blBig = true;

        /// <summary>
        /// 视频帧序号
        /// </summary>
        //    private   int m_iVideo_No = 0;


        #endregion
        public Frm_Video()
        {
            InitializeComponent();

           SysInfo .   m_Cam_Xm.m_bl_Add_MindCam = (SysInfo.m_csInter.IniReadDefine("Cam", "m_bl_Add_MindCam", "1", m_Net_V) =="1");

            if (SysInfo .m_Cam_Xm.m_bl_Add_MindCam)
            {
                m_FrameCallback = new pfnCameraGrabberFrameCallback(CameraGrabberFrameCallback);
                InitCamera();
            }
            else
            {
                Tab_Yay_Pan_1.RowStyles [0] = new System.Windows.Forms.RowStyle (SizeType.Percent, 0);
                Tab_Yay_Pan_1.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 100);
            }

            //     HLSysInfo.StartUp();
        }
        private void CameraGrabberFrameCallback(
        IntPtr Grabber,
        IntPtr pFrameBuffer,
        ref tSdkFrameHead pFrameHead,
        IntPtr Context)
        {
            // 数据处理回调

            // 由于黑白相机在相机打开后设置了ISP输出灰度图像
            // 因此此处pFrameBuffer=8位灰度数据
            // 否则会和彩色相机一样输出BGR24数据

            // 彩色相机ISP默认会输出BGR24图像
            // pFrameBuffer=BGR24数据

            // 执行一次GC，释放出内存
            GC.Collect();

            // 由于SDK输出的数据默认是从底到顶的，转换为Bitmap需要做一下垂直镜像
            MvApi.CameraFlipFrameBuffer(pFrameBuffer, ref pFrameHead, 1);

            int w = pFrameHead.iWidth;
            int h = pFrameHead.iHeight;
            Boolean gray = (pFrameHead.uiMediaType == (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
            Bitmap Image = new Bitmap(w, h,
                gray ? w : w * 3,
                gray ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb,
                pFrameBuffer);

            // 如果是灰度图要设置调色板
            if (gray)
            {
                Image.Palette = m_GrayPal;
            }

            this.Invoke((EventHandler)delegate
            {
                Pic_L.Image = Image;
                Pic_L.Refresh();
            });
        }
        private void InitCamera()
        {
            CameraSdkStatus status = 0;

            tSdkCameraDevInfo[] DevList;
            MvApi.CameraEnumerateDevice(out DevList);
            int NumDev = (DevList != null ? DevList.Length : 0);
            if (NumDev < 1)
            {
                MessageBox.Show("未扫描到相机");
                return;
            }
            else if (NumDev == 1)
            {
                status = MvApi.CameraGrabber_Create(out m_Grabber, ref DevList[0]);
            }
            else
            {
                if (MvApi.CameraGrabber_CreateFromDevicePage != null)
                    status = MvApi.CameraGrabber_CreateFromDevicePage(out m_Grabber);
                else
                    status = CameraSdkStatus. CAMERA_STATUS_FAILED;
            }

            if (status == 0)
            {
                MvApi.CameraGrabber_GetCameraDevInfo(m_Grabber, out m_DevInfo);
                MvApi.CameraGrabber_GetCameraHandle(m_Grabber, out m_hCamera);
                MvApi.CameraCreateSettingPage(m_hCamera, this.Handle, m_DevInfo.acFriendlyName, null, (IntPtr)0, 0);

                MvApi.CameraGrabber_SetRGBCallback(m_Grabber, m_FrameCallback, IntPtr.Zero);

                // 黑白相机设置ISP输出灰度图像
                // 彩色相机ISP默认会输出BGR24图像
                tSdkCameraCapbility cap;
                MvApi.CameraGetCapability(m_hCamera, out cap);
                if (cap.sIspCapacity.bMonoSensor != 0)
                {
                    MvApi.CameraSetIspOutFormat(m_hCamera, (uint)MVSDK.emImageFormat.CAMERA_MEDIA_TYPE_MONO8);

                    // 创建灰度调色板
                    Bitmap Image = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
                    m_GrayPal = Image.Palette;
                    for (int Y = 0; Y < m_GrayPal.Entries.Length; Y++)
                        m_GrayPal.Entries[Y] = Color.FromArgb(255, Y, Y, Y);
                }

                // 为了演示如何在回调中使用相机数据创建Bitmap并显示到PictureBox中，这里不使用SDK内置的绘制操作
                //MvApi.CameraGrabber_SetHWnd(m_Grabber, this.DispWnd.Handle);

                MvApi.CameraGrabber_StartLive(m_Grabber);
            }
            else
            {
                MessageBox.Show(String.Format("打开相机失败，原因：{0}", status));
            }
        }

        private void Time_ShowRun(object sender, System.Timers.ElapsedEventArgs e)
        {
            ShowTi(2);// Pic_Video.Visible = !Pic_Video.Visible;
        }
        void Pho_Video_Usb_MouseWhee(object sender, MouseEventArgs e)
        {
            //int numberOfTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;

            //if (numberOfTextLinesToMove > 0)
            //{
            //    for (int i = 0; i < numberOfTextLinesToMove; i++)
            //    {
            //        max();
            //    }
            //}
            //else if (numberOfTextLinesToMove < 0)
            //{
            //    for (int i = 0; i > numberOfTextLinesToMove; i--)
            //    {
            //        min();
            //    }
            //}

            //double step = 1.2;//缩放倍率
            //if (e.Delta > 0)
            //{
            //    if (Pho_Video_Usb.Height >= Screen.PrimaryScreen.Bounds.Height * 10)
            //        return;
            //    Pho_Video_Usb.Height = (int)(Pho_Video_Usb.Height * step);
            //    Pho_Video_Usb.Width = (int)(Pho_Video_Usb.Width * step);

            //    int px = Cursor.Position.X - Pho_Video_Usb.Location.X;
            //    int py = Cursor.Position.Y - Pho_Video_Usb.Location.Y;
            //    int px_add = (int)(px * (step - 1.0));
            //    int py_add = (int)(py * (step - 1.0));
            //    Pho_Video_Usb.Location = new Point(Pho_Video_Usb.Location.X - px_add, Pho_Video_Usb.Location.Y - py_add);
            //    Application.DoEvents();
            //}
            //else
            //{
            //    if (Pho_Video_Usb.Height <= Screen.PrimaryScreen.Bounds.Height)
            //        return;
            //    Pho_Video_Usb.Height = (int)(Pho_Video_Usb.Height / step);
            //    Pho_Video_Usb.Width = (int)(Pho_Video_Usb.Width / step);

            //    int px = Cursor.Position.X - Pho_Video_Usb.Location.X;
            //    int py = Cursor.Position.Y - Pho_Video_Usb.Location.Y;
            //    int px_add = (int)(px * (1.0 - 1.0 / step));
            //    int py_add = (int)(py * (1.0 - 1.0 / step));
            //    Pho_Video_Usb.Location = new Point(Pho_Video_Usb.Location.X + px_add, Pho_Video_Usb.Location.Y + py_add);
            //    Application.DoEvents();
            //}



            if (e.Delta < 0)
            {
                m_iAdd--;
                if (m_iAdd < 1) m_iAdd = 1;
                if (SysInfo.m_i_Curr_H > 50)
                    SysInfo.m_i_Curr_H -= 20;
                //Pho_Video_Usb.Width -= e.Delta;
                //Pho_Video_Usb.Height -= e.Delta;
            }
            else
            {
                m_iAdd++;
                SysInfo.m_i_Curr_H += 20;
                //Pho_Video_Usb.Width += e.Delta;
                //Pho_Video_Usb.Height += e.Delta;
            }
            m_i_Curr_X = 0;
            m_i_Curr_Y = 0;
        }
        private void max()
        {
            int w = Pic_L.Image.Width;
            int h = Pic_L.Image.Height;
            double div = Convert.ToDouble(h) / Convert.ToDouble(w);

            if (w < this.Width && h < this.Height)
            {
                w = w + 30;
                h = Convert.ToInt32(w * div);
                this.Pic_L.Left -= 15;
                this.Pic_L.Top -= (h - Pic_L.Image.Height) / 2;
            }
            Bitmap NewBitmap = new Bitmap(this.Pic_L.InitialImage, w, h);
            this.Pic_L.Image = NewBitmap;
        }
        private void min()
        {
            int w = Pic_L.Image.Width;
            int h = Pic_L.Image.Height;
            double div = Convert.ToDouble(h) / Convert.ToDouble(w);

            if (w > 30 && (w - 30) * div > 1)
            {
                w = w - 30;
                h = Convert.ToInt32(w * div);
                this.Pic_L.Left = 15;
                this.Pic_L.Top -= (h - Pic_L.Image.Height) / 2;
            }
            Bitmap NewBitmap = new Bitmap(this.Pic_L.InitialImage, w, h);
            this.Pic_L.Image = NewBitmap;
        }
        public byte[] StrToHex(string strHex)
        {
            //清空格
            strHex = strHex.Replace(" ", "");
            if ((strHex.Length % 2) != 0)
                strHex= strHex.Insert(0, "0");
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
        int m_iWaiteTimes = 30;
        private void Frm_Video_Load(object sender, EventArgs e)
        {
            m_iClimbType = int.Parse(SysInfo.m_csInter.IniReadDefine("Magnetic_Climb", "iClimbType", "0", Application.StartupPath + "\\database\\SysConfig.ini"));
            Pic_L.MouseWheel -= new MouseEventHandler(Pho_Video_Usb_MouseWhee);
            //1 视频初始化
            SysInfo.m_blIP = true;

            SysInfo.m_csInter.INIWriteValue("VIDEO", "HaveRun", "0", Application.StartupPath + "\\database\\SysConfig.ini");
            SysInfo.Time_Video.Interval = 500;
            //2 连接服务器准备通讯
            m_Client.ConnectToServer("", "");
            // TOFD:0  Cscan:1  M_Ui:2
            int _iType = int.Parse(SysInfo.m_csInter.IniReadDefine("System", "m_i_TOFD_0_Cscan_1", "0",
                                            Application.StartupPath + "\\database\\SysConfig.ini"));
            switch (_iType )
            {
                case 0:
                    SysInfo.m_iF_B = 1;
                    break;
                default :
                    SysInfo.m_iF_B = 0;
                    break;
            }
         
            string m_NetP = Application.StartupPath + "\\database\\SysConfig.ini";
            //使用500万相机1:500万  0：雄迈
            if (Thread_ShowVideo != null) Thread_ShowVideo.Abort();
            Thread_ShowVideo = new Thread(new ThreadStart(ThreadShow));
            Thread_ShowVideo.IsBackground = true;
            Thread_ShowVideo.Start();

            Bt_LR(1);
            m_blShowFrm = true;

            Pic_L.Height = this.Height;
            SetBtnStyle(Pic_L);
            m_blActive = true;
        }
       
        private void ShowTitl()
        {
            panel1.Parent = Pic_L;
            panel1.BackColor = Color.Transparent;
            Lb_RunMsg.Parent = panel1;
            Lb_RunMsg.BackColor = Color.Transparent;
        }

        int m_i_Dog_R_1 = 0;
        int m_i_Dog_R_2 = 0;
        int m_i_Dog_R_3 = 0;

        private void ThreadShow()
        {
            m_blReLink = false;
            m_Client.m_blOut = false ;
            Link_Phone();
            //int _iAll = m_Cam_Xm.m_iW_iMG_1 * 3;
            // m_i_Dog_R_1 = _iAll * (m_Cam_Xm.m_iH_iMG_1 / 2 - 100);
            // m_i_Dog_R_2 = _iAll * (m_Cam_Xm.m_iH_iMG_1 / 2 + 100);
            // m_i_Dog_R_3 = _iAll * (m_Cam_Xm.m_iH_iMG_1 / 2 + 200);

            if (Thread_RunVideo_CS_1 != null) Thread_RunVideo_CS_1.Abort();
            Thread_RunVideo_CS_1 = new Thread(new ThreadStart(Threa_Xm_Show));
            Thread_RunVideo_CS_1.Priority = ThreadPriority.Highest;
            Thread_RunVideo_CS_1.IsBackground = true;
            Thread_RunVideo_CS_1.Start();
            return;
        }
        delegate void Delg_Show();

        private void Show_Xm()
        {
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_Show ms = new Delg_Show(Threa_Xm_Show);
                    this.Invoke(ms, new object[] { });
                }
                catch
                { }
            }
            else
            {
                Threa_Xm_Show();
            }
        }
        private void Threa_Xm_Show()
        {
            #region 告诉服务器:程序运行啦
            if (m_HaveRun == false)
            {
            //    SysInfo.m_csInter.INIWriteValue("VIDEO", "HaveRun", "1", Application.StartupPath + "\\database\\SysConfig.ini");
                m_HaveRun = true;
            }
      //      SysInfo.m_iF_B= int.Parse(SysInfo.m_csInter.IniReadDefine("VIDEO", "m_iF_B", "2", m_Net_V));
            int _iWaitTime = int.Parse(SysInfo.m_csInter.IniReadDefine("VIDEO", "m_iWaitTime", "20", m_Net_V));
            int iWait_Link = int.Parse(SysInfo.m_csInter.IniReadDefine("Cam", "iWait_Link", "4", m_Net_V));
            m_iWaiteTimes = int.Parse(SysInfo.m_csInter.IniReadDefine("Cam", "m_iWaiteTimes_Rec", "30", m_Net_V));

            string _strT_IP = (SysInfo.m_csInter.IniReadDefine("Cam", "m_Ip_4", "", m_Net_V)).Trim ();
            SysInfo.m_bl_Qhzy = _strT_IP != "" && _strT_IP.Split('.').Length == 4;

            if (SysInfo.m_bl_Qhzy)
            {
                SysInfo.m_bl_Qhzy_50T_100F = true;
                Tab_Main.RowStyles [0] = new System.Windows.Forms.RowStyle (SizeType.Percent, 50);
                Tab_Main.RowStyles [1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 50);
            }
            else
            {
                SysInfo.m_i_Select = 3;
                SysInfo.m_bl_Qhzy_50T_100F = false ;
                Tab_Main.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0);
                Tab_Main.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 100);
            }

            #endregion 告诉服务器

            #region 录像参数初始化
 //           SysInfo.g_USB_Video.m_Cam_bl_Qhzy = SysInfo.m_bl_Qhzy;
            SysInfo.g_USB_Video.m_Cam_i_Num = 4;
            SysInfo.g_USB_Video.m_Cam_bl_Video = new bool[4];
            string _strT = "";
            for(int i=1;i<= SysInfo.g_USB_Video.m_Cam_i_Num;i++)
            {
                _strT = SysInfo.m_csInter.IniReadDefine ("Cam", "Video_"+i,i==1? "1":"0", Application.StartupPath + "\\database\\SysConfig.ini");
                SysInfo.g_USB_Video.m_Cam_bl_Video[i - 1] = _strT == "1";
            }
            SysInfo.g_USB_Video.m_Cam_Arr_videoWriter = new VideoWriter[4];
            #endregion

            DateTime _dtStar = DateTime.Now;
            bool _blLink_Msg_Ok = false;

            int _iFrameNum = 0;
            DateTime _dtStar_Frame = DateTime.Now;
            string HardFileName = Application.StartupPath + "\\database\\SysConfig.ini";
            SysInfo.m_i_Select = 2;
            SysInfo.m_iF_B = 2;
            while (m_Client.m_blOut == false)
            {
                try
                {
                    _iFrameNum++;
                    if (DateTime.Now.Subtract(_dtStar_Frame).TotalMilliseconds >= 1000)
                    {
                        SysInfo.m_Cam_Xm.m_iFrameNum = _iFrameNum;
                        _dtStar_Frame = DateTime.Now;
                        _iFrameNum = 0;
                    }

                    #region 将运行信息发送给服务器
                    if (SysInfo.m_blSendServLinkState == false)
                    {
                        if ((SysInfo.m_Cam_Xm.m_blReLink_1 != 0 || SysInfo.m_Cam_Xm.m_blReLink_2 != 0 || SysInfo.m_Cam_Xm.m_blReLink_3 != 0))
                        {
                            if (_blLink_Msg_Ok == false && DateTime.Now.Subtract(_dtStar).TotalSeconds > iWait_Link)
                                _blLink_Msg_Ok = true;
                        }
                        else
                            _blLink_Msg_Ok = true;
                        m_strServerMsg = "";

                        if (SysInfo.m_Cam_Xm.m_IP_1 != "" && SysInfo.m_Cam_Xm.m_blReLink_1 == 0) m_strServerMsg = "1";
                        if (SysInfo.m_Cam_Xm.m_IP_2 != "" && SysInfo.m_Cam_Xm.m_blReLink_2 == 0) m_strServerMsg += (m_strServerMsg != "" ? "," : "") + "2";
                        if (SysInfo.m_Cam_Xm.m_IP_3 != "" && SysInfo.m_Cam_Xm.m_blReLink_3 == 0) m_strServerMsg += (m_strServerMsg != "" ? "," : "") + "3";
                        if (SysInfo.m_Cam_Xm.m_IP_4 != "" && SysInfo.m_Cam_Xm.m_blReLink_4 == 0) m_strServerMsg += (m_strServerMsg != "" ? "," : "") + "4";
                        m_strServerMsg += "|";
                        if (_blLink_Msg_Ok)
                        {
                            if (SysInfo.m_Cam_Xm.m_IP_1 != "" && SysInfo.m_Cam_Xm.m_blReLink_1 != 0) m_strServerMsg = "1";
                            if (SysInfo.m_Cam_Xm.m_IP_2 != "" && SysInfo.m_Cam_Xm.m_blReLink_2 != 0) m_strServerMsg += (m_strServerMsg != "" ? "," : "") + "2";
                            if (SysInfo.m_Cam_Xm.m_IP_3 != "" && SysInfo.m_Cam_Xm.m_blReLink_3 != 0) m_strServerMsg += (m_strServerMsg != "" ? "," : "") + "3";
                            if (SysInfo.m_Cam_Xm.m_IP_4 != "" && SysInfo.m_Cam_Xm.m_blReLink_4 == 0) m_strServerMsg += (m_strServerMsg != "" ? "," : "") + "4";

                        }
                        SendServe();
                    }
                    #endregion 发送服务器
                    SysInfo.m_csInter.INIWriteValue("VIDEO", "HaveRun_1", SysInfo.m_Cam_Xm.m_blReLink_1.ToString(), HardFileName);
                    SysInfo.m_csInter.INIWriteValue("VIDEO", "HaveRun_2", SysInfo.m_Cam_Xm.m_blReLink_2.ToString(), HardFileName);
                    SysInfo.m_csInter.INIWriteValue("VIDEO", "HaveRun_3", SysInfo.m_Cam_Xm.m_blReLink_3.ToString(), HardFileName);

                    
           
                    if (SysInfo.m_bl_Qhzy)
                    {
                   //     SysInfo.m_iF_B = 0;
                        switch (SysInfo.m_iF_B)
                        {
                            case 0://4个
                                if (SysInfo.m_bl_Qhzy_50T_100F == false )//&& _strT_IP!="")
                                {
                                    SysInfo.m_bl_Qhzy_50T_100F = true;
                                    Tab_Main.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 50);
                                    Tab_Main.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 50);
                                }
                               
                                if (SysInfo.m_Cam_Xm.m_blReLink_1 == 0 || m_bl_CS) ShowVideo_Mul(1, SysInfo.m_Cam_Xm.m_Img_L, Pic_Q);
                                if (SysInfo.m_Cam_Xm.m_blReLink_2 == 0 || m_bl_CS) ShowVideo_Mul(2, SysInfo.m_Cam_Xm.m_Img_R, Pic_H);
                                if (SysInfo.m_Cam_Xm.m_blReLink_3 == 0 || m_bl_CS) ShowVideo_Mul(3, SysInfo.m_Cam_Xm.m_Img_R_3, Pic_L);
                                if (SysInfo.m_Cam_Xm.m_blReLink_4 == 0 || m_bl_CS) ShowVideo_Mul(4, SysInfo.m_Cam_Xm.m_Img_R_4, Pic_R);
                                break;

                            case 1://前 左右
                                if (SysInfo.m_bl_Qhzy_50T_100F == false)
                                {
                                    SysInfo.m_bl_Qhzy_50T_100F = true;
                                    Tab_Main.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 50);
                                    Tab_Main.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 50);
                                }
                           //     SysInfo.m_i_Select = 1;
                                if (SysInfo.m_Cam_Xm.m_blReLink_1 == 0 || m_bl_CS) ShowVideo_Mul(1, SysInfo.m_Cam_Xm.m_Img_L, Pic_Q);
                                if (SysInfo.m_Cam_Xm.m_blReLink_3 == 0 || m_bl_CS) ShowVideo_Mul(3, SysInfo.m_Cam_Xm.m_Img_R_3, Pic_L);
                                if (SysInfo.m_Cam_Xm.m_blReLink_4 == 0 || m_bl_CS) ShowVideo_Mul(4, SysInfo.m_Cam_Xm.m_Img_R_4, Pic_R);
                                break;
                            case 2://后 左右
                                if (SysInfo.m_bl_Qhzy_50T_100F == false)
                                {
                                    SysInfo.m_bl_Qhzy_50T_100F = true;
                                    Tab_Main.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 50);
                                    Tab_Main.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 50);
                                }
                            //    SysInfo.m_i_Select = 2;
                                if (SysInfo.m_Cam_Xm.m_blReLink_2 == 0 || m_bl_CS) ShowVideo_Mul(2, SysInfo.m_Cam_Xm.m_Img_R, Pic_Q);
                                if (SysInfo.m_Cam_Xm.m_blReLink_3 == 0 || m_bl_CS) ShowVideo_Mul(3, SysInfo.m_Cam_Xm.m_Img_R_3, Pic_L);
                                if (SysInfo.m_Cam_Xm.m_blReLink_4 == 0 || m_bl_CS) ShowVideo_Mul(4, SysInfo.m_Cam_Xm.m_Img_R_4, Pic_R);
                                break;
                            case 3:// 左右
                                if (SysInfo.m_bl_Qhzy_50T_100F)
                                {
                             //       SysInfo.m_i_Select = 3;
                                    SysInfo.m_bl_Qhzy_50T_100F = false;
                                    Tab_Main.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0);
                                    Tab_Main.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 100);
                                }
                                if (SysInfo.m_Cam_Xm.m_blReLink_3 == 0 || m_bl_CS) ShowVideo_Mul(3, SysInfo.m_Cam_Xm.m_Img_R_3, Pic_L);
                                if (SysInfo.m_Cam_Xm.m_blReLink_4 == 0 || m_bl_CS) ShowVideo_Mul(4, SysInfo.m_Cam_Xm.m_Img_R_4, Pic_R);
                                break;
                            case 4://前
                                if (SysInfo.m_bl_Qhzy_50T_100F || SysInfo.m_i_Select != 1)
                                {
                                    SysInfo.m_bl_Qhzy_50T_100F = false;
                                    Tab_Main.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 100);
                                    Tab_Main.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0);
                                }
                           //     SysInfo.m_i_Select = 1;
                                if (SysInfo.m_Cam_Xm.m_blReLink_1 == 0 || m_bl_CS) ShowVideo_Mul(1, SysInfo.m_Cam_Xm.m_Img_L, Pic_Q);
                                break;
                            case 5://后
                                if (SysInfo.m_bl_Qhzy_50T_100F || SysInfo.m_i_Select != 2)
                                {
                                    SysInfo.m_bl_Qhzy_50T_100F = false;
                                    Tab_Main.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 100);
                                    Tab_Main.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0);
                                }
                            //    SysInfo.m_i_Select = 2;
                                if (SysInfo.m_Cam_Xm.m_blReLink_3 == 0 || m_bl_CS) ShowVideo_Mul(2, SysInfo.m_Cam_Xm.m_Img_R, Pic_Q);
                                break;
                        }
                    }
                    else
                    {
                        if (SysInfo.m_bl_Qhzy_50T_100F)
                        {
                            SysInfo.m_bl_Qhzy_50T_100F = false;
                            Tab_Main.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0);
                            Tab_Main.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 100);
                        }
                        
                        switch (SysInfo.m_iF_B)
                        {
                            case 1:
                                // SysInfo .m_Cam_Xm.m_clXz_1.i_Xz = int.Parse(SysInfo.m_str_ABC.Substring(0, 1));
                                if (SysInfo.m_Cam_Xm.m_blReLink_1 == 0 || m_bl_CS) ShowVideo_Mul(1, SysInfo.m_Cam_Xm.m_Img_L, Pic_L);//, 1);
                                if (SysInfo.m_Cam_Xm.m_blReLink_3 == 0 || m_bl_CS) ShowVideo_Mul(3, SysInfo.m_Cam_Xm.m_Img_R_3, Pic_R);//, 2);
                                break;
                            case 2:
                                // SysInfo .m_Cam_Xm.m_clXz_1.i_Xz = int.Parse(SysInfo.m_str_ABC.Substring(0, 1));
                                if (SysInfo.m_Cam_Xm.m_blReLink_1 == 0 || m_bl_CS) ShowVideo_Mul(1, SysInfo.m_Cam_Xm.m_Img_L, Pic_L);//, 3);
                                if (SysInfo.m_Cam_Xm.m_blReLink_2 == 0 || m_bl_CS) ShowVideo_Mul(2, SysInfo.m_Cam_Xm.m_Img_R, Pic_R);//, 4);
                                break;
                            case 3:
                           //     SysInfo.m_i_Select = 3;
                                if (SysInfo.m_Cam_Xm.m_blReLink_1 == 0 || m_bl_CS) ShowVideo_Mul(1, SysInfo.m_Cam_Xm.m_Img_L, Pic_L);//, 5);
                                break;
                            case 4:
                             //   SysInfo.m_i_Select = 4;
                                if (SysInfo.m_Cam_Xm.m_blReLink_2 == 0 || m_bl_CS) ShowVideo_Mul(2, SysInfo.m_Cam_Xm.m_Img_R, Pic_L);//, 6);
                                break;
                            case 5:
                            //    SysInfo.m_i_Select = 4;
                                if (SysInfo.m_Cam_Xm.m_blReLink_3 == 0 || m_bl_CS) ShowVideo_Mul(3, SysInfo.m_Cam_Xm.m_Img_R_3, Pic_L);//, 7);
                                break;
                            default:
                                break;
                        }
                    }

                    if (SysInfo.m_iF_B < 0 || SysInfo.m_iF_B > 5)
                        Thread.Sleep(100);
                    else
                        Thread.Sleep(_iWaitTime);
                }
                catch (Exception eee)
                { }
            }
            SysInfo .m_Cam_Xm.blOut = true;
        }
          private void Threa_New_Video_1()
        {
         
            DateTime _dtStar = DateTime.Now;
            bool _blFal = false;//是否掉线
            while (true)
            {
                if (m_blReLink_1) break;
                m_dtStar = DateTime.Now;
                //if (m_Client.m_blLinkServe && SysInfo.g_Video.Read_One("strOut") == "1")
                //{
                //    Time_RunTime.Enabled = false;
                //    Set_Flag(1);
                //    //  MessageBox.Show("退出");
                //    m_blstrOut = true;
                //    Time_Exit.Enabled = true;
                //    Application.Idle -= Application_Idle;
                //    SysInfo.g_Video.Write_One("HaveRun", "0");
                //    return;
                //}
                try
                {

                    if (m_blRepLink_1 == false)
                    {
                        //        SysInfo.m_iF_B = 2;
                          m_blGetImge_1 = Video_1(m_Img_L) == 1;
                        //  m_blGetImge_1 = m_iArrOut_1[0] == 0;
                        if (m_ArrOut[0]==0 )//m_blGetImge_1)
                        {
                            //Get_Video_1(m_Img_L);
                            //if (SysInfo.m_iF_B == 1)// &&  == 0)
                            //    ShowVideo_1(m_Img_L);
                        }
                    }
                }
                catch { }
                #region 外部操作

               

                if (m_blRepLink_1 == false && m_blGetImge_1 == false)
                {
                    if (SysInfo.m_iF_B == 1)
                    {
                        m_iFrameNum++;
                        if (m_iFrameNum >= 2147483647)
                            m_iFrameNum = 0;
                                          }
                    if (_blFal == false)
                    {
                        _blFal = true;
                        _dtStar = DateTime.Now;

                    }
                    else
                    {//m_iClimbType !=1&&
                        if (m_iClimbType != 1 && DateTime.Now.Subtract(_dtStar).TotalSeconds > int.Parse(SysInfo.m_csInter.IniReadDefine("Video", "Gz_Time", "3", Application.StartupPath + "\\database\\SysConfig.ini")))
                        {
                            _blFal = false;
                            m_blReLink_1 = true;
                            break;
                        }
                    }
                }
                else
                    _blFal = false;
                #endregion
            }


            if (m_blReLink_1)
            {
                if (this.InvokeRequired == true)
                {
                    try
                    {
                        Delg_ReLink del_Limit = new Delg_ReLink(ReLink);
                        this.Invoke(del_Limit, new object[] { 1 });
                    }
                    catch { }
                }
                else
                {
                    ReLink(1);
                }

            }
            //      MessageBox.Show("程序退出");
        }
        private void ReLink(int iType)
        {
            Waite(0.1f);
            if (iType == 1)
                Time_ReLink.Enabled = true;
            else
                Time_ReLink_2.Enabled = true;
        }

        //private void Threa_New_Video_2()
        //{
        //    DateTime _dtStar = DateTime.Now;
        //    bool _blFal = false;//是否掉线


        //    while (true)
        //    {
        //        if (m_blReLink_2) break;
        //        //if (m_Client.m_blLinkServe && SysInfo.g_Video.Read_One("strOut") == "1")
        //        //{
        //        //    Time_RunTime.Enabled = false;
        //        //    Set_Flag(1);
        //        //    //  MessageBox.Show("退出");
        //        //    m_blstrOut = true;
        //        //    Time_Exit.Enabled = true;
        //        //    Application.Idle -= Application_Idle;
        //        //    SysInfo.g_Video.Write_One("HaveRun", "0");
        //        //    return;
        //        //}
        //        //  if (SysInfo.m_iF_B == 1) return;

        //        #region 1 视频显示左窗体
        //        if (m_iLink == 0)
        //        {
        //            try
        //            {
        //                if (m_blRepLink_1 == false)
        //                {
        //                  //   SysInfo.m_iF_B = 2;
        //                     m_blGetImge_2 = Video_2(m_Img_R) == 1;
        //                   // m_blGetImge_2 = m_iArrOut_2[0] == 1;
        //                    if (m_ArrOut[1] ==0)//m_blGetImge_2)
        //                    {//Get_Video_2(m_Img_R); 
        //                        if (SysInfo.m_iF_B == 2)//&& m_ArrOut[1] == 0)
        //                            ShowVideo_2(m_Img_R);
        //                    }
        //                }
        //            }
        //            catch { }
        //        }
        //        if (m_blRepLink_1 == false && m_blGetImge_2 == false)
        //        {
        //            if (SysInfo.m_iF_B == 2)
        //            {
        //                m_iFrameNum++;
        //                if (m_iFrameNum >= 2147483647)
        //                    m_iFrameNum = 0;
                       
        //            }
        //            if (_blFal == false)
        //            {
        //                _blFal = true;
        //                _dtStar = DateTime.Now;
        //            }
        //            else
        //            {//m_iClimbType != 1 && 
        //                if (m_iClimbType != 1 && DateTime.Now.Subtract(_dtStar).TotalSeconds > int.Parse(SysInfo.m_csInter.IniReadDefine("Video", "Gz_Time", "3", Application.StartupPath + "\\database\\SysConfig.ini")))
        //                {
        //                    _blFal = false;
        //                    m_blReLink_2 = true;
        //                    break;
        //                }
        //            }
        //        }
        //        else
        //            _blFal = false;

        //        if (m_HaveRun == false)
        //        {
                  
        //            m_HaveRun = true;
        //        }
        //        #endregion

        //    }
        //    if (m_blReLink_2)
        //    {
        //        if (this.InvokeRequired == true)
        //        {
        //            try
        //            {
        //                Delg_ReLink_2 del_Limit = new Delg_ReLink_2(ReLink);
        //                this.Invoke(del_Limit, new object[] { 2 });
        //            }
        //            catch { }
        //        }
        //        else
        //        {
        //            ReLink(2);
        //        }

        //    }
        //}
        //private void ShowVideo_1(byte[] btArr)
        //{
        //    if (this.InvokeRequired == true)
        //    {
        //        try
        //        {
        //            Delg_ShowVideo del_Show = new Delg_ShowVideo(ShowImage_1);
        //            this.Invoke(del_Show, new object[] { btArr });
        //        }
        //        catch { }
        //    }
        //    else
        //    {
        //        ShowImage_1(btArr);
        //    }
        //}
        private void ShowVideo_Mul(int iNo, byte[] btArr,PictureBox PicBox)//, int iTitl_Type)
        {
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_ShowVideo del_Show = new Delg_ShowVideo(ShowImage_Mul);
                    this.Invoke(del_Show, new object[] { iNo, btArr, PicBox });// , iTitl_Type });
                }
                catch { }
            }
            else
            {
                ShowImage_Mul(iNo, btArr, PicBox);//, iTitl_Type);
            }
        }
        private void ShowTi(int iType, string strVal = "")
        {
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_ShowTiTl del_Limit = new Delg_ShowTiTl(MsgShow);
                    this.Invoke(del_Limit, new object[] { iType, strVal });
                }
                catch { }
            }
            else
            {
                MsgShow(iType, strVal);
            }
        }
        private void MsgShow(int iType, String strVal = "")
        {
            try
            {
                //if (Time_RunTime.Enabled == false && m_LstImgDat.Count > 0)
                //    Time_RunTime.Enabled = true;

                if (SysInfo.mVideo == 1)
                {
                    if (m_iShowPh++ % 15 == 0)
                        Bt_Video.Visible = Bt_Video.Visible ? false : true;// Lb_RunMsg.Text = SysInfo.m_strVideoMsg + " " + DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                }
                else if (iType == 2)
                {
                    Lb_Err.Text = "视频为空:" + (m_iErr++).ToString();
           //         if (Lb_Err.Visible == false) Lb_Err.Visible = true;
                }
                else if (iType == 3)
                {
                    Lb_Err_2.Text = "视频: " + (m_iErr_2++).ToString();
              //      if (Lb_Err_2.Visible == false) Lb_Err_2.Visible = true;

                }
                else
                    //  if (Bt_Video.Visible)
                    Bt_Video.Visible = false;
                return;
                switch (iType)
                {
                    case 0:
                        if (m_iShowVideo > 9)
                            Pic_Video.Visible = (m_iShowVideo++ % 10 == 0);
                        break;
                    case 1:
                        if (Pic_Video.Visible)
                            Pic_Video.Visible = strVal == "" ? false : true;
                        break;
                    case 2:
                        if (Pic_Video.Visible)
                            Pic_Video.Visible = false;
                        else
                            Pic_Video.Visible = true;
                        //   textBox1 .Text = SysInfo.g_Video.iVideo_No.ToString ();
                        break;
                }
            }
            catch (Exception e2)
            { }
        }
        #region SDK函数刷新
        private void GetData(Bitmap bmp)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            //    if (m_i_With == 0)
            int m_i_With = bmp.Width;
            int m_i_Height = bmp.Height;

            //1 初始化
            // Ini(2);
            System.Drawing.Imaging.BitmapData bmpdata = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpdata.Scan0;

            int bytes = m_i_With * m_i_Height * 4;
            byte[] Arr_Rgb = new byte[bytes];
            Marshal.Copy(ptr, Arr_Rgb, 0, bytes);
        }
        private void PubCapture_ImageGrabbed(Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> WBitmap)
        {
            //bmp = (Bitmap)WBitmap.Bitmap.Clone();
            ShowPicMsg(WBitmap.Bitmap);
            Pic_L.BackgroundImage = WBitmap.Bitmap;

            #region 录像
            SysInfo.m_ImgDat = WBitmap;

            if (SysInfo.mVideo == 0)
                m_strMsg = SysInfo.m_strVideoMsg + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            else
                m_strMsg = SysInfo.m_strVideoMsg + "   Rec :" + FormatRunTime((long)DateTime.Now.Subtract(SysInfo.m_strTime).TotalSeconds);  //   DateTime.Now.ToString("yy-MM-dd HH:mm:ss");

         //   HLSysInfo.SetOSD(m_strMsg, 1, 0, 0);
            if (SysInfo.mVideo == 1)
            {
                SysInfo.g_USB_Video.VideoNet(WBitmap);
                //  Waite(0.6f);
            }
            if (m_Client.tcpClient != null)
            {
                //if (m_Client.tcpClient.Connected)//m_Client.m_blLinkServe 
                //    m_Client.SendDatToServe("4.0", SysInfo.g_Video.iVideo_No.ToString());
            }
          

            #endregion
           
        }
        private void ShowPicMsg(Bitmap Map)
        {
            Font font = new Font("黑体", 30, FontStyle.Regular, GraphicsUnit.Millimeter);
            SolidBrush drawBrush = new SolidBrush(m_iVideo_Color == 1 ? Color.Yellow : Color.Blue);

            using (Graphics graphics = Graphics.FromImage(Map))//   CvBitmap))
            {

                graphics.DrawString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + (SysInfo.m_iF_B == 1 ? (m_iLanguage == 0 ? "前视" : "Front") : (m_iLanguage == 0 ? "后视" : "Back")), font, drawBrush, 10, 5);//:fff
                                                                                                                                                                                                                      //   graphics.DrawString(strMsg, font, drawBrush, 10, 55);                                                                                          //font = new Font("Arial", 7, FontStyle.Regular, GraphicsUnit.Millimeter);
                                                                                                                                                                                                                      //graphics.DrawString( (flWellDeep.ToString("f3") + "m " + strMsg), font, drawBrush, 0, 25);
                                                                                                                                                                                                                      //font = new Font("Arial", 5, FontStyle.Regular, GraphicsUnit.Millimeter);
                                                                                                                                                                                                                      //graphics.DrawString(DateTime.Now.ToString( strMsg), font, drawBrush, 0,48);
                graphics.Dispose();
            }
        }
        #endregion

        #region 调用DLL
        private void Thread_ReadUI()
        {
            if (Thread_Run_Video != null) Thread_Run_Video.Abort();
            Thread_Run_Video = new Thread(new ThreadStart(Show_Video));
            // Thread_RunVideo.Priority = ThreadPriority.AboveNormal;
            Thread_Run_Video.Name = "Thread_Run_Video";
            Thread_Run_Video.IsBackground = true;
            Thread_Run_Video.Start();
        }
        /// <summary>
        /// 连接相机
        /// </summary>
        private void Link_Phone()
        {
          //  m_blSendServLinkState = false;
            SysInfo .m_Cam_Xm.Link_Phone();
           
        }
        string m_NetP = Application.StartupPath + "\\database\\SysConfig.ini";
            string m_IP="";
            string m_IP_2 = "";

        //private void RunVideo_1()
        //{
        //    string _strMsg = "", _strMsg2 = "";
        //     m_IP = SysInfo.m_csInter.IniReadDefine("Cam", "m_Ip_1", "192.168.1.12", m_NetP);
        //     m_IP_2 = SysInfo.m_csInter.IniReadDefine("Cam", "m_Ip_2", "", m_NetP);


        //    int m_iCam_Len = SysInfo.m_iW_iMG * SysInfo.m_iH_iMG * 3;
        //    m_Img_L = new byte[m_iCam_Len];
        //    m_Img_R = new byte[m_iCam_Len];
        //    SysInfo.m_ImgDat = new Image<Bgr, byte>(SysInfo.m_iW_iMG, SysInfo.m_iH_iMG);

        //    string _Port = SysInfo.m_csInter.IniReadDefine("Cam", "Port_1", "554", m_NetP);
        //    m_ArrOut[0] = 0;
        //    m_ArrOut[1] = 0;
        //    m_ArrOut[2] = 12;
        //    pair_image_capture(SysInfo.m_iW_iMG, SysInfo.m_iH_iMG, "", m_IP, m_IP_2, _Port, m_ArrOut);//, m_Img_L ,m_Img_R );//, m_Img_L, m_Img_R);
        //    m_iLink = (m_ArrOut[0] == 0 || m_ArrOut[1] == 0) ? 0 : -1;
        //    switch (m_ArrOut[0])
        //    {
        //        case -1://1号相机打开失败
        //            _strMsg = m_iLanguage == 0 ? "1号相机打开失败" : "Camera 1 failed to open";
        //            break;
        //        case -11://1号相机rtsp失败
        //            _strMsg = m_iLanguage == 0 ? "1号相机rtsp失败" : "Rtsp failed for camera 1";
        //            break;
        //        case -12://1号相机打开和rtsp失败
        //            _strMsg = m_iLanguage == 0 ? "1号相机打开和rtsp失败" : "Camera 1 is turned on and rtsp fails";
        //            break;
        //        case 0://成功
        //            break;
        //    }
        //    switch (m_ArrOut[1])
        //    {
        //        case -1://1号相机打开失败
        //            _strMsg2 = m_iLanguage == 0 ? "2号相机打开失败" : "Camera 2 failed to open";
        //            break;
        //        case -11://1号相机rtsp失败
        //            _strMsg2 = m_iLanguage == 0 ? "2号相机rtsp失败" : "Rtsp failed for camera 2";
        //            break;
        //        case -12://1号相机打开和rtsp失败
        //            _strMsg2 = m_iLanguage == 0 ? "2号相机打开和rtsp失败" : "Camera 2 is turned on and rtsp fails";
        //            break;
        //        case 0://成功
        //            break;
        //    }
        //    m_strServerMsg = ((_strMsg.Length + _strMsg2.Length) > 0 ? _strMsg + " " + _strMsg2 : "1");
        //    if (Thread_Tcp_Send != null) Thread_Tcp_Send.Abort();
        //    Thread_Tcp_Send = new Thread(new ThreadStart(SendServe));
        //    Thread_Tcp_Send.IsBackground = true;
        //    Thread_Tcp_Send.Start();
           
        //    m_blRepLink_1 = false;

        //    return;
        //   // return;//Thread_Tcp_Send
        //    if (Thread_WhileGetVideo_1 != null) Thread_WhileGetVideo_1.Abort();
        //    Thread_WhileGetVideo_1 = new Thread(new ThreadStart(While_Video_1));
        //    Thread_WhileGetVideo_1.IsBackground = true;
        //    Thread_WhileGetVideo_1.Start();

        //    if (Thread_WhileGetVideo_2 != null) Thread_WhileGetVideo_2.Abort();
        //    Thread_WhileGetVideo_2 = new Thread(new ThreadStart(While_Video_2));
        //    Thread_WhileGetVideo_2.IsBackground = true;
        //    Thread_WhileGetVideo_2.Start();



        //    //bool _blGeet = true;
        //    //while (m_blstrOut == false)
        //    //{
        //    //    if(_blGeet)
        //    //    m_iVideo = Video(m_Img_L, m_Img_R, SysInfo.m_iF_B);
        //    //    _blGeet = !_blGeet;
        //    //    Thread.Sleep(2);
        //    //    System.Windows.Forms.Application.DoEvents();
        //    //}
        //}
        private void SendServe()
        {
            m_Client.SendDatToServe("10.0", m_strServerMsg);
        }
        /// <summary>
        /// 重新开始
        /// </summary>
        private void SendServe_ReStart()
        {
            m_Client.SendDatToServe("8.0", "1");
        }
        private void While_Video_1()
        {
            m_iArrOut_1[0] = -1;// new int[1];
        //  while (true)
            {
                try
                {

                    if (m_blRepLink_1 == false && m_ArrOut[0] == 0)//m_blReLink_1 == false &&
                    {
                        While_Video_1(m_Img_L, m_iArrOut_1); //  m_blGetImge_1 = Video_1(m_Img_L) == 1;
                      //  m_blGetImge_1 = m_iArrOut_1[0] == 1;
                    }
                    Application.DoEvents();
                }
                catch { }
            }
        }
        private void While_Video_2()
        {
            m_iArrOut_2[0] = -1;// 
             //                         while (true)
            {
                try
                {
                    if (m_blRepLink_1 == false && m_ArrOut[1] == 0)//m_blReLink_2 == false && 
                    {   //  m_blGetImge_2 = Video_2(m_Img_R) == 1;
                       While_Video_2(m_Img_R, m_iArrOut_2); // m_blGetImge_2 = Video_2(m_Img_R) == 1;
                     //   m_blGetImge_2 = m_iArrOut_2[0] == 1;

                    }
                    Application.DoEvents();
                }
                catch { }
            }
        }
     
     
        #endregion 调用DLL
        public static void Tool_Tip(System.Windows.Forms.Button BtTi, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(BtTi, strVal);
        }
        private void SetTran(PictureBox pic_Parents, Button pic_Son)
        {
            pic_Parents.SendToBack();
            pic_Son.BackColor = Color.Transparent;
            pic_Son.Parent = pic_Parents;
            pic_Son.BringToFront();
        }
        private void Waite(float flWait = 1)
        {
            DateTime _dtStar = DateTime.Now;
            while (true)
            {
                try
                {
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(_dtStar).TotalSeconds > flWait) break;
                    Thread.Sleep(1);
                }
                catch { break; }
            }
        }
        /// <summary>
        /// 运行摄像头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Idle(object sender, EventArgs e)
        {
            Show_Video();
        }
        public Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
        {
            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放           
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            if (sHeight > destHeight || sWidth > destWidth)
            {
                if ((sWidth * destHeight) > (sHeight * destWidth))
                {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }
            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量     
            System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            System.Drawing.Imaging.EncoderParameter encoderParam = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }
        public enum ZoomType { NearestNeighborInterpolation, BilinearInterpolation }
        /// <summary>
        /// 图像缩放
        /// </summary>
        /// <param name="srcBmp">原始图像</param>
        /// <param name="width">目标图像宽度</param>
        /// <param name="height">目标图像高度</param>
        /// <param name="dstBmp">目标图像</param>
        /// <param name="GetNearOrBil">缩放选用的算法</param>
        /// <returns>处理成功 true 失败 false</returns>
        public Bitmap Zoom(Bitmap srcBmp, double ratioW, double ratioH, ZoomType zoomType)
        {//ZoomType为自定义的枚举类型  out Bitmap dstBmp,

            Bitmap dstBmp;
            //if (srcBmp == null)
            //{
            //    dstBmp = null;
            //    return false;
            //}
            //若缩放大小与原图一样，则返回原图不做处理
            if ((ratioW == 1.0) && ratioH == 1.0)
            {
                dstBmp = new Bitmap(srcBmp);
                return dstBmp;
            }
            //计算缩放高宽
            double height = ratioH * (double)srcBmp.Height;
            double width = ratioW * (double)srcBmp.Width;
            dstBmp = new Bitmap((int)width, (int)height);

            System.Drawing.Imaging.BitmapData srcBmpData = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            System.Drawing.Imaging.BitmapData dstBmpData = dstBmp.LockBits(new Rectangle(0, 0, dstBmp.Width, dstBmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* srcPtr = null;
                byte* dstPtr = null;
                int srcI = 0;
                int srcJ = 0;
                double srcdI = 0;
                double srcdJ = 0;
                double a = 0;
                double b = 0;
                double F1 = 0;//横向插值所得数值
                double F2 = 0;//纵向插值所得数值
                if (zoomType == ZoomType.NearestNeighborInterpolation)
                {//邻近插值法

                    for (int i = 0; i < dstBmp.Height; i++)
                    {
                        srcI = (int)(i / ratioH);//srcI是此时的i对应的原图像的高
                        srcPtr = (byte*)srcBmpData.Scan0 + srcI * srcBmpData.Stride;
                        dstPtr = (byte*)dstBmpData.Scan0 + i * dstBmpData.Stride;
                        for (int j = 0; j < dstBmp.Width; j++)
                        {
                            dstPtr[j * 3] = srcPtr[(int)(j / ratioW) * 3];//j / ratioW求出此时j对应的原图像的宽
                            dstPtr[j * 3 + 1] = srcPtr[(int)(j / ratioW) * 3 + 1];
                            dstPtr[j * 3 + 2] = srcPtr[(int)(j / ratioW) * 3 + 2];
                        }
                    }
                }
                else if (zoomType == ZoomType.BilinearInterpolation)
                {//双线性插值法
                    byte* srcPtrNext = null;
                    for (int i = 0; i < dstBmp.Height; i++)
                    {
                        srcdI = i / ratioH;
                        srcI = (int)srcdI;//当前行对应原始图像的行数
                        srcPtr = (byte*)srcBmpData.Scan0 + srcI * srcBmpData.Stride;//指原始图像的当前行
                        srcPtrNext = (byte*)srcBmpData.Scan0 + (srcI + 1) * srcBmpData.Stride;//指向原始图像的下一行
                        dstPtr = (byte*)dstBmpData.Scan0 + i * dstBmpData.Stride;//指向当前图像的当前行
                        for (int j = 0; j < dstBmp.Width; j++)
                        {
                            srcdJ = j / ratioW;
                            srcJ = (int)srcdJ;//指向原始图像的列
                            if (srcdJ < 1 || srcdJ > srcBmp.Width - 1 || srcdI < 1 || srcdI > srcBmp.Height - 1)
                            {//避免溢出（也可使用循环延拓）
                                dstPtr[j * 3] = 255;
                                dstPtr[j * 3 + 1] = 255;
                                dstPtr[j * 3 + 2] = 255;
                                continue;
                            }
                            a = srcdI - srcI;//计算插入的像素与原始像素距离（决定相邻像素的灰度所占的比例）
                            b = srcdJ - srcJ;
                            for (int k = 0; k < 3; k++)
                            {//插值    公式：f(i+p,j+q)=(1-p)(1-q)f(i,j)+(1-p)qf(i,j+1)+p(1-q)f(i+1,j)+pqf(i+1, j + 1)
                                F1 = (1 - b) * srcPtr[srcJ * 3 + k] + b * srcPtr[(srcJ + 1) * 3 + k];
                                F2 = (1 - b) * srcPtrNext[srcJ * 3 + k] + b * srcPtrNext[(srcJ + 1) * 3 + k];
                                dstPtr[j * 3 + k] = (byte)((1 - a) * F1 + a * F2);
                            }
                        }
                    }
                }
            }
            srcBmp.UnlockBits(srcBmpData);
            dstBmp.UnlockBits(dstBmpData);
            return dstBmp;
        }
        private void Show_Video()
        {
            //if (m_blAddD3D == false)
            //{
            //    m_blAddD3D = true; m_bl_D3D = InitD3D(Pho_Video_Usb.Handle, true, 640, 480) == 0;
            //}

            #region 视频
           
            if (SysInfo.g_USB_Video.m_iType == 0)
            {

                if (SysInfo.mVideo == 0)
                    m_strMsg = SysInfo.m_strVideoMsg + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                else
                    m_strMsg = SysInfo.m_strVideoMsg + "   Rec :" + FormatRunTime((long)DateTime.Now.Subtract(SysInfo.m_strTime).TotalSeconds);  //   DateTime.Now.ToString("yy-MM-dd HH:mm:ss");

                SysInfo.g_USB_Video.GetMat(0, m_iVideo_Color, m_strMsg);//0:兰 1：黄
            }
            //SysInfo.mVideo = 0;
            //SysInfo.g_USB_Video.m_FileName = "4466665";
            //SysInfo.g_USB_Video.Video(SysInfo.mVideo, SysInfo.m_blIP, SysInfo.m_iW_iMG, SysInfo.m_iH_iMG);
            try
            {
                int _iNo = SysInfo.m_iF_B - 1;
                _iNo = _iNo < 0 ? 0 : _iNo;
                //             Pho_Video_Usb.Image = (Bitmap)(SysInfo.g_USB_Video.CvMat.Bitmap);

                if (m_bl_D3D)//false && m_bl_D3D)
                {
                    #region 图像转换成数组
                    //Rectangle rect = new Rectangle(0, 0, SysInfo.g_USB_Video.CvMat.Bitmap.Width, SysInfo.g_USB_Video.CvMat.Bitmap.Height);
                    //System.Drawing.Imaging.BitmapData bmpData =
                    //                       SysInfo.g_USB_Video.CvMat.Bitmap.LockBits(
                    //                           rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    //                           SysInfo.g_USB_Video.CvMat.Bitmap.PixelFormat);
                    //int bytes = Math.Abs(bmpData.Stride) * SysInfo.g_USB_Video.CvMat.Bitmap.Height;
                    //byte[] rgbValues = new byte[bytes];
                    //System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
                    #endregion

                    Render(Invert_Data(SysInfo.g_USB_Video.m_RgbaImg.Bitmap)) ;

                 //  Render(Invert_Data_(SysInfo.g_USB_Video.CvMat.Bitmap));////m_RgbaImg.Bitmap));
                    //   Pic_Rect(ToColorBitmap2(Invert_Data(SysInfo.g_USB_Video.CvMat.Bitmap),
                    //   SysInfo.g_USB_Video.CvMat.Bitmap.Width, SysInfo.g_USB_Video.CvMat.Bitmap.Height));
                }
                else
                //  Pho_Video_Usb.Image = SysInfo.m_ImgDat.Bitmap;
                {
              //     int _il=  SysInfo.g_USB_Video.m_imagesHsv.Length  ;
                    Pic_Rect(SysInfo.g_USB_Video.CvMat.Bitmap);   // SysInfo.g_USB_Video.m_imagesHsv[1].Bitmap );//   SysInfo.g_USB_Video.CvMat.Bitmap);
              //      _il = SysInfo.g_USB_Video.m_imagesHsv[1].Data[0, 91, 0];
                }
                //float _flD = 0.5f;
                //Pho_Video_Usb.Image = (Bitmap)(Zoom ( SysInfo.g_USB_Video.CvMat.Bitmap,
                //     _flD, _flD, ZoomType.NearestNeighborInterpolation));

               
            }
            catch (Exception e)
            { }
            #endregion
        }
        private Bitmap[] m_pBitmaps = new Bitmap[15];
        private int m_nCurrBitmapIdx = -1;
        bool m_bFrmSizeChange;
        public Bitmap ToColorBitmap2(byte[] rawValues, int width, int height)
        {
            // 申请目标位图的变量，并将其内存区域锁定
            //初始化Bitmap数组
            if (m_bFrmSizeChange || m_nCurrBitmapIdx < 0)
            {
                for (int i = 0; i < 15; i++)
                {
                    m_pBitmaps[i] = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                }
                m_nCurrBitmapIdx = 0;
                m_bFrmSizeChange = false;
            }
            Bitmap bmp = m_pBitmaps[m_nCurrBitmapIdx];
            m_nCurrBitmapIdx++;
            if (m_nCurrBitmapIdx >= 15)
                m_nCurrBitmapIdx = 0;

            try
            {
                //Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);   //// 获取图像参数  
                //int stride = bmpData.Stride;  // 扫描线的宽度    
                IntPtr iptr = bmpData.Scan0;  // 获取bmpData的内存起始位置  
                //int scanBytes = stride * height;// 用stride宽度，表示这是内存区域的大小                  用Marshal的Copy方法，将刚才得到的内存字节数组复制到BitmapData中  
                System.Runtime.InteropServices.Marshal.Copy(rawValues, 0, iptr, width * height * 3);
                bmp.UnlockBits(bmpData);  // 解锁内存区域 
                //// 算法到此结束，返回结果  
                return bmp;
            }
            catch (System.Exception e)
            {
                //Tools.m_CreateLogTxt("ToColorBitmap2", e.ToString(), Index);
                return null;
            }
        }
        private byte[] Invert_Data(System.Drawing.Bitmap bmp)
        {
            int bytes = bmp.Width * bmp.Height * 4;

            byte[] rgbvalues = new byte[bytes];
            if (bmp != null)
            {
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);//Format32bppArgb
                System.Drawing.Imaging.BitmapData bmpdata = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);// bmp.PixelFormat);// bmp.PixelFormat);
                IntPtr ptr = bmpdata.Scan0;

                Marshal.Copy(ptr, rgbvalues, 0, bytes);
            }
            return rgbvalues;
        }
        public byte[] Invert_Data_(Bitmap bmp)
        {
            int bytes = bmp.Width * bmp.Height * 3;
            byte[] rgbvalues = new byte[bytes];
            if (bmp != null)
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData bmpdata = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
                IntPtr ptr = bmpdata.Scan0;

                Marshal.Copy(ptr, rgbvalues, 0, bytes);
            }
            return rgbvalues;
        }
        /// 将图片Image转换成Byte[]
        /// </summary>
        /// <param name="Image">image对象</param>
        /// <param name="imageFormat">后缀名</param>
        /// <returns></returns>
        public byte[] ImageToBytes(Image Image, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            if (Image == null) { return null; }
            byte[] data;

            using (System .IO . MemoryStream ms = new System.IO.MemoryStream())
            {
                using (Bitmap Bitmap = new Bitmap(Image))
                {
                    Bitmap.Save(ms, imageFormat);//System.Drawing.Imaging.ImageFormat.Bmp
                    ms.Position = 0;

                    data = ms.GetBuffer();
                    ms.Close();
                    //ms.Read(data, 0, Convert.ToInt32(ms.Length));

                    //ms.Flush();
                }
            }

            return data;
        }

        private void Pic_Rect(Bitmap _Bitmap)
        {
            Pic_L.Image = _Bitmap;
            return;
            Image pic = GetThumbnail(_Bitmap,
                                             SysInfo.m_i_Curr_H, (int)((float)(1.0f * SysInfo.m_iW_iMG / SysInfo.m_iH_iMG) * SysInfo.m_i_Curr_H));
            if (SysInfo.m_i_Curr_H <= SysInfo.m_iH_iMG)
                Pic_L.Image = pic;
            else
            {
                Rectangle _Rect_Kd = new Rectangle((m_i_Curr_X), (m_i_Curr_Y), (int)((float)(1.0f * SysInfo.m_iW_iMG / SysInfo.m_iH_iMG) * SysInfo.m_iH_iMG / m_iAdd), SysInfo.m_iH_iMG / m_iAdd);
                pic=GetRect(pic, _Rect_Kd);
            //    Pho_Video_Usb.Right 
                Pic_L.Image = pic;//  SysInfo.m_iW_iMG  SysInfo.m_iH_iMG
            }
        }

        /// <summary>
        /// 实时随动放大镜功能
        /// </summary>
        /// <param name="i_x">鼠标的X坐标</param>
        /// <param name="i_y">鼠标的Y坐标</param>
        /// <param name="offset">放大镜的偏移量</param>
        /// <param name="select_shape">选择方形显示还是圆形显示：0:圆形 1:方形</param>
        /// <param name="pic_original">需要放大的图形</param>
        /// <param name="pic_display">需要显示的图层</param>
        public static void Zoom_Tool_(int i_x, int i_y, int offset, int select_shape, PictureBox pic_original, PictureBox pic_display)
        {
            if (pic_original.Image == null) return;//chendawei 190731 
            int originalWidth = pic_original.Image.Width;
            int originalHeight = pic_original.Image.Height;

            PropertyInfo rectangleProperty = pic_original.GetType().GetProperty("ImageRectangle", BindingFlags.Instance | BindingFlags.NonPublic);
            Rectangle rectangle = (Rectangle)rectangleProperty.GetValue(pic_original, null);

            int currentWidth = rectangle.Width;
            int currentHeight = rectangle.Height;

            double rate = (double)currentHeight / (double)originalHeight;    //图片缩放比率
            double rate1 = (double)currentWidth / (double)originalWidth;

            double original_x = (double)i_x / rate1;  //鼠标在缩放图片中的坐标
            double original_y = (double)i_y / rate;


            pic_original.Refresh();
            Graphics graphics = pic_display.CreateGraphics();     //实例化pictureBox1控件的Graphics类
            Point p = new Point(i_x, i_y);
            p = new Point(i_x + offset, i_y - offset);

            if (select_shape == 0)   //如果需要圆形画布，需要现在创建一个圆形区域，后续会再此区域中绘制
            {
                GraphicsPath gpath = new GraphicsPath();  //
                gpath.AddEllipse(p.X - 50, p.Y - 50, 100, 100);//添加一个圆形区域
                Region rg = new Region(gpath);
                graphics.Clip = rg;  //设定绘制的区域，以后的绘图都在这个区域内
            }

            //声明两个Rectangle对象，分别用来指定要放大的区域和放大后的区域
            Rectangle sourceRectangle = new Rectangle(Convert.ToInt32(original_x) - 20, Convert.ToInt32(original_y) - 20, 40, 40);  //要放大的区域 
            Rectangle destRectangle = new Rectangle(p.X - 70, p.Y - 70, 140, 140);
            //调用DrawImage方法对选定区域进行重新绘制，以放大该部分
            graphics.DrawImage(pic_original.Image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
            //Brush bush = new SolidBrush(Color.Green);//填充的颜色
            //graphics.FillEllipse(bush, 10, 10, 100, 100);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50

            Pen pen = Pens.Yellow;  //描边，画轮廓
            graphics.DrawEllipse(pen, p.X - 2, p.Y - 2, 4, 4);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50
        }

        /// <summary>
        /// 实时随动放大镜功能
        /// </summary>
        /// <param name="i_x">鼠标的X坐标</param>
        /// <param name="i_y">鼠标的Y坐标</param>
        /// <param name="offset">放大镜的偏移量</param>
        /// <param name="select_shape">选择方形显示还是圆形显示：0:圆形 1:方形</param>
        /// <param name="pic_original">需要放大的图形</param>
        /// <param name="pic_display">需要显示的图层</param>
        public static void Zoom_Tool(int i_x, int i_y, bool blShowUp, int offset, int select_shape, PictureBox pic_original, PictureBox pic_display)
        {
            try
            {
                if (pic_original.Image == null) return;//chendawei 190731 
                int originalWidth = pic_original.Image.Width;
                int originalHeight = pic_original.Image.Height;

                PropertyInfo rectangleProperty = pic_original.GetType().GetProperty("ImageRectangle", BindingFlags.Instance | BindingFlags.NonPublic);
                Rectangle rectangle = (Rectangle)rectangleProperty.GetValue(pic_original, null);

                int currentWidth = rectangle.Width;
                int currentHeight = rectangle.Height;

                double rate = (double)currentHeight / (double)originalHeight;    //图片缩放比率
                double rate1 = (double)currentWidth / (double)originalWidth;

                double original_x = (double)i_x / rate1;  //鼠标在缩放图片中的坐标
                double original_y = (double)i_y / rate;


                pic_original.Refresh();
                Graphics graphics = pic_display.CreateGraphics();     //实例化pictureBox1控件的Graphics类
                if (blShowUp)
                {
                    string _strT = pic_display.Name.ToUpper();
                    if (_strT.IndexOf("R") > 0)
                        i_x = 10;
                    else
                        i_x = pic_display.Width - 120;
                    i_y = 120;
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
                p = new Point(i_x + offset, i_y - offset);

                if (select_shape == 0)   //如果需要圆形画布，需要现在创建一个圆形区域，后续会再此区域中绘制
                {
                    GraphicsPath gpath = new GraphicsPath();  //
                    gpath.AddEllipse(p.X - 60, p.Y - 60, 110, 110);//添加一个圆形区域
                    Region rg = new Region(gpath);
                    graphics.Clip = rg;  //设定绘制的区域，以后的绘图都在这个区域内
                }

                //声明两个Rectangle对象，分别用来指定要放大的区域和放大后的区域
                Rectangle sourceRectangle = new Rectangle(Convert.ToInt32(original_x) - 20, Convert.ToInt32(original_y) - 20, 40, 40);  //要放大的区域 
                Rectangle destRectangle = new Rectangle(0,0, 240, 240);// Rectangle(p.X - 240, p.Y - 240, 240, 240);
                //调用DrawImage方法对选定区域进行重新绘制，以放大该部分
                graphics.DrawImage(pic_original.Image, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                //Brush bush = new SolidBrush(Color.Green);//填充的颜色
                //graphics.FillEllipse(bush, 10, 10, 100, 100);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50

                //Pen pen = Pens.Yellow;  //描边，画轮廓
                //graphics.DrawEllipse(pen, p.X - 2, p.Y - 2, 4, 4);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50
            }
            catch (Exception ex)
            {  }
        }

        private int GetCent(int iData ,int iType=0)
        {
            int _iW_05 = (int)(((float)(1.0f * SysInfo.m_iW_iMG / SysInfo.m_iH_iMG) * SysInfo.m_iH_iMG / 3f)/2f);
            int _iH_05 =(int)(( SysInfo.m_iH_iMG / 3f)/2f);

            int _iRet = 0;
            if(iType ==0)
            {
                if (iData < _iW_05)
                    _iRet = iData;
                if (this.Width - iData < _iW_05)
                    _iRet = this.Width - (int)(2f * _iW_05);
            }
            else
            {
                if (iData < _iH_05)
                    _iRet = iData;
                if (this.Height  - iData < _iH_05)
                    _iRet = this.Height - (int)(2f * _iH_05);

            }
            return _iRet;
        }
        public Bitmap GetRect(Image pic, Rectangle Rect)
        {
            //创建图像
            Rectangle drawRect = new Rectangle(0, 0, Rect.Width, Rect.Height);  //绘制整块区域
            Bitmap tmp = new Bitmap(drawRect.Width, drawRect.Height);           //按指定大小创建位图

            //绘制
            Graphics g = Graphics.FromImage(tmp);                   //从位图创建Graphics对象
            g.Clear(Color.FromArgb(0, 0, 0, 0));                    //清空
            g.DrawImage(pic, drawRect, Rect, GraphicsUnit.Pixel);   //从pic的给定区域进行绘制

            return tmp;     //返回构建的新图像
        }
        private void ShowFrm(bool blVal)
        {
            if (blVal==false)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false; //隐藏自己
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true; //隐藏自己
            }

        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        public void CloseMe(int iType = 0)
        {
            if (iType == 0)
                Application.Idle -= Application_Idle;
            SysInfo.Time_Video.Elapsed -= Time_ShowRun;
            Time_Show.Enabled = false;
            SysInfo.m_csInter.INIWriteValue("VIDEO", "HaveRun", "0", Application.StartupPath + "\\database\\SysConfig.ini");
            Application.Exit();
        }

        private void Time_Exit_Tick(object sender, EventArgs e)
        {
            Time_Exit.Enabled = false;
            //if(m_blShowFrm)
            //    ShowFrm(true);
            //if (m_blPhoto)
            //{
            //    g_USB_Video.Photo(m_iBuff_iRows, m_flDistance_X ,0, m_strJyy);
            //    g_Video.Write_One("Photo", "0");//拍照完毕
            //}
            //if(m_blVideo)
            //{
            //    g_USB_Video.Video(int.Parse (m_strVideo_S_E));
            //    g_Video.Write_One("Video", "0");//录像动作停
            //}
            if (m_blstrOut)
                CloseMe();
        }

        private void Frm_Video_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SysInfo .m_Cam_Xm.m_bl_Add_MindCam && MvApi.CameraGrabber_Destroy!=null)
                try
                {
                    MvApi.CameraGrabber_Destroy(m_Grabber);
                }
                catch { }
            Pic_L.MouseWheel -= new MouseEventHandler(Pho_Video_Usb_MouseWhee);
            Application.Idle -= Application_Idle;
            if (SysInfo.m_blIP)
            {
             //   Set_Flag(1);
                SysInfo .m_Cam_Xm.Close();
            }
            if (m_bl_D3D) Cleanup();
            m_Client.Close();
            CloseMe(1);
        }

        private void Bt_Wave_Show_Click(object sender, EventArgs e)
        {
            Bt_LR();
            this.Width = this.Width + (m_blBig ? -49 : 49);
            m_blBig = !m_blBig;
        }
    
        private void button1_Click(object sender, EventArgs e)
        {
            //m_Client.SendDatToServe("1.0", SysInfo.g_Video.iVideo_No.ToString());
            //SysInfo.g_Video.iVideo_No++;
        }

        private void Pho_Video_Usb_Click(object sender, EventArgs e)
        {
            m_iVideo_Color = (m_iVideo_Color == 0) ? 1 : 0;
            //  m_iVideo_Color = (m_iVideo_Color == 0) ? 1 : 0;
            SysInfo.m_i_Curr_H = SysInfo.m_iH_iMG;
            m_iAdd = 1;
            SysInfo.m_i_Select = 3;
            if (SysInfo.mVideo == 1)
            {
             //   Lab_Video_3.Visible = true;
                Lab_Video_1.Visible = false ;
                Lab_Video_4.Visible = false ;
                Lab_Video_2.Visible = false;
                SysInfo.m_iStart_V = 0;
            }
        }

        private void Time_Show_Tick(object sender, EventArgs e)
        {
            Time_Show.Enabled = false; // Show_Video();
            Show_Video();// Show_DllVideo();
            Time_Show.Enabled = true;
        }
       
        private void ShowImage_1(byte[] ImgDate)
        {
            lock (Lock_Video )
            {
                if (Lb_Err.Visible) Lb_Err.Visible = false;
                SysInfo.m_ImgDat = new Image<Bgr, byte>(SysInfo.m_iW_iMG, SysInfo.m_iH_iMG);
                if (ImgDate != null)
                {
                    m_iFrameNum++;
                    if (m_iFrameNum >= 2147483647)
                        m_iFrameNum = 0;
                   
                    if (ImgDate.Length == SysInfo.m_ImgDat.Bytes.Length)
                    {
                        try
                        {
                            SysInfo.m_ImgDat.Bytes = ImgDate;
                            if (ImgDate[0] > 0 && ImgDate[100] > 0 && ImgDate[200] > 0)
                            {
                                //if (false && m_bl_D3D)
                                //    Render(ImgDate);
                                //else
                                    //  Pho_Video_Usb.Image = SysInfo.m_ImgDat.Bitmap;
                                    Pic_Rect(SysInfo.m_ImgDat.Bitmap);
                            }

                        }
                        catch (Exception e4)
                        { }

                        #region 运行时间
                        m_strMsg = SysInfo.m_strVideoMsg + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        if (SysInfo.mVideo == 1)
                            m_strMsg = SysInfo.m_strVideoMsg + "   Rec :" + FormatRunTime((long)DateTime.Now.Subtract(SysInfo.m_strTime).TotalSeconds);  //   DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                        else
                            ShowTi(0, "");

                        Lb_RunMsg.ForeColor = m_iVideo_Color == 1 ? Color.Yellow : Color.Blue;
                        m_drawBrush = new SolidBrush(m_iVideo_Color == 1 ? Color.Yellow : Color.Blue);

                        try
                        {
                            if (Pic_L.Image != null)
                            {
                                using (Graphics graphics = Graphics.FromImage(Pic_L.Image))//   CvBitmap))
                                {
                                    graphics.DrawString(m_strMsg, m_font, m_drawBrush, Bt_Video.Left + Bt_Video.Width + 80, Bt_Video.Top+ Bt_Video.Height  +80);
                                    graphics.Dispose();
                                }
                            }
                        }
                        catch { }
                        #endregion

                        if (SysInfo.mVideo == 1)
                        {
                            ShowTi(1, "");
                            // ShowTi(2, SysInfo.m_strVideoMsg + " " + FormatRunTime((long)DateTime.Now.Subtract(SysInfo.m_strTime).TotalSeconds));  //   DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                
                            
                            try
                            {
                            //   m_LstImgDat.Add(SysInfo.m_ImgDat);
                               

                               SysInfo.g_USB_Video.VideoNet(SysInfo.m_ImgDat);
                            }
                            catch { }
                          
                        }
                        else
                            ShowTi(1, "");

                        //{

                        //if (m_blSave == false && (SysInfo.mVideo == 1 || m_LstImgDat.Count > 0))
                        //{
                        //    if (Thr_LuXiang != null) Thr_LuXiang.Abort();

                        //    Thr_LuXiang = new Thread(new ThreadStart(Threa_AddVideo));
                        //    Thr_LuXiang.Priority = ThreadPriority.Highest;
                        //    Thr_LuXiang.IsBackground = true;
                        //    Thr_LuXiang.Start();
                        //}

                        //else
                        //    ShowTi(2, SysInfo.m_strVideoMsg + " " + DateTime.Now.ToString("yy-MM-dd HH:mm:ss"));//); Lb_RunMsg.Text = SysInfo.m_strVideoMsg + " " + DateTime.Now.ToString("yy-MM-dd HH:mm:ss"); ;// ShowTi(1);
                    }
                    else
                    {
                        #region 运行时间
                        m_strMsg = SysInfo.m_strVideoMsg + " " + DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                        if (SysInfo.mVideo == 1) m_strMsg = SysInfo.m_strVideoMsg + "   Rec :" + FormatRunTime((long)DateTime.Now.Subtract(SysInfo.m_strTime).TotalSeconds);  //   DateTime.Now.ToString("yy-MM-dd HH:mm:ss");

                        Lb_RunMsg.ForeColor = m_iVideo_Color == 1 ? Color.Yellow : Color.Blue;
                        m_drawBrush = new SolidBrush(m_iVideo_Color == 1 ? Color.Yellow : Color.Blue);

                        try
                        {
                            if (Pic_L.Image != null)
                            {
                                using (Graphics graphics = Graphics.FromImage(Pic_L.Image))//   CvBitmap))
                                {
                                    graphics.DrawString(m_strMsg, m_font, m_drawBrush, Bt_Video.Left + Bt_Video.Width + 80, Bt_Video.Top + Bt_Video.Height + 80);
                                    graphics.Dispose();
                                }
                            }
                        }
                        catch { }
                        #endregion
                    }
                }
                else
                    ShowTi(2);
            }
        }
        private void Threa_AddVideo()
        { 
            m_blSave = true;
            while (SysInfo.mVideo == 1 || m_LstImgDat.Count > 0)
            {
                if (m_LstImgDat.Count > 0)
                {
                    SysInfo.g_USB_Video.VideoNet(m_LstImgDat[0]);// SysInfo.m_ImgDat);
                    m_LstImgDat.RemoveAt(0);
                }
                Application.DoEvents();
            }
            m_blSave = false;
        }
        private void ShowImage_Mul(int iNo,byte[] ImgDate,PictureBox picBox)//,int iTitl_Type)
        {
            lock (Lock_Video)
            {
                if (SysInfo.m_iF_B > 2)
                {
                    if (((float)Tb_Lay_Pan.ColumnStyles[0].Width) < 90)
                    {
                        Tb_Lay_Pan.ColumnStyles[0] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100);
                        Tb_Lay_Pan.ColumnStyles[1] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 0);
                    }
                }
                else
                {
                    if (((float)Tb_Lay_Pan.ColumnStyles[0].Width) > 90)
                    {
                        Tb_Lay_Pan.ColumnStyles[0] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 50);
                        Tb_Lay_Pan.ColumnStyles[1] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 50);
                    }
                }

                if (Lb_Err_2.Visible) Lb_Err_2.Visible = false;
                bool _bl_No_Rec = false;
                try
                {
                    if (iNo == 1)
                    {
                        SysInfo.m_iW_iMG_L = SysInfo.m_Cam_Xm.m_iW_iMG_1;
                        SysInfo.m_iH_iMG_L = SysInfo.m_Cam_Xm.m_iH_iMG_1;

                        SysInfo.m_iW_iMG = SysInfo.m_Cam_Xm.m_iW_iMG_1;
                        SysInfo.m_iH_iMG = SysInfo.m_Cam_Xm.m_iH_iMG_1;

                        if (SysInfo.m_ImgDat_L == null || SysInfo.m_ImgDat_L.Width != SysInfo.m_Cam_Xm.m_iW_iMG_1 || SysInfo.m_ImgDat_L.Height != SysInfo.m_Cam_Xm.m_iH_iMG_1)
                            SysInfo.m_ImgDat_L = new Image<Bgr, byte>(SysInfo.m_Cam_Xm.m_iW_iMG_1, SysInfo.m_Cam_Xm.m_iH_iMG_1);

                        //  SysInfo.m_ImgDat_L = new Image<Bgr, byte>(SysInfo .m_Cam_Xm.m_iW_iMG_1, SysInfo .m_Cam_Xm.m_iH_iMG_1);
                        SysInfo.m_ImgDat_L.Bytes = ImgDate;

                        picBox.Image = SysInfo.m_ImgDat_L.Bitmap;

                        if (!SysInfo.m_bl_Qhzy)
                        {
                            if (SysInfo.m_i_Select == 1)//选中前
                            {
                                SysInfo.m_ImgDat = SysInfo.m_ImgDat_L;
                          //      Lab_Video_3.Visible = SysInfo.m_iStart_V <= m_iWaiteTimes && SysInfo.mVideo == 1;
                                _bl_No_Rec = true;
                            }
                        }
                        else
                        {
                            if (SysInfo.m_i_Select == 1)
                            {
                                SysInfo.m_ImgDat = SysInfo.m_ImgDat_L;
                      //          Lab_Video_1.Visible = SysInfo.m_iStart_V <= m_iWaiteTimes && SysInfo.mVideo == 1;
                                _bl_No_Rec = true;
                            }
                        }
                    }
                    else if (iNo == 2)
                    {
                        SysInfo.m_iW_iMG_R = SysInfo.m_Cam_Xm.m_iW_iMG_2;
                        SysInfo.m_iH_iMG_R = SysInfo.m_Cam_Xm.m_iH_iMG_2;

                        SysInfo.m_iW_iMG = SysInfo.m_Cam_Xm.m_iW_iMG_2;
                        SysInfo.m_iH_iMG = SysInfo.m_Cam_Xm.m_iH_iMG_2;

                        if (SysInfo.m_ImgDat_R == null || SysInfo.m_ImgDat_R.Width != SysInfo.m_Cam_Xm.m_iW_iMG_2 || SysInfo.m_ImgDat_R.Height != SysInfo.m_Cam_Xm.m_iH_iMG_2)
                            SysInfo.m_ImgDat_R = new Image<Bgr, byte>(SysInfo.m_Cam_Xm.m_iW_iMG_2, SysInfo.m_Cam_Xm.m_iH_iMG_2);

                        //        SysInfo.m_ImgDat_R = new Image<Bgr, byte>(SysInfo .m_Cam_Xm.m_iW_iMG_2, SysInfo .m_Cam_Xm.m_iH_iMG_2);
                        SysInfo.m_ImgDat_R.Bytes = ImgDate;

                        picBox.Image = SysInfo.m_ImgDat_R.Bitmap;
                        if (!SysInfo.m_bl_Qhzy)
                        {
                            if (SysInfo.m_i_Select == 2)//选中后
                            {
                                SysInfo.m_ImgDat = SysInfo.m_ImgDat_R;
                          //      Lab_Video_4.Visible = SysInfo.m_iStart_V <= m_iWaiteTimes && SysInfo.mVideo == 1;
                                _bl_No_Rec = true;
                            }
                        }
                        else
                        {
                            if (SysInfo.m_i_Select == 2)
                            {
                                SysInfo.m_ImgDat = SysInfo.m_ImgDat_R;
                            //    Lab_Video_2.Visible = SysInfo.m_iStart_V <= m_iWaiteTimes && SysInfo.mVideo == 1;

                                _bl_No_Rec = true;
                            }
                        }
                    }
                    else if (iNo == 3)
                    {
                        SysInfo.m_iW_iMG_M = SysInfo.m_Cam_Xm.m_iW_iMG_3;
                        SysInfo.m_iH_iMG_M = SysInfo.m_Cam_Xm.m_iH_iMG_3;

                        SysInfo.m_iW_iMG = SysInfo.m_Cam_Xm.m_iW_iMG_3;
                        SysInfo.m_iH_iMG = SysInfo.m_Cam_Xm.m_iH_iMG_3;

                        if (SysInfo.m_ImgDat_M == null || SysInfo.m_ImgDat_M.Width != SysInfo.m_Cam_Xm.m_iW_iMG_3 || SysInfo.m_ImgDat_M.Height != SysInfo.m_Cam_Xm.m_iH_iMG_3)
                            SysInfo.m_ImgDat_M = new Image<Bgr, byte>(SysInfo.m_Cam_Xm.m_iW_iMG_3, SysInfo.m_Cam_Xm.m_iH_iMG_3);

                        //  SysInfo.m_ImgDat_M = new Image<Bgr, byte>(SysInfo .m_Cam_Xm.m_iW_iMG_3, SysInfo .m_Cam_Xm.m_iH_iMG_3);
                        SysInfo.m_ImgDat_M.Bytes = ImgDate;

                        picBox.Image = SysInfo.m_ImgDat_M.Bitmap;
                        if (!SysInfo.m_bl_Qhzy)
                        {
                            if (SysInfo.m_i_Select == 4)//选中探头
                            {
                                SysInfo.m_ImgDat = SysInfo.m_ImgDat_M;
                            //    Lab_Video_4.Visible = SysInfo.m_iStart_V <= m_iWaiteTimes && SysInfo.mVideo == 1;
                                _bl_No_Rec = true;
                            }
                        }
                        else
                        {
                            if (SysInfo.m_i_Select == 3)
                            {
                                SysInfo.m_ImgDat = SysInfo.m_ImgDat_M;
                          //      Lab_Video_3.Visible = SysInfo.m_iStart_V <= m_iWaiteTimes && SysInfo.mVideo == 1;
                                _bl_No_Rec = true;
                            }
                        }
                    }
                    else if (iNo == 4)
                    {
                        SysInfo.m_iW_iMG_4 = SysInfo.m_Cam_Xm.m_iW_iMG_4;
                        SysInfo.m_iH_iMG_4 = SysInfo.m_Cam_Xm.m_iH_iMG_4;

                        SysInfo.m_iW_iMG = SysInfo.m_Cam_Xm.m_iW_iMG_4;
                        SysInfo.m_iH_iMG = SysInfo.m_Cam_Xm.m_iH_iMG_4;

                        if (SysInfo.m_ImgDat_4 == null || SysInfo.m_ImgDat_4.Width != SysInfo.m_Cam_Xm.m_iW_iMG_4 || SysInfo.m_ImgDat_4.Height != SysInfo.m_Cam_Xm.m_iH_iMG_4)
                            SysInfo.m_ImgDat_4 = new Image<Bgr, byte>(SysInfo.m_Cam_Xm.m_iW_iMG_4, SysInfo.m_Cam_Xm.m_iH_iMG_4);

                        SysInfo.m_ImgDat_4.Bytes = ImgDate;

                        picBox.Image = SysInfo.m_ImgDat_4.Bitmap;

                        if (SysInfo.m_i_Select == 4)
                        {
                            SysInfo.m_ImgDat = SysInfo.m_ImgDat_4;
                      //      Lab_Video_4.Visible = SysInfo.m_iStart_V <= m_iWaiteTimes && SysInfo.mVideo == 1;
                            _bl_No_Rec = true;
                        }
                    }
                }
                catch { }
                if (ImgDate != null|| m_bl_CS )
                {
                    m_iFrameNum++;
                    if (m_iFrameNum >= 2147483647)
                        m_iFrameNum = 0;

                    if (m_bl_CS || ImgDate.Length > 0 )
                    {
                        #region 运行时间
                        switch (iNo)
                        {
                            case 1:
                                //   case 3:
                                //   case 5:
                                SysInfo.m_strVideoMsg = SysInfo.m_Cam_Xm.m_str_Name_1;// m_iLanguage == 0 ? "前视" : "Front";
                                break;
                            case 3:
                                //  case 7:
                                if (SysInfo.m_bl_Qhzy)
                                    SysInfo.m_strVideoMsg = SysInfo.m_Cam_Xm.m_str_Name_3;// m_iLanguage == 0 ? "左视" : "Left";
                                else
                                    SysInfo.m_strVideoMsg = SysInfo.m_Cam_Xm.m_str_Name_3;// m_iLanguage == 0 ? "打标" : "Marking";
                                break;
                            case 2:
                                //  case 4:
                                // case 6:
                                SysInfo.m_strVideoMsg = SysInfo.m_Cam_Xm.m_str_Name_2;// m_iLanguage == 0 ? "后视" : "Rear";
                                break;
                            case 4:
                                SysInfo.m_strVideoMsg = SysInfo.m_Cam_Xm.m_str_Name_4;// m_iLanguage == 0 ? "右视" : "Right";
                                break;
                        }

                        m_strMsg = SysInfo.m_strVideoMsg + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        if (SysInfo.mVideo == 1&& iNo == SysInfo.m_i_Select) m_strMsg = SysInfo.m_strVideoMsg + "   Rec :" + FormatRunTime((long)DateTime.Now.Subtract(SysInfo.m_strTime).TotalSeconds);  //   DateTime.Now.ToString("yy-MM-dd HH:mm:ss");


                   //     m_strMsg =  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                  //      if (SysInfo.mVideo == 1 && _bl_No_Rec) m_strMsg = SysInfo.m_strVideoMsg + "   Rec :" + FormatRunTime((long)DateTime.Now.Subtract(SysInfo.m_strTime).TotalSeconds);  //   DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                  //      if (SysInfo.mVideo == 1 && _bl_No_Rec) m_strMsg +=  "   Rec :" + FormatRunTime((long)DateTime.Now.Subtract(SysInfo.m_strTime).TotalSeconds);  //   DateTime.Now.ToString("yy-MM-dd HH:mm:ss");


                        //   m_strMsg += " Fps:" + SysInfo .m_Cam_Xm.m_iFrameNum.ToString();
                        Lb_RunMsg.ForeColor = m_iVideo_Color == 1 ? Color.Yellow : Color.Blue;
                        m_drawBrush = new SolidBrush(m_iVideo_Color == 1 ? Color.Yellow : Color.Blue);

                        try
                        {
                            if (Pic_L.Image != null)
                            {
                                using (Graphics graphics = Graphics.FromImage(picBox.Image))//   CvBitmap))
                                {
                                    if(graphics!=null)
                                    {
                                        try
                                        {
                                            graphics.DrawString(m_strMsg, m_font, m_drawBrush, Bt_Video.Left + Bt_Video.Width + 80, Bt_Video.Top + Bt_Video.Height + 80);
                                            graphics.Dispose();
                                        }
                                        catch (Exception ee3)
                                        {
                                        }
                                    }
                                    else
                                    { }
                                }
                            }
                        }
                        catch { }
                        #endregion

                        if (SysInfo.mVideo == 1)
                        {
                            SysInfo.m_iStart_V++;
                            //if(SysInfo.m_iStart_V >2000&&(Lab_Video_1.Visible|| Lab_Video_3.Visible|| Lab_Video_4.Visible))
                            //{
                            //    Lab_Video_1.Visible = false;
                            //    Lab_Video_3.Visible = false;
                            //    Lab_Video_4.Visible = false;
                            //}
                            ShowTi(1, "");

                            try
                            {
                         //       if(SysInfo .g_USB_Video .m_Cam_bl_Qhzy )
                                {
                                    switch (iNo)
                                    {
                                        case 1:
                                            SysInfo.g_USB_Video.VideoNet(iNo-1, SysInfo.m_ImgDat_L);
                                            break;
                                        case 2:
                                            SysInfo.g_USB_Video.VideoNet(iNo - 1, SysInfo.m_ImgDat_R);
                                            break;
                                        case 3:
                                            SysInfo.g_USB_Video.VideoNet(iNo - 1, SysInfo.m_ImgDat_M);
                                            break;
                                        case 4:
                                            SysInfo.g_USB_Video.VideoNet(iNo - 1, SysInfo.m_ImgDat_4);
                                            break;
                                    }
                                }
                  //              else 
                 //               SysInfo.g_USB_Video.VideoNet(SysInfo.m_ImgDat);
                            }
                            catch { }

                        }
                        else
                            ShowTi(1, "");

                    }
                    else
                    {
                        #region 运行时间
                        m_strMsg = SysInfo.m_strVideoMsg + " " + DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                        if (SysInfo.mVideo == 1) m_strMsg = SysInfo.m_strVideoMsg + "   Rec :" + FormatRunTime((long)DateTime.Now.Subtract(SysInfo.m_strTime).TotalSeconds);  //   DateTime.Now.ToString("yy-MM-dd HH:mm:ss");

                        Lb_RunMsg.ForeColor = m_iVideo_Color == 1 ? Color.Yellow : Color.Blue;
                        m_drawBrush = new SolidBrush(m_iVideo_Color == 1 ? Color.Yellow : Color.Blue);

                        try
                        {
                            if (Pic_L.Image != null)
                            {
                                using (Graphics graphics = Graphics.FromImage(Pic_L.Image))//   CvBitmap))
                                {
                                    graphics.DrawString(m_strMsg, m_font, m_drawBrush, Bt_Video.Left + Bt_Video.Width + 80, Bt_Video.Top + Bt_Video.Height + 80);
                                    graphics.Dispose();
                                }
                            }
                        }
                        catch { }
                        #endregion
                    }
                }
                else
                    ShowTi(2);
            }
        }
        public static String FormatRunTime(long runTime)
        {
            if (runTime < 0) return "00:00:00";

            long hour = runTime / 3600;
            long minute = (runTime % 3600) / 60;
            long second = runTime % 60;

            return hour.ToString("00") + ":" + minute.ToString("00") + ":" +
            second.ToString("00");
        }
        /// <summary>
        /// 设置透明按钮样式
        /// </summary>
        private void SetBtnStyle(PictureBox  btn)
        {
            btn.Visible = true;
          //  btn.FlatStyle = FlatStyle.Flat;//样式
            btn.ForeColor = Color.Transparent;//前景
            btn.BackColor = Color.Transparent;//去背景
            //btn.FlatAppearance.BorderSize = 0;//去边线
            //btn.FlatAppearance.MouseOverBackColor = Color.Transparent;//鼠标经过
            //btn.FlatAppearance.MouseDownBackColor = Color.Transparent;//鼠标按下
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (SysInfo.m_blSendServLinkState == false)
            {
                if (Thread_Tcp_Send != null) Thread_Tcp_Send.Abort();
                Thread_Tcp_Send = new Thread(new ThreadStart(SendServe_ReStart));
                Thread_Tcp_Send.IsBackground = true;
                Thread_Tcp_Send.Start();
            }
          //  m_Client.SendDatToServe("19.0", "1");
            return;
            if (SysInfo.g_USB_Video.m_FileName == "")
                SysInfo.g_USB_Video.m_FileName = DateTime.Now.ToString("yyyyMMdd_HHmm");
            //    SysInfo.mVideo = 1;
         //   if(SysInfo.mVideo==1)
                SysInfo.mVideo = SysInfo.mVideo == 0 ? 1 : 0;
            int _iW=0, _iH=0;
            
         //   SysInfo.g_USB_Video.Video(SysInfo.mVideo, SysInfo.m_blIP, SysInfo .m_Cam_Xm.m_iW_iMG, SysInfo .m_Cam_Xm.m_iH_iMG);
            SysInfo.Time_Video. Enabled = SysInfo.mVideo == 1;
            if (SysInfo.Time_Video.Enabled)
                SysInfo.Time_Video.Start();
            else
                SysInfo.Time_Video.Stop();
        }

        private void Time_Video_Tick(object sender, EventArgs e)
        {
             Pic_Video.Visible = !Pic_Video.Visible;
        }

        private void Time_RunTime_Tick(object sender, EventArgs e)
        {
            //

            //Time_RunTime.Enabled = true;
            if (m_LstImgDat.Count > 0)
            {
                SysInfo.g_USB_Video.VideoNet(m_LstImgDat[0]);// SysInfo.m_ImgDat);
                m_LstImgDat.RemoveAt(0);
            }
            Time_RunTime.Enabled = false;
        }

        private void Pho_Video_Usb_MouseMove(object sender, MouseEventArgs e)
        {
            m_i_Curr_X =  e.X;
            m_i_Curr_Y =  e.Y;
            //Pic_Big.Visible = true;
            //Zoom_Tool(e.X, e.Y,true , 90, 1, Pho_Video_Usb, Pic_Big);
            //Pic_Big.Left = e.X + 10;
            //Pic_Big.Top = e.Y - Pic_Big.Height;
        }

        private void Frm_Video_Activated(object sender, EventArgs e)
        {
         
               
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SysInfo .m_iF_B = SysInfo.m_iF_B == 1 ? 2 : 1;
        }

        private void Bt_Wave_Show_Click_1(object sender, EventArgs e)
        {
            Bt_LR();//   Bt_Wave_Show.Image = Img_LR.Images[m_blBig ? 1 : 0];

            this.Width = this.Width + (m_blBig ? -49 : 49);
            m_blBig = !m_blBig;
        }
        private void Bt_LR(int iType = 0)
        {
            try
            {
                if (iType == 0)
                {
                    if (m_Client.m_blLinkServe)
                        Bt_Wave_Show.BackgroundImage = Img_LR.Images[m_blBig ? 1 : 0];//m_blLinkServe
                    else
                        Bt_Wave_Show.BackgroundImage = Img_LR.Images[m_blBig ? 0 : 1];//m_blLinkServe
                }
                else
                {
                    if (m_Client.m_blLinkServe)
                        Bt_Wave_Show.BackgroundImage = Img_LR.Images[m_blBig ? 0 : 1];//m_blLinkServe
                    else
                        Bt_Wave_Show.BackgroundImage = Img_LR.Images[m_blBig ? 1 : 0];//m_blLinkServe
                }
                //  Application.DoEvents();
            }
            catch { }
        }

        private void Time_R_Tick(object sender, EventArgs e)
        {
            SysInfo .m_Cam_Xm.Close();
          
            if (Thread_ShowVideo != null) Thread_ShowVideo.Abort();
            Thread_ShowVideo = new Thread(new ThreadStart(ThreadShow));
            Thread_ShowVideo.IsBackground = true;
            Thread_ShowVideo.Start();
         
            Time_R.Enabled = false;
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraShowSettingPage(m_hCamera, 1);

        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraGrabber_StartLive(m_Grabber);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
                MvApi.CameraGrabber_StopLive(m_Grabber);
        }

        private void buttonSnap_Click(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
            {
                IntPtr Image;
                if (MvApi.CameraGrabber_SaveImage(m_Grabber, out Image, 2000) == CameraSdkStatus.CAMERA_STATUS_SUCCESS)
                {
                    string filename = System.IO.Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory.ToString(),
                        string.Format("{0}.bmp", System.Environment.TickCount));

                    MvApi.CameraImage_SaveAsBmp(Image, filename);

                    MvApi.CameraImage_Destroy(Image);

                    MessageBox.Show(filename);
                }
                else
                {
                    MessageBox.Show("Snap failed");
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_Grabber != IntPtr.Zero)
            {
                tSdkGrabberStat stat;
                MvApi.CameraGrabber_GetStat(m_Grabber, out stat);
                string info = String.Format("| Resolution:{0}*{1} | DispFPS:{2:0.0} | CapFPS:{3:0.0} |",
                    stat.Width, stat.Height, stat.DispFps, stat.CapFps);
                StateLabel.Text = info;
            }
        }
        bool m_bl_CS = false;
        private void button1_Click_2(object sender, EventArgs e)
        {
            m_bl_CS = true;
            if (SysInfo.g_USB_Video.m_FileName == "")
                SysInfo.g_USB_Video.m_FileName = DateTime.Now.ToString("yyyyMMdd_HHmm");


            SysInfo.mVideo = SysInfo.mVideo == 0 ? 1 : 0;
            if(SysInfo .mVideo ==1) button1.Text = "停止录像";
            if (SysInfo.mVideo == 0) button1.Text = "开始录像";
            int _iW = 0, _iH = 0;
            //if (SysInfo.m_blIP)
            // {
           ///   SysInfo.m_Cam_Xm.m_i_FrameNum_Out = 25;
        //    SysInfo.m_iF_B = 1;
            //SysInfo.m_Cam_Xm.m_iW_iMG_1 = 2560;
            //SysInfo.m_Cam_Xm.m_iH_iMG_1 = 1440;
            if (SysInfo.m_iF_B == 1|| SysInfo.m_iF_B == 0)
                SysInfo.g_USB_Video.Video(SysInfo.mVideo, SysInfo.m_Cam_Xm.m_i_FrameNum_Out, SysInfo.m_blIP, SysInfo.m_Cam_Xm.m_iW_iMG_1, SysInfo.m_Cam_Xm.m_iH_iMG_1);
            else if (SysInfo.m_iF_B == 2)
                SysInfo.g_USB_Video.Video(SysInfo.mVideo, SysInfo.m_Cam_Xm.m_i_FrameNum_Out, SysInfo.m_blIP, SysInfo.m_Cam_Xm.m_iW_iMG_2, SysInfo.m_Cam_Xm.m_iH_iMG_2);
            else if (SysInfo.m_iF_B == 3)
                SysInfo.g_USB_Video.Video(SysInfo.mVideo, SysInfo.m_Cam_Xm.m_i_FrameNum_Out, SysInfo.m_blIP, SysInfo.m_Cam_Xm.m_iW_iMG_3, SysInfo.m_Cam_Xm.m_iH_iMG_3);

        }

        private void Pic_R_Click(object sender, EventArgs e)
        {
            SysInfo.m_i_Select = 4;
            if (SysInfo.mVideo == 1)
            {
               // Lab_Video_4.Visible = true;
                Lab_Video_3.Visible = false ;
                Lab_Video_1.Visible = false ;
                Lab_Video_2.Visible = false;
                SysInfo.m_iStart_V = 0;
            }
        }

        private void Pic_Q_Click(object sender, EventArgs e)
        {
            SysInfo.m_i_Select = 1;
            if (SysInfo.mVideo == 1)
            {
               // Lab_Video_1.Visible = true;
                Lab_Video_3.Visible = false ;
                Lab_Video_4.Visible = false ;
                Lab_Video_2.Visible = false;
                SysInfo.m_iStart_V = 0;
            }
        }
        private void Pic_H_Click(object sender, EventArgs e)
        {
            SysInfo.m_i_Select = 2;
            if (SysInfo.mVideo == 1)
            {
             //   Lab_Video_2.Visible = true;
                Lab_Video_3.Visible = false;
                Lab_Video_1.Visible = false;
                Lab_Video_4.Visible = false;
                SysInfo.m_iStart_V = 0;
            }
        }
        private void Lab_Video_4_Click(object sender, EventArgs e)
        {
            if (SysInfo.mVideo == 1)
            {
              //  Lab_Video_4.Visible = true;
                SysInfo.m_iStart_V = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
            if (SysInfo.m_iF_B < 5) SysInfo.m_iF_B++;
            else
                SysInfo.m_iF_B = 1;
        }
    }

    public class TcpClient_UI
    {
        #region tcp通讯属性

        public bool m_blOut = false;
        /// <summary>
        /// 客户端监听线程
        /// </summary>
        private Thread tcpClientThread = null;

        /// <summary>
        /// 连接服务器的客户端
        /// </summary>
        public  TcpClient tcpClient = null;
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
        public TcpClient_UI()
        {
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

            if (strServIp == "") strServIp = "127.0.0.1";
            if (strPort == "") strPort = "48100";// "48100";48111
            try
            {
                m_strServIp = strServIp;
                m_strPort = strPort;

                if (tcpClient != null) tcpClient.Close();
                tcpClient = new TcpClient(strServIp, int.Parse(strPort));

                //延时操作
                if (tcpClient != null)
                {
                //    Application.DoEvents();
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
            int iLen = 0;

            try
            {
                if (tcpClient.Connected)
                {
                    //19.0 客户端类型  1：视频客户端  2：TOFD波形客户端
                    SendDatToServe("19.0", "1");
                    NetworkStream ns = tcpClient.GetStream();
                    while (tcpClient.Connected)
                    {
                        m_blLinkServe = true;
                        //从网络接收的可供读取的数据字节数据
                        iLen = tcpClient.Available;
                        byte[] Data = new byte[iLen];
                        if (iLen > 0)
                        {
                            iLen = ns.Read(Data, 0, iLen);
                            ns.Flush();
                        }
                      //  Application.DoEvents();
                        if (iLen > 0)
                        {
                            try
                            {
                                ParseRecevDat(Encoding.Default.GetString(Data, 0, Data.Length));
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

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="strRetDat"></param>
        /// <returns></returns>
        public bool ParseRecevDat(string strRetDat)
        {
            //数据帧格式：客户端发来：<clie>,数据,</clie>  
            //主程序发的：<main>,数据,</main>
            bool bRt = false;//数据帧是否正常
            string strRet = "";//返回数据
         //   string strForm = "f" + m_iDot.ToString();//数据小数位
 //MessageBox.Show(strRetDat);
            //1 拆包
            string[] sPara = strRetDat.Split('[');
            if (sPara.Length > 3)
            {
                int _iS = strRetDat.IndexOf("/main>");
                strRetDat = strRetDat.Substring(0, _iS + 6);
                sPara = strRetDat.Split('[');
            //    MessageBox.Show("总：" + strRetDat);
            }
            //数据格式：<main> + "[" + 数据 + "[" + </main>  数据格式：数据类型 / 数据内容
            //例如：<main>[2.0/文件名 | 行号  | 位置 | 检验员 | 1]</main>
            // 2.0  / 文件名 | 行号  | 位置 | 检验员 | 1（开始拍照）   
            // 3.0  / 文件名 | 录像开始1/结束0
            // 99: 关闭程序
            if (sPara.Length == 3)
            {
                //2 确认包
                if (sPara[0].ToLower() == S_Head && sPara[2].ToLower() == S_Tail)
                {
                    string[] _sPara_Sub = sPara[1].Split('/');//数据类型 / 数据内容
                 //   MessageBox.Show("数据：" + sPara[1]);
                    if (_sPara_Sub.Length == 2)
                    {
                        if (_sPara_Sub[0] == "0")// 检定
                        {
                            SysInfo.mVideo = 0;
                            SysInfo.Time_Video.Enabled = false;
                            if (SysInfo.Time_Video.Enabled)
                                SysInfo.Time_Video.Start();
                            else
                                SysInfo.Time_Video.Stop();
                        }
                        else if (_sPara_Sub[0] == "1.0" || _sPara_Sub[0] == "1")//数据类型 1.0：获得波形和厚度数据
                        {

                        }
                        else if (_sPara_Sub[0] == "99")//退出程序
                        {
                            //      MessageBox.Show("退出");
                            m_blOut = true;
                        }
                        //else if (_sPara_Sub[0] == "88")//下次继续检定
                        //{
                        //  //       MessageBox.Show("退出后继续 " + SysInfo.g_Video.iVideo_No.ToString ());
                        //    SysInfo.g_Video.Write_One("iVideo_No", SysInfo.g_Video.iVideo_No.ToString());
                        //}
                        else if (_sPara_Sub[0] == "2.0")// 拍照
                        {
                            //2.0/     _223|1|0|D:\数据库\

                            //    MessageBox.Show("拍照: " + _sPara_Sub[1]);
                            //    MessageBox.Show(sPara[1]);
                            _sPara_Sub = _sPara_Sub[1].Split('|');//文件名 | 行号  | 位置 | 路径

                            if (_sPara_Sub.Length > 3)
                            {
                               
                                //       MessageBox.Show("拍照");
                             
                               
                                //   MessageBox.Show("拍照 " + _sPara_Sub[1] + " " + _sPara_Sub[2] + "  " + _sPara_Sub[3]);

                         //       if (SysInfo.m_bl_Qhzy)
                                { string _strNewName= _sPara_Sub[0];

                                    if (SysInfo.m_Cam_Xm.m_blReLink_1 == 0)
                                    {
                                        SysInfo.g_USB_Video.m_FileName = _strNewName + "_1";
                                        SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_L, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                      float.Parse(_sPara_Sub[2]), 0,
                                                      _sPara_Sub[3], _sPara_Sub[3]);
                                    }
                                    if (SysInfo.m_Cam_Xm.m_blReLink_2 == 0)
                                    {
                                        SysInfo.g_USB_Video.m_FileName = _strNewName + "_2";
                                        SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_R, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                      float.Parse(_sPara_Sub[2]), 0,
                                                      _sPara_Sub[3], _sPara_Sub[3]);
                                    }
                                    if (SysInfo.m_Cam_Xm.m_blReLink_3 == 0)
                                    {
                                        SysInfo.g_USB_Video.m_FileName = _strNewName + "_3";
                                        SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_M, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                     float.Parse(_sPara_Sub[2]), 0,
                                                     _sPara_Sub[3], _sPara_Sub[3]);
                                    }
                                    if (SysInfo.m_Cam_Xm.m_blReLink_4 == 0)
                                    {
                                        SysInfo.g_USB_Video.m_FileName = _strNewName + "_4";
                                        SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_4, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                     float.Parse(_sPara_Sub[2]), 0,
                                                     _sPara_Sub[3], _sPara_Sub[3]);
                                    }

                                    /*
                                    switch (SysInfo.m_iF_B)
                                    {
                                        case 0:
                                            if (SysInfo.m_Cam_Xm.m_blReLink_1 == 0)
                                            {
                                                SysInfo.g_USB_Video.m_FileName = _strNewName + "_1";
                                                SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_L, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                              float.Parse(_sPara_Sub[2]), 0,
                                                              _sPara_Sub[3], _sPara_Sub[3]);
                                            }
                                            if (SysInfo.m_Cam_Xm.m_blReLink_2 == 0)
                                            {
                                                SysInfo.g_USB_Video.m_FileName = _strNewName + "_2";
                                                SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_R, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                              float.Parse(_sPara_Sub[2]), 0,
                                                              _sPara_Sub[3], _sPara_Sub[3]);
                                            }
                                            if (SysInfo.m_Cam_Xm.m_blReLink_3 == 0)
                                            {
                                                SysInfo.g_USB_Video.m_FileName = _strNewName + "_3";
                                                SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_M, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                             float.Parse(_sPara_Sub[2]), 0,
                                                             _sPara_Sub[3], _sPara_Sub[3]);
                                            }
                                            if (SysInfo.m_Cam_Xm.m_blReLink_4 == 0)
                                            {
                                                SysInfo.g_USB_Video.m_FileName = _strNewName + "_4";
                                                SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_4, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                             float.Parse(_sPara_Sub[2]), 0,
                                                             _sPara_Sub[3], _sPara_Sub[3]);
                                            }
                                            break;
                                        case 1:
                                            SysInfo.g_USB_Video.m_FileName = _strNewName + "_1";
                                            SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_L, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                          float.Parse(_sPara_Sub[2]), 0,
                                                          _sPara_Sub[3], _sPara_Sub[3]);
                                            SysInfo.g_USB_Video.m_FileName = _strNewName + "_3";
                                            SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_M, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                         float.Parse(_sPara_Sub[2]), 0,
                                                         _sPara_Sub[3], _sPara_Sub[3]);
                                            SysInfo.g_USB_Video.m_FileName = _strNewName + "_4";
                                            SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_4, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                         float.Parse(_sPara_Sub[2]), 0,
                                                         _sPara_Sub[3], _sPara_Sub[3]);
                                            break;
                                        case 2:
                                            SysInfo.g_USB_Video.m_FileName = _strNewName + "_2";
                                            SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_R, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                          float.Parse(_sPara_Sub[2]), 0,
                                                          _sPara_Sub[3], _sPara_Sub[3]);
                                            SysInfo.g_USB_Video.m_FileName = _strNewName + "_3";
                                            SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_M, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                         float.Parse(_sPara_Sub[2]), 0,
                                                         _sPara_Sub[3], _sPara_Sub[3]);
                                            SysInfo.g_USB_Video.m_FileName = _strNewName + "_4";
                                            SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_4, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                         float.Parse(_sPara_Sub[2]), 0,
                                                         _sPara_Sub[3], _sPara_Sub[3]);
                                            break;
                                        case 3:
                                            SysInfo.g_USB_Video.m_FileName = _strNewName + "_3";
                                            SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_M, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                         float.Parse(_sPara_Sub[2]), 0,
                                                         _sPara_Sub[3], _sPara_Sub[3]);
                                            SysInfo.g_USB_Video.m_FileName = _strNewName + "_4";
                                            SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_4, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                         float.Parse(_sPara_Sub[2]), 0,
                                                         _sPara_Sub[3], _sPara_Sub[3]);
                                            break;
                                        case 4:
                                            SysInfo.g_USB_Video.m_FileName = _strNewName + "_1";
                                            SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_L, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                          float.Parse(_sPara_Sub[2]), 0,
                                                          _sPara_Sub[3], _sPara_Sub[3]);
                                            break;
                                        case 5:
                                            SysInfo.g_USB_Video.m_FileName = _strNewName + "_2";
                                            SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat_R, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                          float.Parse(_sPara_Sub[2]), 0,
                                                          _sPara_Sub[3], _sPara_Sub[3]);
                                            break;
                                    }

                                    */
                                }
                                //else
                                //{
                                //    SysInfo.g_USB_Video.m_FileName = _sPara_Sub[0];
                                //    SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                //                               float.Parse(_sPara_Sub[2]), 0,
                                //                               _sPara_Sub[3], _sPara_Sub[3]);
                                //}

                            }
                        }
                        else if (_sPara_Sub[0] == "3.0")// 录像
                        {
                        //    MessageBox.Show("录像: " + _sPara_Sub[1]);
                            _sPara_Sub = _sPara_Sub[1].Split('|');//文件名 |    _223|1|D:\\数据库\\_@@\\|2560&1440
                                                                  //  WaitTime(0.2f);// MessageBox.Show("开始0000000000000 ：" + _sPara_Sub[0] + " ////////// " + _sPara_Sub[1] + " =====");
                            if (_sPara_Sub.Length > 2)
                            {
                        //        MessageBox.Show("录像名: " + _sPara_Sub[0] + " " + (_sPara_Sub[1] == "1" ? "开始" : "结束") + " " + _sPara_Sub);
                                SysInfo.g_USB_Video.m_FileName = _sPara_Sub[0] + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                //        string _strT = _sPara_Sub[3];

                                if (int.Parse(_sPara_Sub[1]) == 0)
                                {
                                    SysInfo.mVideo = 0;
                                    //  SysInfo.Time_Video.Enabled = false;

                                    WaitTime(0.32f);
                                }
                                int _iW = SysInfo.m_iW_iMG;
                                int _iH = SysInfo.m_iH_iMG;
                                //if (_sPara_Sub.Length > 3)
                                //{
                                //    string[] _strP_Sub = _sPara_Sub[3].Split('&');
                                //    if (_strP_Sub.Length > 1)
                                //    {
                                //        _iW = int.Parse(_strP_Sub[0]);
                                //        _iH = int.Parse(_strP_Sub[1]);
                                //    }
                                //}
                                //if (SysInfo.m_blIP)
                                    SysInfo.g_USB_Video.Video(int.Parse(_sPara_Sub[1]), SysInfo.m_Cam_Xm.m_i_FrameNum_Out, SysInfo.m_blIP, _iW, _iH, _sPara_Sub[2]);
                                //else
                                //    SysInfo.g_USB_Video.Video(int.Parse(_sPara_Sub[1]), SysInfo.m_Cam_Xm.m_i_FrameNum_Out, SysInfo.m_blIP, _iW, _iH);
                                //   MessageBox.Show("录像");
                                SysInfo.mVideo = int.Parse(_sPara_Sub[1]);
                                SysInfo.m_iStart_V = 0;
                                SysInfo.g_USB_Video.m_iRun = SysInfo.mVideo;
                                //  SysInfo.Time_Video.Enabled = SysInfo.mVideo == 1;
                                //if (SysInfo.Time_Video.Enabled)
                                //{
                                if (SysInfo.mVideo == 1) SysInfo.m_strTime = DateTime.Now;
                                //    SysInfo.Time_Video.Start();
                                //}
                                //else
                                //    SysInfo.Time_Video.Stop();
                                //if (int.Parse(_sPara_Sub[1]) == 1)
                                //    SysInfo.g_Video.iVideo_No = 0;

                                //if (SysInfo.g_USB_Video.intRun == 1)
                                //{
                                //    SysInfo.g_Video.iVideo_No = 0;
                                //      MessageBox.Show("录像开始");
                                //}
                                //else
                                //{
                                //    string _strT = SysInfo.g_Video.Read_One("iVideo_No");
                                //    if (_strT == "") _strT = "0";
                                //    SysInfo.g_Video.iVideo_No = int.Parse(_strT);
                                //    MessageBox.Show("录像:  " + _strT);
                                //}

                                //if (SysInfo.m_blIP)
                                //    SysInfo.g_USB_Video.Video(int.Parse(_sPara_Sub[1]),25,SysInfo.m_blIP, SysInfo.m_iW_iMG, SysInfo.m_iH_iMG, _strT);
                                //else
                                //{

                                //    SysInfo.g_USB_Video.Video(int.Parse(_sPara_Sub[1]),25, SysInfo.m_blIP, SysInfo.m_iW_iMG, SysInfo.m_iH_iMG, _strT);
                                //}
                                ////   MessageBox.Show("录像");
                                //SysInfo.mVideo = int.Parse(_sPara_Sub[1]);
                                //SysInfo.g_USB_Video.m_iRun = SysInfo.mVideo;
                                //if (SysInfo.mVideo == 1) SysInfo.m_strTime = DateTime.Now;

                                ////    SysInfo.Time_Video.Enabled = SysInfo.mVideo == 1;
                                ////if (SysInfo.Time_Video.Enabled)
                                ////{
                                ////    SysInfo.m_strTime = DateTime.Now;
                                ////    SysInfo.Time_Video.Start();
                                ////}
                                ////else
                                ////    SysInfo.Time_Video.Stop();
                                ////if (int.Parse(_sPara_Sub[1]) == 1)
                                ////    SysInfo.g_Video.iVideo_No = 0;

                                ////if (SysInfo.g_USB_Video.intRun == 1)
                                ////{
                                ////    SysInfo.g_Video.iVideo_No = 0;
                                ////      MessageBox.Show("录像开始");
                                ////}
                                ////else
                                ////{
                                ////    string _strT = SysInfo.g_Video.Read_One("iVideo_No");
                                ////    if (_strT == "") _strT = "0";
                                ////    SysInfo.g_Video.iVideo_No = int.Parse(_strT);
                                ////    MessageBox.Show("录像:  " + _strT);
                                ////}
                            }
                        }
                        else if (_sPara_Sub[0] == "4.0")// 摄像头
                        {
                            SysInfo.m_strVideoMsg = _sPara_Sub[1];
                        }
                        else if (_sPara_Sub[0] == "5.0")// 前后视频  镜头 1：前视 3：探头  2：后视
                        {
                            // MessageBox.Show(_sPara_Sub[1]);
                            string[] _DataArr = _sPara_Sub[1].Split(',');
                            if (_DataArr.Length == 2)
                            {
                                try
                                {
                                    SysInfo.m_iF_B = int.Parse(_DataArr[0]);
                                    SysInfo.m_strVideoMsg = _DataArr[1];
                                }
                                catch { }
                            }
                            //   HLSysInfo.iF_B = SysInfo.m_iF_B;
                            //  MessageBox.Show("录像开始" + SysInfo.m_iF_B);
                        }
                        else if (_sPara_Sub[0] == "6.0")//主程序运行状态 1：运行  0：停止
                        {
                            _sPara_Sub = _sPara_Sub[1].Split('|');//文件名 |
                            if (_sPara_Sub.Length > 1)
                            {
                                SysInfo.g_USB_Video.m_FileName = _sPara_Sub[0];
                                //MessageBox.Show("录像开始" + SysInfo.g_USB_Video.m_FileName + "  " + _sPara_Sub[1]);
                                //
                                SysInfo.g_USB_Video.m_iRun = int.Parse(_sPara_Sub[1]);
                                if (SysInfo.m_blIP == false)
                                    SysInfo.g_USB_Video.StopVal(SysInfo.mVideo == 0);
                                SysInfo.m_iStart_V = 0;
                                SysInfo.mVideo = SysInfo.g_USB_Video.m_iRun;
                                SysInfo.Time_Video.Enabled = SysInfo.mVideo == 1;
                                if (SysInfo.Time_Video.Enabled)
                                {
                                    SysInfo.m_strTime = DateTime.Now;
                                    SysInfo.Time_Video.Start();
                                }
                                else
                                    SysInfo.Time_Video.Stop();

                                if (SysInfo.g_USB_Video.m_iRun == 1)
                                {
                                    // MessageBox.Show("名称" + _sPara_Sub[0] + "  " + SysInfo.g_USB_Video.m_FileName_Old);
                                    if (SysInfo.g_USB_Video.m_FileName != SysInfo.g_USB_Video.m_FileName_Old)  // if (SysInfo.g_USB_Video.JugVideo() == false)
                                    {

                                        if (SysInfo.g_USB_Video.m_FileName_Old != "")
                                        {
                                            if (SysInfo.g_USB_Video.m_FileName != SysInfo.g_USB_Video.m_FileName_Old)
                                                SysInfo.g_USB_Video.Video(0);
                                        }
                                        SysInfo.g_USB_Video.m_FileName_Old = SysInfo.g_USB_Video.m_FileName;
                                        //  if (SysInfo.m_blIP)
                                        SysInfo.g_USB_Video.Video(1, 25, SysInfo.m_blIP, SysInfo.m_iW_iMG, SysInfo.m_iH_iMG);
                                        //else
                                        //    SysInfo.g_USB_Video.Video(1);
                                    }
                                }
                            }
                        }
                        else if (_sPara_Sub[0] == "7.0")//不再发送
                        {
                            SysInfo.m_blSendServLinkState = true;
                        }
                        else if (_sPara_Sub[0] == "11.0")
                        {
                            string[] _DataArr = _sPara_Sub[1].Split(',');
                            if (_DataArr.Length == 1)
                            {
                                try
                                {
                                    if (_DataArr[0].Length == 3)
                                        SysInfo.m_str_ABC = _DataArr[0];
                                }
                                catch { }
                            }
                        }
                    }

                }
            }
            return bRt;
        }
        private void WaitTime(double dbWait)
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
                    if (tcpClient != null && tcpClient.Connected)
                    {
                        string _strSendDat = strKey + "/" + strDat;//发送数据:信息类型/数据
                        m_iRecevo = 0;

                        //2 准备发送
                        if (SendDataToServer(GetSendZhen(_strSendDat)) == false)
                            SendDataToServer(GetSendZhen(_strSendDat));
                    }
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
                         //   Application.DoEvents();
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
                        if (tcpClientThread.IsAlive) tcpClientThread.Abort();//关闭线程
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
            System.Environment.Exit(0);
            //  this.Close();
            Application.Exit();
        }

    }
    public class ClDog
    {
        /// <summary>
        /// 是否有异常现象
        /// </summary>
        public bool blAlarm = false;
        /// <summary>
        /// 异常发现起始时间
        /// </summary>
        public DateTime dtStar = DateTime.Now;
        /// <summary>
        /// 异常持续时长
        /// </summary>
        public int iWaitLen = 3;
        /// <summary>
        /// 异常持续时长超过门限3秒
        /// </summary>
        public bool bl_Dog = false;

    }
}
