using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;//数据库
using System.Data.OleDb;//数据库类
using ClassLib_TestData;
using ClassLib_DataMang.DataBaseMang.DBUtility;//操作数据库

namespace ClassLib_DataMang.DataBaseMang.OleDal
{
    public class Test_Statistical_Report
    {

        /// <summary>
        /// 操作的数据表名称 Test_Statistical_Report
        /// </summary>
        private static string _DbTableName = "Test_Statistical_Report";
        /// <summary>
        /// 指定数据时的操作表名称 Test_Statistical_Report
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
                              " Part_No    varchar(50) NOT  NULL , " +//部位编号

                              " fl_Start_Distance    float  NULL , " +//开始位置
                              " fl_End_Distance    float  NULL , " +//开始位置
                              (_iDataBase_Type == 0 ? " i_DefectLengt    Integer  NULL , " : "i_DefectLengt  tinyint  NULL, ");//缺陷长度
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
            { System.Windows.Forms.MessageBox.Show("Test_Statistical_Report.cs  Creat_Table error：" + e.Message + e.StackTrace + " " + strSQL); }
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
        /// <param name="Sub_ID"></param>
        /// <returns></returns>
        public bool DeleteDat_ForID(string ID,string Sub_ID)
        {
            bool _blRet = false;//操作返回
            string strSQL = "DELETE   from  " + _DbTableName + "  Where ID='" + ID + "'" +
                             " and Sub_ID='" + Sub_ID +"'";
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
        /// 保存数据到临时库 0:失败 1：覆盖 2：插入新的
        /// </summary>
        /// <param name="_Infor"></param>
        /// <returns>0:失败 1：覆盖 2：插入新的</returns>
        public int SaveData(ClassLib_TestData.Class_Test_Statistical_Report _Infor)
        {
            int iRet = 0;
            string strSQL = "";
            string strAdd_Val = "";//增加Sql值
            string strInsert_Add_Zd = "";//插入字段表
            string strUpdata_Add_Zd = "";//更新字符串
            try
            {
                if (_Infor.ID == "" || _Infor.Sub_ID == "") return 0;

                #region 插入字符串
                strInsert_Add_Zd = "ID,Sub_ID,Part_No," +
                             "strMarking,i_Defectdepth,i_DefectLengt,strData,strData_2," +
                             "fl_Start_Distance,fl_End_Distance," +
                             "i_DefectLengt";
                // "ReMark";

                strAdd_Val = "'" + _Infor.ID + "'";
                strAdd_Val += ",'" + _Infor.Sub_ID + "'";
                strAdd_Val += ",'" + _Infor.Part_No + "'";

                strAdd_Val += "," + _Infor.fl_Start_Distance;
                strAdd_Val += "," + _Infor.fl_End_Distance;
                strAdd_Val += "," + _Infor.i_DefectLengt;
                #endregion 插入字符串



                switch (_iDataBase_Type)
                {
                    case 0:
                        #region 0
                        try
                        {
                            strSQL = "insert into " + _DbTableName + "  (" + strInsert_Add_Zd + ") values(" + strAdd_Val + ")";
                            if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                iRet = 0;
                            else
                                iRet = 2;
                        }

                        catch (Exception e)
                        { }
                        #endregion 0
                        break;
                    case 1:
                        if (SqlDbHelper.m_DataBase == "")
                            SqlDbHelper.Init();
                        #region 1
                        strSQL = "insert into " + _DbTableName + "  (" + strInsert_Add_Zd + ") values(" + strAdd_Val + ")";
                        if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                            iRet = 0;
                        else
                            iRet = 2;

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
        public List<Class_Test_Statistical_Report > GetData(string ID, string Sub_ID)
        {
            List<Class_Test_Statistical_Report> _lst_Info = new List<Class_Test_Statistical_Report>();
            string strSQL = "SELECT * from " + _DbTableName + " where ID=' " + ID + "' and  Sub_ID='" + Sub_ID + "'";
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
                            Class_Test_Statistical_Report _Infor = new Class_Test_Statistical_Report();
                            #region 字段赋值
                            _Infor.ID = DBConvert.ToString(dr["ID"]);
                            _Infor.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                            _Infor.Part_No = DBConvert.ToString(dr["Part_No"]);

                            _Infor.fl_Start_Distance = float.Parse(DBConvert.ToString(dr["fl_Start_Distance"]));
                            _Infor.fl_End_Distance = float.Parse(DBConvert.ToString(dr["fl_End_Distance"]));
                            _Infor.i_DefectLengt = DBConvert.ToInt32(dr["i_DefectLengt"]);
                            
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
                            Class_Test_Statistical_Report _Infor = new Class_Test_Statistical_Report();
                            #region 字段赋值
                            _Infor.ID = DBConvert.ToString(dr["ID"]);
                            _Infor.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                            _Infor.Part_No = DBConvert.ToString(dr["Part_No"]);

                            _Infor.fl_Start_Distance = float.Parse(DBConvert.ToString(dr["fl_Start_Distance"]));
                            _Infor.fl_End_Distance = float.Parse(DBConvert.ToString(dr["fl_End_Distance"]));
                            _Infor.i_DefectLengt = DBConvert.ToInt32(dr["i_DefectLengt"]);

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
