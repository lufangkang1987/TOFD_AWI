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
namespace Tofd_AWI.From
{
    public partial class Frm_Tofd_Calcu : Form
    {
        #region 变量
        bool m_blActive = false;
        /// <summary>
        /// 探头中心间距
        /// </summary>
        float m_fPcsLen;      
        /// <summary>
        /// 弧长
        /// </summary>
        float m_fPcsArc;      
        /// <summary>
        /// 弦高
        /// </summary>
        float m_fPcsChord;    
        /// <summary>
        /// 外壁的外径或内壁的内径
        /// </summary>
        float m_fPcsDia;      
        /// <summary>
        /// 楔块角度
        /// </summary>
        float m_fPcsAngle;    
        /// <summary>
        /// 分层起点
        /// </summary>
        float m_fPcsStart;    
        /// <summary>
        /// 分层终点
        /// </summary>
        float m_fPcsEnd;      
        #endregion 变量
        public Frm_Tofd_Calcu()
        {
            InitializeComponent();
        }

        private void Frm_Tofd_Calcu_Load(object sender, EventArgs e)
        {
            //参数初始化
            IniIt();
            m_blActive = true;
        }
        private void IniIt(int iType=0)
        {
            int _iCurrChan = SysInfo.m_Tofd_DLL.m_icurChan;
            if (iType == 0)
            {
                SysInfo.m_blFrm_TOFD_Open[0] = true;
                #region  读数据

                Cmb_Bmq_Fx.SelectedIndex = SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iEnPos == 1 ? 1 : 0;
                Cmb_PcsType.SelectedIndex = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsType;
                Cmb_PcsMode.SelectedIndex = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsMode;
                WnNj(SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsMode != 0);

                Cmb_Mody.SelectedIndex = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_BScanMode;
                Cmb_m_iCurEn.SelectedIndex = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn;
                // SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iEnPos;

                Lb_V_Pcs.Text = "----.--";// SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsLen.ToString ("f2");
                Lb_V_Hu.Text = "----.--";// SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsArc.ToString ("f2");
                Lb_V_Xian.Text = "----.--";// SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsChord.ToString ("f2");

                Txt_m_fPcsDia.Text = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsDia.ToString();
                Txt_m_fPcsAngle.Text = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsAngle.ToString();
                Txt_m_fPcsStart.Text = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsStart.ToString();
                Txt_m_fPcsEnd.Text = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsEnd.ToString();

                SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X = SysInfo.m_Plant.Scree_iDotWithmm_X;
                Txt_Scree_iDotWithmm_X.Text = (SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X * 1000).ToString();// SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fEnStep.ToString ();
                if (SysInfo.m_i_Cl0_Cx1 == 1 || SysInfo.m_iRun == 1)
                    Txt_Scree_iDotWithmm_X.Enabled = false;

                Txt_lineEdit_EnPrec.Text = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].
                        m_fEnRatio[SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn].ToString();
                #endregion
            }
            else
            {
                 SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iEnPos =(byte ) Cmb_Bmq_Fx.SelectedIndex;
                SysInfo.m_blFrm_TOFD_Open[0] = false;

                byte _btMode = (byte)Cmb_Mody.SelectedIndex;
                if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode != _btMode)
                {
                    if (_btMode == 1)
                    {
                        if (SysInfo.Thread_Get_TofdData != null) SysInfo.Thread_Get_TofdData.Abort();
                        SysInfo.WaitTime(0.01f);
                        SysInfo.m_Tofd_DLL.Link();
                    }
                    SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode = _btMode;
                }

                #region  写数据
                SysInfo.m_Test_Parts.ProbeSpacing = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsLen;
                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_BScanMode", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_BScanMode.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_iCurEn", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn.ToString(), SysInfo.HardFileName);

                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_iPcsType", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsType.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_iPcsMode", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsMode.ToString(), SysInfo.HardFileName);

                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsLen", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsLen.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsArc", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsArc.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsChord", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsChord.ToString(), SysInfo.HardFileName);

                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsDia", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsDia.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsAngle", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsAngle.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsStart", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsStart.ToString(), SysInfo.HardFileName);
                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsEnd", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsEnd.ToString(), SysInfo.HardFileName);

                //SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fEnStep", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fEnStep.ToString(), SysInfo.HardFileName);
                if (SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn == 0)
                {
                    SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fEnRatio_0", Txt_lineEdit_EnPrec.Text, SysInfo.HardFileName);
                    SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fEnRatio[0] = float.Parse(Txt_lineEdit_EnPrec.Text);
                }
                else
                {
                    SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fEnRatio_1", Txt_lineEdit_EnPrec.Text, SysInfo.HardFileName);
                    SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fEnRatio[1] = float.Parse(Txt_lineEdit_EnPrec.Text);
                }
                
                float _flData = float.Parse(Txt_Scree_iDotWithmm_X.Text) / 1000;
                if (_flData != SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X)
                {
                    SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X = _flData;
                    SysInfo.m_Plant.Scree_iDotWithmm_X = SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X;
                    SysInfo.csInter.INIWriteValue("Cls_Plant", "Scree_iDotWithmm_X", (SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X * 1000).ToString(), SysInfo.HardFileName);

                    SysInfo.g_Msg_InterFace.Fun_Brush_D(0);
                }
                #endregion 
            }
        }
        /// <summary>
        /// 是否显示内外径参数 true: 有弧度就显示  false: 平板不显示
        /// </summary>
        /// <param name="blVal"></param>
        private void WnNj(bool blVal)
        {
            label12.Visible = blVal;
            Txt_m_fPcsDia.Visible = blVal;
            label19.Visible = blVal;

            Pic_Cal_Mody.BackgroundImage = Img_Lst.Images[Cmb_PcsMode.SelectedIndex];
        }

        private void Bt_Clear_Click(object sender, EventArgs e)
        {
            SysInfo.m_Tofd_DLL.Init_Encoder();
        }

        private void Cmb_Mody_Click(object sender, EventArgs e)
        {
        }

        private void Txt_m_fEnStep_TextChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            try
            {
                //float _flData = float.Parse(Txt_Scree_iDotWithmm_X.Text) / 1000;
                //int _iData = (int)(_flData);
                //SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X = _iData;
                //SysInfo.m_Plant.Scree_iDotWithmm_X = SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X;
            }
            catch { }

        }

        private void Bt_Jz_Click(object sender, EventArgs e)
        {
            int _iNo = SysInfo.m_Tofd_DLL.m_icurChan;
            int _iA_B = SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iCurEn;
            
            //2 计算精度
            int _iPul = SysInfo.m_Tofd_DLL.m_pSparam_Real[_iNo].m_iEnPul[_iA_B] < 0 ? 0 : SysInfo.m_Tofd_DLL.m_pSparam_Real[_iNo].m_iEnPul[_iA_B];
            float _flDistan = 0;
            try
            {
                _flDistan = float.Parse(Txt_RealDist.Text);
                if (_iPul > 0)
                {
                    SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fEnRatio[_iA_B] = _flDistan / _iPul;
                    Txt_lineEdit_EnPrec.Text = SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fEnRatio[_iA_B].ToString();
                    MessageBox.Show("校准值：" + Txt_lineEdit_EnPrec.Text);
                }
                else
                    MessageBox.Show("脉冲数必须大于0!");
            }
            catch { }
        }

        private void Bt_PcsCal_Click(object sender, EventArgs e)
        {
            int _iCurrChan = SysInfo.m_Tofd_DLL.m_icurChan;
            #region 参数设置
            try
            {
                m_fPcsDia = float.Parse(Txt_m_fPcsDia.Text);      //外壁的外径或内壁的内径
                m_fPcsAngle = float.Parse(Txt_m_fPcsAngle.Text);    //楔块角度
                m_fPcsStart = float.Parse(Txt_m_fPcsStart.Text);    //分层起点
                m_fPcsEnd = float.Parse(Txt_m_fPcsEnd.Text);      //分层终点
            }
            catch { }

            //Lb_V_Pcs.Text = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsLen.ToString();
            //Lb_V_Hu.Text = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsArc.ToString();
            //Lb_V_Xian.Text = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsChord.ToString();

            SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsDia = m_fPcsDia;
            SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsAngle = m_fPcsAngle;
            SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsStart = m_fPcsStart;
            SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsEnd = m_fPcsEnd;

            //   Txt_Scree_iDotWithmm_X.Text = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fEnStep.ToString();
            //Txt_lineEdit_EnPrec.Text = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].
            //        m_fEnRatio[SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn].ToString();
            #endregion

            int iPcsMode = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsMode;

            if (SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsType == 0)
            {
                switch (iPcsMode)
                {
                    case 0:
                        PcsFlatCal();
                        break;
                    case 1:
                        PcsEktCal_1();
                        break;
                    case 2:
                        PcsWallCal_1();
                        break;
                }
            }
            else
            {
                switch (iPcsMode)
                {
                    case 0:
                        m_fPcsAngle = 55;
                        PcsFlatCal();
                        break;
                    case 1:
                        PcsEktCal_2();
                        break;
                    case 2:
                        PcsWallCal_2();
                        break;
                }
            }
        }
        #region 计算函数
        void  PcsFlatCal()  // 平板
        {
            double fA = m_fPcsAngle * Math .PI  / 180.0;
            double fS = m_fPcsStart;
            double fE = m_fPcsEnd;
            double fD = fE - fS;

            double fPcs = 2.0 * (Math .Tan  (fA) *   2.0 * fD / 3.0);
            m_fPcsLen =(float ) fPcs;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsLen = m_fPcsLen;
           
            Lb_V_Pcs.Text = m_fPcsLen.ToString("f2");
            Lb_V_Hu.Text = "----.--";
            Lb_V_Xian.Text = "----.--";
        }
     
        void  PcsEktCal_1()   // 圆弧外壁
        {
            double fS = m_fPcsStart;
            double fE = m_fPcsEnd;
            double fD = fE - fS;
            double fR = m_fPcsDia / 2.0f;

            double fsita = m_fPcsAngle * Math .PI  / 180.0f;
            double fbeta = Math.Asin (fR * Math.Sin (fsita) / (fR - fS - 2.0 * fD / 3.0));
            double fgama = Math.PI - fbeta;
            if (fgama < Math.PI / 2.0)
            {
                fgama = Math.PI - fgama;
            }

            double falfa = Math.PI - (fgama + fsita);
            double fPCS = 2.0 * fR * Math.Sin (falfa);        // pcs
            double fARC = 2.0 * fR * falfa;             // 弧长
            double fARH = fR * (1.0 - Math.Cos (falfa));      // 弦高

            double fsbt = Math.Asin((fR - fS - 2.0 * fD / 3.0) / fR);

            if (fsita > fsbt)
            {
                MessageBox .Show ("Those Parameters,failed to calculate!");
          
                return;
            }

            m_fPcsLen =(float ) fPCS;
            m_fPcsArc = (float)fARC;
            m_fPcsChord = (float)fARH;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsLen = m_fPcsLen;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsArc = m_fPcsArc;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsChord = m_fPcsChord;
            Lb_V_Pcs.Text = m_fPcsLen.ToString("f2");
            Lb_V_Hu.Text = m_fPcsArc.ToString("f2");
            Lb_V_Xian.Text = m_fPcsChord.ToString("f2");
        }

        void  PcsEktCal_2()   // 圆弧外壁
        {
            double fS = m_fPcsStart;
            double fE = m_fPcsEnd;
            double fD = fE - fS;
            double fR = m_fPcsDia / 2.0f;

            double fgama = Math.PI - 55.0 * Math.PI / 180.0;
            double fsita = Math.Asin ((fR - fS - 2.0 * fD / 3.0) * Math.Sin (fgama) / fR);

            double falfa = Math.PI - (fgama + fsita);
            double fPCS = 2.0 * fR * Math.Sin (falfa);        // pcs
            double fARC = 2.0 * fR * falfa;             // 弧长
            double fARH = fR * (1.0 - Math.Cos (falfa));      // 弦高

            double fsbt = Math.Asin ((fR - fS - 2.0 * fD / 3.0) / fR);

            if (fsita > fsbt)
            {
               MessageBox .Show ("Those Parameters,failed to calculate!");
              
                return;
            }

            m_fPcsAngle =(float )( fsita * 180.0 / Math.PI);
            m_fPcsLen = (float)fPCS;
            m_fPcsArc = (float)fARC;
            m_fPcsChord = (float)fARH;

            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsAngle = m_fPcsAngle;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsLen = m_fPcsLen;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsArc = m_fPcsArc;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsChord = m_fPcsChord;

            Txt_m_fPcsAngle.Text = m_fPcsAngle.ToString("f2");
            Lb_V_Pcs.Text = m_fPcsLen.ToString("f2");
            Lb_V_Hu.Text = m_fPcsArc.ToString("f2");
            Lb_V_Xian.Text = m_fPcsChord.ToString("f2");
        }

        void  PcsWallCal_1()  // 圆弧内壁
        {
            double fS = m_fPcsStart;
            double fE = m_fPcsEnd;
            double fD = fE - fS;
            double fR = m_fPcsDia / 2.0f;

            double fsita = m_fPcsAngle * Math.PI / 180.0f;
            double fgama = Math.PI - fsita;

            if (fgama < Math.PI / 2.0)
            {
                fgama = Math.PI - fgama;
            }

            double fbeta = Math.Asin(fR * Math.Sin (fsita) / (fR + fS + 2.0 * fD / 3.0));
            double falfa = Math.PI - (fgama + fbeta);
            double fPCS = 2.0 * fR * Math.Sin(falfa);            // pcs
            double fARC = 2.0 * fR * falfa;             // 弧长
            double fARH = fR * (1.0 - Math.Cos (falfa));         // 弦高

            m_fPcsLen = (float)fPCS;
            m_fPcsArc = (float)fARC;
            m_fPcsChord = (float)fARH;

            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsLen = m_fPcsLen;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsArc = m_fPcsArc;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsChord = m_fPcsChord;

            Lb_V_Pcs.Text = m_fPcsLen.ToString("f2");
            Lb_V_Hu.Text = m_fPcsArc.ToString("f2");
            Lb_V_Xian.Text = m_fPcsChord.ToString("f2");

        }

        void  PcsWallCal_2()  // 圆弧内壁
        {
            double fS = m_fPcsStart;
            double fE = m_fPcsEnd;
            double fD = fE - fS;
            double fR = m_fPcsDia / 2.0f;

            double fbeta = 55 * Math.PI / 180;
            double fgama = Math.Asin ((fR + fS + 2.0 * fD / 3.0) * Math.Sin (fbeta) / fR);
            if (fgama < Math.PI / 2)
            {
                fgama = Math.PI - fgama;
            }

            double falfa = Math.PI - (fgama + fbeta);
            double fsita = Math.PI - fgama;

            double fPCS = 2.0 * fR * Math .Sin(falfa);            // pcs
            double fARC = 2.0 * fR * falfa;             // 弧长
            double fARH = fR * (1.0 - Math.Cos (falfa));         // 弦高

            m_fPcsAngle =(float )( fsita * 180.0 / Math.PI);
            m_fPcsLen = (float)fPCS;
            m_fPcsArc = (float)fARC;
            m_fPcsChord = (float)fARH;
           
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsAngle = m_fPcsAngle;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsLen = m_fPcsLen;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsArc = m_fPcsArc;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsChord = m_fPcsChord;

            Txt_m_fPcsAngle.Text = m_fPcsAngle.ToString("f2");
            Lb_V_Pcs.Text = m_fPcsLen.ToString("f2");
            Lb_V_Hu.Text = m_fPcsArc.ToString("f2");
            Lb_V_Xian.Text = m_fPcsChord.ToString("f2");
        }
        #endregion 计算函数

        private void Bt_PcsApl_Click(object sender, EventArgs e)
        {
            float fpcs = SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsLen;
            double fD = SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsEnd -
                       SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsStart;

            float ftmp = (float)(3.5 * Math.Sqrt((fpcs / 2) * (fpcs / 2) + fD * fD));     //m_fPcsTotalRange
            float fr0 = ftmp / 2.0f;
            float fr2 = SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_fPcsLen / 2.0f;

            float _flRange = (10.0f / 8.0f) * (fr0 - fr2);
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iRange = (int)_flRange;
            float fParallel = (9.5f * fr2 - 1.5f * fr0) / 8.0f;
            float SAMPLE_T = 1f / 200f;
            float fRangePrec = SAMPLE_T * SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_dSpeed / 2000.0f;  // 每一采样点的距离(全展开)
            float _flPT = 100.0f * SAMPLE_T * fParallel / fRangePrec;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iParallelTime = (int)_flPT;
            SysInfo.m_Tofd_DLL.SendCmdFreqRatio();
            SysInfo.m_Tofd_DLL.SendCmdParallel();
        }

        private void Cmb_PcsMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iPcsMode = (byte)Cmb_PcsMode.SelectedIndex;
            WnNj(SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iPcsMode != 0);
        }

        private void Frm_Tofd_Calcu_FormClosed(object sender, FormClosedEventArgs e)
        {
            IniIt(1);
        }

        private void Cmb_m_iCurEn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iCurEn = (byte)Cmb_m_iCurEn.SelectedIndex;
        }

        private void Cmb_Mody_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blActive == false) return;
         //   SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode = (byte)Cmb_Mody.SelectedIndex;
        }

        private void Cmb_Bmq_Fx_Click(object sender, EventArgs e)
        {
           
        }

        private void Cmb_Bmq_Fx_SelectedIndexChanged(object sender, EventArgs e)
        {
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iEnPos = (byte)(Cmb_Bmq_Fx.SelectedIndex == 1 ? 1 : 0);
        }
    }
}
