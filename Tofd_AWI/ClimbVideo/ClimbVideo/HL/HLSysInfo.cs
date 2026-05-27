using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using NetClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HL
{
    public class HLSysInfo
    {

        static System.Windows.Forms.Panel  m_Pannel_Pic;
        public static bool stopVedio = false ;
        /// <summary>
        /// 前视
        /// </summary>
        public static bool stopVedio_1 = false;
        /// <summary>
        /// 后视频
        /// </summary>
        public static bool stopVedio_2 = false;
        /// <summary>
        /// 相机数据
        /// </summary>
        public static int m_iLen = 0;
        /// <summary>
        /// 对应主相机序号 0：测试  1：前置 2：后置
        /// </summary>
        public static int iNo = 0;

        
        /// <summary>
        /// 对应主相机序号 0：测试  1：前置 2：后置
        /// </summary>
        public int m_iNo
        {
            get { return iNo; }
            set { iNo = value; }
        }
        //SysInfo.m_iF_B 
        public static string strIP = "192.168.1.2";
        public string m_strIP
        {
            get { return strIP; }
            set { strIP = value; }
        }
        /// <summary>
        /// 选中相机  镜头 1：前视 3：探头  2：后视
        /// </summary>
        public static int iF_B = 1;
        /// <summary>
        /// 镜头 1：前视 3：探头  2：后视
        /// </summary>
        public string m_iF_B
        {
            get { return strIP; }
            set { strIP = value; }
        }
        /// <summary>
        /// IP
        /// </summary>
       // public  string strIP = "192.168.1.2";
        /// <summary>
        /// 故障信息
        /// </summary>
        public string strErrMsg = "";
        /// <summary>
        /// 用户名
        /// </summary>
        public static string strUser = "admin";
        public string m_strUser
        {
            get { return strUser; }
            set { strUser = value; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public static string strPwd = "admin";
        public string m_strPwd
        {
            get { return strPwd; }
            set { strPwd = value; }
        }



      //  public static UInt32 uiConID = UInt32.MaxValue;
        //当前登录状态结构体
        public static CLIENTINFO m_cltInfo;
        //视频窗口对应的连接状态结构体数组
        static CONNECT_STATE[] m_conState;

        private static MAIN_NOTIFY_V4 MainNotify_V40 = null;
       

        private static ALARM_NOTIFY_V4 AlarmNotify_V40 = null;

        public static ITS_TTimeRangeParam its_TTimeRangeParam;

        //启动SDK并初始化
        public static   void StartUp()//System .Windows .Forms .Panel Pannel_Pic)
        {
            //  m_Pannel_Pic = Pannel_Pic;
            //设置客户端和主控端所用的默认网络端口
            NVSSDK.NetClient_SetPort(3000, 6000);// NVSSDK.NetClient_SetPort(3000, 6000);

            //启动SDK
            NVSSDK.NetClient_Startup();

            //初始化NSLook库
            NVSSDK.NSLook_Startup();

            // 设置登陆成功回调
            MainNotify_V40 = MyMAIN_NOTIFY_V4;
            AlarmNotify_V40 = MyAlarm_NOTIFY_V4;            
         //   NVSSDK.NetClient_SetNotifyFunction_V4(MainNotify_V40, AlarmNotify_V40, null, null, null);
            if (m_conState == null)
            {
                m_conState = new CONNECT_STATE[3];
                for (int i = 0; i < m_conState.Length; i++)
                {
                    m_blLink[i] = false;
                    //初始化连接状态结构体
                    m_conState[i].m_iChannelNO = -1;
                    m_conState[i].m_iLogonID = -1;
                    m_conState[i].m_uiConID = UInt32.MaxValue;
                }
            }
        }

        private static void MyMAIN_NOTIFY_V4(UInt32 _ulLogonID, IntPtr _iWparam, IntPtr _iLParam, Int32 _iUser)
        {
            return;
                switch (_iWparam.ToInt32())
            {
                //登陆状态消息 
                //param1 登陆IP
                //param2 登陆ID
                //param3 登陆状态
                case SDKConstMsg.WCM_LOGON_NOTIFY:
                    {
                        //m_conState[m_iCurrentFrame].m_iLogonID = (int)_ulLogonID;
                        switch (_iLParam.ToInt32())
                        {
                            case SDKConstMsg.LOGON_SUCCESS:
                             //   m_blLink[(int)_ulLogonID] = true;
                                //MessageBox.Show("登陆成功！notify_v4");
                                break;
                            case SDKConstMsg.LOGON_TIMEOUT:
                                //MessageBox.Show("登陆超时！notify_v4");
                              //  Close((int)_ulLogonID);
                                break;
                            default:
                             //   Close((int)_ulLogonID);
                                break;
                        }

                        break;
                    }
                default:
                    break;
            }
          
    }

        private static void MyAlarm_NOTIFY_V4(Int32 _ulLogonID, Int32 _iChan, Int32 _iAlarmState, Int32 _iAlarmType, Int32 _iUser)
        {
            return;
            StringBuilder sbAlarmMsg = new StringBuilder("AlarmMsg-", 128);

            sbAlarmMsg.Append(DateTime.Now.ToLocalTime().ToString());

            switch (_iAlarmType)
            {
                case AlarmConstMsgType.ALARM_VDO_MOTION:
                    sbAlarmMsg.Append("- MOTION");
                    break;
                case AlarmConstMsgType.ALARM_VDO_REC:
                    sbAlarmMsg.Append("- REC");
                    break;
                case AlarmConstMsgType.ALARM_VDO_LOST:
                    sbAlarmMsg.Append("- LOST");
                    break;
                case AlarmConstMsgType.ALARM_VDO_INPORT:
                    sbAlarmMsg.Append("- INPORT");
                    break;
                case AlarmConstMsgType.ALARM_VDO_OUTPORT:
                    sbAlarmMsg.Append("- OUTPORT");
                    break;
                case AlarmConstMsgType.ALARM_VDO_COVER:
                    sbAlarmMsg.Append("- COVER");
                    break;
                case AlarmConstMsgType.ALARM_VCA_INFO:
                    sbAlarmMsg.Append("- VCA");
                    break;
                default:
                    sbAlarmMsg.Append("-" + _iAlarmType.ToString());
                    break;
            }

            switch (_iAlarmState)
            {
                case 0:
                    sbAlarmMsg.Append("- OFF");
                    break;
                case 1:
                    sbAlarmMsg.Append("- ON");
                    break;
                default:
                    sbAlarmMsg.Append("-" + _iAlarmState.ToString());
                    break;
            }
        }

        public delegate void OnImageGrabbed_0<B>(B bmp);
        public static  event OnImageGrabbed_0<Emgu.CV.Image<Bgr, byte>> ImageGrabbed_0;

        public delegate void OnImageGrabbed_1<B>(B bmp);
        public static event OnImageGrabbed_1<Emgu.CV.Image<Bgr, byte>> ImageGrabbed_1;

        public delegate void OnImageGrabbed_2<B>(B bmp);
        public static event OnImageGrabbed_2<Emgu.CV.Image<Bgr, byte>> ImageGrabbed_2;
        /// <summary>
        /// 当前视频帧数
        /// </summary>
        public static int FrameNumber;

        public static Bitmap bitmapTmp = null;
        public static Image imgTmp = null;
        public static IntPtr hWnd;
        public static IntPtr _decHandle;

        public static  int m_iCanneNo = 0;
        public static Mat CvMat = new Mat();
        public static int FrameHeight;
        /// <summary>
        /// 相机高度
        /// </summary>
        public int m_FrameHeight
        {
            get { return FrameHeight; }
            set { FrameHeight = value; }
        }
        public static   int FrameWidth;
        /// <summary>
        /// 相机宽度
        /// </summary>
        public int m_FrameWidth
        {
            get { return FrameWidth; }
            set { FrameWidth = value; }
        }

        public static int FrameHeight_1;
        /// <summary>
        /// 相机高度
        /// </summary>
        public int m_FrameHeight_1
        {
            get { return FrameHeight_1; }
            set { FrameHeight_1 = value; }
        }
        public static int FrameWidth_1;
        /// <summary>
        /// 相机宽度
        /// </summary>
        public int m_FrameWidth_1
        {
            get { return FrameWidth_1; }
            set { FrameWidth_1 = value; }
        }


        public static int FrameHeight_2;
        /// <summary>
        /// 相机高度
        /// </summary>
        public int m_FrameHeight_2
        {
            get { return FrameHeight_2; }
            set { FrameHeight_2 = value; }
        }
        public static int FrameWidth_2;
        /// <summary>
        /// 相机宽度
        /// </summary>
        public int m_FrameWidth_2
        {
            get { return FrameWidth_2; }
            set { FrameWidth_2 = value; }
        }
        public static   void Load( int m_iCurrentFrame)
        {
            string strProxy = "";
            string strProxyID = "";
            int iPort = 3000;
           
            //登录指定的网络视频服务器
            int iRet = NVSSDK.NetClient_Logon(strProxy, strIP, strUser, strPwd, strProxyID, iPort);
            if (iRet < 0)
            {
                m_cltInfo.m_iServerID = -1;
                //MessageBox.Show("Logon failed !");
                return;
            }
            m_cltInfo.m_iServerID = iRet;

            m_cltInfo.m_iChannelNo = 0;// iNo;//Remote host to be connected video channel number (Begin from 0)
            m_cltInfo.m_iNetMode = 1;//Select net mode 1--TCP  2--UDP  3--Multicast
            m_cltInfo.m_iStreamNO = 0;//Stream type

            m_cltInfo.m_cNetFile = new char[255];
            m_cltInfo.m_cRemoteIP = new char[16];

            Array.Copy(strIP.ToCharArray(), m_cltInfo.m_cRemoteIP, strIP.Length);
            UInt32 uiConID = m_conState[m_iCurrentFrame].m_uiConID;
            //获得当前窗口对应的视频播放状态
         
            iRet = NVSSDK.NetClient_GetPlayingStatus(uiConID);

            //如果正在播放视频，不进行连接操作
            if (iRet != SDKConstMsg.PLAYER_PLAYING)
            {
                int iChannelNum = 0;
                //Thread.Sleep(500);
                ////获得当前窗口连接的网络视频服务器最大通道数
                NVSSDK.NetClient_GetChannelNum(m_cltInfo.m_iServerID, ref iChannelNum);

                //判断是否超过最大通道号
                if (m_cltInfo.m_iChannelNo >= iChannelNum)
                {
                    //MessageBox.Show("Max Channel is " + iChannelNum);
                    return;
                }

                //改变分辨率
                //iRet = NVSSDK.NetClient_SetVideoSize(uiConID, 0, 0x820, 1);

                Thread.Sleep(200);
                //开始接收一路视频数据	 uiConID:增加相机此数据加一
                iRet = NVSSDK.NetClient_StartRecv(ref uiConID, ref m_cltInfo, null);
               
               // Thread.Sleep(500);
                //RECT rect = new RECT();
                //开始播放某路视频
                //iRet = NVSSDK.NetClient_StartPlay(uiConID, hWnd, rect, 0);
                //操作失败，清除结构体m_conState的信息
                if (iRet < 0)
                {
                    m_conState[m_iCurrentFrame].m_iLogonID = -1;
                    m_conState[m_iCurrentFrame].m_uiConID = UInt32.MaxValue;
                    m_conState[m_iCurrentFrame].m_iChannelNO = -1;
                    //MessageBox.Show("Connect failed !");
                    return;
                }
                //操作成功，更新结构体m_conState的信息
                m_conState[m_iCurrentFrame].m_iLogonID = m_cltInfo.m_iServerID;
                m_conState[m_iCurrentFrame].m_iChannelNO = m_cltInfo.m_iChannelNo;
                m_conState[m_iCurrentFrame].m_uiConID = uiConID;
                m_conState[m_iCurrentFrame].m_iStreamNO = m_cltInfo.m_iStreamNO;

                Thread.Sleep(500);
                RECT rect = new RECT()
                {
                    left = 0
                };
                //开始播放某路视频
                NVSSDK.NetClient_StartPlay(uiConID, hWnd, rect, 0);
                switch (m_iCurrentFrame)
                {
                    case 0:
                        cbk_DecYUV0 = SetDecCallBack;
                        iRet = NVSSDK.NetClient_SetDecCallBack(uiConID, cbk_DecYUV0, IntPtr.Zero);
                        break;
                    case 1:
                        cbk_DecYUV1 = SetDecCallBack1;
                        iRet = NVSSDK.NetClient_SetDecCallBack(uiConID, cbk_DecYUV1, IntPtr.Zero);
                        break;
                    case 2:
                        cbk_DecYUV2 = SetDecCallBack2;
                        iRet = NVSSDK.NetClient_SetDecCallBack(uiConID, cbk_DecYUV2, IntPtr.Zero);
                        break;
                }
                //开始播放某路视频
                // NVSSDK.NetClient_StartPlay(uiConID, m_Pannel_Pic .Handle, rect, 0);

                //cbk_DecYUV = SetDecCallBack;
                ////获得yuv数据
                //iRet = NVSSDK.NetClient_SetDecCallBack(uiConID, cbk_DecYUV, IntPtr.Zero);
            }
        }
        /// <summary>
        /// 关闭相机
        /// </summary>
        public static   void Close()
        {
            //停止接受视频数据
            if (m_conState == null) return;
            for (int i = 0; i < m_conState.Count(); i++)
            {
                if (m_conState[i].m_uiConID != UInt32.MaxValue)
                {
                    int iRet = NVSSDK.NetClient_StopCaptureData(m_conState[i].m_uiConID);
                    NVSSDK.NetClient_StopPlay(m_conState[i].m_uiConID);
                    NVSSDK.NetClient_Logoff(m_conState[i].m_iLogonID);
                }
            }
            stopVedio = true;
            stopVedio_1 = true;
            stopVedio_2 = true;
        }
        public static void Close(int iNo)
        {
            //停止接受视频数据
            if (m_conState == null) return;
            if (m_conState[iNo].m_uiConID != UInt32.MaxValue)
            {
                int iRet = NVSSDK.NetClient_StopCaptureData(m_conState[iNo].m_uiConID);
                //停止播放某路视频
                iRet = NVSSDK.NetClient_StopPlay(m_conState[iNo].m_uiConID);//uiConID
            }
        }
        //static uint nStamp = 0;
        // private static DECYUV_NOTIFY cbk_DecYUV = null;
        private static DECYUV_NOTIFY cbk_DecYUV0 = null;
        private static DECYUV_NOTIFY cbk_DecYUV1 = null;
        private static DECYUV_NOTIFY cbk_DecYUV2 = null;
        private static DECYUV_NOTIFY cbk_DecYUV3 = null;

        private static byte[] yuvBuffer = new byte[0];
        private static byte[] yuvBuffer_1 = new byte[0];
        private static byte[] yuvBuffer_2 = new byte[0];
       // private static byte[] rgbBuffer = new byte[0];
        private static Task task1;
        private static Image<Bgr, byte> m_frame;
        private static Image<Bgr, byte> m_frame1;
        private static Image<Bgr, byte> m_frame2;

        public static bool [] m_blLink =new bool[3];

        //public static byte[] bytesTmp;
        private static  void YuvToRgbEx(uint _ulID, IntPtr _pData, int _iLen)
        {
            try
            {
                m_blLink[0] = true;
                task1 = new Task(() =>
                {
                    try
                    {
                        if (stopVedio) return;
                        Marshal.Copy(_pData, yuvBuffer, 0, _iLen);
                        GCHandle handle = GCHandle.Alloc(yuvBuffer, GCHandleType.Pinned);
                        using (Image<Bgr, byte> yuv420p = new Image<Bgr, byte>((int)FrameWidth, ((int)FrameHeight >> 1) * 3, (int)FrameWidth, handle.AddrOfPinnedObject()))
                        {
                            //  if (yuv420p.Data != null)
                            {
                                try
                                {
                                    CvInvoke.CvtColor(yuv420p, m_frame, ColorConversion.Yuv420P2Rgb);
                                }
                                catch { }
                                //  if(ImageGrabbed != null)
                                //   if(frame2.Data .Length == FrameHeight * FrameWidth *3)
                                //switch (_ulID)
                                //{
                                //    case 0:
                                if (m_frame != null && ImageGrabbed_0!=null )
                                {
                                    ImageGrabbed_0(m_frame); //break;
                                    m_blLink[0] = true;
                                }
                                    //    case 1:
                                    //        ImageGrabbed_1(frame2); break;
                                    //    case 2:
                                    //        ImageGrabbed_2(frame2); break;
                                    //}

                                    yuv420p.Dispose();
                            }
                        }
                        if (handle.IsAllocated) handle.Free();

                    }
                    catch (Exception ex)
                    {
                      //  Debug.Write(ex.ToString());
                    }
                }
                );
                task1.Start();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }
        private static void YuvToRgbEx1(uint _ulID, IntPtr _pData, int _iLen)
        {
            try
            {//
                m_blLink[1] = true;
                   task1 = new Task(() =>
                {
                    try
                    {
                       
                        if (stopVedio_1) return;
                        Marshal.Copy(_pData, yuvBuffer_1, 0, _iLen);
                        GCHandle handle = GCHandle.Alloc(yuvBuffer_1, GCHandleType.Pinned);
                        using (Image<Bgr, byte> yuv420p = new Image<Bgr, byte>((int)FrameWidth_1, ((int)FrameHeight_1 >> 1) * 3, (int)FrameWidth_1, handle.AddrOfPinnedObject()))
                        {
                            //  if (yuv420p.Data != null)
                            {
                                try
                                {
                                    CvInvoke.CvtColor(yuv420p, m_frame1, ColorConversion.Yuv420P2Rgb);
                                }
                                catch { }
                                //  if(ImageGrabbed != null)
                                //   if(frame2.Data .Length == FrameHeight * FrameWidth *3)
                                //switch (_ulID)
                                //{
                                //    case 0:\
                                if (m_frame1 != null && ImageGrabbed_1 != null)
                                {
                                    m_blLink[1] = true; 
                                    ImageGrabbed_1(m_frame1); //break;
                                }
                                //    case 1:
                                //        ImageGrabbed_1(frame2); break;
                                //    case 2:
                                //        ImageGrabbed_2(frame2); break;
                                //}

                                yuv420p.Dispose();
                            }
                        }
                        if (handle.IsAllocated) handle.Free();

                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex.ToString());
                    }
                }
                );
                task1.Start();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }
        private static void YuvToRgbEx2(uint _ulID, IntPtr _pData, int _iLen)
        {
            try
            {
                m_blLink[2] = true;
                task1 = new Task(() =>
                {
                    try
                    {
                        if (stopVedio_2) return;
                        Marshal.Copy(_pData, yuvBuffer_2, 0, _iLen);
                        GCHandle handle = GCHandle.Alloc(yuvBuffer_2, GCHandleType.Pinned);
                        using (Image<Bgr, byte> yuv420p = new Image<Bgr, byte>((int)FrameWidth_2, ((int)FrameHeight_2 >> 1) * 3, (int)FrameWidth_2, handle.AddrOfPinnedObject()))
                        {
                            //  if (yuv420p.Data != null)
                            {
                                try
                                {
                                    CvInvoke.CvtColor(yuv420p, m_frame2, ColorConversion.Yuv420P2Rgb);
                                }
                                catch { }
                                //  if(ImageGrabbed != null)
                                //   if(frame2.Data .Length == FrameHeight * FrameWidth *3)
                                //switch (_ulID)
                                //{
                                //    case 0:
                                if (m_frame2 != null && ImageGrabbed_2 != null)
                                {
                                    ImageGrabbed_2(m_frame2); //break;
                                    m_blLink[2] = true;
                                }
                                    //    case 1:
                                    //        ImageGrabbed_1(frame2); break;
                                    //    case 2:
                                    //        ImageGrabbed_2(frame2); break;
                                    //}

                                    yuv420p.Dispose();
                            }
                        }
                        if (handle.IsAllocated) handle.Free();

                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex.ToString());
                    }
                }
                );
                task1.Start();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }
        private static  void SetDecCallBack(uint _ulID, IntPtr _pData, int _iLen, ref FRAME_INFO _pFrameInfo, IntPtr _pContext)
        {
            if (stopVedio) return;
            if (iF_B != 1) return;

            m_iLen = _iLen;
            if (_pFrameInfo.nType == 1)
            {
                try
                {
                    //if (nStamp == _pFrameInfo.nStamp)
                    //    return;
                    //LogManager.WriteLog("", "进=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " _iLen=" + _iLen
                    //   + " nWidth=" + _pFrameInfo.nWidth + " nHeight=" + _pFrameInfo.nHeight + " nStamp=" + _pFrameInfo.nStamp + " nType=" + _pFrameInfo.nType
                    //   + " nFrameRate=" + _pFrameInfo.nFrameRate + " nReserved=" + _pFrameInfo.nReserved);
                    //   nStamp = _pFrameInfo.nStamp;

                    //SetParameters(_pData, _iLen, (int)_pFrameInfo.nWidth, (int)_pFrameInfo.nHeight);
                    FrameHeight = (int)_pFrameInfo.nHeight;
                    FrameWidth = (int)_pFrameInfo.nWidth;

                    if (yuvBuffer.Length == 0)
                    {
                        yuvBuffer = new byte[(int)(_pFrameInfo.nWidth * _pFrameInfo.nHeight * 1.5)];
                      //  rgbBuffer = new byte[FrameWidth * FrameHeight * 3];
                        m_frame = new Image<Bgr, byte>(FrameWidth, FrameHeight);
                    }

                    Task taskTmp = new Task(() =>
                    {
                        if (task1 == null)
                            YuvToRgbEx(_ulID, _pData, _iLen);
                        // while (true)
                        //  {
                        if (task1.IsCompleted)
                        {
                            YuvToRgbEx(_ulID,_pData, _iLen);
                            //    break;
                        }
                        //else
                        //{
                         //  Thread.Sleep(25);
                        //}
                        //  }
                    });
                    taskTmp.Start();
                }
                catch (Exception ex)
                {

                }
            }
        }
        private static void SetDecCallBack1(uint _ulID, IntPtr _pData, int _iLen, ref FRAME_INFO _pFrameInfo, IntPtr _pContext)
        {
            if (stopVedio_1) return;
            if (iF_B != 2) return;

            m_iLen = _iLen;
            if (_pFrameInfo.nType == 1)
            {
                try
                {
                    //if (nStamp == _pFrameInfo.nStamp)
                    //    return;
                    //LogManager.WriteLog("", "进=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " _iLen=" + _iLen
                    //   + " nWidth=" + _pFrameInfo.nWidth + " nHeight=" + _pFrameInfo.nHeight + " nStamp=" + _pFrameInfo.nStamp + " nType=" + _pFrameInfo.nType
                    //   + " nFrameRate=" + _pFrameInfo.nFrameRate + " nReserved=" + _pFrameInfo.nReserved);
                    //   nStamp = _pFrameInfo.nStamp;

                    //SetParameters(_pData, _iLen, (int)_pFrameInfo.nWidth, (int)_pFrameInfo.nHeight);
                    FrameHeight_1 = (int)_pFrameInfo.nHeight;
                    FrameWidth_1 = (int)_pFrameInfo.nWidth;

                    if (yuvBuffer_1.Length == 0)
                    {
                        yuvBuffer_1 = new byte[(int)(_pFrameInfo.nWidth * _pFrameInfo.nHeight * 1.5)];
                       // rgbBuffer = new byte[FrameWidth_1 * FrameHeight * 3];
                        m_frame1 = new Image<Bgr, byte>(FrameWidth_1, FrameHeight_1);
                    }

                    Task taskTmp = new Task(() =>
                    {
                        if (task1 == null)
                            YuvToRgbEx1(_ulID, _pData, _iLen);
                        // while (true)
                        //  {
                        if (task1.IsCompleted)
                        {
                            YuvToRgbEx1(_ulID, _pData, _iLen);
                            //    break;
                        }
                        //else
                        //{
                        //  Thread.Sleep(25);
                        //}
                        //  }
                    });
                    taskTmp.Start();
                }
                catch (Exception ex)
                {

                }
            }
        }
        private static void SetDecCallBack2(uint _ulID, IntPtr _pData, int _iLen, ref FRAME_INFO _pFrameInfo, IntPtr _pContext)
        {
            if (stopVedio_2) return;
            m_iLen = _iLen;
            if (_pFrameInfo.nType == 1)
            {
                try
                {
                    FrameHeight_2 = (int)_pFrameInfo.nHeight;
                    FrameWidth_2 = (int)_pFrameInfo.nWidth;

                    if (yuvBuffer_2.Length == 0)
                    {

                        yuvBuffer_2 = new byte[(int)(_pFrameInfo.nWidth * _pFrameInfo.nHeight * 1.5)];
                      //  rgbBuffer = new byte[FrameWidth_2 * FrameHeight * 3];
                        m_frame2 = new Image<Bgr, byte>(FrameWidth_2, FrameHeight_2);
                    }

                    Task taskTmp = new Task(() =>
                    {
                        if (task1 == null)
                            YuvToRgbEx2(_ulID, _pData, _iLen);
                        if (task1.IsCompleted)
                            YuvToRgbEx2(_ulID, _pData, _iLen);
                    });
                    taskTmp.Start();
                }
                catch (Exception ex)
                {
                }
            }
        }

        /// <summary>
        /// 叠加汉字
        /// </summary>
        /// <param name="strOSD">叠加字符</param>
        /// <param name="_iEnabled">1：加 0：不加</param>
        /// <param name="iX">x 位置</param>
        /// <param name="iY">y 位置</param>
        public  static void SetOSD( string strOSD, int _iEnabled,int iX=0,int iY=0)
        {
            int iType = 0x02;
            //转化为字符叠加类型码,0x01 叠加时间,0x02 叠加字符串,0x04 叠加LOGO标志 
            NVSSDK.NetClient_SetOsdType(0, 0, iX, iY, iType, _iEnabled);
            //判断是否为空字符串
            strOSD = strOSD == "" ? " " : strOSD;
            UInt32 uiColor = 0;
            //在视频源上叠加一个字符串
            NVSSDK.NetClient_SetOsdText(0, 0, Encoding.Default.GetBytes(strOSD), uiColor);
        }

        public static void SetColor(int iX = 0)
        {
            int iType = 0x02;
            //转化为字符叠加类型码,0x01 叠加时间,0x02 叠加字符串,0x04 叠加LOGO标志 
            //int iX = 0;
            //int iY = 0;
          //  NVSSDK.NetClient_SetOSDTypeColor(0, 0, iX);
        }
    }
}
