/*
 * Copyright(C) 2 2017 河南德朗智能科技有限公司
 * 文件名: Frm_Bz.cs
 * 文件功能描述: 当前视频界面伤点标注参数确定
 * 目的：伤点的新增、修改、删除操作
 * 创建标识: 陈大伟 2021-11-02
 * 修改标识: 
 * 修改描述:
 

 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ClassLib_DataMang.DataBaseMang.OleDal;//数据库操作类
using Tofd_AWI.Class;
using ClassLib_TestData;//数据类

namespace Tofd_AWI.From
{
    public partial class Frm_Bz : Form
    {
        public Frm_Bz()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 标注列表初始化
        /// </summary>
        private void InitGrd()
        {
            Rd_D_Len.Checked = true;

            int _iNo = 1;
            string _strT = "";
            Cmb_Type.Items.Clear();
            int _iNum = int.Parse(SysInfo.csInter.IniReadDefine("BiaoZhu", "TypeNum", "0", SysInfo.HardFileName));
            if (_iNum == 0)
            {
                Cmb_Type.Items.Add(SysInfo.csInter.IniReadDefine("BiaoZhu", (_iNo++).ToString(), "直通波断开，有下端点", SysInfo.HardFileName));
                Cmb_Type.Items.Add(SysInfo.csInter.IniReadDefine("BiaoZhu", (_iNo++).ToString(), "直通波滞后", SysInfo.HardFileName));
                Cmb_Type.Items.Add(SysInfo.csInter.IniReadDefine("BiaoZhu", (_iNo++).ToString(), "有上端点, 底波断开", SysInfo.HardFileName));
                Cmb_Type.Items.Add(SysInfo.csInter.IniReadDefine("BiaoZhu", (_iNo++).ToString(), "有上端点, 底波滞后", SysInfo.HardFileName));
                Cmb_Type.Items.Add(SysInfo.csInter.IniReadDefine("BiaoZhu", (_iNo++).ToString(), "有上端点和下端点", SysInfo.HardFileName));
                Cmb_Type.Items.Add(SysInfo.csInter.IniReadDefine("BiaoZhu", (_iNo++).ToString(), "直通波滞后", SysInfo.HardFileName));
                Cmb_Type.Items.Add(SysInfo.csInter.IniReadDefine("BiaoZhu", (_iNo).ToString(), "单点", SysInfo.HardFileName));
                SysInfo.csInter.INIWriteValue("BiaoZhu", "TypeNum", _iNo.ToString(), SysInfo.HardFileName);
            }
            else
            {
                for (int i = 0; i < _iNum; i++)
                {
                    _strT = SysInfo.csInter.IniReadDefine("BiaoZhu", (_iNo++).ToString(), "", SysInfo.HardFileName);
                    if (_strT != "")
                        Cmb_Type.Items.Add(_strT);
                }
            }
            DublGrid.SetDoubleBuffered(Dg_TOFD_Bz, true);
            SysInfo.Grd_IniGrid_ShowZdName(Dg_TOFD_Bz, 2);
        }
        private void Frm_Bz_Load(object sender, EventArgs e)
        {
            //1 列表初始化
            InitGrd();
             //2 填充列表数据
            BrushGrd();
            //3 界面标题：
            this.Text = "标注界面   (当前数据是第 " + (SysInfo.m_Plant.g_iCurrUse_ScreenNo + 1).ToString() + " 屏的检测数据。   焊缝编号为:" + SysInfo.m_Test_Parts.Part_No +")";
            SysInfo.g_Msg_InterFace.Inter_Brush_BiaoZhu -= new ClassLib_TestData.MsgInterFace.OnBrush_BiaoZhu (BrushBiaoZhu);
            SysInfo.g_Msg_InterFace.Inter_Brush_BiaoZhu += new ClassLib_TestData.MsgInterFace.OnBrush_BiaoZhu(BrushBiaoZhu);

            SysInfo.m_blFrmOpen[4] = true;
        }
        private void BrushBiaoZhu()
        {
            float _T = 0;
            switch (SysInfo.m_BiaoZhu_No.iDataType)
            {
                case 0:
                    Txt_flLen.Text = "";
                    switch (SysInfo.m_BiaoZhu_No.i_S0_E1)
                    {
                        case 0:
                            Txt_flLen_S.Text = SysInfo.m_BiaoZhu.flLen_S.ToString();
                            break;
                        case 1:
                            Txt_flLen_E.Text = SysInfo.m_BiaoZhu.flLen_E.ToString();
                            break;
                    } 
                     _T = SysInfo.m_BiaoZhu.flLen_E - SysInfo.m_BiaoZhu.flLen_S;
                    if (_T > 0)
                    {
                        Txt_flLen.Text = _T.ToString("f2");
                        SysInfo.m_BiaoZhu.flLen = float.Parse(Txt_flLen.Text);
                    }
                    break;
                case 1:
                    switch (SysInfo.m_BiaoZhu_No.i_S0_E1)
                    {
                        case 0:
                            Txt_flHeight_S.Text = SysInfo.m_BiaoZhu.flHeight_S.ToString ();
                            break;
                        case 1:
                            Txt_flHeight_E.Text = SysInfo.m_BiaoZhu.flHeight_E.ToString();
                            break;
                    }
                    _T = GetDistanc(SysInfo.m_BiaoZhu.flHeight_E) - GetDistanc(SysInfo.m_BiaoZhu.flHeight_S);
                    if (_T > 0)
                    {
                        Txt_flHeight.Text = _T.ToString("f2");
                        SysInfo.m_BiaoZhu.flHeight = float.Parse(Txt_flHeight.Text);
                    }
                    break;
                case 2:
                    switch (SysInfo.m_BiaoZhu_No.i_S0_E1)
                    {
                        case 0:
                            Txt_flDepth_S.Text = SysInfo.m_BiaoZhu.flDepth_S.ToString();
                            SysInfo.m_BiaoZhu.flDepth =float .Parse ( GetDistanc(SysInfo.m_BiaoZhu.flHeight_E).ToString ("f2"));
                            Txt_flDepth.Text = SysInfo.m_BiaoZhu.flDepth.ToString();
                            break;
                    }
                    break;
            }
        }

        private float GetDistanc(float flTime)
        {
            int _iN0 = SysInfo.m_Tofd_DLL.m_icurChan;
            //    float _Time = Tofd.m_pTimeBuf[SysInfo.m_Plant.m_i_X_No] * 0.01f;
            float _f1 = SysInfo.m_Tofd_DLL.m_pSparam[_iN0].m_dSpeed / 1000f * 0.5f *
                (flTime - SysInfo.m_Tofd_DLL.m_pSparam_Real[_iN0].T0);
            _f1 *= _f1;
            float _f2 = SysInfo.m_Tofd_DLL.m_pSparam[_iN0].m_fPcsLen;
            _f2 *= _f2;
            double _d = Math.Sqrt((double)(_f1 - _f2));
            if (_f1 < _f2) _d = 0;
            return (float)_d;
        }
        /// <summary>
        /// 显示当前屏幕的缺陷数据
        /// </summary>
        private void BrushGrd()
        {
            List<Class_Test_AlarmArea> _lstAlarm = DbGlobal.ImAlarm.GetData(SysInfo.m_Test_Parts.ID,
                                                                            SysInfo.m_Test_Parts.Sub_ID,
                                                                            SysInfo.m_Plant.g_iCurrUse_ScreenNo,
                                                                            SysInfo .m_Plant .lst_Screenkd);

            if (_lstAlarm.Count > 0)
                Dg_TOFD_Bz.RowCount = _lstAlarm.Count + 1;
            for (int i = 0; i < _lstAlarm.Count; i++)
                if (SysInfo.m_Plant.g_iCurrDistanc_Calcu_ScreenNo == SysInfo.m_Plant.Get_No(_lstAlarm[i].flLen_S))
                    SysInfo.Grd_AddData(Dg_TOFD_Bz, i, _lstAlarm[i]); 
        }
        private void Frm_Bz_FormClosing(object sender, FormClosingEventArgs e)
        {
            SysInfo.g_Msg_InterFace.Inter_Brush_BiaoZhu -= new ClassLib_TestData.MsgInterFace.OnBrush_BiaoZhu(BrushBiaoZhu);
            SysInfo.m_blFrmOpen[4] = false;
        }
        /// <summary>
        /// 数据保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_Save_Click(object sender, EventArgs e)
        {
            Class_Test_AlarmArea _Alarm = new Class_Test_AlarmArea();
            try
            {
                #region 数据检查
                _Alarm.flLen_S = float.Parse(Txt_flLen_S.Text);
                _Alarm.flLen_E = float.Parse(Txt_flLen_E.Text);
                _Alarm.flLen = float.Parse(Txt_flLen.Text);
                _Alarm.iY_No = SysInfo.m_BiaoZhu.iY_No;
                _Alarm.iX_No = SysInfo.m_BiaoZhu.iX_No;
                if (_Alarm.flLen <= 0) { MessageBox.Show("长度应>0"); return; }

                _Alarm.flHeight_S = float.Parse(Txt_flHeight_S.Text);
                _Alarm.flHeight_E = float.Parse(Txt_flHeight_E.Text);
                _Alarm.flHeight = float.Parse(Txt_flHeight.Text);
                if (_Alarm.flHeight <= 0) { MessageBox.Show("高度应>0"); return; }

                _Alarm.flDepth_S = float.Parse(Txt_flDepth_S.Text);
                _Alarm.flDepth = float.Parse(Txt_flDepth.Text);
                if (_Alarm.flDepth < 0) { MessageBox.Show("深度应>=0"); return; }

                #endregion
                #region 添加数据
                _Alarm.ID = SysInfo.m_Test_Parts.ID;
                _Alarm.Sub_ID = SysInfo.m_Test_Parts.Sub_ID;
                _Alarm.Part_No = SysInfo.m_Test_Parts.Part_No;

                _Alarm.strType = Cmb_Type.Text;

                if (DbGlobal.ImAlarm.SaveData(_Alarm) == 0)
                    MessageBox.Show("添加失败。");
                else
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    dr.CreateCells(Dg_TOFD_Bz);
                    Dg_TOFD_Bz.Rows.Insert(1, dr);

                    SysInfo.Grd_AddData(Dg_TOFD_Bz, 0, _Alarm);
                    for(int iRow=1;iRow < Dg_TOFD_Bz.RowCount;iRow ++)
                        Dg_TOFD_Bz[0, iRow].Value = iRow.ToString();
                    MessageBox.Show("添加： 成功!");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("数据异常，请检查填写数据！");
            }
            #endregion 添加数据
        }

        private void Bt_Delet_Click(object sender, EventArgs e)
        {
            int iCurrRow = Dg_TOFD_Bz.CurrentCell.RowIndex;//当前行

            if (iCurrRow < 1 || iCurrRow > Dg_TOFD_Bz.RowCount - 1) { return; }
            if (Dg_TOFD_Bz[6, iCurrRow].Value == null) { return; }
            if (Dg_TOFD_Bz[6, iCurrRow].Value.ToString() == "") { return; }
            string _strT = Dg_TOFD_Bz[6, iCurrRow].Value.ToString();
            string strMsg = "真要删除 ";
            string strTt = "删除数据提醒";

            if (MessageBox.Show(strMsg + "距离为: " + _strT + " 的数据吗?", strTt, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //删除数据库指定位置数据
                try
                {
                    if (_strT != "")
                        DbGlobal.ImAlarm.DeleteDat_ForID(SysInfo.m_Test_Parts.ID,
                                                         SysInfo.m_Test_Parts.Sub_ID,
                                                         float.Parse(_strT));
                    //1 列表初始化
                    DublGrid.SetDoubleBuffered(Dg_TOFD_Bz, true);
                    SysInfo.Grd_IniGrid_ShowZdName(Dg_TOFD_Bz, 2);
                    //2 填充列表数据
                    BrushGrd();
                }
                catch (Exception e3)
                { MessageBox.Show("删除" + _strT + "位置数据异常:" + e3.Message); }
            }
        }

        private void Txt_flLen_E_Click(object sender, EventArgs e)
        {
            SetSelect(0, 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iType">0:长度 1：高度 2：深度</param>
        /// <param name="i_S_E">0：开始 1：结束</param>
        private void SetSelect(int iType=0,int i_S_E=0)
        {
            switch (iType)
                {
                case 0:
                    Rd_D_Len.Checked = true;
                    Rd_D_Depth.Checked = false;
                    Rd_D_Height.Checked = false;
                    break;
                case 1:
                    Rd_D_Len.Checked = false;
                    Rd_D_Depth.Checked = false;
                    Rd_D_Height.Checked =true ;
                    break;
                case 2:
                    Rd_D_Len.Checked = false;
                    Rd_D_Depth.Checked =true ;
                    Rd_D_Height.Checked = false;
                    break;

            }
            SysInfo.m_BiaoZhu_No.iDataType = iType;
            SysInfo.m_BiaoZhu_No.i_S0_E1 = i_S_E;
        }

        private void Txt_flLen_S_Click(object sender, EventArgs e)
        {
            SetSelect(0, 0);
        }

        private void Txt_flHeight_S_Click(object sender, EventArgs e)
        {
            SetSelect(1, 0);
        }

        private void Txt_flHeight_E_Click(object sender, EventArgs e)
        {
            SetSelect(1, 1);
        }

        private void Txt_flDepth_S_Click(object sender, EventArgs e)
        {
            SetSelect(2, 0);
        }

        private void Dg_TOFD_Bz_Click(object sender, EventArgs e)
        {
            int iCurrRow = Dg_TOFD_Bz.CurrentCell.RowIndex;//当前行
            if (iCurrRow < 1) return;
            //    if (SysInfo.m_Test_Item .ID == "") return;
            try
            {
                int _iX = int.Parse(Dg_TOFD_Bz[8, iCurrRow].Value.ToString());
                int _iY = int.Parse(Dg_TOFD_Bz[9, iCurrRow].Value.ToString());
               
                SysInfo.g_Msg_InterFace.Fun_BrushBiaoZhu_Posit(_iX, _iY);
            }
            catch { }

        }
    }
}
