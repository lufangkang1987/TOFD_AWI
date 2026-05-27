/*
   Access数据库的通用访问类(静态类)
 */
using System;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Text;
using System.Configuration;

using System.Windows.Forms;
//using ClassLibrary_Interface;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace ClassLib_DataMang.DataBaseMang.DBUtility
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
        static ClassInterFace m_csInter = new ClassInterFace();
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

    public class ClassInterFace
    {
        #region  1 INI文件操作方法
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        private static object LockFile = new object();

        /// <summary>
        /// 日志文件
        /// </summary>
        public string strErrFileName = Application.StartupPath + "\\datalog\\Err.ini";
        /// <summary>
        /// void EraseSection        删除指定[Section]的全部内容
        /// string Section           [Section]
        /// string strFileName       配置文件名称
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="strFileName"></param>
        public void EraseSection(string Section, string strFileName)
        {
            WritePrivateProfileString(Section, null, null, strFileName);
        }
        /// <summary>
        /// void EraseSectionOneItem    删除指定[Section]的Key值内容
        /// string Section           [Section]
        /// string strKey            strKey=?
        /// string strFileName       配置文件名称
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="strKey"></param>
        /// <param name="strFileName"></param>
        public void EraseSectionOneItem(string Section, string strKey, string strFileName)
        {
            WritePrivateProfileString(Section, strKey, null, strFileName);
        }
        /// <summary>
        /// INIReadValue           从INI文件里读数据
        /// string Section     配置文件[]里的值
        /// string strKey         []下标记名
        /// string strValue       标记名的默认值
        /// string strFileName    配置文件名称
        /// </summary>
        public string INIReadValue(string Section, string strKey, string strValue, string strFileName)
        {   //从ini配置文件读取-----------
            StringBuilder sbTemp = new StringBuilder(1024);
            int i = GetPrivateProfileString(Section, strKey, strValue, sbTemp, 1024, strFileName);
            string strTmp = "";
            strTmp = sbTemp.ToString().Trim();
            if (strTmp.Length == 0) strTmp = strValue;//给出默认值
            return strTmp;
        }
        /// <summary>
        /// INIWriteValue         从INI文件里写数据
        /// string Section     配置文件[]里的值
        /// string strKey         []下标记名
        /// string strValue       标记名的值
        /// string strFileName    配置文件名称
        /// </summary>
        public void INIWriteValue(string Section, string strKey, string strValue, string strFileName)
        {   //写入ini配置文件-----------
            string strTmp = "";
            if (strValue != null)
            {
                strTmp = strValue.Trim();
                strValue.Replace("/n", "");		//替代回车换行
                long n = WritePrivateProfileString(Section, strKey, strTmp, strFileName);
            }
        }
        /// <summary>
        /// 给配置初始化默认参数
        /// string strValue就是默认参数
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public string IniReadDefine(string Section, string strKey, string strValue, string strFileName)
        {
            string strTmp = INIReadValue(Section, strKey, "", strFileName);
            if (strTmp.Length == 0)
            {
                strTmp = strValue;
                INIWriteValue(Section, strKey, strTmp, strFileName);
            }
            return strTmp;
        }

        /// <summary>
        /// 文件操作
        /// </summary>
        /// <param name="strPathFileName">包含路径的文件名称</param>
        /// <param name="strMsg">写入信息</param>
        /// <param name="blSendOrRec">true：发送 false:返回</param>
        public void WriteErrorLog(string strPathFileName, string strMsg, bool blSendOrRec)
        {
            lock (LockFile)
            {
                strMsg = strMsg.Trim();
                if (strMsg == "") return;

                strMsg += "\r\n";
                System.IO.StreamWriter swTxt = null;

                //1 判断路径是否存在
                string strPath = strPathFileName;
                if (strPathFileName.IndexOf("datalog") > 0)
                {
                    strPath = System.Windows.Forms.Application.StartupPath + "\\datalog\\";
                }
                else if (strPathFileName.IndexOf("Log") > 0)
                {
                    strPath = System.Windows.Forms.Application.StartupPath + "\\log\\";
                }

                if (System.IO.Directory.Exists(strPath) == false)
                {
                    System.IO.Directory.CreateDirectory(strPath);
                }
                //2 创建文件
                try
                {
                    swTxt = System.IO.File.AppendText(strPathFileName);
                }
                catch (Exception e)
                {

                }
                //3 写入数据
                string strValue = "";
                strValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + (blSendOrRec ? "->" : "<-") + strMsg;
                //strValue = srTxt.ReadLine();
                try
                {
                    swTxt.WriteLine(strValue);
                    swTxt.Flush();
                    swTxt.Close();
                }
                catch (Exception e)
                {
                }
                swTxt = null;
            }
        }

        #endregion 文件操作

        /// <summary>
        /// 等待指定时间（秒）
        /// </summary>
        /// <param name="dbWait"></param>
        public void WaitTime(double dbWait, ref bool m_blApp)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                try
                {
                    if (m_blApp) break;
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > dbWait) break;
                    Thread.Sleep(10);
                }
                catch { break; }
            }
        }
        /// <summary>
        /// 等待制定时间
        /// </summary>
        /// <param name="dbWait">秒</param>
        public void WaitTime(double dbWait)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                try
                {

                    Application.DoEvents();
                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > dbWait) break;
                    Thread.Sleep(5);
                }
                catch { break; }
            }
        }

        public bool IsNum(string strDat)
        {
            bool _blRet = false;

            try
            {
                float _flDat = float.Parse(strDat);
                _blRet = true;

            }
            catch { }

            return _blRet;
        }
        /*
         1，C#追加文件
　　　　StreamWriter sw = File.AppendText(Server.MapPath(".")+"\\myText.txt");
　　　　sw.WriteLine("追逐理想");
　　　　sw.WriteLine("kzlll");
　　　　sw.WriteLine(".NET笔记");
　　　　sw.Flush();
　　　　sw.Close();,
2，C#拷贝文件
　　　　string OrignFile,NewFile;
　　　　OrignFile = Server.MapPath(".")+"\\myText.txt";
　　　　NewFile = Server.MapPath(".")+"\\myTextCopy.txt";
　　　　File.Copy(OrignFile,NewFile,true);
3，C#删除文件
　　　　string delFile = Server.MapPath(".")+"\\myTextCopy.txt";
　　　　File.Delete(delFile);
4，C#移动文件
　　　　string OrignFile,NewFile;
　　　　OrignFile = Server.MapPath(".")+"\\myText.txt";
　　　　NewFile = Server.MapPath(".")+"\\myTextCopy.txt";
　　　　File.Move(OrignFile,NewFile);
5，C#创建目录
// 创建目录c:\sixAge
　　　　DirectoryInfo d=Directory.CreateDirectory("c:\\sixAge");
// d1指向c:\sixAge\sixAge1
　　　　DirectoryInfo d1=d.CreateSubdirectory("sixAge1");
// d2指向c:\sixAge\sixAge1\sixAge1_1
　　　　DirectoryInfo d2=d1.CreateSubdirectory("sixAge1_1");
// 将当前目录设为c:\sixAge
　　　　Directory.SetCurrentDirectory("c:\\sixAge");
// 创建目录c:\sixAge\sixAge2
　　　　Directory.CreateDirectory("sixAge2");
// 创建目录c:\sixAge\sixAge2\sixAge2_1
　　　　Directory.CreateDirectory("sixAge2\\sixAge2_1");
         */
        /// <summary>
        /// 文件拷贝
        /// </summary>
        /// <param name="OldPathFile"></param>
        /// <param name="NewPathFile"></param>
        /// <returns></returns>
        public bool FileCopy(string OldPathFile, string NewPathFile)
        {
            bool blRet = false;
            try
            {
                System.IO.File.Copy(OldPathFile, NewPathFile, true);
                blRet = System.IO.File.Exists(NewPathFile);
            }
            catch (Exception e)
            { }
            return blRet;
        }
        /// <summary>
        /// 修改文件名字
        /// </summary>
        /// <param name="sourceFileName">带路径的老文件名</param>
        /// <param name="destFileName">带路径的新文件名</param>
        public bool Change_FileName(string sourceFileName, string destFileName)
        {
            bool _blRet = false;
            //string[] strDirs_S = System.IO.Directory.GetDirectories(sourceFileName);
            //if (System.IO.Directory.Exists(sourceFileName))
            //{
            //    string[] strDirs_d = System.IO.Directory.GetDirectories(destFileName);
            //    if (strDirs_S[0] == strDirs_d[0])
            //    {
            try
            {
                System.IO.File.Move(sourceFileName, destFileName); _blRet = true;
            }
            catch (Exception e)
            { }
            //    }
            //}
            return _blRet;
        }
        public string[] GetLatestFiles(string Path, int count)
        {
            string[] strArr = new string[1];

            string strPath = Application.StartupPath;// AppDomain.CurrentDomain.BaseDirectory;
            Path = strPath + "\\" + Path;
            if (System.IO.Directory.Exists(Path))
            {
                var query = (from f in System.IO.Directory.GetFiles(Path, "*.mdb")
                             let fi = new System.IO.FileInfo(f)
                             orderby fi.CreationTime descending
                             select fi.FullName).Take(count);
                strArr = query.ToArray();
                // return query.ToArray();
            }
            if (strArr != null)
            {
                if (strArr.Count() > 0)
                {
                    string[] sPara = "".Split(',');

                    for (int i = 0; i < strArr.Count(); i++)
                    {
                        if (strArr[i] != "")
                            sPara = strArr[i].Split('\\');
                        if (sPara.Count() > 0)
                            strArr[i] = sPara[sPara.Count() - 1];
                    }
                }
            }
            return strArr;
        }

        /// <summary>
        /// 清空文件夹下
        /// </summary>
        /// <param name="strDir">目录地址:文件夹名字</param>
        public void DeleteFiles(string strDir)
        {
            try
            {
                string strPath = Application.StartupPath;// AppDomain.CurrentDomain.BaseDirectory;
                strDir = strPath + "\\" + strDir;
                if (System.IO.Directory.Exists(strDir))
                {
                    string[] strDirs = System.IO.Directory.GetDirectories(strDir);
                    string[] strFiles = System.IO.Directory.GetFiles(strDir);
                    foreach (string strFile in strFiles)
                    {
                        System.IO.File.Delete(strFile);
                    }

                    //foreach (string strdir in strDirs)
                    //{
                    //    Directory.Delete(strdir, true);
                    //}
                    Console.WriteLine("删除成功！");
                }
                else
                {
                    Console.WriteLine("此目录不存在！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除文件夹保存：" + ex.Message);
            }
        }

        /// <summary>
        /// 删除当前路径下文件夹下某文件(例如：\\datalog\\GWJMRunMsg.ini)或者当前路径下文件  陈大伟 
        /// </summary>
        /// <param name="strPathFile">格式：\\datalog\\GWJMRunMsg.ini</param>
        public bool DeleFile(string strPathFile, int iType = 0)
        {
            bool _blRet = true;
            try
            {
                if (strPathFile == "") return _blRet;
                string strPath = "";
                int iS = strPathFile.IndexOf('\\');
                if (iS == 0)
                    strPath = Application.StartupPath;// AppDomain.CurrentDomain.BaseDirectory;
                else
                    strPath = AppDomain.CurrentDomain.BaseDirectory;

                string _strPathFile = strPath + strPathFile;
                if (iType == 1 && strPathFile != "")
                    _strPathFile = strPathFile;
                if (System.IO.File.Exists(_strPathFile))
                    System.IO.File.Delete(_strPathFile);
            }
            catch (Exception Err)
            {
                _blRet = false;
                //       MessageBox.Show("删除文件：" + strPathFile + "出错！" + Err.Message);
            }
            return _blRet;
        }
    }
}
