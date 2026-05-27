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

namespace DVS.HL
{
    public class HLSysInfo
    {       
        public static bool stopVedio = true;
        public static string strIP = "192.168.1.2";
        public static UInt32 uiConID = UInt32.MaxValue;
        //当前登录状态结构体
        public static CLIENTINFO m_cltInfo;

        private static MAIN_NOTIFY_V4 MainNotify_V40 = null;

        private static ALARM_NOTIFY_V4 AlarmNotify_V40 = null;

        public static ITS_TTimeRangeParam its_TTimeRangeParam;

        //启动SDK并初始化
        public static void StartUp()
        {
            //设置客户端和主控端所用的默认网络端口
            NVSSDK.NetClient_SetPort(3000, 6000);

            //启动SDK
            NVSSDK.NetClient_Startup();

            //初始化NSLook库
            NVSSDK.NSLook_Startup();

            // 设置登陆成功回调
            MainNotify_V40 = MyMAIN_NOTIFY_V4;
            AlarmNotify_V40 = MyAlarm_NOTIFY_V4;            
            NVSSDK.NetClient_SetNotifyFunction_V4(MainNotify_V40, AlarmNotify_V40, null, null, null);           
        }

        private static void MyMAIN_NOTIFY_V4(UInt32 _ulLogonID, IntPtr _iWparam, IntPtr _iLParam, Int32 _iUser)
        {
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
                                //MessageBox.Show("登陆成功！notify_v4");
                                break;
                            case SDKConstMsg.LOGON_TIMEOUT:
                                //MessageBox.Show("登陆超时！notify_v4");
                                break;
                            default:
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

        public delegate void OnImageGrabbed<B>(B bmp);
        public static event OnImageGrabbed<Bitmap> ImageGrabbed;
        /// <summary>
        /// 当前视频帧数
        /// </summary>
        public static int FrameNumber;

        public static Bitmap bitmapTmp = null;
        public static Image imgTmp = null;
        public static IntPtr hWnd;
        public static IntPtr _decHandle;

        public static Mat CvMat = new Mat();
        public static int FrameHeight;
        public static int FrameWidth;
        public static void Load()
        {
            string strProxy = "";
            string strUser = "admin";
            string strPwd = "admin";
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

            m_cltInfo.m_iChannelNo = 0;
            m_cltInfo.m_iNetMode = 1;
            m_cltInfo.m_iStreamNO = 0;

            m_cltInfo.m_cNetFile = new char[255];
            m_cltInfo.m_cRemoteIP = new char[16];

            Array.Copy(strIP.ToCharArray(), m_cltInfo.m_cRemoteIP, strIP.Length);
            Thread.Sleep(500);
            //获得当前窗口对应的视频播放状态
            iRet = NVSSDK.NetClient_GetPlayingStatus(uiConID);

            //如果正在播放视频，不进行连接操作
            if (iRet != SDKConstMsg.PLAYER_PLAYING)
            {
                //int iChannelNum = 0;
                //Thread.Sleep(500);
                ////获得当前窗口连接的网络视频服务器最大通道数
                //iRet = NVSSDK.NetClient_GetChannelNum(m_cltInfo.m_iServerID, ref iChannelNum);

                ////判断是否超过最大通道号
                //if (m_cltInfo.m_iChannelNo >= iChannelNum)
                //{
                //    //MessageBox.Show("Max Channel is " + iChannelNum);
                //    return;
                //}

                //改变分辨率
                //iRet = NVSSDK.NetClient_SetVideoSize(uiConID, 0, 0x820, 1);

                Thread.Sleep(500);
                //开始接收一路视频数据	
                iRet = NVSSDK.NetClient_StartRecv(ref uiConID, ref m_cltInfo, null);

                Thread.Sleep(500);
                RECT rect = new RECT();
                //开始播放某路视频
                iRet = NVSSDK.NetClient_StartPlay(uiConID, hWnd, rect, 0);

                cbk_DecYUV = SetDecCallBack;
                //获得yuv数据
                iRet = NVSSDK.NetClient_SetDecCallBack(uiConID, cbk_DecYUV, IntPtr.Zero);
            }
        }
        
        static uint nStamp = 0;
        private static DECYUV_NOTIFY cbk_DecYUV = null;
        private static byte[] yuvBuffer = new byte[0];
        private static byte[] rgbBuffer = new byte[0];
        private static Task task1;
        private static Image<Bgr, byte> frame2;
        //public static byte[] bytesTmp;
        private static void YuvToRgbEx(IntPtr _pData, int _iLen)
        {
            try
            {
                task1 = new Task(() =>
                {
                    try
                    {
                        Marshal.Copy(_pData, yuvBuffer, 0, _iLen);
                        GCHandle handle = GCHandle.Alloc(yuvBuffer, GCHandleType.Pinned);
                        using (Image<Bgr, byte> yuv420p = new Image<Bgr, byte>((int)FrameWidth, ((int)FrameHeight >> 1) * 3, (int)FrameWidth, handle.AddrOfPinnedObject()))
                        {
                            CvInvoke.CvtColor(yuv420p, frame2, ColorConversion.Yuv420P2Rgb);
                           if(ImageGrabbed != null)
                            ImageGrabbed(frame2.Bitmap);
                            yuv420p.Dispose();
                        }
                        if (handle.IsAllocated) handle.Free();

                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex.ToString());
                    }
                });
                task1.Start();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }       
        private static void SetDecCallBack(uint _ulID, IntPtr _pData, int _iLen, ref FRAME_INFO _pFrameInfo, IntPtr _pContext)
        {
            if (_pFrameInfo.nType == 1)
            {
                try
                {
                    if (nStamp == _pFrameInfo.nStamp)
                        return;
                    //LogManager.WriteLog("", "进=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " _iLen=" + _iLen
                    //   + " nWidth=" + _pFrameInfo.nWidth + " nHeight=" + _pFrameInfo.nHeight + " nStamp=" + _pFrameInfo.nStamp + " nType=" + _pFrameInfo.nType
                    //   + " nFrameRate=" + _pFrameInfo.nFrameRate + " nReserved=" + _pFrameInfo.nReserved);
                    nStamp = _pFrameInfo.nStamp;

                    //SetParameters(_pData, _iLen, (int)_pFrameInfo.nWidth, (int)_pFrameInfo.nHeight);
                    if (yuvBuffer.Length == 0)
                    {
                        FrameHeight = (int)_pFrameInfo.nHeight;
                        FrameWidth = (int)_pFrameInfo.nWidth;

                        yuvBuffer = new byte[(int)(_pFrameInfo.nWidth * _pFrameInfo.nHeight * 1.5)];
                        rgbBuffer = new byte[FrameWidth * FrameHeight * 3];
                        frame2 = new Image<Bgr, byte>(FrameWidth, FrameHeight);
                    }
                    Task taskTmp = new Task(() =>
                    {
                        if (task1 == null)
                            YuvToRgbEx(_pData, _iLen);
                        while (true)
                        {
                            if (task1.IsCompleted)
                            {
                                YuvToRgbEx(_pData, _iLen);
                                break;
                            }
                            else
                            {
                                Thread.Sleep(5);
                            }
                        }
                    });
                    taskTmp.Start();
                    //System.IO.File.WriteAllBytes(DateTime.Now.ToString("HH_mm_ss_fff") + ".YUV", yuvFrame);
                    //LogManager.WriteLog("", "出=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " _iLen=" + _iLen
                    //    + " nWidth=" + _pFrameInfo.nWidth + " nHeight=" + _pFrameInfo.nHeight + " nStamp=" + _pFrameInfo.nStamp + " nType=" + _pFrameInfo.nType
                    //    + " nFrameRate=" + _pFrameInfo.nFrameRate + " nReserved=" + _pFrameInfo.nReserved);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
