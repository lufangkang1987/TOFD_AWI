/*
 * Copyright(C) 2 2017 郑州金润高科电子有限公司
 * 文件名: Equipment_Info.cs
 * 文件功能描述: 检测设备信息数据操作
 * 目的：用户信息数据库文件
 * 创建标识: 陈大伟 2017-1
 * 修改标识: 
 * 修改描述:
 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;//数据库
using System.Data.OleDb;//数据库类

using ClassLib_TestData;
using ClassLib_DataMang.DataBaseMang.DBUtility;//操作数据库

namespace ClassLib_DataMang.DataBaseMang.OleDal
{
    public class Test_Item
    {
        /// <summary>
        /// 操作的数据表名称 Test_Item_Info
        /// </summary>
        private static string _DbTableName = "Test_Item_Info";
        /// <summary>
        /// 指定数据时的操作表名称 Test_Item_Info
        /// </summary>
        public string DbTableName
        {
            get { return _DbTableName; }
            set { _DbTableName = value; }
        }
        /// <summary>
        /// 指定操作数据库类型  0：Accesse  1：Sel Serve
        /// </summary>
        private static int _iDataBase_Type = 0;
        /// <summary>
        /// 指定操作数据库类型 0：Accesse 1：Sel Serve
        /// </summary>
        public int m_iDataBase_Type
        {
            get { return _iDataBase_Type; }
            set { _iDataBase_Type = value; }
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <returns></returns>
        public bool Creat_Table()
        {
            bool _blRet = false;
            string strSQL = "", strFileName = ""; ;
            try
            {
                #region 字段表
                strFileName = " ID varchar(20)     NOT NULL," +//ID号 YYMMDDHHmm
                               "strDwmc    varchar(50)  NULL , " +//单位名称
                              " strItemName    varchar(250)  NULL , " +//工程名称
                              " strSbbh    varchar(50)  NULL , " +//受检设备编号
                          
                             " strSurface_condition    varchar(50)  NULL , " +//表面状况（清理《12.5um、打磨、毛坯）
                              " strMaterial    varchar(30)  NULL , " +//材质（Q345B）
                              " strWeldingtype    varchar(30)  NULL , " +//焊接类型
                             " strPkxs    varchar(10)   NULL, " +//坡口形式

                              " strJcSbName    varchar(50)  NULL , " +//检测设备名称
                              " strModel    varchar(30)  NULL , " +//检测设备型号
                              " strSerial_number    varchar(30)  NULL , " +//检测仪器编号
                              " strTestblock  varchar(55)  NULL , " +//试块：CSK-1A 多个试块以"/"间隔
                              " strTest_Proportion  varchar(10)  NULL , " +//检测比例（100%）
                              " strTesting_Standard  varchar(25)  NULL , " +//检测标准（NB/T4730-10-2015）
                              " strProcess_Doc_Number  varchar(25)  NULL , " +//工艺文件编号
                      
                              " strJLdw    varchar(100)  NULL , " +//监理单位单位
                              " strZzdw    varchar(100)  NULL , " +//制造单位"
                        
                              " strBgbh    varchar(50)  NULL , " +//报告编号
                             " strJyy    varchar(50)  NULL , " +//报告人以及资质UT(TOFD)-II
                             " strHyy    varchar(50)   NULL, " +//审核人 以及资质UT(TOFD)-II
                             " strSp    varchar(50)  NULL , " +//监理人

                            " strOther_1    varchar(50)  NULL , " +//备用1
                            " strOther_2    varchar(50)  NULL , " +//备用2

(_iDataBase_Type == 0 ? " iOther_1    Integer  NULL , " : "iOther_1 tinyint  NULL, ") +//备用3
(_iDataBase_Type == 0 ? " iOther_2    Integer  NULL , " : "iOther_2 tinyint  NULL, ") +//备用4

                            " ReMark    text "; //备注
                #endregion 字段表

                strSQL = "CREATE Table " + _DbTableName + "( " + strFileName + " )";
                switch (_iDataBase_Type)
                {
                    case 0:
                        if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                            _blRet = false;
                        else
                            _blRet = true;
                        break;
                    case 1:
                        if (SqlDbHelper.m_DataBase == "")
                            SqlDbHelper.Init();
                        if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                            _blRet = false;
                        else
                            _blRet = true;
                        break;
                }
            }
            catch (Exception e)
            { System.Windows.Forms.MessageBox.Show("Manage_Test_Item.cs的 Creat_Table error：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public bool DeleteDat()
        {
            string strSQL = "";
            bool _blRet = false;
            try
            {
                strSQL = "DELETE FROM " + _DbTableName;
                switch (_iDataBase_Type)
                {
                    case 0:
                        if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                            _blRet = false;
                        else
                            _blRet = true;
                        break;
                    case 1:
                        if (SqlDbHelper.m_DataBase == "")
                            SqlDbHelper.Init();
                        if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                            _blRet = false;
                        else
                            _blRet = true;
                        break;
                }
            }
            catch (Exception e)
            { System.Windows.Forms.MessageBox.Show("Manage_Test_Item.cs的DeleteDaterror：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
        /// <summary>
        /// 删除指定ID数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="iDown"></param>
        /// <returns></returns>
        public bool DeleteDat_ForID(string ID)
        {
            bool _blRet = false;//操作返回
            string strSQL = "DELETE   from  " + _DbTableName + "  Where ID='" + ID + "'";
            try
            {
                switch (_iDataBase_Type)
                {
                    case 0:
                        if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                            _blRet = false;
                        else
                            _blRet = true;
                        break;
                    case 1:
                        if (SqlDbHelper.m_DataBase == "")
                            SqlDbHelper.Init();
                        if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                            _blRet = false;
                        else
                            _blRet = true;
                        break;
                }
            }
            catch (Exception e)
            { System.Windows.Forms.MessageBox.Show("Manage_AlarmDat.cs的DeleteDat_ForIDerror：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
        /// <summary>
        /// 获得指定字段数据
        /// </summary>
        /// <returns></returns>
        public string GetDat(string strZd_Name, string strWhere)
        {
            string _strRet = "";
            string strSQL = "";
            try
            {
                //1 查询临时库中报警数据
                strSQL = "SELECT DISTINCT " + strZd_Name + "  from " + _DbTableName + " " + strWhere;
                switch (_iDataBase_Type)
                {
                    case 0:
                        DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //2 循环拿记录
                            foreach (DataRow dr in dt.Rows)
                            {
                                _strRet += (_strRet == "" ? "" : "@") + DBConvert.ToString(dr[strZd_Name]);
                            }
                        }
                        break;
                    case 1:
                        if (SqlDbHelper.m_DataBase == "")
                            SqlDbHelper.Init();
                        DataTable dt_1 = SqlDbHelper.ExecuteDataTable(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                        if (dt_1 != null && dt_1.Rows.Count > 0)
                        {
                            //2 循环拿记录
                            foreach (DataRow dr in dt_1.Rows)
                            {
                                _strRet += (_strRet == "" ? "" : "@") + DBConvert.ToString(dr[strZd_Name]);
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            { //System.Windows.Forms.MessageBox.Show("CngInfo.cs的GetDat：" + e.Message + e.StackTrace + " " + strSQL);

            }

            return _strRet;
        }

        public int UpData_Data(ref ClassLib_TestData. Class_Test_Item _Infor)
        {
            int iRet = 0;
            string strSQL = "";
            string strAdd_Val = "";//增加Sql值
            string strUpdata_Add_Zd = "";//更新字符串
            string strInsert_Add_Zd = "";
            try
            {
                //1 查询
                //单位名称  受检设备名称  设备地址
                strSQL = "SELECT * from " + _DbTableName + " where ID= '" + _Infor.ID  + "'" ;

                #region 插入字符串
                strInsert_Add_Zd = "ID,strDwmc,strItemName,strSbbh," +
                             "strSurface_condition,strMaterial,strWeldingtype,strPkxs," +
                             "strJcSbName,strModel,strSerial_number,strTestblock,strTest_Proportion,strTesting_Standard,strProcess_Doc_Number," +
                             "strJLdw,strZzdw," +
                             "strBgbh,strJyy,strHyy,strSp," +
                             "strOther_1,strOther_2," +
                             "iOther_1,iOther_2," +
                             "ReMark";
               
                strAdd_Val = "'" + _Infor.ID + "'";
                strAdd_Val += ",'" + _Infor.Dwmc  + "'";
                strAdd_Val += ",'" + _Infor.ItemName  + "'";
                strAdd_Val += ",'" + _Infor.Sbbh   + "'";

                strAdd_Val += ",'" + _Infor.Surface_condition + "'";
                strAdd_Val += ",'" + _Infor.Material  + "'";
                strAdd_Val += ",'" + _Infor.Weldingtype  + "'";
                strAdd_Val += ",'" + _Infor.strPkxs  + "'";

                strAdd_Val += ",'" + _Infor.JcSbName  + "'";
                strAdd_Val += ",'" + _Infor.Model  + "'";
                strAdd_Val += ",'" + _Infor.Serial_number + "'";
                strAdd_Val += ",'" + _Infor.Testblock  + "'";
                strAdd_Val += ",'" + _Infor.Test_Proportion  + "'";
                strAdd_Val += ",'" + _Infor._Testing_Standard + "'";
                strAdd_Val += ",'" + _Infor.Process_Doc_Number  + "'";

                strAdd_Val += ",'" + _Infor.strJLdw  + "'";
                strAdd_Val += ",'" + _Infor.strZzdw  + "'";
                strAdd_Val += ",'" + _Infor.strBgbh  + "'";
                strAdd_Val += ",'" + _Infor.strJyy  + "'";
                strAdd_Val += ",'" + _Infor.strHyy  + "'";
                strAdd_Val += ",'" + _Infor.strSp  + "'";

                strAdd_Val += ",'" + _Infor.strOther1 + "'";
                strAdd_Val += ",'" + _Infor.strOther2  + "'";
                strAdd_Val += "," + _Infor.iOther1  ;
                strAdd_Val += "," + _Infor.iOther2 ;

                strAdd_Val += ",'" + _Infor.ReMark  + "'";
                #endregion 插入字符串

                #region 更新字符串
                if (_Infor.ID == "")
                {
                    _Infor.ID = DateTime.Now.ToString("yyMMddHHmmss");
                    iRet = 2;
                }
                //3 插入数据
                strUpdata_Add_Zd = "SET ID='" + _Infor.ID + "'";
                strUpdata_Add_Zd += ",strDwmc='" + _Infor.Dwmc  + "'";
                strUpdata_Add_Zd += ",strItemName='" + _Infor.ItemName  + "'";
                strUpdata_Add_Zd += ",strSbbh='" + _Infor.Sbbh  + "'";

                strUpdata_Add_Zd += ",strSurface_condition='" + _Infor.Surface_condition  + "'";
                strUpdata_Add_Zd += ",strMaterial='" + _Infor.Material  + "'";
                strUpdata_Add_Zd += ",strWeldingtype='" + _Infor.Weldingtype   + "'";
                strUpdata_Add_Zd += ",strPkxs='" + _Infor.strPkxs  + "'";

                strUpdata_Add_Zd += ",strJcSbName='" + _Infor.JcSbName  + "'";
                strUpdata_Add_Zd += ",strModel='" + _Infor.Model  + "'";
                strUpdata_Add_Zd += ",strSerial_number='" + _Infor.Serial_number  + "'";
                strUpdata_Add_Zd += ",strTestblock='" + _Infor.Testblock  + "'";
                strUpdata_Add_Zd += ",strTest_Proportion='" + _Infor.Test_Proportion  + "'";
                strUpdata_Add_Zd += ",strTesting_Standard='" + _Infor.Testing_Standard  + "'";
                strUpdata_Add_Zd += ",strProcess_Doc_Number='" + _Infor.Process_Doc_Number  + "'";

                strUpdata_Add_Zd += ",strJLdw='" + _Infor.strJLdw  + "'";
                strUpdata_Add_Zd += ",strZzdw='" + _Infor.strZzdw  + "'";

                strUpdata_Add_Zd += ",strBgbh='" + _Infor.strBgbh  + "'";
                strUpdata_Add_Zd += ",strJyy='" + _Infor.strJyy  + "'";
                strUpdata_Add_Zd += ",strHyy='" + _Infor.strHyy  + "'";
                strUpdata_Add_Zd += ",strSp='" + _Infor.strSp  + "'";

                strUpdata_Add_Zd += ",strOther_1='" + _Infor.strOther1  + "'";
                strUpdata_Add_Zd += ",strOther_2='" + _Infor.strOther2  + "'";
                strUpdata_Add_Zd += ",iOther_1=" + _Infor .iOther1;
                strUpdata_Add_Zd += ",iOther_2=" + _Infor.iOther2   ;
                strUpdata_Add_Zd += ",ReMark='" + _Infor.ReMark  + "'";
                #endregion

                switch (_iDataBase_Type)
                {
                    case 0:
                        #region 0
                        try
                        {
                            DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                //2 更新数据 
                                if (_DbTableName != "" && strUpdata_Add_Zd != "")
                                {
                                    //2.3 添加到数据库
                                    strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  ID= '" + _Infor.ID + "'";

                                    if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                        iRet = 0;
                                    else
                                        iRet = 1;
                                }
                                return iRet;
                            }
                        }
                        catch { }
                        {
                                iRet = 0;
                        }
                        #endregion 0
                        break;
                    case 1:
                        if (SqlDbHelper.m_DataBase == "")
                            SqlDbHelper.Init();
                        #region 1
                        DataTable dt_1 = SqlDbHelper.ExecuteDataTable(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);
                        if (dt_1 != null && dt_1.Rows.Count > 0)
                        {
                            //2 更新数据 

                            if (_DbTableName != "" && strUpdata_Add_Zd != "")
                            {
                                //2.3 添加到数据库
                                strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  ID= '" + _Infor.ID + "'";

                                if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                    iRet = 0;
                                else
                                    if (iRet != 2)
                                    iRet = 1;
                            }
                        }
                        else
                        {
                            iRet = 0;
                        }
                        #endregion 1
                        break;
                }
            }
            catch (Exception e)
            { System.Windows.Forms.MessageBox.Show("Manage_Test_Item.cs的SaveData_Tmperror：" + e.Message + e.StackTrace + " " + strSQL); }

            return iRet;
        }
        /// <summary>
        /// 保存数据到临时库 0:失败 1：覆盖 2：插入新的
        /// </summary>
        /// <param name="_Infor"></param>
        /// <returns>0:失败 1：覆盖 2：插入新的</returns>
        public int SaveData(ref ClassLib_TestData.Class_Test_Item _Infor)
        {
            int iRet = 0;
            string strSQL = "";
            string strAdd_Val = "";//增加Sql值
            string strInsert_Add_Zd = "";//插入字段表
            string strUpdata_Add_Zd = "";//更新字符串
            try
            {
                if (_Infor.ID == "") return 0;
                //1 查询
                //单位名称  受检设备名称  设备地址
                strSQL = "SELECT * from " + _DbTableName + " where ID= '" + _Infor.ID  + "'";


                #region 插入字符串
                strInsert_Add_Zd = "ID,strDwmc,strItemName,strSbbh," +
                             "strSurface_condition,strMaterial,strWeldingtype,strPkxs," +
                             "strJcSbName,strModel,strSerial_number,strTestblock,strTest_Proportion,strTesting_Standard,strProcess_Doc_Number," +
                             "strJLdw,strZzdw," +
                             "strBgbh,strJyy,strHyy,strSp," +
                             "strOther_1,strOther_2," +
                             "iOther_1,iOther_2," +
                             "ReMark";

                strAdd_Val = "'" + _Infor.ID + "'";
                strAdd_Val += ",'" + _Infor.Dwmc + "'";
                strAdd_Val += ",'" + _Infor.ItemName + "'";
                strAdd_Val += ",'" + _Infor.Sbbh + "'";

                strAdd_Val += ",'" + _Infor.Surface_condition + "'";
                strAdd_Val += ",'" + _Infor.Material + "'";
                strAdd_Val += ",'" + _Infor.Weldingtype + "'";
                strAdd_Val += ",'" + _Infor.strPkxs + "'";

                strAdd_Val += ",'" + _Infor.JcSbName + "'";
                strAdd_Val += ",'" + _Infor.Model + "'";
                strAdd_Val += ",'" + _Infor.Serial_number + "'";
                strAdd_Val += ",'" + _Infor.Testblock + "'";
                strAdd_Val += ",'" + _Infor.Test_Proportion + "'";
                strAdd_Val += ",'" + _Infor._Testing_Standard + "'";
                strAdd_Val += ",'" + _Infor.Process_Doc_Number + "'";

                strAdd_Val += ",'" + _Infor.strJLdw + "'";
                strAdd_Val += ",'" + _Infor.strZzdw + "'";
                strAdd_Val += ",'" + _Infor.strBgbh + "'";
                strAdd_Val += ",'" + _Infor.strJyy + "'";
                strAdd_Val += ",'" + _Infor.strHyy + "'";
                strAdd_Val += ",'" + _Infor.strSp + "'";

                strAdd_Val += ",'" + _Infor.strOther1 + "'";
                strAdd_Val += ",'" + _Infor.strOther2 + "'";
                strAdd_Val += "," + _Infor.iOther1;
                strAdd_Val += "," + _Infor.iOther2;

                strAdd_Val += ",'" + _Infor.ReMark + "'";
                #endregion 插入字符串

                #region 更新字符串
  
                //3 插入数据
                strUpdata_Add_Zd = "SET ID='" + _Infor.ID + "'";
                strUpdata_Add_Zd += ",strDwmc='" + _Infor.Dwmc + "'";
                strUpdata_Add_Zd += ",strItemName='" + _Infor.ItemName + "'";
                strUpdata_Add_Zd += ",strSbbh='" + _Infor.Sbbh + "'";

                strUpdata_Add_Zd += ",strSurface_condition='" + _Infor.Surface_condition + "'";
                strUpdata_Add_Zd += ",strMaterial='" + _Infor.Material + "'";
                strUpdata_Add_Zd += ",strWeldingtype='" + _Infor.Weldingtype + "'";
                strUpdata_Add_Zd += ",strPkxs='" + _Infor.strPkxs + "'";

                strUpdata_Add_Zd += ",strJcSbName='" + _Infor.JcSbName + "'";
                strUpdata_Add_Zd += ",strModel='" + _Infor.Model + "'";
                strUpdata_Add_Zd += ",strSerial_number='" + _Infor.Serial_number + "'";
                strUpdata_Add_Zd += ",strTestblock='" + _Infor.Testblock + "'";
                strUpdata_Add_Zd += ",strTest_Proportion='" + _Infor.Test_Proportion + "'";
                strUpdata_Add_Zd += ",strTesting_Standard='" + _Infor.Testing_Standard + "'";
                strUpdata_Add_Zd += ",strProcess_Doc_Number='" + _Infor.Process_Doc_Number + "'";

                strUpdata_Add_Zd += ",strJLdw='" + _Infor.strJLdw + "'";
                strUpdata_Add_Zd += ",strZzdw='" + _Infor.strZzdw + "'";

                strUpdata_Add_Zd += ",strBgbh='" + _Infor.strBgbh + "'";
                strUpdata_Add_Zd += ",strJyy='" + _Infor.strJyy + "'";
                strUpdata_Add_Zd += ",strHyy='" + _Infor.strHyy + "'";
                strUpdata_Add_Zd += ",strSp='" + _Infor.strSp + "'";

                strUpdata_Add_Zd += ",strOther_1='" + _Infor.strOther1 + "'";
                strUpdata_Add_Zd += ",strOther_2='" + _Infor.strOther2 + "'";
                strUpdata_Add_Zd += ",iOther_1=" + _Infor.iOther1;
                strUpdata_Add_Zd += ",iOther_2=" + _Infor.iOther2;
                strUpdata_Add_Zd += ",ReMark='" + _Infor.ReMark + "'";
                #endregion

                switch (_iDataBase_Type)
                {
                    case 0:
                        #region 0
                        try
                        {
                            DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                //2 更新数据 
                                if (_DbTableName != "" && strUpdata_Add_Zd != "")
                                {
                                    //2.3 添加到数据库
                                    strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  ID= '" + _Infor.ID + "'";

                                    if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                        iRet = 0;
                                    else
                                        iRet = 1;
                                }
                                return iRet;
                            }
                        }
                        catch { }
                        {
                            //3 插入数据
                            strSQL = "insert into " + _DbTableName + "  (" + strInsert_Add_Zd + ") values(" + strAdd_Val + ")";
                            if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                iRet = 0;
                            else
                                iRet = 2;
                        }
                        #endregion 0
                        break;
                    case 1:
                        if (SqlDbHelper.m_DataBase == "")
                            SqlDbHelper.Init();
                        #region 1
                        DataTable dt_1 = SqlDbHelper.ExecuteDataTable(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);
                        if (dt_1 != null && dt_1.Rows.Count > 0)
                        {
                            //2 更新数据 
                            if (_DbTableName != "" && strUpdata_Add_Zd != "")
                            {
                                //2.3 添加到数据库
                                strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  ID= " + _Infor.ID + "'";

                                if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                    iRet = 0;
                                else
                                    if (iRet != 2)
                                    iRet = 1;
                            }
                        }
                        else
                        {
                            strSQL = "insert into " + _DbTableName + "  (" + strInsert_Add_Zd + ") values(" + strAdd_Val + ")";
                            if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                iRet = 0;
                            else
                                iRet = 2;
                        }
                        #endregion 1
                        break;
                }
            }
            catch (Exception e)
            { System.Windows.Forms.MessageBox.Show("Manage_Test_Item.cs的SaveData_Tmperror：" + e.Message + e.StackTrace + " " + strSQL); }

            return iRet;
        }

        /// <summary>
        /// 根据单位名称、受检设备名、设备地址查询
        /// </summary>
        /// <param name="_Infor"></param>
        /// <returns></returns>
        public bool GetData(ref ClassLib_TestData.Class_Test_Item _Infor)
        {
            bool _blRet = false;
            try
            {
                string strSQL = "SELECT * from " + _DbTableName + " where ID= '" + _Infor.ID + "'";
                
                switch (_iDataBase_Type)
                {
                    case 0:
                        #region  0
                        DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //2 循环拿记录
                            foreach (DataRow dr in dt.Rows)
                            {
                                #region 字段赋值
                                _Infor.ID = DBConvert.ToString(dr["ID"]);
                                _Infor.Dwmc  = DBConvert.ToString(dr["strDwmc"]);
                                _Infor.ItemName = DBConvert.ToString(dr["strItemName"]);
                                _Infor.Sbbh  = DBConvert.ToString(dr["strSbbh"]);

                                _Infor.Surface_condition  = DBConvert.ToString(dr["strSurface_condition"]);
                                _Infor.Material  = DBConvert.ToString(dr["strMaterial"]);
                                _Infor.Weldingtype  = DBConvert.ToString(dr["strWeldingtype"]);
                                _Infor.strPkxs  = DBConvert.ToString(dr["strPkxs"]);

                                _Infor.JcSbName  = DBConvert.ToString(dr["strJcSbName"]);
                                _Infor.Model  = DBConvert.ToString(dr["strModel"]);
                                _Infor.Serial_number  = DBConvert.ToString(dr["strSerial_number"]);
                                _Infor.Testblock  = DBConvert.ToString(dr["strTestblock"]);
                                _Infor.Test_Proportion  = DBConvert.ToString(dr["strTest_Proportion"]);
                                _Infor.Testing_Standard  = DBConvert.ToString(dr["strTesting_Standard"]);
                                _Infor.Process_Doc_Number  = DBConvert.ToString(dr["strProcess_Doc_Number"]);

                                _Infor.strJLdw  = DBConvert.ToString(dr["strJLdw"]);
                                _Infor.strZzdw  = DBConvert.ToString(dr["strZzdw"]);

                                _Infor.strBgbh  = DBConvert.ToString(dr["strBgbh"]);
                                _Infor.strJyy   = DBConvert.ToString(dr["strJyy"]);
                                _Infor.strHyy  = DBConvert.ToString(dr["strHyy"]);
                                _Infor.strSp  = DBConvert.ToString(dr["strSp"]);

                                _Infor.strOther1  = DBConvert.ToString(dr["strOther_1"]);
                                _Infor.strOther2 = DBConvert.ToString(dr["strOther_2"]);
                                _Infor.iOther1  = DBConvert.ToInt32(dr["iOther_1"]);
                                _Infor.iOther2  = DBConvert.ToInt32(dr["iOther_2"]);
                                _Infor.ReMark  = DBConvert.ToString(dr["ReMark"]);
                                #endregion 字段赋值
                                _blRet = true;
                                break;
                            }
                        }
                        #endregion 0
                        break;
                    case 1:
                        if (SqlDbHelper.m_DataBase == "")
                            SqlDbHelper.Init();
                        #region 1
                        DataTable dt_1 = SqlDbHelper.ExecuteDataTable(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                        if (dt_1 != null && dt_1.Rows.Count > 0)
                        {
                            //2 循环拿记录
                            foreach (DataRow dr in dt_1.Rows)
                            {
                                #region 字段赋值
                                _Infor.ID = DBConvert.ToString(dr["ID"]);
                                _Infor.Dwmc = DBConvert.ToString(dr["strDwmc"]);
                                _Infor.ItemName = DBConvert.ToString(dr["strItemName"]);
                                _Infor.Sbbh = DBConvert.ToString(dr["strSbbh"]);

                                _Infor.Surface_condition = DBConvert.ToString(dr["strSurface_condition"]);
                                _Infor.Material = DBConvert.ToString(dr["strMaterial"]);
                                _Infor.Weldingtype = DBConvert.ToString(dr["strWeldingtype"]);
                                _Infor.strPkxs = DBConvert.ToString(dr["strPkxs"]);

                                _Infor.JcSbName = DBConvert.ToString(dr["strJcSbName"]);
                                _Infor.Model = DBConvert.ToString(dr["strModel"]);
                                _Infor.Serial_number = DBConvert.ToString(dr["strSerial_number"]);
                                _Infor.Testblock = DBConvert.ToString(dr["strTestblock"]);
                                _Infor.Test_Proportion = DBConvert.ToString(dr["strTest_Proportion"]);
                                _Infor.Testing_Standard = DBConvert.ToString(dr["strTesting_Standard"]);
                                _Infor.Process_Doc_Number = DBConvert.ToString(dr["strProcess_Doc_Number"]);

                                _Infor.strJLdw = DBConvert.ToString(dr["strJLdw"]);
                                _Infor.strZzdw = DBConvert.ToString(dr["strZzdw"]);

                                _Infor.strBgbh = DBConvert.ToString(dr["strBgbh"]);
                                _Infor.strJyy = DBConvert.ToString(dr["strJyy"]);
                                _Infor.strHyy = DBConvert.ToString(dr["strHyy"]);
                                _Infor.strSp = DBConvert.ToString(dr["strSp"]);

                                _Infor.strOther1 = DBConvert.ToString(dr["strOther_1"]);
                                _Infor.strOther2 = DBConvert.ToString(dr["strOther_2"]);
                                _Infor.iOther1 = DBConvert.ToInt32(dr["iOther_1"]);
                                _Infor.iOther2 = DBConvert.ToInt32(dr["iOther_2"]);
                                _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);
                                #endregion 字段赋值
                                _blRet = true;
                                break;
                            }
                        }
                        #endregion 1
                        break;
                }
            }
            catch (Exception e)
            { }
            return _blRet;
        }
        public List<ClassLib_TestData.Class_Test_Item > GetData(string strWhere)
        {
            List<Class_Test_Item> _lst_Info = new List<Class_Test_Item>();
            string strSQL = "SELECT * from " + _DbTableName + " where " + strWhere;
            switch (_iDataBase_Type)
            {
                case 0:
                    #region 0
                    DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //2 循环拿记录
                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                Class_Test_Item _Infor = new Class_Test_Item();
                                #region 字段赋值
                                _Infor.ID = DBConvert.ToString(dr["ID"]);
                                _Infor.Dwmc = DBConvert.ToString(dr["strDwmc"]);
                                _Infor.ItemName = DBConvert.ToString(dr["strItemName"]);
                                _Infor.Sbbh = DBConvert.ToString(dr["strSbbh"]);

                                _Infor.Surface_condition = DBConvert.ToString(dr["strSurface_condition"]);
                                _Infor.Material = DBConvert.ToString(dr["strMaterial"]);
                                _Infor.Weldingtype = DBConvert.ToString(dr["strWeldingtype"]);
                                _Infor.strPkxs = DBConvert.ToString(dr["strPkxs"]);

                                _Infor.JcSbName = DBConvert.ToString(dr["strJcSbName"]);
                                _Infor.Model = DBConvert.ToString(dr["strModel"]);
                                _Infor.Serial_number = DBConvert.ToString(dr["strSerial_number"]);
                                _Infor.Testblock = DBConvert.ToString(dr["strTestblock"]);
                                _Infor.Test_Proportion = DBConvert.ToString(dr["strTest_Proportion"]);
                                _Infor.Testing_Standard = DBConvert.ToString(dr["strTesting_Standard"]);
                                _Infor.Process_Doc_Number = DBConvert.ToString(dr["strProcess_Doc_Number"]);

                                _Infor.strJLdw = DBConvert.ToString(dr["strJLdw"]);
                                _Infor.strZzdw = DBConvert.ToString(dr["strZzdw"]);

                                _Infor.strBgbh = DBConvert.ToString(dr["strBgbh"]);
                                _Infor.strJyy = DBConvert.ToString(dr["strJyy"]);
                                _Infor.strHyy = DBConvert.ToString(dr["strHyy"]);
                                _Infor.strSp = DBConvert.ToString(dr["strSp"]);

                                _Infor.strOther1 = DBConvert.ToString(dr["strOther_1"]);
                                _Infor.strOther2 = DBConvert.ToString(dr["strOther_2"]);
                                _Infor.iOther1 = DBConvert.ToInt32(dr["iOther_1"]);
                                _Infor.iOther2 = DBConvert.ToInt32(dr["iOther_2"]);
                                _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);
                                #endregion 字段赋值
                                _lst_Info.Add(_Infor);
                            }
                            catch (Exception E)
                            {

                            }
                        }
                    }
                    #endregion 0
                    break;
                case 1:
                    #region  1
                    if (SqlDbHelper.m_DataBase == "")
                        SqlDbHelper.Init();

                    DataTable dt_1 = SqlDbHelper.ExecuteDataTable(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                    if (dt_1 != null && dt_1.Rows.Count > 0)
                    {
                        //2 循环拿记录
                        foreach (DataRow dr in dt_1.Rows)
                        {
                            try
                            {
                                Class_Test_Item _Infor = new Class_Test_Item();

                                #region 字段赋值
                                _Infor.ID = DBConvert.ToString(dr["ID"]);
                                _Infor.Dwmc = DBConvert.ToString(dr["strDwmc"]);
                                _Infor.ItemName = DBConvert.ToString(dr["strItemName"]);
                                _Infor.Sbbh = DBConvert.ToString(dr["strSbbh"]);

                                _Infor.Surface_condition = DBConvert.ToString(dr["strSurface_condition"]);
                                _Infor.Material = DBConvert.ToString(dr["strMaterial"]);
                                _Infor.Weldingtype = DBConvert.ToString(dr["strWeldingtype"]);
                                _Infor.strPkxs = DBConvert.ToString(dr["strPkxs"]);

                                _Infor.JcSbName = DBConvert.ToString(dr["strJcSbName"]);
                                _Infor.Model = DBConvert.ToString(dr["strModel"]);
                                _Infor.Serial_number = DBConvert.ToString(dr["strSerial_number"]);
                                _Infor.Testblock = DBConvert.ToString(dr["strTestblock"]);
                                _Infor.Test_Proportion = DBConvert.ToString(dr["strTest_Proportion"]);
                                _Infor.Testing_Standard = DBConvert.ToString(dr["strTesting_Standard"]);
                                _Infor.Process_Doc_Number = DBConvert.ToString(dr["strProcess_Doc_Number"]);

                                _Infor.strJLdw = DBConvert.ToString(dr["strJLdw"]);
                                _Infor.strZzdw = DBConvert.ToString(dr["strZzdw"]);

                                _Infor.strBgbh = DBConvert.ToString(dr["strBgbh"]);
                                _Infor.strJyy = DBConvert.ToString(dr["strJyy"]);
                                _Infor.strHyy = DBConvert.ToString(dr["strHyy"]);
                                _Infor.strSp = DBConvert.ToString(dr["strSp"]);

                                _Infor.strOther1 = DBConvert.ToString(dr["strOther_1"]);
                                _Infor.strOther2 = DBConvert.ToString(dr["strOther_2"]);
                                _Infor.iOther1 = DBConvert.ToInt32(dr["iOther_1"]);
                                _Infor.iOther2 = DBConvert.ToInt32(dr["iOther_2"]);
                                _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);
                                #endregion 字段赋值
                                _lst_Info.Add(_Infor);
                            }
                            catch (Exception E)
                            {

                            }
                        }
                    }
                    #endregion 1
                    break;
            }
            return _lst_Info;
        }
        /// <summary>
        /// 获得指定文件的记录
        /// </summary>
        /// <returns></returns>
        public Class_Test_Item GetData_One(string strSourcePathFileName)
        {
            string strOld_DT = "";
            Class_Test_Item _Infor = new Class_Test_Item();
            string strSQL = "SELECT * from " + _DbTableName +" Where 1=1";
            switch (_iDataBase_Type)
            {
                case 0:
                    #region 0
                    strOld_DT = OleDbHelper.DbConnString_DT;
                    OleDbHelper.DbConnString_DT = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strSourcePathFileName;//子表：铭牌信息表、距离厚度表、波形表

                    DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);
                
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //2 循环拿记录
                        foreach (DataRow dr in dt.Rows)
                        {
                            #region 字段赋值
                            _Infor.ID = DBConvert.ToString(dr["ID"]);
                            _Infor.Dwmc = DBConvert.ToString(dr["strDwmc"]);
                            _Infor.ItemName = DBConvert.ToString(dr["strItemName"]);
                            _Infor.Sbbh = DBConvert.ToString(dr["strSbbh"]);

                            _Infor.Surface_condition = DBConvert.ToString(dr["strSurface_condition"]);
                            _Infor.Material = DBConvert.ToString(dr["strMaterial"]);
                            _Infor.Weldingtype = DBConvert.ToString(dr["strWeldingtype"]);
                            _Infor.strPkxs = DBConvert.ToString(dr["strPkxs"]);

                            _Infor.JcSbName = DBConvert.ToString(dr["strJcSbName"]);
                            _Infor.Model = DBConvert.ToString(dr["strModel"]);
                            _Infor.Serial_number = DBConvert.ToString(dr["strSerial_number"]);
                            _Infor.Testblock = DBConvert.ToString(dr["strTestblock"]);
                            _Infor.Test_Proportion = DBConvert.ToString(dr["strTest_Proportion"]);
                            _Infor.Testing_Standard = DBConvert.ToString(dr["strTesting_Standard"]);
                            _Infor.Process_Doc_Number = DBConvert.ToString(dr["strProcess_Doc_Number"]);

                            _Infor.strJLdw = DBConvert.ToString(dr["strJLdw"]);
                            _Infor.strZzdw = DBConvert.ToString(dr["strZzdw"]);

                            _Infor.strBgbh = DBConvert.ToString(dr["strBgbh"]);
                            _Infor.strJyy = DBConvert.ToString(dr["strJyy"]);
                            _Infor.strHyy = DBConvert.ToString(dr["strHyy"]);
                            _Infor.strSp = DBConvert.ToString(dr["strSp"]);

                            _Infor.strOther1 = DBConvert.ToString(dr["strOther_1"]);
                            _Infor.strOther2 = DBConvert.ToString(dr["strOther_2"]);
                            _Infor.iOther1 = DBConvert.ToInt32(dr["iOther_1"]);
                            _Infor.iOther2 = DBConvert.ToInt32(dr["iOther_2"]);
                            _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);
                            #endregion 字段赋值
                            break;
                        }
                    }
                    #endregion 0
                    OleDbHelper.DbConnString_DT= strOld_DT;
                    break;
                case 1:
                    #region  1
                    if (SqlDbHelper.m_DataBase == "")
                        SqlDbHelper.Init();

                    strOld_DT = SqlDbHelper.DbConnString_DT;
                    DataTable dt_1 = SqlDbHelper.ExecuteDataTable(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                    if (dt_1 != null && dt_1.Rows.Count > 0)
                    {
                        //2 循环拿记录
                        foreach (DataRow dr in dt_1.Rows)
                        {
                            #region 字段赋值
                            _Infor.ID = DBConvert.ToString(dr["ID"]);
                            _Infor.Dwmc = DBConvert.ToString(dr["strDwmc"]);
                            _Infor.ItemName = DBConvert.ToString(dr["strItemName"]);
                            _Infor.Sbbh = DBConvert.ToString(dr["strSbbh"]);

                            _Infor.Surface_condition = DBConvert.ToString(dr["strSurface_condition"]);
                            _Infor.Material = DBConvert.ToString(dr["strMaterial"]);
                            _Infor.Weldingtype = DBConvert.ToString(dr["strWeldingtype"]);
                            _Infor.strPkxs = DBConvert.ToString(dr["strPkxs"]);

                            _Infor.JcSbName = DBConvert.ToString(dr["strJcSbName"]);
                            _Infor.Model = DBConvert.ToString(dr["strModel"]);
                            _Infor.Serial_number = DBConvert.ToString(dr["strSerial_number"]);
                            _Infor.Testblock = DBConvert.ToString(dr["strTestblock"]);
                            _Infor.Test_Proportion = DBConvert.ToString(dr["strTest_Proportion"]);
                            _Infor.Testing_Standard = DBConvert.ToString(dr["strTesting_Standard"]);
                            _Infor.Process_Doc_Number = DBConvert.ToString(dr["strProcess_Doc_Number"]);

                            _Infor.strJLdw = DBConvert.ToString(dr["strJLdw"]);
                            _Infor.strZzdw = DBConvert.ToString(dr["strZzdw"]);

                            _Infor.strBgbh = DBConvert.ToString(dr["strBgbh"]);
                            _Infor.strJyy = DBConvert.ToString(dr["strJyy"]);
                            _Infor.strHyy = DBConvert.ToString(dr["strHyy"]);
                            _Infor.strSp = DBConvert.ToString(dr["strSp"]);

                            _Infor.strOther1 = DBConvert.ToString(dr["strOther_1"]);
                            _Infor.strOther2 = DBConvert.ToString(dr["strOther_2"]);
                            _Infor.iOther1 = DBConvert.ToInt32(dr["iOther_1"]);
                            _Infor.iOther2 = DBConvert.ToInt32(dr["iOther_2"]);
                            _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);
                            #endregion 字段赋值
                            break;
                        }
                    }
                    #endregion 1 
                    SqlDbHelper.DbConnString_DT= strOld_DT;
                    break;
            }
            return _Infor;
        }
        /// <summary>
        /// 获得字段值集合
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public List<string > GetData_ByMySel(string strName)
        {
            List<string> _lst_Info = new List<string>();
            string strSQL = "SELECT DISTINCT " + strName + " from " + _DbTableName;
            string strDat = "";

            switch (_iDataBase_Type)
            {
                case 0:
                    #region 0
                    DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //2 循环拿记录
                        foreach (DataRow dr in dt.Rows)
                        {
                            #region 字段赋值
                            strDat = DBConvert.ToString(dr[strName]);
                            #endregion 字段赋值
                            _lst_Info.Add(strDat );
                        }
                    }
                    #endregion 0
                    break;
                case 1:
                    #region  1
                    if (SqlDbHelper.m_DataBase == "")
                        SqlDbHelper.Init();

                    DataTable dt_1 = SqlDbHelper.ExecuteDataTable(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                    if (dt_1 != null && dt_1.Rows.Count > 0)
                    {
                        //2 循环拿记录
                        foreach (DataRow dr in dt_1.Rows)
                        {
                            #region 字段赋值
                            strDat = DBConvert.ToString(dr[strName]);
                            #endregion 字段赋值
                            _lst_Info.Add(strDat);
                        }
                    }
                    #endregion 1
                    break;
            }
            return _lst_Info;
        }
        /// <summary>
        /// 获得字段值集合
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public List<string> GetData_ByMySel_Where(string strName,string strZdName,string strVal)
        {
            List<string> _lst_Info = new List<string>();
            string strSQL = "SELECT DISTINCT " + strName + " from " + _DbTableName + " where " + strZdName + " = '"+ strVal +"'";
            string strDat = "";

            switch (_iDataBase_Type)
            {
                case 0:
                    #region 0
                    DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //2 循环拿记录
                        foreach (DataRow dr in dt.Rows)
                        {
                            #region 字段赋值
                            strDat = DBConvert.ToString(dr[strName]);
                            #endregion 字段赋值
                            _lst_Info.Add(strDat);
                        }
                    }
                    #endregion 0
                    break;
                case 1:
                    #region  1
                    if (SqlDbHelper.m_DataBase == "")
                        SqlDbHelper.Init();

                    DataTable dt_1 = SqlDbHelper.ExecuteDataTable(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                    if (dt_1 != null && dt_1.Rows.Count > 0)
                    {
                        //2 循环拿记录
                        foreach (DataRow dr in dt_1.Rows)
                        {
                            #region 字段赋值
                            strDat = DBConvert.ToString(dr[strName]);
                            #endregion 字段赋值
                            _lst_Info.Add(strDat);
                        }
                    }
                    #endregion 1
                    break;
            }
            return _lst_Info;
        }
    }
}
