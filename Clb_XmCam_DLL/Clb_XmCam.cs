using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Forms;
using System.Threading;


namespace Clb_XmCam_DLL
{
    public class Clb_XmCam
    {
        #region 变量
        /// <summary>
        /// 加载mind相机
        /// </summary>
        public bool m_bl_Add_MindCam = false;
        /// <summary>
        /// 是否退出程序true: 退出 false: 正常运行
        /// </summary>
        public bool blOut = false;
        /// <summary>
        /// 联机初始化状态 true:联机  false:正常通讯
        /// </summary>
        public bool blInit = true;
        /// <summary>
        /// 显示帧率
        /// </summary>
        public int m_iFrameNum = 0;
        /// <summary>
        ///  镜头 1：前视 3：探头  2：后视
        /// </summary>
        public int m_iF_B = 1;
        /// <summary>
        /// 相机1 联机 0：成功  -1：失败
        /// </summary>
        public int m_blReLink_1 = -1;
        /// <summary>
        /// 相机2 联机 0：成功  -1：失败
        /// </summary>
        public int m_blReLink_2 = -1;
        /// <summary>
        /// 相机3 联机 0：成功  -1：失败
        /// </summary>
        public int m_blReLink_3 = -1;
        /// <summary>
        /// 相机4 联机 0：成功  -1：失败
        /// </summary>
        public int m_blReLink_4 = -1;
        /// <summary>
        /// 接收数据 1:正常 0:异常
        /// </summary>
        public int m_i_GetData_1 = 0;
        /// <summary>
        /// 接收数据 1:正常 0:异常
        /// </summary>
        public int m_i_GetData_2 = 0;
        /// <summary>
        /// 接收数据 1:正常 0:异常
        /// </summary>
        public int m_i_GetData_3 = 0;

        /// <summary>
        /// 接收数据 1:正常 0:异常
        /// </summary>
        public int m_i_GetData_4 = 0;
        /// <summary>
        /// 前视相机缓存
        /// </summary>
        public byte[] m_Img_L;

        byte[] m_Img_L_;
        /// <summary>
        /// 后视相机缓存
        /// </summary>
        public byte[] m_Img_R;

        /// <summary>
        /// 打标相机缓存
        /// </summary>
        public byte[] m_Img_R_3;
        /// <summary>
        /// 4号相机缓存
        /// </summary>
        public byte[] m_Img_R_4;
        byte[] m_Img_R_;
        byte[] m_Img_3_;
        byte[] m_Img_4_;
        csInterface m_csInter = new csInterface();

        //1 相机初始化
        [DllImport("V_DLL.dll", EntryPoint = "pair_image_capture", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_capture(int camera_w, int camare_h, string Str_In_Key_Value, string Ip, string Ip2, string iPort, int[] _iOutArr);


        /// <summary>
        /// 模拟相机打开
        /// </summary>
        /// <param name="index"></param>
        /// <param name="camera_w"></param>
        /// <param name="camare_h"></param>
        /// <returns></returns>
        [DllImport("V_DLL.dll", EntryPoint = "pair_image_1", CallingConvention = CallingConvention.Cdecl)]

        static extern int OpenCamera(int index, int camera_w, int camare_h);




        [DllImport("V_DLL.dll", EntryPoint = "pair_image_1", CallingConvention = CallingConvention.Cdecl)]
        
        static extern int pair_image_1(string User, string Pass, string Ip1, string iPort, int[] _iOutArr);

        [DllImport("V_DLL.dll", EntryPoint = "pair_image_2", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_2(string User, string Pass, string Ip2, string iPort, int[] _iOutArr);

        [DllImport("V_DLL.dll", EntryPoint = "pair_image_3", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_3(string User, string Pass, string Ip2, string iPort, int[] _iOutArr);


        [DllImport("V_DLL.dll", EntryPoint = "pair_image_4", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_4(string User, string Pass, string Ip2, string iPort, int[] _iOutArr);
        //2 关闭
        [DllImport("V_DLL.dll", EntryPoint = "Set_Flag", CallingConvention = CallingConvention.Cdecl)]
        static extern void Set_Flag(int iType);  //2   0：视频、 1：关闭相机   2：拍照

        //设置是否旋转 1：旋转 0：不旋转   角度：flJd
        [DllImport("V_DLL.dll", EntryPoint = "Set_Xz_1", CallingConvention = CallingConvention.Cdecl)]
        static extern void Set_Xz_1(int iXz, float flJd);
        [DllImport("V_DLL.dll", EntryPoint = "Set_Xz_2", CallingConvention = CallingConvention.Cdecl)]
        static extern void Set_Xz_2(int iXz, float flJd);
        [DllImport("V_DLL.dll", EntryPoint = "Set_Xz_3", CallingConvention = CallingConvention.Cdecl)]
        static extern void Set_Xz_3(int iXz, float flJd);
        //3 拿相机图像
        [DllImport("V_DLL.dll", EntryPoint = "Video_1", CallingConvention = CallingConvention.Cdecl)]
        static extern int Video_1(byte[] ImageBuffer);

        [DllImport("V_DLL.dll", EntryPoint = "Video_1_New", CallingConvention = CallingConvention.Cdecl)]
        static extern byte[] Video_1_New(int[] _iOutArr);


        [DllImport("V_DLL.dll", EntryPoint = "Video_2", CallingConvention = CallingConvention.Cdecl)]
        static extern int Video_2(byte[] ImageBuffer);
        [DllImport("V_DLL.dll", EntryPoint = "Video_3", CallingConvention = CallingConvention.Cdecl)]
        static extern int Video_3(byte[] ImageBuffer);

        [DllImport("V_DLL.dll", EntryPoint = "Video_4", CallingConvention = CallingConvention.Cdecl)]
        static extern int Video_4(byte[] ImageBuffer);
        /// <summary>
        /// 模拟相机拿视频图像
        /// </summary>
        /// <param name="ImageBuffer"></param>
        /// <returns></returns>

        [DllImport("V_DLL.dll", EntryPoint = "Video_0", CallingConvention = CallingConvention.Cdecl)]
        static extern int Video_0(byte[] ImageBuffer);
        public  string m_NetP = Application.StartupPath + "\\database\\SysConfig.ini";

        public string m_User_1 = "", m_Pass_1 = "", m_User_2 = "", m_Pass_2 = "", m_User_3 = "", m_Pass_3 = "", m_User_4 = "", m_Pass_4 = "";
        /// <summary>
        /// 300万相机像素 宽度
        /// </summary>
        public int m_iW_iMG_1 = 2304;
        /// <summary>
        ///  300万相机像素 高度
        /// </summary>
        public int m_iH_iMG_1 = 1296;
        /// <summary>
        /// 1号 相机名称
        /// </summary>
        public string m_str_Name_1 = "";

        /// <summary>
        /// 300万相机像素 宽度
        /// </summary>
        public int m_iW_iMG_2 = 2304;
        /// <summary>
        ///  300万相机像素 高度
        /// </summary>
        public int m_iH_iMG_2 = 1296;
        /// <summary>
        /// 2号 相机名称
        /// </summary>
        public string m_str_Name_2 = "";

        /// <summary>
        /// 
        /// <summary>
        /// 300万相机像素 宽度
        /// </summary>
        public int m_iW_iMG_3 = 2304;
        /// <summary>
        ///  300万相机像素 高度
        /// </summary>
        public int m_iH_iMG_3 = 1296;
        /// <summary>
        /// 3号 相机名称
        /// </summary>
        public string m_str_Name_3 = "";


        public int m_iW_iMG_4 = 2304;
        /// <summary>
        ///  300万相机像素 高度
        /// </summary>
        public int m_iH_iMG_4 = 1296;
        /// <summary>
        /// 4号 相机名称
        /// </summary>
        public string m_str_Name_4 = "";

        /// <summary>
        /// <summary>
        /// 相机类型： 0模拟，1网络
        /// </summary>
        public int m_i_Mn0_Net1 = 1;

        #region 帧率计算

        /// <summary>
        /// 是否计算帧率
        /// </summary>
        bool m_blCalFrameNum = false;
        /// <summary>
        /// 显示帧率
        /// </summary>
        public int m_i_FrameNum_Out = 10;
        /// <summary>
        /// 帧率
        /// </summary>
        int m_i_FrameNum = 0;

        /// <summary>
        /// 计算帧率
        /// </summary>
        System.Timers.Timer m_Tim_FrameNum = new System.Timers.Timer();
        #endregion 帧率计算
        /// <summary>
        /// 相机图像变量长度
        /// </summary>
        int m_iCam_Len = 0;
        public string m_IP_1 = "192.168.1.12";
        public string m_IP_2 = "192.168.1.13";
        public string m_IP_3 = "192.168.1.13";
        public string m_IP_4 = "192.168.1.13";
        string m_Port = "554";
        /// <summary>
        /// 视频1运行
        /// </summary>
        Thread Thread_RunVideo_1 = null;
        /// <summary>
        /// 视频2运行
        /// </summary>
        Thread Thread_RunVideo_2 = null;
        /// <summary>
        /// 视频3运行
        /// </summary>
        Thread Thread_RunVideo_3 = null;
        /// <summary>
        /// 视频4运行
        /// </summary>
        Thread Thread_RunVideo_4 = null;

        /// <summary>
        /// 旋转1
        /// </summary>
        public ClXzJd m_clXz_1 = new ClXzJd();
        /// <summary>
        /// 旋转2
        /// </summary>
        public ClXzJd m_clXz_2 = new ClXzJd();
        /// <summary>
        /// 旋转3
        /// </summary>
        public ClXzJd m_clXz_3 = new ClXzJd();
        #endregion 变量

        #region 方法
        public void Close()
        {
            blOut = true;
            try
            {
                Set_Flag(1);
            }
            catch (Exception e)
            {
            }
        }
        int m_iLanguage = 0;
        /// <summary>
        /// 相机联机
        /// </summary>
        public void Link_Phone()//PictureBox Pho_Video)
        {
            blInit = true;
            blOut = true;
            m_NetP = Application.StartupPath + "\\database\\HardConfig.ini";
            m_iLanguage = int.Parse(m_csInter.IniReadDefine("Language", "m_iLanguage", "0", Application.StartupPath + "\\database\\SysConfig.ini"));

            m_bl_Add_MindCam =  (m_csInter.IniReadDefine("Cam", "m_bl_Add_MindCam", "1", m_NetP) == "1");
            m_i_Mn0_Net1 = int.Parse(m_csInter.IniReadDefine("Cam", "m_i_Mn0_Net1", "1", m_NetP));

                m_i_FrameNum_Out = int.Parse(m_csInter.IniReadDefine("Cam", "m_i_FrameNum_Out", "10", m_NetP));
            if (m_bl_Add_MindCam == false)
                m_IP_1 = m_csInter.IniReadDefine("Cam", "m_Ip_1", "", m_NetP);
            else
                m_IP_1 = "";
            m_IP_2 = m_csInter.IniReadDefine("Cam", "m_Ip_2", "", m_NetP);
            m_IP_3 = m_csInter.IniReadDefine("Cam", "m_IP_3", "", m_NetP);
            m_IP_4 = m_csInter.IniReadDefine("Cam", "m_IP_4", "", m_NetP);
            m_Port = m_csInter.IniReadDefine("Cam", "Port_1", "554", m_NetP);

            m_User_1 = m_csInter.IniReadDefine("Cam", "m_User_1", "admin", m_NetP);
            m_Pass_1 = m_csInter.IniReadDefine("Cam", "m_Pass_1", "", m_NetP);
            m_str_Name_1= m_csInter.IniReadDefine("Cam", "m_str_Name_1", (m_iLanguage==0? "1号相机": "The first camera"), m_NetP);

            m_User_2 = m_csInter.IniReadDefine("Cam", "m_User_2", "admin", m_NetP);
            m_Pass_2 = m_csInter.IniReadDefine("Cam", "m_Pass_2", "", m_NetP);
            m_str_Name_2 = m_csInter.IniReadDefine("Cam", "m_str_Name_2", (m_iLanguage == 0 ? "2号相机" : "The second camera"), m_NetP);

            m_User_3 = m_csInter.IniReadDefine("Cam", "m_User_3", "admin", m_NetP);
            m_Pass_3 = m_csInter.IniReadDefine("Cam", "m_Pass_3", "", m_NetP);
            m_str_Name_3 = m_csInter.IniReadDefine("Cam", "m_str_Name_3", (m_iLanguage == 0 ? "3号相机" : "The third camera"), m_NetP);

            m_User_4 = m_csInter.IniReadDefine("Cam", "m_User_4", "admin", m_NetP);
            m_Pass_4 = m_csInter.IniReadDefine("Cam", "m_Pass_4", "", m_NetP);
            m_str_Name_4 = m_csInter.IniReadDefine("Cam", "m_str_Name_4", (m_iLanguage == 0 ? "4号相机" : "The fourth camera"), m_NetP);

            m_i_ShowWaitTime = int.Parse(m_csInter.IniReadDefine("Cam", "m_i_ShowWaitTime", "30", m_NetP));

            //m_iCam_Len = m_iW_iMG * m_iH_iMG * 3;
            //m_Img_L = new byte[m_iCam_Len];
            //m_Img_R = new byte[m_iCam_Len];
            //m_Img_R_3 = new byte[m_iCam_Len];

            //m_Img_L_ = new byte[m_iCam_Len];
            //m_Img_R_ = new byte[m_iCam_Len];
            //m_Img_3_ = new byte[m_iCam_Len];
            try
            {
                m_Tim_FrameNum.Interval = 1000;
                m_Tim_FrameNum.Elapsed -= Cal_Frame;
                m_Tim_FrameNum.Elapsed += Cal_Frame;
                Set_Flag(1);//  m_i_Mn0_Net1==1?1:2);
                Waite(0.1f);
                blOut = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("调用DLL失败" + e.Message);
            }
            blOut = false;
            if (m_IP_1 != "")
            {
                m_blReLink_1 = -1;
                if (Thread_RunVideo_1 != null) Thread_RunVideo_1.Abort();
                Thread_RunVideo_1 = new Thread(new ThreadStart(RunVideo_1));
                Thread_RunVideo_1.IsBackground = true;
                Thread_RunVideo_1.Start();
            }
            if (m_IP_2 != "" && m_i_Mn0_Net1 == 1)
            {
                Waite(0.2f);
                m_blReLink_2 = -1;
                if (Thread_RunVideo_2 != null) Thread_RunVideo_2.Abort();
                Thread_RunVideo_2 = new Thread(new ThreadStart(RunVideo_2));
                Thread_RunVideo_2.IsBackground = true;
                Thread_RunVideo_2.Start();
            }

            if (m_IP_3 != "" && m_i_Mn0_Net1 == 1)
            {
                Waite(0.2f);
                m_blReLink_3 = -1;
                if (Thread_RunVideo_3 != null) Thread_RunVideo_3.Abort();
                Thread_RunVideo_3 = new Thread(new ThreadStart(RunVideo_3));
                Thread_RunVideo_3.IsBackground = true;
                Thread_RunVideo_3.Start();
            }
            if (m_IP_4 != "" && m_i_Mn0_Net1 == 1)
            {
                Waite(0.2f);
                m_blReLink_4 = -1;
                if (Thread_RunVideo_4 != null) Thread_RunVideo_4.Abort();
                Thread_RunVideo_4 = new Thread(new ThreadStart(RunVideo_4));
                Thread_RunVideo_4.IsBackground = true;
                Thread_RunVideo_4.Start();
            }
        }
        public int m_i_ShowWaitTime = 20;
        /// <summary>
        /// 重复联机次数
        /// </summary>
        public int m_iLink_Times = 7;
        private void RunVideo_1()
        {
            int[] _ArrOut = new int[5];
            //返回 0:成功  -1：失败
            REP_LINK:
            try
            {
                int iTimes = 0;
                m_blReLink_1 = 1;
                while (m_blReLink_1 != 0 && iTimes < m_iLink_Times)
                {
                    if (m_i_Mn0_Net1 == 0)
                    {
                        m_blReLink_1 = OpenCamera(0, m_iW_iMG_1, m_iH_iMG_1) == 1 ? 0 : -1;
                    }
                    else
                        m_blReLink_1 = pair_image_1(m_User_1, m_Pass_1, m_IP_1, m_Port, _ArrOut);
                    if (m_blReLink_1 == 0)
                    {
                        // if (_ArrOut[3] != m_iW_iMG_1)
                        {
                            m_iW_iMG_1 = _ArrOut[3]; m_csInter.INIWriteValue("Cam", "m_iWith_1", m_iW_iMG_1.ToString(), m_NetP);
                            m_iH_iMG_1 = _ArrOut[4]; m_csInter.INIWriteValue("Cam", "m_iHeight_1", m_iH_iMG_1.ToString(), m_NetP);
                            m_iCam_Len = m_iW_iMG_1 * m_iH_iMG_1 * 3;
                            blOut = false;
                            m_Img_L = new byte[m_iCam_Len];
                            m_Img_L_ = new byte[m_iCam_Len];
                            m_csInter.INIWriteValue("VIDEO", "HaveRun_1", "0", Application.StartupPath + "\\database\\SysConfig.ini");
                        }
                        break;
                    }
                    Waite(0.1f);
                    iTimes++;
                }
            }
            catch (Exception e)
            { }
            m_csInter.INIWriteValue("VIDEO", "HaveRun", "1", Application.StartupPath + "\\database\\SysConfig.ini");
            // int[] _iArrOut = new int[1]; 
            // IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Img_L_, 0);
            if (m_blReLink_1 == 0)
            {
                while (blOut == false)
                {
                    if (m_blReLink_1 == 0)
                    {
                        //  Set_Xz_1(m_clXz_1.i_Xz, m_clXz_1.fl_Jd);
                        try
                        {
                            if (blInit) blInit = false;
                            if (blOut == false) m_i_GetData_1 = Video_1(m_Img_L);
                            if (m_i_GetData_1 == 0)
                            {
                                Waite(0.02f);
                                goto REP_LINK;
                                //  blOut = true;
                            }
                        }
                        catch (Exception e1)
                        { }

                        if (m_blCalFrameNum)
                            m_i_FrameNum++;
                        //if (m_i_GetData_1 == 1)
                        //{
                        //    ptr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Img_L_, 0);
                        //    Marshal.Copy(ptr, m_Img_L, 0, m_iCam_Len);
                        //}
                        Thread.Sleep(m_i_ShowWaitTime);
                    }
                    else
                    {
                        Waite(0.02f);
                        goto REP_LINK;
                    }
                    Application.DoEvents();
                }
            }
            blInit = false;
        }
        private void Cal_Frame(object sender, System.Timers.ElapsedEventArgs e)
        {
            m_i_FrameNum_Out = m_i_FrameNum;
            m_i_FrameNum = 0;
        }
        /// <summary>
        /// 开始计算帧率
        /// </summary>
        public void Frame_Start(int iType = 0)
        {
            if (iType == 0)
            {
                m_blCalFrameNum = true;
                m_i_FrameNum = 0; m_Tim_FrameNum.Enabled = true; ;
            }
            else
            {
                m_blCalFrameNum = false;
                m_Tim_FrameNum.Enabled = false;
            }
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
        private void RunVideo_2()
        {
            int[] _ArrOut = new int[5];
            REP_LINK:
            //返回 0:成功  -1：失败
            try
            {
                int iTimes = 0;
                m_blReLink_2 = 1;
                while (m_blReLink_2 != 0 && iTimes < m_iLink_Times)
                {
                    m_blReLink_2 = pair_image_2(m_User_2, m_Pass_2, m_IP_2, m_Port, _ArrOut);
                    if (m_blReLink_2 == 0)
                    {
                        // if (_ArrOut[3] != m_iW_iMG)
                        {
                            m_iW_iMG_2 = _ArrOut[3]; m_csInter.INIWriteValue("Cam", "m_iWith_2", m_iW_iMG_2.ToString(), m_NetP);
                            m_iH_iMG_2 = _ArrOut[4]; m_csInter.INIWriteValue("Cam", "m_iHeight_2", m_iH_iMG_2.ToString(), m_NetP);
                            m_iCam_Len = m_iW_iMG_2 * m_iH_iMG_2 * 3;
                            blOut = false;
                            m_Img_R = new byte[m_iCam_Len];
                            m_Img_R_ = new byte[m_iCam_Len];
                            m_csInter.INIWriteValue("VIDEO", "HaveRun_2", "0", Application.StartupPath + "\\database\\SysConfig.ini");
                        }
                        break;
                    }
                    Waite(0.1f);
                    iTimes++;
                }

            }
            catch (Exception e)
            { }

            // IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Img_R_, 0);
            if (m_blReLink_2 == 0)
            {
                while (blOut == false)
                {
                    if (m_blReLink_2 == 0)
                    {
                        //  Set_Xz_2(m_clXz_2.i_Xz, m_clXz_2.fl_Jd);
                        try
                        {
                            if (blInit) blInit = false;
                            if (blOut == false) m_i_GetData_2 = Video_2(m_Img_R);
                            if (m_i_GetData_2 == 0)
                            {
                                Waite(0.02f); goto REP_LINK;
                                //   blOut = true;
                            }
                        }
                        catch (Exception e2)
                        { }
                        if (m_blCalFrameNum)
                            m_i_FrameNum++;
                        //if (m_i_GetData_2 == 1)
                        //{
                        //    ptr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Img_R_, 0);
                        //    Marshal.Copy(ptr, m_Img_R, 0, m_iCam_Len);
                        //}
                        Thread.Sleep(m_i_ShowWaitTime);
                    }
                    else
                    {
                        Waite(0.02f); goto REP_LINK;
                    }
                    Application.DoEvents();
                }

            }
            else
                blInit = false;
        }
        private void RunVideo_3()
        {
            int[] _ArrOut = new int[5];
            REP_LINK:
            //返回 0:成功  -1：失败
            try
            {
                int iTimes = 0;
                m_blReLink_3 = 1;
                while (m_blReLink_3 != 0 && iTimes < m_iLink_Times)
                {
                    m_blReLink_3 = pair_image_3(m_User_3, m_Pass_3, m_IP_3, m_Port, _ArrOut);
                    if (m_blReLink_3 == 0)
                    {
                        //if (_ArrOut[3] != m_iW_iMG)
                        {
                            m_iW_iMG_3 = _ArrOut[3]; m_csInter.INIWriteValue("Cam", "m_iWith_3", m_iW_iMG_3.ToString(), m_NetP);
                            m_iH_iMG_3 = _ArrOut[4]; m_csInter.INIWriteValue("Cam", "m_iHeight_3", m_iH_iMG_3.ToString(), m_NetP);
                            m_iCam_Len = m_iW_iMG_3 * m_iH_iMG_3 * 3;
                            blOut = false;
                            m_Img_R_3 = new byte[m_iCam_Len];
                            m_Img_3_ = new byte[m_iCam_Len];
                            m_csInter.INIWriteValue("VIDEO", "HaveRun_3", "0", Application.StartupPath + "\\database\\SysConfig.ini");
                        }
                        break;
                    }
                    Waite(0.1f);
                    iTimes++;
                }

            }
            catch (Exception e)
            { }
          //  IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Img_3_, 0);
            if (m_blReLink_3 == 0)
            {
                while (blOut == false)
                {
                    if (m_blReLink_3 == 0)
                    {
                        //  Set_Xz_3(m_clXz_3.i_Xz, m_clXz_3.fl_Jd);
                        try
                        {
                            if (blInit) blInit = false;
                            if (blOut == false) m_i_GetData_3 = Video_3(m_Img_R_3);
                            if (m_i_GetData_3 == 0)
                            {
                                Waite(0.02f); goto REP_LINK;
                                //    blOut = true;
                            }
                        }
                        catch (Exception e3)
                        { }
                        if (m_blCalFrameNum)
                            m_i_FrameNum++;
                        //if (m_i_GetData_3 == 1)
                        //{
                        //    ptr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Img_3_, 0);
                        //    Marshal.Copy(ptr, m_Img_R_3, 0, m_iCam_Len);
                        //}
                        Thread.Sleep(m_i_ShowWaitTime);
                    }
                    else
                    {
                        Waite(0.02f);
                        goto REP_LINK;
                    }
                    Application.DoEvents();
                }
            }
            else
                blInit = false;
        }
        private void RunVideo_4()
        {
            int[] _ArrOut = new int[5];
            REP_LINK:
            //返回 0:成功  -1：失败
            try
            {
                int iTimes = 0;
                m_blReLink_4 = 1;
                while (m_blReLink_4 != 0 && iTimes < m_iLink_Times)
                {
                    m_blReLink_4 = pair_image_4(m_User_4, m_Pass_4, m_IP_4, m_Port, _ArrOut);
                    if (m_blReLink_4 == 0)
                    {
                        //if (_ArrOut[3] != m_iW_iMG)
                        {
                            m_iW_iMG_4 = _ArrOut[3]; m_csInter.INIWriteValue("Cam", "m_iWith_4", m_iW_iMG_4.ToString(), m_NetP);
                            m_iH_iMG_4 = _ArrOut[4]; m_csInter.INIWriteValue("Cam", "m_iHeight_4", m_iH_iMG_4.ToString(), m_NetP);
                            m_iCam_Len = m_iW_iMG_4 * m_iH_iMG_4 * 3;
                            blOut = false;
                            m_Img_R_4 = new byte[m_iCam_Len];
                            m_Img_4_ = new byte[m_iCam_Len];
                            m_csInter.INIWriteValue("VIDEO", "HaveRun_4", "0", Application.StartupPath + "\\database\\SysConfig.ini");
                        }
                        break;
                    }
                    Waite(0.1f);
                    iTimes++;
                }

            }
            catch (Exception e)
            { }
            //  IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(m_Img_4_, 0);
            if (m_blReLink_4 == 0)
            {
                while (blOut == false)
                {
                    if (m_blReLink_4 == 0)
                    {
                        //  Set_Xz_4(m_clXz_4.i_Xz, m_clXz_4.fl_Jd);
                        try
                        {
                            if (blInit) blInit = false;
                            if (blOut == false) m_i_GetData_4 = Video_4(m_Img_R_4);
                            if (m_i_GetData_4 == 0)
                            {
                                Waite(0.02f); goto REP_LINK;
                                //    blOut = true;
                            }
                        }
                        catch (Exception e3)
                        { }
                        if (m_blCalFrameNum)
                            m_i_FrameNum++;
                        Thread.Sleep(m_i_ShowWaitTime);
                    }
                    else
                    {
                        Waite(0.02f);
                        goto REP_LINK;
                    }
                    Application.DoEvents();
                }
            }
            else
                blInit = false;
        }
        #endregion  方法
    }
  
    public class csInterface
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

        /// <summary>
        /// 等待指定时间（秒）
        /// </summary>
        /// <param name="dbWait"></param>
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

    public class ClXzJd
    {
        /// <summary>
        /// 是否旋转 1：旋转 0：不旋转
        /// </summary>
        public int i_Xz = 0;
        /// <summary>
        /// 旋转角度0-359
        /// </summary>
        public float fl_Jd = 0;
    }
}
