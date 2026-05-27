/*
   Access数据库的通用访问类(静态类)
 */
using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Configuration;

using System.Windows.Forms;
using FrameWork.Class;

namespace FrameWork.DataBaseMang.DBUtility
{
    /// <summary>
    /// Access数据库的通用访问类(静态类)
    /// </summary>
    public class OleDbHelper
    {
        /// <summary>
        /// 主表数据库路径
        /// </summary>
        public  static string strDbPath = Application.StartupPath + "\\database";
        /// <summary>
        /// 子表数据库路径，数据表包含：只保存一个用户名点铭牌信息、厚度信息、波形信息
        /// </summary>
        public static string strDb_SubDbPath = Application.StartupPath + "\\SubData";//用户名+日期
        /// <summary>
        /// 临时数据库
        /// </summary>
        public static string strDb_TmpDbPath = Application.StartupPath + "\\SubData\\Tmp";//临时表

        public static string strDataBaseName = "DellData.mdb";//数据库
        /// <summary>
        /// 子表数据库名称
        /// </summary>
        public static string strSubDataBaseName = "";//+ (strJdrq=="" ? DateTime.Now.ToString("yyMMdd_HHmmss"): strJdrq)
        /// <summary>
        /// 主表还是子表 true:主表 false：子表
        /// 主表路径：database + DellData.mdb  存放总表名称信息
        /// 子表：子表路径 + 用户名 +检定日期  存放子表名称信息 + 检定数据。拷贝：拷贝到 SubData文件夹下，程序提起此文件，将信息添加到总表中，主键仍然是ID，重复提示是否覆盖
        /// 查询数据方式：主表找ID,子表找数据
        /// </summary>
        public static bool blMainSub_DataBase = true;

        public static string ErrFile = "";// Application.StartupPath + "\\ErrFile_" + DateTime.Now.ToString("yy-MM-dd ")+".INI";
        /// <summary>
        /// 文件操作
        /// </summary>
       static csInterface  m_csInter = new csInterface ();
        /// <summary>
        /// 获取数据库联机字符串，其属于静态变量且，项目中所有文档可以直接使用，原则上不能修改
        /// </summary>
        public static string DbConnString_DT = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                             (blMainSub_DataBase ?(strDbPath +"\\"+ strDataBaseName) ://主表：主铭牌信息表、用户名称表
                             (strDb_SubDbPath+"\\"+ strSubDataBaseName  +".mdb")//子表：铭牌信息表、距离厚度表、波形表
                              );
       ///// <summary>
       ///// 方案数据库：功能方案   协议方案  协议测试数据
       ///// </summary>
       // public static string DbConnString_FA = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strDbPath + "HyFkt_Fa.mdb";
       // /// <summary>
       // /// 系统数据库：字典Sys_Dict  用户名：Sys_User
       // /// </summary>
       // public static string DbConnString_Sys = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strDbPath + "HyFkt_Sys.mdb";
       
        
        /// <summary>
        /// 哈希表用来存储缓存的参数信息，哈希表可以存储任意类型的参数
        /// </summary>
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 执行一个不需要返回值的OleDbCommand命令，通过指定专用的联机字符串
        /// 使用参数数组形式提供参数列表
        /// </summary>
        /// <remarks>
        /// 使用示例：
        /// int result = ExecuteNonQuery(connString,CommandType.StoredProcedure,"PulishOrders",new OleDbParameter("@prodid",24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库联机字符串</param>
        /// <param name="cmdType">OleDbCommand命令类型(存储过程'StoredProcedure'，表的名称'TableDirect'，
        /// T-SQL 文本命令语句'Text'(默认)，等等)</param>
        /// <param name="cmdText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="commandParamters">以数组形式提供OleDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此OleDbCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params OleDbParameter[] commandParamters)
        {
            OleDbCommand cmd = new OleDbCommand();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {

                    //通过PrePareCommand方法将参数逐个加入到OleDbCommand的参数集合中
                //    OleDbTransaction trans = conn.BeginTransaction();
                    PrepareCommand(cmd, conn, null , cmdType, cmdText, commandParamters);
                    cmd.ExecuteNonQuery();
                    //清空OleDbCommand中的参数列表
                 //   trans.Commit();
                    cmd.Parameters.Clear();

                    conn.Close();
                    conn.Dispose();

                    return 1;

                }
                catch (Exception e)
                {
                    
                    conn.Close();
                    conn.Dispose();

                    // MessageBox.Show(e.Message);
                    //数据库错误多为致命错误，所以暂时全部上抛处理.
                    return 0;
                }
            }
        }

        /// <summary>
        /// 执行一个不需要返回值的OleDbCommand命令，通过一个已经存在的数据库联机
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：
        /// int result = ExecuteNonQuery(conn,CommandType.StoredProcedure,"PulishOrders",new OleDbParameter("@prodid",24));
        /// </remarks>
        /// <param name="connection">一个现有的数据库联机</param>
        /// <param name="cmdType">OleDbCommand命令类型(存储过程'StoredProcedure'，表的名称'TableDirect'，
        /// T-SQL 文本命令语句'Text'(默认)， 等等)</param>
        /// <param name="cmdText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="commandParameters">以数组形式提供OleDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此OleDbCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(OleDbConnection connection, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                cmd.ExecuteNonQuery();

                //清空OleDbCommand中的参数列表
                cmd.Parameters.Clear();
                return 1;
            }
            catch
            {
                return 0;
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
        /// <param name="cmdType">OleDbCommand命令类型(存储过程'StoredProcedure'，表的名称'TableDirect'，
        /// T-SQL 文本命令语句'Text'(默认)， 等等)</param>
        /// <param name="cmdText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="commandParameters">以数组形式提供OleDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此OleDbCommand命令执行后影响的行数</returns>
        public static int ExecuteNonQuery(OleDbTransaction trans, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
                cmd.ExecuteNonQuery();

                //清空OleDbCommand中的参数列表
                cmd.Parameters.Clear();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 返回DATASET
        /// </summary>
        /// <param name="connectionString">联机字符串</param>
        /// <param name="cmdType">OleDbCommand命令类型</param>
        /// <param name="cmdText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="dataname">返回的DataSet中DataTable的名字</param>
        /// <param name="commandParameters">参数列表</param>
        /// <returns>返回DataSet</returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType cmdType, string cmdText, string dataname, params OleDbParameter[] commandParameters)
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection conn = new OleDbConnection(connectionString);
            //SqlDataReader rdr = null;

            /**********
            在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
            CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            关闭数据库联机，并通过throw再次引发捕捉到的异常。
            **********/
            try
            {
                OleDbTransaction trans = conn.BeginTransaction();
                PrepareCommand(cmd, conn, trans, cmdType, cmdText, commandParameters);
                OleDbDataAdapter adp = new OleDbDataAdapter();
                cmd.Connection = conn;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds, dataname);

                cmd.Parameters.Clear();
                trans.Commit();
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
                m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
                throw Ex;
            }
        }

        /// <summary>
        /// 返回DATATABLE
        /// </summary>
        /// <param name="connectionString">联机字符串</param>
        /// <param name="cmdType">OleDbCommand命令类型</param>
        /// <param name="cmdText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="dataname">返回的DataSet中DataTable的名字</param>
        /// <param name="commandParameters">参数列表</param>
        /// <returns>返回DataSet</returns>
        public static DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, string dataname, params OleDbParameter[] commandParameters)
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection conn = new OleDbConnection(connectionString);
            //SqlDataReader rdr = null;

            /**********
            在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
            CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            关闭数据库联机，并通过throw再次引发捕捉到的异常。
            **********/
            try
            {
              //  OleDbTransaction trans = conn.BeginTransaction();
                PrepareCommand(cmd, conn, null , cmdType, cmdText, commandParameters);
                OleDbDataAdapter adp = new OleDbDataAdapter();
                cmd.Connection = conn;
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds, dataname);
            //    trans.Commit();
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
               // m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
              // throw Ex;
            }
            return null;
        }

        /// <summary>
        /// 执行一条返回结果集的OleDbCommand命令，通过专用的联机字符串
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  OleDbDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库联机字符串</param>
        /// <param name="cmdType">OleDbCommand命令类型(存储过程<StoredProcedure>，表的名称<TableDirect>， T-SQL 文本命令语句<Text>(默认)， 等等。)</param>
        /// <param name="cmdText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="commandParameters">以数组形式提供OleDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个包含结果的OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbDataReader rdr = null;

            /**********
            在这里使用try/catch处理是因为如果方法出现异常，则OleDbDataReader就不存在，
            CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            关闭数据库联机，并通过throw再次引发捕捉到的异常。
            **********/
            try
            {
              //  OleDbTransaction trans = conn.BeginTransaction();
                PrepareCommand(cmd, conn, null , cmdType, cmdText, commandParameters);
                rdr = cmd.ExecuteReader();// CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
          
        

                //    trans.Commit();
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
                conn.Dispose();
                //数据库错误多为致命错误，所以暂时全部上抛处理.
               m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
             //   throw Ex;
            }
            return rdr;
        }

        public static OleDbDataReader ExecuteReader_Read(string connectionString, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
        {
            ArrayList alist_Ret = new ArrayList();
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbDataReader rdr = null;

            /**********
            在这里使用try/catch处理是因为如果方法出现异常，则OleDbDataReader就不存在，
            CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            关闭数据库联机，并通过throw再次引发捕捉到的异常。
            **********/
            try
            {
                OleDbTransaction trans = conn.BeginTransaction();
                PrepareCommand(cmd, conn, trans, cmdType, cmdText, commandParameters);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                trans.Commit();
                //while (rdr.Read())
                //{
                //    alist_Ret.Add(rdr["Wave"]);
                //}
                //rdr.Close();
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
                m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
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
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    OleDbTransaction trans = conn.BeginTransaction();
                    PrepareCommand(cmd, conn, trans, cmdType, cmdText, commandParameters);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    trans.Commit();
                    return val;
                }
            }
            catch (Exception Ex)
            {
                //数据库错误多为致命错误，所以暂时全部上抛处理.
               m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
                throw Ex;
            }
        }


        /// <summary>
        /// 执行一条返回第一条记录第一列的OleDbCommand命令，通过已经存在的数据库连接
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例： 
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OleDbParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个已经存在的数据库连接</param>
        /// <param name="commandType">OleDbCommand命令类型 (存储过程<StoredProcedure>，表的名称<TableDirect>， T-SQL 文本命令语句<Text>(默认)， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者表的名字或者 T-SQL 文本命令语句</param>
        /// <param name="commandParameters">以数组形式提供OleDbCommand命令中用到的参数列表</param>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public static object ExecuteScalar(OleDbConnection connection, CommandType cmdType, string cmdText, params OleDbParameter[] commandParameters)
        {
            OleDbCommand cmd = new OleDbCommand();
            try
            {
                OleDbTransaction trans = connection.BeginTransaction();
                PrepareCommand(cmd, connection, trans, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                trans.Commit();
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception Ex)
            {
                //数据库错误多为致命错误，所以暂时全部上抛处理.
               m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
                throw Ex;
            }
        }


        /// <summary>
        /// 缓存参数数组
        /// </summary>
        /// <param name="cacheKey">参数缓存的键值</param>
        /// <param name="commandParameters">被缓存的参数列表</param>
        public static void CacheParameters(string cacheKey, params OleDbParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }


        /// <summary>
        /// 获取被缓存的参数
        /// </summary>
        /// <param name="cacheKey">用于查找参数的KEY值</param>
        /// <returns>返回缓存的参数数组</returns>
        public static OleDbParameter[] GetCachedParameters(string cacheKey)
        {
            OleDbParameter[] cachedParms = (OleDbParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            //新建一个参数的克隆列表
            OleDbParameter[] clonedParms = new OleDbParameter[cachedParms.Length];

            //通过循环为克隆参数列表赋值
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                //使用clone方法复制参数列表中的参数
                clonedParms[i] = (OleDbParameter)((ICloneable)cachedParms[i]).Clone();

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
        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, CommandType cmdType, string cmdText, OleDbParameter[] cmdParms)
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
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}
