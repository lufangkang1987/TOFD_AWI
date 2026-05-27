/*
 * Copyright(C) 2 2017 郑州金润高科电子有限公司
 * 文件名: User.cs
 * 文件功能描述: 用户信息数据操作
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

using FrameWork. Struct;//类文件
using FrameWork.DataBaseMang.DBUtility;//操作数据库
namespace FrameWork.DataBaseMang.OleDal
{
    public class Manage_User
    {
        /// <summary>
        /// 操作的数据表名称
        /// </summary>
        private static string _DbTableName = "CngUser";
        /// <summary>
        /// 指定数据时的操作表名称
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
        #region 数据表创建
        public bool Creat_Table()
        {
            bool _blRet = false;
            string strSQL = "", strFileName = ""; ;
            try
            {
                #region 字段表
                strFileName = "  Name varchar(100)     NOT NULL," +
                             " Pass    varchar(20)  NOT NULL, " +
                             "strLevel    varchar(5)   NOT NULL " ;
                #endregion 字段表

                strSQL = "CREATE Table " + _DbTableName + "( " + strFileName + " )";
                switch (_iDataBase_Type)
                {
                    case 0:
           //OleDbHelper.DbConnString_DT += " ;Jet OLEDB:Engine Type=5";
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
            { System.Windows.Forms.MessageBox.Show("CngInfo.cs的DeleteDaterror：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
        #endregion 数据表创建
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
            { System.Windows.Forms.MessageBox.Show("User.cs的DeleteDaterror：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="iDown"></param>
        /// <returns></returns>
        public bool DeleteDat_ForName(string strName)
        {
            bool _blRet = false;//操作返回
            string strSQL = "DELETE   from  " + _DbTableName + "  Where Name='" + strName + "'";
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
            { System.Windows.Forms.MessageBox.Show("User.cs的 DeleteDat_ForName error：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="strFileName">字段名</param>
        /// <param name="strVal">字段值</param>
        /// <param name="intStringFloatInt">字段类型 0：字符串 1：整形 2：float型</param>
        /// <param name="blFalseTrue">值的性质：值相同还是不同</param>
        /// <returns></returns>
        public bool DeleteDat(string strFileName, string strVal, int intStringFloatInt, bool blFalseTrue)
        {
            string strSQL = "";
            bool _blRet = false;
            try
            {
                if (blFalseTrue)
                {
                    switch (intStringFloatInt)
                    {
                        case 0:
                            strSQL = "DELETE FROM " + _DbTableName + "  where  strFileName ='" + strVal + "'"; break;
                        case 1:
                            strSQL = "DELETE FROM " + _DbTableName + "  where  strFileName =" + int.Parse(strVal); break;
                        case 2:
                            strSQL = "DELETE FROM " + _DbTableName + "  where  strFileName =" + float.Parse(strVal); break;
                    }
                }
                else
                {
                    switch (intStringFloatInt)
                    {
                        case 0:
                            strSQL = "DELETE FROM " + _DbTableName + "  where  strFileName !='" + strVal + "'"; break;
                        case 1:
                            strSQL = "DELETE FROM " + _DbTableName + "  where  strFileName !==" + int.Parse(strVal); break;
                        case 2:
                            strSQL = "DELETE FROM " + _DbTableName + "  where  strFileName !==" + float.Parse(strVal); break;
                    }
                }
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
            { System.Windows.Forms.MessageBox.Show("User.cs的DeleteDaterror：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
      
        /// <summary>
        /// 保存数据到临时库 0:失败 1：覆盖 2：插入新的
        /// </summary>
        /// <param name="_Infor"></param>
        /// <returns>0:失败 1：覆盖 2：插入新的</returns>
        public int SaveData(ref Class_User _User)
        {
            int iRet = 0;
            string strSQL = "";
            string strAdd_Zd = "";//增加Sql字段
            string strAdd_Val = "";//增加Sql值

            try
            {
                //1 查询是否有深度对应的数据，有就更新，没有插入
                strSQL = "SELECT * from " + _DbTableName + " where Name= '" + _User.Name  +  "'";
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
                                strAdd_Zd = " SET Name='" + _User.Name + "',";
                                strAdd_Zd += " Pass='" + _User.Pass + "',";
                                strAdd_Zd += "strLevel='" + _User.strLevel + "'";

                                if (_DbTableName != "" && strAdd_Zd != "")
                                {
                                    //2.3 添加到数据库
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
                            strAdd_Zd = "Name,Pass,strLevel";
                       
                            strAdd_Val = "'" + _User .Name  + "'";
                            strAdd_Val += ",'" + _User.Pass + "'";
                            strAdd_Val += ",'" + _User.strLevel + "'";
                         
                            strSQL = "insert into " + _DbTableName + "  (" + strAdd_Zd + ") values(" + strAdd_Val + ")";
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
                            strAdd_Zd = " SET Name='" + _User.Name+"',";
                            strAdd_Zd += " Pass='" + _User.Pass + "',";
                            strAdd_Zd += "strLevel='" + _User.strLevel + "'";

                            if (_DbTableName != "" && strAdd_Zd != "")
                            {
                                //2.3 添加到数据库
                                strSQL = "Update " + _DbTableName + " " + strAdd_Zd + " Where  Name= '" + _User.Name;
                                if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                    iRet = 0;
                                else
                                    if (iRet != 2)
                                    iRet = 1;
                            }
                        }
                        else
                        {
                            //3 插入数据
                            strAdd_Zd = "Name,Pass,strLevel";

                            strAdd_Val = "'" + _User.Name + "'";
                            strAdd_Val += ",'" + _User.Pass + "'";
                            strAdd_Val += ",'" + _User.strLevel + "'";

                            strSQL = "insert into " + _DbTableName + "  (" + strAdd_Zd + ") values(" + strAdd_Val + ")";
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
            { System.Windows.Forms.MessageBox.Show("User.cs的SaveDataerror：" + e.Message + e.StackTrace + " " + strSQL); }

            return iRet;
        }
        /// <summary>
        /// 获得临时库指定井号数据
        /// </summary>
        /// <param name="_Infor"></param>
        /// <returns></returns>
        public bool GetData(ref Class_User  _User)
        {
            bool _blRet = false;
            string strSQL = "";
            try
            {
                //1 查询临时库中报警数据
                strSQL = "SELECT * from " + _DbTableName + " where Name= '" + _User.Name  + "' ";
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
                                _User.Name = DBConvert.ToString(dr["Name"]);
                                _User.Pass  = DBConvert.ToString(dr["Pass"]);
                                _User.strLevel  = DBConvert.ToString(dr["strLevel"]);
                               
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
                                _User.Name = DBConvert.ToString(dr["Name"]);
                                _User.Pass = DBConvert.ToString(dr["Pass"]);
                                _User.strLevel = DBConvert.ToString(dr["strLevel"]);

                                _blRet = true;
                                break;
                            }
                        }
                        #endregion 1
                        break;
                }
            }
            catch (Exception e)
            { System.Windows.Forms.MessageBox.Show("User.cs的GetData：" + e.Message + e.StackTrace + " " + strSQL); }

            return _blRet;
        }
        /// <summary>
        /// 获得所有数据，按照名称排序
        /// </summary>
        /// <returns></returns>
        public List <Class_User> GetData_All()
        {
            string strSQL = "";
            List<Class_User> _Ret_lstUser = new List<Struct.Class_User>();
            try
            {
                //1 查询临时库中报警数据
                strSQL = "SELECT * from " + _DbTableName + " where 1=1  order by Name";
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
                                Class_User _User = new Struct.Class_User();
                                _User.Name = DBConvert.ToString(dr["Name"]);
                                _User.Pass = DBConvert.ToString(dr["Pass"]);
                                _User.strLevel = DBConvert.ToString(dr["strLevel"]);

                                _Ret_lstUser.Add(_User);
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
                                Class_User _User = new Struct.Class_User();
                                _User.Name = DBConvert.ToString(dr["Name"]);
                                _User.Pass = DBConvert.ToString(dr["Pass"]);
                                _User.strLevel = DBConvert.ToString(dr["strLevel"]);

                                _Ret_lstUser.Add(_User);
                            }
                        }
                        #endregion 1
                        break;
                }
            }
            catch (Exception e)
            { System.Windows.Forms.MessageBox.Show("User.cs的 GetData_All：" + e.Message + e.StackTrace + " " + strSQL); }

            return _Ret_lstUser;
        }
        /// <summary>
        /// 获得指定字段对应所有数据，数据之间以“@”间隔
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
            { System.Windows.Forms.MessageBox.Show("User.cs的GetDat：" + e.Message + e.StackTrace + " " + strSQL); }

            return _strRet;
        }
    }
}

