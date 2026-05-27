using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STARDC.PubFunc;
using STARDC.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Xml;
using Emgu.CV;
using System.Windows.Forms;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Video
{
    public class ClassVideo
    {
        #region 视频相关
        /// <summary>
        /// Ini文件操作
        /// </summary>
        csInterface m_Ini = new csInterface();
        public static string m_SysFileName = Application.StartupPath + "\\database\\SysConfig.ini";//
        /// <summary>
        /// 主程序运行状态 1：运行  0：停止
        /// </summary>
        public int m_iRun = 0;
        /// <summary>
        /// 相机类型0：USB相机 1：网络摄像头
        /// </summary>
        public int m_iType = 0;
        /// <summary>
        /// 网络相机uri   rtsp://
        /// </summary>
        public string m_IP_1 = "192.168.1.2";
        /// <summary>
        /// 网络相机uri   rtsp://
        /// </summary>
        public string m_IP_2 = "192.168.1.3";
        /// <summary>
        /// 视频
        /// </summary>
        public Capture capture;
        /// <summary>
        /// 检定员视频
        /// </summary>
        public Capture capture_CheckMan;
        /// <summary>
        /// 拍照照片
        /// </summary>
        public Bitmap CvBitmap_CheckMan;
        public static Mat CvMat_CheckMan;

        /// <summary>
        /// 0：检测 1：检测员拍照
        /// </summary>
        public int m_iPhone_Type = 0;
        ///// <summary>
        ///// 拍照保存文件路径
        ///// </summary>
        //public  string imageFilePath;

        ///// <summary>
        ///// 录像保存文件路径
        ///// </summary>
        //public  string videoFilePath;
        /// <summary>
        /// 更新CvMat 0无更新 1主窗体更新  2超声页面更新
        /// </summary>
        public int UpDataCvMat;
        /// <summary>
        /// 定时器
        /// </summary>
        public int CvInterval = 100;
        /// <summary>
        /// 拍照照片
        /// </summary>
        public Bitmap CvBitmap;

        /// <summary>
        /// 是否获得有效帧
        /// </summary>
        public bool m_blGetPic = false;
        /// <summary>
        /// 录像
        /// </summary>
        public static VideoWriter videoWriter = null;
        /// <summary>
        /// 4个相机
        /// </summary>
        public bool m_Cam_bl_Qhzy = false;
        /// <summary>
        /// 相机个数
        /// </summary>
        public  int m_Cam_i_Num=1;
        /// <summary>
        /// 相机组是否录像
        /// </summary>
        public  bool[] m_Cam_bl_Video = null;

        public int[] m_Cam_i_W = null;

        public int[] m_Cam_i_H = null;
        /// <summary>
        /// 多个相机开始录像
        /// </summary>
        public  VideoWriter[] m_Cam_Arr_videoWriter = null;
        /// <summary>
        /// 录像是否忙
        /// </summary>
       // public static bool m_blBussy = false;
        /// <summary>
        /// 停止录像
        /// </summary>
        public static bool stopVedio = true;

        public   Mat CvMat
        {
            get { return _CvMat; }
            set { _CvMat = value; }

        }

        private Mat _CvMat;

        public Image<Rgba, byte> m_RgbaImg;

        public Image<Gray, Byte>[] m_imagesHsv;



        /// <summary>
        /// 文件名组合:单位名称_井号_日期
        /// </summary>
        public   string m_FileName = "";
        /// <summary>
        /// 老文件名
        /// </summary>
        public string m_FileName_Old = "";
        /// <summary>
        /// 开始测试0：停止测试 1：开始测试
        /// </summary>
        public int intRun = 0;
        /// <summary>
        /// 开始录像
        /// </summary>
        public bool blBeginV = false;
        /// <summary>
        /// 是否完成检测
        /// </summary>
        public bool blFinish = false;

        /// <summary>
        /// 录像序号
        /// </summary>
        public int iVideo_No = 0;
        /// <summary>
        /// 文件路径 :Video\\单位名称_井号_日期\\
        /// </summary>
        public  static  string m_strPhath = "";
        #endregion
        #region 摄像头相关   
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="strDwmc">单位</param>
        /// <param name="iJingHao">井号</param>
        /// <param name="strJdrq">日期</param>
        public bool Init(string strDwmc, int iJingHao, string strJdrq, int iJK = 1, int iZp = 0)
        {
            bool _blRet = false;
            try
            {
                m_strPhath = Application.StartupPath + "\\Video\\";// + m_FileName;
                if (Directory.Exists(m_strPhath) == false)
                {
                    Directory.CreateDirectory(m_strPhath);
                }
                //iJK = 0;
                //iZp = 1;
                if (capture == null)
                {
                    try
                    {
                        if (m_iType == 0)
                        {//rtsp://192.168.1.13:554/snl/live/1/1/Ux/sido=-Ux/sido=.
                         //capture = new Capture("rtsp://192.168.1.13:554/user=admin&password=&channel=1&stream=0.sdp?");//   "rtsp://192.168.1.13:554/user=admin");// &password=&channel=1&stream=0.sdp?");//(iJK);
                         //capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 640);
                         //capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 480);


                            // capture = new Capture("rtsp://" + "192.168.1.2" + ":" + "554" + "/user=admin&password=&channel=1&stream=0.sdp?");//"rtsp://" + "192.168.1.2" + ":" + "554" + "/user=admin&password=&channel=1&stream=0.sdp?"
                            capture = new Capture(0);
                            //if (iZp != 99)
                            //    capture_CheckMan = new Capture(iZp);
                            //capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 640);
                            //capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 480);
                            if (capture == null)
                            {
                                WaitTime(1);
                                capture = new Capture(iJK);
                            }
                            if (iZp != 99)
                            {
                                capture_CheckMan = new Capture(iZp);
                                if (capture_CheckMan == null)
                                {
                                    WaitTime(1);
                                    capture_CheckMan = new Capture(iZp);
                                }
                            }
                        }
                        else if(m_iType ==1)
                        {
                            capture = new Capture("rtsp://" + m_IP_1 );
                            if (capture == null)
                            {
                                WaitTime(1);
                                capture = new Capture("rtsp://" + m_IP_1);
                            }
                            if (iZp != 99)
                            {
                                capture_CheckMan = new Capture("rtsp://" + m_IP_2);
                                if (capture_CheckMan == null)
                                {
                                    WaitTime(1);
                                    capture_CheckMan = new Capture("rtsp://" + m_IP_2);
                                }
                            }

                        }

                    }
                    catch { }
                }
              
                _blRet = capture.Height > 0;

            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
          //  catch { MessageBox.Show(iJK.ToString ()+ "摄像头控件调用失败,在配置文件将capture值修改为0"); };
            return _blRet;
        }
        public void WaitTime(double dbWait, bool m_blCloseApp = false)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                try
                {
                    if (m_blCloseApp) break;
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > dbWait) break;
                    System.Threading. Thread.Sleep(5);
                }
                catch { break; }
            }
        }
        public void StopVal(bool  blVal)
        {
            stopVedio = blVal;
        }
        /// <summary>
        /// 录像
        /// </summary>
        /// <param name="iType">1:录像 0：停止录像</param>
        /// <returns>0:文件路径不存在 1：开始录像 2：停止录像</returns>
        public int Video(int iType,int iFrameNo=25, bool blIP = false, int Out_Width = 0, int Out_Height = 0,string strPath="")
        {
            int _iRet = -1;
            //m_strPhath = Application.StartupPath + "\\Video\\";// + m_FileName;

            if (strPath == "")
                m_strPhath = Application.StartupPath + "\\Video\\";// + m_FileName;
            else
                m_strPhath = strPath;

            if (Directory.Exists(m_strPhath) == false)
            {
                Directory.CreateDirectory(m_strPhath);
                WaitTime(0.5f);
            }

            if (!Directory.Exists(m_strPhath))
            {
                _iRet = 0;
                MessageBox.Show("录像文件保存的路径不存在，请到系统设置内设置录像文件保存路径!", "警告信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return _iRet;
            }
            if (iType == 1)//录像
            {
                string strVideoPath = m_strPhath + m_FileName + ".avi";// + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".avi"; 
           //     WaitTime(1);                                                //  MessageBox.Show("开始陆续---- ：" +(blIP==false ?"0":"1")+"   " + strVideoPath);
                try
                {
       //             if (m_Cam_bl_Qhzy)
                    {
                        m_Cam_i_W = new int[m_Cam_i_Num];
                        m_Cam_i_H = new int[m_Cam_i_Num];
                        for (int i=0;i<m_Cam_i_Num;i++)
                        {

                            //---
                            m_Cam_bl_Video[i] = m_Ini.IniReadDefine("Cam", "Video_"+(i+1), "1", Application.StartupPath + "\\database\\SysConfig.ini") == "1";
                            string _strP = Application.StartupPath + "\\database\\HardConfig.ini";
                            m_Cam_i_W[i]= int.Parse ( m_Ini.IniReadDefine("Cam", "m_iWith_"+(i+1), "2304", _strP));
                            m_Cam_i_H[i]= int.Parse(m_Ini.IniReadDefine("Cam", "m_iHeight_" + (i + 1), "1296", _strP));
                           
                            if (m_Cam_bl_Video[i] && m_Cam_i_W[i]>100)
                            {
                                m_Cam_Arr_videoWriter[i] = new VideoWriter(m_strPhath + m_FileName + "_" + (i + 1) + ".avi", //文件名                        
                                   4,
                                  iFrameNo, //帧率 25
                                   new Size(m_Cam_i_W[i], //视频宽度
                                   m_Cam_i_H[i]), //视频高度
                                   true);//彩色
                            }
                        }
                    }
                    //else
                    //{
                    //    videoWriter = new VideoWriter(strVideoPath, //文件名                        
                    //              4,
                    //             iFrameNo, //帧率 25
                    //              new Size(Out_Width, //视频宽度
                    //              Out_Height), //视频高度
                    //              true);//彩色
                    //}
                }
                catch (Exception e)
                {
                    MessageBox.Show("录像错误：" + e.Message);
                }
                // MessageBox.Show("录像初始化");
                stopVedio = false;
                _iRet = 1;
            }
            else if (iType == 0)//停止录像
            {
                stopVedio = true;
       //         if (m_Cam_bl_Qhzy)
                {
                    for (int i = 0; i < m_Cam_i_Num; i++)
                    {
                        if (m_Cam_bl_Video[i] && m_Cam_Arr_videoWriter[i] != null)
                        {
                            m_Cam_Arr_videoWriter[i].Dispose();
                            m_Cam_Arr_videoWriter[i] = null;
                        }
                    }
                }
                //else
                //{
                //    if (videoWriter != null)
                //    {
                //        try
                //        {
                //            videoWriter.Dispose();
                //            videoWriter = null;
                //            m_Ini.INIWriteValue("Video", "Run_012", "2", m_SysFileName);
                //        }
                //        catch (Exception exo)
                //        { }
                //    }
                //}
                _iRet = 2;
            }
            return _iRet;
        }
        public bool JugVideo()
        {
            bool _blRet = false;
            if (videoWriter == null) return _blRet;
            if (videoWriter.Ptr  ==  IntPtr.Zero) return _blRet;
            return true;
        }
        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="flDeep">深度</param>
        /// <returns>0:文件路径不存在! 2:无视频查看设备 1:拍照完毕</returns>
        public int Photo( Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte>  ImgDa,bool blIP,
            int iRow, float flDistance, int iType = 0,
            string strName = "",string strPath="")
        {
            int _iRet = -1;
            //   MessageBox.Show("1");

            if (strPath == "")
                m_strPhath = Application.StartupPath + "\\Video\\";// + m_FileName;
            else
                m_strPhath = strPath;
            if (Directory.Exists(m_strPhath) == false)
            {
                Directory.CreateDirectory(m_strPhath);
            }
            if (!Directory.Exists(m_strPhath))
            {
                _iRet = 0;//
                MessageBox.Show("Path does not exist: (" + m_strPhath + ")", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return _iRet;
            }
            string strImgPath = m_strPhath + m_FileName + "_" + iRow.ToString() + "_" + flDistance.ToString("f0") + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".jpg";
        //    MessageBox.Show("1----"+ iType.ToString ());
            if (iType == 1)
                strImgPath = m_strPhath + m_FileName + "_" + strName + ".jpg";
            else
                strImgPath = m_strPhath + m_FileName + "_" + iRow.ToString() + "_" + flDistance.ToString("f0") + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss-fff") + ".jpg";

            if (iType == 1)
            {
                if (capture_CheckMan == null)
                {
                    _iRet = 2;
                    //  MessageBox.Show("无视频查看设备!", "警告信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        if (CvBitmap_CheckMan != null)
                        { _iRet = 1; CvBitmap_CheckMan.Save(strImgPath); }
                        else
                            MessageBox.Show("拍照kong !"); 

                    }
                    catch (Exception c)
                    { MessageBox.Show(c.Message); }
                    //  MessageBox.Show("拍照完毕!", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

            }
            else
            {
                if (capture == null && blIP==false )
                {
                    _iRet = 2;
                    MessageBox.Show("No camera found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(ImgDa!=null )
                {
                    _iRet = 1;
                    try
                    {
                        if (blIP)
                            ImgDa.Bitmap.Save(strImgPath);
                        else
                        {
                            if (CvBitmap != null)
                            {
                                CvBitmap.Save(strImgPath);
                            //      MessageBox.Show("拍照完毕!" + strImgPath, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                            else
                                MessageBox.Show("No camera found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception c)
                    { MessageBox.Show(c.Message); }

                }
            }
            return _iRet;
        }
        public Image<Bgr, byte> GetMat( int iCol, string strMsg = "")
        {
  
            Image<Bgr, byte> originalImg=null ;

            return originalImg;
        }
        /// <summary>
        /// 网络视频录像
        /// </summary>
        /// <param name="ImgData"></param>
        public void VideoNet(Image< Bgr, byte> ImgData)
        {
            try
            {

                if ( V_Tj() && videoWriter!=null&& videoWriter.Ptr != IntPtr.Zero)
                {
                    try
                    {
                        if (ImgData != null)
                            videoWriter.Write(ImgData.Mat);
                        //      m_blBussy = false;
                        //    m_Ini.INIWriteValue("Video", "Run_012", "1", m_SysFileName);
                    }
                    catch (Exception e)
                    { };// MessageBox.Show("22" + " " +e.Message); }
                }
            }
            catch (Exception e)
            { }
        }
        public void VideoNet(int iNo, Image<Bgr, byte> ImgData)
        {
            try
            {
                if (iNo > -1 && iNo < m_Cam_i_Num && m_Cam_bl_Video [iNo ])
                {
                    if (V_Tj() && m_Cam_Arr_videoWriter[iNo] != null && m_Cam_Arr_videoWriter[iNo].Ptr != IntPtr.Zero)
                    {
                        try
                        { 
                            if (ImgData != null )
                                m_Cam_Arr_videoWriter[iNo].Write(ImgData.Mat);
                        }
                        catch (Exception e)
                        { };
                    }
                }
            }
            catch (Exception e)
            { }
        }
        /// <summary>
        /// 录像条件
        /// </summary>
        /// <returns></returns>
        public bool  V_Tj()
        {
            return (m_iRun == 1 || !stopVedio);//运行或者手动
        }
        public void   GetMat(float  mVideo, int iCol,string strMsg="")
        {
            try
            {
                if (capture != null)
                {
                    m_blGetPic = false;
                   _CvMat = capture.QueryFrame();
                    Mat frame = new Mat();


                    //---
                    //Image<Hls , byte> hlsImg = new Image<Hls, byte>(_CvMat.Bitmap.Width, _CvMat.Bitmap.Height);//转换为Hls图像
                    //Image<Bgr, Byte> bgrImg = new Image<Bgr, Byte>(_CvMat.Bitmap);
                    //CvInvoke.CvtColor(bgrImg, hlsImg, Emgu.CV.CvEnum.ColorConversion.Bgr2Hls);
                    // m_imagesHsv = hlsImg.Split();
                    //---
                    Image<Rgb, Byte> bgrImg = new Image<Rgb, Byte>(_CvMat.Bitmap);
                    m_RgbaImg = bgrImg.Convert<Rgba, Byte>();//将色彩空间从BGR转


                    //  if (!capture.Retrieve(frame,1))
                    if (_CvMat != null)
                    {
                        //  if (CvMat.ElementSize  > 10)
                        {
                            if (V_Tj() && videoWriter.Ptr != IntPtr.Zero)
                            {
                                videoWriter.Write(_CvMat);
                                m_Ini.INIWriteValue("Video", "Run_012", "1", m_SysFileName);
                            }
                                CvBitmap = new Bitmap(CvMat.Bitmap);
                           
                             m_blGetPic = true;
                            int iDotNum = 1;
                            int _iFalseN_1 = 0, _iFalseN_2 = 0; Color _Incol;
                            for (int iIndex = 0; iIndex < iDotNum; iIndex++)
                            {
                                _Incol = _CvMat.Bitmap .GetPixel(iIndex, 3);
                                if (_Incol.R == 0 && _Incol.G == 0 && _Incol.B == 0)
                                    _iFalseN_1++;
                            }
                            for (int iIndex = 0; iIndex < iDotNum; iIndex++)
                            {
                                _Incol = _CvMat.Bitmap.GetPixel(2, iIndex);
                                if (_Incol.R == 0 && _Incol.G == 0 && _Incol.B == 0)
                                    _iFalseN_2++;
                            }
                            if (_iFalseN_1 == iDotNum && _iFalseN_2 == iDotNum)
                                m_blGetPic = false;

                            Font font = new Font("Arial", 6, FontStyle.Regular, GraphicsUnit.Millimeter);
                            SolidBrush drawBrush = new SolidBrush(iCol == 1 ? Color.Yellow : Color.Blue);
                            
                            using (Graphics graphics = Graphics.FromImage(_CvMat.Bitmap))//   CvBitmap))
                            {
                               // graphics.DrawString(strMsg, font, drawBrush, 0, 5);
                                graphics.DrawString(strMsg, font, drawBrush, 0, 35);//:DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                graphics.Dispose();


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Debug.Write(ex.ToString());
            }
        }
     
        public void GetMat_MyPhone(float flWellDeep, int iCol, string strMsg)
        {
            try
            {
                if (capture_CheckMan != null)
                {
                    CvMat_CheckMan = capture_CheckMan.QueryFrame();
                    if (CvMat_CheckMan != null)
                    {
                        if (!stopVedio && videoWriter.Ptr != IntPtr.Zero)
                            videoWriter.Write(CvMat_CheckMan);
                        CvBitmap_CheckMan = new Bitmap(CvMat_CheckMan.Bitmap);

                        Font font = new Font("Arial", 4, FontStyle.Regular, GraphicsUnit.Millimeter);
                        SolidBrush drawBrush = new SolidBrush(iCol == 0 ? Color.Yellow : Color.Blue);

                        using (Graphics graphics = Graphics.FromImage(CvBitmap_CheckMan))
                        {
                            graphics.DrawString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff \r\n" + strMsg), font, drawBrush, 0, 0);
                            graphics.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Debug.Write(ex.ToString());
            }
        }
        #endregion

        #region 
        /*
         这样显示不会影响对窗体的拖动

[csharp] view plain copy
using System;  
using System.Collections.Generic;  
using System.ComponentModel;  
using System.Data;  
using System.Drawing;  
using System.Linq;  
using System.Text;  
using System.Windows.Forms;  
using Emgu.CV;  
using Emgu.CV.Structure;  
using Emgu.CV.ML;  
using Emgu.Util;  
using System.IO;  
  
namespace 视频读取  
{  
    public partial class Form1 : Form  
    {  
   
        MovieInfo movieInfo;//结构体在最下面  
        int Interval;  
        string video;  
        Timer myTimer;  
        Capture a;  
        int zongg;  
   
        RunTaskDelegate runTask ;  
        public Form1()  
        {  
            InitializeComponent();  
        }  
        delegate void RunTaskDelegate(int seconds); //需要委托   
        private void button1_Click(object sender, EventArgs e)  
        {  
  
            myTimer = new Timer();  
            myTimer.Interval = 1000 / Convert.ToInt32(movieInfo.fps);  
            myTimer.Tick += new EventHandler(MyTimer_Tick);  
            myTimer.Start();  
  
        }  
  
  
        private void MyTimer_Tick(object sender, EventArgs e)  
        {  
            int p = movieInfo.currentFrame + 1;  
            
            if (p >= movieInfo.frameCount)  
            {  
                myTimer.Stop();  
  
                return;  
            }  
            else  
            {  
                runTask = new RunTaskDelegate(PlayVideoFile);  
                runTask.Invoke(1);  
                 
                  
            }  
  
        }  
  
  
        private void Form1_Load(object sender, EventArgs e)  
        {  
              
//读取视频里面的参数  
            video = "1.avi";  
            a = new Emgu.CV.Capture(video);  
            movieInfo = new MovieInfo(video, a);  
            Interval = 1000 / Convert.ToInt32(movieInfo.fps);  
            zongg = movieInfo.frameCount;//总帧数  
  
             
        }  
  
  
  
  
  
        private void PlayVideoFile(int c)//每次显示一帧  
        {  
    
                Image<Bgr, byte> frame = a.QueryFrame();  
                if (frame != null)  
                {  
                    Image<Gray, byte> grayFrame = frame.Convert<Gray, byte>();  
                    pictureBox1.Image = grayFrame.ToBitmap();  
                }      
              
        }  
  
  
    }  
  
    struct MovieInfo//结构体，描述视频里面的内容  
    {  
        public String filename;  
        public int frameCount;  
        public int width;  
        public int height;  
        public int currentFrame;  
        public int fps;  
        public MovieInfo(String filename1, IntPtr capture)  
        {  
            filename = filename1;  
            frameCount = Convert.ToInt32(CvInvoke.cvGetCaptureProperty(capture, Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_COUNT));  
            width = Convert.ToInt32(CvInvoke.cvGetCaptureProperty(capture, Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH));  
            height = Convert.ToInt32(CvInvoke.cvGetCaptureProperty(capture, Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT));  
            currentFrame = Convert.ToInt32(CvInvoke.cvGetCaptureProperty(capture, Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_POS_FRAMES));  
            fps = Convert.ToInt32(CvInvoke.cvGetCaptureProperty(capture, Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FPS));  
        }  
    }  
}  

         */
        #endregion
    }
    public class csInterface
    {
        #region  1 INI文件操作方法
        [DllImport("kernel32")]//, CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]//, CharSet = CharSet.Unicode)]
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
        public void WriteErrorLog(string strPathFileName, string strMsg, bool blSendOrRec)
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

        #endregion 文件操作


     
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
        public void DeleFile(string strPathFile, int iType = 0)
        {
            try
            {
                if (strPathFile == "") return;
                string strPath = "";
                int iS = strPathFile.IndexOf('\\');
                if (iS == 0)
                    strPath = Application.StartupPath;// AppDomain.CurrentDomain.BaseDirectory;
                else
                    strPath = AppDomain.CurrentDomain.BaseDirectory;

                string _strPathFile = strPath + strPathFile;
                if (iType == 1 && strPathFile != "")
                    _strPathFile = strPathFile;
                System.IO.File.Delete(_strPathFile);
            }
            catch (Exception Err)
            {
                MessageBox.Show("Delete Files：" + strPathFile + "error！" + Err.Message);
            }
        }

    }
}
