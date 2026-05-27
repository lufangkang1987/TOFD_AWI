/*
 * Copyright(C) 2 2017 河南德朗智能科技有限公司
 * 文件名: Frm_Weld.cs
 * 文件功能描述: 焊缝管理
 * 目的：焊缝的新增、修改、查询、删除等操作
 * 创建标识: 陈大伟 20210819
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
    public partial class Frm_Weld : Form
    {
        #region 变量
        List<Class_Test_Parts> m_lstPart;
        #endregion  变量
        public Frm_Weld()
        {
            InitializeComponent();
        }

        private void Frm_Weld_Load(object sender, EventArgs e)
        {
            //1 下拉框初始化
            #region 参数刷新
            bool _blHave = true;
            List<string> _lst_Info=null ;
            try
            {
                DbGlobal.ImTest_Parts.DbTableName = SysInfo.GetCurrDataTableName(0, SysInfo.m_Test_Item.ID, SysInfo.m_Test_Item.Dwmc);
                _lst_Info = DbGlobal.ImTest_Parts.GetData_ByMySel("Part_No");
            }
            catch (Exception e1)
            {
                if (SysInfo.m_i_Cl0_Cx1 == 0)
                    DbGlobal.ImTest_Parts.Creat_Table();
                Lb_Weld_ID.Text = DateTime.Now.ToString("yyMMddHHmmss");
                _blHave = false;
            }
            if(_lst_Info.Count ==0)
            {
                if (SysInfo.m_i_Cl0_Cx1 == 0)
                    DbGlobal.ImTest_Parts.Creat_Table();
                Lb_Weld_ID.Text = DateTime.Now.ToString("yyMMddHHmmss");
                _blHave = false;
            }
            ComInit(Cmb_Bh, _lst_Info);

            if(_blHave )  ItemData(0);
            else
            {
                int _iCurrChan = SysInfo.m_Tofd_DLL.m_icurChan;
                SysInfo.m_Test_Parts.ProbeSpacing = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsLen;
                Txt_ProbeSpacing.Text = SysInfo.m_Test_Parts.ProbeSpacing.ToString("f1");
                Txt_ReMark.Text = SysInfo.m_Test_Parts.ReMark;
                float _flHd = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsEnd - SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsStart;
                Txt_Thick.Text = _flHd.ToString("f1");

            }
            #endregion 参数刷新

            #region 历史数据
            DublGrid.SetDoubleBuffered(Dg_Part, true);
            DublGrid.SetDoubleBuffered(Dg_Alarm, true);

            SysInfo.Grd_IniGrid_ShowZdName(Dg_Part, 1);
            SysInfo.Grd_IniGrid_ShowZdName(Dg_Alarm, 2);
            if (_blHave)
            {
                if (SysInfo.m_Test_Item.ID != "")//&& SysInfo.m_Test_Parts.Sub_ID != "")
                {
                    ShowSubId("", 1);
                     m_lstPart = DbGlobal.ImTest_Parts.GetData(SysInfo.m_Test_Item.ID);
                    if (m_lstPart.Count > 0)
                        Dg_Part.RowCount = m_lstPart.Count + 1;
                    for (int i = 0; i < m_lstPart.Count; i++)
                        SysInfo.Grd_AddData(Dg_Part, i, m_lstPart[i]);
                }
            }
            Tab_Contrl.SelectedIndex = SysInfo.m_i_Cl0_Cx1 == 1 ? 1 : 0;
            #endregion 历史数据
            SysInfo.m_blFrmOpen[2] = true;
        }
        /// <summary>
        /// 显示当前焊缝索引号
        /// </summary>
        private void ShowSubId(string strSubID, int iIt = 1)
        {
            if (iIt == 0)
                Lb_SubID_Val.Text = "";
            else
                Lb_SubID_Val.Text = "当前查询焊缝ID号：" + strSubID;// SysInfo.m_Test_Parts.Sub_ID;
        }

        /// <summary>
        /// 项目参数读写
        /// </summary>
        /// <param name="iType">0:读  1：更新</param>
        private void ItemData(int iType = 0)
        {
            int _iCurrChan = SysInfo.m_Tofd_DLL.m_icurChan;
            if (iType == 0)
            {
                SysInfo.m_Test_Parts.ID = SysInfo.m_Test_Item.ID;
                Lb_Weld_ID.Text = SysInfo.m_Test_Parts.Sub_ID ;
                Cmb_Bh.Text = SysInfo.m_Test_Parts.Part_No;
                Cmb_DetectionSite.Text = (SysInfo.m_Test_Parts.DetectionSite==0?"外壁":"内壁");
                Txt_Thick.Text = SysInfo.m_Test_Parts.flThicknise.ToString ();
                SysInfo.m_Test_Parts.ProbeSpacing = SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsLen;
                Txt_ProbeSpacing.Text = SysInfo.m_Test_Parts.ProbeSpacing.ToString ();
                Txt_ReMark.Text = SysInfo.m_Test_Parts.ReMark ;
                float _flHd =    SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsEnd- SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsStart;
                Txt_Thick.Text = _flHd.ToString();
            }
            else
            {
                try
                {
                    SysInfo.m_Test_Parts.ID = SysInfo.m_Test_Item.ID;
                    SysInfo.m_Test_Parts.Sub_ID = Lb_Weld_ID.Text;
                    SysInfo.m_Test_Parts.Part_No = Cmb_Bh.Text;
                    SysInfo.m_Test_Parts.DetectionSite = (Cmb_DetectionSite.Text== "外壁"?0:1);
                    SysInfo.m_Test_Parts.flThicknise =float .Parse ( Txt_Thick.Text);
                    SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsEnd = SysInfo.m_Test_Parts.flThicknise;
                    SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsStart = 0;
                    SysInfo.m_Test_Parts.ProbeSpacing = float.Parse(Txt_ProbeSpacing.Text);
                    SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsLen = SysInfo.m_Test_Parts.ProbeSpacing;
                    SysInfo.m_Test_Parts.ReMark = Txt_ReMark.Text;
                    SysInfo.m_Test_Parts.TOFD_Para = SavePara_Tofd();

                    if (DbGlobal.ImTest_Parts.SaveData(ref SysInfo.m_Test_Parts) != 0)
                    {
                        SysInfo.m_Test_Record.ID = SysInfo.m_Test_Item.ID;
                        SysInfo.m_Test_Record.Sub_ID = SysInfo.m_Test_Parts.Sub_ID;

                        SysInfo.m_Test_Alarm.ID = SysInfo.m_Test_Item.ID;
                        SysInfo.m_Test_Alarm.Sub_ID = SysInfo.m_Test_Parts.Sub_ID;

                        if (SysInfo.m_blRunStepTips)
                            MessageBox.Show("焊缝选择: OK! \r\n请点击: " + "3 开始检测");
                    }
                    else
                    {
                        SysInfo.m_Test_Parts.Sub_ID = "";
                        MessageBox.Show("焊缝选择失败！");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        /// <summary>
        /// 保存/恢复运行参数
        /// </summary>
        /// <returns></returns>
        private string SavePara_Tofd(int iType=0)
        { 
            int _iCurrChan = SysInfo.m_Tofd_DLL.m_icurChan;
            string _strRet = "";

            if (iType == 0)
            {
                #region 保存
                _strRet = SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X.ToString() + ",";
                _strRet += SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_dSpeed.ToString() + ",";
                _strRet += SysInfo.m_Tofd_DLL.m_pSparam_Real[_iCurrChan].T0 .ToString() + ",";
               
                _strRet += SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsType.ToString() + ",";
                _strRet += SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsMode.ToString() + ",";

                _strRet += SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_BScanMode.ToString() + ",";
                _strRet += SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn.ToString() + ",";

                _strRet += SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsDia.ToString() + ",";
                _strRet += SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsAngle.ToString() + ",";
                _strRet += SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsStart.ToString() + ",";
                _strRet += SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsEnd.ToString() + ",";

                _strRet += SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fEnRatio[
                    SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn].ToString();
                #endregion 
            }
            else
            {
                #region 恢复
                int iCurrRow = Dg_Part.CurrentCell.RowIndex-1;//当前行
                if (iCurrRow < 0) return "";
              
                string[] _sPara = "".Split(',');
                if (iCurrRow < m_lstPart.Count)
                {
                    _sPara = m_lstPart[iCurrRow].TOFD_Para.Split(',');
                    if (_sPara.Length > 11)
                    {
                        int _iNo = 0;
                        SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X = SysInfo.m_Plant.Scree_iDotWithmm_X = float.Parse(_sPara[_iNo++]);
                        SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_dSpeed = int.Parse(_sPara[_iNo++]);
                        SysInfo.m_Tofd_DLL.m_pSparam_Real[_iCurrChan].T0 = float.Parse(_sPara[_iNo++]);

                        SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsType = byte.Parse(_sPara[_iNo++]);
                        SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsMode = byte.Parse(_sPara[_iNo++]);

                        SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_BScanMode = byte.Parse(_sPara[_iNo++]);
                        SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn = byte.Parse(_sPara[_iNo++]);

                        SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsDia = float.Parse(_sPara[_iNo++]);
                        SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsAngle = float.Parse(_sPara[_iNo++]);
                        SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsStart = float.Parse(_sPara[_iNo++]);
                        SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsEnd = float.Parse(_sPara[_iNo++]);

                        SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fEnRatio[
                          SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn] = float.Parse(_sPara[_iNo]);
                    }
                }
                #endregion 
            }
            return _strRet;
        }
        private void ComInit(ComboBox Cmb, List<string> lst_Info)
        {
            Cmb.Items.Clear();
            if (lst_Info != null)
            {
                for (int i = 0; i < lst_Info.Count; i++)
                    Cmb.Items.Add(lst_Info[i]);
                Application.DoEvents();
            }
        }
        private void Frm_Weld_FormClosing(object sender, FormClosingEventArgs e)
        {
            SysInfo.m_blFrmOpen[2] = false;
        }

        private void Bt_Ok_Click(object sender, EventArgs e)
        {
            try
            {
                float _flT = float.Parse(Txt_Thick.Text);
                float _flJj = float.Parse(Txt_ProbeSpacing.Text);

                if (_flT>0 && _flJj>0 &&  Lb_Weld_ID.Text != "" && Cmb_Bh.Text != "" &&
                    Txt_Thick.Text != "" && Txt_ProbeSpacing.Text != "")
                {
                    ItemData(1);
                    if (SysInfo.m_Test_Parts.Sub_ID != "")
                        SysInfo.g_Msg_InterFace.Fun_BrushCurrWeld(3);
                    this.Close();
                }
                else
                    MessageBox.Show("请核对下参数!");
            }
            catch (Exception e22)
            {
                MessageBox.Show(e22.Message);
            }
        }

        private void Dg_Part_Click(object sender, EventArgs e)
        {
            Bt_Original.Enabled = true;
            int iCurrRow = Dg_Part.CurrentCell.RowIndex;//当前行
            if (iCurrRow < 1)return;
            if (SysInfo.m_Test_Parts.ID == "") return ;
            try
            {
                string _Sub_ID = Dg_Part[1, iCurrRow].Value.ToString();
                if (_Sub_ID != "") _Sub_ID = _Sub_ID.Trim();
                if (_Sub_ID != "")
                {
                    SysInfo.Grd_IniGrid_ShowZdName(Dg_Alarm, 2);

                    ShowSubId(_Sub_ID);
                    List<Class_Test_AlarmArea> _lstAlarm = DbGlobal.ImAlarm.GetData(SysInfo.m_Test_Parts.ID, _Sub_ID);

                    if (_lstAlarm.Count > 0)
                    {
                        Dg_Alarm.RowCount = _lstAlarm.Count + 1;
                    }
                    for (int i = 0; i < _lstAlarm.Count; i++)
                        SysInfo.Grd_AddData(Dg_Alarm, i, _lstAlarm[i]);
                }
            }
            catch (Exception e4)
            { MessageBox.Show(e4.Message); }
        }

        private void Lb_Weld_ID_Click(object sender, EventArgs e)
        {
            Lb_Weld_ID.Text = DateTime.Now.ToString("yyMMddHHmmss");
        }

        private void Bt_CanCel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Bt_Original_Click(object sender, EventArgs e)
        {
            //拿选中数据
            int iCurrRow = Dg_Part.CurrentCell.RowIndex;//当前行
            if (iCurrRow < 1) return;
            int _iCol = 1;
            string[] _sPara = "".Split(',');

            SysInfo.m_Test_Parts.ID = SysInfo.m_Test_Item.ID;
            SysInfo.m_Test_Parts.Sub_ID = Dg_Part[_iCol++, iCurrRow].Value.ToString();
            SysInfo.m_Test_Parts.Part_No = Dg_Part[_iCol++, iCurrRow].Value.ToString();
            SysInfo.m_Test_Parts.DetectionSite = (Dg_Part[_iCol++, iCurrRow].Value.ToString() == "外壁" ? 0 : 1);
            SysInfo.m_Test_Parts.flThicknise = float.Parse(Dg_Part[_iCol++, iCurrRow].Value.ToString());
            SysInfo.m_Test_Parts.ProbeSpacing = float.Parse(Dg_Part[_iCol++, iCurrRow].Value.ToString());
            SysInfo.m_Test_Parts.ReMark = Dg_Part[_iCol++, iCurrRow].Value.ToString();
            SavePara_Tofd(1);
            //查看指定焊缝ID的数据
            if (SysInfo.m_Test_Parts.Sub_ID != "")
                SysInfo.g_Msg_InterFace.Fun_BrushCurrWeld(2);
            this.Close();
        }

        private void Bt_New_Click(object sender, EventArgs e)
        {
            Lb_Weld_ID.Text = DateTime.Now.ToString("yyMMddHHmmss");
        }

        private void Bt_Delete_Click(object sender, EventArgs e)
        {

        }
    }
}
