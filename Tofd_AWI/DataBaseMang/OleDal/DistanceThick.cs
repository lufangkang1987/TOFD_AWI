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

using FrameWork.Struct;//类文件
using FrameWork.DataBaseMang.DBUtility;//操作数据库
namespace FrameWork.DataBaseMang.OleDal
{
    public class Manage_DistanceThick
    {
        /// <summary>
        /// 操作的数据表名称
        /// </summary>
        private static string _DbTableName = "DistanceThick";
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
                strFileName = "ID varchar(20)     NOT NULL," +//ID
                            " flDistance_X    float  NOT NULL , " +//X轴方向距离
                            " flDistance_Y    float  NOT NULL , " +//Y轴方向距离
                             "Buff_iRows " + (_iDataBase_Type == 0 ? "Integer" : "tinyint") + " not null, "+//行号：数据库对应的行

                             "Trajectory_Type " + (_iDataBase_Type == 0 ? "Integer" : "tinyint") + ", " +//轨迹 0：直线 2：直线往返
                             "VideoNo " + (_iDataBase_Type == 0 ? "Integer" : "tinyint") + " , ";//行号：数据库对应的行
                for (int iChn = 1; iChn <= iChnns; iChn++)
                    strFileName += "iChn_" + iChn.ToString() + " varchar(63) ,";// + (iChn != iChnns ? ", " : "");//通道1-N 壁厚  数据格式： 厚度 | 色标（R/G/B）| 打标0或者1 | 增益
                strFileName += "  Wave  text ,";//  varchar(4000)  NULL "; //一个通道波形
                strFileName += "  Wave_2  text ";
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
            { System.Windows.Forms.MessageBox.Show("DistanceThick.cs  Creat_Table error：" + e.Message + e.StackTrace + " " + strSQL); }
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
            { System.Windows.Forms.MessageBox.Show("DistanceThick.cs的DeleteDaterror：" + e.Message + e.StackTrace + " " + strSQL); }
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
        public void  AddDataBase(string strTmpName,int iChnnNums)
        {
            int iRet = 0;
            string strAdd_Val = "";//增加Sql值
            string strInsert_Add_Zd = "";//插入字段表
            string strSQL = "insert into [;database=D:\a3.accdb].A select * from [;database=D:\a4.accdb].A";
            // DOCMD.RUNSQL "insert into [;database=D:\a3.accdb].A select * from [;database=D:\a4.accdb].A"

            switch (_iDataBase_Type)
            {
                case 0:
                    strSQL = "SELECT * from " + _DbTableName;
                    OleDbHelper.DbConnString_DT = OleDbHelper.DbConnString_DT = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        OleDbHelper.strDb_TmpDbPath + "\\" + strTmpName;
                    DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                    int iNum = dt.Rows.Count;
                    if (dt != null && iNum > 0)
                    {
                        OleDbHelper.DbConnString_DT = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                         OleDbHelper.strDb_SubDbPath + "\\" + OleDbHelper.strSubDataBaseName;
                        foreach (DataRow dr in dt.Rows)
                        {
                            strInsert_Add_Zd = "ID,Buff_iRows,flDistance_X,flDistance_Y,Buff_iRows,Trajectory_Type,VideoNo,";
                            for (int i = 0; i < iChnnNums; i++)
                                strInsert_Add_Zd += (i == 0 ? "" : ",") + "iChn_" + (i + 1).ToString();
                            strInsert_Add_Zd += ",Wave";

                            strAdd_Val = "'" + DBConvert.ToString(dr["ID"]) + "'";
                            strAdd_Val += "," + DBConvert.ToInt32(dr["Buff_iRows"]);
                            strAdd_Val += "," + float.Parse(DBConvert.ToString(dr["flDistance_X"]));
                            strAdd_Val += "," + float.Parse(DBConvert.ToString(dr["flDistance_Y"]));

                            strAdd_Val += "," + DBConvert.ToString(dr["Buff_iRows"]);
                            try
                            {
                                strAdd_Val += "," + DBConvert.ToString(dr["Trajectory_Type"]);
                            }
                            catch { }
                            strAdd_Val += "," + DBConvert.ToString(dr["VideoNo"]);

                            strAdd_Val += ",";
                            for (int i = 0; i < iChnnNums; i++)//厚度 | 色标（R/G/B）| 打标0或者1
                                strAdd_Val += (i == iChnnNums - 1 ? "" : ",") +
                                    ("'" + DBConvert.ToString(dr["iChn_"+(i+1).ToString ()]) + "'");
                            strAdd_Val += ", '" + DBConvert.ToString(dr["Wave"]) + "'";

                            strSQL = "insert into " + _DbTableName + "  (" + strInsert_Add_Zd + ") values(" + strAdd_Val + ")";
                            if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                iRet = 0;
                            else
                                iRet = 2;
                            
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// 获得对应波形数据
        /// </summary>
        /// <param name="ID">主键</param>
        /// <param name="Buff_iRows">行号0-N</param>
        /// <param name="flDistance_X">X轴位置</param>
        /// <param name="flDistance_Y"></param>
        /// <param name="_blOne">默认单通道true:单通道  false:双通道</param>
        /// <returns></returns>
        public string GetWave(string ID, int Buff_iRows, float flDistance_X, float flDistance_Y, bool _blOne = true)
        {
            string strRet = "";
            string strSQL = "";

           if (_blOne)//1通道没有Y轴数据，只能根据数据行数来代替Y轴偏移
                strSQL = "SELECT * from " + _DbTableName + " where ID= '" + ID + "'  and Buff_iRows=" + Buff_iRows + "  and flDistance_X=" + flDistance_X;
            else
                strSQL = "SELECT * from " + _DbTableName + " where ID= '" + ID + "' and Buff_iRows=" + Buff_iRows + " and flDistance_Y=" + flDistance_Y + "  and flDistance_X=" + flDistance_X;
            //测试钢板_2341253345_1235134512_181122_085343_第2工作区.mdb"
            bool _blTable = false;

            if (_blTable)
            {
                DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            if (_blOne)
                                strRet = DBConvert.ToString(dr["Wave"]);
                            else
                                strRet = DBConvert.ToString(dr["Wave"]) + "|" + (DBConvert.ToString(dr["Wave_2"]));
                        }
                        catch { }
                        break;
                    }
                }
            }
            else
            {
                switch (_iDataBase_Type)
                {
                    case 0:
                        OleDbDataReader _Read = OleDbHelper.ExecuteReader(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null);
                        if (_Read != null)
                        {
                            while (_Read.Read())
                            {
                                if (_blOne)
                                    strRet = DBConvert.ToString(_Read["Wave"]);
                                else
                                    strRet = DBConvert.ToString(_Read["Wave"]) + "|" + (DBConvert.ToString(_Read["Wave_2"]));
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
                                if (_blOne)
                                    strRet = DBConvert.ToString(dr["Wave"]);
                                else
                                    strRet = DBConvert.ToString(dr["Wave"]) + "|" + (DBConvert.ToString(dr["Wave_2"]));
                            }
                            catch { }
                            break;
                        }
                        break;
                }
            }
            return strRet;
        }
        public string GetWave_4(string ID, int Buff_iRows, float flDistance_X, float flDistance_Y, bool _blOne = true)
        {
            string strRet = "";
            string strSQL = "";

            //  if (_blOne)//1通道没有Y轴数据，只能根据数据行数来代替Y轴偏移
            strSQL = "SELECT * from " + _DbTableName + " where ID= '" + ID + "'  and Buff_iRows=" + Buff_iRows + "  and flDistance_Y=" + flDistance_Y + "  and flDistance_X=" + flDistance_X;
            //else
            //    strSQL = "SELECT * from " + _DbTableName + " where ID= '" + ID + "'  and flDistance_Y=" + flDistance_Y + "  and flDistance_X=" + flDistance_X;
            //测试钢板_2341253345_1235134512_181122_085343_第2工作区.mdb"
            bool _blTable = false;

            if (_blTable)
            {
                DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            if (_blOne)
                                strRet = DBConvert.ToString(dr["Wave"]);
                            else
                                strRet = DBConvert.ToString(dr["Wave"]) + "|" + (DBConvert.ToString(dr["Wave_2"]));
                        }
                        catch { }
                        break;
                    }
                }
            }
            else
            {
                switch (_iDataBase_Type)
                {
                    case 0:
                        OleDbDataReader _Read = OleDbHelper.ExecuteReader(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null);
                        if (_Read != null)
                        {
                            while (_Read.Read())
                            {
                                if (_blOne)
                                    strRet = DBConvert.ToString(_Read["Wave"]);
                                else
                                    strRet = DBConvert.ToString(_Read["Wave"]) + "|" + (DBConvert.ToString(_Read["Wave_2"]));
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
                                if (_blOne)
                                    strRet = DBConvert.ToString(dr["Wave"]);
                                else
                                    strRet = DBConvert.ToString(dr["Wave"]) + "|" + (DBConvert.ToString(dr["Wave_2"]));
                            }
                            catch { }
                            break;
                        }
                        break;
                }
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
       
        public int Mody_Thickness_By_Wave_2(string ID, int iBuff_iRows, float flDistance_X, float flThick,string strColor,int iType=0,float fl_Y=-1)
        {
            int iRet = -1;
            #region  1 寻找待修改的记录
            string strWherre= " where ID= '" + ID + "'  and Buff_iRows=" + iBuff_iRows +
                              "  and flDistance_X=" + flDistance_X;
            if (iType == 1) strWherre += " and flDistance_Y =" + fl_Y;
            string strSQL = "SELECT * from " + _DbTableName + strWherre;
            string str_iChn_1 = "";//待修改的字段
            string str_Wave_2 = ""; 
            switch (_iDataBase_Type)
            {
                case 0:
                    #region 0
                    try
                    {
                        OleDbDataReader _Read = OleDbHelper.ExecuteReader(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null);
                        if (_Read != null)
                        {
                            while (_Read.Read())
                            {
                                str_iChn_1 = (DBConvert.ToString(_Read["iChn_1"]));
                                str_Wave_2 =(DBConvert.ToString(_Read["Wave_2"]));
                                if (str_Wave_2 != "") str_Wave_2 = str_Wave_2.Trim();
                                break;
                            }
                            _Read.Close();
                        }

                        if (str_iChn_1 != "")
                        {
                            //2 更新数据 
                            if (_DbTableName != "" )
                            {
                                #region 2 组织更新字段
                                string[] _sPara = str_iChn_1.Split('|');
                                if (_sPara.Length == 3)//厚度 |  颜色  | 打标
                                {
                                    _sPara[0] = flThick.ToString();
                                    _sPara[1] = strColor;
                                    string _strNew_iChn_1 = _sPara[0] + "|" + _sPara[1] + "|" + _sPara[2];

                                    string strUpdata_Add_Zd = "";
                                    if (str_Wave_2 == "")
                                    {
                                        strUpdata_Add_Zd = "SET iChn_1='" + _strNew_iChn_1 + "'";
                                        strUpdata_Add_Zd += ", Wave_2='" + str_iChn_1 + "'";
                                    }
                                    else
                                        strUpdata_Add_Zd = "SET iChn_1='" + _strNew_iChn_1 + "'";
                                    #endregion 2
                                    #region  3 添加到数据库
                                    strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + strWherre;

                                    if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                        iRet = 0;
                                    else
                                        iRet = 1;
                                    #endregion 3
                                }
                            }
                            return iRet;
                        }
                    }
                    catch (Exception e)
                    { }
                  
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
                        if (_DbTableName != "")
                        {
                            foreach (DataRow dr in dt_1.Rows)
                            {
                                str_iChn_1 = (DBConvert.ToString(dr["iChn_1"]));
                                str_Wave_2 = (DBConvert.ToString(dr["Wave_2"]));
                                if (str_Wave_2 != "") str_Wave_2 = str_Wave_2.Trim();
                                break;
                            }
                            if (str_iChn_1 != "")
                            {
                                //2 更新数据 
                                if (_DbTableName != "")
                                {
                                    #region 2 组织更新字段
                                    string[] _sPara = str_iChn_1.Split('|');
                                    if (_sPara.Length == 3)//厚度 |  颜色  | 打标
                                    {
                                        _sPara[0] = flThick.ToString();
                                        _sPara[1]= strColor;
                                        string _strNew_iChn_1 = _sPara[0] + "|" + _sPara[1] + "|" + _sPara[2];

                                        string strUpdata_Add_Zd = "";
                                        if (str_Wave_2 == "")
                                        {
                                            strUpdata_Add_Zd = "SET iChn_1='" + _strNew_iChn_1 + "'";
                                            strUpdata_Add_Zd += ", Wave_2='" + str_iChn_1 + "'";
                                        }
                                        else
                                            strUpdata_Add_Zd = "SET iChn_1='" + _strNew_iChn_1 + "'";
                                        #endregion 2
                                        #region  3 添加到数据库
                                        strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + strWherre;

                                        if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                            iRet = 0;
                                        else
                                            iRet = 1;
                                        #endregion 3
                                    }
                                }
                                return iRet;
                            }

                        }
                    }
               
                    #endregion 1
                    break;
            }
            #endregion  1
            return iRet;
        }
        /// <summary>
        /// 保存数据到临时库 0:失败 1：覆盖 2：插入新的
        /// </summary>
        /// <param name="_Infor"></param>
        /// <returns>0:失败 1：覆盖 2：插入新的</returns>
        public int SaveData(Class_DataBae _Infor)
        {
            int iRet = 0;
            string strSQL = "";
            string strAdd_Val = "";//增加Sql值
            string strInsert_Add_Zd = "";//插入字段表
            string strUpdata_Add_Zd = "";//更新字符串
            int iChnnNums = _Infor.flArrThick.Length;

            bool _blOne = iChnnNums == 1 || _Infor.flDistance_Y == -1 ? true : false;//单通道 ，没有Y轴数据  
            try
            {
                //string strTick = _Infor.flDistance_X.ToString("f3");
                //_Infor.flDistance_X = float.Parse(strTick);
                //1 查询
                //单位名称  受检设备名称  设备地址
             //   if (_blOne)//1通道没有Y轴数据，只能根据数据行数来代替Y轴偏移
                    strSQL = "SELECT * from " + _DbTableName + " where ID= '" + _Infor.ID + "'  and Buff_iRows=" + _Infor.Buff_iRows + 
                    "  and flDistance_X=" + _Infor.flDistance_X + "  and flDistance_Y=" + _Infor.flDistance_Y;
                //else
                //    strSQL = "SELECT * from " + _DbTableName + " where ID= '" + _Infor.ID + "'  and flDistance_Y=" + _Infor.flDistance_Y + "  and flDistance_X=" + _Infor.flDistance_X;

                #region 插入字符串
                strInsert_Add_Zd = "ID,Buff_iRows,flDistance_X,flDistance_Y,VideoNo,Trajectory_Type,";
                for (int i = 0; i < iChnnNums; i++)
                    strInsert_Add_Zd += (i == 0 ? "" : ",") + "iChn_" + (i + 1).ToString();
                strInsert_Add_Zd += ",Wave";
                if (iChnnNums > 1)
                    strInsert_Add_Zd += " ,Wave_2";

                strAdd_Val = "'" + _Infor.ID + "'";// DateTime.Now.ToString("yyMMddHHmmss")
                strAdd_Val += "," + _Infor.Buff_iRows;
                strAdd_Val += "," + _Infor.flDistance_X;
                strAdd_Val += "," + _Infor.flDistance_Y;
                strAdd_Val += "," + _Infor.VideoNo;
                strAdd_Val += "," + _Infor.Trajectory_Type;

                strAdd_Val += ",";
                for (int i = 0; i < iChnnNums; i++)//厚度 | 色标（R/G/B）| 打标0或者1
                {
                    strAdd_Val += (i == 0 ? "" : ",") +
                                  ("'" + _Infor.flArrThick[i].ToString("f3") + "|" +
                                  _Infor.strArrColor[i] + "|" +
                                  (_Infor.strArrLable[i] == null ? "0" : _Infor.strArrLable[i]) + "'");
                }

                strAdd_Val += ", '" + _Infor.strArrWave[0] + "'";
                if (_Infor.strArrWave.Length > 1)
                    strAdd_Val += ", '" + (_Infor.strArrWave[1] == null ? "" : _Infor.strArrWave[1]) + "'";

                #endregion 插入字符串

                #region 更新字符串
                if (_Infor.ID == "")
                {
                    _Infor.ID = DateTime.Now.ToString("yyMMddHHmmss");
                    iRet = 2;
                }
                //3 插入数据
                strUpdata_Add_Zd = "SET ID='" + _Infor.ID + "'";
                strUpdata_Add_Zd += ",Buff_iRows=" + _Infor.Buff_iRows;
                strUpdata_Add_Zd += ",flDistance_X=" + _Infor.flDistance_X;
                strUpdata_Add_Zd += ",flDistance_Y=" + _Infor.flDistance_Y;
                strUpdata_Add_Zd += ",VideoNo=" + _Infor.VideoNo;
                strUpdata_Add_Zd += ",Trajectory_Type=" + _Infor.Trajectory_Type;

                for (int i = 0; i < iChnnNums; i++)
                    strUpdata_Add_Zd += "," + "iChn_" + (i + 1).ToString() + "='" +
                                                                ((_Infor.flArrThick[i].ToString("f3") + "|" +
                                                                _Infor.strArrColor[i] + "|" +
                                                                (_Infor.strArrLable[i] == null ? "0" : _Infor.strArrLable[i])) + "'");
                strUpdata_Add_Zd += ", Wave='" + _Infor.strArrWave[0] + "'";
                if(_Infor.strArrWave.Length >1)
                    strUpdata_Add_Zd += ", Wave_2='" + (_Infor.strArrWave[1] == null ? "" : _Infor.strArrWave[1]) + "'";
                else
                    strUpdata_Add_Zd += ", Wave_2='" + "" + "'";
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
                                    strRet = (DBConvert.ToString(_Read["flDistance_X"]));
                                    break;
                                }
                                _Read.Close();
                            }

                            //  DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                            if (strRet != "")// dt != null && dt.Rows.Count > 0)
                            {
                                //2 更新数据 
                                if (_DbTableName != "" && strUpdata_Add_Zd != "")
                                {
                                    //2.3 添加到数据库
                                    //if (_blOne)//1通道没有Y轴数据，只能根据数据行数来代替Y轴偏移
                                        strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  ID= '" + _Infor.ID + 
                                        "'  and Buff_iRows=" + _Infor.Buff_iRows +
                                        "  and flDistance_X=" + _Infor.flDistance_X +
                                        "  and flDistance_Y=" + _Infor.flDistance_Y;
                                    //else
                                    //    strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  ID= '" + _Infor.ID + "'  and flDistance_Y=" + _Infor.flDistance_Y + "  and flDistance_X=" + _Infor.flDistance_X;

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
                        //  else
                        {
                            #region 字符串 ID,Buff_iRows,flDistance_X,flDistance_Y,iChn_1
                            //3 插入数据 '171208114139',0,-10.30003,-120.132|123,104,238|
                            //  _Infor.ID = DateTime.Now.ToString("yyMMddHHmmss");
                            #endregion

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
                            //    if (_blOne)//1通道没有Y轴数据，只能根据数据行数来代替Y轴偏移
                                    strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  ID= '" + _Infor.ID + 
                                    "'  and Buff_iRows=" + _Infor.Buff_iRows + 
                                    "  and flDistance_X=" + _Infor.flDistance_X+
                                    "  and flDistance_Y=" + _Infor.flDistance_Y;
                                //else
                                //    strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  ID= '" + _Infor.ID + "'  and flDistance_Y=" + _Infor.flDistance_Y + "  and flDistance_X=" + _Infor.flDistance_X;

                                if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                    iRet = 0;
                                else
                                    if (iRet != 2)
                                    iRet = 1;
                            }
                        }
                        else
                        {
                            #region 字符串
                            //3 插入数据
                            _Infor.ID = DateTime.Now.ToString("yyMMddHHmmss");

                            #endregion

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
            { }// System.Windows.Forms.MessageBox.Show("DistanceThick.cs的SaveData_Tmperror：" + e.Message + e.StackTrace + " " + strSQL); }

            return iRet;
        }
        /// <summary>
        /// 通过屏幕序号获得当前屏幕的终止位置刻度
        /// </summary>
        /// <param name="iNo"></param>
        /// <returns></returns>
        public float GetScreen_End(int iNo, Class_Plant Plant_Para)
        {
            float _flData = 0;
          //  _flData = (float)((iNo + 1) * Plant_Para.Scree_Stant_Distance);

           _flData = (iNo ) * (Plant_Para.Scree_Stant_Distance + Plant_Para.Scree_iDotWithmm_X) + Plant_Para.Scree_Stant_Distance;// (float)((iNo + 1) * Plant_Para.Scree_Stant_Distance);

            return _flData;
        }
        /// <summary>
        /// 通过屏幕序号获得当前屏幕的开始位置刻度
        /// </summary>
        /// <param name="iNo"></param>
        /// <returns></returns>
        public float GetScreen_Start(int iNo, Class_Plant Plant_Para)
        {
            float _flData = 0;
            if (iNo == 0)
                _flData = 0;
            else
                //         _flData = (float)(iNo * Plant_Para.Scree_Stant_Distance + Plant_Para .Scree_iDotWithmm_X);
                _flData = (float)(iNo * (Plant_Para.Scree_Stant_Distance + Plant_Para.Scree_iDotWithmm_X));
            return _flData;
        }

        public float GetScreen_S(float _flDistance_X, Class_Plant Plant_Para)
        {
            float iNo = (_flDistance_X / (Plant_Para.Scree_Stant_Distance + Plant_Para.Scree_iDotWithmm_X));
            int iNo_Bf = (int)iNo;
            if (iNo - iNo_Bf > 0.9) iNo_Bf++;
          
            if (iNo < 0) iNo = 0;
            iNo = iNo_Bf;

            iNo = (int)iNo;
            float flStar = 0;
            flStar = (int)iNo * (Plant_Para.Scree_Stant_Distance + Plant_Para.Scree_iDotWithmm_X); //(iNo > 0 ? Scree_iDotWithmm_X : 0); // LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[iNo].flStar_Distance;
            flStar = float.Parse(flStar.ToString("f3"));
            return flStar;
        }
        public float GetScreen_E(float _flDistance_X, Class_Plant Plant_Para)
        {
            float iNo = (_flDistance_X / (Plant_Para.Scree_Stant_Distance + Plant_Para.Scree_iDotWithmm_X));

            int iNo_Bf = (int)iNo;
            if (iNo - iNo_Bf > 0.9) iNo_Bf++;

            if (iNo < 0) iNo = 0;
            iNo = iNo_Bf;

            iNo = (int)iNo;

            float flStar = 0;
            flStar = (int)iNo * (Plant_Para.Scree_Stant_Distance + Plant_Para.Scree_iDotWithmm_X); //(iNo > 0 ? Scree_iDotWithmm_X : 0); // LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[iNo].flStar_Distance;
            flStar += Plant_Para.Scree_Stant_Distance;
            flStar = float.Parse(flStar.ToString("f3"));

            return flStar;
        }
        /// <summary>
        /// 查询获得指定多个距离厚度数据  ClassStand g_Stand
        /// </summary>
        /// <param name="strWhere"> 根据单位名称、受检设备名、设备地址查询</param>
        /// <param name="iChnns">通道数</param>
        /// <param name="blHaveY">是否有Y轴编码器</param>
        /// <param name="iWantGetRows"></param>
        /// <returns></returns>
        public List<ClassListDistanceThick_2> GetData(string strWhere, ref Class_Equipment_Info g_Info, Class_Plant Plant_Para,
                                                      ref int iDataNum,ref int iVideoAllNum, int iWantGetRows = 70000)
        {
            int iOtherRow = 0;
            int iRow = -1;//行号    
            int iTmp = -1;//临时
            string[] _sPara = "".Split(',');
            int iChnns = Plant_Para.m_iRomoteNum;// g_Info.iChnns;
            bool blHaveY = g_Info.iHaveY == 0 ? false : true;
            float _flWc = 0;
            g_Info.flThick_Min = 100; g_Info.flThick_Max = 0;
            float _flThick_Minlimit = g_Info.flNormal_Thickness * float.Parse(g_Info.strMinThickLimit ) / 100;//最小厚度限
            List<ClassListDistanceThick_2> LstBuff_Distance_WallThick_Show = new List<Struct.ClassListDistanceThick_2>();

            int iCurrDataRulerNo = 0;
            int iDataNums = 0;
            float flAverage_Thickness = 0f;
            g_Info.flAverage_Thickness = 0;
            float flEnd = -1;
            try
            {
                int _i_Integer, _iCurrRow, _iCurrPageNo, _iTowChannel_iCol = 0;
                float _iT;
                int iGetDataNum = 0;
                string strSQL = "", strFeldName = "";
                //    if (iChnns == 1 || blHaveY == false)
                strSQL = "SELECT * from " + _DbTableName + " where  " + strWhere + " order by ID, Buff_iRows,  flDistance_X";
                if (Plant_Para.m_Page_Tc.m_lstPage.Count > 0) Plant_Para.m_Page_Tc = new Class_Page_TowChannel();
                //else
                //    strSQL = "SELECT * from " + _DbTableName + " where  " + strWhere + " order by ID,  flDistance_Y, flDistance_X";
                switch (_iDataBase_Type)
                {
                    case 0:
                        #region  0
                        DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            iDataNum = dt.Rows.Count;
                            //2 循环拿记录 ID,Buff_iRows,flDistance,iChn_";
                            foreach (DataRow dr in dt.Rows)
                            {
                                Class_DataBae _Infor = new Struct.Class_DataBae(iChnns);
                                ClassChnn_Thick _ArrThick = new ClassChnn_Thick(iChnns);
                                //ClassChnn_Color _ArrColor = new ClassChnn_Color(iChnns);
                                //ClassChnn_Label _ArrLabel = new ClassChnn_Label(iChnns);

                                ClassKd _ArrKd = new ClassKd();

                                _Infor.Buff_iRows = DBConvert.ToInt32(dr["Buff_iRows"]);
                                if (iRow == -1 && _Infor.Buff_iRows != 0)
                                    iOtherRow = _Infor.Buff_iRows;
                                _Infor.Buff_iRows -= iOtherRow;

                                _Infor.flDistance_X = float.Parse(DBConvert.ToString(dr["flDistance_X"]));
                                _Infor.flDistance_Y = float.Parse(DBConvert.ToString(dr["flDistance_Y"]));
                                _Infor.VideoNo = int.Parse(DBConvert.ToString(dr["VideoNo"]));
                                if(iVideoAllNum < _Infor.VideoNo)
                                iVideoAllNum = _Infor.VideoNo;
                                try
                                {
                                    _Infor.Trajectory_Type = int.Parse(DBConvert.ToString(dr["Trajectory_Type"]));
                                }
                                catch { }

                                _ArrThick.flDistance_Y = _Infor.flDistance_Y;
                                _ArrThick.flDistance_X = _Infor.flDistance_X;
                                _ArrThick.VideoNo = _Infor.VideoNo;
                                _ArrThick.Trajectory_Type = _Infor.Trajectory_Type;

                                #region 增加
                                if (Plant_Para.m_iRomoteNum == 2)
                                {
                                    #region 0  距离转换成图像的列和行号
                                    _i_Integer = 0;
                                    try
                                    {
                                        _i_Integer = (int)(_ArrThick.flDistance_X / Plant_Para.m_flArr_Rul_E[0]);//计算行号
                                       
                                    }
                                    catch { }
                                    _iT = (_ArrThick.flDistance_X % Plant_Para.m_flArr_Rul_E[0]);

                                    _iCurrRow = 0;//计算屏幕对应行号
                                    _iCurrPageNo = 0;//当前屏幕序号

                                    if (_iT == 0 && _ArrThick.flDistance_X > 0)//计算真实的屏幕序号，这个数是当前行最末一个数
                                        _iCurrRow = _i_Integer - 1;
                                    else
                                        _iCurrRow = _i_Integer;
                                    #endregion 0

                                    #region 1 屏幕数据记录
                                    if (Plant_Para.m_Page_Tc.m_iCurrPageNo == -1 || Plant_Para.m_Page_Tc.m_lstPage.Count == 0)
                                    {
                                        //添加第一屏页数据
                                        Class_Page _Page = new Class_Page();
                                        _Page.iStart_Col = _iTowChannel_iCol;
                                        _Page.iStart_Distan = _ArrThick.flDistance_X;

                                        Plant_Para.m_Page_Tc.m_lstPage.Add(_Page);
                                        Plant_Para.m_Page_Tc.m_iCurrPageNo = 0;
                                    }
                                    else//正常的前进后退测量
                                    {
                                        if (_iCurrRow > 1)
                                        {

                                        }
                                        if (_iCurrRow > 1 && _iCurrRow % Plant_Para.Scree_iAllRows_Browse[2] == 0)
                                            _iCurrPageNo = _iCurrRow / Plant_Para.Scree_iAllRows_Browse[2];
                                        if (Plant_Para.Ck_Gsc == 1)
                                        {
                                            _iCurrPageNo = 0;
                                            Plant_Para.Scree_iAllRows_Browse[2] = 85;
                                        }
                                        if (Plant_Para.m_Page_Tc.m_iCurrPageNo == _iCurrPageNo)//同一屏幕数据如果开始数据有变化
                                        {
                                            if (_ArrThick.flDistance_X < Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Distan)//后退
                                            {
                                                Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Distan = _ArrThick.flDistance_X;
                                                Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Col = _iTowChannel_iCol;
                                            }
                                        }
                                        else//换屏了：前进或者后退,开始位置有变化，就修改
                                        {
                                            if (Plant_Para.m_Page_Tc.m_iCurrPageNo < _iCurrPageNo)//前进
                                            {
                                                #region 1.1 前进测量
                                                if (Plant_Para.m_Page_Tc.m_lstPage.Count > Plant_Para.m_Page_Tc.m_iCurrPageNo + 1)
                                                {
                                                    #region 1.1.1如果下一屏已经有数据，则要清屏和画老数据
                                                    ////清屏
                                                    //GetRulerPara_C();//左通道清除
                                                    //Plant_Ruler_C();

                                                    //GetRulerPara_C_TowChannel();//右通道清除
                                                    //Plant_Ruler_C_TowChannel();
                                                    //// 画老数据
                                                    //Plant_TowChannel_Old(_iCurrPageNo, LstBuff_Distance_WallThick_Show);
                                                    #endregion 1.1
                                                }
                                                if (Plant_Para.m_Page_Tc.m_lstPage.Count == Plant_Para.m_Page_Tc.m_iCurrPageNo + 1)
                                                {
                                                    #region 1.1.2 如果下一屏没有数据，则只清屏
                                                    //添加新页
                                                    Class_Page _Page = new Class_Page();
                                                    _Page.iStart_Col = _iTowChannel_iCol;
                                                    _Page.iStart_Distan = _ArrThick.flDistance_X;

                                                    Plant_Para.m_Page_Tc.m_lstPage.Add(_Page);
                                                    ////清屏
                                                    //GetRulerPara_C();//左通道清除
                                                    //Plant_Ruler_C();

                                                    //GetRulerPara_C_TowChannel();//右通道清除
                                                    //Plant_Ruler_C_TowChannel();
                                                    #endregion 1.1.2
                                                }
                                                #endregion  1.1
                                            }
                                            else //后退
                                            {
                                                #region 1.2 后退测量，则要清屏和画老数据
                                                ////清屏
                                                //GetRulerPara_C();//左通道清除
                                                //Plant_Ruler_C();

                                                //GetRulerPara_C_TowChannel();//右通道清除
                                                //Plant_Ruler_C_TowChannel();
                                                //// 画老数据
                                                //Plant_TowChannel_Old(_iCurrPageNo, LstBuff_Distance_WallThick_Show);
                                                #endregion 1.2
                                            }
                                            Plant_Para.m_Page_Tc.m_iCurrPageNo = _iCurrPageNo;//改变屏幕当前运行序号

                                            //Chart_Run_iStartRow_Buff_TowChannel = _iCurrRow;
                                            //Chart_Run_iStart_Row_Browse[2] = _iCurrRow;
                                        }
                                    }
                                    #endregion 1 屏幕开始数据记录
                                }
                                _iTowChannel_iCol++;
                                #endregion  增加


                                #region 统计报警数据
                                ClassAlarm _Alarm = new Struct.ClassAlarm(iChnns);
                                _Alarm.iRow = _Infor.Buff_iRows;
                                _Alarm.flDistance_X = _ArrThick.flDistance_X;
                                _Alarm.flDistance_Y = _ArrThick.flDistance_Y;
                                #endregion 

                                #region 0 当前点尺子序号
                                if (LstBuff_Distance_WallThick_Show.Count > 0 && LstBuff_Distance_WallThick_Show.Count > iRow)
                                    if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count > 0)
                                        iCurrDataRulerNo = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count - 1;
                                #endregion 0

                                #region 多通道数据赋值
                                for (int iChn = 1; iChn <= iChnns; iChn++)
                                {
                                    strFeldName = "iChn_" + iChn.ToString();
                                    _sPara = DBConvert.ToString(dr[strFeldName]).Split('|');//厚度 | 色标（R/G/B）| 打标0或者1
                                    if (_sPara.Length >= 3)
                                    {
                                        _ArrThick.flArrThick[iChn - 1] = float.Parse(_sPara[0]);

                                        #region 运行过程厚度统计
                                        _flWc = g_Info.flNormal_Thickness - _ArrThick.flArrThick[iChn - 1];
                                        //1 统计：误差超过报警限
                                        if (Math.Abs(_flWc) >= float.Parse(g_Info.strThickAlarm))
                                        {
                                            _Alarm.m_Arr_WallThick[iChn - 1] = _ArrThick.flArrThick[iChn - 1];
                                            _Alarm.m_Arr_Label[iChn - 1] = _sPara[2];
                                            if (g_Info.m_LstAlarm != null)
                                                g_Info.m_LstAlarm.Add(_Alarm);
                                        }
                                        //2 统计：误差大于最小厚度限
                                        if (_ArrThick.flArrThick[iChn - 1] >= _flThick_Minlimit)// float.Parse(g_Info.strThickAlarm))// strMinThickLimit))
                                        {
                                            flAverage_Thickness += _ArrThick.flArrThick[iChn - 1];//累加总厚度
                                            iDataNums++;//累加数据个数

                                            if (g_Info.flThick_Min > _ArrThick.flArrThick[iChn - 1] || g_Info.flThick_Min == 0)
                                            {
                                                g_Info.flThick_Min = _ArrThick.flArrThick[iChn - 1];
                                                g_Info.flPosition_Min = _Infor.flDistance_X;
                                                g_Info.iThick_Min_Row = _Infor.Buff_iRows;
                                                g_Info.flPosition_Min_Y = _Infor.flDistance_Y;
                                                g_Info.iChn_Min = iChn;
                                                g_Info.flWc_Min = _flWc;
                                            }
                                            if (Math.Abs(g_Info.flThick_Max) < Math.Abs(_ArrThick.flArrThick[iChn - 1]))
                                            {
                                                g_Info.flThick_Max = _ArrThick.flArrThick[iChn - 1];
                                                g_Info.flPosition_Max = _Infor.flDistance_X;
                                                g_Info.iThick_Max_Row = _Infor.Buff_iRows;
                                                g_Info.flPosition_Max_Y = _Infor.flDistance_Y;
                                                g_Info.iChn_Max = iChn;
                                                g_Info.flWc_Max = _flWc;
                                            }
                                        }
                                        #endregion 运行过程厚度统计

                                        _ArrThick.strArrColor[iChn - 1] = _sPara[1];
                                        _ArrThick.strArrLabel[iChn - 1] = _sPara[2];
                                    }
                                }

                                if (_Infor.Buff_iRows != iRow)//第一个数 行号变化
                                {
                                    #region 上一行数据的末尾坐标值修改
                                    if (LstBuff_Distance_WallThick_Show.Count > 0)
                                    {
                                        iTmp = LstBuff_Distance_WallThick_Show.Count - 1;
                                        int _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;//多屏个数
                                        if (_iNum > 0)//多屏
                                        {
                                            //         LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                        }
                                    }
                                    #endregion 

                                    iRow = _Infor.Buff_iRows;
                                    ClassListDistanceThick_2 _NewData = new ClassListDistanceThick_2();
                                    int iNo_ = Plant_Para.Get_No(_ArrThick.flDistance_X);
                                    _ArrKd.flStar_Distance = Plant_Para.m_flArr_Rul_S[iNo_]; // GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                    _ArrKd.flEnd_Distance = Plant_Para.m_flArr_Rul_E[iNo_];// GetScreen_E (_ArrThick.flDistance_X, Plant_Para);//
                                    _ArrKd.iStar_BuffNo = 0;//开始序号
                                    _ArrKd.Trajectory_Type = _Infor.Trajectory_Type;

                                    _NewData.iCurrRowDataNums = 0;
                                    //厚度
                                    _NewData.LstChnns_Thick.Add(_ArrThick);
                                    //色标
                                    //  _NewData.LstChnns_Colr.Add(_ArrColor);
                                    //坐标
                                    _NewData.LstBuff_Ruler.Add(_ArrKd);
                                    //打标
                                    //如果打标则判断是否超过报警
                                    //   _NewData.LstChnns_Label.Add(_ArrLabel);
                                    LstBuff_Distance_WallThick_Show.Add(_NewData);
                                }
                                else//同行数据
                                {
                                    #region 6 现在行数据添加
                                    int iCol = LstBuff_Distance_WallThick_Show[iRow].iCurrRowDataNums;
                                    float flDistanceWc = _ArrThick.flDistance_X - LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].flDistance_X;

                                    #region 右边超出屏幕
                                    //if(flEnd<0)
                                    //flEnd = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);

                                    //if (iCurrDataRulerNo == 1 && _Infor.Trajectory_Type == 2)
                                    //{
                                    flEnd = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[iCurrDataRulerNo].flEnd_Distance;
                                    //}
                                    //else
                                    //    flEnd = GetScreen_End(iCurrDataRulerNo, Plant_Para);

                                    if (_ArrThick.flDistance_X > flEnd)//   GetScreen_End(iCurrDataRulerNo, Plant_Para))
                                    {
                                        //    flEnd = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);
                                        #region 7.2.1记录换屏前当前行对应缓存的开始结束位置
                                        int _iNum = 0;
                                        if (LstBuff_Distance_WallThick_Show.Count > 0)
                                            if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler != null)
                                            {
                                                _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;
                                                if (_iNum > 0)
                                                {
                                                    //   LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                                    LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                                }
                                            }
                                        #endregion  7.2.1
                                        #region 7.2.4 记录换屏后新的开始点
                                        if (_iNum > 0)
                                        {
                                            _ArrKd.iStar_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count;//新屏幕开始序号
                                                                                                                             //    _ArrKd.flStar_Distance = GetScreen_Start(_iNum, Plant_Para);

                                            //_ArrKd.flStar_Distance = GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                            //_ArrKd.flEnd_Distance = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);//


                                            int iNo_ = Plant_Para.Get_No(_ArrThick.flDistance_X);
                                            _ArrKd.flStar_Distance = Plant_Para.m_flArr_Rul_S[iNo_]; // GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                            _ArrKd.flEnd_Distance = Plant_Para.m_flArr_Rul_E[iNo_];// GetScreen_E (_ArrThick.flDistance_X, Plant_Para);//




                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Add(_ArrKd);
                                        }
                                        #endregion 7.2.4
                                    }
                                    #endregion

                                    #region 6.1直线不往返、圆形轨迹
                                    if (flDistanceWc > 0)//新数据点
                                    {
                                        LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Add(_ArrThick);
                                        //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Colr.Add(_ArrColor);
                                        //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Label.Add(_ArrLabel);
                                        LstBuff_Distance_WallThick_Show[iRow].iCurrRowDataNums++;
                                    }
                                    else if (flDistanceWc == 0)//x距离位置没有变化
                                    {
                                        for (int i = 0; i < iChnns; i++)
                                        {
                                            LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].flArrThick[i] = _ArrThick.flArrThick[i];
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].strArrColor[i] = _ArrColor.strArrColor[i];
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].strArrLabel[i] = _ArrLabel.strArrLabel[i];

                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Colr.Add(_ArrColor);
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Label.Add(_ArrLabel);
                                        }
                                    }
                                    else //有重复点：通过距离差找到当前点在链表大致位置
                                    {
                                    }
                                    #endregion 6.1
                                    #endregion 6
                                }

                                #endregion 单通道数据赋值
                                iGetDataNum++;
                                if (iGetDataNum > iWantGetRows) break;
                            }
                            if (iDataNums != 0)
                                flAverage_Thickness /= iDataNums;//平均厚度
                            g_Info.flAverage_Thickness = flAverage_Thickness;

                            if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count > 0)//最后一行的记录结束行末尾坐标
                            {
                                int _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;//多屏个数
                                if (_iNum > 0)//多屏
                                {
                                    // LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                    LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                }
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
                            iDataNum = dt_1.Rows.Count;
                            //2 循环拿记录
                            foreach (DataRow dr in dt_1.Rows)
                            {

                                Class_DataBae _Infor = new Struct.Class_DataBae(iChnns);
                                ClassChnn_Thick _ArrThick = new ClassChnn_Thick(iChnns);
                                //ClassChnn_Color _ArrColor = new ClassChnn_Color(iChnns);
                                //ClassChnn_Label _ArrLabel = new ClassChnn_Label(iChnns);

                                ClassKd _ArrKd = new ClassKd();

                                _Infor.Buff_iRows = DBConvert.ToInt32(dr["Buff_iRows"]);
                                if (iRow == -1 && _Infor.Buff_iRows != 0)
                                    iOtherRow = _Infor.Buff_iRows;
                                _Infor.Buff_iRows -= iOtherRow;

                                _Infor.flDistance_X = float.Parse(DBConvert.ToString(dr["flDistance_X"]));
                                _Infor.flDistance_Y = float.Parse(DBConvert.ToString(dr["flDistance_Y"]));
                                _Infor.VideoNo = int.Parse(DBConvert.ToString(dr["VideoNo"]));
                                if (iVideoAllNum < _Infor.VideoNo)
                                    iVideoAllNum = _Infor.VideoNo;
                                try
                                {
                                    _Infor.Trajectory_Type = int.Parse(DBConvert.ToString(dr["Trajectory_Type"]));
                                }
                                catch { }

                                _ArrThick.flDistance_Y = _Infor.flDistance_Y;
                                _ArrThick.flDistance_X = _Infor.flDistance_X;
                                _ArrThick.VideoNo = _Infor.VideoNo;
                                _ArrThick.Trajectory_Type = _Infor.Trajectory_Type;

                                #region 增加
                                if (Plant_Para.m_iRomoteNum == 2)
                                {
                                    #region 0  距离转换成图像的列和行号
                                    _i_Integer = 0;
                                    try
                                    {
                                        _i_Integer = (int)(_ArrThick.flDistance_X / Plant_Para.m_flArr_Rul_E[0]);//计算行号

                                    }
                                    catch { }
                                    _iT = (_ArrThick.flDistance_X % Plant_Para.m_flArr_Rul_E[0]);

                                    _iCurrRow = 0;//计算屏幕对应行号
                                    _iCurrPageNo = 0;//当前屏幕序号

                                    if (_iT == 0 && _ArrThick.flDistance_X > 0)//计算真实的屏幕序号，这个数是当前行最末一个数
                                        _iCurrRow = _i_Integer - 1;
                                    else
                                        _iCurrRow = _i_Integer;
                                    #endregion 0

                                    #region 1 屏幕数据记录
                                    if (Plant_Para.m_Page_Tc.m_iCurrPageNo == -1 || Plant_Para.m_Page_Tc.m_lstPage.Count == 0)
                                    {
                                        //添加第一屏页数据
                                        Class_Page _Page = new Class_Page();
                                        _Page.iStart_Col = _iTowChannel_iCol;
                                        _Page.iStart_Distan = _ArrThick.flDistance_X;

                                        Plant_Para.m_Page_Tc.m_lstPage.Add(_Page);
                                        Plant_Para.m_Page_Tc.m_iCurrPageNo = 0;
                                    }
                                    else//正常的前进后退测量
                                    {
                                        if (_iCurrRow > 1)
                                        {

                                        }
                                        if (_iCurrRow > 1 && _iCurrRow % Plant_Para.Scree_iAllRows_Browse[2] == 0)
                                            _iCurrPageNo = _iCurrRow / Plant_Para.Scree_iAllRows_Browse[2];
                                        if (Plant_Para.Ck_Gsc == 1)
                                        {
                                            _iCurrPageNo = 0;
                                            Plant_Para.Scree_iAllRows_Browse[2] = 85;
                                        }
                                        if (Plant_Para.m_Page_Tc.m_iCurrPageNo == _iCurrPageNo)//同一屏幕数据如果开始数据有变化
                                        {
                                            if (_ArrThick.flDistance_X < Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Distan)//后退
                                            {
                                                Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Distan = _ArrThick.flDistance_X;
                                                Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Col = _iTowChannel_iCol;
                                            }
                                        }
                                        else//换屏了：前进或者后退,开始位置有变化，就修改
                                        {
                                            if (Plant_Para.m_Page_Tc.m_iCurrPageNo < _iCurrPageNo)//前进
                                            {
                                                #region 1.1 前进测量
                                                if (Plant_Para.m_Page_Tc.m_lstPage.Count > Plant_Para.m_Page_Tc.m_iCurrPageNo + 1)
                                                {
                                                    #region 1.1.1如果下一屏已经有数据，则要清屏和画老数据
                                                    ////清屏
                                                    //GetRulerPara_C();//左通道清除
                                                    //Plant_Ruler_C();

                                                    //GetRulerPara_C_TowChannel();//右通道清除
                                                    //Plant_Ruler_C_TowChannel();
                                                    //// 画老数据
                                                    //Plant_TowChannel_Old(_iCurrPageNo, LstBuff_Distance_WallThick_Show);
                                                    #endregion 1.1
                                                }
                                                if (Plant_Para.m_Page_Tc.m_lstPage.Count == Plant_Para.m_Page_Tc.m_iCurrPageNo + 1)
                                                {
                                                    #region 1.1.2 如果下一屏没有数据，则只清屏
                                                    //添加新页
                                                    Class_Page _Page = new Class_Page();
                                                    _Page.iStart_Col = _iTowChannel_iCol;
                                                    _Page.iStart_Distan = _ArrThick.flDistance_X;

                                                    Plant_Para.m_Page_Tc.m_lstPage.Add(_Page);
                                                    ////清屏
                                                    //GetRulerPara_C();//左通道清除
                                                    //Plant_Ruler_C();

                                                    //GetRulerPara_C_TowChannel();//右通道清除
                                                    //Plant_Ruler_C_TowChannel();
                                                    #endregion 1.1.2
                                                }
                                                #endregion  1.1
                                            }
                                            else //后退
                                            {
                                                #region 1.2 后退测量，则要清屏和画老数据
                                                ////清屏
                                                //GetRulerPara_C();//左通道清除
                                                //Plant_Ruler_C();

                                                //GetRulerPara_C_TowChannel();//右通道清除
                                                //Plant_Ruler_C_TowChannel();
                                                //// 画老数据
                                                //Plant_TowChannel_Old(_iCurrPageNo, LstBuff_Distance_WallThick_Show);
                                                #endregion 1.2
                                            }
                                            Plant_Para.m_Page_Tc.m_iCurrPageNo = _iCurrPageNo;//改变屏幕当前运行序号

                                            //Chart_Run_iStartRow_Buff_TowChannel = _iCurrRow;
                                            //Chart_Run_iStart_Row_Browse[2] = _iCurrRow;
                                        }
                                    }
                                    #endregion 1 屏幕开始数据记录
                                }
                                _iTowChannel_iCol++;
                                #endregion  增加


                                #region 统计报警数据
                                ClassAlarm _Alarm = new Struct.ClassAlarm(iChnns);
                                _Alarm.iRow = _Infor.Buff_iRows;
                                _Alarm.flDistance_X = _ArrThick.flDistance_X;
                                _Alarm.flDistance_Y = _ArrThick.flDistance_Y;
                                #endregion 

                                #region 0 当前点尺子序号
                                if (LstBuff_Distance_WallThick_Show.Count > 0 && LstBuff_Distance_WallThick_Show.Count > iRow)
                                    if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count > 0)
                                        iCurrDataRulerNo = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count - 1;
                                #endregion 0

                                #region 多通道数据赋值
                                for (int iChn = 1; iChn <= iChnns; iChn++)
                                {
                                    strFeldName = "iChn_" + iChn.ToString();
                                    _sPara = DBConvert.ToString(dr[strFeldName]).Split('|');//厚度 | 色标（R/G/B）| 打标0或者1
                                    if (_sPara.Length == 3)
                                    {
                                        _ArrThick.flArrThick[iChn - 1] = float.Parse(_sPara[0]);

                                        #region 运行过程厚度统计
                                        _flWc = g_Info.flNormal_Thickness - _ArrThick.flArrThick[iChn - 1];
                                        //1 统计：误差超过报警限
                                        if (Math.Abs(_flWc) >= float.Parse(g_Info.strThickAlarm))
                                        {
                                            _Alarm.m_Arr_WallThick[iChn - 1] = _ArrThick.flArrThick[iChn - 1];
                                            _Alarm.m_Arr_Label[iChn - 1] = _sPara[2];
                                            if (g_Info.m_LstAlarm != null)
                                                g_Info.m_LstAlarm.Add(_Alarm);
                                        }
                                        //2 统计：误差大于最小厚度限
                                        if (_ArrThick.flArrThick[iChn - 1] >= _flThick_Minlimit)// float.Parse(g_Info.strThickAlarm))// strMinThickLimit))
                                        {
                                            flAverage_Thickness += _ArrThick.flArrThick[iChn - 1];//累加总厚度
                                            iDataNums++;//累加数据个数

                                            if (g_Info.flThick_Min > _ArrThick.flArrThick[iChn - 1] || g_Info.flThick_Min == 0)
                                            {
                                                g_Info.flThick_Min = _ArrThick.flArrThick[iChn - 1];
                                                g_Info.flPosition_Min = _Infor.flDistance_X;
                                                g_Info.iThick_Min_Row = _Infor.Buff_iRows;
                                                g_Info.flPosition_Min_Y = _Infor.flDistance_Y;
                                                g_Info.iChn_Min = iChn;
                                                g_Info.flWc_Min = _flWc;
                                            }
                                            if (Math.Abs(g_Info.flThick_Max) < Math.Abs(_ArrThick.flArrThick[iChn - 1]))
                                            {
                                                g_Info.flThick_Max = _ArrThick.flArrThick[iChn - 1];
                                                g_Info.flPosition_Max = _Infor.flDistance_X;
                                                g_Info.iThick_Max_Row = _Infor.Buff_iRows;
                                                g_Info.flPosition_Max_Y = _Infor.flDistance_Y;
                                                g_Info.iChn_Max = iChn;
                                                g_Info.flWc_Max = _flWc;
                                            }
                                        }
                                        #endregion 运行过程厚度统计

                                        _ArrThick.strArrColor[iChn - 1] = _sPara[1];
                                        _ArrThick.strArrLabel[iChn - 1] = _sPara[2];
                                    }
                                }

                                if (_Infor.Buff_iRows != iRow)//第一个数 行号变化
                                {
                                    #region 上一行数据的末尾坐标值修改
                                    if (LstBuff_Distance_WallThick_Show.Count > 0)
                                    {
                                        iTmp = LstBuff_Distance_WallThick_Show.Count - 1;
                                        int _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;//多屏个数
                                        if (_iNum > 0)//多屏
                                        {
                                            //         LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                        }
                                    }
                                    #endregion 

                                    iRow = _Infor.Buff_iRows;
                                    ClassListDistanceThick_2 _NewData = new ClassListDistanceThick_2();
                                    int iNo_ = Plant_Para.Get_No(_ArrThick.flDistance_X);
                                    _ArrKd.flStar_Distance = Plant_Para.m_flArr_Rul_S[iNo_]; // GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                    _ArrKd.flEnd_Distance = Plant_Para.m_flArr_Rul_E[iNo_];// GetScreen_E (_ArrThick.flDistance_X, Plant_Para);//
                                    _ArrKd.iStar_BuffNo = 0;//开始序号
                                    _ArrKd.Trajectory_Type = _Infor.Trajectory_Type;

                                    _NewData.iCurrRowDataNums = 0;
                                    //厚度
                                    _NewData.LstChnns_Thick.Add(_ArrThick);
                                    //色标
                                    //  _NewData.LstChnns_Colr.Add(_ArrColor);
                                    //坐标
                                    _NewData.LstBuff_Ruler.Add(_ArrKd);
                                    //打标
                                    //如果打标则判断是否超过报警
                                    //   _NewData.LstChnns_Label.Add(_ArrLabel);
                                    LstBuff_Distance_WallThick_Show.Add(_NewData);
                                }
                                else//同行数据
                                {
                                    #region 6 现在行数据添加
                                    int iCol = LstBuff_Distance_WallThick_Show[iRow].iCurrRowDataNums;
                                    float flDistanceWc = _ArrThick.flDistance_X - LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].flDistance_X;

                                    #region 右边超出屏幕
                                    //if(flEnd<0)
                                    //flEnd = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);

                                    //if (iCurrDataRulerNo == 1 && _Infor.Trajectory_Type == 2)
                                    //{
                                    flEnd = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[iCurrDataRulerNo].flEnd_Distance;
                                    //}
                                    //else
                                    //    flEnd = GetScreen_End(iCurrDataRulerNo, Plant_Para);

                                    if (_ArrThick.flDistance_X > flEnd)//   GetScreen_End(iCurrDataRulerNo, Plant_Para))
                                    {
                                        //    flEnd = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);
                                        #region 7.2.1记录换屏前当前行对应缓存的开始结束位置
                                        int _iNum = 0;
                                        if (LstBuff_Distance_WallThick_Show.Count > 0)
                                            if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler != null)
                                            {
                                                _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;
                                                if (_iNum > 0)
                                                {
                                                    //   LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                                    LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                                }
                                            }
                                        #endregion  7.2.1
                                        #region 7.2.4 记录换屏后新的开始点
                                        if (_iNum > 0)
                                        {
                                            _ArrKd.iStar_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count;//新屏幕开始序号
                                                                                                                             //    _ArrKd.flStar_Distance = GetScreen_Start(_iNum, Plant_Para);

                                            //_ArrKd.flStar_Distance = GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                            //_ArrKd.flEnd_Distance = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);//


                                            int iNo_ = Plant_Para.Get_No(_ArrThick.flDistance_X);
                                            _ArrKd.flStar_Distance = Plant_Para.m_flArr_Rul_S[iNo_]; // GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                            _ArrKd.flEnd_Distance = Plant_Para.m_flArr_Rul_E[iNo_];// GetScreen_E (_ArrThick.flDistance_X, Plant_Para);//




                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Add(_ArrKd);
                                        }
                                        #endregion 7.2.4
                                    }
                                    #endregion

                                    #region 6.1直线不往返、圆形轨迹
                                    if (flDistanceWc > 0)//新数据点
                                    {
                                        LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Add(_ArrThick);
                                        //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Colr.Add(_ArrColor);
                                        //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Label.Add(_ArrLabel);
                                        LstBuff_Distance_WallThick_Show[iRow].iCurrRowDataNums++;
                                    }
                                    else if (flDistanceWc == 0)//x距离位置没有变化
                                    {
                                        for (int i = 0; i < iChnns; i++)
                                        {
                                            LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].flArrThick[i] = _ArrThick.flArrThick[i];
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].strArrColor[i] = _ArrColor.strArrColor[i];
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].strArrLabel[i] = _ArrLabel.strArrLabel[i];

                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Colr.Add(_ArrColor);
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Label.Add(_ArrLabel);
                                        }
                                    }
                                    else //有重复点：通过距离差找到当前点在链表大致位置
                                    {
                                    }
                                    #endregion 6.1
                                    #endregion 6
                                }

                                #endregion 单通道数据赋值
                                iGetDataNum++;
                                if (iGetDataNum > iWantGetRows) break;
                            }

                            if (iDataNums != 0)
                                flAverage_Thickness /= iDataNums;//平均厚度
                            g_Info.flAverage_Thickness = flAverage_Thickness;

                            if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count > 0)//最后一行的记录结束行末尾坐标
                            {
                                int _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;//多屏个数
                                if (_iNum > 0)//多屏
                                {
                                    // LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                    LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                }
                            }
                        }
                        #endregion 1
                        break;
                }
            }
            catch (Exception e)
            {
                //  System.Windows.Forms.MessageBox.Show(e.Message);
            }
            return LstBuff_Distance_WallThick_Show;
        }
        public List<ClassListDistanceThick_2> GetData_Gsb(string strWhere, ref Class_Equipment_Info g_Info, Class_Plant Plant_Para,
                                                          ref int iDataNum, ref int iVideoAllNum, int iWantGetRows = 70000)
        {
            int iOtherRow = 0;
            int iRow = -1;//行号    
            int iTmp = -1;//临时
            string[] _sPara = "".Split(',');
            int iChnns = Plant_Para.m_iRomoteNum;// g_Info.iChnns;
            bool blHaveY = g_Info.iHaveY == 0 ? false : true;
            float _flWc = 0;
            g_Info.flThick_Min = 100; g_Info.flThick_Max = 0;
            float _flThick_Minlimit = g_Info.flNormal_Thickness * float.Parse(g_Info.strMinThickLimit) / 100;//最小厚度限
            List<ClassListDistanceThick_2> LstBuff_Distance_WallThick_Show = new List<Struct.ClassListDistanceThick_2>();

            int iCurrDataRulerNo = 0;
            int iDataNums = 0;
            float flAverage_Thickness = 0f;
            g_Info.flAverage_Thickness = 0;
            float flEnd = -1;
            try
            {
                int _i_Integer, _iCurrRow, _iCurrPageNo, _iTowChannel_iCol = 0;
                float _iT;
                int iGetDataNum = 0;
                string strSQL = "", strFeldName = "";
                //    if (iChnns == 1 || blHaveY == false)
                strSQL = "SELECT * from " + _DbTableName + " where  " + strWhere + " order by ID, Buff_iRows, flDistance_X,flDistance_Y";
                if (Plant_Para.m_Page_Tc.m_lstPage.Count > 0) Plant_Para.m_Page_Tc = new Class_Page_TowChannel();
                switch (_iDataBase_Type)
                {
                    case 0:
                        #region  0
                        DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            iDataNum = dt.Rows.Count;
                            //2 循环拿记录 ID,Buff_iRows,flDistance,iChn_";
                            foreach (DataRow dr in dt.Rows)
                            {
                                #region 数据
                                Class_DataBae _Infor = new Struct.Class_DataBae(iChnns);
                                ClassChnn_Thick _ArrThick = new ClassChnn_Thick(iChnns);
                                //ClassChnn_Color _ArrColor = new ClassChnn_Color(iChnns);
                                //ClassChnn_Label _ArrLabel = new ClassChnn_Label(iChnns);

                                ClassKd _ArrKd = new ClassKd();

                                _Infor.Buff_iRows = DBConvert.ToInt32(dr["Buff_iRows"]);
                                if (iRow == -1 && _Infor.Buff_iRows != 0)
                                    iOtherRow = _Infor.Buff_iRows;
                                _Infor.Buff_iRows -= iOtherRow;

                                _Infor.flDistance_X = float.Parse(DBConvert.ToString(dr["flDistance_X"]));
                                _Infor.flDistance_Y = float.Parse(DBConvert.ToString(dr["flDistance_Y"]));
                                _Infor.VideoNo = int.Parse(DBConvert.ToString(dr["VideoNo"]));
                                if (iVideoAllNum < _Infor.VideoNo)
                                    iVideoAllNum = _Infor.VideoNo;
                                try
                                {
                                    _Infor.Trajectory_Type = int.Parse(DBConvert.ToString(dr["Trajectory_Type"]));
                                }
                                catch { }

                                _ArrThick.flDistance_Y = _Infor.flDistance_Y;
                                _ArrThick.flDistance_X = _Infor.flDistance_X;
                                _ArrThick.VideoNo = _Infor.VideoNo;
                                _ArrThick.Trajectory_Type = _Infor.Trajectory_Type;

                                #region 增加
                                if (Plant_Para.m_iRomoteNum == 2)
                                {
                                    #region 0  距离转换成图像的列和行号
                                    _i_Integer = 0;
                                    try
                                    {
                                        _i_Integer = (int)(_ArrThick.flDistance_X / Plant_Para.m_flArr_Rul_E[0]);//计算行号

                                    }
                                    catch { }
                                    _iT = (_ArrThick.flDistance_X % Plant_Para.m_flArr_Rul_E[0]);

                                    _iCurrRow = 0;//计算屏幕对应行号
                                    _iCurrPageNo = 0;//当前屏幕序号

                                    if (_iT == 0 && _ArrThick.flDistance_X > 0)//计算真实的屏幕序号，这个数是当前行最末一个数
                                        _iCurrRow = _i_Integer - 1;
                                    else
                                        _iCurrRow = _i_Integer;
                                    #endregion 0

                                    #region 1 屏幕数据记录
                                    if (Plant_Para.m_Page_Tc.m_iCurrPageNo == -1 || Plant_Para.m_Page_Tc.m_lstPage.Count == 0)
                                    {
                                        //添加第一屏页数据
                                        Class_Page _Page = new Class_Page();
                                        _Page.iStart_Col = _iTowChannel_iCol;
                                        _Page.iStart_Distan = _ArrThick.flDistance_X;

                                        Plant_Para.m_Page_Tc.m_lstPage.Add(_Page);
                                        Plant_Para.m_Page_Tc.m_iCurrPageNo = 0;
                                    }
                                    else//正常的前进后退测量
                                    {
                                        if (_iCurrRow > 1)
                                        {

                                        }
                                        if (_iCurrRow > 1 && _iCurrRow % Plant_Para.Scree_iAllRows_Browse[2] == 0)
                                            _iCurrPageNo = _iCurrRow / Plant_Para.Scree_iAllRows_Browse[2];
                                        if (Plant_Para.Ck_Gsc == 1)
                                        {
                                            _iCurrPageNo = 0;
                                            Plant_Para.Scree_iAllRows_Browse[2] = 85;
                                        }
                                        if (Plant_Para.m_Page_Tc.m_iCurrPageNo == _iCurrPageNo)//同一屏幕数据如果开始数据有变化
                                        {
                                            if (_ArrThick.flDistance_X < Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Distan)//后退
                                            {
                                                Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Distan = _ArrThick.flDistance_X;
                                                Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Col = _iTowChannel_iCol;
                                            }
                                        }
                                        else//换屏了：前进或者后退,开始位置有变化，就修改
                                        {
                                            if (Plant_Para.m_Page_Tc.m_iCurrPageNo < _iCurrPageNo)//前进
                                            {
                                                #region 1.1 前进测量
                                                if (Plant_Para.m_Page_Tc.m_lstPage.Count > Plant_Para.m_Page_Tc.m_iCurrPageNo + 1)
                                                {
                                                    #region 1.1.1如果下一屏已经有数据，则要清屏和画老数据
                                                    ////清屏
                                                    //GetRulerPara_C();//左通道清除
                                                    //Plant_Ruler_C();

                                                    //GetRulerPara_C_TowChannel();//右通道清除
                                                    //Plant_Ruler_C_TowChannel();
                                                    //// 画老数据
                                                    //Plant_TowChannel_Old(_iCurrPageNo, LstBuff_Distance_WallThick_Show);
                                                    #endregion 1.1
                                                }
                                                if (Plant_Para.m_Page_Tc.m_lstPage.Count == Plant_Para.m_Page_Tc.m_iCurrPageNo + 1)
                                                {
                                                    #region 1.1.2 如果下一屏没有数据，则只清屏
                                                    //添加新页
                                                    Class_Page _Page = new Class_Page();
                                                    _Page.iStart_Col = _iTowChannel_iCol;
                                                    _Page.iStart_Distan = _ArrThick.flDistance_X;

                                                    Plant_Para.m_Page_Tc.m_lstPage.Add(_Page);
                                                    ////清屏
                                                    //GetRulerPara_C();//左通道清除
                                                    //Plant_Ruler_C();

                                                    //GetRulerPara_C_TowChannel();//右通道清除
                                                    //Plant_Ruler_C_TowChannel();
                                                    #endregion 1.1.2
                                                }
                                                #endregion  1.1
                                            }
                                            else //后退
                                            {
                                                #region 1.2 后退测量，则要清屏和画老数据
                                                ////清屏
                                                //GetRulerPara_C();//左通道清除
                                                //Plant_Ruler_C();

                                                //GetRulerPara_C_TowChannel();//右通道清除
                                                //Plant_Ruler_C_TowChannel();
                                                //// 画老数据
                                                //Plant_TowChannel_Old(_iCurrPageNo, LstBuff_Distance_WallThick_Show);
                                                #endregion 1.2
                                            }
                                            Plant_Para.m_Page_Tc.m_iCurrPageNo = _iCurrPageNo;//改变屏幕当前运行序号

                                            //Chart_Run_iStartRow_Buff_TowChannel = _iCurrRow;
                                            //Chart_Run_iStart_Row_Browse[2] = _iCurrRow;
                                        }
                                    }
                                    #endregion 1 屏幕开始数据记录
                                }
                                _iTowChannel_iCol++;
                                #endregion  增加


                                #region 统计报警数据
                                ClassAlarm _Alarm = new Struct.ClassAlarm(iChnns);
                                _Alarm.iRow = _Infor.Buff_iRows;
                                _Alarm.flDistance_X = _ArrThick.flDistance_X;
                                _Alarm.flDistance_Y = _ArrThick.flDistance_Y;
                                #endregion 

                                #region 0 当前点尺子序号
                                if (LstBuff_Distance_WallThick_Show.Count > 0 && LstBuff_Distance_WallThick_Show.Count > iRow)
                                    if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count > 0)
                                        iCurrDataRulerNo = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count - 1;
                                #endregion 0

                                #region 多通道数据赋值
                                for (int iChn = 1; iChn <= iChnns; iChn++)
                                {
                                    strFeldName = "iChn_" + iChn.ToString();
                                    _sPara = DBConvert.ToString(dr[strFeldName]).Split('|');//厚度 | 色标（R/G/B）| 打标0或者1
                                    if (_sPara.Length == 3)
                                    {
                                        _ArrThick.flArrThick[iChn - 1] = float.Parse(_sPara[0]);

                                        #region 运行过程厚度统计
                                        _flWc = g_Info.flNormal_Thickness - _ArrThick.flArrThick[iChn - 1];
                                        //1 统计：误差超过报警限
                                        if (Math.Abs(_flWc) >= float.Parse(g_Info.strThickAlarm))
                                        {
                                            _Alarm.m_Arr_WallThick[iChn - 1] = _ArrThick.flArrThick[iChn - 1];
                                            _Alarm.m_Arr_Label[iChn - 1] = _sPara[2];
                                            if (g_Info.m_LstAlarm != null)
                                                g_Info.m_LstAlarm.Add(_Alarm);
                                        }
                                        //2 统计：误差大于最小厚度限
                                        if (_ArrThick.flArrThick[iChn - 1] >= _flThick_Minlimit)// float.Parse(g_Info.strThickAlarm))// strMinThickLimit))
                                        {
                                            flAverage_Thickness += _ArrThick.flArrThick[iChn - 1];//累加总厚度
                                            iDataNums++;//累加数据个数

                                            if (g_Info.flThick_Min > _ArrThick.flArrThick[iChn - 1] || g_Info.flThick_Min == 0)
                                            {
                                                g_Info.flThick_Min = _ArrThick.flArrThick[iChn - 1];
                                                g_Info.flPosition_Min = _Infor.flDistance_X;
                                                g_Info.iThick_Min_Row = _Infor.Buff_iRows;
                                                g_Info.flPosition_Min_Y = _Infor.flDistance_Y;
                                                g_Info.iChn_Min = iChn;
                                                g_Info.flWc_Min = _flWc;
                                            }
                                            if (Math.Abs(g_Info.flThick_Max) < Math.Abs(_ArrThick.flArrThick[iChn - 1]))
                                            {
                                                g_Info.flThick_Max = _ArrThick.flArrThick[iChn - 1];
                                                g_Info.flPosition_Max = _Infor.flDistance_X;
                                                g_Info.iThick_Max_Row = _Infor.Buff_iRows;
                                                g_Info.flPosition_Max_Y = _Infor.flDistance_Y;
                                                g_Info.iChn_Max = iChn;
                                                g_Info.flWc_Max = _flWc;
                                            }
                                        }
                                        #endregion 运行过程厚度统计

                                        _ArrThick.strArrColor[iChn - 1] = _sPara[1];
                                        _ArrThick.strArrLabel[iChn - 1] = _sPara[2];
                                    }
                                }

                                if (_Infor.Buff_iRows != iRow)//第一个数 行号变化
                                {
                                    #region 上一行数据的末尾坐标值修改
                                    if (LstBuff_Distance_WallThick_Show.Count > 0)
                                    {
                                        iTmp = LstBuff_Distance_WallThick_Show.Count - 1;
                                        int _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;//多屏个数
                                        if (_iNum > 0)//多屏
                                        {
                                            //         LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                        }
                                    }
                                    #endregion 

                                    iRow = _Infor.Buff_iRows;
                                    ClassListDistanceThick_2 _NewData = new ClassListDistanceThick_2();
                                    int iNo_ = Plant_Para.Get_No(_ArrThick.flDistance_X);
                                    _ArrKd.flStar_Distance = Plant_Para.m_flArr_Rul_S[iNo_]; // GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                    _ArrKd.flEnd_Distance = Plant_Para.m_flArr_Rul_E[iNo_];// GetScreen_E (_ArrThick.flDistance_X, Plant_Para);//
                                    _ArrKd.iStar_BuffNo = 0;//开始序号
                                    _ArrKd.Trajectory_Type = _Infor.Trajectory_Type;

                                    _NewData.iCurrRowDataNums = 0;
                                    //厚度
                                    _NewData.LstChnns_Thick.Add(_ArrThick);
                                    //色标
                                    //  _NewData.LstChnns_Colr.Add(_ArrColor);
                                    //坐标
                                    _NewData.LstBuff_Ruler.Add(_ArrKd);
                                    //打标
                                    //如果打标则判断是否超过报警
                                    //   _NewData.LstChnns_Label.Add(_ArrLabel);
                                    LstBuff_Distance_WallThick_Show.Add(_NewData);
                                }
                                else//同行数据
                                {
                                    #region 6 现在行数据添加
                                    int iCol = LstBuff_Distance_WallThick_Show[iRow].iCurrRowDataNums;
                                    float flDistanceWc = _ArrThick.flDistance_X - LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].flDistance_X;

                                    #region 右边超出屏幕
                                    //if(flEnd<0)
                                    //flEnd = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);

                                    //if (iCurrDataRulerNo == 1 && _Infor.Trajectory_Type == 2)
                                    //{
                                    flEnd = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[iCurrDataRulerNo].flEnd_Distance;
                                    //}
                                    //else
                                    //    flEnd = GetScreen_End(iCurrDataRulerNo, Plant_Para);

                                    if (_ArrThick.flDistance_X > flEnd)//   GetScreen_End(iCurrDataRulerNo, Plant_Para))
                                    {
                                        //    flEnd = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);
                                        #region 7.2.1记录换屏前当前行对应缓存的开始结束位置
                                        int _iNum = 0;
                                        if (LstBuff_Distance_WallThick_Show.Count > 0)
                                            if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler != null)
                                            {
                                                _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;
                                                if (_iNum > 0)
                                                {
                                                    //   LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                                    LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                                }
                                            }
                                        #endregion  7.2.1
                                        #region 7.2.4 记录换屏后新的开始点
                                        if (_iNum > 0)
                                        {
                                            _ArrKd.iStar_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count;//新屏幕开始序号
                                                                                                                             //    _ArrKd.flStar_Distance = GetScreen_Start(_iNum, Plant_Para);

                                            //_ArrKd.flStar_Distance = GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                            //_ArrKd.flEnd_Distance = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);//


                                            int iNo_ = Plant_Para.Get_No(_ArrThick.flDistance_X);
                                            _ArrKd.flStar_Distance = Plant_Para.m_flArr_Rul_S[iNo_]; // GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                            _ArrKd.flEnd_Distance = Plant_Para.m_flArr_Rul_E[iNo_];// GetScreen_E (_ArrThick.flDistance_X, Plant_Para);//




                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Add(_ArrKd);
                                        }
                                        #endregion 7.2.4
                                    }
                                    #endregion

                                    #region 6.1直线不往返、圆形轨迹
                                    if (flDistanceWc > 0 || Plant_Para.iClimbType ==1)//新数据点
                                    {
                                        LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Add(_ArrThick);
                                        //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Colr.Add(_ArrColor);
                                        //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Label.Add(_ArrLabel);
                                        LstBuff_Distance_WallThick_Show[iRow].iCurrRowDataNums++;
                                    }
                                    else if (flDistanceWc == 0)//x距离位置没有变化
                                    {
                                        for (int i = 0; i < iChnns; i++)
                                        {
                                            LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].flArrThick[i] = _ArrThick.flArrThick[i];
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].strArrColor[i] = _ArrColor.strArrColor[i];
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].strArrLabel[i] = _ArrLabel.strArrLabel[i];

                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Colr.Add(_ArrColor);
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Label.Add(_ArrLabel);
                                        }
                                    }
                                    else //有重复点：通过距离差找到当前点在链表大致位置
                                    {
                                    }
                                    #endregion 6.1
                                    #endregion 6
                                }

                                #endregion 单通道数据赋值
                                iGetDataNum++;
                                if (iGetDataNum > iWantGetRows) break;
                                #endregion  
                            }
                            if (iDataNums != 0)
                                flAverage_Thickness /= iDataNums;//平均厚度
                            g_Info.flAverage_Thickness = flAverage_Thickness;

                            if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count > 0)//最后一行的记录结束行末尾坐标
                            {
                                int _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;//多屏个数
                                if (_iNum > 0)//多屏
                                {
                                    // LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                    LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                }
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
                            iDataNum = dt_1.Rows.Count;
                            //2 循环拿记录
                            foreach (DataRow dr in dt_1.Rows)
                            {

                                #region 数据
                                Class_DataBae _Infor = new Struct.Class_DataBae(iChnns);
                                ClassChnn_Thick _ArrThick = new ClassChnn_Thick(iChnns);
                                //ClassChnn_Color _ArrColor = new ClassChnn_Color(iChnns);
                                //ClassChnn_Label _ArrLabel = new ClassChnn_Label(iChnns);

                                ClassKd _ArrKd = new ClassKd();

                                _Infor.Buff_iRows = DBConvert.ToInt32(dr["Buff_iRows"]);
                                if (iRow == -1 && _Infor.Buff_iRows != 0)
                                    iOtherRow = _Infor.Buff_iRows;
                                _Infor.Buff_iRows -= iOtherRow;

                                _Infor.flDistance_X = float.Parse(DBConvert.ToString(dr["flDistance_X"]));
                                _Infor.flDistance_Y = float.Parse(DBConvert.ToString(dr["flDistance_Y"]));
                                _Infor.VideoNo = int.Parse(DBConvert.ToString(dr["VideoNo"]));
                                if (iVideoAllNum < _Infor.VideoNo)
                                    iVideoAllNum = _Infor.VideoNo;
                                try
                                {
                                    _Infor.Trajectory_Type = int.Parse(DBConvert.ToString(dr["Trajectory_Type"]));
                                }
                                catch { }

                                _ArrThick.flDistance_Y = _Infor.flDistance_Y;
                                _ArrThick.flDistance_X = _Infor.flDistance_X;
                                _ArrThick.VideoNo = _Infor.VideoNo;
                                _ArrThick.Trajectory_Type = _Infor.Trajectory_Type;

                                #region 增加
                                if (Plant_Para.m_iRomoteNum == 2)
                                {
                                    #region 0  距离转换成图像的列和行号
                                    _i_Integer = 0;
                                    try
                                    {
                                        _i_Integer = (int)(_ArrThick.flDistance_X / Plant_Para.m_flArr_Rul_E[0]);//计算行号

                                    }
                                    catch { }
                                    _iT = (_ArrThick.flDistance_X % Plant_Para.m_flArr_Rul_E[0]);

                                    _iCurrRow = 0;//计算屏幕对应行号
                                    _iCurrPageNo = 0;//当前屏幕序号

                                    if (_iT == 0 && _ArrThick.flDistance_X > 0)//计算真实的屏幕序号，这个数是当前行最末一个数
                                        _iCurrRow = _i_Integer - 1;
                                    else
                                        _iCurrRow = _i_Integer;
                                    #endregion 0

                                    #region 1 屏幕数据记录
                                    if (Plant_Para.m_Page_Tc.m_iCurrPageNo == -1 || Plant_Para.m_Page_Tc.m_lstPage.Count == 0)
                                    {
                                        //添加第一屏页数据
                                        Class_Page _Page = new Class_Page();
                                        _Page.iStart_Col = _iTowChannel_iCol;
                                        _Page.iStart_Distan = _ArrThick.flDistance_X;

                                        Plant_Para.m_Page_Tc.m_lstPage.Add(_Page);
                                        Plant_Para.m_Page_Tc.m_iCurrPageNo = 0;
                                    }
                                    else//正常的前进后退测量
                                    {
                                        if (_iCurrRow > 1)
                                        {

                                        }
                                        if (_iCurrRow > 1 && _iCurrRow % Plant_Para.Scree_iAllRows_Browse[2] == 0)
                                            _iCurrPageNo = _iCurrRow / Plant_Para.Scree_iAllRows_Browse[2];
                                        if (Plant_Para.Ck_Gsc == 1)
                                        {
                                            _iCurrPageNo = 0;
                                            Plant_Para.Scree_iAllRows_Browse[2] = 85;
                                        }
                                        if (Plant_Para.m_Page_Tc.m_iCurrPageNo == _iCurrPageNo)//同一屏幕数据如果开始数据有变化
                                        {
                                            if (_ArrThick.flDistance_X < Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Distan)//后退
                                            {
                                                Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Distan = _ArrThick.flDistance_X;
                                                Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Col = _iTowChannel_iCol;
                                            }
                                        }
                                        else//换屏了：前进或者后退,开始位置有变化，就修改
                                        {
                                            if (Plant_Para.m_Page_Tc.m_iCurrPageNo < _iCurrPageNo)//前进
                                            {
                                                #region 1.1 前进测量
                                                if (Plant_Para.m_Page_Tc.m_lstPage.Count > Plant_Para.m_Page_Tc.m_iCurrPageNo + 1)
                                                {
                                                    #region 1.1.1如果下一屏已经有数据，则要清屏和画老数据
                                                    ////清屏
                                                    //GetRulerPara_C();//左通道清除
                                                    //Plant_Ruler_C();

                                                    //GetRulerPara_C_TowChannel();//右通道清除
                                                    //Plant_Ruler_C_TowChannel();
                                                    //// 画老数据
                                                    //Plant_TowChannel_Old(_iCurrPageNo, LstBuff_Distance_WallThick_Show);
                                                    #endregion 1.1
                                                }
                                                if (Plant_Para.m_Page_Tc.m_lstPage.Count == Plant_Para.m_Page_Tc.m_iCurrPageNo + 1)
                                                {
                                                    #region 1.1.2 如果下一屏没有数据，则只清屏
                                                    //添加新页
                                                    Class_Page _Page = new Class_Page();
                                                    _Page.iStart_Col = _iTowChannel_iCol;
                                                    _Page.iStart_Distan = _ArrThick.flDistance_X;

                                                    Plant_Para.m_Page_Tc.m_lstPage.Add(_Page);
                                                    ////清屏
                                                    //GetRulerPara_C();//左通道清除
                                                    //Plant_Ruler_C();

                                                    //GetRulerPara_C_TowChannel();//右通道清除
                                                    //Plant_Ruler_C_TowChannel();
                                                    #endregion 1.1.2
                                                }
                                                #endregion  1.1
                                            }
                                            else //后退
                                            {
                                                #region 1.2 后退测量，则要清屏和画老数据
                                                ////清屏
                                                //GetRulerPara_C();//左通道清除
                                                //Plant_Ruler_C();

                                                //GetRulerPara_C_TowChannel();//右通道清除
                                                //Plant_Ruler_C_TowChannel();
                                                //// 画老数据
                                                //Plant_TowChannel_Old(_iCurrPageNo, LstBuff_Distance_WallThick_Show);
                                                #endregion 1.2
                                            }
                                            Plant_Para.m_Page_Tc.m_iCurrPageNo = _iCurrPageNo;//改变屏幕当前运行序号

                                            //Chart_Run_iStartRow_Buff_TowChannel = _iCurrRow;
                                            //Chart_Run_iStart_Row_Browse[2] = _iCurrRow;
                                        }
                                    }
                                    #endregion 1 屏幕开始数据记录
                                }
                                _iTowChannel_iCol++;
                                #endregion  增加


                                #region 统计报警数据
                                ClassAlarm _Alarm = new Struct.ClassAlarm(iChnns);
                                _Alarm.iRow = _Infor.Buff_iRows;
                                _Alarm.flDistance_X = _ArrThick.flDistance_X;
                                _Alarm.flDistance_Y = _ArrThick.flDistance_Y;
                                #endregion 

                                #region 0 当前点尺子序号
                                if (LstBuff_Distance_WallThick_Show.Count > 0 && LstBuff_Distance_WallThick_Show.Count > iRow)
                                    if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count > 0)
                                        iCurrDataRulerNo = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count - 1;
                                #endregion 0

                                #region 多通道数据赋值
                                for (int iChn = 1; iChn <= iChnns; iChn++)
                                {
                                    strFeldName = "iChn_" + iChn.ToString();
                                    _sPara = DBConvert.ToString(dr[strFeldName]).Split('|');//厚度 | 色标（R/G/B）| 打标0或者1
                                    if (_sPara.Length == 3)
                                    {
                                        _ArrThick.flArrThick[iChn - 1] = float.Parse(_sPara[0]);

                                        #region 运行过程厚度统计
                                        _flWc = g_Info.flNormal_Thickness - _ArrThick.flArrThick[iChn - 1];
                                        //1 统计：误差超过报警限
                                        if (Math.Abs(_flWc) >= float.Parse(g_Info.strThickAlarm))
                                        {
                                            _Alarm.m_Arr_WallThick[iChn - 1] = _ArrThick.flArrThick[iChn - 1];
                                            _Alarm.m_Arr_Label[iChn - 1] = _sPara[2];
                                            if (g_Info.m_LstAlarm != null)
                                                g_Info.m_LstAlarm.Add(_Alarm);
                                        }
                                        //2 统计：误差大于最小厚度限
                                        if (_ArrThick.flArrThick[iChn - 1] >= _flThick_Minlimit)// float.Parse(g_Info.strThickAlarm))// strMinThickLimit))
                                        {
                                            flAverage_Thickness += _ArrThick.flArrThick[iChn - 1];//累加总厚度
                                            iDataNums++;//累加数据个数

                                            if (g_Info.flThick_Min > _ArrThick.flArrThick[iChn - 1] || g_Info.flThick_Min == 0)
                                            {
                                                g_Info.flThick_Min = _ArrThick.flArrThick[iChn - 1];
                                                g_Info.flPosition_Min = _Infor.flDistance_X;
                                                g_Info.iThick_Min_Row = _Infor.Buff_iRows;
                                                g_Info.flPosition_Min_Y = _Infor.flDistance_Y;
                                                g_Info.iChn_Min = iChn;
                                                g_Info.flWc_Min = _flWc;
                                            }
                                            if (Math.Abs(g_Info.flThick_Max) < Math.Abs(_ArrThick.flArrThick[iChn - 1]))
                                            {
                                                g_Info.flThick_Max = _ArrThick.flArrThick[iChn - 1];
                                                g_Info.flPosition_Max = _Infor.flDistance_X;
                                                g_Info.iThick_Max_Row = _Infor.Buff_iRows;
                                                g_Info.flPosition_Max_Y = _Infor.flDistance_Y;
                                                g_Info.iChn_Max = iChn;
                                                g_Info.flWc_Max = _flWc;
                                            }
                                        }
                                        #endregion 运行过程厚度统计

                                        _ArrThick.strArrColor[iChn - 1] = _sPara[1];
                                        _ArrThick.strArrLabel[iChn - 1] = _sPara[2];
                                    }
                                }

                                if (_Infor.Buff_iRows != iRow)//第一个数 行号变化
                                {
                                    #region 上一行数据的末尾坐标值修改
                                    if (LstBuff_Distance_WallThick_Show.Count > 0)
                                    {
                                        iTmp = LstBuff_Distance_WallThick_Show.Count - 1;
                                        int _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;//多屏个数
                                        if (_iNum > 0)//多屏
                                        {
                                            //         LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                        }
                                    }
                                    #endregion 

                                    iRow = _Infor.Buff_iRows;
                                    ClassListDistanceThick_2 _NewData = new ClassListDistanceThick_2();
                                    int iNo_ = Plant_Para.Get_No(_ArrThick.flDistance_X);
                                    _ArrKd.flStar_Distance = Plant_Para.m_flArr_Rul_S[iNo_]; // GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                    _ArrKd.flEnd_Distance = Plant_Para.m_flArr_Rul_E[iNo_];// GetScreen_E (_ArrThick.flDistance_X, Plant_Para);//
                                    _ArrKd.iStar_BuffNo = 0;//开始序号
                                    _ArrKd.Trajectory_Type = _Infor.Trajectory_Type;

                                    _NewData.iCurrRowDataNums = 0;
                                    //厚度
                                    _NewData.LstChnns_Thick.Add(_ArrThick);
                                    //色标
                                    //  _NewData.LstChnns_Colr.Add(_ArrColor);
                                    //坐标
                                    _NewData.LstBuff_Ruler.Add(_ArrKd);
                                    //打标
                                    //如果打标则判断是否超过报警
                                    //   _NewData.LstChnns_Label.Add(_ArrLabel);
                                    LstBuff_Distance_WallThick_Show.Add(_NewData);
                                }
                                else//同行数据
                                {
                                    #region 6 现在行数据添加
                                    int iCol = LstBuff_Distance_WallThick_Show[iRow].iCurrRowDataNums;
                                    float flDistanceWc = _ArrThick.flDistance_X - LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].flDistance_X;

                                    #region 右边超出屏幕
                                    //if(flEnd<0)
                                    //flEnd = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);

                                    //if (iCurrDataRulerNo == 1 && _Infor.Trajectory_Type == 2)
                                    //{
                                    flEnd = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[iCurrDataRulerNo].flEnd_Distance;
                                    //}
                                    //else
                                    //    flEnd = GetScreen_End(iCurrDataRulerNo, Plant_Para);

                                    if (_ArrThick.flDistance_X > flEnd)//   GetScreen_End(iCurrDataRulerNo, Plant_Para))
                                    {
                                        //    flEnd = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);
                                        #region 7.2.1记录换屏前当前行对应缓存的开始结束位置
                                        int _iNum = 0;
                                        if (LstBuff_Distance_WallThick_Show.Count > 0)
                                            if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler != null)
                                            {
                                                _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;
                                                if (_iNum > 0)
                                                {
                                                    //   LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                                    LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                                }
                                            }
                                        #endregion  7.2.1
                                        #region 7.2.4 记录换屏后新的开始点
                                        if (_iNum > 0)
                                        {
                                            _ArrKd.iStar_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count;//新屏幕开始序号
                                                                                                                             //    _ArrKd.flStar_Distance = GetScreen_Start(_iNum, Plant_Para);
                                            int iNo_ = Plant_Para.Get_No(_ArrThick.flDistance_X);
                                            _ArrKd.flStar_Distance = Plant_Para.m_flArr_Rul_S[iNo_]; // GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                            _ArrKd.flEnd_Distance = Plant_Para.m_flArr_Rul_E[iNo_];// GetScreen_E (_ArrThick.flDistance_X, Plant_Para);//

                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Add(_ArrKd);
                                        }
                                        #endregion 7.2.4
                                    }
                                    #endregion

                                    #region 6.1直线不往返、圆形轨迹
                                    if (flDistanceWc > 0 || Plant_Para.iClimbType == 1)//新数据点
                                    {
                                        LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Add(_ArrThick);
                                        //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Colr.Add(_ArrColor);
                                        //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Label.Add(_ArrLabel);
                                        LstBuff_Distance_WallThick_Show[iRow].iCurrRowDataNums++;
                                    }
                                    else if (flDistanceWc == 0)//x距离位置没有变化
                                    {
                                        for (int i = 0; i < iChnns; i++)
                                        {
                                            LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].flArrThick[i] = _ArrThick.flArrThick[i];
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].strArrColor[i] = _ArrColor.strArrColor[i];
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].strArrLabel[i] = _ArrLabel.strArrLabel[i];

                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Colr.Add(_ArrColor);
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Label.Add(_ArrLabel);
                                        }
                                    }
                                    else //有重复点：通过距离差找到当前点在链表大致位置
                                    {
                                    }
                                    #endregion 6.1
                                    #endregion 6
                                }

                                #endregion 单通道数据赋值
                                iGetDataNum++;
                                if (iGetDataNum > iWantGetRows) break;
                                #endregion  
                            }
                            if (iDataNums != 0)
                                flAverage_Thickness /= iDataNums;//平均厚度
                            g_Info.flAverage_Thickness = flAverage_Thickness;

                            if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count > 0)//最后一行的记录结束行末尾坐标
                            {
                                int _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;//多屏个数
                                if (_iNum > 0)//多屏
                                {
                                    // LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                    LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                }
                            }
                        }
                        #endregion 1
                        break;
                }
            }
            catch (Exception e)
            {
                //  System.Windows.Forms.MessageBox.Show(e.Message);
            }
            return LstBuff_Distance_WallThick_Show;
        }
        /// <summary>
        /// C扫检测项目个数
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public int Get_iCurrBuffNums(string strWhere)
        {
            int iRetData = -1;

            try
            {
                string strSQL = "";
                strSQL = "SELECT DISTINCT Buff_iRows from " + _DbTableName + " where  " + strWhere + " order by  Buff_iRows desc";
                switch (_iDataBase_Type)
                {
                    case 0:
                        #region  0
                        DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //2 循环拿记录 ID,Buff_iRows,flDistance,iChn_";
                            foreach (DataRow dr in dt.Rows)
                            {
                                iRetData = DBConvert.ToInt32(dr["Buff_iRows"]);
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
                                iRetData = DBConvert.ToInt32(dr["Buff_iRows"]);
                                break;
                            }
                        }
                        #endregion 1
                        break;
                }
            }
            catch (Exception e)
            {
                //  System.Windows.Forms.MessageBox.Show(e.Message);
            }
            return iRetData+1;
        }

        /// <summary>
        /// 查询获得指定多个距离厚度数据  ClassStand g_Stand
        /// </summary>
        /// <param name="strWhere"> 根据单位名称、受检设备名、设备地址查询</param>
        /// <param name="iChnns">通道数</param>
        /// <param name="blHaveY">是否有Y轴编码器</param>
        /// <param name="iWantGetRows"></param>
        /// <returns></returns>
        public List<ClassListDistanceThick_2> GetData_ModyLimit(string strWhere, ref Class_Equipment_Info g_Info, Class_Plant Plant_Para,ref ClassStand g_Stand,
                                                      ref int iDataNum, ref int iVideoAllNum, bool blGetModyData, int iWantGetRows = 70000)
        {
            int iOtherRow = 0;
            int iRow = -1;//行号    
            int iTmp = -1;//临时
            string[] _sPara = "".Split(',');
            int iChnns = Plant_Para.m_iRomoteNum;// g_Info.iChnns;
            bool blHaveY = g_Info.iHaveY == 0 ? false : true;
            float _flWc = 0;
            g_Info.flThick_Min = 100; g_Info.flThick_Max = 0;
            float _flThick_Minlimit = g_Info.flNormal_Thickness * float.Parse(g_Info.strMinThickLimit) / 100;//最小厚度限
              List<ClassListDistanceThick_2> LstBuff_Distance_WallThick_Show = new List<Struct.ClassListDistanceThick_2>();

            int iCurrDataRulerNo = 0;
            int iDataNums = 0;
            float flAverage_Thickness = 0f;
            g_Info.flAverage_Thickness = 0;
            float flEnd = -1;
            int R=0, G=0, B = 0;//重新计算误差颜色
            int _iVideo_S = -1;
            iVideoAllNum = -1;
            try
            {
                int _i_Integer, _iCurrRow, _iCurrPageNo, _iTowChannel_iCol = 0;
                float _iT;
                int iGetDataNum = 0;
                string strSQL = "", strFeldName = "";
                //    if (iChnns == 1 || blHaveY == false)
                strSQL = "SELECT * from " + _DbTableName + " where  " + strWhere + " order by ID, Buff_iRows,  flDistance_X";
                if (Plant_Para.m_Page_Tc.m_lstPage.Count > 0) Plant_Para.m_Page_Tc = new Class_Page_TowChannel();
                //else
                //    strSQL = "SELECT * from " + _DbTableName + " where  " + strWhere + " order by ID,  flDistance_Y, flDistance_X";
                switch (_iDataBase_Type)
                {
                    case 0:
                        #region  0
                        DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);
                        
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            g_Stand.m_iAllDataNum = dt.Rows.Count;
                            iDataNum = dt.Rows.Count;
                            //2 循环拿记录 ID,Buff_iRows,flDistance,iChn_";
                            foreach (DataRow dr in dt.Rows)
                            {
                                Class_DataBae _Infor = new Struct.Class_DataBae(iChnns);
                                ClassChnn_Thick _ArrThick = new ClassChnn_Thick(iChnns);
                                //ClassChnn_Color _ArrColor = new ClassChnn_Color(iChnns);
                                //ClassChnn_Label _ArrLabel = new ClassChnn_Label(iChnns);

                                ClassKd _ArrKd = new ClassKd();

                                _Infor.Buff_iRows = DBConvert.ToInt32(dr["Buff_iRows"]);
                                if (iRow == -1 && _Infor.Buff_iRows != 0)
                                    iOtherRow = _Infor.Buff_iRows;
                                _Infor.Buff_iRows -= iOtherRow;

                                _Infor.flDistance_X = float.Parse(DBConvert.ToString(dr["flDistance_X"]));
                                _Infor.flDistance_Y = float.Parse(DBConvert.ToString(dr["flDistance_Y"]));
                                _Infor.VideoNo = int.Parse(DBConvert.ToString(dr["VideoNo"]));
                                if (iVideoAllNum < _Infor.VideoNo)
                                    iVideoAllNum = _Infor.VideoNo;
                                if(_iVideo_S==-1) _iVideo_S= _Infor.VideoNo;
                                try
                                {
                                    _Infor.Trajectory_Type = int.Parse(DBConvert.ToString(dr["Trajectory_Type"]));
                                }
                                catch { }

                                _ArrThick.flDistance_Y = _Infor.flDistance_Y;
                                _ArrThick.flDistance_X = _Infor.flDistance_X;
                                _ArrThick.VideoNo = _Infor.VideoNo;
                                _ArrThick.Trajectory_Type = _Infor.Trajectory_Type;

                                #region 增加
                                if (Plant_Para.m_iRomoteNum == 2)
                                {
                                    #region 0  距离转换成图像的列和行号
                                    _i_Integer = 0;
                                    try
                                    {
                                        _i_Integer = (int)(_ArrThick.flDistance_X / Plant_Para.m_flArr_Rul_E[0]);//计算行号

                                    }
                                    catch { }
                                    _iT = (_ArrThick.flDistance_X % Plant_Para.m_flArr_Rul_E[0]);

                                    _iCurrRow = 0;//计算屏幕对应行号
                                    _iCurrPageNo = 0;//当前屏幕序号

                                    if (_iT == 0 && _ArrThick.flDistance_X > 0)//计算真实的屏幕序号，这个数是当前行最末一个数
                                        _iCurrRow = _i_Integer - 1;
                                    else
                                        _iCurrRow = _i_Integer;
                                    #endregion 0

                                    #region 1 屏幕数据记录
                                    if (Plant_Para.m_Page_Tc.m_iCurrPageNo == -1 || Plant_Para.m_Page_Tc.m_lstPage.Count == 0)
                                    {
                                        //添加第一屏页数据
                                        Class_Page _Page = new Class_Page();
                                        _Page.iStart_Col = _iTowChannel_iCol;
                                        _Page.iStart_Distan = _ArrThick.flDistance_X;

                                        Plant_Para.m_Page_Tc.m_lstPage.Add(_Page);
                                        Plant_Para.m_Page_Tc.m_iCurrPageNo = 0;
                                    }
                                    else//正常的前进后退测量
                                    {
                                        if (_iCurrRow > 1)
                                        {

                                        }
                                        if (_iCurrRow > 1 && _iCurrRow % Plant_Para.Scree_iAllRows_Browse[2] == 0)
                                            _iCurrPageNo = _iCurrRow / Plant_Para.Scree_iAllRows_Browse[2];
                                        if (Plant_Para.Ck_Gsc == 1)
                                        {
                                            _iCurrPageNo = 0;
                                            Plant_Para.Scree_iAllRows_Browse[2] = 85;
                                        }
                                        if (Plant_Para.m_Page_Tc.m_iCurrPageNo == _iCurrPageNo)//同一屏幕数据如果开始数据有变化
                                        {
                                            if (_ArrThick.flDistance_X < Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Distan)//后退
                                            {
                                                Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Distan = _ArrThick.flDistance_X;
                                                Plant_Para.m_Page_Tc.m_lstPage[_iCurrPageNo].iStart_Col = _iTowChannel_iCol;
                                            }
                                        }
                                        else//换屏了：前进或者后退,开始位置有变化，就修改
                                        {
                                            if (Plant_Para.m_Page_Tc.m_iCurrPageNo < _iCurrPageNo)//前进
                                            {
                                                #region 1.1 前进测量
                                                if (Plant_Para.m_Page_Tc.m_lstPage.Count > Plant_Para.m_Page_Tc.m_iCurrPageNo + 1)
                                                {
                                                    #region 1.1.1如果下一屏已经有数据，则要清屏和画老数据
                                                    ////清屏
                                                    //GetRulerPara_C();//左通道清除
                                                    //Plant_Ruler_C();

                                                    //GetRulerPara_C_TowChannel();//右通道清除
                                                    //Plant_Ruler_C_TowChannel();
                                                    //// 画老数据
                                                    //Plant_TowChannel_Old(_iCurrPageNo, LstBuff_Distance_WallThick_Show);
                                                    #endregion 1.1
                                                }
                                                if (Plant_Para.m_Page_Tc.m_lstPage.Count == Plant_Para.m_Page_Tc.m_iCurrPageNo + 1)
                                                {
                                                    #region 1.1.2 如果下一屏没有数据，则只清屏
                                                    //添加新页
                                                    Class_Page _Page = new Class_Page();
                                                    _Page.iStart_Col = _iTowChannel_iCol;
                                                    _Page.iStart_Distan = _ArrThick.flDistance_X;

                                                    Plant_Para.m_Page_Tc.m_lstPage.Add(_Page);
                                                    ////清屏
                                                    //GetRulerPara_C();//左通道清除
                                                    //Plant_Ruler_C();

                                                    //GetRulerPara_C_TowChannel();//右通道清除
                                                    //Plant_Ruler_C_TowChannel();
                                                    #endregion 1.1.2
                                                }
                                                #endregion  1.1
                                            }
                                            else //后退
                                            {
                                                #region 1.2 后退测量，则要清屏和画老数据
                                                ////清屏
                                                //GetRulerPara_C();//左通道清除
                                                //Plant_Ruler_C();

                                                //GetRulerPara_C_TowChannel();//右通道清除
                                                //Plant_Ruler_C_TowChannel();
                                                //// 画老数据
                                                //Plant_TowChannel_Old(_iCurrPageNo, LstBuff_Distance_WallThick_Show);
                                                #endregion 1.2
                                            }
                                            Plant_Para.m_Page_Tc.m_iCurrPageNo = _iCurrPageNo;//改变屏幕当前运行序号

                                            //Chart_Run_iStartRow_Buff_TowChannel = _iCurrRow;
                                            //Chart_Run_iStart_Row_Browse[2] = _iCurrRow;
                                        }
                                    }
                                    #endregion 1 屏幕开始数据记录
                                }
                                _iTowChannel_iCol++;
                                #endregion  增加


                                #region 统计报警数据
                                ClassAlarm _Alarm = new Struct.ClassAlarm(iChnns);
                                _Alarm.iRow = _Infor.Buff_iRows;
                                _Alarm.flDistance_X = _ArrThick.flDistance_X;
                                _Alarm.flDistance_Y = _ArrThick.flDistance_Y;
                                #endregion 

                                #region 0 当前点尺子序号
                                if (LstBuff_Distance_WallThick_Show.Count > 0 && LstBuff_Distance_WallThick_Show.Count > iRow)
                                    if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count > 0)
                                        iCurrDataRulerNo = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count - 1;
                                #endregion 0

                                #region 多通道数据赋值
                                for (int iChn = 1; iChn <= iChnns; iChn++)
                                {
                                    if (blGetModyData == false && iChn == 1 && DBConvert.ToString(dr["Wave_2"]) != "")
                                        strFeldName = "Wave_2";
                                    else
                                        strFeldName = "iChn_" + iChn.ToString();
                                    _sPara = DBConvert.ToString(dr[strFeldName]).Split('|');//厚度 | 色标（R/G/B）| 打标0或者1
                                    if (_sPara.Length == 3)
                                    {
                                        _ArrThick.flArrThick[iChn - 1] = float.Parse(_sPara[0]);

                                        #region 运行过程厚度统计
                                        _flWc = g_Info.flNormal_Thickness - _ArrThick.flArrThick[iChn - 1];


                                        #region 误差统计
                                        for (int i = 0; i < 8; i++)
                                        {
                                            if (Math.Abs(_flWc) >= g_Stand.m_flArrLimit[i])
                                                g_Stand.m_iArrCurrLimitDataNum[i]++;
                                        }

                                        #endregion  误差统计

                                        //1 统计：误差超过报警限
                                        if (Math.Abs(_flWc) >= float.Parse(g_Info.strThickAlarm))
                                        {
                                            _Alarm.m_Arr_WallThick[iChn - 1] = _ArrThick.flArrThick[iChn - 1];
                                            _Alarm.m_Arr_Label[iChn - 1] = _sPara[2];
                                            if (g_Info.m_LstAlarm != null)
                                                g_Info.m_LstAlarm.Add(_Alarm);
                                        }
                                        //2 统计：误差大于最小厚度限
                                        if (_ArrThick.flArrThick[iChn - 1] >= _flThick_Minlimit)// float.Parse(g_Info.strThickAlarm))// strMinThickLimit))
                                        {
                                            flAverage_Thickness += _ArrThick.flArrThick[iChn - 1];//累加总厚度
                                            iDataNums++;//累加数据个数

                                            if (g_Info.flThick_Min > _ArrThick.flArrThick[iChn - 1] || g_Info.flThick_Min == 0)
                                            {
                                                g_Info.flThick_Min = _ArrThick.flArrThick[iChn - 1];
                                                g_Info.flPosition_Min = _Infor.flDistance_X;
                                                g_Info.iThick_Min_Row = _Infor.Buff_iRows;
                                                g_Info.flPosition_Min_Y = _Infor.flDistance_Y;
                                                g_Info.iChn_Min = iChn;
                                                g_Info.flWc_Min = _flWc;
                                            }
                                            if (Math.Abs(g_Info.flThick_Max) < Math.Abs(_ArrThick.flArrThick[iChn - 1]))
                                            {
                                                g_Info.flThick_Max = _ArrThick.flArrThick[iChn - 1];
                                                g_Info.flPosition_Max = _Infor.flDistance_X;
                                                g_Info.iThick_Max_Row = _Infor.Buff_iRows;
                                                g_Info.flPosition_Max_Y = _Infor.flDistance_Y;
                                                g_Info.iChn_Max = iChn;
                                                g_Info.flWc_Max = _flWc;
                                            }
                                        }
                                        #endregion 运行过程厚度统计
                                        g_Stand.CalculThickColor(_flWc, out R, out G, out B);
                                        _ArrThick.strArrColor[iChn - 1] = R.ToString() + "/" + G.ToString() + "/" + B.ToString();
                                        _ArrThick.strArrLabel[iChn - 1] = _sPara[2];
                                    }
                                }

                                if (_Infor.Buff_iRows != iRow)//第一个数 行号变化
                                {
                                    #region 上一行数据的末尾坐标值修改
                                    if (LstBuff_Distance_WallThick_Show.Count > 0)
                                    {
                                        iTmp = LstBuff_Distance_WallThick_Show.Count - 1;
                                        int _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;//多屏个数
                                        if (_iNum > 0)//多屏
                                        {
                                            //         LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                        }
                                    }
                                    #endregion 

                                    iRow = _Infor.Buff_iRows;
                                    ClassListDistanceThick_2 _NewData = new ClassListDistanceThick_2();
                                    int iNo_ = Plant_Para.Get_No(_ArrThick.flDistance_X);
                                    _ArrKd.flStar_Distance = Plant_Para.m_flArr_Rul_S[iNo_]; // GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                    _ArrKd.flEnd_Distance = Plant_Para.m_flArr_Rul_E[iNo_];// GetScreen_E (_ArrThick.flDistance_X, Plant_Para);//
                                    _ArrKd.iStar_BuffNo = 0;//开始序号
                                    _ArrKd.Trajectory_Type = _Infor.Trajectory_Type;

                                    _NewData.iCurrRowDataNums = 0;
                                    //厚度
                                    _NewData.LstChnns_Thick.Add(_ArrThick);
                                    //色标
                                    //  _NewData.LstChnns_Colr.Add(_ArrColor);
                                    //坐标
                                    _NewData.LstBuff_Ruler.Add(_ArrKd);
                                    //打标
                                    //如果打标则判断是否超过报警
                                    //   _NewData.LstChnns_Label.Add(_ArrLabel);
                                    LstBuff_Distance_WallThick_Show.Add(_NewData);
                                }
                                else//同行数据
                                {
                                    #region 6 现在行数据添加
                                    int iCol = LstBuff_Distance_WallThick_Show[iRow].iCurrRowDataNums;
                                    float flDistanceWc = _ArrThick.flDistance_X - LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].flDistance_X;

                                    #region 右边超出屏幕
                                    //if(flEnd<0)
                                    //flEnd = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);

                                    //if (iCurrDataRulerNo == 1 && _Infor.Trajectory_Type == 2)
                                    //{
                                    flEnd = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[iCurrDataRulerNo].flEnd_Distance;
                                    //}
                                    //else
                                    //    flEnd = GetScreen_End(iCurrDataRulerNo, Plant_Para);

                                    if (_ArrThick.flDistance_X > flEnd)//   GetScreen_End(iCurrDataRulerNo, Plant_Para))
                                    {
                                        //    flEnd = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);
                                        #region 7.2.1记录换屏前当前行对应缓存的开始结束位置
                                        int _iNum = 0;
                                        if (LstBuff_Distance_WallThick_Show.Count > 0)
                                            if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler != null)
                                            {
                                                _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;
                                                if (_iNum > 0)
                                                {
                                                    //   LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                                    LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                                }
                                            }
                                        #endregion  7.2.1
                                        #region 7.2.4 记录换屏后新的开始点
                                        if (_iNum > 0)
                                        {
                                            _ArrKd.iStar_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count;//新屏幕开始序号
                                                                                                                             //    _ArrKd.flStar_Distance = GetScreen_Start(_iNum, Plant_Para);

                                            //_ArrKd.flStar_Distance = GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                            //_ArrKd.flEnd_Distance = GetScreen_E(_ArrThick.flDistance_X, Plant_Para);//


                                            int iNo_ = Plant_Para.Get_No(_ArrThick.flDistance_X);
                                            _ArrKd.flStar_Distance = Plant_Para.m_flArr_Rul_S[iNo_]; // GetScreen_S(_ArrThick.flDistance_X, Plant_Para);// _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                            _ArrKd.flEnd_Distance = Plant_Para.m_flArr_Rul_E[iNo_];// GetScreen_E (_ArrThick.flDistance_X, Plant_Para);//




                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Add(_ArrKd);
                                        }
                                        #endregion 7.2.4
                                    }
                                    #endregion

                                    #region 6.1直线不往返、圆形轨迹
                                    if (flDistanceWc > 0)//新数据点
                                    {
                                        LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Add(_ArrThick);
                                        //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Colr.Add(_ArrColor);
                                        //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Label.Add(_ArrLabel);
                                        LstBuff_Distance_WallThick_Show[iRow].iCurrRowDataNums++;
                                    }
                                    else if (flDistanceWc == 0)//x距离位置没有变化
                                    {
                                        for (int i = 0; i < iChnns; i++)
                                        {
                                            LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].flArrThick[i] = _ArrThick.flArrThick[i];
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].strArrColor[i] = _ArrColor.strArrColor[i];
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].strArrLabel[i] = _ArrLabel.strArrLabel[i];

                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Colr.Add(_ArrColor);
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Label.Add(_ArrLabel);
                                        }
                                    }
                                    else //有重复点：通过距离差找到当前点在链表大致位置
                                    {
                                    }
                                    #endregion 6.1
                                    #endregion 6
                                }

                                #endregion 单通道数据赋值
                                iGetDataNum++;
                                if (iGetDataNum > iWantGetRows) break;
                            }
                            if (iDataNums != 0)
                                flAverage_Thickness /= iDataNums;//平均厚度
                            g_Info.flAverage_Thickness = flAverage_Thickness;

                            if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count > 0)//最后一行的记录结束行末尾坐标
                            {
                                int _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;//多屏个数
                                if (_iNum > 0)//多屏
                                {
                                    // LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                    LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                }
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
                            iDataNum = dt_1.Rows.Count;
                            //2 循环拿记录
                            foreach (DataRow dr in dt_1.Rows)
                            {
                                Class_DataBae _Infor = new Struct.Class_DataBae(iChnns);
                                ClassChnn_Thick _ArrThick = new ClassChnn_Thick(iChnns);
                                //ClassChnn_Color _ArrColor = new ClassChnn_Color(iChnns);
                                //ClassChnn_Label _ArrLabel = new ClassChnn_Label(iChnns);

                                ClassKd _ArrKd = new ClassKd();

                                _Infor.Buff_iRows = DBConvert.ToInt32(dr["Buff_iRows"]);
                                _Infor.flDistance_X = float.Parse(DBConvert.ToString(dr["flDistance_X"]));
                                _Infor.flDistance_Y = float.Parse(DBConvert.ToString(dr["flDistance_Y"]));
                                _ArrThick.flDistance_Y = _Infor.flDistance_Y;
                                _ArrThick.flDistance_X = _Infor.flDistance_X;

                                #region 0 当前点尺子序号

                                if (LstBuff_Distance_WallThick_Show.Count > 0 && LstBuff_Distance_WallThick_Show.Count > iRow)
                                    if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count > 0)
                                        iCurrDataRulerNo = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count - 1;
                                #endregion 0


                                #region 多通道数据赋值
                                for (int iChn = 1; iChn <= iChnns; iChn++)
                                {
                                    strFeldName = "iChn_" + iChn.ToString();
                                    _sPara = DBConvert.ToString(dr[strFeldName]).Split('|');//厚度 | 色标（R/G/B）| 打标0或者1
                                    if (_sPara.Length == 3)
                                    {
                                        _ArrThick.flArrThick[iChn - 1] = float.Parse(_sPara[0]);

                                        #region 运行过程厚度统计
                                        _flWc = g_Info.flNormal_Thickness - _ArrThick.flArrThick[iChn - 1];
                                        if (_ArrThick.flArrThick[iChn - 1] > _flThick_Minlimit)// float.Parse(g_Info.strThickAlarm))// strMinThickLimit))
                                        {
                                            flAverage_Thickness += _Infor.flDistance_X;//累加总厚度
                                            iDataNums++;

                                            if (g_Info.flThick_Min > _ArrThick.flArrThick[iChn - 1] || g_Info.flThick_Min == 0)
                                            {
                                                g_Info.flThick_Min = _ArrThick.flArrThick[iChn - 1];
                                                g_Info.flPosition_Min = _Infor.flDistance_X;
                                                g_Info.flPosition_Min_Y = _Infor.flDistance_Y;
                                                g_Info.iChn_Min = iChn;
                                                g_Info.flWc_Min = _flWc;
                                            }
                                            if (Math.Abs(g_Info.flThick_Max) < Math.Abs(_ArrThick.flArrThick[iChn - 1]))
                                            {
                                                g_Info.flThick_Max = _ArrThick.flArrThick[iChn - 1];
                                                g_Info.flPosition_Max = _Infor.flDistance_X;
                                                g_Info.flPosition_Max_Y = _Infor.flDistance_Y;
                                                g_Info.iChn_Max = iChn;
                                                g_Info.flWc_Max = _flWc;
                                            }
                                        }
                                        #endregion 运行过程厚度统计


                                        _ArrThick.strArrColor[iChn - 1] = _sPara[1];
                                        _ArrThick.strArrLabel[iChn - 1] = _sPara[2];
                                    }
                                }

                                if (_Infor.Buff_iRows != iRow)//第一个数 行号变化
                                {
                                    #region 上一行数据的末尾坐标值修改

                                    if (LstBuff_Distance_WallThick_Show.Count > 0)
                                    {
                                        iTmp = LstBuff_Distance_WallThick_Show.Count - 1;
                                        int _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;//多屏个数
                                        if (_iNum > 0)//多屏
                                        {
                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                        }
                                    }
                                    #endregion 

                                    iRow = _Infor.Buff_iRows;
                                    ClassListDistanceThick_2 _NewData = new ClassListDistanceThick_2();

                                    _ArrKd.flStar_Distance = _ArrThick.flDistance_X <= Plant_Para.Scree_iDotWithmm_X ? 0 : _ArrThick.flDistance_X;//开始位置
                                    _ArrKd.iStar_BuffNo = 0;//开始序号

                                    _NewData.iCurrRowDataNums = 0;
                                    //厚度
                                    _NewData.LstChnns_Thick.Add(_ArrThick);
                                    //色标
                                    //_NewData.LstChnns_Colr.Add(_ArrColor);
                                    //坐标
                                    _NewData.LstBuff_Ruler.Add(_ArrKd);
                                    //打标
                                    //如果打标则判断是否超过报警
                                    //   _NewData.LstChnns_Label.Add(_ArrLabel);
                                    LstBuff_Distance_WallThick_Show.Add(_NewData);
                                }
                                else//同行数据
                                {
                                    #region 6 现在行数据添加
                                    int iCol = LstBuff_Distance_WallThick_Show[iRow].iCurrRowDataNums;
                                    float flDistanceWc = _ArrThick.flDistance_X - LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].flDistance_X;

                                    #region 右边超出屏幕
                                    if (_ArrThick.flDistance_X > GetScreen_End(iCurrDataRulerNo, Plant_Para))
                                    {
                                        #region 7.2.1记录换屏前当前行对应缓存的开始结束位置
                                        int _iNum = 0;
                                        if (LstBuff_Distance_WallThick_Show.Count > 0)
                                            if (LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler != null)
                                            {
                                                _iNum = LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Count;
                                                if (_iNum > 0)
                                                {
                                                    LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].flEnd_Distance = GetScreen_End(_iNum - 1, Plant_Para);//结束位置
                                                    LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler[_iNum - 1].iEnd_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count - 1;//结束序号
                                                }
                                            }
                                        #endregion  7.2.1
                                        #region 7.2.4 记录换屏后新的开始点
                                        if (_iNum > 0)
                                        {
                                            _ArrKd.iStar_BuffNo = LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Count;//新屏幕开始序号
                                            _ArrKd.flStar_Distance = GetScreen_Start(_iNum, Plant_Para);
                                            LstBuff_Distance_WallThick_Show[iRow].LstBuff_Ruler.Add(_ArrKd);
                                        }
                                        #endregion 7.2.4
                                    }
                                    #endregion

                                    #region 6.1直线不往返、圆形轨迹
                                    if (flDistanceWc > 0)//新数据点
                                    {
                                        LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick.Add(_ArrThick);
                                        //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Colr.Add(_ArrColor);
                                        //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Label.Add(_ArrLabel);
                                        LstBuff_Distance_WallThick_Show[iRow].iCurrRowDataNums++;
                                    }
                                    else if (flDistanceWc == 0)//x距离位置没有变化
                                    {
                                        for (int i = 0; i < iChnns; i++)
                                        {
                                            LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].flArrThick[i] = _ArrThick.flArrThick[i];
                                            LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].strArrColor[i] = _ArrThick.strArrColor[i];
                                            LstBuff_Distance_WallThick_Show[iRow].LstChnns_Thick[iCol].strArrLabel[i] = _ArrThick.strArrLabel[i];

                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Colr.Add(_ArrColor);
                                            //LstBuff_Distance_WallThick_Show[iRow].LstChnns_Label.Add(_ArrLabel);
                                        }
                                    }
                                    else //有重复点：通过距离差找到当前点在链表大致位置
                                    {
                                    }
                                    #endregion 6.1
                                    #endregion 6
                                }

                                #endregion 单通道数据赋值
                                iGetDataNum++;
                                if (iGetDataNum > iWantGetRows) break;
                            }
                        }
                        #endregion 1
                        break;
                }
            }
            catch (Exception e)
            {
                //  System.Windows.Forms.MessageBox.Show(e.Message);
            }

            iVideoAllNum -= _iVideo_S;
            return LstBuff_Distance_WallThick_Show;
        }

        /// <summary>
        /// 查询记录个数
        /// </summary>
        /// <param name="strWhere"> 根据单位名称、受检设备名、设备地址查询</param>
        /// <param name="iChnns">通道数</param>
        /// <param name="blHaveY">是否有Y轴编码器</param>
        /// <param name="iWantGetRows"></param>
        /// <returns></returns>
        public int  GetData_Num(string strWhere,  Class_Equipment_Info g_Info)
        {
            string[] _sPara = "".Split(',');
            int iNum = 0;
            int iChnns = g_Info.iChnns;
            bool blHaveY = g_Info.iHaveY == 0 ? false : true;
           
            List<ClassListDistanceThick_2> LstBuff_Distance_WallThick_Show = new List<Struct.ClassListDistanceThick_2>();

            try
            {
                
                string strSQL = "";
                //if (iChnns == 1 || blHaveY == false)
                    strSQL = "SELECT * from " + _DbTableName + " where  " + strWhere + " order by ID, Buff_iRows,  flDistance_X";
                //else
                //    strSQL = "SELECT * from " + _DbTableName + " where  " + strWhere + " order by ID,  flDistance_Y, flDistance_X";
                switch (_iDataBase_Type)
                {
                    case 0:
                        #region  0
                        DataTable dt = OleDbHelper.ExecuteDataTable(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, DbTableName, null);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            iNum = dt.Rows.Count;
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
                            iNum = dt_1.Rows.Count;
                        }
                        #endregion 1
                        break;
                }
            }
            catch (Exception e)
            {
              //  System.Windows.Forms.MessageBox.Show(e.Message);
            }
            return iNum;
        }
    }
}
