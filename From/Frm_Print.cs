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
using System.IO;

using ClassLib_TestData;
namespace Tofd_AWI.From
{
    public partial class Frm_Print : Form
    {
        public Frm_Print()
        {
            InitializeComponent();
        }

        private void Frm_Print_Load(object sender, EventArgs e)
        {
            SysInfo.m_blFrmOpen[8] = true;
            //1 列表初始化
            SysInfo.Grd_IniGrid_ShowZdName(Dg_Item, 3);
            SysInfo.Grd_IniGrid_ShowZdName(Dgr_Records, 4);
            SysInfo.Grd_IniGrid_ShowZdName(Dgr_Record_Curr, 5);
            SysInfo.Grd_IniGrid_ShowZdName(Dgr_Record_Alarm, 2);
            //2 项目数据刷新界面
            if (SysInfo.m_Lst_Print_Item.Count == 0)
            {
            }
            else
            {
                Dg_Item.RowCount = SysInfo.m_Lst_Print_Item.Count + 1;
                for (int i = 0; i < SysInfo.m_Lst_Print_Item.Count; i++)
                {
                    SysInfo.Grd_AddData(Dg_Item, i, SysInfo.m_Lst_Print_Item[i]);
                }
            }
            ////3 刷新当前项目检测记录

            ////4 刷新当前检测记录对应的数据

            ////5 刷新当前检测记录对应的报警数据

        }
        /// <summary>
        /// 选择文件夹，增加项目
        /// 添加当前文件夹下所有文件，然后根据缓存记录选择，刷新列表的记录选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_Item_Add_Click(object sender, EventArgs e)
        {
            //1 选择
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";
       
            string _strIniPath = "";
            if (SysInfo.m_W_strItemName == "")
                _strIniPath = SysInfo.m_W_i_FilePath + "\\";
            else
            {
                SysInfo.m_W_strItemName = SysInfo.m_W_strItemName.Replace(SysInfo.m_ItemMark, "");
                _strIniPath = SysInfo.m_W_i_FilePath + "\\" + SysInfo.m_W_strItemName + SysInfo.m_ItemMark;
            }
            dilog.SelectedPath = _strIniPath;

            dilog.ShowNewFolderButton = false;
            string _strPath = "";
            string _strFoldName = "";
            string[] _sPara = "".Split('\\');
            int _iNum = 0,_iSelectNo=0;
            bool _blHave = false;//项目不包含当前选中文件夹对应项目

            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                _strPath = dilog.SelectedPath;//目录文件是单选
                _sPara = _strPath.Split('\\');
                _iNum = _sPara.Length;
            }
            //添加项目和项目文件夹下的记录文件
            if (_iNum > 1 && _sPara[_iNum - 1].IndexOf("@@") > 0)
            {
                _strFoldName = _sPara[_iNum - 1].Split('_')[0];
                //2 判断项目缓存是否重复选择
                for (int i = 0; i < SysInfo.m_Lst_Print_Item.Count; i++)
                {
                    if (_strFoldName == SysInfo.m_Lst_Print_Item[i].strItemName)
                    {
                        _iSelectNo = i;//缓存对应文件
                        _blHave = true;
                        break;
                    }
                }
                if (_blHave == false)//项目缓存不包含本项目
                {
                    //3 添加进缓存
                    Cl_Print_Item _clsI = new Cl_Print_Item();
                    _clsI.strItemName = _strFoldName;
                    _clsI.strItemName_Path = _strPath;//D:\陈大伟\Tofd_AWI-2022-9-20\Tofd_AWI\bin\Debug\Temp\20221030213349_@@

                    #region 3.2 添加项目文件夹对应的文件
                    //3.2.1 获得文件
                    List<FileInfo> lst = Getdir(_strPath, "tdf");

                    //3.3.2 添加项目对应的所有新文件
                    foreach (FileInfo fl in lst)
                    {
                        Cl_Print_Record _clR = new Cl_Print_Record();
                        _clR.strRecordName = fl.Name.Split ('.')[0];
                        _clR.strRecordName_Path = fl.FullName;
                    //    _clR.blCheck = false;

                        _clsI.m_LstRecord.Add(_clR);
                    }
                    //3.3.3 项目缓存添加本文件
                    SysInfo.m_Lst_Print_Item.Add(_clsI);
                    _iSelectNo = SysInfo.m_Lst_Print_Item.Count - 1;
                    #endregion 3.2

                    //4 添加进项目列表
                    //      SysInfo.Grd_AddData(Dg_Item, _iSelectNo, SysInfo.m_Lst_Print_Item[_iSelectNo]);
                    Grd_BrushItem();
                }
                else//项目缓存已经包含本项目，检查其中是否有新文件
                {
                    List<FileInfo> lst = Getdir(_strPath, "tdf");

                    //3.3.2 添加项目对应的所有新文件
                    foreach (FileInfo fl in lst)
                    {
                        _blHave = false;
                        _iNum = SysInfo.m_Lst_Print_Item[_iSelectNo].m_LstRecord.Count;
                        for (int _No = 0; _No < _iNum; _No++)
                        {
                            if (fl.Name.IndexOf (  SysInfo.m_Lst_Print_Item[_iSelectNo].m_LstRecord[_No].strRecordName)>-1)
                            { _blHave = true; break; }
                        }//20221030213349_44-22-22
                        if (_blHave == false)//本项目文件不在对应的缓存中，就添加
                        {
                            Cl_Print_Record _clR = new Cl_Print_Record();
                            _clR.strRecordName = fl.Name.Split('.')[0];
                            _clR.strRecordName_Path = fl.FullName;
                          //  _clR.blCheck = false;
                            SysInfo.m_Lst_Print_Item[_iSelectNo].m_LstRecord.Add(_clR);
                        }
                    }
                }
                //5 显当前项目文件下下的文件
                GrdRecord_Brush(_iSelectNo);
            }
        }
        /// <summary>
        /// 已知项目列表选中序号，刷新检测记录列表
        /// </summary>
        /// <param name="iSelectNo"></param>
        private void GrdRecord_Brush(int iSelectNo)
        {
            SysInfo.Grd_IniGrid_ShowZdName(Dgr_Records, 4);
            Dgr_Records.RowCount = SysInfo.m_Lst_Print_Item[iSelectNo].m_LstRecord.Count + 1;
            for (int i = 0; i < SysInfo.m_Lst_Print_Item[iSelectNo].m_LstRecord.Count; i++)
                SysInfo.Grd_AddData(Dgr_Records, i, SysInfo.m_Lst_Print_Item[iSelectNo].m_LstRecord[i]);
        }
        private void Grd_BrushItem()
        {
            SysInfo.Grd_IniGrid_ShowZdName(Dg_Item, 3);
            Dg_Item.RowCount = SysInfo.m_Lst_Print_Item.Count + 1;
            for (int i = 0; i < SysInfo.m_Lst_Print_Item.Count; i++)
                SysInfo.Grd_AddData(Dg_Item, i, SysInfo.m_Lst_Print_Item[i]);
        }
        /// <summary>
        /// 私有方法,递归获取指定类型文件,包含子文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extName"></param>
        private List<FileInfo> Getdir(string path, string extName)
        {
            List<FileInfo> lst = new List<FileInfo>();
            try
            {
                extName = extName.ToLower();
                string [] _Arrdir = Directory.GetDirectories(path); //文件夹列表   
                DirectoryInfo fdir = new DirectoryInfo(path);
                FileInfo[] file = fdir.GetFiles();
                //FileInfo[] file = Directory.GetFiles(path); //文件列表   
                if (file.Length != 0 )//|| fdir. != 0) //当前目录文件或文件夹不为空                   
                {
                    foreach (FileInfo f in file) //显示当前目录所有文件   
                    {
                        if (f.Extension.ToLower() .IndexOf(extName) > 0)
                        {
                            lst.Add(f);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
             //   LogHelper.WriteLog(ex);
                throw ex;
            }
            return lst;
        }

        private void Bt_Item_Delet_Click(object sender, EventArgs e)
        {
            if (m_i_DgrItem_SelectRow > 0 && m_i_DgrItem_SelectRow <= SysInfo.m_Lst_Print_Item.Count)
            { 
                //1 删除缓存对应位置数据
                int _iCurrNo = m_i_DgrItem_SelectRow - 1;
                SysInfo.m_Lst_Print_Item.RemoveAt(_iCurrNo);

                Grd_BrushItem();

                SysInfo.Grd_IniGrid_ShowZdName(Dgr_Records, 4);
                SysInfo.Grd_IniGrid_ShowZdName(Dgr_Record_Curr, 5);
                SysInfo.Grd_IniGrid_ShowZdName(Dgr_Record_Alarm, 2);
                return;
                SysInfo.Grd_IniGrid_ShowZdName(Dg_Item, 3);

                //2 项目数据刷新界面
                for (int i = 0; i < SysInfo.m_Lst_Print_Item.Count; i++)
                    SysInfo.Grd_AddData(Dg_Item, i, SysInfo.m_Lst_Print_Item[i]);
            }
        }
        /// <summary>
        /// 记录选中行
        /// </summary>
        int m_i_DgrRec_SelectRow = -1;
        /// <summary>
        /// 项目选中行
        /// </summary>
        int m_i_DgrItem_SelectRow = -1;
        private void Dgr_Records_Click(object sender, EventArgs e)
        {
            if (Dgr_Records.CurrentCell.RowIndex > 0)
            {
                m_i_DgrRec_SelectRow = Dgr_Records.CurrentCell.RowIndex;
                string _Hfbh = SysInfo.m_W_strWeldID;
                try
                {             
                    //3 找到记录，刷新记录内容列表
                    SysInfo.Grd_IniGrid_ShowZdName(Dgr_Record_Curr, 5);
                    SysInfo.Grd_IniGrid_ShowZdName(Dgr_Record_Alarm, 2);
                    if (m_i_DgrItem_SelectRow == -1|| SysInfo.m_Lst_Print_Item.Count ==1) m_i_DgrItem_SelectRow = 1;
                    //2 寻找对应记录数据
                    string _strPath = SysInfo.m_Lst_Print_Item[m_i_DgrItem_SelectRow-1].m_LstRecord[m_i_DgrRec_SelectRow-1].strRecordName_Path;
                    SysInfo.csInter.FileReadByte_GetDat(_strPath);// 读数据，存放到显示缓存

                    //4 保存系统数据：系统缓存、报表数据和缺陷数据
                    SysInfo.Grd_AddData(Dgr_Record_Curr, 0, SysInfo.m_Report_Para_CS);
                    Dgr_Record_Alarm.RowCount = SysInfo.m_Lst_Mark_Record_CS.Count + 1;
                    for (int i = 0; i < SysInfo.m_Lst_Mark_Record_CS.Count; i++)
                        SysInfo.Grd_AddData_Print(Dgr_Record_Alarm, i, SysInfo.m_Lst_Mark_Record_CS[i]);
                }
                catch (Exception exrcord)
                { }
                SysInfo.m_W_strWeldID = _Hfbh;
            }
        }

        private void Dg_Item_Click(object sender, EventArgs e)
        {
            if (Dg_Item.CurrentCell.RowIndex > 0)
            {
                //1 获得索引号
                m_i_DgrItem_SelectRow = Dg_Item.CurrentCell.RowIndex;
                int _iSelectNo = m_i_DgrItem_SelectRow - 1;

                List<FileInfo> lst = Getdir(SysInfo.m_Lst_Print_Item[_iSelectNo].strItemName_Path, "tdf");
                bool _blHave = false;
                int _iNum = 0;
                //3.3.2 添加项目对应的所有新文件
                foreach (FileInfo fl in lst)
                {
                    _blHave = false;
                    _iNum = SysInfo.m_Lst_Print_Item[_iSelectNo].m_LstRecord.Count;
                    for (int _No = 0; _No < _iNum; _No++)
                    {
                        if (fl.Name.IndexOf(SysInfo.m_Lst_Print_Item[_iSelectNo].m_LstRecord[_No].strRecordName) > -1)
                        { _blHave = true; break; }
                    }//20221030213349_44-22-22
                    if (_blHave == false)//本项目文件不在对应的缓存中，就添加
                    {
                        Cl_Print_Record _clR = new Cl_Print_Record();
                        _clR.strRecordName = fl.Name.Split('.')[0];
                        _clR.strRecordName_Path = fl.FullName;
                        //  _clR.blCheck = false;
                        SysInfo.m_Lst_Print_Item[_iSelectNo].m_LstRecord.Add(_clR);
                    }
                }

                //2 项目文件对应的检测记录填充检测记录列表
                GrdRecord_Brush(m_i_DgrItem_SelectRow-1);
                SysInfo.Grd_IniGrid_ShowZdName(Dgr_Record_Curr, 5);
                SysInfo.Grd_IniGrid_ShowZdName(Dgr_Record_Alarm, 2);
            }
        }

        private void Bt_Ok_Click(object sender, EventArgs e)
        {
            #region 模板文件名
            string strMbFileName = System.Windows.Forms.Application.StartupPath + "\\DataBase\\";
            groupBox1.Enabled = false;

            if (SysInfo.m_Plant.iRad_Dw == 1)
                strMbFileName += "Original record_inch.xlsx";
            else
            {
                if (SysInfo.m_iLanguage == 0)
                    strMbFileName += "Thickness record.xlsx";
                else
                    strMbFileName += "Original record.xlsx";
            }
            string strResultRep = "";
            string strOutPath= SysInfo.csInter.IniReadDefine("Browse", "strPathFileName", "", SysInfo.HardFileName);
            if (strOutPath == "")
                strOutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
           
            if (System.IO.Directory.Exists(strOutPath) == false)
            {
                MessageBox.Show("报表保存路径不存在：" + strOutPath + "!  请在系统配置中，设置报表保存路径。");
                return;
            }

            string _strFileName = SysInfo.GetCurrDataTableName(3, SysInfo.m_Test_Item.ID, SysInfo.m_Report_Para_CS.Txt_Wtdw );
            strResultRep = strOutPath + "\\" + _strFileName + ".xlsx";
            Prg_Print_Bar.Visible = true;
            #endregion 模板

            string _Path = Application.StartupPath + "\\DataBase\\";
            //先实现当前数据报表，后面再做多项目的联合报表
            //当前方案下的数据

            List<ClassPage> _Lst_Item = new List<ClassPage>();//检测报告声明页统计
            int _iRecord_Num = 0;
            string _strPath = "";
            SysInfo.m_LstReport_Para.Clear();
            for (int _i_ItemNo = 0; _i_ItemNo < SysInfo.m_Lst_Print_Item.Count; _i_ItemNo++)
            {
                //2 拿项目
                _iRecord_Num = SysInfo.m_Lst_Print_Item[_i_ItemNo].m_LstRecord.Count;

                if (_iRecord_Num == 0) continue;//没有检测数据

                SysInfo.m_Report.m_i_Pages_No = 1;

                #region 3 统计当前项目（储罐)缺陷
                List<Class_Test_AlarmArea> _Lst_Print_Alrm = new List<Class_Test_AlarmArea>();
                int _i_Alarm_Num = 0;
                string _strGjbh = "";
                for (int _i_Rec = 0; _i_Rec < _iRecord_Num; _i_Rec++)
                {

                    //3.1 拿项目对应检测记录数据
                    if (SysInfo.m_Lst_Print_Item[_i_ItemNo].m_LstRecord[_i_Rec].blCheck)
                    {
                        _strPath = SysInfo.m_Lst_Print_Item[_i_ItemNo].m_LstRecord[_i_Rec].strRecordName_Path;
                        SysInfo.csInter.FileReadByte_GetDat(_strPath);// 读数据，获得项目参数与缺陷数据

                        //3.2 检测记录的项目信息
                        _strGjbh = SysInfo.m_Report_Para_CS.Txt_Gjbh;
                        if (_i_Rec == 0)
                        {
                            ClassPage _Cl_P = new ClassPage();
                            _Cl_P.Gjmc = SysInfo.m_Report_Para_CS.Txt_Gjmc;
                            _Cl_P.Bgbh = SysInfo.m_Report_Para_CS.Txt_Bg_Bgbh;
                            _Cl_P.Remark = SysInfo.m_Report_Para_CS.Txt_Bz;
                            _Lst_Item.Add(_Cl_P);//等待统计页数
                        }
                        //3.3 检测记录对应缺陷的缺陷记录汇总
                        _i_Alarm_Num += SysInfo.m_Lst_Mark_Record_CS.Count;
                        for (int _i_Alrm = 0; _i_Alrm < SysInfo.m_Lst_Mark_Record_CS.Count; _i_Alrm++)
                        {
                            Class_Test_AlarmArea _Alrm = new Class_Test_AlarmArea();
                            _Alrm.Gjbh = _strGjbh;
                            _Alrm.flLen_S = SysInfo.m_Lst_Mark_Record_CS[_i_Alrm].flLen_S;
                            _Alrm.Part_No = SysInfo.m_Lst_Mark_Record_CS[_i_Alrm].Part_No + "/" + (_i_Alrm + 1);
                            _Alrm.flLen = SysInfo.m_Lst_Mark_Record_CS[_i_Alrm].flLen;
                            _Alrm.flDepth = SysInfo.m_Lst_Mark_Record_CS[_i_Alrm].flDepth;
                            _Alrm.flHeight = SysInfo.m_Lst_Mark_Record_CS[_i_Alrm].flHeight;
                            _Alrm.strType = SysInfo.m_Lst_Mark_Record_CS[_i_Alrm].strType;
                            _Alrm.strZldj = SysInfo.m_Lst_Mark_Record_CS[_i_Alrm].strZldj;
                            _Alrm.strFileName = SysInfo.m_Lst_Mark_Record_CS[_i_Alrm].strFileName;
                            _Alrm.Remark = SysInfo.m_Lst_Mark_Record_CS[_i_Alrm].Remark;

                            _Lst_Print_Alrm.Add(_Alrm);
                        }
                    }
                }
                #endregion
                //4 当前项目参数
                int _iInt = (int)(_i_Alarm_Num / 24);
                int _iYs = _i_Alarm_Num % 24 > 0 ? 1 : 0;
                SysInfo.m_Report.m_i_Pages = _iInt + _iYs + 1;//检测报告续页 + 检测报告

                _Lst_Item[_Lst_Item.Count - 1].iPages = SysInfo.m_Report.m_i_Pages;
                Prg_Print_Bar.Maximum = SysInfo.m_Report.m_i_Pages + 14;
                Prg_Print_Bar.Value = 0;

                if (SysInfo.m_Report_Para_CS.Txt_Gjmc != "" &&
                  SysInfo.m_Report_Para_CS.Txt_Gjbh != "" &&
                  SysInfo.m_Report_Para_CS.Txt_Jcrq != "" || _Lst_Print_Alrm.Count >0)
                {
                    //4.1 项目内容
                    strMbFileName = _Path + "forging report_3.xlsx";
                    strResultRep = strOutPath + "\\" + _strFileName + "_3" + ".xlsx";
                    SysInfo.Get_UI();
                    Prg_Print_Bar.Value++;
                    SysInfo.m_Report.Set_Report_3(SysInfo.m_Report_Para_CS, strMbFileName, strResultRep);
                    SysInfo.m_Report.Save_Ysjl_ExcelFile();
                    //4.2 详细数据
                    Prg_Print_Bar.Value++;
                    strMbFileName = _Path + "forging report_4.xlsx";
                    strResultRep = strOutPath + "\\" + _strFileName + "_4" + ".xlsx";
                    SysInfo.m_Report.Set_Report_4(SysInfo.m_Report_Para_CS.Txt_Bg_Bgbh,
                                           SysInfo.m_Report_Para_CS.Txt_Gjbh,
                                           _Lst_Print_Alrm,
                                           strMbFileName, strResultRep);
                    SysInfo.m_Report.Save_Ysjl_ExcelFile();
                    Prg_Print_Bar.Value += _i_Alarm_Num;
                }
        
                //4.3 项目首页
                strMbFileName = _Path + "Report_1.xlsx";
                strResultRep = strOutPath + "\\" + _strFileName + "_1" + ".xlsx";
                SysInfo.m_Report.Set_Report_1(SysInfo.m_Report_Para_CS, strMbFileName, strResultRep);
                SysInfo.m_Report.Save_Ysjl_ExcelFile();
                Prg_Print_Bar.Value++;
                //4.4 检测报告声明页统计
                strMbFileName = _Path + "Report_2.xlsx";
                strResultRep = strOutPath + "\\" + _strFileName + "_2" + ".xlsx";
                SysInfo.m_Report.Set_Report_2(_Lst_Item, strMbFileName, strResultRep);
                SysInfo.m_Report.Save_Ysjl_ExcelFile();
                Prg_Print_Bar.Value = Prg_Print_Bar.Maximum;
                Prg_Print_Bar.Visible = false;
            }
            MessageBox.Show("报表生成路径：" + strOutPath);
            groupBox1.Enabled = true;
        }

        private void Frm_Print_FormClosed(object sender, FormClosedEventArgs e)
        {
            SysInfo.m_blFrmOpen[8] = false;
        }
    }
}

