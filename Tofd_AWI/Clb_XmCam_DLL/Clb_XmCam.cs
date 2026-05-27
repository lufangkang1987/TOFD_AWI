using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Forms;
using System.Threading;

namespace Clb_XmCam_DLL
{
    public class Clb_XmCam
    {
        #region 变量
        /// <summary>
        /// 是否退出程序true: 退出 false: 正常运行
        /// </summary>
        public bool blOut = false;
        /// <summary>
        ///  镜头 1：前视 3：探头  2：后视
        /// </summary>
        public  int m_iF_B = 1;
        /// <summary>
        /// 相机1 联机 0：成功  -1：失败
        /// </summary>
        public int m_blReLink_1 = -1;
        /// <summary>
        /// 相机2 联机 0：成功  -1：失败
        /// </summary>
        public int m_blReLink_2 = -1;
        /// <summary>
        /// 左相机缓存
        /// </summary>
        public byte[] m_Img_L;
        /// <summary>
        /// 右相机缓存
        /// </summary>
        public byte[] m_Img_R;
       

        csInterface m_csInter = new csInterface();

        //1 相机初始化
        [DllImport("V_DLL.dll", EntryPoint = "pair_image_capture", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_capture(int camera_w, int camare_h, string Str_In_Key_Value, string Ip, string Ip2, string iPort, int[] _iOutArr);


        /// <summary>
        /// 模拟相机打开
        /// </summary>
        /// <param name="index"></param>
        /// <param name="camera_w"></param>
        /// <param name="camare_h"></param>
        /// <returns></returns>
        [DllImport("V_DLL.dll", EntryPoint = "pair_image_1", CallingConvention = CallingConvention.Cdecl)]
        
        static extern int OpenCamera(int index, int camera_w, int camare_h);




        [DllImport("V_DLL.dll", EntryPoint = "pair_image_1", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_1(int camera_w, int camare_h, string Str_In_Key_Value, string Ip1, string iPort, int[] _iOutArr);

        [DllImport("V_DLL.dll", EntryPoint = "pair_image_2", CallingConvention = CallingConvention.Cdecl)]
        static extern int pair_image_2(int camera_w, int camare_h, string Str_In_Key_Value, string Ip2, string iPort, int[] _iOutArr);

        //2 关闭
        [DllImport("V_DLL.dll", EntryPoint = "Set_Flag", CallingConvention = CallingConvention.Cdecl)]
        static extern void Set_Flag(int iType);  //2   0：视频、 1：关闭相机   2：拍照
        //3 拿相机图像
        [DllImport("V_DLL.dll", EntryPoint = "Video_1", CallingConvention = CallingConvention.Cdecl)]
        static extern int Video_1(byte[] ImageBuffer); 
        [DllImport("V_DLL.dll", EntryPoint = "Video_2", CallingConvention = CallingConvention.Cdecl)]
        static extern int Video_2(byte[] ImageBuffer);
        /// <summary>
        /// 模拟相机拿视频图像
        /// </summary>
        /// <param name="ImageBuffer"></param>
        /// <returns></returns>

        [DllImport("V_DLL.dll", EntryPoint = "Video_0", CallingConvention = CallingConvention.Cdecl)]
        static extern int Video_0(byte[] ImageBuffer);
        string m_NetP = Application.StartupPath + "\\database\\SysConfig.ini";
        /// <summary>
        /// 300万相机像素 宽度
        /// </summary>
        public int m_iW_iMG = 2304;
        /// <summary>
        /// 相机类型： 0模拟，1网络
        /// </summary>
        public int m_i_Mn0_Net1 = 1;
        /// <summary>
        ///  300万相机像素 高度
        /// </summary>
        public int m_iH_iMG = 1296;
        /// <summary>
        /// 相机图像变量长度
        /// </summary>
        int m_iCam_Len = 0;
        string m_IP_1 = "192.168.1.12";
        string m_IP_2 = "192.168.1.13";
        string m_Port = "554";
        /// <summary>
        /// 视频1运行
        /// </summary>
         Thread Thread_RunVideo_1 = null;
        /// <summary>
        /// 视频2运行
        /// </summary>
         Thread Thread_RunVideo_2 = null;


        #endregion 变量

        #region 方法
        public void Close()
        {
            blOut = true;
            try
            {
                Set_Flag(1);
            }
            catch (Exception e)
            {
            }
        }
        /// <summary>
        /// 相机联机
        /// </summary>
        public void Link_Phone()//PictureBox Pho_Video)
        {
            blOut = true;
            m_NetP = Application.StartupPath + "\\database\\SysConfig.ini";
            m_i_Mn0_Net1 = int.Parse(m_csInter.IniReadDefine("Cam", "m_i_Mn0_Net1", "1", m_NetP));
            m_iW_iMG = int.Parse(m_csInter.IniReadDefine("Cam", "m_iWith", "2304", m_NetP));
            m_iH_iMG = int.Parse(m_csInter.IniReadDefine("Cam", "m_iHeight", "1296", m_NetP));
            m_IP_1 = m_csInter.IniReadDefine("Cam", "m_Ip_1", "192.168.1.12", m_NetP);
            m_IP_2 = m_csInter.IniReadDefine("Cam", "m_Ip_2", "192.168.1.13", m_NetP);
            m_Port = m_csInter.IniReadDefine("Cam", "Port_1", "554", m_NetP);

            m_iCam_Len = m_iW_iMG * m_iH_iMG * 3;
            m_Img_L = new byte[m_iCam_Len];
            m_Img_R = new byte[m_iCam_Len];

            try
            {
                Set_Flag(m_i_Mn0_Net1==1?1:2);
            }
            catch (Exception e)
            {
                MessageBox.Show("调用DLL失败");
            }
            blOut = false;
            if (m_IP_1 != "")
            {
                if (Thread_RunVideo_1 != null) Thread_RunVideo_1.Abort();
                Thread_RunVideo_1 = new Thread(new ThreadStart(RunVideo_1));
                Thread_RunVideo_1.IsBackground = true;
                Thread_RunVideo_1.Start();
            }
            if (m_IP_2 != "" && m_i_Mn0_Net1==1)
            {
                Waite(0.2f);
                if (Thread_RunVideo_2 != null) Thread_RunVideo_2.Abort();
                Thread_RunVideo_2 = new Thread(new ThreadStart(RunVideo_2));
                Thread_RunVideo_2.IsBackground = true;
                Thread_RunVideo_2.Start();
            }
        }
        private void RunVideo_1()
        {
            int[] _ArrOut = new int[3];
            //返回 0:成功  -1：失败
            try
            {
             //   m_blReLink_1 = pair_image_1(m_iW_iMG, m_iH_iMG, "", m_IP_1, m_Port, _ArrOut);

                int iTimes = 0;
                while (m_blReLink_1 != 0 && iTimes < 2)
                {
                    if(m_i_Mn0_Net1==0)
                    {
                        m_blReLink_1= OpenCamera(0, m_iW_iMG, m_iH_iMG)==1?0:-1;
                    }
                    else
                    m_blReLink_1 = pair_image_1(m_iW_iMG, m_iH_iMG, "", m_IP_1, m_Port, _ArrOut);
                    if (m_blReLink_1 == 0) break;
                    Waite(0.1f);
                    iTimes++;
                }
            }
            catch (Exception e)
            { }
            if(m_blReLink_1==0)
            while (blOut==false )
            {
                    if (m_blReLink_1 == 0)
                    {
                        if (m_i_Mn0_Net1 == 0)
                            Video_0(m_Img_L);
                        else
                            Video_1(m_Img_L);
                        Thread.Sleep(5);
                    }
                    else
                        Thread.Sleep(100);
               Application.DoEvents();
            }
        }
        private void Waite(float flWait = 1)
        {
            DateTime _dtStar = DateTime.Now;
            while (true)
            {
                try
                {
                    Application.DoEvents();
                    if (DateTime.Now.Subtract(_dtStar).TotalSeconds > flWait) break;
                    Thread.Sleep(1);
                }
                catch { break; }
            }
        }
        private void RunVideo_2()
        {
            int[] _ArrOut = new int[3];
            //返回 0:成功  -1：失败
            try
            {
                int iTimes = 0;
                while (m_blReLink_2 != 0 && iTimes<3)
                {
                    m_blReLink_2 = pair_image_2(m_iW_iMG, m_iH_iMG, "", m_IP_2, m_Port, _ArrOut);
                    if (m_blReLink_2 == 0) break;
                    Waite(0.1f);
                    iTimes++;
                }

            }
            catch (Exception e)
            { }
            if (m_blReLink_2 == 0)
                while (blOut == false)
                {
                    if (m_blReLink_2 == 0)
                    {
                        Video_2(m_Img_R);
                        Thread.Sleep(5);
                    }
                    else
                        Thread.Sleep(100);
                    Application.DoEvents();
                }
        }

        #endregion  方法
    }
  
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
                MessageBox.Show("Delete Files：" + strPathFile + "error！" + Err.Message);
            }
        }

    }

}
