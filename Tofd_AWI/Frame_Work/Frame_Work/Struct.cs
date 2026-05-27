using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace Frame_Work
{
    /// <summary>
    /// 公共数据集合
    /// </summary>
    public class ClassSys_Buff
    {
        /// <summary>
        /// 以距离为主键的数据字典 距离转换成0.1mm小数变整数
        /// </summary>
        public Dictionary<int, Class_X_Data> g_Dic_Alarm = new Dictionary<int, Class_X_Data>();
    }
    /// <summary>
    /// 测量数据
    /// </summary>
    public class Class_X_Data
    {
        /// <summary>
        /// 母材厚度
        /// </summary>
        public float m_flThick = 0.5f;
        /// <summary>
        /// 结论true:合格 false:不合格
        /// </summary>
        public bool blJL = true;
        /// <summary>
        /// 焊缝正面还是背面  true:正面 false:背面
        /// </summary>
        public bool m_blZm1_Bm0 = true;
        /// <summary>
        /// 对接：false  角焊：true
        /// </summary>
        public bool m_blDuijie0_JiaoHan1 = false;
        /// <summary>
        /// 凸面个数
        /// </summary>
        public int m_iBall = 0;
        /// <summary>
        /// 是否有缺陷（凸面）
        /// </summary>
        public bool m_blBall = false;
        /// <summary>
        /// 是否有缺陷（凹陷）
        /// </summary>
        public bool m_blAlarm = false;
        /// <summary>
        /// 一帧数据长度
        /// </summary>
        public int m_iprofileCnt = 0;
        /// <summary>
        /// 母材厚度:统计的最低值
        /// </summary>
        public double m_dbL_Min_H = 0;
        /// <summary>
        /// 最大值
        /// </summary>
        public double m_dbL_Max_H = 0;

        /// <summary>
        /// 焊缝
        /// </summary>
        public Class_Weld m_clsWeld = new Class_Weld();
        /// <summary>
        /// 当前帧对应的单轮廓的X轴位置数组
        /// </summary>
        public double[] m_Arr_profileX = new double[1000];

        /// <summary>
        /// 当前帧对应的单轮廓的Z轴方向深度数组
        /// </summary>
        public double[] m_Arr_profileZ = new double[1000];

        /// <summary>
        /// 当前帧数据对应的缺陷（凹陷）
        /// </summary>
       public   List<Class_Alarm> m_lstAlarm=new List<Class_Alarm> ();
        /// <summary>
        /// 凹陷个数
        /// </summary>
        public int iDownNum = 0;


    }

    public class Class_Weld
    {
        /// <summary>
        /// 是否有焊缝
        /// </summary>
        public bool m_blHave = false;
        /// <summary>
        /// 
        /// </summary>
        public int iType = 0;
        /// <summary>
        /// 焊缝与母材左边交点 X值
        /// </summary>
        public float  m_i_W_Start_X = -1;
        /// <summary>
        /// 焊缝与母材左边交点  Y值
        /// </summary>
        public double m_i_W_Start_Y = -1;
        /// <summary>
        /// 焊缝与母材右边交点 X值
        /// </summary>
        public float  m_i_W_End_X = -1;
        /// <summary>
        /// 焊缝与母材右边交点 Y值
        /// </summary>
        public double  m_i_W_End_Y = -1;
        /// <summary>
        /// 两条母材边延长线相交的X点
        /// </summary>
        public double  db_Line_LR_X = -1;
        /// <summary>
        /// 两条母材边延长线相交的Y点
        /// </summary>
        public double db_Line_LR_Y = -1;
        /// <summary>
        /// 母材角点与此角平分线与母材焊缝顶面交点距离
        /// </summary>
        public double  db_Line_3_Mucai_H= -1;
        /// <summary>
        /// 计算最短距离的应一边的交点  X点
        /// </summary>
        public int i_MinDist_X = -1;
        /// <summary>
        /// 计算最短距离的应一边的交点  Y点
        /// </summary>
        public int i_MinDist_Y = -1;
        /// <summary>
        /// 焊缝余高:与焊缝面高度比较， 内角测量点到线短，外角测量点到线则长
        /// </summary>
        public double m_dbCentWeld_H = -1;


        /// <summary>
        /// 焊缝宽度
        /// </summary>
        public double m_dbCentWeld_W = -1;
        /// <summary>
        /// 点到直线的距离
        /// </summary>
        public double m_dbPoint_Line4 = -1;
        /// <summary>
        /// 焊瘤点 X
        /// </summary>
        public double m_Point_4_x = -1;//
        /// <summary>
        ///  焊瘤点 Y
        /// </summary>
        public double m_Point_4_y = -1;//
        /// <summary>
        /// 角焊接过渡部队称时正常厚度a（没有余高）
        /// </summary>
        public double m_Point_4_a = -1;
        /// <summary>
        /// 角焊接过渡部队称时焊瘤长度h
        /// </summary>
        public double m_Point_4_h = -1;
        /// <summary>
        /// 结论true:合格 false:不合格
        /// </summary>
        public bool blJL = true;
        /// <summary>
        /// 结论
        /// </summary>
        public List<csJL> m_lstJL = new List<csJL>();
    }
    /// <summary>
    /// 结论类
    /// </summary>
    public class csJL
    {
        /// <summary>
        /// 检查内容
        /// </summary>
        public string strType = "";
        /// <summary>
        /// 缺陷编号
        /// </summary>
        public int iType = -1;
        /// <summary>
        ///  根部凹入 结论 -1 1：合格：0：不合格
        /// </summary>
        public int iJL = -1;
        /// <summary>
        /// 结论内容
        /// </summary>
        public string strJL = "";
    }
    /// <summary>
    /// INI文件操作类
    /// </summary>
    public class csInterface
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
            if (strPathFileName == "") return;
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
        public void WaitTime(double dbWait, bool m_blCloseApp = false)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                try
                {
                    if (m_blCloseApp) break;
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(dtStar).TotalSeconds > dbWait) break;
                    System.Threading.Thread.Sleep(10);
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
        public void DeleFile(string strPathFile, int iType = 0)
        {
            try
            {
                if (strPathFile == "") return;
                string strPath = "";
                int iS = strPathFile.IndexOf('\\');
                if (iS == 0)
                    strPath = Application.StartupPath;// AppDomain.CurrentDomain.BaseDirectory;
                else
                    strPath = AppDomain.CurrentDomain.BaseDirectory;

                string _strPathFile = strPath + strPathFile;
                if (iType == 1 && strPathFile != "")
                    _strPathFile = strPathFile;
                System.IO.File.Delete(_strPathFile);
            }
            catch (Exception Err)
            {
                MessageBox.Show("Delete Files：" + strPathFile + " error！" + Err.Message);
            }
        }

    }
    /// <summary>
    /// 凸起物
    /// </summary>
    public class Class_ball
    {
        /// <summary>
        /// 统计降落时个数
        /// </summary>
        public int iDownNum = 0;
        /// <summary>
        /// 起点值
        /// </summary>
        public double  dbStartVal=0;
        /// <summary>
        /// 开始扬起点
        /// </summary>
        public int iStart=-1;
        /// <summary>
        /// 回落点
        /// </summary>
        public int iEnd = -1;
        /// <summary>
        /// 高度
        /// </summary>
        public double  flHeight = 0;
        /// <summary>
        /// 高度位置
        /// </summary>
        public int iHeightPoint = -1;

    }

    /// <summary>
    /// 焊缝位置
    /// </summary>
    public class Class_Weld_Position
    {
        public bool m_blAlarm = false;
        /// <summary>
        /// 缺陷X轴开始位置：数组序号，是实际值/10
        /// </summary>
        public int i_Start = -1;
        /// <summary>
        /// 缺陷X轴结束位置
        /// </summary>
        public int i_End = -1;
        /// <summary>
        /// 缺陷X轴最深处深度
        /// </summary>
        public double dbDepth = -1;
        /// <summary>
        /// 焊缝位置
        /// </summary>
        public int i_Cent = -1;
        /// <summary>
        /// 斜率
        /// </summary>
        public float f_K = -99;
    }
        /// <summary>
        /// 一帧数据中缺陷数据
        /// </summary>
        public class Class_Alarm
    {
        /// <summary>
        /// 缺陷类型 0：母材上凹陷 1：凸起  2：咬边 3 母材上凹陷 -1:取消此缺陷
        /// </summary>
        public int i_Type = -1;
        /// <summary>
        /// 缺陷X轴开始位置：数组序号，是实际值/10
        /// </summary>
        public int i_Start = -1;
        /// <summary>
        /// 缺陷X轴结束位置
        /// </summary>
        public int i_End = -1;
        /// <summary>
        /// 缺陷X轴最深处深度
        /// </summary>
        public double dbDepth = -1;
        /// <summary>
        /// 起点深度
        /// </summary>
        public double dbDepth_S = -1;
        /// <summary>
        /// 是否已经刷新图
        /// </summary>
        public bool bl_Show = false;


        /// <summary>
        /// 结论true:合格 false:不合格
        /// </summary>
        public bool blJL = true;
        /// <summary>
        /// 结论
        /// </summary>
        public List<csJL> m_lstJL = new List<csJL>();
    }


    #region 图像
    public struct Struct_G
    {
        public Bitmap image;
        // 获取背景层
        public Bitmap bg;
        // 初始化整个画布
        public Bitmap canvas;
        // 初始化图形面板，获取这块内存画布的Graphics的引用
        public Graphics g;
        public Graphics gb;

        public int iCs_Num;
        /// <summary>
        /// 数据缓存：根据X、Y填写测量点数据
        /// </summary>
        public PointF[] Buff;
    }

 
    /// <summary>
    /// 图像控制
    /// </summary>
    public class Class_Plant
    {
        public float[] m_flArr_Rul_S = new float[900];
        public float[] m_flArr_Rul_E = new float[900];
        public void Cal_S_E()
        {
            m_flArr_Rul_S = new float[900];
            m_flArr_Rul_E = new float[900];

            for (int i = 0; i < m_flArr_Rul_S.Count(); i++)
            {
                if (i == 0)
                {
                    m_flArr_Rul_S[i] = float.Parse((i * (Scree_Stant_Distance + Scree_iDotWithmm_X)).ToString("f3"));
                    m_flArr_Rul_E[i] = float.Parse((i * (Scree_Stant_Distance + Scree_iDotWithmm_X) + Scree_Stant_Distance).ToString("f3")); // + Scree_iDotWithmm_X-0.001).ToString("f3"));
                }
                else
                {
                    m_flArr_Rul_S[i] = m_flArr_Rul_E[i - 1];
                    m_flArr_Rul_E[i] = float.Parse((i * (Scree_Stant_Distance) + Scree_Stant_Distance).ToString("f3"));
                }
            }
        }
        public int Get_No(float flDistance)
        {
            //  Cal_S_E();
            flDistance = float.Parse(flDistance.ToString("f1"));
            int iNo = 0;
            if (m_flArr_Rul_S[0] == 0 && m_flArr_Rul_E[0] == 0)
                return 0;
            //    Cal_S_E();
            for (int i = 0; i < m_flArr_Rul_S.Count(); i++)
            {
                if (flDistance >= m_flArr_Rul_S[i] && (flDistance <= m_flArr_Rul_E[i]))//|| flDistance <= m_flArr_Rul_S[i+1]))
                {
                    iNo = i;
                    break;
                }
                if (m_flArr_Rul_S[i] == 0 && m_flArr_Rul_E[i] == 0)
                {
                    iNo = i;
                    break;
                }
            }
            return iNo;
        }
        /// <summary>
        /// 是否计算公称厚度  true:计算  false: 不计算
        /// </summary> 
        public bool Ck_No_Normal_Thickness = true;
        /// <summary>
        /// 是否退出
        /// </summary>
        public bool m_blOut = false;
        /// <summary>
        /// 锁
        /// </summary>
        public object lockValue_Pic = new object();
        /// <summary>
        /// 通道个数
        /// </summary>
        public int m_iRomoteNum = 1;

        /// <summary>
        /// 是否浏览回放
        /// </summary>
        public bool m_blBrowse = false;
        /// <summary>
        /// 当前运行时对应队列屏号
        /// </summary>
        public int m_iCurRuning_No = 0;
        /// <summary>
        /// 画尺子后列个数
        /// </summary>
        public int m_iCol_HeardNum = 0;
        /// <summary>
        /// 用于B扫多行鼠标移动查看时用
        /// true:程序运行  false:没有运行，显示数据为初始化程序时调用数据库数据
        /// </summary>
        public bool m_blHaveRun = true;
        ///// <summary>
        ///// 当前点运行屏幕序号
        ///// </summary>
        //public int iCurrRunRulerNo = 0;
        /// <summary>
        /// 画图距离：可以最远距离给，基本和测量差距不大，类似梯形图上下边的最长边距离；也可以由上一行数据给，基本是矩形框
        /// </summary>
        public float Check_iDistance_Plant = 0;
        /// <summary>
        /// 幅度、dB刻度切换 0：幅度 1：dB
        /// </summary>
        public int m_Per_dB = 0;
        /// <summary>
        /// 相位切换 使用平板和台体  0:台体 1：平板
        /// </summary>
        public int m_iAsus = 0;
        /// <summary>
        /// 幅度不对应时，调整基准
        /// </summary>
        public int m_iHigh = 100;

        /// <summary>
        /// 上次填充位置
        /// </summary>
        public int m_iLastNo = 0;
        /// <summary>
        /// 多波形色标缓存
        /// </summary>
        public System.Drawing.Color[] m_800Limit = null;
        /// <summary>
        /// 运行位置标记
        /// </summary>
       // public RunMark m_RunMark = new RunMark();
        /// <summary>
        /// 不行步长
        /// </summary>
        public int m_iWave_Step_Type = 2;
        /// <summary>
        /// 是否可以开始绘波形图
        /// </summary>
        public bool m_blCanPlantWave = true;
        /// <summary>
        /// 重绘尺子
        /// </summary>
        public bool blRepPlantRuler = false;
        /// <summary>
        /// 重绘屏内容类型 0：磁爬从起始行开始 1：当前行右边出界开始  2：直线有返回的从磁爬换行最右开始 3：直线有返回从左边出界
        /// </summary>
        public int iRepPlantType = 0;
        /// <summary>
        /// 0、1正在画刻度尺  2、3画灰度图  4：行超出后，画新屏刻度、画指定区域图
        /// </summary>
        public int blPlantRuler = 0;
        /// <summary>
        /// 画图方式 0：探头事件驱动  1：循环画图
        /// </summary>
        public int iPlant_Event = 0;
        /// <summary>
        /// 配置文件名
        /// </summary>
        public string HardFileName = "";
        /// <summary>
        /// 屏幕B图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea = null;
        /// <summary>
        /// 屏幕多图类似C图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea_TowChannel = null;
        /// <summary>
        /// 屏幕B1图波形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea_B_Wave = null;
        /// <summary>
        /// 屏幕B1图多次波形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea_B_Wave_Mul = null;
        /// <summary>
        /// 屏幕B1图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea_B = null;

        /// <summary>
        /// 屏幕C图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea_C = null;



        /// <summary>
        /// 图像变量
        /// </summary>
        public Struct_G m_G = new Struct_G();

        /// <summary>
        /// C图像变量
        /// </summary>
        public Struct_G m_G_C = new Struct_G();

        /// <summary>
        /// 双通道刷屏次数
        /// </summary>
        public int m_i_Num_C_TowChannel = 0;
        /// <summary>
        /// 2个通道C图像变量
        /// </summary>
        public Struct_G m_G_C_TowChannel = new Struct_G();
        /// <summary>
        /// 1张B图变量
        /// </summary>
        public Struct_G m_G_B_One = new Struct_G();


        /// <summary>
        /// 1张B图变量
        /// </summary>
        public Struct_G m_G_B_One_Wave = new Struct_G();

        /// <summary>
        /// 1张B图多次波形图变量
        /// </summary>
        public Struct_G m_G_PicArea_B = new Struct_G();

        #region 浏览变量
        /// <summary>
        /// 500个数据的灰度图
        /// </summary>
        Struct_G m_G_PicArea_C = new Struct_G();
        /// <summary>
        /// B图全部图
        /// </summary>
        Struct_G m_G_Browse_B = new Struct_G();
        /// <summary>
        /// 浏览C图
        /// </summary>
        Struct_G m_G_Browse_C = new Struct_G();
        #endregion 浏览变量

        #region  屏幕尺子
        /// <summary>
        /// 波形幅度调整
        /// </summary>
        public int iWave_Adjust_range_filter = 25;
        /// <summary>
        /// 1张B时图片占界面的比例 百分制 默认63%
        /// </summary>
        public int Ratio_Pic_Grup = 63;
        /// <summary>
        /// 画图类型 0：C扫图 1：B扫图
        /// </summary>
        public int Scree_Chart_B_c = 0;
        /// <summary>
        /// 屏幕宽度
        /// </summary>
        public int Scree_iScreenWith = 0;
        /// <summary>
        /// 屏幕高度
        /// </summary>
        public int Scree_iScreenHeight = 0;
        /// <summary>
        /// true;//以1毫米为数据间隔  false:以列表距离间隔为依据画列表
        /// </summary>
        public bool blGrdShowJgType = false;
        /// <summary>
        /// 一个点代表距离  默认0.5毫米  单位m
        /// </summary>
        public float Scree_iDotWithmm_X = 0.5f;
        /// <summary>
        /// 应用磁爬控制速度：一个点代表距离  默认10毫米  单位m
        /// </summary>
        public float Scree_iDotWithmm_X_Climb = 10;
        /// <summary>
        /// 一个点宽度
        /// </summary>
        public int Scree_iDotWith_X = 1;
        /// <summary>
        /// 一个点高度
        /// </summary>
        public int Scree_iDotHeight = 1;


        /// <summary>
        /// B扫纵轴一个点代表距离  默认1毫米
        /// </summary>
        public int Scree_iDotHeightmm_B_Y = 1;
        /// <summary>
        /// B扫纵轴一个点高度
        /// </summary>
        public int Scree_iDotHeight_B_Y = 1;
        /// <summary>
        /// B扫纵轴一个点最大厚度比公称厚度多mm 默认5毫米
        /// </summary>
        public int Scree_iDotHeight_B_Y_Addmm = 5;
        /// <summary>
        /// 公称厚度 画图使用
        /// </summary>
        public float flNormal_Thickness = 0;

        /// <summary>
        /// 双通道公称厚度
        /// </summary>
        public float flNormal_Thickness_2 = 0;
        /// <summary>
        /// 报警线
        /// </summary>
        public float flstrThickAlarm = 0;
        /// <summary>
        /// 一条数据占最大行数
        /// </summary>
        public int Scree_iDataMaxHeight = 0;
        /// <summary>
        /// B图时一条数据占最大行数
        /// </summary>
        public int Scree_iDataMaxHeight_B_One = 0;
        /// <summary>
        /// B扫时一屏显示总数据个数
        /// </summary>
        public int Scree_iAllDataNum_B = 0;

        /// <summary>
        /// 屏幕总行数 ：由屏幕高度和一个点高度得到   B扫图：数据行数
        /// </summary>
        public int Scree_iAllRows = 0;

        public int Scree_iAllRows_C = 0;
        /// <summary>
        /// B1屏幕总行数 ：由屏幕高度和一个点高度得到   B扫图：数据行数
        /// </summary>
        public int Scree_iAllRows_B_One = 0;
        /// <summary>
        /// 屏幕总列数：由屏幕宽度和一个点宽度得到
        /// </summary>
        public int Scree_iAllCols = 0;
        /// <summary>
        /// B图屏幕总列数：1
        /// </summary>
        public int Scree_iAllCols_B_One = 0;
        /// <summary>
        /// 一屏标准长度
        /// </summary>
        public float Scree_Stant_Distance = 0;
        /// <summary>
        /// 刻度按照 true:米 false:毫米
        /// </summary>
        public bool m_blM_mm = false;
        #endregion 屏幕尺子

        #region 图形运行变量
        /// <summary>
        /// 图形左起开始位置坐标 单位 m
        /// </summary>
        public float Chart_Run_flStart_Distance = 0;
        /// <summary>
        /// 图形右边结束位置坐标 单位 m
        /// </summary>
        public float Chart_Run_flEnd_Distance = 0;
        /// <summary>
        /// 重绘图形
        /// </summary>
        public bool m_blRepPlant = false;
        /// <summary>
        /// 图形左起开始位置坐标 单位 m
        /// </summary>
        public float Chart_Run_flStart_Distance_B_One = 0;
        /// <summary>
        /// 图形右边结束位置坐标 单位 m
        /// </summary>
        public float Chart_Run_flEnd_Distance_B_One = 0;


        /// <summary>
        /// 查询图形左起开始位置坐标 单位 m  
        /// 0：B一张图 1：B全图  2：C图
        /// </summary>
        public float[] Chart_Run_flStart_Distance_Browse = new float[5];
        /// <summary>
        /// 查询图形右边结束位置坐标 单位 m
        /// 0：B一张图 1：B全图  2：C图
        /// </summary>
        public float[] Chart_Run_flEnd_Distance_Browse = new float[5];


        /// <summary>
        /// 查询图形开始行号
        /// 0：B一张图 1：B全图  2：C图
        /// </summary>
        public int[] Chart_Run_iStart_Row_Browse = new int[5];
        /// <summary>
        /// 查询图形右边结束结束行号
        /// 0：B一张图 1：B全图  2：C图
        /// </summary>
        public int[] Chart_Run_iEnd_Row_Browse = new int[5];


        /// <summary>
        /// 查询屏幕列数  0：B一张图 1：B全图  2：C图
        /// </summary>
        public int[] Scree_iAllCols_Browse = new int[5];
        /// <summary>
        /// 查询屏幕宽度  0：B一张图 1：B全图  2：C图
        /// </summary>
        public float[] Scree_Stant_Distance_Browse = new float[5];
        /// <summary>
        /// 查询屏幕数据行数  0：B一张图 1：B全图  2：C图
        /// </summary>
        public int[] Scree_iAllRows_Browse = new int[5];
        public bool[] Scree_iAllRows_Browse_Bl = new bool[5];
        /// <summary>
        /// 查询一条数据占多少行数
        /// </summary>
        public int Scree_iDataMaxHeight_Browse_B = 0;


        /// <summary>
        /// 过山车有过原点方式，坐标两侧距离都会有数据 0: 初始距离数据 1: 过原点厚度距离数据
        /// </summary>
        public int m_iFx_Gsc = 0;

        /// <summary>
        /// 图形对应缓存开始行号
        /// </summary>
        public int Chart_Run_iStartRow_Buff = 0;
        /// <summary>
        /// 2通道换屏开始行号
        /// </summary>
        public int Chart_Run_iStartRow_Buff_TowChannel = 0;
        /// <summary>
        /// B1张图图形对应缓存开始行号
        /// </summary>
        public int Chart_Run_iStartRow_Buff_B_One = 0;
        /// <summary>
        /// 图形对应缓存当前运行行号
        /// 
        /// 1 增加规则：接收到磁爬完成命令了，并且接收到有效数据后，Chart_Run_iEndRow_Buff=iBuff_Rows赋值
        /// 2 同时判断：当 Chart_Run_iEndRow_Buff -Chart_Run_iStartRow_Buff +1 >iScree_AllRows 时，Chart_Run_iStartRow_Buff=Chart_Run_iEndRow_Buff
        /// </summary>
        public int Chart_Run_iEndRow_Buff = 0;
        /// <summary>
        /// B1张图
        /// </summary>
        public int Chart_Run_iEndRow_Buff_B_One = 0;
        /// <summary>
        /// 语言0：中文  1：英文
        /// </summary>
        public int m_iLanguage = 0;
        /// <summary>
        /// 当前图形是否出右边，用于换行时画屏 true:当前屏幕有过右出界 false:没有出界
        /// </summary>
        public bool Chart_blRightOut = false;
        #endregion 图形运行变量

        #region 图形查询变量:运行时记录当前图形对应位置
        /// <summary>
        /// 奇数行缓存计算右超屏刻度 1-N
        /// </summary>
        public float Chart_B_One_flEnd_Distance = 0;
        /// <summary>
        /// 偶数行缓存计算左超屏刻度 1-N
        /// </summary>
        public float Chart_B_One_flStart_Distance = 0;
        /// <summary>
        /// 查询图形左起开始位置坐标
        /// </summary>
        public float Chart_Brows_B_One_flStart_Distance = 0;
        /// <summary>
        /// 查询图形右边结束位置坐标
        /// </summary>
       // public float Chart_Brows_B_One_flEnd_Distance = 0;
        /// <summary>
        /// 查询图形左起开始缓存序号
        /// </summary>
        public int Chart_Brows_B_One_iStart_ColNo = 0;
        /// <summary>
        /// 查询图形右边结束缓存序号
        /// </summary>
        public int Chart_Brows_B_One_iEnd_ColNo = 0;
        /// <summary>
        /// 是否过山车 0：不是1：是
        /// </summary>
        public int Ck_Gsc = 0;
        /// <summary>
        /// 查询图形对应缓存行号
        /// </summary>
        public int Chart_Brows_B_One_Buff_iCurr_Row = 0;
        #endregion 图形查询变量

        #region 画图中字体大小
        /// <summary>
        /// 尺子字体大小
        /// </summary>
        public int Chart_Ruler_font = 8;
        ///// <summary>
        ///// 字体颜色
        ///// </summary>
        //public Color Chart_Ruler_BrushColor = Color.Wheat;
        Font drawFont = new Font("Arial", (float)10);
        SolidBrush drawBrush = new SolidBrush(Color.Black);

        SolidBrush drawBrush_One = new SolidBrush(Color.Black);
        /// <summary>
        /// 横轴刻度字在刻度线上左偏移刻度量
        /// </summary>
        public int Chart_Ruler_Word_X = 2;
        /// <summary>
        /// 横轴纵轴刻度字下降量
        /// </summary>
        public int Chart_Ruler_Word_H = 2;
        /// <summary>
        /// 线段颜色
        /// </summary>
        Pen Ruler_p = new Pen(Brushes.Black);

        Pen Ruler_p_One = new Pen(Brushes.Black);
        /// <summary>
        /// 报警线
        /// </summary>
        Pen Ruler_p_Limit = new Pen(Brushes.Red);
        /// <summary>
        /// 图形坐标：横轴开始像素位置
        /// </summary>
        public int Chart_Ruler_X_Start = 10;
        /// <summary>
        /// 左偏移量
        /// </summary>
        public int Plant_Offset = 2;
        /// <summary>
        /// 图形坐标：纵轴开始像素位置
        /// </summary>
        public int Chart_Ruler_Y_Start = 10;
        /// <summary>
        /// B扫双通道时起始位置
        /// </summary>
        public int Chart_Ruler_Y_Start_TowChannel = 0;
        /// <summary>
        /// 实际数据的高度，分成两半，中间加一个像素的宽度
        /// </summary>
      // public  int iScreen_Height = 0;//
        /// <summary>
        /// 图形坐标：刻度小数位
        /// </summary>
        public int Chart_Ruler_Xsw = 3;
        #endregion 画图中字体大小

        #region 数据缓存行号
        /// <summary>
        /// 0-N  增加规则：接收到磁爬完成命令了，并且接收到有效数据后，加1
        /// </summary>
        public int Buff_iRows = 0;

        #endregion 数据缓存行号
        /// <summary>
        /// 图像类参数读写 0：读 1：写
        /// </summary>
        /// <param name="iType"></param>
        public bool Init(int iType=0)
        {
            bool _blRet = false;
          csInterface csInter = new csInterface();
            if (HardFileName == "") HardFileName = Application.StartupPath + "\\database\\HardConfig.ini";

            if (iType == 0)
            {
                Ck_No_Normal_Thickness = int.Parse(csInter.IniReadDefine("Class_Plant", "Ck_No_Normal_Thickness", "1", HardFileName)) == 1;
                blGrdShowJgType = int.Parse(csInter.IniReadDefine("Class_Plant", "blGrdShowJgType", "1", HardFileName)) == 1;
                m_blM_mm = int.Parse(csInter.IniReadDefine("Class_Plant", "m_blM_mm", "1", HardFileName)) == 1;
                m_iHigh = int.Parse(csInter.IniReadDefine("Class_Plant", "m_iHigh", "127", HardFileName));
                m_iAsus = int.Parse(csInter.IniReadDefine("Class_Plant", "m_iAsus", "1", HardFileName));
                m_iWave_Step_Type = int.Parse(csInter.IniReadDefine("Class_Plant", "m_iWave_Step_Type", "2", HardFileName));
                iWave_Adjust_range_filter = int.Parse(csInter.IniReadDefine("Class_Plant", "iWave_Adjust_range_filter", "25", HardFileName));
                Ratio_Pic_Grup = int.Parse(csInter.IniReadDefine("Class_Plant", "Ratio_Pic_Grup", "63", HardFileName));
                Scree_Chart_B_c = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_Chart_B_c", "1", HardFileName));

                Scree_iDotHeight_B_Y_Addmm = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotHeight_B_Y_Addmm", "0", HardFileName));
                Scree_iDotHeightmm_B_Y = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotHeightmm_B_Y", "0", HardFileName));
                Scree_iDotHeight_B_Y = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotHeight_B_Y", "0", HardFileName));

                iPlant_Event = int.Parse(csInter.IniReadDefine("Class_Plant", "iPlant_Event", "0", HardFileName));
                Scree_iDotWithmm_X = float .Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotWithmm_X", "0.5", HardFileName));
                //Scree_iDotWithmm_X *= 0.001F;
  //              Scree_iDotWithmm_X = float.Parse(Scree_iDotWithmm_X.ToString("f3"));

                Scree_iDotWithmm_X_Climb = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotWithmm_X_Climb", "10", HardFileName));
                Scree_iDotWithmm_X_Climb *= 0.001F;
                Scree_iDotWithmm_X_Climb = float.Parse(Scree_iDotWithmm_X_Climb.ToString("f3"));

                Scree_iDotWith_X = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotWith_X", "1", HardFileName));
                Scree_iDotHeight = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotHeight", "1", HardFileName));

                Chart_Run_flStart_Distance = float.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_flStart_Distance", "0", HardFileName));
                Chart_Run_flEnd_Distance = float.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_flEnd_Distance", "10", HardFileName));
                Chart_Run_iStartRow_Buff = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_iStartRow_Buff", "0", HardFileName));
                Chart_Run_iEndRow_Buff = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_iEndRow_Buff", "10", HardFileName));

                Chart_Run_flStart_Distance_B_One = float.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_flStart_Distance_B_One", "0", HardFileName));
                Chart_Run_flEnd_Distance_B_One = float.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_flEnd_Distance_B_One", "10", HardFileName));
                Chart_Run_iStartRow_Buff_B_One = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_iStartRow_Buff_B_One", "0", HardFileName));
                Chart_Run_iEndRow_Buff_B_One = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Run_iEndRow_Buff_B_One", "10", HardFileName));

                Chart_Ruler_font = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_font", "8", HardFileName));
                Chart_Ruler_Word_X = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_Word_X", "2", HardFileName));
                Chart_Ruler_Word_H = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_Word_H", "2", HardFileName));
                Chart_Ruler_X_Start = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_X_Start", "10", HardFileName));
                Chart_Ruler_Y_Start = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_Y_Start", "0", HardFileName));
                Chart_Ruler_Xsw = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_Xsw", "2", HardFileName));
            }
            else
            {
                csInter.INIWriteValue("Class_Plant", "Ck_No_Normal_Thickness", (Ck_No_Normal_Thickness ? "1" : "0"), HardFileName);
                csInter.INIWriteValue("Class_Plant", "blGrdShowJgType", (blGrdShowJgType ? "1" : "0"), HardFileName);
                csInter.INIWriteValue("Class_Plant", "m_blM_mm", (m_blM_mm ? "1" : "0"), HardFileName);
                csInter.INIWriteValue("Class_Plant", "m_blM_mm", (m_blM_mm ? "1" : "0"), HardFileName);
                csInter.INIWriteValue("Class_Plant", "m_iHigh", m_iHigh.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "m_iAsus", m_iAsus.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "m_iWave_Step_Type", m_iWave_Step_Type.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "iWave_Adjust_range_filter", iWave_Adjust_range_filter.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Ratio_Pic_Grup", Ratio_Pic_Grup.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "iPlant_Event", iPlant_Event.ToString(), HardFileName);
                float iW = (float)(Scree_iDotWithmm_X * 1000f);
                csInter.INIWriteValue("Class_Plant", "Scree_Chart_B_c", Scree_Chart_B_c.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Scree_iDotHeight_B_Y_Addmm", Scree_iDotHeight_B_Y_Addmm.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Scree_iDotHeightmm_B_Y", Scree_iDotHeightmm_B_Y.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Scree_iDotHeight_B_Y", Scree_iDotHeight_B_Y.ToString(), HardFileName);

                csInter.INIWriteValue("Class_Plant", "Scree_iDotWithmm_X", iW.ToString("f0"), HardFileName);
                iW = (float)(Scree_iDotWithmm_X_Climb * 1000f);
                csInter.INIWriteValue("Class_Plant", "Scree_iDotWithmm_X_Climb", iW.ToString("f0"), HardFileName);

                csInter.INIWriteValue("Class_Plant", "Scree_iDotWith_X", Scree_iDotWith_X.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Scree_iDotHeight", Scree_iDotHeight.ToString(), HardFileName);

                csInter.INIWriteValue("Class_Plant", "Chart_Run_flStart_Distance", Chart_Run_flStart_Distance.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Run_flEnd_Distance", Chart_Run_flEnd_Distance.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Run_iStartRow_Buff", Chart_Run_iStartRow_Buff.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Run_iEndRow_Buff", Chart_Run_iEndRow_Buff.ToString(), HardFileName);

                csInter.INIWriteValue("Class_Plant", "Chart_Run_flStart_Distance_B_One", Chart_Run_flStart_Distance_B_One.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Run_flEnd_Distance_B_One", Chart_Run_flEnd_Distance_B_One.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Run_iStartRow_Buff_B_One", Chart_Run_iStartRow_Buff_B_One.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Run_iEndRow_Buff_B_One", Chart_Run_iEndRow_Buff_B_One.ToString(), HardFileName);

                csInter.INIWriteValue("Class_Plant", "Chart_Ruler_font", Chart_Ruler_font.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Ruler_Word_X", Chart_Ruler_Word_X.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Ruler_Word_H", Chart_Ruler_Word_H.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Ruler_X_Start", Chart_Ruler_X_Start.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Ruler_Y_Start", Chart_Ruler_Y_Start.ToString(), HardFileName);
                csInter.INIWriteValue("Class_Plant", "Chart_Ruler_Xsw", Chart_Ruler_Xsw.ToString(), HardFileName);
            }
            return _blRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iType"></param>
        public void Write_One(string strKey, string strVal)
        {
          csInterface csInter = new csInterface();
            if (HardFileName == "") HardFileName = Application.StartupPath + "\\database\\HardConfig.ini";

            csInter.INIWriteValue("Class_Plant", strKey, strVal, HardFileName);
        }
       /// <summary>
       /// 计算横轴点个数
       /// </summary>
        public void GetRulerPara_B_One()
        {
            if (PicArea_B == null) return;  //
            #region 由一个点宽度高度，获得屏幕参数
            int iScreen_With = PicArea_B.Width - Chart_Ruler_X_Start;
            int iScreen_Height = PicArea_B.Height - Chart_Ruler_Y_Start;

            Scree_iAllCols = iScreen_With / Scree_iDotWith_X;
            Scree_iAllRows_B_One = 1;
            #endregion 一个点宽度高度

            Scree_Stant_Distance = Scree_iAllCols * Scree_iDotWithmm_X;
        }
        /// <summary>
        /// 画B1曲线的刻度
        /// </summary>
        public void Plant_Ruler_H()
        {
            if (PicArea_B == null) return;
            int i_X = 0, i_Y = 0;
            string strT = "";
            int iLine_X = (int)(Chart_Ruler_X_Start + Scree_iAllCols * Scree_iDotWith_X);
            blPlantRuler = 0;
            Chart_Clear(m_G_PicArea_B);
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            SolidBrush _drawBrush_One = new SolidBrush(Color.White);
            Pen _Ruler_p_One = new Pen(Brushes.White);
            PointF drawPoint;

            #region 画刻度尺
            #region 0 画布准备:初始化、右边出图、满屏时
            // 初始化画板，在内存中建立一块虚拟画布
            m_G_PicArea_B.image = new Bitmap(PicArea_B.ClientSize.Width, PicArea_B.ClientSize.Height);
            // 获取背景层
            m_G_PicArea_B.bg = (Bitmap)PicArea_B.BackgroundImage;
            // 初始化整个画布
            m_G_PicArea_B.canvas = new Bitmap(PicArea_B.ClientSize.Width, PicArea_B.ClientSize.Height);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            m_G_PicArea_B.g = Graphics.FromImage(m_G_PicArea_B.image);
            m_G_PicArea_B.gb = Graphics.FromImage(m_G_PicArea_B.canvas);
            m_G_PicArea_B.g.Clear(Color.FromArgb(255, 30, 30, 30));// Color.Gray); ;
            m_G_PicArea_B.Buff = new PointF[0];
            float iLeft = Ruler_p_One.Width;
            int iTop = PicArea_B.Top;

            int i_S_H = 0, i_S_L = 0;
            float _flEndKd = 0f;
            #endregion 0 画布准备

            #region 1 画横轴刻度
            i_S_H = Chart_Ruler_Y_Start - 6; i_S_L = Chart_Ruler_Y_Start - 3;
            for (int i = 1; i <= Scree_iAllCols; i++)
            {
                i_X = (int)(Chart_Ruler_X_Start + i * Scree_iDotWith_X);
                if (i_X > PicArea_B.ClientSize.Width) break;

                _flEndKd = Chart_Run_flStart_Distance_B_One + (i * Scree_iDotWithmm_X);

                if (i % 5 != 0)//if (i % 5 != 0)
                    m_G_PicArea_B.g.DrawLine(_Ruler_p_One, new Point(i_X, i_S_L), new Point(i_X, Chart_Ruler_Y_Start));
                else
                    m_G_PicArea_B.g.DrawLine(_Ruler_p_One, new Point(i_X, i_S_H), new Point(i_X, Chart_Ruler_Y_Start));
                if (i % 10 == 0)
                {
                    #region 标记横轴刻度值
                    if (m_blM_mm == false)
                    {
                   //     _flEndKd *= 1000;
                        strT = _flEndKd.ToString("f0") + (i == 10 ? "mm" : "");
                    }
                    else
                        strT = _flEndKd.ToString("f2") + (i == 10 ? "m" : "");

                    drawPoint = new PointF(i_X - 9, Chart_Ruler_Word_H);//- Chart_Ruler_Word_X
                    m_G_PicArea_B.g.DrawString(strT, drawFont, _drawBrush_One, drawPoint);
                    #endregion 标记横轴刻度值
                }
            }
            int _iNo = Get_No(Chart_Run_flStart_Distance_B_One);
            m_flArr_Rul_S[_iNo] = Chart_Run_flStart_Distance_B_One;
            m_flArr_Rul_E[_iNo] = _flEndKd;
            Chart_Run_flEnd_Distance_B_One = _flEndKd;
          //  Chart_Brows_B_One_flEnd_Distance = _flEndKd;

            Chart_Brows_B_One_iEnd_ColNo = Chart_Brows_B_One_iStart_ColNo + Scree_iAllCols - 2;
            m_G_PicArea_B.g.DrawLine(_Ruler_p_One, new Point(Chart_Ruler_X_Start, Chart_Ruler_Y_Start), new Point(iLine_X, Chart_Ruler_Y_Start));
            #endregion 1 画横轴刻度

            #region 3 刷新
            Rectangle _Rect_Kd = new Rectangle(0, 0, PicArea_B.Width, PicArea_B.Height);
            if (m_G_PicArea_B.bg != null)
            {
                try
                {
                    m_G_PicArea_B.gb.DrawImage(m_G_PicArea_B.bg, _Rect_Kd);// 先绘制背景层
                }
                catch (Exception de)
                { }
            }
            m_G_PicArea_B.gb.DrawImage(m_G_PicArea_B.image, _Rect_Kd); // 再绘制绘画层
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            try
            {
                PicArea_B.BackgroundImage = (Bitmap)m_G_PicArea_B.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                PicArea_B.Refresh();
            }
            catch (Exception e2)
            { }
            #endregion 3
            #endregion 刻度尺

            Application.DoEvents();
            blPlantRuler = 1;
        }
        public void Plant_Ruler_Z()
        {
            if (PicArea_C == null) return;
            int i_X = 0, i_Y = 0;
            string strT = "";
            int iLine_X = (int)(Chart_Ruler_X_Start + Scree_iAllCols * Scree_iDotWith_X);
            blPlantRuler = 0;
            Chart_Clear(m_G_PicArea_C);
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            SolidBrush _drawBrush_One = new SolidBrush(Color.White);
            Pen _Ruler_p_One = new Pen(Brushes.White);

            #region 画刻度尺
            #region 0 画布准备:初始化、右边出图、满屏时
            // 初始化画板，在内存中建立一块虚拟画布
            m_G_PicArea_C.image = new Bitmap(PicArea_C.ClientSize.Width, PicArea_C.ClientSize.Height);
            // 获取背景层
            m_G_PicArea_C.bg = (Bitmap)PicArea_C.BackgroundImage;
            // 初始化整个画布
            m_G_PicArea_C.canvas = new Bitmap(PicArea_C.ClientSize.Width, PicArea_C.ClientSize.Height);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            m_G_PicArea_C.g = Graphics.FromImage(m_G_PicArea_C.image);
            m_G_PicArea_C.gb = Graphics.FromImage(m_G_PicArea_C.canvas);
            m_G_PicArea_C.g.Clear(Color.FromArgb(255, 30, 30, 30));// Color.Gray); ;
            m_G_PicArea_C.Buff = new PointF[0];
            float iLeft = Ruler_p_One.Width;
            int iTop = PicArea_C.Top;

            int i_S_H = 0, i_S_L = 0;
            #endregion 0 画布准备

            #region 1.5 画纵坐标
            float _i_X = 5;

            float flD = 0.0f, fl_D2 = 0.0f;
            string strD = "";
            float flJg_Fz_X = PicArea_C.Height / 10;
            Pen p_xy = new Pen(Brushes.CadetBlue);
            p_xy.Width = 1;
            PointF drawPoint_W = new PointF(10, 10);
            Font drawFont_TiTl_XY = new Font("Arial", 10);
            int _Chart_Ruler_X_Start = Chart_Ruler_X_Start - 3;
            i_S_H = _Chart_Ruler_X_Start - 6; i_S_L = _Chart_Ruler_X_Start - 3;
            SolidBrush drawBrush_TiTl_XY = new SolidBrush(Color.White);
            StringFormat StrF = new StringFormat();
            StrF.FormatFlags = StringFormatFlags.DirectionVertical; // 竖排

            for (int i = 1; i < 10; i++)
            {
                i_Y =  (int)(i * flJg_Fz_X);//画刻度线Chart_Ruler_Y_Start 

                flD = (i * _i_X);
                fl_D2 = (int)flD;
                strD = flD.ToString(flD == fl_D2 ? "f0" : "f1") + (i == 9 ? " mm" : "");

                m_G_PicArea_C.g.DrawLine(_Ruler_p_One, new Point(i_S_H, i_Y), new Point(_Chart_Ruler_X_Start, i_Y));
                m_G_PicArea_C.g.DrawString(strD , drawFont, _drawBrush_One, -3, i_Y - (i==1?10: 18), StrF);
            }
            m_G_PicArea_C.g.DrawLine(_Ruler_p_One, new Point(_Chart_Ruler_X_Start, 0), new Point(_Chart_Ruler_X_Start, PicArea_C.ClientSize.Height));//Chart_Ruler_Y_Start

            #endregion 1.5
            #region 3 刷新
            Rectangle _Rect_Kd = new Rectangle(0, 0, PicArea_C.Width, PicArea_C.Height);
            if (m_G_PicArea_C.bg  != null)
            {
                try
                {
                    m_G_PicArea_C.gb.DrawImage(m_G_PicArea_C.bg, _Rect_Kd);// 先绘制背景层
                }
                catch (Exception de)
                { }
            }
            m_G_PicArea_C.gb.DrawImage(m_G_PicArea_C.image, _Rect_Kd); // 再绘制绘画层
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;
            try
            {
                PicArea_C.BackgroundImage = (Bitmap)m_G_PicArea_C.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                PicArea_C.Refresh();
            }
            catch (Exception e2)
            { }
            #endregion 3
            #endregion 刻度尺

            Application.DoEvents();
            blPlantRuler = 1;
        }
        /// <summary>
        /// 绘制当前距离数据
        /// </summary>
        /// <param name="iKey_Dist">1-N：0.1mm-N/10mm</param>
        /// <param name="x_Data">报文数据</param>
        public void Plant_CurrDist(int iKey_Dist, Class_X_Data x_Data)
        {
            try
            {
                //1 判断当前点是否在图形刻度尺范围
                float _flKey_Dist = iKey_Dist / 10f;//转换成实际距离

                int _i_BeginPlan = -1;//准备画初始图  0:左图 1：右图
                int i_X = 0, i_Y = 0;
                float flJg_H = 0;
                int iDat = 0;//测量高度数据
                int _fl_One_H = 1;

                if (x_Data.m_iprofileCnt > 0)
                {
                    flJg_H = (PicArea_C.Height) / ((float)x_Data.m_iprofileCnt);// - (Chart_Ruler_Y_Start + Scree_iDotHeight_B_Y)
                                                                                //      _fl_One_H = int.Parse(flJg_H.ToString());
                }

                if (_flKey_Dist > Chart_Run_flEnd_Distance_B_One) _i_BeginPlan = 1;
                if (_flKey_Dist < Chart_Run_flStart_Distance_B_One) _i_BeginPlan = 0;
                #region  2 不在就重新绘制刻度尺
                if (_i_BeginPlan == 1)
                {
                    m_blRepPlant = true;
                    Chart_Run_flStart_Distance_B_One = Chart_Run_flEnd_Distance_B_One + Scree_iDotWithmm_X;
                    Plant_Ruler_H();
                    Plant_Ruler_Z();
                }
                if (_i_BeginPlan == 0)
                {
                    m_blRepPlant = true;
                    int _iNo = Get_No(_flKey_Dist);
                    Chart_Run_flStart_Distance_B_One = m_flArr_Rul_S[_iNo];
                    Plant_Ruler_H();
                    Plant_Ruler_Z();
                }
                m_blRepPlant = false ;
                #endregion  2

                #region  3 绘制当前点对于灰度图
                i_X = GetPointByDistance(_flKey_Dist);
                i_Y = (int)(Chart_Ruler_Y_Start + Scree_iDotHeight_B_Y);
                #region 3.1 显示 如果有缺陷，就画报警
                if ( i_X <= PicArea_B.Width && i_Y <= PicArea_B.Height)
                {
                    InitColor(m_G_PicArea_B.g, i_X, i_Y , Scree_iDotWith_X, Scree_iDotWith_X+3,
                        x_Data.iDownNum >0 ? Color.FromArgb(255, 255, 0, 0): Color.FromArgb(255, 30, 30, 30));

                    Rectangle _Rect = new Rectangle(0, 0, PicArea_B.Width, PicArea_B.Height);
                    try
                    {
                        if (m_G_PicArea_B.bg != null)
                            try
                            {
                                m_G_PicArea_B.gb.DrawImage(m_G_PicArea_B.bg, _Rect);// 先绘制背景层
                            }
                            catch (Exception ebg)//GDI+ 中发生一般性错误。
                            {
                            }
                        m_G_PicArea_B.gb.DrawImage(m_G_PicArea_B.image, _Rect); // 再绘制绘画层
                        PicArea_B.BackgroundImage = (Bitmap)m_G_PicArea_B.canvas.Clone();
                    }
                    catch (Exception e)//GDI+ 中发生一般性错误。
                    {
                    }
                }
                #endregion 3.1

                #region 3.2
                if (x_Data.m_iprofileCnt > 0)
                {
                    //int _iStart = x_Data.m_i_W_End - 100;
                    //_iStart = _iStart < 0 ? 0 : _iStart;
                    //int _iEnd = x_Data.m_i_W_End + 170;
                    //_iEnd = _iEnd > x_Data.m_iprofileCnt ? x_Data.m_iprofileCnt: _iEnd;
                    int[] _Arr = new int[500];
                    for (int iNo = 0; iNo < x_Data.m_iprofileCnt; iNo++)
                    {
                        iDat = int.Parse(((x_Data.m_Arr_profileZ[iNo] - x_Data.m_dbL_Min_H ) * 10).ToString("f0"));
                        iDat += 100;

                        if (iDat > 255) 
                            iDat = 255;
                        _Arr[iNo] = iDat;
                        if (iDat > 0)
                        {
                            i_Y = (int)(iNo * flJg_H);//iStartChart_Ruler_Y_Start
                            InitColor(m_G_PicArea_C.g, i_X, i_Y, Scree_iDotWith_X, _fl_One_H, System.Drawing.Color.FromArgb((byte)iDat, (byte)iDat, (byte)iDat));
                        }
                    }

                    Rectangle _Rect = new Rectangle(0, 0, PicArea_C.Width, PicArea_C.Height);

                    if (m_G_PicArea_C.bg != null)
                        m_G_PicArea_C.gb.DrawImage(m_G_PicArea_C.bg, _Rect);// 先绘制背景层

                    m_G_PicArea_C.gb.DrawImage(m_G_PicArea_C.image, _Rect); // 再绘制绘画层
                    PicArea_C.BackgroundImage = (Bitmap)m_G_PicArea_C.canvas.Clone();
                }
                #endregion 3.2 一帧数据的灰度图
                #endregion  3
            }
            catch (Exception e)
            {

            }
        }
        /// <summary>
        /// 画色块
        /// </summary>
        /// <param name="G">画布</param>
        /// <param name="intStar_X">起点X</param>
        /// <param name="intStar_Y">起点Y</param>
        /// <param name="iWith">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <param name="color">颜色</param>
        public void InitColor(Graphics G, int intStar_X, int intStar_Y, int iWith, int iHeight, Color color)
        {
            SolidBrush mybrush = new SolidBrush(color);
            try
            {
                G.FillRectangle(mybrush, intStar_X, intStar_Y, iWith, iHeight);
            }
            catch (Exception e)
            { }
        }
        public int GetPointByDistance(float _flDistance_X)//, ref bool blX)
        {
            //1 转换成整数
            int _i_X = 0;
            float _flDat = (_flDistance_X - Chart_Run_flStart_Distance_B_One) / Scree_iDotWithmm_X;//单位m
            _flDat = float.Parse(_flDat.ToString("f1"));
            _i_X = (int)_flDat;//由实际点计算屏幕开始序号
          //  blX = _i_X == 0;
            if (_flDat - _i_X >= 0.5) _i_X++;
            //2 转换成屏幕列位置
            _i_X = (int)(Chart_Ruler_X_Start + _i_X * Scree_iDotWith_X);// + (blX ? 1 : 0);//由序号计算屏幕对应的像素位置

            return _i_X;
        }

        /// <summary>
        /// 图形清零
        /// </summary>
        public void Chart_Clear_C()
        {
            if (m_G_C.g != null)
            {
                m_G_C.g.Dispose(); m_G_C.g = null;
                m_G_C.image.Dispose();
                m_G_C.gb.Dispose();
                m_G_C.canvas.Dispose();
                Application.DoEvents();
            }
        }
        public void Chart_Clear_C_TowChannel()
        {
            if (m_G_C_TowChannel.g != null)
            {
                m_G_C_TowChannel.g.Dispose(); m_G_C_TowChannel.g = null;
                m_G_C_TowChannel.image.Dispose();
                m_G_C_TowChannel.gb.Dispose();
                m_G_C_TowChannel.canvas.Dispose();
                Application.DoEvents();
            }
        }
        public void Chart_Clear_B_One()
        {
            if (m_G_B_One.g != null)
            {
                m_G_B_One.g.Dispose(); m_G_B_One.g = null;
                m_G_B_One.image.Dispose();
                m_G_B_One.gb.Dispose();
                m_G_B_One.canvas.Dispose();
                Application.DoEvents();
            }
        }
        public void Chart_Clear(Struct_G mg)
        {
            if (mg.g != null)
            {
                mg.g.Dispose(); mg.g = null;
                mg.image.Dispose();
                mg.gb.Dispose();
                mg.canvas.Dispose();
                Application.DoEvents();
            }
        }

        /// <summary>
        /// 清理图缓存
        /// </summary>
        /// <param name="G"></param>
        public void Chart_Clear(ref Struct_G G)
        {
            if (G.g != null)
            {
                G.g.Dispose(); G.g = null;
                G.image.Dispose();
                G.gb.Dispose();
                G.canvas.Dispose();
                Application.DoEvents();
            }
        }
       
    }  
    
    #endregion  
}
