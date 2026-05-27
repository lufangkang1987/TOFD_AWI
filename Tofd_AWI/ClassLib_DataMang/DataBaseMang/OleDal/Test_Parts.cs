using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;//数据库
using ClassLib_TestData;
using ClassLib_DataMang.DataBaseMang.DBUtility;//操作数据库

namespace ClassLib_DataMang.DataBaseMang.OleDal
{
    public class Test_Parts
    {
        /// <summary>
        /// 操作的数据表名称 Test_Parts_Info
        /// </summary>
        private static string _DbTableName = "Test_Parts_Info";
        /// <summary>
        /// 指定数据时的操作表名称 Test_Parts_Info
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
            string strSQL = "", strFileName = ""; 
            try
            {
                #region 字段表
                strFileName = " ID varchar(20)     NOT NULL," +//ID号 YYMMDDHHmm
                               "Sub_ID    varchar(20) NOT  NULL , " +// 焊缝ID号:  MMDDHHmmss
                              " Part_No    varchar(100) NOT  NULL , " +//部位编号
                              (_iDataBase_Type == 0 ? " DetectionSite    Integer  NULL , " : "DetectionSite tinyint  NULL, ") + //焊缝的1内侧/外侧0（例如球馆外部/内部焊缝）

                              " flThicknise    float  NULL , " +//壁厚
                              " ProbeSpacing    float  NULL , " +//探头间距
                              " Detection_Direction    text , " +//检测方向:可以使用编码，也可使用文字说明，检测编号起码（小数）在前、止码(大数)在后
                              " sPeed   float   NULL, " +//声速
                              " T0   float   NULL, " +//延时
                              " TOFD_Para  text ,  " +//通道参数
                          
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
            { System.Windows.Forms.MessageBox.Show("Manage_Test_Parts.cs的 Creat_Table error：" + e.Message + e.StackTrace + " " + strSQL); }
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

        public int UpData_Data(ref Class_Test_Parts  _Infor)
        {
            int iRet = 0;
            string strSQL = "";
            string strUpdata_Add_Zd = "";//更新字符串
            try
            {
                //1 查询
                //单位名称  受检设备名称  设备地址
                strSQL = "SELECT * from " + _DbTableName + " where ID= '" + _Infor.ID + "' and Sub_ID='" + _Infor.Sub_ID + "'";
                
                #region 插入数据
                if (_Infor.ID == "")
                {
                    _Infor.ID = DateTime.Now.ToString("yyMMddHHmmss");
                    iRet = 2;
                }
                strUpdata_Add_Zd = "SET ID='" + _Infor.ID + "'";
                strUpdata_Add_Zd += ",Sub_ID='" + _Infor.Sub_ID + "'";
                strUpdata_Add_Zd += ",DetectionSite=" + _Infor.DetectionSite;
                strUpdata_Add_Zd += ",Part_No='" + _Infor.Part_No + "'";

                strUpdata_Add_Zd += ",strThicknise=" + _Infor.flThicknise ;
                strUpdata_Add_Zd += ",ProbeSpacing=" + _Infor.ProbeSpacing ;
                strUpdata_Add_Zd += ",Detection_Direction='" + _Infor.Detection_Direction + "'";
                strUpdata_Add_Zd += ",sPeed=" + _Infor.sPeed ;

                strUpdata_Add_Zd += ",T0=" + _Infor.T0;
                strUpdata_Add_Zd += ",TOFD_Para='" + _Infor.TOFD_Para + "'";
                strUpdata_Add_Zd += ",strOther_1='" + _Infor.strOther1 + "'";
                strUpdata_Add_Zd += ",strOther_2='" + _Infor.strOther2 + "'";
                strUpdata_Add_Zd += ",iOther_1=" + _Infor.iOther1 ;
                strUpdata_Add_Zd += ",iOther_2=" + _Infor.iOther2 ;
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
                                    strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  ID= '" + _Infor.ID + "'  and Sub_ID='" + _Infor.Sub_ID + "'";

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
                                strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  ID= '" + _Infor.ID + "'  and Sub_ID='" + _Infor.Sub_ID + "'";

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
        public int SaveData(ref Class_Test_Parts  _Infor)
        {
            int iRet = 0;
            string strSQL = "",strWhere="";
            string strAdd_Val = "";//增加Sql值
            string strInsert_Add_Zd = "";//插入字段表
            string strUpdata_Add_Zd = "";//更新字符串
            try
            {
                if (_Infor.ID == "" || _Infor.Sub_ID == "") return 0;
                //1 查询
                //单位名称  受检设备名称  设备地址
                strWhere = " where ID= '" + _Infor.ID + "' and   Sub_ID='" + _Infor.Sub_ID + "'";
                strSQL = "SELECT * from " + _DbTableName + strWhere;


                #region 插入字符串
                strInsert_Add_Zd = "ID,Sub_ID,DetectionSite,Part_No," +
                             "flThicknise,ProbeSpacing,Detection_Direction,sPeed,T0,TOFD_Para," +
                             "strOther_1,strOther_2," +
                             "iOther_1,iOther_2," +
                             "ReMark";

                strAdd_Val = "'" + _Infor.ID + "'";
                strAdd_Val += ",'" + _Infor.Sub_ID + "'";
                strAdd_Val += "," + _Infor.DetectionSite;
                strAdd_Val += ",'" + _Infor.Part_No + "'";

                strAdd_Val += "," + _Infor.flThicknise ;
                strAdd_Val += "," + _Infor.ProbeSpacing ;
                strAdd_Val += ",'" + _Infor.Detection_Direction + "'";
                strAdd_Val += "," + _Infor.sPeed ;
                strAdd_Val += "," + _Infor.T0 ;
                strAdd_Val += ",'" + _Infor.TOFD_Para + "'";

                strAdd_Val += ",'" + _Infor.strOther1 + "'";
                strAdd_Val += ",'" + _Infor.strOther2 + "'";
                strAdd_Val += "," + _Infor.iOther1;
                strAdd_Val += "," + _Infor.iOther2;

                strAdd_Val += ",'" + _Infor.ReMark + "'";
                #endregion 插入字符串

                #region 插入数据
                strUpdata_Add_Zd = "SET ID='" + _Infor.ID + "'";
                strUpdata_Add_Zd += ",Sub_ID='" + _Infor.Sub_ID + "'";
                strUpdata_Add_Zd += ",DetectionSite=" + _Infor.DetectionSite;
                strUpdata_Add_Zd += ",Part_No='" + _Infor.Part_No + "'";

                strUpdata_Add_Zd += ",flThicknise=" + _Infor.flThicknise;
                strUpdata_Add_Zd += ",ProbeSpacing=" + _Infor.ProbeSpacing;
                strUpdata_Add_Zd += ",Detection_Direction='" + _Infor.Detection_Direction + "'";
                strUpdata_Add_Zd += ",sPeed=" + _Infor.sPeed;

                strUpdata_Add_Zd += ",T0=" + _Infor.T0;
                strUpdata_Add_Zd += ",TOFD_Para='" + _Infor.TOFD_Para + "'";
                strUpdata_Add_Zd += ",strOther_1='" + _Infor.strOther1 + "'";
                strUpdata_Add_Zd += ",strOther_2='" + _Infor.strOther2 + "'";
                strUpdata_Add_Zd += ",iOther_1=" + _Infor.iOther1;
                strUpdata_Add_Zd += ",iOther_2=" + _Infor.iOther2 ;
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
                                    strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + strWhere;

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
                                strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + strWhere;

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
        public bool GetData(ref Class_Test_Parts  _Infor)
        {
            bool _blRet = false;
            try
            {
                string strWhere = " where ID= '" + _Infor.ID + "' and Sub_ID='" + _Infor.Sub_ID + "'";
                string strSQL = "SELECT * from " + _DbTableName + strWhere;

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
                                _Infor.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                                _Infor.DetectionSite =int.Parse ( DBConvert.ToString(dr["DetectionSite"]));
                                _Infor.Part_No = DBConvert.ToString(dr["Part_No"]);

                                _Infor.flThicknise =float .Parse ( DBConvert.ToString(dr["flThicknise"]));
                                _Infor.ProbeSpacing = float.Parse(DBConvert.ToString(dr["ProbeSpacing"]));
                                _Infor.Detection_Direction = DBConvert.ToString(dr["Detection_Direction"]);
                                _Infor.sPeed = float .Parse ( DBConvert.ToString(dr["sPeed"]));
                                _Infor.T0 = float.Parse(DBConvert.ToString(dr["T0"]));
                                _Infor.TOFD_Para = DBConvert.ToString(dr["TOFD_Para"]);

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
                                _Infor.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                                _Infor.DetectionSite =int.Parse ( DBConvert.ToString(dr["DetectionSite"]));
                                _Infor.Part_No = DBConvert.ToString(dr["Part_No"]);

                                _Infor.flThicknise = float.Parse(DBConvert.ToString(dr["flThicknise"]));
                                _Infor.ProbeSpacing = float.Parse(DBConvert.ToString(dr["ProbeSpacing"]));
                                _Infor.Detection_Direction = DBConvert.ToString(dr["Detection_Direction"]);
                                _Infor.sPeed = float.Parse(DBConvert.ToString(dr["sPeed"]));
                                _Infor.T0 = float.Parse(DBConvert.ToString(dr["T0"]));
                                _Infor.TOFD_Para = DBConvert.ToString(dr["TOFD_Para"]);

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
        public List<Class_Test_Parts > GetData(string ID)
        {
            List<Class_Test_Parts > _lst_Info = new List<Class_Test_Parts >();
            string strSQL = "SELECT * from " + _DbTableName + "  where ID = '" + ID + "'  order by Sub_ID "; 
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
                            Class_Test_Parts  _Infor = new Class_Test_Parts ();
                            #region 字段赋值
                            _Infor.ID = DBConvert.ToString(dr["ID"]);
                            _Infor.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                            _Infor.DetectionSite = int.Parse(DBConvert.ToString(dr["DetectionSite"]));
                            _Infor.Part_No = DBConvert.ToString(dr["Part_No"]);

                            _Infor.flThicknise = float.Parse(DBConvert.ToString(dr["flThicknise"]));
                            _Infor.ProbeSpacing = float.Parse(DBConvert.ToString(dr["ProbeSpacing"]));
                            _Infor.Detection_Direction = DBConvert.ToString(dr["Detection_Direction"]);
                            _Infor.sPeed = float.Parse(DBConvert.ToString(dr["sPeed"]));
                            _Infor.T0 = float.Parse(DBConvert.ToString(dr["T0"]));
                            _Infor.TOFD_Para = DBConvert.ToString(dr["TOFD_Para"]);

                            _Infor.strOther1 = DBConvert.ToString(dr["strOther_1"]);
                            _Infor.strOther2 = DBConvert.ToString(dr["strOther_2"]);
                            _Infor.iOther1 = DBConvert.ToInt32(dr["iOther_1"]);
                            _Infor.iOther2 = DBConvert.ToInt32(dr["iOther_2"]);
                            _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);
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
                            Class_Test_Parts  _Infor = new Class_Test_Parts ();

                            #region 字段赋值
                            _Infor.ID = DBConvert.ToString(dr["ID"]);
                            _Infor.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                            _Infor.DetectionSite = int.Parse(DBConvert.ToString(dr["DetectionSite"]));
                            _Infor.Part_No = DBConvert.ToString(dr["Part_No"]);

                            _Infor.flThicknise = float.Parse(DBConvert.ToString(dr["flThicknise"]));
                            _Infor.ProbeSpacing = float.Parse(DBConvert.ToString(dr["ProbeSpacing"]));
                            _Infor.Detection_Direction = DBConvert.ToString(dr["Detection_Direction"]);
                            _Infor.sPeed = float.Parse(DBConvert.ToString(dr["sPeed"]));
                            _Infor.T0 = float.Parse(DBConvert.ToString(dr["T0"]));
                            _Infor.TOFD_Para = DBConvert.ToString(dr["TOFD_Para"]);

                            _Infor.strOther1 = DBConvert.ToString(dr["strOther_1"]);
                            _Infor.strOther2 = DBConvert.ToString(dr["strOther_2"]);
                            _Infor.iOther1 = DBConvert.ToInt32(dr["iOther_1"]);
                            _Infor.iOther2 = DBConvert.ToInt32(dr["iOther_2"]);
                            _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);
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
        /// 获得指定文件的记录
        /// </summary>
        /// <returns></returns>
        public Class_Test_Parts  GetData_One(string strSourcePathFileName)
        {
            string strOld_DT = "";
            Class_Test_Parts  _Infor = new Class_Test_Parts ();
            string strSQL = "SELECT * from " + _DbTableName + " Where 1=1";
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
                            _Infor.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                            _Infor.DetectionSite = int.Parse(DBConvert.ToString(dr["DetectionSite"]));
                            _Infor.Part_No = DBConvert.ToString(dr["Part_No"]);

                            _Infor.flThicknise = float.Parse(DBConvert.ToString(dr["flThicknise"]));
                            _Infor.ProbeSpacing = float.Parse(DBConvert.ToString(dr["ProbeSpacing"]));
                            _Infor.Detection_Direction = DBConvert.ToString(dr["Detection_Direction"]);
                            _Infor.sPeed = float.Parse(DBConvert.ToString(dr["sPeed"]));
                            _Infor.T0 = float.Parse(DBConvert.ToString(dr["T0"]));
                            _Infor.TOFD_Para = DBConvert.ToString(dr["TOFD_Para"]);

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
                    OleDbHelper.DbConnString_DT = strOld_DT;
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
                            _Infor.Sub_ID = DBConvert.ToString(dr["Sub_ID"]);
                            _Infor.DetectionSite = int.Parse(DBConvert.ToString(dr["DetectionSite"]));
                            _Infor.Part_No = DBConvert.ToString(dr["Part_No"]);

                            _Infor.flThicknise = float.Parse(DBConvert.ToString(dr["flThicknise"]));
                            _Infor.ProbeSpacing = float.Parse(DBConvert.ToString(dr["ProbeSpacing"]));
                            _Infor.Detection_Direction = DBConvert.ToString(dr["Detection_Direction"]);
                            _Infor.sPeed = float.Parse(DBConvert.ToString(dr["sPeed"]));
                            _Infor.T0 = float.Parse(DBConvert.ToString(dr["T0"]));
                            _Infor.TOFD_Para = DBConvert.ToString(dr["TOFD_Para"]);

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
                    SqlDbHelper.DbConnString_DT = strOld_DT;
                    break;
            }
            return _Infor;
        }
        /// <summary>
        /// 获得字段值集合
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public List<string> GetData_ByMySel(string strName)
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
        /// <summary>
        /// 获得字段值集合
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public List<string> GetData_ByMySel_Where(string strName, string strZdName, string strVal)
        {
            List<string> _lst_Info = new List<string>();
            string strSQL = "SELECT DISTINCT " + strName + " from " + _DbTableName + " where " + strZdName + " = '" + strVal + "'";
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
