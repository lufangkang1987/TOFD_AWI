using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Windows.Forms;

using FrameWork.Class;

namespace FrameWork.DataBaseMang.DBUtility
{
    /// <summary>
    /// 数据库的通用访问代码
    /// 此类为抽象类，不允许实例化，在应用时直接调用即可
    /// </summary>

    public abstract class SqlDbHelper
    {
        //获取数据库连接字符串，其属于静态变量且只读，项目中所有文档可以直接使用，但不能修改
        //此处暂不用配置文件，直接引用
        //DbConnString  //sql连接数据库字符串
        //localhost取代当前的计算机名。database表示所使用的数据库名
        //uid为指定的数据库用户名，pwd为指定的用户口令
        /// <summary>
        /// 总表信息存放路径，只保存总铭牌信息
        /// </summary>
        public static string strDbPath = Application.StartupPath + "\\database\\";
 

        /// <summary>
        /// 是否使用密码  0：不使用 1：使用
        /// </summary>
        public static string m_UsePassWord = "0";
        /// <summary>
        /// 服务器地址
        /// </summary>
        public static string m_Server = "";
        /// <summary>
        /// 数据库名
        /// </summary>
        public static string m_DataBase = "";
        /// <summary>
        /// 数据库名称
        /// </summary>
        public static string m_uid = "";
        /// <summary>
        /// 用户口令
        /// </summary>
        public static string m_pwd = "";
        /// <summary>
        /// sql连接字符串
        /// </summary>
        public static string DbConnString_DT = "server=localhost;database=" + m_DataBase + ";uid=" + m_uid + ";pwd=" + m_pwd + ";";

        public static string DbConnStringZD_FA = "Data Source=10.98.100.02;Initial Catalog=HY;User Id=SA;Password=123;";
        public static string DbConnStringZD_Sys = "Data Source=10.98.100.02;Initial Catalog=HY;User Id=SA;Password=123;";

        //哈希表用来存储缓存的参数信息，哈希表可以存储任意类型的参数
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());
        public static string ErrFile = Application.StartupPath + "ErrFile_" + DateTime.Now.ToString("yy-MM-dd ");
        /// <summary>
        /// 文件操作
        /// </summary>
        static csInterface m_csInter = new csInterface();
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            string strFileName = "SysConfig.ini";

            m_UsePassWord = m_csInter.IniReadDefine("SqlDbHelper", "m_UsePassWord", "0", strDbPath + strFileName);
            m_Server = m_csInter.IniReadDefine("SqlDbHelper", "m_Server", "DESKTOP-A2L3AJS", strDbPath + strFileName);
            m_DataBase = m_csInter.IniReadDefine("SqlDbHelper", "m_DataBase", "DellData", strDbPath + strFileName);
            m_uid = m_csInter.IniReadDefine("SqlDbHelper", "m_uid", "sa", strDbPath + strFileName);
            m_pwd = m_csInter.IniReadDefine("SqlDbHelper", "m_pwd", "cdw28", strDbPath + strFileName);
           // m_Server = "CDW-PC\\JIN_RUN";
            if (m_UsePassWord == "0")
                DbConnString_DT = "server=" + m_Server + ";database=" +  m_DataBase + ";Integrated Security = True;";
            else
                DbConnString_DT = "server=" + m_Server + ";database=" + m_DataBase + "; uid=" + m_uid + ";pwd=" + m_pwd + ";";
        }
        /// <summary>
        /// 创建新数据库
        /// </summary>
        public static void Init_Creat()
        {
            string strFileName = "SysConfig.ini";

            m_UsePassWord = m_csInter.IniReadDefine("SqlDbHelper", "m_UsePassWord", "0", strDbPath + strFileName);
            m_Server = m_csInter.IniReadDefine("SqlDbHelper", "m_Server", "localhost", strDbPath + strFileName);
            m_DataBase = m_csInter.IniReadDefine("SqlDbHelper", "m_DataBase_Creat", "master", strDbPath + strFileName);
            m_uid = m_csInter.IniReadDefine("SqlDbHelper", "m_uid", "sa", strDbPath + strFileName);
            m_pwd = m_csInter.IniReadDefine("SqlDbHelper", "m_pwd", "cdw28", strDbPath + strFileName);
            // m_Server = "CDW-PC\\JIN_RUN";
            if (m_UsePassWord == "0")
                DbConnString_DT = "server=" + m_Server + ";database=" + m_DataBase + ";Integrated Security = True;";
            else
                DbConnString_DT = "server=" + m_Server + ";database=" + m_DataBase + "; uid=" + m_uid + ";pwd=" + m_pwd + ";";
        }
        public static void W_Ini()
        {
            string strFileName = "SysConfig.ini";

            m_csInter.INIWriteValue("SqlDbHelper", "m_UsePassWord", m_UsePassWord, strDbPath + strFileName);
            m_csInter.INIWriteValue("SqlDbHelper", "m_Server", m_Server, strDbPath + strFileName);
            m_csInter.INIWriteValue("SqlDbHelper", "m_DataBase", m_DataBase, strDbPath + strFileName);
            m_csInter.INIWriteValue("SqlDbHelper", "m_uid", m_uid, strDbPath + strFileName);
            m_csInter.INIWriteValue("SqlDbHelper", "m_pwd", m_pwd, strDbPath + strFileName);

            if (m_UsePassWord == "0")
                DbConnString_DT = "server=" + m_Server + ";database=" + m_DataBase + ";Integrated Security = True;";
            else
                DbConnString_DT = "server=" + m_Server + ";database=" + m_DataBase + "; uid=" + m_uid + ";pwd=" + m_pwd + ";";
        }
        public static bool Jg_Conn(string connectionString)
        {
            bool _blRet = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    try
                    {
                        conn.Open();
                        _blRet = true;
                    }
                    catch(Exception ec) { }
                    
                }


            }
            return _blRet;
        }
        //private static  string serverip
        //{
        //    C StSysParm stSysParm = new Comm.Struct.StSysParm();

        //    stSysParm = IDAL.IUtility.SelectSysParm();

        //    //string tmpIp;
        //    if (!Comm.Function.Net.IsIP(stSysParm.ServerIP))
        //    {
        //        stSysParm.ServerIP = Comm.Function.Net.GetIpByHostName(stSysParm.ServerIP);

        //     }
        //}
        /// <summary>
        /// 执行一个不需要返回值的OleDbCommand命令，通过指定专用的连接字符串
        /// 使用参数数组形式提供参数列表
        /// </summary>
        /// <remarks>
        /// 使用示例：
        /// int result = ExecuteNonQuery(connString,CommandType.StoredProcedure,"PulishOrders",new OleDbParameter("@prodid",24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">OleDbCommand命令类型 (存储过程<StoredProcedure>，表的名称<TableDirect>， T-SQL 文本命令语句<Text>(默认)， 等等。)</param>
        /// <param name="cmdText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="commandParamters">以数组形式提供OleDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此OleDbCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParamters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //通过PrePareCommand方法将参数逐个加入到OleDbCommand的参数集合中
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParamters);
                    int val = cmd.ExecuteNonQuery();

                    //清空OleDbCommand中的参数列表
                    cmd.Parameters.Clear();
                    return val;
                }
            }
            catch (Exception Ex)
            {
                //数据库错误多为致命错误，所以暂时全部上抛处理.
     //           m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
                // throw Ex;
                return 0;
            }
        }

        /// <summary>
        /// 执行一个不需要返回值的OleDbCommand命令，通过一个已经存在的数据库连接
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：
        /// int result = ExecuteNonQuery(conn,CommandType.StoredProcedure,"PulishOrders",new OleDbParameter("@prodid",24));
        /// </remarks>
        /// <param name="connection">一个现有的数据库连接</param>
        /// <param name="cmdType">OleDbCommand命令类型 (存储过程<StoredProcedure>，表的名称<TableDirect>， T-SQL 文本命令语句<Text>(默认)， 等等。)</param>
        /// <param name="cmdText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="commandParameters">以数组形式提供OleDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此OleDbCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空OleDbCommand中的参数列表
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception Ex)
            {
                //数据库错误多为致命错误，所以暂时全部上抛处理.
      //          m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
                throw Ex;
            }
        }

        /// <summary>
        /// 执行一个不需要返回值的OleDbCommand命令，通过一个已经存在的数据库事务处理
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：
        /// int result = ExecuteNonQuery(trans,CommandType.StoredProcedure,"PulishOrders",new OleDbParameter("@prodid",24));
        /// </remarks>
        /// <param name="trans">一个存在的OleDb事务处理</param>
        /// <param name="cmdType">OleDbCommand命令类型 (存储过程<StoredProcedure>，表的名称<TableDirect>， T-SQL 文本命令语句<Text>(默认)， 等等。)</param>
        /// <param name="cmdText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="commandParameters">以数组形式提供OleDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此OleDbCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空OleDbCommand中的参数列表
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception Ex)
            {
                //数据库错误多为致命错误，所以暂时全部上抛处理.
     //           m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
                throw Ex;
            }
        }


        /// <summary>
        /// 执行一条返回结果集的OleDbCommand命令，通过专用的连接字符串
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">OleDbCommand命令类型 (存储过程<StoredProcedure>，表的名称<TableDirect>， T-SQL 文本命令语句<Text>(默认)， 等等。)</param>
        /// <param name="cmdText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="commandParameters">以数组形式提供OleDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个包含结果的SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlDataReader rdr = null;

            /**********
            在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
            CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            关闭数据库连接，并通过throw再次引发捕捉到的异常。
            **********/
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            /*
            catch (OleDbException dbEx)
            {
                conn.Close();
                throw dbEx;
            }
            */
            catch (Exception Ex)
            {
                conn.Close();
                //数据库错误多为致命错误，所以暂时全部上抛处理.
     //           m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
                throw Ex;
            }
        }

        public static DataSet Executedataset(string connectionString, CommandType cmdType, string cmdText, string dataname, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            //SqlDataReader rdr = null;

            /**********
            在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
            CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            关闭数据库连接，并通过throw再次引发捕捉到的异常。
            **********/
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataAdapter adp = new SqlDataAdapter();
                cmd.Connection = conn;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds, dataname);

                cmd.Parameters.Clear();


                return ds;
            }
            /*
            catch (OleDbException dbEx)
            {
                conn.Close();
                throw dbEx;
            }
            */
            catch (Exception Ex)
            {
                conn.Close();
                //数据库错误多为致命错误，所以暂时全部上抛处理.
      //          m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
                throw Ex;
            }
        }
        public static DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, string dataname, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            //SqlDataReader rdr = null;

            /**********
            在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
            CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            关闭数据库连接，并通过throw再次引发捕捉到的异常。
            **********/
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataAdapter adp = new SqlDataAdapter();
                cmd.Connection = conn;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds, dataname);

                cmd.Parameters.Clear();

                if (ds != null && ds.Tables.Count > 0)
                {
                    conn.Close();
                    return ds.Tables[0];
                }
                return null;
            }
            /*
            catch (OleDbException dbEx)
            {
                conn.Close();
                throw dbEx;
            }
            */
            catch (Exception Ex)
            {
                conn.Close();
                //数据库错误多为致命错误，所以暂时全部上抛处理.
      //          m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
                throw Ex;
            }
        }
        /// <summary>
        /// 执行一条返回第一条记录第一列的OleDbCommand命令，通过专用的连接字符串
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">OleDbCommand命令类型 (存储过程<StoredProcedure>，表的名称<TableDirect>， T-SQL 文本命令语句<Text>(默认)， 等等。)</param>
        /// <param name="cmdText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="commandParameters">以数组形式提供OleDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                    object val = cmd.ExecuteScalar();

                    cmd.Parameters.Clear();
                    return val;
                }
            }
            catch (Exception Ex)
            {
                //数据库错误多为致命错误，所以暂时全部上抛处理.
    //            m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
                throw Ex;
            }
        }


        /// <summary>
        /// 执行一条返回第一条记录第一列的sqlCommand命令，通过已经存在的数据库连接
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例： 
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个已经存在的数据库连接</param>
        /// <param name="commandType">sqlCommand命令类型 (存储过程<StoredProcedure>，表的名称<TableDirect>， T-SQL 文本命令语句<Text>(默认)， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="commandParameters">以数组形式提供sqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception Ex)
            {
                //数据库错误多为致命错误，所以暂时全部上抛处理.
      //          m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
                throw Ex;
            }
        }


        /// <summary>
        /// 缓存参数数组
        /// </summary>
        /// <param name="cacheKey">参数缓存的键值</param>
        /// <param name="commandParameters">被缓存的参数列表</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }


        /// <summary>
        /// 获取被缓存的参数
        /// </summary>
        /// <param name="cacheKey">用于查找参数的KEY值</param>
        /// <returns>返回缓存的参数数组</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            //新建一个参数的克隆列表
            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            //通过循环为克隆参数列表赋值
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                //使用clone方法复制参数列表中的参数
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }


        /// <summary>
        /// 为执行命令准备参数
        /// </summary>
        /// <param name="cmd">OleDbCommand命令</param>
        /// <param name="conn">已经存在的数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">OleDbCommand命令类型 (存储过程<StoredProcedure>，表的名称<TableDirect>， T-SQL 文本命令语句<Text>(默认)， 等等。)</param>
        /// <param name="cmdText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="cmdParms">以数组形式提供OleDbCommand命令中用到的参数列表</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            //判断数据库连接状态
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            //判断是否需要事务处理
            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        /// <summary>
        /// 块拷贝
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="DestinationTableName">待插入数据表名称</param>
        /// <param name="SourceTable">数据源表</param>
        /// <returns>-2：没有要插入表名称 -1：插入数量为0  0：插入异常 1：插入成功  </returns>
        public static  int BulckCopy(string connectionString, string DestinationTableName, DataTable SourceTable)
        {
            int _iRet = -1;//没有数据要插入
            int iDataNum = SourceTable.Rows.Count;//待插入数据量
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
               
                SqlTransaction trans = conn.BeginTransaction();

                #region 待插入数据数量检查
                _iRet = -2;
                if (DestinationTableName == "") return _iRet;//没有要插入表名称
                _iRet = -1;
                if (iDataNum <= 0) return _iRet;//插入数量为0
                #endregion 待插入数据数量检查

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(trans.Connection, SqlBulkCopyOptions.KeepIdentity, trans))
                {
                    bulkCopy.BatchSize = iDataNum;// 100000;
                    bulkCopy.BulkCopyTimeout = 10;


                    //将DataTable表名作为待导入库中的目标表名   
                    bulkCopy.DestinationTableName = DestinationTableName;


                    //将数据集合和目标服务器库表中的字段对应    
                    for (int i = 0; i < SourceTable.Columns.Count; i++)
                    {
                        //列映射定义数据源中的列和目标表中的列之间的关系  
                        //bulkCopy  
                        bulkCopy.ColumnMappings.Add(SourceTable.Columns[i].ColumnName, SourceTable.Columns[i].ColumnName);
                    }
                    //SqlBulkCopyMapping(bulkCopy);  
                    try
                    {
                        bulkCopy.WriteToServer(SourceTable);
                        trans.Commit();
                        conn.Close();
                        conn.Dispose();
                        _iRet = 1;
                    }
                    catch (Exception ex)
                    {
                        _iRet = 0;
                        Console.WriteLine(ex.Message);
                        trans.Rollback();
                    }
                }
            }
            catch { }
            return _iRet;
        }
        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="columns"></param>

        public static DataTable CreateTable(System.Collections.Generic .List <string> columns)
        {
            var dt = new DataTable();
            foreach (var c in columns)
            {
                dt.Columns.Add(c);
            }
//            if exists(select * from sysobjects where name = 'Employee')
// drop table Employee
//create table Employee
//(
//    Id int primary key identity(1, 1), --primary key: 主键：非空，唯一   identity(1, 1：identity（标识种子，标识增量）
//    CardId char(18) not null, --not null标记非空, 如果没有标记就说明可以为null
//    Name nvarchar(50) not null,
//    Gender bit not null,
//    InTime datetime null,
//    Age int not null check(age > 0 and age < 100),
//    [Address] nvarchar(255) default('广州'), --[Address]将系统关键字当成普通的用户自定义字符串进行处理
//    CellPhone char(11),
//    DepId int not null, --外键
//    Email varchar(50) not null
//)
            return dt;

        }
    }
}
