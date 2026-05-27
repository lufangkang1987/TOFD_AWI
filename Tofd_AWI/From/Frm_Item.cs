/*
 * Copyright(C) 2 2017 河南德朗智能科技有限公司
 * 文件名: Frm_Item.cs
 * 文件功能描述: 项目管理
 * 目的：项目的新增、修改、查询、删除，以及系统参数修改等操作
 * 创建标识: 陈大伟 20210820
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
using System.Windows.Forms;

using ClassLib_DataMang.DataBaseMang.OleDal;//数据库操作类
using Tofd_AWI.Class;
using ClassLib_TestData;


namespace Tofd_AWI.From
{
    public partial class Frm_Item : Form
    {
        #region 变量

        /// <summary>
        /// 0:确认 1：查询 2：取消
        /// </summary>
        int m_iCloseType = 0;
        /// <summary>
        /// 查询单位信息
        /// </summary>
        List<Class_Test_Item> m_lstInfo = new List<Class_Test_Item>();
        /// <summary>
        /// 窗体激活
        /// </summary>
        bool m_blActive = false;

        #endregion 变量
        public Frm_Item()
        {
            InitializeComponent();
        }

        private void Bt_Ok_Click(object sender, EventArgs e)
        {
            if(Lb_ID.Text=="" || Cmb_Dwmc.Text==""|| Cmb_ItemName.Text=="")
            {
                MessageBox.Show("ID、单位名称、项目名称：不能为空!");
                return;
            }
            ItemData(1);
            m_iCloseType = 0;
            if (SysInfo.m_Test_Item.ID!="")
                SysInfo.g_Msg_InterFace.Fun_BrushCurrWeld(1);
            this.Close();
        }

        private void Frm_Item_Load(object sender, EventArgs e)
        {
            #region  项目参数
            //单位名称从数据库中拿数据
            List<string > _lst_Info = DbGlobal.ImTest_Item  .GetData_ByMySel("strDwmc");
            ComInit(Cmb_Dwmc, _lst_Info);

            ItemData(0);
            #endregion

            #region 历史数据
            #region 查询初始化
            Init_Cx();
            SysInfo.Grd_IniGrid_ShowZdName(Dg_Item, 0);
      //      SysInfo.Grd_IniGrid_ShowZdName(Dg_Alarm, 2);
            #endregion
            #endregion 
            #region 系统参数

            #endregion 系统参数
            m_blActive = true;
        }
        /// <summary>
        /// 查询界面初始化
        /// </summary>
        private void Init_Cx()
        {
            string strT = "";
            string[] strArr;//临时
            #region 1 设置双缓冲
            /// <summary>
            /// 设置GridView双缓冲
            /// </summary>
            DublGrid.SetDoubleBuffered(Dg_Item, true);
            DublGrid.SetDoubleBuffered(Dg_Alarm, true);
            #endregion 1 

            #region 2 查询
            if (SysInfo.m_iLanguage == 0)
                strT = SysInfo.csInter.IniReadDefine("Browse", "Filed_Names", "ID,单位名称,项目名称,设备编号,试块名称,检测依据,检验员", SysInfo.HardFileName);
            else
                strT = SysInfo.csInter.IniReadDefine("Browse", "Filed_Names_English", "ID,Unit name,Project name, Equipment number, Test block name, Test basis, Inspector", SysInfo.HardFileName);

            strArr = strT.Split(',');
            if (strArr.Length > 0)
            {
                Cmb_Zd_1.Items.Clear();
                Cmb_Zd_2.Items.Clear();
                Cmb_Zd_3.Items.Clear();
                Cmb_Zd_4.Items.Clear();
                Cmb_Zd_5.Items.Clear();
                for (int i = 0; i < strArr.Length; i++)
                {
                    Cmb_Zd_1.Items.Add(strArr[i]);
                    Cmb_Zd_2.Items.Add(strArr[i]);
                    Cmb_Zd_3.Items.Add(strArr[i]);
                    Cmb_Zd_4.Items.Add(strArr[i]);
                    Cmb_Zd_5.Items.Add(strArr[i]);
                }
            }
            #endregion  2

            SysInfo.Grd_IniGrid_ShowZdName(Dg_Alarm, 2);
        }
        /// <summary>
        /// 项目参数读写
        /// </summary>
        /// <param name="iType">0:读  1：更新</param>
        private void ItemData(int iType=0)
        {
            if (iType == 0)
            {
                SysInfo.m_blFrmOpen[1] = true;
                if (SysInfo.m_Test_Item.Testing_Standard == "") SysInfo.m_Test_Item.Testing_Standard = "NB/T4730-10-2015";
                if (SysInfo.m_Test_Item.Testblock == "") SysInfo.m_Test_Item.Testblock = "CSK-1A";

                if (SysInfo.m_Test_Item.ID == "")
                    Lb_Item_ID.Text = DateTime.Now.ToString("yyMMddHHmmss");
                else
                    Lb_Item_ID.Text = SysInfo.m_Test_Item.ID;
                Cmb_Dwmc.Text = SysInfo.m_Test_Item.Dwmc;
                Cmb_ItemName.Text = SysInfo.m_Test_Item.ItemName;
                Txt_Sbbh.Text = SysInfo.m_Test_Item.Sbbh;
                Txt_Testblock.Text = SysInfo.m_Test_Item.Testblock;
                Txt_Standard.Text = SysInfo.m_Test_Item.Testing_Standard;
                Txt_Jyy.Text = SysInfo.m_Test_Item.strJyy;
            }
            else
            {
                try
                {
                    SysInfo.m_Test_Item.ID = Lb_Item_ID.Text;
                    SysInfo.m_Test_Item.Dwmc = Cmb_Dwmc.Text;
                    SysInfo.m_Test_Item.ItemName = Cmb_ItemName.Text;
                    SysInfo.m_Test_Item.Sbbh = Txt_Sbbh.Text;
                    SysInfo.m_Test_Item.Testblock = Txt_Testblock.Text;
                    SysInfo.m_Test_Item.Testing_Standard = Txt_Standard.Text;
                    SysInfo.m_Test_Item.strJyy = Txt_Jyy.Text;

                    if (DbGlobal.ImTest_Item.SaveData(ref SysInfo.m_Test_Item) != 0)
                    {
                        SysInfo.m_i_Cl0_Cx1 = 0;
                        if (SysInfo.m_blRunStepTips) MessageBox.Show("项目创建:  OK!");

                    }
                    else
                    {
                        SysInfo.m_Test_Item.ID = "";
                        MessageBox.Show("项目创建失败！");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
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
        private void Frm_Item_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (m_iCloseType < 2)
            //    SysInfo.g_Msg_InterFace.Fun_BrushCurrWeld(0);
            SysInfo.m_blFrmOpen[1] = false ;
        }

        private void Lb_Item_ID_Click(object sender, EventArgs e)
        {
            Lb_Item_ID.Text  = DateTime.Now.ToString("yyMMddHHmmss");
        }

        private void Bt_CanCel_Click(object sender, EventArgs e)
        {
            m_iCloseType = 2;
            this.Close();
        }

        private void Cmb_Dwmc_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Cmb_Dwmc_TextChanged(object sender, EventArgs e)
        {
            //选择单位名称，调出对应的项目名称集合
            List<string> _lst_Info = DbGlobal.ImTest_Item.GetData_ByMySel_Where("strItemName", "strDwmc", Cmb_Dwmc.Text);
            ComInit(Cmb_ItemName, _lst_Info);

        }

        private void Cmb_Zd_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SysInfo.csInter.INIWriteValue("Browse", "Data_Cmb_Zd_1", Cmb_Zd_1.SelectedIndex.ToString(), SysInfo.HardFileName);
            Add_Val_Cmb(1, Cmb_Zd_1.Text);
        }
        private string GetZdName(string strZj_Zd_Val)
        {
            string strRet = "";
            if (SysInfo.m_iLanguage == 0)
            {
                switch (strZj_Zd_Val)
                {
                    case "ID"://
                        strRet = "ID";
                        break;
                    case "单位名称"://
                        strRet = "strDwmc";
                        break;

                    case "项目名称"://
                        strRet = "strItemName";
                        break;
                    case "设备编号"://
                        strRet = "strSbbh";
                        break;
                    case "试块名称"://
                        strRet = "strTestblock";
                        break;
                    case "检测依据"://
                        strRet = "strTesting_Standard";
                        break;
                    case "检验员"://
                        strRet = "strJyy";
                        break;
                }
            }
            else
            {
                switch (strZj_Zd_Val)
                {
                    case "ID"://
                        strRet = "ID";
                        break;
                    case "Unit name"://
                        strRet = "strDwmc";
                        break;

                    case "Project name"://
                        strRet = "strItemName";
                        break;
                    case "Equipment number"://设备序号
                        strRet = "strSbbh";
                        break;
                    case "Test block name"://检验日期
                        strRet = "strTestblock";
                        break;
                    case "Test basis"://检测员
                        strRet = "strTesting_Standard";
                        break;
                    case "Inspector"://校核员
                        strRet = "strJyy";
                        break;
                }
            }
            return strRet;
        }
        /// <summary>
        /// 字段变化后，搜索数据库指定字段数据
        /// </summary>
        /// <param name="iNo"></param>
        /// <param name="strZj_Zd_Val"></param>
        private void Add_Val_Cmb(int iNo, string strZj_Zd_Val)
        {
            string strTmp = "";//临时

            #region 使用数据库数据
            strTmp = GetZdName(strZj_Zd_Val);
            //查找数据库
            if (strTmp != "")
            {
                string strSort = "";
                if (strTmp == "ID")
                {
                    strSort = " order by " + strTmp + " Desc";
                }
                strTmp = DbGlobal.ImTest_Item  .GetDat(strTmp, strSort);
                if (strTmp != "")
                {
                    #region  清除字段指定选择列的数据
                    switch (iNo)//字段位置
                    {
                        case 1:
                            Cmb_Val_1.Items.Clear(); Ck_1.Checked = true; break;
                        case 2:
                            Cmb_Val_2.Items.Clear(); Ck_2.Checked = true; break;
                        case 3:
                            Cmb_Val_3.Items.Clear(); Ck_3.Checked = true; break;
                        case 4:
                            Cmb_Val_4.Items.Clear(); Ck_4.Checked = true; break;
                        case 5:
                            Cmb_Val_5.Items.Clear(); Ck_5.Checked = true; break;
                    }
                    #endregion 清除

                    #region 添加字段数据
                    string[] _sPara = strTmp.Split('@');
                    for (int i = 0; i < _sPara.Length; i++)
                    {
                        if (_sPara[i] != "")
                        {
                            switch (iNo)//字段位置
                            {
                                case 1:
                                    Cmb_Val_1.Items.Add(_sPara[i]); break;
                                case 2:
                                    Cmb_Val_2.Items.Add(_sPara[i]); break;
                                case 3:
                                    Cmb_Val_3.Items.Add(_sPara[i]); break;
                                case 4:
                                    Cmb_Val_4.Items.Add(_sPara[i]); break;
                                case 5:
                                    Cmb_Val_5.Items.Add(_sPara[i]); break;
                            }
                        }
                    }
                    #endregion 添加字段数据
                }
            }
            #endregion 使用数据库数据
        }

        private void Cmb_Zd_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Add_Val_Cmb(2, Cmb_Zd_2.Text);
        }

        private void Cmb_Zd_3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Add_Val_Cmb(3, Cmb_Zd_3.Text);
        }

        private void Cmb_Zd_4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Add_Val_Cmb(4, Cmb_Zd_4.Text);
        }

        private void Cmb_Zd_5_SelectedIndexChanged(object sender, EventArgs e)
        {
            Add_Val_Cmb(5, Cmb_Zd_5.Text);
        }

        private void Bt_B_Click(object sender, EventArgs e)
        {
            Bt_BrowseData();
        }
        private void Bt_BrowseData()
        {
            m_lstInfo = GetQuery_SQL();
            // 2 组合查询数据
            SysInfo.Grd_IniGrid_ShowZdName(Dg_Item, 0);

            //3 显示数据
            Dg_Brush();
            Bt_Ckyssj.Enabled = false ;
            Grp_His.Visible = false;
        }
        /// <summary>
        /// 根据查询数据刷新单位信息列表
        /// </summary>
        private void Dg_Brush()
        {
            int iCol = 0;
            try
            {
                Dg_Item.RowCount = m_lstInfo.Count + 1;
                for (int iRow = 1; iRow <= m_lstInfo.Count; iRow++)
                {
                    iCol = 0;

                    Dg_Item[iCol, iRow].Value = iRow.ToString(); iCol++;

                    Dg_Item[iCol, iRow].Value = m_lstInfo[iRow - 1].ID ; iCol++;//ID
                    Dg_Item[iCol, iRow].Value = m_lstInfo[iRow - 1].Dwmc ; iCol++;//单位名称
                    Dg_Item[iCol, iRow].Value = m_lstInfo[iRow - 1].ItemName ; iCol++;//工程名称
                    Dg_Item[iCol, iRow].Value = m_lstInfo[iRow - 1].Sbbh ; iCol++;//受检设备编号
                    Dg_Item[iCol, iRow].Value = m_lstInfo[iRow - 1].Testblock; iCol++;//试块：CSK-1A 多个试块以"/"间隔
                    Dg_Item[iCol, iRow].Value = m_lstInfo[iRow - 1].Testing_Standard; iCol++;//检测标准（NB/T4730-10-2015）
                    Dg_Item[iCol, iRow].Value = m_lstInfo[iRow - 1].strJyy ; iCol++;//检验员
                }
            }
            catch (Exception e)
            { MessageBox.Show("Dg_Brush error：" + e.Message + e.StackTrace); }
        }
        /// <summary>
        /// 查询获得列表数据
        /// </summary>
        /// <returns></returns>
        private List<Class_Test_Item > GetQuery_SQL()
        {
            string strSQL = "";

            List<Class_Test_Item> _lstInfo = new List<Class_Test_Item>();
            #region  1 组装SQL
            if (Ck_1.Checked)
            {
                if (Cmb_Zd_1.Text != "" && Cmb_Fh_1.Text != "" && Cmb_Val_1.Text != "")
                    strSQL = GetZdName(Cmb_Zd_1.Text) + Cmb_Fh_1.Text + "'" + Cmb_Val_1.Text + "' ";
            }
            if (Ck_2.Checked)
            {
                if (Cmb_Zd_2.Text != "" && Cmb_Fh_2.Text != "" && Cmb_Val_2.Text != "")
                {
                    if (strSQL != "") strSQL += " and  ";
                    strSQL += GetZdName(Cmb_Zd_2.Text) + Cmb_Fh_2.Text + "'" + Cmb_Val_2.Text + "' ";
                }
            }
            if (Ck_3.Checked)
            {
                if (Cmb_Zd_3.Text != "" && Cmb_Fh_3.Text != "" && Cmb_Val_3.Text != "")
                {
                    if (strSQL != "") strSQL += " and  ";
                    strSQL += GetZdName(Cmb_Zd_3.Text) + Cmb_Fh_3.Text + "'" + Cmb_Val_3.Text + "' ";
                }
            }
            if (Ck_4.Checked)
            {
                if (Cmb_Zd_4.Text != "" && Cmb_Fh_4.Text != "" && Cmb_Val_4.Text != "")
                {
                    if (strSQL != "") strSQL += " and  ";
                    strSQL += GetZdName(Cmb_Zd_4.Text) + Cmb_Fh_4.Text + "'" + Cmb_Val_4.Text + "' ";
                }
            }
            if (Ck_5.Checked)
            {
                if (Cmb_Zd_5.Text != "" && Cmb_Fh_5.Text != "" && Cmb_Val_5.Text != "")
                {
                    if (strSQL != "") strSQL += " and  ";
                    strSQL += GetZdName(Cmb_Zd_5.Text) + Cmb_Fh_5.Text + "'" + Cmb_Val_5.Text + "' ";
                }
            }
            #endregion 1 组装
            if (strSQL != "")
            {
                strSQL += " order by ID desc";
                _lstInfo = DbGlobal.ImTest_Item  .GetData(strSQL);
            }
            return _lstInfo;
        }

        private void Dg_Item_Click(object sender, EventArgs e)
        {
            int iCurrRow = Dg_Item.CurrentCell.RowIndex;//当前行
            if (iCurrRow < 1) return;
        //    if (SysInfo.m_Test_Item .ID == "") return;

            string _ID = Dg_Item[1, iCurrRow].Value.ToString();
            string _Dwmc = Dg_Item[2, iCurrRow].Value.ToString();
            if (_ID != "") _ID = _ID.Trim();
            if (_Dwmc != "") _Dwmc = _Dwmc.Trim();
            if (_ID != "" && _Dwmc!="")
            {
                SysInfo.Grd_IniGrid_ShowZdName(Dg_Alarm, 2);
                Bt_Ckyssj.Enabled = true;
                ShowItemID(_ID);
                DbGlobal.ImAlarm.DbTableName = SysInfo.GetCurrDataTableName(2,_ID, _Dwmc);
                List<Class_Test_AlarmArea> _lstAlarm = DbGlobal.ImAlarm.GetData(_ID, "");
                if(_lstAlarm .Count >0)
                    Dg_Alarm.RowCount = _lstAlarm.Count + 1;
                for (int i = 0; i < _lstAlarm.Count; i++)
                    SysInfo.Grd_AddData(Dg_Alarm, i, _lstAlarm[i]);
            }

        }
        private void ShowItemID(String strID)
        {
            Lb_Item_Val.Text = "当前查询的项目ID号：" + strID;
        }

        private void Bt_Qure_Click(object sender, EventArgs e)
        {
            Grp_His.Visible = true ;
        }

        private void Bt_B_Cance_Click(object sender, EventArgs e)
        {
            Grp_His.Visible = false;
        }

        private void Bt_Ckyssj_Click(object sender, EventArgs e)
        {
            //以当前查看的ID，调用当前数据，刷新主界面查看历史数据
            //将当前数据刷新项目ID
            try
            {
                int iCurrRow = Dg_Item.CurrentCell.RowIndex;//当前行
                if (iCurrRow < 1) return;
                int _iCol = 1;
                SysInfo.m_Test_Item.ID = Dg_Item[_iCol++, iCurrRow].Value.ToString();
                SysInfo.m_Test_Item.Dwmc  = Dg_Item[_iCol++, iCurrRow].Value.ToString();
                SysInfo.m_Test_Item.ItemName  = Dg_Item[_iCol++, iCurrRow].Value.ToString();
                SysInfo.m_Test_Item.Sbbh = Dg_Item[_iCol++, iCurrRow].Value.ToString();

                SysInfo.m_Test_Item.Testblock  = Dg_Item[_iCol++, iCurrRow].Value.ToString();
                SysInfo.m_Test_Item.Testing_Standard = Dg_Item[_iCol++, iCurrRow].Value.ToString();
                SysInfo.m_Test_Item.strJyy  = Dg_Item[_iCol++, iCurrRow].Value.ToString();
                if (SysInfo.m_Test_Item.ID != "")
                    SysInfo.g_Msg_InterFace.Fun_BrushCurrWeld(1);
                m_iCloseType = 1;
            }
            catch (Exception e2)
            {
                SysInfo.m_Test_Item.ID = "";
                SysInfo.m_Test_Item.ItemName = "";
                MessageBox.Show(e2.Message);
                return;
            }
            SysInfo.m_i_Cl0_Cx1 = 1;
            this.Close();
        }

        private void Bt_Sys_Ok_Click(object sender, EventArgs e)
        {
            //保存数据

            m_iCloseType = 2;
            this.Close();
        }

        private void Bt_Sys_CanCel_Click(object sender, EventArgs e)
        {
            m_iCloseType = 2;
            this.Close();
        }

        private void Bt_New_Click(object sender, EventArgs e)
        {
            Lb_Item_ID.Text = DateTime.Now.ToString("yyMMddHHmmss");
        }
    }
}
