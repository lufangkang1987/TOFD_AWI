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
        public Frm_Move()
        {
            InitializeComponent();
        }

        private void Frm_Move_Load(object sender, EventArgs e)
        {
            #region 联机参数
            string[] _arrPort_Names = System.IO.Ports.SerialPort.GetPortNames();
            Cmb_Climb.Items.Clear();
            for (int i = 0; i < _arrPort_Names.Length; i++)
                Cmb_Climb.Items.Add(_arrPort_Names[i]);

            Lb_LinkState.Text = "联机:" + (SysInfo.m_Climb4.m_blLink ? "成功" : "失败");
            Lb_LinkState.ForeColor = SysInfo.m_Climb4.m_blLink ? Color.White : Color.Red;
            Ck_Com_Can.Checked = SysInfo.m_Climb4.m_blCom1_Can0 == 1;
            // SysInfo.m_Climb4.m_blCom1_Can0 = int.Parse(SysInfo.csInter.IniReadDefine("COM_Can", "m_blCom1_Can0", "1", SysInfo.HardFileName));
            //搜索电脑COM
            Cmb_Climb.Text = SysInfo.csInter.IniReadDefine("COM_Can", "COM", "COM3", SysInfo.HardFileName);
            Cmb_Climb.Visible = Ck_Com_Can.Checked;
            #endregion 
            SysInfo.m_blFrmOpen[3] = true;
            Lb_Titl.Text = SysInfo.m_Climb.Speed.ToString() + "%";
            Track_Sd.Value = (int)SysInfo.m_Climb.Speed;
            cK_Bmq_T0_41.Checked= SysInfo.m_Climb.iBmq_Type ==1  ?true  : false ;
            string _strP = Application.StartupPath + "\\ICO";
            try
            {
                //1 左右
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\1-1.png"));
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\1-2.png"));
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\2-1.png"));
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\2-2.png"));
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\3-1.png"));
                Img_Lst.Images.Add(Image.FromFile(_strP + "\\3-2.png"));
            }
            catch { }

            m_iGsbTqLx = SysInfo.m_Climb.iGsbTtLx;
            m_iAlarmMark = SysInfo.m_Climb.iAlarmMark;
            GsbTqLx();
            MarkTqLx();

            //Bt_Up.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            //Bt_Down.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            //Bt_Ty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
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
            SysInfo.m_blFrmOpen[3] = false;

        }

    
        private void Bt_Down_Click(object sender, EventArgs e)
        {
            try
            {
                if (SysInfo.m_Climb4.m_blLink == false) return;
                //01：前进    02：后退      03：停止 04：前进左转 
                //05：前进右转 06：后退左转 07：后退右转
              //  SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
                if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                    SysInfo.m_Climb4.SendData(1, 1, 6, 0, "6");
            }
            catch { }
        }

        private void Bt_Stop_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
        }

        private void Bt_Add_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            if (Track_Sd.Value < Track_Sd.Maximum)
                Track_Sd.Value += 1;

            SetSpeed();
        }

        private void Bt_Desc_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            if (Track_Sd.Value > Track_Sd.Minimum)
                Track_Sd.Value -= 1;
            SetSpeed();
        }

        private void Track_Sd_Scroll(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            Lb_Titl.Text = Track_Sd.Value.ToString() + "%";
            SetSpeed();
        }
        private void SetSpeed()
        {
            Lb_Titl.Text = Track_Sd.Value.ToString() + "%";
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                SysInfo.m_Climb4.SendData(2, 2,//Ck_Auto1_ByHand.Checked ? 3 : 2,//3自动 2手动
                                 0, 0, (Track_Sd.Value.ToString() + ",0"));
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
            SysInfo.m_Climb4.m_blCom1_Can0 = Ck_Com_Can.Checked ? 1 : 0;
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 0) return;
           
            SysInfo.m_Climb4.CloseSet();

            SysInfo.csInter.INIWriteValue("COM_Can", "m_blCom1_Can0", SysInfo.m_Climb4.m_blCom1_Can0.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue("COM_Can", "COM", Cmb_Climb.Text, SysInfo.HardFileName);

            if (SysInfo.m_Climb4.m_blCom1_Can0 == 0)
                SysInfo.m_Climb4.InitCan();
            else
            {
                SysInfo.m_Climb4.InitCom(Cmb_Climb.Text);

                DateTime dtStar = DateTime.Now;
                while (true)
                {
                    try
                    {
                        Application.DoEvents();
                        if (DateTime.Now.Subtract(dtStar).TotalSeconds > 1) break;
                        System.Threading.Thread.Sleep(10);
                    }
                    catch { break; }
                }
            }
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
            Lb_LinkState.Text = "联机:" + (SysInfo.m_Climb4.m_blLink ? "成功" : "失败");
            Lb_LinkState.ForeColor = SysInfo.m_Climb4.m_blLink ? Color.White : Color.Red;
            MessageBox.Show("车体联机：" + (SysInfo.m_Climb4.m_blLink ? "成功" : "失败") + "\r\n\r\n" + SysInfo.m_strLinkMsg);

            if (SysInfo.m_Climb4.m_blLink)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "3");
        }

        private void Bt_Alarm_Click(object sender, EventArgs e)
        {
            MarkTqLx();
        }

        private void Bt_H_Click(object sender, EventArgs e)
        {
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
          //  SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "2");
        }

        private void Bt_H_R_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
         //   SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iEnPos = 0;
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "7");

        }

        private void Bt_Q_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
         //   SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iEnPos = 1;
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "1");
        }

        private void Bt_U_L_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
          //  SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iEnPos = 1;
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "4");
        }

        private void Bt_Q_R_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb4.m_blLink == false) return;
            //01：前进    02：后退      03：停止 04：前进左转 
            //05：前进右转 06：后退左转 07：后退右转
          //  SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iEnPos = 1;
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 1)
                SysInfo.m_Climb4.SendData(1, 1, 6, 0, "5");
        }

        private void cK_Bmq_T0_41_Click(object sender, EventArgs e)
        {
            SysInfo.m_Climb.iBmq_Type = cK_Bmq_T0_41.Checked ? 1 : 0;
        }


        private void Bt_Jl_Clear_Click(object sender, EventArgs e)
        {
            if (SysInfo.m_Climb.iBmq_Type == 1)
                SysInfo.m_Climb4.SendData(2, 4, 0, 0, "0");
            else
                SysInfo.m_Tofd_DLL.Init_Encoder();
        }
    }
}
