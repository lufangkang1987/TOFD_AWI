/*
 * Copyright(C) 2 2017 河南德朗智能科技有限公司
 * 文件名: AlarmData_UT.cs
 * 文件功能描述: TOFD异常信息数据操作
 * 目的：保存测量过程异常报警信息数据库文件
 * 创建标识: 陈大伟 20210819
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
     public class Test_Alarm
    {

        /// <summary>
        /// 操作的数据表名称
        /// </summary>
        private static string _DbTableName = "ScreenAlarmData";
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


        /// <summary>
        /// 创建屏幕超限的最大最小值数据表
        /// </summary>
        /// <returns></returns>
        public bool Creat_Table()
        {
            bool _blRet = false;
            string strSQL = "", strFileName = "";
            try
            {
                #region 字段表
                strFileName = "ID varchar(20)     NOT NULL," +//ID   iQidian_L_R
                             " Sub_ID varchar(20)     NOT NULL," +//焊缝ID号

                             " Part_No varchar(100)    ," +//部位编号
                             " strType varchar(80)     ," +//异常缺陷类型

                             " flLen_S  float  , " +//X轴开始位置
                             " flLen_E  float ,  " +//X轴结束位置
                             " flLen    float," +//长度
                             (_iDataBase_Type == 0 ? " iX_No    Integer  NULL , " : "iX_No tinyint  NULL, ") +//D图X轴位置序号
                             (_iDataBase_Type == 0 ? " iY_No    Integer  NULL , " : "iY_No tinyint  NULL, ") +//D图Y轴位置序号
                             " flHeight_S  float  , " +//高度开始时间
                             " flHeight_E    float ,  " +//高度结束时间
                             " flHeight    float ,  " +//高度

                              " flDepth_S    float ,  " +//深度开始时间
                              " flDepth    float ";//深度
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
            { System.Windows.Forms.MessageBox.Show("AlarmData_UT.cs  Creat_Table error：" + e.Message + e.StackTrace + " " + strSQL); }
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
                strSQL = "drop  table " + _DbTableName;
                // strSQL = "delete FROM " + _DbTableName;
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
            { System.Windows.Forms.MessageBox.Show("AlarmData_UT.cs的DeleteDaterror：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
        /// <summary>
        /// 删除指定ID数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="iDown"></param>
        /// <returns></returns>
        public bool DeleteDat_ForID(string ID,string Sub_ID,float flLen_S)
        {
            bool _blRet = false;//操作返回
            string strSQL = "DELETE   from  " + _DbTableName ;
            string    strWhere = " where ID = '" + ID + "'" +
                              " and Sub_ID = '" + Sub_ID + "'" +
                             "  and flLen_S=" + flLen_S;
            strSQL += strWhere;
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
            { System.Windows.Forms.MessageBox.Show("AlarmData_UT.cs的DeleteDat_ForIDerror：" + e.Message + e.StackTrace + " " + strSQL); }
            return _blRet;
        }
        /// <summary>
        /// 删除数据表
        /// </summary>
        /// <param name="表附加名称"></param>
        /// <returns></returns>
        public bool DropDat_ForName(string strSaveDataFileName)
        {
            bool _blRet = false;//操作返回
            string strSQL = "drop table  " + _DbTableName + "_" + strSaveDataFileName;
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
            {
                //      System.Windows.Forms.MessageBox.Show("AlarmData_UT.cs的DeleteDat_ForID出错：" + e.Message + e.StackTrace + " " + strSQL); 
            }
            return _blRet;
        }
        /// <summary>
        /// 保存数据到临时库 0:失败 1：覆盖 2：插入新的
        /// </summary>
        /// <param name="DataCol"></param>
        /// <returns>0:失败 1：覆盖 2：插入新的</returns>
        public int SaveData( Class_Test_AlarmArea DataCol)
        {
            int iRet = 0;
            string strSQL = "";
            string strAdd_Val = "";//增加Sql值
            string strInsert_Add_Zd = "";//插入字段表 9+8、=-1·   
            string strUpdata_Add_Zd = "";//更新字符串
            string strWhere = "";//条件
            try
            {
                //1 查询
                //单位名称  受检设备名称  设备地址
                if (DataCol.ID == "" || DataCol.Sub_ID == "") return 0;
               
                strSQL = "SELECT * from " + _DbTableName;
                strWhere = " where ID = '" + DataCol.ID + "'" +
                            " and Sub_ID = '" + DataCol.Sub_ID + "'" +
                           "  and flLen_S=" + DataCol.flLen_S;

                strSQL += strWhere;
                #region 插入字符串
                strInsert_Add_Zd = "ID,Sub_ID,Part_No,strType," +
                                   "flLen_S,flLen_E,flLen,iX_No," +
                                   "iY_No, flHeight_S,flHeight_E,flHeight,"+
                                   "flDepth_S,flDepth ";

                strAdd_Val = "'" + DataCol.ID + "'";
                strAdd_Val += ",'" + DataCol.Sub_ID + "'";
                strAdd_Val += ",'" + DataCol.Part_No + "'";

                strAdd_Val += ",'" + DataCol.strType + "'";

                strAdd_Val += "," + DataCol.flLen_S;
                strAdd_Val += "," + DataCol.flLen_E ;
                strAdd_Val += "," + DataCol.flLen;
               
                strAdd_Val += "," + DataCol.iX_No;
                strAdd_Val += "," + DataCol.iY_No;

                strAdd_Val += "," + DataCol.flHeight_S;
                strAdd_Val += "," + DataCol.flHeight_E;
                strAdd_Val += "," + DataCol.flHeight;

                strAdd_Val += "," + DataCol.flDepth_S;
                strAdd_Val += "," + DataCol.flDepth;

                #endregion 插入字符串

                #region 更新字符串

                //3 插入数据
                strUpdata_Add_Zd = "SET ID='" + DataCol.ID + "'";
                strUpdata_Add_Zd += ",Sub_ID='" + DataCol.Sub_ID + "'";
                strUpdata_Add_Zd += ",Part_No='" + DataCol.Part_No + "'";

                strUpdata_Add_Zd += ",strType='" + DataCol.strType + "'";

                strUpdata_Add_Zd += ",flLen_S=" + DataCol.flLen_S;
                strUpdata_Add_Zd += ",flLen_E=" + DataCol.flLen_E;
                strUpdata_Add_Zd += ", flLen=" + DataCol.flLen;
              
                strUpdata_Add_Zd += ",iX_No=" + DataCol.iX_No;
                strUpdata_Add_Zd += ",iY_No=" + DataCol.iY_No;
             
                strUpdata_Add_Zd += ", flHeight_S=" + DataCol.flHeight_S;
                strUpdata_Add_Zd += ", flHeight_E=" + DataCol.flHeight_E;
                strUpdata_Add_Zd += ", flHeight=" + DataCol.flHeight;

                strUpdata_Add_Zd += ", flDepth_S=" + DataCol.flDepth_S;
                strUpdata_Add_Zd += ", flDepth=" + DataCol.flDepth;

                #endregion

                switch (_iDataBase_Type)
                {
                    case 0:
                        #region 0
                        try
                        {
                            string strRet = "";
                            OleDbDataReader _Read = OleDbHelper.ExecuteReader(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null);
                            if (_Read != null)
                            {
                                while (_Read.Read())
                                {
                                    strRet = (DBConvert.ToString(_Read["ID"]));
                                    break;
                                }
                                _Read.Close();
                            }

                            if (strRet != "")
                            {
                                //2 更新数据 
                                if (_DbTableName != "" && strUpdata_Add_Zd != "")
                                {
                                    //2.3 添加到数据库
                                    strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + strWhere;

                                    if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                        iRet = 0;
                                    else
                                        iRet = 1;
                                }
                                return iRet;
                            }
                        }
                        catch (Exception e)
                        { }
                        strSQL = "insert into " + _DbTableName + "  (" + strInsert_Add_Zd + ") values(" + strAdd_Val + ")";// + strWhere ;
                        if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                            iRet = 0;
                        else
                            iRet = 2;
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
                                strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + strWhere;

                                if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                    iRet = 0;
                                else
                                    iRet = 1;
                            }
                        }
                        else
                        {
                            //3 插入数据
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
            { }
            return iRet;
        }
        /// <summary>
        /// 获得异常数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="iScreenNo"></param>
        /// <param name="iQiDian_L_R"></param>
        /// <returns></returns>
        public List<Class_Test_AlarmArea> GetData(string ID, string Sub_ID)
        {
            List<Class_Test_AlarmArea> _lst_Ret = new List<Class_Test_AlarmArea>();

            //1 查询指定ID数据，按照X轴、Y轴曾序排序
            string strSQL = "";
            if (Sub_ID != "")
                strSQL = "SELECT * from " + _DbTableName + " where ID='" + ID + "' and Sub_ID='" + Sub_ID + "' order by flLen_S";
            else
                strSQL = "SELECT * from " + _DbTableName + " where ID='" + ID + "' order by Sub_ID,flLen_S";

            switch (_iDataBase_Type)
            {
                case 0:
                    #region  0
                    try
                    {
                        DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            #region 拿数据
                            foreach (DataRow dr in dt.Rows)
                            {
                                Class_Test_AlarmArea _Ret_Data = new Class_Test_AlarmArea();
                                //2.1  将当前记录赋值给 

                                _Ret_Data.ID = DBConvert.ToString(dr["ID"]);
                                _Ret_Data.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                                _Ret_Data.Part_No = DBConvert.ToString(dr["Part_No"]);
                                _Ret_Data.strType = DBConvert.ToString(dr["strType"]);
                            
                                _Ret_Data.flLen_S = float.Parse(DBConvert.ToString(dr["flLen_S"]));
                                _Ret_Data.flLen_E = float.Parse(DBConvert.ToString(dr["flLen_E"]));
                                _Ret_Data.flLen = float.Parse(DBConvert.ToString(dr["flLen"]));
                              
                                _Ret_Data.iX_No = int.Parse(DBConvert.ToString(dr["iX_No"]));
                                _Ret_Data.iY_No = int.Parse(DBConvert.ToString(dr["iY_No"]));
                       
                                _Ret_Data.flHeight_S = float.Parse(DBConvert.ToString(dr["flHeight_S"]));
                                _Ret_Data.flHeight_E = float.Parse(DBConvert.ToString(dr["flHeight_E"]));
                                _Ret_Data.flHeight = float.Parse(DBConvert.ToString(dr["flHeight"]));

                                _Ret_Data.flDepth_S = float.Parse(DBConvert.ToString(dr["flDepth_S"]));
                                _Ret_Data.flDepth = float.Parse(DBConvert.ToString(dr["flDepth"]));

                                _lst_Ret.Add(_Ret_Data);
                                break;
                            }
                            #endregion  拿数据
                        }
                    }
                    catch (Exception e2)
                    { }
                    #endregion 0
                    break;
                case 1:
                    if (SqlDbHelper.m_DataBase == "")
                        SqlDbHelper.Init();
                    #region 1 
                    try
                    {
                        DataTable dt_1 = SqlDbHelper.ExecuteDataTable(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                        if (dt_1 != null && dt_1.Rows.Count > 0)
                        {
                            #region 拿数据
                            foreach (DataRow dr in dt_1.Rows)
                            {
                                //2.1  将当前记录赋值给 
                                Class_Test_AlarmArea _Ret_Data = new Class_Test_AlarmArea();
                                //2.1  将当前记录赋值给 

                                _Ret_Data.ID = DBConvert.ToString(dr["ID"]);
                                _Ret_Data.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                                _Ret_Data.Part_No = DBConvert.ToString(dr["Part_No"]);

                                _Ret_Data.strType = DBConvert.ToString(dr["strType"]);
                                try
                                {
                                    _Ret_Data.flLen_S = float.Parse(DBConvert.ToString(dr["flLen_S"]));
                                    _Ret_Data.flLen_E = float.Parse(DBConvert.ToString(dr["flLen_E"]));
                                    _Ret_Data.flLen = float.Parse(DBConvert.ToString(dr["flLen"]));
                                 
                                    _Ret_Data.iX_No = int.Parse(DBConvert.ToString(dr["iX_No"]));
                                    _Ret_Data.iY_No = int.Parse(DBConvert.ToString(dr["iY_No"]));
                                 
                                    _Ret_Data.flHeight_S = float.Parse(DBConvert.ToString(dr["flHeight_S"]));
                                    _Ret_Data.flHeight_E = float.Parse(DBConvert.ToString(dr["flHeight_E"]));
                                    _Ret_Data.flHeight = float.Parse(DBConvert.ToString(dr["flHeight"]));

                                    _Ret_Data.flDepth_S = float.Parse(DBConvert.ToString(dr["flDepth_S"]));
                                    _Ret_Data.flDepth = float.Parse(DBConvert.ToString(dr["flDepth"]));
                                }
                                catch (Exception e2)
                                { }
                                _lst_Ret.Add(_Ret_Data);
                            }
                            #endregion  拿数据
                        }
                    }
                    catch (Exception e3)
                    {

                    }
                    #endregion 1
                    break;
            }
            return _lst_Ret;
        }

        /// <summary>
        /// 获得异常数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="iScreenNo"></param>
        /// <param name="iQiDian_L_R"></param>
        /// <returns></returns>
        public List<Class_Test_AlarmArea> GetData(string ID, string Sub_ID,int iScreenNo,List<Class_Screen_Kd> lst_Screenkd)
        {
            List<Class_Test_AlarmArea> _lst_Ret = new List<Class_Test_AlarmArea>();

            //1 查询指定ID数据，按照X轴、Y轴曾序排序
            string strSQL = "";
            int iNo = 0;//屏幕序号
            if (Sub_ID != "")
                strSQL = "SELECT * from " + _DbTableName + " where ID='" + ID + "' and Sub_ID='" + Sub_ID + "' order by flLen_S";
            else
                strSQL = "SELECT * from " + _DbTableName + " where ID='" + ID + "' order by Sub_ID,flLen_S";

            switch (_iDataBase_Type)
            {
                case 0:
                    #region  0
                    try
                    {
                        DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            #region 拿数据
                            foreach (DataRow dr in dt.Rows)
                            {
                                Class_Test_AlarmArea _Ret_Data = new Class_Test_AlarmArea();
                                //2.1  将当前记录赋值给 

                                _Ret_Data.ID = DBConvert.ToString(dr["ID"]);
                                _Ret_Data.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                                _Ret_Data.Part_No = DBConvert.ToString(dr["Part_No"]);
                                _Ret_Data.strType = DBConvert.ToString(dr["strType"]);

                                _Ret_Data.flLen_S = float.Parse(DBConvert.ToString(dr["flLen_S"]));
                                #region 检测当前的屏幕序号
                                iNo = 0;
                                for (int i = 0; i < lst_Screenkd.Count; i++)
                                {
                                    if (_Ret_Data.flLen_S >= lst_Screenkd[i].Chart_Run_flStart_Distance && (_Ret_Data.flLen_S < lst_Screenkd[i].Chart_Run_flEnd_Distance))//|| flDistance <= m_flArr_Rul_S[i+1]))
                                    {
                                        iNo = i;
                                        break;
                                    }
                                }
                                if (iNo != iScreenNo) continue;//不是当前数据，则不添加
                                #endregion
                                _Ret_Data.flLen_E = float.Parse(DBConvert.ToString(dr["flLen_E"]));
                                _Ret_Data.flLen = float.Parse(DBConvert.ToString(dr["flLen"]));

                                _Ret_Data.iX_No = int.Parse(DBConvert.ToString(dr["iX_No"]));
                                _Ret_Data.iY_No = int.Parse(DBConvert.ToString(dr["iY_No"]));

                                _Ret_Data.flHeight_S = float.Parse(DBConvert.ToString(dr["flHeight_S"]));
                                _Ret_Data.flHeight_E = float.Parse(DBConvert.ToString(dr["flHeight_E"]));
                                _Ret_Data.flHeight = float.Parse(DBConvert.ToString(dr["flHeight"]));

                                _Ret_Data.flDepth_S = float.Parse(DBConvert.ToString(dr["flDepth_S"]));
                                _Ret_Data.flDepth = float.Parse(DBConvert.ToString(dr["flDepth"]));

                                _lst_Ret.Add(_Ret_Data);
                                break;
                            }
                            #endregion  拿数据
                        }
                    }
                    catch (Exception e2)
                    { }
                    #endregion 0
                    break;
                case 1:
                    if (SqlDbHelper.m_DataBase == "")
                        SqlDbHelper.Init();
                    #region 1 
                    try
                    {
                        DataTable dt_1 = SqlDbHelper.ExecuteDataTable(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                        if (dt_1 != null && dt_1.Rows.Count > 0)
                        {
                            #region 拿数据
                            foreach (DataRow dr in dt_1.Rows)
                            {
                                //2.1  将当前记录赋值给 
                                Class_Test_AlarmArea _Ret_Data = new Class_Test_AlarmArea();
                                //2.1  将当前记录赋值给 

                                _Ret_Data.ID = DBConvert.ToString(dr["ID"]);
                                _Ret_Data.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                                _Ret_Data.Part_No = DBConvert.ToString(dr["Part_No"]);

                                _Ret_Data.strType = DBConvert.ToString(dr["strType"]);
                                try
                                {
                                    _Ret_Data.flLen_S = float.Parse(DBConvert.ToString(dr["flLen_S"]));

                                    #region 检测当前的屏幕序号
                                     iNo = 0;
                                    for (int i = 0; i < lst_Screenkd.Count; i++)
                                    {
                                        if (_Ret_Data.flLen_S >= lst_Screenkd[i].Chart_Run_flStart_Distance && (_Ret_Data.flLen_S < lst_Screenkd[i].Chart_Run_flEnd_Distance))//|| flDistance <= m_flArr_Rul_S[i+1]))
                                        {
                                            iNo = i;
                                            break;
                                        }
                                    }
                                    if (iNo != iScreenNo) continue;//不是当前数据，则不添加
                                    #endregion 

                                    _Ret_Data.flLen_E = float.Parse(DBConvert.ToString(dr["flLen_E"]));
                                    _Ret_Data.flLen = float.Parse(DBConvert.ToString(dr["flLen"]));

                                    _Ret_Data.iX_No = int.Parse(DBConvert.ToString(dr["iX_No"]));
                                    _Ret_Data.iY_No = int.Parse(DBConvert.ToString(dr["iY_No"]));

                                    _Ret_Data.flHeight_S = float.Parse(DBConvert.ToString(dr["flHeight_S"]));
                                    _Ret_Data.flHeight_E = float.Parse(DBConvert.ToString(dr["flHeight_E"]));
                                    _Ret_Data.flHeight = float.Parse(DBConvert.ToString(dr["flHeight"]));

                                    _Ret_Data.flDepth_S = float.Parse(DBConvert.ToString(dr["flDepth_S"]));
                                    _Ret_Data.flDepth = float.Parse(DBConvert.ToString(dr["flDepth"]));
                                }
                                catch (Exception e2)
                                { }
                                _lst_Ret.Add(_Ret_Data);
                            }
                            #endregion  拿数据
                        }
                    }
                    catch (Exception e3)
                    {

                    }
                    #endregion 1
                    break;
            }
            return _lst_Ret;
        }
    }
}
