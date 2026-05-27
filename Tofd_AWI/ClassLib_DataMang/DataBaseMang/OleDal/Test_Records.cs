/*
 * Copyright(C) 2 2017 郑州金润高科电子有限公司
 * 文件名: DistanceThick.cs
 * 文件功能描述: 距离厚度信息数据操作
 * 目的：距离厚度数据库文件
 * 创建标识: 陈大伟 2017-11
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
    public class Test_Records
    {
        /// <summary>
        /// 操作的数据表名称 TestRecord
        /// </summary>
        private static string _DbTableName = "TestRecord";
        /// <summary>
        /// 指定数据时的操作表名称 TestRecord
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

        private static int _iChnns = 0;
        /// <summary>
        ///通道数
        /// </summary>
        public int m_iChnns
        {
            get { return _iChnns; }
            set { _iChnns = value; }
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <returns></returns>
        public bool Creat_Table(int iChnns = 1)
        {
            bool _blRet = false;
            string strSQL = "", strFileName = ""; ;
            try
            {
                #region 字段表
                strFileName = " ID varchar(20)     NOT NULL," +//ID号 YYMMDDHHmm
                               "Sub_ID    varchar(20) NOT NULL , " +// 焊缝ID号:  MMDDHHmmss
                              " flDistance_X    float NOT NULL , " +//当前距离

                              " strMarking    varchar(5)  NULL , " +//异常是否打标 不打标：0，打标“1”
                              (_iDataBase_Type == 0 ? " i_Defectdepth    Integer  NULL , " : "i_Defectdepth  tinyint  NULL, ") +//缺陷深度
                              (_iDataBase_Type == 0 ? " i_DefectLengt    Integer  NULL , " : "i_DefectLengt tinyint  NULL, ") +//缺陷长度

                              " strData  text  NULL ,  " +//报文数据
                              " strData_2  text  NULL ,  " +//报文2数据
                              " strWave_Time  text  NULL ,  " +//采样时间 波峰序列每个值对应的时间序列，每个值乘以10ns为时间的采样时间
                              " TOFD_Para  text ,  " +//通道参数,记录运行过程修改的参数

                              " strOther_1    varchar(50)  NULL , " +//备用1
                              " strOther_2    varchar(50)  NULL , " +//备用2

                             (_iDataBase_Type == 0 ? " iOther_1    Integer  NULL , " : "iOther_1 tinyint  NULL, ") +//备用3
                             (_iDataBase_Type == 0 ? " iOther_2    Integer  NULL , " : "iOther_2 tinyint  NULL, ");//备用4
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
            { System.Windows.Forms.MessageBox.Show("Test_Records.cs  Creat_Table error：" + e.Message + e.StackTrace + " " + strSQL); }
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
            { System.Windows.Forms.MessageBox.Show("Manage_Test_Records.cs的DeleteDaterror：" + e.Message + e.StackTrace + " " + strSQL); }
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
            { System.Windows.Forms.MessageBox.Show("Manage_Test_Records.cs的DeleteDat_ForIDerror：" + e.Message + e.StackTrace + " " + strSQL); }
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
                //      System.Windows.Forms.MessageBox.Show("Manage_DeepThick.cs的DeleteDat_ForID出错：" + e.Message + e.StackTrace + " " + strSQL); 
            }
            return _blRet;
        }

        /// <summary>
        /// 获得对应波形数据
        /// </summary>
        /// <param name="ID">主键</param>
        /// <param name="Sub_ID">焊缝主键</param>
        /// <param name="flDistance_X">X轴位置</param>
        /// <param name="_blOne">true:第一报文/ false:两个报文</param>
        /// <returns></returns>
        public string GetWave(string ID, string  Sub_ID, float flDistance_X )
        {
            string strRet = "";
            string strSQL = "";

            strSQL = "SELECT * from " + _DbTableName + " where ID= '" + ID + "'  and Sub_ID='" + Sub_ID+"'" +
                    "  and flDistance_X=" + flDistance_X;

            switch (_iDataBase_Type)
            {
                case 0:
                    OleDbDataReader _Read = OleDbHelper.ExecuteReader(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null);
                    if (_Read != null)
                    {
                        while (_Read.Read())
                        {
                            //if (_blOne)
                            //    strRet = DBConvert.ToString(_Read["strData"]);
                            //else
                        //        strRet = DBConvert.ToString(_Read["strData"]) + "/" + (DBConvert.ToString(_Read["strData_2"]));
                            strRet = DBConvert.ToString(_Read["strData"]) + "/" +
                                         DBConvert.ToString(_Read["strData_2"]) + "/" +
                                         DBConvert.ToString(_Read["strWave_Time"]);
                            break;
                        }
                    }
                    break;
                case 1:
                    if (SqlDbHelper.m_DataBase == "")
                        SqlDbHelper.Init();
                    DataTable dt_1 = SqlDbHelper.ExecuteDataTable(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, _DbTableName, null);

                    foreach (DataRow dr in dt_1.Rows)
                    {
                        try
                        {
                            //if (_blOne)
                            //    strRet = DBConvert.ToString(dr["strData"]);
                            //else
                                strRet = DBConvert.ToString(dr["strData"]) + "/" +
                                         DBConvert.ToString(dr["strData_2"]) +"/" + 
                                         DBConvert.ToString(dr["strWave_Time"]) ;
                        }
                        catch { }
                        break;
                    }
                    break;
            }
            return strRet;
        }
        public OleDbDataReader GetWave_Arr(string ID, int Buff_iRows, float flDistance_X, float flDistance_End_X, float flDistance_Y, bool _blOne = true)
        {
            string strSQL = "";

                strSQL = "SELECT * from " + _DbTableName + " where ID= '" + ID + "'  and Buff_iRows=" + Buff_iRows + "  and flDistance_X >=" + flDistance_X + "  and flDistance_X <=" + flDistance_End_X;
            OleDbDataReader _Read = OleDbHelper.ExecuteReader(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL,  null);

            return _Read;
        }


        public DataTable GetWave_Arr_Sql(string ID, int Buff_iRows, float flDistance_X, float flDistance_End_X, float flDistance_Y, bool _blOne = true)
        {
            //    if (_blOne)//1通道没有Y轴数据，只能根据数据行数来代替Y轴偏移
            string strSQL = "SELECT * from " + _DbTableName + " where ID= '" + ID + "'  and Buff_iRows=" + Buff_iRows + "  and flDistance_X >=" + flDistance_X + "  and flDistance_X <=" + flDistance_End_X;

            DataTable dt_1 = SqlDbHelper.ExecuteDataTable(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, _DbTableName, null);

            return dt_1;
        }
       
        /// <summary>
        /// 保存数据到临时库 0:失败 1：覆盖 2：插入新的
        /// </summary>
        /// <param name="_Infor"></param>
        /// <returns>0:失败 1：覆盖 2：插入新的</returns>
        public int SaveData(ClassLib_TestData.Class_Test_Records _Infor)
        {
            int iRet = 0;
            string strSQL = "";
            string strAdd_Val = "";//增加Sql值
            string strInsert_Add_Zd = "";//插入字段表
            string strUpdata_Add_Zd = "";//更新字符串
            try
            {
                if (_Infor.ID == "" || _Infor.Sub_ID == "") return 0;

                //1 查询
                //单位名称  受检设备名称  设备地址
             //   if (_blOne)//1通道没有Y轴数据，只能根据数据行数来代替Y轴偏移
                    strSQL = "SELECT * from " + _DbTableName + " where ID= '" + _Infor.ID + "'"  +
                            " and Sub_ID = '" + _Infor.Sub_ID  + "'"  + 
                           "  and flDistance_X=" + _Infor.flDistance_X;

                #region 插入字符串
                strInsert_Add_Zd = "ID,Sub_ID,flDistance_X," +
                             "strMarking,i_Defectdepth,i_DefectLengt,strData,strData_2,strWave_Time," +
                             "strOther_1,strOther_2," +
                             "iOther_1,iOther_2,TOFD_Para";
                            // "ReMark";

                strAdd_Val = "'" + _Infor.ID + "'";
                strAdd_Val += ",'" + _Infor.Sub_ID + "'";
                strAdd_Val += "," + _Infor.flDistance_X;

                strAdd_Val += ",'" + _Infor.strMarking + "'";
                strAdd_Val += "," + _Infor.i_Defectdepth;
                strAdd_Val += "," + _Infor.i_DefectLengt;
                strAdd_Val += ",'" + _Infor.strData + "'";
                strAdd_Val += ",'" + _Infor.strData_2 + "'";
                strAdd_Val += ",'" + _Infor.strWave_Time + "'";

                strAdd_Val += ",'" + _Infor.strOther1 + "'";
                strAdd_Val += ",'" + _Infor.strOther2 + "'";
                strAdd_Val += "," + _Infor.iOther1;
                strAdd_Val += "," + _Infor.iOther2;
                strAdd_Val += ",'" + _Infor.TOFD_Para + "'";
                #endregion 插入字符串

                #region 更新字符串
                //3 插入数据
                strUpdata_Add_Zd = "SET ID='" + _Infor.ID + "'";
                strUpdata_Add_Zd += ",Sub_ID='" + _Infor.Sub_ID + "'";
                strUpdata_Add_Zd += ",flDistance_X=" + _Infor.flDistance_X;

                strUpdata_Add_Zd += ",strMarking='" + _Infor.strMarking + "'";
                strUpdata_Add_Zd += ",i_Defectdepth=" + _Infor.i_Defectdepth;
                strUpdata_Add_Zd += ",i_DefectLengt=" + _Infor.i_DefectLengt;

                strUpdata_Add_Zd += ",strData='" + _Infor.strData + "'";
                strUpdata_Add_Zd += ",strData_2='" + _Infor.strData_2 + "'";
                strUpdata_Add_Zd += ",strWave_Time='" + _Infor.strWave_Time + "'";

                strUpdata_Add_Zd += ",strOther_1='" + _Infor.strOther1 + "'";
                strUpdata_Add_Zd += ",strOther_2='" + _Infor.strOther2 + "'";
                strUpdata_Add_Zd += ",iOther_1=" + _Infor.iOther1 ;
                strUpdata_Add_Zd += ",iOther_2=" + _Infor.iOther2 ;
                strUpdata_Add_Zd += ",TOFD_Para='" + _Infor.TOFD_Para + "'";
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
                                    strRet = (DBConvert.ToString(_Read["strData"]));
                                    break;
                                }
                                _Read.Close();
                            }

                            if (strRet != "")// dt != null && dt.Rows.Count > 0)
                            {
                                //2 更新数据 
                                if (_DbTableName != "" && strUpdata_Add_Zd != "")
                                {
                                    //2.3 添加到数据库
                                    //if (_blOne)//1通道没有Y轴数据，只能根据数据行数来代替Y轴偏移
                                        strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + 
                                                " Where  ID= '" + _Infor.ID + "'" +
                                                       " and Sub_ID = '" + _Infor.Sub_ID  + "'" +
                                                      "  and flDistance_X=" + _Infor.flDistance_X;

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
      
                            strSQL = "insert into " + _DbTableName + "  (" + strInsert_Add_Zd + ") values(" + strAdd_Val + ")";
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
                                //    if (_blOne)//1通道没有Y轴数据，只能根据数据行数来代替Y轴偏移
                                strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd +
                                        " Where  ID= '" + _Infor.ID + "'" +
                                               " and Sub_ID = '" + _Infor.Sub_ID  + "'" +
                                              "  and flDistance_X=" + _Infor.flDistance_X;

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
            { }// System.Windows.Forms.MessageBox.Show("Manage_Test_Records.cs的SaveData_Tmperror：" + e.Message + e.StackTrace + " " + strSQL); }

            return iRet;
        }
        /// <summary>
        /// 查询获得指定多个距离厚度数据  ClassStand g_Stand
        /// </summary>
        /// <param name="strWhere"> 根据单位名称、受检设备名、设备地址查询</param>
        /// <param name="iChnns">通道数</param>
        /// <param name="blHaveY">是否有Y轴编码器</param>
        /// <param name="iWantGetRows"></param>
        /// <returns></returns>
        public List<Class_Test_Records > GetData(string ID,string Sub_ID)
        {
            List<Class_Test_Records> _lst_Info = new List<Class_Test_Records>();
            string strSQL = "SELECT * from " + _DbTableName + " where ID=' " + ID + "'  and  Sub_ID='" + Sub_ID + "' order by flDistance_X";
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
                            Class_Test_Records _Infor = new Class_Test_Records();
                            #region 字段赋值
                            _Infor.ID = DBConvert.ToString(dr["ID"]);
                            _Infor.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                            _Infor.flDistance_X = float .Parse ( DBConvert.ToString(dr["flDistance_X"]));

                            _Infor.strMarking = DBConvert.ToString(dr["strMarking"]);
                            _Infor.i_Defectdepth =DBConvert.ToInt32 (dr["i_Defectdepth"]);
                            _Infor.i_DefectLengt = DBConvert.ToInt32(dr["i_DefectLengt"]);

                            _Infor.strData = DBConvert.ToString(dr["strData"]);
                            _Infor.strData_2 = DBConvert.ToString(dr["strData_2"]);
                            _Infor.strWave_Time = DBConvert.ToString(dr["strWave_Time"]);

                            _Infor.strOther1 = DBConvert.ToString(dr["strOther_1"]);
                            _Infor.strOther2 = DBConvert.ToString(dr["strOther_2"]);
                            _Infor.iOther1 = DBConvert.ToInt32(dr["iOther_1"]);
                            _Infor.iOther2 = DBConvert.ToInt32(dr["iOther_2"]);
                            _Infor.TOFD_Para  = DBConvert.ToString(dr["TOFD_Para"]);
                            #endregion 字段赋值 
                            _lst_Info.Add(_Infor);
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
                            Class_Test_Records _Infor = new Class_Test_Records();
                            #region 字段赋值
                            _Infor.ID = DBConvert.ToString(dr["ID"]);
                            _Infor.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                            _Infor.flDistance_X = float.Parse(DBConvert.ToString(dr["flDistance_X"]));

                            _Infor.strMarking = DBConvert.ToString(dr["strMarking"]);
                            _Infor.i_Defectdepth = DBConvert.ToInt32(dr["i_Defectdepth"]);
                            _Infor.i_DefectLengt = DBConvert.ToInt32(dr["i_DefectLengt"]);

                            _Infor.strData = DBConvert.ToString(dr["strData"]);
                            _Infor.strData_2 = DBConvert.ToString(dr["strData_2"]);
                            _Infor.strWave_Time = DBConvert.ToString(dr["strWave_Time"]);

                            _Infor.strOther1 = DBConvert.ToString(dr["strOther_1"]);
                            _Infor.strOther2 = DBConvert.ToString(dr["strOther_2"]);
                            _Infor.iOther1 = DBConvert.ToInt32(dr["iOther_1"]);
                            _Infor.iOther2 = DBConvert.ToInt32(dr["iOther_2"]);
                            _Infor.TOFD_Para = DBConvert.ToString(dr["TOFD_Para"]);
                            #endregion 字段赋值 
                            _lst_Info.Add(_Infor);
                        }
                    }
                    #endregion 1
                    break;
            }
            return _lst_Info;
        }

        /// <summary>
        /// 查询获得指定多个距离厚度数据  ClassStand g_Stand
        /// </summary>
        /// <param name="strWhere"> 根据单位名称、受检设备名、设备地址查询</param>
        /// <param name="iChnns">通道数</param>
        /// <param name="blHaveY">是否有Y轴编码器</param>
        /// <param name="iWantGetRows"></param>
        /// <returns></returns>
        public List<Class_Test_Records> GetData(string ID, string Sub_ID,float fl_X_S,float fl_X_E)
        {
            List<Class_Test_Records> _lst_Info = new List<Class_Test_Records>();
            string strSQL = "SELECT * from " + _DbTableName + " where ID='" + ID + "' and" +
                            " Sub_ID='"+ Sub_ID +"' and  flDistance_X >="+ fl_X_S + " and flDistance_X <"+ fl_X_E +  " order by flDistance_X";
                 
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
                            Class_Test_Records _Infor = new Class_Test_Records();
                            #region 字段赋值
                            _Infor.ID = DBConvert.ToString(dr["ID"]);
                            _Infor.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                            _Infor.flDistance_X = float.Parse(DBConvert.ToString(dr["flDistance_X"]));

                            _Infor.strMarking = DBConvert.ToString(dr["strMarking"]);
                            _Infor.i_Defectdepth = DBConvert.ToInt32(dr["i_Defectdepth"]);
                            _Infor.i_DefectLengt = DBConvert.ToInt32(dr["i_DefectLengt"]);

                            _Infor.strData = DBConvert.ToString(dr["strData"]);
                            _Infor.strData_2 = DBConvert.ToString(dr["strData_2"]);
                            _Infor.strWave_Time = DBConvert.ToString(dr["strWave_Time"]);

                            _Infor.strOther1 = DBConvert.ToString(dr["strOther_1"]);
                            _Infor.strOther2 = DBConvert.ToString(dr["strOther_2"]);
                            _Infor.iOther1 = DBConvert.ToInt32(dr["iOther_1"]);
                            _Infor.iOther2 = DBConvert.ToInt32(dr["iOther_2"]);
                            _Infor.TOFD_Para = DBConvert.ToString(dr["TOFD_Para"]);
                            #endregion 字段赋值 
                            _lst_Info.Add(_Infor);
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
                            Class_Test_Records _Infor = new Class_Test_Records();
                            #region 字段赋值
                            try
                            {
                                _Infor.ID = DBConvert.ToString(dr["ID"]);
                                _Infor.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                                _Infor.flDistance_X = float.Parse(DBConvert.ToString(dr["flDistance_X"]));

                                _Infor.strMarking = DBConvert.ToString(dr["strMarking"]);
                                _Infor.i_Defectdepth = DBConvert.ToInt32(dr["i_Defectdepth"]);
                                _Infor.i_DefectLengt = DBConvert.ToInt32(dr["i_DefectLengt"]);

                                _Infor.strData = DBConvert.ToString(dr["strData"]);
                                _Infor.strData_2 = DBConvert.ToString(dr["strData_2"]);
                                try
                                {
                                    _Infor.strWave_Time = DBConvert.ToString(dr["strWave_Time"]);
                                }
                                catch(Exception e333)
                                { }

                                _Infor.strOther1 = DBConvert.ToString(dr["strOther_1"]);
                                _Infor.strOther2 = DBConvert.ToString(dr["strOther_2"]);
                                _Infor.iOther1 = DBConvert.ToInt32(dr["iOther_1"]);
                                _Infor.iOther2 = DBConvert.ToInt32(dr["iOther_2"]);
                                _Infor.TOFD_Para = DBConvert.ToString(dr["TOFD_Para"]);
                            }
                            catch (Exception e2)
                            { }
                            #endregion 字段赋值 
                            _lst_Info.Add(_Infor);
                        }
                    }
                    #endregion 1
                    break;
            }
            return _lst_Info;
        }
    }
}
