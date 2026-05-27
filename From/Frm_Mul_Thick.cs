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
    public partial class Frm_Mul_Thick : Form
    {
        int m_i_Norm_Select = 0;

        //public EM_RIT.ClEM_RIT m_UT_My_DLL;
        public Frm_Mul_Thick()
        {
            InitializeComponent();
        }

        private void Frm_Mul_Thick_Load(object sender, EventArgs e)
        {
            SysInfo.m_blFrmOpen[9] = true;
            Show_Bt_Norm();
        }
        /// <summary>
        /// 显示设置公称厚度
        /// </summary>
        private void Show_Bt_Norm()
        {
            Cmb_Yzdw.Visible = false;
            if (SysInfo.m_SysBuff_C.m_Plant_C.iRad_Dw  == 1)
            {
                Cmb_Yzdw.Visible = true;
                Lb_Dw_1.Text = "in.";
                Lb_Dw_2.Text = "in.";
                Lb_Dw_3.Text = "in.";
            }
            else
            {
                Lb_Dw_1.Text = "mm";
                Lb_Dw_2.Text = "mm";
                Lb_Dw_3.Text = "mm";
            }
            Bt_Norm_11.Text = SysInfo.m_SysBuff_C.m_Plant_C.flNormal_Thickness.ToString();
            Bt_Norm_1.Text = SysInfo.m_SysBuff_C.m_Plant_C.flNormal_Thickness.ToString();

            #region 公称厚度设置
            if (SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_flNormal_Thickness.Count == 0)
                SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_flNormal_Thickness.Add(SysInfo.m_SysBuff_C.m_Plant_C.flNormal_Thickness);
            Control con;
            for (int i = 0; i < SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_flNormal_Thickness.Count; i++)
            {
                con = SysInfo.Get_Control(this, "Bt_Norm_" + (i + 1));
                con.Text = SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_flNormal_Thickness[i].ToString();

                con = SysInfo.Get_Control(this, "Bt_H_" + (i + 1));
                if (i < SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_fl_H.Count)
                    con.Text = SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_fl_H[i].ToString();
                else
                    con.Text = "2.3";
            }
            for (int i = SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_flNormal_Thickness.Count + 1; i < 11; i++)
            {
                con = SysInfo.Get_Control(this, "Bt_Norm_" + (i));
                con.Text = SysInfo.m_SysBuff_C.m_Plant_C.flNormal_Thickness.ToString();
                con.Visible = false;

                con = SysInfo.Get_Control(this, "Bt_H_" + (i));
                con.Text = SysInfo.m_SysBuff_C.m_Plant_C.flNormal_Thickness.ToString();
                con.Visible = false;
            }
            #endregion 设置公称厚度
            Ck_Auto_NormalThick.Checked=  SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.m_bl_Jg_Normal ;
            ck_2bei.Checked=  SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.m_bl_2BeiSjxx  ;

            Txt_Norm_Num.Text = SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_flNormal_Thickness.Count.ToString();
            Grp_Norm.Dock = DockStyle.Fill;
            Grp_Norm.Visible = true;
            this.ControlBox = false;
        }

        private void Bt_Norm_Ok_Click(object sender, EventArgs e)
        {
            #region 添加多个公称厚度 2022-6-6
            SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_flNormal_Thickness.Clear();
            SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_fl_H.Clear();

            if (Bt_Norm_11.Text == "") Bt_Norm_11.Text = SysInfo.m_SysBuff_C.m_Plant_C.flNormal_Thickness.ToString();
            float _fl_NormThick = float.Parse(Bt_Norm_11.Text);

            int _iT = int.Parse(Txt_Norm_Num.Text);
            SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.m_iNowDataNum = _iT;
            SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.m_csInter.INIWriteValue("System_Para", "lst_flNormal_Thickness_Num", SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.m_iNowDataNum.ToString(), SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.HardFileName);
            Control con;
            float _flT = 0;
            string _str_Norm_Select = "", _str_Norm_H = "";
            int _i_Norm_Select = -1;
            for (int i = 1; i <= _iT; i++)
            {
                con = SysInfo.Get_Control(this, "Bt_Norm_" + (i));
                if (con.Text != "")
                {
                    _str_Norm_Select += (i == 1 ? "" : ",") + con.Text;
                    _flT = float.Parse(con.Text);
                    SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_flNormal_Thickness.Add(_flT);
                    SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.m_csInter.INIWriteValue("System_Para", "flNormal_Thickness_No_" + i, _flT.ToString(), SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.HardFileName);
                    if (_i_Norm_Select == -1 &&
                        _flT == _fl_NormThick)
                        _i_Norm_Select = i;//当前选择的厚度列表包含了公称厚度
                }

                con = SysInfo.Get_Control(this, "Bt_H_" + (i));

                if (con.Text != "")
                {
                    _str_Norm_H += (i == 1 ? "" : ",") + con.Text;
                    _flT = float.Parse(con.Text);
                    SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_fl_H.Add(_flT);
                    SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.m_csInter.INIWriteValue("System_Para", "lst_fl_H_" + i, _flT.ToString(), SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.HardFileName);
                }
            }
            if (_i_Norm_Select == -1)
            {
                if (_i_Norm_Select == -1)
                {
                    string _strT = (SysInfo.m_iLanguage == 0 ? "公称厚度列表中(" + _str_Norm_Select + ")" + "\r\n" + "缺少当前指定的公称厚度值，请添加:" :
                             "In the nominal thickness list(" + _str_Norm_Select + ")" + "\r\n the currently specified nominal thickness value is missing. Please add it: ") +
                             _fl_NormThick.ToString();
                    MessageBox.Show(_strT);
                    return;
                }
            }
            #endregion

            SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.m_bl_Jg_Normal = Ck_Auto_NormalThick.Checked;
            SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.m_bl_2BeiSjxx = ck_2bei.Checked;
            SysInfo.m_SysBuff_C.m_Plant_C.flNormal_Thickness = float.Parse(Bt_Norm_11.Text);

          
            SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_csInter.INIWriteValue("System_Para", "m_bl_Jg_Normal", SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.m_bl_Jg_Normal?"1":"0", SysInfo.m_Tofd_C_Scan.m_UT_My_DLL.HardFileName);
            SysInfo.m_blFrmOpen[9] = false ;
            this.ControlBox = true;
            Grp_Norm.Visible = false;
            this.Close();
        }

        private void Bt_Norm_A_Click(object sender, EventArgs e)
        {
            if (Txt_Norm_Num.Text == "") Txt_Norm_Num.Text = "1";
            int _iT = int.Parse(Txt_Norm_Num.Text);

            if (_iT < 10)
            {
                _iT++;
                SetBtNorm_E(_iT);
            }
         
            Ck_Auto_NormalThick.Checked = _iT > 1;
        }
        private void SetBtNorm_E(int _iT)
        {
            Txt_Norm_Num.Text = _iT.ToString();
            Control con;
            Control con_2;

            for (int i = 1; i <= _iT; i++)
            {
                con = SysInfo.Get_Control(this, "Bt_Norm_" + (i));
                con.Text = Bt_Norm_11.Text;
                con.Visible = true;

                con_2 = SysInfo.Get_Control(this, "Bt_H_" + (i));
                con_2.Text = "2.3";
                con_2.Visible = true;

            }
            for (int i = _iT + 1; i < 11; i++)
            {
                con = SysInfo.Get_Control(this, "Bt_Norm_" + (i));
                con.Visible = false;

                con_2 = SysInfo.Get_Control(this, "Bt_H_" + (i));
                con_2.Visible = false;
            }
        }

        private void Bt_Norm_D_Click(object sender, EventArgs e)
        {
            if (Txt_Norm_Num.Text == "") Txt_Norm_Num.Text = "1";
            int _iT = int.Parse(Txt_Norm_Num.Text);
            if (_iT > 1)
            {
                _iT--;
                SetBtNorm_E(_iT);
            }
            Ck_Auto_NormalThick.Checked = _iT > 1;

        }

        private void Bt_NormV_A_Click(object sender, EventArgs e)
        {
            Set_Bt_Norm_V(0);
        }

        private void Bt_NormV_D_Click(object sender, EventArgs e)
        {
            Set_Bt_Norm_V(1);
        }
        private void Set_Bt_Norm_V(int iType)
        {
            if (m_i_Norm_Select > 0 && m_i_Norm_Select < 12)
            {
                Control con;
                con = SysInfo.Get_Control(this, "Bt_Norm_" + m_i_Norm_Select);

                if (con.Text == "") con.Text = SysInfo.m_Tofd_C_Scan. m_UT_My_DLL.lst_flNormal_Thickness.Count.ToString();

                try
                {
                    float _flT = float.Parse(con.Text);
                    if (SysInfo.m_SysBuff_C.m_Plant_C.iRad_Dw == 1)
                    {
                        float _fl_Yz = Get_Yzdw_Zxdw();
                        if (iType == 0)
                            _flT += _fl_Yz;
                        else
                        {
                            if (_flT >= _fl_Yz * 2)
                                _flT -= _fl_Yz;
                        }
                    }
                    else
                    {
                        if (iType == 0)
                            _flT++;
                        else
                        {
                            if (_flT > 2)
                                _flT--;
                        }
                    }
                    string _sT = _flT.ToString();
                    string[] _sPara = _sT.Split('.');
                    int _iN = 0;
                    if (_sPara.Length > 1)
                    {
                        _iN = _sPara[1].Length;
                    }
                    con.Text = _flT.ToString("f" + _iN);
                }
                catch { }
            }
        }

        private float Get_Yzdw_Zxdw()
        {
            float _fl_R = 0;
            if (Cmb_Yzdw.Text.IndexOf("32") > 0)
                _fl_R = 0.03125f;
            else if (Cmb_Yzdw.Text.IndexOf("4") > 0)
                _fl_R = 0.25f;
            else if (Cmb_Yzdw.Text.IndexOf("8") > 0)
                _fl_R = 0.125f;
            else if (Cmb_Yzdw.Text.IndexOf("16") > 0)
                _fl_R = 0.0625f;
            else if (Cmb_Yzdw.Text.IndexOf("2") > 0)
                _fl_R = 0.5f;

            else
                _fl_R = 1;
            return _fl_R;
        }

        private void Bt_H_3_Click(object sender, EventArgs e)
        {
            SysInfo.SetKeyBorad(this.Left - 240, Bt_H_1.Top + this.Top + 50, "num_keyboard");
        }

        private void Bt_H_10_Leave(object sender, EventArgs e)
        {
            SysInfo.SetKeyBorad("num_keyboard");
        }

        private void Bt_Norm_11_Click(object sender, EventArgs e)
        {
            m_i_Norm_Select = 11;
            SysInfo.SetKeyBorad(this.Left - 240, Bt_H_1.Top + this.Top + 50, "num_keyboard");
        }

        private void Bt_Norm_1_Click(object sender, EventArgs e)
        {
            
            m_i_Norm_Select = 1;
        }

        private void Bt_Norm_2_Click(object sender, EventArgs e)
        {
            m_i_Norm_Select = 2;
        }

        private void Bt_Norm_3_Click(object sender, EventArgs e)
        {
            m_i_Norm_Select = 3;
        }

        private void Bt_Norm_4_Click(object sender, EventArgs e)
        {
            m_i_Norm_Select = 4;
        }

        private void Bt_Norm_5_Click(object sender, EventArgs e)
        {
            m_i_Norm_Select = 5;
        }

        private void Bt_Norm_6_Click(object sender, EventArgs e)
        {
            m_i_Norm_Select = 6;
        }

        private void Bt_Norm_7_Click(object sender, EventArgs e)
        {
            m_i_Norm_Select = 7;
        }

        private void Bt_Norm_8_Click(object sender, EventArgs e)
        {
            m_i_Norm_Select = 8;
        }

        private void Bt_Norm_9_Click(object sender, EventArgs e)
        {
            m_i_Norm_Select = 9;
        }

        private void Bt_Norm_10_Click(object sender, EventArgs e)
        {
            m_i_Norm_Select = 10;
        }
    }
}
