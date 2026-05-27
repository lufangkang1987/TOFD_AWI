using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tofd_AWI.Class;
using Frame_Work;
using AnyCardInterface;
//using Tofd_DLL;
//using EM_RIT;
using Microsoft.VisualBasic;
namespace Tofd_AWI.From
{
    public partial class Frm_TOFD : Form
    {
        /// <summary>
        /// 是否修改颜色
        /// </summary>
        bool m_bl_Color_Mdf = false;
        string m_NetP = Application.StartupPath + "\\database\\SysConfig.ini";
        string m_Net_V = Application.StartupPath + "\\database\\HardConfig.ini";
        /// <summary>
        /// B扫描幅度对应颜色的数量个数
        /// </summary>
        int m_i_Stand_Num = 0;
        /// <summary>
        /// 工艺文件个数
        /// </summary>
        int m_Craft_iNum = 0;
        /// <summary>
        /// 窗体是否激活
        /// </summary>
        bool m_blActive = false;

        public Frm_TOFD()
        {
            InitializeComponent();
        }

        private void Bt_Link_Click(object sender, EventArgs e)
        {
            Lb_Send.Text = "联机TOFD...";
            bool _blRet = SysInfo.m_SysBuff.m_Tofd_DLL.Link();
            Lb_Send.Text = "联机TOFD: 结束";
            Lb_Link.Text = _blRet ? "成功" : "失败";
            Lb_Link.ForeColor = _blRet ? Color.Green : Color.Red;
        }

        private void Frm_TOFD_Load(object sender, EventArgs e)
        {
            Init_Video();
            groupBox1.Visible = true;
            tabControl1.TabPages.Clear();
           
            Txt_RecDisc_Time.Text =  SysInfo.m_i_Add_MarkDisc.ToString();

           

            tabPage2.Text  = SysInfo.m_iLanguage == 0 ? "系统参数" : "System parameter";
            tabControl1.TabPages.Add(tabPage2);//系统配置
           
            if ( SysInfo.m_iLanguage == 1)
            {
                this.Text = "Parameter setting";
                Cmb_UI_DLL_Type.Items.Clear();
                Cmb_UI_DLL_Type.Items.Add("Henan Dellon");

                Cmb_UI_Type.Items.Clear();
                Cmb_UI_Type.Items.Add("Zk");
                Cmb_UI_Type.Items.Add("Lw");
            }
            InitFrm();
            UI_IP_Init();
            UI_IP_Init_Coat();
            UI_IP_Init_Ect();
       
            Ck_Begin_Down.Checked = SysInfo.m_SysBuff.m_Climb.i_Begin_Down == 1;
            Lb_Send.Text = SysInfo.m_iLanguage == 0 ? "TOFD 联机: 结束" : "TOFD online:End";
            Lb_Link.Text = SysInfo.m_SysBuff.m_Tofd_DLL.blNetLink ? (SysInfo.m_iLanguage == 0 ? "成功": "success") :
                (SysInfo.m_iLanguage == 0 ? "失败": "failure");
            Lb_Link.ForeColor = SysInfo.m_SysBuff.m_Tofd_DLL.blNetLink ? Color.Green : Color.Red;

            Rad_C.Checked = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1;
            Rad_Tofd.Checked = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0;
            Rad_M_UI.Checked = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2;
            Rad_Coat.Checked = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3;

            
          //  Cmb_UI_DLL_Type.Visible = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 0;



           if ( SysInfo.m_iLanguage == 1)
            SysInfo.Language(this, "Frm_TOFD");
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0)
                Grp_Stand_Color.Text = SysInfo.m_iLanguage==0? "1 B扫描厚度误差对应颜色" : "1 B scanning thickness error corresponds to color";
            else if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)
                Grp_Stand_Color.Text = SysInfo.m_iLanguage == 0 ? "1 B/C扫描厚度误差对应颜色" : "1 B/C scanning thickness error corresponds to color";

            InitColor(SysInfo .m_str_B_Stand_Color);
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 0)
                m_i_Stand_Num = SysInfo.m_SysBuff.Txt_Wc_Num;
            else
                m_i_Stand_Num = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iDemodulation_Flag != 2 ? 256 : 128;
            SysInfo.InitLimitPic(SysInfo.m_str_B_Stand_Color,   m_i_Stand_Num, P_1, P_Md, P_2, Grp_Stand_Color);
           
            Set_Pan_T_C();
            SysInfo.m_blFrmOpen[0] = true;
            m_blActive = true;

            if (Txt_m_i_Alarm_Limit.Visible)
            {
                Set_Alarm(false);
            }
         
            Txt_m_i_Alarm_Limit.Enabled = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0;
            Bt_Alarm_D.Enabled = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0;
            Bt_Alarm_A.Enabled = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0;
            Grp_UDP.Top = 20;
            Bt_Osk.Top = Bt_IP_Visi.Top;
            Ect_P();
            Ck_Have_TOFD.Visible = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0;
            Ck_Have_TOFD.Checked = SysInfo.csInter.INIReadValue("TOFD", "Have", (Ck_Have_TOFD.Checked ? "1" : "0"), SysInfo.HardFileName) == "1";
       
            string    _strT = SysInfo.csInter.IniReadDefine("ClassUltrasGate", "Txt_Mul_Select", "", System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini");
          

        }
        private void Set_Cmd_Vis()
        {
           
            if(SysInfo.m_i_TOFD_0_Cscan_1_Mui_2==0)
            {
                Cmb_UI_DLL_Type.Visible = false ;
                label45.Visible = false ;

            }
        }
        private void Set_Alarm(bool blVal)
        {
            Txt_m_i_Alarm_Limit.Visible = blVal;
            Lb_Alrm.Visible = Txt_m_i_Alarm_Limit.Visible;// Txt_m_i_Alarm_Limit.Visible;
            Bt_Alarm_A.Visible = Txt_m_i_Alarm_Limit.Visible;
            Bt_Alarm_D.Visible = Txt_m_i_Alarm_Limit.Visible;

        }
        /// <summary>
        /// 读帮助文件
        /// </summary>
       
        private void Set_Pan_T_C()
        {
           
            Bt_IP_Visi.Visible = true;//  SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 0;
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0)
            {
                Rad_C_D.Visible = false;
                Rad_C_A.Visible = false;
            }
            Grp_Coat.Visible = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3;

            Pan_Tofd.Visible = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0;
            Ck_Alarm_Data.Visible = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 0;
            Bt_Copy.Visible = !Pan_Tofd.Visible;
            Rad_C_D.Visible = Bt_Copy.Visible;
            Rad_C_A.Visible = Bt_Copy.Visible;
         //   Txt_WaitTims.Enabled = Pan_Tofd.Visible;

            Txt_m_i_Alarm_Limit.Visible  = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0;
            Lb_Alrm.Visible = Txt_m_i_Alarm_Limit.Visible;
            Bt_Alarm_A.Visible = Txt_m_i_Alarm_Limit.Visible;
            Bt_Alarm_D.Visible = Txt_m_i_Alarm_Limit.Visible;

            //Cmb_UI_DLL_Type.Visible = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 0;
            //label45.Visible = Cmb_UI_DLL_Type.Visible;
            Set_Cmd_Vis();

            Pan_C_Scan.Visible = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo. m_i_TOFD_0_Cscan_1_Mui_2 ==2;
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2)
            {
                Txt_Gsb_Len.Visible = false;
               Lb_GsbLen.Visible = false;
                Lb_Gsbcd.Visible = false;
            }
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 >= 1)
            {
                Txt_Gsb_Len.Visible = true ;
                Lb_GsbLen.Visible = true;
                Lb_Gsbcd.Visible = true;
            }
            SysInfo.csInter.INIWriteValue("System", "m_i_TOFD_0_Cscan_1", SysInfo.m_i_TOFD_0_Cscan_1_Mui_2.ToString(),
                                          Application.StartupPath + "\\database\\SysConfig.ini");

            Ck_Have_TOFD.Visible = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0;
        }
        private void VoltChange()
        {
            switch (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iVolt)
            {
                case 0:
                    Txt_Gytj.Text = "400V";
                    break;
                case 1:
                    Txt_Gytj.Text = "200V";
                    break;
                case 2:
                    Txt_Gytj.Text = "300V";
                    break;
            }
        }
        /// <summary>
        /// 频率值刷新界面
        /// </summary>
        private void FreqChange()
        {
            switch ((int)SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iRepeatFreq)
            {
                case 0:
                    Txt_Cfpl.Text = "15Hz"; break;
                case 1:
                    Txt_Cfpl.Text = "30Hz"; break;
                case 2:
                    Txt_Cfpl.Text = "60Hz"; break;
                case 3:
                    Txt_Cfpl.Text = "100Hz"; break;
                case 4:
                    Txt_Cfpl.Text = "200Hz"; break;
                case 5:
                    Txt_Cfpl.Text = "300Hz"; break;
                case 6:
                    Txt_Cfpl.Text = "400Hz"; break;
                case 7:
                    Txt_Cfpl.Text = "500Hz"; break;
                case 8:
                    Txt_Cfpl.Text = "1kHz"; break;

            }
        }
        private void Bt_Ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 界面初始化
        /// </summary>
        private void InitFrm(int iType = 0)
        {
            Cmb_UI_Type.SelectedIndex = SysInfo.m_i_UI_Type;

            if (SysInfo.m_i_UI_Type == 0)
            {
                int i = SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan;

                Txt_Xg.Text = SysInfo.m_SysBuff.m_Tofd_DLL.m_i_XiangGuan_Num.ToString();
                //1 增益
                Track_Zy.Value = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_idB> Track_Zy.Maximum ?700: SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_idB;
                Txt_Zy.Text = (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_idB / 10f).ToString("f1") + "dB";
                //2 范围
                Track_Fw.Value = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iRange;
                Txt_Fw.Text = (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iRange).ToString() + "mm";
                //3 零偏
                if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iZeroTime < 0) SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iZeroTime = 0;
                if (Track_Lp.Maximum < SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iZeroTime) SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iZeroTime = 0;
                Track_Lp.Value = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iZeroTime;
                float _T = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iZeroTime / 100f;
                Txt_Lp.Text = _T.ToString("f2") + "us";
                SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].T0 = _T;
                //4 平移
                Track_Py.Value = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iParallelTime;
                Txt_Py.Text = (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iParallelTime / 100f).ToString("f2") + "us";

                //5 脉冲个数 待命
                Track_Mckd.Value = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iPulWidthCode;
                Txt_Mckd.Text = ((SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iPulWidthCode - 1) * 5).ToString() + "ns";
                //6 检波方式
                Cmb_Jbfs.SelectedIndex = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iDemodulation_Flag;
                //7  重复频率
                Track_Cfpl.Value = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iRepeatFreq;
                FreqChange();
                //7 工作方式
                Cmb_Gzfs.SelectedIndex = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iWorkMode;

                //8 宽带选择
                Cmb_Kdxz.SelectedIndex = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iSecBandWidthF;
                //9 阻抗匹配
                Cmb_Zkpp.SelectedIndex = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iImpedanceF;
                //10 高压调节
                Track_Gytj.Value = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iVolt;
                VoltChange();
                //11 速度
                if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_dSpeed < Track_Sd.Minimum)
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_dSpeed = Track_Sd.Minimum;
                Track_Sd.Value = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_dSpeed;
                Txt_Sd.Text = (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_dSpeed).ToString() + "m/s";


                //14 前放开关
                Cmb_Qf.SelectedIndex = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[i].m_iForword;

                //string _strM = SysInfo.csInter.IniReadDefine("Cam", "FZ_ACB", "000", SysInfo.HardFileName);
                //if (_strM.Length == 3)
                //{
                //    Ck_1.Checked = _strM.Substring(0, 1) == "1";
                //    Ck_2.Checked = _strM.Substring(2, 1) == "1";
                //    Ck_3.Checked = _strM.Substring(1, 1) == "1";
                //}
            }
            
            #region 系统参数
         
            Init_Sys();
            #endregion 
            //重新调用不刷新工艺文件列表
            if (iType == 1) return;

            #region 工艺文件列表刷新
            SysInfo.m_blFrm_TOFD_Open[0] = false;
            SysInfo.m_blFrm_TOFD_Open[1] = false;

            string _strT = SysInfo.csInter.IniReadDefine("Tofd_Craft", "CurrCraft_Name", "", SysInfo.HardFileName);
            m_Craft_iNum = int.Parse(SysInfo.csInter.IniReadDefine("Tofd_Craft", "Num", "0", SysInfo.HardFileName));

            Cmb_Gywj.Items.Clear();
            Cmb_Gywj_BJ.Items.Clear();
            if (_strT != "" && m_Craft_iNum > 0)
            {
                Txt_Gywjmc.Text = _strT;

                for (int _iNo = 0; _iNo < m_Craft_iNum; _iNo++)
                {
                    _strT = SysInfo.csInter.INIReadValue("Tofd_Craft", (_iNo + 1).ToString(), "", SysInfo.HardFileName);
                    if (_strT != "")
                        if (SysInfo.m_i_UI_Type == 0)
                            Cmb_Gywj.Items.Add(_strT);
                        else
                            Cmb_Gywj_BJ.Items.Add(_strT);
                }
            }

            #endregion
           
        }
        private void Init_Sys(int iType = 0)
        {
            if (iType == 0)
            {
                Txt_fl_Max_Limit.Text = SysInfo.m_SysBuff.m_Climb.fl_Max_Limit.ToString();
                Ck_Alarm_Data.Checked = SysInfo.m_SysBuff.m_Climb.i_Alarm == 1;
                flNormal_Thickness.Text = SysInfo.m_SysBuff.m_Climb.flstrThickAlarm.ToString();
                strThickAlarm.Text = SysInfo.m_SysBuff.m_Climb.strWc_Bfz;
                //if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                //    SysInfo.m_SysBuff.m_Climb.iGsb_Len -= 20;
                    Txt_Gsb_Len.Text = SysInfo.m_SysBuff.m_Climb.iGsb_Len.ToString();
                Txt_Speed.Text = SysInfo.m_SysBuff.m_Climb.strSpeed;

                SysInfo.m_i_ReportType = int.Parse(SysInfo.csInter.IniReadDefine("System", "m_i_ReportType", "0", SysInfo.HardFileName));
                Rad_Print_1.Checked = SysInfo.m_i_ReportType == 0;
                Rad_Print_2.Checked = SysInfo.m_i_ReportType == 1;

                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1|| SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 ==3)
                {
                    Rad_Print_3.Visible = true;
                    Rad_Print_3.Checked = SysInfo.m_i_ReportType == 2;
                }
                else
                {
                    Rad_Print_3.Visible = false;
                }

                Txt_m_fl_MarkLag.Text = SysInfo.m_fl_MarkLag.ToString();
                Txt_m_Ip_1.Text = SysInfo.csInter.IniReadDefine("Cam", "m_Ip_1", "192.168.1.12", m_Net_V);
                Txt_m_Ip_2.Text = SysInfo.csInter.IniReadDefine("Cam", "m_Ip_2", "", m_Net_V);
                Txt_m_Ip_3.Text = SysInfo.csInter.IniReadDefine("Cam", "m_Ip_3", "", m_Net_V);
                Txt_m_Ip_4.Text = SysInfo.csInter.IniReadDefine("Cam", "m_Ip_4", "", m_Net_V);

                Txt_WaitTims.Text = SysInfo.m_SysBuff.m_Tofd_DLL.g_iWaitTime.ToString();

                //Ck_m_blXunJi_Prog_0PC_1YY.Checked = int.Parse(SysInfo.csInter.IniReadDefine("System", "m_blXunJi_Prog_0PC_1YY", "0", SysInfo.HardFileName)) == 1;
                //Ck_Save_OneFile.Checked = int.Parse(SysInfo.csInter.IniReadDefine("System", "m_W_i_SaveType_1One_0Web", "1", SysInfo.HardFileName)) == 1;
                Txt_FilePath.Text = SysInfo.m_W_i_FilePath;
                Txt_PrintFile_Path.Text = SysInfo.csInter.IniReadDefine("Browse", "strPathFileName", "", SysInfo.HardFileName);

                Txt_m_iA_Dellon_Num.Text = SysInfo.m_iA_Dellon_MaxNum.ToString();

                Txt_m_i_Alarm_Limit.Text = SysInfo.m_SysBuff.m_Tofd_DLL.m_i_Alarm_Limit.ToString();

               
            }
            else
            {
                
                try
                {
                    SysInfo.m_SysBuff.m_Climb.fl_Max_Limit = float.Parse(Txt_fl_Max_Limit.Text);
                    SysInfo.m_SysBuff.m_Climb.i_Alarm = Ck_Alarm_Data.Checked ? 1 : 0;
                }
                catch { }
                try
                {
                    SysInfo.m_SysBuff.m_Climb.flstrThickAlarm = float.Parse(flNormal_Thickness.Text);
                }
                catch { }
                SysInfo.m_SysBuff.m_Climb.strWc_Bfz = strThickAlarm.Text;

                SysInfo.m_SysBuff.m_Climb.iGsb_Len = int.Parse(Txt_Gsb_Len.Text);
                //if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
                //    SysInfo.m_SysBuff.m_Climb.iGsb_Len += 20;
                SysInfo.m_SysBuff.m_Climb.strSpeed = Txt_Speed.Text;

                SysInfo.m_i_UI_Type = Cmb_UI_Type.SelectedIndex;
               
                try
                {
                    SysInfo.m_SysBuff.m_Tofd_DLL.g_iWaitTime = int.Parse(Txt_WaitTims.Text);
                    SysInfo.csInter.INIWriteValue("TOFD", "g_iWaitTime", SysInfo.m_SysBuff.m_Tofd_DLL.g_iWaitTime.ToString(), m_NetP);
                }
                catch { }
                SysInfo.m_fl_MarkLag = float.Parse(Txt_m_fl_MarkLag.Text);
                SysInfo.m_fl_MarkLag_m = SysInfo.m_fl_MarkLag / 1000;
                SysInfo.csInter.INIWriteValue("TOFD", "m_fl_MarkLag", SysInfo.m_fl_MarkLag.ToString(), m_NetP);

                SysInfo.csInter.INIWriteValue("Cam", "m_Ip_1", Txt_m_Ip_1.Text, m_Net_V);
                SysInfo.csInter.INIWriteValue("Cam", "m_Ip_2", Txt_m_Ip_2.Text, m_Net_V);
                SysInfo.csInter.INIWriteValue("Cam", "m_Ip_3", Txt_m_Ip_3.Text, m_Net_V);
                SysInfo.csInter.INIWriteValue("Cam", "m_Ip_4", Txt_m_Ip_4.Text, m_Net_V);

                string _strT_IP = Txt_m_Ip_4.Text.Trim();
                SysInfo.m_bl_Qhzy = _strT_IP != "" && _strT_IP.Split ('.').Length ==4;

                //SysInfo.csInter.INIWriteValue("System", "m_blXunJi_Prog_0PC_1YY", Ck_m_blXunJi_Prog_0PC_1YY.Checked ? "1" : "0", SysInfo.HardFileName);
                //SysInfo.m_blXunJi_Prog_0PC_1YY = Ck_m_blXunJi_Prog_0PC_1YY.Checked;

                //SysInfo.m_W_i_SaveType_1One_0Web = Ck_Save_OneFile.Checked ? 1 : 0;
                //SysInfo.csInter.INIWriteValue("System", "m_W_i_SaveType_1One_0Web", Ck_Save_OneFile.Checked ? "1" : "0", SysInfo.HardFileName);

                SysInfo.csInter.INIWriteValue("System", "m_i_ReportType", SysInfo.m_i_ReportType.ToString(), SysInfo.HardFileName);
            }
        }
        private void Track_Zy_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_idB = Track_Zy.Value;
            Tofd.SendCmdDB(SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_idB);

            Txt_Zy.Text = (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_idB / 10f).ToString("f1") + "dB";
            Lb_Send.Text = "增益: " + Txt_Zy.Text;
        }

        private void Track_Fw_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;

            Track_Fw_Change();
        }
        private void Track_Lp_Scroll(object sender, EventArgs e)
        {
            Lp_Cg();
        }
         private void Set_Lp(bool blAdd=true )
        {
            if (blAdd)

                Track_Lp.Value = Track_Lp.Value + 1;
            else
            {
                if (Track_Lp.Value > 1)
                    Track_Lp.Value = Track_Lp.Value - 1;
            }
            Lp_Cg();
        }
        private void Lp_Cg ()
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iZeroTime = Track_Lp.Value;
            Tofd.SendCmdZeroTime(SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iZeroTime);

            Txt_Lp.Text = (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iZeroTime / 100f).ToString("f2") + "us";

            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].T0 =
                SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iZeroTime / 100f;

            Lb_Send.Text = "零偏: " + Txt_Lp.Text;
        }
        private void Track_Py_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iParallelTime = Track_Py.Value;
            Tofd.SendCmdParallel(SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iParallelTime);

            Txt_Py.Text = (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iParallelTime / 100f).ToString("f2") + "us";
            Lb_Send.Text = "平移: " + Txt_Py.Text;
        }

        private void Cmb_Jbfs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iDemodulation_Flag = (byte)Cmb_Jbfs.SelectedIndex;
            Tofd.SendCmdWaveType((int)SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iDemodulation_Flag);
            Lb_Send.Text = "检波方式：" + Cmb_Jbfs.Text;
            m_i_Stand_Num = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iDemodulation_Flag != 2 ? 256 : 128;
           
            SysInfo.InitLimitPic(Rad_C_A .Checked ?  SysInfo.m_str_B_Stand_Color: SysInfo.m_str_B_Stand_Color_A, m_i_Stand_Num, P_1, P_Md, P_2, Grp_Stand_Color);
            m_bl_Color_Mdf = true;
        }

        private void Track_Cfpl_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iRepeatFreq = (byte)Track_Cfpl.Value;
            Tofd.SendCmdRepeatFreq((int)SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iRepeatFreq);

            FreqChange();
            Lb_Send.Text = "重复频率:: " + Txt_Cfpl.Text;
        }

        private void Cmb_Gzfs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iWorkMode = (byte)Cmb_Gzfs.SelectedIndex;
            Tofd.SendCmdWorkMode((int)SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iWorkMode);
            Lb_Send.Text = "工作方式:: " + Cmb_Gzfs.Text;
        }

        private void Cmb_Kdxz_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iSecBandWidthF = (byte)Cmb_Kdxz.SelectedIndex;
            Tofd.SendCmdBandWidth((int)SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iSecBandWidthF);
            Lb_Send.Text = "宽带选择：" + Cmb_Kdxz.Text;
        }

        private void Cmb_Zkpp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iImpedanceF = (byte)Cmb_Zkpp.SelectedIndex;
            Tofd.SendCmdImpdance((int)SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iImpedanceF);

            Lb_Send.Text = "阻抗匹配：" + Cmb_Zkpp.Text;
        }

        private void Track_Gytj_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iVolt = (byte)Track_Gytj.Value;
            Tofd.SendCmdHighVoltage((int)SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iVolt);

            VoltChange();
            Lb_Send.Text = "高压调节:：" + Txt_Gytj.Text;
        }

        private void Track_Sd_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_dSpeed = Track_Sd.Value;
            Txt_Sd.Text = (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_dSpeed).ToString() + "m/s";

            Tofd.SendCmdFreqRatio((int)SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iRange, SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_dSpeed);
            Lb_Send.Text = "声速: " + Txt_Sd.Text;
            Cal_JG();
        }
        private void  Cal_JG()
        {
            SysInfo.m_SysBuff.m_Tofd_DLL.m_fl_Time_JG = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iRange * 2.0f /
                         (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_dSpeed / 1000f)
                         / Tofd.UTS_DATA_WIDTH;

        }
        private void Cmb_Qf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iForword = (byte)Cmb_Qf.SelectedIndex;
            Tofd.SendCmdForword((int)SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iForword);
            Lb_Send.Text = "前放开关: " + Cmb_Qf.Text;
        }

        private void Track_Mckd_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iPulWidthCode = (ushort)Track_Mckd.Value;
            Txt_Mckd.Text = ((SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iPulWidthCode - 1) * 5).ToString() + "ns";
            Tofd.SendCmdPulWid((int)SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iPulWidthCode);
            Lb_Send.Text = "脉冲个数: " + Txt_Mckd.Text;
        }

        private void Bt_CalCu_Click(object sender, EventArgs e)
        {
            Bt_CalCu.Enabled = false;
            Frm_Tofd_Calcu _flFrm = new Frm_Tofd_Calcu();
            _flFrm.Show();
            Bt_CalCu.Enabled = true;
        }
        /// <summary>
        /// 探头校准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_Jz_Click(object sender, EventArgs e)
        {
            SysInfo.SetKeyBorad("num_keyboard");
            Bt_Jz.Enabled = false;
    
            Bt_Jz.Enabled = true;
        }

        private void Frm_TOFD_FormClosing(object sender, FormClosingEventArgs e)
        {
            SysInfo.SetKeyBorad("num_keyboard");
            //   if (SysInfo.m_SysBuff  .m_Tofd_DLL.m_iRun == 1 && SysInfo.m_SysBuff.m_Tofd_DLL.m_iRun_State == 1) 
            //       SysInfo.m_iA_Dellon_MaxNum = 0;
            SysInfo.m_blFrmOpen[0] = false;

            //  SysInfo.csInter.("ClassUltrasGate", "m_iRomoteNum", "1", System.Windows.Forms.Application.StartupPath + "\\HardConfig.ini")
            SysInfo.m_SysBuff_C.m_Plant_C.Init(1);
            Init_Sys(1);
            SysInfo.Init(1);
            UI_IP_Init(1);
            
                    UI_IP_Init_Coat(1);
            UI_IP_Init_Ect(1);
            //if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 && SysInfo.m_Tofd_C_Scan !=null )
            //    SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum = 0;

           
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != SysInfo.m_i_TOFD_0_Cscan_1_Mui_2_Old)
                this.Visible =false ;
            SysInfo.g_Msg_InterFace.Fun_Tofd_Para(1);
            //      if (m_bl_Color_Mdf) SysInfo.g_Msg_InterFace.Fun_ModyColor();
        }
        /// <summary>
        /// 工艺文件保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_Mody_Click(object sender, EventArgs e)
        {
            //1 文件名检测
            if (SysInfo.m_blFrm_TOFD_Open[0] || SysInfo.m_blFrm_TOFD_Open[1])
            {
                MessageBox.Show("请先关闭参数计算或者校准界面!");
                return;
            }
            if (Txt_Gywjmc.Text == "")
            {
                //组织文件名
                Txt_Gywjmc.Text = GetCurr_CraftFimeName();

                MessageBox.Show("新文件名：请确认!");
                return;
            }
            //2 更新系统文件名
            SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection = Txt_Gywjmc.Text;

            //1 寻找是否有重名
            bool _blOk = false;
            for (int i = 0; i < Cmb_Gywj.Items.Count; i++)
            {
                if (Cmb_Gywj.Text == SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection)
                {
                    _blOk = true;
                    break;
                }
            }

            if (_blOk == false)
            {
                m_Craft_iNum++;
                SysInfo.csInter.INIWriteValue("Tofd_Craft", "Num", m_Craft_iNum.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("Tofd_Craft", "CurrCraft_Name", SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("Tofd_Craft", m_Craft_iNum.ToString(), SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, SysInfo.HardFileName);

                Cmb_Gywj.Items.Add(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection);
            }

            // 3 新文件保存对应参数
            //3.1 探头参数保存
            SysInfo.Init(1);
            //3.2 计算参数
            SysInfo.IniIt_Calcu();
            //3.3 校准参数
            SysInfo.Init_Jz();
        }
        /// <summary>
        /// 得到当前文件名
        /// </summary>
        private string GetCurr_CraftFimeName()
        {
            string _strRet = "平板";
            int _iCurrChan = SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan;
            //1 类型
            int _iT = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsMode;
            switch (_iT)
            {
                case 0://平板
                    _strRet = "平板";
                    break;
                case 1://圆弧外壁
                    _strRet = "外壁";
                    break;
                case 2://圆弧内壁
                    _strRet = "内壁";
                    break;

            }
            //2 板子厚度
            float _T = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsEnd - SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsStart;
            _strRet += "厚度" + _T.ToString("f0");
            //3 楔块角度
            string _Jd = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsAngle.ToString("f0");
            _strRet += "角度" + _Jd;
            return _strRet;
        }
        private void Bt_Dell_Click(object sender, EventArgs e)
        {
            if (Cmb_Gywj.Text == "")
            {
                MessageBox.Show("列表的文件名不能为空!");
                return;
            }
            if (m_Craft_iNum > 0 && Cmb_Gywj.SelectedIndex > -1)
            {
                //2 文件删除
                SysInfo.csInter.EraseSection(Cmb_Gywj.SelectedItem.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.EraseSection("Tofd_Craft", SysInfo.HardFileName);
                SysInfo.WaitTime(0.1f);
                //3 重新组织列表文件        
                //1 下拉框删除
                Cmb_Gywj.Items.RemoveAt(Cmb_Gywj.SelectedIndex);
                for (int i = 0; i < Cmb_Gywj.Items.Count; i++)
                    SysInfo.csInter.INIWriteValue("Tofd_Craft", (i + 1).ToString(), Cmb_Gywj.Items[i].ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("Tofd_Craft", "Num", Cmb_Gywj.Items.Count.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("Tofd_Craft", "CurrCraft_Name", "", SysInfo.HardFileName);
                m_Craft_iNum = Cmb_Gywj.Items.Count;
                //4 刷新当前文件名
                Cmb_Gywj.Text = "";
                Txt_Gywjmc.Text = "";
            }
        }

        private void Txt_Gywjmc_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_blFrm_TOFD_Open[0] || SysInfo.m_blFrm_TOFD_Open[1])
            {
                MessageBox.Show("请先关闭参数计算或者校准界面!");
                return;
            }
            if (Txt_Gywjmc.Text == "")
            {
                //组织文件名
                Txt_Gywjmc.Text = GetCurr_CraftFimeName();

                MessageBox.Show("新文件名：请确认!");
                return;
            }
        }

        private void Cmb_Gywj_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            if (SysInfo.m_blFrm_TOFD_Open[0] || SysInfo.m_blFrm_TOFD_Open[1])
            {
                MessageBox.Show("请先关闭参数计算或者校准界面!");
                return;
            }
            #region 添加新的工艺文件

            //1 文件主键
            SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection = Cmb_Gywj.SelectedItem.ToString();
            SysInfo.csInter.INIWriteValue("Tofd_Craft", "CurrCraft_Name", SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, SysInfo.HardFileName);
            //2 调用数据
            SysInfo.Init();
            //3 刷新界面
            InitFrm(1);
            Txt_Gywjmc.Text = SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection;
            #endregion
        }

        private void Bt_Gywjm_Brsh_Click(object sender, EventArgs e)
        {
            Txt_Gywjmc.Text = GetCurr_CraftFimeName();
        }
        private void InitColor( string _str_B_Stand_Color)
        {
            // 1 选择颜色初始化
            string[] _sPara = "".Split('|');
            string[] _sPara_Sub = "".Split('|');
            int iR = 0, iG = 0, iB = 0;
            if (_str_B_Stand_Color != "")
                _sPara = _str_B_Stand_Color.Split('|');
            if (_sPara.Length >= 4)
            {
                #region 1 开始色
                _sPara_Sub = _sPara[0].Split('/');
                if (_sPara_Sub.Length == 3)
                {
                    if (_sPara_Sub[0] != "" && _sPara_Sub[1] != "" && _sPara_Sub[2] != "")
                    {
                        iR = int.Parse(_sPara_Sub[0]);
                        iG = int.Parse(_sPara_Sub[1]);
                        iB = int.Parse(_sPara_Sub[2]);
                        P_My_S.BackColor = Color.FromArgb((byte)iR, (byte)iG, (byte)iB);
                    }
                }
                #endregion 1
                #region 2 中间开始色
                _sPara_Sub = _sPara[1].Split('/');
                if (_sPara_Sub.Length == 3)
                {
                    if (_sPara_Sub[0] != "" && _sPara_Sub[1] != "" && _sPara_Sub[2] != "")
                    {
                        iR = int.Parse(_sPara_Sub[0]);
                        iG = int.Parse(_sPara_Sub[1]);
                        iB = int.Parse(_sPara_Sub[2]);
                        P_My_M_Start.BackColor = Color.FromArgb((byte)iR, (byte)iG, (byte)iB);
                    }
                }
                #endregion 2
                #region 3 中间开始色
                _sPara_Sub = _sPara[2].Split('/');
                if (_sPara_Sub.Length == 3)
                {
                    if (_sPara_Sub[0] != "" && _sPara_Sub[1] != "" && _sPara_Sub[2] != "")
                    {
                        iR = int.Parse(_sPara_Sub[0]);
                        iG = int.Parse(_sPara_Sub[1]);
                        iB = int.Parse(_sPara_Sub[2]);
                        P_My_M_End.BackColor = Color.FromArgb((byte)iR, (byte)iG, (byte)iB);
                    }
                }
                #endregion 3
                #region 4 结束色
                _sPara_Sub = _sPara[3].Split('/');
                if (_sPara_Sub.Length == 3)
                {
                    if (_sPara_Sub[0] != "" && _sPara_Sub[1] != "" && _sPara_Sub[2] != "")
                    {
                        iR = int.Parse(_sPara_Sub[0]);
                        iG = int.Parse(_sPara_Sub[1]);
                        iB = int.Parse(_sPara_Sub[2]);
                        P_My_E.BackColor = Color.FromArgb((byte)iR, (byte)iG, (byte)iB);
                    }
                }
                #endregion 4
            }
        }
        private void GetMyColor(PictureBox Pic_My)
        {
            InitColor( Rad_C_D .Checked ? SysInfo.m_str_B_Stand_Color : SysInfo.m_str_B_Stand_Color_A);

            ColorDialog colorDialog = new ColorDialog();
            colorDialog.CustomColors = new int[] { 255, 65536, 65408, 4259584, 16776960, 12615680, 12615808, 16711935, 64, 16512, 16384, 4210688, 8388608, 4194304, 4194368, 8388672 };
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Pic_My.BackColor = colorDialog.Color;

                string strConet = (P_My_S.BackColor.R.ToString() + "/" + P_My_S.BackColor.G.ToString() + "/" + P_My_S.BackColor.B.ToString()) + "|";
                strConet += (P_My_M_Start.BackColor.R.ToString() + "/" + P_My_M_Start.BackColor.G.ToString() + "/" + P_My_M_Start.BackColor.B.ToString()) + "|";
                strConet += (P_My_M_End.BackColor.R.ToString() + "/" + P_My_M_End.BackColor.G.ToString() + "/" + P_My_M_End.BackColor.B.ToString()) + "|";
                strConet += (P_My_E.BackColor.R.ToString() + "/" + P_My_E.BackColor.G.ToString() + "/" + P_My_E.BackColor.B.ToString());

                if (Rad_C_D.Checked)
                {
                    SysInfo.m_str_B_Stand_Color = strConet;
                    SysInfo.csInter.INIWriteValue("TOFD", "m_str_B_Stand_Color", SysInfo.m_str_B_Stand_Color, SysInfo.HardFileName);
                }
                else if (Rad_C_A.Checked)
                {
                    SysInfo.m_str_B_Stand_Color_A = strConet;
                    SysInfo.csInter.INIWriteValue("TOFD", "m_str_B_Stand_Color_A", SysInfo.m_str_B_Stand_Color_A, SysInfo.HardFileName);
                }

                SysInfo.InitLimitPic(strConet, m_i_Stand_Num, P_1, P_Md, P_2, Grp_Stand_Color);
                m_bl_Color_Mdf = true;
            }
        }
        private void P_My_S_Click(object sender, EventArgs e)
        {
            GetMyColor(P_My_S);
        }

        private void P_My_M_Start_Click(object sender, EventArgs e)
        {
            GetMyColor(P_My_M_Start);
        }

        private void P_My_M_End_Click(object sender, EventArgs e)
        {
            GetMyColor(P_My_M_End);
        }

        private void P_My_E_Click(object sender, EventArgs e)
        {
            GetMyColor(P_My_E);
        }

        private void Ck_m_blXunJi_Prog_0PC_1YY_Click(object sender, EventArgs e)
        {
            //SysInfo.csInter.INIWriteValue("System", "m_blXunJi_Prog_0PC_1YY", Ck_m_blXunJi_Prog_0PC_1YY.Checked ? "1" : "0", SysInfo.HardFileName);
            //SysInfo.m_blXunJi_Prog_0PC_1YY = Ck_m_blXunJi_Prog_0PC_1YY.Checked;
        }

        private void Bt_SelectPath_Click(object sender, EventArgs e)
        {
            string strPathFileName = SysInfo.m_W_i_FilePath;

            FolderBrowserDialog dialog = new FolderBrowserDialog();

            dialog.SelectedPath = SysInfo.m_W_i_FilePath;
            if (SysInfo.m_iLanguage == 0)
                dialog.Description = "请选择文件路径";
            else
                dialog.Description = "Please select the file path";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SysInfo.m_W_i_FilePath = dialog.SelectedPath;
                SysInfo.csInter.INIWriteValue("System", SysInfo .m_i_TOFD_0_Cscan_1_Mui_2 == 0 ? "m_W_i_FilePath": (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2==1? "m_W_i_FilePath_Cscan": "m_W_i_FilePath_M_UI"), SysInfo.m_W_i_FilePath, SysInfo.HardFileName);
            }
            Txt_FilePath.Text = SysInfo.m_W_i_FilePath;

        }

        private void Ck_Save_OneFile_Click(object sender, EventArgs e)
        {
            //SysInfo.m_W_i_SaveType_1One_0Web = Ck_Save_OneFile.Checked ? 1 : 0;
            //SysInfo.csInter.INIWriteValue("System", "m_W_i_SaveType_1One_0Web", Ck_Save_OneFile.Checked ? "1" : "0", SysInfo.HardFileName);
        }

        private void Txt_WaitTims_TextChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            try
            {
                int _fData = int.Parse(Txt_WaitTims.Text);
                if (_fData >= 0)
                {
                    SysInfo.m_SysBuff.m_Tofd_DLL.g_iWaitTime = int.Parse(Txt_WaitTims.Text);
                    SysInfo.csInter.INIWriteValue("TOFD", "g_iWaitTime", SysInfo.m_SysBuff.m_Tofd_DLL.g_iWaitTime.ToString(), m_NetP);
                }
                else
                {
                    Txt_WaitTims.Text = "0";
                    SysInfo.m_SysBuff.m_Tofd_DLL.g_iWaitTime = int.Parse(Txt_WaitTims.Text);
                    MessageBox.Show("此数值不能太小！经验值>0");
                }
            }
            catch { }
        }

        private void Txt_m_iA_Dellon_Num_TextChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            try
            {
                int _fData = int.Parse(Txt_m_iA_Dellon_Num.Text);
                if (_fData >= 0)
                {
                    SysInfo.m_iA_Dellon_MaxNum = _fData;
                    SysInfo.csInter.INIWriteValue("System", "m_iA_Dellon_MaxNum", SysInfo.m_iA_Dellon_MaxNum.ToString(), m_NetP);
                }
                else
                {
                    Txt_m_iA_Dellon_Num.Text = "100";
                    SysInfo.m_iA_Dellon_MaxNum = 100;
                    MessageBox.Show("此数值不能太小！经验值>100");
                }
            }
            catch { }
        }

        private void Txt_m_i_Alarm_Limit_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int _iD = int.Parse(Txt_m_i_Alarm_Limit.Text);
                SysInfo.m_SysBuff.m_Tofd_DLL.m_i_Alarm_Limit = _iD;
                SysInfo.csInter.INIWriteValue ("TOFD", "m_i_Alarm_Limit", _iD.ToString (), SysInfo.HardFileName);
            }
            catch
            {
            }
        }

        private void SendFz()
        {
            string _strM = Ck_1.Checked ? "1" : "0";
                   _strM += Ck_3.Checked ? "1" : "0";
                   _strM += Ck_2.Checked ? "1" : "0";
            SysInfo.m_ServerUI.SendData_M(0, _strM, "11.0");
            SysInfo.csInter.INIWriteValue("Cam", "FZ_ACB", _strM, SysInfo.HardFileName);
        }
        private void Ck_A_Click(object sender, EventArgs e)
        {
            SendFz();
        }

        private void Ck_B_Click(object sender, EventArgs e)
        {
            SendFz();
        }

        private void Ck_C_Click(object sender, EventArgs e)
        {
            SendFz();
        }

        private void label3_Click(object sender, EventArgs e)
        {
          //  Ck_1.Visible = !Ck_1.Visible;
        }

        private void label4_Click(object sender, EventArgs e)
        {
         //   Ck_2.Visible = !Ck_2.Visible;
        }

        private void label9_Click(object sender, EventArgs e)
        {
         //   Ck_3.Visible = !Ck_3.Visible;
        }

        private void Bt_Calcu_PCS_Click(object sender, EventArgs e)
        {
            Bt_Calcu_PCS.Enabled = false;
            Frm_Tofd_Calcu _flFrm = new Frm_Tofd_Calcu();
            _flFrm.Show();
            Bt_Calcu_PCS.Enabled = true;
        }

        private void Bt_CalCu_T0_Click(object sender, EventArgs e)
        {
            Bt_CalCu_T0.Enabled = false;
       
            Bt_CalCu_T0.Enabled = true;
        }

        private void Bt_TbName_BJ_Click(object sender, EventArgs e)
        {
            Txt_Gywjmc_BJ.Text = GetCurr_CraftFimeName();
        }

        private void Bt_Mody_Bj_Click(object sender, EventArgs e)
        {

            //1 文件名检测
            if (SysInfo.m_blFrm_TOFD_Open[0] || SysInfo.m_blFrm_TOFD_Open[1])
            {
                MessageBox.Show("请先关闭参数计算或者校准界面!");
                return;
            }
            if (Txt_Gywjmc_BJ.Text == "")
            {
                //组织文件名
                Txt_Gywjmc_BJ.Text = GetCurr_CraftFimeName();

                MessageBox.Show("新文件名：请确认!");
                return;
            }
            //2 更新系统文件名
            SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection = Txt_Gywjmc_BJ.Text;

            //1 寻找是否有重名
            bool _blOk = false;
            for (int i = 0; i < Cmb_Gywj_BJ.Items.Count; i++)
            {
                if (Cmb_Gywj_BJ.Text == SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection)
                {
                    _blOk = true;
                    break;
                }
            }

            if (_blOk == false)
            {
                m_Craft_iNum++;
                SysInfo.csInter.INIWriteValue("Tofd_Craft", "Num", m_Craft_iNum.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("Tofd_Craft", "CurrCraft_Name", SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("Tofd_Craft", m_Craft_iNum.ToString(), SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, SysInfo.HardFileName);

                Cmb_Gywj_BJ.Items.Add(SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection);
            }

            // 3 新文件保存对应参数
            //3.1 探头参数保存
            SysInfo.Init(1);
            //3.2 计算参数
            SysInfo.IniIt_Calcu();
            //3.3 校准参数
            SysInfo.Init_Jz();
        }

        private void Bt_Dell_BJ_Click(object sender, EventArgs e)
        {
            if (Cmb_Gywj_BJ.Text == "")
            {
                MessageBox.Show("列表的文件名不能为空!");
                return;
            }
            if (m_Craft_iNum > 0 && Cmb_Gywj_BJ.SelectedIndex > -1)
            {
                //2 文件删除
                SysInfo.csInter.EraseSection(Cmb_Gywj_BJ.SelectedItem.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.EraseSection("Tofd_Craft", SysInfo.HardFileName);
                SysInfo.WaitTime(0.1f);
                //3 重新组织列表文件        
                //1 下拉框删除
                Cmb_Gywj_BJ.Items.RemoveAt(Cmb_Gywj_BJ.SelectedIndex);
                for (int i = 0; i < Cmb_Gywj_BJ.Items.Count; i++)
                    SysInfo.csInter.INIWriteValue("Tofd_Craft", (i + 1).ToString(), Cmb_Gywj_BJ.Items[i].ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("Tofd_Craft", "Num", Cmb_Gywj_BJ.Items.Count.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue("Tofd_Craft", "CurrCraft_Name", "", SysInfo.HardFileName);
                m_Craft_iNum = Cmb_Gywj_BJ.Items.Count;
                //4 刷新当前文件名
                Cmb_Gywj_BJ.Text = "";
                Txt_Gywjmc_BJ.Text = "";
            }
        }

        private void Cmb_Gywj_BJ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            if (SysInfo.m_blFrm_TOFD_Open[0] || SysInfo.m_blFrm_TOFD_Open[1])
            {
                MessageBox.Show("请先关闭参数计算或者校准界面!");
                return;
            }
            #region 添加新的工艺文件

            //1 文件主键
            SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection = Cmb_Gywj_BJ.SelectedItem.ToString();
            SysInfo.csInter.INIWriteValue("Tofd_Craft", "CurrCraft_Name", SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection, SysInfo.HardFileName);
            //2 调用数据
            SysInfo.Init();
            //3 刷新界面
            InitFrm(1);
            Txt_Gywjmc_BJ.Text = SysInfo.m_SysBuff.m_Tofd_DLL.m_strSection;
            #endregion
        }

        private void Txt_Gywjmc_BJ_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_blFrm_TOFD_Open[0] || SysInfo.m_blFrm_TOFD_Open[1])
            {
                MessageBox.Show("请先关闭参数计算或者校准界面!");
                return;
            }
            if (Txt_Gywjmc_BJ.Text == "")
            {
                //组织文件名
                Txt_Gywjmc_BJ.Text = GetCurr_CraftFimeName();

                MessageBox.Show("新文件名：请确认!");
                return;
            }
        }

        private void numericUpDownDataGain_ValueChanged(object sender, EventArgs e)
        {
    //        if (m_blActive == false) return;
             }

        private void Scb_Zy_Scroll(object sender, ScrollEventArgs e)
        {
            if (m_blActive == false) return;
            numericUpDownDataGain.Text = Scb_Zy.Value.ToString();
        }

        private void Scb_Fw_Scroll(object sender, ScrollEventArgs e)
        {
            if (m_blActive == false) return;
            Num_UD_Fw.Text = Scb_Fw.Value.ToString();
        }
        private double ChangeD_T(int Distanc)
        {
            double _dbRet = 0;

            _dbRet = (int)(Distanc / (float.Parse(Num_UD_Ss.Value.ToString()) * 0.001f));
            return _dbRet;
        }
        /// <summary>
        /// 获得压缩比例
        /// </summary>
        private void Get_Ysbl()
        {
       //   SysInfo.m_Tofd_BJ.m_fl_Ysbl = (float)(SysInfo.m_Tofd_BJ.m_ChScanRange * 100 / SysInfo.m_Tofd_BJ.dataLength);
        }
        private void Num_UD_Fw_ValueChanged(object sender, EventArgs e)
        {
         //   if (m_blActive == false) return;
            //if (m_blActiv)
            //{
            //    if (m_ChScanRange != _oldData)
            //    {
            //        int _iLeft = 0;
            //        double _X = Bt_Line.Left * _oldData / m_ChScanRange;
            //        if (_X > 10 && _X < panelAScan0.Width - 20)
            //        {
            //            Txt_Line.Text = _X.ToString("f0");

            //            _iLeft = (int)_X;
            //        }
            //        //  Bt_Line.Left = (int)_X;
            //        m_iLimit_No = (int)(_iLeft * m_LineStep);

            //        m_iLinit_No_R = (int)(Bt_Line_0_R.Left * m_LineStep);
            //    }
            //}
        }

        private void Hsc_Ss_Scroll(object sender, ScrollEventArgs e)
        {
            if (m_blActive == false) return;
            Num_UD_Ss.Value = Hsc_Ss.Value;
        }

        private void Num_UD_DY_ValueChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            /*
         设置发射电压值
        ChannelIndex		通道号
        value 		int 型参数。设置范围 50-400，单位步进 10。
         */

            int _T = System.Convert.ToInt32(Num_UD_DY.Value);
            CardInterface.AnyCardX_SetVoltage(_T);
        }

        private void Num_UD_Mckd_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void N_F_ValueChanged(object sender, EventArgs e)
        {
         

        }

        private void numericUpDownPRF_ValueChanged(object sender, EventArgs e)
        { 
        }

        private void comboBoxTrigMode_SelectedIndexChanged(object sender, EventArgs e)
        {
                  }

        private void Cmb_Lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            /*  ChannelIndex 通道号
 value
 int 型参数。
 0：全通； 1：0.5 - 2 MHz； 2：1.0 - 5MHz； 3：2 - 6MHz；4：4 - 9MHz； 5：7 - 15MHz； 6：10 - 20MHz； 7：>= 15MHz；。
 */
            int iT = Cmb_Lb.SelectedIndex;
       //   CardInterface.AnyCardX_SetFilterIndex(SysInfo .m_Tofd_BJ . m_iTdNo, iT);
        }

        private void Cmb_Bx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            /*
                        ChannelIndex		通道号
           value 		Int 型参数。0：全波检波；1：正波检波；2：负波检波；3：RF 射频检波。
                        */
        }

        private void Cmb_Pj_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            /*平均值次数设置函数
            ChannelIndex		通道号
            value	int 型参数。
            0：1 次平均；1：2 次平均；2：4 次平均；3：8 次平均；4：16 次平均。其他值不允许。
                        */
        }

        private void Cmb_Zn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            /* 设置接收阻尼
            ChannelIndex		通道号
            value int 型参数。0：阻尼电阻 80；1：阻尼电阻 400；
             */
            int iT = Cmb_Zn.SelectedIndex;
            CardInterface.AnyCardX_SetDampIndex(iT);
        }

        private void Cmb_TD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
        }

        private void numStartPos_ValueChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            // 设置捕获起始位置，单位 us；
        }

        private void Bt_Re_Link_BJ_Click(object sender, EventArgs e)
        {
        }

        private void Num_UD_Ss_ValueChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
        }

        private void label50_Click(object sender, EventArgs e)
        {
            Cmb_UI_Type.Visible =!Cmb_UI_Type.Visible;
            label10.Visible = Cmb_UI_Type.Visible;
        }

        private void Cmb_UI_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            MessageBox.Show("此参数改变后，请退出程序重新进入.");
        }

        private void Bt_SelectPrintPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            string _Path = "";
            dialog.SelectedPath = SysInfo.csInter.IniReadDefine("Browse", "strPathFileName", "", SysInfo.HardFileName);
            if (SysInfo.m_iLanguage == 0)
                dialog.Description = "请选择文件路径";
            else
                dialog.Description = "Please select the file path";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _Path = dialog.SelectedPath;
                SysInfo.csInter.INIWriteValue("Browse", "strPathFileName", _Path, SysInfo.HardFileName);
            }
            Txt_PrintFile_Path.Text = _Path;

        }

        private void Rad_Print_1_Click(object sender, EventArgs e)
        {
            SysInfo.m_i_ReportType = 0;
        }

        private void Rad_Print_2_Click(object sender, EventArgs e)
        {
            SysInfo.m_i_ReportType = 1;
        }

        private void Rad_Tofd_CheckedChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 = Rad_Tofd.Checked ? 0 : 1;
            Set_Pan_T_C();
        }

        private void Rad_C_CheckedChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 = Rad_C.Checked ? 1 : 0;
            Set_Pan_T_C();
        }

        private void Bt_C_Speed_Click(object sender, EventArgs e)
        {
            if (float.Parse(Txt_Speed.Text) > 9999 || float.Parse(Txt_Speed.Text) < 1000)
                Txt_Speed.Text = "3200";
            GetSpeedAddSet();
        }
        private void GetSpeedAddSet()
        {
            string _strT = Txt_Speed.Text.Split(' ')[0];

            try
            {
                int _iSp = int.Parse(_strT);
                if (_iSp > 1000)
                {
                   
                }
            }
            catch { }
        }

        private void Bt_Jz_Speed_Click(object sender, EventArgs e)
        {
         
        }
        private void Hd_Zz_0()
        {
          
        }
        private void Hd_Zz_1()
        {
            float _strSjHd = SysInfo.m_SysBuff.m_Tofd_DLL.m_flThick;
            //2 读当前声速
            int _iSpeed = SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_dSpeed;
            float _flD = float.Parse(flNormal_Thickness.Text);
            //3 计算标准声速
            int _JsSs = (int)(_flD * _iSpeed / _strSjHd);
            if (_JsSs < 0 || _JsSs > 9999)
            {
                _JsSs = 3250;
            }
            //4 刷新声速
       
            Tofd.SendCmdFreqRatio((int)SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iRange,
                                          _JsSs);
            Txt_Speed.Text = _JsSs.ToString();

            Txt_Sd.Text = Txt_Speed.Text;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_dSpeed = _JsSs ;
            Track_Sd.Value = _JsSs;
        }

        private void Rad_M_UI_CheckedChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 = Rad_M_UI.Checked ? 2 : 0;
            Set_Pan_T_C();

            Ect_P();
        }
        private void Ect_P()
        {
         
        }
        private void Bt_Ip_Set_Click(object sender, EventArgs e)
        {
                   }



        private void Bt_Ip_Click(object sender, EventArgs e)
        {
            Grp_UDP.Visible = true;
        }
        /// <summary>
        /// 电磁超声模块IP参数读写
        /// </summary>
        /// <param name="iType"></param>
        private void UI_IP_Init(int iType=0)
        {
            string HardFileName = System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini";
            if (iType ==0)
            {
                #region 读
                #region 
                //1 耗时间隔
                Txt_iTimeDelay.Text  = SysInfo . csInter.IniReadDefine("ClassUltrasGate", "m_iTimeDelay", "0", HardFileName);//探头扫查间隔时间
                Txt_Mul_Select.Text = SysInfo.csInter.IniReadDefine("ClassUltrasGate", "Txt_Mul_Select", "1", HardFileName);

                int _iRomoteNum = int.Parse(SysInfo.csInter.IniReadDefine("ClassUltrasGate", "m_iRomoteNum", "1", HardFileName));
                if (_iRomoteNum == 0) _iRomoteNum = 1;
                //主机IP
                string  _Ip_local = SysInfo.csInter.IniReadDefine("ClassUltrasGate", "m_Ip_local", "192.168.1.10", HardFileName);
                string[] _sPata = _Ip_local.Split(',');
                if(_sPata .Length ==4)
                {
                    m_Ip_remote.Text = _sPata[0] + "." + _sPata[1] + "." + _sPata[2];
                    m_Ip_local.Text = _sPata[3];
                }
                //主机端口
                m_Port_local.Text  = SysInfo.csInter.IniReadDefine("ClassUltrasGate", "m_Port_local", "12001", HardFileName);

                string [] _IpArr_remote = new string[_iRomoteNum];
                
                string _strAll_IP = "";
                for (int iT = 0; iT < _iRomoteNum; iT++)
                {
                    _IpArr_remote[iT] = SysInfo.csInter.IniReadDefine("ClassUltrasGate", "m_Ip_remote_" + (iT + 1), "192.168.1." + (11 + iT), HardFileName);
                    _sPata = _IpArr_remote[iT].Split('.');
                    if (_sPata.Length == 4)
                        _strAll_IP += (_strAll_IP != "" ? "," : "") + _sPata[3];
                }
                //模块IP
                m_Ip_remote_2.Text = _strAll_IP;
                //模块端口
                m_Port_remote.Text  = SysInfo.csInter.IniReadDefine("ClassUltrasGate", "m_Port_remote", "12000", HardFileName);
                #endregion UDP
                #endregion
            }
            else
            {
                #region 写
                //1 耗时间隔
                SysInfo.csInter.INIWriteValue ("ClassUltrasGate", "m_iTimeDelay", Txt_iTimeDelay.Text, HardFileName);

                //主机IP
                 SysInfo.csInter.INIWriteValue("ClassUltrasGate", "m_Ip_local", m_Ip_remote.Text +"."+ m_Ip_local.Text, HardFileName);
              
                //主机端口
                SysInfo.csInter.INIWriteValue("ClassUltrasGate", "m_Port_local", m_Port_local.Text, HardFileName);

                //模块IP
                m_Ip_remote_2.Text = m_Ip_remote_2.Text.Replace("，", ",");
                string[] _sPara = m_Ip_remote_2.Text.Split(',');
                int _iNum = _sPara.Length;
                string _strT = "";
                //模块使用选择
                SysInfo.csInter.INIWriteValue("ClassUltrasGate", "Txt_Mul_Select", Txt_Mul_Select.Text, HardFileName);
              

                SysInfo.csInter.INIWriteValue("ClassUltrasGate", "m_iRomoteNum", _iNum.ToString (), HardFileName);
                for(int iT = 0; iT < _iNum; iT++)
                {
                    SysInfo.csInter.INIWriteValue("ClassUltrasGate", "m_Ip_remote_" + (iT + 1), m_Ip_remote.Text+ "." + _sPara[iT ], HardFileName);
                }
                //模块端口
                SysInfo.csInter.INIWriteValue("ClassUltrasGate", "m_Port_remote", m_Port_remote.Text, HardFileName);

                _strT = SysInfo.csInter.IniReadDefine("ClassUltrasGate", "Txt_Mul_Select", "", HardFileName);
              

                #endregion
            }
        }

        private void UI_IP_Init_Coat(int iType = 0)
        {
            string HardFileName = System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini";
            if (iType == 0)
            {
                #region 读
                #region 
                //1 耗时间隔
               // Txt_iTimeDelay.Text = SysInfo.csInter.IniReadDefine("Coat", "m_iTimeDelay", "0", HardFileName);//探头扫查间隔时间
              //  Txt_Mul_Select.Text = SysInfo.csInter.IniReadDefine("Coat", "Txt_Mul_Select", "1", HardFileName);

                int _iRomoteNum = int.Parse(SysInfo.csInter.IniReadDefine("Coat", "m_iRomoteNum", "1", HardFileName));
                if (_iRomoteNum == 0) _iRomoteNum = 1;
                //主机IP
                string _Ip_local = SysInfo.csInter.IniReadDefine("Coat", "m_Ip_local", "192.168.16.100", HardFileName);
                string[] _sPata = _Ip_local.Split('.');
                if (_sPata.Length == 4)
                {
                    Txt_Coat_Main_IP.Text = _sPata[0] + "." + _sPata[1] + "." + _sPata[2];
                    Txt_Coat_Loat_IP.Text = _sPata[3];
                }
                //主机端口
                Txt_Coat_Loat_Port.Text = SysInfo.csInter.IniReadDefine("Coat", "m_Port_local", "13001", HardFileName);

                string[] _IpArr_remote = new string[_iRomoteNum];

                string _strAll_IP = "";
                for (int iT = 0; iT < _iRomoteNum; iT++)
                {
                    _IpArr_remote[iT] = SysInfo.csInter.IniReadDefine("Coat", "m_Ip_remote_" + (iT + 1), "192.168.16." + (22 + iT), HardFileName);
                    _sPata = _IpArr_remote[iT].Split('.');
                    if (_sPata.Length == 4)
                        _strAll_IP += (_strAll_IP != "" ? "," : "") + _sPata[3];
                }
                //模块IP
                Txt_Coat_Remote_IP.Text = _strAll_IP;
                //模块端口
                Txt_Coat_Remote_Port.Text = SysInfo.csInter.IniReadDefine("Coat", "m_Port_remote", "13000", HardFileName);
                #endregion UDP
                #endregion
            }
            else
            {
                #region 写
                //1 耗时间隔
              //  SysInfo.csInter.INIWriteValue("Coat", "m_iTimeDelay", Txt_iTimeDelay.Text, HardFileName);

                //主机IP
                SysInfo.csInter.INIWriteValue("Coat", "m_Ip_local", Txt_Coat_Main_IP.Text + "." + Txt_Coat_Loat_IP.Text, HardFileName);

                //主机端口
                SysInfo.csInter.INIWriteValue("Coat", "m_Port_local", Txt_Coat_Loat_Port.Text, HardFileName);

                //模块IP
                Txt_Coat_Remote_IP.Text = Txt_Coat_Remote_IP.Text.Replace("，", ",");
                string[] _sPara = Txt_Coat_Remote_IP.Text.Split(',');
                string[] _sArrPara = "".Split('.');
                int _iNum = _sPara.Length;

                              SysInfo.csInter.INIWriteValue("Coat", "m_iRomoteNum", _iNum.ToString(), HardFileName);
                               //模块端口
                SysInfo.csInter.INIWriteValue("Coat", "m_Port_remote", Txt_Coat_Remote_Port.Text, HardFileName);
                #endregion
            }
        }

        private void UI_IP_Init_Ect(int iType = 0)
        {
            string HardFileName = System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini";
            if (iType == 0)
            {
                #region 读
                //探头个数
                int _iRomoteNum = int.Parse(SysInfo.csInter.IniReadDefine("Pulsed_Eddy", "m_iRomoteNum", "1", HardFileName));
                if (_iRomoteNum == 0) _iRomoteNum = 1;
                Txt_ECT_m_iRomoteNum.Text = _iRomoteNum.ToString();
                //负压动力
                Ck_Fy.Checked = int.Parse(SysInfo.csInter.IniReadDefine("Pulsed_Eddy", "m_bl_My_Distanc", "1", HardFileName))==1;

                //主机IP
                string _Ip_local = SysInfo.csInter.IniReadDefine("Pulsed_Eddy", "m_Ip_local", "192.168.1.100", HardFileName);
                string[] _sPata = _Ip_local.Split('.');
                if (_sPata.Length == 4)
                {
                    Txt_ECT_Main_IP.Text = _sPata[0] + "." + _sPata[1] + "." + _sPata[2];
                    Txt_ECT_Loat_IP.Text = _sPata[3];
                }
                //主机端口
                Txt_ECT_Port.Text = SysInfo.csInter.IniReadDefine("Pulsed_Eddy", "m_Port_local", "13000", HardFileName);

                //模块IP
                _sPata = SysInfo.csInter.IniReadDefine("Pulsed_Eddy", "m_Ip_Remote", "192.168.1.101", HardFileName).Split('.');
                if (_sPata.Length == 4)
                    Txt_ECT_Remote_IP.Text = _sPata[3];
                //模块端口
                Txt_ECT_Remote_Port.Text = SysInfo.csInter.IniReadDefine("Pulsed_Eddy", "m_Port_remote", "13001", HardFileName);
                //探头起始地址
                Txt_ECT_No_S.Text = SysInfo.csInter.IniReadDefine("Pulsed_Eddy", "m_str_ECT_No_S", "0102", HardFileName);
                #endregion
            }
            else
            {
                #region 写
                int _iNum = int.Parse(Txt_ECT_m_iRomoteNum.Text.Trim());
                SysInfo.csInter.INIWriteValue("Pulsed_Eddy", "m_iRomoteNum", _iNum.ToString (), HardFileName);
                //负压动力
                SysInfo.csInter.INIWriteValue("Pulsed_Eddy", "m_bl_My_Distanc",(Ck_Fy .Checked ? "1":"0"), HardFileName);

                //主机IP
                SysInfo.csInter.INIWriteValue("Pulsed_Eddy", "m_Ip_local", Txt_ECT_Main_IP.Text + "." + Txt_ECT_Loat_IP.Text, HardFileName);

                //主机端口
                SysInfo.csInter.INIWriteValue("Pulsed_Eddy", "m_Port_local", Txt_ECT_Port.Text, HardFileName);
               //模块IP
                SysInfo.csInter.INIWriteValue("Pulsed_Eddy", "m_Ip_Remote", Txt_ECT_Main_IP.Text + "."+ Txt_ECT_Remote_IP.Text, HardFileName);
                //模块端口号
                SysInfo.csInter.INIWriteValue("Pulsed_Eddy", "m_Port_remote", Txt_ECT_Remote_Port.Text, HardFileName);
              
                //探头起始地址
                SysInfo.csInter.INIWriteValue("Pulsed_Eddy", "m_str_ECT_No_S", Txt_ECT_No_S.Text, HardFileName);
                #endregion
            }
        }

        private void Bt_Fw_D_Click(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            if (Track_Fw.Value > Track_Fw.Minimum) Track_Fw.Value = Track_Fw.Value - 1;
            Track_Fw_Change();
        }
        private void Track_Fw_Change()
        {
            SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iRange = Track_Fw.Value;
            Txt_Fw.Text = (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iRange).ToString("") + "mm";

            Tofd.SendCmdFreqRatio(SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_iRange, SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_dSpeed);
            Lb_Send.Text = "范围: " + Txt_Fw.Text;
            Cal_JG();
        }

        private void Bt_Alarm_A_Click(object sender, EventArgs e)
        {
            int _iD = int.Parse(Txt_m_i_Alarm_Limit.Text);
            SysInfo.m_SysBuff.m_Tofd_DLL.m_i_Alarm_Limit = ++_iD;
            Txt_m_i_Alarm_Limit.Text = SysInfo.m_SysBuff.m_Tofd_DLL.m_i_Alarm_Limit.ToString();
            SysInfo.csInter.INIWriteValue("TOFD", "m_i_Alarm_Limit", _iD.ToString(), SysInfo.HardFileName);
        }

        private void Bt_Alarm_D_Click(object sender, EventArgs e)
        {
            int _iD = int.Parse(Txt_m_i_Alarm_Limit.Text);
            _iD--;
            if (_iD > 0)
            {
                SysInfo.m_SysBuff.m_Tofd_DLL.m_i_Alarm_Limit = _iD;
                Txt_m_i_Alarm_Limit.Text = SysInfo.m_SysBuff.m_Tofd_DLL.m_i_Alarm_Limit.ToString();

                SysInfo.csInter.INIWriteValue("TOFD", "m_i_Alarm_Limit", _iD.ToString(), SysInfo.HardFileName);
            }
        }

        private void Bt_IP_Visi_Click(object sender, EventArgs e)
        {
            Grp_UDP.Visible = true;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex>-1)
            Set_Alarm(tabControl1.TabPages [tabControl1 .SelectedIndex ].Name      == "tabPage2");
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Txt_WaitTims.Enabled = !Txt_WaitTims.Enabled;
        }

        private void Rad_C_D_Click(object sender, EventArgs e)
        {
            InitColor(SysInfo.m_str_B_Stand_Color);
            SysInfo.InitLimitPic(SysInfo.m_str_B_Stand_Color, m_i_Stand_Num, P_1, P_Md, P_2, Grp_Stand_Color);
        }

        private void Rad_C_A_Click(object sender, EventArgs e)
        {
            InitColor(SysInfo.m_str_B_Stand_Color_A);
            SysInfo.InitLimitPic(SysInfo.m_str_B_Stand_Color_A, m_i_Stand_Num, P_1, P_Md, P_2, Grp_Stand_Color);
        }

        private void Bt_Copy_Click(object sender, EventArgs e)
        {
            Rad_C_A.Checked = true;
            Rad_C_D.Checked = false;
       
            SysInfo.m_str_B_Stand_Color = SysInfo.csInter.IniReadDefine("Cls_Plant", "m_str_B_Stand_Color_OO", SysInfo.m_str_B_Stand_Color, SysInfo.HardFileName);
            SysInfo.m_str_B_Stand_Color_A = SysInfo.csInter.IniReadDefine("Cls_Plant", "m_str_B_Stand_Color_AA", SysInfo.m_str_B_Stand_Color_A, SysInfo.HardFileName);
           
            InitColor(SysInfo.m_str_B_Stand_Color_A);
            SysInfo.InitLimitPic(SysInfo.m_str_B_Stand_Color_A, m_i_Stand_Num, P_1, P_Md, P_2, Grp_Stand_Color);
        }

        private void Rad_R_Click(object sender, EventArgs e)
        {
            Set_Xg(1);

        }
        private void Set_Xg(int iType)
        {
            switch (iType )
            {
                case 1:
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_XiangGuan = true;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_PingJun = false;
                    break;
                case 2:
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_XiangGuan = false;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_PingJun = true;

                    break;
                default:
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_XiangGuan = false;
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_PingJun = false;
                    break;
            }
        }
        private void Set_From_Xg()
        {
          if(SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_XiangGuan)
            {
                Rad_R.Checked = true;
                Rad_Ave.Checked = false;
                Rad_Raw.Checked = false;
            }
            else  if (SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_PingJun)
            {
                Rad_R.Checked = false;
                Rad_Ave.Checked = true;
                Rad_Raw.Checked = false;
            }
            else
            {
                Rad_R.Checked = false;
                Rad_Ave.Checked = false;
                Rad_Raw.Checked = false;
            }
        }
        private void Rad_Ave_Click(object sender, EventArgs e)
        {
            Set_Xg(2);
        }

        private void Rad_Raw_Click(object sender, EventArgs e)
        {
            Set_Xg(3);
        }

        private void track_Xg_Scroll(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Tofd_DLL.m_i_XiangGuan_Num = (byte)track_Xg.Value;
            Txt_Xg.Text = SysInfo.m_SysBuff.m_Tofd_DLL.m_i_XiangGuan_Num.ToString();

        }

        private void Bt_Z_Add_Click(object sender, EventArgs e)
        {
            Set_Lp();
        }

        private void Bt_Z_Dll_Click(object sender, EventArgs e)
        {
            Set_Lp(false);
        }

        private void Rad_Coat_Click(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 = 3;
            Grp_Coat.Visible = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3;
            SysInfo.csInter.INIWriteValue("System", "m_i_TOFD_0_Cscan_1", SysInfo.m_i_TOFD_0_Cscan_1_Mui_2.ToString(),
                                   Application.StartupPath + "\\database\\SysConfig.ini");
        }

        private void label48_Click(object sender, EventArgs e)
        {
            Cmb_UI_DLL_Type.Visible = !Cmb_UI_DLL_Type.Visible;
            label45.Visible = Cmb_UI_DLL_Type.Visible;
        }

        private void Bt_Osk_Click(object sender, EventArgs e)
        {
            int _iL =this .Left + groupBox1.Left + Bt_Osk.Left-160;
            int _iT =this .Top + Bt_Osk.Height + 80 + Bt_Osk.Top;

            SysInfo.SetKeyBorad(_iL, _iT, "num_keyboard");
        }

        private void Rad_Print_3_Click(object sender, EventArgs e)
        {
            SysInfo.m_i_ReportType = 2;
        }

        private void Ck_Report_Click(object sender, EventArgs e)
        {
            Pan_Tofd.Visible =! Pan_Tofd.Visible;
            Pan_C_Scan.Visible = !Pan_Tofd.Visible;
        }

        private void Ck_Have_TOFD_Click(object sender, EventArgs e)
        {
            SysInfo.csInter.INIWriteValue("TOFD", "Have", (Ck_Have_TOFD.Checked ?"1": "0"), SysInfo.HardFileName);
        }

        private void Ck_Begin_Down_CheckedChanged(object sender, EventArgs e)
        {
            SysInfo.m_SysBuff.m_Climb.i_Begin_Down = Ck_Begin_Down.Checked ? 1 : 0;
              SysInfo.csInter.INIWriteValue ("TOFD", "i_Begin_Down", 
                   SysInfo.m_SysBuff.m_Climb.i_Begin_Down.ToString (), SysInfo.HardFileName);
        }

        private void label44_Click(object sender, EventArgs e)
        {
            Pan_PassWord.Visible = !Pan_PassWord.Visible ;
        }
        private void Ctrl_Mody(bool blVal)
        {
            string _strTitl = SysInfo.m_iLanguage == 0 ? "请输入密码" : "Please enter password";
            string _strMsg = SysInfo.m_iLanguage == 0 ? "密码: " : "Password:";
            string strKey =Txt_Password .Text ;// Interaction.InputBox(_strTitl, _strMsg, "", -1, -1);
            string _strInput=  SysInfo.csInter.IniReadDefine ("TOFD", "Input", "400698", SysInfo.HardFileName);
            if(_strInput==strKey )
            {
                Rad_Tofd.Enabled = blVal;
                Rad_M_UI.Enabled = blVal;

             
            }
            Pan_PassWord.Visible = false;
            Txt_Password.Text = "";
        }

        private void Bt_Pass_Click(object sender, EventArgs e)
        {
            Ctrl_Mody(!Rad_Tofd.Enabled);
        }

        private void Txt_i_C_UIDLL_Coat_iWaitTime_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Txt_RecDisc_Time_TextChanged(object sender, EventArgs e)
        {
            int _iDat = 0;
            try
            {
                _iDat = int.Parse(Txt_RecDisc_Time.Text);

                SysInfo.m_i_Add_MarkDisc = _iDat;  
                SysInfo.csInter.INIWriteValue ("System", "m_i_Add_MarkDisc", _iDat.ToString (), SysInfo.HardFileName);
            }
            catch { Txt_RecDisc_Time.Text = "9"; }

              
        }

        private void Bt_Mul_Thick_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_blFrmOpen[8] == false)
            {
           //     Frm_Mul_Thick _frm = new Frm_Mul_Thick();

          //      _frm.ShowDialog ();
                SysInfo.m_SysBuff.m_Climb.flstrThickAlarm = SysInfo.m_SysBuff_C.m_Plant_C.flNormal_Thickness;
                flNormal_Thickness.Text = SysInfo.m_SysBuff_C.m_Plant_C.flNormal_Thickness.ToString();
            }
        }

        private void Ck_bl_Discharge_CheckedChanged(object sender, EventArgs e)
        {
            Txt_RecDisc_Time.Visible = Ck_bl_Discharge.Checked;
        }

        private void Cmb_UI_DLL_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void Txt_ECT_m_iRomoteNum_TextChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            int _iT = 0;
            try
            {
                _iT = int.Parse(Txt_ECT_m_iRomoteNum.Text);
            }
            catch { Txt_ECT_m_iRomoteNum.Text = "1"; }
        }

        private void Bt_CalCu_JL_Click(object sender, EventArgs e)
        {
     
        }
        
        private void Init_Video()
        {
            Ck_1.Checked = SysInfo.csInter.IniReadDefine ("Cam", "Video_1", "1", m_NetP)=="1";
            Ck_2.Checked = SysInfo.csInter.IniReadDefine("Cam", "Video_2", "0", m_NetP) == "1";
            Ck_3.Checked = SysInfo.csInter.IniReadDefine("Cam", "Video_3", "0", m_NetP) == "1";
            Ck_4.Checked = SysInfo.csInter.IniReadDefine("Cam", "Video_4", "0", m_NetP) == "1";
        }
        private void Ck_1_Click(object sender, EventArgs e)
        {
            SysInfo.csInter.INIWriteValue("Cam", "Video_1", Ck_1.Checked ? "1" : "0", m_NetP);
        }

        private void Ck_2_Click(object sender, EventArgs e)
        {
            SysInfo.csInter.INIWriteValue("Cam", "Video_2", Ck_2.Checked ? "1" : "0", m_NetP);
        }

        private void Ck_3_Click(object sender, EventArgs e)
        {
            SysInfo.csInter.INIWriteValue("Cam", "Video_3", Ck_3.Checked ? "1" : "0", m_NetP);
        }

        private void Ck_4_Click(object sender, EventArgs e)
        {
            SysInfo.csInter.INIWriteValue("Cam", "Video_4", Ck_4.Checked ? "1" : "0", m_NetP);
        }
    }
}
