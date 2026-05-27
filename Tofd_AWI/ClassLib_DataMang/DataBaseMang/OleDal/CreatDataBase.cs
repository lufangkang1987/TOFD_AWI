/*
 * Copyright(C) 2 2017 郑州金润高科电子有限公司
 * 文件名: CreatDataBase.cs
 * 文件功能描述: 创建数据库操作
 * 目的：创建数据库文件
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

using ClassLib_DataMang.DataBaseMang.DBUtility;//操作数据库
using ADOX;

namespace ClassLib_DataMang.DataBaseMang.OleDal
{
    public class CreatDataBase
    {
        /// <summary>
        /// 指定操作数据库类型  0：Access  1：Sel Server
        /// </summary>
        private static int _iDataBase_Type = 1;
        /// <summary>
        /// 指定操作数据库类型 0：Access 1：Sel Server
        /// </summary>
        public int m_iDataBase_Type
        {
            get { return _iDataBase_Type; }
            set { _iDataBase_Type = value; }
        }

        /// <summary>
        /// 子表数据库名称:用户名 + 受检设备名 + 地址 + 检定时间
        /// </summary>
        private static string _strSubDataBaseName = "";
        /// <summary>
        /// 子表数据库名称:用户名 + 受检设备名 + 地址 + 检定时间
        /// </summary>
        public string m_strSubDataBaseName
        {
            get { return _strSubDataBaseName; }
            set { _strSubDataBaseName = value; }
        }
        #region 数据库创建
        public void SetSubDataBaseName(string strName)
        {
            OleDbHelper.strSubDataBaseName = strName + (_iDataBase_Type == 0 ? ".mdb" : ".mdf");//   ".mdb";
        }
        /// <summary>
        /// 获得主表文件路径
        /// </summary>
        /// <param name="iType">0:主表  1：次表</param>
        /// <returns></returns>
        public string GetstrDataBaseName(int iType)
        {
            if (iType == 0)
            {
                return  @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                                OleDbHelper.strDbPath + "\\" + OleDbHelper.strDataBaseName; }
            else
            {
                return @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                               OleDbHelper.strDb_SubDbPath + "\\" + OleDbHelper.strSubDataBaseName;
            }
        }

        /// <summary>
        /// 判断数据库是否存在
        /// 主表：判断用户名表的用户名字段是否存在
        /// 子表：判断指定路径下文件名的厚度表字Y轴字段是否存在
        /// </summary>
        /// <returns></returns>
        public bool Jg_DataBase( ref string  strConn , bool blMainSub_DataBase = false, string strSubDataBaseName = "")
        {
            bool _blRet = false;
            string strSQL = "";
           
            try
            {
                if (blMainSub_DataBase)
                    strSQL = "select * From CngUser where name = '" + "Dellon" + "'";//主表
                else
                    strSQL = "select * From " + "DistanceThick" + "_" + strSubDataBaseName;//主表
                switch (_iDataBase_Type)
                {
                    case 0:
                        OleDbHelper.blMainSub_DataBase = blMainSub_DataBase;

                        if (OleDbHelper.blMainSub_DataBase == false)//子表
                        {
                            _strSubDataBaseName = strSubDataBaseName;
                            if (_strSubDataBaseName == "")
                                _strSubDataBaseName = DateTime.Now.ToString("yyMMdd_HHmmss");
                            //if (_strSubDataBaseName.ToUpper().IndexOf(".MDB") == -1)
                            //    _strSubDataBaseName += (_iDataBase_Type == 0 ? ".mdb" : ".mdf");
                            //if (_strSubDataBaseName.ToUpper().IndexOf(".MDF") == -1)
                            //    _strSubDataBaseName += (_iDataBase_Type == 0 ? ".mdb" : ".mdf");

                            if (_iDataBase_Type == 0 && _strSubDataBaseName.ToUpper().IndexOf(".MDB") == -1)
                                _strSubDataBaseName += (_iDataBase_Type == 0 ? ".mdb" : ".mdf");

                            if (_iDataBase_Type == 1 && _strSubDataBaseName.ToUpper().IndexOf(".MDF") == -1)
                                _strSubDataBaseName += (_iDataBase_Type == 0 ? ".mdb" : ".mdf");

                            OleDbHelper.strSubDataBaseName = _strSubDataBaseName;
                            strSQL = "select * From Test_Item_Info ";
                        }
                        AccessPath();
                        if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) != 0)
                            _blRet = true;
                        else
                            _blRet = false;
                        strConn = OleDbHelper.DbConnString_DT;
                        break;
                    case 1:
                        if (SqlDbHelper.m_DataBase == "")
                            SqlDbHelper.Init();
                        if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) != 0)
                            _blRet = true;
                        strConn = SqlDbHelper.DbConnString_DT;
                        break;
                }
            }
            catch (Exception e)
            { System.Windows.Forms.MessageBox.Show("CreatDataBase.cs Jg_DataBase：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
        /// <summary>
        /// Access数据库路径
        /// </summary>
        private void AccessPath()
        {
            if (_iDataBase_Type == 0)
            {
                if (OleDbHelper.blMainSub_DataBase)//主表
                {
                    if (System.IO.Directory.Exists(OleDbHelper.strDbPath) == false)
                    {
                        System.IO.Directory.CreateDirectory(OleDbHelper.strDbPath);
                    }
                    OleDbHelper.DbConnString_DT = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                                OleDbHelper.strDbPath + "\\" + OleDbHelper.strDataBaseName;
                }
                else//子表
                {
                    if (System.IO.Directory.Exists(OleDbHelper.strDb_SubDbPath) == false)
                    {
                        System.IO.Directory.CreateDirectory(OleDbHelper.strDb_SubDbPath);
                    }

                    OleDbHelper.DbConnString_DT = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        OleDbHelper.strDb_SubDbPath + "\\" + OleDbHelper.strSubDataBaseName;
                }
            }
        }
        /// <summary>
        /// 获得子文件路径
        /// </summary>
        /// <returns></returns>
        public string GetSubPath()
        {
            return @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        OleDbHelper.strDb_SubDbPath + "\\";
        }
        private void AccessTmpPath()
        {
            if (_iDataBase_Type == 0)
            {
                if (OleDbHelper.blMainSub_DataBase)//主表
                {
                    if (System.IO.Directory.Exists(OleDbHelper.strDb_TmpDbPath) == false)
                    {
                        System.IO.Directory.CreateDirectory(OleDbHelper.strDb_TmpDbPath);
                    }
                    OleDbHelper.DbConnString_DT = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                                OleDbHelper.strDb_TmpDbPath + "\\" + OleDbHelper.strDataBaseName;
                }
                else//子表
                {
                    if (System.IO.Directory.Exists(OleDbHelper.strDb_TmpDbPath) == false)
                    {
                        System.IO.Directory.CreateDirectory(OleDbHelper.strDb_TmpDbPath);
                    }

                    OleDbHelper.DbConnString_DT = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        OleDbHelper.strDb_TmpDbPath + "\\" + OleDbHelper.strSubDataBaseName;
                }
            }
        }
        public void AccessTmp_FileName(string strName)
        {
            switch (_iDataBase_Type)
            {
                case 0:
                 //   OleDbHelper.strDataBaseName = strName;
                    OleDbHelper.DbConnString_DT = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                              OleDbHelper.strDb_TmpDbPath + "\\" + strName;
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strConn"></param>
        /// <param name="blMainSub_DataBase"></param>
        /// <param name="strSubDataBaseName">Tmp_1-->Tmp_10</param>
        /// <returns></returns>
        public bool Jg_Tmp_DataBase(ref string strConn, bool blMainSub_DataBase = false, string strSubDataBaseName = "")
        {
            bool _blRet = false;
            string strSQL = "";
            try
            {
                strSQL = "select * From CngUser where name = '" + "Dellon" + "'";//主表
                switch (_iDataBase_Type)
                {
                    case 0:
                        OleDbHelper.blMainSub_DataBase = blMainSub_DataBase;

                        if (OleDbHelper.blMainSub_DataBase == false)//子表
                        {
                            _strSubDataBaseName = strSubDataBaseName;
                            if (_strSubDataBaseName == "")
                                _strSubDataBaseName = DateTime.Now.ToString("yyMMdd_HHmmss");
                            //if (_strSubDataBaseName.ToUpper().IndexOf(".MDB") == -1)
                            //    _strSubDataBaseName += (_iDataBase_Type == 0 ? ".mdb" : ".mdf");
                            //if (_strSubDataBaseName.ToUpper().IndexOf(".MDF") == -1)
                            //    _strSubDataBaseName += (_iDataBase_Type == 0 ? ".mdb" : ".mdf");
                            if (_iDataBase_Type == 0 && _strSubDataBaseName.ToUpper().IndexOf(".MDB") == -1)
                                _strSubDataBaseName += (_iDataBase_Type == 0 ? ".mdb" : ".mdf");

                            if (_iDataBase_Type == 1 && _strSubDataBaseName.ToUpper().IndexOf(".MDF") == -1)
                                _strSubDataBaseName += (_iDataBase_Type == 0 ? ".mdb" : ".mdf");

                            OleDbHelper.strSubDataBaseName = _strSubDataBaseName;
                            strSQL = "select * From DistanceThick ";
                        }
                        AccessTmpPath();
                        if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) != 0)
                            _blRet = true;
                        else
                            _blRet = false;
                        strConn = OleDbHelper.DbConnString_DT;
                        break;
                    case 1:
                        if (SqlDbHelper.m_DataBase == "")
                            SqlDbHelper.Init();
                        if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) != 0)
                            _blRet = true;
                        strConn = SqlDbHelper.DbConnString_DT;
                        break;
                }
            }
            catch (Exception e)
            { System.Windows.Forms.MessageBox.Show("CreatDataBase.cs的Jg_DataBase：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
        /// <summary>
        /// 设置子表的数据库名称 true:主表 false:子表 / 子表的数据库名称
        /// </summary>
        /// <param name="blMainSub_DataBase">true:主表 false:子表</param>
        /// <param name="strSubDataBaseName">子表的数据库名称</param>
        public void Set_blMainSub_DataBase(bool blMainSub_DataBase = false, string strSubDataBaseName = "")
        {
            OleDbHelper.blMainSub_DataBase = blMainSub_DataBase;
            _strSubDataBaseName = strSubDataBaseName;
            switch (_iDataBase_Type)
            {
                case 0:
                    if (_strSubDataBaseName == "")
                        _strSubDataBaseName = DateTime.Now.ToString("yyMMdd_HHmmss");

                    if (_iDataBase_Type==0 &&  _strSubDataBaseName.ToUpper().IndexOf(".MDB") == -1)
                        _strSubDataBaseName += (_iDataBase_Type == 0 ? ".mdb" : ".mdf");
            
                    if (_iDataBase_Type==1&& _strSubDataBaseName.ToUpper().IndexOf(".MDF") == -1)
                        _strSubDataBaseName += (_iDataBase_Type == 0 ? ".mdb" : ".mdf");
                    if (blMainSub_DataBase == false)
                        OleDbHelper.strSubDataBaseName = _strSubDataBaseName;

                    OleDbHelper.blMainSub_DataBase = blMainSub_DataBase;
                    AccessPath();
                    break;
                case 1:
                    if (blMainSub_DataBase == false)
                    {
                     //   DbGlobal.ImDistanceThick.DbTableName = "DistanceThick" + "_" + strSubDataBaseName;
                    }
                    break;
            }
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="blMainSub_DataBase">true:主表 false:子表</param>
        /// <returns></returns>
        public bool Creat_DataBase(bool blMainSub_DataBase)
        {
            bool _blRet = false;
            string strSQL = "";
            try
            {
                switch (_iDataBase_Type)
                {
                    case 0:
                        OleDbHelper.blMainSub_DataBase = blMainSub_DataBase;
                        AccessPath();

                        ADOX.Catalog cat = new ADOX.Catalog();
                        cat.Create(OleDbHelper.DbConnString_DT + " ;Jet OLEDB:Engine Type=5");

                        _blRet = true;
                        break;
                    case 1:
                        SqlDbHelper.Init();
                        strSQL = " CREATE DATABASE " + SqlDbHelper.m_DataBase;
                      SqlDbHelper.Init_Creat();
                        if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                            _blRet = false;
                        else
                            _blRet = true;
                        SqlDbHelper.m_DataBase = "";
                        break;
                }
            }
            catch (Exception e)
            {
                _blRet = false;
                System.Windows.Forms.MessageBox.Show("CreatDataBase.cs的Creat_DataBase：" + e.Message + e.StackTrace + " " + strSQL);
            }
            return _blRet;
        }


        public bool Add_DataBase(string s1,string s2)
        {
            bool _blRet = false;

            //ADOX.Catalog.Execute "insert into [;database=" & App.Path & "\工作日志.mdb;pwd=密码].项目表" _
            //          & " select * from 项目表" _
            //          & " where 项目名称 not in (select 项目名称 from [;database=" & App.Path & "\工作日志.mdb;pwd=密码].项目表)"
            return _blRet;
        }

        public bool Creat_TmpDataBase(bool blMainSub_DataBase)
        {
            bool _blRet = false;
            string strSQL = "";
            try
            {
                switch (_iDataBase_Type)
                {
                    case 0:
                        OleDbHelper.blMainSub_DataBase = blMainSub_DataBase;
                        AccessTmpPath();

                        ADOX.Catalog cat = new ADOX.Catalog();
                        cat.Create(OleDbHelper.DbConnString_DT + " ;Jet OLEDB:Engine Type=5");
                      
                        _blRet = true;
                        break;
                    case 1:
                        strSQL = " CREATE DATABASE " + SqlDbHelper.m_DataBase;
                        SqlDbHelper.Init_Creat();
                        if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                            _blRet = false;
                        else
                            _blRet = true;
                        SqlDbHelper.m_DataBase = "";
                        break;
                }
            }
            catch (Exception e)
            {
                _blRet = false;
                System.Windows.Forms.MessageBox.Show("CreatDataBase.cs的Creat_DataBase：" + e.Message + e.StackTrace + " " + strSQL);
            }
            return _blRet;
        }
        #endregion 数据库创建
    }
}
