/*
 * Copyright(C) 2 2017 郑州金润高科电子有限公司
 * 文件名: Wave.cs
 * 文件功能描述: 原始波形数据操作
 * 目的：操作深度对应原始波形数据库文件
 * 创建标识: 陈大伟 2017-1
 * 修改标识: 
 * 修改描述:
 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;//数据库
using System.Data.OleDb;//数据库类

using FrameWork.Struct;//类文件
using FrameWork.DataBaseMang.DBUtility;//操作数据库

namespace FrameWork.DataBaseMang.OleDal
{
    /// <summary>
    /// 波形表
    /// </summary>
    public class Manage_Wave
    {
        /// <summary>
        /// 操作的数据表名称 DeepThick
        /// </summary>
        private static string _DbTableName = "Wave_New";
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

        #region 方法
        #region 数据表创建
        public bool Creat_Table()
        {
            bool _blRet = false;
            string strSQL = "", strFileName = ""; ;
            try
            {
                #region 字段表
                //strFileName = "  ID varchar(16)  NOT NULL," +
                //            "Deep    float NOT NULL," +
                //            "iDown tinyint  NOT NULL," +

                strFileName = "ID varchar(20)     NOT NULL," +//ID
                            " flDistance_X    float  NOT NULL , " +//X轴方向距离
                            " flDistance_Y    float  NOT NULL , " +//Y轴方向距离
                             "Buff_iRows " + (_iDataBase_Type == 0 ? "Integer" : "tinyint") + " not null, "+//行号：数据库对应的行
                             "Chn_All text  NULL";

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
            { System.Windows.Forms.MessageBox.Show("Manage_Wave.cs的 Creat_Table error：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
        #endregion 数据表创建
        /// <summary>
        /// 删除临时数据
        /// </summary>
        /// <returns></returns>
        public bool DeleteDat()
        {
            string strSQL = "";
            bool _blRet = false;
            try
            {
             //   strSQL = "DELETE FROM  " + _DbTableName;
                strSQL = "drop  table  " + _DbTableName;
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
            { System.Windows.Forms.MessageBox.Show("Manage_Wave.cs DeleteDaterror：" + e.Message + e.StackTrace + " " + strSQL); }
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
            try//Truncate
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
            { System.Windows.Forms.MessageBox.Show("DistanceThick.cs的DeleteDat_ForIDerror：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
        #endregion 方法
    }
}
