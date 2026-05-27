/*
 TOFD程序 
 * 
 */ 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Threading;

using ClassLib_DataMang.DataBaseMang.OleDal;//数据库操作类
using Tofd_AWI.Class;
using HL;
using Clb_XmCam_DLL;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using ReportDLL;
using ClassLib_TestData;


namespace Tofd_AWI
{
    public partial class Frm_Main : Form
    {
        #region 变量
        #region 测高焊缝跟踪
        /// <summary>
        /// 寻迹程序是否在研杨板子上，随车体安装 
        /// true: 在研杨板子上 
        /// false: 在PC上
        /// </summary>
        bool m_blXunJi_Prog_0PC_1YY = false;
        /// <summary>
        /// 寻迹激光器图像宽度 默认50，实际按照激光器供货修改
        /// </summary>
        int m_iData_With = 50;

        /// <summary>
        /// 是否刷新帧数据
        /// </summary>
        bool m_blBrushFram = true;
        /// <summary>
        /// 是否自动校正水平线
        /// </summary>
        bool m_ckAutoJZ = true;
        /// <summary>
        /// 给车体发送循迹命令等待时间间隔个数
        /// </summary>
        int m_i_WaitTimeNum = 0;
        /// <summary>
        /// 给车体发送循迹命令等待总时间间隔
        /// </summary>
        int m_i_WaitTimeNum_Max = 0;
        /// <summary>
        /// 开始运行次数
        /// </summary>
        int m_iTimes = 0;
        /// <summary>
        /// 还没有计算
        /// </summary>
        bool m_blTimes = false;
        /// <summary>
        /// 串口发送报文
        /// </summary>
        byte[] m_btArr_SendVideo = new byte[3 + 22 + 1];//26
        /// <summary>
        /// 申请测量数据字典
        /// </summary>
        static Frame_Work.ClassSys_Buff m_Insp_Data = new Frame_Work.ClassSys_Buff();
        /// <summary>
        /// 画图工具
        /// </summary>
        Frame_Work.Class_Plant m_Plant = new Frame_Work.Class_Plant();
        /// <summary>
        /// 激光测量类
        /// </summary>
        HD850_64.Cl_HD850_64 m_Hd850 = new HD850_64.Cl_HD850_64(ref m_Insp_Data);
        #endregion 
        /// <summary>
        /// 读Pc电源间隔次数
        /// </summary>
        int m_iPcPower_Times = 0;
        /// <summary>
        /// 串口名称集合
        /// </summary>
        string[] m_arrPort_Names;
        /// <summary>
        /// 串口连接状态
        /// </summary>
        bool[] m_blArrLinkState;
        /// <summary>
        /// 报表输出
        /// </summary>
        ClsReport m_Report = new ClsReport();
        /// <summary>
        /// 使用雄迈相机
        /// </summary>
        Clb_XmCam_DLL.Clb_XmCam m_Cam_Xm = new Clb_XmCam_DLL.Clb_XmCam();
        /// <summary>
        /// 视频1运行
        /// </summary>
        Thread Thread_ShowVideo = null;

        /// <summary>
        /// 视频通讯服务器
        /// </summary>
        Class_Server_UI m_ServerUI = new Class_Server_UI();
        /// <summary>
        /// 视频程序指针
        /// </summary>
        System.Diagnostics.Process m_p = new System.Diagnostics.Process();
        /// <summary>
        /// 视频运行 true:运行  false:关闭
        /// </summary>
        private bool m_bl_Video_Run = true;
        /// <summary>
        /// 查询出的数据
        /// </summary>
        List<ClassLib_TestData.Class_Test_Records> m_lstRec;
        /// <summary>
        /// D图鼠标X轴移动位置
        /// </summary>
        private int m_iX_D = 0;
        /// <summary>
        /// D图鼠标Y轴移动位置
        /// </summary>
        private int m_iY_D = 0;
        /// <summary>
        /// 当前鼠标对应的距离
        /// </summary>
        private float  m_flCurrDistanc=0;
        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hChild, IntPtr hParent);
        [DllImport("user32")]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool bReDrow);

        /// <summary>
        /// 视频占窗体左右比例
        /// </summary>
        float m_fl_LR_New_R = 50;
        /// <summary>
        ///  视频占窗体上下比例
        /// </summary>
        float m_fl_UD_New_D = 28.91f;
        /// <summary>
        /// 0：缩小 1：放大
        /// </summary>
        int m_iLit0_Bit1 = 0;
        /// <summary>
        /// 0：缩小 1：放大
        /// </summary>
        int m_iLit0_Bit1_A = 0;

        /// <summary>
        /// 翻页右边是否有数据
        /// </summary>
        bool m_blPages_R = true;
        #region 窗体
        From.Frm_TOFD m_frm_Tofd = new From.Frm_TOFD();
        From.Frm_Item m_frm_Item = new From.Frm_Item();
        From.Frm_Weld m_frm_Weld = new From.Frm_Weld();
        From.Frm_Move m_frm_Move = new From.Frm_Move();
        /// <summary>
        /// 标注
        /// </summary>
        From.Frm_Bz m_frm_Bz = new From.Frm_Bz();
        #endregion 窗体
        /// <summary>
        /// 窗体是否激活
        /// </summary>
        bool m_blActive = false;

        #region Tofd数据处理
        public PointF[] m_Pit_Pbl;
        /// <summary>
        /// 抛物线系数，数字越小开口越大
        /// </summary>
        public float m_Pbl_P = 2;
        /// <summary>
        /// <summary>
        /// 画波形
        /// </summary>
        delegate void Delg_PlantAllWave();
      
        /// <summary>
        /// TOFD联机
        /// </summary>
        private Thread Thread_Tofd_Link = null;
        #endregion  Tofd数据处理
        /// <summary>
        /// 放大缩小图像 false: 小图像 true:大图像
        /// </summary>
        bool m_blBig = false;
        /// <summary>
        /// 寻迹：连接服务器线程
        /// </summary>
        Thread TreadTcp_Server;
        /// <summary>
        /// 测高寻迹报文显示行数
        /// </summary>
        int m_i_Com_Num = 0;
        /// <summary>
        /// 抛物线高度
        /// </summary>
        int m_iHeight = 50;
        /// <summary>
        /// X轴距离信息
        /// </summary>
        private float[] m_Axis_X = new float[500];
        private double m_maxX = 50;//轮廓线X最大范围
        private double m_maxZ = 80;//轮廓线Z最大范围
        private double m_scaleX = 5;//显示轮廓的X刻度
        private double m_scaleZ = 8;//显示轮廓的Z刻度
        /// <summary>
        /// 上下抛物线开口程度
        /// </summary>
        int m_i_Pbl_X = 50;
        /// <summary>
        /// 0: 开口朝下  1：开口朝上
        /// </summary>
        int m_i_Pbl_Type = 0;
        /// <summary>
        /// 是否显示抛物线
        /// </summary>
      //  bool m_bl_Show_Pbl = true;
        #endregion 变量
        public Frm_Main()
        {
            InitializeComponent();
            MouseWheel += new MouseEventHandler(FrmD_MouseWheel);
     //      this.Pic_D.Click += new System.EventHandler(this.Pic_D_Click);
        }
        private void FrmD_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (m_i_Pbl_Type < 2)
                {
                    if (m_i_Pbl_X < 150)
                        m_i_Pbl_X+=2;
                }
               
            }
            else
            {
                if (m_i_Pbl_Type < 2)
                {
                    if (m_i_Pbl_X > 10)
                        m_i_Pbl_X-=2;
                }
            }
            if (m_i_Pbl_Type < 2)
                m_iHeight = m_i_Pbl_X;
        
        }
        private bool  InitDataBase()
        {
            bool _Ret = false;
            //0 使用Accesse桌面数据库还是sql server网络数据库
            DbGlobal.ImCreatDataBase.m_iDataBase_Type = int.Parse(SysInfo.csInter.IniReadDefine("SqlDbHelper", "m_iDataBase_Type", "0", SysInfo.HardFileName));

            DbGlobal.ImUser.m_iDataBase_Type = DbGlobal.ImCreatDataBase.m_iDataBase_Type;//用户表，只查看是否建立了数据库
            DbGlobal.ImAlarm.m_iDataBase_Type = DbGlobal.ImCreatDataBase.m_iDataBase_Type;//信息表，测量数据ID主信息
            DbGlobal.ImTest_Item.m_iDataBase_Type = DbGlobal.ImCreatDataBase.m_iDataBase_Type;//厚度表  ID对应厚度数据
            DbGlobal.ImTest_Parts.m_iDataBase_Type = DbGlobal.ImCreatDataBase.m_iDataBase_Type;//厚度表  ID对应厚度数据
            DbGlobal.ImTest_Records.m_iDataBase_Type = DbGlobal.ImCreatDataBase.m_iDataBase_Type;//厚度表  ID对应厚度数据

            try
            {
                if (DbGlobal.ImCreatDataBase.m_iDataBase_Type == 0)
                    _Ret = DbGlobal.CreatDataBase_New(true);
                else
                    _Ret = DbGlobal.CreatDataBase_New_1(true);
            }
            catch { }
            return _Ret;
        }
        private void ClearOneClient(string AppEXE)
        {
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName(AppEXE))
            {
                p.Kill();
            }
        }
        /// <summary>
        /// D图自适应高度
        /// </summary>
        private void ScreenUpChange()
        {
            m_fl_UD_New_D =   Tb_Main_Data.RowStyles[1] .Height ;
            while (Pic_D.Height < Tofd.UTS_DATA_WIDTH+15)
            {
                m_fl_UD_New_D++;
                Tb_Main_Data.RowStyles[1].Height = m_fl_UD_New_D;
                Tb_Main_Data.RowStyles[0].Height -= 1;
            }

        }
        private void Frm_Main_Load(object sender, EventArgs e)
        {
            ClearOneClient("ClimbVideo");
         //   Init_Screen();
            SysInfo .  m_strLinkMsg = "";
            #region 数据库处理
            if (InitDataBase() == false)
                SysInfo.m_strLinkMsg = "数据库连接失败";
            #endregion
            this.Height = Screen.PrimaryScreen.WorkingArea.Height + 10;
            this.Left = -10;
            SysInfo.Init();
            SysInfo.m_Plant.Init();

            ScreenUpChange();

            #region 1 相机连接
            //监视相机联机


            int iCs = 0;string _strMsg = "";
            if (iCs == 0)
            {
                int _iCam_Type = 0;
                if (_iCam_Type == 0)
                {
                   LinkServer();
                    Link_New_V();
                    SysInfo.csInter.WaitTime(1);
                }
                else
                {
                    HLSysInfo.iNo = 0;
                    HLSysInfo.strIP = SysInfo.m_strIP;
                    HLSysInfo.strUser = SysInfo.m_strUser;
                    HLSysInfo.strPwd = SysInfo.m_strPwd;
                    #region 合力相机

                    for (int _i = 0; _i < 2; _i++)
                    {
                        switch (_i)
                        {
                            case 0:
                                if (HLSysInfo.m_blLink[_i]) HLSysInfo.Close(_i);
                                HLSysInfo.iNo = _i;
                                HLSysInfo.strIP = SysInfo.csInter.IniReadDefine("Camera", "Ip_" + (_i + 1), "192.168.1.2", SysInfo.HardFileName);
                                HLSysInfo.strUser = SysInfo.csInter.IniReadDefine("Camera", "User_" + (_i + 1), "admin", SysInfo.HardFileName);
                                HLSysInfo.strPwd = SysInfo.csInter.IniReadDefine("Camera", "Pwd_" + (_i + 1), "admin", SysInfo.HardFileName);
                                HLSysInfo.StartUp();
                                HLSysInfo.Load(_i);
                                HLSysInfo.ImageGrabbed_0 -= PubCapture_ImageGrabbed_0;
                                HL.HLSysInfo.ImageGrabbed_0 += PubCapture_ImageGrabbed_0;

                                if (HLSysInfo.m_blLink[_i] == false)
                                    _strMsg += (_strMsg == "" ? "" : ",") + "相机" + (_i + 1);
                                break;
                            case 1:
                                if (HLSysInfo.m_blLink[_i]) HLSysInfo.Close(_i);
                                HLSysInfo.iNo = _i;
                                HLSysInfo.strIP = SysInfo.csInter.IniReadDefine("Camera", "Ip_" + (_i + 1), "192.168.1.3", SysInfo.HardFileName);
                                HLSysInfo.strUser = SysInfo.csInter.IniReadDefine("Camera", "User_" + (_i + 1), "admin", SysInfo.HardFileName);
                                HLSysInfo.strPwd = SysInfo.csInter.IniReadDefine("Camera", "Pwd_" + (_i + 1), "admin", SysInfo.HardFileName);
                                HLSysInfo.StartUp();
                                HLSysInfo.Load(_i);
                                HLSysInfo.ImageGrabbed_1 -= PubCapture_ImageGrabbed_1;
                                HL.HLSysInfo.ImageGrabbed_1 += PubCapture_ImageGrabbed_1;

                                if (HLSysInfo.m_blLink[_i] == false)
                                    _strMsg += (_strMsg == "" ? "" : ",") + "相机" + (_i + 1);
                                break;
                        }
                    }
                    if (_strMsg != "")
                    {
                        _strMsg += ": 联机失败";
                        SysInfo.m_strLinkMsg += (SysInfo.m_strLinkMsg != "" ? "," : "") + _strMsg;
                    }
                    #endregion  合力相机
                }
            }
            //平板电池板通讯联机
            PowerLink();
            if (SysInfo . m_PcPower.m_blNetLink == false)
                SysInfo.m_strLinkMsg += (SysInfo.m_strLinkMsg != "" ? "," : "") + "平板电池板通讯联机失败";
           
            //寻迹相机客户端连接
            SysInfo.g_Msg_InterFace.Inter_GetServe_CgXj -= new ClassLib_TestData.MsgInterFace.OnGetCgXjServe_Data(ShowServeData);
            SysInfo.g_Msg_InterFace.Inter_GetServe_CgXj += new ClassLib_TestData. MsgInterFace.OnGetCgXjServe_Data(ShowServeData);

            SysInfo.g_Msg_InterFace.Inter_Brush_D -= new ClassLib_TestData.MsgInterFace.OnBrush_D (Init_PlantD );
            SysInfo.g_Msg_InterFace.Inter_Brush_D += new ClassLib_TestData.MsgInterFace.OnBrush_D(Init_PlantD);

            float _flD = 0;
            for (int i = 1; i < 500; i++)
            {
                _flD += 0.1f;
                m_Axis_X[i] = float.Parse(_flD.ToString("f1"));
            }
            m_blXunJi_Prog_0PC_1YY = int.Parse(SysInfo.csInter.IniReadDefine("XunJi", "m_blXunJi_Prog_0PC_1YY", "0", SysInfo.HardFileName)) == 1;

            if(m_blXunJi_Prog_0PC_1YY)  ConnectToServer();

            Txt_Limt_L.Text = SysInfo.csInter.IniReadDefine("System", "Txt_Limt_L", "1", SysInfo.HardFileName);
            Txt_Limt_R.Text = SysInfo.csInter.IniReadDefine("System", "Txt_Limt_R", "0.5", SysInfo.HardFileName);

            if (m_blXunJi_Prog_0PC_1YY  && SysInfo.m_Client != null)
                SysInfo.m_Client.SendData_C(7, Txt_Limt_L.Text + "," + Txt_Limt_R.Text);
            #endregion  1

            #region 2 车体连接
            SysInfo.m_Climb4 = new Clb_MT_Comm.MT_Comm(ref SysInfo.m_Climb, ref SysInfo.g_Msg_InterFace, SysInfo.m_i_Can_BrushTime);
            SysInfo.m_Climb4.m_blCom1_Can0 = int.Parse (SysInfo.csInter.IniReadDefine("COM_Can", "m_blCom1_Can0", "0", SysInfo.HardFileName));
            if (iCs == 0)
            {
                if (SysInfo.m_Climb4.m_blCom1_Can0 == 0)
                    SysInfo.m_Climb4.InitCan();
                else
                {
                    for (int iCom_No = 0; iCom_No < m_blArrLinkState.Count(); iCom_No++)
                    {
                        if (m_blArrLinkState[iCom_No] == false)//m_arrPort_Names
                        {
                            SysInfo.m_Climb.Trip_Com_mm = 0;
                            if (SysInfo.m_Climb4.m_blLink)
                                SysInfo.m_Climb4.CloseSet();
                            SysInfo.m_Climb4.InitCom(m_arrPort_Names[iCom_No ]);
                            DateTime dtStar = DateTime.Now;
                            while (true)
                            {
                                try
                                {
                                    if (SysInfo.m_Climb.Trip_Com_mm != 0) break;
                                    Application.DoEvents();
                                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > 2) break;
                                    Thread.Sleep(10);
                                }
                                catch { break; }
                            }
                        }
                    }
                }
                if (SysInfo.m_Climb4.m_blLink == false)
                {
                    SysInfo.m_Climb4.CloseSet();
                    //if (SysInfo.m_Client.m_blLinkServe == false)
                        SysInfo.m_strLinkMsg += "," + " 车体联机失败";
                }
            }

            if (SysInfo.m_Climb4.m_blLink )
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
            #endregion  2
            #region 3 寻迹设备联机
            if (m_blXunJi_Prog_0PC_1YY == false)
            {
                string _T = SysInfo.csInter.IniReadDefine("XunJi", "m_iData_With", "100", SysInfo.HardFileName);
                string _Heart = SysInfo.csInter.IniReadDefine("XunJi", "Heart", "H", SysInfo.HardFileName);
                m_iData_With = int.Parse(_T==""?"100":_T);
                m_Hd850.Lb_YuanShiTu = Lb_LunKuo;
                string _StrFileID = SysInfo.csInter.IniReadDefine("XunJi", "SN", "SN8-0050W-1207839", SysInfo.HardFileName);
                if (m_Hd850.Run_Cam(_StrFileID))
                    m_Hd850.SetLaser(true);//打开激光
                else
                    SysInfo.m_strLinkMsg += (SysInfo.m_strLinkMsg != "" ? ", " : "") + ("寻迹相机联机失败！");
            }
            #endregion 3
            #region 4 TOFD连接
            if (m_blXunJi_Prog_0PC_1YY && SysInfo.m_Client.m_blLinkServe == false)
                SysInfo.m_strLinkMsg += (SysInfo.m_strLinkMsg == "" ? "" : ",") + "  寻迹相机联机失败";

            Init_Tofd();
            Thread_ReadUI();
            #endregion  4
          

            #region 5 界面初始化
            Init_Screen();
        //    SetTran(Pic_D, Pic_Son_X);
            SysInfo.g_Msg_InterFace.Inter_BrushCurrWeld -= new ClassLib_TestData.MsgInterFace.OnBrushCurrWeld(BrushCurrWeld);
            SysInfo.g_Msg_InterFace.Inter_BrushCurrWeld += new ClassLib_TestData.MsgInterFace.OnBrushCurrWeld(BrushCurrWeld);

            SysInfo.g_Msg_InterFace.Inter_BrushBiaoZhu_Posit -= new ClassLib_TestData.MsgInterFace.OnBrushBiaoZhu_Posit(Brush_Posit);
            SysInfo.g_Msg_InterFace.Inter_BrushBiaoZhu_Posit += new ClassLib_TestData.MsgInterFace.OnBrushBiaoZhu_Posit(Brush_Posit);

            switch (SysInfo.iNetTrue_Video)
            {
                case 0:
                    _strMsg = "";
                    break;
                case 1:
                    _strMsg = "2号相机联机失败";
                    break;
                case 2:
                    _strMsg = "1号相机联机失败";
                    break;
                case 3:
                    _strMsg = "两个相机联机失败";
                    break;
            }
            if (_strMsg != "")
            {
                SysInfo.m_strLinkMsg += (SysInfo.m_strLinkMsg != "" ? ", " : "") + _strMsg;
            }
            m_ServerUI.SendData("", "7.0");
            #endregion 4

            m_blActive = true;

        }

        /// <summary>
        /// 平板电池通讯板联机
        /// </summary>
        private void PowerLink()
        {
            try
            {
                if (SysInfo.m_PcPower == null || SysInfo.m_PcPower != null && SysInfo.m_PcPower.m_blNetLink == false) //   if (SysInfo.m_SysInfo.g_Gate.Com_Power_Com != SysInfo.m_SysInfo.g_Climb.Com_portName_Climb)
                {
                    if (SysInfo.m_PcPower != null) SysInfo.m_PcPower.CloseCom();//失败就释放资源
                    SysInfo.m_PcPower = new Cmm_PcPower.ClasPcPower();//= new SysInfo.ClasPcPower();
                    SysInfo.m_PcPower.m_blNetLink = false;
                    int iTimes = 0;
                    string _stCom = "";
                   
                    while (true)//获得系统串口
                    {
                        m_arrPort_Names = System.IO.Ports.SerialPort.GetPortNames();
                        for (int iT = 0; iT < m_arrPort_Names.Count(); iT++)
                            _stCom += (iT == 0 ? "" : ",") + m_arrPort_Names[iT];
                        if (m_arrPort_Names.Count() > 0) break;
                        SysInfo.WaitTime(0.05f);
                        iTimes++;
                        if (iTimes == 3) break;
                    }
                    m_blArrLinkState = new bool[m_arrPort_Names.Count()];
                    if (m_arrPort_Names.Count() > 0)
                    {
                        DateTime dtStar;
                        for (int iN = 0; iN < m_arrPort_Names.Count(); iN++)
                        {
                            SysInfo.m_PcPower.InitCom(m_arrPort_Names[iN] + "," + "4800,n,8,1");
                            SysInfo.m_PcPower.SendData();

                            dtStar = DateTime.Now;
                            while (true)
                            {
                                try
                                {
                                    if (SysInfo.m_PcPower.m_strPowerPC != "") break;
                                    Application.DoEvents();
                                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > 1) break;
                                    Thread.Sleep(10);
                                }
                                catch { break; }
                            }
                            if (SysInfo.m_PcPower.m_strPowerPC != "")
                            {
                                m_blArrLinkState[iN] = true;//m_arrPort_Names
                                SysInfo.m_PcPower.m_blNetLink = true;
                                break;
                            }
                        }
                    }
                    if (SysInfo.m_PcPower.m_blNetLink == false) SysInfo.m_PcPower.CloseCom();//失败就释放资源
                }
            }
            catch { }
        }
        #region 相机联机
        private void LinkServer()
        {
            m_ServerUI.Start();
            SysInfo.csInter.WaitTime(0.5);
        }
        private void Link_New_V(bool blRun = true)
        {
            if (blRun == false)
                m_ServerUI.SendData("", "99");
           
            if (m_ServerUI.m_TcpC != null)
            {
                if (m_ServerUI.m_TcpC.Connected == false)
                {
                    LinkServer();
                    SysInfo.csInter.WaitTime(0.1);
                }
            }
            // MessageBox.Show("3");
                    m_p = new System.Diagnostics.Process();
           
            SysInfo.csInter.INIWriteValue ("VIDEO", "HaveRun", "0", SysInfo.HardFileName);
            m_p.StartInfo.FileName = Application.StartupPath + "\\ClimbVideo.exe";
            m_p.StartInfo.UseShellExecute = false;
            try
            {
                m_p.Start();
                m_p.WaitForInputIdle();

                DateTime dtStar = DateTime.Now;
                bool _blOutT = false;
                while (true)
                {
                    Application.DoEvents();
                    if (SysInfo.csInter.INIReadValue ("VIDEO", "HaveRun", "1", SysInfo.HardFileName) == "1")
                        break;

                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > 20)
                    {
                        _blOutT = true; break;
                    }
                }
                VideoSize();
            }
            catch { }
            
            ShowVideoFBM();
        }
        /// <summary>
        /// 视频尺寸调整
        /// </summary>
        private void VideoSize(int iType = 0)
        {
            try
            {
                Pan_Video.Left = 0;
                Pan_Video.Top = 0;
                Pan_Video.Width = Pan_V.Width - Rad_V_F.Width-7;// +30;
                Pan_Video.Height = Pan_V.Height;
              
                SetParent(m_p.MainWindowHandle, Pan_Video.Handle);// SetParent(p.MainWindowHandle, panel1.Handle);
                if (iType == 1)
                    MoveWindow(m_p.MainWindowHandle, -7,-7, Pan_Video.Width , Pan_V.Height+10 , true);//+ 40
                else
                    MoveWindow(m_p.MainWindowHandle, -8,-8, Pan_Video.Width+20 , Pan_V.Height +16, true);
            }
            catch { }//-7  -32
        }
        private void ShowVideoFBM()
        {
            string strMsg = "";
            #region 是否有此摄像头
            if ( SysInfo.m_iShowPhone == 0)//前
                strMsg = SysInfo.m_iLanguage == 0 ? "前" : "Front";
            if ( SysInfo.m_iShowPhone == 1)//后
                strMsg = SysInfo.m_iLanguage == 0 ? "后" : "Rear";
          
            #endregion

            m_ServerUI.SendData((SysInfo.m_iLanguage == 0 ? "" : "") + strMsg, "4.0");
            SysInfo.csInter.WaitTime(0.2);
            m_ServerUI.SendData((SysInfo.m_iShowPhone == 0 ? 1 : 2).ToString(), "5.0");
        }
        #endregion  相机联机
        private void ShowCam_Xm()
        {
            while (SysInfo.m_iRun != 10)
            {
                switch (m_Cam_Xm.m_iF_B)
                {
                    case 1:
                        if (m_Cam_Xm.m_blReLink_1 == 0 )
                        {
                            ShowVideo(m_Cam_Xm.m_Img_L);
                        }
                        break;
                    case 2:
                        if ( m_Cam_Xm.m_blReLink_2 == 0)
                        {
                            ShowVideo(m_Cam_Xm.m_Img_R);
                        }
                        break;
                    default:
                        Thread.Sleep(100);
                        break;
                }
               
            }
        }
        private void ShowVideo(byte[] btArr)
        {
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_ShowVideo_Xm del_Show = new Delg_ShowVideo_Xm(ShowImage_2);
                    this.Invoke(del_Show, new object[] { btArr });
                }
                catch { }
            }
            else
            {
                ShowImage_2(btArr);
            }
        }
        /// <summary>
        /// 网络相机实时数据
        /// </summary>
        public static Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> m_ImgDat;
        private void ShowImage_2(byte[] ImgDate)
        {
            m_ImgDat = new Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte>(m_Cam_Xm.m_iW_iMG, m_Cam_Xm.m_iH_iMG);
       //     Pic_V.Image = m_ImgDat.Bitmap;
        }
        /// <summary>
        /// 图标显示位置
        /// </summary>
        /// <param name="iX"></param>
        /// <param name="iY"></param>
        private void Brush_Posit(int iX,int iY)
        {
            if (iX >= 0)
            {
                Bt_BZ_Point.Left = iX * SysInfo.m_Plant.Scree_iDotWith_X- SysInfo.m_Plant.Scree_iDotWith_X;// - Bt_BZ_Point.Width /2;// + SysInfo.m_Plant.Chart_Ruler_X_Start;// - Bt_BZ_Point.Width / 2;
                Bt_BZ_Point.Top = (iY + 15) * SysInfo.m_Plant.Scree_iDotHeight + SysInfo.m_Plant.Chart_Ruler_Y_Start;
                Bt_BZ_Point.Visible = true;
            }
            else
                Bt_BZ_Point.Visible = false ;
        }
        /// <summary>
        /// 刷新当前焊缝D扫描数据
        /// </summary>
        /// <param name="iData">0:提醒项目名称 1：刷新焊缝数据</param>
        private void BrushCurrWeld(int iData)
        {
            ShowMainTitl(iData);
        }
        private void ShowMainTitl(int iType = 0)
        {
            switch (iType)
            {
                case 0:
                    this.Text = SysInfo.m_strProName; break;
                case 1:
                    Bt_Data_Left.Visible = false;
                    Bt_Data_Right.Visible = false;

                    this.Text = SysInfo.m_strProName + "      " + "当前项目名称: " + SysInfo.m_Test_Item.ItemName;
                    break;
                case 2:
                    this.Text = SysInfo.m_strProName + "      " + "当前项目名称: " + SysInfo.m_Test_Item.ItemName + "    " + "焊缝编号: " + SysInfo.m_Test_Parts.Part_No;
                    SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo = 0;
                    SysInfo.m_Plant.g_iCurrUse_ScreenNo = -1;
                    Brush_D_OldData(SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo);
                    Bt_Data_Left.Visible = true;
                    Bt_Data_Right.Visible = true;
                    break;
                case 3:
                    Bt_Data_Left.Visible = false;
                    Bt_Data_Right.Visible = false;

                    this.Text = SysInfo.m_strProName + "      " + "当前项目名称: " + SysInfo.m_Test_Item.ItemName + "    " + "焊缝编号: " + SysInfo.m_Test_Parts.Part_No;
                    break;
            }
        }
        private void Tofd_Init_Link()
        {
            if (Thread_Tofd_Link != null) Thread_Tofd_Link.Abort();
            Thread_Tofd_Link = new Thread(new ThreadStart(Tofd_Link));

            Thread_Tofd_Link.Name = "Thread_Tofd_Link";
            Thread_Tofd_Link.IsBackground = true;
            Thread_Tofd_Link.Start();
        }
        private void Tofd_Link()
        {
            SysInfo.m_Tofd_DLL.Link();
            if (SysInfo.m_strLinkMsg != "")
                MessageBox.Show(SysInfo.m_strLinkMsg);
        }
        /// <summary>
        /// 启动TOFD数据处理线程
        /// </summary>
        private void Thread_ReadUI()
        {
            if (SysInfo . Thread_Get_TofdData != null) SysInfo.Thread_Get_TofdData.Abort();
            SysInfo.Thread_Get_TofdData = new Thread(new ThreadStart(Read_TOFD));

            SysInfo.Thread_Get_TofdData.Name = "Thread_Read_Tofd";
            SysInfo.Thread_Get_TofdData.IsBackground = true;
            SysInfo.Thread_Get_TofdData.Start();
        }
        /// <summary>
        /// 获得TOFD数据进行处理   单调
        /// </summary>
        private void Read_TOFD()
        {
            while (SysInfo.m_iRun != 10)
            {
                SysInfo.m_Tofd_DLL.GetDistan();
                if (SysInfo.m_iRun != 3)
                {
                    if (SysInfo.m_Tofd_DLL.m_i_State == 0)
                    {
                        //1 读取数据
                        if (SysInfo.m_Tofd_DLL.blNetLink ||
                            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 2 ||
                            SysInfo.m_Plant.m_bl_Ck_Wave)
                            SysInfo.m_Tofd_DLL.setPeakBuffer(SysInfo.m_Tofd_DLL.m_fWaveFramePerHeight,
                                                             SysInfo.m_Tofd_DLL.m_fWaveFramePerWidth,
                                                             SysInfo.m_iRun);
                        if (SysInfo.m_PcPower != null)
                        {
                            if (SysInfo.m_PcPower.m_blNetLink && m_iPcPower_Times == 0)
                            {
                                SysInfo.m_PcPower.SendData();
                                m_iPcPower_Times++;
                                if (m_iPcPower_Times == 100) m_iPcPower_Times = 0;
                            }
                        }
                        //2 显示波形
                        #region 画图
                        if (this.InvokeRequired == true)
                        {
                            try
                            {
                                Delg_PlantAllWave ms = new Delg_PlantAllWave(PlantWave);
                                this.Invoke(ms, new object[] { });
                            }
                            catch
                            { }
                        }
                        else
                        {
                            PlantWave();
                        }
                        #endregion
                    }
                }
                //3 如果正式测量，就保存数据
                if (SysInfo.m_iRun == 1 && SysInfo.m_Test_Record.flDistance_X>=0)
                {
                    #region 将数据保存到list，为文件保存准备数据
                    Tofd_Arr _CurrData = new Tofd_Arr();
                  
                    var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_CurrData.ChannelBuf, 0);
                    Marshal.Copy(Tofd.m_pChannelBuf, 0, IntPtArr, Tofd.UTS_DATA_WIDTH);

                    IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_CurrData.ValueBuf , 0);
                    Marshal.Copy(Tofd.m_pValueBuf, 0, IntPtArr, Tofd.UTS_DATA_WIDTH);
                    Tofd.m_lstTofd.Add(_CurrData);
                    #endregion

                    #region 保存到数据库
                    SysInfo.m_Test_Record.flDistance_X = (float)SysInfo.m_Climb.Trip;
                    SysInfo.m_Test_Record.strMarking = SysInfo.m_Tofd_DLL.blAlarm ? "1" : "0";
                    SysInfo.m_Test_Record.strData = SysInfo.m_Tofd_DLL.strWave;
                    SysInfo.m_Test_Record.strData_2 = SysInfo.m_Tofd_DLL.strWave_2;
                    SysInfo.m_Test_Record.strWave_Time = SysInfo.m_Tofd_DLL.strWave_Time;

                    DbGlobal.ImTest_Records.SaveData(SysInfo.m_Test_Record);
                    #endregion 
                }
                //4 如果需要延时，就延时
                if (SysInfo.m_Tofd_DLL.flWaitTime > 0) SysInfo.WaitTime( SysInfo.m_Tofd_DLL.flWaitTime);
            }
        }
        float fl_D = 0;
        /// <summary>
        /// 画图：A扫  D扫  单调
        /// </summary>
        private void PlantWave()
        {
            #region A扫图
            SysInfo.m_Plant.Plant_A(Pic_A, SysInfo.m_Tofd_DLL);

            ShowPwx(m_i_Pbl_Type, Pic_D);
            #endregion A 扫
          
            #region 平板电池容量
            fl_D = float.Parse(SysInfo.m_PcPower.m_strPowerPC==""?"0": SysInfo.m_PcPower.m_strPowerPC);//     m_PcPower.m_strRetDat);
            Lb_PC.Text = "PC:" + fl_D + "%";

            if (fl_D > 0 && fl_D <= 100)
                Prg_Bar_PC.Value = (int)fl_D;
            #endregion

            #region 寻迹激光器画图
            if (m_blXunJi_Prog_0PC_1YY == false)
            {
                //1 给车体发送数据
                if(m_i_WaitTimeNum== m_i_WaitTimeNum_Max)
                SendData_4_Climb(m_iData_With, 0, m_Hd850.g_WeldPosition.i_Cent / 10, 0, m_Hd850.g_WeldPosition.f_K);
                #region 2 画图
                if (m_ckAutoJZ)
                {
                    m_iTimes++;
                    if (m_blTimes == false && m_iTimes >= 10 && m_Hd850.g_profileCnt > 0)//有数据
                    {
                        m_blTimes = true;
                        m_Hd850.g_blAngle_JZ = true;
                        LaserHoriz();
                        m_ckAutoJZ = false;
                    }
                }
                if (m_Insp_Data.g_Dic_Alarm.ContainsKey(m_Hd850.g_iKey_Dist_Frame))
                {
                    Frame_Work.Class_X_Data _X_Data = m_Insp_Data.g_Dic_Alarm[m_Hd850.g_iKey_Dist_Frame];
                    m_Hd850.Draw_LunKuoTu(ref Lb_LunKuo, ref _X_Data);
                }
                #endregion 2画图
            }
            #endregion  寻迹激光器画图

            #region D扫
            if (SysInfo.m_iRun == 1)
            {
                SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo = SysInfo.m_Plant.Get_No(SysInfo.m_Climb.Trip_mm);
                //是否画新屏幕
                Brush_D_OldData(SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo);
                //画D扫描
                SysInfo.m_Plant.Plant_D(Pic_D, SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo, SysInfo.m_Tofd_DLL);
                Bt_Run_Posit.Left = SysInfo.m_Plant.m_iCurr_Run_Position - Bt_Run_Posit.Width;
            }
            #endregion D扫
        }
        /// <summary>
        /// 激光器水平线校正
        /// </summary>
        private void LaserHoriz()
        {
            if (m_Hd850.g_dbOutCentWeld < 20)
                m_Hd850.LeftAdRightHorizontal_correction_Get(45, 50);
            else
                m_Hd850.LeftAdRightHorizontal_correction_Get(1, 5);
            while (m_Hd850.g_profileCnt > 0 && JugLval_Rval() > 0.1)
            {
                m_Hd850.LeftAdRightHorizontal_correction_Set();
                break;
            }
        }
        /// <summary>
        /// 判断左右数据不平衡
        /// </summary>
        private double JugLval_Rval()
        {
            double _dbLval = 0, _dbRval = 0, _dbL_Allval = 0, _dbR_Allval = 0;
            int _iL_Num = 0, _iR_Num = 0;
            for (int i = 0; i < 50; i++)
            {
                _dbLval = m_Hd850.g_profileZ[i];
                if (_dbLval > 0)
                {
                    _iL_Num++;
                    _dbL_Allval += _dbLval;
                }
                _dbRval = m_Hd850.g_profileZ[m_Hd850.g_profileCnt - 1 - i];
                if (_dbRval > 0)
                {
                    _iR_Num++;
                    _dbR_Allval += _dbRval;
                }
            }
            _dbL_Allval /= _iL_Num;
            _dbR_Allval /= _iR_Num;
            m_Hd850.g_blAngle_JZ = Math.Abs(_dbL_Allval - _dbR_Allval) <= 0.1;
            return Math.Abs(_dbL_Allval - _dbR_Allval);
        }

        /// <summary>
        /// 给车体发送数据
        /// </summary>
        /// <param name="iWith">屏幕宽度 4位</param>
        /// <param name="iHeght">屏幕宽度 4位</param>
        /// <param name="iCenter_X">中心点 列号 4位</param>
        /// <param name=" iCent_Y">中心点 行号 4位</param>
        /// <param name="fl_K">色带斜率 共6位： 1位符号 4位整数1位小数</param>
        /// <returns></returns>
        public bool SendData_4_Climb(int iWith, int iHeght, int iCenter_X, int iCent_Y, float fl_K)
        {
            string _strD = "";
            string _strD_16 = "";
            string[] _sPara = "".Split('.');
            bool _blRet = false;
            int _iNo = 0;
            m_i_WaitTimeNum = 0;
            try
            {
                //1 帧头
                m_btArr_SendVideo[0] = 0xFE;
                m_btArr_SendVideo[1] = 0xFE;
                m_btArr_SendVideo[2] = 0xFE;

                //2 结束符
                m_btArr_SendVideo[25] = 0x0D;

                #region  1 宽度
                _strD = iWith.ToString();
                _strD = _strD.PadLeft(4, '0');
                _iNo = 3;//数据开始序号
                for (int i = 0; i < _strD.Length; i++)
                {
                    _strD_16 = Convert.ToString(int.Parse(_strD.Substring(i, 1)), 10);
                    m_btArr_SendVideo[i + _iNo] = Convert.ToByte(_strD_16, 16);
                }
                #endregion 1 
                #region 2 高度度
                _strD = iHeght.ToString();
                _strD = _strD.PadLeft(4, '0');
                _iNo = 3 + 4;//数据开始序号
                for (int i = 0; i < _strD.Length; i++)
                {
                    _strD_16 = Convert.ToString(int.Parse(_strD.Substring(i, 1)), 10);
                    m_btArr_SendVideo[i + _iNo] = Convert.ToByte(_strD_16, 16);
                }
                #endregion 2
                #region 3 中心点 行号
                _strD = iCent_Y.ToString();
                _strD = _strD.PadLeft(4, '0');
                _iNo = 7 + 4;//数据开始序号
                for (int i = 0; i < _strD.Length; i++)
                {
                    _strD_16 = Convert.ToString(int.Parse(_strD.Substring(i, 1)), 10);
                    m_btArr_SendVideo[i + _iNo] = Convert.ToByte(_strD_16, 16);
                }
                #endregion 3
                #region 4 中心点 列号
                _strD = iCenter_X.ToString();
                _strD = _strD.PadLeft(4, '0');
                _iNo = 11 + 4;//数据开始序号
                for (int i = 0; i < _strD.Length; i++)
                {
                    _strD_16 = Convert.ToString(int.Parse(_strD.Substring(i, 1)), 10);
                    m_btArr_SendVideo[i + _iNo] = Convert.ToByte(_strD_16, 16);
                }
                #endregion 4
                #region 5 频率
                int _iZf = fl_K > 0 ? 0 : 1;
                fl_K = Math.Abs(fl_K);
                _sPara = fl_K.ToString("f1").Split('.');
                _iNo = 15 + 4;//数据开始序号

                if (_sPara.Length == 2)
                {
                    _strD = _iZf.ToString() + _sPara[0].PadLeft(4, '0') + _sPara[1];
                    for (int i = 0; i < _strD.Length; i++)
                    {
                        _strD_16 = Convert.ToString(int.Parse(_strD.Substring(i, 1)), 10);
                        m_btArr_SendVideo[i + _iNo] = Convert.ToByte(_strD_16, 16);
                    }
                }
                #endregion 5
                #region  6 发送 m_btArr_SendVideo
                if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    SysInfo.m_Climb4.SendData(5, 3, 0, 0, iWith.ToString () +"," + iCenter_X.ToString ()+","+ fl_K.ToString ());
                #endregion 6

                #region 7 显示
                SysInfo.m_strComMsg = TcpClient_UI.byteToHexStr(m_btArr_SendVideo);
                ShowCom();
                #endregion 7
            }
            catch { }
            return _blRet;
        }
        /// <summary>
        /// 设置picturebox叠加，上层透明
        /// </summary>
        /// <param name="pic_Parents"></param>
        /// <param name="pic_Son"></param>
        private void SetTran(PictureBox pic_Parents, PictureBox pic_Son)
        {
            pic_Parents.SendToBack();
            pic_Son.BackColor = Color.Transparent;
            pic_Son.Parent = pic_Parents;
            pic_Son.BringToFront();

            pic_Son.Visible = true;
        }
        /// <summary>
        /// 画抛物线
        /// </summary>
        /// <param name="iType"></param>
        /// <param name="Pic_Son"></param>
        private void ShowPwx(int iType,PictureBox Pic_Son)
        {
            int _iWith = 100;// Pic_Son.ClientSize.Width;
            float _iWith2 = _iWith / 2f;
           // Pic_Son.ClientSize.Height;
            Bitmap image = new Bitmap(Pic_Son.ClientSize.Width, Pic_Son.ClientSize.Height );
            Graphics g = Graphics.FromImage(image);
            #region 画抛物线
           Pen pen_Pbl = new Pen(Color.Blue , 1); 
         
            m_Pit_Pbl = new PointF[_iWith];

            float  Y = 0,X=0;
            float _iX = 0,_iY=0,flT=0, _flMax_Y=0, flTiTl_x=0;
            int iNo = 0;
            switch (iType )
            {
                case 0://开口朝下  X2=-2py
                    _iX =  _iWith2;
                     _flMax_Y = m_iHeight / ((_iWith2) * (_iWith2) /(-2 * m_Pbl_P ) );
                    #region  1 计算左半部分
                    while (iNo<50)//   _iX >0 &&  Y <= _iHeight +2)
                    {
                        //1 计算当前数
                        X = _iX * _iX;
                        Y = X / (-2 * m_Pbl_P);
                        Y *= _flMax_Y;
                        m_Pit_Pbl[iNo] = new PointF(SysInfo .m_Plant .m_i_X_D   -_iX, SysInfo.m_Plant.m_i_Y_D + Y);
                        iNo++; 
                        _iX = _iWith2 - iNo;
                    }
                    iNo--;
                    #endregion  1

                    #region 2 计算右半部分
                    _iX = 0;
                    _iY = 0;
                    iNo++;
                    while (_iX < _iWith2 && _iY <= m_iHeight + 2)
                    {
                        //1 计算当前数
                        X = _iX * _iX;
                        Y = X / (-2 * m_Pbl_P);
                        Y *= _flMax_Y;

                        m_Pit_Pbl[iNo] = new PointF(SysInfo.m_Plant.m_i_X_D   + _iX, SysInfo.m_Plant.m_i_Y_D + Y);
                        iNo++;
                        _iX ++;
                    }

               
                    #endregion 2 
                    break;
                case 1://开口朝上 x2=2py
                    _iX = _iWith2;
                     _flMax_Y = (m_iHeight - 2) / ((_iWith2) * (_iWith2) / (2 * m_Pbl_P));
                    #region  1 计算左半部分
                    while (_iX > 0 && Y <= m_iHeight + 2)
                    {
                        //1 计算当前数
                        X = _iX * _iX;
                        Y = X / (2 * m_Pbl_P);
                        Y *= _flMax_Y;
                        Y = m_iHeight - Y;
             
                        m_Pit_Pbl[iNo] = new PointF(SysInfo.m_Plant.m_i_X_D - _iX, SysInfo.m_Plant.m_i_Y_D + Y- m_iHeight);
                        iNo++;
                        _iX = _iWith2 - iNo;
                    }
                    iNo--;
                    #endregion  1

                    #region 2 计算右半部分
                    _iX = 0;
                    _iY = 0;
                    iNo++;
                    while (_iX < _iWith2 && _iY <= m_iHeight + 2)
                    {
                        //1 计算当前数
                        X = _iX * _iX;
                        Y = X / (2 * m_Pbl_P);
                        Y *= _flMax_Y;
                        Y = m_iHeight - Y;
                        m_Pit_Pbl[iNo] = new PointF(SysInfo.m_Plant.m_i_X_D + _iX, SysInfo.m_Plant.m_i_Y_D + Y- m_iHeight);
                        iNo++;
                        _iX++;
                    }
                    #endregion 2 
                    break;
                case 2://开口朝右 y2=-2px
                  
                    break;
                case 3://开口朝左 y2=2px

                    break;
            } 
            if(m_Pit_Pbl!=null )
            g.DrawLines(pen_Pbl, m_Pit_Pbl);

            #region 画十字
            pen_Pbl = new Pen(Color.Red , 1);
            g.DrawLine(pen_Pbl, new Point(SysInfo.m_Plant.m_i_X_D - 55, SysInfo.m_Plant.m_i_Y_D),
                                                    new Point(SysInfo.m_Plant.m_i_X_D + 55, SysInfo.m_Plant.m_i_Y_D));
            g.DrawLine(pen_Pbl, new Point(SysInfo.m_Plant.m_i_X_D, SysInfo.m_Plant.Chart_Ruler_Y_Start),
                                                  new Point(SysInfo.m_Plant.m_i_X_D, Pic_D .Height-25 ));
            #endregion  

            #endregion
            Pic_Son.Image = image;
        }
     
        private void ShowOneSon(int iCol)
        {
        //    Bitmap image = new Bitmap(Pic_Son_X.ClientSize.Width, Pic_Son_X.ClientSize.Height);

        //    // 获取背景层
        //    Bitmap bg = (Bitmap)Pic_Son_X.BackgroundImage;
        //    // 初始化整个画布
        //    Bitmap canvas = new Bitmap(Pic_Son_X.ClientSize.Width, Pic_Son_X.ClientSize.Height);
        //    //初始化图形面板，获取这块内存画布的Graphics的引用
        //    Graphics g = Graphics.FromImage(image);
        //    Graphics gb = Graphics.FromImage(canvas);
        //    g.Clear(Color.Black);

        //    Pen pen = new Pen(Brushes.White, 2);
        ////    g.DrawLine(pen, iCol, SysInfo.m_SysInfo.g_ScreenPlant.Chart_Ruler_Y_Start + 6, iCol, Pic_OneSon.Height - 10);


        //    Rectangle _Rect = new Rectangle(0, 0, Pic_Son_X.Width, Pic_Son_X.Height);

        //    if (bg != null)
        //        gb.DrawImage(bg, _Rect);// 先绘制背景层
        //    gb.DrawImage(image, _Rect); // 再绘制绘画层

        //    Pic_Son_X.BackgroundImage = (Bitmap)canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
        //    g.Dispose(); g = null;
        //    image.Dispose();
        //    gb.Dispose();
        //    canvas.Dispose();
        }
        /// <summary>
        /// 画指定屏幕数据的D图
        /// </summary>
        private float   Brush_D_OldData(int iScreenNo)
        {
            float _flLast_X = -1;
            if (iScreenNo != SysInfo.m_Plant.g_iCurrUse_ScreenNo)
            {
                //1 画新界面
                SysInfo.m_Plant.g_iCurrUse_ScreenNo = iScreenNo;

                SysInfo.m_Plant.GetRulerPara_C(Pic_D, Tofd.UTS_DATA_WIDTH);
                SysInfo.m_Plant.Plant_Ruler_C(Pic_D, SysInfo.m_Plant.g_iCurrUse_ScreenNo);

                //2 得到当前屏幕数据，
                DbGlobal.ImTest_Records.DbTableName= SysInfo.GetCurrDataTableName(1, SysInfo.m_Test_Item.ID, SysInfo.m_Test_Item.Dwmc);
                 m_lstRec = DbGlobal.ImTest_Records.GetData(SysInfo.m_Test_Parts.ID,
                                    SysInfo.m_Test_Parts.Sub_ID,
                                    SysInfo.m_Plant.lst_Screenkd[SysInfo.m_Plant.g_iCurrUse_ScreenNo].Chart_Run_flStart_Distance,
                                   SysInfo.m_Plant.lst_Screenkd[SysInfo.m_Plant.g_iCurrUse_ScreenNo].Chart_Run_flEnd_Distance);
                //3 画老数据
                string[] _sPara = "".Split(',');
                
                int _iNum = m_lstRec.Count;
                SysInfo.m_Plant.m_bl_His_Data = _iNum > 0;
                for (int i = 0; i < _iNum; i++)
                {
                    SysInfo.m_Tofd_DLL.m_ArrWave_Time = m_lstRec[i].strWave_Time.Split(',');
                    _flLast_X = m_lstRec[i].flDistance_X;
                  
                    _sPara = m_lstRec[i].strData.Split('|');
                    if (_sPara.Length == 2)
                    {
                        SysInfo.m_Tofd_DLL.m_ArrWave = _sPara[1].Split(',');
                        SysInfo.m_Tofd_DLL.m_flDistanc_X = _flLast_X;
                        SysInfo.m_Plant.Plant_D(Pic_D, iScreenNo, SysInfo.m_Tofd_DLL, 1,i== _iNum-1);
                    }
                }
            }
            return _flLast_X;
        }
       
        private void ShowServeData()
        {
            try
            {
                if (SysInfo.m_iTrack_Type == 2)
                {
                    if (SysInfo.m_lst_Weld.Count > 0)
                    {

                        ShowTitl();

                        SysInfo.m_lst_Weld.RemoveAt(0);
                    }
                }
            }
            catch { }
        }
        delegate void Delg_ShowTiTl();
        private void ShowTitl()
        {

            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_ShowTiTl del_Limit = new Delg_ShowTiTl(MsgShow);
                    this.Invoke(del_Limit, new object[] { });
                }
                catch { }
            }
            else
            {
                MsgShow();
            }
        }
        private void MsgShow()
        {
            if (SysInfo.m_iTrack_Type == 2)
            {
                if (Lb_LunKuo.Visible == false) Lb_LunKuo.Visible = true;
                DrawImageProflie(ref Lb_LunKuo, m_maxX, m_maxZ,
                                m_scaleX, m_scaleZ, ref m_Axis_X, ref SysInfo.m_lst_Weld[0].dbArrData,
                                SysInfo.m_lst_Weld[0], SysInfo.m_lst_Weld[0].dbArrData.Count());
                SysInfo.m_strComMsg = SysInfo.m_lst_Weld[0].strFrame;
                ShowCom();
            }
        }
        private void ShowCom()
        {
            Txt_Com.Text += DateTime.Now.ToString("HH:mm:ss:fff") + " " + SysInfo.m_strComMsg + "\r\n";
            Txt_Com.SelectionStart = Txt_Com.Text.Length;

            Txt_Com.ScrollToCaret();
            m_i_Com_Num++;

            if (m_i_Com_Num > 30)
            {
                Txt_Com.Text = "";
                m_i_Com_Num = 0;
            }
        }
        //画焊缝
        private void DrawImageProflie(ref Label hWnd, double MaxX, double MaxZ,
                                      double ScaleX, double ScaleZ, ref float[] Axis_X, ref double[] Axis_Z,
                                      Class_Xj_GetData WeldPosition, int AxisCount)
        {
            Bitmap bmp = new Bitmap((int)hWnd.Width, (int)hWnd.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(0, 0, 0));// g.Clear(Color.FromArgb(0, 125, 125));
            Color colorGreen = Color.FromArgb(0, 200, 0);//颜色为绿色
            Color colorB = Color.FromArgb(255, 0, 0);//颜色为绿色
            Color colorRed = Color.FromArgb(255, 0, 0);//颜色为红色
            Color colorGray = Color.FromArgb(200, 200, 200);//颜色为白色
            Color colorBlack = Color.FromArgb(30, 30, 30);//颜色为黑色
            Color colorYellow = Color.FromArgb(200, 200, 0);//颜色为黑色
            Pen PGreen = new Pen(colorGreen, 2);//创建一个画笔对象,该画笔的颜色为绿色，笔触大小为2个像素
            Pen pRed = new Pen(colorB, 4);//创建一个画笔对象,该画笔的颜色为黄色，笔触大小为1个像素
            Pen pGray = new Pen(Color.FromArgb(200, 200, 200), 1);//创建一个画笔对象,该画笔的颜色为灰色，笔触大小为1个像素

            Pen pGray_R = new Pen(Color.FromArgb(0, 0, 255), 1);//创建一个画笔对象,该画笔的颜色为灰色，笔触大小为1个像素
            Pen pBalck = new Pen(colorBlack, 3);//创建一个画笔对象,该画笔的颜色为白色，笔触大小为3个像素
            Pen pYellow = new Pen(colorYellow, 3);//创建一个画笔对象,该画笔的颜色为白色，笔触大小为1个像素
            pGray.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            Font font = new Font("Adobe Gothic Std", 9f, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.FromArgb(255, 255, 255)); //Brush brush = new SolidBrush(Color.FromArgb(0, 0, 0));
            Brush brush_R = new SolidBrush(Color.FromArgb(255, 0, 0));
            uint axisInfo_top = 20;
            uint axisInfo_right = (uint)hWnd.Width - 20;
            uint axisInfo_left = 20;
            uint axisInfo_bottom = (uint)hWnd.Height - 30;
            g.DrawLine(pBalck, axisInfo_left, axisInfo_bottom, axisInfo_left, axisInfo_top);
            g.DrawLine(pBalck, axisInfo_left, axisInfo_bottom, axisInfo_right, axisInfo_bottom);
            //需设置一个页面的X及Z的显示范围及分度格数
            uint xNum, zNum;
            xNum = (uint)(MaxX / ScaleX);//x方向格子总数量
            zNum = (uint)(MaxZ / ScaleZ);//z方向格子总数量
            double XZoom, ZZoom;
            XZoom = (axisInfo_right - axisInfo_left) / MaxX;
            ZZoom = (axisInfo_bottom - axisInfo_top) / MaxZ;
            float scaleXlen = (float)(ScaleX * XZoom);
            float scaleZlen = (float)(ScaleZ * ZZoom);
            string str;
            int m_Weld_Type = 1;
            int _iShowNo = -1;
            //绘制刻度
            for (uint i = 1; i <= zNum; i++)
            {
                g.DrawLine(pGray, axisInfo_left, axisInfo_bottom - scaleZlen * i, axisInfo_right, axisInfo_bottom - scaleZlen * i);
                str = (ScaleZ * i).ToString();
                g.DrawString(str, font, brush, axisInfo_left - 10, axisInfo_bottom - scaleZlen * i);
            }
            g.DrawString("采集区域 ：" + (m_Weld_Type == 0 ? "0 - 15" : "0 - 15 45-50"), font, brush, 50, axisInfo_bottom + 15);
            for (uint j = 1; j <= xNum; j++)
            {
                if (j == 1 || j == 9)//&& m_Weld_Type == 1)
                    g.DrawLine(pYellow, axisInfo_left + scaleXlen * j, axisInfo_bottom, axisInfo_left + scaleXlen * j, axisInfo_top);
                else
                    g.DrawLine(pGray, axisInfo_left + scaleXlen * j, axisInfo_bottom, axisInfo_left + scaleXlen * j, axisInfo_top);

                //       g.DrawLine(j == 5 ? pGray_R:  pGray, axisInfo_left + scaleXlen * j, axisInfo_bottom, axisInfo_left + scaleXlen * j, axisInfo_top);
                str = (ScaleX * j).ToString();

                g.DrawString(str, font, brush, axisInfo_left - 20 + scaleXlen * j, axisInfo_bottom);
            }
            //绘制轮廓线
            if (AxisCount > 10)
            {


                for (uint k = 0; k < AxisCount - 1; k++)
                {
                    if (k == WeldPosition.i_Cent)
                    {
                        g.DrawString("余高：" + WeldPosition.dbDepth.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-25 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                        g.DrawString("位置：" + (WeldPosition.i_Cent / 10f).ToString("f1") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-48 + axisInfo_bottom - ZZoom * Axis_Z[k]));


                        g.DrawLine(pRed, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-6 + axisInfo_bottom - ZZoom * Axis_Z[k]),
                                  (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(6 + axisInfo_bottom - ZZoom * Axis_Z[k + 0]));
                    }
                    //if (X_Data.m_lstAlarm.Count > 0 || X_Data.m_clsWeld.m_blHave)
                    //{
                    //    int _iShowNo = -1;
                    //    int _iShowNo_Ao = -1;
                    //    if (X_Data.m_clsWeld.m_blHave)//焊缝 Convert.ToString(166, 16)
                    //    {
                    //        if (k == (int)(X_Data.m_clsWeld.m_i_W_Start_X * 10 + (X_Data.m_clsWeld.m_i_W_End_X - X_Data.m_clsWeld.m_i_W_Start_X) * 10 / 2.0f))
                    //        {
                    //            g.DrawString("焊缝余高：" + X_Data.m_clsWeld.m_dbCentWeld_H.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(15 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                    //            g.DrawString("宽：" + X_Data.m_clsWeld.m_dbCentWeld_W.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(35 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                    //            g.DrawString("总高：" + X_Data.m_clsWeld.db_Line_3_Mucai_H.ToString("f2") + "mm", font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(55 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                    //            if (X_Data.m_clsWeld.m_lstJL.Count > 0)
                    //            {
                    //                string _strT = "";
                    //                for (int _i = 0; _i < X_Data.m_clsWeld.m_lstJL.Count; _i++)
                    //                {
                    //                    _strT += X_Data.m_clsWeld.m_lstJL[_i].strType + "" + "\r\n" + X_Data.m_clsWeld.m_lstJL[_i].strJL + "\r\n";
                    //                    g.DrawString(_strT, font, X_Data.m_clsWeld.m_lstJL[_i].iJL == 1 ? brush : brush_R, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(75 + axisInfo_bottom - ZZoom * Axis_Z[k]));
                    //                }
                    //            }


                    //        }
                    if (k >= (int)(WeldPosition.i_Start) && k <= (int)(WeldPosition.i_End))
                        _iShowNo = 1;
                    //    }


                    if (_iShowNo > -1)
                    {
                        g.DrawLine(pRed, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k]),
                                  (float)(axisInfo_left + XZoom * Axis_X[k + 1]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));
                    }
                    else
                        g.DrawLine(PGreen, (float)(axisInfo_left + XZoom * Axis_X[k]),
                                  (float)(axisInfo_bottom - ZZoom * Axis_Z[k]), (float)(axisInfo_left + XZoom * Axis_X[k + 1]),
                                  (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));
                    //    if (_iShowNo_Ao > 0)
                    //g.DrawLine(pRed, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k]),
                    //           (float)(axisInfo_left + XZoom * Axis_X[k + 1]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));

                    //}
                    //else
                    g.DrawLine(pRed, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k]), (float)(axisInfo_left + XZoom * Axis_X[k + 1]),
                                  (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));
                }
            }
            hWnd.CreateGraphics().DrawImage(bmp, 0, 0);
            pRed.Dispose(); PGreen.Dispose(); pYellow.Dispose();
            pGray.Dispose(); pBalck.Dispose(); font.Dispose();
            brush.Dispose(); bmp.Dispose(); g.Dispose();
            pGray_R.Dispose();

        }

        /// <summary>
        /// 刷新视频
        /// </summary>
        /// <param name="Value"></param>
        delegate void Delg_ShowVideo(Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> WBitmap);
        /// <summary>
        /// 雄迈相机显示视频
        /// </summary>
        /// <param name="ImgDate"></param>
        delegate void Delg_ShowVideo_Xm(byte[] ImgDate);
        /// <summary>
        /// 视频0显示
        /// </summary>
        /// <param name="WBitmap"></param>
        private void PubCapture_ImageGrabbed_0(Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> WBitmap)
        {
            if (m_blActive == false) return;
            if (SysInfo.m_iShowPhone != 0) return;
            if (WBitmap != null)
            {
                if (this.InvokeRequired == true)
                {
                    if (SysInfo.iWidth != WBitmap.Width)
                    {
                        SysInfo.iWidth = WBitmap.Width;
                        SysInfo.iHeight = WBitmap.Height;
                    }

                    try
                    {
                        Delg_ShowVideo del_Show = new Delg_ShowVideo(ShowImage);
                        this.Invoke(del_Show, new object[] { WBitmap });
                    }
                    catch { }
                }
                else
                {
                    ShowImage(WBitmap);
                }
            }
        }
        /// <summary>
        /// 视频1显示
        /// </summary>
        /// <param name="WBitmap"></param>
        private void PubCapture_ImageGrabbed_1(Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> WBitmap)
        {
            if (m_blActive == false) return;
            if (SysInfo.m_iShowPhone != 1) return;
            if (WBitmap != null)
            {
                if (this.InvokeRequired == true)
                {
                    if (SysInfo.iWidth != WBitmap.Width)
                    {
                        SysInfo.iWidth = WBitmap.Width;
                        SysInfo.iHeight = WBitmap.Height;
                    }

                    try
                    {
                        Delg_ShowVideo del_Show = new Delg_ShowVideo(ShowImage);
                        this.Invoke(del_Show, new object[] { WBitmap });
                    }
                    catch { }
                }
                else
                {
                    ShowImage(WBitmap);
                }
            }
        }
        private void ShowImage(Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> WBitmap)
        {
         //   Pic_V.Image = WBitmap.Bitmap;
        }
        /// <summary>
        /// TOFD初始化
        /// </summary>
        private void Init_Tofd()
        {
            //1 初始化C扫数据值与颜色对照表
            //Bitmap Rr_Bitmap = new Bitmap(Pic_StandColor.ClientSize.Width, Pic_StandColor.ClientSize.Height);
            //SysInfo.m_Plant.PlanScheme_ColorLimit(Pic_StandColor, ref Rr_Bitmap, 1);
            //SysInfo.m_Plant.SaveStandColor(Rr_Bitmap, Tofd.UTS_DATA_WIDTH);

            //2 D扫图初始化
            Init_PlantD();
            //3 TOFD联机
            SysInfo.m_Tofd_DLL.SetH_W(Pic_A.Width, Pic_A.Height, 0);
            Tofd_Init_Link();// SysInfo.m_Tofd_DLL.Link();
        }
        /// <summary>
        /// 初始化D图
        /// </summary>
        private void Init_PlantD(int iType = 0)
        {
            SysInfo.m_Plant.GetRulerPara_C(Pic_D, Tofd.UTS_DATA_WIDTH);
            if (iType == 0)
                SysInfo.m_Plant.Init_ScreenKd(Pic_D.Width, Pic_D.Height);
            SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X = SysInfo.m_Plant.Scree_iDotWithmm_X;
            SysInfo.m_Plant.Plant_Ruler_C(Pic_D, 0);
        }
        /// <summary>
        /// 界面初始化
        /// </summary>
        private void Init_Screen()
        {
            #region 1 视频和寻迹
            m_i_WaitTimeNum_Max = int.Parse(SysInfo.csInter.IniReadDefine("Zoom", "m_i_WaitTimeNum_Max", "5", SysInfo.HardFileName));
             m_fl_LR_New_R = float.Parse(SysInfo.csInter.IniReadDefine("Zoom", "LR", "50", SysInfo.HardFileName));
          ///   m_fl_UD_New_U = float.Parse(SysInfo.csInter.IniReadDefine("Zoom", "UD", "28.91", SysInfo.HardFileName));

            ShowMainTitl();
            Pan_Xj.Dock = DockStyle.Fill;
         //   Pan_Video.Dock = DockStyle.Fill;
           // Pan_V.Dock =  DockStyle.Fill;
           Pan_V.Visible = true;
          //  Pan_Video.Visible = true;
            Show_F_B(true);
            #endregion  1
        }
        private void Show_F_B(bool blVal)
        {
            Pan_Power.Visible = blVal;
            Ck_Pic_Big1_Lit0.Visible = blVal;
            Rad_V_F.Visible = blVal;
            Rad_V_B.Visible = blVal;
            Rd_2.Visible = blVal;
        }
        private void Pic_V_DoubleClick(object sender, EventArgs e)
        {
            if (m_blBig == false)//小变大
            {
                this.Tb_Up.ColumnStyles[0] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 0);
                this.Tb_Up.ColumnStyles[1] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100);

                Tb_Main_Data.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 100);
                Tb_Main_Data.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0);

                m_blBig = true;
            }
            else//大变小
            {
                this.Tb_Up.ColumnStyles[0] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 50);
                this.Tb_Up.ColumnStyles[1] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 50);

                Tb_Main_Data.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 24.5f);
                Tb_Main_Data.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 75.5f);
                m_blBig = false;
            }
        }
        #region 寻迹
        /// <summary>
        /// 连接寻迹服务器
        /// </summary>
        private void ConnectToServer()
        {
            try
            {
                SysInfo.m_Client = new TcpClient_UI();
                if (TreadTcp_Server != null) TreadTcp_Server.Abort();

                TreadTcp_Server = new Thread(new ThreadStart(Connect));
                TreadTcp_Server.Start();
                TreadTcp_Server.IsBackground = true;
            }
            catch (Exception ex)
            {
            }
        }
        private void Connect()
        {

            SysInfo.m_Client.ConnectToServer(SysInfo.IP_XunJi_Server, "");
        }
        #endregion 寻迹

        private void Rd_2_CheckedChanged(object sender, EventArgs e)
        {
            Rd_2.Visible = false;
           // Pan_V_F_B.Visible = false;
            Show_F_B(false);
            Xj_Sp(true);
        }
        private void Bw_Tx(bool blVal)
        {
            Txt_Com.Visible = blVal;
            Lb_LunKuo.Visible = !blVal;

        }
        private void Xj_Sp(bool blVal)
        {
            Pan_Xj.Visible = blVal;
            Pan_Video  .Visible = !blVal;//Pic_V

        }
        private void Bt_Xj_Exit_Click(object sender, EventArgs e)
        {
            Rd_2.Checked = false;
            Rd_2.Visible = true;
          //  Pan_V_F_B.Visible = true;
            Show_F_B(true);
            Xj_Sp(false);
        }

        private void Ck_Bw1_Tx0_Click(object sender, EventArgs e)
        {
            Bw_Tx(Ck_Bw1_Tx0.Checked);
        }

        private void Txt_Limt_L_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
             if(m_blXunJi_Prog_0PC_1YY ==false )   m_Hd850.g_fl_Limit_L = float.Parse(Txt_Limt_L.Text);
                SysInfo.csInter.INIWriteValue("System", "Txt_Limt_L", Txt_Limt_L.Text, SysInfo.HardFileName);
                if (m_blXunJi_Prog_0PC_1YY && SysInfo.m_Client.m_blLinkServe)
                    SysInfo.m_Client.SendData_C(7, Txt_Limt_L.Text + "," + Txt_Limt_R.Text);
            }
        }

        private void Txt_Limt_R_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (m_blXunJi_Prog_0PC_1YY == false) m_Hd850.g_fl_Limit_R = float.Parse(Txt_Limt_R.Text);
                SysInfo.csInter.INIWriteValue("System", "Txt_Limt_R", Txt_Limt_R.Text, SysInfo.HardFileName);
                if (m_blXunJi_Prog_0PC_1YY  && SysInfo.m_Client.m_blLinkServe)
                    SysInfo.m_Client.SendData_C(7, Txt_Limt_L.Text + "," + Txt_Limt_R.Text);
            }
        }

        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool _blOk = true;
            for (int i = 0; i < SysInfo.m_blFrmOpen.Length; i++)
                if (SysInfo.m_blFrmOpen[i]) { _blOk = false; break; }
            if (_blOk == false)
            {
                e.Cancel = true; return;
            }
            if (MessageBox.Show("确定关闭软件？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            SysInfo.m_iRun = 10;
            
            //关闭定义的事件
            SysInfo.Thread_Get_TofdData.Abort();
            SysInfo.g_Msg_InterFace.Inter_GetServe_CgXj -= new ClassLib_TestData.MsgInterFace.OnGetCgXjServe_Data(ShowServeData);
            SysInfo.g_Msg_InterFace.Inter_BrushCurrWeld -= new ClassLib_TestData.MsgInterFace.OnBrushCurrWeld(BrushCurrWeld);
            SysInfo.g_Msg_InterFace.Inter_BrushBiaoZhu_Posit -= new ClassLib_TestData.MsgInterFace.OnBrushBiaoZhu_Posit(Brush_Posit);
          
            if(m_Hd850!=null )
            m_Hd850.SetLaser(false );
            m_Cam_Xm.Close();
            m_ServerUI.SendData("", "99");//关闭相机程序

            SysInfo.m_Tofd_DLL.Close_TOFD();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void Bt_Tofd_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_blFrmOpen[0] == false)
            {
                m_frm_Tofd = new From.Frm_TOFD();
                m_frm_Tofd.Show();
            }
        }
        private void Bt_Item_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_blFrmOpen[1] == false)
            {
                m_frm_Item = new From.Frm_Item();
                m_frm_Item.Show();
            }
        }
        private void Bt_Weld_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Test_Item.ID == "" || SysInfo.m_Test_Item.Dwmc == "" ||
               SysInfo.m_Test_Item.ItemName == "")
            {
                MessageBox.Show("请先选择：1项目管理(新建项目或查询历史项目)");
                return;
            }
            if (SysInfo.m_blFrmOpen[2] == false)
            {
                m_frm_Weld = new From.Frm_Weld();
                m_frm_Weld.Show();
            }
        }
        private void Bt_Move_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_blFrmOpen[3] == false)
            {
                m_frm_Move = new From.Frm_Move();
                m_frm_Move.Show();
            }
        }

        private void Bt_Start_Click(object sender, EventArgs e)
        {
            #region 检查
            //1 参数检查
            string _strT = "";
            if (SysInfo.m_Test_Item.ID == "")

                _strT = "请先选择：《项目管理》,您需要先建立检测项目;";

            if (SysInfo.m_Test_Parts.Sub_ID == "")
            {
                if (_strT == "")
                    _strT = "请先选择：《焊缝管理》，选择您要检测哪条焊缝;";
            }
            //2 设备检查
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode!=2 &&
                SysInfo.m_Tofd_DLL.blNetLink == false)
            {
                if (_strT != "") _strT += "\r\n\r\n";
                _strT += "Tofd设备联机失败;";
            }
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 1 &&
                SysInfo.m_Climb4.m_blLink == false)
            {
                if (_strT != "") _strT += "\r\n\r\n";
                _strT += "4轮车体联机失败;";
            }

            if (_strT != "") { MessageBox.Show(_strT, "请检查:"); return; }
            int _iCurrChan = SysInfo.m_Tofd_DLL.m_icurChan;
           if( SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_dSpeed <3200||
            SysInfo.m_Tofd_DLL.m_pSparam_Real[_iCurrChan].T0 ==0)
            {
                if (_strT != "") _strT += "\r\n\r\n";
                _strT += "5 探头没有校准(TOFD参数-->探头校准-->声速和延时计算。);";
            }
            #endregion 检查
           //0 距离清零
            if (SysInfo.m_Climb.iBmq_Type == 1)
                SysInfo.m_Climb4.SendData(2, 4, 0, 0, "0");
            else
                SysInfo.m_Tofd_DLL.Init_Encoder();
            //1 车体开始运行
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                SysInfo.m_Climb4.SendData(1, 1, 3, 0, "1");//前进
            //2 开始采集
            DbGlobal.ImTest_Records.DbTableName = SysInfo.GetCurrDataTableName(1, SysInfo.m_Test_Item.ID, SysInfo.m_Test_Item.Dwmc);
            DbGlobal.ImTest_Records.Creat_Table();
            DbGlobal.ImAlarm.DbTableName = SysInfo.GetCurrDataTableName(2, SysInfo.m_Test_Item.ID, SysInfo.m_Test_Item.Dwmc);
            DbGlobal.ImAlarm.Creat_Table();
            Tofd.m_lstTofd.Clear();
            //3 清理D图
            m_lstRec = new List<ClassLib_TestData.Class_Test_Records>();
            SysInfo.m_Plant.m_bl_His_Data = false;
            Ck_Bx.Checked = false;
            SysInfo.m_Plant.m_bl_Ck_Wave = Ck_Bx.Checked;
            SysInfo.m_Tofd_DLL.m_lstAlarm.Clear();
            Init_PlantD(1);
            SysInfo.m_iRun = 1;
            Bt_E(false);
            Bt_Data_Left.Visible = false;
            Bt_Data_Right.Visible = false;
        }
        private void Bt_E(bool blVal)
        {
            Bt_Item.Enabled = blVal;
            Bt_Weld.Enabled = blVal;
            Bt_Start.Enabled = blVal;
        }
        private void Bt_Stop_Click(object sender, EventArgs e)
        {
            Ck_Bx.Visible = false;
            Application.DoEvents();
            if (SysInfo.m_Test_Item.ID == "")
            {
                MessageBox.Show("没有项目要停止。");
                return;
            }
            //停止检测，设置标记
            SysInfo.m_iRun = 3;
            Bt_E(true);
            SysInfo.m_Climb4.SendData(1, 1, 6, 0, "1");
            SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
            SysInfo.m_iRun = 0;
            Bt_Data_Left.Visible = true;
            Bt_Data_Right.Visible = true;
            string _Id = SysInfo.m_Test_Item.ID.Substring(0, 6);
           string strVideoPath = Application.StartupPath + "\\Temp\\"+ _Id + "\\" + DbGlobal.ImTest_Records.DbTableName + ".tdf";
            SysInfo.csInter.CreatCurrDir(_Id);

           SysInfo.csInter .FileSaveByte(strVideoPath);//陈大伟 2021-8-24 删除
        //    SEmatChanParam _Tofd_P = new SEmatChanParam();
        //    SysInfo.csInter.FileReadByte(strVideoPath, ref _Tofd_P);

            //           System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            //        folderBrowserDialog.SelectedPath = "";

            //if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    string _T = folderBrowserDialog.SelectedPath;
            //}

            Ck_Bx.Visible = true ;
        }

        private void Bt_Data_Left_Click(object sender, EventArgs e)
        {
            Page_L0_R1(0);
        }

        private void Bt_Data_Right_Click(object sender, EventArgs e)
        {
            Page_L0_R1(1);
        }
        /// <summary>
        /// 左右翻页 0：左  1：右
        /// </summary>
        /// <param name="iType"></param>
        private void Page_L0_R1(int iType = 0)
        {
            if (SysInfo.m_blFrmOpen[4] && m_frm_Bz != null)
                m_frm_Bz.Close ();
            bool _blOk = false;
            //1 左右翻页决定屏幕计算序号
            if (iType == 0)
            {
                if (SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo > 0)
                { SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo--; _blOk = true; }
            }
            else
            {
                //if (SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo<
                //    SysInfo.m_Plant.g_iCurrDistanc_Calcu_All_ScreenNo)//右边有数据
                { SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo++; _blOk = true; }
            }
            if (_blOk)
            {
                float _flLastX = Brush_D_OldData(SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo);
                if (iType == 1)
                {
                    Application.DoEvents();
                    float _flD = SysInfo.m_Plant.lst_Screenkd[SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo].Chart_Run_flEnd_Distance - _flLastX;
                    _flD = float.Parse(_flD.ToString("f3"));
                    if (_flD > SysInfo.m_Plant.Scree_iDotWithmm_X)
                        MessageBox.Show("右边没有数据了!");
                }
            }
        }
       
        private void Bt_Move_RL(int iType, object sen, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//按压左键拖动
            {
                Button _bt = sen as Button;

                switch (iType)
                {
                    case 0://左右键 
                        _bt.Location = new Point(m_Bt_Data_Point.X, _bt.Location.Y + (e.Y - m_Bt_Data_Point.Y));
                        break;

                    case 1://上下键 
                        _bt.Location = new Point(_bt.Location.X + (e.X - m_Bt_Data_Point.X), m_Bt_Data_Point.Y);
                        break;


                }
            }
        }

        private void Bt_Data_Left_MouseDown(object sender, MouseEventArgs e)
        {
            m_Bt_Data_Point.X = Bt_Data_Left.Left;
            m_Bt_Data_Point.Y = e.Y;
        }

        private void Bt_Data_Left_MouseMove(object sender, MouseEventArgs e)
        {
            Bt_Move_RL(0, sender, e);
        }
        /// <summary>
        /// 数据翻页按钮位置
        /// </summary>
        Point m_Bt_Data_Point;
        private void Bt_Data_Right_MouseDown(object sender, MouseEventArgs e)
        {
            m_Bt_Data_Point.X = Bt_Data_Right.Left;
            m_Bt_Data_Point.Y = e.Y;
        }
        private void Bt_Data_Right_MouseMove(object sender, MouseEventArgs e)
        {
            Bt_Move_RL(0, sender, e);
        }

        private void Rad_V_F_Click(object sender, EventArgs e)
        {
            SysInfo.m_iShowPhone = 0;
            ShowVideoFBM();
        }

        private void Rad_V_B_Click(object sender, EventArgs e)
        {
            SysInfo.m_iShowPhone = 1;
            ShowVideoFBM();
        }

        private void Ck_Pic_Big1_Lit0_Click(object sender, EventArgs e)
        {
            m_iLit0_Bit1 = Ck_Pic_Big1_Lit0.Checked ? 1 : 0;
            Pic_Zoom();
        }
        private void Pic_Zoom_A()
        {
            if (m_iLit0_Bit1_A == 1)
            {
                #region 放大
                //1 左右
                Tb_Up.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 100);
                Tb_Up.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 0);
                //2 上下
          //      Tb_Main_Data.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 70);
          //      Tb_Main_Data.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 30);
                #endregion
            }
            else
            {
                #region 缩小
                //1 左右
                Tb_Up.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, m_fl_LR_New_R);
                Tb_Up.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 100 - m_fl_LR_New_R);

                ////2 上下
                //Tb_Main_Data.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, m_fl_UD_New_D);
                //Tb_Main_Data.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 100 - m_fl_UD_New_D);
                #endregion 
            }
            Application.DoEvents();
            VideoSize();
        }
        /// <summary>
        /// 视频放大缩小
        /// </summary>
        private void Pic_Zoom()
        {
            if(m_iLit0_Bit1==1)
            {
                #region 放大
                try
                {
                    int _iLen = int.Parse(SysInfo.csInter.IniReadDefine("Zoom", "Tb_Up_L", "50", SysInfo.HardFileName));
                    if (_iLen > 100 || _iLen < 0) _iLen = 50;
                    //1 左右
                    Tb_Up.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, _iLen);
                    Tb_Up.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 100 - _iLen);
                }
                catch { }
                //2 上下
                Tb_Main_Data.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 80);
                Tb_Main_Data.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 20);
                #endregion
            }
            else
            {
                #region 缩小
                //1 左右
                Tb_Up.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, m_fl_LR_New_R );
                Tb_Up.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 100- m_fl_LR_New_R);
         
                //2 上下
                Tb_Main_Data.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent,100- m_fl_UD_New_D);
                Tb_Main_Data.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent,m_fl_UD_New_D);
                #endregion 
            }
     
            Application.DoEvents();
            VideoSize();  
            SysInfo.m_Plant.GetRulerPara_C(Pic_D, Tofd.UTS_DATA_WIDTH);
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
        public static void Zoom_Tool(int i_x, int i_y, int offset, int select_shape, Bitmap pic_original, PictureBox Pic_A, PictureBox pic_display)
        {
            if (pic_original == null) return;//chendawei 190731 
            int originalWidth = pic_original.Width;
            int originalHeight = pic_original.Height;

            System.Reflection. PropertyInfo rectangleProperty = pic_original.GetType().GetProperty("ImageRectangle", 
                      System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
  //          Rectangle rectangle = (Rectangle)rectangleProperty.GetValue(Pic_A, null);

            int currentWidth = originalWidth;// rectangle.Width;
            int currentHeight = originalHeight;// rectangle.Height;

            double rate = (double)currentHeight / (double)originalHeight;    //图片缩放比率
            double rate1 = (double)currentWidth / (double)originalWidth;

            double original_x = (double)i_x / rate1;  //鼠标在缩放图片中的坐标
            double original_y = (double)i_y / rate;


            Pic_A.Refresh();
            Graphics graphics = pic_display.CreateGraphics();     //实例化pictureBox1控件的Graphics类
            Point p = new Point(i_x, i_y);
            p = new Point(i_x + offset, i_y - offset);

            if (select_shape == 0)   //如果需要圆形画布，需要现在创建一个圆形区域，后续会再此区域中绘制
            {
                System.Drawing.Drawing2D. GraphicsPath gpath = new System.Drawing.Drawing2D.GraphicsPath();  //
                gpath.AddEllipse(p.X - 50, p.Y - 50, 100, 100);//添加一个圆形区域
                Region rg = new Region(gpath);
                graphics.Clip = rg;  //设定绘制的区域，以后的绘图都在这个区域内
            }

            //声明两个Rectangle对象，分别用来指定要放大的区域和放大后的区域
            Rectangle sourceRectangle = new Rectangle(Convert.ToInt32(original_x) - 20, Convert.ToInt32(original_y) - 20, 40, 40);  //要放大的区域 
            Rectangle destRectangle = new Rectangle(p.X - 70, p.Y - 70, 140, 140);
            //调用DrawImage方法对选定区域进行重新绘制，以放大该部分
            graphics.DrawImage(pic_original, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
            //Brush bush = new SolidBrush(Color.Green);//填充的颜色
            //graphics.FillEllipse(bush, 10, 10, 100, 100);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50

            Pen pen = Pens.Yellow;  //描边，画轮廓
            graphics.DrawEllipse(pen, p.X - 2, p.Y - 2, 4, 4);//画填充椭圆的方法，x坐标、y坐标、宽、高，如果是100，则半径为50
        }

        private void Frm_Main_ResizeEnd(object sender, EventArgs e)
        {
            SysInfo.m_Tofd_DLL.m_fWaveFramePerHeight = Pic_A.Height / ((float)Tofd.UTS_DATA_HEIGHT);
            SysInfo.m_Tofd_DLL.m_fWaveFramePerWidth = Pic_A.Width / ((float)Tofd.UTS_DATA_WIDTH);
        }

        private void Pic_A_MouseMove(object sender, MouseEventArgs e)
        {
            SysInfo.m_Plant.m_bl_Hid_Sz = false ;
            float _fld = e.X / SysInfo.m_Tofd_DLL.m_fWaveFramePerWidth;
            SysInfo.m_Plant.m_i_X_No = (int)_fld;
            SysInfo.m_Plant.m_i_X_A = e.X;// (int)_fld;
            SysInfo.m_Plant.m_i_Y_A = e.Y;// (int)( e.Y / SysInfo.m_Tofd_DLL.m_fWaveFramePerHeight);
                                          //Pic_Z.Visible = true ;
                                          //Zoom_Tool(e.X, e.Y, 5, 0,SysInfo .m_Plant . m_image,Pic_A , Pic_Z);
            //if (Pic_Son_X.Visible)
            //{
            //    Pic_Son_X.Visible = false;
            //    Lb_RunMsg.Visible = false;
            //}
        }

      
        private void Ck_Pic_A_Click(object sender, EventArgs e)
        {
            m_iLit0_Bit1_A = Ck_Pic_A.Checked ? 1 : 0;
            Pic_Zoom_A();
        }

        private void Pic_A_Resize(object sender, EventArgs e)
        {
            SysInfo.m_Tofd_DLL.m_fWaveFramePerHeight = Pic_A.Height / ((float)Tofd.UTS_DATA_HEIGHT);
            SysInfo.m_Tofd_DLL.m_fWaveFramePerWidth = Pic_A.Width / ((float)Tofd.UTS_DATA_WIDTH);
        }

        private void Ck_Bx_Click(object sender, EventArgs e)
        {
            SysInfo.m_Plant.m_bl_Ck_Wave = Ck_Bx.Checked;
        }

      
        private void Get_D_Wave()
        {

            //查看对应波形
            if (SysInfo.m_Plant.m_bl_Ck_Wave)
            {
                bool _blRet = false;
                if (SysInfo.m_Plant.m_bl_His_Data)//依据查询，从记录中拿
                {
                    if (m_iX_D < m_lstRec.Count)
                        _blRet = GetHisData(m_lstRec[m_iX_D].strData, m_lstRec[m_iX_D].strData_2, m_lstRec[m_iX_D].strWave_Time);
                }
                else//依据屏幕序号从数据库拿
                {
                    string _strT = DbGlobal.ImTest_Records.GetWave(SysInfo.m_Test_Parts.ID,
                                                                     SysInfo.m_Test_Parts.Sub_ID,
                                                                     m_flCurrDistanc);
                    string[] _sPara = _strT.Split('/');
                    if (_sPara.Length == 3)//波形类型|波峰序列/波形类型|波谷序列/时间序列
                    {
                        string[] _sPara_1 = "".Split(',');
                        string[] _sPara_2 = "".Split(',');

                        _sPara_1 = _sPara[0].Split('|');
                        _sPara_2 = _sPara[1].Split('|');
                        if (_sPara_1.Length == 2 && _sPara_2.Length == 2)
                            _blRet = GetHisData(_sPara_1[1], _sPara_2[1], _sPara[2]);
                    }
                }
            }
        }
        /// <summary>
        /// 解析历史数据的波峰、波谷、时间
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="strData_2"></param>
        /// <param name="strWave_Time"></param>
        private bool GetHisData(string strData, string strData_2, string strWave_Time)
        {
            string[] _sPara_1 = "".Split(',');
            string[] _sPara_2 = "".Split(',');
            string[] _sPara_3 = "".Split(',');
            bool _blRet = false;
            bool _bl2 = false;
            _sPara_1 = strData.Split('|');
            if (_sPara_1.Length == 2)
            { _bl2 = true; _sPara_1 = _sPara_1[1].Split(','); }

            _sPara_2 = strData_2.Split('|');
            if (_sPara_2.Length == 2)
                _sPara_2 = _sPara_2[1].Split(',');

            if (_bl2==false )
            {
                _sPara_1 = strData.Split(',');
                _sPara_2 = strData_2.Split(',');
            }
            _sPara_3 = strWave_Time.Split(',');

            int _iN_1 = _sPara_1.Count();
            int _iN_2 = _sPara_2.Count();
            int _iN_3 = _sPara_3.Count();
            if (_iN_1 == _iN_2)//&& _iN_2 == _iN_3)
            {
                _blRet = true;
                for (int i = 0; i < _iN_1; i++)
                {
                    Tofd.m_pChannelBuf[i] = byte.Parse(_sPara_1[i]);
                    Tofd.m_pValueBuf[i] = byte.Parse(_sPara_2[i]);
                    if (_iN_3 == _iN_1)
                        Tofd.m_pTimeBuf[i] = int.Parse(_sPara_3[i]);
                }
            }
            return _blRet;
        }
        private void Pic_D_MouseMove(object sender, MouseEventArgs e)
        {
            Bt_BZ_Point.Visible = false;
            int _E_X = e.X - SysInfo.m_Plant.Chart_Ruler_X_Start;if (_E_X < 0) return;
             m_iX_D = _E_X / SysInfo.m_Plant.Scree_iDotWith_X;

            int _E_Y = e.Y - SysInfo.m_Plant.Chart_Ruler_Y_Start; if (_E_Y < 0) return;
            m_iY_D = _E_Y / SysInfo.m_Plant.Scree_iDotHeight;
            SysInfo.m_Plant.m_i_X_No = m_iY_D;

            float _T = SysInfo.m_Tofd_DLL.m_fWaveFramePerWidth * m_iY_D;
            SysInfo.m_Plant.m_i_X_A =(int)_T;
            SysInfo.m_Plant.m_i_Y_A = Pic_A.Height / 2;

            float _fl=SysInfo.m_Plant.lst_Screenkd[SysInfo.m_Plant.g_iCurrUse_ScreenNo].Chart_Run_flStart_Distance  +
                             m_iX_D * SysInfo.m_Plant.Scree_iDotWithmm_X;
            string[] _sPara = SysInfo.m_Plant.Scree_iDotWithmm_X.ToString().Split('.');
            int _iNum = 3;
            if (_sPara.Length == 2)
                _iNum = _sPara[1].Length;
            m_flCurrDistanc =float .Parse ( _fl.ToString("f"+ _iNum.ToString ()));

            //if (m_i_Pbl_Type == 0)
            //{
                Lb_RunMsg.Left = e.X -25;
                Lb_RunMsg.Top = e.Y + 25;
            //}
            //else
            //{
            //    Lb_RunMsg.Left = e.X - 10;
            //    Lb_RunMsg.Top = e.Y + 10;
            //}
            SysInfo.m_Plant.m_i_X_D = e.X;// (int)_fld;
            SysInfo.m_Plant.m_i_Y_D = e.Y;// (int)( e.Y / SysInfo.m_Tofd_DLL.m_fWaveFramePerHeight);
       //     m_bl_Show_Pbl = true;
    
            if(Lb_RunMsg.Visible==false )    
                Lb_RunMsg.Visible = true ;
            Lb_RunMsg.Text = m_flCurrDistanc.ToString() + "m";
        }
      
        private void Pic_A_MouseLeave(object sender, EventArgs e)
        {
            SysInfo.m_Plant .m_bl_Hid_Sz = true;
        }

        private void Pic_D_MouseLeave(object sender, EventArgs e)
        {
        //    m_bl_Show_Pbl = false;
    //        Pic_Son_X.Visible = false;
    //         Pic_Son_Y.Visible = false;
    //        Lb_RunMsg.Visible = false;
        }
        
        private void Ck_Pbl_Kk_Click(object sender, EventArgs e)
        {
           
        }

        private void Pic_D_Click(object sender, EventArgs e)
        {
            #region 确定标注的位置
            switch (SysInfo.m_BiaoZhu_No.iDataType)
            {
                case 0://长度
                    switch (SysInfo.m_BiaoZhu_No.i_S0_E1)
                    {
                        case 0:
                            SysInfo.m_BiaoZhu.iY_No = m_iY_D;
                            SysInfo.m_BiaoZhu.iX_No = m_iX_D;
                            SysInfo.m_BiaoZhu.flLen_S = m_flCurrDistanc;
                            break;
                        case 1:
                            SysInfo.m_BiaoZhu.flLen_E = m_flCurrDistanc;
                            break;
                    }
                    break;
                case 1://高度
                    switch (SysInfo.m_BiaoZhu_No.i_S0_E1)
                    {
                        case 0:
                            SysInfo .m_BiaoZhu.flHeight_S= Tofd.m_pTimeBuf[SysInfo.m_Plant.m_i_X_No] * 0.01f;
                            break;
                        case 1:
                            SysInfo.m_BiaoZhu.flHeight_E = Tofd.m_pTimeBuf[SysInfo.m_Plant.m_i_X_No] * 0.01f;
                            break;
                    }
                    break;
                case 2://深度 
                    switch (SysInfo.m_BiaoZhu_No.i_S0_E1)
                    {
                        case 0:
                            SysInfo.m_BiaoZhu.flDepth_S  = Tofd.m_pTimeBuf[SysInfo.m_Plant.m_i_X_No] * 0.01f;
                            break;
                    }
                    break;

            }
            SysInfo.g_Msg_InterFace.Fun_Brush_BiaoZhu();
            #endregion
            Get_D_Wave();
        }
       
        private void Pic_D_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_i_Pbl_Type = m_i_Pbl_Type == 0 ? 1 : 0;
            }
        }

        private void Bt_UrgentStop_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
            {
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "1");
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
            }
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
        }

        private void Bt_Bz_MouseDown(object sender, MouseEventArgs e)
        {
            m_Bt_Data_Point.X = e.X;
            m_Bt_Data_Point.Y = Bt_Bz.Top;
        }
        private void Bt_Bz_MouseMove(object sender, MouseEventArgs e)
        {
            Bt_Move_RL(1, sender, e);
        }

        private void Bt_Bz_Click(object sender, EventArgs e)
        {
          if( SysInfo.m_Test_Parts.ID=="" || SysInfo.m_Test_Parts.Sub_ID=="")
            {
                MessageBox.Show("没有项目或没选中指定焊缝数据!");
                return;
            }
            if (SysInfo.m_blFrmOpen[4] == false)
            {
                SysInfo.m_BiaoZhu_No.iDataType = 0;
                SysInfo.m_BiaoZhu_No.i_S0_E1 = 0;
                Ck_Bx.Checked = true;
                SysInfo.m_Plant.m_bl_Ck_Wave = Ck_Bx.Checked;
                m_frm_Bz = new From.Frm_Bz ();
                m_frm_Bz.Show();
            }
        }

        private void Bt_Print_MouseDown(object sender, MouseEventArgs e)
        {
            m_Bt_Data_Point.X = e.X;
            m_Bt_Data_Point.Y = Bt_Print.Top;
        }

        private void Bt_Print_MouseMove(object sender, MouseEventArgs e)
        {
            Bt_Move_RL(1, sender, e);
        }

        private void Bt_Print_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Test_Item.ID == "" || SysInfo.m_Test_Item.Dwmc == "")
            {
                MessageBox.Show("没有选中要打印的项目!");
                return;
            }
            Bt_Print.Enabled = false;
            #region 模板文件名
            string strMbFileName = System.Windows.Forms.Application.StartupPath + "\\DataBase\\";
            if (SysInfo.m_Plant.iRad_Dw == 1)
                strMbFileName += "Original record_inch.xlsx";
            else
            {
                if (SysInfo.m_iLanguage == 0)
                    strMbFileName += "Thickness record.xlsx";
                else
                    strMbFileName += "Original record.xlsx";
            }
            #endregion 模板
            #region 输出路径
            string strPathFileName = SysInfo.csInter.IniReadDefine("Browse", "strPathFileName", "", SysInfo.HardFileName);

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

            dialog.SelectedPath = strPathFileName;
            if (SysInfo.m_iLanguage == 0)
                dialog.Description = "请选择文件路径";
            else
                dialog.Description = "Please select the file path";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                strPathFileName = dialog.SelectedPath;
                SysInfo.csInter.INIWriteValue("Browse", "strPathFileName", strPathFileName, SysInfo.HardFileName);
            }
            else
            {
                Bt_Print.Enabled = true;
                return;
            }
            try
            {
             

                string strResultRep = "";
                if (strPathFileName == "")
                {
                    strResultRep = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
                else
                {
                    strResultRep = strPathFileName;
                }
                string _strFileName = SysInfo.GetCurrDataTableName(3, SysInfo.m_Test_Item.ID, SysInfo.m_Test_Item.Dwmc);
                strResultRep += "\\" + _strFileName + ".xlsx";
               Prg_Print_Bar.Visible = true;
                #endregion  输出路径

                //2 表头数据
                int _iCurrChan = SysInfo.m_Tofd_DLL.m_icurChan;
                m_Report.Set_Ysjl_Excel_2(SysInfo.m_Test_Item, SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsAngle.ToString(),
                    SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsLen.ToString()
                          , strMbFileName, strResultRep);
               
                Prg_Print_Bar.Value = m_Report. m_iPrintRunState;
                //3 拿标注数据
                bool _blAdd = false;//是否有详细数据添加
                int _iRow_No = 20;//图像开始行  1-9表头数据
                int _iColsHeBing = 11;//合并列数
                int _iNo = 1, _iPic_No = 1, _iNumAll = 0, _iCurrDataScreenNo = 0;//数据序号，图片序号,总数据量,当前数据对应的屏幕序号
                int iHeight = 0;//图像高度
                string _strImgPath = System.Windows.Forms.Application.StartupPath + "\\" + "_TmpPhone" + ".bmp";//临时图片
                System.Drawing.Bitmap _pic_MapSave = null;//图片
                DbGlobal.ImAlarm.DbTableName = SysInfo.GetCurrDataTableName(2,
                                    SysInfo.m_Test_Item.ID, SysInfo.m_Test_Item.Dwmc);
                List<Class_Test_AlarmArea> _lstAlarm = DbGlobal.ImAlarm.GetData(SysInfo.m_Test_Item.ID, "");
                _iNumAll = _lstAlarm.Count;
                Prg_Print_Bar.Maximum  = _iNumAll+m_Report.m_iPrintRunState;

                for (int i = 0; i < _iNumAll; i++)
                {
                    Prg_Print_Bar.Value = i + m_Report.m_iPrintRunState;
                    //3.1 显示出图片
                    //拿距离信息，转换成平面序号
                    SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo = SysInfo.m_Plant.Get_No(_lstAlarm[i].flLen_S);
                    Brush_D_OldData(SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo);
                    //3.2 输出图片
                    //3.2.1 图片文字说明
                    m_Report.InsertRow(_iRow_No); m_Report.Set_RowHeight(_iRow_No, _iRow_No, 15);
                    m_Report.MergeCells("A", "J", _iRow_No);
                    m_Report.Set_Txt(_iRow_No, "焊缝编号:" + _lstAlarm[i].Part_No + " 图片，总计第" + _iPic_No++ + "张");


                    //3.2.2 行列调整：添加行，合并列
                    _iRow_No++;
                    m_Report.InsertRow(_iRow_No);
                    //  合并列
                    m_Report.Merge_Colums(_iRow_No, 1, _iColsHeBing);//合并图片行

                    //3.2.3图像拷贝 
                    _pic_MapSave = (Bitmap)SysInfo.m_Plant.m_G_C.canvas.Clone();// (System.Drawing.Bitmap)SysInfo.m_SysBuff.g_ScreenPlant.Plant_UI_C_bmp.Clone();
                                                                                //3.2.4 调整图片大小
                    _pic_MapSave.Save(_strImgPath);//保存图片
                    iHeight = _pic_MapSave.Height > 400 ? 220 : _pic_MapSave.Height;
                    m_Report.Set_RowHeight(_iRow_No, _iRow_No, iHeight);//设置行高度
                                                                        //3.2.5  将指定文件路径图片保存到Excel指定行
                    m_Report.Set_Picture(_iRow_No, 1, _iColsHeBing, _strImgPath);

                    //3.3 循环输出本图片中详细数据
                    //3.3.1 添加详细数据文字
                    _iRow_No++;
                    m_Report.InsertRow(_iRow_No); m_Report.Set_RowHeight(_iRow_No, _iRow_No, 15);
                    m_Report.MergeCells("A", "J", _iRow_No);
                    m_Report.Set_Txt(_iRow_No, "焊缝编号:" + _lstAlarm[i].Part_No + " 图片中标注的详细数据如下:");
                    _blAdd = false;
                    for (int _iT = i; _iT < _iNumAll; _iT++)
                    {
                        _iCurrDataScreenNo = SysInfo.m_Plant.Get_No(_lstAlarm[_iT].flLen_S);
                        if (_iCurrDataScreenNo == SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo)
                        {
                            // 3.3.2 添加详细数据的列表
                            _iRow_No++;

                            m_Report.InsertRow(_iRow_No); m_Report.Set_RowHeight(_iRow_No, _iRow_No, 15);
                            m_Report.MergeCells("G", "H", _iRow_No);//缺陷类型
                            m_Report.MergeCells("I", "J", _iRow_No);//备注
                                                                    //3.3.3 添加数据
                            m_Report.Set_Txt(_iRow_No, "A", _iNo++.ToString());//序号
                            m_Report.Set_Txt(_iRow_No, "B", _lstAlarm[_iT].Part_No); //焊缝编号
                            m_Report.Set_Txt(_iRow_No, "C", _lstAlarm[_iT].flLen_S.ToString()); //缺陷位置
                            m_Report.Set_Txt(_iRow_No, "D", _lstAlarm[_iT].flLen.ToString()); //长度(mm)
                            m_Report.Set_Txt(_iRow_No, "E", _lstAlarm[_iT].flDepth.ToString()); //深度(mm)
                            m_Report.Set_Txt(_iRow_No, "F", _lstAlarm[_iT].flHeight.ToString()); //高度(mm)
                            m_Report.Set_Txt(_iRow_No, "G", _lstAlarm[_iT].strType); //缺陷类型
                            i++;
                            _blAdd = true;
                        }
                        else
                        {
                            if (_blAdd) i--;
                            break;
                        }
                    }

                    //3.3.3 准备下一张图片
                    _iRow_No++;
                }
                //4 报表数据保存
                m_Report.Save_Ysjl_ExcelFile();
                MessageBox.Show("文件：" + _strFileName + ".xlsx" +" 成功输出。");

            }
            catch { }
            Bt_Print.Enabled = true;
            Prg_Print_Bar.Visible = false ;
        }
    }

    /// <summary>
    /// 视频服务器
    /// </summary>
    public class Class_Server_UI
    {
        /// <summary>
        /// 联机上客户端
        /// </summary>
        public bool m_blLink = false;
        /// <summary>
        /// 是否发送数据
        /// </summary>
        public bool m_blSend = false;
        /// <summary>
        /// 接收到数据
        /// </summary>
        public bool m_blRecev = false;

        #region 数据帧格式
        /// <summary>
        /// 接收客户端数据帧头<clie>
        /// </summary>
        public string C_Head = "<clie>";
        /// <summary>
        /// 接收客户端数据帧尾 </clie>
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
        /// <summary>
        /// 服务器
        /// </summary>
        public TcpListener m_TcpServer = null;
        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient m_TcpC = null;
        /// <summary>
        /// 通讯线程
        /// </summary>
        Thread m_trdServer = null;
        /// <summary>
        /// 解析网络数据
        /// </summary>
        Thread m_trd_Parsing = null;
        /// <summary>
        /// 终端服务器监听线程
        /// </summary>
        private Thread TreadTcp_Server;

        ///// <summary>
        ///// 启动涡流程序
        ///// </summary>
        //public void Run_Client()
        //{
        //    Thread tdS = new Thread(new ThreadStart(LoadClientProgram));
        //    tdS.Start();
        //    tdS.IsBackground = true;
        //}
        /// <summary>
        /// 关闭客户端
        /// </summary>
        public void End_Client()
        {
            SendData("", "99");
        }
        //private void LoadClientProgram()
        //{
        //    System.Diagnostics.Process pAppInterface = new System.Diagnostics.Process();
        //    pAppInterface.StartInfo.FileName = Application.StartupPath + "\\Auto_Pulsed_Eddy.exe";
        //    pAppInterface.Start();
        //    pAppInterface.Close();
        //}

        /// <summary>
        /// 启动服务器监听的方式
        /// </summary>
        /// <param name="strServIp">127.0.0.1</param>
        /// <param name="Port">48100</param>
        public void Start(string strServIp = "127.0.0.1", int Port = 48100)
        {
            try
            {
                if (TreadTcp_Server != null) TreadTcp_Server.Abort();
                if (m_TcpServer != null) m_TcpServer.Stop();

                m_TcpServer = new TcpListener(System.Net.IPAddress.Parse(strServIp == "" ? "127.0.0.1" : strServIp), Port);
                TreadTcp_Server = new Thread(new ThreadStart(StartListen));
                TreadTcp_Server.Start();
                TreadTcp_Server.IsBackground = true;
            }
            catch (Exception ex)
            {
            }
        }
        private void StartListen()
        {
            try
            {
                m_TcpServer.Start();
                //             Run_Client();
                while (true)//
                {
                    if (m_TcpServer.Pending())
                    {
                        if (m_TcpC != null)
                            m_TcpC.Close();
                        m_TcpC = null;
                        m_TcpC = m_TcpServer.AcceptTcpClient();
                        //MessageBox.Show("客户端收到");
                        if (m_trdServer != null) m_trdServer.Abort();
                        m_trdServer = new Thread(new ParameterizedThreadStart(AcceptClientMsg));
                        m_trdServer.Start(m_TcpC);
                        m_trdServer.IsBackground = true;

                        Thread.Sleep(100);
                        Application.DoEvents();
                        break;
                    }
                }
            }
            catch (Exception Err)
            {
            }
        }
        private void NetParsing(string strMsg)//陈大伟WWW
        {
            if (m_trd_Parsing != null) m_trd_Parsing.Abort();
            m_trd_Parsing = new Thread(new ParameterizedThreadStart(Parsing));
            m_trd_Parsing.Start(strMsg);
            m_trd_Parsing.IsBackground = true;
        }
        private void Parsing(object ObjMsg)
        {
            string _strMsg = (string)ObjMsg;
            ExplainClientMsg(_strMsg);
        }
        private void AcceptClientMsg(object arg)
        {
            TcpClient TcpC = (TcpClient)arg;
            m_blLink = false;
            string strMsg = "";

            if (TcpC != null)
            {
                NetworkStream ns = TcpC.GetStream();
                while (TcpC.Connected == true)
                {
                    try
                    {
                        Thread.Sleep(10);
                        m_blLink = true;
                        int num = TcpC.Available;
                        if (num > 0)
                        {
                            //1 接收
                            byte[] Msg = new byte[num];
                            int Count = TcpC.Client.Receive(Msg);

                            if (Msg != null && Msg.Length > 0)
                            {
                                //2 拿返回数据
                                strMsg = Encoding.Default.GetString(Msg, 0, Msg.Length);
                                if (strMsg.Length > 0)
                                {
                                    NetParsing(strMsg);// ExplainClientMsg(strMsg);
                                }
                            }
                        }
                    }
                    catch
                    {

                        //WriteErrorLog(ee.Message);
                        // return;
                    }
                    Thread.Sleep(100);
                    Application.DoEvents();
                }
            }
        }
        /// <summary>
        /// 数据解析
        /// </summary>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        private string ExplainClientMsg(string strRetDat)
        {
            string strRet = "";

            #region 依据帧头 帧尾 截取有效数据
            int _iT = strRetDat.IndexOf(C_Head);

            strRetDat = strRetDat.Substring(_iT);
            _iT = strRetDat.IndexOf(C_Tail);
            if (_iT > -1)
                strRetDat = strRetDat.Substring(0, _iT + C_Tail.Length);
            #endregion 

            string[] sPara = strRetDat.Split('[');
            //数据格式：<main/clie> + "[" + 数据 + "[" + </main/clie>  数据格式：数据类型 / 数据内容
            //例如：<clie> + "[" + 1.0/235432 + "[" + </clie>
            if (sPara.Length == 3)
            {
                //2 确认包
                if (sPara[0].ToLower() == C_Head && sPara[2].ToLower() == C_Tail)
                {
                    string[] _sPara = sPara[1].Split('/'); //数据格式：数据类型 / 数据内容
                    if (_sPara.Length == 2)
                    {
                        if (_sPara[0] == "4.0")//数据类型 1.0：数据格式： 视频帧号标志 / 视频帧号
                        {
                            try
                            {
                                //SysInfo.m_SysInfo.g_Climb.m_iVideo_No = int.Parse(_sPara[1]);
                                //SysInfo.g_Msg_InterFace.Fun_RunInfo("3#" + SysInfo.m_SysInfo.g_Climb.m_iVideo_No.ToString());
                                //if (SysInfo.m_SysInfo.g_Video.blNetTrue == false)
                                //    SysInfo.m_SysInfo.g_Video.blNetTrue = true;
                              //  SysInfo.blNetTrue_Video = true;
                                m_blRecev = true;
                            }
                            catch (Exception ee
                            )
                            { m_blRecev = false; }
                        }
                        if (_sPara[0] == "7.0")
                        {
                            if (int.Parse(_sPara[1]) == 1)
                            {//重新启动
                              //  SysInfo.g_Msg_InterFace.Fun_ReLink_Phone();//SysInfo.m_SysInfo.g_Video.dt_GetDataTime = new DateTime();
                            }
                        }
                        if (_sPara[0] == "10.0")//数据类型 1.0：数据格式： 视频帧号标志 / 视频帧号
                        {
                            try
                            {
                                //  SysInfo.m_SysInfo.g_Climb.m_iVideo_No = int.Parse(_sPara[1]);

                                //    SysInfo.g_Msg_InterFace.Fun_RunInfo("3#" + _sPara[1]);
                                try
                                {
                                    SysInfo.iNetTrue_Video = int.Parse(_sPara[1]);
                                }
                                catch { }
                               
                                
                                m_blRecev = true;
                            }
                            catch (Exception ee
                            )
                            { m_blRecev = false; }
                        }
                        if (_sPara[0] == "18.0")//心跳  1次/30帧
                        {
                            //SysInfo.m_SysInfo.g_Video.dt_GetDataTime = DateTime.Now;
                            //SysInfo.m_SysInfo.g_Video.m_iRunning = 2;
                        }
                    }
                }
            }
            return strRet;
        }
        /// <summary>
        /// 数据解析
        /// </summary>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        private string ExplainClientMsg_Old(string strRetDat)
        {
            string strRet = "";

            #region 依据帧头 帧尾 截取有效数据
            int _iT = strRetDat.IndexOf(C_Head);

            strRetDat = strRetDat.Substring(_iT);
            _iT = strRetDat.IndexOf(C_Tail);
            if (_iT > -1)
                strRetDat = strRetDat.Substring(0, _iT + C_Tail.Length);
            #endregion 

            string[] sPara = strRetDat.Split('[');
            //数据格式：<main/clie> + "[" + 数据 + "[" + </main/clie>  数据格式：数据类型 / 数据内容
            //例如：<clie> + "[" + 1.0/235432 + "[" + </clie>
            if (sPara.Length == 3)
            {
                //2 确认包
                if (sPara[0].ToLower() == C_Head && sPara[2].ToLower() == C_Tail)
                {
                    string[] _sPara = sPara[1].Split('/'); //数据格式：数据类型 / 数据内容
                    if (_sPara.Length == 2)
                    {
                        if (_sPara[0] == "4.0")//数据类型 1.0：数据格式： 视频帧号标志 / 视频帧号
                        {
                            try
                            {
                                //SysInfo.m_SysInfo.g_Climb.m_iVideo_No = int.Parse(_sPara[1]);
                                //SysInfo.g_Msg_InterFace.Fun_RunInfo("3#" + SysInfo.m_SysInfo.g_Climb.m_iVideo_No.ToString());
                                //if (SysInfo.m_SysInfo.g_Video.blNetTrue == false || m_blRecev == false)
                                //    SysInfo.m_SysInfo.g_Video.blNetTrue = true;
                               // SysInfo.blNetTrue_Video = true;
                                m_blRecev = true;
                            }
                            catch (Exception ee
                            )
                            { m_blRecev = false; }
                        }
                    }
                }
            }
            return strRet;
        }
        /// <summary>
        /// 发送数据   拍照数据格式  2.0  / 文件名 | 行号  | 位置 | 检验员 | 1（开始拍照）   录像数据格式  3.0  / 文件名 | 录像开始1/接收0
        /// </summary>
        /// <param name="strKey">1.0: 系统信息 2.0:拍照 3.0:录像 99：退出程序</param>
        /// <param name="strDat">拍照数据格式  2.0  / 文件名 | 行号  | 位置 | 检验员 | 1（开始拍照）   录像数据格式  3.0  / 文件名 | 录像开始1/接收0 99: 关闭程序</param>
        public void SendData(string strDat, string strKey = "1.0")
        {
            string strSendMsg = strKey + "/" + strDat; //发送数据: 信息类型 / 数据
            string strSend = S_Head + "[" + strSendMsg + "[" + S_Tail;
            TcpClient tcpC = (TcpClient)m_TcpC;
            if (tcpC != null)
            {
                if (tcpC.Connected)
                {
                    //if (strKey == "1.0")
                    //{
                    //    m_blRecev = false;
                    //}
                    NetworkStream ns = tcpC.GetStream();
                    byte[] SendMsg = Encoding.Default.GetBytes(strSend);
                    try
                    {
                        ns.Write(SendMsg, 0, SendMsg.Length);
                        ns.Flush();
                    }
                    catch (Exception e)
                    { }
                }
            }
        }
        //public void Get_Data()
        //{
        //    DateTime dtStar = DateTime.Now;
        //    while (m_blSend)
        //    {
        //        if (m_blRecev)
        //        {
        //           // if (m_flThick_Per > 0)
        //            {
        //                break;
        //            }
        //        }
        //        if (DateTime.Now.Subtract(dtStar).TotalSeconds > 4) break;
        //        //  Application.DoEvents();
        //        Thread.Sleep(10);
        //    }
        //}
        /// <summary>
        /// 关闭网络
        /// </summary>
        public void Close()
        {
            if (TreadTcp_Server != null) TreadTcp_Server.Abort();
            if (m_TcpServer != null) m_TcpServer.Stop();
            m_blLink = false;

            if (m_TcpC != null)
            {
                if (m_TcpC.Connected)
                {
                    m_TcpC.Close();
                }
            }
            //3 关闭线程
            if (m_trdServer != null) m_trdServer.Abort();
            m_trdServer = null;
        }
    }
}