using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tofd_AWI.Class;
using System.Threading;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using Tofd_AWI.Class;
using ClassLib_DataMang.DataBaseMang.OleDal;//数据库操作类
//using HL;
using Clb_XmCam_DLL;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
//using ReportDLL;
using ClassLib_TestData;
using System.IO;//文件操作引用

using System.IO.Compression;

using Tofd_AWI.From;
using Frame_Work;
//using Tofd_DLL;
using Microsoft.VisualBasic;
using CL3d;
using System.IO.MemoryMappedFiles;
//using EM_RIT;

namespace Tofd_AWI
{
    public partial class Frm_Main_C : Form
    {
        float m_fl_Larser_Font = 19;
        int m_i_Larser_H_Pianyi = 20;

        #region 相机变量
        delegate void Delg_ShowVideo_Xj(int iType);
        bool m_blActive = false;
        /// <summary>
        /// 默认正常
        /// </summary>
        bool m_bl_Big = false;
        /// <summary>
        /// 后视相机
        /// </summary>
        string m_strP = "";
        /// <summary>
        /// 打标
        /// </summary>
        string m_strP_3 = "";

        #region 寻迹
        /// <summary>
        /// 测高寻迹报文显示行数
        /// </summary>
        int m_i_Com_Num = 0;
        /// <summary>
        /// X轴距离信息
        /// </summary>
        private float[] m_Axis_X = new float[500];
        private double m_maxX = 50;//轮廓线X最大范围
        private double m_maxZ = 80;//轮廓线Z最大范围
        private double m_scaleX = 5;//显示轮廓的X刻度
        private double m_scaleZ = 8;//显示轮廓的Z刻度
        #endregion 
        /// <summary>
        /// 申请测量数据字典
        /// </summary>
        static Frame_Work.ClassSys_Buff m_Insp_Data = new Frame_Work.ClassSys_Buff();

        /// <summary>
        /// 激光测量类
        /// </summary>
      //  HD850_64.Cl_HD850_64 m_Hd850 = new HD850_64.Cl_HD850_64(ref m_Insp_Data);

        /// <summary>
        /// 视频程序指针
        /// </summary>
        System.Diagnostics.Process m_p = new System.Diagnostics.Process();

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hChild, IntPtr hParent);
        [DllImport("user32")]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool bReDrow);

        #endregion
        /// <summary>
        /// 打印进度显示
        /// </summary>
        Thread Thread_ShowRunSate;
        /// <summary>
        /// 厚度字符左边位置
        /// </summary>
        int m_i_Thick_Left = 0;
        /// <summary>
        /// 共享内存文件
        /// </summary>
        static MemoryMappedFile m_mmf;
        /// <summary>
        /// 共享内存变量
        /// </summary>
        static MemoryMappedViewStream m_mmv;
        /// <summary>
        /// 寻迹：连接服务器线程
        /// </summary>
        Thread TreadTcp_Server;
        /// <summary>
        /// 刷新A扫描是否完成
        /// </summary>
        private bool m_bl_Pic_A_B_Plant = false;

        /// <summary>
        /// 刷新A扫描是否完成
        /// </summary>
        private bool m_bl_Pic_A_B_Plant_1 = false;
        /// <summary>
        /// 涂层数据是否刷新完成
        /// </summary>
        private bool m_bl_Pic_Coat_Plant = false;
        /// <summary>
        /// A控件通道号
        /// </summary>
        private int m_i_Cann_A = 0;
        /// <summary>
        /// B控件通道号
        /// </summary>
        private int m_i_Cann_B = 1;
        #region 任务栏


        private const int SW_HIDE = 0; //隐藏任务栏
        private const int SW_RESTORE = 9;//显示任务栏

        [DllImport("user32.dll")]
        public static extern int ShowWindow(int hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        #endregion

        #region A扫描闸门
        /// <summary>
        /// 窗口是否调用
        /// </summary>
        bool m_bl_Active = false;
        int m_iXmax = 1600;

        Point m_mousePos = Control.MousePosition;
        /// <summary>
        ///  Measurement algorithm: 0-auto, 1- 2 gates, 2- gate #1, 3-gate #2
        /// </summary>
        int m_Algorighm_2 = 0;
        /// <summary>
        /// Gate 1 starts
        /// </summary>
        int m_Bt_1_L = -20;
        /// <summary>
        /// Gate 1ends
        /// </summary>
        int m_Bt_1_R = -30;
        /// <summary>
        /// Gate 2 starts
        /// </summary>
        int m_Bt_2_L = -20;
        /// <summary>
        /// Gate 2 ends
        /// </summary>
        int m_Bt_2_R = -30;


        /// <summary>
        /// Image gate 1 start
        /// </summary>
        public int m_Plant_1_L = 20;
        /// <summary>
        /// End of Image Gate 1
        /// </summary>
        public int m_Plant_1_R = 30;
        /// <summary>
        /// Line 1 high
        /// </summary>
        public int m_Plant_1_H = 0;
        /// <summary>
        /// Image gate 2 start
        /// </summary>
        public int m_Plant_2_L = 20;
        /// <summary>
        /// End of Image Gate 2
        /// </summary>
        public int m_Plant_2_R = 30;
        /// <summary>
        /// Line 2 high
        /// </summary>
        public int m_Plant_2_H = 0;
        #endregion A闸门
        #region 变量
        delegate void Delg_Save(string strPath, int iType);
        /// <summary>
        /// 读取通道数据
        /// </summary>
        public static Thread Thread_Brush_C = null;
        public static Thread Thread_Brush_V = null;
        /// <summary>
        /// 刷新A扫描波形
        /// </summary>
        private Thread Thread_Brush_A = null;
        private Thread Thread_Brush_A_2 = null;

        private Thread Thread_Power = null;
        /// <summary>
        /// 刷新涂层数据
        /// </summary>
        private Thread Thread_Brush_Coat = null;
        /// <summary>
        /// 给视觉系统发送距离
        /// </summary>
        private Thread Thread_Send_mm = null;
        /// <summary>
        /// 发送距离值
        /// </summary>
        private bool m_bl_Send_mm = true;
        /// <summary>
        /// 速度刷新
        /// </summary>
        private Thread Thread_Brush_Distanc = null;
        ///// <summary>
        ///// A扫描
        ///// </summary>
        //public static Thread Thread_Brush_A = null;
        /// <summary>
        /// A扫描事件
        /// </summary>
        delegate void Delg_PlantWave();

        public static Thread Thread_Show = null;
        /// <summary>
        /// 系统变量,使用检测缓存
        /// </summary>
        SysInfo m_Sys_C = new SysInfo();

        /// <summary>
        /// 数据保存状态返回 1：成功 其他：异常
        /// </summary>
        int m_iSaveRet = 0;
        KeyboardHook k_hook;

        #region 窗体
        From.Frm_TOFD m_frm_Tofd = new From.Frm_TOFD();
        From.Frm_Move m_frm_Move = new From.Frm_Move();

   //     Frm_Video m_frm_Video = new Frm_Video();
        /// <summary>
        /// 缺陷样板图
        /// </summary>
    //    From.Frm_Template m_frm_Temp = new From.Frm_Template();
        /// <summary>
        /// 标注
        /// </summary>
  //      From.Frm_Bz m_frm_Bz = new From.Frm_Bz();
        #endregion 窗体

        #region 通讯变量
        /// <summary>
        /// 串口连接状态
        /// </summary>
        bool[] m_blArrLinkState;
        /// <summary>
        /// 串口名称集合
        /// </summary>
        string[] m_arrPort_Names;


        #endregion 通讯
        #endregion 变量
        public Frm_Main_C()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        //    PictureBox.CheckForIllegalCrossThreadCalls = false;
        }
        private void Init_Mult_Memory(string Memory_Name= "Distan_Mul")
        {
            if (m_mmf == null)
            {
                m_mmf = MemoryMappedFile.CreateOrOpen(Memory_Name, 4, MemoryMappedFileAccess.ReadWrite);
                if (m_mmf == null)
                    m_mmf = MemoryMappedFile.CreateNew(Memory_Name, 4, MemoryMappedFileAccess.ReadWrite);

                m_mmv = m_mmf.CreateViewStream();
            }
          //  Set_Memory(22);
        }
        public  void Set_Memory(int iDistance)
        {
            if (m_mmf != null )
            {
               m_mmv.Seek(0, System.IO.SeekOrigin.Begin);
                byte[] ls1 = BitConverter.GetBytes(iDistance);
                m_mmv.Write(ls1, 0, ls1.Length);
            }
        }
        public int Get_Memory()
        {
            int _iRet = 0;
            if (m_mmf != null)
            {
                byte[] _Lst = new byte[4];
                m_mmv.Seek(0, System.IO.SeekOrigin.Begin);
                m_mmv.Read(_Lst, 0, _Lst.Length);

                _iRet = BitConverter.ToInt16 (_Lst, 0);
            }
            return _iRet;
        }
        /*
        /// <summary>
        /// 翻页按钮位置初始化
        /// </summary>
        private void Bt_Page_L_R_U_D_Init()
        {
            Bt_Data_Left.Left = 25;
            Bt_Data_Right.Left = Pic_C.Width - Bt_Data_Right.Width;
            Bt_Data_Left.Top = (Pic_C.Height - Bt_Data_Left.Height) / 2;
            Bt_Data_Right.Top = Bt_Data_Left.Top;

            Pan_C.Dock = DockStyle.Fill;
            Pic_Coat_All.Dock = DockStyle.Fill;
            //Application.DoEvents();

            Bt_C_All_Left.Left = 45;
            Bt_C_All_Up.Left = (Pic_C.Width - Bt_C_All_Up.Width) / 2;
            Bt_C_All_Up.Top = 25;
            Bt_C_All_Down.Left = Bt_C_All_Up.Left;
        }
        #region 涡流
        delegate void Delg_ECT(int iType, int iData = 0);
        private void Get_Ect_Data(int iType, int iData = 0)
        {
            switch (iType)
            {
                case 30://标定数据
                    bl_BtSignal = true;
                    Plant_A_ECT();
                    Save_Bd_Data();
                    SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.SendData_ECT(1000, 60, 0,
                        SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_iBrushTipPh.ToString ());
                    break;
                default:
                    bl_BtSignal = false;
                    break;
            }
            if (iType == 10)//状态查询
                Ect_SendState(1);
            if (iData > 0 && iData < 5)
                SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.g_i_SysStage = iData;
            else if (iType == 40)//数据返回
                Ect_SendState(4);
            else if (iType == 40 + ECT_DLL.Cl_ECT.m_iRomoteNum - 1)
                Ect_SendState(2);
            else if (iType == 60)
            {
                SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_iBrushTipPh = iData;
                SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.Write_One("m_iBrushTipPh", iData.ToString());
            }
            else
                Ect_SendState(2);
        }
        /// <summary>
        /// 发送指示灯 0:发送命令  1：探头联机成功 2: 接收命令返回
        /// </summary>
        /// <param name="iType">0:发送命令  1：探头联机成功 2: 接收命令返回</param>
        private void Ect_SendState(int iType = 0)
        {
            switch (iType)
            {
                case 1:
                    Bt_S.Enabled  = false ;
                    Bt_R.Enabled  = true;
                    break;
                case 3:
                     Bt_S.Enabled = true;
                    Bt_R.Enabled = false;
                    break;
                case 0:
                case 2:
                case 4:
                    Bt_S.Enabled = true ;
                    Bt_R.Enabled = false ;
                  
                    SysInfo.csInter.WaitTime(0.1f);
                 
                    Bt_S.Enabled = false;
                    Bt_R.Enabled = true;
                    break;
            }
            System.Windows.Forms.Application.DoEvents();
        }
        /// <summary>
        /// 将标定数据存储
        /// </summary>
        private void Save_Bd_Data()
        {
            //将标定数据存储
            if (SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.blNetTrue == false) return;
          
            if (SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_ArrECT_Cali[0].blCurrCmm)
            {
                SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_ArrECT_Cali[0].iWaveStandNo++;
                Bd_Arr_Add();
            }
        }
        /// <summary>
        /// 加载标定数据
        /// </summary>
        private void Bd_Arr_Add()
        {
            CL_ECT_Bd _Bd = new CL_ECT_Bd();
            _Bd.ID = SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_ArrECT_Cali[0].iWaveStandNo;

            int _i_Len = SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_ArrECT_Cali[0].m_iArrWave.Length;
            _Bd.m_iArrWave = new float[_i_Len];

            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Bd.m_iArrWave, 0);//   
            Marshal.Copy(SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_ArrECT_Cali[0].m_iArrWave, 0, IntPtArr, _i_Len);

            SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_lst_Bd.Add(_Bd);
        }
        /// <summary>
        /// 接收到涡流数据：标定、状态询问
        /// </summary>
        private void Show_Get_ETC_Data(int iType, int iData = 0)
        {
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_ECT ms = new Delg_ECT(Get_Ect_Data);
                    this.Invoke(ms, new object[] { iType,iData });
                }
                catch
                { }
            }
            else
            {
                Get_Ect_Data(iType, iData);
            }
        }
        
            delegate void Delg_ECT_A();
        /// <summary>
        /// 画图：脉冲涡流
        /// </summary>
        private void Plant_A_ECT()
        {
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_ECT_A ms = new Delg_ECT_A(Plant_A_ECT_Run);
                    this.Invoke(ms, new object[] {  });
                }
                catch
                { }
            }
            else
            {
                Plant_A_ECT_Run();
            }
        }
        private void Plant_A_ECT_Run()
        {
            float _fl_Half = ECT_DLL.Cl_ECT.m_iRomoteNum / 2;
            int _i_Half =(int)( Math.Round(_fl_Half, 0));
            int _iNum = ECT_DLL.Cl_ECT.m_iRomoteNum > 4 ? _i_Half : ECT_DLL.Cl_ECT.m_iRomoteNum;

            Plant_Wave_ECT_A_Test(Pic_A,
                                    SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_ArrECT_Cali[0].m_iArr_Len,
                                    SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_ArrECT_Cali[0].m_iArrWave,
                                    _iNum, SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_ArrECT_Buff, 0);
            if (ECT_DLL.Cl_ECT.m_iRomoteNum > 4)
                Plant_Wave_ECT_A_Test(Pic_B,
                                    SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_ArrECT_Cali[0].m_iArr_Len,
                                    SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_ArrECT_Cali[0].m_iArrWave,
                                    ECT_DLL.Cl_ECT.m_iRomoteNum - _iNum, SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_ArrECT_Buff, _iNum);
        }
        #endregion 涡流
        /// <summary>
        /// 显示0：超声 1：涂层
        /// </summary>
        /// <param name="iType"></param>
       private void Show_E0_C1(int iType=0)
        {
            int _iW = Pan_C.Width;
            int _iH = Pan_C.Height;
            Pic_Coat_All.Width=Pic_C_All .Width ;
            Pic_Coat_All.Height= Pic_C_All.Height ;
            Pan_C.Dock = DockStyle.Fill;
            Pic_Coat_All.Dock = DockStyle.Fill;
            switch (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2)
            {
                case 3:
                    if (SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have)
                    {
                        switch (iType)
                        {
                            case 0://涂层
                                if (Rad_2.Checked == false)
                                {
                                    SetTran(Pic_C, Bt_Data_Left);
                                    SetTran(Pic_C, Bt_Data_Right);
                                    Pic_C.Visible = true;
                                    Pic_Coat.Visible = false;
                                }
                                else
                                {
                                    Pic_C_All.Visible = true;
                                    Pic_Coat_All.Visible = false;
                                }
                                break;
                            case 1://超声
                                if (Rad_2.Checked == false)
                                {
                                    SetTran(Pic_Coat, Bt_Data_Left);
                                    SetTran(Pic_Coat, Bt_Data_Right);

                                    Pic_C.Visible = false;
                                    Pic_Coat.Visible = true;
                                    Pic_Coat.Dock = DockStyle.Fill;
                                }
                                else//涂层
                                {
                                    Pic_C_All.Visible = false;
                                    Pic_Coat_All.Visible = true;
                                }
                                break;
                        }
                    }
                    else
                    {
                        Ck_E.Checked = false;
                        Ck_E.Visible = false;

                        Pic_C.Visible = false; 
                        Pic_Coat.Dock = DockStyle.Fill;
                        Pic_C_All.Visible = false;
                        if (Rad_2.Checked == false)
                        {
                            Pic_Coat.Visible = true;
                            SetTran(Pic_Coat, Bt_Data_Left);
                            SetTran(Pic_Coat, Bt_Data_Right);
                        }
                        else
                            Pic_Coat_All.Visible = true;
                    }
                    break;
                default:
                    Ck_E.Visible = false;
                    if (Rad_2.Checked == false)
                    {
                        SetTran(Pic_C, Bt_Data_Left);
                        SetTran(Pic_C, Bt_Data_Right);
                        Pic_C.Visible = true;
                        Pic_Coat.Visible = false;
                    }
                    else
                    {
                        Pic_C_All.Visible = true;
                        Pic_Coat_All.Visible = false;
                    }
                    break ;
            }
        //    Application.DoEvents();
        }

        private void Show_E0_C1_Report(int iType = 0)
        {
            int _iW = Pan_C.Width;
            int _iH = Pan_C.Height;
            Pic_Coat_All.Width = Pic_C_All.Width;
            Pic_Coat_All.Height = Pic_C_All.Height;
            Pan_C.Dock = DockStyle.Fill;
            Pic_Coat_All.Dock = DockStyle.Fill;
            switch (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2)
            {
                case 3:
                    if (SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have)
                    {
                        switch (iType)
                        {
                            case 1://超声
                                Pic_C_All.Visible = true;
                                Pic_Coat_All.Visible = false;

                                break;
                            case 0://涂层
                                Pic_C_All.Visible = false;
                                Pic_Coat_All.Visible = true;
                                break;
                        }
                    }
                    else
                    {
                        Ck_E.Checked = false;
                        Ck_E.Visible = false;

                        Pic_C.Visible = false;
                        Pic_Coat.Dock = DockStyle.Fill;
                        Pic_Coat_All.Visible = true;
                    }
                    break;
                default:
                    Ck_E.Visible = false;
                    Pic_C_All.Visible = true;
                    Pic_Coat_All.Visible = false;
                    break;
            }
            Application.DoEvents();
        }

        */
        private void Frm_Loading()
        {
         //   showtask();
            Tb_Main.Height = this.Height;
            //1 初始化
            for (int i = 2; i < 6; i++)
                Tb_Contrl.RowStyles[i] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0);
            for (int i = 8; i < 16; i++)
                Tb_Contrl.RowStyles[i] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0);
            #region 标题栏
            int _iCol = 0;
            Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 30);//25
            Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 30);//19
            Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent,0f);
            Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 25);
            Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 15);
            #endregion

            if (Screen.PrimaryScreen.Bounds.Width == 1280)
            {
                #region 字体设置
                button1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                Txt_X_JL.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                button4.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                Txt_Y_JL.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                button5.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                Txt_Gchd.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                Txt_Date.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                Txt_Time.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                Bt_Dc.Font = new System.Drawing.Font("黑体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                Lb_Ver.Font = new System.Drawing.Font("黑体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                //   Bt_Row_Num.Font = new System.Drawing.Font("黑体", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                #endregion
            }
   //         SysInfo.Language(this, "Main_C");
          
            #region 手柄控制4级挡位
            SysInfo.m_i_Add_MarkDisc = int.Parse(SysInfo.csInter.IniReadDefine("System", "m_i_Add_MarkDisc", "9", SysInfo.HardFileName));
            SysInfo.Time_Key_Down = int.Parse(SysInfo.csInter.IniReadDefine("System", "Time_Key_Down", "10", SysInfo.HardFileName));
            Time_Key.Interval = int.Parse(SysInfo.csInter.IniReadDefine("System", "Time_Key", "200", SysInfo.HardFileName));
            SysInfo.m_bt_Arr[0] = byte.Parse(SysInfo.csInter.IniReadDefine("System", "By_Hand_Speed_0", "10", SysInfo.HardFileName));
            SysInfo.m_bt_Arr[1] = byte.Parse(SysInfo.csInter.IniReadDefine("System", "By_Hand_Speed_1", "30", SysInfo.HardFileName));
            SysInfo.m_bt_Arr[2] = byte.Parse(SysInfo.csInter.IniReadDefine("System", "By_Hand_Speed_2", "60", SysInfo.HardFileName));
            SysInfo.m_bt_Arr[3] = byte.Parse(SysInfo.csInter.IniReadDefine("System", "By_Hand_Speed_3", "100", SysInfo.HardFileName));
            #endregion

      //      Win_Task_H_S(true );
            SysInfo.m_strLinkMsg = "";

            Init();
            SysInfo.m_blXunJi_Prog_0PC_1YY = int.Parse(SysInfo.csInter.IniReadDefine("XunJi", "m_blXunJi_Prog_0PC_1YY", "0", SysInfo.HardFileName)) == 1;
            if (SysInfo.m_blXunJi_Prog_0PC_1YY) ConnectToServer();//启动寻迹连接服务器

            //2 车体连接
            //Init_Video();

            Run_Video();

            //3 相机启动
            Lb_Init.Visible = true;// = SysInfo.m_iLanguage == 0 ? "正在启动相机..." : "Starting the camera...";

            //4 线程启动：显示车体距离信息

            if (Thread_Brush_C != null) Thread_Brush_C.Abort();
            Thread_Brush_C = new Thread(new ThreadStart(Thread_Plant_C));
            Thread_Brush_C.Priority = ThreadPriority.Highest;
            Thread_Brush_C.Name = "Thread_Brush_C";
            Thread_Brush_C.IsBackground = true;
            Thread_Brush_C.Start();
            
            int _ih = Tb_Main.Height;
            
        }
        /// <summary>
        /// 启动相机
        /// </summary>
        private void Run_Video()
        {
            if (Thread_Brush_V != null) Thread_Brush_V.Abort();
            Thread_Brush_V = new Thread(new ThreadStart(Init_Video));
            Thread_Brush_V.Priority = ThreadPriority.Highest;
            Thread_Brush_V.Name = "Init_Video";
            Thread_Brush_V.IsBackground = true;
            Thread_Brush_V.Start();
        }
        #region 
        /// <summary>
        /// 更换相机
        /// </summary>
        /// <param name="iType"></param>
        private void Init_ChangeVideo(int iType)
        {
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_ShowVideo_Xj del_Show = new Delg_ShowVideo_Xj(Set_V_0F_1B);
                    this.Invoke(del_Show, new object[] { iType });
                }
                catch { }
            }
            else
            {
                Set_V_0F_1B(iType);
            }
        }
        /// <summary>
        /// 寻迹、相机切换
        /// </summary>
        /// <param name="iVal"></param>
        private void Set_V_0F_1B(int iVal)
        {
            Chg_Video();
        }
        delegate void Delg_Video_Change();
        private void Chg_Video()
        {
            if (SysInfo.m_iShowPhone < 5)
                SysInfo.m_iShowPhone++;
            else
                SysInfo.m_iShowPhone = 1;

            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_Video_Change del_Show = new Delg_Video_Change(ShowVideoFBM);
                    this.Invoke(del_Show, new object[] { });
                }
                catch { }
            }
            else
            {
                ShowVideoFBM();
            }
        }
        private void ShowVideoFBM()
        {
            SysInfo.csInter.INIWriteValue("Cam", "m_iShowPhone", SysInfo.m_iShowPhone.ToString(), SysInfo.HardFileName);
            if (SysInfo.m_bl_Qhzy)
            {
                switch (SysInfo.m_iShowPhone)
                {
                    case 1:
                        Bt_V_Chge.Text = SysInfo.m_iLanguage == 0 ? "前/左右视" : "Front/\r\nLeft Right view";
                        break;
                    case 2:
                        Bt_V_Chge.Text = SysInfo.m_iLanguage == 0 ? "后/左右视" : "Rear/\r\nLeft Right view";
                        break;
                    case 3:
                        Bt_V_Chge.Text = SysInfo.m_iLanguage == 0 ? "左右视" : "Left Right view";
                        break;
                    case 4:
                        Bt_V_Chge.Text = SysInfo.m_iLanguage == 0 ? "前视" : "Front\r\nview";
                        break;
                    case 5:
                        Bt_V_Chge.Text = SysInfo.m_iLanguage == 0 ? "后视" : "Rear\r\nview";
                        break;
                }
            }
            else
            {
                switch (SysInfo.m_iShowPhone)
                {
                    case 1:
                        Bt_V_Chge.Text = SysInfo.m_iLanguage == 0 ? "前/中视" : "Front/\r\nMiddle view";

                        break;
                    case 2:
                        Bt_V_Chge.Text = SysInfo.m_iLanguage == 0 ? "前/后视" : "Front/\r\nrear view";
                        break;
                    case 3:
                        Bt_V_Chge.Text = SysInfo.m_iLanguage == 0 ? "前视" : "Front\r\nview";
                        break;
                    case 4:
                        Bt_V_Chge.Text = SysInfo.m_iLanguage == 0 ? "后视" : "Rear\r\nview";
                        break;
                    case 5:
                        Bt_V_Chge.Text = SysInfo.m_iLanguage == 0 ? "中视" : "Middle\r\nview";
                        break;
                }
            }
            string _strT_IP = (SysInfo.csInter.IniReadDefine("Cam", "m_Ip_4", "", Application.StartupPath + "\\database\\HardConfig.ini")).Trim();
            string[] _sPara = _strT_IP.Split('.');
            Bt_V_Chge.Visible = true;
            if (_sPara.Length == 4)
            {
                SysInfo.m_iShowPhone = 0;
                Bt_V_Chge.Text = SysInfo.m_iLanguage == 0 ? "切换" : "Switch\r\ncamera";
                Bt_V_Chge.Visible = false;
            }

            SysInfo.m_ServerUI.SendData_M(0, SysInfo.m_iShowPhone + "," + "", "5.0");
   //         SysInfo.csInter.WaitTime(0.1);
        }
        #region 寻迹
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
        /// <summary>
        /// 1:100 0:50
        /// </summary>
        int m_i50_100 = 0;
        private void MsgShow()
        {
            if (SysInfo.m_iTrack_Type == 2)
            {
                if (m_Axis_X.Length != 660)
                {
                    m_i50_100 = SysInfo.m_lst_Weld[0].dbArrData.Length == 660 ? 1 : 0;
                    m_maxX = m_i50_100 == 1 ? 100 : 50;//100
                    m_maxZ = m_i50_100 == 1 ? 100 : 80;
                    m_scaleX = m_i50_100 == 1 ? 10 : 5;
                    m_scaleZ = m_i50_100 == 1 ? 10 : 8;

                    float _flD = 0;
                    int _iNum = SysInfo.m_lst_Weld[0].dbArrData.Length;
                    m_Axis_X = new float[_iNum];
                    for (int i = 1; i < _iNum; i++)
                    {
                        _flD += 0.15f;
                        m_Axis_X[i] = float.Parse(_flD.ToString("f1"));
                    }
                }
                //if (m_i50_100 == 1)
                //{
                //    SysInfo.m_fl_Larser_Center = (100f * SysInfo.m_lst_Weld[0].i_Cent / 660f);

                //    // _Cent = _f.ToString("f1") + "mm";
                //}
                //else
                //    SysInfo.m_fl_Larser_Center = (SysInfo.m_lst_Weld[0].i_Cent / 10f);
                //if (SysInfo.m_bl_Larser_UP1_Down0 == false)
                //    SysInfo.m_fl_Larser_Center = (m_i50_100 == 1 ? 100 : 50) - SysInfo.m_fl_Larser_Center;

                Lb_Tempr.Text = SysInfo.m_i_LasersTempr.ToString();
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
            try
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
                Pen p_S = new Pen(colorB, 8);//创建一个画笔对象,该画笔的颜色为黄色，笔触大小为1个像素
                Pen pGray = new Pen(Color.FromArgb(200, 200, 200), 1);//创建一个画笔对象,该画笔的颜色为灰色，笔触大小为1个像素

                Pen pGray_R = new Pen(Color.FromArgb(0, 0, 255), 1);//创建一个画笔对象,该画笔的颜色为灰色，笔触大小为1个像素
                Pen pBalck = new Pen(colorBlack, 3);//创建一个画笔对象,该画笔的颜色为白色，笔触大小为3个像素
                Pen pYellow = new Pen(colorYellow, 3);//创建一个画笔对象,该画笔的颜色为白色，笔触大小为1个像素
                pGray.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                Font font = new Font("Adobe Gothic Std", 9f, FontStyle.Bold);
                Font font_Yg = new Font("Adobe Gothic Std", 14f, FontStyle.Bold);
                Font font_H = new Font("Adobe Gothic Std", m_fl_Larser_Font, FontStyle.Bold);
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
                }//(m_Weld_Type == 0 ? "0 - 15" : "0 - 15 45-50")
                g.DrawString("采集区域 ：黄线两侧", font, brush, 50, axisInfo_bottom + 15);
                double _D = 0;
                for (uint j = 1; j <= xNum; j++)
                {
                    if (j == 1 || j == 9)//&& m_Weld_Type == 1)
                        g.DrawLine(pYellow, axisInfo_left + scaleXlen * j, axisInfo_bottom, axisInfo_left + scaleXlen * j, axisInfo_top);
                    else
                        g.DrawLine(pGray, axisInfo_left + scaleXlen * j, axisInfo_bottom, axisInfo_left + scaleXlen * j, axisInfo_top);

                    _D = ScaleX * j;
                    //  str =  ((SysInfo.m_bl_Larser_UP1_Down0 == false) ? (m_i50_100 == 1 ? 100 : 50) - _D : _D).ToString();

                    g.DrawString(_D.ToString(), font, brush, axisInfo_left - 20 + scaleXlen * j, axisInfo_bottom);
                }
                //绘制轮廓线
                if (AxisCount > 10)
                {
                    for (uint k = 0; k < AxisCount - 1; k++)
                    {
                        if (k == WeldPosition.i_Cent)
                        {
                            //Txt_Gd.Text = WeldPosition.dbDepth.ToString("f2");//     g.DrawString("C:" + (SysInfo.m_fl_Larser_Center.ToString ("f1")+"mm"), font_H, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-m_i_Larser_H_Pianyi));// + axisInfo_bottom - ZZoom * Axis_Z[k]
                            //Txt_Kd .Text = SysInfo.m_fl_Larser_Center.ToString("f1");//  g.DrawString("余高：" + WeldPosition.dbDepth.ToString("f2") + "mm", font_Yg, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-22-m_i_Larser_H_Pianyi));// + axisInfo_bottom - ZZoom * Axis_Z[k])

                            g.DrawString("C:" + (SysInfo.m_fl_Larser_Center.ToString("f1") + "mm"), font_H, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-m_i_Larser_H_Pianyi + axisInfo_bottom - ZZoom * Axis_Z[k]));
                            g.DrawString("余高：" + WeldPosition.dbDepth.ToString("f2") + "mm", font_Yg, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-22 - m_i_Larser_H_Pianyi + axisInfo_bottom - ZZoom * Axis_Z[k]));
                            g.DrawString("序号：" + WeldPosition.i_Cent, font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(20 + axisInfo_bottom - ZZoom * Axis_Z[k]));

                            g.DrawLine(pRed, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-6 + axisInfo_bottom - ZZoom * Axis_Z[k]),
                                      (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(6 + axisInfo_bottom - ZZoom * Axis_Z[k + 0]));
                        }
                        if (k == WeldPosition.i_Start)
                        {
                            g.DrawString("起点：" + WeldPosition.i_Start, font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(20 + axisInfo_bottom - ZZoom * Axis_Z[k]));

                            g.DrawLine(p_S, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-6 + axisInfo_bottom - ZZoom * Axis_Z[k]),
                                      (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(6 + axisInfo_bottom - ZZoom * Axis_Z[k + 0]));
                        }
                        if (k == WeldPosition.i_End)
                        {
                            g.DrawString("终点：" + WeldPosition.i_End, font, brush, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(20 + axisInfo_bottom - ZZoom * Axis_Z[k]));

                            g.DrawLine(p_S, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(-6 + axisInfo_bottom - ZZoom * Axis_Z[k]),
                                      (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(6 + axisInfo_bottom - ZZoom * Axis_Z[k + 0]));
                        }

                        _iShowNo = -1;
                        if (k >= (int)(WeldPosition.i_Start) && k <= (int)(WeldPosition.i_End))
                            _iShowNo = 1;

                        if (_iShowNo > -1)
                        {
                            g.DrawLine(pRed, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k]),
                                             (float)(axisInfo_left + XZoom * Axis_X[k + 1]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));
                        }
                        else
                            g.DrawLine(PGreen, (float)(axisInfo_left + XZoom * Axis_X[k]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k]),
                                               (float)(axisInfo_left + XZoom * Axis_X[k + 1]), (float)(axisInfo_bottom - ZZoom * Axis_Z[k + 1]));
                    }
                }
                hWnd.CreateGraphics().DrawImage(bmp, 0, 0);
                pRed.Dispose(); PGreen.Dispose(); pYellow.Dispose();
                pGray.Dispose(); pBalck.Dispose(); font.Dispose();
                brush.Dispose(); bmp.Dispose(); g.Dispose();
                pGray_R.Dispose();
            }
            catch { }
        }

        #endregion 寻迹

        private void Link_New_V(bool blRun = true)
        {
            Pan_V_Main.Left = 0;
            Pan_V_Main.Top = 0;
            Pan_V_Main.Width = this.Width;
            Pan_V_Main.Height = this.Height;
            Pan_Xj.Width = Pan_V_Main.Width + 20;
            Pan_Xj.Height = Pan_V_Main.Height;
            Pan_Xj.Top = 0;
            Pan_Xj.Left = 0;


            if (blRun )
                SysInfo.m_ServerUI.SendData_M(0, "", "99");

            if (SysInfo.m_ServerUI.m_TcpC != null)
            {
                if (SysInfo.m_ServerUI.m_TcpC.Connected == false)
                {
                    //    LinkServer();
            //        SysInfo.csInter.WaitTime(0.1);
                }
            }
            SysInfo.csInter.WaitTime(0.1);
            // MessageBox.Show("3");
            m_p = new System.Diagnostics.Process();

            SysInfo.csInter.INIWriteValue("VIDEO", "HaveRun_1", "1", SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue("VIDEO", "HaveRun_2", "1", SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue("VIDEO", "HaveRun_3", "1", SysInfo.HardFileName);

            m_p.StartInfo.FileName = Application.StartupPath + "\\ClimbVideo.exe";
            m_p.StartInfo.UseShellExecute = false;
            try
            {
                m_p.Start();
                m_p.WaitForInputIdle();

                DateTime dtStar = DateTime.Now;
                bool [] _blArrLink =new bool[4];
                bool[] _bl_ArrHave = new bool[4];

                string m_Net_V = Application.StartupPath + "\\database\\HardConfig.ini";

                _bl_ArrHave[0]= SysInfo.csInter.INIReadValue("Cam", "m_Ip_1", "", m_Net_V).Split ('.').Length ==4;
                _bl_ArrHave[1] = SysInfo.csInter.INIReadValue("Cam", "m_Ip_2", "", m_Net_V).Split('.').Length == 4;
                _bl_ArrHave[2] = SysInfo.csInter.INIReadValue("Cam", "m_Ip_3", "", m_Net_V).Split('.').Length == 4;
                _bl_ArrHave[3] = SysInfo.csInter.INIReadValue("Cam", "m_Ip_4", "", m_Net_V).Split('.').Length == 4;

                while (true)
                {
                  //  Application.DoEvents();

                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > 0.1)
                    {
                        if (_bl_ArrHave[0] && SysInfo.csInter.INIReadValue("VIDEO", "HaveRun_1", "1", SysInfo.HardFileName) == "0")
                        { _blArrLink[0] = true; break; }
                        if (_bl_ArrHave[1] && SysInfo.csInter.INIReadValue("VIDEO", "HaveRun_2", "1", SysInfo.HardFileName) == "0")
                        { _blArrLink[1] = true; break; }
                        if (_bl_ArrHave[2] && SysInfo.csInter.INIReadValue("VIDEO", "HaveRun_3", "1", SysInfo.HardFileName) == "0")
                        { _blArrLink[2] = true; break; }
                        if (_bl_ArrHave[3] && SysInfo.m_bl_Qhzy &&  SysInfo.csInter.INIReadValue("VIDEO", "HaveRun_4", "1", SysInfo.HardFileName) == "0")
                        { _blArrLink[3] = true; break; }

                        if (DateTime.Now.Subtract(dtStar).TotalSeconds > 10)
                        {  break; }
                    }
                }
         
            bool _blVideo_Link = true;
                for (int i = 0; i < 4; i++)
                    if (_bl_ArrHave[i] && _blArrLink[i]==false )
                    { _blVideo_Link = false; break; }

                //if (_blVideo_Link == false)
                //   SysInfo.m_strLinkMsg +=   (SysInfo.m_iLanguage == 0 ? " 相机" : " Camera"); SysInfo.m_iLanguage == 0 ? "联机失败": "connection fail"

                if (SysInfo.m_strLinkMsg!="")
                    Lb_Init .Text = SysInfo.m_strLinkMsg+" :" + (SysInfo.m_iLanguage == 0 ? " 联机失败" : " connection failed");
            }
            catch { }
            string _strM = SysInfo.csInter.IniReadDefine("Cam", "FZ_ABC", "000", SysInfo.HardFileName);
    //        SysInfo.csInter.WaitTime(0.1);
            SysInfo.m_ServerUI.SendData_M(0, _strM, "11.0");
            SysInfo.m_iShowPhone = int.Parse(SysInfo.csInter.IniReadDefine("Cam", "m_iShowPhone", "2", SysInfo.HardFileName));
            ShowVideoFBM();

            VideoSize();
        }
        /// <summary>
        /// 视频尺寸调整
        /// </summary>
        private void VideoSize(int iType = 0)
        {
          //  SysInfo.WaitTime(100);
            try
            {
                Pan_Xj.Left = 0;
                Pan_Xj.Top = 0;
                //       Pan_Xj.Width = Pan_V.Width - Rad_V_F.Width;// - 7;// +30;
                Pan_Xj.Height = Pan_V_Main.Height;

                SetParent(m_p.MainWindowHandle, Pan_V_Main.Handle);// SetParent(p.MainWindowHandle, panel1.Handle);
                if (iType == 1)
                    MoveWindow(m_p.MainWindowHandle, -7, -7, Pan_V_Main.Width, Pan_V_Main.Height + 10, true);//+ 40Pan_V
                else
                    SetLarm();
            }
            catch(Exception ee) 
            {
                MessageBox.Show(ee.Message);
            }//-7  -32
        }
        private void SetLarm()
        {
            try
            {
                MoveWindow(m_p.MainWindowHandle, -8, -8, Pan_V_Main.Width + 30, Pan_V_Main.Height + 30, true);
            }
            catch (Exception ee)
            { }
            //  Bt_V_Chge.Top = Pan_V.Height - 20;
            //   Ck_Larm.Top = Pan_V.Height - 15;
        }
      
        private void Larm_Top()
        {
            Ck_Larm.Top = 6;
        //    Bt_V_Chge.Top = 8;
        }
        private void Larm()
        {
            if (Ck_Larm.Checked)
            {
                Tb_Lbyou.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 50);
                Tb_Lbyou.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 50);
            }
            else
            {

                Tb_Lbyou.RowStyles[0] = new System.Windows.Forms.RowStyle(SizeType.Percent, 100);
                Tb_Lbyou.RowStyles[1] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0);
            }
            SetLarm();
            SysInfo.m_i_Alarm = Ck_Larm.Checked ? 1 : 0;
            Larm_Top();
            SysInfo.csInter.INIWriteValue("System", "m_i_Alarm", SysInfo.m_i_Alarm.ToString(), SysInfo.HardFileName);
        }
       
        #endregion 
        private void Init_Video()
        {
            string _strT_IP = (SysInfo.csInter.IniReadDefine("Cam", "m_Ip_4", "", Application.StartupPath + "\\database\\HardConfig.ini")).Trim();
            SysInfo.m_bl_Qhzy = _strT_IP != "" && _strT_IP.Split('.').Length == 4;
            SysInfo.m_bl_Video_Init = true;
        //    Bt_B_L.Text = SysInfo.m_iLanguage == 0 ? "放大" : "Magnify";
            Time_Key.Interval = int.Parse(SysInfo.csInter.IniReadDefine("System", "Time_Key", "100", SysInfo.HardFileName));

            Ck_Ri_Zhi.Checked = SysInfo.m_i_Lars_RiZhi == 1;
            Ck_Track_Auto.Checked = SysInfo.m_bl_Track_Auto;
            SysInfo.m_fl_Gain = float.Parse(SysInfo.csInter.IniReadDefine("System", "m_fl_Gain", "5", SysInfo.HardFileName));

            SysInfo.m_bl_Larser_UP1_Down0 = int.Parse(SysInfo.csInter.IniReadDefine("Larser", "m_bl_Larser_UP1_Down0", "0", Application.StartupPath + "\\database\\HardConfig.ini")) == 1;
            m_i_Larser_H_Pianyi = int.Parse(SysInfo.csInter.IniReadDefine("Larser", "m_i_Larser_H_Pianyi", "25", Application.StartupPath + "\\database\\HardConfig.ini"));
            m_fl_Larser_Font = float.Parse(SysInfo.csInter.IniReadDefine("Larser", "m_fl_Larser_Font", "18", Application.StartupPath + "\\database\\HardConfig.ini"));
            SysInfo.m_iShowPhone = int.Parse(SysInfo.csInter.IniReadDefine("VIDEO", "m_iF_B", "2", Application.StartupPath + "\\database\\HardConfig.ini"));
            SysInfo.g_Msg_InterFace.Inter_ChangeVideo -= new ClassLib_TestData.MsgInterFace.OnChangeVideo(Init_ChangeVideo);
            SysInfo.g_Msg_InterFace.Inter_ChangeVideo += new ClassLib_TestData.MsgInterFace.OnChangeVideo(Init_ChangeVideo);
            this.Top = SysInfo.m_i_Pic_A_Height + 40;
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width - 150;
            SysInfo.m_bl_Show_Video = true;
            float _flD = 0;
            for (int i = 1; i < 500; i++)
            {
                _flD += 0.1f;
                m_Axis_X[i] = float.Parse(_flD.ToString("f1"));
            }
            #region 前视相机按钮是否显示
            string _strP_1 = SysInfo.csInter.IniReadDefine("Cam", "m_Ip_1", "", Application.StartupPath + "\\database\\HardConfig.ini");
            m_strP = SysInfo.csInter.IniReadDefine("Cam", "m_Ip_2", "", Application.StartupPath + "\\database\\HardConfig.ini");
            m_strP_3 = SysInfo.csInter.IniReadDefine("Cam", "m_Ip_3", "", Application.StartupPath + "\\database\\HardConfig.ini");

            #endregion
            #region 寻迹参数
            Txt_Gain.Text = SysInfo.m_fl_Gain.ToString("f2");
            Txt_Bg.Text = SysInfo.m_i_Bg_Time.ToString();
            Cmb_Ys.Text = SysInfo.m_i_WaitTimeNum_Max.ToString();

            Txt_Limt_L.Text = SysInfo.csInter.IniReadDefine("System", "Txt_Limt_L", "0.3", SysInfo.HardFileName);
            Txt_Limt_R.Text = SysInfo.csInter.IniReadDefine("System", "Txt_Limt_R", "0.3", SysInfo.HardFileName);

            Track_L.Value = (int)(float.Parse(Txt_Limt_L.Text) * 10);
            Track_R.Value = (int)(float.Parse(Txt_Limt_R.Text) * 10);
            //寻迹相机客户端连接
            SysInfo.g_Msg_InterFace.Inter_GetServe_CgXj -= new ClassLib_TestData.MsgInterFace.OnGetCgXjServe_Data(ShowServeData);
            SysInfo.g_Msg_InterFace.Inter_GetServe_CgXj += new ClassLib_TestData.MsgInterFace.OnGetCgXjServe_Data(ShowServeData);
            #endregion

            Link_New_V();

            Larm_Top();
            Ck_Larm.Checked = SysInfo.m_i_Alarm == 1;
            Larm();
            //Bt_V_Chge.Left = Pan_V.Width - Bt_V_Chge.Width-20;
            //Ck_Larm.Left = Bt_V_Chge.Left-10- Ck_Larm.Width ;
            SysInfo.Language(this, "Frm_Video");
           // this.Text = SysInfo.m_iLanguage == 0 ? "寻迹/视频" : "Tracking/Video";

            ShowVideoFBM();
            m_blActive = true;

        //    Bt_V_Chge.Top = Bt_B_L.Top;
       //     Bt_B_L.Visible = true;
            SysInfo.m_bl_Video_Init = false;
        }
        // private void FrmMain()
        // {
        //     Bt_Wave_Show.Enabled = false;

        //     this.tableLayoutPanel1.ColumnStyles[0] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 50);
        //     this.tableLayoutPanel1.ColumnStyles[1] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 50);
        //     this.tableLayoutPanel1.ColumnStyles[2] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 0);
        //     this.tableLayoutPanel1.ColumnStyles[3] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 0);

        //     Tb_Contrl.Enabled = false;

        //     if (Screen.PrimaryScreen.Bounds.Width == 1280)
        //     {
        //         #region 字体设置
        //         button1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        //         Txt_X_JL.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        //         button4.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        //         Txt_Y_JL.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        //         button5.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        //         Txt_Gchd.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        //         Txt_Date.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        //         Txt_Time.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        //         Bt_Dc.Font = new System.Drawing.Font("黑体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        //         Lb_Ver.Font = new System.Drawing.Font("黑体", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        //         //   Bt_Row_Num.Font = new System.Drawing.Font("黑体", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        //         #endregion
        //     }
        //     SysInfo.Language(this, "Main_C");
        //     //  Lb_Init.Visible = false;
        //     if (SysInfo.m_iLanguage == 1)
        //     {
        //         Lb_Save.Text = "Data saving is underway, please wait...";
        //         Lb_Init.Text = "Device is initializing, please wait...";
        //         Cmb_Algorithm.Items.Clear();
        //         Cmb_Algorithm.Items.Add("Auto");
        //         Cmb_Algorithm.Items.Add("Two gates");
        //         Cmb_Algorithm.Items.Add("One gates");
        //     }
        //     #region 1 界面调整
        //     #region 手柄控制4级挡位
        //     SysInfo.m_i_Add_MarkDisc = int.Parse(SysInfo.csInter.IniReadDefine("System", "m_i_Add_MarkDisc", "9", SysInfo.HardFileName));
        //     SysInfo.Time_Key_Down = int.Parse(SysInfo.csInter.IniReadDefine("System", "Time_Key_Down", "10", SysInfo.HardFileName));
        //     Time_Key.Interval = int.Parse(SysInfo.csInter.IniReadDefine("System", "Time_Key", "200", SysInfo.HardFileName));
        //     SysInfo.m_bt_Arr[0] = byte.Parse(SysInfo.csInter.IniReadDefine("System", "By_Hand_Speed_0", "10", SysInfo.HardFileName));
        //     SysInfo.m_bt_Arr[1] = byte.Parse(SysInfo.csInter.IniReadDefine("System", "By_Hand_Speed_1", "30", SysInfo.HardFileName));
        //     SysInfo.m_bt_Arr[2] = byte.Parse(SysInfo.csInter.IniReadDefine("System", "By_Hand_Speed_2", "60", SysInfo.HardFileName));
        //     SysInfo.m_bt_Arr[3] = byte.Parse(SysInfo.csInter.IniReadDefine("System", "By_Hand_Speed_3", "100", SysInfo.HardFileName));
        //     #endregion
        //     //k_hook = new KeyboardHook();
        //     //k_hook.KeyDownEvent += new System.Windows.Forms.KeyEventHandler(hook_KeyDown);//钩住键按下 
        //     //k_hook.Start();//安装键盘钩子

        //     Win_Task_H_S(false);
        //     //0 属于单独文件类型
        //     //如果不是单独文件，就删除数据查询
        //     Bt_Start.Enabled = true;
        //     Txt_Date.Text = DateTime.Now.ToString("yyyy-MM-dd");
        //     Pic_A.Visible = SysInfo.csInter.IniReadDefine("System", "Pic_A_V", "1", SysInfo.HardFileName) == "1";

        //     SysInfo.m_strLinkMsg = "";
        //     #region 2 读配置文件                 

        //     Init();

        //     Txt_Gchd.Text = SysInfo.m_SysBuff_C.m_Plant_C.flNormal_Thickness.ToString();
        //     string _Net_V = Application.StartupPath + "\\database\\HardConfig.ini";
        //     SysInfo.m_SysBuff_C.m_Ctrl.m_bl_CS = int.Parse(SysInfo.csInter.IniReadDefine("CS", "CS", "1", SysInfo.HardFileName)) == 1;

        //     #endregion 2 

        //     for (int i = 4; i < 6; i++)
        //         Tb_Contrl.RowStyles[i] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0);

        //     if (SysInfo.m_SysBuff_C.m_Ctrl.m_bl_FY)
        //     {
        //         Tb_Contrl.RowStyles[2] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0);//视频
        //         Tb_Contrl.RowStyles[3] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0); bt_Item_T.Visible = false;
        //         Tb_Contrl.RowStyles[6] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0);//运动控制
        //         Tb_Contrl.RowStyles[7] = new System.Windows.Forms.RowStyle(SizeType.Percent, 0); Lb_Ctrl.Visible = false;
        //     }
        //     Application.DoEvents();
        //     //  ClearOneClient("ClimbVideo");


        //     Show_E0_C1(Ck_E.Checked ? 0 : 1);
        //     // Bt_Spark.Visible = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3;//有放电信号就显示

        //     SysInfo.m_Climb4.m_SysBuf.m_i_TOFD_0_Cscan_1_Mui_2 = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2;
        //     SysInfo.m_Climb4.m_SysBuf.m_bl_Have = SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have;
        //     ck_2bei.Checked = SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_2BeiSjxx;
        //     if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 ||
        //         SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)//C扫描界面标题栏
        //     {
        //         int _iCol = 0;

        //         Pan_Y_JL.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 55.78f);
        //         Pan_Y_JL.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 44.22f);
        //         _iCol = 0;
        //         #region 标题栏
        //         Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 24);//25
        //         Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 19.71f);//19
        //         Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 27f);
        //         Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 17);
        //         Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 14.33f);
        //         #endregion

        //         this.tableLayoutPanel1.ColumnStyles[0] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 60);//_fl_LR_Old_L
        //         this.tableLayoutPanel1.ColumnStyles[1] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 40);//_fl_LR_Old_R
        //     }
        //     else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)//多通道标题栏
        //     {
        //         this.tableLayoutPanel1.ColumnStyles[0] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 50);//_fl_LR_Old_L
        //         this.tableLayoutPanel1.ColumnStyles[1] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 50);//_fl_LR_Old_R

        //         int _iCol = 0;
        //         button1.Text = "距离m:";
        //         button4.Text = "厚度mm:";
        //         #region 标题栏
        //         Pan_Y_JL.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 28.02f);
        //         Pan_Y_JL.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 71.98f);
        //         _iCol = 0;
        //         Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 22.38f);
        //         Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 39.57f);
        //         Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 22.04f);
        //         Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 0f);
        //         Pan_Titl.ColumnStyles[_iCol++] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 15.91f);
        //         #endregion
        //     }
        //     if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)//
        //     {
        //         if (SysInfo.m_Tofd_C_Scan.m_Coat_DLL != null)
        //             Pan_Auto1_Hand0.BackColor = SysInfo.m_Tofd_C_Scan.m_Coat_DLL.m_i_AutoCmd == 0 ? Color.Red : Color.Black;

        //         #region 界面布局
        //         SetTran(Pic_Spark, Bt_Fd);
        //         //SetTran(Pic_B, Bt_Spark);
        //         //Bt_Win_Mw.BackColor = Color.Black;
        //         //Bt_Win_Mw.ForeColor  = Color.White ;
        //         //Ck_His_Wave.BackColor = Color.Black;
        //         //Ck_His_Wave.ForeColor = Color.White;
        ////         Ck_His_Wave.Visible = SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have;
        //         //如果没有超声，则将pic_A隐藏
        //         //if ( SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum == 0 )
        //         //{
        //         //    this.tableLayoutPanel1.ColumnStyles[0] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 0);
        //         //    this.tableLayoutPanel1.ColumnStyles[1] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100);
        //         //}
        //         //else
        //         {
        //             Pic_B_3.Dock = DockStyle.Fill;
        //             if (SysInfo.m_iLanguage == 0)
        //                 Bt_Cmd_2_3_Mode.Text = SysInfo.m_Tofd_C_Scan.m_Coat_DLL.m_i_Mode_Cellect == 2 ? "模式:单次" : "模式:连续";
        //             else
        //                 Bt_Cmd_2_3_Mode.Text = SysInfo.m_Tofd_C_Scan.m_Coat_DLL.m_i_Mode_Cellect == 2 ? "Mode:Single" : "Mode:Cont.";
        //             if (SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have == false)
        //             {
        //                 Ck_E.Visible = false;
        //                 this.tableLayoutPanel1.ColumnStyles[0] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 0);
        //                 if (SysInfo.m_Tofd_C_Scan.m_Coat_DLL != null && SysInfo.m_Tofd_C_Scan.m_Coat_DLL.m_iRomoteNum == 1)
        //                 {
        //                     Pan_TcTest.Visible = true;
        //                     Pic_B_3.Visible = false;
        //                     this.tableLayoutPanel1.ColumnStyles[1] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 44);
        //                     this.tableLayoutPanel1.ColumnStyles[2] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 48);
        //                     this.tableLayoutPanel1.ColumnStyles[3] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 8f);
        //                 }
        //                 else
        //                 {
        //                     if (SysInfo.m_Tofd_C_Scan.m_Coat_DLL != null)
        //                     {
        //                         float _fl_Wight = 100f / SysInfo.m_Tofd_C_Scan.m_Coat_DLL.m_iRomoteNum;

        //                         for (int i = 0; i < SysInfo.m_Tofd_C_Scan.m_Coat_DLL.m_iRomoteNum; i++)
        //                             this.tableLayoutPanel1.ColumnStyles[i + 1] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, _fl_Wight);
        //                     }
        //                 }
        //             }
        //             else
        //             {
        //                 Pan_TcTest.Visible = true;
        //                 Bt_Win_Mw.Visible = false;
        //                 Bt_V_Calcu.Visible = false;
        //                 this.tableLayoutPanel1.ColumnStyles[0] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 46);
        //                 this.tableLayoutPanel1.ColumnStyles[1] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 46);
        //                 this.tableLayoutPanel1.ColumnStyles[2] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 0);
        //                 this.tableLayoutPanel1.ColumnStyles[3] = new System.Windows.Forms.ColumnStyle(SizeType.Percent, 8F);
        //             }
        //             Bt_V_Calcu.Visible = SysInfo.csInter.IniReadDefine("CALCU_Pic", "Bt_V_Calcu", "0", SysInfo.HardFileName) == "1";
        //         }
        //         //pic_B 画涂层的厚度值
        //         int iRow = 0;
        //         Pan_A.RowStyles[iRow++] = new RowStyle(SizeType.Percent, 10.49f);
        //         Pan_A.RowStyles[iRow++] = new RowStyle(SizeType.Percent, 40.25f);

        //         if (SysInfo.m_Tofd_C_Scan.m_Coat_DLL.m_bl_Discharge)
        //         {
        //             Pan_A.RowStyles[iRow++] = new RowStyle(SizeType.Percent, 44.5f);
        //             Pan_A.RowStyles[iRow] = new RowStyle(SizeType.Percent, 4.76f);
        //         }
        //         else
        //             Pan_A.RowStyles[iRow] = new RowStyle(SizeType.Percent, 49.26f);
        //         Pic_Coat.Dock = DockStyle.Fill;
        //         int _iUp = 42;
        //         Ck_E.Top -= _iUp;
        //         Rad_0.Top -= _iUp;
        //         Rad_1.Top -= _iUp;
        //         Rad_2.Top -= _iUp;
        //         Rad_3.Top -= _iUp;

        //         Coat_Bt(true);

        //         if (SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have == false)
        //         {
        //             Ck_E.Visible = false;
        //             Rad_1.Text = "主界面";
        //         }
        //         if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
        //         {
        //             Video_Start();
        //         }
        //         #endregion 界面布局
        //     }
        //     else
        //         Coat_Bt(false );
        //     Ck_His_Wave.Visible = SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have;
        //     Plant_C_Init();//画图参数
        //     SysInfo.m_SysBuff.m_Climb.fl4Car_JzXs = SysInfo.m_SysBuff.m_Tofd_DLL.fl4Car_JzXs;
        //     Bt_Page_L_R_U_D_Init();
        //     Plant_Rep_Init();
        //     #endregion 1

        //     SysInfo.m_blXunJi_Prog_0PC_1YY = int.Parse(SysInfo.csInter.IniReadDefine("XunJi", "m_blXunJi_Prog_0PC_1YY", "0", SysInfo.HardFileName)) == 1;
        //     if (SysInfo.m_blXunJi_Prog_0PC_1YY) ConnectToServer();//启动寻迹连接服务器

        //     #region 3 启动A扫描线程
        //     Show_C();
        //     //   Show_A();

        //     //if (Thread_Brush_C != null) Thread_Brush_C.Abort();
        //     //Thread_Brush_C = new Thread(new ThreadStart(Thread_Plant_C));
        //     //Thread_Brush_C.Priority = ThreadPriority.Highest;
        //     //Thread_Brush_C.Name = "Thread_Brush_C";
        //     //Thread_Brush_C.IsBackground = true;
        //     //Thread_Brush_C.Start();
        //     #endregion 3  

        //     #region 4 我的超声界面参数
        //     switch (SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type)
        //     {
        //         case 0:
        //             Txt_Speed.Text = EMA_Para_RIT.SoundSpeed_2.ToString();
        //             Set_Wave_WaveType();
        //             Cmb_Algorithm.SelectedIndex = SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iAlgorithm;//算法
        //             Cmb_AvCount.Text = SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iAvCount.ToString();//积分相干数
        //             switch (SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iStep)
        //             {
        //                 case 1:
        //                     Bt_Set_Step.Text = "X:0.025uS";
        //                     break;
        //                 case 2:
        //                     Bt_Set_Step.Text = "X:0.05us";
        //                     break;
        //                 case 4:
        //                     Bt_Set_Step.Text = "X:0.1us";
        //                     break;
        //             }
        //             break;
        //         case 1:

        //             break;
        //     }
        //     SysInfo.m_SysBuff.m_Climb.m_UI_Gsb_Distan_Min = int.Parse(SysInfo.csInter.IniReadDefine("SYSinfo", "m_UI_Gsb_Distan_Min", "5", SysInfo.HardFileName));

        //     string _strP = Application.StartupPath + "\\ICO";
        //     ImgLst_UI.Images.Add(Image.FromFile(_strP + "\\csszdk.png"));
        //     ImgLst_UI.Images.Add(Image.FromFile(_strP + "\\csszsq.png"));

        //     Set_Ico(1);
        //     //---
        //     //拍照
        //     SysInfo.g_Msg_InterFace.Inter_Photo -= new ClassLib_TestData.MsgInterFace.OnData_Photo(Bt_To_Phone);
        //     SysInfo.g_Msg_InterFace.Inter_Photo += new ClassLib_TestData.MsgInterFace.OnData_Photo(Bt_To_Phone);
        //     //录像
        //     SysInfo.g_Msg_InterFace.Inter_Video -= new ClassLib_TestData.MsgInterFace.OnData_Video(Bt_To_Video);
        //     SysInfo.g_Msg_InterFace.Inter_Video += new ClassLib_TestData.MsgInterFace.OnData_Video(Bt_To_Video);

        //     //---
        //     SysInfo.g_Msg_InterFace.Inter_ShowCoat_C -= new ClassLib_TestData.MsgInterFace.OnShow_Coat_C(Show_Coat_C);
        //     SysInfo.g_Msg_InterFace.Inter_ShowCoat_C += new ClassLib_TestData.MsgInterFace.OnShow_Coat_C(Show_Coat_C);

        //     SysInfo.g_Msg_InterFace.Inter_Show_C -= new ClassLib_TestData.MsgInterFace.OnShow_C(Show_DLL_C);
        //     SysInfo.g_Msg_InterFace.Inter_Show_C += new ClassLib_TestData.MsgInterFace.OnShow_C(Show_DLL_C);


        //     SysInfo.g_Msg_InterFace.Inter_Run_C -= new ClassLib_TestData.MsgInterFace.OnRun_C(Begentest_Run);
        //     SysInfo.g_Msg_InterFace.Inter_Run_C += new ClassLib_TestData.MsgInterFace.OnRun_C(Begentest_Run);

        //     SysInfo.g_Msg_InterFace.Inter_Tofd_Para -= new MsgInterFace.OnTofd_Para(Para_Cg);
        //     SysInfo.g_Msg_InterFace.Inter_Tofd_Para += new MsgInterFace.OnTofd_Para(Para_Cg);

        //     SysInfo.g_Msg_InterFace.Inter_GetEtcReceive -= new MsgInterFace.OnETC_GetEtcReceive(Show_Get_ETC_Data);
        //     SysInfo.g_Msg_InterFace.Inter_GetEtcReceive += new MsgInterFace.OnETC_GetEtcReceive(Show_Get_ETC_Data);
        //     SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode = 1;

        //     if (SysInfo.m_Climb4.m_blLink)
        //     {
        //         if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 3)//&& SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have)
        //         {
        //             SysInfo.Coat_SentCmd_Black();
        //         }
        //     }
        //     #endregion
        //     // SetTran(Pic_C_All, Pic_3);
        //     Tb_Contrl.Enabled = true;
        //     Bt_Start.Enabled = true;
        //     Bt_Wave_Show.Enabled = true;
        //     m_bl_Active = true;

        //     ShowMessg();
        // }
        private void Frm_Main_C_Load(object sender, EventArgs e)
        {
            Frm_Loading();
           // FrmMain();
        }
        private void Show_A()
        {
            if (Thread_Brush_A != null) Thread_Brush_A.Abort();
            Thread_Brush_A = new Thread(new ThreadStart(Show_A_Scan));
            Thread_Brush_A.Name = "Thread_Brush_A";
            Thread_Brush_A.IsBackground = true;
            Thread_Brush_A.Start();
        }

        private void Show_A_Scan()
        {
            while (SysInfo.m_SysBuff_C.m_Ctrl.m_iRun != 10)
            {
                if (m_bl_A1_C0)
                {
                    #region 画D图
                    if (this.InvokeRequired == true)
                    {
                        try
                        {
                            Delg_C_Wave ms = new Delg_C_Wave(Plant_A_Wave);
                            this.Invoke(ms, new object[] { });
                        }
                        catch
                        { }
                    }
                    else
                    {
                        Plant_A_Wave();
                    }
                    #endregion
                }
                SysInfo.WaitTime_Main(5);
            }
        }
        private void Show_C(int iType=0)
        {

            if (Thread_Brush_C != null) Thread_Brush_C.Abort();
            if (iType == 0)
            {
                Thread_Brush_C = new Thread(new ThreadStart(Thread_Plant_C));
                Thread_Brush_C.Priority = ThreadPriority.Highest;
                Thread_Brush_C.Name = "Thread_Brush_C";
                Thread_Brush_C.IsBackground = true;
                Thread_Brush_C.Start();
            }
        }
        private void Coat_Bt(bool blVal)
        {
            //Pan_Cmd.Visible = blVal;
            //Ck_E.Visible = blVal;
            //Bt_Win_Mw.BackColor = blVal ? Color.DimGray : Color.White;
            //Ck_His_Wave.BackColor = blVal ? Color.DimGray : Color.White;
        }
        private void SetBtnStyle(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;//样式
            btn.ForeColor = Color.Transparent;//前景
            btn.BackColor = Color.Transparent;//去背景
            btn.FlatAppearance.BorderSize = 0;//去边线
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;//鼠标经过
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;//鼠标按下
           
        }
        private void SetTran(PictureBox pic_Parents, PictureBox pic_Son)
        {
            pic_Parents.SendToBack();
            pic_Son.BackColor = Color.Transparent;
            pic_Son.Parent = pic_Parents;
            pic_Son.BringToFront();
        }
        private void SetTran(PictureBox pic_Parents, Button pic_Son)
        {
            pic_Parents.SendToBack();
            pic_Son.BackColor = Color.Transparent;
            pic_Son.Parent = pic_Parents;
            pic_Son.BringToFront();
        }
        /// <summary>
        /// 连接距离客户端（视觉识别需要距离
        /// </summary>
        private void LinkServer_Distanc()
        {
            SysInfo.m_Server_MulDistan.Start();
            SysInfo.csInter.WaitTime(0.5);
        }
        #region 电源管理

        #endregion 电源管理


        /// <summary>
        /// 远程寻迹服务器重连
        /// </summary>
        private void ReLink_XunJi()
        {
            SysInfo.m_Dog = new ClDog();
            //      SysInfo.m_Client.Close();
            SysInfo.m_Client.m_blOut = false;
            if (SysInfo.m_blXunJi_Prog_0PC_1YY) ConnectToServer();
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
                SysInfo.WaitTime(1);
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
        private void Thread_Plant_A()
        {
            while (SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun!=10)
            {
                // if (m_blLink)
                {
                    if (this.InvokeRequired == true)
                    {
                        try
                        {
                            Delg_PlantWave ms = new Delg_PlantWave(Plant_A_Wave);
                            this.Invoke(ms, new object[] { });
                        }
                        catch
                        { }
                    }
                    else
                    {
                        Plant_A_Wave();
                    }
                }
            }
        }

        int m_iPow_Show_No = 0;
        float fl_D = 0;

        /// <summary>
        /// 画D扫描波形图
        /// </summary>
        delegate void Delg_Plant_C_Wave();
        private void Plant_C()
        {
         //   while (SysInfo  .m_SysBuff_C.m_Ctrl.m_iRun != 10)
            {
                #region 画D图
                if (this.InvokeRequired == true)
                {
                    try
                    {
                        Delg_Plant_C_Wave ms = new Delg_Plant_C_Wave(Plant_A_Wave);
                        this.Invoke(ms, new object[] { });
                    }
                    catch(Exception ddd)
                    { }
                }
                else
                {
                    Plant_A_Wave();
                }
                #endregion
        //       SysInfo.WaitTime(5);
            }
        }

        private void Plant_A_Wave()
        {
            m_bl_Pic_A_B_Plant_1 = true;
            try
            {
                #region 2 平板电池容量
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2!=2 && m_iPow_Show_No++ == 50 && SysInfo.m_PcPower!=null )
                {
                    if (SysInfo.m_PcPower.m_blNetLink)
                        SysInfo.m_PcPower.SendData();
                    m_iPow_Show_No = 0;
                    fl_D = float.Parse(SysInfo.m_PcPower.m_strPowerPC == "" ? "0" : SysInfo.m_PcPower.m_strPowerPC);//     m_PcPower.m_strRetDat);
                    Bt_Dc.Text =  fl_D.ToString () + "%";
                    if (fl_D > 0 && fl_D <= 100)
                        Prg_Bar_PC.Value = (int)fl_D;
                }
                #endregion 2


                //switch (SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type)
                //{
                //    case 0:
                //        try
                //        {
                //            int _i_Cann_A = m_i_i_ShowThick_StartNo;
                //            int _i_Cann_B = _i_Cann_A+1;
                //     //       SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iWaveType = 1;
                //            if (SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iWaveType == 0)
                //            {
                //                if (_i_Cann_A > -1 && _i_Cann_A < SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum)
                //                    if (SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_Arr_RemoteIpPort[_i_Cann_A].blUse)
                //                        Draw_Wave_Test_Radio_Frequency(Pic_A, _i_Cann_A);
                //                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 &&
                //                    _i_Cann_B > -1 && _i_Cann_B < SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum)
                //                    if (SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_Arr_RemoteIpPort[_i_Cann_B].blUse)
                //                        Draw_Wave_Test_Radio_Frequency(Pic_B, _i_Cann_B);
                //            }
                //            else
                //            {
                //                if (_i_Cann_A > -1 && _i_Cann_A < SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum)
                //              //      if (SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_Arr_RemoteIpPort[_i_Cann_A].blUse)
                //                        Draw_Wave_Test(Pic_A, _i_Cann_A);//(Rad_1.Checked ? 0 : 1));
                //                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 &&
                //                    _i_Cann_B > -1 && _i_Cann_B < SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum)
                //                {
                //                    if(SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_Arr_RemoteIpPort[_i_Cann_B].blUse)
                //                         Draw_Wave_Test(Pic_B, _i_Cann_B);//
                //                }
                //            }
                //        }
                //        catch { }
                     
                //        break;
                //    case 1:

                //        break;
                //    case 2:
                //        Plant_A_ECT();
                //        break;
                //}
            }
            catch { }  
            m_bl_Pic_A_B_Plant_1 = false;
        }

        delegate void Delg_Plant_Coat();
        private void Plant_Coat()
        {
            #region 画涂层厚度值
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_Plant_Coat ms = new Delg_Plant_Coat(Plant_CurrData_Coat);
                    this.Invoke(ms, new object[] { });
                }
                catch (Exception ddd)
                { }
            }
            else
            {
                Plant_CurrData_Coat();
            }
            #endregion
        }
        /// <summary>
        /// 显示当前涂层数据
        /// </summary>
        private void Plant_CurrData_Coat()
        {
            //  Bt_Prepare.Visible = SysInfo.m_Climb4.m_SysBuf.iUpDownState == 18;|| Bt_Prepare.Visible

            //if (SysInfo.m_Tofd_C_Scan.m_Coat_DLL!=null&& SysInfo.m_Tofd_C_Scan.m_Coat_DLL.m_i_Mode_Cellect == 3 && SysInfo.m_Climb4.m_SysBuf.iUpDownState == 17)
            //{
            //    Bt_BeginCollec.Visible = true;
            //    Time_Collect.Enabled = true;
            //    SysInfo.m_Tofd_C_Scan.m_Coat_DLL.SendAllChannel(2, 4);

            //    SysInfo.m_Climb4.m_SysBuf.iUpDownState = 1;
            //}
            //m_bl_Pic_Coat_Plant = true;
            //Draw_Coat(Pic_B);
            //if(SysInfo.m_Tofd_C_Scan.m_Coat_DLL != null && SysInfo.m_Tofd_C_Scan.m_Coat_DLL.m_iRomoteNum>1)
            //    Draw_Coat(Pic_B_2,1);
            //if (SysInfo.m_Tofd_C_Scan.m_Coat_DLL != null && SysInfo.m_Tofd_C_Scan.m_Coat_DLL.m_iRomoteNum > 2)
            //    Draw_Coat(Pic_B_3,2);
            //m_bl_Pic_Coat_Plant = false;
      //      Bt_BeginCollec.Visible = false;
        }

        public bool Draw_Coat(System.Windows.Forms.PictureBox PicArea,int i_Equip_No= 0)
        {
          
            return false ;
        }
        int[] m_iArrWave = new int[800];
        byte[] m_btArrWave = new byte[800];
        /// <summary>
        /// 刻度颜色变化
        /// </summary>
        bool m_bl_Ruler_Color = false;
        /// <summary>
        /// 画A扫描
        /// </summary>
        private static object m_blPlantA_Lock = new object();
        /// <summary>
        /// 检波波形显示
        /// </summary>
        /// <param name="PicArea">图形控件</param>
        /// <param name="iType">通道号0-N</param>
        /// <param name="iShow_TitlPort"></param>
        /// <returns></returns>
        public bool Draw_Wave_Test(System.Windows.Forms.PictureBox PicArea, int iType = 0,
                          int iShow_TitlPort = 1)
        {
            return false;
        }
     
        /// <summary>
        /// 字体大小
        /// </summary>
        int m_fldrawFont_Alarm = 60;
        /// <summary>
        /// 是否标定数据
        /// </summary>
        bool bl_BtSignal = false;
        /// <summary>
        /// 画涡流A扫描图
        /// </summary>
        /// <param name="G"></param>
        /// <param name="i_StandNum">标定线数据长度</param>
        /// <param name="iArrStandWave">标定数组</param>
        /// <param name="iRomoteNum">探头个数</param>
        /// <param name="m_ArrECT">探头实时波形数据</param>
        public void Plant_Wave_ECT_A_Test(System.Windows.Forms.PictureBox PicArea, int i_StandNum,
                         float[] iArrStandWave, int iRomoteNum, CLECT_Data[] m_ArrECT, int iStatNo = 0, int iWait = 0)
        {
           
        }
        /// <summary>
        /// 射频波形
        /// </summary>
        /// <param name="iType">0:老的USB通讯  1 网络通讯模块个数</param>
        /// <param name="PicArea">老波形 ，</param>
        /// <param name="Gate"></param>
        /// <param name="iShow_TitlPort"></param>
        /// <returns></returns>
        public bool Draw_Wave_Test_Radio_Frequency( System.Windows.Forms.PictureBox PicArea,int iType, int m_Per_dB=0)
        {
            
            return false ;
        }
        private void ShowMessg()
        {
           
        }
        /// <summary>
        /// 画D扫描波形图  
        /// </summary>
        delegate void Delg_C_Wave();
        private void Thread_Plant_C()
        {
            while (SysInfo  .m_SysBuff_C.m_Ctrl.m_iRun != 10)
            {
                //if (m_bl_A1_C0 )
                {
                    #region 画D图
                    if (this.InvokeRequired == true)
                    {
                        try
                        {
                            Delg_C_Wave ms = new Delg_C_Wave(PlantWave_C);
                            this.Invoke(ms, new object[] { });
                        }
                        catch
                        { }
                    }
                    else
                    {
                        PlantWave_C();
                    }
                    #endregion
                }
                SysInfo.WaitTime_Main(350);
            }
        }

        private int Distan_ScreenCol(int _Trip_Com_mm)
        {
          float   _flDat = ((_Trip_Com_mm - SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_Start_Distance) / SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X);
            int _iCurr_X = (int)_flDat;//
            if (_flDat - _iCurr_X >= 0.5) _iCurr_X++;
            return  _iCurr_X;
        }

        float  m_fl_Real_X = 0;
        bool m_bl_Bruch_Mul = false;
        /// <summary>
        /// 开始序号
        /// </summary>
        int m_i_i_ShowThick_StartNo = 0;
        DateTime m_C_dtStar = DateTime.Now;

        private void Send_mm()
        {
            m_bl_Send_mm = false;
            SysInfo.m_Server_MulDistan.SendData(SysInfo.m_Climb4.m_SysBuf.Trip_Com_mm.ToString(), "1");
            m_bl_Send_mm = true;
        }
        /// <summary>
        /// A扫描1  C界面0
        /// </summary>
        bool m_bl_A1_C0 = false;
        /// <summary>
        /// 画图
        /// </summary>
        bool m_bl_A_Inv1_Other0 = false;
        /// <summary>
        /// 刷新C/B扫描  单调
        /// </summary>
        private void PlantWave_C()
        {
            #region 2 平板电池容量
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 2 && m_iPow_Show_No++ == 50 && SysInfo.m_PcPower != null)
            {
                if (SysInfo.m_PcPower.m_blNetLink)
                    SysInfo.m_PcPower.SendData();
                m_iPow_Show_No = 0;
                fl_D = float.Parse(SysInfo.m_PcPower.m_strPowerPC == "" ? "0" : SysInfo.m_PcPower.m_strPowerPC);//     m_PcPower.m_strRetDat);
                Bt_Dc.Text = fl_D.ToString() + "%";
                if (fl_D > 0 && fl_D <= 100)
                    Prg_Bar_PC.Value = (int)fl_D;
            }
            #endregion 2
            Txt_Date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            Txt_Time.Text = DateTime.Now.ToString("HH:mm:ss");
            Txt_X_JL.Text = SysInfo.m_Climb4.m_SysBuf.Trip.ToString("f3");Txt_Y_JL.Text = SysInfo.m_Climb4.m_SysBuf.i_Para_Wz.ToString();

            #region 寻迹心跳判断
            if (SysInfo.m_Client.tcpClient != null)
            {
                if (SysInfo.m_Client.tcpClient.Connected)
                {
                    SysInfo.m_Dog.iAddNum++;
                    if (SysInfo.m_Dog.iAddNum >= 2147483647) SysInfo.m_Dog.iAddNum = 0;
                    if (SysInfo.m_Dog.iAddNum_Old > 0 && SysInfo.m_Dog.iAddNum_Old - SysInfo.m_Dog.iAddNum > 4)
                    {
                        if (SysInfo.m_Dog.ilAlarm == -1)
                        {
                            SysInfo.m_Dog.ilAlarm = 1;
                            SysInfo.m_Dog.dtStar = DateTime.Now;
                        }
                        else if (SysInfo.m_Dog.ilAlarm == 1)
                        {
                            //2 异常时长超过3秒
                            if (DateTime.Now.Subtract(SysInfo.m_Dog.dtStar).TotalSeconds > SysInfo.m_Dog.iWaitLen)
                            {
                                ReLink_XunJi();
                            }
                        }
                    }
                }
            }
            #endregion 寻迹心跳判断
        }
        /// <summary>
        /// 涂层测厚刷新厚度Text的值
        /// </summary>
        /// <param name="iX"></param>
        /// <param name="iY"></param>
        /// <param name="flThick"></param>
        /// <param name="blAlarm"></param>
        private void Txt_Coat_Brush(int iX, int iY, Cls_EMAT_2 _Data )// float flThick, bool blAlarm)
        {
            string _strTime = DateTime.Now.ToString("HH:mm:ss:fff");
           
           

           
        }
        private void Txt_Coat_Brush_N(int iX, int iY, float flThick, bool blAlarm,bool _blShow,bool blDischarge,bool blDischarge_To_Mark,bool bl_Last=true )
        {
          
        }
        /// <summary>
        /// 点测显示涂层C扫数据
        /// </summary>
        public void Show_Coat_C(bool blHaveData)
        {
            
        }

        private int m_i_Coat_Thick_X = -1;

        private int m_i_Coat_Thick_Y = -1;

        /// <summary>
        /// 最后放电位置
        /// </summary>
        private int m_i_Coat_Discharge_X = -1;
        private int m_i_Coat_Discharge_Y = -1;
        /// <summary>
        /// 最后打标位置
        /// </summary>
        private int m_i_Coat_Mark_X = -1;
        private int m_i_Coat_Mark_Y = -1;
        private void Show_Coat_DisMark(int iAdd = 0)
        {
            //if (SysInfo.m_Climb4.m_SysBuf.bl_Discharge_Txt && SysInfo.m_Climb4.m_SysBuf.bl_Discharge_To_Mark_Txt)
            //{
            //    if (m_i_Coat_Discharge_X ==-1 ||  SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_X- m_i_Coat_Discharge_X > SysInfo. m_i_Add_MarkDisc ||
            //        m_i_Coat_Discharge_Y==-1 ||   SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_Y- m_i_Coat_Discharge_Y  > SysInfo.m_i_Add_MarkDisc ||
            //        m_i_Coat_Mark_X ==-1 ||  SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_X - m_i_Coat_Mark_X > SysInfo.m_i_Add_MarkDisc ||
            //        m_i_Coat_Mark_Y ==-1 || SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_Y- m_i_Coat_Mark_Y> SysInfo.m_i_Add_MarkDisc)
            //    {
            //        Txt_RunMsg.Text += SysInfo.m_Climb4.m_SysBuf.str_Discharge_Txt_Time + "  X=" + SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_X.ToString() +
            //                                                                              "  Y=" + SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_Y.ToString() + " 放电" + "\r\n";
            //        Txt_RunMsg.Text += SysInfo.m_Climb4.m_SysBuf.str_Discharge_To_Mark_Txt_Time + "  X=" + SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_X.ToString()+
            //                                                                                      "  Y=" + SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_Y.ToString() + " 打标" + "\r\n";

            //        Txt_RunMsg.SelectionStart = Txt_RunMsg.Text.Length;
            //        Txt_RunMsg.ScrollToCaret();

            //        SysInfo.m_Climb4.m_SysBuf.bl_Discharge = false;
            //        SysInfo.m_Climb4.m_SysBuf.bl_Discharge_To_Mark = false;
            //        SysInfo.m_Climb4.m_SysBuf.bl_Discharge_Txt = false;
            //        SysInfo.m_Climb4.m_SysBuf.bl_Discharge_To_Mark_Txt = false;
            //        if (m_i_Coat_Discharge_X != SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_X)
            //            m_i_Coat_Discharge_X = SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_X;
            //        if (m_i_Coat_Discharge_Y != SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_Y)
            //            m_i_Coat_Discharge_Y = SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_Y;
            //        if (m_i_Coat_Mark_X != SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_X)
            //            m_i_Coat_Mark_X = SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_X;
            //        if (m_i_Coat_Mark_Y != SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_Y)
            //            m_i_Coat_Mark_Y = SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_Y;

            //        if (iAdd == 1)
            //        {  //添加数据
            //            Cls_Coat_Dis_Mark _Curr_1 = new Cls_Coat_Dis_Mark();
            //            _Curr_1.i_CurrRow_No = SysInfo.m_C_Item_Info.i_C_AllRows;
            //            _Curr_1.strTime = SysInfo.m_Climb4.m_SysBuf.str_Discharge_Txt_Time;
            //            _Curr_1.iType = 1;
            //            _Curr_1.i_X = SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_X;
            //            _Curr_1.i_Y = SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_Y;
            //            SysInfo.m_SysBuff_C.m_C_One_Buff_Coat_ThickDisMark.Add(_Curr_1);

            //            Cls_Coat_Dis_Mark _Curr_2 = new Cls_Coat_Dis_Mark();
            //            _Curr_2.i_CurrRow_No = SysInfo.m_C_Item_Info.i_C_AllRows;
            //            _Curr_2.strTime = SysInfo.m_Climb4.m_SysBuf.str_Discharge_To_Mark_Txt_Time;
            //            _Curr_2.iType = 2;
            //            _Curr_2.i_X = SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_X;
            //            _Curr_2.i_Y = SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_Y;
            //            SysInfo.m_SysBuff_C.m_C_One_Buff_Coat_ThickDisMark.Add(_Curr_2);
            //        }
            //    }
            //}
            //else if (SysInfo.m_Climb4.m_SysBuf.bl_Discharge_Txt)
            //{
            //    if (m_i_Coat_Discharge_X == -1 || SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_X - m_i_Coat_Discharge_X > SysInfo.m_i_Add_MarkDisc ||
            //        m_i_Coat_Discharge_Y == -1 || SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_Y - m_i_Coat_Discharge_Y > SysInfo.m_i_Add_MarkDisc )
            //    {
            //        if (m_i_Coat_Discharge_X != SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_X)
            //            m_i_Coat_Discharge_X = SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_X;
            //        if (m_i_Coat_Discharge_Y != SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_Y)
            //            m_i_Coat_Discharge_Y = SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_Y;
            //        SysInfo.m_Climb4.m_SysBuf.bl_Discharge = false;
            //        SysInfo.m_Climb4.m_SysBuf.bl_Discharge_Txt = false;
            //        Txt_RunMsg.Text += SysInfo.m_Climb4.m_SysBuf.str_Discharge_Txt_Time +
            //                     "  X=" + SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_X.ToString() +
            //                     "  Y=" + SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_Y.ToString() + " 放电" + "\r\n";
            //        Txt_RunMsg.SelectionStart = Txt_RunMsg.Text.Length;
            //        Txt_RunMsg.ScrollToCaret();
            //        //添加数据
            //        if (iAdd == 1)
            //        {
            //            Cls_Coat_Dis_Mark _Curr_1 = new Cls_Coat_Dis_Mark();
            //            _Curr_1.i_CurrRow_No = SysInfo.m_C_Item_Info.i_C_AllRows;
            //            _Curr_1.strTime = SysInfo.m_Climb4.m_SysBuf.str_Discharge_Txt_Time;
            //            _Curr_1.iType = 1;
            //            _Curr_1.i_X = SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_X;
            //            _Curr_1.i_Y  = SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_Y;
            //            SysInfo.m_SysBuff_C.m_C_One_Buff_Coat_ThickDisMark.Add(_Curr_1);
            //        }
            //    }
            //}
            //else if (SysInfo.m_Climb4.m_SysBuf.bl_Discharge_To_Mark_Txt)
            //{
            //    if (m_i_Coat_Mark_X == -1 || SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_X - m_i_Coat_Mark_X > SysInfo.m_i_Add_MarkDisc ||
            //        m_i_Coat_Mark_Y == -1 || SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_Y - m_i_Coat_Mark_Y > SysInfo.m_i_Add_MarkDisc)
            //    {
            //        if (m_i_Coat_Mark_X != SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_X)
            //            m_i_Coat_Mark_X = SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_X;
            //        if (m_i_Coat_Mark_Y != SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_Y)
            //            m_i_Coat_Mark_Y = SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_Y;
            //        SysInfo.m_Climb4.m_SysBuf.bl_Discharge_To_Mark = false;
            //        SysInfo.m_Climb4.m_SysBuf.bl_Discharge_To_Mark_Txt = false;
            //        Txt_RunMsg.Text += SysInfo.m_Climb4.m_SysBuf.str_Discharge_To_Mark_Txt_Time +
            //                   "  X=" + SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_X.ToString()+
            //                    "  Y=" + SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_Y.ToString() + " 打标" + "\r\n";
            //        Txt_RunMsg.SelectionStart = Txt_RunMsg.Text.Length;
            //        Txt_RunMsg.ScrollToCaret();
            //        //添加数据
            //        if (iAdd == 1)
            //        {
            //            Cls_Coat_Dis_Mark _Curr_2 = new Cls_Coat_Dis_Mark();
            //            _Curr_2.i_CurrRow_No = SysInfo.m_C_Item_Info.i_C_AllRows;
            //            _Curr_2.strTime = SysInfo.m_Climb4.m_SysBuf.str_Discharge_To_Mark_Txt_Time;
            //            _Curr_2.iType = 2;
            //            _Curr_2.i_X = SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_X;
            //            _Curr_2.i_Y = SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_Y;
            //            SysInfo.m_SysBuff_C.m_C_One_Buff_Coat_ThickDisMark.Add(_Curr_2);
            //        }
            //    }
            //}
        }
        private void Show_Coat_A()
        {
           
        }
        /// <summary>
        /// dll调用画C扫描图   
        /// </summary>
        public void Show_DLL_C()
        {
            
        }
        Button[] m_ArrButt;
        bool[] m_ArrButt_Use;

        Button[] m_ArrButt_Mark;
        bool[] m_ArrButt_Use_Mark;
        private void DischargeImage_Set(int iScreenPort, bool blDischarge)
        {
            //1 消除
            //if ( blDischarge == false &&m_ArrButt_Use[iScreenPort])
            //    Pic_Spark.Controls.Remove(m_ArrButt[iScreenPort]);
            ////2 位置加载显示
            //if (blDischarge)
            //    Pic_Spark.Controls.Add(m_ArrButt[iScreenPort]);
            //3 状态
            if (iScreenPort < SysInfo.m_SysBuff_C.m_Plant_C.Scree_iAllCols_C)
            {
                m_ArrButt_Use[iScreenPort] = blDischarge;
                m_ArrButt[iScreenPort].Visible = blDischarge;
            }
            else
            { }
            
        }
        private void DischargeImage_Set_Mark(int iScreenPort, bool blDischarge)
        {
            //1 消除
            //if ( blDischarge == false &&m_ArrButt_Use[iScreenPort])
            //    Pic_Spark.Controls.Remove(m_ArrButt[iScreenPort]);
            ////2 位置加载显示
            //if (blDischarge)
            //    Pic_Spark.Controls.Add(m_ArrButt[iScreenPort]);
            //3 状态
            if (iScreenPort < SysInfo.m_SysBuff_C.m_Plant_C.Scree_iAllCols_C)
            {
                m_ArrButt_Use_Mark[iScreenPort] = blDischarge;
                m_ArrButt_Mark[iScreenPort].Visible = blDischarge;
            }
            else
            { }

        }
        //----
        delegate void Delg_DischargeImage(int iType);
        private void Plant_DischargeImage(int iType)
        {
            #region 画漏电
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_DischargeImage ms = new Delg_DischargeImage(DischargeImage);
                    this.Invoke(ms, new object[] { iType });
                }
                catch (Exception ddd)
                { }
            }
            else
            {
                DischargeImage(iType );
            }
            #endregion
        }
        private void DischargeImage(int iType)
        {
              

        }
        private void DischargeImage_Clear(int iDoEvents = 0)
        {
            int i_X = 0;
            String _strT = "";
            //1 清理当前界面
            if (m_ArrButt != null)
            {
                for (int i = 0; i < m_ArrButt.Count(); i++)
                {
                    m_ArrButt[i].Visible = false;
                    //       if (m_ArrButt_Use[i])
              //      Pic_Spark.Controls.Remove(m_ArrButt[i]);
                }
                if (iDoEvents == 0)
                    Application.DoEvents();
            }
            if (m_ArrButt_Mark != null)
            {
                for (int i = 0; i < m_ArrButt_Mark.Count(); i++)
                {
                    m_ArrButt_Mark[i].Visible = false;
                    //       if (m_ArrButt_Use[i])
               //     Pic_Spark.Controls.Remove(m_ArrButt_Mark[i]);
                }
                if (iDoEvents == 0)
                    Application.DoEvents();
            }
            //2 初始化当前界面放电按钮
            m_ArrButt = new Button[SysInfo.m_SysBuff_C.m_Plant_C.Scree_iAllCols_C];
            m_ArrButt_Use = new bool[SysInfo.m_SysBuff_C.m_Plant_C.Scree_iAllCols_C];
            m_ArrButt_Mark = new Button[SysInfo.m_SysBuff_C.m_Plant_C.Scree_iAllCols_C];
            m_ArrButt_Use_Mark = new bool[SysInfo.m_SysBuff_C.m_Plant_C.Scree_iAllCols_C];
         //SetBtnStyle(button7);
            //SetBtnStyle(button6);

         
        }
        private void Bt_Discharge_MouseDown(object sender, MouseEventArgs e)
        {

        }
        private void ModyColor()
        {
            Val_Chg(1);

          }
        /// <summary>
        /// 画图和参数配置数据交换
        /// </summary>
        /// <param name="iType"></param>
        /// <param name="iW"></param>
        private void Val_Chg(int iType = 0)
        {
            if (iType == 0)
            {
                SysInfo.m_SysBuff.m_Climb.flstrThickAlarm = SysInfo  .m_SysBuff_C.m_Plant_C.flNormal_Thickness;
                SysInfo.m_SysBuff.m_Climb.strWc_Bfz = SysInfo  .m_SysBuff_C.m_Plant_C.strWc_Bfz;

                SysInfo  .m_SysBuff_C.m_Plant_C.iGsb_Len = SysInfo.m_SysBuff.m_Climb.iGsb_Len;

                SysInfo.m_SysBuff.m_Climb.fl_Max_Limit = SysInfo  .m_SysBuff_C.m_Plant_C.fl_Max_Limit;
                SysInfo.m_SysBuff.m_Climb.i_Alarm = SysInfo  .m_SysBuff_C.m_Plant_C.i_Alarm;

                SysInfo.m_str_B_Stand_Color = SysInfo  .m_SysBuff_C.m_Plant_C.str_Wc_Start_End_Col;
                SysInfo.m_str_B_Stand_Color_A = SysInfo  .m_SysBuff_C.m_Plant_C.str_Wc_Start_End_Col_A;

            }
            else
            {
                SysInfo  .m_SysBuff_C.m_Plant_C.i_Alarm = SysInfo.m_SysBuff.m_Climb.i_Alarm;

                SysInfo  .m_SysBuff_C.m_Plant_C.fl_Max_Limit = SysInfo.m_SysBuff.m_Climb.fl_Max_Limit;
                SysInfo  .m_SysBuff_C.m_Plant_C.iGsb_Len = SysInfo.m_SysBuff.m_Climb.iGsb_Len;
                SysInfo  .m_SysBuff_C.m_Plant_C.flNormal_Thickness = SysInfo.m_SysBuff.m_Climb.flstrThickAlarm;
                SysInfo  .m_SysBuff_C.m_Plant_C.strWc_Bfz = SysInfo.m_SysBuff.m_Climb.strWc_Bfz;


                SysInfo  .m_SysBuff_C.m_Plant_C.str_Wc_Start_End_Col = SysInfo.m_str_B_Stand_Color;
                SysInfo  .m_SysBuff_C.m_Plant_C.str_Wc_Start_End_Col_A = "0/0/255|0/0/160|0/0/64|64/0/0";// SysInfo.m_str_B_Stand_Color_A;


                SysInfo  .m_SysBuff_C.m_Plant_C.Write_One("i_Alarm", SysInfo  .m_SysBuff_C.m_Plant_C.i_Alarm.ToString());
                SysInfo  .m_SysBuff_C.m_Plant_C.Write_One("iGsb_Len", SysInfo  .m_SysBuff_C.m_Plant_C.iGsb_Len.ToString());
                SysInfo  .m_SysBuff_C.m_Plant_C.Write_One("flNormal_Thickness", SysInfo  .m_SysBuff_C.m_Plant_C.flNormal_Thickness.ToString());
                SysInfo  .m_SysBuff_C.m_Plant_C.Write_One("strWc_Bfz", SysInfo  .m_SysBuff_C.m_Plant_C.strWc_Bfz);
                SysInfo  .m_SysBuff_C.m_Plant_C.Write_One("fl_Max_Limit", SysInfo  .m_SysBuff_C.m_Plant_C.fl_Max_Limit.ToString());

                SysInfo  .m_SysBuff_C.m_Plant_C.Write_One("str_Wc_Start_End_Col", SysInfo  .m_SysBuff_C.m_Plant_C.str_Wc_Start_End_Col);
                SysInfo  .m_SysBuff_C.m_Plant_C.Write_One("str_Wc_Start_End_Col_A", SysInfo  .m_SysBuff_C.m_Plant_C.str_Wc_Start_End_Col_A);

            }
        }
        private void Para_Cg(int iType = 0)
        {
            #region 判断是否项目类型切换了
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != SysInfo.m_i_TOFD_0_Cscan_1_Mui_2_Old)
            {
                SysInfo.m_SysBuff_C.m_Ctrl.m_iRun = 10;
                int _iNewNo = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2;
                SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2_Old;
                if (SysInfo.m_bl_Save == false)
                {
                    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 && SysInfo.m_SysBuff_C.m_Lst_C_Buff.Count > 0 ||
                        SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 && SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat.Count > 0)
                    {
                        SysInfo.Get_C_FileName();
                        Save_Data_C();
                    }
                }     
                //1 关闭
            //    if (SysInfo.m_bl_Show_Video)
             //       m_frm_Video.Close();
                //2 界面调整
                int iRow = 0;
            //    Cls_C_Scan.m_P_i_Len = 2000;
                Pan_A.RowStyles[iRow++] = new RowStyle(SizeType.Percent, 11.19f);
                Pan_A.RowStyles[iRow++] = new RowStyle(SizeType.Percent, 41.4f);
                Pan_A.RowStyles[iRow++] = new RowStyle(SizeType.Percent, 46.99f);
                Pan_A.RowStyles[iRow++] = new RowStyle(SizeType.Percent, 0);
        
                SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 = _iNewNo;
                SysInfo.m_i_TOFD_0_Cscan_1_Mui_2_Old = _iNewNo;
                //3 切换
            }
            #endregion

            Val_Chg(iType);

       //   Show_E0_C1(Ck_E .Checked ?0:1);
            SysInfo  .m_SysBuff_C.m_Ctrl.i_C_UIDLL_iWaitTime=int.Parse ( SysInfo.csInter.IniReadDefine("ClassUltrasGate", "m_iTimeDelay", "0",
                                                            System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini"));
            SysInfo  .m_SysBuff_C.m_Plant_C.Alarm_Init();

            //2 判断是否重新绘图
       //     if (SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X != SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval)
            {
                SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X = SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval;



//
                SysInfo.m_str_B_Stand_Color = SysInfo  .m_SysBuff_C.m_Plant_C.str_Wc_Start_End_Col;
                SysInfo.m_str_B_Stand_Color_A = SysInfo  .m_SysBuff_C.m_Plant_C.str_Wc_Start_End_Col_A;

                SysInfo.m_SysBuff.Txt_Wc_Num =int.Parse ( SysInfo  .m_SysBuff_C.m_Plant_C.Txt_Wc_Num);
            }
            SysInfo.m_Climb4.m_SysBuf.m_i_TOFD_0_Cscan_1_Mui_2 = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2;
        }
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        /// <summary>
        /// 字符串转字节数组
        /// </summary>
        /// <param name="strHex"></param>
        /// <returns></returns>
        public byte[] StrToHex_M(string strHex)
        {
            //清空格
            strHex = strHex.Replace(" ", "");
            if ((strHex.Length % 2) != 0)
                strHex.Insert(0, "0");
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
        public void ParseDat()
        {
            //     byte[] _btArrGet = new byte[8];//实际报文数据
            bool _blVal = true;
            string strCmdRev = "", _strCmdRev_Para_Old = "";
            bool _blHave_Para = false;//是否找到真正报文
            
            string _strCmd_Type = "";
            string _strCmdRev_Para = "0000001143590000000000000000000E460000000000000000000011475900000000DC50";//可能包含帧头帧尾的接收报文转换字符串
            _strCmdRev_Para_Old = _strCmdRev_Para; ;
            byte[] _btArrGet;
            int _iT = 0;
            double _db_T = 0;
            string _strT = "";

            while (_strCmdRev_Para.Length > 15)
            {
                _blHave_Para = false;
                {
                    _btArrGet = StrToHex_M(_strCmdRev_Para);//输入报文
                    strCmdRev = byteToHexStr(_btArrGet);//解析报文
                    #region   1、动作指令
                    if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x52)
                    {
                        _blHave_Para = true;
                        _iT = 10;
                        _iT = Convert.ToInt32(strCmdRev.Substring(4, 2), 16);
                        _iT = Convert.ToInt32(strCmdRev.Substring(6, 2), 16);
                        _iT = Convert.ToInt32(strCmdRev.Substring(8, 2), 16);
                        _iT = Convert.ToInt32(strCmdRev.Substring(10, 2), 16);
                        _iT = Convert.ToInt32(strCmdRev.Substring(12, 2), 16);
                        _iT = Convert.ToInt32(strCmdRev.Substring(14, 2), 16);
                        _strCmd_Type = "打磨机回传运动状态:";
                        goto JumpPosition;
                        /*
                    // 2、打磨机回传运动状态 0x43	0x52	 0xXX	 0xXX	 0xXX	 0xXX	 0xXX	 0xXX
                    第三字节 01：自动模式启动 
                      02：自动模式关闭
             第四字节 01：磨头旋转
                      02：旋转停止
             第五字节 01：升降压下
                      02：升降抬起
             第六字节 01：前进
                      02：后退
                      03：停止
                      04：前进左转
                      05：前进右转
                      06：后退左转
                      07：后退右转
             第七字节 01: 横扫启动
                      02：横扫停止
                      03：左扫
                      04：右扫
                      05：满行程左扫（用于设定横扫区间值）
                      06：满行程右扫（用于设定横扫区间值）
             第八字节 01：左纠偏
                      02：右纠偏
                      03：停止纠偏
                    */
                    }
                    #endregion

                    #region 二、参数指令
                    if (_btArrGet[0] == 0x42 && _btArrGet[1] == 0x4C)
                    {
                        _iT = 21;
                        _iT = Convert.ToInt32(strCmdRev.Substring(8, 4), 16);
                        _iT = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);

                        _blHave_Para = true;
                        _strCmd_Type = "滑台参数:";
                   //     goto JumpPosition;
                        //  1、滑台参数0x42  0x4C     0x00    0x00    0x00    0x00    0x00    0x00
                        /*
                          前两字节是指令的针头，用于识别指令的类型；
                  第三字节空
                  第四字节：00：向打磨机写入参数
                            01：向打磨机读取参数
                  第五字节:左限位参数高位
                  第六字节；左限位参数低位  不同光栅臂读写数据范围不同，例如0-200
                  第七字节：右限位参数高位
                  第八字节：右限位参数低位
                         */
                    }
                    if (_btArrGet[0] == 0x4A && _btArrGet[1] == 0x56)
                    {
                        _iT = 22;
                       _db_T  = double.Parse(Convert.ToInt32(strCmdRev.Substring(8, 4), 16).ToString());
                        _iT = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);

                        _blHave_Para = true;
                        _strCmd_Type = "速度参数:";
                 //       goto JumpPosition;
                        // 2、速度参数 0x4A	0x56	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                        /*
                        前两字节是指令的针头，用于识别指令的类型；	 
                 第三字节空
                 第四字节：00：向打磨机写入参数
                           01：向打磨机读取参数
                 第五字节:车体速度参数高位
                 第六字节；车体速度参数低位 设定速度例如：0-300 实际速度也基本为此速度
                 第七字节：滑台参数高位
                 第八字节：滑台参数低位
                        */
                    }
                    if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x59)
                    {
                        /// m_blLink = true;
                        _iT = 23;
                        double _dbJuli = double.Parse(Convert.ToInt32(strCmdRev.Substring(8, 8), 16).ToString());
                        _iT = (int)_dbJuli;

                        _db_T  = _dbJuli / 1000f;//2023-1-11 add
                                                        //double  _flInd =double .Parse ( (Math.Abs(_dbJuli) / m_MainBuff.g_Climb.Interval).ToString ("f0"));

                        //m_MainBuff.g_Gate.blGsb_RunFx_L_to_R = (int)(_flInd) % 2 == 0;

                        //m_MainBuff.g_Gate.flDistance_X = (float)_dbJuli/1000f;

                        _blHave_Para = true;
                        _strCmd_Type = "车体总行程:" +_iT ;
             //           goto JumpPosition;
                        //3、车体总行程设定参数
                        /* 0x43	0x59	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                         前两字节是指令的针头，用于识别指令的类型；	 
                  第三字节空
                  第四字节：00：向打磨机写入参数 只是回传位置mm
                            01：向打磨机读取参数
                 第五六七八字节分别是总行程的高位至低位(因为单位是mm所以参数需要使用四个字节来表达一个参数)

                        */
                    }
                    if (_btArrGet[0] == 0x4F && _btArrGet[1] == 0x53)
                    {
                        _iT = 24;
                        _iT = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                        _strCmd_Type = "单步间距:" + _iT;

                        _blHave_Para = true;
               //         goto JumpPosition;
                        //4、车体单次行程参数
                        /*0x4F	0x53	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                         前两字节是指令的针头，用于识别指令的类型；	
                 第三字节空
                 第四字节：00：向打磨机写入参数 单步间距 mm
                           01：向打磨机读取参数
                 第五六字节空
                 第七字节：单步参数高位
                 第八字节：单步参数低位

                        */
                    }
                    if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x4F)
                    {
                        _iT = 25;
                        _iT = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
                        _strCmd_Type = "纠偏参数:" + _iT;

                        _blHave_Para = true;
               //         goto JumpPosition;
                        //5、纠偏参数  设置系数，暂时没使用
                        /*0x43	0x4F	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                        前两字节是指令的针头，用于识别指令的类型；	
                 第三字节空
                 第四字节：00：向打磨机写入参数
                           01：向打磨机读取参数
                 第五六字节空
                 第七字节：纠偏参数高位
                 第八字节：纠偏参数低位
                        */
                    }

                    //6、心跳指令
                    /*0x48	0x42	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                     * 
                        每1.5秒发送一次，用于车体检测通讯是否正常，发送模式为，其他指令成功传输后1.5内没有其他指令发送，则发送一条心跳指令，来维持车体正常运转。
                    */
                    if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x58)
                    {
                        _iT = 27;
                        _iT = Convert.ToInt32(strCmdRev.Substring(8, 8), 16);

                        //m_SysBuf.blGsb_RunFx_L_to_R = Convert.ToInt32(strCmdRev.Substring(4, 2), 16) == 1;// m_SysBuf.blGsb_RunFx_L_to_R = m_bl_L_R;//1:远离原点 2：向原点运动
                        //m_SysBuf.strTrip_mm += ((m_SysBuf.strTrip_mm == "" ? "" : ",") + m_SysBuf.i_Para_Wz.ToString());
                        //if (m_SysBuf.blGsb_RunFx_L_to_R_Old != m_SysBuf.blGsb_RunFx_L_to_R)//  m_iGsb_No++==300)  // m_SysBuf.i_Para_Wz== m_SysBuf.  i_Gsb_End_Pos)
                        //{

                        //    m_iGsb_No = 0;

                        //    if (m_SysBuf.strTrip_mm.Length > 20)
                        //        WriteError_Gsb_Log(strLogFileName_Gsb, false, (m_SysBuf.Trip_Com_mm - m_SysBuf.i_Gsb_Interval).ToString() + "--" + m_SysBuf.strTrip_mm);
                        //    m_SysBuf.strTrip_mm = "";
                        //}

                        //m_SysBuf.i_Para_Wz = m_iT;
                        _blHave_Para = true;

                        _strCmd_Type = "光栅臂位置:" + _iT.ToString();
               //         goto JumpPosition;

                        //7、滑台实时位置 （CY）
                        /*0x43	0x58	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                         前两字节是指令的针头，用于识别指令的类型；	 
                  第三字节空
                  第四字节：00：向打磨机写入参数
                            01：向打磨机读取参数
                  第五六七八字节分别是滑台数据的高位至低位

                        */
                    }
                    #endregion 二
                    #region 三 附加指令
                    if (_btArrGet[0] == 0x47 && _btArrGet[1] == 0x59)
                    {
                        _iT = 28;
                        _strT  = (Convert.ToInt16(strCmdRev.Substring(4, 4), 16) / 100F).ToString("f2");
                        _strT = (Convert.ToInt16(strCmdRev.Substring(8, 4), 16) / 100F).ToString("f2");
                        _strT = (Convert.ToInt16(strCmdRev.Substring(12, 4), 16) / 100F).ToString("f2");
                      //  _strCmd_Type = "陀螺仪数据:" + m_SysBuf.X + "," + m_SysBuf.Y + "," + m_SysBuf.Z;

                        _blHave_Para = true;
             //           goto JumpPosition;
                        /*1、陀螺仪数据 gyroscopic
                        字节	1	2	3	4	5	6	7	8
                         指令示例：	0x47	0x59	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                                    偏航角（z轴）*100	俯仰角（y轴）*100	滚转角（x轴）*100
                        前两字节是指令的针头，用于识别指令的类型；	 
                              第三四字节：偏航角（z轴）*100的高位至低位
                              第三四字节：俯仰角（y轴）*100的高位至低位
                        第三四字节：滚转角（x轴）*100的高位至低位
                        */
                    }
                    if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x49)
                    {
                        //   m_SysBuf.iDatIndex = 29;


                        if (Convert.ToInt16(strCmdRev.Substring(4, 2), 16) == 1)//拍照
                            _iT = 1;// g_Msg_InterFace.Fun_Photo();
                        if (Convert.ToInt16(strCmdRev.Substring(6, 2), 16) == 1)//1开始录像
                            _iT = 1;// g_Msg_InterFace.Fun_Video(1);
                        if (Convert.ToInt16(strCmdRev.Substring(6, 2), 16) == 2)//2停止录像
                            _iT = 1;//g_Msg_InterFace.Fun_Video(2);
                        _strCmd_Type = "相机指令  " + "拍照：" + Convert.ToInt16(strCmdRev.Substring(4, 2), 16) + ", 录像：" + Convert.ToInt16(strCmdRev.Substring(6, 2), 16);

                        _blHave_Para = true;
              //          goto JumpPosition;
                        /*1、相机指令  Camera instruction
                        字节	1	2	3	4	5	6	7	8
                         指令示例：	0x43	0x49	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
                        第三字节  01：拍照 

                             第四字节 01：开始录像
                                      02：停止录像
                        */
                    }
                    if (_btArrGet[0] == 0x41 && _btArrGet[1] == 0x4E)
                    {
                       // m_SysBuf.iDatIndex = 30;

                        _db_T = Convert.ToInt16(strCmdRev.Substring(12, 4), 16) / 100f;
                        _strCmd_Type = "陀螺仪校正角度值：  " + _db_T ;
       //                 goto JumpPosition;
                        /*陀螺仪矫正 angle  PC iComType_1==1 case 1://陀螺仪矫正 angle
                        字节	1	2	3	4	5	6	7	8
                         指令示例：	0x41	0x4E	 0x00	 0x00	 0x00	 0x00	    0x00	 0x00
                        前两字节是指令的针头，用于识别指令的类型；	 
                              第三字节空
                              第四字节：00：向车体写入参数
                                        01：向车体读取参数
                              第七八字节分别是数据*100的高位至低位

                        */
                        _blHave_Para = true;
                    }
                    #endregion  附加指令

                    if (_blHave_Para)//有此报文，就拿本字符串的下一帧报文
                    {
                        if (_strCmdRev_Para.Length > 31)
                            _strCmdRev_Para = _strCmdRev_Para.Substring(16);
                        else
                            break;

                    }
                    else if (_strCmdRev_Para.Length > 16)
                        _strCmdRev_Para= _strCmdRev_Para.Substring(2);
                    else
                        break;

                    //if (m_SysBuf.blWinOpen)
                    //    g_Msg_InterFace.Fun_UI_ClimbUpData(strCmdRev);
                }


                //   Array.Copy(_btArrGet, 4, _btArrGet, 0, _btArrGet.Length);
            }
        JumpPosition:
            _iT = 0;
          //  WriteErrorLog(strLogFileName, (_strCmd_Type + " 报文：") + strCmdRev, false);


            //-----------------------
            //    if (_blVal)
            //    {
            //        m_blLink = true;
            //        strCmdRev = SerialClass.byteToHexStr(_btArrGet);
            //        #region   1、动作指令
            //        if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x52)
            //        {
            //            m_SysBuf.iDatIndex = 10;
            //            m_SysBuf.i_Auto_Run = Convert.ToInt32(strCmdRev.Substring(4, 2), 16);
            //            m_SysBuf.i_Xz = Convert.ToInt32(strCmdRev.Substring(6, 2), 16);
            //            m_SysBuf.i_Sj_Up = Convert.ToInt32(strCmdRev.Substring(8, 2), 16);
            //            m_SysBuf.i_Run = Convert.ToInt32(strCmdRev.Substring(10, 2), 16);
            //            m_SysBuf.i_Gsb_Hs = Convert.ToInt32(strCmdRev.Substring(12, 2), 16);
            //            m_SysBuf.i_Jp = Convert.ToInt32(strCmdRev.Substring(14, 2), 16);
            //            _strCmd_Type = "打磨机回传运动状态:";
            //            goto JumpPosition;
            //            /*
            //        // 2、打磨机回传运动状态 0x43	0x52	 0xXX	 0xXX	 0xXX	 0xXX	 0xXX	 0xXX
            //        第三字节 01：自动模式启动 
            //          02：自动模式关闭
            // 第四字节 01：磨头旋转
            //          02：旋转停止
            // 第五字节 01：升降压下
            //          02：升降抬起
            // 第六字节 01：前进
            //          02：后退
            //          03：停止
            //          04：前进左转
            //          05：前进右转
            //          06：后退左转
            //          07：后退右转
            // 第七字节 01: 横扫启动
            //          02：横扫停止
            //          03：左扫
            //          04：右扫
            //          05：满行程左扫（用于设定横扫区间值）
            //          06：满行程右扫（用于设定横扫区间值）
            // 第八字节 01：左纠偏
            //          02：右纠偏
            //          03：停止纠偏
            //        */
            //        }
            //        #endregion

            //        #region 二、参数指令
            //        if (_btArrGet[0] == 0x42 && _btArrGet[1] == 0x4C)
            //        {
            //            m_SysBuf.iDatIndex = 21;
            //            m_SysBuf.i_Para_Zxw = Convert.ToInt32(strCmdRev.Substring(8, 4), 16);
            //            m_SysBuf.i_Para_Yxw = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
            //            _strCmd_Type = "滑台参数:";
            //            goto JumpPosition;
            //            // return;
            //            //  1、滑台参数0x42  0x4C     0x00    0x00    0x00    0x00    0x00    0x00
            //            /*
            //              前两字节是指令的针头，用于识别指令的类型；
            //      第三字节空
            //      第四字节：00：向打磨机写入参数
            //                01：向打磨机读取参数
            //      第五字节:左限位参数高位
            //      第六字节；左限位参数低位  不同光栅臂读写数据范围不同，例如0-200
            //      第七字节：右限位参数高位
            //      第八字节：右限位参数低位
            //             */
            //        }
            //        if (_btArrGet[0] == 0x4A && _btArrGet[1] == 0x56)
            //        {
            //            m_SysBuf.iDatIndex = 22;
            //            m_SysBuf.Speed = double.Parse(Convert.ToInt32(strCmdRev.Substring(8, 4), 16).ToString());
            //            m_SysBuf.i_Para_Speed_Gsb = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
            //            _strCmd_Type = "速度参数:";
            //            goto JumpPosition;
            //            // return;
            //            // 2、速度参数 0x4A	0x56	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
            //            /*
            //            前两字节是指令的针头，用于识别指令的类型；	 
            //     第三字节空
            //     第四字节：00：向打磨机写入参数
            //               01：向打磨机读取参数
            //     第五字节:车体速度参数高位
            //     第六字节；车体速度参数低位 设定速度例如：0-300 实际速度也基本为此速度
            //     第七字节：滑台参数高位
            //     第八字节：滑台参数低位
            //            */
            //        }
            //        if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x59)
            //        {
            //            m_SysBuf.iDatIndex = 23;
            //            double _dbJuli = double.Parse(Convert.ToInt32(strCmdRev.Substring(8, 8), 16).ToString());
            //            m_SysBuf.Trip = _dbJuli;
            //            //double _flInd = double.Parse((Math.Abs(_dbJuli) / m_MainBuff.g_Climb.Interval).ToString("f0"));
            //            //m_MainBuff.g_Gate.flDistance_X = (float)_dbJuli / 1000f;
            //            _strCmd_Type = "车体总行程:" + m_SysBuf.Trip/1000f;
            //            goto JumpPosition;
            //            // return;
            //            //3、车体总行程设定参数
            //            /* 0x43	0x59	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
            //             前两字节是指令的针头，用于识别指令的类型；	 
            //      第三字节空
            //      第四字节：00：向打磨机写入参数 只是回传位置mm
            //                01：向打磨机读取参数
            //     第五六七八字节分别是总行程的高位至低位(因为单位是mm所以参数需要使用四个字节来表达一个参数)

            //            */
            //        }
            //        if (_btArrGet[0] == 0x4F && _btArrGet[1] == 0x53)
            //        {
            //            m_SysBuf.iDatIndex = 24;
            //            m_SysBuf.i_Para_Dbjg = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
            //            _strCmd_Type = "单步间距:" + m_SysBuf.i_Para_Dbjg;
            //            goto JumpPosition;
            //            return;
            //            //4、车体单次行程参数
            //            /*0x4F	0x53	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
            //             前两字节是指令的针头，用于识别指令的类型；	
            //     第三字节空
            //     第四字节：00：向打磨机写入参数 单步间距 mm
            //               01：向打磨机读取参数
            //     第五六字节空
            //     第七字节：单步参数高位
            //     第八字节：单步参数低位

            //            */
            //        }
            //        if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x4F)
            //        {
            //            m_SysBuf.iDatIndex = 25;
            //            m_SysBuf.i_Para_Jp = Convert.ToInt32(strCmdRev.Substring(12, 4), 16);
            //            _strCmd_Type = "纠偏参数:" + m_SysBuf.i_Para_Jp;
            //            goto JumpPosition;
            //            // return;
            //            //5、纠偏参数  设置系数，暂时没使用
            //            /*0x43	0x4F	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
            //            前两字节是指令的针头，用于识别指令的类型；	
            //     第三字节空
            //     第四字节：00：向打磨机写入参数
            //               01：向打磨机读取参数
            //     第五六字节空
            //     第七字节：纠偏参数高位
            //     第八字节：纠偏参数低位
            //            */
            //        }

            //        //6、心跳指令
            //        /*0x48	0x42	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
            //            每1.5秒发送一次，用于车体检测通讯是否正常，发送模式为，其他指令成功传输后1.5内没有其他指令发送，则发送一条心跳指令，来维持车体正常运转。
            //        */
            //        if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x58)
            //        {
            //            m_SysBuf.iDatIndex = 27;
            //            m_SysBuf.i_Para_Wz = Convert.ToInt32(strCmdRev.Substring(8, 8), 16);

            //          //  m_MainBuff.g_Gate.blGsb_RunFx_L_to_R = Convert.ToInt32(strCmdRev.Substring(4, 2), 16) == 1;//1:远离原点 2：向原点运动
            //            //  m_blCanCalcu = false;

            //            //当前光栅臂位置
            //            flDistance_Y = m_SysBuf.i_Para_Wz / 1000f;
            //         /*   flDistance_Y = Math.Abs(flDistance_Y);
            //            if (m_MainBuff.g_Climb.iRun_Gsb_Sc1_Zx0 == 1)
            //            {
            //                switch (m_MainBuff.g_Climb.m_iGetDist_Type)
            //                {
            //                    case 0:
            //                        #region 0
            //                        if (m_MainBuff.g_Climb.m_iRun == 1)
            //                        {
            //                            #region 判断运行方向  在开始运行头20mm内判断运行方向
            //                            //if (flDistance_Y < m_iJugeDist && m_MainBuff.g_Gate.flDistance_Y < m_iJugeDist)
            //                            //{
            //                            //    // m_blCanCalcu = true;
            //                            //    m_MainBuff.g_Gate.blGsb_RunFx_L_to_R = flDistance_Y > m_MainBuff.g_Gate.flDistance_Y;
            //                            //}
            //                            //else if (flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len - m_iJugeDist && m_MainBuff.g_Gate.flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len - m_iJugeDist)// m_MainBuff.g_Gate.m_UI_StartDistanc)
            //                            //{
            //                            //    // m_blCanCalcu = true;
            //                            //    m_MainBuff.g_Gate.blGsb_RunFx_L_to_R = !(flDistance_Y < m_MainBuff.g_Gate.flDistance_Y);
            //                            //}
            //                            #endregion  判断运行方向

            //                            if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R && flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min && m_MainBuff.g_Gate.flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min ||
            //                               m_MainBuff.g_Gate.blGsb_RunFx_L_to_R == false && flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max && m_MainBuff.g_Gate.flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max)
            //                            {
            //                                #region 超声延时数据处理 2021-03-22 1942
            //                                if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R)//光栅臂   从左往右
            //                                {
            //                                    //1 开始测量时延时接收数据
            //                                    m_MainBuff.g_Gate.m_UI_Start_Bool = flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min - m_MainBuff.g_Climb.m_UI_Gsb_Add;

            //                                    if (m_MainBuff.g_Gate.m_UI_Start_Bool)
            //                                    {
            //                                        //2 结束后，再接收20个数据补足剩余20mm距离
            //                                        if (m_MainBuff.g_Gate.m_UI_End_Bool == false && m_MainBuff.g_Gate.m_UI_EndBuff_Cs != 0) m_MainBuff.g_Gate.m_UI_EndBuff_Cs = 0;
            //                                        m_MainBuff.g_Gate.m_UI_End_Bool = flDistance_Y >= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max;

            //                                        //  if(m_MainBuff.g_Gate.m_UI_StartDistanc > m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min)
            //                                        flDistance_Y -= (m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min);// +0.04f;
            //                                        if (flDistance_Y < 0) return;
            //                                        //else
            //                                        //    flDistance_Y -= m_MainBuff.g_Gate.m_UI_StartDistanc;
            //                                    }
            //                                }
            //                                else //if (m_MainBuff.g_ScreenPlant.iCurrBuffRows % 2 == 1)//光栅臂   从右往左
            //                                {
            //                                    //1 开始测量时延时接收数据
            //                                    m_MainBuff.g_Gate.m_UI_Start_Bool = flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max + m_MainBuff.g_Climb.m_UI_Gsb_Add;
            //                                    if (m_MainBuff.g_Gate.m_UI_Start_Bool)
            //                                    {
            //                                        //2 结束后，再接收20个数据补足剩余20mm距离
            //                                        if (m_MainBuff.g_Gate.m_UI_End_Bool == false && m_MainBuff.g_Gate.m_UI_EndBuff_Cs != 0) m_MainBuff.g_Gate.m_UI_EndBuff_Cs = 0;
            //                                        m_MainBuff.g_Gate.m_UI_End_Bool = flDistance_Y <= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;

            //                                        //      flDistance_Y -= m_MainBuff.g_Gate.m_UI_StartDistanc;// + 0.04f;
            //                                        //   if (m_MainBuff.g_Gate.m_UI_StartDistanc > m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min)
            //                                        flDistance_Y += m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;// +0.04f;
            //                                        if (flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len / 1000f) return;
            //                                        //else
            //                                        //    flDistance_Y += m_MainBuff.g_Gate.m_UI_StartDistanc;
            //                                    }
            //                                }
            //                                #endregion 超声延时数据处理
            //                            }
            //                        }
            //                        #endregion  0
            //                        break;
            //                    case 1:
            //                        #region 1
            //                        #region 超声延时数据处理 2021-03-22 1942
            //                        if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R)//光栅臂   从左往右
            //                        {
            //                            //1 开始测量时延时接收数据
            //                            flDistance_Y -= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;// +0.04f;
            //                            if (flDistance_Y < m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min) return;
            //                        }
            //                        else //光栅臂   从右往左
            //                        {
            //                            //1 开始测量时延时接收数据
            //                            flDistance_Y += m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;// +0.04f;
            //                            if (flDistance_Y > m_MainBuff.g_Climb.m_UI_Gsb_Distan_Max) return;
            //                        }
            //                        #endregion 超声延时数据处理
            //                        #endregion 1
            //                        break;
            //                    case 2:
            //                        #region 1
            //                        #region 超声延时数据处理 2021-03-22 1942
            //                        if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R)//光栅臂   从左往右
            //                        {

            //                            //1 开始测量时延时接收数据
            //                            flDistance_Y -= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;
            //                            if (flDistance_Y < 0) return;
            //                        }
            //                        else //光栅臂   从右往左
            //                        {
            //                            //1 开始测量时延时接收数据
            //                            flDistance_Y += m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;
            //                            if (flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len / 1000f) return;
            //                        }
            //                        #endregion 超声延时数据处理
            //                        #endregion 1
            //                        break;
            //                }
            //            }
            //            flDistance_Y = float.Parse(flDistance_Y.ToString("f3"));
            //            m_MainBuff.g_Gate.flDistance_Y = flDistance_Y;
            //            */
            //            _strCmd_Type = "滑台实时位置:" + m_SysBuf.i_Para_Wz + " " + flDistance_Y;

            //            goto JumpPosition;
            //            //#region 适应电磁超声延时
            //            //flDistance_Y = m_SysBuf.i_Para_Wz / 1000f;
            //            //flDistance_Y = Math.Abs(flDistance_Y);

            //            //#region 1
            //            //#region 超声延时数据处理 2021-03-22 1942
            //            //if (m_MainBuff.g_Gate.blGsb_RunFx_L_to_R)//光栅臂   从左往右
            //            //{
            //            //    //1 开始测量时延时接收数据
            //            //    flDistance_Y -= m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;
            //            //    if (flDistance_Y < 0) return;
            //            //}
            //            //else //光栅臂   从右往左
            //            //{
            //            //    //1 开始测量时延时接收数据
            //            //    flDistance_Y += m_MainBuff.g_Climb.m_UI_Gsb_Distan_Min;
            //            //    if (flDistance_Y > m_MainBuff.g_ScreenPlant.iGsb_Len / 1000f) return;
            //            //}
            //            //flDistance_Y = float.Parse(flDistance_Y.ToString("f3"));
            //            //m_MainBuff.g_Gate.flDistance_Y = flDistance_Y;
            //            //_strCmd_Type = "滑台实时位置:" + m_SysBuf.i_Para_Wz + " " + m_MainBuff.g_Gate.flDistance_Y;
            //            //goto JumpPosition;

            //            ////    _strCmd_Type = "滑台实时位置:" + m_SysBuf.i_Para_Wz + " " + m_MainBuff.g_Gate.flDistance_Y;
            //            //#endregion 超声延时数据处理
            //            //#endregion 1
            //            //#endregion
            //            return;

            //            //7、滑台实时位置 （CY）
            //            /*0x43	0x58	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
            //             前两字节是指令的针头，用于识别指令的类型；	 
            //      第三字节空
            //      第四字节：00：向打磨机写入参数
            //                01：向打磨机读取参数
            //      第五六七八字节分别是滑台数据的高位至低位

            //            */
            //        }
            //        #endregion 二

            //        #region 三 附加指令
            //        if (_btArrGet[0] == 0x47 && _btArrGet[1] == 0x59)
            //        {
            //            m_SysBuf.iDatIndex = 28;
            //            m_SysBuf.Z = (Convert.ToInt16(strCmdRev.Substring(4, 4), 16) / 100F).ToString("f2");
            //            m_SysBuf.Y = (Convert.ToInt16(strCmdRev.Substring(8, 4), 16) / 100F).ToString("f2");
            //            m_SysBuf.X = (Convert.ToInt16(strCmdRev.Substring(12, 4), 16) / 100F).ToString("f2");
            //            _strCmd_Type = "陀螺仪数据:" + m_SysBuf.X + "," + m_SysBuf.Y + "," + m_SysBuf.Z;
            //            goto JumpPosition;
            //            /*1、陀螺仪数据 gyroscopic
            //            字节	1	2	3	4	5	6	7	8
            //             指令示例：	0x47	0x59	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
            //               偏航角（z轴）*100	俯仰角（y轴）*100	滚转角（x轴）*100
            //            前两字节是指令的针头，用于识别指令的类型；	 
            //               第三四字节：偏航角（z轴）*100的高位至低位
            //               第三四字节：俯仰角（y轴）*100的高位至低位
            //            第三四字节：滚转角（x轴）*100的高位至低位
            //            */
            //        }
            //        if (_btArrGet[0] == 0x43 && _btArrGet[1] == 0x49)
            //        {
            //            m_SysBuf.iDatIndex = 29;

            //            if (Convert.ToInt16(strCmdRev.Substring(4, 2), 16) == 1)//拍照
            //                g_Msg_InterFace.Fun_Photo();
            //            if (Convert.ToInt16(strCmdRev.Substring(6, 2), 16) == 1)//1开始录像
            //                g_Msg_InterFace.Fun_Video(1);
            //            if (Convert.ToInt16(strCmdRev.Substring(6, 2), 16) == 2)//2停止录像
            //                g_Msg_InterFace.Fun_Video(2);
            //            _strCmd_Type = "相机指令  " + "拍照：" + Convert.ToInt16(strCmdRev.Substring(4, 2), 16) + ", 录像：" + Convert.ToInt16(strCmdRev.Substring(6, 2), 16);
            //            goto JumpPosition;
            //            /*1、相机指令  Camera instruction
            //            字节	1	2	3	4	5	6	7	8
            //             指令示例：	0x43	0x49	 0x00	 0x00	 0x00	 0x00	 0x00	 0x00
            //            第三字节  01：拍照 

            //              第四字节 01：开始录像
            //                       02：停止录像
            //            */
            //        }
            //        if (_btArrGet[0] == 0x41 && _btArrGet[1] == 0x4E)
            //        {
            //            m_SysBuf.iDatIndex = 30;

            //            m_SysBuf.flAnle = Convert.ToInt16(strCmdRev.Substring(12, 4), 16) / 100f;
            //            _strCmd_Type = "陀螺仪校正角度值：  " + m_SysBuf.flAnle;
            //            goto JumpPosition;
            //            /*陀螺仪矫正 angle  PC iComType_1==1 case 1://陀螺仪矫正 angle
            //            字节	1	2	3	4	5	6	7	8
            //             指令示例：	0x41	0x4E	 0x00	 0x00	 0x00	 0x00	    0x00	 0x00
            //            前两字节是指令的针头，用于识别指令的类型；	 
            //               第三字节空
            //               第四字节：00：向车体写入参数
            //                         01：向车体读取参数
            //               第七八字节分别是数据*100的高位至低位

            //            */
            //        }
            //        #endregion  附加指令
            //        //if (m_SysBuf.blWinOpen)
            //        //    g_Msg_InterFace.Fun_UI_ClimbUpData(strCmdRev);
            //    }
            //JumpPosition:
            //    WriteErrorLog(strLogFileName, (_strCmd_Type + " 报文：") + strCmdRev, false);


        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            #region 1 读参数
            SysInfo.Init();

           // Plant_C_Init();//画图参数
            SysInfo  .m_SysBuff_C.m_Ctrl.Init();//超声读数据间隔时间
            m_i_Thick_Left= int.Parse(SysInfo.csInter.IniReadDefine("Plant_A", "m_i_Thick_Left", "0", SysInfo.HardFileName));
            #endregion 1

            #region 2 车体
            //  if (int.Parse(SysInfo.csInter.IniReadDefine("CAN", "Have", "1", SysInfo.HardFileName)) == 1)
            {
                SysInfo.m_Climb4 = new Clb_MT_Comm.MT_Comm(ref SysInfo.m_SysBuff.m_Climb, ref SysInfo.g_Msg_InterFace, SysInfo.m_i_Can_BrushTime);
                SysInfo.m_Climb4.m_blCom1_Can0 = int.Parse(SysInfo.csInter.IniReadDefine("COM_Can", "m_blCom1_Can0", "0", SysInfo.HardFileName));
                SysInfo.m_Climb4.strBtl   = SysInfo.csInter.IniReadDefine("COM_Can", "BTL", "115200,0,8,1", SysInfo.HardFileName);
                if (SysInfo.m_Climb4.m_blCom1_Can0 == 0)
                    SysInfo.m_Climb4.InitCan();
                else
                {
                    SysInfo.m_Climb4.strBtl = SysInfo.csInter.IniReadDefine("COM_Can", "COMBTL", "115200,0,8,1", SysInfo.HardFileName);
                    string strCom = SysInfo.csInter.IniReadDefine("COM_Can", "COM", "COM2", SysInfo.HardFileName);
                    
                    SysInfo.Link_Climb_Com(SysInfo.m_Climb4.m_blCom1_Can0, strCom);
                   
                }
            }
            if (SysInfo.m_Climb4.m_blLink == false)
            {
                SysInfo.m_Climb4.CloseSet();
                SysInfo.m_strLinkMsg += (SysInfo.m_strLinkMsg == "" ? "" : ", ") + (SysInfo.m_iLanguage == 0 ? " 车体":" Robot");
            }
            SysInfo.m_SysBuff.m_Climb.i_Begin_Down = int.Parse(SysInfo.csInter.IniReadDefine("TOFD", "i_Begin_Down", "1", SysInfo.HardFileName));
            #endregion 2车体

            // #region 3 数据缓存
            SysInfo  .m_SysBuff_C.m_Plant_C.m_iLanguage = SysInfo.m_iLanguage;
            //if (SysInfo.m_SysBuff_C.m_Ctrl.m_bl_FY)
            //    SysInfo.m_SysBuff_C.m_Plant_C.Get_Stand_ECT_Limit(P_FY);
            //else 
            //SysInfo  .m_SysBuff_C.m_Plant_C.InitLimitPic(SysInfo  .m_SysBuff_C.m_Plant_C.Txt_Wc_Num,
            //                                             P_1, P_Md, P_2, Grp_Stand_Color, 0.4f);
            //SysInfo  .m_SysBuff_C.m_Plant_C.InitLimitPic_A(SysInfo  .m_SysBuff_C.m_Plant_C.Txt_Wc_Num,
            //                                             P_1, P_Md, P_2, Grp_Stand_Color, 0.4f);

            SysInfo.m_SysBuff.Txt_Wc_Num = int.Parse(SysInfo  .m_SysBuff_C.m_Plant_C.Txt_Wc_Num);
            SysInfo.m_str_B_Stand_Color = SysInfo  .m_SysBuff_C.m_Plant_C.str_Wc_Start_End_Col;
            SysInfo.m_str_B_Stand_Color_A = SysInfo  .m_SysBuff_C.m_Plant_C.str_Wc_Start_End_Col_A;

            //SysInfo.m_Tofd_C_Scan = new Cls_C_Scan(ref SysInfo  .m_SysBuff_C, ref SysInfo.m_Climb4,
            //                                       ref SysInfo.m_SysBuff, SysInfo.g_Msg_InterFace,
            //                                           SysInfo.m_i_TOFD_0_Cscan_1_Mui_2);
            //超声模块初始化
           
            //switch (SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type)
            //{
            //    case 0:
            //        SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.Udp_Ini();
            //        Ck_By_My.Checked = SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_CalcuThick;
            //        Cmb_Algorithm.Enabled = !SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_CalcuThick;
            //        Ck_By_My.Checked = (SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_CalcuThick);
                  
            //        break;
            //    case 1:
            //        Bt_1_L.Visible = Bt_1_R.Visible = true;
            //        Bt_1_L.Left = SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_S_X;
            //        Bt_1_R.Left = SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_E_X;
            //        Bt_1_L.Top = SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_Y - Bt_1_L.Height / 2;
            //        Bt_1_R.Top = SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_Y - Bt_1_L.Height / 2;

            //        Bt_2_L.Left = SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_S_X;
            //        Bt_2_R.Left = SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_E_X;
            //        Bt_2_L.Top = SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_Y - Bt_1_L.Height / 2;
            //        Bt_2_R.Top = SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_Y - Bt_1_L.Height / 2;
            //        if (SysInfo.m_SysBuff.m_Tofd_DLL.m_Data_Type == 2)
            //        {
            //            Bt_2_L.Visible = true;
            //            Bt_2_R.Visible = true;
            //        }
            //        Rad_Gate_1.Checked = SysInfo.m_SysBuff.m_Tofd_DLL.m_Data_Type == 1;
            //        Rad_Gate_2.Checked = SysInfo.m_SysBuff.m_Tofd_DLL.m_Data_Type == 2;
            //        break;
            //    case 2:
            //        SysInfo.m_W_i_FilePath = Application.StartupPath + "\\Data_C_Scan_ECT";
            //        break;
            //}
            //SysInfo.m_W_i_FilePath = SysInfo.csInter.IniReadDefine("System", SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0 ? "m_W_i_FilePath"
            //                : (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 ? (SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type == 2 ? "m_W_i_FilePath_Cscan_ECT" : "m_W_i_FilePath_Cscan") :
            //                "m_W_i_FilePath_M_UI"), SysInfo.m_W_i_FilePath, SysInfo.HardFileName);

            //SysInfo.m_Tofd_C_Scan.Init_UI();
            //Ck_Thick_2.Checked = SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_2BeiSjxx;

            //#region 超声模块使用选择
            //string _strT =   SysInfo.csInter.IniReadDefine ("ClassUltrasGate", "Txt_Mul_Select","", System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini");
            //SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have = SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum>0;
            //SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have &=( _strT.IndexOf("1") > -1);

            //Bt_Prepare.Visible = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 &&
            //        SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Only_Coat && _strT.IndexOf("1") > -1;

            //if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 && SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Only_Coat &&
            //    SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have)
            //{
            //    SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have = false;
            //}

            //if (_strT != "")
            //{
            //    int _iUseNum = _strT.Length;
            //    if (_iUseNum == SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum && SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_Arr_RemoteIpPort != null )
            //    {
            //        for (int i = 0; i < _iUseNum; i++)
            //            SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_Arr_RemoteIpPort[i].blUse = _strT.Substring(i, 1) == "1";
            //    }
            //}
            //#endregion 

            //if (SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type == 1)
            //{
            //    SysInfo.m_SysBuff.m_Tofd_DLL.SetH_W(Pic_A.Width, Pic_A.Height, 0);
            //    Pan_A_Ctrl_0.Visible = true;
            //    Pan_A_Ctrl.Visible = false;

            //    Pan_A_Ctrl_0.Visible = false;
            //}
            //else if(SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type == 2&& SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)
            //{
            //    Pan_A_ECT_Ctrl.Visible = false ;
            //    Pan_A_Ctrl_0.Visible = false ;
            //    Pan_A_Ctrl.Visible = false;
            //    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)  
            //    {
            //        if (SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL != null)
            //            SysInfo.m_SysBuff_C.m_Ctrl.m_bl_FY = SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type == 2 && SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.m_bl_My_Distanc;
            //    }
            //    #region 涡流
            //    string _strFile = System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini";
            //    #region 1 构件形状
            //    Cmb_Gjxz.Items.Clear();
            //    int _iNum = int.Parse(SysInfo.csInter.IniReadDefine("COM_Gjxz", "Num", "3", _strFile));
            //    for (int _iN0 = 1; _iN0 <= _iNum; _iN0++)
            //    {
            //        _strT = "";
            //        switch (_iN0)
            //        {
            //            case 1:
            //                _strT = "平面";
            //                break;
            //            case 2:
            //                _strT = "管道";
            //                break;
            //            case 3:
            //                _strT = "罐体";
            //                break;
            //        }
            //        Cmb_Gjxz.Items.Add(SysInfo.csInter.IniReadDefine("COM_Gjxz", _iN0.ToString(), _strT, _strFile));
            //    }
            //    Cmb_Gjxz.SelectedIndex = SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.g_i_Gjxz;
            //    #endregion 1 构件形状
            //    #region 2 本体材质
            //    Cmb_Btcz.Items.Clear();
            //    _iNum = int.Parse(SysInfo.csInter.IniReadDefine("Cmb_Btcz", "Num", "2", _strFile));
            //    for (int _iN0 = 1; _iN0 <= _iNum; _iN0++)
            //    {
            //        _strT = "";
            //        switch (_iN0)
            //        {
            //            case 1:
            //                _strT = "碳钢";
            //                break;
            //            case 2:
            //                _strT = "不锈钢";
            //                break;
            //        }
            //        Cmb_Btcz.Items.Add(SysInfo.csInter.IniReadDefine("Cmb_Btcz", _iN0.ToString(), _strT, _strFile));
            //    }
            //    Cmb_Btcz.SelectedIndex = SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.g_iClcz;
            //    #endregion 2 本体材质
            //    #region 3 保护层材质
            //    Cmb_Bhccz.Items.Clear();
            //    _iNum = int.Parse(SysInfo.csInter.IniReadDefine("Cmb_Bhccz", "Num", "3", _strFile));
            //    for (int _iN0 = 1; _iN0 <= _iNum; _iN0++)
            //    {
            //        _strT = "";
            //        switch (_iN0)
            //        {
            //            case 1:
            //                _strT = "无";
            //                break;
            //            case 2:
            //                _strT = "吕皮";
            //                break;
            //            case 3:
            //                _strT = "铁皮";
            //                break;
            //            case 4:
            //                _strT = "铁丝";
            //                break;
            //        }
            //        Cmb_Bhccz.Items.Add(SysInfo.csInter.IniReadDefine("Cmb_Bhccz", _iN0.ToString(), _strT, _strFile));
            //    }
            //    Cmb_Bhccz.SelectedIndex = SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.g_i_Bfccz;
            //    #endregion 3 保护层材质

            //    #region 4 选用探头
            //    Cmb_Xytt.Items.Clear();
            //    _iNum = int.Parse(SysInfo.csInter.IniReadDefine("Cmb_Xytt", "Num", "2", _strFile));
            //    for (int _iN0 = 1; _iN0 <= _iNum; _iN0++)
            //    {
            //        _strT = "";
            //        switch (_iN0)
            //        {
            //            case 1:
            //                _strT = "P0";
            //                break;
            //            case 2:
            //                _strT = "P1";
            //                break;
            //        }
            //        Cmb_Xytt.Items.Add(SysInfo.csInter.IniReadDefine("Cmb_Xytt", _iN0.ToString(), _strT, _strFile));
            //    }
            //    Cmb_Xytt.SelectedIndex = SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.g_i_Ttlx;
            //    #endregion 4 选用探头

            //    Txt_Tld.Text = SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL  .g_fl_Tld.ToString();

            //    Txt_Gj.Text = SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.g_fl_Gj.ToString();
            //    Txt_Tld.Text = SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.g_fl_Tld.ToString();
            //    Txt_Gcm.Text = SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.g_Gcm;
            //    Txt_Xmm.Text = SysInfo.m_Tofd_C_Scan.m_ECT_My_DLL.g_Xmm;
            //    #endregion 涡流
            //}
            //else
            //{
            //    Pan_A_Ctrl_0.Visible = false;
            //}

            //if (SysInfo.m_Tofd_C_Scan.m_blLink == false && ! (SysInfo .m_SysBuff_C .m_Ctrl .m_bl_FY ||
            //                                   SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 && SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type == 2))
            //{
            //    //      SysInfo.m_strLinkMsg += (SysInfo.m_strLinkMsg == "" ? "" : ", ") + " 超声模块";
            //    if (SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type!=1)    Time_UI_ReLink.Enabled = true;
            //}
            //SysInfo.m_SysBuff_C.m_Ctrl.m_iRun = 0;
            //SysInfo.m_Tofd_C_Scan.Thread_ReadUI();
            //#endregion 3 

            #region 4 连相机、电池 
          
            //1平板电池板通讯联机
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 2)
            {
                Bt_Dc.Visible = true ;
                //       PowerLink();
                if (Thread_Power != null) Thread_Power.Abort();
                Thread_Power = new Thread(new ThreadStart(PowerLink));
                //  Thread_Brush_A.Priority = ThreadPriority.Highest;
                Thread_Power.Name = "Thread_Power";
                Thread_Power.IsBackground = true;
                Thread_Power.Start();
            }

             
                Video_Start();
          

            #endregion 4
        }
        private void Video_Start()
        {
            #region  4.2相机服务器
           // LinkServer();//
            Link_Video();

            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2) LinkServer_Distanc();
            #endregion 4.2
        }
        /// <summary>
        /// TOFD联机
        /// </summary>
        private Thread Thread_Tofd_Link = null;
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
            SysInfo.m_SysBuff.m_Tofd_DLL.Link();
            if (SysInfo.m_SysBuff.m_Tofd_DLL.m_strLinkMsg != "")

                SysInfo.m_strLinkMsg += (SysInfo.m_strLinkMsg != "" ? "," : "") + SysInfo.m_SysBuff.m_Tofd_DLL.m_strLinkMsg;
            if (SysInfo.m_strLinkMsg != "")
                MessageBox.Show(SysInfo.m_strLinkMsg);
        }
        private void Plant_Rep_Init()
        {
          
        }
        private Thread Thread_Video_Link = null;

        /// <summary>
        /// 画D扫描波形图  
        /// </summary>
        delegate void Delg_Video();
        /// <summary>
        /// 启动相机服务器
        /// </summary>
        private void LinkServer()
        {
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_Video ms = new Delg_Video(Video_Show);
                    this.Invoke(ms, new object[] { });
                }
                catch
                { }
            }
            else
            {
                Video_Show();
            }
        }

        private void Video_Show()
        {
            if (int.Parse(SysInfo.csInter.IniReadDefine("Video", "Link_V", "1", SysInfo.HardFileName)) == 1)
            {
                ClearOneClient("ClimbVideo");
                SysInfo.m_ServerUI.Start();
                Link_V();
                SysInfo.csInter.WaitTime(0.5, ref SysInfo.m_ServerUI.m_blLink);

                if (SysInfo.m_ServerUI.m_blLink == false)
                    SysInfo.m_strLinkMsg += (SysInfo.m_strLinkMsg != "" ? ", " : "") + (SysInfo.m_iLanguage == 0 ? " 相机" : " camera");
            }
        }
        private void Link_Video()
        {
            if (Thread_Video_Link != null) Thread_Video_Link.Abort();
            Thread_Video_Link = new Thread(new ThreadStart(LinkServer));

            Thread_Video_Link.Name = "Thread_Video_Link";
            Thread_Video_Link.IsBackground = true;
            Thread_Video_Link.Start();
        }

        /// <summary>
        /// 平板电池通讯板联机
        /// </summary>
        private void PowerLink()
        {
            try
            {
                if (SysInfo.m_PcPower == null || SysInfo.m_PcPower != null && SysInfo.m_PcPower.m_blNetLink == false) //   if (m_UT_My_DLL.Com_Power_Com != SysInfo.m_SysInfo.g_Climb.Com_portName_Climb)
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
                            bool _blOk = false;
                            for (int i = 0; i < 4; i++)
                            {
                                if (m_blArrLinkState[iN] == false && SysInfo.m_PcPower.m_blNetLink==false )
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
                                            if (DateTime.Now.Subtract(dtStar).TotalSeconds > 2) break;
                                            Thread.Sleep(10);
                                        }
                                        catch { break; }
                                    }
                                    if (SysInfo.m_PcPower.m_strPowerPC != "")
                                    {
                                        m_blArrLinkState[iN] = true;//m_arrPort_Names
                                        SysInfo.m_PcPower.m_blNetLink = true;
                                      //  MessageBox.Show("COM" + m_arrPort_Names[iN] + "成功");
                                        break;
                                    }
                                    SysInfo.csInter.WaitTime(1);
                                }
                            }
                            if (SysInfo.m_PcPower.m_blNetLink) break;
                        }
                    }
                    if (SysInfo.m_PcPower.m_blNetLink == false) SysInfo.m_PcPower.CloseCom();//失败就释放资源
                }
            }
            catch { }

            if (SysInfo.m_PcPower.m_blNetLink == false)
                SysInfo.m_strLinkMsg += (SysInfo.m_strLinkMsg != "" ? ", " : "") + (SysInfo.m_iLanguage == 0 ? " 电池通讯" : " Battery commun.");
        }
        /// <summary>
        /// 图像初始化
        /// </summary>
        private void Plant_C_Init()
        {
            int iTimes = 0;
            string _stCom = "";
            //while (true)//获得系统串口
            //{
            //    m_arrPort_Names = System.IO.Ports.SerialPort.GetPortNames();
            //    for (int iT = 0; iT < m_arrPort_Names.Count(); iT++)
            //        _stCom += (iT == 0 ? "" : ",") + m_arrPort_Names[iT];
            //    if (m_arrPort_Names.Count() > 0) break;
            //    SysInfo.WaitTime(0.05f);
            //    iTimes++;
            //    if (iTimes == 3) break;
            //}
       //     m_blArrLinkState = new bool[m_arrPort_Names.Count()];
            //SysInfo  .m_SysBuff_C.m_Plant_C.Pic_A = Pic_A;
            //SysInfo  .m_SysBuff_C.m_Plant_C.Pic_B = Pic_B;
            //SysInfo  .m_SysBuff_C.m_Plant_C.Pic_C = Pic_C;
            //Pic_Coat.Width = Pic_C.Width;
            //Pic_Coat.Height  = Pic_C.Height;
            //SysInfo.m_SysBuff_C.m_Plant_C.Pic_Coat_C = Pic_Coat ;
            //SysInfo  .m_SysBuff_C.m_Plant_C.Pic_C_All = Pic_C_All;
            //SysInfo.m_SysBuff_C.m_Plant_C.Pic_Coat_All = Pic_Coat_All;
            //SysInfo.m_SysBuff_C.m_Plant_C.m_bt_Coat_All_Fd = Bt_Coat_All_Fd;
            //SysInfo.m_SysBuff_C.m_Plant_C.m_bt_Coat_All_Fd_To_Mark = Bt_Coat_All_Fd_To_Mark;
            ////     SysInfo  .m_SysBuff_C.m_Plant_C.Init();

            SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X = SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval;
            if (SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X == 0)
                SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X = 1;
            Val_Chg();
        }
        private void ClearOneClient(string AppEXE)
        {
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName(AppEXE))
            {
                try
                {
                    p.Kill();
                }
                catch { }
            }
        }
        #region 注册
        /// <summary>
        /// 单点按钮
        /// </summary>
        bool m_bl_AnYaAnNiu = false;
        int m_i_Moov_F1_F7 = -1, m_iAlarmMark = 0;
        int m_i_Moov_F1_F7_Old = -1;
      
        int m_i_Sd = 1;

        bool m_bl_Cmd_Key = false;
        /// <summary>
        /// 车体状态 3：停车 1：前进 2：后退 4：前左拐弯 5：前右拐弯 6：后左拐弯 7：后右拐弯
        /// </summary>
        int m_i_CarState = 3;
        DateTime m_dtKeyStar = DateTime.Now;
        
        private void hook_KeyDown_New(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (DateTime.Now.Subtract(m_dtKeyStar).TotalSeconds < 0.5) return;
                m_dtKeyStar = DateTime.Now;

                #region 键值处理
                if (SysInfo.m_Climb4 == null) return;
                if (e.KeyCode == Keys.F2)//前进
                {
                    if(m_i_CarState==3)
                    {
                        m_i_CarState = 1; SysInfo.m_Climb4.SendData(1, 1, 6, 0, "1");
                    }
                    else if(m_i_CarState==1)
                    { }
                    else
                    {
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");//停车
                        m_i_CarState = 3;
                    }
                    


                    //By_Have_Run();
                    ////01：前进    02：后退      03：停止 04：前进左转 
                    ////05：前进右转 06：后退左转 07：后退右转
                    //if (SysInfo.m_Climb4.m_blLink == false) return;
                    //if (m_bl_Cmd_Key == false)
                    //{
                    //    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "1");
                    //    m_bl_Cmd_Key = true;
                    //}

                }
                else if (e.KeyCode == Keys.F4)//后退
                {
                    if (m_i_CarState == 3)
                    {
                        m_i_CarState = 2; SysInfo.m_Climb4.SendData(1, 1, 6, 0, "2");
                    }
                    else if (m_i_CarState == 2)
                    { }
                    else
                    {
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");//停车
                        m_i_CarState = 3;
                    }
                    //By_Have_Run();
                    ////     if (m_bl_Stop_Time) return;
                    ////01：前进    02：后退      03：停止 04：前进左转 
                    ////05：前进右转 06：后退左转 07：后退右转
                    ////  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
                    //if (SysInfo.m_Climb4.m_blLink == false) return;
                    ////  if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    //if (m_bl_Cmd_Key == false)
                    //{
                    //    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "2"); m_bl_Cmd_Key = true;
                    //}
                }
                else if (e.KeyCode == Keys.F3)//前进左转
                {
                    if (m_i_CarState == 3)
                    {
                        m_i_CarState = 4; SysInfo.m_Climb4.SendData(1, 1, 6, 0, "4");
                    }
                    else if (m_i_CarState ==4)
                    { }
                    else
                    {
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");//停车
                        m_i_CarState = 3;
                    }
                    //By_Have_Run();
                    //// if (m_bl_Stop_Time) return;
                    //if (SysInfo.m_Climb4.m_blLink == false) return;
                    ////01：前进    02：后退      03：停止 04：前进左转 
                    ////05：前进右转 06：后退左转 07：后退右转
                    ////  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 1;
                    ////  if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    //if (m_bl_Cmd_Key == false)
                    //{
                    //    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "4"); m_bl_Cmd_Key = true;
                    //}
                }
                else if (e.KeyCode == Keys.F1)//前进右转  
                {
                    if (m_i_CarState == 3)
                    {
                        m_i_CarState = 5; SysInfo.m_Climb4.SendData(1, 1, 6, 0, "5");
                    }
                    else if (m_i_CarState ==5)
                    { }
                    else
                    {
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");//停车
                        m_i_CarState = 3;
                    }
                    //By_Have_Run();
                    //// if (m_bl_Stop_Time) return; 
                    //if (SysInfo.m_Climb4.m_blLink == false) return;
                    ////01：前进    02：后退      03：停止 04：前进左转 
                    ////05：前进右转 06：后退左转 07：后退右转
                    ////  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 1;
                    ////  if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    //if (m_bl_Cmd_Key == false)
                    //{
                    //    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "5"); m_bl_Cmd_Key = true;
                    //}
                }
                else if (e.KeyCode == Keys.F7)//后退左转  左侧下
                {
                    if (m_i_CarState == 3)
                    {
                        m_i_CarState = 6; SysInfo.m_Climb4.SendData(1, 1, 6, 0, "6");
                    }
                    else if (m_i_CarState == 6)
                    { }
                    else
                    {
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");//停车
                        m_i_CarState = 3;
                    }
                    //try
                    //{
                    //    By_Have_Run();
                    //    //   if (m_bl_Stop_Time) return; 
                    //    if (SysInfo.m_Climb4.m_blLink == false) return;
                    //    //   SysInfo.csInter.WaitTime(0.05);
                    //    //01：前进    02：后退      03：停止 04：前进左转 
                    //    //05：前进右转 06：后退左转 07：后退右转
                    //    //  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
                    //    //   if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    //    if (m_bl_Cmd_Key == false)
                    //    {
                    //        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "6"); m_bl_Cmd_Key = true;
                    //    }
                    //}
                    //catch { }
                }
                else if (e.KeyCode == Keys.F6)//后退右转   右侧下
                {
                    if (m_i_CarState == 3)
                    {
                        m_i_CarState = 7; SysInfo.m_Climb4.SendData(1, 1, 6, 0, "7");
                    }
                    else if (m_i_CarState == 7)
                    { }
                    else
                    {
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");//停车
                        m_i_CarState = 3;
                    }
                    //By_Have_Run();
                    ////  if (m_bl_Stop_Time) return; 
                    //if (SysInfo.m_Climb4.m_blLink == false) return;
                    ////    SysInfo.csInter.WaitTime(0.05);
                    ////01：前进    02：后退      03：停止 04：前进左转 
                    ////05：前进右转 06：后退左转 07：后退右转
                    ////   SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
                    ////   if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    //if (m_bl_Cmd_Key == false)
                    //{
                    //    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "7");
                    //    m_bl_Cmd_Key = true;
                    //}
                }

                else if (e.KeyCode == Keys.F8)// 2022-6-28 修改成相机切换    左侧上: 停车
                {
                    if (m_bl_AnYaAnNiu == false)
                    {
                        m_bl_AnYaAnNiu = true;
                        if (SysInfo.m_iShowPhone < 3)
                            SysInfo.m_iShowPhone++;
                        else
                            SysInfo.m_iShowPhone = 0;
                        SysInfo.g_Msg_InterFace.Fun_ChangeVideo(SysInfo.m_iShowPhone);

                        SysInfo.csInter.WaitTime(0.2);
                        m_bl_AnYaAnNiu = false;
                    }
                }
                else if (e.KeyCode == Keys.F10)//打标  下方右侧
                {
                    By_Have_Run();
                    m_iAlarmMark = 1; if (SysInfo.m_Climb4.m_blLink == false) return;
                    SysInfo.m_Climb4.SendData(5, 2, 0, 0, m_iAlarmMark.ToString());
                }

                else if (e.KeyCode == Keys.F9&& SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 3)//探头升降  下方左侧
                {
                    m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;
                    if (SysInfo.m_Climb4.m_blLink == false) return;
                    if (m_bl_AnYaAnNiu == false)
                    {
                        m_bl_AnYaAnNiu = true;
                        SysInfo.m_SysBuff.m_Climb.iGsbTtLx = SysInfo.m_SysBuff.m_Climb.iGsbTtLx == 0 ? 1 : 0;
                        SysInfo.m_Climb4.SendData(5, 1, 0, 0, SysInfo.m_SysBuff.m_Climb.iGsbTtLx.ToString());
                        SysInfo.csInter.WaitTime(2);
                        m_bl_AnYaAnNiu = false;
                    }
                }
                else if (e.KeyCode == Keys.F5 || e.KeyCode.ToString() == "LWin")//  右侧上：运行后的暂停
                {
                    m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;
                    //      if (SysInfo.m_Climb4.m_blLink == false) return;
                    if (m_bl_AnYaAnNiu == false)
                    {
                        //  SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
                        m_bl_AnYaAnNiu = true;

                        if (m_i_Sd == SysInfo.m_bt_Arr.Length)//4
                            m_i_Sd = 0;
                        SysInfo.m_SysBuff.m_Climb.Speed = SysInfo.m_bt_Arr[m_i_Sd++];

                        //  SysInfo.m_SysBuff.m_Climb.i_Gsb_Speed = (int)Track_Gsb.Value;
                        SysInfo.SetSpeed();
                        SysInfo.csInter.WaitTime(1.5);
                        m_bl_AnYaAnNiu = false;
                    }

                    //----

                }


                else if (e.KeyCode == Keys.F11)//项目开始结束33
                {
                    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");//停车

                    //m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;
                    //if (SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun == 0)
                    //    BeginTest();
                    //else
                    //    StopTest();

                }
                #endregion
            }
            catch { }

            //   SysInfo.csInter.WaitTime(0.01);
            Application.DoEvents();
        }
        private void hook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
              
                #region 键值处理
                if (SysInfo.m_Climb4 == null) return;
                if (e.KeyCode == Keys.F2)//前进
                {
                    By_Have_Run();
                    //    if (m_bl_Stop_Time) return;
                    //01：前进    02：后退      03：停止 04：前进左转 
                    //05：前进右转 06：后退左转 07：后退右转
                    if (SysInfo.m_Climb4.m_blLink == false) return;
                    //   if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    if (m_bl_Cmd_Key == false)
                    {
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "1");
                        m_bl_Cmd_Key = true;
                    }

                }
                else if (e.KeyCode == Keys.F4)//后退
                {
                    By_Have_Run();
                    //     if (m_bl_Stop_Time) return;
                    //01：前进    02：后退      03：停止 04：前进左转 
                    //05：前进右转 06：后退左转 07：后退右转
                    //  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
                    if (SysInfo.m_Climb4.m_blLink == false) return;
                    //  if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    if (m_bl_Cmd_Key == false)
                    {
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "2"); m_bl_Cmd_Key = true;
                    }
                }
                else if (e.KeyCode == Keys.F3)//前进左转
                {
                    By_Have_Run();
                    // if (m_bl_Stop_Time) return;
                    if (SysInfo.m_Climb4.m_blLink == false) return;
                    //01：前进    02：后退      03：停止 04：前进左转 
                    //05：前进右转 06：后退左转 07：后退右转
                    //  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 1;
                    //  if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    if (m_bl_Cmd_Key == false)
                    {
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "4"); m_bl_Cmd_Key = true;
                    }
                }
                else if (e.KeyCode == Keys.F1)//前进右转  
                {
                    By_Have_Run();
                    // if (m_bl_Stop_Time) return; 
                    if (SysInfo.m_Climb4.m_blLink == false) return;
                    //01：前进    02：后退      03：停止 04：前进左转 
                    //05：前进右转 06：后退左转 07：后退右转
                    //  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 1;
                    //  if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    if (m_bl_Cmd_Key == false)
                    {
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "5"); m_bl_Cmd_Key = true;
                    }
                }
                else if (e.KeyCode == Keys.F7)//后退左转  左侧下
                {
                    try
                    {
                        By_Have_Run();
                        //   if (m_bl_Stop_Time) return; 
                        if (SysInfo.m_Climb4.m_blLink == false) return;
                        //   SysInfo.csInter.WaitTime(0.05);
                        //01：前进    02：后退      03：停止 04：前进左转 
                        //05：前进右转 06：后退左转 07：后退右转
                        //  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
                        //   if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                        if (m_bl_Cmd_Key == false)
                        {
                            SysInfo.m_Climb4.SendData(1, 1, 6, 0, "6"); m_bl_Cmd_Key = true;
                        }
                    }
                    catch { }
                }
                else if (e.KeyCode == Keys.F6)//后退右转   右侧下
                {
                    By_Have_Run();
                    //  if (m_bl_Stop_Time) return; 
                    if (SysInfo.m_Climb4.m_blLink == false) return;
                    //    SysInfo.csInter.WaitTime(0.05);
                    //01：前进    02：后退      03：停止 04：前进左转 
                    //05：前进右转 06：后退左转 07：后退右转
                    //   SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
                    //   if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    if (m_bl_Cmd_Key == false)
                    {
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "7");
                        m_bl_Cmd_Key = true;
                    }
                }

                else if (e.KeyCode == Keys.F8)// 2022-6-28 修改成相机切换    左侧上: 停车
                {
                    if (m_bl_AnYaAnNiu == false)
                    {
                        m_bl_AnYaAnNiu = true;
                        if (SysInfo.m_iShowPhone < 3)
                            SysInfo.m_iShowPhone++;
                        else
                            SysInfo.m_iShowPhone = 0;
                        SysInfo.g_Msg_InterFace.Fun_ChangeVideo(SysInfo.m_iShowPhone);

                        SysInfo.csInter.WaitTime(0.2);
                        m_bl_AnYaAnNiu = false;
                    }
                    //    m_i_Moov_F1_F7 = 0; Time_Key.Enabled = false;
                    //if (SysInfo.m_Climb4.m_blLink == false) return;
                    //if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    //    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
                }
                else if (e.KeyCode == Keys.F10)//打标  下方右侧
                {
                    //  m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;

                    By_Have_Run();
                    m_iAlarmMark = 1; if (SysInfo.m_Climb4.m_blLink == false) return;
                    SysInfo.m_Climb4.SendData(5, 2, 0, 0, m_iAlarmMark.ToString());
                }

                else if (e.KeyCode == Keys.F9&& SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 3)//探头升降  下方左侧
                {
                    m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;
                    if (SysInfo.m_Climb4.m_blLink == false) return;
                    if (m_bl_AnYaAnNiu == false)
                    {
                        m_bl_AnYaAnNiu = true;
                        SysInfo.m_SysBuff.m_Climb.iGsbTtLx = SysInfo.m_SysBuff.m_Climb.iGsbTtLx == 0 ? 1 : 0;
                        SysInfo.m_Climb4.SendData(5, 1, 0, 0, SysInfo.m_SysBuff.m_Climb.iGsbTtLx.ToString());
                        SysInfo.csInter.WaitTime(2);
                        m_bl_AnYaAnNiu = false;
                    }
                }
                else if (e.KeyCode == Keys.F5 || e.KeyCode.ToString() == "LWin")//  右侧上：运行后的暂停
                {
                    //m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;
                    //if (SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun == 1)
                    //    BeginTest();

                    //--------------
                    m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;
                    //      if (SysInfo.m_Climb4.m_blLink == false) return;
                    if (m_bl_AnYaAnNiu == false)
                    {
                        //  SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
                        m_bl_AnYaAnNiu = true;

                        if (m_i_Sd == SysInfo.m_bt_Arr.Length)//4
                            m_i_Sd = 0;
                        SysInfo.m_SysBuff.m_Climb.Speed = SysInfo.m_bt_Arr[m_i_Sd++];

                        //  SysInfo.m_SysBuff.m_Climb.i_Gsb_Speed = (int)Track_Gsb.Value;
                        SysInfo.SetSpeed();
                        SysInfo.csInter.WaitTime(1.5);
                        m_bl_AnYaAnNiu = false;
                    }

                    //----

                }


                else if (e.KeyCode == Keys.F11)//项目开始结束33
                {
                    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");//停车

                    //m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;
                    //if (SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun == 0)
                    //    BeginTest();
                    //else
                    //    StopTest();

                }
                #endregion
            }
            catch { }


            //try
            //{

            //    #region 键值处理
            //    if (SysInfo.m_Climb4 == null) return;
            //    if (e.KeyCode == Keys.F2)//前进
            //    {
            //        By_Have_Run();
            //        //    if (m_bl_Stop_Time) return;
            //        //01：前进    02：后退      03：停止 04：前进左转 
            //        //05：前进右转 06：后退左转 07：后退右转
            //        if (SysInfo.m_Climb4.m_blLink == false) return;
            //      //  if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
            //        {//m_bl_Cmd_Key == false &&
            //            SysInfo.m_Climb4.SendData(1, 1, 6, 0, "1");
            //          //  m_bl_Cmd_Key = true;
            //        }

            //    }
            //    else if (e.KeyCode == Keys.F4)//后退
            //    {
            //        By_Have_Run();
            //        //     if (m_bl_Stop_Time) return;
            //        //01：前进    02：后退      03：停止 04：前进左转 
            //        //05：前进右转 06：后退左转 07：后退右转
            //        //  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
            //        if (SysInfo.m_Climb4.m_blLink == false) return;
            //    //    if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
            //        {
            //            SysInfo.m_Climb4.SendData(1, 1, 6, 0, "2");//m_bl_Cmd_Key = true;
            //        }
            //    }
            //    else if (e.KeyCode == Keys.F3)//前进左转
            //    {
            //        By_Have_Run();
            //       // if (m_bl_Stop_Time) return;
            //        if (SysInfo.m_Climb4.m_blLink == false) return;
            //        //01：前进    02：后退      03：停止 04：前进左转 
            //        //05：前进右转 06：后退左转 07：后退右转
            //        //  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 1;
            //   //     if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
            //        {
            //            SysInfo.m_Climb4.SendData(1, 1, 6, 0, "4"); //m_bl_Cmd_Key = true;
            //        }
            //    }
            //    else if (e.KeyCode == Keys.F1)//前进右转  
            //    {
            //        By_Have_Run();
            //       // if (m_bl_Stop_Time) return; 
            //        if (SysInfo.m_Climb4.m_blLink == false) return;
            //        //01：前进    02：后退      03：停止 04：前进左转 
            //        //05：前进右转 06：后退左转 07：后退右转
            //        //  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 1;
            //      //  if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
            //        {
            //            SysInfo.m_Climb4.SendData(1, 1, 6, 0, "5"); //m_bl_Cmd_Key = true;
            //        }
            //    }
            //    else if (e.KeyCode == Keys.F7)//后退左转  左侧下
            //    {
            //        try
            //        {
            //            By_Have_Run();
            //         //   if (m_bl_Stop_Time) return; 
            //            if (SysInfo.m_Climb4.m_blLink == false) return;
            //            //   SysInfo.csInter.WaitTime(0.05);
            //            //01：前进    02：后退      03：停止 04：前进左转 
            //            //05：前进右转 06：后退左转 07：后退右转
            //            //  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
            //       //     if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
            //            {
            //                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "6");// m_bl_Cmd_Key = true;
            //            }
            //        }
            //        catch { }
            //    }
            //    else if (e.KeyCode == Keys.F6)//后退右转   右侧下
            //    {
            //        By_Have_Run();
            //      //  if (m_bl_Stop_Time) return; 
            //        if (SysInfo.m_Climb4.m_blLink == false) return;
            //        //    SysInfo.csInter.WaitTime(0.05);
            //        //01：前进    02：后退      03：停止 04：前进左转 
            //        //05：前进右转 06：后退左转 07：后退右转
            //        //   SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
            //    //    if ( SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
            //        {
            //            SysInfo.m_Climb4.SendData(1, 1, 6, 0, "7");
            //          //  m_bl_Cmd_Key = true;
            //        }
            //        }

            //    else if (e.KeyCode == Keys.F8)// 2022-6-28 修改成相机切换    左侧上: 停车
            //    {
            //        if (m_bl_AnYaAnNiu == false)
            //        {
            //            m_bl_AnYaAnNiu = true;
            //            if (SysInfo.m_iShowPhone < 3)
            //                SysInfo.m_iShowPhone++;
            //            else
            //                SysInfo.m_iShowPhone = 0;
            //            SysInfo.g_Msg_InterFace.Fun_ChangeVideo(SysInfo.m_iShowPhone);

            //            SysInfo.csInter.WaitTime(0.2);
            //            m_bl_AnYaAnNiu = false;
            //        }
            //        //    m_i_Moov_F1_F7 = 0; Time_Key.Enabled = false;
            //        //if (SysInfo.m_Climb4.m_blLink == false) return;
            //        //if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
            //        //    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
            //    }
            //    else if (e.KeyCode == Keys.F10)//打标  下方右侧
            //    {
            //        //  m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;

            //        By_Have_Run();
            //        m_iAlarmMark = 1; if (SysInfo.m_Climb4.m_blLink == false) return;
            //        SysInfo.m_Climb4.SendData(5, 2, 0, 0, m_iAlarmMark.ToString());
            //    }

            //    else if (e.KeyCode == Keys.F9)//探头升降  下方左侧
            //    {
            //        m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;
            //        if (SysInfo.m_Climb4.m_blLink == false) return;
            //        if (m_bl_AnYaAnNiu == false)
            //        {
            //            m_bl_AnYaAnNiu = true;
            //            SysInfo.m_SysBuff.m_Climb.iGsbTtLx = SysInfo.m_SysBuff.m_Climb.iGsbTtLx == 0 ? 1 : 0;
            //            SysInfo.m_Climb4.SendData(5, 1, 0, 0, SysInfo.m_SysBuff.m_Climb.iGsbTtLx.ToString());
            //            SysInfo.csInter.WaitTime(2);
            //            m_bl_AnYaAnNiu = false;
            //        }
            //    }
            //    else if (e.KeyCode == Keys.F5 || e.KeyCode.ToString ()=="LWin")//  右侧上：运行后的暂停
            //    {
            //        //m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;
            //        //if (SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun == 1)
            //        //    BeginTest();

            //        //--------------
            //        m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;
            //        //      if (SysInfo.m_Climb4.m_blLink == false) return;
            //        if (m_bl_AnYaAnNiu == false)
            //        {
            //            //  SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
            //            m_bl_AnYaAnNiu = true;

            //            if (m_i_Sd == SysInfo.m_bt_Arr.Length)//4
            //                m_i_Sd = 0;
            //            SysInfo.m_SysBuff.m_Climb.Speed = SysInfo.m_bt_Arr[m_i_Sd++];

            //            //  SysInfo.m_SysBuff.m_Climb.i_Gsb_Speed = (int)Track_Gsb.Value;
            //            SysInfo.SetSpeed();
            //            SysInfo.csInter.WaitTime(1.5);
            //            m_bl_AnYaAnNiu = false;
            //        }

            //        //----

            //    }


            //    else if (e.KeyCode == Keys.F11)//项目开始结束33
            //    {
            //        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");//停车

            //        //m_i_Moov_F1_F7 = -1; Time_Key.Enabled = false;
            //        //if (SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun == 0)
            //        //    BeginTest();
            //        //else
            //        //    StopTest();

            //    }
            //    #endregion
            //}
            //catch { }

            //   SysInfo.csInter.WaitTime(0.01);
            Application.DoEvents();
        }

        bool m_bl_Stop_Time = false;
        private void By_Have_Run()
        {
            //Time_Key.Enabled = false;
            //if (m_bl_Stop_Time == false)
            //    Time_Key.Enabled = true;
            //m_i_Moov_F1_F7++;

            if (Time_Key.Enabled == false)
            {
                m_i_Moov_F1_F7 = 0; m_i_Moov_F1_F7_Old = -1;
                Time_Key.Enabled = true;
            }
            m_i_Moov_F1_F7++;
        }

        private void Time_Key_Tick(object sender, EventArgs e)
        {
            //    m_i_Moov_F1_F7++;
            //     m_i_Moov_F1_F7_Old += 10;
            //     int _iAdd = m_i_Moov_F1_F7_Old - m_i_Moov_F1_F7;

            if (m_i_Moov_F1_F7 > m_i_Moov_F1_F7_Old)
            {
                m_i_Moov_F1_F7_Old = m_i_Moov_F1_F7;

            }
            else if (m_i_Moov_F1_F7 == m_i_Moov_F1_F7_Old)
            {
                Time_Key.Enabled = false;
                m_i_Moov_F1_F7_Old = 0;
                m_i_Moov_F1_F7 = 0;
                m_bl_Cmd_Key = false;
                if (m_iAlarmMark == 1)
                {
                    m_iAlarmMark = 0; SysInfo.m_Climb4.SendData(5, 2, 0, 0, m_iAlarmMark.ToString());
                }
                else
                {
                    if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");//停车
                }
            }

            /*
              //    m_i_Moov_F1_F7++;
            //     m_i_Moov_F1_F7_Old += 10;
            //     int _iAdd = m_i_Moov_F1_F7_Old - m_i_Moov_F1_F7;

            if (m_i_Moov_F1_F7 > m_i_Moov_F1_F7_Old)
            {
                m_i_Moov_F1_F7_Old = m_i_Moov_F1_F7;

            }
            else if (m_i_Moov_F1_F7 == m_i_Moov_F1_F7_Old)
            if (m_i_Moov_F1_F7 > SysInfo.Time_Key_Down && Time_Key.Enabled)
            {
                m_bl_Cmd_Key = false;
                m_bl_Stop_Time = true;
                m_i_Moov_F1_F7 = 0;
                Time_Key.Enabled = false;
                m_i_Moov_F1_F7_Old = 0;


                if (m_iAlarmMark == 1)
                {
                    m_iAlarmMark = 0; SysInfo.m_Climb4.SendData(5, 2, 0, 0, m_iAlarmMark.ToString());
                }
                else
                {
                    //   if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)

           //         JinJiTingChe();
                    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "1");
                    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
                    m_i_Moov_F1_F7 = 0;
                    Time_Key.Enabled = false;
                    m_bl_Stop_Time = false;
                }
            }
             */
        }

        private void Bt_Start_Click(object sender, EventArgs e)
        {
            Begentest_Run(0);
        }
        private void KaiShi()
        {
            //SysInfo.m_Climb4.SendData(2, 4, 0, 1, SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval.ToString());
            //if (Rad_1.Checked == false)
            //    Rad_Visue(0);
            //Lb_Save.Visible = false;
            //BeginTest();
        }

    
        /// <summary>
        /// 开始检测
        /// </summary>
        private void BeginTest()
        {
          //  Lb_Init.Visible = false;
            //判断数据是否转存完成
            if (SysInfo  .m_SysBuff_C.m_Ctrl.m_iRun != 1)
            {
                //1 项目焊缝信息采集
                if (SysInfo.m_C_Item_Info.StrGcmc != SysInfo.m_C_Item_Info.StrGcmc_Old)
                    SysInfo.m_C_Item_Info.i_C_AllRows = 0;
                else
                {
                   
                }
                SysInfo.m_C_Item_Info.i_CurrRow_No = SysInfo.m_C_Item_Info.i_C_AllRows - 1;
                SysInfo.m_C_Item_Info.strJyrq = DateTime.Now.ToString("yyyy-MM-dd");

                #region 防止使用结束保存功能
                if (SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff.Count > 0)
                {
                    SysInfo.m_strArrGjbh = "";
                    for (int i = 0; i < SysInfo.m_C_Item_Info.i_C_AllRows; i++)
                        SysInfo.m_strArrGjbh += (i == 0 ? "" : "&") + SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[i].strGjbh;
                    SysInfo.m_strStart_Time = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[0].strTime;
                    SysInfo.m_strEnd_Time = SysInfo.m_C_Item_Info.i_C_AllRows > 1 ? SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_C_AllRows - 1].strTime : "";
                }
                #endregion

                SysInfo.m_Report_Para.m_Save_FileName= SysInfo.csInter.IniReadDefine ("System", "m_Save_FileName", "", SysInfo.HardFileName);
              
            }
        }
        /// <summary>
        /// 张萌的涂层测厚：开始检测后，3秒内不能点停止
        /// </summary>
        DateTime m_dt_BeginTime;
        private void Begentest_Run(int iType)
        {
            //3 抬起探架
            if (SysInfo.m_SysBuff.m_Climb.i_Begin_Down == 1)//涂层带超声
            {
                SysInfo.m_SysBuff.m_Climb.iGsbTtLx = 1;

                SysInfo.m_Climb4.SendData(5, 1, 0, 0, SysInfo.m_SysBuff.m_Climb.iGsbTtLx.ToString());
                try
                {
                    SysInfo.m_Climb4.m_i_Down_WaitTime = int.Parse(SysInfo.csInter.IniReadDefine("SysInfo", "m_i_Down_WaitTime", "2", SysInfo.HardFileName));
                }
                catch { SysInfo.m_Climb4.m_i_Down_WaitTime = 2; }
                SysInfo.WaitTime_S(SysInfo.m_Climb4.m_i_Down_WaitTime);
                SysInfo.csInter.INIWriteValue("SysInfo", "m_i_Down_WaitTime", SysInfo.m_Climb4.m_i_Down_WaitTime.ToString(), SysInfo.HardFileName);
            }
            SysInfo.m_SysBuff_C.m_Plant_C.m_Ck_His_Wave = false;


            //       MessageBox.Show("运行位置 3");
            //if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 3 && !(SysInfo.m_SysBuff_C.m_Ctrl.m_bl_FY ||
            //     SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 && SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type == 2) ||
            //    SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 && SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have)
            //    SysInfo.m_Climb4.SendData(2, 4, 0, 0, SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval.ToString() + "," +
            //                                          SysInfo.m_SysBuff.m_Climb.i_Coat_Interval.ToString());//写光栅臂间隔、单步间距
            //                                                                                                //      MessageBox.Show("3-2");
            int _Trip_Com_mm = 0;
         

            //       MessageBox.Show("3-4");

            Start(1);



            //日期
            Txt_Date.Text = SysInfo.m_C_Item_Info.strJyrq;//DateTime.Now.ToString("yyyy-MM-dd");

            //4 车体：位置清零、运行模式
            _Trip_Com_mm = int.Parse(SysInfo.csInter.IniReadDefine("System", "Trip_Com_mm", "0", SysInfo.HardFileName));
          
            m_dt_BeginTime = DateTime.Now;

            //        MessageBox.Show("运行位置 4");
            //if (!(SysInfo.m_SysBuff_C.m_Ctrl.m_bl_FY ||
            //     SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 && SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type == 2))
                SysInfo.Set_Mode_Start(1);
        }
        /// <summary>
        /// 停止检测
        /// </summary>
        private void StopTest()
        {
            //1 停止检测
          
            //3 在线程中：绘制剩余C扫描图

            //4 在线程中：绘制当前界面整体C扫描图
            Bt_Row_Num.Text =  "......";
            SysInfo.WaitTime(100);
            SysInfo.m_SysBuff_C.m_Ctrl.m_iRun = 0;
      //      Ck_His_Wave.Visible = true;

            //5 数据保存
         
            //6 界面处理
            Start(0);
        }
        #region 数据保存
        /// <summary>
        /// 保存当前检测数据和标注数据
        /// </summary>
        private void Save_Data_C()
        {
            if (SysInfo.m_C_Item_Info.i_C_AllRows == 0)
            {
                MessageBox.Show("没有需要保存的数据。");
                return;
            }
            //Prg_Print_Bar.Visible = true;
            //Prg_Print_Bar.Value = 0;
            int iRec_Num = 0;

            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
            {
                for (int iRow = 0; iRow < SysInfo.m_C_Item_Info.i_C_AllRows; iRow++)
                    iRec_Num += SysInfo  .m_SysBuff_C.m_Lst_C_Buff[iRow].Count;
            }
            else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 ==2)
            {
                for (int iRow = 0; iRow < SysInfo.m_C_Item_Info.i_C_AllRows; iRow++)
                    iRec_Num += SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[iRow].lst_Ori_Data .Count ;
            }
            else
            {
                for (int iRow = 0; iRow < SysInfo.m_SysBuff_C.m_Lst_C_Buff.Count; iRow++)
                    iRec_Num += SysInfo.m_SysBuff_C.m_Lst_C_Buff[iRow].Count;

                for (int iRow = 0; iRow < SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat.Count; iRow++)
                    iRec_Num += SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[iRow].Count;
            }
        //    Prg_Print_Bar.Maximum = iRec_Num;
            bool _blHz = SysInfo.m_Report_Para.m_Save_FileName.IndexOf("tdf") < 0;

            string strVideoPath = SysInfo.m_Report_Para.m_Save_FilePath + SysInfo.m_Report_Para.m_Save_FileName+ (_blHz ? ("_"+ DateTime.Now.ToString("yyMMddHHmmss")):"") + (_blHz?".tdf":"");
            
            SysInfo.csInter.CreatCurrDir(SysInfo.m_W_strItemName + SysInfo.m_ItemMark);
            SysInfo.Get_UI();
       
            SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark = new CLS_C_ALL();
 
            SysInfo.m_C_Item_Info.StrGcmc_Old = "";
            SysInfo.m_C_Item_Info.i_CurrRow_No = SysInfo.m_C_Item_Info.i_C_AllRows - 1;
            SaveCurr(strVideoPath);
        }

        private void SaveCurr(string filename, int iType = 0)
        {
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_Save del_Show = new Delg_Save(FileSaveByte);
                    this.Invoke(del_Show, new object[] { filename, iType });
                }
                catch { }
            }
            else
            {
                FileSaveByte(filename, iType);
            }
        }
        /// <summary>
        /// 写检测文件
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="iType">记录长度；0：写 1：不写</param>
        /// <returns></returns>
        public void FileSaveByte(string filename, int iType = 0)
        {
           
        }
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="_TofdData"></param>
        /// <param name="iType"></param>
        /// <returns></returns>
        public int FileReadByte_C_Scan(string filename)
        {
          
            return SysInfo.m_C_Item_Info.i_C_AllRows;
        }
        /// <summary>
        /// 读取数据时同一位置数据置换
        /// </summary>
        /// <param name="iAll_Num"></param>
        /// <param name="_iArr_Len"></param>
        /// <param name="_Arr_In"></param>
        /// <param name="Arr_Emat"></param>
        private void Mody_Mul_CurrData(int iAll_Num, int _iArr_Len, Cls_EMAT_1[] _Arr_In,  ref Cls_EMAT_1[] Arr_Emat)
        {
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_In[0].btArrWave , 0);

            for (int _iR = 0; _iR < iAll_Num; _iR++)//多通道序号
            {
                Arr_Emat[_iR].blUse = _Arr_In[_iR].blUse;
                //如果当前通道使用
                if (_Arr_In[_iR].blUse == false) continue;

                Arr_Emat[_iR].flThick = _Arr_In[_iR].flThick ;
                Arr_Emat[_iR].iGain = _Arr_In[_iR].iGain;//通道增益值
                Arr_Emat[_iR].blAlarm = _Arr_In[_iR].blAlarm;//报警

                Arr_Emat[_iR].strColor = _Arr_In[_iR].strColor;//颜色值
                Arr_Emat[_iR].R = _Arr_In[_iR].R;
                Arr_Emat[_iR].G = _Arr_In[_iR].G;
                Arr_Emat[_iR].B = _Arr_In[_iR].B;
                IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(Arr_Emat[_iR].btArrWave, 0);
                Marshal.Copy(_Arr_In[_iR].btArrWave , 0, IntPtArr, _iArr_Len);//注意数据范围

             
            }
        }

        #endregion 数据保存
        private void Start(int iType = 0)
        {
            if (iType == 1)
            {
                bt_Start_T.Text =SysInfo .m_iLanguage ==0? "正在检测": "Be testing";
                bt_Start_T.ForeColor = Color.Yellow;

           //     Rad_1.Visible = false;
                //Rad_2.Visible = false;
                //Rad_0.Visible = false;
                //Bt_FindData.Enabled = false;
                //Bt_Sys_OSK.Enabled = false;
                //Bt_Stop.Enabled = true;
                //Bt_Data_Left.Visible = false;
                //Bt_Data_Right.Visible = false;
                //Bt_Start.Enabled = false;
            }
            else
            {
                Bt_Start.Enabled = true;
                bt_Start_T.Text =SysInfo .m_iLanguage ==0? "开始检测" : "Start detect.";
                bt_Start_T.ForeColor = Color.White;

       //         Rad_1.Visible = true;
       //         Rad_2.Visible = true;
       ////         Rad_0.Visible = true;

       //         Bt_FindData.Enabled = true;
       //         Bt_Sys_OSK.Enabled = true;
       //         Bt_Stop.Enabled = false;
       //         Bt_Data_Left.Visible = true;
       //         Bt_Data_Right.Visible = true;
       //         Bt_Start.Enabled = true;
            }
        }
        private void Bt_Stop_Click(object sender, EventArgs e)
        {
            if (DateTime.Now.Subtract(m_dt_BeginTime).TotalSeconds < 4) return;

            
        }

        private void Frm_Main_C_FormClosing(object sender, FormClosingEventArgs e)
        {
        //    if (SysInfo.m_bl_Show_Video) m_frm_Video.Close();
            if (MessageBox.Show(SysInfo .m_iLanguage ==0? "确定退出软件吗？": "Are you sure you want to quit the software?", SysInfo.m_iLanguage == 0 ? "提示信息": "Prompt message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SysInfo.Init(1);
                SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun = 10;
                showtask();

                SysInfo.g_Msg_InterFace.Inter_Photo -= new ClassLib_TestData.MsgInterFace.OnData_Photo(Bt_To_Phone);
                SysInfo.g_Msg_InterFace.Inter_Video -= new ClassLib_TestData.MsgInterFace.OnData_Video(Bt_To_Video);

                SysInfo.g_Msg_InterFace.Inter_ModyColor -= new ClassLib_TestData.MsgInterFace.OnMody_Color(ModyColor);
           //     k_hook.KeyDownEvent -= new System.Windows.Forms.KeyEventHandler(hook_KeyDown);//钩住键按下 
               
                SysInfo.g_Msg_InterFace.Inter_Tofd_Para -= new MsgInterFace.OnTofd_Para(Para_Cg);
            //    SysInfo.g_Msg_InterFace.Inter_GetEtcReceive -= new MsgInterFace.OnETC_GetEtcReceive(Show_Get_ETC_Data);

                SysInfo.g_Msg_InterFace.Inter_ShowCoat_C -= new ClassLib_TestData.MsgInterFace.OnShow_Coat_C(Show_Coat_C);
                SysInfo.g_Msg_InterFace.Inter_Run_C -= new ClassLib_TestData.MsgInterFace.OnRun_C(Begentest_Run);

                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2) SysInfo.m_Server_MulDistan.Close();
             
                if (SysInfo.m_blXunJi_Prog_0PC_1YY) SysInfo.m_Client.Close();
                SysInfo.m_Climb4.CloseSet();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void Ck_Show_V_CheckedChanged(object sender, EventArgs e)
        {
            //  if (Ck_Show_V.Checked)
            {
              
            }
        }

        private void Bt_Video_Click(object sender, EventArgs e)
        {
           Link_V();
        }
        /// <summary>
        /// 视频连接
        /// </summary>
        private void Link_V()
        {
            if (int.Parse(SysInfo.csInter.IniReadDefine("Video", "Link_V", "1", SysInfo.HardFileName)) == 1)
            {
                if (SysInfo.m_bl_Show_Video == false)
                {
                    try
                    {
                     //   m_frm_Video = new Frm_Video();
                     //   if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 && SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum == 1)
                     //   {
                     //       m_frm_Video.Left = tableLayoutPanel1.Width / 2 + 1;
                     //       m_frm_Video.Height = tableLayoutPanel1.Height + 20;
                     //       // m_frm_Video.Top =  Pan_Titl.Height+ 10;
                     //       m_frm_Video.Width = tableLayoutPanel1.Width / 2 + 10;
                     //   }
                     //   if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 && SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum == 1)
                     //   {
                     ////       m_frm_Video.Left = 0;// (int)( tableLayoutPanel1.Width *(0.44f-0.08)) + 1 ;
                     //       m_frm_Video.Height = tableLayoutPanel1.Height + 20;
                     //       // m_frm_Video.Top =  Pan_Titl.Height+ 10;
                     //       m_frm_Video.Width = (int)(tableLayoutPanel1.Width * 0.48f) + 10;
                     //   }
                     //   m_frm_Video.Show();
                     //   if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 && SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum == 1)
                     //       m_frm_Video.Left = (int)(tableLayoutPanel1.Width * (0.44f )) + 1;
                  //      m_frm_Video.Top = Pan_Titl.Height + 10;
                    }
                    catch (Exception e1)
                    { }
                }
            }
        }
        private void Bt_Sys_OSK_Click(object sender, EventArgs e)
        {
            Exit_0();
        }
        private void Exit_0()
        {
            Bt_Sys_OSK.Enabled = false;
            if (Exit_Prg() == false)
                Bt_Sys_OSK.Enabled = true;
        }
        /// <summary>
        /// 关机
        /// </summary>
        /// <param name="e"></param>
        /// <returns>退出结果 true:成功  false:失败</returns>
        private bool Exit_Prg()
        {
            bool blRet = false;
            bool _blOk = true;

            //1 判断窗体是否都关闭
            for (int i = 0; i < SysInfo.m_blFrmOpen.Length; i++)
                if (SysInfo.m_blFrmOpen[i]) { _blOk = false; break; }

            if (_blOk == false)//&& MessageBox.Show("有窗体没有关闭，确定关闭软件？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                Bt_Sys_OSK.Enabled = true;
                return blRet;
            }
            //2 咨询是否退出
          //  if (SysInfo.m_bl_Show_Video) m_frm_Video.Close();
            
            if (MessageBox.Show(SysInfo .m_iLanguage ==0? "确定退出软件吗？": "Are you sure you want to quit the software?", SysInfo.m_iLanguage == 0 ? "提示信息": "Prompt message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                Bt_Sys_OSK.Enabled = true;
                //m_frm_Video = new Frm_Video();
                //m_frm_Video.Show();
                return blRet;
            }

            //3 系统参数关闭
            //关闭视频界面

            blRet = true;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun = 10;
            showtask();
            SysInfo.Init(1);
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 )//武汉中科压电探头，保存闸门位置
            {
                SysInfo.csInter.INIWriteValue("C_Scan_Whzk", "m_Data_Type", SysInfo.m_SysBuff.m_Tofd_DLL.m_Data_Type.ToString(), SysInfo.HardFileName);

                SysInfo.csInter.INIWriteValue("C_Scan_Whzk", "1_i_S_X", SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_S_X.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("C_Scan_Whzk", "1_i_E_X", SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_E_X.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("C_Scan_Whzk", "1_i_Y", SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_Y.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("C_Scan_Whzk", "2_i_S_X", SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_S_X.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("C_Scan_Whzk", "2_i_E_X", SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_E_X.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("C_Scan_Whzk", "2_i_Y", SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_Y.ToString(), SysInfo.HardFileName);

                SysInfo.csInter.INIWriteValue("C_Scan_Whzk", "1_i_Gate_S", SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_Gate_S.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("C_Scan_Whzk", "1_i_Gate_E", SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_1.i_Gate_E.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("C_Scan_Whzk", "2_i_Gate_S", SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_Gate_S.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("C_Scan_Whzk", "2_i_Gate_E", SysInfo.m_SysBuff.m_Tofd_DLL.m_Gate_2.i_Gate_E.ToString(), SysInfo.HardFileName);

            }
            #region 4 仪器关闭
            SysInfo.m_ServerUI.SendData("", "99");//关闭相机程序

         
            #endregion 4

            //5 关闭
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            return blRet;
        }
        /// <summary>
        /// 显示windows的任务栏
        /// </summary>
        public static void showtask()
        {
            ShowWindow(FindWindow("Shell_TrayWnd", null), SW_RESTORE);
        }

        private void Bt_Move_Click(object sender, EventArgs e)
        {
            Bt_Move_0();
        }
        private void Bt_Move_0()
        {
            if (SysInfo.m_blFrmOpen[3] == false)
            {
                m_frm_Move = new From.Frm_Move();
                m_frm_Move.Show();
            }
        }

        private void Bt_Tofd_Click(object sender, EventArgs e)
        {
            Bt_Tofd_0();
        }
        private void Bt_Tofd_0()
        {
            if (SysInfo.m_blFrmOpen[0] == false)
            {
                SysInfo.m_i_TOFD_0_Cscan_1_Mui_2_Old = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2;
                m_frm_Tofd = new From.Frm_TOFD();
                m_frm_Tofd.Show();
            }
        }

        private void Rad_2_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void Ask_R1_L0()
        {
            string _strOk = "", _strNo = "";
            if (SysInfo.m_iLanguage == 0)
            {
                _strOk = (SysInfo.m_SysBuff.m_Climb.i_R1_L0 == 1 ? "右侧" : "左侧");
                _strNo = (SysInfo.m_SysBuff.m_Climb.i_R1_L0 == 1 ? "左侧" : "右侧");
            }
            else
            {
                _strOk = (SysInfo.m_SysBuff.m_Climb.i_R1_L0 == 1 ? "Right side" : "left side");
                _strNo = (SysInfo.m_SysBuff.m_Climb.i_R1_L0 == 1 ? "left side" : "Right side");
            }

            string _strMsg = (SysInfo.m_iLanguage == 0 ? "未检区域在车体: " : "The uninspected area is in the car body: ") + _strOk + "\r\n\r\n";
            _strMsg += _strOk + ": Yes   " + _strNo + ": No";

            DialogResult dr = MessageBox.Show(_strMsg, (SysInfo.m_iLanguage == 0 ? "图形拼接位置确认" : "Confirm the graphics stitching position"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            SysInfo.m_SysBuff.m_Climb.i_R1_L0 = 1;
            if (dr == DialogResult.No)
                SysInfo.m_SysBuff.m_Climb.i_R1_L0 = 0;// SysInfo.m_SysBuff.m_Climb.i_R1_L0 == 1 ? 0 : 1;
        }

        private void Rad_1_CheckedChanged(object sender, EventArgs e)
        {

        }
        bool m_bl_Run_StopState = false ;
        delegate void Delg_Run();
        private void ShowStopText()
        {
            if (m_bl_Run_StopState == false)
            {
                m_bl_Run_StopState = true;
                Bt_Urgent.Text = "继 续";

                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                {
                    SysInfo.m_Climb4.SendData(1, 1, 3, 0, "10");// 自动模式暂停(车体自己和遥控型号都能让车暂停)
                }
                else
                {
                    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1|| SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 )
                        SysInfo.m_Climb4.SendData(1, 1, 3, 0, "2");// 自动模式关闭
                    else
                        SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");//停止运动
                }

            }
            else
            {
                m_bl_Run_StopState = false;
                Bt_Urgent.Text = "急 停";
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                {
                    SysInfo.m_Climb4.SendData(1, 1, 3, 0, "11");//自动模式(暂停后)继续
                }
                else
                {
                    SysInfo.Set_Mode_Start(1);
                }

            }
        }
        private void Bt_UrgentStop_Click(object sender, EventArgs e)
        {
            JinJiTingChe();
        }

        private void JinJiTingChe()
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;

            if (SysInfo  .m_SysBuff_C.m_Ctrl.m_iRun == 1)
            {

                if (this.InvokeRequired == true)
                {
                    try
                    {
                        Delg_Run ms = new Delg_Run(ShowStopText);
                        this.Invoke(ms, new object[] { });
                    }
                    catch
                    { }
                }
                else
                {
                    ShowStopText();
                }


            }
            else
            {
                SysInfo.Set_Mode_Start(0);
            }
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
        }
        /// <summary>
        /// 数据查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_FindData_Click(object sender, EventArgs e)
        {
            ChaXun();
        }
        private void ChaXun()
        {
         
        }
        private void Qurey_0()
        {
        }
        Thread m_Thread_CoatText;
        private void Show_Text_Thread()
        {
            if (m_Thread_CoatText != null) m_Thread_CoatText.Abort();
            m_Thread_CoatText = new Thread(new ThreadStart(Show_Text_Coat));
            m_Thread_CoatText.Name = "m_Thread_CoatText";
            m_Thread_CoatText.IsBackground = true;
            m_Thread_CoatText.Start();
        }
        private void Show_Text_Coat()
        {
        
        }
        /// <summary>
        /// C扫描界面是否调用完成
        /// </summary>
        bool m_bl_C_All_Active = false;
        private void Rad_2_Click(object sender, EventArgs e)
        {
            Rad_Visue(1);
        }

        private void Rad_1_Click(object sender, EventArgs e)
        {
            Rad_Visue(0);
        }
        private void Thread_PrintState()
        {
            //  Time_UI_ReLink.Enabled = false;
            if (Thread_ShowRunSate != null) Thread_ShowRunSate.Abort();
            Thread_ShowRunSate = new Thread(new ThreadStart(ShowPrintState));
            // Thread_ShowRunSate.Priority = ThreadPriority.AboveNormal;
            Thread_ShowRunSate.Name = "Thread_ReadUI";
            Thread_ShowRunSate.IsBackground = true;
            Thread_ShowRunSate.Start();
        }
        /// <summary>
        /// 显示打印运行进度
        /// </summary>
        private void ShowPrintState()
        {
          
        }
      
      


        /// <summary>
        /// 报表输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rad_3_Click(object sender, EventArgs e)
        {
           
            Bt_FindData.Enabled = false;
            Bt_Start.Enabled = false;
            if (SysInfo.m_i_ReportType == 2)
            {
                try
                {
                  
                }
                catch { }
                Bt_Start.Enabled = true;
                Bt_FindData.Enabled = true;

              
                return;
            }
           
               
        }
        int m_i_Alarm_AllNum = 0;
        /// <summary>
        /// 将缺陷参与统计标记设置为初始
        /// </summary>
        private void Tj_Int()
        {
            m_i_Alarm_AllNum = 0;
            for (int _iR = 0; _iR < SysInfo  .m_SysBuff_C.m_Lst_Alarm_Buff.Count; _iR++)// SysInfo  .m_SysBuff_C.m_Lst_C_Buff.Count; _iR++)
            {
                //       List<CLs_EMAT_Data> _lstRowRecord = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[_iR];

                //3.2 计算当前行当前屏幕的开始/结束位置
                List<CL_AlarmData> _One_Alarm_Buff = SysInfo  .m_SysBuff_C.m_Lst_Alarm_Buff[_iR];
                for (int _iCol = 0; _iCol < _One_Alarm_Buff.Count; _iCol++)
                {
                    _One_Alarm_Buff[_iCol].bl_Use = false;
                    m_i_Alarm_AllNum++;
                }
            }
        }
        #region 整体C扫描事件
        private void Rad_Visue(int iA0_C1)
        {
            if (iA0_C1 == 0)
            {
            //    if (m_bl_Pic_A_B_Plant_1)
                   
                Pan_A.Visible = true;
                //Pan_C.Visible = false;
                //Rad_3.Visible = false;
                m_bl_A1_C0 = true;
                m_bl_Pic_A_B_Plant = false; 
                
                SysInfo.WaitTime(100f);
                Show_C(); 
                m_bl_C_All_Active = false ;
            }
            else
            {
                m_bl_Pic_A_B_Plant = true;
                m_bl_A1_C0 = false;
              
                Pan_A.Visible = false;
                //Pan_C.Visible = true;
                //Pic_C_All.Visible = true;
                //Pan_C.Dock = DockStyle.Fill;
                //Pic_Coat_All .Dock = DockStyle.Fill;
                //1 是否已经绘制
                if (SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark != null)
                {
                    //1 决定未检区域在车体哪一侧
                    Ask_R1_L0();
                    
                    Init_C_Plant();
              //      Rad_3.Visible = true;
                    GetMax_ScreenNo();
                }
                else
                {
                    MessageBox.Show("没有检测数据。");
                }
                Show_C(1);
              //  SetTran(Pic_C_All, Pic_3);
                m_bl_C_All_Active = true ;
              
            }
        }
        /// <summary>
        /// 显示C扫描界面
        /// </summary>
        private void Init_C_Plant()
        {
            //1 参数准备
            SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.bl_R1_L0 = SysInfo.m_SysBuff.m_Climb.i_R1_L0 == 1;
            SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No = 0;
            SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start = 1;

            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
            {
                SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End = SysInfo.m_C_Item_Info.i_C_AllRows >
                                             SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_ScreenRows ?
                                             SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_ScreenRows : SysInfo.m_C_Item_Info.i_C_AllRows;
                //2 画图
                if (SysInfo.m_SysBuff_C.m_Lst_C_Buff.Count > 0)
                    SysInfo.m_SysBuff_C.m_Plant_C.Plant_C_ALL(SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.bl_R1_L0,
                                                               SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No,
                                                               SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start,
                                                               SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End,
                                                               SysInfo.m_SysBuff_C.m_Lst_C_Buff);

                if (SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat.Count > 0)
                {
                    SysInfo.m_SysBuff_C.m_Plant_C.Fd_All_Clear();
               //     Show_E0_C1(SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have? ( Ck_E.Checked ? 0 : 1):1);
                    Application.DoEvents();
                    SysInfo.m_SysBuff_C.m_Plant_C.Plant_C_ALL_Coat(SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.bl_R1_L0,
                                                                       SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No,
                                                                       SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start,
                                                                       SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End,
                                                                       SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat);
                }
            }
            else
            {
                //屏幕总行数
                SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End = SysInfo.m_C_Item_Info.i_C_AllRows;// * SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum;

                //2 画图
                SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_Row_Start = 1;
                //SysInfo.m_SysBuff_C.m_Plant_C.Plant_C_ALL_Mul(SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.bl_R1_L0,
                //                                           SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No,
                //                                           SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start,
                //                                           SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End,
                //                                           SysInfo.m_SysBuff_C.m_Lst_Mul_All_Buff,

                //                                           SysInfo.m_Tofd_C_Scan.m_i_UI_DLL_Type == 2 ? ECT_DLL.Cl_ECT.m_iRomoteNum :
                //                                           SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum);
            }
        }
        /// <summary>
        /// 最大屏幕序号
        /// </summary>
        int m_iMax_ScreenNo = 0;
        /// <summary>
        /// 计算当前页的结束屏幕序号
        /// </summary>
        /// <param name="iPage"></param>
        private void GetMax_ScreenNo(int iPage = 0)
        {
            int _iMax_X = -1;
            m_iMax_ScreenNo = -1;
            int _iColAll = 0;

            int _iStartRow = iPage * SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_ScreenRows;
            int _iEndRow = (iPage + 1) * SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_ScreenRows;

       

            if (_iMax_X > 0)
                m_iMax_ScreenNo = SysInfo.m_SysBuff_C.m_Plant_C.Juge_ScreenNo_C(_iMax_X);
        }
        private void GetMax_ScreenNo_Mul(int iPage = 0)
        {
            int _iMax_X = -1;
            m_iMax_ScreenNo = -1;
            int _iColAll = 0, _iCol_Start=0, _iCol_End=0,_iDist=0;

            int _iStartRow = iPage * SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_ScreenRows;
            int _iEndRow = (iPage + 1) * SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_ScreenRows;
            if (_iEndRow >= SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff.Count) _iEndRow = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff.Count;
            for (int _iRow = _iStartRow; _iRow < _iEndRow; _iRow++)
            {
                Cls_Mul_UI_Data _lstRowRecord = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[_iRow];

                //3.1拿当前行数据
                bool _blDd = false;
                int _iLen = _lstRowRecord.lst_Show_Buff.Count - 1;
                if (_iLen > 2)
                {
                    _iDist = _lstRowRecord.lst_Show_Buff[_iLen - 2].m_i_X;
                    _blDd = _lstRowRecord.lst_Show_Buff[1].m_i_X > _lstRowRecord.lst_Show_Buff[_iLen - 2].m_i_X;
                    if (_blDd)
                        _iDist = _lstRowRecord.lst_Show_Buff[1].m_i_X;
                }
    
                if (_iMax_X < _iDist) _iMax_X = _iDist;
            }
            if (_iMax_X > 0)
                m_iMax_ScreenNo = SysInfo  .m_SysBuff_C.m_Plant_C.Juge_ScreenNo_C(_iMax_X);

        }
        private void Pic_C_All_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (m_bl_C_All_Active == false) return;
                Zoom_Coat(e);
                if (SysInfo.m_SysBuff_C.m_Ctrl.m_iRun == 0 && SysInfo.m_C_Item_Info.i_C_AllRows < 1) return;
             //   Lab_C_All.Text = "";
                //1 X轴序号
                float fl = (float)(e.X - SysInfo.m_SysBuff_C.m_Plant_C.Chart_Ruler_X_Start_C_All + 0) / SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWith_X;
                if (fl < 0) return;
                int _i_X_No = (int)fl;
                m_i_X_No = _i_X_No;
                if (fl - m_i_X_No >= 0) m_i_X_No++;
                m_i_X_No--;

                if (_i_X_No < 0) return;
                if (SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No < 0) return;
                if (SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd_C_ALL.Count == 0) return;

                int _i_Dist_S = SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd_C_ALL[SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No].i_Start;
                int _i_Curr_Dist = _i_Dist_S + _i_X_No * SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
                if (_i_Curr_Dist >= SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd_C_ALL[SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No].i_End) return;

                fl = _i_Curr_Dist / SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
                _i_X_No = (int)fl;
                if (fl - _i_X_No >= 0.5) _i_X_No++;

                //2 Y轴序号
                fl = (e.Y - SysInfo.m_SysBuff_C.m_Plant_C.Chart_Ruler_Y_Start - 1) / SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.Scree_flDotHeight;
                int _iY = (int)fl;
                string _strT = "";

                #region 显示界面缓存

                if (_iY > -1 && _iY < SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl.Count)
                {
                    if (m_i_X_No < SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl[_iY].lst_One_Row_Data.Count)
                    {
                        Cls_One_Data _One = SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl[_iY].lst_One_Row_Data[m_i_X_No];
                        if (_One.i_X == -1)
                            return;
                        if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
                        {
                            if (SysInfo.m_iLanguage == 0)
                                _strT = "X=" + _One.i_X + "mm " +
                                         " " + _One.strGdbh + " 行:" + _One.i_ABC.ToString() +
                                         " 厚度:" + _One.flThick.ToString("f2") + "mm" + "  Erro:" + _One.flWc;// + "\r\n" +
                            else
                                _strT = "X=" + _One.i_X + "mm " +
                                         " " + _One.strGdbh + " Y:" + _One.i_ABC.ToString() +
                                         " Thick.:" + _One.flThick.ToString("f2") + "mm" + "  Erro:" + _One.flWc;// + "\r\n" +

                        }
                        else
                        {

                            _strT = "X=" + _One.i_X + "mm " +
                                 " " + _One.strGdbh + " 通道:" + _One.i_ABC.ToString() +
                                 " 厚度:" + _One.flThick.ToString("f2") + "mm" + "  Erro:" + _One.flWc;// + "\r\n" +


                        }
                    }
                }
                #endregion

           //     Lab_Titl(Pic_C_All, Lab_C_All, _strT, e);

            }
            catch (Exception e3)
            { }
        }
        private void Zoom_Coat(MouseEventArgs e)
        {
       //     if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 3) return;
        
        }
        /// <summary>
        /// 整体C扫描图翻页
        /// </summary>
        /// <param name="iType">0：左翻页 1：右翻页 2：上翻页 3：下翻页</param>
        private void Page_C_ALL(int iType)
        {
            bool _blOk = false;
            int _iRow_S = 0, _iT = 0;
            if (SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.bl_R1_L0 == false)
            {
                if (iType == 2)
                {
                    iType = 3;
                }
                else
                {
                    if (iType == 3)
                        iType = 2;
                }
            }
            switch (iType)
            {
                case 0://左翻页
                    if (SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No > 0)
                        SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No--;
                    _blOk = true;
                    break;
                case 1://右翻页
                    if (SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No + 1 < SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd_C_ALL.Count)
                    {
                        SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No++;
                        _blOk = true;
                    }
                    break;
                case 2://上翻页
                    _iRow_S = SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start;
                    if (_iRow_S > SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_ScreenRows)
                    {
                        _iRow_S -= SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_ScreenRows;

                        SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start = _iRow_S;

                        SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End = SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start +
                            SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_ScreenRows - 1;
                        _blOk = true;
                    }
                    break;
                case 3://下翻页
                    _iRow_S = SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End;
                    if (_iRow_S < SysInfo.m_C_Item_Info.i_C_AllRows)//后续还有数据
                    {
                        SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start = ++_iRow_S;//开始序号
                        _iT = SysInfo.m_C_Item_Info.i_C_AllRows - _iRow_S;//实际记录个数  - 开始序号
                        if (_iT >= 0)
                        {
                            if (_iT == 0)//只有一个
                            {
                                SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End = SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start;
                            }
                            else if (_iT > 0)//有多个
                            {
                                if (_iT < SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_ScreenRows)//一屏幕数量
                                    SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End = SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start + _iT;
                                else//超过一屏幕
                                    SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End = SysInfo  .m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start + 3;
                            }
                        }
                        _blOk = true;
                    }
                    break;
            }
            if (_blOk)
            {
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
                    SysInfo.m_SysBuff_C.m_Plant_C.Plant_C_ALL(SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.bl_R1_L0,
                                               SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No,
                                               SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start,
                                               SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End,
                                               SysInfo.m_SysBuff_C.m_Lst_C_Buff);
                else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                {
                   if (SysInfo.m_SysBuff_C.m_Lst_C_Buff.Count >0)
                        SysInfo.m_SysBuff_C.m_Plant_C.Plant_C_ALL(SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.bl_R1_L0,
                                              SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No,
                                              SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start,
                                              SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End,
                                              SysInfo.m_SysBuff_C.m_Lst_C_Buff);
                    //else
                    if (SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat.Count > 0)
                    {
                        SysInfo.m_SysBuff_C.m_Plant_C.Fd_All_Clear();
                        SysInfo.m_SysBuff_C.m_Plant_C.Plant_C_ALL_Coat(SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.bl_R1_L0,
                                                   SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No,
                                                   SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_Start,
                                                   SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Row_End,
                                                   SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat);
                    }
                }
                          }
            
        }


        private void Bt_C_All_Up_Click(object sender, EventArgs e)
        {
            Page_C_ALL(2);
        }

        private void Bt_C_All_Left_Click(object sender, EventArgs e)
        {
            Page_C_ALL(0);
        }

        private void Bt_C_All_Down_Click(object sender, EventArgs e)
        {
            Page_C_ALL(3);
        }

        private void Bt_C_All_Right_Click(object sender, EventArgs e)
        {
            Page_C_ALL(1);
        }

       #endregion 整体C扫描
        /// <summary>
        /// 查询状态
        /// </summary>
        bool m_bl_Cx = false;
        private void Bt_Data_Left_Click(object sender, EventArgs e)
        { 
            Page_L0_R1(0);
           
        }
        /// <summary>
        /// 左右翻页 0：左  1：右
        /// </summary>
        /// <param name="iType"></param>
        private void Page_L0_R1(int iType = 0)
        {
            m_bl_Cx = true;

            int _iT = 0;
            int _iDist_CurrRow_End = 0;
            m_i_AllNum_Dd = 0;
            int _i_Dist_S = 0, _i_Dist_E = 0;//开始结束距离
            bool _blOk = false;

            //1 左右翻页决定屏幕计算序号
            if (iType == 0)
            {
                #region 左边翻页
                // 当前行  当前屏幕序号对应的开始结束位置，计算：左侧屏幕序号对应开始/结束位置  或  上一行位置对应屏幕序号对应开始/结束位置
                if (SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_No >= 0)
                {
                    _iT = SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_No - 1;

                    if (_iT < 0)//拿上一行
                    {
                        _iT = SysInfo.m_C_Item_Info.i_CurrRow_No - 1;//减行
                        if (_iT >= 0)//还有上一行
                        {
                            //行号确认
                            int _iRow_No = _iT;//    
                            //由上行末尾值确定屏幕序号
                          
                            
                            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
                                _iT = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[_iRow_No].Count - 1;//末尾序号
                            else  if ( SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                                _iT = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[_iRow_No].Count - 1;
                            else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)
                                _iT = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[_iRow_No].lst_Ori_Data.Count - 1;
                          
                            if(_iT <0)
                            {
                                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                                {
                                    while (SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[_iRow_No].Count == 0)//cdw 7-2
                                        _iRow_No--;
                                }
                                else
                                    while (SysInfo.m_SysBuff_C.m_Lst_C_Buff[_iRow_No].Count == 0)//cdw 7-2
                                        _iRow_No--;


                                if (_iRow_No > -1)
                                {
                                    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
                                        _iT = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[_iRow_No].Count - 1;//末尾序号
                                    else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                                        _iT = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[_iRow_No].Count - 1;//末尾序号
                                    else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)
                                        _iT = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[_iRow_No].lst_Ori_Data.Count - 1;
                                }
                            }

                            if (_iT > -1)
                            {
                                int _i_S = 0, _i_E = 0;
                                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
                                {
                                    _iDist_CurrRow_End = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[_iRow_No][_iT].i_X_mm;//行尾部距离

                                    _i_S = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[_iRow_No][0].i_X_mm;
                                    _i_E = _iDist_CurrRow_End;
                                    if (_i_S > _iDist_CurrRow_End)
                                    {
                                        _iDist_CurrRow_End = _i_S;
                                        _i_S = _i_E;
                                        m_i_AllNum_Dd = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[_iRow_No].Count - 1;
                                    }
                                }
                                else if ( SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                                {
                                    _iDist_CurrRow_End = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[_iRow_No][_iT].i_X_mm;//行尾部距离

                                    _i_S = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[_iRow_No][0].i_X_mm;
                                    _i_E = _iDist_CurrRow_End;
                                    if (_i_S > _iDist_CurrRow_End)
                                    {
                                        _iDist_CurrRow_End = _i_S;
                                        _i_S = _i_E;
                                        m_i_AllNum_Dd = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[_iRow_No].Count - 1;
                                    }
                                }
                                else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)
                                {
                                    _iDist_CurrRow_End = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[_iRow_No].lst_Ori_Data[_iT].i_X_mm;
                                     _i_S = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[_iRow_No].lst_Ori_Data[0].i_X_mm;
                                     _i_E = _iDist_CurrRow_End;
                                    if (_i_S > _iDist_CurrRow_End)
                                    {
                                        _iDist_CurrRow_End = _i_S;  
                                        _i_S = _i_E;

                                        m_i_AllNum_Dd = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[_iRow_No].lst_Ori_Data.Count - 1;
                                    }
                                }
                                _iT = SysInfo  .m_SysBuff_C.m_Plant_C.Juge_ScreenNo_C(_iDist_CurrRow_End);//由尾部距离确定屏幕序号
                                if (_iT >= 0)
                                {
                                    SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_No = _iT;
                                    SysInfo.m_C_Item_Info.i_CurrRow_No = _iRow_No;
                                    //2 确定开始结束距离
                                    _i_Dist_S = SysInfo  .m_SysBuff_C.m_Plant_C.lst_Screenkd[_iT].i_Start;
                                    _i_Dist_E = _iDist_CurrRow_End + SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;//尾部距离落在其屏幕距离中

                                    if (_i_S > _i_Dist_S && _i_S< _i_Dist_E)
                                        _i_Dist_S = _i_S;
                                    if (_iDist_CurrRow_End > _i_Dist_S && _iDist_CurrRow_End <_i_Dist_E)
                                        _i_Dist_E = _iDist_CurrRow_End;

                                    _blOk = true;
                                }
                            }
                        }
                    }
                    else//同一行数据
                    {
                        _i_Dist_S = SysInfo  .m_SysBuff_C.m_Plant_C.lst_Screenkd[_iT].i_Start;
                        _i_Dist_E = SysInfo  .m_SysBuff_C.m_Plant_C.lst_Screenkd[_iT].i_End;
                        SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_No--;
                        _blOk = true;
                    }
                }
                #endregion 左边翻页
            }
            else
            {
                //2 右侧翻页：本行右侧屏幕有数据  有下一行的数据     
                _iT = SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_No + 1;
                if (_iT < SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd.Count )
                    _i_Dist_S = SysInfo  .m_SysBuff_C.m_Plant_C.lst_Screenkd[_iT].i_Start;//下一屏幕开始距离
                else
                    
                   return;
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 )
                {
                    _iT = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].Count - 1;//末尾序号
                    _iDist_CurrRow_End = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No][_iT].i_X_mm;//行尾部距离
                    int _i_S = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No][0].i_X_mm;
                    int _iMax = _i_S > _iDist_CurrRow_End ? _i_S : _iDist_CurrRow_End;

                    if (_i_Dist_S < _iMax)//判断右侧是否还有数据
                    {
                        if (_i_S > _iDist_CurrRow_End)
                        {
                            _i_Dist_S = _iDist_CurrRow_End;
                            _iDist_CurrRow_End = _i_S;
                            _i_Dist_E = _i_S;
                            m_i_AllNum_Dd = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].Count - 1;
                        }
                    }
                }
                else if ( SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                {
                    _iT = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No].Count - 1;//末尾序号
                    _iDist_CurrRow_End = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No][_iT].i_X_mm;//行尾部距离
                    int _i_S = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No][0].i_X_mm;
                    int _iMax = _i_S > _iDist_CurrRow_End ? _i_S : _iDist_CurrRow_End;

                    if (_i_Dist_S < _iMax)//判断右侧是否还有数据
                    {
                        if (_i_S > _iDist_CurrRow_End)
                        {
                            _i_Dist_S = _iDist_CurrRow_End;
                            _iDist_CurrRow_End = _i_S;
                            _i_Dist_E = _i_S;
                            m_i_AllNum_Dd = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No].Count - 1;
                        }
                    }
                }
                else if(SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)
                {
                    if (SysInfo.m_C_Item_Info.i_CurrRow_No > -1 && SysInfo.m_C_Item_Info.i_CurrRow_No < SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff.Count)
                    {
                        _iT = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Ori_Data.Count - 1;
                        _iDist_CurrRow_End = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Ori_Data[_iT].i_X_mm;

                        int _iStat = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Ori_Data[0].i_X_mm;

                        if (_iStat > _iDist_CurrRow_End)
                        {
                            int _i_T = _iDist_CurrRow_End;
                            _iDist_CurrRow_End = _iStat;

                            m_i_AllNum_Dd = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Ori_Data.Count - 1;
                        }
                    }
                    else
                        return;
                }
                if (_i_Dist_S <= _iDist_CurrRow_End)//2.1 本行右侧有数据
                {
                    _iT = ++SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_No;

                    _i_Dist_S = SysInfo  .m_SysBuff_C.m_Plant_C.lst_Screenkd[_iT].i_Start;
                    _i_Dist_E = SysInfo  .m_SysBuff_C.m_Plant_C.lst_Screenkd[_iT].i_End;

                    if (_iDist_CurrRow_End < _i_Dist_E)
                        _i_Dist_E = _iDist_CurrRow_End + SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
                    _blOk = true;
                }
                else//本行的右侧没有数据了
                {
                    _iT = SysInfo.m_C_Item_Info.i_CurrRow_No + 1;//下一行号
                    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
                    {
                        #region 
                        if (_iT < SysInfo  .m_SysBuff_C.m_Lst_C_Buff.Count())
                        {
                            //还有下一行,确定行号
                            SysInfo.m_C_Item_Info.i_CurrRow_No++;
                            //确定屏幕序号
                            _iT = 0;
                            SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_No = 0;
                            //确定距离
                            _i_Dist_S = SysInfo  .m_SysBuff_C.m_Plant_C.lst_Screenkd[_iT].i_Start;
                            _i_Dist_E = SysInfo  .m_SysBuff_C.m_Plant_C.lst_Screenkd[_iT].i_End;

                            int _iCurrRow = SysInfo.m_C_Item_Info.i_CurrRow_No;

                            while (SysInfo  .m_SysBuff_C.m_Lst_C_Buff[_iCurrRow].Count == 0)//cdw 7-2
                                _iCurrRow++;
                            if (_iCurrRow > -1)
                                SysInfo.m_C_Item_Info.i_CurrRow_No = _iCurrRow;
                            else
                                return;

                            _iT = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].Count - 1;//末尾序号
                            
                            _iDist_CurrRow_End = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No][_iT].i_X_mm;//行尾部距离

                            int _i_S = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No][0].i_X_mm;
                           
                            if (_i_S > _iDist_CurrRow_End)
                            {
                                _i_Dist_S = _iDist_CurrRow_End;
                                _iDist_CurrRow_End = _i_S;
                                m_i_AllNum_Dd = SysInfo  .m_SysBuff_C.m_Lst_C_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].Count-1;
                            }

                            if (_iDist_CurrRow_End <= _i_Dist_E)
                                _i_Dist_E = _iDist_CurrRow_End + SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
                            _blOk = true;
                        }
                        #endregion 
                    }

                    else if ( SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                    {
                        #region 
                        if (_iT < SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat.Count())
                        {
                            //还有下一行,确定行号
                            SysInfo.m_C_Item_Info.i_CurrRow_No++;
                            //确定屏幕序号
                            _iT = 0;
                            SysInfo.m_SysBuff_C.m_Plant_C.i_Screen_No = 0;
                            //确定距离
                            _i_Dist_S = SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd[_iT].i_Start;
                            _i_Dist_E = SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd[_iT].i_End;

                            int _iCurrRow = SysInfo.m_C_Item_Info.i_CurrRow_No;

                            while (SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[_iCurrRow].Count == 0)//cdw 7-2
                                _iCurrRow++;
                            if (_iCurrRow > -1)
                                SysInfo.m_C_Item_Info.i_CurrRow_No = _iCurrRow;
                            else
                                return;

                            _iT = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No].Count - 1;//末尾序号

                            _iDist_CurrRow_End = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No][_iT].i_X_mm;//行尾部距离

                            int _i_S = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No][0].i_X_mm;

                            if (_i_S > _iDist_CurrRow_End)
                            {
                                _i_Dist_S = _iDist_CurrRow_End;
                                _iDist_CurrRow_End = _i_S;
                                m_i_AllNum_Dd = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No].Count - 1;
                            }

                            if (_iDist_CurrRow_End <= _i_Dist_E)
                                _i_Dist_E = _iDist_CurrRow_End + SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
                            _blOk = true;
                        }
                        #endregion 
                    }
                    else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)
                    {
                        #region 多通道右翻页本行右边没有了
                        if (_iT < SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff.Count())
                        {
                            SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_No = 0;
                            //还有下一行,确定行号
                            SysInfo.m_C_Item_Info.i_CurrRow_No++;
                            //确定屏幕序号
                            _iT = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Ori_Data.Count - 1;
                            if (SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Ori_Data.Count == 0) return;

                            _iDist_CurrRow_End = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Ori_Data[_iT].i_X_mm;

                            int _i_S = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Ori_Data[0].i_X_mm;

                            if (_i_S > _iDist_CurrRow_End)
                            {
                                _i_S = _iDist_CurrRow_End;
                                SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_No = SysInfo  .m_SysBuff_C.m_Plant_C.Juge_ScreenNo_C(_i_S);
                            }
                           _iT = SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_No;
                            
                            //确定距离
                            _i_Dist_S = SysInfo  .m_SysBuff_C.m_Plant_C.lst_Screenkd[_iT].i_Start;
                            _i_Dist_E = SysInfo  .m_SysBuff_C.m_Plant_C.lst_Screenkd[_iT].i_End;

                            _iT = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Ori_Data.Count - 1;
                            _iDist_CurrRow_End = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Ori_Data[_iT].i_X_mm;

                             _i_S = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Ori_Data[0].i_X_mm;

                            if (_i_S > _iDist_CurrRow_End)
                            {
                                int _i_T = _iDist_CurrRow_End;
                                _iDist_CurrRow_End = _i_S;
                                _i_S = _i_T;

                                if (_i_Dist_S < _i_S) _i_Dist_S = _i_S;
                                if (_i_Dist_E > _iDist_CurrRow_End) _i_Dist_E = _iDist_CurrRow_End;

                                 m_i_AllNum_Dd = SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Ori_Data.Count - 1;
                            }

                            if (_iDist_CurrRow_End <= _i_Dist_E)
                                _i_Dist_E = _iDist_CurrRow_End + SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
                            _blOk = true;
                        }
                        #endregion 
                    }
                }
            }
            if (_blOk)
            {
                float _fl = 0;
                Bt_Row_Num.Text = "总共:" + SysInfo.m_C_Item_Info.i_C_AllRows + "行,当前第" + (SysInfo.m_C_Item_Info.i_CurrRow_No + 1).ToString() + "行";
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                {
                    SysInfo  .m_SysBuff_C.m_Plant_C.GetRulerPara_C();
                    SysInfo  .m_SysBuff_C.m_Plant_C.Plant_Ruler_C();
                    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                    {
                        SysInfo.m_SysBuff_C.m_Plant_C.Plant_Ruler_Coat_C();
                        DischargeImage_Clear();
                    }
                    _fl = (_i_Dist_S - SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_Start_Distance) / SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
                    m_i_Start_No = (int)_fl;
                    if (_fl - m_i_Start_No >= 0.5) m_i_Start_No++;
                   
                //    SysInfo  .m_SysBuff_C.m_Plant_C.Mul_Titl_Init_A(SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iAllRows_C);

                    SysInfo.m_SysBuff_C.m_Plant_C.Mul_Titl_Init_A(SysInfo.m_SysBuff_C.m_Plant_C.Scree_iAllRows_C);
                    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                        SysInfo.m_SysBuff_C.m_Plant_C.Mul_Titl_Init_A_Coat(SysInfo.m_SysBuff_C.m_Plant_C.Scree_iAllRows_C_Coat+1);
                    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 3)
                        SysInfo.m_SysBuff_C.m_Plant_C.Plant_Screen_C(SysInfo.m_C_Item_Info.i_CurrRow_No,
                                                       _i_Dist_S, _i_Dist_E, SysInfo  .m_SysBuff_C.m_Lst_C_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No]);
                    if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                    {      //涂层数据
                        SysInfo.m_SysBuff_C.m_Plant_C.Plant_Screen_Coat_C(SysInfo.m_C_Item_Info.i_CurrRow_No,
                                   _i_Dist_S, _i_Dist_E, SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No]);

                        Brus_Curr_Discharg();
                        Show_Text_Thread();
                    }
                }
                else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)
                {
          
                    _fl = (_i_Dist_S - SysInfo  .m_SysBuff_C.m_Plant_C.i_Screen_Start_Distance) / SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
                    m_i_Start_No = (int)_fl;
                    if (_fl - m_i_Start_No >= 0.5) m_i_Start_No++;

                    SysInfo  .m_SysBuff_C.m_Plant_C.Plant_Screen_Mul_Dd(_i_Dist_S, _i_Dist_E,
                                                        SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Show_Buff,
                                                        SysInfo  .m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].strGjbh );
                }
                Application.DoEvents();  
            }
        }
        private void Brus_Curr_Discharg()
        {
            float _fl = 0;
            int _iX = 0;
            int _iS = SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd[SysInfo.m_SysBuff_C.m_Plant_C.i_Screen_No].i_Start;
            int _iE = SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd[SysInfo.m_SysBuff_C.m_Plant_C.i_Screen_No].i_End;
            int _iNo = 0;
            for (int _i = 0; _i < SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No].Count; _i++)
            //放电数据
            {
                _iX = SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No][_i].i_X_mm;
                if (_iX >= _iS && _iX <= _iE)
                {
                    _fl = (_iX - _iS) / SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
                    _iNo = (int)_fl;
                    if (_fl - _iNo >= 0.5) _iNo++;
                    DischargeImage_Set(_iNo, SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No][_i].bl_Discharge);
                    DischargeImage_Set_Mark(_iNo, SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No][_i].bl_Discharge_To_Mark);
                }
            }
        }
        /// <summary>
        /// 数据翻页按钮位置
        /// </summary>
        Point m_Bt_Data_Point;
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


        #region 单一C扫描左右翻页


        private void Bt_Data_Left_MouseMove(object sender, MouseEventArgs e)
        {
            Bt_Move_RL(0, sender, e);
        }

        private void Bt_Data_Left_MouseDown(object sender, MouseEventArgs e)
        {
       //     m_Bt_Data_Point.X = Bt_Data_Left.Left;
            m_Bt_Data_Point.Y = e.Y;
        }

        private void Bt_Data_Right_MouseMove(object sender, MouseEventArgs e)
        {
            Bt_Move_RL(0, sender, e);
        }

        private void Bt_Data_Right_MouseDown(object sender, MouseEventArgs e)
        {
        //    m_Bt_Data_Point.X = Bt_Data_Right.Left;
            m_Bt_Data_Point.Y = e.Y;
        }

        private void Bt_Data_Right_Click(object sender, EventArgs e)
        {
            Page_L0_R1(1);
        }

        private void Bt_C_Left_MouseMove(object sender, MouseEventArgs e)
        {
            Bt_Move_RL(0, sender, e);
        }

        private void Bt_C_Left_MouseDown(object sender, MouseEventArgs e)
        {
         //   m_Bt_Data_Point.X = Bt_C_All_Left.Left;
            m_Bt_Data_Point.Y = e.Y;
        }



        private void Bt_C_Right_MouseMove(object sender, MouseEventArgs e)
        {
            Bt_Move_RL(0, sender, e);
        }

        private void Bt_C_Right_MouseDown(object sender, MouseEventArgs e)
        {
          //  m_Bt_Data_Point.X = Bt_C_All_Right.Left;
            m_Bt_Data_Point.Y = e.Y;
        }


        private void Bt_C_Down_MouseMove(object sender, MouseEventArgs e)
        {
            Bt_Move_RL(1, sender, e);
        }

        private void Bt_C_Down_MouseDown(object sender, MouseEventArgs e)
        {
            m_Bt_Data_Point.X = e.X;
         //   m_Bt_Data_Point.Y = Bt_C_All_Down.Top;
        }

        private void Bt_C_Up_MouseDown(object sender, MouseEventArgs e)
        {
            m_Bt_Data_Point.X = e.X;
          //  m_Bt_Data_Point.Y = Bt_C_All_Up.Top;
        }

        private void Bt_C_Up_MouseMove(object sender, MouseEventArgs e)
        {
            Bt_Move_RL(1, sender, e);
        }
        /// <summary>
        /// X轴数据
        /// </summary>
        public int i_X_B_mm = 0;

        private void Pic_C_Click(object sender, EventArgs e)
        {
            try
            {
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
                {
                    #region 
                    if (SysInfo.m_SysBuff_C.m_Ctrl.m_iRun != 1 && SysInfo.m_SysBuff_C.m_Lst_C_Buff.Count > 0 && SysInfo.m_C_Item_Info.i_CurrRow_No > -1)
                    {
                        int _i_LstOneLen = SysInfo.m_SysBuff_C.m_Lst_C_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].Count;
                        if (m_i_Arr_X_No > -1 && _i_LstOneLen > m_i_Arr_X_No)
                        {
                            i_X_B_mm = SysInfo.m_SysBuff_C.m_Lst_C_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No][m_i_Arr_X_No].i_X_mm;
                            m_Arr_B_Qur = SysInfo.m_SysBuff_C.m_Lst_C_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No][m_i_Arr_X_No].Arr_C_Data;
                    
                            if(m_Arr_B_Qur[0].iArr_ShowNo < 0)     m_Arr_B_Qur[0].iArr_ShowNo =(int)( m_i_Y_No* SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotHeightmm_Y);
                            if (m_Arr_B_Qur[0].iArr_ShowNo < 0)
                                
                                return;
                            int _iArr_Len = SysInfo.m_SysBuff_C.m_Lst_C_Buff
                                        [SysInfo.m_C_Item_Info.i_CurrRow_No][m_i_Arr_X_No].Arr_Emat[m_Arr_B_Qur[0].iArr_ShowNo].btArrWave.Length;
                            if (m_Arr_B_Qur != null)
                            {
                                for (int _iNo = 0; _iNo < m_Arr_B_Qur.Length; _iNo++)
                                {
                                    if (m_Arr_B_Qur[_iNo].iArr_ShowNo > -1 && m_Arr_B_Qur[_iNo].iArr_ShowNo < SysInfo.m_SysBuff.m_Climb.iGsb_Len)
                                    {
                                        m_Arr_B_Qur[_iNo].btArrWave = new byte[_iArr_Len];
                                        var IntPtArr_S = Marshal.UnsafeAddrOfPinnedArrayElement(m_Arr_B_Qur[_iNo].btArrWave, 0);
                                        Marshal.Copy(SysInfo.m_SysBuff_C.m_Lst_C_Buff
                                            [SysInfo.m_C_Item_Info.i_CurrRow_No][m_i_Arr_X_No].Arr_Emat[m_Arr_B_Qur[_iNo].iArr_ShowNo].btArrWave,
                                            0, IntPtArr_S, _iArr_Len);//注意数据范围
                                    }
                                }
                                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
                                {
                                    SysInfo.m_SysBuff_C.m_Plant_C.Plant_Ruler_B(SysInfo.m_iLanguage);
                                    for (int _iY = 0; _iY < m_Arr_B_Qur.Count(); _iY++)
                                        SysInfo.m_SysBuff_C.m_Plant_C.Plant_B(_iY, m_Arr_B_Qur[_iY], 0);
                                    SysInfo.m_SysBuff_C.m_Plant_C.Plant_B_Brush();
                                }
                                else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                                {

                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)
                {
                    if (SysInfo.m_SysBuff_C.m_Plant_C.m_Ck_His_Wave == false) return;


                    if (SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl.Count == 0) return;
                    if (m_i_Y_No < 0 || m_i_Y_No >= SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl.Count) return;

                    int _iAllCount = SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl[m_i_Y_No].lst_One_Row_Data.Count();
                    if (_iAllCount == 0 || m_i_X_No >= _iAllCount) return;

                    m_i_Y_Arr_No = m_i_X_No;
                    Cls_One_Data _One = SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl[m_i_Y_No].lst_One_Row_Data[m_i_X_No];
                    if (_One.btArrWave == null) return;
                    int _iArrLen = _One.btArrWave.Length;

            

                }
            }
            catch { }
        }
        /// <summary>
        /// X轴位置序号：鼠标在屏幕的位置
        /// </summary>
        int m_i_X_No = 0;
        /// <summary>
        /// 对应当前行缓存序号
        /// </summary>
        int m_i_Arr_X_No = 0;
        /// <summary>
        /// 倒序检测时使用：开始序号
        /// </summary>
        int m_i_Start_No = 0;
         /// <summary>
         /// 倒序检查时：总长度
         /// </summary>
        int m_i_AllNum_Dd = 0;
        /// <summary>
        /// Y轴位置序号
        /// </summary>
        int m_i_Y_No = 0;
        /// <summary>
        /// 鼠标点，对应光栅臂波形数据的位置
        /// </summary>
        int m_i_Y_Arr_No = -1;
        /// <summary>
        /// 单一C扫图描鼠标移动位置对应的B扫
        /// </summary>
        Cls_EMAT_2[] m_Arr_B_Qur = null;
        private void Pic_C_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
             //   Lab_C.Text = "";
                if (SysInfo.m_C_Item_Info.i_CurrRow_No < 0) return;

                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                {
                    if (SysInfo.m_SysBuff_C.m_C_One_Buff.Count() == 0)
                        if (SysInfo.m_SysBuff_C.m_Lst_C_Buff.Count == 0)
                            return;
                }
                else
                {
                    if (SysInfo.m_SysBuff_C.m_Lst_Mul_All_Buff.Count == 0) return;
                    if (SysInfo.m_SysBuff_C.m_Lst_Mul_All_Buff[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Show_Buff.Count == 0)
                        return;
                }
                //if (SysInfo.m_C_Item_Info.i_CurrRow_No < 0) return;
                //1 X轴序号
                float fl = (float)(e.X - SysInfo.m_SysBuff_C.m_Plant_C.Chart_Ruler_X_Start + 0) / SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWith_X;
                //     float fl = (float)(e.X - SysInfo  .m_SysBuff_C.m_Plant_C.Chart_Ruler_X_Start - 1) / SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWith_X;
                if (fl < 0) return;

                m_i_X_No = (int)fl;
                if (fl - m_i_X_No >= 0) m_i_X_No++;
                m_i_X_No--;

                if (m_i_X_No < 0) return;
                if (SysInfo.m_SysBuff_C.m_Plant_C.i_Screen_No >= SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd.Count) return;

                int _i_Dist_S = SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd[SysInfo.m_SysBuff_C.m_Plant_C.i_Screen_No].i_Start;
                int _i_Curr_Dist = _i_Dist_S + m_i_X_No * SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
                if (_i_Curr_Dist >= SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd[SysInfo.m_SysBuff_C.m_Plant_C.i_Screen_No].i_End) return;
                fl = _i_Curr_Dist / SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
                int _i_X_No = (int)fl;
                if (fl - _i_X_No >= 0.5) _i_X_No++;
                m_i_Arr_X_No = _i_X_No;
                m_i_Arr_X_No -= m_i_Start_No;
                if (m_i_AllNum_Dd > 0)
                    m_i_Arr_X_No = m_i_AllNum_Dd - m_i_Arr_X_No;
                //2 Y轴序号
                fl = (e.Y - SysInfo.m_SysBuff_C.m_Plant_C.Chart_Ruler_Y_Start - 1) / (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ? SysInfo.m_SysBuff_C.m_Plant_C.m_iMul_OneB_Heigh : SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotHeight);
                m_i_Y_No = (int)fl;

                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                {
                    if (m_i_Y_No < 0 || m_i_Y_No >= SysInfo.m_SysBuff_C.m_Plant_C.i_Gsb_Arr_Len) return;
                }
                else
                {
                }

             //   fl = (e.Y - SysInfo.m_SysBuff_C.m_Plant_C.Chart_Ruler_Y_Start - 1) / SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.Scree_flDotHeight;
                int _iY = m_i_Y_No;// (int)fl;

                string _strT = "";

                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                {
                    if (_iY > -1 && _iY < SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl_A.lst_Mul_Titl.Count)
                    {
                        if (m_i_X_No < SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl_A.lst_Mul_Titl[_iY].lst_One_Row_Data.Count)
                        {
                            Cls_One_Data _One = SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl_A.lst_Mul_Titl[_iY].lst_One_Row_Data[m_i_X_No];
                            if (_One.i_X == -1)
                                return;
                            //     if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
                            {
                                if (SysInfo.m_iLanguage == 0)
                                    _strT = "X=" + _i_Curr_Dist + "mm " +//_One.i_X
                                             " Y:" + ((_One.i_ABC-1) * SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotHeightmm_Y).ToString() +//" "+ _iY+
                                             " 厚度:" + _One.flThick.ToString("f2") + "mm" + "  Erro:" + _One.flWc +
                                             "第" + ((SysInfo.m_C_Item_Info.i_CurrRow_No == -1 ? 0 : SysInfo.m_C_Item_Info.i_CurrRow_No) + 1) + "行\r\n";
                                else
                                    _strT = "X=" + _i_Curr_Dist + "mm " +
                                         " Y:" + _One.strGdbh + ":" + ((_One.i_ABC - 1) * SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X).ToString() +
                                         " Thick.:" + _One.flThick.ToString("f2") + "mm" + "  Erro:" + _One.flWc +
                                         "Line:" + ((SysInfo.m_C_Item_Info.i_CurrRow_No == -1 ? 0 : SysInfo.m_C_Item_Info.i_CurrRow_No) + 1);

                            }
                        }
                    }
                }
                else
                {
                    if (SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl.Count == 0) return;
                    if (m_i_Y_No >= SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl.Count) return;
                    int _iAllCount = SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl[m_i_Y_No].lst_One_Row_Data.Count();
                    if (_iAllCount == 0 || m_i_X_No >= _iAllCount) return;

                    m_i_Y_Arr_No = m_i_X_No;
                    Cls_One_Data _One = SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl[m_i_Y_No].lst_One_Row_Data[m_i_X_No];
                    if (_One.i_X == -1)
                        return;
                    if (SysInfo.m_iLanguage == 0)
                        _strT = "X=" + _One.i_X + "mm " +
                              _One.flThick.ToString("f2") + "mm" + "  Erro:" + _One.flWc + "\r\n" +
                             "第" + ((SysInfo.m_C_Item_Info.i_CurrRow_No) + 1) + "行 "+
                              (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2?(_iY+1).ToString () +"通道":"");
                    else
                        _strT = "X=" + _One.i_X + "mm " +
                             "Y=" + ((m_i_Y_No + 1) + " ") + " Thick.:" + _One.flThick.ToString("f2") + "mm" + "  Erro:" + _One.flWc + "\r\n" +
                             "Line:" + ((SysInfo.m_C_Item_Info.i_CurrRow_No) + 1) + " " +
                              (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ? (_iY + 1).ToString() + "-channel" : "");
                }

            //    Show_Val(Pic_C, Lab_C, _strT, e);
            }
            catch (Exception e2)
            { }
        }
        delegate void Delg_Show(PictureBox Pic, Label _Lab, string Val, MouseEventArgs e);
        private void Show_Val(PictureBox Pic, Label _Lab, string Val, MouseEventArgs e)
        {
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_Show ms = new Delg_Show(Lab_Titl);
                    this.Invoke(ms, new object[] { Pic,_Lab ,Val ,e });
                }
                catch
                { }
            }
            else
            {
                Lab_Titl(Pic, _Lab, Val, e);
            }
        }
        private void Lab_Titl(PictureBox Pic, Label _Lab, string Val, MouseEventArgs e)
        {
          //  Bt_Row_Num.Text = Val;
            _Lab.Text = Val;
            _Lab.Visible = true;
            if (Pic.Width - e.X < _Lab.Width+20)
                _Lab.Left = e.X - _Lab.Width - 10;
            else
                _Lab.Left = e.X + 20;
            _Lab.Top = e.Y - 20;
        }
        /// <summary>
        /// 当前B扫描对应位置
        /// </summary>
        private int m_i_B_Y_No = 0;
        private void Pic_B_MouseMove(object sender, MouseEventArgs e)
        {
    //        if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 3) return;
            if (m_bl_Active == false) return;
            if (SysInfo  .m_SysBuff_C.m_Ctrl.m_iRun == 1) return; 
            if (m_Arr_B_Qur == null) return;
            if (SysInfo.m_SysBuff_C.m_Plant_C.m_Scree_iDotWith_B == 0) return;
            try
            {
                float fl = (e.X - SysInfo.m_SysBuff_C.m_Plant_C.Chart_Ruler_X_Start - 1) / SysInfo.m_SysBuff_C.m_Plant_C.m_Scree_iDotWith_B;
                m_i_B_Y_No = (int)fl;
                if (m_i_B_Y_No < 0 || m_i_B_Y_No >= m_Arr_B_Qur.Count() || m_Arr_B_Qur[m_i_B_Y_No].flThick < 0) return;

                string _strT = "X=" + i_X_B_mm + "  Y=" + (m_i_B_Y_No) * SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotHeightmm_Y + "mm\r\n" +
                         (SysInfo.m_iLanguage == 0 ? "厚度:" : "Thick.：") + m_Arr_B_Qur[m_i_B_Y_No].flThick.ToString("f2") + "mm" + "Erro:" + m_Arr_B_Qur[m_i_B_Y_No].flWc;
              //  Lab_Titl(Pic_B, Lab_B, _strT, e);
            }
            catch (Exception ddd)
            { }
        }

        private void Pic_C_MouseLeave(object sender, EventArgs e)
        {
          //  Lab_C.Visible = false;
        }

        private void Pic_B_MouseLeave(object sender, EventArgs e)
        {
         //   Lab_B.Visible = false;
        }

        CL3d.Form1 m_frm_3D = new CL3d.Form1();
    
        bool m_bl_3D = false;
       // private Kitware.VTK.RenderWindowControl m_RendWin_Ctr = new Kitware.VTK.RenderWindowControl();
        private void Bt_3D_Click(object sender, EventArgs e)
        {
            Show3D();
        }
        private void Show3D(int iOnOFf=1)
        {
            if (m_bl_3D)
                m_frm_3D.deleteAllVTKObjects();
            m_frm_3D = new CL3d.Form1();

            if (SysInfo  .m_SysBuff_C.m_Lst_C_Buff.Count == 0)
            {
                MessageBox.Show("没有要显示的数据!");
                return;
            }
            m_frm_3D.m_Plant_3D = iOnOFf;// Ck_3D.Checked ? 1 : 0;
            if (m_frm_3D.m_Plant_3D == 1 && m_frm_3D.m_Table == null)
            {
                if (SysInfo  .m_SysBuff_C.m_Lst_C_Buff.Count > 0)
                {
                    Class_Info _inf = new Class_Info();
                    _inf.fl_Normal_Thickness = SysInfo  .m_SysBuff_C.m_Plant_C.flNormal_Thickness;
                    _inf.iGsbLen = SysInfo  .m_SysBuff_C.m_Plant_C.iGsb_Len;
                    _inf.i_Xmm = SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;

                    _inf.i_Ymm =(int) SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotHeightmm_Y;
                    m_frm_3D.m_Table = new CL3d.Class_Tablets(SysInfo  .m_SysBuff_C.m_Lst_C_Buff, _inf);
                }
            }
            //  if(m_bl_3D)
            {
                // m_frm_3D.deleteAllVTKObjects();

                switch (m_frm_3D.m_Plant_3D)
                {
                    case 0:
                        m_frm_3D.m_Pipe.Plant_Wg();
                        m_frm_3D.Plant_Obj(m_frm_3D.m_Pipe.g_PointPoly);
                        break;
                    case 1:
                        m_frm_3D.m_Table.Plant();

                        m_frm_3D.Plant_Obj(m_frm_3D.m_Table.m_PointPoly);
                        break;
                }

                // return;
            }

            m_bl_3D = true;
           // CreateMDIControl(m_frm_3D);
            if (m_bl_3D_DLL)
            {
                SysInfo.WaitTime(5000);
                m_frm_3D.m_bl_3D_Dell = false;
            }
        }
        private Form f = null;
        private void CreateMDIControl(Form frmBase)
        {
            //frmBase.Show();
            //return;
            if (f != null)
            {
        //        m_frm_3D.m_Plant_3D = 1;
        //        m_frm_3D.deleteAllVTKObjects();
                f.Dispose(); f.Close();
                //  m_frm.Close();
                // m_frm = new CL3d.Form1();
            }
            f = frmBase;
            try
            {
            //  Pan_3D   .Controls.Clear();
             //   Pan_3D.Visible = true;
                frmBase.FormBorderStyle = FormBorderStyle.None;
                frmBase.TopLevel = false ;

                frmBase.Dock = DockStyle.Fill;

            //    this.Pan_3D.Controls.Add(f);
                frmBase.Show();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //loading.CloseWaitForm();
            }
        }
        bool m_bl_3D_DLL = false;
        private void button6_Click(object sender, EventArgs e)
        {
            m_frm_3D.deleteAllVTKObjects();
      //      SysInfo.WaitTime(5000);
            m_bl_3D_DLL = true;
        }
        bool m_bl_Show = false;
        private void Rad_0_Click(object sender, EventArgs e)
        {
         //   Rad_0.Enabled = false;
          //  Rad_3.Visible = false ;
            try
            {
                if (m_bl_Show)
                {
                    m_frm_3D.deleteAllVTKObjects();
                    m_bl_3D_DLL = true;
                }
                else
                {
            
                    Show3D();
                  //  CreateMDIControl(m_frm_3D);
                }

                m_bl_Show = !m_bl_Show;
            }
            catch { }
      //      Rad_0.Enabled = true;
        }
        bool m_bl_Curr_Pic_Af_Bt = false;
        private void Bt_1_L_MouseUp(object sender, MouseEventArgs e)
        {
            Set_L_R(1);
        }
        private void Set_L_R(int iType)
        {
         
            m_bl_A_Inv1_Other0 = false;
            m_bl_Curr_Pic_Af_Bt = false;
            m_bl_Pic_A_B_Plant = false;


        }
        private void Set_L_R_2(int iType)
        {
           
            m_bl_A_Inv1_Other0 = false;
            m_bl_Curr_Pic_Af_Bt = true;
            m_bl_Pic_A_B_Plant = false;


        }
        private void Bt_1_L_MouseMove(object sender, MouseEventArgs e)
        {
            Move_Wave_1_2(1, sender, e);
        }
        private void Move_Wave_1_2(int iType, object sen, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)//
            {
                Button _bt = sen as Button;
                int _iX = _bt.Location.X + (e.X - m_mousePos.X);
                if (_iX <= 0) return;

              

            }

        
        }

        private void Move_Wave_1_2_Tow(int iType, object sen, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)//
            {
                Button _bt = sen as Button;
                int _iX = _bt.Location.X + (e.X - m_mousePos.X);
                if (_iX <= 0) return;

              

            }

            
        }
        private int GetWaveDataPoint(int iCurr)
        {
            int _iRet = 0;
          
            return _iRet;
        }
        private void Bt_1_L_MouseDown(object sender, MouseEventArgs e)
        {
          //  GetCurrX(Bt_1_L, e);
        }
        private void GetCurrX(Button Bt_Curr, MouseEventArgs e)
        {
            m_bl_A_Inv1_Other0 = true;
            m_bl_Pic_A_B_Plant = true;
            m_mousePos.X = e.X;
            m_mousePos.Y = e.Y;// Bt_Curr.Top;
        }
        private void Bt_1_R_MouseUp(object sender, MouseEventArgs e)
        {
            Set_L_R(1);
        }

        private void Bt_1_R_MouseMove(object sender, MouseEventArgs e)
        {
            Move_Wave_1_2(2, sender, e);
        }

        private void Bt_1_R_MouseDown(object sender, MouseEventArgs e)
        {
       //     GetCurrX(Bt_1_R, e);
        }

        private void Bt_2_L_MouseUp(object sender, MouseEventArgs e)
        {
            Set_L_R(2);
        }

        private void Bt_2_L_MouseMove(object sender, MouseEventArgs e)
        {
            Move_Wave_1_2(3, sender, e);
        }
        //--------------

        //delegate void Delg_Move(int iType, object sender, MouseEventArgs e);
        //private void Thread_Move(int iType, object sender, MouseEventArgs e)
        //{

        //    #region 画D图
        //    if (this.InvokeRequired == true)
        //    {
        //        try
        //        {
        //            Delg_Move ms = new Delg_Move(Move_Wave_1_2);
        //            this.Invoke(ms, new object[] { iType, sender,e });
        //        }
        //        catch
        //        { }
        //    }
        //    else
        //    {
        //        Move_Wave_1_2(iType, sender, e);
        //    }
        //    #endregion
        //}

        //------------
        private void Bt_2_L_MouseDown(object sender, MouseEventArgs e)
        {
          //  GetCurrX(Bt_2_L, e);
        }

        private void Bt_2_R_MouseUp(object sender, MouseEventArgs e)
        {
            Set_L_R(2);
        }

        private void Bt_2_R_MouseMove(object sender, MouseEventArgs e)
        {
            Move_Wave_1_2(4, sender, e);
        }

        private void Bt_2_R_MouseDown(object sender, MouseEventArgs e)
        {
          //  GetCurrX(Bt_2_R, e);
        }

        private void Txt_Speed_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void Bt_WaveType_Click(object sender, EventArgs e)
        {
          
        }
        private void SendWaveType(int iWaveType=2)
        {
           
        }
        /// <summary>
        /// Wave type name
        /// </summary>
        private void Set_Wave_WaveType()
        {
           
           
        }

        private void Cmb_Algorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        private void Bt_L_R(bool blVal)
        {
           
        }
        private void Bt_L_R_2(bool blVal)
        {
          
        }
        private void Cmb_AvCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_bl_Active)
            {
                SetAvCount_Cmb();
                AvCountSave();
            }
        }
        private void AvCountSave()
        {
            if (m_bl_Active)
            {
                AccBrush();
             //   SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_csInter.INIWriteValue("System_Para", "Cmb_AvCount", Cmb_AvCount.Text, SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.HardFileName);
            }
        }
        private void AccBrush()
        {
            string strData = "";
           
        }
        private void SetAvCount_Cmb()
        {
            Av_Change();
            SetAvCount();
        }
        /// <summary>
        /// Set the integral coherence number
        /// </summary>
        private void SetAvCount()
        {
                 }
        /// <summary>
        /// 
        /// </summary>
        private void Av_Change()
        {
            int iData = 6;
         

        }

        private void Ck_AutoGain_Click(object sender, EventArgs e)
        {
          
        }

        private void Bt_Set_Step_Click(object sender, EventArgs e)
        {
        }
        private void SetStep()
        {
            Set_Wave_Sf();

          }
        private void Set_Wave_Sf(int iType = 0)
        {
   

           
        }

        private void Bt_Wave_Show_Click(object sender, EventArgs e)
        {
            UI_KeyBox();
        }
        private void Set_Ico(int iData)
        {
         //   Bt_Wave_Show.Image = ImgLst_UI.Images[iData == 1 ? 0 : 1];//1: 朝左   0：朝右
        }
        private void UI_KeyBox()
        {
          
        }

        private void Txt_X_JL_Click(object sender, EventArgs e)
        {

        }

        private void Cmb_Gate_0_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }
        private void Set_Gate_A_B()
        {
          // 
          
        }
        private void Bt_Win_Mw_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;// WindowState.Minimized;

        }

        private void Time_UI_ReLink_Tick(object sender, EventArgs e)
        {
          
        }

        private void Cmb_Gjxz_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Txt_Tld_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Txt_Gj_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Cmb_Btcz_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void Cmb_Bhccz_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void Cmb_Xytt_SelectedIndexChanged(object sender, EventArgs e)
        {
           
           
        }
      
        /// <summary>
        /// 脉冲涡流参数下发：项目建立
        /// </summary>
        private void SendPara_ECT()
        {
          
        }
        private void Bt_Bd_Click(object sender, EventArgs e)
        {
          
        }

        private void Bt_ECT_Pause_Click(object sender, EventArgs e)
        {
            
        }

        private void Bt_Parat_Click(object sender, EventArgs e)
        {
        
        }

        private void Bt_Continu_Click(object sender, EventArgs e)
        {
        
        }

        private void Pic_A_Click(object sender, EventArgs e)
        {
          RunTime();
        }
        private void RunTime()
        {
          //  Lb_Runing.Visible = !Lb_Runing.Visible;
           // Lab_C_Time.Visible = !Lab_C_Time.Visible;
            m_bl_Ruler_Color = !m_bl_Ruler_Color;
        }
        private void Pic_B_Click(object sender, EventArgs e)
        {
           
        }

        private void Ck_His_Wave_CheckedChanged(object sender, EventArgs e)
        {
     
        }
        /// <summary>
        /// 增益门限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txt_i_Gain_Limit_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void Ck_Thick_2_CheckedChanged(object sender, EventArgs e)
        {
           // SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_2BeiSjxx = Ck_Thick_2.Checked;

            string _strFile = System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini";
           // SysInfo.csInter.INIWriteValue("System_Para", "m_iGain_Limit", SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_2BeiSjxx?"1":"0", _strFile);

        }

        private void Txt_Y_JL_Click(object sender, EventArgs e)
        {
  
        }

        private void Bt_Urgent_Click(object sender, EventArgs e)
        {
            JinJiTingChe();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChaXun();
        }

        private void Bt_OSK_Titl_Click(object sender, EventArgs e)
        {
            Exit_0();
        }

        private void bt_Start_T_Click(object sender, EventArgs e)
        {
            KaiShi();
        }

        private void bt_Move_T_Click(object sender, EventArgs e)
        {
            Bt_Move_0();
        }

        private void bt_Item_T_Click(object sender, EventArgs e)
        {
            Link_V();
        }

        private void bt_Tofd_T_Click(object sender, EventArgs e)
        {
            Bt_Tofd_0();
        }

        private void Pic_A_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void Pic_A_MouseLeave(object sender, EventArgs e)
        {
          //  Lb_Thick.Visible = false;
        }

        private void Ck_By_My_Click(object sender, EventArgs e)
        {
          
            //if (Ck_By_My.Checked)
            //{
            //    SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iStep = 2;
            //    SetStep();
            //}
        }

        private void Rad_Gate_1_Click(object sender, EventArgs e)
        {
            SysInfo.m_SysBuff.m_Tofd_DLL.m_Data_Type =1;
        
            Set_Gate_A_B();
        }

        private void Rad_Gate_2_Click(object sender, EventArgs e)
        {
            SysInfo.m_SysBuff.m_Tofd_DLL.m_Data_Type = 2;
        
            Set_Gate_A_B();
        }
        DateTime m_KeyStar = DateTime.Now;
        private void Frm_Main_C_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Ck_E_Click(object sender, EventArgs e)
        {
          
        }

        private void Bt_Cmd_1_Click(object sender, EventArgs e)
        {
         
        }
        private void Ask_Coat_State()
        {
            
        }
        private void Bt_Cmd_2_0_Click(object sender, EventArgs e)
        {
        
        }
        private void ReStart_Equi()
        {
            Thread _Thread_Restar =null ;
            if (_Thread_Restar != null) _Thread_Restar.Abort();
            _Thread_Restar = new Thread(new ThreadStart(Delg_Restart_Wait));
         //   _Thread_Restar = new Thread(new ParameterizedThreadStart(Delg_Restart_Wait));
           
            _Thread_Restar.Priority = ThreadPriority.Highest;
            _Thread_Restar.Name = "Delg_Restart_Wait";
            _Thread_Restar.IsBackground = true;
            _Thread_Restar.Start();
        }
        delegate void Delg_Restart(int iWait);
        int m_i_Restart_Time = 10;
        private void Delg_Restart_Wait()
        {
           // int iType = 60;
            if (this.InvokeRequired == true)
            {
                try
                {
                    Delg_Restart ms = new Delg_Restart(Restart_Wait);
                    this.Invoke(ms, new object[] { m_i_Restart_Time });
                }
                catch
                { }
            }
            else
            {
                Restart_Wait(m_i_Restart_Time);
            }
        }
        private void Rstart_Enb(bool blVal)
        {
         
        }
        public void Restart_Wait(int dbWait)
        {
           
        }
        /// <summary>
        /// 当前涂层设备
        /// </summary>
        private int m_i_Coat_No = 0;
        private void Bt_Cmd_2_1_Click(object sender, EventArgs e)
        {
          
        }

        private void Pic_Coat_MouseMove(object sender, MouseEventArgs e)
        {
         //   Lab_C.Text = "";
            if (SysInfo.m_C_Item_Info.i_CurrRow_No < 0) return;

            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
            {
                if (SysInfo.m_SysBuff_C.m_Lst_C_Buff_Coat.Count() == 0)
                    if (SysInfo.m_SysBuff_C.m_Lst_C_Buff.Count == 0)
                        return;
            }
            else
            {
                if (SysInfo.m_SysBuff_C.m_Lst_Mul_All_Buff_Coat.Count == 0) return;
                if (SysInfo.m_SysBuff_C.m_Lst_Mul_All_Buff_Coat[SysInfo.m_C_Item_Info.i_CurrRow_No].lst_Show_Buff.Count == 0)
                    return;
            }
            //if (SysInfo.m_C_Item_Info.i_CurrRow_No < 0) return;
            //1 X轴序号
            float fl = (float)(e.X - SysInfo.m_SysBuff_C.m_Plant_C.Chart_Ruler_X_Start + 0) / SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWith_X;
            //     float fl = (float)(e.X - SysInfo  .m_SysBuff_C.m_Plant_C.Chart_Ruler_X_Start - 1) / SysInfo  .m_SysBuff_C.m_Plant_C.Scree_iDotWith_X;
            if (fl < 0) return;

            m_i_X_No = (int)fl;
            if (fl - m_i_X_No >= 0) m_i_X_No++;
            m_i_X_No--;

            if (m_i_X_No < 0) return;
            if (SysInfo.m_SysBuff_C.m_Plant_C.i_Screen_No >= SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd.Count) return;

            int _i_Dist_S = SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd[SysInfo.m_SysBuff_C.m_Plant_C.i_Screen_No].i_Start;
            int _i_Curr_Dist = _i_Dist_S + m_i_X_No * SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
            if (_i_Curr_Dist >= SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd[SysInfo.m_SysBuff_C.m_Plant_C.i_Screen_No].i_End) return;
            fl = _i_Curr_Dist / SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
            int _i_X_No = (int)fl;
            if (fl - _i_X_No >= 0.5) _i_X_No++;
            m_i_Arr_X_No = _i_X_No;
            m_i_Arr_X_No -= m_i_Start_No;
            if (m_i_AllNum_Dd > 0)
                m_i_Arr_X_No = m_i_AllNum_Dd - m_i_Arr_X_No;
            //2 Y轴序号
            fl = (e.Y - SysInfo.m_SysBuff_C.m_Plant_C.Chart_Ruler_Y_Start - 1) / (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ?
                     SysInfo.m_SysBuff_C.m_Plant_C.m_iMul_OneB_Heigh : SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotHeight_Coat);
            m_i_Y_No = (int)fl;// -1;

            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
            {
                if (m_i_Y_No < 0 || m_i_Y_No > SysInfo.m_SysBuff_C.m_Plant_C.i_Gsb_Arr_Len_Coat_ALL) 
                    return;
            }
            else
            {
                
            }

     //       fl = (e.Y - SysInfo.m_SysBuff_C.m_Plant_C.Chart_Ruler_Y_Start - 1) / SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.Scree_flDotHeight;
            int _iY = m_i_Y_No;// (int)fl;

            string _strT = "";

            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
            {
                if (_iY > -1 && _iY < SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl_Coat_A.lst_Mul_Titl.Count)
                {
                    if (m_i_X_No < SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl_Coat_A.lst_Mul_Titl[_iY].lst_One_Row_Data.Count)
                    {
                        Cls_One_Data _One = SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl_Coat_A.lst_Mul_Titl[_iY].lst_One_Row_Data[m_i_X_No];
                        if (_One.i_X == -1)
                            return;
                        //     if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
                        {
                            if (SysInfo.m_iLanguage == 0)
                                _strT = "X=" + _i_Curr_Dist + "mm " +//_One.i_X
                                         " Y:" + _One.i_ABC.ToString() +
                                         " 厚度:" + _One.flThick.ToString("f2") + "um" + "  Erro:" + _One.flWc +
                                         "第" + ((SysInfo.m_C_Item_Info.i_CurrRow_No == -1 ? 0 : SysInfo.m_C_Item_Info.i_CurrRow_No) + 1) + "行\r\n";
                            else
                                _strT = "X=" + _i_Curr_Dist + "mm " +
                                     " Y:" + _One.strGdbh + ":" + _One.i_ABC.ToString() +
                                     " Thick.:" + _One.flThick.ToString("f2") + "um" + "  Erro:" + _One.flWc +
                                     "Line:" + ((SysInfo.m_C_Item_Info.i_CurrRow_No == -1 ? 0 : SysInfo.m_C_Item_Info.i_CurrRow_No) + 1);

                        }
                    }
                }
            }
            else
            {
                if (SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl.Count == 0) return;
                int _iAllCount = SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl[m_i_Y_No].lst_One_Row_Data.Count();
                if (_iAllCount == 0 || m_i_X_No >= _iAllCount) return;

                m_i_Y_Arr_No = m_i_X_No;
                Cls_One_Data _One = SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl.lst_Mul_Titl[m_i_Y_No].lst_One_Row_Data[m_i_X_No];
                if (_One.i_X == -1)
                    return;
                if (SysInfo.m_iLanguage == 0)
                    _strT = "X=" + _One.i_X + "mm " +
                          _One.flThick.ToString("f2") + "mm" + "  Erro:" + _One.flWc + "\r\n" +
                         "第" + ((SysInfo.m_C_Item_Info.i_CurrRow_No) + 1) + "行\r\n";
                else
                    _strT = "X=" + _One.i_X + "mm " +
                         "Y=" + ((m_i_Y_No + 1) + " ") + " Thick.:" + _One.flThick.ToString("f2") + "mm" + "  Erro:" + _One.flWc + "\r\n" +
                         "Line:" + ((SysInfo.m_C_Item_Info.i_CurrRow_No) + 1) + "\r\n";
            }

           // Show_Val(Pic_C, Lab_C, _strT, e);
        }

    
        private void Pic_Coat_All_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_bl_C_All_Active == false) return;
            Zoom_Coat(e);
            if (SysInfo.m_SysBuff_C.m_Ctrl.m_iRun == 0 && SysInfo.m_C_Item_Info.i_C_AllRows < 1) return;
        //    Lab_C_All.Text = "";
            //1 X轴序号
            float fl = (float)(e.X - SysInfo.m_SysBuff_C.m_Plant_C.Chart_Ruler_X_Start_C_All + 0) / SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWith_X;
            if (fl < 0) return;
            int _i_X_No = (int)fl;
            m_i_X_No = _i_X_No;
            if (fl - m_i_X_No >= 0) m_i_X_No++;
            m_i_X_No--;

            if (_i_X_No < 0) return;
            if (SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No < 0) return;
            if (SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd_C_ALL.Count == 0) return;

            int _i_Dist_S = SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd_C_ALL[SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No].i_Start;
            int _i_Curr_Dist = _i_Dist_S + _i_X_No * SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
            if (_i_Curr_Dist >= SysInfo.m_SysBuff_C.m_Plant_C.lst_Screenkd_C_ALL[SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.i_Screen_No].i_End) return;

            fl = _i_Curr_Dist / SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotWithmm_X;
            _i_X_No = (int)fl;
            if (fl - _i_X_No >= 0.5) _i_X_No++;

            //2 Y轴序号
            fl = (e.Y - SysInfo.m_SysBuff_C.m_Plant_C.Chart_Ruler_Y_Start - 1) / SysInfo.m_SysBuff_C.m_Plant_C.cls_C_All_Mark.Scree_flDotHeight_Coat;
            int _iY = (int)fl;
            string _strT = "";

            #region 显示界面缓存

            if (_iY > -1 && _iY < SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl_Coat.lst_Mul_Titl.Count)
            {
                if (m_i_X_No < SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl_Coat.lst_Mul_Titl[_iY].lst_One_Row_Data.Count)
                {
                    Cls_One_Data _One = SysInfo.m_SysBuff_C.m_Plant_C.m_Mul_Titl_Coat.lst_Mul_Titl[_iY].lst_One_Row_Data[m_i_X_No];
                    if (_One.i_X == -1)
                        return;
                 //   if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
                    {
                        if (SysInfo.m_iLanguage == 0)
                            _strT = "X=" + _One.i_X + "mm " +
                                    " " + _One.strGdbh + " Y:" + _One.i_ABC.ToString() +
                                     " 厚度:" + _One.flThick.ToString("f2") + "mm" + "  Erro:" + _One.flWc;// + "\r\n" +
                        else
                            _strT = "X=" + _One.i_X + "mm " +
                                     " " + _One.strGdbh + " Y:" + _One.i_ABC.ToString() +
                                     " Thick.:" + _One.flThick.ToString("f2") + "mm" + "  Erro:" + _One.flWc;// + "\r\n" +

                    }
                    //else
                    //{

                    //    _strT = "X=" + _One.i_X + "mm " +
                    //         " " + _One.strGdbh + " 通道:" + _One.i_ABC.ToString() +
                    //         " 厚度:" + _One.flThick.ToString("f2") + "mm" + "  Erro:" + _One.flWc;// + "\r\n" +


                    //}
                }
            }
            #endregion

         //   Lab_Titl(Pic_Coat_All, Lab_C_All, _strT, e);
           
        }

        private void Time_Spark_Tick(object sender, EventArgs e)
        {
       //     Bt_Spark.Visible = false;
        //    Bt_Spark_Cs.Visible = false;
            Time_Spark.Enabled = false;
            SysInfo.m_Climb4.m_SysBuf.bl_Discharge_Show = false;
            SysInfo.m_Climb4.m_SysBuf.bl_Discharge_Show_To_Mark = false;
        }

        private void Bt_Spark_Cs_Click(object sender, EventArgs e)
        {
          //  Bt_Spark_Cs.Enabled = false;
            SysInfo.m_Climb4.m_SysBuf.bl_Discharge_To_Mark = true;
            SysInfo.m_Climb4.m_SysBuf.bl_Discharge_Show_To_Mark = true;
         //   Bt_Spark_Cs.Enabled = true;
        }

        private void Txt_Speed_Click(object sender, EventArgs e)
        {
       //     int _iL = Pan_A_Ctrl.Left + Txt_Speed.Left;
        //    int _iT = Txt_Speed.Height + 90 + Txt_Speed.Top;
        //    SysInfo.SetKeyBorad(_iL, _iT, "num_keyboard");
        }

        private void Txt_Speed_Leave(object sender, EventArgs e)
        {
            SysInfo.SetKeyBorad("num_keyboard");
        }

        private void Rad_3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Bt_V_Calcu_Click(object sender, EventArgs e)
        {
            //if(SysInfo . m_bl_Img_Calcu==false )
            //{
            //    Frm_Img_Calcu _frm = new Frm_Img_Calcu();
            //    _frm.Show();
            //}
        }
        #endregion 单一C扫描左右翻页
        Rectangle m_ScreenArea_2;

        private void Bt_V_Phone_Click(object sender, EventArgs e)
        {
            Photo_Video();
        }
        /// <summary>
        /// 拍照
        /// </summary>
        private void Bt_To_Phone()
        {
        //    Bt_V_Phone.Enabled = false;
            Photo_Video();
        //    Bt_V_Phone.Enabled = true;
        }
      
        /// <summary>
        /// 录像
        /// </summary>
        /// <param name="iType">1：开始 0：结束</param>
        private void Video(int iType)
        {
            //

            Time_Photo.Enabled = true;
            
            if (SysInfo.m_W_strItemName == "")
                SysInfo.m_Report_Para.m_Save_FilePath = SysInfo.m_W_i_FilePath + "\\";
            else
                SysInfo.m_Report_Para.m_Save_FilePath = SysInfo.m_W_i_FilePath + "\\" + SysInfo.m_W_strItemName + SysInfo.m_ItemMark + "\\";
           Lb_Photo.Text = "录像路径：" + SysInfo.m_Report_Para.m_Save_FilePath;
            Lb_Photo.Visible = true;
            //
            
            SysInfo.m_Report_Para.m_Save_FilePath = SysInfo.m_W_i_FilePath + "\\" + SysInfo.m_W_strItemName + SysInfo.m_ItemMark + "\\";
            //     int _iW = 2560;
            //      int _iH = 1440;

            //----
            string _NetP = Application.StartupPath + "\\database\\HardConfig.ini";

            int _iNo = int.Parse(SysInfo.csInter.IniReadDefine("Cam", "Cam_No", "1", _NetP));//拍照是第几个相机
            int _iW = int.Parse(SysInfo.csInter.IniReadDefine("Cam", "m_iWith_" + _iNo, "2304", _NetP));
            int _iH = int.Parse(SysInfo.csInter.IniReadDefine("Cam", "m_iHeight_" + _iNo, "1296", _NetP));

            string _strAdd = "|" + SysInfo.m_Report_Para.m_Save_FilePath + "|" + _iW.ToString() + "&" + _iH.ToString();
            SysInfo.m_ServerUI.SendData(SysInfo.m_W_strItemName + "_" + SysInfo.m_C_Item_Info.strGjbh + "|" + iType.ToString() + _strAdd, "3.0");



            //--

       
            //int _iNo = int.Parse(SysInfo.csInter.IniReadDefine("Cam", "Cam_No", "1", SysInfo.HardFileName));//拍照是第几个相机
            //_iW = int.Parse(SysInfo.csInter.IniReadDefine("Cam", "m_iWith_" + _iNo, "2560", SysInfo.HardFileName));
            //_iH = int.Parse(SysInfo.csInter.IniReadDefine("Cam", "m_iHeight_" + _iNo, "1440", SysInfo.HardFileName));

            //string _strAdd = "|" + SysInfo.m_Report_Para.m_Save_FilePath+"|"+_iW .ToString() +"&"+_iH .ToString ();
            //SysInfo.m_ServerUI.SendData(SysInfo.m_W_strItemName + "_" + SysInfo.m_C_Item_Info.strGjbh + "|" + iType.ToString()+ _strAdd, "3.0");
        }
        private void Photo_Video(string strCmd = "2.0")
        {
            if (SysInfo.m_ServerUI.m_blLink == false)
            {
                MessageBox.Show("相机连接异常");
                return;
            }
            if (SysInfo.m_bl_Show_Video == false)
            {
                Link_V();
            }
            Time_Photo.Enabled = true;
           
            if (SysInfo.m_W_strItemName == "")
                SysInfo.m_Report_Para.m_Save_FilePath = SysInfo.m_W_i_FilePath + "\\";
            else
                SysInfo.m_Report_Para.m_Save_FilePath = SysInfo.m_W_i_FilePath + "\\" + SysInfo.m_W_strItemName + SysInfo.m_ItemMark + "\\";
            Lb_Photo.Text = "拍照路径：" + SysInfo.m_Report_Para.m_Save_FilePath;
            string strT = "";
           Lb_Photo.Visible = true;
            strT = (SysInfo.m_C_Item_Info.i_C_AllRows + 1).ToString() + "|" +
                 SysInfo.m_Climb4.m_SysBuf.Trip_Com_mm.ToString("f0");

            SysInfo.m_ServerUI.SendData(SysInfo.m_W_strItemName + "_" + SysInfo.m_C_Item_Info.strGjbh + "|" + strT + "|" + SysInfo.m_Report_Para.m_Save_FilePath, strCmd);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;// WindowState.Minimized;
        }

        private void Bt_Win_Coat_Click(object sender, EventArgs e)
        {
        //    m_frm_Video.Close();
        //    this.WindowState = FormWindowState.Minimized;
        }

        private void Bt_Dc_Click(object sender, EventArgs e)
        {
            PowerLink();
        }

        private void Bt_V_Video_LuXiang_Click(object sender, EventArgs e)
        {
            int _iNo = int.Parse(SysInfo.csInter.IniReadDefine("Cam", "Cam_No", "1", SysInfo.HardFileName));//拍照是第几个相机
            int    m_i_Cam_With = int.Parse(SysInfo.csInter.IniReadDefine("Cam", "m_iWith_" + _iNo, "2304", SysInfo.HardFileName));
            int m_i_Cam_Heght = int.Parse(SysInfo.csInter.IniReadDefine("Cam", "m_iHeight_" + _iNo, "1296", SysInfo.HardFileName));

            Bt_To_Video(SysInfo.m_i_Video);
        }

        private void Bt_To_Video(int iType)
        {

            if (SysInfo.m_ServerUI.m_blLink == false)
            {
                MessageBox.Show("相机连接异常");
                return;
            }
            if (SysInfo.m_bl_Show_Video == false)
            {
                Link_V();
            }
            if (iType == 0)
            {
                SysInfo . m_i_Video = 1;
            //    Bt_V_Video_LuXiang.Text = "停止录像";
            //    Bt_V_Video_LuXiang.ForeColor = Color.Red;
            }
            else
            {
                SysInfo.m_i_Video = 0;
            //    Bt_V_Video_LuXiang.Text = "开始录像";
            //    Bt_V_Video_LuXiang.ForeColor = Color.White;
            }
            Video(SysInfo.m_i_Video);
        }

        private void Bt_Spark_Click(object sender, EventArgs e)
        {
         
        }

        private void Lb_Ver_DoubleClick(object sender, EventArgs e)
        {
       
        }

        private void Bt_Auto_D_Click(object sender, EventArgs e)
        {
           
        }

        private void Bt_Auto_A_Click(object sender, EventArgs e)
        {
           
        }
        private void SetAutoGain(int iCann=1000)
        {
          
        }
        private void SetAutoGain_2(int iCann = 1000)
        {
           
        }
        private void Track_AutoGain_Scroll(object sender, EventArgs e)
        {
            
        }

        private void Track_AutoGain_2_Scroll(object sender, EventArgs e)
        {
            
        }

        private void Bt_Auto_A_2_Click(object sender, EventArgs e)
        {
          
        }

        private void Bt_Auto_D_2_Click(object sender, EventArgs e)
        {
            
        }

        private void Bt_1_L_2_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void Bt_1_L_2_MouseMove(object sender, MouseEventArgs e)
        {
            Move_Wave_1_2_Tow(1, sender, e);
        }

        private void Bt_1_L_2_MouseUp(object sender, MouseEventArgs e)
        {
            Set_L_R_2(1);
        }

        private void Bt_1_R_2_MouseDown(object sender, MouseEventArgs e)
        {
          
        }

        private void Bt_1_R_2_MouseMove(object sender, MouseEventArgs e)
        {
            Move_Wave_1_2_Tow(2, sender, e);
        }

        private void Bt_1_R_2_MouseUp(object sender, MouseEventArgs e)
        {
            Set_L_R_2(1);
        }

        private void Bt_2_L_2_MouseDown(object sender, MouseEventArgs e)
        {
          
        }

        private void Bt_2_L_2_MouseMove(object sender, MouseEventArgs e)
        {
            Move_Wave_1_2_Tow(3, sender, e);
        }

        private void Bt_2_L_2_MouseUp(object sender, MouseEventArgs e)
        {
            Set_L_R_2(2);
        }

        private void Bt_2_R_2_MouseDown(object sender, MouseEventArgs e)
        {
          //  GetCurrX(Bt_2_R_2, e);
        }

        private void Bt_2_R_2_MouseMove(object sender, MouseEventArgs e)
        {
            Move_Wave_1_2_Tow(4, sender, e);
        }

        private void Bt_2_R_2_MouseUp(object sender, MouseEventArgs e)
        {
            Set_L_R_2(2);
        }

        private void Bt_Dis_Cs_Click(object sender, EventArgs e)
        {
            //

            SysInfo.m_Climb4.m_SysBuf.bl_Discharge_Txt = true;
            SysInfo.m_Climb4.m_SysBuf.i_Discharge_Txt_X = SysInfo.m_Climb4.m_SysBuf.Trip_Com_mm;
            SysInfo.m_Climb4.m_SysBuf.str_Discharge_Txt_Time = DateTime.Now.ToString("HH:mm:ss:fff");
        }

        private void Bt_Mark_Cs_Click(object sender, EventArgs e)
        {
          //
     
            SysInfo.m_Climb4.m_SysBuf.bl_Discharge_To_Mark_Txt = true;
            SysInfo.m_Climb4.m_SysBuf.i_Discharge_To_Mark_Txt_X = SysInfo.m_Climb4.m_SysBuf.Trip_Com_mm;
            SysInfo.m_Climb4.m_SysBuf.str_Discharge_To_Mark_Txt_Time = DateTime.Now.ToString("HH:mm:ss:fff");
        }

        private void Bt_V_Phone_MouseDown(object sender, MouseEventArgs e)
        {
          //  Bt_V_Phone.BackColor = Color.Red;
        }

        private void Bt_V_Phone_MouseUp(object sender, MouseEventArgs e)
        {
           // Bt_V_Phone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64))))); ;
        }

        private void Time_Photo_Tick(object sender, EventArgs e)
        {
            Time_Photo.Enabled = false;
            Lb_Photo.Visible = false ;
        }

        private void Pic_3_MouseMove(object sender, MouseEventArgs e)
        {
     //       SysInfo.Zoom_Tool(e.X, e.Y, true, 90, 1, Pic_Coat_All, Pic_3, true );
        }

        private void ck_2bei_CheckedChanged(object sender, EventArgs e)
        {
          
            //SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_csInter.INIWriteValue("System_Para", "m_bl_2BeiSjxx", SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_2BeiSjxx ? "1" : "0", SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.HardFileName);
        }

        private void Bt_Cmd_2_3_Mode_Click(object sender, EventArgs e)
        {
            
        }

        private void Time_Collect_Tick(object sender, EventArgs e)
        {
            Time_Collect.Enabled = false;
            //Bt_Prepare.Visible = false;
          //  Bt_BeginCollec.Visible = false  ;
        }

        private void Bt_Prepare_Click(object sender, EventArgs e)
        {
                 }

        private void button2_Click_2(object sender, EventArgs e)
        {
            SysInfo.m_Climb4.m_SysBuf.m_bl_Cs_Y = true ;
        }

        private void Ck_Larm_Click(object sender, EventArgs e)
        {
            Larm();
        }

        private void Bt_Laser_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
        }

        private void Bt_V_Chge_Click(object sender, EventArgs e)
        {
            Chg_Video();
        }

        private void Bt_B_L_Click(object sender, EventArgs e)
        {
            
        }

        private void Bt_Photo_Click(object sender, EventArgs e)
        {
            Photo_Video();
        }

        private void button2_Click_3(object sender, EventArgs e)
        {
            int _iT = SysInfo.m_i_Video;
            Video(_iT);
            //    
            button2.Text = SysInfo.m_i_Video == 0 ? "开始录像" : "停止录像";

            button2.ForeColor = SysInfo.m_i_Video == 1 ? Color.Red : Bt_Video.ForeColor = Color.Black ;
         SysInfo.m_i_Video = SysInfo.m_i_Video == 0 ? 1 : 0; }

        private void Txt_Gain_TextChanged(object sender, EventArgs e)
        {
            try
            {
                SysInfo.m_fl_Gain = float.Parse(Txt_Gain.Text == "" ? "5" : Txt_Gain.Text);
            }
            catch { Txt_Gain.Text = "5"; }

            SysInfo.csInter.INIWriteValue("System", "m_fl_Gain", SysInfo.m_fl_Gain.ToString(), SysInfo.HardFileName);
            if (SysInfo.m_blXunJi_Prog_0PC_1YY && SysInfo.m_Client.m_blLinkServe)
            {
                SysInfo.m_Client.SendData_C(6, SysInfo.m_fl_Gain + ",");
                // 关机 1
                //跟踪参数 2  
                // 激光器开关  3
                //9 曝光  4
                //10帧率  5
                //11 增益  6
                //12 日子  7
            }
        }

        private void Txt_Limt_L_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float _Fl = float.Parse(Txt_Limt_L.Text);
                if (_Fl < 0.1) Txt_Limt_L.Text = "0.1";
            }
            catch { Txt_Limt_L.Text = "0.4"; }
            SysInfo.csInter.INIWriteValue("System", "Txt_Limt_L", Txt_Limt_L.Text, SysInfo.HardFileName);
            if (SysInfo.m_blXunJi_Prog_0PC_1YY && SysInfo.m_Client.m_blLinkServe)
                Send_Jgq_Para();
        }
        private void Send_Jgq_Para()
        {
            if (SysInfo.m_blXunJi_Prog_0PC_1YY && SysInfo.m_Client.m_blLinkServe)
                SysInfo.m_Client.SendData_C(2, Txt_Limt_L.Text + "," + Txt_Limt_R.Text + "," +
                   SysInfo.m_i_WaitTimeNum_Max);// SysInfo.m_i_Bg_Time + "," + SysInfo.m_i_Frame_Num + "," +
                                                // 关机 1
                                                //跟踪参数 2  
                                                // 激光器开关  3
                                                //9 曝光  4
                                                //10帧率  5
                                                //11 增益  6
                                                //12 日子  7
        }
        private void Track_L_Scroll(object sender, EventArgs e)
        {
            Txt_Limt_L.Text = (Track_L.Value / 10f).ToString("f1");
        }

        private void Txt_Bg_TextChanged(object sender, EventArgs e)
        {
              try
            {
                SysInfo.m_i_Bg_Time = int.Parse(Txt_Bg.Text == "" ? "10" : Txt_Bg.Text);
            }
            catch { Txt_Bg.Text = "20"; }

            SysInfo.csInter.INIWriteValue("System", "m_i_Bg_Time", SysInfo.m_i_Bg_Time.ToString(), SysInfo.HardFileName);
            if (SysInfo.m_blXunJi_Prog_0PC_1YY && SysInfo.m_Client.m_blLinkServe)
            {
                    SysInfo.m_Client.SendData_C(4, SysInfo.m_i_Bg_Time + "," );
                // 关机 1
                //跟踪参数 2  
                // 激光器开关  3
                //9 曝光  4
                //10帧率  5
                //11 增益  6
                //12 日子  7
            }

        }

        private void Txt_Limt_R_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float _Fl = float.Parse(Txt_Limt_R.Text);
                if (_Fl < 0.1) Txt_Limt_R.Text = "0.1";
            }
            catch { Txt_Limt_R.Text = "0.4"; }

            SysInfo.csInter.INIWriteValue("System", "Txt_Limt_R", Txt_Limt_R.Text, SysInfo.HardFileName);
            if (SysInfo.m_blXunJi_Prog_0PC_1YY && SysInfo.m_Client.m_blLinkServe)
                Send_Jgq_Para();
        }

        private void Cmb_Ys_TextChanged(object sender, EventArgs e)
        {
            int _iWaitTime = 0;
            try
            {
                _iWaitTime = int.Parse(Cmb_Ys.Text);
                if (_iWaitTime < 0) Cmb_Ys.Text = "1";
            }
            catch { Cmb_Ys.Text = "3"; }
            SysInfo.m_i_WaitTimeNum_Max = _iWaitTime;
            SysInfo.csInter.INIWriteValue("System", "m_i_WaitTimeNum_Max", _iWaitTime.ToString(), SysInfo.HardFileName);
            if (SysInfo.m_blXunJi_Prog_0PC_1YY && SysInfo.m_Client.m_blLinkServe)
                Send_Jgq_Para();
        }

        private void Bt_R_A_Click(object sender, EventArgs e)
        {
            if (Track_R.Value < Track_R.Maximum)
            {
                Track_R.Value++;
                Txt_Limt_R.Text = (Track_R.Value / 10f).ToString("f1");
            }
        }

        private void Ck_Track_Auto_Click(object sender, EventArgs e)
        {
            SysInfo.m_bl_Track_Auto = Ck_Track_Auto.Checked;
            if (SysInfo.m_Climb4.m_blLink == false) return;
            if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                SysInfo.m_Climb4.SendData(1, 1, 8, 0, (Ck_Track_Auto.Checked ? "17" : "18"));//0x11：视觉寻迹行走打开     0x12：视觉寻迹行走关闭

        }

        private void Ck_First_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SysInfo.m_Ck_First = Ck_First.Checked ? 1 : 0;

            }
            catch { }

            SysInfo.csInter.INIWriteValue("System", "m_Ck_First", SysInfo.m_Ck_First.ToString(), SysInfo.HardFileName);
            if (SysInfo.m_blXunJi_Prog_0PC_1YY && SysInfo.m_Client.m_blLinkServe)
            {
                SysInfo.m_Client.SendData_C(8, SysInfo.m_Ck_First + ",");
                // 关机 1
                //跟踪参数 2  
                // 激光器开关  3
                //9 曝光  4
                //10帧率  5
                //11 增益  6
                //12 日子  7
            }
        }

        private void Ck_Ri_Zhi_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SysInfo.m_i_Lars_RiZhi = Ck_Ri_Zhi.Checked ? 1 : 0;
                SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_Xj_Write = Ck_Ri_Zhi.Checked;
            }
            catch { }

            SysInfo.csInter.INIWriteValue("System", "m_i_Lars_RiZhi", SysInfo.m_i_Lars_RiZhi.ToString(), SysInfo.HardFileName);
            if (SysInfo.m_blXunJi_Prog_0PC_1YY && SysInfo.m_Client.m_blLinkServe)
            {
                SysInfo.m_Client.SendData_C(7, SysInfo.m_i_Lars_RiZhi + ",");
                // 关机 1
                //跟踪参数 2  
                // 激光器开关  3
                //9 曝光  4
                //10帧率  5
                //11 增益  6
                //12 日子  7
            }
        }

        private void Ck_Bw1_Tx0_Click(object sender, EventArgs e)
        {
            Bw_Tx(Ck_Bw1_Tx0.Checked);
        }
        private void Bw_Tx(bool blVal)
        {
            Txt_Com.Visible = blVal;
            Lb_LunKuo.Visible = !blVal;

        }

        private void Ck_Larser_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_blXunJi_Prog_0PC_1YY && SysInfo.m_Client.m_blLinkServe)
            {
                //SysInfo.m_bl_Larser_UP1_Down0 = Ck_Larser.Checked;
                //SysInfo.m_Client.SendData_C(3, Ck_Larser.Checked ?"1":"0");

                SysInfo.csInter.INIWriteValue("Larser", "m_bl_Larser_UP1_Down0", (SysInfo.m_bl_Larser_UP1_Down0 ? "1" : "0"), Application.StartupPath + "\\database\\HardConfig.ini");
                // 关机 1
                //跟踪参数 2  
                // 激光器开关  3
                //9 曝光  4
                //10帧率  5
                //11 增益  6
                //12 日子  7
            }
        }
        int m_i_Lasers_O_C = 1;
        private void Bt_Lasers_O1_C0_Click(object sender, EventArgs e)
        {
            m_i_Lasers_O_C = m_i_Lasers_O_C == 1 ? 0 : 1;
            if (SysInfo.m_blXunJi_Prog_0PC_1YY && SysInfo.m_Client.m_blLinkServe)
            {
                SysInfo.m_Client.SendData_C(3, m_i_Lasers_O_C.ToString());
                Bt_Lasers_O1_C0.Text = m_i_Lasers_O_C == 0 ? "开灯" : "关灯";
                // 关机 1
                //7跟踪参数 2  
                //8 激光器开关  3
                //9 曝光  4
                //10帧率  5
                //11 增益  6
                //12 日子  7
            }
        }

        private void Pan_V_Click(object sender, EventArgs e)
        {
            Lb_Init.Visible = false;
        }

        private void Lb_Ctrl_Click(object sender, EventArgs e)
        {

        }

        private void Bt_Win_Coat_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bt_Item_T_Click_1(object sender, EventArgs e)
        {

        }
        //相机重启
        private void button6_Click_1(object sender, EventArgs e)
        {
            Run_Video();
        }


        /// <summary>
        /// 任务栏隐藏显示调整界面高度
        /// </summary>
        /// <param name="blH"></param>
        private void Win_Task_H_S(bool blH)
        {
            //这个区域不包括任务栏的
            Rectangle ScreenArea_1 = System.Windows.Forms.Screen.GetWorkingArea(this);
            //这个区域包括任务栏，就是屏幕显示的物理范围
            m_ScreenArea_2 = System.Windows.Forms.Screen.GetBounds(this);
            //
         
            if (blH)
                this.Height = m_ScreenArea_2.Height;// this.Height - 10;// Screen.PrimaryScreen.WorkingArea.Height - 10;
            else
                this.Height = ScreenArea_1.Height + 7;// Screen.PrimaryScreen.WorkingArea.Height - 10;
           
            this.Left = -10;
            //界面自动调整D图高度     ScreenUpChange();
        }
        #endregion  注册
    }

    public class Compression
    {
        /// <summary>  
        /// 对字符串进行压缩  
        /// </summary>  
        /// <param name="str">待压缩的字符串</param>  
        /// <returns>压缩后的字符串</returns>  
        public static string CompressString(string str)
        {
            string compressString = "";
            byte[] compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(str);
            byte[] compressAfterByte = Compress(compressBeforeByte);
            //compressString = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);  
            compressString = Convert.ToBase64String(compressAfterByte);
            return compressString;
        }
        /// <summary>  
        /// 对字符串进行解压缩  
        /// </summary>  
        /// <param name="str">待解压缩的字符串</param>  
        /// <returns>解压缩后的字符串</returns>  
        public static string DecompressString(string str)
        {
            string compressString = "";
            //byte[] compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(str);  
            byte[] compressBeforeByte = Convert.FromBase64String(str);
            byte[] compressAfterByte = Decompress(compressBeforeByte);
            compressString = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);
            return compressString;
        }
        /// <summary>  
        /// 对文件进行压缩  
        /// </summary>  
        /// <param name="sourceFile">待压缩的文件名</param>  
        /// <param name="destinationFile">压缩后的文件名</param>  
        public static void CompressFile(string sourceFile, string destinationFile)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>  
        /// 对文件进行解压缩  
        /// </summary>  
        /// <param name="sourceFile">待解压缩的文件名</param>  
        /// <param name="destinationFile">解压缩后的文件名</param>  
        /// <returns></returns>  
        public static void DecompressFile(string sourceFile, string destinationFile)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>  
        /// 对byte数组进行压缩  
        /// </summary>  
        /// <param name="data">待压缩的byte数组</param>  
        /// <returns>压缩后的byte数组</returns>  
        public static byte[] Compress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(data, 0, data.Length);
                zip.Close();
                byte[] buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
                ms.Close();
                return buffer;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static byte[] Decompress(byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream(data);
                GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
                MemoryStream msreader = new MemoryStream();
                byte[] buffer = new byte[0x1000];
                while (true)
                {
                    int reader = zip.Read(buffer, 0, buffer.Length);
                    if (reader <= 0)
                    {
                        break;
                    }
                    msreader.Write(buffer, 0, reader);
                }
                zip.Close();
                ms.Close();
                msreader.Position = 0;
                buffer = msreader.ToArray();
                msreader.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
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
        /// 多个客户端
        /// </summary>
        public TcpClient[] m_TcpArr = new TcpClient[2];
        /// <summary>
        /// 通讯线程
        /// </summary>
        Thread m_trdServer = null;

        Thread[] m_treaArr = new Thread[2];
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
            /*
            Thread myThread = new Thread(new ParameterizedThreadStart(printReceiveMsg));
            myThread.Start(client);
             */
            try
            {
                m_TcpServer.Start();
                SysInfo.csInter.WriteErrorLog(SysInfo.m_Log_Main, "相机服务器启动...");
                //             Run_Client();m_ServerUI
                while (true && SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun != 10)//
                {
                    if (m_TcpServer.Pending())
                    {
                        //if (m_TcpC != null)
                        //    m_TcpC.Close();
                        TcpClient _TcpC = null;
                        _TcpC = m_TcpServer.AcceptTcpClient();
                        //MessageBox.Show("客户端收到");
                        // if (m_trdServer != null) m_trdServer.Abort();
                        Thread _trdServer = new Thread(new ParameterizedThreadStart(AcceptClientMsg));
                        _trdServer.Start(_TcpC);
                        _trdServer.IsBackground = true;

                        Thread.Sleep(100);
                        Application.DoEvents();
                        //   break;
                    }
                }
            }
            catch (Exception Err)
            {
            }
        }

        private void StartListen_Mul()
        {
            try
            {
                m_TcpServer.Start();
                //             Run_Client();m_ServerUI
                while (true)//
                {
                    if (m_TcpServer.Pending())
                    {
                        TcpClient _TcpC = null;

                        m_TcpC = null;
                        _TcpC = m_TcpServer.AcceptTcpClient();
                        String _t = _TcpC.Client.RemoteEndPoint.ToString();

                        Socket s = _TcpC.Client;
                        Console.WriteLine("I am connected to " +
                System.Net.IPAddress.Parse(((System.Net.IPEndPoint)s.RemoteEndPoint).Address.ToString()) +
                        "on port number " + ((System.Net.IPEndPoint)s.RemoteEndPoint).Port.ToString());

                        //  m_TcpArr.Add(_TcpC);
                        //MessageBox.Show("客户端收到");
                        if (m_trdServer != null) m_trdServer.Abort();
                        m_trdServer = new Thread(new ParameterizedThreadStart(AcceptClientMsg));
                        m_trdServer.Start(_TcpC);
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
            string _strRet = "";
            if (TcpC != null)
            {
                NetworkStream ns = TcpC.GetStream();
                while (TcpC.Connected == true && SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun != 10)
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
                                    //  NetParsing(strMsg);//
                                    _strRet = ExplainClientMsg(strMsg);
                                    if (strMsg.IndexOf("19.0") > 0)
                                    {
                                        switch (_strRet)
                                        {
                                            case "1"://视频客户端
                                                SysInfo.csInter.WriteErrorLog(SysInfo.m_Log_Main, "收到相机客户端连接确认消息");
                                                m_TcpArr[0] = new TcpClient();
                                                m_TcpArr[0] = TcpC;

                                                break;
                                            case "2"://tofd波形客户端
                                                SysInfo.csInter.WriteErrorLog(SysInfo.m_Log_Main, "收到TOFD客户端连接确认消息");
                                                m_TcpArr[1] = new TcpClient();
                                                m_TcpArr[1] = TcpC;
                                                break;
                                        }
                                    }
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
            if (sPara.Length > 3)
            {
                int _iS = strRetDat.IndexOf("/main>");
                strRetDat = strRetDat.Substring(0, _iS + 6);
                sPara = strRetDat.Split('[');
            }
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
                        if (_sPara[0] == "8.0")
                        {
                            //重新启动
                            SysInfo.g_Msg_InterFace.Fun_ReLink_Videl();//SysInfo.m_SysInfo.g_Video.dt_GetDataTime = new DateTime();
                        }
                        if (_sPara[0] == "10.0")//数据类型 1.0：数据格式： 视频帧号标志 / 视频帧号
                        {
                            try
                            {
                                //  SysInfo.m_SysInfo.g_Climb.m_iVideo_No = int.Parse(_sPara[1]);

                                //    SysInfo.g_Msg_InterFace.Fun_RunInfo("3#" + _sPara[1]);
                                try
                                {
                                    SysInfo.strNetTrue_Video = _sPara[1];
                                    SysInfo.m_ServerUI.SendData_M(0, "", "7.0");
                                }
                                catch { }


                                m_blRecev = true;
                            }
                            catch (Exception ee
                            )
                            { m_blRecev = false; }
                        }
                        if (_sPara[0] == "18.0")//
                        {
                            //SysInfo.m_SysInfo.g_Video.dt_GetDataTime = DateTime.Now;
                            //SysInfo.m_SysInfo.g_Video.m_iRunning = 2;
                        }
                        if (_sPara[0] == "19.0") //客户端类型
                        {
                            if (_sPara.Length == 2)
                            {
                                switch (int.Parse(_sPara[1]))
                                {
                                    case 1://视频客户端
                                        strRet = "1";
                                        break;
                                    case 2://TOFD波形客户端
                                        strRet = "2";
                                        break;
                                }
                            }
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
            //  <main>[1:201[</main>

            TcpClient tcpC = (TcpClient)m_TcpArr[0];
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
        public void SendData_M(int iNo, string strDat, string strKey = "1.0")
        {
            if (m_TcpArr != null && iNo < m_TcpArr.Length)
            {
                if (m_TcpArr[iNo] == null) return;

                string strSendMsg = strKey + "/" + strDat; //发送数据: 信息类型 / 数据
                string strSend = S_Head + "[" + strSendMsg + "[" + S_Tail;
                TcpClient tcpC = (TcpClient)m_TcpArr[iNo];
                if (tcpC != null)
                {
                    if (tcpC.Connected)
                    {
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
            else
            {
            }
        }
        /// <summary>
        /// 发送波形数据
        /// </summary>
        /// <param name="iTcpNo">1:固定TOFD的A扫描成像客户端  0：视频客户端</param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public bool SendData_M(int iTcpNo, byte[] Data, int iDataLen)
        {
            bool _blRet = false;
            int _iLen = 1 + iDataLen + 3;//头 1 +  尾部3
            byte[] _SendDat = new byte[_iLen];
            //1 组帧
            _SendDat[0] = 89;//数据

            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_SendDat, 1);

            Marshal.Copy(Data, 0, IntPtArr, iDataLen);
            _SendDat[_iLen - 1] = 0xFE;
            _SendDat[_iLen - 2] = 0xFE;
            _SendDat[_iLen - 3] = 0xFE;

            int _i8 = _SendDat[_iLen - 7];
            int _i7 = _SendDat[_iLen - 6];
            int _i6 = _SendDat[_iLen - 5];
            int _i5 = _SendDat[_iLen - 4];

            //2 压缩
            //  GZip.GZIPCompress(ref _SendDat);
            //3 发送
            SendData(iTcpNo, _SendDat);
            return _blRet;
        }
        /// <summary>
        /// 发送字节数据：波形数据
        /// </summary>
        /// <param name="iTcpNo"></param>
        /// <param name="SendMsg"></param>
        private void SendData(int iTcpNo, byte[] SendMsg)
        {
            if (m_TcpArr[iTcpNo] == null) return;
            TcpClient tcpC = (TcpClient)m_TcpArr[iTcpNo];
            if (tcpC != null)
            {
                int _iLen = SendMsg.Length;
                int _i8 = SendMsg[_iLen - 7];
                int _i7 = SendMsg[_iLen - 6];
                int _i6 = SendMsg[_iLen - 5];
                int _i5 = SendMsg[_iLen - 4];
                int _i4 = SendMsg[_iLen - 1];
                if (tcpC.Connected)
                {
                    NetworkStream ns = tcpC.GetStream();
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
    ///// <summary>
    ///// 视频服务器
    ///// </summary>
    //public class Class_Server_UI
    //{
    //    /// <summary>
    //    /// 联机上客户端
    //    /// </summary>
    //    public bool m_blLink = false;
    //    /// <summary>
    //    /// 是否发送数据
    //    /// </summary>
    //    public bool m_blSend = false;
    //    /// <summary>
    //    /// 接收到数据
    //    /// </summary>
    //    public bool m_blRecev = false;

    //    #region 数据帧格式
    //    /// <summary>
    //    /// 接收客户端数据帧头<clie>
    //    /// </summary>
    //    public string C_Head = "<clie>";
    //    /// <summary>
    //    /// 接收客户端数据帧尾 </clie>
    //    /// </summary>
    //    public string C_Tail = "</clie>";
    //    /// <summary>
    //    /// 服务器发送帧头<main>
    //    /// </summary>
    //    public string S_Head = "<main>";
    //    /// <summary>
    //    /// 服务器发送帧尾</main>
    //    /// </summary>
    //    public string S_Tail = "</main>";
    //    #endregion
    //    /// <summary>
    //    /// 服务器
    //    /// </summary>
    //    public TcpListener m_TcpServer = null;
    //    /// <summary>
    //    /// 客户端
    //    /// </summary>
    //    public TcpClient m_TcpC = null;
    //    /// <summary>
    //    /// 通讯线程
    //    /// </summary>
    //    Thread m_trdServer = null;
    //    /// <summary>
    //    /// 解析网络数据
    //    /// </summary>
    //    Thread m_trd_Parsing = null;
    //    /// <summary>
    //    /// 终端服务器监听线程
    //    /// </summary>
    //    private Thread TreadTcp_Server;

    //    ///// <summary>
    //    ///// 启动涡流程序
    //    ///// </summary>
    //    //public void Run_Client()
    //    //{
    //    //    Thread tdS = new Thread(new ThreadStart(LoadClientProgram));
    //    //    tdS.Start();
    //    //    tdS.IsBackground = true;
    //    //}
    //    /// <summary>
    //    /// 关闭客户端
    //    /// </summary>
    //    public void End_Client()
    //    {
    //        SendData("", "99");
    //    }
    //    //private void LoadClientProgram()
    //    //{
    //    //    System.Diagnostics.Process pAppInterface = new System.Diagnostics.Process();
    //    //    pAppInterface.StartInfo.FileName = Application.StartupPath + "\\Auto_Pulsed_Eddy.exe";
    //    //    pAppInterface.Start();
    //    //    pAppInterface.Close();
    //    //}

    //    /// <summary>
    //    /// 启动服务器监听的方式
    //    /// </summary>
    //    /// <param name="strServIp">127.0.0.1</param>
    //    /// <param name="Port">48100</param>
    //    public void Start(string strServIp = "127.0.0.1", int Port = 48100)
    //    {
    //        try
    //        {
    //            if (TreadTcp_Server != null) TreadTcp_Server.Abort();
    //            if (m_TcpServer != null) m_TcpServer.Stop();

    //            m_TcpServer = new TcpListener(System.Net.IPAddress.Parse(strServIp == "" ? "127.0.0.1" : strServIp), Port);
    //            TreadTcp_Server = new Thread(new ThreadStart(StartListen));
    //            TreadTcp_Server.Start();
    //            TreadTcp_Server.IsBackground = true;
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //    }
    //    private void StartListen()
    //    {
    //        try
    //        {
    //            m_TcpServer.Start();
    //            //             Run_Client();
    //            while (true)//
    //            {
    //                if (m_TcpServer.Pending())
    //                {
    //                    if (m_TcpC != null)
    //                        m_TcpC.Close();
    //                    m_TcpC = null;
    //                    m_TcpC = m_TcpServer.AcceptTcpClient();
    //                    //MessageBox.Show("客户端收到");
    //                    if (m_trdServer != null) m_trdServer.Abort();
    //                    m_trdServer = new Thread(new ParameterizedThreadStart(AcceptClientMsg));
    //                    m_trdServer.Start(m_TcpC);
    //                    m_trdServer.IsBackground = true;

    //                    Thread.Sleep(100);
    //                    Application.DoEvents();
    //                    break;
    //                }
    //            }
    //        }
    //        catch (Exception Err)
    //        {
    //        }
    //    }
    //    private void NetParsing(string strMsg)//陈大伟WWW
    //    {
    //        if (m_trd_Parsing != null) m_trd_Parsing.Abort();
    //        m_trd_Parsing = new Thread(new ParameterizedThreadStart(Parsing));
    //        m_trd_Parsing.Start(strMsg);
    //        m_trd_Parsing.IsBackground = true;
    //    }
    //    private void Parsing(object ObjMsg)
    //    {
    //        string _strMsg = (string)ObjMsg;
    //        ExplainClientMsg(_strMsg);
    //    }
    //    private void AcceptClientMsg(object arg)
    //    {
    //        TcpClient TcpC = (TcpClient)arg;
    //        m_blLink = false;
    //        string strMsg = "";

    //        if (TcpC != null)
    //        {
    //            NetworkStream ns = TcpC.GetStream();
    //            while (TcpC.Connected == true)
    //            {
    //                try
    //                {
    //                    Thread.Sleep(10);
    //                    m_blLink = true;
    //                    int num = TcpC.Available;
    //                    if (num > 0)
    //                    {
    //                        //1 接收
    //                        byte[] Msg = new byte[num];
    //                        int Count = TcpC.Client.Receive(Msg);

    //                        if (Msg != null && Msg.Length > 0)
    //                        {
    //                            //2 拿返回数据
    //                            strMsg = Encoding.Default.GetString(Msg, 0, Msg.Length);
    //                            if (strMsg.Length > 0)
    //                            {
    //                                NetParsing(strMsg);// ExplainClientMsg(strMsg);
    //                            }
    //                        }
    //                    }
    //                }
    //                catch
    //                {

    //                    //WriteErrorLog(ee.Message);
    //                    // return;
    //                }
    //                Thread.Sleep(100);
    //                Application.DoEvents();
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// 数据解析
    //    /// </summary>
    //    /// <param name="strMsg"></param>
    //    /// <returns></returns>
    //    private string ExplainClientMsg(string strRetDat)
    //    {
    //        string strRet = "";

    //        #region 依据帧头 帧尾 截取有效数据
    //        int _iT = strRetDat.IndexOf(C_Head);

    //        strRetDat = strRetDat.Substring(_iT);
    //        _iT = strRetDat.IndexOf(C_Tail);
    //        if (_iT > -1)
    //            strRetDat = strRetDat.Substring(0, _iT + C_Tail.Length);
    //        #endregion 

    //        string[] sPara = strRetDat.Split('[');
    //        //数据格式：<main/clie> + "[" + 数据 + "[" + </main/clie>  数据格式：数据类型 / 数据内容
    //        //例如：<clie> + "[" + 1.0/235432 + "[" + </clie>
    //        if (sPara.Length == 3)
    //        {
    //            //2 确认包
    //            if (sPara[0].ToLower() == C_Head && sPara[2].ToLower() == C_Tail)
    //            {
    //                string[] _sPara = sPara[1].Split('/'); //数据格式：数据类型 / 数据内容
    //                if (_sPara.Length == 2)
    //                {
    //                    if (_sPara[0] == "4.0")//数据类型 1.0：数据格式： 视频帧号标志 / 视频帧号
    //                    {
    //                        try
    //                        {
    //                            //SysInfo.m_SysInfo.g_Climb.m_iVideo_No = int.Parse(_sPara[1]);
    //                            //SysInfo.g_Msg_InterFace.Fun_RunInfo("3#" + SysInfo.m_SysInfo.g_Climb.m_iVideo_No.ToString());
    //                            //if (SysInfo.m_SysInfo.g_Video.blNetTrue == false)
    //                            //    SysInfo.m_SysInfo.g_Video.blNetTrue = true;
    //                          //  SysInfo.blNetTrue_Video = true;
    //                            m_blRecev = true;
    //                        }
    //                        catch (Exception ee
    //                        )
    //                        { m_blRecev = false; }
    //                    }
    //                    if (_sPara[0] == "7.0")
    //                    {
    //                        if (int.Parse(_sPara[1]) == 1)
    //                        {//重新启动
    //                          //  SysInfo.g_Msg_InterFace.Fun_ReLink_Phone();//SysInfo.m_SysInfo.g_Video.dt_GetDataTime = new DateTime();
    //                        }
    //                    }
    //                    if (_sPara[0] == "10.0")//数据类型 1.0：数据格式： 视频帧号标志 / 视频帧号
    //                    {
    //                        try
    //                        {
    //                            //  SysInfo.m_SysInfo.g_Climb.m_iVideo_No = int.Parse(_sPara[1]);

    //                            //    SysInfo.g_Msg_InterFace.Fun_RunInfo("3#" + _sPara[1]);
    //                            try
    //                            {
    //                                SysInfo.iNetTrue_Video = int.Parse(_sPara[1]);
    //                            }
    //                            catch { }


    //                            m_blRecev = true;
    //                        }
    //                        catch (Exception ee
    //                        )
    //                        { m_blRecev = false; }
    //                    }
    //                    if (_sPara[0] == "18.0")//心跳  1次/30帧
    //                    {
    //                        //SysInfo.m_SysInfo.g_Video.dt_GetDataTime = DateTime.Now;
    //                        //SysInfo.m_SysInfo.g_Video.m_iRunning = 2;
    //                    }
    //                }
    //            }
    //        }
    //        return strRet;
    //    }
    //    /// <summary>
    //    /// 数据解析
    //    /// </summary>
    //    /// <param name="strMsg"></param>
    //    /// <returns></returns>
    //    private string ExplainClientMsg_Old(string strRetDat)
    //    {
    //        string strRet = "";

    //        #region 依据帧头 帧尾 截取有效数据
    //        int _iT = strRetDat.IndexOf(C_Head);

    //        strRetDat = strRetDat.Substring(_iT);
    //        _iT = strRetDat.IndexOf(C_Tail);
    //        if (_iT > -1)
    //            strRetDat = strRetDat.Substring(0, _iT + C_Tail.Length);
    //        #endregion 

    //        string[] sPara = strRetDat.Split('[');
    //        //数据格式：<main/clie> + "[" + 数据 + "[" + </main/clie>  数据格式：数据类型 / 数据内容
    //        //例如：<clie> + "[" + 1.0/235432 + "[" + </clie>
    //        if (sPara.Length == 3)
    //        {
    //            //2 确认包
    //            if (sPara[0].ToLower() == C_Head && sPara[2].ToLower() == C_Tail)
    //            {
    //                string[] _sPara = sPara[1].Split('/'); //数据格式：数据类型 / 数据内容
    //                if (_sPara.Length == 2)
    //                {
    //                    if (_sPara[0] == "4.0")//数据类型 1.0：数据格式： 视频帧号标志 / 视频帧号
    //                    {
    //                        try
    //                        {
    //                            //SysInfo.m_SysInfo.g_Climb.m_iVideo_No = int.Parse(_sPara[1]);
    //                            //SysInfo.g_Msg_InterFace.Fun_RunInfo("3#" + SysInfo.m_SysInfo.g_Climb.m_iVideo_No.ToString());
    //                            //if (SysInfo.m_SysInfo.g_Video.blNetTrue == false || m_blRecev == false)
    //                            //    SysInfo.m_SysInfo.g_Video.blNetTrue = true;
    //                           // SysInfo.blNetTrue_Video = true;
    //                            m_blRecev = true;
    //                        }
    //                        catch (Exception ee
    //                        )
    //                        { m_blRecev = false; }
    //                    }
    //                }
    //            }
    //        }
    //        return strRet;
    //    }
    //    /// <summary>
    //    /// 发送数据   拍照数据格式  2.0  / 文件名 | 行号  | 位置 | 检验员 | 1（开始拍照）   录像数据格式  3.0  / 文件名 | 录像开始1/接收0
    //    /// </summary>
    //    /// <param name="strKey">1.0: 系统信息 2.0:拍照 3.0:录像 99：退出程序</param>
    //    /// <param name="strDat">拍照数据格式  2.0  / 文件名 | 行号  | 位置 | 检验员 | 1（开始拍照）   录像数据格式  3.0  / 文件名 | 录像开始1/接收0 99: 关闭程序</param>
    //    public void SendData(string strDat, string strKey = "1.0")
    //    {
    //        string strSendMsg = strKey + "/" + strDat; //发送数据: 信息类型 / 数据
    //        string strSend = S_Head + "[" + strSendMsg + "[" + S_Tail;
    //        TcpClient tcpC = (TcpClient)m_TcpC;
    //        if (tcpC != null)
    //        {
    //            if (tcpC.Connected)
    //            {
    //                //if (strKey == "1.0")
    //                //{
    //                //    m_blRecev = false;
    //                //}
    //                NetworkStream ns = tcpC.GetStream();
    //                byte[] SendMsg = Encoding.Default.GetBytes(strSend);
    //                try
    //                {
    //                    ns.Write(SendMsg, 0, SendMsg.Length);
    //                    ns.Flush();
    //                }
    //                catch (Exception e)
    //                { }
    //            }
    //        }
    //    }
    //    //public void Get_Data()
    //    //{
    //    //    DateTime dtStar = DateTime.Now;
    //    //    while (m_blSend)
    //    //    {
    //    //        if (m_blRecev)
    //    //        {
    //    //           // if (m_flThick_Per > 0)
    //    //            {
    //    //                break;
    //    //            }
    //    //        }
    //    //        if (DateTime.Now.Subtract(dtStar).TotalSeconds > 4) break;
    //    //        //  Application.DoEvents();
    //    //        Thread.Sleep(10);
    //    //    }
    //    //}
    //    /// <summary>
    //    /// 关闭网络
    //    /// </summary>
    //    public void Close()
    //    {
    //        if (TreadTcp_Server != null) TreadTcp_Server.Abort();
    //        if (m_TcpServer != null) m_TcpServer.Stop();
    //        m_blLink = false;

    //        if (m_TcpC != null)
    //        {
    //            if (m_TcpC.Connected)
    //            {
    //                m_TcpC.Close();
    //            }
    //        }
    //        //3 关闭线程
    //        if (m_trdServer != null) m_trdServer.Abort();
    //        m_trdServer = null;
    //    }
    //}
}