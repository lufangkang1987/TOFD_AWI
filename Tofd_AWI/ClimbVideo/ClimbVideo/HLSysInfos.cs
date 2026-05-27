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
    public class HLSysInfos
    {
        [DllImport(@"YUVToRGB.dll", EntryPoint = "init_yuv420p_table", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        extern static void init_yuv420p_table();

        [DllImport(@"YUVToRGB.dll", EntryPoint = "yuv420p_to_rgb24", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        extern static void yuv420p_to_rgb24(byte[] yuvbuffer, byte[] rgbbuffer, int width, int height);

        [DllImport(@"YUVToRGB.dll", EntryPoint = "yuv420p_to_rgb24_Scale", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        extern static void yuv420p_to_rgb24_Scale(byte[] yuvbuffer, byte[] rgbbuffer, byte[] Scalebuffer, int width, int height);

        [DllImport(@"YUVToRGB.dll", EntryPoint = "SetRRGB", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static void SetRRGB(bool bRray, bool bBlue, bool bGreen, bool bRed);

        public static bool stopVedio = true;
        //当前登录状态结构体
        public static CLIENTINFO m_cltInfo;

        //视频窗口对应的连接状态结构体数组
        static CONNECT_STATE[] m_conState;

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

            m_conState = new CONNECT_STATE[5];
            NVSSDK.NetClient_SetNotifyFunction_V4(MainNotify_V40, AlarmNotify_V40, null, null, null);

            for (int i = 0; i < m_conState.Length; i++)
            {
                //初始化连接状态结构体
                m_conState[i].m_iChannelNO = -1;
                m_conState[i].m_iLogonID = -1;
                m_conState[i].m_uiConID = UInt32.MaxValue;
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="P1"></typeparam>
        /// <typeparam name="P2"></typeparam>
        /// <param name="_bmp">图像</param>
        /// <param name="_Pos">相机序号</param>
        public delegate void OnH264Grabbed<P1, P2, P3>(P1 _pData, P2 _iLen, P3 p3);
        public static event OnH264Grabbed<IntPtr, uint, ulong> H264Grabbed0;
        public static event OnH264Grabbed<IntPtr, uint, ulong> H264Grabbed1;
        public static event OnH264Grabbed<IntPtr, uint, ulong> H264Grabbed2;
        public static event OnH264Grabbed<IntPtr, uint, ulong> H264Grabbed3;

        public delegate void OnImageGrabbed<B>(B bmp);
        public static event OnImageGrabbed<Bitmap> ImageGrabbed;

        /// <summary>
        /// 当前视频帧数
        /// </summary>
        public static int FrameNumber;
        public static int FrameHeight;
        public static int FrameWidth;

        public static void Load(string _cIP, string _cUserName, string _cPassword, IntPtr _hWnd, int m_iCurrentFrame)
        {
            string strProxy = "";
            string strProxyID = "";
            int iPort = 3000;
            int iRet;
            //登录指定的网络视频服务器
            iRet = NVSSDK.NetClient_Logon(strProxy, _cIP, _cUserName, _cPassword, strProxyID, iPort);
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
            Array.Copy(_cIP.ToCharArray(), m_cltInfo.m_cRemoteIP, _cIP.Length);
            UInt32 uiConID = m_conState[m_iCurrentFrame].m_uiConID;

            //获得当前窗口对应的视频播放状态
            iRet = NVSSDK.NetClient_GetPlayingStatus(uiConID);

            //如果正在播放视频，不进行连接操作
            if (iRet != SDKConstMsg.PLAYER_PLAYING)
            {
                int iChannelNum = 0;

                //获得当前窗口连接的网络视频服务器最大通道数
                NVSSDK.NetClient_GetChannelNum(m_cltInfo.m_iServerID, ref iChannelNum);

                //判断是否超过最大通道号
                if (m_cltInfo.m_iChannelNo >= iChannelNum)
                {
                    //MessageBox.Show("Max Channel is " + iChannelNum);
                    return;
                }
                Thread.Sleep(500);
                //开始接收一路视频数据	
                iRet = NVSSDK.NetClient_StartRecv(ref uiConID, ref m_cltInfo, null);

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

                //开始导出收到的数据
                //NVSSDK.NetClient_StartCaptureData(uiConID);
                Thread.Sleep(500);
                RECT rect = new RECT() {
                    left = 0
                };

                //开始播放某路视频
                NVSSDK.NetClient_StartPlay(uiConID, _hWnd, rect, 0);
                //btnPlay.Text = "Stop";
                //GetWindowStates();  
                //获得H264数据
                switch (m_iCurrentFrame)
                {
                    case 0:
                        //iRet = NVSSDK.NetClient_SetRawFrameCallBack(uiConID, SetRawFrameCallBack0, IntPtr.Zero);
                        cbk_DecYUV0 = SetDecCallBack0;
                        iRet = NVSSDK.NetClient_SetDecCallBack(uiConID, cbk_DecYUV0, IntPtr.Zero);
                        break;
                    case 1:
                        //iRet = NVSSDK.NetClient_SetRawFrameCallBack(uiConID, SetRawFrameCallBack1, IntPtr.Zero);
                        cbk_DecYUV1 = SetDecCallBack1;
                        iRet = NVSSDK.NetClient_SetDecCallBack(uiConID, cbk_DecYUV1, IntPtr.Zero);
                        break;
                    case 2:
                        //iRet = NVSSDK.NetClient_SetRawFrameCallBack(uiConID, SetRawFrameCallBack2, IntPtr.Zero);
                        cbk_DecYUV2 = SetDecCallBack2;
                        iRet = NVSSDK.NetClient_SetDecCallBack(uiConID, cbk_DecYUV2, IntPtr.Zero);
                        break;
                    case 3:
                        //iRet = NVSSDK.NetClient_SetRawFrameCallBack(uiConID, SetRawFrameCallBack3, IntPtr.Zero);
                        cbk_DecYUV3 = SetDecCallBack3;
                        iRet = NVSSDK.NetClient_SetDecCallBack(uiConID, cbk_DecYUV3, IntPtr.Zero);
                        break;
                    //case 4:
                    //    cbk_DecYUV1 = SetDecCallBack0;
                    //    iRet = NVSSDK.NetClient_SetDecCallBack(uiConID, cbk_DecYUV, IntPtr.Zero);
                    //    break;
                }


                //switch (m_iCurrentFrame)
                //{
                //    case 0:
                //        cbk_DecYUV[m_iCurrentFrame] = SetDecCallBack0;
                //        break;
                //    case 1:
                //        cbk_DecYUV[m_iCurrentFrame] = SetDecCallBack1;
                //        break;
                //    case 2:
                //        cbk_DecYUV[m_iCurrentFrame] = SetDecCallBack2;
                //        break;
                //    case 3:
                //        cbk_DecYUV[m_iCurrentFrame] = SetDecCallBack3;
                //        break;
                //    case 4:
                //        cbk_DecYUV[m_iCurrentFrame] = SetDecCallBack4;
                //        break;
                //}
                ////获得yuv数据
                //iRet = NVSSDK.NetClient_SetDecCallBack(uiConID, cbk_DecYUV[m_iCurrentFrame], IntPtr.Zero);

            }
        }
        private static Image<Bgr, byte>[] frame2 = new Image<Bgr, byte>[5];
        //public static byte[] bytesTmp;
        private static void YuvToRgbEx(IntPtr _pData, int _iLen, uint _ulID)
        {
            try
            {
                Task task1 = new Task(() =>
                {
                });
                task1.Start();
            }
            catch
            {
                //return null;
            }
            //
        }
        #region SetDecCallBack

        public static Image<Bgr, byte> originalImg;
        private static byte[] yuvBuffer = new byte[0];
        private static byte[] rgbBuffer = new byte[0];
        private static Task task1;
        private static DECYUV_NOTIFY cbk_DecYUV0 = null;
        private static DECYUV_NOTIFY cbk_DecYUV1 = null;
        private static DECYUV_NOTIFY cbk_DecYUV2 = null;
        private static DECYUV_NOTIFY cbk_DecYUV3 = null;
        private static void SetDecCallBackEx(IntPtr _pData, int _iLen)
        {
            try
            {
                task1 = new Task(() =>
                {
                    try
                    {
                        if (yuvBuffer.Length > 1)
                        {
                            Marshal.Copy(_pData, yuvBuffer, 0, _iLen);
                            yuv420p_to_rgb24(yuvBuffer, rgbBuffer, FrameWidth, FrameHeight);
                            originalImg.Bytes = rgbBuffer;
                              if(ImageGrabbed != null)
                            ImageGrabbed(originalImg.Bitmap);
                        }
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
        static uint nStamp0;
        private static void SetDecCallBack0(uint _ulID, IntPtr _pData, int _iLen, ref FRAME_INFO _pFrameInfo, IntPtr _pContext)
        {
            if (_pFrameInfo.nType == 1)
            {
                try
                {
                    if (nStamp0 == _pFrameInfo.nStamp)
                        return;
                    nStamp0 = _pFrameInfo.nStamp;

                    //if (yuvBuffer.Length == 0)
                    //{
                    //    FrameHeight = (int)_pFrameInfo.nHeight;
                    //    FrameWidth = (int)_pFrameInfo.nWidth;
                    //    yuvBuffer = new byte[(int)(_pFrameInfo.nWidth * _pFrameInfo.nHeight * 1.5)];
                    //    rgbBuffer = new byte[FrameWidth * FrameHeight * 3];
                    //    originalImg = new Image<Bgr, byte>(FrameWidth, FrameHeight);

                    //    init_yuv420p_table();
                    //}
                    //Task taskTmp1 = new Task(() =>
                    //{
                    //    if (task1 == null)
                    //        SetDecCallBackEx(_pData, _iLen);
                    //    while (true)
                    //    {
                    //        if (task1.IsCompleted)
                    //        {
                    //            SetDecCallBackEx(_pData, _iLen);
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            Thread.Sleep(5);
                    //        }
                    //    }
                    //});
                    //taskTmp1.Start();
                }
                catch
                {
                }
            }
        }
        static uint nStamp1;
        private static void SetDecCallBack1(uint _ulID, IntPtr _pData, int _iLen, ref FRAME_INFO _pFrameInfo, IntPtr _pContext)
        {
            if (_pFrameInfo.nType == 1)
            {
                try
                {
                    if (nStamp1 == _pFrameInfo.nStamp)
                        return;
                    nStamp1 = _pFrameInfo.nStamp;
                }
                catch
                {
                }
            }
        }
        static uint nStamp2;
        private static void SetDecCallBack2(uint _ulID, IntPtr _pData, int _iLen, ref FRAME_INFO _pFrameInfo, IntPtr _pContext)
        {
            if (_pFrameInfo.nType == 1)
            {
                try
                {
                    if (nStamp2 == _pFrameInfo.nStamp)
                        return;
                    nStamp2 = _pFrameInfo.nStamp;
                }
                catch
                {
                }
            }
        }
        static uint nStamp3;
        private static void SetDecCallBack3(uint _ulID, IntPtr _pData, int _iLen, ref FRAME_INFO _pFrameInfo, IntPtr _pContext)
        {
            if (_pFrameInfo.nType == 1)
            {
                try
                {
                    if (nStamp3 == _pFrameInfo.nStamp)
                        return;
                    nStamp3 = _pFrameInfo.nStamp;
                }
                catch
                {
                }
            }
        }
        //static uint nStamp;
        //private static void SetDecCallBack(uint _ulID, IntPtr _pData, int _iLen, ref FRAME_INFO _pFrameInfo, IntPtr _pContext)
        //{
            //if (_pFrameInfo.nType == 1)
            //{
            //    try
            //    {
            //        if (nStamp == _pFrameInfo.nStamp)
            //            return;
            //        nStamp = _pFrameInfo.nStamp;

            //        if (yuvBuffer.Length == 0)
            //        {
            //            FrameHeight = (int)_pFrameInfo.nHeight;
            //            FrameWidth = (int)_pFrameInfo.nWidth;
            //            yuvBuffer = new byte[(int)(_pFrameInfo.nWidth * _pFrameInfo.nHeight * 1.5)];
            //            rgbBuffer = new byte[FrameWidth * FrameHeight * 3];
            //            originalImg = new Image<Bgr, byte>(FrameWidth, FrameHeight);
                        
            //            init_yuv420p_table();
            //        }
            //        Task taskTmp1 = new Task(() =>
            //        {
            //            if (task1 == null)
            //                SetDecCallBackEx(_pData, _iLen);
            //            while (true)
            //            {
            //                if (task1.IsCompleted)
            //                {
            //                    SetDecCallBackEx(_pData, _iLen);
            //                    break;
            //                }
            //                else
            //                {
            //                    Thread.Sleep(5);
            //                }
            //            }
            //        });
            //        taskTmp1.Start();
            //    }
            //    catch
            //    {
            //    }
            //}
        //}
        #endregion
        #region etRawFrameCallBack
        private static void SetRawFrameCallBack0(UInt32 _ulID, IntPtr _pData, Int32 _iLen, ref RAWFRAME_INFO _pRawFrameInfo, IntPtr _pContext)
        {
            try
            {
                if(H264Grabbed0!=null )
                H264Grabbed0(_pData, (uint)_iLen, _pRawFrameInfo.nStamp);
            }
            catch
            {
            }
        }
        private static void SetRawFrameCallBack1(UInt32 _ulID, IntPtr _pData, Int32 _iLen, ref RAWFRAME_INFO _pRawFrameInfo, IntPtr _pContext)
        {
            try
            {
                if(H264Grabbed1!=null )
                H264Grabbed1(_pData, (uint)_iLen, _pRawFrameInfo.nStamp);
            }
            catch
            {
            }
        }
        private static void SetRawFrameCallBack2(UInt32 _ulID, IntPtr _pData, Int32 _iLen, ref RAWFRAME_INFO _pRawFrameInfo, IntPtr _pContext)
        {
            try
            {
                if(H264Grabbed2!=null )
                H264Grabbed2(_pData, (uint)_iLen, _pRawFrameInfo.nStamp);
            }
            catch
            {
            }
        }
        private static void SetRawFrameCallBack3(UInt32 _ulID, IntPtr _pData, Int32 _iLen, ref RAWFRAME_INFO _pRawFrameInfo, IntPtr _pContext)
        {
            try
            {
                if(H264Grabbed3!=null )
                H264Grabbed3(_pData, (uint)_iLen, _pRawFrameInfo.nStamp);
            }
            catch
            {
            }
        }
        #endregion
    }
}