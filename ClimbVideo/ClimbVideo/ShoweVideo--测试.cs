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
using HL;//恒力SDK
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using  Emgu.CV;
namespace ClimbVideo
{
    public partial class Frm_Video : Form
    {

        #region 变量
        /// <summary>
        /// 循环读视频
        /// </summary>
        Thread Thread_RunVideo = null;
        Task Task_SaveVideo = null;
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="Value"></param>
        delegate void Delg_ShowTiTl(int iType, string  Value);
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
        public  Thread Thread_ShowVideo = null;
        #region 调用DLL

        /// <summary>
        /// 相机视频
        /// </summary>
        public  Thread Thread_Run_Video = null;
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
        static extern int pair_image_capture(int camera_w, int camare_h, string Str_In_Key_Value, string Ip, string Ip2, string  iPort,int[] _iOutArr);
        //3 抓视频图像
        [DllImport("V_DLL.dll", EntryPoint = "Video", CallingConvention = CallingConvention.Cdecl)]//SetLastError = false)
        static extern int Get_Video(byte[] ImageBuffer_1, byte[] ImageBuffer_2); //static extern int Video(byte[] ImageBuffer_1, int iNo);//, byte[] ImageBuffer_2h);
        //4 视频循环捕捉                                                                         //3 视频
        [DllImport("V_DLL.dll", EntryPoint = "Video", CallingConvention = CallingConvention.Cdecl)]//SetLastError = false)
        static extern void   Video(); //static extern int Video(byte[] ImageBuffer_1, int iNo);//, byte[] ImageBuffer_2h);

        //5 关闭
        [DllImport("V_DLL.dll", EntryPoint = "Set_Flag", CallingConvention = CallingConvention.Cdecl)]
        static extern void Set_Flag(int iType);  //2   0：视频、 1：关闭相机   2：拍照

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
        NetClient.ITS_TTimeRangeParam _lpBuf = new NetClient.ITS_TTimeRangeParam();
        #endregion SDK

        /// <summary>
        /// 视频字体颜色
        /// </summary>
        private int m_iVideo_Color = 0;

        /// <summary>
        /// 接收服务器数据
        /// </summary>
        private  TcpClient_UI m_Client = new TcpClient_UI();
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
        public  int m_iBuff_iRows = 0;
        public  float  m_flDistance_X =0;
        public  string m_strJyy = "";
        /// <summary>
        /// 是否录像
        /// </summary>
        private bool m_blVideo = false;
        /// <summary>
        /// 录像开始还是结束  1:开始  0：结束
        /// </summary>
        private string  m_strVideo_S_E = "0";
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
            HLSysInfo.StartUp();
        }
        private void Time_ShowRun(object sender, System.Timers.ElapsedEventArgs e)
        {
            ShowTi(2);// Pic_Video.Visible = !Pic_Video.Visible;
        }
        private void Frm_Video_Load(object sender, EventArgs e)
        {
            //   ShowFrm(false);

            //1 视频初始化
            //   SysInfo.g_Video.Write_One("strOut", "0");
            SysInfo.m_blIP = SysInfo.m_Cam.JugNetCam();
            SysInfo.g_Video.Write_One("HaveRun", "0");
            m_iLanguage = int.Parse(SysInfo.m_csInter.IniReadDefine("Language", "Choose", "1", Application.StartupPath + "\\database\\SysConfig.ini"));
            SysInfo.Time_Video.Interval = 500;
            //SysInfo.Time_Video.Elapsed -= Time_ShowRun;
            //SysInfo.Time_Video.Elapsed += Time_ShowRun;

            //Time_SaveData.Elapsed -= Time_SaveData_Eng;
            //Time_SaveData.Elapsed += Time_SaveData_Eng;
            if (SysInfo.m_blIP)
            {
                if (Thread_ShowVideo != null) Thread_ShowVideo.Abort();
                Thread_ShowVideo = new Thread(new ThreadStart(ThreadShow));
                Thread_ShowVideo.Priority = ThreadPriority.Highest;
                Thread_ShowVideo.IsBackground = true;
                Thread_ShowVideo.Start();
            }
            else
            {
                SysInfo.g_Video.Init(0);
                Application.Idle -= Application_Idle;
                SysInfo.g_Video.iZP = 99;
                SysInfo.g_USB_Video.m_iType = 0;

                if (SysInfo.g_USB_Video.Init("", 1, "",
                    SysInfo.g_Video.iJK, SysInfo.g_Video.iZP))
                {
                    //   Time_Show.Enabled = true;// 
                    Application.Idle += Application_Idle;

                }
            }
            //2 连接服务器准备通讯
            m_Client.ConnectToServer("", "");
          //  Waite(0.01f);

            Bt_LR(1);

            Time_Exit.Enabled = true;
            m_blShowFrm = true;

            Bt_Wave_Show.Top = Pho_Video_Usb.Height - Bt_Wave_Show.Height + 5;
            Pho_Video_Usb.Height = this.Height;
            SetTran(Pho_Video_Usb, Bt_Wave_Show);

            Tool_Tip(Bt_Wave_Show, "显示/隐藏 键盘");
            //   SetBtnStyle(Bt_Wave_Show);

            SetBtnStyle(Pic_Video);
            Bt_Wave_Show.Parent = Pho_Video_Usb;
            Bt_Wave_Show.BackColor = Color.FromArgb(0, 0, 0, 0);
            Bt_Wave_Show.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 0, 0, 0);
            Bt_Wave_Show.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 0, 0, 0);
        }
        private void ThreadShow()
        {
            Link_Phone();
            while (true) Show_DllVideo();
        }
        private void ShowTi(int iType, string strVal="")
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
                MsgShow(iType,strVal);
            }
        }
        private void MsgShow(int iType, String strVal="")
        {
            try
            {
                return;
                switch (iType)
                {
                    case 0:
                        if(m_iShowVideo>9)
                        Pic_Video.Visible = (m_iShowVideo++ % 10 == 0);
                        break;
                    case 1:
                        if(Pic_Video.Visible)
                        Pic_Video.Visible = strVal==""?false:true;
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
            ShowPicMsg( WBitmap.Bitmap );
            Pho_Video_Usb.BackgroundImage = WBitmap.Bitmap;

            #region 录像
            SysInfo.m_ImgDat = WBitmap;

            if (SysInfo.mVideo == 1)
            {
                SysInfo.g_USB_Video.VideoNet(WBitmap);
              //  Waite(0.6f);
            }
            if (m_Client.tcpClient != null)
            {
                if (m_Client.tcpClient.Connected)//m_Client.m_blLinkServe 
                    m_Client.SendDatToServe("4.0", SysInfo.g_Video.iVideo_No.ToString());
            }
            SysInfo.g_Video.iVideo_No++;
            if (SysInfo.g_Video.iVideo_No >= 2147483647) SysInfo.g_Video.iVideo_No = 0;

            
            if (SysInfo.g_Video.Read_One("strOut") == "1")
            {
                m_blstrOut = true;
                Time_Exit.Enabled = true;
                Application.Idle -= Application_Idle;
                SysInfo.g_Video.Write_One("HaveRun", "0");
                return;
            }
            
            #endregion
            SysInfo.g_Video.Write_One("HaveRun", "1");
        }
       private void ShowPicMsg(Bitmap Map)
        {
            Font font = new Font("黑体", 30, FontStyle.Regular, GraphicsUnit.Millimeter);
            SolidBrush drawBrush = new SolidBrush(m_iVideo_Color == 1 ? Color.Yellow : Color.Blue);

            using (Graphics graphics = Graphics.FromImage(Map))//   CvBitmap))
            {
               
                graphics.DrawString(  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"\r\n"+ (SysInfo.m_iF_B==1? (m_iLanguage==0?"前视": "Front") :(m_iLanguage == 0 ? "后视" : "Back")), font, drawBrush, 10, 5);//:fff
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
            string _strMsg = "", _strMsg2 = "";
            string m_NetP = Application.StartupPath + "\\database\\HardConfig.ini";
            SysInfo.m_iW_iMG = int.Parse(SysInfo.m_csInter.IniReadDefine("Cam", "m_iWith", "2304", m_NetP));// Application.StartupPath + "\\database\\SysConfig.ini"));
            SysInfo.m_iH_iMG = int.Parse(SysInfo.m_csInter.IniReadDefine("Cam", "m_iHeight", "1296", m_NetP));// Application.StartupPath + "\\database\\SysConfig.ini"));


            string _IP = SysInfo.m_csInter.IniReadDefine("Cam", "m_Ip_1", "192.168.1.12", m_NetP);
            string _IP_2 = SysInfo.m_csInter.IniReadDefine("Cam", "m_Ip_2", "192.168.1.13", m_NetP);
            string _Port = SysInfo.m_csInter.IniReadDefine("Cam", "Port_1", "554", m_NetP);

            int m_iCam_Len = SysInfo.m_iW_iMG * SysInfo.m_iH_iMG * 3;
            m_Img_L = new byte[m_iCam_Len];
            m_Img_R = new byte[m_iCam_Len];
            SysInfo.m_ImgDat = new Image<Bgr, byte>(SysInfo.m_iW_iMG, SysInfo.m_iH_iMG);

            //2 打开相机
            string _strPath = Application.StartupPath;
            try
            {
                Set_Flag(1);
            }
            catch (Exception e)
            {

            }
            SetPath(_strPath);
            int[] _ArrOut = new int[2];
            
            m_iLink = pair_image_capture(SysInfo.m_iW_iMG, SysInfo.m_iH_iMG, "", _IP, _IP_2, _Port, _ArrOut);
            switch (_ArrOut[0])
            {
                case -1://1号相机打开失败
                    _strMsg = "1号相机打开失败";
                    break;
                case -11://1号相机rtsp失败
                    _strMsg = "1号相机rtsp失败";
                    break;
                case -12://1号相机打开和rtsp失败
                    _strMsg = "1号相机打开和rtsp失败";
                    break;
                case 0://成功
                    break;
            }
            switch (_ArrOut[1])
            {
                case -1://1号相机打开失败
                    _strMsg2 = "2号相机打开失败";
                    break;
                case -11://1号相机rtsp失败
                    _strMsg2 = "2号相机rtsp失败";
                    break;
                case -12://1号相机打开和rtsp失败
                    _strMsg2 = "2号相机打开和rtsp失败";
                    break;
                case 0://成功
                    break;
            }
            if (_strMsg == "" || _strMsg2 == "")
            {
                if (Thread_RunVideo != null) Thread_RunVideo.Abort();
                Thread_RunVideo = new Thread(new ThreadStart(RunVideo));
                Thread_RunVideo.IsBackground = true;
                Thread_RunVideo.Start();
            }
            m_Client.SendDatToServe("10.0", ((_strMsg.Length + _strMsg2.Length )>0 ? _strMsg + " " + _strMsg2:"1"));
            //if (m_iLink < 0)
            //    MessageBox.Show(_strMsg +" " + _strMsg2);
        }
        /// <summary>
        /// 循环读视频
        /// </summary>
        private void RunVideo()
        {
            Video();
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
        private void SetTran(PictureBox pic_Parents, Button  pic_Son)
        {
            pic_Parents.SendToBack();
            pic_Son.BackColor = Color.Transparent;
            pic_Son.Parent = pic_Parents;
            pic_Son.BringToFront();
        }
        private void Waite(float flWait=1)
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
        private void Show_Video()
        {
            #region 视频
            #region 外部操作
            if (SysInfo.g_Video.Read_One("strOut") == "1")
            {
                m_blstrOut = true;
                Time_Exit.Enabled = true;
                Application.Idle -= Application_Idle;
                SysInfo.g_Video.Write_One("HaveRun", "0");
                return;
            }

            #endregion
            if (SysInfo.g_USB_Video.m_iType == 0)
                SysInfo.g_USB_Video.GetMat(0, m_iVideo_Color, SysInfo.m_strVideoMsg);//0:兰 1：黄

            try
            {
                Pho_Video_Usb.Image = (Bitmap)(SysInfo.g_USB_Video.CvMat.Bitmap);
                if (m_Client.tcpClient != null)
                {
                    if (m_Client.tcpClient.Connected)//m_Client.m_blLinkServe 
                        m_Client.SendDatToServe("4.0", SysInfo.g_Video.iVideo_No.ToString());
                }
                SysInfo.g_Video.iVideo_No++;
                if (SysInfo.g_Video.iVideo_No >= 2147483647) SysInfo.g_Video.iVideo_No = 0;
            }
            catch (Exception e)
            { }
            #endregion
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
            SysInfo.g_Video.Write_One("strOut", "0");
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
            Application.Idle -= Application_Idle;
            m_Client.Close();
            CloseMe(1);
        }

        private void Bt_Wave_Show_Click(object sender, EventArgs e)
        {/*
            设置按钮的backcolor为0，0，0，0 的数字即可，同时设置FlatApperance的BorderSize为0，FlatStyle为Flat，这样该按钮就完全透明了，此种效果常用在用 背景做皮肤，然后在相应位置放一个按钮，控制按钮事件，但不想让用户看到按钮的情况；
            */
            Bt_LR();//   Bt_Wave_Show.Image = Img_LR.Images[m_blBig ? 1 : 0];

            this .Width = this .Width + (m_blBig ? -49 : 49);
            m_blBig = !m_blBig;
        }
        private void Bt_LR(int iType=0)
        {
            try
            {
                if (iType == 0)
                {
                    if (m_Client.m_blLinkServe)
                        Bt_Wave_Show.Image = Img_LR.Images[m_blBig ? 1 : 0];//m_blLinkServe
                    else
                        Bt_Wave_Show.Image = Img_LR.Images[m_blBig ? 3 : 2];//m_blLinkServe
                }
                else
                {
                    if (m_Client.m_blLinkServe)
                        Bt_Wave_Show.Image = Img_LR.Images[m_blBig ? 0 : 1];//m_blLinkServe
                    else
                        Bt_Wave_Show.Image = Img_LR.Images[m_blBig ? 2 : 3];//m_blLinkServe
                }
            }
            catch { }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            m_Client.SendDatToServe("1.0", SysInfo.g_Video.iVideo_No.ToString());
            SysInfo.g_Video.iVideo_No++;
        }

        private void Pho_Video_Usb_Click(object sender, EventArgs e)
        {
            m_iVideo_Color = (m_iVideo_Color == 0) ? 1 : 0;
            //  m_iVideo_Color = (m_iVideo_Color == 0) ? 1 : 0;
        }

        private void Time_Show_Tick(object sender, EventArgs e)
        {
            Time_Show.Enabled = false; // Show_Video();
            Show_DllVideo();
            Time_Show.Enabled = true;
        }
        /// <summary>
        /// 显示DLL视频
        /// </summary>
        private void Show_DllVideo()
        {
           
                #region 外部操作
                //if (m_Client.m_blLinkServe && SysInfo.g_Video.Read_One("strOut") == "1")
                //{
                //  //  MessageBox.Show("退出");
                //    m_blstrOut = true;
                //    Time_Exit.Enabled = true;
                //    Application.Idle -= Application_Idle;
                //    SysInfo.g_Video.Write_One("HaveRun", "0");
                //    return;
                //}
                //if (m_HaveRun == false)
                //{
                //    SysInfo.g_Video.Write_One("HaveRun", "1");
                //    m_HaveRun = true;
                //}
            #endregion

            #region 1 视频显示左窗体
            if (m_iLink == 0)
            {
                m_iVideo = Get_Video(m_Img_L, m_Img_R);
                //  if (SysInfo.m_iF_B == 1)
                ShowImage(SysInfo.m_iF_B == 1 ? m_Img_L : m_Img_R);
                //else
                //    ShowImage(m_Img_R);
            }
        }
        private void ShowImage(byte [] ImgDate)
        {
            SysInfo.m_ImgDat = new Image<Bgr, byte>(SysInfo.m_iW_iMG, SysInfo.m_iH_iMG);
            if (ImgDate != null)
            {
                if (ImgDate.Length == SysInfo.m_ImgDat.Bytes.Length)
                {
                    SysInfo.m_ImgDat.Bytes = ImgDate;
                    try
                    {
                        Pho_Video_Usb.Image = SysInfo.m_ImgDat.Bitmap;
                    }
                    catch (Exception  e4)
                    { }
                    //if(Time_Video.Enabled ==false && SysInfo.mVideo == 1)
                    //   Time_Video.Enabled =true ;
                    if (SysInfo.mVideo == 1)
                    {
                        // ShowTi(0);

                        Lb_RunMsg.Text = SysInfo.m_strVideoMsg + " " + DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                        #region 现在字符
                        //Font font = new Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Millimeter);
                        //SolidBrush drawBrush = new SolidBrush(m_iVideo_Color == 1 ? Color.Yellow : Color.Blue);
                        //Lb_RunMsg.ForeColor = m_iVideo_Color == 1 ? Color.Yellow : Color.Blue;
                        
                        //using (Graphics graphics = Graphics.FromImage(SysInfo.m_ImgDat.Bitmap))//   CvBitmap))
                        //{
                        //    string strT = SysInfo.m_strVideoMsg + " " + DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
                        //    graphics.DrawString(strT, font, drawBrush, 52, 10);//:fff
                        //    graphics.Dispose();
                        //}
                        #endregion
                        #region 与服务器
                        if (m_Client.tcpClient != null)
                        {
                            if (m_Client.tcpClient.Connected)
                            {
                                m_Client.SendDatToServe("4.0", SysInfo.g_Video.iVideo_No.ToString());
                            }
                        }
                        SysInfo.g_Video.iVideo_No++;
                        if (SysInfo.g_Video.iVideo_No >= 2147483647) SysInfo.g_Video.iVideo_No = 0;
                        #endregion
                        try
                        {

                         //   Task_SaveVideo = new Task(() =>
                         //  {
                               SysInfo.g_USB_Video.VideoNet(SysInfo.m_ImgDat);
                         //  });
                         //   Task_SaveVideo.Start();
                        }
                        catch { }
                        #endregion
                    }
                    else
                        ShowTi(1);
                }
            }
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
            if (SysInfo.g_USB_Video.m_FileName == "")
                SysInfo.g_USB_Video.m_FileName = DateTime.Now.ToString("yyyyMMdd_HHmm");
            //    SysInfo.mVideo = 1;
         //   if(SysInfo.mVideo==1)
                SysInfo.mVideo = SysInfo.mVideo == 0 ? 1 : 0;
            int _iW=0, _iH=0;
            if (SysInfo.m_Cam.m_ArrCame!= null)
            {
                _iW = SysInfo.m_Cam.m_ArrCame[0].m_iWith;
                _iH =  SysInfo.m_Cam.m_ArrCame[0].m_iHeight;
            }
          //  SysInfo.mVideo = SysInfo.mVideo == 0 ? 1 : 0;
            SysInfo.g_USB_Video.Video(SysInfo.mVideo, SysInfo.m_blIP, SysInfo.m_iW_iMG, SysInfo.m_iH_iMG);
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
    }

    public class TcpClient_UI
    {
        #region tcp通讯属性

        private bool m_blOut = false;
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
                        else if (_sPara_Sub[0] == "1.0")//数据类型 1.0：获得波形和厚度数据
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
                            //    MessageBox.Show(sPara[1]);
                            _sPara_Sub = _sPara_Sub[1].Split('|');//文件名 | 行号  | 位置 | 检验员 | 1]
                            if (_sPara_Sub.Length > 3)
                            {
                                //     MessageBox.Show("拍照");
                                SysInfo.g_USB_Video.m_FileName = _sPara_Sub[0];
                              //   MessageBox.Show("拍照 " + _sPara_Sub[1] + " " + _sPara_Sub[2] + "  " + _sPara_Sub[3]);
                                SysInfo.g_USB_Video.Photo(SysInfo.m_ImgDat, SysInfo.m_blIP, int.Parse(_sPara_Sub[1]),
                                                           float.Parse(_sPara_Sub[2]), 0,
                                                           _sPara_Sub[3]);

                            }
                        }
                        else if (_sPara_Sub[0] == "3.0")// 录像
                        {
                            _sPara_Sub = _sPara_Sub[1].Split('|');//文件名 |
                          //  WaitTime(0.2f);// MessageBox.Show("开始0000000000000 ：" + _sPara_Sub[0] + " ////////// " + _sPara_Sub[1] + " =====");
                            if (_sPara_Sub.Length > 1)
                            {

                                SysInfo.g_USB_Video.m_FileName = _sPara_Sub[0];

                              
                                if (int.Parse(_sPara_Sub[1]) == 0)
                                {
                                    SysInfo.mVideo = 0;
                                    SysInfo.Time_Video.Enabled = false;
                                   
                                    WaitTime(0.32f);
                                }
                                if (SysInfo.m_blIP)
                                    SysInfo.g_USB_Video.Video(int.Parse(_sPara_Sub[1]), SysInfo.m_blIP, SysInfo.m_iW_iMG, SysInfo.m_iH_iMG);
                                else
                                {
                                    //     MessageBox.Show("准备");
                                    SysInfo.g_USB_Video.Video(SysInfo.mVideo);
                                }
                                SysInfo.mVideo = int.Parse(_sPara_Sub[1]);
                                SysInfo.g_USB_Video.m_iRun = SysInfo.mVideo;
                                SysInfo.Time_Video.Enabled = SysInfo.mVideo == 1;
                                if (SysInfo.Time_Video.Enabled)
                                    SysInfo.Time_Video.Start();
                                else
                                    SysInfo.Time_Video.Stop();
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
                            }
                        }
                        else if (_sPara_Sub[0] == "4.0")// 摄像头
                        {
                            SysInfo.m_strVideoMsg = _sPara_Sub[1];
                        }
                        else if (_sPara_Sub[0] == "5.0")// 前后视频  镜头 1：前视 3：探头  2：后视
                        {
                            SysInfo.m_iF_B = int.Parse(_sPara_Sub[1]);
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
                                SysInfo.mVideo =  SysInfo.g_USB_Video.m_iRun ;
                                SysInfo.Time_Video.Enabled = SysInfo.mVideo == 1;
                                if (SysInfo.Time_Video.Enabled)
                                    SysInfo.Time_Video.Start();
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
                                        if (SysInfo.m_blIP)
                                            SysInfo.g_USB_Video.Video(1, SysInfo.m_blIP, SysInfo.m_iW_iMG, SysInfo.m_iH_iMG);
                                        else
                                            SysInfo.g_USB_Video.Video(1);
                                    }
                                }
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
}
