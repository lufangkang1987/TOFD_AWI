using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Tofd_AWI.Class;
namespace Tofd_AWI.From
{
    public partial class Frm_Move : Form
    {
        /// <summary>
        /// 光栅臂抬起1落下0
        /// </summary>
        int m_iGsbTqLx = 0;
        /// <summary>
        /// 缺陷打标1抬起0
        /// </summary>
        int m_iAlarmMark = 0;
        /// <summary>
        /// 界面激活
        /// </summary>
        bool m_blActive = false;
        public Frm_Move()
        {
            InitializeComponent();
        }

        private void Frm_Move_Load(object sender, EventArgs e)
        {
            SysInfo.Coat_SentCmd_Black();
            //  SysInfo.m_SysBuff.m_Climb.m_i_Xs = int.Parse(SysInfo.csInter.IniReadDefine("SYSinfo", "iSpeed_Xs", "3", SysInfo.HardFileName));
            SysInfo.m_SysBuff.m_Climb.m_UI_Gsb_Distan_Min = int.Parse(SysInfo.csInter.IniReadDefine("SYSinfo", "m_UI_Gsb_Distan_Min", "5", SysInfo.HardFileName));// SysInfo.m_SysBuff.m_Climb.m_i_Xs;// int.Parse((SysInfo.m_SysBuff.m_Climb.i_Para_Speed_Gsb / SysInfo.m_SysBuff.m_Climb.m_i_Xs).ToString("f0"));
            
            NumAdd.Value = SysInfo.m_SysBuff.m_Climb.m_UI_Gsb_Distan_Min;
            Cmb_Item.SelectedIndex = 0;
            #region 联机参数
            //Lb_LinkState.Text = "联机:" + (SysInfo.m_Climb4.m_blLink ? "成功" : "失败");
            //Lb_LinkState.ForeColor = SysInfo.m_Climb4.m_blLink ? Color.White : Color.Yellow;

            Ck_Auto1_ByHand.Checked = SysInfo.m_i_Climb_Hand0_Auto1 == 1;
            string[] _arrPort_Names = System.IO.Ports.SerialPort.GetPortNames();
            Cmb_Climb.Items.Clear();
            for (int i = 0; i < _arrPort_Names.Length; i++)
                Cmb_Climb.Items.Add(_arrPort_Names[i]);

            //Lb_LinkState.Text = "联机:" + (SysInfo.m_Climb4.m_blLink ? "成功" : "失败");
            //Lb_LinkState.ForeColor = SysInfo.m_Climb4.m_blLink ? Color.White : Color.Red;
            Ck_Com_Can.Checked = SysInfo.m_Climb4.m_blCom1_Can0 == 1;
            // SysInfo.m_Climb4.m_blCom1_Can0 = int.Parse(SysInfo.csInter.IniReadDefine("COM_Can", "m_blCom1_Can0", "1", SysInfo.HardFileName));
            //搜索电脑COM
            m_i_Move_Add = int.Parse(SysInfo.csInter.IniReadDefine("COM_Can", "m_i_Move_Add", "1", SysInfo.HardFileName));
            Cmb_Climb.Text = SysInfo.csInter.IniReadDefine("COM_Can", "COM", "COM3", SysInfo.HardFileName);
            Cmb_Climb.Visible = Ck_Com_Can.Checked;

            #endregion
            SysInfo.m_blFrmOpen[3] = true;

            if (SysInfo.m_bl_Q1_H0)
                Rad_L_Q.Checked = true;
            else
                Rad_L_H.Checked = true;

            Ck_Mark.Checked = SysInfo.m_bl_Mark;

            Rad_JP_L.Checked = SysInfo.m_Climb4.m_i_Jp == 1;
            Rad_JP_R.Checked = SysInfo.m_Climb4.m_i_Jp == 2;
            Rad_JP_S.Checked = SysInfo.m_Climb4.m_i_Jp == 3;

            Lb_Titl.Text = SysInfo.m_SysBuff.m_Climb.Speed.ToString();// + "%";
            Lb_Gsb.Text = SysInfo.m_SysBuff.m_Climb.i_Para_Speed_Gsb.ToString();//+ "%";
            Track_Sd.Value = (int)SysInfo.m_SysBuff.m_Climb.Speed;
            //MessageBox.Show(" " + SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval);
            Track_Gsb.Value = (int)SysInfo.m_SysBuff.m_Climb.i_Para_Speed_Gsb;
            if (SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval < Num_BjJl.Minimum)
                SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval =(int) Num_BjJl.Minimum;
            Num_BjJl.Value = SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval;
            Num_4_Qd.Value = SysInfo.m_SysBuff.m_Climb.i_Gsb_Start_Pos;
            //if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
            //{
            //    Num_4_Zd.Maximum = SysInfo.m_SysBuff.m_Climb.iGsb_Len - 20;
            //    if (SysInfo.m_SysBuff.m_Climb.i_Gsb_End_Pos > SysInfo.m_SysBuff.m_Climb.iGsb_Len-20)
            //        SysInfo.m_SysBuff.m_Climb.i_Gsb_End_Pos = SysInfo.m_SysBuff.m_Climb.iGsb_Len-20;
            //}
            //else
            {
                Num_4_Zd.Maximum = SysInfo.m_SysBuff.m_Climb.iGsb_Len;
                if (SysInfo.m_SysBuff.m_Climb.i_Gsb_End_Pos > SysInfo.m_SysBuff.m_Climb.iGsb_Len)
                    SysInfo.m_SysBuff.m_Climb.i_Gsb_End_Pos = SysInfo.m_SysBuff.m_Climb.iGsb_Len;
            }
            
            Num_Coat_Bj.Value = SysInfo.m_SysBuff.m_Climb.i_Coat_Interval;


            SysInfo.Whzk();
            cK_Bmq_T0_41.Checked = SysInfo.m_SysBuff.m_Climb.iBmq_Type == 1 ? true : false;
            string _strP = Application.StartupPath + "\\ICO";
            try
            {
                //1 左右
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\1-1.png"));//0
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\1-2.png"));//1
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\2-1.png"));//2
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\2-2.png"));//3
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\3-1.png"));//4
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\3-2.png"));//5

                Img_Lst.Images.Add(Image.FromFile(_strP + "\\4-Left.png"));//6
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\4-Right.png"));//7
            }
            catch { }

          
            Ck_Track_Auto.Checked = SysInfo.m_bl_Track_Auto;
            m_iGsbTqLx = SysInfo.m_SysBuff.m_Climb.iGsbTtLx;
            m_iAlarmMark = SysInfo.m_SysBuff.m_Climb.iAlarmMark;
            Bt_Ty.BackgroundImage = Img_Lst.Images[m_iGsbTqLx == 0 ? 4 : 5];
            Bt_Alarm.BackgroundImage = Img_Lst.Images[m_iAlarmMark == 0 ? 4 : 5];
            //GsbTqLx();
            //MarkTqLx();

            //Bt_Up.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            //Bt_Down.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            //Bt_Ty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;

            Pic_4_Sc1_Zx0.BackgroundImage = Img_Lst.Images[SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 == 1 ? 6 : 7];
            Pic_4_Qj1_Ht0.BackgroundImage = Img_Lst.Images[SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Qj1_Ht0 == 1 ? 6 : 7];
            Num_4_Qd.Text = SysInfo.m_SysBuff.m_Climb.i_Gsb_Start_Pos.ToString();
            Num_4_Zd.Text = SysInfo.m_SysBuff.m_Climb.i_Gsb_End_Pos.ToString();
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 != 3)
            {
                Lb_Step_Coat.Visible = false;
                Num_Coat_Bj.Visible = false;
            }
            Pan_Gsb_Mul.Visible = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2;
            Pan_Gsb_1.Visible = !Pan_Gsb_Mul.Visible;

            if (SysInfo.m_iLanguage == 1)
            {
                string _strTool = "01: sweep start" + "\r\n" +
                                "02: Sweep stops" + "\r\n" +
                                "03: Left scan" + "\r\n" +
                                "04: right scan" + "\r\n" +
                                "05: Full stroke left sweep(used to set sweep interval value)" + "\r\n" +
                                "06: Full stroke right scan(used to set sweep interval value)" + "\r\n" +
                                "07: Full stroke left sweep(used to set the sweep interval value _ for convenience only the keys do not show the handle)" + "\r\n" +
                                "08: Full stroke right scan(used to set the sweep interval value _ for convenience only the keys do not show the handle)" + "\r\n" +
                                "10: Walk to position 1(temporarily used for C thickness scanning when the grating arm reaches the specified position)" + "\r\n" +
                   "11: Walk to position 2(temporarily used for C thickness scanning when the grating arm reaches the specified position) ";
                Tool_Tip(Gsb_Int, _strTool);

                Cmb_Item.Visible = false;
                Ck_Mark.Visible = false;
                Bt_Cxfhzt.Visible = false;
                this.Text = "Move control";

                cK_Bmq_T0_41.Text = SysInfo.m_i_UI_Type==0 && SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0 ? "TOFD0/4whee.1" : "Dist.Cali.";

            }
            else
            {
                Lb_Step_Coat.Text = "光栅步进距离mm";

                cK_Bmq_T0_41.Text = SysInfo.m_i_UI_Type == 0 && SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0 ? "TOFD0/4轮1" : "4轮编码器";
            }
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3)
            {

                Pan_Gsb_1.Visible = false;
                Pan_Gsb_Mul.Visible = false;
                //纠偏
                Rad_JP_L.Visible = false; Rad_JP_S.Visible = false; Rad_JP_R.Visible = false;
                label4.Visible = false; label5.Visible = false; label6.Visible = false;
                Pan_Gsb_1.Visible = false;//左右移动

                //步进距离
                //      Num_BjJl.Enabled = false; label3.Enabled = false;
                Pic_4_Sc1_Zx0.Enabled = false;//扫查/直行

                //Lb_4_Sc.Visible = false;//扫查
                //Lb_4_Zx.Visible = false;//直行

                SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 = 1;

                Pic_4_Sc1_Zx0.BackgroundImage = Img_Lst.Images[SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 == 1 ? 6 : 7];
                Set_ZxParaVis(SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 == 0 ? false : true);

                //光栅臂起止
                string _strT = SysInfo.csInter.IniReadDefine("ClassUltrasGate", "Txt_Mul_Select", "", System.Windows.Forms.Application.StartupPath + "\\database\\HardConfig.ini");

                bool _bl3 = false;// (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 && SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Only_Coat &&
               //     SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_iRomoteNum > 0 && _strT.IndexOf("1") > -1);
                if (_bl3 == false)
                {
                    Num_4_Qd.Enabled = false; Num_4_Zd.Enabled = false;
                    Lb_4_Qd.Enabled = false; Lb_4_Zd.Enabled = false;
                }
                //手动速度
                Ck_Auto1_ByHand.Enabled = false;
                Ck_Auto1_ByHand.Checked = false;
                SysInfo.m_i_Climb_Hand0_Auto1 = Ck_Auto1_ByHand.Checked ? 1 : 0;
                SysInfo.csInter.INIWriteValue("System", "m_i_Climb_Hand0_Auto1", SysInfo.m_i_Climb_Hand0_Auto1.ToString(), SysInfo.HardFileName);
            }
           
                SysInfo.Language(this, "Move_control");
            if (SysInfo.m_iLanguage == 1)
                cK_Bmq_T0_41.Text = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0 ? "TOFD0/4whee.1" : "Dist.Cali.";
            else
                cK_Bmq_T0_41.Text = SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 0 ? "TOFD0/4轮1" : "4轮编码器";
            LinkState(1);
            m_blActive = true;
        }
        private void GsbTqLx()
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            m_iGsbTqLx = m_iGsbTqLx == 0 ? 1 : 0;
            SysInfo.m_Climb4.SendData(5, 1, 0, 0, m_iGsbTqLx.ToString());

            Lb_GsbTitl.Text = m_iGsbTqLx == 0 ? "光栅臂抬起" : "光栅臂落下";
            Bt_Ty.BackgroundImage = Img_Lst.Images[m_iGsbTqLx == 0 ? 4 : 5];
        }
        private void MarkTqLx()
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            m_iAlarmMark = m_iAlarmMark == 0 ? 1 : 0;
            SysInfo.m_Climb4.SendData(5, 2, 0, 0, m_iAlarmMark.ToString());

            Lb_Alarm.Text = m_iAlarmMark == 1 ? " 打标" : "停止打标";
            Bt_Alarm.BackgroundImage = Img_Lst.Images[m_iAlarmMark == 0 ? 4 : 5];
        }
        private void Frm_Move_FormClosing(object sender, FormClosingEventArgs e)
        {
            SysInfo.SetKeyBorad("num_keyboard");
            SysInfo.m_blFrmOpen[3] = false;
            //       SysInfo.Whzk();
            Value_Change();
            Init();
          //  MessageBox.Show("jieguo " + SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval);
        }

    
        private void Bt_Down_Click(object sender, EventArgs e)
        {
            try
            {
                if (SysInfo.m_Climb4.m_blLink == false) return;
                //01：前进    02：后退      03：停止 04：前进左转 
                //05：前进右转 06：后退左转 07：后退右转
                //  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
                bool _blSendSpeed = false;
                if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ||
                    SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    _blSendSpeed = true;
                if (_blSendSpeed)
                    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "6");
            }
            catch { }
        }

        private void Bt_Stop_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            bool _blSendSpeed = false;
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ||
                SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                _blSendSpeed = true;
            if (_blSendSpeed)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
        }
        int m_i_Move_Add = 1;
        private void Bt_Add_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            if (Track_Sd.Value +1< Track_Sd.Maximum)
                Track_Sd.Value += m_i_Move_Add;

            Set_Speed();
        }

        private void Bt_Desc_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            if (Track_Sd.Value -1> Track_Sd.Minimum)
                Track_Sd.Value -= m_i_Move_Add;
            Set_Speed();
        }

        private void Track_Sd_Scroll(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;

            Set_Speed();
        }
        private void Set_Speed()
        {
            SysInfo.m_SysBuff.m_Climb.Speed = (int)Track_Sd.Value;
            SysInfo.m_SysBuff.m_Climb.i_Para_Speed_Gsb = (int)Track_Gsb.Value;
            SysInfo.SetSpeed();

            Lb_Titl.Text = Track_Sd.Value.ToString();// + "%";
            Lb_Gsb.Text = Track_Gsb.Value.ToString();// + "%";
        }
     
        private void Bt_Ty_Click(object sender, EventArgs e)
        {
        //   m_iGsbTqLx = m_iGsbTqLx == 0 ? 1 : 0;
            GsbTqLx();
        }

      

        private void Ck_Com_Can_CheckedChanged(object sender, EventArgs e)
        {
            Cmb_Climb.Visible = Ck_Com_Can.Checked;
        }

        private void Bt_ReLink_Click(object sender, EventArgs e)
        {
            SysInfo.Link_Climb_Com(Ck_Com_Can.Checked ? 1 : 0, Cmb_Climb.Text);
          
            if (SysInfo.m_Climb4.m_blLink == false)
            {
                SysInfo.m_Climb4.CloseSet();
                if (SysInfo.m_Client.m_blLinkServe == false)
                {
                    SysInfo.m_strLinkMsg = SysInfo.m_strLinkMsg.Replace(" 车体联机失败,", "");
                    if (SysInfo.m_strLinkMsg == "")
                        SysInfo.m_strLinkMsg = SysInfo.m_strLinkMsg.Replace(" 车体联机失败", "");
                    else
                        SysInfo.m_strLinkMsg = SysInfo.m_strLinkMsg.Replace(", 车体联机失败", "");
                    SysInfo.m_strLinkMsg += (SysInfo.m_strLinkMsg != "" ? "," : "") + " 车体联机失败";
                }
            }
            LinkState();
            if (SysInfo.m_Climb4.m_blLink)
            {
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
                SysInfo.csInter.INIWriteValue ("COM_Can", "COM", Cmb_Climb.Text, SysInfo.HardFileName);
           //     if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 && SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have)
                {
                    SysInfo.Coat_SentCmd_Black();

                }
            }
        }

        private void LinkState(int iType=0)
        {
            Lb_LinkState.ForeColor = SysInfo.m_Climb4.m_blLink ? Color.White : Color.Yellow;
            if (SysInfo.m_iLanguage == 1)
            {
                Lb_LinkState.Text = "State:" + (SysInfo.m_Climb4.m_blLink ? "Success" : "Failure");
                MessageBox.Show("Link status:：" + (SysInfo.m_Climb4.m_blLink ? "Success" : "Failure") + "\r\n\r\n" + SysInfo.m_strLinkMsg);
            }
            else
            {
                Lb_LinkState.Text = "联机:" + (SysInfo.m_Climb4.m_blLink ? "成功" : "失败");
               if(iType==0)
                MessageBox.Show("车体联机：" + (SysInfo.m_Climb4.m_blLink ? "成功" : "失败") + "\r\n\r\n" + SysInfo.m_strLinkMsg);
            }
            
        }

        private void Bt_Alarm_Click(object sender, EventArgs e)
        {
            MarkTqLx();
        }

        private void Bt_H_Click(object sender, EventArgs e)
        {
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
            //  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
            bool _blSendSpeed = false;
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ||
                SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                _blSendSpeed = true;
            if (_blSendSpeed)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "2");
        }

        private void Bt_H_R_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
            //   SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
            bool _blSendSpeed = false;
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ||
                SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                _blSendSpeed = true;
            if (_blSendSpeed)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "7");

        }

        private void Bt_Q_Click(object sender, EventArgs e)
        {
          //  if (SysInfo.m_Climb4.m_blLink == false) return;
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
            //   SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 1;
            bool _blSendSpeed = false;
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ||
                SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                _blSendSpeed = true;
            if (_blSendSpeed)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "1");
        }

        private void Bt_U_L_Click(object sender, EventArgs e)
        {
        //    if (SysInfo.m_Climb4.m_blLink == false) return;
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
            //  SysInfo.m_SysBuff  .m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff  .m_Tofd_DLL.m_icurChan].m_iEnPos = 1;
            bool _blSendSpeed = false;
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ||
                SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                _blSendSpeed = true;
            if (_blSendSpeed)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "4");
        }

        private void Bt_Q_R_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
            bool _blSendSpeed = false;
            if (SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1 || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 2 ||
                SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                _blSendSpeed = true;
            if (_blSendSpeed)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "5");
        }

        private void cK_Bmq_T0_41_Click(object sender, EventArgs e)
        {
            SysInfo .m_SysBuff   .m_Climb.iBmq_Type = cK_Bmq_T0_41.Checked ? 1 : 0;
            SysInfo.csInter.INIWriteValue("System", "iBmq_Type", SysInfo.m_SysBuff.m_Climb.iBmq_Type.ToString(), SysInfo.HardFileName);
        }


        private void Bt_Jl_Clear_Click(object sender, EventArgs e)
        {
            SysInfo. Clear_Dis();
            return;
            if (    SysInfo .m_SysBuff   .m_Climb.iBmq_Type == 1)
                SysInfo.m_Climb4.SendData(2, 3, 0, 0, "0");
            else
                SysInfo.m_SysBuff.m_Tofd_DLL.Init_Encoder();
        }

        private void Ck_Auto1_ByHand_Click(object sender, EventArgs e)
        {
            SysInfo.m_i_Climb_Hand0_Auto1 = Ck_Auto1_ByHand.Checked ? 1 : 0;
           SysInfo . csInter.INIWriteValue ("System", "m_i_Climb_Hand0_Auto1", SysInfo.m_i_Climb_Hand0_Auto1.ToString (), SysInfo.HardFileName);
        }

        private void Ck_Track_Auto_Click(object sender, EventArgs e)
        {
            SysInfo.m_bl_Track_Auto = Ck_Track_Auto.Checked;
            if (SysInfo.m_Climb4.m_blLink == false) return;
            if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                SysInfo.m_Climb4.SendData(1, 1, 8, 0, (Ck_Track_Auto.Checked ? "17":"18"));//0x11：视觉寻迹行走打开     0x12：视觉寻迹行走关闭
                                                                                           //01：左纠偏
           // 02：右纠偏

           //   03：停止纠偏
        }
        /// <summary>
        /// 01：左纠偏    02：右纠偏   03：停止纠偏
        /// </summary>
        /// <param name="iData"></param>
        private void Set_JP(int iData)
        {
            if (iData > 0 && iData < 4)
            {
                if (SysInfo.m_Climb4.m_blLink == false) return;
                if (SysInfo.m_SysBuff.m_Tofd_DLL.m_pSparam[SysInfo.m_SysBuff.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    SysInfo.m_Climb4.SendData(1, 1, 8, 0, iData.ToString());//0x11：视觉寻迹行走打开     0x12：视觉寻迹行走关闭
                                                                            //01：左纠偏
                                                                            // 02：右纠偏
                                                                            // 03：停止纠偏
                SysInfo.m_Climb4.m_i_Jp = iData;
            }
        }
        int m_i_DaBiao = 0;
        private void Bt_Alarm_MouseDown(object sender, MouseEventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
           // m_iAlarmMark = m_iAlarmMark == 0 ? 1 : 0;
            m_iAlarmMark = 1;
            //if (Ck_Mark.Checked == false || SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 3 && SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Have==false )
            //{
            //    SysInfo.m_Climb4.SendData(5, 2, 0, 0, m_iAlarmMark.ToString());

            //    Lb_Alarm.Text = m_iAlarmMark == 1 ? " 打标" : "停止打标";
            //    Bt_Alarm.BackgroundImage = Img_Lst.Images[m_iAlarmMark == 0 ? 4 : 5];
            //}
            //else
            {
                Ctrl_4_DB();
            }
        }
       private void Ctrl_4_DB()
        {
            switch (Cmb_Item.SelectedIndex)
            {
                /*
                 打标
                降机构
                升机构
                停止升降
                旋出笔芯
                旋入笔芯
                停止笔芯动作
                设置有打标功能
                设置没有打标功能
                自动检测打标笔是否安装
                询问打标笔是否已安装
                询问打标笔芯是否耗尽
                笔芯旋出长度自动识别(关闭)
                笔芯旋出长度自动识别(开启)
                 */
                case 0://打标动作
                    if (m_i_DaBiao == 0)
                    {
                        m_i_DaBiao = 1;
                        SysInfo.DaBiao();
                        m_i_DaBiao = 0;
                    }
                    break;
                case 1://打标机构降
                    SysInfo.m_Climb4.SendData(5, 4, 2);
                    break;
                case 2://打标机构升
                    SysInfo.m_Climb4.SendData(5, 4, 3);
                    break;
                case 3://打标机构停止升降
                    SysInfo.m_Climb4.SendData(5, 4, 4);
                    break;
                case 4://打标笔芯旋出
                    SysInfo.m_Climb4.SendData(5, 4, 11);
                    break;
                case 5://打标笔芯旋入
                    SysInfo.m_Climb4.SendData(5, 4, 12);
                    break;
                case 6://打标笔芯停止旋出/入
                    SysInfo.m_Climb4.SendData(5, 4, 13);
                    break;
                case 7://设置为打标笔已经安装
                    SysInfo.m_Climb4.SendData(5, 4, 21);
                    break;
                case 8://设置为打标笔未安装
                    SysInfo.m_Climb4.SendData(5, 4, 22);
                    break;



                case 9://车体自动识别打标笔是否安装
                    SysInfo.m_Climb4.SendData(5, 4, 31);
                    break;
                case 10://控制器询问车体打标笔是否已安装
                    SysInfo.m_Climb4.SendData(5, 4, 23);
                    /*
                24	（车体上传打标笔是否安装）未安装
                25	（车体上传打标笔是否安装）已安装
                   */
                    break;
                case 11://控制器询问车体打标笔芯是否耗尽
                    SysInfo.m_Climb4.SendData(5, 4, 26);
                    /*
                      27	（车体上传打标笔芯是否耗尽）未耗尽
                      28	（车体上传打标笔芯是否耗尽）已耗尽
                     */
                    break;
                case 12://笔芯旋出长度自动识别(关闭)
                    SysInfo.m_Climb4.SendData(5, 4, 32);
                    break;
                case 13://笔芯旋出长度自动识别(启动)
                    SysInfo.m_Climb4.SendData(5, 4, 33);
                    break;
            }
        }
        private void Bt_Alarm_MouseUp(object sender, MouseEventArgs e)
        {
            if (Ck_Mark.Checked) return;
            if (SysInfo.m_Climb4.m_blLink == false) return;
            float _flWait=float .Parse (   SysInfo.csInter.IniReadDefine("Alarm_", "Mark_Time", "0.3", SysInfo.HardFileName));
            try
            {
                if(_flWait>0 && _flWait<5)
                SysInfo.csInter.WaitTime(_flWait);
            }
            catch { }
            // m_iAlarmMark = m_iAlarmMark == 0 ? 1 : 0;
            m_iAlarmMark = 0;
            SysInfo.m_Climb4.SendData(5, 2, 0, 0, m_iAlarmMark.ToString());

            Lb_Alarm.Text = m_iAlarmMark == 1 ? " 打标" : "停止打标";
            Bt_Alarm.BackgroundImage = Img_Lst.Images[m_iAlarmMark == 0 ? 4 : 5];
        }
        
        private void Ck_Qh_Click(object sender, EventArgs e)
        {
           SysInfo . m_bl_Q1_H0 = Ck_Qh.Checked;
            Trac_DG.Value = 5;
            SysInfo.m_Climb4.SendData(4, 1, SysInfo.m_bl_Q1_H0 ?3:4, 0, Trac_DG.Value.ToString());
        }

        private void Bt_Dg_Add_Click(object sender, EventArgs e)
        {
            if (Trac_DG.Value < Trac_DG.Maximum)
                Trac_DG.Value = Trac_DG.Value + 1;
            else if (Trac_DG.Value ==Trac_DG.Maximum)
                Trac_DG.Value = Trac_DG.Maximum;
            SysInfo.m_Climb4.SendData(4, 1, SysInfo.m_bl_Q1_H0 ? 3 : 4, 0, Trac_DG.Value.ToString());
        }

        private void Bt_Dg_De_Click(object sender, EventArgs e)
        {
            if (Trac_DG.Value >Trac_DG.Minimum )
                Trac_DG.Value = Trac_DG.Value - 1;
            else if (Trac_DG.Value == Trac_DG.Minimum)
                Trac_DG.Value =1;

            SysInfo.m_Climb4.SendData(4, 1, SysInfo.m_bl_Q1_H0 ? 3 : 4, 0, Trac_DG.Value.ToString());
        }

        private void Trac_DG_Scroll(object sender, EventArgs e)
        {
            SysInfo.m_Climb4.SendData(4, 1, SysInfo.m_bl_Q1_H0 ? 3 : 4, 0, Trac_DG.Value.ToString());
        }
        /// <summary>
        /// 光栅臂速度加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_Gsb_Add_Click(object sender, EventArgs e)
        {
            if (Track_Gsb .Value < Track_Gsb.Maximum)
            {
                Track_Gsb.Value += 1;
                Set_Speed();
            }
        }
      
        /// <summary>
        /// 光栅臂移动光标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Track_Gsb_Scroll(object sender, EventArgs e)
        {
      //      if (m_blActive == false) return;
            if (SysInfo.m_Climb4.m_blLink == false) return;
            Lb_Gsb .Text = Track_Gsb.Value.ToString() + "%";
            Set_Speed();
        }
        /// <summary>
        /// 光栅臂速度减
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_Gsb_Desc_Click(object sender, EventArgs e)
        {
            if (Track_Gsb.Value > Track_Gsb.Minimum )
            {
                Track_Gsb.Value -= 1;
                Set_Speed();
            }
        }

        private void Pic_4_Sc1_Zx0_Click(object sender, EventArgs e)
        {
            SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 = SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 == 1 ? 0 : 1;

            Pic_4_Sc1_Zx0.BackgroundImage = Img_Lst.Images[SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 == 1 ? 6 : 7];
            Set_ZxParaVis(SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0 == 0 ? false : true);
        }
        /// <summary>
        /// 终点值，直行：隐藏  扫查：显示
        /// </summary>
        /// <param name="blVal"></param>
        private void Set_ZxParaVis(bool blVal)
        {
            Lb_4_Qd.Visible = blVal;
            Num_4_Qd.Visible = blVal;
        }
        private void Pic_4_Qj1_Ht0_Click(object sender, EventArgs e)
        {
            SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Qj1_Ht0 = SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Qj1_Ht0 == 1 ? 0 : 1;
            Pic_4_Qj1_Ht0.BackgroundImage = Img_Lst.Images[SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Qj1_Ht0 == 1 ? 6 : 7];
        }


      
        private void Init()
        {
            SysInfo.csInter.INIWriteValue("GSB", "i_Speed", SysInfo.m_SysBuff.m_Climb.Speed.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue("GSB", "i_Gsb_Speed", SysInfo.m_SysBuff.m_Climb.i_Para_Speed_Gsb.ToString(), SysInfo.HardFileName);

            SysInfo.csInter.INIWriteValue("GSB", "i_Gsb_Start_Pos", SysInfo.m_SysBuff.m_Climb.i_Gsb_Start_Pos.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue("GSB", "i_Gsb_End_Pos", SysInfo.m_SysBuff.m_Climb.i_Gsb_End_Pos.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue("GSB", "iRun_Gsb_Sc1_Zx0", SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Sc1_Zx0.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue("GSB", "iRun_Gsb_Qj1_Ht0", SysInfo.m_SysBuff.m_Climb.iRun_Gsb_Qj1_Ht0.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue("GSB", "iGsb_Len", SysInfo.m_SysBuff.m_Climb.iGsb_Len.ToString(), SysInfo.HardFileName);

            SysInfo.csInter.INIWriteValue("GSB", "i_Coat_Interval", SysInfo.m_SysBuff.m_Climb.i_Coat_Interval.ToString(), SysInfo.HardFileName);
         //   MessageBox.Show(" 保存" + SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval);
            SysInfo.csInter.INIWriteValue("GSB", "i_Gsb_Interval", SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue("Class_Plant", "Scree_iDotWithmm_X", SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval.ToString(), SysInfo.HardFileName);
        }

        private void Num_4_Zd_ValueChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            if (Num_4_Zd.Value < 10) Num_4_Zd.Value = 10; ;
          SysInfo .  Set_L_R(int.Parse(Num_4_Qd.Value.ToString()), int.Parse(Num_4_Zd.Value.ToString()));
        }

        private void Num_4_Qd_ValueChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.Set_L_R(int.Parse(Num_4_Qd.Value.ToString()), int.Parse(Num_4_Zd.Value.ToString()));
        }

        private void Num_BjJl_ValueChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval = (int)Num_BjJl.Value;
        //    MessageBox.Show(" 变更 " + SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval);
            SysInfo.m_Climb4.SendData(2, 4, 0, 0, SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval.ToString ()+","+ 
                     SysInfo.m_SysBuff.m_Climb.i_Coat_Interval.ToString ());//写光栅臂间隔、单步间距

          
        }
        private void Value_Change()
        {
            SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval = (int)Num_BjJl.Value;
            //    MessageBox.Show(" 变更 " + SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval);
            SysInfo.m_Climb4.SendData(2, 4, 0, 0, SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval.ToString() + "," +
                     SysInfo.m_SysBuff.m_Climb.i_Coat_Interval.ToString());//写光栅臂间隔、单步间距


            SysInfo.m_SysBuff.m_Climb.i_Coat_Interval = (int)Num_Coat_Bj.Value;
            SysInfo.m_SysBuff_C.m_Plant_C.Scree_iDotHeightmm_Y_Coat = (int)Num_Coat_Bj.Value;

            SysInfo.m_Climb4.SendData(2, 4, 0, 0, SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval.ToString() + "," +
                                                  SysInfo.m_SysBuff.m_Climb.i_Coat_Interval.ToString());//写光栅臂间隔、单步间距


            SysInfo.Set_L_R(int.Parse(Num_4_Qd.Value.ToString()), int.Parse(Num_4_Zd.Value.ToString()));

        }
        private void Bt_Gsb_Do_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_iLanguage == 0)
            {
                if (Bt_Gsb_Do.Text == "启动")
                {
                    SysInfo.Set_Mode_Start(1);
                    Bt_Gsb_Do.Text = "停止";
                }
                else
                {
                    SysInfo.Set_Mode_Start(0);
                    Bt_Gsb_Do.Text = "启动";
                }
            }
            else
            {
                if (Bt_Gsb_Do.Text == "Begin")
                {
                    SysInfo.Set_Mode_Start(1);
                    Bt_Gsb_Do.Text = "Stop";
                }
                else
                {
                    SysInfo.Set_Mode_Start(0);
                    Bt_Gsb_Do.Text = "Begin";
                }
            }

        }

        private void Ck_Xj_Write_Click(object sender, EventArgs e)
        {
          //  SysInfo.m_SysBuff.m_Tofd_DLL.m_bl_Xj_Write = Ck_Xj_Write.Checked;
        }

        private void Rad_L_Q_Click(object sender, EventArgs e)
        {
            Set_Light_Q_H(true );

        }
        private void Set_Light_Q_H(bool blVal)
        {
            SysInfo.m_bl_Q1_H0 = blVal;
            Trac_DG.Value = 5;
            SysInfo.m_Climb4.SendData(4, 1, SysInfo.m_bl_Q1_H0 ? 3 : 4, 0, Trac_DG.Value.ToString());

        }

        private void Rad_L_H_Click(object sender, EventArgs e)
        {
            Set_Light_Q_H(false );
        }

        private void Bt_Win_Mw_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;// WindowState.Minimized;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SysInfo.m_Climb4.SendData(1, 1, 7, 0, Gsb_Int.Value .ToString ());//倒车

        }
        public void Tool_Tip(NumericUpDown NuD,string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(NuD, strVal);
        }
            

        public  void Tool_Tip(System.Windows.Forms.ComboBox cmbTiTl, string strVal)
        {
            ToolTip tooltip2 = new ToolTip();
            //设置提示框显示时间，默认5000，最大为32767，超过此数，将以默认5000显示           
            tooltip2.AutoPopDelay = 6000;
            //设置要显示提示框的控件 button1按钮
            tooltip2.SetToolTip(cmbTiTl, strVal);
        }
        private void Rad_JP_L_Click(object sender, EventArgs e)
        {
            Set_JP(1);
        }

        private void Rad_JP_S_Click(object sender, EventArgs e)
        {
            Set_JP(3);
        }

        private void Rad_JP_R_Click(object sender, EventArgs e)
        {
            Set_JP(2);
        }

        private void Cmb_Item_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            Tool_Tip(Cmb_Item, Cmb_Item.Text);
            Ctrl_4_DB();
        }

        private void Ck_Mark_CheckedChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_bl_Mark = Ck_Mark.Checked;

            SysInfo.csInter.INIWriteValue ("System", "m_bl_Mark", SysInfo.m_bl_Mark?"1":"0", SysInfo.HardFileName);
        }

        private void Bt_Cxfhzt_Click(object sender, EventArgs e)
        {
            switch (SysInfo.m_SysBuff.m_Climb.flAnle)
            {
                case 24://车体上传打标笔是否安装）未安装
                    this .Text  = "(车体上传打标笔芯是否耗尽）未耗尽";
                    break;
                case 25:
                    this.Text = "(车体上传打标笔是否安装）已安装";
                    break;//
                case 27:
                    this.Text = "(车体上传打标笔芯是否耗尽）未耗尽";
                    break;//
                case 28:
                    this.Text = "(车体上传打标笔芯是否耗尽）已耗尽";
                    break;//
            }
        }

        private void Num_Coat_Bj_ValueChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_SysBuff.m_Climb.i_Coat_Interval = (int)Num_Coat_Bj.Value;
            SysInfo.m_SysBuff_C.m_Plant_C.  Scree_iDotHeightmm_Y_Coat= (int)Num_Coat_Bj.Value;

            SysInfo.m_Climb4.SendData(2, 4, 0, 0, SysInfo.m_SysBuff.m_Climb.i_Gsb_Interval.ToString() + "," +
                                                  SysInfo.m_SysBuff.m_Climb.i_Coat_Interval.ToString());//写光栅臂间隔、单步间距

        }

        private void Num_4_Qd_Click(object sender, EventArgs e)
        {
            SysInfo.SetKeyBorad(this, Num_4_Qd, "num_keyboard", false);
        }

        private void Num_4_Qd_Leave(object sender, EventArgs e)
        {
            SysInfo.SetKeyBorad("num_keyboard");
        }

        private void Num_4_Zd_Click(object sender, EventArgs e)
        {
            SysInfo.SetKeyBorad(this, Num_4_Zd, "num_keyboard", false);
        }

        private void Num_4_Zd_Leave(object sender, EventArgs e)
        {
            SysInfo.SetKeyBorad("num_keyboard");
        }

        private void Num_BjJl_Click(object sender, EventArgs e)
        {
            SysInfo.SetKeyBorad(this, Num_BjJl, "num_keyboard", false);

        }

        private void Num_BjJl_Leave(object sender, EventArgs e)
        {
            SysInfo.SetKeyBorad("num_keyboard");
        }

        private void Num_Coat_Bj_Leave(object sender, EventArgs e)
        {
            SysInfo.SetKeyBorad("num_keyboard");
        }

        private void Num_Coat_Bj_Click(object sender, EventArgs e)
        {
            SysInfo.SetKeyBorad(this, Num_Coat_Bj, "num_keyboard", false);
        }

        private void NumAdd_ValueChanged(object sender, EventArgs e)
        {
            SysInfo.m_SysBuff.m_Climb.m_UI_Gsb_Distan_Min =(int) NumAdd.Value;
            SysInfo.csInter.INIWriteValue ("SYSinfo", "m_UI_Gsb_Distan_Min", SysInfo.m_SysBuff.m_Climb.m_UI_Gsb_Distan_Min.ToString (), SysInfo.HardFileName);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if(SysInfo.m_i_TOFD_0_Cscan_1_Mui_2 == 1)
            NumAdd.Visible  = !NumAdd.Visible;
        }

        private void Time_Speed_Tick(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4 == null) return;
            if (SysInfo.m_Climb4.m_blLink == false) return;

            if (SysInfo.m_Climb4.m_blLink == false) return;
            if (SysInfo.m_SysBuff.m_Climb.Speed != (int)Track_Sd.Value)
                Set_Speed_Show();
        }
        private void Set_Speed_Show()
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            Track_Sd.Value = (int)SysInfo.m_SysBuff.m_Climb.Speed;
            Track_Gsb.Value = (int)SysInfo.m_SysBuff.m_Climb.i_Para_Speed_Gsb;
            SysInfo.SetSpeed();

            Lb_Titl.Text = Track_Sd.Value.ToString();// + "%";
            Lb_Gsb.Text = Track_Gsb.Value.ToString();// + "%";
        }

        private void Bt_Move_Gsb_MouseUp(object sender, MouseEventArgs e)
        {
            Gsb_Stop_3_4();
        }
        private void Gsb_Stop_3_4()
        {
            if (Gsb_Int.Value == 3 || Gsb_Int.Value == 4)
                SysInfo.m_Climb4.SendData(1, 1, 7, 0, "2");//停止
        }

        private void Bt_Move_Gsb_KeyUp(object sender, KeyEventArgs e)
        {
            Gsb_Stop_3_4();
        }

       
        private void Bt_Move_L_KeyUp(object sender, KeyEventArgs e)
        {
            SysInfo.m_Climb4.SendData(1, 1, 7, 0, "2");//停止
        }

        private void Bt_Move_L_MouseUp(object sender, MouseEventArgs e)
        {
            SysInfo.m_Climb4.SendData(1, 1, 7, 0, "2");//停止
        }

        private void Bt_Move_L_KeyDown(object sender, KeyEventArgs e)
        {
            SysInfo.m_Climb4.SendData(1, 1, 7, 0, "4");//左移动
        }

        private void Bt_Move_L_MouseDown(object sender, MouseEventArgs e)
        {
            SysInfo.m_Climb4.SendData(1, 1, 7, 0, "4");//左移动
        }

        private void Bt_Move_R_KeyDown(object sender, KeyEventArgs e)
        {
            SysInfo.m_Climb4.SendData(1, 1, 7, 0, "3");//右移动
        }

        private void Bt_Move_R_MouseDown(object sender, MouseEventArgs e)
        {
            SysInfo.m_Climb4.SendData(1, 1, 7, 0, "3");//右移动
        }

        private void Bt_Move_R_KeyUp(object sender, KeyEventArgs e)
        {
            SysInfo.m_Climb4.SendData(1, 1, 7, 0, "2");//停止
        }

        private void Bt_Move_R_MouseUp(object sender, MouseEventArgs e)
        {
            SysInfo.m_Climb4.SendData(1, 1, 7, 0, "2");//停止
        }

        private void Bt_Move_L_Click(object sender, EventArgs e)
        {

        }

        private void Bt_Move_R_Click(object sender, EventArgs e)
        {

        }

        private void Bt_CalCu_JL_Click(object sender, EventArgs e)
        {
            Bt_CalCu_JL.Enabled = false;
            Frm_Tofd_Calcu _flFrm = new Frm_Tofd_Calcu();
            _flFrm.Show();
            Bt_CalCu_JL.Enabled = true;
        }
    }
}
