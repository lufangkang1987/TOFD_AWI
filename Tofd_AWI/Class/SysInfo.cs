using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClassLibrary_Interface;
using Clb_MT_Comm;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

using System.Reflection;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using ClassLib_TestData;

using Cmm_PcPower;//电池
using System.IO;//文件操作引用

namespace Tofd_AWI.Class
{
    /// <summary>
    /// 系统资源
    /// </summary>
    public  class  SysInfo
    {
        /// <summary>
        /// 0：中文 1：英文
        /// </summary>
        public static int m_iLanguage = 0;
        /// <summary>
        /// 读取通道数据
        /// </summary>
        public static Thread Thread_Get_TofdData = null;
        /// <summary>
        /// 程序运行名称
        /// </summary>
        public static string m_strProName = "TOFD自动焊缝检测系统";
        /// <summary>
        /// 前后视频连接状态 00 01 10 11  0:联机成功  1：2号失败 2:1号失败  3：都失败
        /// </summary>
        public static int iNetTrue_Video = -1;
        /// <summary>
        /// 1：开始 0：停止 2: 检定完成  10:退出程序
        /// </summary>
        public static int m_iRun = 0;

        /// <summary>
        /// 窗体是否打开0：Tofd 1:项目管理 2：焊缝3：车体控制  4: 标注
        /// </summary>
        public static bool[] m_blFrmOpen = new bool[5];
        /// <summary>
        /// TOFD俩个子窗口
        /// </summary>
        public static bool[] m_blFrm_TOFD_Open = new bool[2];
        /// <summary>
        /// 显示哪个相机图像 0：前置 1：后置 2：寻迹
        /// </summary>
        public static int m_iShowPhone = 0;
        /// <summary>
        /// 联机信息
        /// </summary>
        public static string m_strLinkMsg = "";

        #region 检测数据
        /// <summary>
        /// 当前测量0：查询状态：1
        /// </summary>
        public static int m_i_Cl0_Cx1 = 0;
        /// <summary>
        /// 子文件名称，方便共享
        /// </summary>
        public static string m_strSaveDataFileName = "";//子文件名称
        /// <summary>
        /// 步骤是否提示
        /// </summary>
        public static bool m_blRunStepTips = true;
        /// <summary>
        /// 项目检测记录
        /// </summary>
        public static Class_Test_Item m_Test_Item = new Class_Test_Item();
        /// <summary>
        /// 检测焊缝
        /// </summary>
        public static Class_Test_Parts m_Test_Parts = new Class_Test_Parts();
        /// <summary>
        /// 测量原始记录
        /// </summary>
        public static Class_Test_Records m_Test_Record = new Class_Test_Records();
        /// <summary>
        /// 异常点记录
        /// </summary>
        public static  Class_Test_AlarmArea m_Test_Alarm = new Class_Test_AlarmArea();

        /// <summary>
        /// 标注数据参数
        /// </summary>
        public static ClBiaoZhu m_BiaoZhu = new ClBiaoZhu();
        /// <summary>
        /// 标注序号 
        /// </summary>
        public static CL_BiaoZ_S m_BiaoZhu_No = new CL_BiaoZ_S();
        #endregion 检测数据
        /// <summary>
        /// 电池
        /// </summary>
        public static ClasPcPower m_PcPower;
        /// <summary>
        /// 采集数据
        /// </summary>
        public static bool  blCollection = true;
        /// <summary>
        /// Tofd控件
        /// </summary>
        public static Tofd m_Tofd_DLL = new Tofd();
        /// <summary>
        /// 文件读写
        /// </summary>
        public static ClassInterFace csInter = new ClassInterFace();
        public static string HardFileName = Application.StartupPath + "\\database\\SysConfig.ini";

        /// <summary>
        /// 消息接口
        /// </summary>
        public static ClassLib_TestData.MsgInterFace g_Msg_InterFace = ClassLib_TestData.MsgInterFace.GetInstance();
        #region 车体信息
        /// <summary>
        /// 接收服务器数据
        /// </summary>
        public static TcpClient_UI m_Client;
        /// <summary>
        /// 寻迹类型 2：测高 0: 色带 1：焊缝
        /// </summary>
        public static int m_iTrack_Type = 2;
        /// <summary>
        /// 接收到的测高寻迹报文
        /// </summary>
        public static string m_strComMsg = "";
        /// <summary>
        /// 焊缝寻迹服务器IP
        /// </summary>
        public static string IP_XunJi_Server = "";
        /// <summary>
        /// 接收到的测高寻迹数据
        /// </summary>
        public static List<Class_Xj_GetData> m_lst_Weld = new List<Class_Xj_GetData>();

        /// <summary>
        /// 4轮控制
        /// </summary>
        public static Clb_MT_Comm.MT_Comm m_Climb4;
        /// <summary>
        /// 距离信息
        /// </summary>
        public static ClassLib_TestData.clStreamVideo m_Climb = new ClassLib_TestData.clStreamVideo();
        
        /// <summary>
        /// Can刷新时间间隔
        /// </summary>
        public static int m_i_Can_BrushTime = 20;
        #endregion 车体信息

        #region 相机
        /// <summary>
        /// 图像高度比例
        /// </summary>
        public static float  m_flPic_Height = 30;
        /// <summary>
        /// 相机IP
        /// </summary>
        public static string m_strIP = "";
        /// <summary>
        /// 用户名
        /// </summary>
        public static string m_strUser = "";
        /// <summary>
        /// 密码
        /// </summary>
        public static string m_strPwd = "";
        /// <summary>
        /// 相机分辨率 宽度
        /// </summary>
        public static int iWidth = 0;
        /// <summary>
        /// 相机分辨率 高度
        /// </summary>
        public static int iHeight = 0;
        #endregion 相机

        #region 系统画图

        public static Cls_Plant m_Plant = new Cls_Plant();
        #endregion 系统画图

        #region 列表函数

        /// <summary>
        /// 列表界面初始化 0:项目  1：焊缝 2：报警
        /// </summary>
        /// <param name="Dg_TestItem"></param>
        /// <param name="iType">0:项目  1：焊缝 2：报警 3: 异常数据标注</param>
        /// <returns></returns>
        public static float Grd_IniGrid_ShowZdName(DataGridView Dg_TestItem, int iType)
        {
            float _flRet = 0;
            try
            {
                string strSection = "";//搜寻配置文件主键
                string strTmp = "";
                //1 读要求显示字段个数
                int iMax = 0;
                string[] Para = null;
                int iNo = 0;//字段序号
                string[] _ArrTitl = null;  //  #region 1 获得显示字段配置主键  字段名|标题名|文本框或下拉框(0-文本框,1-下拉框)|是否显示(0-不显示,1-显示)|列宽度
                switch (iType)
                {
                    case 0://项目记录    
                        #region
                        strSection = "IniGrid_Item";
                        iNo = 0;
                        iMax = Int32.Parse(csInter.INIReadValue(strSection, "sum", "0", HardFileName));
                        if (iMax == 0)
                        {
                          //  csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Row|行号|0|1|60", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_ID|ID|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_Dwmc|单位名称|0|1|150", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_ItemName|项目名称|0|1|120", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_Sbbh|设备编号|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_Testblock|试块名称|0|1|80", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_Stand|检测依据|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "c_Jyy|检验员|0|1|90", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "sum", iNo.ToString(), HardFileName);
                            iMax = iNo;
                        }
                        #endregion
                        break;
                    case 1://焊缝记录
                        #region
                        iNo = 0;
                        strSection = "IniGrid_Weld";
                        iMax = Int32.Parse(csInter.INIReadValue(strSection, "sum", "0", HardFileName));
                        if (iMax == 0)
                        {
                          //  csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Row|行号|0|1|60", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Sub_ID|焊缝ID|0|1|120", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Part_No|焊缝编号|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "DetectionSite|外壁/内壁|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flThicknise|厚度|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "ProbeSpacing|探头间距|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "ReMark|备注|0|1|190", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "sum", iNo.ToString(), HardFileName);
                            iMax = iNo;
                        }
                        #endregion
                        break;
                    #region 删除
                    //case 2://异常数据
                    //    #region
                    //    iNo = 0;
                    //    strSection = "IniGrid_Alarm";
                    //    iMax = Int32.Parse(csInter.INIReadValue(strSection, "sum", "0", HardFileName));
                    //    if (iMax == 0)
                    //    {
                    //    //    csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Row|行号|0|1|60", HardFileName); iNo++;

                    //        csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Sub_ID|焊缝ID|0|1|120", HardFileName); iNo++;
                    //        csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Part_No|焊缝编号|0|1|120", HardFileName); iNo++;
                    //        csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "strType|类型|0|1|120", HardFileName); iNo++;
                    //        csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flDistancX_Start|开始距离|0|1|90", HardFileName); iNo++;
                    //        csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flDistancX_End|结束距离|0|1|90", HardFileName); iNo++;

                    //        csInter.INIWriteValue(strSection, "sum", iNo.ToString(), HardFileName);
                    //        iMax = iNo;
                    //    }
                    //    #endregion
                    //    break;
                    #endregion 删除
                    case 2://异常数据标注
                        #region
                        iNo = 0;
                        strSection = "IniGrid_TOFD_Bz";
                        iMax = Int32.Parse(csInter.INIReadValue(strSection, "sum", "0", HardFileName));
                        if (iMax == 0)
                        {
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "ID|项目ID|0|1|1", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Sub_ID|焊缝ID|0|1|1", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Part_No|焊缝编号|0|1|1", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "strType|缺陷类型|0|1|120", HardFileName); iNo++;
                     

                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flLen|长度|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flLen_S|开始距离|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flLen_E|结束距离|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "iX_No|X轴No|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "iY_No|Y轴No|0|1|90", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flHeight|高度|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flHeight_S|开始时间|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flHeight_E|结束时间|0|1|90", HardFileName); iNo++;
                           
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flDepth|深度|0|1|90", HardFileName); iNo++;
                            csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flDepth_S|深度开始时间|0|1|90", HardFileName); iNo++;

                            csInter.INIWriteValue(strSection, "sum", iNo.ToString(), HardFileName);
                            iMax = iNo;
                        }
                        #endregion
                        break;
                    default:
                        break;
                }
               // #endregion

                Dg_TestItem.RowCount =  2;//总行数
                Dg_TestItem.ColumnCount = 1;

                //1 列号  4轮C扫查显示当前数据是第几排  ，下面行为光栅臂的位置
                Dg_TestItem[0, 0].Value =  "NO";
                Dg_TestItem[0, 0].Style.BackColor = System.Drawing.Color.SeaShell;
                Dg_TestItem[0, 0].ReadOnly = true;//定义第二列是选择列
                #region 2 画列表头
                try
                {
                    for (int i = 0; i < iMax; i++)
                    {
                        //        Application.DoEvents();
                        //2 读当前字段显示值：  0：字段名|       1：标题|      :2：文本框或下拉框(0-文本框,1-下拉框)|     :3： 此字段是否显示(0-不显示,1-显示)|     :4： 列宽度
                        //if (iType == 1)
                        //    strTmp = _ArrTitl[i];
                        //else
                            strTmp = csInter.INIReadValue(strSection, "P" + i.ToString(), "", HardFileName);
                        Para = strTmp.Split('|');
                        //3 要求显示此字段
                        if (Para.Length > 3)
                            if (Para[3].Replace(" ", "") == "1")
                            {
                                try
                                {
                                    //4 添加此列数据
                                    DataGridViewTextBoxColumn txtCol = new DataGridViewTextBoxColumn();
                                    txtCol.Name = Para[0].ToUpper();//字段名称

                                    txtCol.HeaderText = Para[1];//标题头名称
                                    if (Para.Length == 5)
                                    {
                                        txtCol.Width = int.Parse(Para[4]);//列宽度
                                    }
                                    else
                                        txtCol.Width = 80;//列宽度
                                    txtCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                    Dg_TestItem.Columns.Add(txtCol);//列表增加此列

                                    Dg_TestItem[Para[0], 0].Value = Para[1];//标题头名称
                                    Dg_TestItem[Para[0], 0].Style.BackColor = System.Drawing.Color.SeaShell;
                                    Dg_TestItem[Para[0], 0].ReadOnly = true;
                                    Dg_TestItem[Para[0], 0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    Dg_TestItem[Para[0], 0].Tag = (object)Para[2];//记录下此列显示 文本/下拉框，供点击时判断使用

                                    //      _strZdNameList += ((_strZdNameList == "" ? "" : ",") + txtCol.Name);//获得配置文件中要求显示的方案表字段名称
                                }
                                catch (Exception e)
                                { }
                            }
                    }
                }
                catch (Exception e)
                {

                }
                #endregion 2
                Dg_TestItem.Rows[0].Frozen = true;//列头行固定
                                                  //if (iType != 5)
                Dg_TestItem.Columns[0].Frozen = true;//列头第1列固定
                Dg_TestItem.Columns[0].Width = iType != 5 ? 50 : 20;
                switch (iType)
                {//1: 全部数据  2: 报警数据  3: 查询指定数据 4: 伤面积数据  5: 视频数据 6:单位名牌信息 7:波形原始数据 8:伤点统计列表</param>
                    case 1:
                        Dg_TestItem.Columns[1].Frozen = true;//列头第1列固定
                        break;

                    default:
                        if (iType == 2)
                        {
                            Dg_TestItem.Columns[0].Frozen = true;//列头第1列固定
                            Dg_TestItem.Columns[3].Frozen = true;

                        }
                        else
                        {
                            Dg_TestItem.Columns[1].Frozen = true;//列头第1列固定
                            Dg_TestItem.Columns[2].Frozen = true;
                        }
                        break;
                }

                Dg_TestItem.ColumnHeadersVisible = false;// 列标题不显示
                Dg_TestItem.RowHeadersVisible = false;
            }
            catch (Exception Ex)
            {
                /// m_csInter.WriteErrorLog(ErrFile, Ex.Message, true);
            }
            return _flRet;
        }
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="Dg_TestItem"></param>
        /// <param name="iR">0-N</param>
        public static bool Grd_AddData(DataGridView Dg_TestItem, int iR, Class_Test_Item Test_Item)
        {
            bool _blRet = false;
            try
            {
                #region 如果当前行大于列表行，添加新行
                int iRow = iR + 1;//1-N
                int iCol = 1;
                //if (iRow + 1 > Dg_TestItem.RowCount)
                //    Dg_TestItem.RowCount = iRow + 1;
                #endregion
                if (iRow + 1 > Dg_TestItem.RowCount) return _blRet;
                if (iCol + 1 > Dg_TestItem.ColumnCount)
                    return _blRet;
                if (iCol < 0 || iRow < 1) return _blRet;
                try
                {
                    for (int i = 0; i < Dg_TestItem.ColumnCount; i++)
                        Dg_TestItem[i, iRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch { }
                Dg_TestItem[iCol++, iRow].Value = iRow.ToString();

                Dg_TestItem[iCol++, iRow].Value = Test_Item.ID;
                Dg_TestItem[iCol++, iRow].Value = Test_Item.Dwmc;
                Dg_TestItem[iCol++, iRow].Value = Test_Item.ItemName;
                Dg_TestItem[iCol++, iRow].Value = Test_Item.Sbbh;
                Dg_TestItem[iCol++, iRow].Value = Test_Item.Testblock;
                Dg_TestItem[iCol++, iRow].Value = Test_Item.Testing_Standard;
                Dg_TestItem[iCol++, iRow].Value = Test_Item.strJyy;

                Dg_TestItem.FirstDisplayedScrollingRowIndex = iRow;
                _blRet = true;
            }
            catch (Exception e3)
            {

            }//MessageBox.Show("Grd_AddData：" + e3.Message + e3.StackTrace); }
            return _blRet;
        }

        /// <summary>
        /// 焊缝列表
        /// </summary>
        /// <param name="Dg_TestItem"></param>
        /// <param name="iR">0-N</param>
        public static bool Grd_AddData(DataGridView Dg_TestItem, int iR, Class_Test_Parts Test_Part)
        {
            bool _blRet = false;
            try
            {
                #region 如果当前行大于列表行，添加新行
                int iRow = iR + 1;//1-N
                int iCol = 0;
                //if (iRow + 1 > Dg_TestItem.RowCount)
                //    Dg_TestItem.RowCount = iRow + 1;
                #endregion
                if (iRow + 1 > Dg_TestItem.RowCount) return _blRet;
                if (iCol + 1 > Dg_TestItem.ColumnCount)
                    return _blRet;
                if (iCol < 0 || iRow < 1) return _blRet;
                try
                {
                    for (int i = 0; i < Dg_TestItem.ColumnCount; i++)
                        Dg_TestItem[i, iRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch { }
                Dg_TestItem[iCol, iRow].Value = iRow.ToString(); iCol++;
           //     Dg_TestItem[iCol++, iRow].Value = Test_Part.ID;
                Dg_TestItem[iCol, iRow].Value = Test_Part.Sub_ID; iCol++;
                Dg_TestItem[iCol, iRow].Value = Test_Part.Part_No; iCol++;
                Dg_TestItem[iCol, iRow].Value = Test_Part.DetectionSite==0?"外壁":"内壁"; iCol++;
                Dg_TestItem[iCol, iRow].Value = Test_Part.flThicknise; iCol++;
                Dg_TestItem[iCol, iRow].Value = Test_Part.ProbeSpacing; iCol++;
                Dg_TestItem[iCol, iRow].Value = Test_Part.ReMark;

        //        Dg_TestItem.FirstDisplayedScrollingRowIndex = iRow;
                _blRet = true;
            }
            catch (Exception e3)
            {
                //csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Sub_ID|焊缝ID|0|1|90", HardFileName); iNo++;
                //csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "Part_No|焊缝编号|0|1|90", HardFileName); iNo++;
                //csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "DetectionSite|外壁/内壁|0|1|90", HardFileName); iNo++;
                //csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "flThicknise|厚度|0|1|90", HardFileName); iNo++;
                //csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "ProbeSpacing|探头间距|0|1|90", HardFileName); iNo++;
                //csInter.INIWriteValue(strSection, "P" + iNo.ToString(), "ReMark|备注|0|1|190", HardFileName); iNo++;

            }//MessageBox.Show("Grd_AddData：" + e3.Message + e3.StackTrace); }
            return _blRet;
        }
        /// <summary>
        /// 报警列表
        /// </summary>
        /// <param name="Dg_TestItem"></param>
        /// <param name="iR">0-N</param>
        public static bool Grd_AddData(DataGridView Dg_TestItem, int iR, Class_Test_AlarmArea  Test_Alarm)
        {
            bool _blRet = false;
            try
            {
                #region 如果当前行大于列表行，添加新行
                int iRow = iR + 1;//1-N
                int iCol = 0;
                    
                #endregion
                if (iRow + 1 > Dg_TestItem.RowCount) return _blRet;
                if (iCol + 1 > Dg_TestItem.ColumnCount)
                    return _blRet;
                if (iCol < 0 || iRow < 1) return _blRet;
                try
                {
                    for (int i = 0; i < Dg_TestItem.ColumnCount; i++)
                        Dg_TestItem[i, iRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                catch { }
                Dg_TestItem[iCol++, iRow].Value = iRow.ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.ID;
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.Sub_ID;
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.Part_No;
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.strType;
         
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flLen.ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flLen_S.ToString ();
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flLen_E.ToString();
         
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.iX_No .ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.iY_No .ToString();
           
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flHeight.ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flHeight_S.ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flHeight_E.ToString();
         
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flDepth.ToString();
                Dg_TestItem[iCol++, iRow].Value = Test_Alarm.flDepth_S.ToString();

                Dg_TestItem.FirstDisplayedScrollingRowIndex = iRow;
                _blRet = true;
            }
            catch (Exception e3)
            {

            }//MessageBox.Show("Grd_AddData：" + e3.Message + e3.StackTrace); }
            return _blRet;
        }


        #endregion  列表函数
        /// <summary>
        /// 获得当前数据表名称  0: 焊缝 1:原始记录 2:异常数据
        /// </summary>
        /// <param name="iType">0: 焊缝 1:原始记录 2:异常数据</param>
        /// <returns></returns>
        public static string GetCurrDataTableName(int iType,string ID,string Dwmc)
        {    string strType = "";
            string _DataTableName = "";
            switch (iType)
            {
                case 0://焊缝
                    strType = "Part";
                    break;
                case 1://原始记录
                    strType = "Record";
                    break;
                case 2://异常数据
                    strType = "Alarm";
                    break;
                case 3:
                    strType = "EXCEL";
                    break;
            }

            
            int _iL = Dwmc.Length;
          //  int _iL2 = ItemName.Length;
            _DataTableName =strType + "_" + ID + "_" +  Dwmc.Substring(0, (_iL > 5 ? 5 : _iL));// + "_" +
                                            //       SysInfo.m_Test_Item.ItemName.Substring(0, (_iL2 > 5 ? 5 : _iL2));
            return _DataTableName;
        }
        /// <summary>
        /// 系统参数读写
        /// </summary>
        /// <param name="iType"></param>
        public static void  Init(int iType=0)
        {
            if (iType == 0)
            {
                #region 读 
                m_strProName = csInter.IniReadDefine("Cls_Plant", "m_strProName", "DAUT200自动化TOFD焊缝检测系统", HardFileName);
                m_i_Can_BrushTime = int.Parse(csInter.IniReadDefine("Cls_Plant", "m_i_Can_BrushTime", "20", HardFileName));
                m_flPic_Height = int.Parse(csInter.IniReadDefine("Cls_Plant", "m_flPic_Height", "30", HardFileName));

                m_strIP = csInter.IniReadDefine("Cls_Plant", "m_strIP", "192.168.1.2", HardFileName);
                m_strUser = csInter.IniReadDefine("Cls_Plant", "m_strUser", "admin", HardFileName);
                m_strPwd = csInter.IniReadDefine("Cls_Plant", "m_strPwd", "123", HardFileName);

                IP_XunJi_Server = csInter.IniReadDefine("Cls_Plant", "IP_XunJi_Server", "192.168.1.22", HardFileName);

                m_Tofd_DLL.flWaitTime = float.Parse(csInter.IniReadDefine("TOFD", "flWaitTime", "10", HardFileName));

                m_Tofd_DLL.m_pSparam = new SEmatChanParam[Tofd.CHAN_OF_CLIENT];
                m_Tofd_DLL.m_pSparam_Real = new Emat_Real[Tofd.CHAN_OF_CLIENT];
                
               m_Tofd_DLL .  m_strSection = "m_Tofd_DLL";
                #region 工艺文件
                string _strT=  csInter.IniReadDefine("Tofd_Craft", "CurrCraft_Name", "", HardFileName);
                int _iT=int.Parse ( csInter.IniReadDefine("Tofd_Craft", "Num", "0", HardFileName));
                if (_strT != "" && _iT > 0)
                    m_Tofd_DLL.m_strSection = _strT;
                #endregion
                
                for (int i = 0; i < Tofd.CHAN_OF_CLIENT; i++)
                {
                    m_Tofd_DLL.m_pSparam[i].m_fEnStep = SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X;

                    m_Tofd_DLL.m_pSparam[i].m_fEnRatio = new float[2];
                    m_Tofd_DLL.m_pSparam_Real[i].m_iEnPul = new int[2];
                    m_Tofd_DLL.m_pSparam_Real[i].m_fEnReal = new float[2];

                    m_Tofd_DLL.m_pSparam[i].m_idB = int.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_idB", "300", HardFileName));// 300;
                    m_Tofd_DLL.m_pSparam[i].m_iRange = int.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iRange", "200", HardFileName));//200;
                    m_Tofd_DLL.m_pSparam[i].m_iZeroTime = int.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iZeroTime", "0", HardFileName));// 0;
                    m_Tofd_DLL.m_pSparam[i].m_iParallelTime = int.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iParallelTime", "0", HardFileName));//0;

                    m_Tofd_DLL.m_pSparam[i].m_iPulWidthCode = ushort.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iPulWidthCode", "80", HardFileName));//80;
                    m_Tofd_DLL.m_pSparam[i].m_iDemodulation_Flag = byte.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iDemodulation_Flag", "2", HardFileName));// 3;
                    m_Tofd_DLL.m_pSparam[i].m_iRepeatFreq = byte.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iRepeatFreq", "2", HardFileName));//2;
                    m_Tofd_DLL.m_pSparam[i].m_iWorkMode = byte.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iWorkMode", "0", HardFileName));//0;

                    m_Tofd_DLL.m_pSparam[i].m_iSecBandWidthF = byte.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iSecBandWidthF", "0", HardFileName));// 0;
                    m_Tofd_DLL.m_pSparam[i].m_iImpedanceF = byte.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iImpedanceF", "1", HardFileName));//1;
                    m_Tofd_DLL.m_pSparam[i].m_iVolt = byte.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iVolt", "0", HardFileName));//0;
                    m_Tofd_DLL.m_pSparam[i].m_dSpeed = int.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_dSpeed", "5900", HardFileName));//3240;

                    m_Tofd_DLL.m_iMinRange[i] = int.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iMinRange", "10", HardFileName));//10;
                    m_Tofd_DLL.m_iMAxRange[i] = int.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iMAxRange", "1000", HardFileName));//1000;

                    m_Tofd_DLL.m_pSparam[i].m_iForword = byte.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iForword", "0", HardFileName));//0;

                    //下面2021-10-10增加
                  //  if (i == 0)
                    {
                        m_Tofd_DLL.m_pSparam[i].m_iPcsType = byte.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iPcsType", "0", HardFileName));
                        m_Tofd_DLL.m_pSparam[i].m_iPcsMode = byte.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iPcsMode", "0", HardFileName));
                        m_Tofd_DLL.m_pSparam[i].m_BScanMode = byte.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_BScanMode", "0", HardFileName));
                        m_Tofd_DLL.m_pSparam[i].m_iCurEn = byte.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iCurEn", "0", HardFileName));
                        m_Tofd_DLL.m_pSparam[i].m_iEnPos = 1;// byte.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_iEnPos", "0", HardFileName));

                        m_Tofd_DLL.m_pSparam[i].m_fPcsLen = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_fPcsLen", "0", HardFileName));
                        m_Tofd_DLL.m_pSparam[i].m_fPcsArc = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_fPcsArc", "0", HardFileName));
                        m_Tofd_DLL.m_pSparam[i].m_fPcsChord = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_fPcsChord", "0", HardFileName));

                        m_Tofd_DLL.m_pSparam[i].m_fPcsDia = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_fPcsDia", "300", HardFileName));
                        m_Tofd_DLL.m_pSparam[i].m_fPcsAngle = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_fPcsAngle", "60", HardFileName));
                        m_Tofd_DLL.m_pSparam[i].m_fPcsStart = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_fPcsStart", "0", HardFileName));
                        m_Tofd_DLL.m_pSparam[i].m_fPcsEnd = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_fPcsEnd", "20", HardFileName));

                    //    m_Tofd_DLL.m_pSparam[i].m_fEnStep = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_fEnStep", "0", HardFileName));
                        m_Tofd_DLL.m_pSparam[i].m_fEnRatio[0] = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_fEnRatio_0", "0.04", HardFileName));
                        m_Tofd_DLL.m_pSparam[i].m_fEnRatio[1] = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "m_fEnRatio_1", "0.04", HardFileName));

                        m_Tofd_DLL.m_pSparam_Real[i]. T0 = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "T0", "0", HardFileName));
                        m_Tofd_DLL.m_pSparam_Real[i].L0 = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "L0", "0", HardFileName));
                        m_Tofd_DLL.m_pSparam_Real[i].L0_Distan = float.Parse(csInter.IniReadDefine(m_Tofd_DLL.m_strSection, "L0_Distan", "0", HardFileName));
                    }
                }
                #endregion 读
            }
            else
            {
                #region 写
                
                csInter.INIWriteValue("Cls_Plant", "m_i_Can_BrushTime", m_i_Can_BrushTime.ToString(), HardFileName);
                csInter.INIWriteValue("Cls_Plant", "m_flPic_Height", m_flPic_Height.ToString(), HardFileName);
                csInter.INIWriteValue("Cls_Plant", "m_strIP", m_strIP, HardFileName);
                csInter.INIWriteValue("Cls_Plant", "m_strPwd", m_strPwd, HardFileName);

                csInter.INIWriteValue("Cls_Plant", "IP_XunJi_Server", IP_XunJi_Server, HardFileName);

                csInter.INIWriteValue("TOFD", "flWaitTime", m_Tofd_DLL.flWaitTime.ToString(), HardFileName);

                for (int i = 0; i < Tofd.CHAN_OF_CLIENT; i++)
                {m_Tofd_DLL.m_pSparam[0].m_fEnStep = SysInfo.m_Tofd_DLL.Scree_iDotWithmm_X;
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_idB", m_Tofd_DLL.m_pSparam[i].m_idB.ToString(), HardFileName);// 300;
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iRange", m_Tofd_DLL.m_pSparam[i].m_iRange.ToString(), HardFileName);//200;
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iZeroTime", m_Tofd_DLL.m_pSparam[i].m_iZeroTime.ToString(), HardFileName);// 0;
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iParallelTime", m_Tofd_DLL.m_pSparam[i].m_iParallelTime.ToString(), HardFileName);//0;

                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iPulWidthCode", m_Tofd_DLL.m_pSparam[i].m_iPulWidthCode.ToString(), HardFileName);//80;
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iDemodulation_Flag", m_Tofd_DLL.m_pSparam[i].m_iDemodulation_Flag.ToString(), HardFileName);// 3;
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iRepeatFreq", m_Tofd_DLL.m_pSparam[i].m_iRepeatFreq.ToString(), HardFileName);//2;
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iWorkMode", m_Tofd_DLL.m_pSparam[i].m_iWorkMode.ToString(), HardFileName);//0;

                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iSecBandWidthF", m_Tofd_DLL.m_pSparam[i].m_iSecBandWidthF.ToString(), HardFileName);// 0;
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iImpedanceF", m_Tofd_DLL.m_pSparam[i].m_iImpedanceF.ToString(), HardFileName);//1;
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iVolt", m_Tofd_DLL.m_pSparam[i].m_iVolt.ToString(), HardFileName);//0;
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_dSpeed", m_Tofd_DLL.m_pSparam[i].m_dSpeed.ToString(), HardFileName);//3240;

                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iMinRange", m_Tofd_DLL.m_iMinRange[i].ToString(), HardFileName);//10;
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iMAxRange", m_Tofd_DLL.m_iMAxRange[i].ToString(), HardFileName);//1000;

                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "m_iForword", m_Tofd_DLL.m_pSparam[i].m_iForword.ToString(), HardFileName);//0;
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "T0", m_Tofd_DLL.m_pSparam_Real[i].T0.ToString(), HardFileName);
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "L0", m_Tofd_DLL.m_pSparam_Real[i].L0.ToString(), HardFileName);
                    csInter.INIWriteValue(m_Tofd_DLL.m_strSection, "L0", m_Tofd_DLL.m_pSparam_Real[i].L0_Distan .ToString(), HardFileName);

                }
                #endregion 写
            }
        }
        /// <summary>
        /// 参数计算改变参数保存
        /// </summary>
        /// <param name="iType"></param>
        public static void IniIt_Calcu()
        {
            int _iCurrChan = SysInfo.m_Tofd_DLL.m_icurChan;

            #region  写数据
            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_iPcsType", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsType.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_iPcsMode", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iPcsMode.ToString(), SysInfo.HardFileName);

            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_BScanMode", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_BScanMode.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_iCurEn", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn.ToString(), SysInfo.HardFileName);

            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsLen", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsLen.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsArc", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsArc.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsChord", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsChord.ToString(), SysInfo.HardFileName);

            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsDia", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsDia.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsAngle", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsAngle.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsStart", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsStart.ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fPcsEnd", SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fPcsEnd.ToString(), SysInfo.HardFileName);

            if (SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_iCurEn == 0)
                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fEnRatio_0",
                    SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fEnRatio[0].ToString(), SysInfo.HardFileName);
            else
                SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_fEnRatio_1",
                    SysInfo.m_Tofd_DLL.m_pSparam[_iCurrChan].m_fEnRatio[1].ToString(), SysInfo.HardFileName);
            #endregion
        }
        /// <summary>
        /// 校准参数保存
        /// </summary>
        public static void Init_Jz()
        {
            SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iZeroTime =(int)( SysInfo.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_Tofd_DLL.m_icurChan].T0*100f);
            Tofd.SendCmdZeroTime(SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iZeroTime);

            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "T0",
                  SysInfo.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_Tofd_DLL.m_icurChan].T0.ToString(), SysInfo.HardFileName);

            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "L0",
                        SysInfo.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_Tofd_DLL.m_icurChan].L0.ToString(), SysInfo.HardFileName);

            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "L0_Distan",
                        SysInfo.m_Tofd_DLL.m_pSparam_Real[SysInfo.m_Tofd_DLL.m_icurChan].L0_Distan .ToString(), SysInfo.HardFileName);
            SysInfo.csInter.INIWriteValue(SysInfo.m_Tofd_DLL.m_strSection, "m_dSpeed",
                        SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_dSpeed.ToString(), SysInfo.HardFileName);
        }
        /// <summary>
        /// 写配置
        /// </summary>
        /// <param name="strKey">主键</param>
        /// <returns></returns>
        public string Read_One(string strKey)
        {
            return csInter.IniReadDefine("Cls_Plant", strKey, "", HardFileName);
        }
        /// <summary>
        /// 写配置
        /// </summary>
        /// <param name="strKey">主键</param>
        /// <param name="strVal">对应值</param>
        public void Write_One(string strKey, string strVal)
        {
            csInter.INIWriteValue("Cls_Plant", strKey, strVal, HardFileName);
        }
        /// <summary>
        /// 以毫秒为单位延时
        /// </summary>
        /// <param name="dbWait">延时时间：ms</param>
        public static  void WaitTime(float dbWait)
        {
            DateTime dtStar = DateTime.Now;
            while (true)
            {
                Application.DoEvents();
                if (DateTime.Now.Subtract(dtStar).TotalMilliseconds > dbWait) break;
                System.Threading.Thread.Sleep(1);
            }
        }
    }
    /// <summary>
    /// TOFD控制类
    /// </summary>
    public class Tofd
    {
        #region TOFD的Dll调用
        /// <summary>
        /// 联机是否成功
        /// </summary>
        public bool blNetLink = false;
        /// <summary>
        /// 0:正常运行  1：校准
        /// </summary>
        public int m_i_State = 0;

        /// <summary>
        /// 当时数据是否有异常
        /// </summary>
        public bool blAlarm=false ;
        /// <summary>
        /// 报文数据 波形类型 | 波形数据,','间隔512个数据
        /// </summary>
        public string strWave = "";
        /// <summary>
        /// 报文数据 波形类型 | 波形数据,','间隔512个数据
        /// </summary>
        public string strWave_2 = "";
        /// <summary>
        /// 采样时间 波峰序列每个值对应的时间序列，每个值乘以10ns为时间的采样时间
        /// </summary>
        public string strWave_Time = "";
  
        /// <summary>
        /// 数据处理循环间隔时间，单位ms
        /// </summary>
        public float flWaitTime = 10f;
        /// <summary>
        /// 通道数量
        /// </summary>
        public const int HSD_CLIENT = 1;
        /// <summary>
        /// 探头连接 true:成功 false:失败
        /// </summary>
        public bool[] bl_ArrNetTrue = new bool[HSD_CLIENT];
        /// <summary>
        /// 每块通道数
        /// </summary>
        public const int CHAN_OF_CLIENT = 1;
        /// <summary>
        /// 工艺文件夹名称
        /// </summary>
        public string m_strSection = "";
        /// <summary>
        /// 通讯参数
        /// </summary>
        public SEmatChanParam[] m_pSparam;
        /// <summary>
        /// 实际读数
        /// </summary>
        public Emat_Real[] m_pSparam_Real;
        /// <summary>
        /// 全数据最大显示范围
        /// </summary>
        public int[] m_iMinRange = new int[CHAN_OF_CLIENT];
        /// <summary>
        /// 全数据最大显示范围
        /// </summary>
        public int[] m_iMAxRange = new int[CHAN_OF_CLIENT];
        /// <summary>
        /// 联机状态
        /// </summary>
        public bool[] m_connectStatus = new bool[HSD_CLIENT];
        /// <summary>
        /// 显示宽度，总采样点数 512
        /// </summary>
        public const int UTS_DATA_WIDTH = 512;
        /// <summary>
        /// 显示高度
        /// </summary>
        public const int UTS_DATA_HEIGHT = 255;
        /// <summary>
        /// 波形图后面的格子的总数
        /// </summary>
        public const int CHAN_BACK_LINE = 10;
        /// <summary>
        /// 
        /// </summary>
        public const int TEXTLENGTH = 40;
        /*
         TOFD检测模块动态库接口文档
        整体调用流程：
        1.网络连接
        InitModule->CreateHostSocket->GetConnectStatus
        2.通道数据下发(N个通道就循环下发N次，该模块只有一个通道)
        SendCmdCurrentChan->SendCmdDB->SendCmdFreqRatio->
        SendCmdZeroTime->SendCmdParallel->SendCmdPulWid->
        SendCmdWaveType->SendCmdRepeatFreq->SendCmdWorkMode->
        SendCmdBandWidth ->SendCmdImpdance->SendCmdHighVoltage->
        SendCmdForword
        3.	全局数据下发 InitEncoder
        4.	设置当前通道，设置波形数据指针，开启采样
        SendCmdCurrentChan(curChan)-> RealWave->SendCmdSampleStart
        5.关闭程序ExitModule

        关于初始化参数问题，InitParam()初始化了链接库的所有参数，可以在调用网络相关函数之前调用；
        InitSendParam()包含所有初始参数的发码，连接成功后可以调用。硬件初始化就全部处理了，此时可
        以只处理自己想要改变的硬件发码。后文对各个函数详细说明。
         */

        #region  
        [DllImport("NetModulDll.dll", EntryPoint = "InitModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool InitModule(System.UInt32 ChannelSum, System.UInt32 BoardSum);
        /*
         //初始化模块    1.	BOOL		InitModule(ULONG ChannelSum, ULONG BoardSum);
        ChannelSum：通道数，此模块通道数为1
        BoardSum：客户端数，此模块客户端数为1
        说明：
        ①判断ChannelSum和BoardSum是否都不为0，否则返回false;判断ChannelSum和BoardSum是否超过最大通道数和最大客户端数，超过则使用最大值;
        ②初始化每个模块连接状态和客户端套接字;
        ③初始化线程句柄，创建接收数据的线程;
         */
        #endregion

        #region  2 创建网络连接
        [DllImport("NetModulDll.dll", EntryPoint = "CreateHostSocket", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CreateHostSocket();
        /*
       //本机IP:192.168.1.240    PORT 8765   创建网络连接  2.	BOOL 		CreateHostSocket(); 
        说明：
        ①创建连接套接字，设置服务器地址信息，端口号为8765，绑定服务器地址和端口，
        失败返回false，对服务器的socket进行监听，失败返回false，全部成功则返回true。
         */
        #endregion 2

        #region  3 监听线程中调用    3.	BOOL  	GetConnectStatus(USHORT* pConnectNo); 
        [DllImport("NetModulDll.dll", EntryPoint = "GetConnectStatus", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetConnectStatus(System.UInt16[] pConnectNo);
        /*
         pConnectNo = 最大模块数+1；
        说明：传入模块数量，监听服务器套接字socket，有新的连接连入保存为新的客户端套接字，获取客户端的IP地址，如果包含在客户机地址列表(见文档末尾)中则保存当前客户端套接字及连接状态，客户端连接数++，pConnectNo指针指向的值为当前客户端的index,判断客户端连接数是否等于客户端数，如果相等则代表所有指定客户端都已连接，停止监听线程;
        上层判断pConnectNo >= 0 && pConnectNo < 客户端数,则关闭等待连接窗口进入主界面;
        网络连接成功后，接下来就是初始化每个通道数据并下发给仪器获取初始化波形数据。
         */
        #endregion 3

        #region  4 说明：退出程序时调用，关闭动态库中相关接口，返回true
        [DllImport("NetModulDll.dll", EntryPoint = "ExitModule", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ExitModule();
        #endregion 4

        #region  5 说明：初始化全局参数，可在调用网络连接相关函数之前调用
        [DllImport("NetModulDll.dll", EntryPoint = "InitParam", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitParam();
        #endregion 5

        #region  6 说明：初始化硬件发码，可在网络连接成功后调用
        [DllImport("NetModulDll.dll", EntryPoint = "InitSendParam", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitSendParam();
        #endregion 6

        #region 7 开启采样
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdSampleStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdSampleStart();
        #endregion 7

        #region  8  关闭采样
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdSampleClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdSampleClose();
        #endregion 8

        #region  9 关闭服务器
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdServerClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdServerClose();
        #endregion 9

        #region  10 当前通道发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdCurrentChan", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdCurrentChan(int iData);
        /*
        iChan：range(0)，显示为iChan+1;
        说明：切换当前要显示的通道，此模块只有一个通道，该值为0
        */
        #endregion 10

        #region  11 增益控制发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdDB", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdDB(int iData);
        /*
        iData: range(0,1100) ，界面单位dB，值=iData/10;
        说明：设置当前通道增益，默认300，即30.0dB
        */
        #endregion

        #region   12 分频比发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdFreqRatio", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdFreqRatio(int iRange, int iSpeed);
        /*
        iRange: 范围，range(10,1000)，单位mm ;
        iSpeed:声速，range(2500,3500)，单位m/s;
        说明：设置当前通道分频比，是范围和声速对应的发码，
        默认200mm、3240m/s
        */
        #endregion 12

        #region  13 零偏发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdZeroTime", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdZeroTime(int iData);
        /*
        iData: 延时, range(0,3000)，界面单位us，值=iData/100;
        说明：设置当前通道零偏，默认0
        */
        #endregion 13

        #region  14 平移发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdParallel", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdParallel(int iData);
        /*
        iData: 延时, range(0,3000)，界面单位us，值=iData/100;
        说明：设置当前通道平移，默认0
        */
        #endregion

        #region  15 脉冲宽度发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdPulWid", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdPulWid(int iData);
        /*
        iData: range(1,101)  界面显示值=(iData-1)*5，单位ns;
        说明：设置当前通道脉冲宽度，默认80
        */
        #endregion 15

        #region   16 检波方式发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdWaveType", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdWaveType(int iData);
        /*
         iData: range(0,3)  0-3分别表示正检波，负检波，射频波，全检波
        说明：设置当前通道检波方式，默认3
        */
        #endregion 16

        #region  17 重复频率发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdRepeatFreq", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdRepeatFreq(int iData);
        /*
        iData：range(0,7)  0-8分别代表15Hz，30Hz，60Hz，100Hz，200Hz，300Hz，400Hz，500Hz，1kHz
        说明:设置全局重复频率，默认2
        */
        #endregion 17


        #region  18 工作模式发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdWorkMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdWorkMode(int iData);
        /*
         iData：range(0,1)，0代表自发自收，1代表一发一收
        说明:设置工作模式，默认0
        */
        #endregion 18
        #region 19   带宽发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdBandWidth", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdBandWidth(int iData);
        /*
        iData：range(0,3) ，0-3分别表示2-8M、1-30M、0.5-4M和5-15M
        说明:设置带宽，默认0
        */
        #endregion 19

        #region 20 阻抗发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdImpdance", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdImpdance(int iData);
        /*
        iData：range(0,1)，0代表48欧，1代表500欧
        说明:设置阻抗，默认1
        */
        #endregion 20

        #region 21 高压调节发码
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdHighVoltage", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdHighVoltage(int iData);
        /*
         iData:  range(0,2)  0-2分别代表400V、200V和300V
         说明：设置当前通道电压，默认0
        */
        #endregion 21


        #region 22 前放
        [DllImport("NetModulDll.dll", EntryPoint = "SendCmdForword", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCmdForword(int iData);
        //iData： range(0,1)，0-关，1-开
        #endregion 22


        #region 23 编码器初始化
        [DllImport("NetModulDll.dll", EntryPoint = "InitEncoder", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitEncoder(int iIndex, int iData);
        //iIndex：range(0,1)，0-编码器A，1-编码器B
        //iData:  0
        #endregion 23

        #region 24 RealWave 实时采样数据和最大值对应的序列，通道数为1，采样点数为512
        /// <summary>
        /// 波形数
        /// </summary>
        public string[] m_ArrWave;
        /// <summary>
        /// 波峰序列每个值对应的时间序列，每个值乘以10ns为时间的采样时间
        /// </summary>
        public string[] m_ArrWave_Time;
        /// <summary>
        /// 波峰序列
        /// </summary>
        public static byte [] m_pChannelBuf = new byte[ UTS_DATA_WIDTH];
        /// <summary>
        /// 波谷序列
        /// </summary>
        public static byte[] m_pValueBuf = new byte[ UTS_DATA_WIDTH];
        /// <summary>
        /// 检测数据缓存
        /// </summary>
        public static List<Tofd_Arr> m_lstTofd = new List<Tofd_Arr>();
        /// <summary>
        /// 波峰序列每个值对应的时间序列，每个值乘以10ns为时间的采样时间
        /// </summary>
        public static int[] m_pTimeBuf = new int[ UTS_DATA_WIDTH];
        [DllImport("NetModulDll.dll", EntryPoint = "RealWave", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void RealWave(int clientChan, byte *[] pWaveDataBuf, byte*[] pValueBuf, int*[] pTimeBuf);
        /*void RealWave(int clientChan, int **pWaveDataBuf, int **pValueBuf ,  int **pTimeBuf);	
         pWaveDataBuf：波峰指针，
        例如：unsigned char m_pPeakBuf[通道数][采样点数]
        pValueBuf：波谷指针，
        例如：unsigned char m_pDestBuf[通道数][采样点数]
        pValueBuf：TimeBuf指针，波峰序列每个值对应的时间序列，每个值乘以10ns为时间的采样时间
        例如：int  m_pDestBuf[通道数][采样点数]
        说明：实时采样数据和最大值对应的序列，通道数为1，采样点数为512
         */
        [DllImport("NetModulDll.dll", EntryPoint = "GetRatioData", CallingConvention = CallingConvention.Cdecl)]

        public static extern void GetRatioData(int iChan, byte [] pWaveDataBuf, byte [] pValueBuf, int [] pTimeBuf); //获取采样值 
        public static void DLLTofd_RealWave_(int clientChan, ref byte [,] WaveDataBuf, ref byte[,] ValueBuf, ref int[,] TimeBuf)
        {
            int row = WaveDataBuf.GetUpperBound(0) + 1;
            int col = WaveDataBuf.GetUpperBound(1) + 1;
            unsafe
            {
                byte*[] iArr_1 = new byte*[row];
                byte*[] iArr_2 = new byte*[row];
                int*[] iArr_3 = new int*[row];

                for (int iNo = 0; iNo <= clientChan; iNo++)
                {
                    iArr_1 = new byte*[row];
                    iArr_2 = new byte*[row];
                    iArr_3 = new int*[row];
                    fixed (byte * fp_1 = WaveDataBuf)
                    {
                        for (int i = 0; i < row; i++)
                            iArr_1[i] = fp_1 + i * col;
                    }
                    fixed (byte* fp_2 = ValueBuf)
                    {
                        for (int i = 0; i < row; i++)
                            iArr_2[i] = fp_2 + i * col;
                    }
                    fixed (int* fp_3 = TimeBuf)
                    {
                        for (int i = 0; i < row; i++)
                            iArr_3[i] = fp_3 + i * col;
                    }

                    RealWave(iNo, iArr_1, iArr_2, iArr_3);
                }
            }
            return;
        }
        /// <summary>
        /// 采集数据
        /// </summary>
        /// <param name="clientChan"></param>
        public static void DLLTofd_RealWave_1(int clientChan)
        {
            if (SysInfo.m_Plant.m_bl_Ck_Wave == false)
            {  
                GetRatioData(SysInfo.m_Tofd_DLL.m_icurChan, m_pChannelBuf, m_pValueBuf, m_pTimeBuf);

                if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 2)
                {

                    bool _blOk = true;
                    for (int i = 0; i < 50; i++)
                    { if (m_pChannelBuf[i] > 0) _blOk = false; break; }
                    if (_blOk)
                    {
                        for (int i = 0; i < UTS_DATA_WIDTH; i++)
                        {
                            m_pValueBuf[i] = 127; m_pChannelBuf[i] = 127;
                        }
                        int _iNo = 0, _iNo_L = 0;
                        m_pChannelBuf[_iNo++] = 127; m_pChannelBuf[_iNo++] = 132; m_pChannelBuf[_iNo++] = 125;
                        m_pChannelBuf[_iNo++] = 128; m_pChannelBuf[_iNo++] = 127; m_pChannelBuf[_iNo++] = 105;
                        m_pChannelBuf[_iNo++] = 0; m_pChannelBuf[_iNo++] = 0; m_pChannelBuf[_iNo++] = 132;
                        m_pChannelBuf[_iNo++] = 255; m_pChannelBuf[_iNo++] = 255; m_pChannelBuf[_iNo++] = 255;
                        m_pChannelBuf[_iNo++] = 255; m_pChannelBuf[_iNo++] = 217; m_pChannelBuf[_iNo++] = 54;
                        m_pChannelBuf[_iNo++] = 1; m_pChannelBuf[_iNo++] = 0; m_pChannelBuf[_iNo++] = 0;
                        m_pChannelBuf[_iNo++] = 0; m_pChannelBuf[_iNo++] = 17; m_pChannelBuf[_iNo++] = 92;
                        m_pChannelBuf[_iNo++] = 150; m_pChannelBuf[_iNo++] = 180; m_pChannelBuf[_iNo++] = 197;
                        m_pChannelBuf[_iNo++] = 205; m_pChannelBuf[_iNo++] = 206; m_pChannelBuf[_iNo++] = 203;
                        m_pChannelBuf[_iNo++] = 196; m_pChannelBuf[_iNo++] = 189; m_pChannelBuf[_iNo++] = 178;
                        m_pChannelBuf[_iNo++] = 162; m_pChannelBuf[_iNo++] = 152; m_pChannelBuf[_iNo++] = 143;
                        m_pChannelBuf[_iNo++] = 137; m_pChannelBuf[_iNo++] = 130; m_pChannelBuf[_iNo++] = 125;

                        m_pValueBuf[_iNo_L++] = 127; m_pValueBuf[_iNo_L++] = 122; m_pValueBuf[_iNo_L++] = 125;
                        m_pValueBuf[_iNo_L++] = 126; m_pValueBuf[_iNo_L++] = 118; m_pValueBuf[_iNo_L++] = 0;
                        m_pValueBuf[_iNo_L++] = 0; m_pValueBuf[_iNo_L++] = 0; m_pValueBuf[_iNo_L++] = 12;
                        m_pValueBuf[_iNo_L++] = 196; m_pValueBuf[_iNo_L++] = 255; m_pValueBuf[_iNo_L++] = 255;
                        m_pValueBuf[_iNo_L++] = 254; m_pValueBuf[_iNo_L++] = 72; m_pValueBuf[_iNo_L++] = 11;
                        m_pValueBuf[_iNo_L++] = 0; m_pValueBuf[_iNo_L++] = 0; m_pValueBuf[_iNo_L++] = 0;
                        m_pValueBuf[_iNo_L++] = 0; m_pValueBuf[_iNo_L++] = 0; m_pValueBuf[_iNo_L++] = 34;
                        m_pValueBuf[_iNo_L++] = 104; m_pValueBuf[_iNo_L++] = 158; m_pValueBuf[_iNo_L++] = 198;
                        m_pValueBuf[_iNo_L++] = 200; m_pValueBuf[_iNo_L++] = 204; m_pValueBuf[_iNo_L++] = 212;
                        m_pValueBuf[_iNo_L++] = 190; m_pValueBuf[_iNo_L++] = 180; m_pValueBuf[_iNo_L++] = 164;
                        m_pValueBuf[_iNo_L++] = 154; m_pValueBuf[_iNo_L++] = 145; m_pValueBuf[_iNo_L++] = 138;
                        m_pValueBuf[_iNo_L++] = 131; m_pValueBuf[_iNo_L++] = 126; m_pValueBuf[_iNo_L++] = 123;
                    }
                }
            }
        }
        public static void DLLTofd_RealWave(int clientChan,ref  byte[,] WaveDataBuf,ref  byte[,] ValueBuf, ref int[,] TimeBuf)
        {
            int row = WaveDataBuf.GetUpperBound(0) + 1;
            int col = WaveDataBuf.GetUpperBound(1) + 1;
            unsafe
            {
                fixed (byte* fp_1 = WaveDataBuf)
                {
                    fixed (byte* fp_2 = ValueBuf)
                    {
                        fixed (int* fp_3 = TimeBuf)
                        {
                            byte*[] iArr_1 = new byte*[row];
                            byte*[] iArr_2 = new byte*[row];
                            int*[] iArr_3 = new int*[row];

                            for (int i = 0; i < row; i++)
                            {
                                iArr_1[i] = fp_1 + i * col;
                                iArr_2[i] = fp_2 + i * col;
                                iArr_3[i] = fp_3 + i * col;
                                RealWave(i, iArr_1, iArr_2, iArr_3);
                            }
                        }
                    }
                }
            }
            return;
        }

        public static void DLLTofd_RealWave_cs(int clientChan, ref byte[,] WaveDataBuf, ref byte[,] ValueBuf, ref int[,] TimeBuf)
        {
            int row = WaveDataBuf.GetUpperBound(0) + 1;
            int col = WaveDataBuf.GetUpperBound(1) + 1;
            unsafe
            {
                fixed (byte* fp_1 = WaveDataBuf)
                {
                    fixed (byte* fp_2 = ValueBuf)
                    {
                        fixed (int* fp_3 = TimeBuf)
                        {
                            byte*[] iArr_1 = new byte*[row];
                            byte*[] iArr_2 = new byte*[row];
                            int*[] iArr_3 = new int*[row];

                            for (int i = 0; i < row; i++)
                            {
                                iArr_1[i] = fp_1 + i * col;
                                iArr_2[i] = fp_2 + i * col;
                                iArr_3[i] = fp_3 + i * col;
                                RealWave(i, iArr_1, iArr_2, iArr_3);
                            }
                        }
                    }
                }
            }
            return;
        }

        public static void DLLTofd_RealWave_Old(int clientChan, ref byte[,] WaveDataBuf, ref byte[,] ValueBuf, ref int[,] TimeBuf)
        {
            int row = WaveDataBuf.GetUpperBound(0) + 1;
            int col = WaveDataBuf.GetUpperBound(1) + 1;
            unsafe
            {
                  for (int iNo = 0; iNo <= clientChan; iNo++)
                {
                    fixed (byte* fp_1 = WaveDataBuf)
                    {
                        fixed (byte* fp_2 = ValueBuf)
                        {
                            fixed (int* fp_3 = TimeBuf)
                            {
                                byte*[] iArr_1 = new byte*[row];
                                byte*[] iArr_2 = new byte*[row];
                                int*[] iArr_3 = new int*[row];

                                for (int i = 0; i < row; i++)
                                {
                                    iArr_1[i] = fp_1 + i * col;
                                    iArr_2[i] = fp_2 + i * col;
                                    iArr_3[i] = fp_3 + i * col;

                                }
                                RealWave(iNo, iArr_1, iArr_2, iArr_3);
                            }
                        }
                    }
                }
            }
            return;
        }

        #endregion 24 RealWave

        #region 25．unsigned int  getEncoderValue(int Client,int iIndex);  //编码器值
        //Client和iIndex均为0，返回值为当前编码器的脉冲个数。
        //说明：实际编码器值和编码器方向相关
        //如果是反向，编码器值 = 0x800000 – 返回值；
        //如果是正向，编码器值 = 返回值 – 0x800000。

        [DllImport("NetModulDll.dll", EntryPoint = "getEncoderValue", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getEncoderValue(int Client, int iIndex);

        #endregion 25
        #region 26 保存数据
        [DllImport("NetModulDll.dll", EntryPoint = "SaveData", CallingConvention = CallingConvention.Cdecl)]

        public static extern  bool  SaveData(int iChan);    //保存数据
        #endregion 26
        /// <summary>
        /// 联机
        /// </summary>
        /// <returns></returns>
        public bool Link()
        {
            try
            {
                if (m_pSparam == null)
                    m_pSparam = new SEmatChanParam[CHAN_OF_CLIENT];
                if (blNetLink) 
                    Close_TOFD();
                Init_Param();
                blNetLink = initNetWork();
                if (blNetLink)
                {
                    blNetLink = false;
                    if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode == 2) goto CDW;
                    run();
                    Set_Cannl_Param();
                }
            }
            catch (Exception Linke)
            { }
        CDW:
            if (blNetLink == false)
            {
                SysInfo.m_strLinkMsg = SysInfo.m_strLinkMsg.Replace("TOFD 联机失败,", "");
                if (SysInfo.m_strLinkMsg != "")
                    SysInfo.m_strLinkMsg = SysInfo.m_strLinkMsg.Replace(", TOFD 联机失败", "");
                else
                    SysInfo.m_strLinkMsg = SysInfo.m_strLinkMsg.Replace(" TOFD 联机失败", "");
                SysInfo.m_strLinkMsg += (SysInfo.m_strLinkMsg != "" ? "," : "") + " TOFD 联机失败";
            }

            return blNetLink;
        }
        /// <summary>
        /// 测试使用：保存数据
        /// </summary>
        /// <param name="iChan"></param>
        /// <returns></returns>
        public bool Save_Data(int iChan)
        {
           bool _blRet=   SaveData(iChan);
            return _blRet;
        }
        /// <summary>
        /// 1 参数初始化
        /// </summary>
        public void Init_Param()
        {
            InitParam();
        }
        /// <summary>
        /// 车体前进true/后退false
        /// </summary>
        //public bool m_blF_W = false;
        /// <summary>
        /// 获得车体位置：单位是：m
        /// </summary>
        /// <returns></returns>
        public void  GetDistan()
        { 
            float _flDis = 0;
            if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_BScanMode !=1)
            {
                #region 模拟距离
                if (SysInfo.m_iRun == 1)
                {
                    if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iEnPos == 1)
                        _flDis = (float)SysInfo.m_Climb.Trip + Scree_iDotWithmm_X;//m
                    else
                        _flDis = (float)SysInfo.m_Climb.Trip - Scree_iDotWithmm_X;
                    #endregion
                }
            }
            else
            {
                #region 实时采集编码器
                int _iNo = SysInfo.m_Tofd_DLL.m_icurChan;
                int _iA_B = SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iCurEn;
                //1 得到脉冲数
                if (SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iEnPos == 1)
                {
                    SysInfo.m_Tofd_DLL.m_pSparam_Real[_iNo].m_iEnPul[0] = getEncoderValue(0, 0) - 0x800000;
                    SysInfo.m_Tofd_DLL.m_pSparam_Real[_iNo].m_iEnPul[1] = getEncoderValue(0, 1) - 0x800000;
                }
                else
                {
                    SysInfo.m_Tofd_DLL.m_pSparam_Real[_iNo].m_iEnPul[0] = 0x800000 - getEncoderValue(0, 0);
                    SysInfo.m_Tofd_DLL.m_pSparam_Real[_iNo].m_iEnPul[1] = 0x800000 - getEncoderValue(0, 1);
                }
                //2 计算实际距离
                int _iPul =  SysInfo.m_Tofd_DLL.m_pSparam_Real[_iNo].m_iEnPul[_iA_B];

                _flDis = SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fEnRatio[_iA_B] * _iPul;
                _flDis = (int)_flDis;//ms
                SysInfo.m_Tofd_DLL.m_pSparam_Real[_iNo].m_fEnReal[_iA_B] = _flDis;
                _flDis /= 1000;//m
                #endregion
            }
           //单位是：m
            _flDis = float.Parse(_flDis.ToString("f3"));
            if (SysInfo.m_Climb.iBmq_Type == 1)
            {
                _flDis = SysInfo.m_Climb.Trip_Com_mm;_flDis /= 1000;//m
            }
            
            m_flDistanc_X = _flDis;
            SysInfo.m_Climb.Trip = _flDis;
            SysInfo.m_Climb.Trip_mm = _flDis;
        }
        /// <summary>
        /// 联机
        /// </summary>
        public bool initNetWork()
        {
            bool blRet = false;
            m_connectStatus = new bool[HSD_CLIENT];

            blRet = InitModule(CHAN_OF_CLIENT, HSD_CLIENT);
            if (blRet == false)
            {
                MessageBox.Show("Network settings error!");
                return blRet;
            }
            blRet = CreateHostSocket();
            if (blRet == false)
                MessageBox.Show("Local network settings failed!");
            return blRet;
        }
        public void connectStatues(int i)
        {
            if (i >= 0 && i < HSD_CLIENT)
                m_connectStatus[i] = true;
        }
        private bool stopped = false;
        public void run()
        {
            UInt16[] curUtsCardOk = new UInt16[1];
            curUtsCardOk[0]= HSD_CLIENT + 1;
            stopped = false;
            while (!stopped)
            {
                if (GetConnectStatus(curUtsCardOk))
                {
                    stopped = false ;
                }
                if (curUtsCardOk[0] < HSD_CLIENT && curUtsCardOk[0] >= 0)
                {
                    m_pChannelBuf = new byte[ UTS_DATA_WIDTH];
                    m_pValueBuf = new byte[ UTS_DATA_WIDTH];
                    m_pTimeBuf = new int[ UTS_DATA_WIDTH];

                    connectStatues(curUtsCardOk[0]);
                    stopped = true;
                    blNetLink = true;
                }
                System.Threading.Thread.Sleep(10);
            }
        }
        public void Set_Cannl_Param()
        {
            InitSendParam();
            Demo_SendParam();
            setPeakBuffer(m_fWaveFramePerHeight, m_fWaveFramePerWidth);
            SendCmdSampleStart(); //增加开关采样，不再默认开采样
        }
        /// <summary>
        /// 关闭TOFD
        /// </summary>
        public void Close_TOFD()
        {
            blNetLink = false;
            //     if (blNetLink)    
            ExitModule();
        }
        /// <summary>
        /// 当前通道 0-N
        /// </summary>
        public int m_icurChan = 0;
        /// <summary>
        /// 一个数据点高度
        /// </summary>
       public   float m_fWaveFramePerHeight;
        /// <summary>
        /// 一个数据点宽度
        /// </summary>
       public  float m_fWaveFramePerWidth;


        /// <summary>
        /// 图形左边起点
        /// </summary>
        float m_iWaveLeft = 0;
        /// <summary>
        /// 每个点的高度与宽度
        /// </summary>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <param name="iLeft"></param>
        public  void SetH_W(int iWidth,int iHeight,int iLeft)
        {
            m_fWaveFramePerHeight = iHeight / ((float)UTS_DATA_HEIGHT);
            m_fWaveFramePerWidth = iWidth / ((float)UTS_DATA_WIDTH);
            m_iWaveLeft = iLeft;
        }
        /// <summary>
        /// 正/负/全检波数据
        /// </summary>
       public    PointF[] m_points = new PointF[UTS_DATA_WIDTH];
        /// <summary>
        /// 射频波数据
        /// </summary>
        public PointF[] m_points_2 = new PointF[UTS_DATA_WIDTH * 2];


        /// 开口方向
        /// </summary>
        public int m_Pbl_Zf = -1;
        /// <summary>
        /// 波峰位置统计
        /// </summary>
        public Tofd_Peak m_PeakPosi = new Tofd_Peak();
   
        /// <summary>
        /// 波峰位置统计
        /// </summary>
        public struct Tofd_Peak
        {
            /// <summary>
              /// 波峰门限
              /// </summary>
            public int iPeakLimit ;
            /// <summary>
            /// 表面波标准 首波 ：开始位置
            /// </summary>
            public  int i_Stand_Start_L;
            /// <summary>
            /// 表面波标准 首波 ：终止位置
            /// </summary>
            public int i_Stand_End_L;
            /// <summary>
            /// 超出范围
            /// </summary>
            public int i_Ou_of_Range;

            /// <summary>
            /// 表面波标准 底波 ：开始位置
            /// </summary>
            public int i_Stand_Start_R;
            /// <summary>
            /// 表面波标准 底波 ：终止位置
            /// </summary>
            public int i_Stand_End_R;
        }
        public class CliPeak
        {
            /// <summary>
            /// 波形开始
            /// </summary>
            public int iPeak_Start = -1;
            /// <summary>
            ///波形结束
            /// </summary>
            public int iPeak_End = -1;
        }
        int _iLenStart = UTS_DATA_WIDTH / 3;
        int _iLenEnd = 2 * UTS_DATA_WIDTH / 3;
        /// <summary>
        /// 当前数据波形数据
        /// </summary>
        List<CliPeak> m_LstPeakData;
        /// <summary>
        /// 获得TOFD数据
        /// </summary>
        public void setPeakBuffer(float fWaveFramePerHeight,float fWaveFramePerWidth, int iRun=0,bool blCollection =true )
        {
            int iChan = m_icurChan;
            int _iData = 0, _iData_2 = 0;
            blAlarm = false;//当前数据没有异常
            strWave = ""; strWave_2 = ""; strWave_Time = "";

            m_LstPeakData = new List<CliPeak>();
            CliPeak _Peak = new CliPeak();
            for (int i = 0; i < CHAN_OF_CLIENT; ++i)
            {
                DLLTofd_RealWave_1(i);
                if (blCollection )//手动时，实时显示实际位置
                {
                    m_PeakPosi = new Tofd_Peak();
                    m_PeakPosi.iPeakLimit = 127;
                    m_PeakPosi.i_Ou_of_Range = 10;
                    m_PeakPosi.i_Stand_Start_L = -1;
                    m_PeakPosi.i_Stand_End_L = -1;
                    m_PeakPosi.i_Stand_Start_R = -1;
                    m_PeakPosi.i_Stand_End_R = -1;
                }
               
                if (m_pSparam[m_icurChan].m_iDemodulation_Flag != 2)//不是射频波
                {
                    PointF[] points = new PointF[UTS_DATA_WIDTH];

                    for (int j = 0; j < UTS_DATA_WIDTH; ++j)
                    {
                        _iData = m_pChannelBuf[ j];
                        strWave += (j == 0 ? "" : ",") + _iData.ToString();
                        strWave_Time+= (j == 0 ? "" : ",") + m_pTimeBuf[j].ToString();
                        float y = (float)((UTS_DATA_HEIGHT - _iData) * m_fWaveFramePerHeight);
                       // float y = (float)(_iData * fWaveFramePerHeight);
                        float x = (float)(j * fWaveFramePerWidth) + m_iWaveLeft;
                        points[j] = new PointF(x, y);

                        #region 伤点判断
                        if (blCollection)
                            Juge_limit(j,  _iData);
                        if (j > 3 && j < UTS_DATA_WIDTH - 3)
                            Juge_Peak(ref _Peak, j, m_pChannelBuf[ j - 1], _iData, m_pChannelBuf[ j + 1], ref m_LstPeakData);

                        #endregion  伤点判断
                    }
                    m_points = (PointF[])points.Clone();
                }
                else//射频波
                {
                    PointF[] points_2 = new PointF[UTS_DATA_WIDTH * 2];
                    for (int j = 0; j < UTS_DATA_WIDTH; ++j)
                    {
                        _iData = m_pChannelBuf[ j];
                        _iData_2 = m_pValueBuf[ j];
                        strWave += (j == 0 ? "" : ",") + _iData.ToString();
                        strWave_Time += (j == 0 ? "" : ",") + m_pTimeBuf[j].ToString();

                        float x = (float)(j * fWaveFramePerWidth) + m_iWaveLeft;
                        //float y = (float)((UTS_DATA_HEIGHT - _iData) * m_fWaveFramePerHeight);
                        //float y1 = (float)((UTS_DATA_HEIGHT - _iData_2) * m_fWaveFramePerHeight);
                        float y = (float)((_iData) * fWaveFramePerHeight);
                        float y1 = (float)(( _iData_2) * fWaveFramePerHeight);

                        strWave_2 += (j == 0 ? "" : ",") + _iData_2.ToString();
                
                        points_2[j * 2] = new PointF(x, y);
                       points_2[j * 2 + 1] = new PointF(x, y1);

                        #region 伤点判断
                        if (blCollection)
                            Juge_limit(j,  _iData);
                        if (j > 3 && j < UTS_DATA_WIDTH - 3)
                            Juge_Peak(ref _Peak, j, m_pChannelBuf[ j - 1], _iData, m_pChannelBuf[ j + 1], ref m_LstPeakData);
                        #endregion  伤点判断
                    }
                    m_points_2 = (PointF[])points_2.Clone(); 
                    strWave_2 = m_pSparam[m_icurChan].m_iDemodulation_Flag + "|" + strWave_2;
                }
                strWave = m_pSparam[m_icurChan].m_iDemodulation_Flag + "|" + strWave;
               

                #region  波形类型确认
                Juge_m_LstPeakData();
                #endregion

            }
        }
        /// <summary>
        /// 缺陷定性
        /// </summary>
        private void Juge_m_LstPeakData()
        {
            int _iLimit_L_R = 5;//直通波和底波波峰左右偏移误差

            ClAlarm _Alarm = new ClAlarm();
            int _iLastNo = m_LstPeakData.Count - 1;

            try
            {
                for (int i = 0; i < m_LstPeakData.Count; i++)
                {
                    //依据标准波形的直通波和底波的开始结束区间为界限:总共7种类型
                    // 1 直通波没有
                    if (i == 0 && m_LstPeakData[i].iPeak_Start - m_PeakPosi.i_Stand_End_L > _iLimit_L_R &&
                       m_LstPeakData[i].iPeak_End - m_PeakPosi.i_Stand_Start_R > _iLimit_L_R)
                        _Alarm.lstType.Add("1");
                    //2 直通波滞后
                    if (i == 0 && m_LstPeakData[i].iPeak_Start - m_PeakPosi.i_Stand_Start_L > _iLimit_L_R &&
                        m_LstPeakData[i].iPeak_End - m_PeakPosi.i_Stand_End_L > _iLimit_L_R)
                        _Alarm.lstType.Add("2");
                    //3 没有底波  ，只有直通波和底波上端点
                    if (i == _iLastNo && m_LstPeakData[i].iPeak_Start - m_PeakPosi.i_Stand_End_L > _iLimit_L_R &&
                      m_LstPeakData[i].iPeak_End > m_PeakPosi.i_Stand_Start_R)
                        _Alarm.lstType.Add("3");
                    //4 有底波  ，底波滞后
                    if (i == _iLastNo && m_LstPeakData[i].iPeak_Start - m_PeakPosi.i_Stand_Start_R > _iLimit_L_R &&
                      m_LstPeakData[i].iPeak_End - m_PeakPosi.i_Stand_End_R > _iLimit_L_R)
                        _Alarm.lstType.Add("4");
                    //5 、7 有直通波和底波
                    if (i > 0 && i < _iLastNo && m_LstPeakData[i].iPeak_Start - m_PeakPosi.i_Stand_End_L > _iLimit_L_R &&
                    m_LstPeakData[i].iPeak_End > m_PeakPosi.i_Stand_Start_R)
                        _Alarm.lstType.Add("5");
                    //6 直通波有尾巴
                    if (i == 0 && m_LstPeakData[i].iPeak_Start - m_PeakPosi.i_Stand_Start_L < _iLimit_L_R &&
                      m_LstPeakData[i].iPeak_End - m_PeakPosi.i_Stand_End_L > _iLimit_L_R)
                        _Alarm.lstType.Add("6");
                }
                if (_Alarm.lstType.Count > 0)//有异常报警
                {
                    blAlarm = true;//当前数据有异常
                    int _iNum = m_lstAlarm.Count;
                    if (_iNum == 0)
                    {
                        _Alarm.flDistanc_X_Start = m_flDistanc_X;
                        _Alarm.flDistanc_X_End = m_flDistanc_X;
                        m_lstAlarm.Add(_Alarm);
                    }
                    else
                    {
                        if (m_flDistanc_X - m_lstAlarm[_iNum - 1].flDistanc_X_End > Scree_iDotWithmm_X)
                        {
                            //新增异常点
                            _Alarm.flDistanc_X_Start = m_flDistanc_X;
                            _Alarm.flDistanc_X_End = m_flDistanc_X;
                            m_lstAlarm.Add(_Alarm);
                        }
                        else// if(m_lstAlarm[_iNum - 1]. //同一种类型
                            m_lstAlarm[_iNum - 1].flDistanc_X_End = m_flDistanc_X;
                    }
                    string _strType = "";
                    for (int i = 0; i < _iNum; i++)
                        _strType += (i == 0 ? "" : ",") + _Alarm.lstType[i];
                    m_lstAlarm[_iNum - 1].strType = _strType;
                }
            }
            catch (Exception e)
            { }
        }
        /// <summary>
        /// tofd缺陷统计
        /// </summary>
        public List<ClAlarm> m_lstAlarm = new List<ClAlarm>();
        /// <summary>
        /// 图点间隔
        /// </summary>
        public float Scree_iDotWithmm_X = 5;
        /// <summary>
        /// 当前距离
        /// </summary>
        public float m_flDistanc_X = 0;
        /// <summary>
        /// TOFD异常统计
        /// </summary>
        public class ClAlarm
        {
            /// <summary>
            /// 类型
            /// </summary>
            public List<string> lstType =new List<string> ();
            /// <summary>
            /// 类型
            /// </summary>
            public string strType = "";
            /// <summary>
            /// 开始位置
            /// </summary>
            public float flDistanc_X_Start = -1;
            /// <summary>
            /// 开始位置
            /// </summary>
            public float flDistanc_X_End = -1;

        }

        /// <summary>
        /// 寻找直通波与底波界限
        /// </summary>
        /// <param name="j">数据X轴位置</param>
        /// <param name="blCollection">是否采集直通波和底波边界</param>
        /// <param name="_iData">当前峰值</param>
        private void Juge_limit(int j, int _iData)
        {
            #region 伤点判断 在X轴上判断Y数据异常：中心的数值为127

            //1 起点和终端位置判断
            //波峰位置
           
                if (j < _iLenStart)
                {
                    if (m_PeakPosi.i_Stand_Start_L == -1)
                        if (Math.Abs(_iData - m_PeakPosi.iPeakLimit) >= m_PeakPosi.i_Ou_of_Range)
                            m_PeakPosi.i_Stand_Start_L = _iData;
                    if (m_PeakPosi.i_Stand_Start_L != -1)
                        if (Math.Abs(_iData - m_PeakPosi.iPeakLimit) < m_PeakPosi.i_Ou_of_Range)
                            m_PeakPosi.i_Stand_End_L = _iData;
                }
                if (j > _iLenEnd)
                {
                    if ((m_PeakPosi.i_Stand_Start_L != -1 && m_PeakPosi.i_Stand_End_L != -1) &&
                            m_PeakPosi.i_Stand_Start_R == -1)
                    {
                        if (Math.Abs(_iData - m_PeakPosi.iPeakLimit) >= m_PeakPosi.i_Ou_of_Range)
                            m_PeakPosi.i_Stand_Start_R = _iData;
                    }
                    if (m_PeakPosi.i_Stand_Start_R != -1)
                    {
                        if (Math.Abs(_iData - m_PeakPosi.iPeakLimit) < m_PeakPosi.i_Ou_of_Range)
                            m_PeakPosi.i_Stand_End_L = _iData;
                    }
                }
           
          
                    //2 在起点和终端中间有异常
                    //2.1 有上端点
                    //2.2 有下端点
                    //2.3 短小缺陷时，上下端的距离很小
                    //2.4 单个气孔：由于几何尺寸小，没有明显分离的上下端的回波，
                    //形成一个独立的小月牙状，相位无法分辨

              

                //3 直通波被阻断：上/下表面开口裂纹
                //3.1 没有起点/终点
                //3.2 起点和终点之间有下/上端点
                //4 表面开口深度非常小，直通波没有断开，但明显滞后
                //4.1 起点滞后
                //4.2 有下/上端点
           
            #endregion  伤点判断

        }
        /// <summary>
        /// 中轴线
        /// </summary>
        public int m_iPeakLmit = 127;
        /// <summary>
        /// 波峰最低值限度
        /// </summary>
        public int m_iLimit = 5;
        /// <summary>
        /// 三个点判断波峰开始和结束
        /// </summary>
        /// <param name="iDat1"></param>
        /// <param name="iDat2"></param>
        /// <param name="iDat3"></param>
        /// <param name="_LstPeakData"></param>
        private void Juge_Peak(ref CliPeak _iCurPeak,int j, int iDat1,int iDat2,int iDat3, ref List<CliPeak> _LstPeakData)
        {
            if(_iCurPeak.iPeak_Start ==-1)
            {
                if(Math.Abs(iDat3 - m_iPeakLmit) > m_iLimit &&
                  ( iDat1 <= iDat2 && iDat2 <iDat3 || iDat1 >= iDat2 && iDat2 > iDat3))
                {
                    _iCurPeak.iPeak_Start = j;//波峰起点
                }
            }
            else
            {
                if (Math.Abs(iDat1 - m_iPeakLmit) < m_iLimit &&
                   Math.Abs(iDat2 - m_iPeakLmit) < m_iLimit &&
                   Math.Abs(iDat3 - m_iPeakLmit) < m_iLimit)
                {
                    _iCurPeak.iPeak_End  = j;//波峰回落点

                    _LstPeakData.Add(_iCurPeak);
                    _iCurPeak = new CliPeak();
                }
            }
        }
        public void Demo_SendParam()
        {
            for (int i = 0; i < CHAN_OF_CLIENT; i++)
            {
                SendCmdCurrentChan(i);

                SendCmdDB(m_pSparam[i].m_idB);
                SendCmdFreqRatio(m_pSparam[i].m_iRange, m_pSparam[i].m_dSpeed);
                SendCmdZeroTime(m_pSparam[i].m_iZeroTime);
                SendCmdParallel(m_pSparam[i].m_iParallelTime);

                SendCmdForword(m_pSparam[i].m_iForword);

                SendCmdPulWid(m_pSparam[i].m_iPulWidthCode);
                SendCmdWaveType((int)m_pSparam[i].m_iDemodulation_Flag);
                SendCmdRepeatFreq((int)m_pSparam[i].m_iRepeatFreq);
                SendCmdWorkMode((int)m_pSparam[i].m_iWorkMode);

                SendCmdBandWidth((int)m_pSparam[i].m_iSecBandWidthF);
                SendCmdImpdance((int)m_pSparam[i].m_iImpedanceF);
                SendCmdHighVoltage((int)m_pSparam[i].m_iVolt);
            }

            SendCmdCurrentChan(0);
            InitEncoder(0, 0);
            InitEncoder(1, 0);
        }
        /// <summary>
        /// 说明：设置当前通道分频比，是范围和声速对应的发码
        /// </summary>
        public void SendCmdFreqRatio()
        {
            SendCmdFreqRatio(m_pSparam[m_icurChan ].m_iRange, m_pSparam[m_icurChan].m_dSpeed);
        }
        /// <summary>
        /// 说明：设置当前通道平移，默认0
        /// </summary>
        public void SendCmdParallel()
        {
            SendCmdParallel(SysInfo.m_Tofd_DLL.m_pSparam[SysInfo.m_Tofd_DLL.m_icurChan].m_iParallelTime);
        }
        public void Init_Encoder()
        {
            InitEncoder(m_pSparam[m_icurChan ].m_iCurEn, 0);
        }
        #endregion 方法
    }

    public class Tofd_Arr
    {
        /// <summary>
        /// 显示宽度，总采样点数 512
        /// </summary>
        public const int UTS_DATA_WIDTH = 512;
        /// <summary>
        /// 波峰序列
        /// </summary>
        public  byte[] ChannelBuf = new byte[UTS_DATA_WIDTH];
        /// <summary>
        /// 波谷序列
        /// </summary>
        public  byte[] ValueBuf = new byte[UTS_DATA_WIDTH];
    }
    public class CL_BiaoZ_S
    {
        /// <summary>
        /// 数据类型0：长度 1：高度 2：深度
        /// </summary>
        public int iDataType = 0;
        /// <summary>
        /// 0:开始  1：结束
        /// </summary>
        public int i_S0_E1 = 0;
    }
    /// <summary>
    /// 标注数据类型
    /// </summary>
    public class ClBiaoZhu
    {
        /// <summary>
        /// 标注数据类型，0：长度 1：高度  2：深度
        /// </summary>
        public int iSelectAddType = -1;
        /// <summary>
        /// 屏幕序号
        /// 此数据由当前屏幕实时计算，
        /// 保存后再现时也按照实时计算方法
        /// </summary>
        public int iScreenNo = 0;
        /// <summary>
        /// D图X轴位置序号
        /// </summary>
        public int iX_No = 0;
        /// <summary>
        /// D图Y轴位置序号
        /// </summary>
        public int iY_No = 0;

        /// <summary>
        /// 长度开始距离
        /// </summary>
        public float flLen_S = 0f;
        /// <summary>
        /// 长度结束距离
        /// </summary>
        public float flLen_E = 0f;
        /// <summary>
        /// 长度
        /// </summary>
        public float flLen = 0;

        /// <summary>
        /// 高度开始时间
        /// </summary>
        public float flHeight_S = 0f;
        /// <summary>
        /// 高度结束时间
        /// </summary>
        public float flHeight_E = 0f;
        /// <summary>
        /// 高度
        /// </summary>
        public float flHeight = 0;

        /// <summary>
        /// 深度开始时间
        /// </summary>
        public float flDepth_S = 0f;
        /// <summary>
        /// 深度
        /// </summary>
        public float flDepth = 0;
    }
    /// <summary>
    /// 画图类
    /// </summary>
    public class Cls_Plant
    {
        /// <summary>
        /// 画图类初始化
        /// </summary>
        public Cls_Plant()
        {
            Init();
        }
        #region 变量
        /// <summary>
        /// 是否查看历史数据对应的波形 true:查看历史数据  false:实时数据
        /// </summary>
        public bool m_bl_Ck_Wave = false;
        /// <summary>
        /// 当前屏幕数据是历史查询得到的，true:依据屏幕序号从记录中拿，false:否则依据距离从数据库拿
        /// </summary>
        public bool m_bl_His_Data = false;
        /// <summary>
        /// 默认显示指针，离开后不显示
        /// </summary>
        public bool m_bl_Hid_Sz = false;
        /// <summary>
        /// A扫描图备份
        /// </summary>
      //  public Bitmap m_image;
        /// <summary>
        /// 标准颜色
        /// </summary>
        public List<ColorRange> m_StandcolorRange;
        /// <summary>
        /// 单位制式：0：公制单位mm  1：英制单位in
        /// </summary>
        public int iRad_Dw = 0;
        /// <summary>
        /// 文件读写
        /// </summary>
        ClassInterFace csInter = new ClassInterFace();
        string HardFileName = Application.StartupPath + "\\database\\SysConfig.ini";


        /// <summary>
        /// 图形坐标：横轴开始像素位置 15
        /// </summary>
        public int Chart_Ruler_X_Start = 15;
        /// <summary>
        /// C扫图像左上角距离图像顶部高度 18
        /// </summary>
        public int Chart_Ruler_Y_Start = 18;
        /// <summary>
        /// 一个点宽度 5
        /// </summary>
        public int Scree_iDotWith_X = 5;
        /// <summary>
        /// 一个点高度 5
        /// </summary>
        public int Scree_iDotHeight = 5;
        ///// <summary>
        ///// A扫描图一个间隔的像素宽度
        ///// </summary>
        //public int m_iKd_W_512Num = 0;
        /// <summary>
        /// A图像X位置
        /// </summary>
        public int m_i_X_No = 0;
        /// <summary>
        /// 图像X位置
        /// </summary>
        public int m_i_X_A = 0;
        /// <summary>
        /// 图像Y位置
        /// </summary>
        public int m_i_Y_A = 0;

        /// <summary>
        /// 图像X位置
        /// </summary>
        public int m_i_X_D = 0;
        /// <summary>
        /// 图像Y位置
        /// </summary>
        public int m_i_Y_D = 0;
        /// <summary>
        /// 横轴间隔个数
        /// </summary>
        public int m_iJgNum = 10;
        /// <summary>
        /// D扫描行数，这个由TOFD数据个数决定
        /// </summary>
        public int Scree_iAllRows = 0;
        /// <summary>
        /// 屏幕刻度（以起点为原点的左右刻度是相同的，屏幕序号以各自区间独立判断）
        /// </summary>
        public List<Class_Screen_Kd> lst_Screenkd = new List<Class_Screen_Kd>();
        /// <summary>
        /// 一个点代表距离  默认10毫米  单位m
        /// </summary>
        public float Scree_iDotWithmm_X = 10;
        /// <summary>
        /// 英寸最小单位
        /// </summary>
        public float m_fl_In_MinLimit = 1 / 32f;
        /// <summary>
        /// <summary>
        /// 屏幕总列数
        /// </summary>
        public int Scree_iAllCols = 0;
        /// <summary>
        /// 横轴纵轴刻度字下降量
        /// </summary>
        public int Chart_Ruler_Word_H = 2;
        /// <summary>
        /// 最远距离，画图默认1000米
        /// </summary>
        public float flMaxDistance = 10000;

        /// <summary>
        /// 当前距离对应的屏幕序号
        /// </summary>
        public int g_iCurrDistanc_Calcu_ScreenNo = -1;
        /// <summary>
        /// 屏幕序号统计
        /// </summary>
        public int g_iCurrDistanc_Calcu_All_ScreenNo = -1;
        /// <summary>
        /// 当前屏幕使用的序号
        /// </summary>
        public int g_iCurrUse_ScreenNo = 0;



        /*
        /// <summary>
        /// A扫或者C扫纵轴的一个像素点时间长度
        /// </summary>
        public float fl_Onel_Height_Time = 0;
        /// <summary>
        /// A扫图像左起位置
        /// </summary>
        public int i_A_Screen_Left = 0;
        /// <summary>
        /// A扫图像右下角距离图像底部高度
        /// </summary>
        public int i_A_Screen_Bottom = 20;
        #region A扫参数
        /// <summary>
        /// 总数据个数
        /// </summary>
        public int i_Arr_Num = 0;
        /// <summary>
        /// 探头宽度
        /// </summary>
        public float fl_A_PCS = 0;
        /// <summary>
        /// A扫一个像素点的像素宽度
        /// </summary>
        public int i_A_Onepixel_With = 0;
        /// <summary>
        /// A扫一个像素点的像素高度
        /// </summary>
        public int i_A_Onepixel_Height = 0;
        /// <summary>
        /// 图像实际高度
        /// </summary>
        public int i_A_Height = 0;
        #endregion  A扫

        #region C扫界面

        /// <summary>
        /// 一屏标准长度
        /// </summary>
        public float Scree_Stant_Distance = 0;
        /// <summary>
        /// 屏幕B图形变量
        /// </summary>
        public System.Windows.Forms.PictureBox PicArea_C = null;
        
    
        /// <summary>
        /// C图一个像素点的像素高度
        /// </summary>
        public int i_C_Onepixel_Height = 0;
        /// <summary>
        /// 一个像素点的像素宽度
        /// </summary>
        public int Scree_C_iDotWith_X = 0;
        /// <summary>
        /// C扫横坐标一个像素点代表距离
        /// </summary>
        public int i_One_With_mm = 0;
        /// <summary>
        /// C扫图像左起位置
        /// </summary>
        public int i_C_Screen_Left = 10;
    
        /// <summary>
        /// X轴坐标起点值
        /// </summary>
        public float Chart_Run_flStart_Distance = 0;
        
        #endregion C扫  
        */
        /// <summary>
        /// C图像变量
        /// </summary>
        public Struct_G m_G_C = new Struct_G();
        /// <summary>
        /// 当前运行位置
        /// </summary>
        public int m_iCurr_Run_Position = 0;
       
        /// <summary>
        /// 屏幕信息对照表
        /// </summary>
        public Class_Screen_Info[] m_Arr_Screen;
        #endregion 变量
        #region 操作配置文件
        /// <summary>
        /// 配置文件读写 0：读 1：写
        /// </summary>
        /// <param name="iType">0：读 1：写</param>
        /// <returns></returns>
        public bool Init(int iType = 0)
        {
            bool _blRet = false;

            try
            {
                if (iType == 0)
                {
                    #region 读

                    Chart_Ruler_X_Start = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_X_Start", "15", HardFileName));
                    Chart_Ruler_Y_Start = int.Parse(csInter.IniReadDefine("Class_Plant", "Chart_Ruler_Y_Start", "18", HardFileName));
                    Scree_iDotWith_X = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotWith_X", "5", HardFileName));
                    Scree_iDotHeight = int.Parse(csInter.IniReadDefine("Class_Plant", "Scree_iDotHeight", "15", HardFileName));
                    try
                    {
                        Scree_iDotWithmm_X = int.Parse(csInter.IniReadDefine("Cls_Plant", "Scree_iDotWithmm_X", "10", HardFileName));
                        if (Scree_iDotWithmm_X == 0)
                        {
                            Scree_iDotWithmm_X = 10;
                            csInter.INIWriteValue("Cls_Plant", "Scree_iDotWithmm_X", "10", HardFileName);
                        }
                    }
                    catch { Scree_iDotWithmm_X = 10; }
                    flMaxDistance = int.Parse(csInter.IniReadDefine("Cls_Plant", "flMaxDistance", "10000", HardFileName));
                    m_fl_In_MinLimit = float.Parse(csInter.IniReadDefine("Class_Plant", "m_fl_In_MinLimit", "0.03125", HardFileName));

                    if (iRad_Dw == 0)
                    {
                        Scree_iDotWithmm_X *= 0.001F;
                        Scree_iDotWithmm_X = float.Parse(Scree_iDotWithmm_X.ToString("f3"));
                    }
                    else
                    {
                        if (Scree_iDotWithmm_X == 0) Scree_iDotWithmm_X = m_fl_In_MinLimit;
                    }
                    /*
                    fl_Onel_Height_Time = float.Parse(csInter.IniReadDefine("Cls_Plant", "fl_Onel_Height_Time", "1", HardFileName));
                    fl_A_PCS = float.Parse(csInter.IniReadDefine("Cls_Plant", "fl_A_PCS", "1", HardFileName));
                    i_A_Onepixel_With = int.Parse(csInter.IniReadDefine("Cls_Plant", "i_A_Onepixel_With", "1", HardFileName));
                    i_A_Screen_Left = int.Parse(csInter.IniReadDefine("Cls_Plant", "i_A_Screen_Left", "0", HardFileName));
                    i_A_Screen_Bottom = int.Parse(csInter.IniReadDefine("Cls_Plant", "i_A_Screen_Bottom", "20", HardFileName));

                    i_C_Onepixel_Height = int.Parse(csInter.IniReadDefine("Cls_Plant", "i_C_Onepixel_Height", "1", HardFileName));
                    Scree_C_iDotWith_X = int.Parse(csInter.IniReadDefine("Cls_Plant", "Scree_C_iDotWith_X", "1", HardFileName));
                    i_One_With_mm = int.Parse(csInter.IniReadDefine("Cls_Plant", "i_One_With_mm", "1", HardFileName));

                    i_C_Screen_Left = int.Parse(csInter.IniReadDefine("Cls_Plant", "i_C_Screen_Left", "10", HardFileName));
                    Chart_Ruler_Y_Start = int.Parse(csInter.IniReadDefine("Cls_Plant", "Chart_Ruler_Y_Start", "20", HardFileName));
                    Chart_Ruler_Word_H = int.Parse(csInter.IniReadDefine("Cls_Plant", "Chart_Ruler_Word_H", "2", HardFileName));

                   */
                    #endregion 读
                }
                else
                {
                    #region 写
                    /*
                    csInter.INIWriteValue("Cls_Plant", "fl_Onel_Height_Time", fl_Onel_Height_Time.ToString(), HardFileName);
                    csInter.INIWriteValue("Cls_Plant", "fl_A_PCS", fl_A_PCS.ToString(), HardFileName);

                    csInter.INIWriteValue("Cls_Plant", "i_A_Onepixel_With", i_A_Onepixel_With.ToString(), HardFileName);

                    csInter.INIWriteValue("Cls_Plant", "i_C_Onepixel_Height", i_C_Onepixel_Height.ToString(), HardFileName);
                    csInter.INIWriteValue("Cls_Plant", "Scree_C_iDotWith_X", Scree_C_iDotWith_X.ToString(), HardFileName);
                    csInter.INIWriteValue("Cls_Plant", "i_One_With_mm", i_One_With_mm.ToString(), HardFileName);
                    */
                    #endregion
                }
                _blRet = true;
            }
            catch { }
            return _blRet;
        }
        /// <summary>
        /// 写配置
        /// </summary>
        /// <param name="strKey">主键</param>
        /// <returns></returns>
        public string Read_One(string strKey)
        {
            return csInter.IniReadDefine("Cls_Plant", strKey, "", HardFileName);
        }
        /// <summary>
        /// 写配置
        /// </summary>
        /// <param name="strKey">主键</param>
        /// <param name="strVal">对应值</param>
        public void Write_One(string strKey, string strVal)
        {
            csInter.INIWriteValue("Cls_Plant", strKey, strVal, HardFileName);
        }
        #endregion 配置文件
        /// <summary>
        /// 图像初坐标
        /// </summary>
        /// <param name="iTotalTime">总时长</param>
        /// <param name="iDataNum">数据个数</param>
        /// <param name="iWith">图像宽度</param>
        /// <param name="iHeight">图像高度</param>
        public void Screen_Init_A(int iTotalTime, int iDataNum, int iWith, int iHeight)
        {
            #region 1 计算纵横刻度
            //i_Arr_Num = iDataNum;
            //fl_Onel_Height_Time = 1.0f * iTotalTime / iDataNum;//一个点代表时间长度

            //i_A_Onepixel_With = (iWith - i_A_Screen_Left) / iDataNum;//一个像素点宽度
            //i_A_Height = iHeight - i_A_Screen_Bottom;
            //i_A_Onepixel_Height = i_A_Height / 255;//一个像素点高度
            #endregion 1
        }
        /// <summary>
        /// 获得灰度图内存数据：黑-白变化
        /// </summary>
        /// <param name="PicArea">画图板</param>
        /// <param name="Rr_Bitmap">图像内存</param>
        /// <param name="iHorizontal_0">0:横向 1：竖向</param>
        /// <returns></returns>
        public int PlanScheme_ColorLimit(System.Windows.Forms.PictureBox PicArea,  ref Bitmap Rr_Bitmap, int iHorizontal_0)
        {
            int _iRet = 0;//画图返回
            try
            {
                float flPic_W = PicArea.Width;//画布宽度
                float flPic_H = PicArea.Height;//画布高度

                #region 1.2 准备画布
                // 初始化画板，在内存中建立一块虚拟画布
                Bitmap image = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                // 获取背景层
                Bitmap bg = (Bitmap)PicArea.BackgroundImage;
                // 初始化整个画布
                Bitmap canvas = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
                // 初始化图形面板，获取这块内存画布的Graphics的引用
                Graphics g = Graphics.FromImage(image);
                Graphics gb = Graphics.FromImage(canvas);
                #endregion  准备画布
              
                g.Clear(Color.White);// g.Clear(Color.Teal);// CadetBlue);

                Rectangle rect = new Rectangle(0, 0, PicArea.ClientSize.Width, PicArea.ClientSize.Height);
                rect.Location = new Point(0, 0);
                LinearGradientBrush b3 = new LinearGradientBrush(rect, Color.Black , Color.White ,
                                             iHorizontal_0 == 0? LinearGradientMode.Horizontal: LinearGradientMode.Vertical);//WhiteSmoke
                g.FillRectangle(b3, rect);

                #region  4 刷新
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (bg != null)
                    gb.DrawImage(bg, _Rect);// 先绘制背景层
                gb.DrawImage(image, _Rect); // 再绘制绘画层

                PicArea.BackgroundImage = (Bitmap)canvas.Clone();// (Bitmap)canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                Rr_Bitmap = (Bitmap)canvas.Clone();

                g.Dispose(); g = null;
                image.Dispose();
                gb.Dispose();
                canvas.Dispose();

                PicArea.Refresh();
                #endregion 刷新

                canvas.Dispose();
                image.Dispose();
            }
            catch { }

            return _iRet;
        }
  
        /// <summary>
        /// 保存标准颜色
        /// </summary>
        /// <param name="Rr_Bitmap"></param>
        /// <param name="iWithJgNum"></param>
        public void SaveStandColor(Bitmap Rr_Bitmap, int iWithJgNum)
        {
            float _flJG = Rr_Bitmap.Width / iWithJgNum;
           // int _iHeght = Rr_Bitmap.Height / 2;

            m_StandcolorRange = new List<ColorRange>();

            for (int i = 0; i < iWithJgNum; i++)
            {
                ColorRange _Col = new ColorRange();
                _Col.Col   = Rr_Bitmap.GetPixel((int)(i * (_flJG )), 0);
                _Col.strColor  = _Col.Col.R.ToString() + "," + _Col.Col.G.ToString() + "," + _Col.Col.B.ToString();

                m_StandcolorRange.Add(_Col);
            }
        }
        public struct ColorRange
        {
            /// <summary>
            /// R,G,B
            /// </summary>
            public string strColor;
            /// <summary>
            /// 最大厚度误差 方案中存储方式：从小到大排列
            /// </summary>
            public float Max_Limit;
            /// <summary>
            /// 颜色
            /// </summary>
            public Color Col;
          
        }
        /// <summary>
        /// 画A扫波形
        /// </summary>
        /// <param name="PicArea">画布</param>
        /// <param name="ArrData_A">A扫数据</param>
        public void Plant_A(System.Windows.Forms.PictureBox PicArea, Tofd Tofd_Data)
        {
            try
            {
                #region 1 初始化
                #region 1.1 画布
                float flPic_W = PicArea.Width;//画布宽度  965
                float flPic_H = PicArea.Height;//画布高度   364
                bool _blSp = Tofd_Data.m_pSparam[Tofd_Data.m_icurChan].m_iDemodulation_Flag == 2;//射频信号

                int iKd_W_Num = int.Parse((flPic_W / m_iJgNum).ToString("f0"));//横轴10个数据一个刻度
                int iKd_H_Num = int.Parse((flPic_H / m_iJgNum).ToString("f0"));//纵轴10个数据一个刻度
                int i_X = 0, i_Y = 0;//刻度

                // 初始化画板，在内存中建立一块虚拟画布
                Bitmap image = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);

                // 获取背景层
                Bitmap bg = (Bitmap)PicArea.BackgroundImage;
                // 初始化整个画布
                Bitmap canvas = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
                // 初始化图形面板，获取这块内存画布的Graphics的引用
                Graphics g = Graphics.FromImage(image);
                Graphics gb = Graphics.FromImage(canvas);
                g.Clear(Color.Black);//

                g.SmoothingMode = SmoothingMode.AntiAlias;
                #endregion 1.1

                #region 1.2 刻度画笔
                //1 坐标笔
                PointF Zb_X = new PointF(0, 10);
                Pen p_Zb = new Pen(Brushes.White);//坐标笔
                Font Font_Zb = new Font("黑体", 18);
                SolidBrush Color_Zb = new SolidBrush(Color.White);
                //2 中心线条
                Color _ColorL = Color.FromArgb(255, 0, 98, 0);
                Pen p_xy = new Pen(_ColorL);// Brushes.LimeGreen CadetBlue);
                p_xy.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                p_xy.Width = 1;
                // g.DrawLine(p_xy, new Point(0, i_A_Height / 2), new Point((int)flPic_W, i_A_Height / 2));
                #endregion 1.2
                #endregion 1

                #region 2 画横、纵轴分别10格的间隔线
                for (int i = 1; i < m_iJgNum; i++)
                {
                    i_X = i * iKd_W_Num;//竖线
                    g.DrawLine(p_xy, new Point(i_X, 0), new Point(i_X, (int)flPic_H));
                    i_Y = i * iKd_H_Num;//横线
                    g.DrawLine(p_xy, new Point(0, i_Y), new Point((int)flPic_W, i_Y));
                }
                #endregion 2

                #region 3 画波形
                Pen mysum = new Pen(Color.GreenYellow, 1);
                g.DrawLines(mysum, _blSp ? Tofd_Data.m_points_2 : Tofd_Data.m_points);
                #endregion 3
                #region 3.2 提示信息
                Font drawFont_Thick = new Font("黑体", 14);
                SolidBrush drawBrush_TiTl = new SolidBrush(Color.White);
                float flTiTl_x = 7.5f * iKd_W_Num;
                PointF drawPoint_W = new PointF(flTiTl_x, 2);
                int _iN0 = SysInfo.m_Tofd_DLL.m_icurChan;
                g.DrawString("距离: " + SysInfo.m_Tofd_DLL.m_flDistanc_X +"m", drawFont_Thick, drawBrush_TiTl, drawPoint_W);
               
                drawPoint_W = new PointF(flTiTl_x, 20);
                g.DrawString("S: " + SysInfo.m_Tofd_DLL.m_pSparam[_iN0].m_dSpeed.ToString("f0") + "m/s"
                    , drawFont_Thick, drawBrush_TiTl, drawPoint_W);
                drawPoint_W = new PointF(flTiTl_x, 36);
                g.DrawString("2T0: " + SysInfo.m_Tofd_DLL.m_pSparam_Real[_iN0].T0.ToString("f3") + "us"
                    , drawFont_Thick, drawBrush_TiTl, drawPoint_W);

                drawPoint_W = new PointF(flTiTl_x, 52);
                #region 计算高度
                Pen g_Sz = new Pen(Brushes.Red  );
                g_Sz.Width = 4;
         //      if(m_bl_Hid_Sz==false ) 
                    g.DrawLine(g_Sz, new Point(SysInfo.m_Plant.m_i_X_A-5, SysInfo.m_Plant.m_i_Y_A),
                                                      new Point(SysInfo.m_Plant.m_i_X_A + 5, SysInfo.m_Plant.m_i_Y_A));
                g_Sz.Width = 1;
                 g.DrawLine(g_Sz, new Point(SysInfo.m_Plant.m_i_X_A, 0),
                                                      new Point(SysInfo.m_Plant.m_i_X_A,(int ) flPic_H));
                float _Time = Tofd.m_pTimeBuf[SysInfo.m_Plant.m_i_X_No] * 0.01f;
                float _f1 = SysInfo.m_Tofd_DLL.m_pSparam[_iN0].m_dSpeed / 1000f * 0.5f *
                    (_Time - SysInfo.m_Tofd_DLL.m_pSparam_Real[_iN0].T0);
                _f1 *= _f1;
                float _f2 = SysInfo.m_Tofd_DLL.m_pSparam[_iN0].m_fPcsLen;
                _f2 *= _f2;
                double _d = Math.Sqrt((double)(_f1 - _f2));
                #endregion 计算高度

                g.DrawString("X:" + SysInfo.m_Plant.m_i_X_No.ToString("f0") + " Time:" + _Time.ToString ("f1") + "\r\nH:" + _d.ToString ("f1")
                    , drawFont_Thick, drawBrush_TiTl, drawPoint_W);
                drawPoint_W = new PointF(flTiTl_x, 98);
                g.DrawString("P/V/T: " +Tofd .m_pChannelBuf[SysInfo.m_Plant.m_i_X_No]+"/"+ 
                                           Tofd.m_pValueBuf[SysInfo.m_Plant.m_i_X_No] + "/"+ 
                                           Tofd.m_pTimeBuf[SysInfo.m_Plant.m_i_X_No]
                   , drawFont_Thick, drawBrush_TiTl, drawPoint_W);

                #endregion
                #region  4 刷新
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (bg != null)
                    gb.DrawImage(bg, _Rect);// 先绘制背景层
                gb.DrawImage(image, _Rect); // 再绘制绘画层

                PicArea.BackgroundImage = (Bitmap)canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布

        //        m_image = (Bitmap)image.Clone();
                g.Dispose(); g = null;
                image.Dispose();
                gb.Dispose();
                canvas.Dispose();

                PicArea.Refresh();

                p_Zb.Dispose();
          //      canvas.Dispose();
                mysum.Dispose();
         //       image.Dispose();
                #endregion 刷新
            }
            catch (Exception ee)
            { }
        }
        /// <summary>
        /// 画TOFD的D灰度图
        /// </summary>
        /// <param name="PicArea">画板</param>
        /// <param name="iScreenNo">屏幕序号</param>
        /// <param name="Tofd_Data">数据</param>
        /// <param name="iType">0：实时数据 1：历史数据</param>
        public void Plant_D(PictureBox PicArea,int iScreenNo, Tofd Tofd_Data,int iType=0,bool blBrush=true )
        {
            //1 获得距离对应图像X轴位置
            float _flDat = ((Tofd_Data.m_flDistanc_X  - lst_Screenkd[iScreenNo ].Chart_Run_flStart_Distance) / Scree_iDotWithmm_X);//Chart_Run_flStart_Distance
            int i_X = (int)_flDat;//由实际点计算屏幕开始序号
            if (_flDat - i_X >= 0.5) i_X++;

            i_X = (int)(Chart_Ruler_X_Start + i_X * Scree_iDotWith_X);
           
            //2 由帧数据对应D图
            int i_Y2=0, i_Y=0,iDat=0;
            Color _CurColor = Color .White ;
            m_iCurr_Run_Position = i_X;
            for (int j = 0; j < Tofd.UTS_DATA_WIDTH; ++j)
            {
                //2.1 帧数据
                if (iType == 0)
                    iDat = Tofd.m_pChannelBuf[j];
                else
                    iDat = int.Parse(Tofd_Data.m_ArrWave[j]);
                iDat = Tofd.UTS_DATA_HEIGHT - iDat;

                //数据值与颜色转换
                if (iDat > -1 && iDat < Tofd.UTS_DATA_HEIGHT)
                    _CurColor = Color.FromArgb(255, iDat, iDat, iDat);// m_StandcolorRange[iDat].Col  ;
                else
                    _CurColor = Color.White;

                //2.2 Y轴位置
                i_Y = Chart_Ruler_Y_Start + j * Scree_iDotHeight;
                
                //2.3 画图
                InitColor(m_G_C.g, i_X, i_Y, Scree_iDotWith_X, Scree_iDotHeight, _CurColor);// System.Drawing.Color.FromArgb((byte)iDat, (byte)iDat, (byte)iDat));

                #region 删除
                //2.4 射频时不清楚如何画:武汉中科的周工说，D图不用m_pValueBuf数据,它只用画波形
                //if (Tofd_Data.m_pSparam[Tofd_Data.m_icurChan].m_iDemodulation_Flag == 2)
                //{  i_Y2 = i_Y;
                //    iDat = Tofd.m_pValueBuf[ j];
                //    //数据值与颜色转换
                //    iDat = iDat;
                //    InitColor(m_G_C.g, i_X, i_Y2, Scree_iDotWith_X, Scree_iDotHeight, System.Drawing.Color.FromArgb((byte)iDat, (byte)iDat, (byte)iDat));
                //}
                #endregion 
            }
            if (blBrush)
            {
                Rectangle _Rect = new Rectangle(0, 0, PicArea.Width, PicArea.Height);

                if (m_G_C.bg != null)
                    m_G_C.gb.DrawImage(m_G_C.bg, _Rect);// 先绘制背景层

                m_G_C.gb.DrawImage(m_G_C.image, _Rect); // 再绘制绘画层
                PicArea.BackgroundImage = (Bitmap)m_G_C.canvas.Clone();
            }
        }

            /// <summary>
            /// 计算屏幕行列个数
            /// </summary>
            /// <param name="PicArea"></param>
            public void GetRulerPara_C(System.Windows.Forms.PictureBox PicArea,int iRowNum)
        {
            if (PicArea == null) return;
          
  
            int iScreen_With = PicArea.Width - Chart_Ruler_X_Start;
            int iScreen_Height = PicArea.Height - Chart_Ruler_Y_Start;

            Scree_iAllCols = iScreen_With / Scree_iDotWith_X;
         
            #region Y轴光栅臂对应图像点宽度以及数据行数
            Scree_iAllRows = iRowNum;
            Scree_iDotHeight=int.Parse ( (iScreen_Height / iRowNum).ToString ());
            if (Scree_iDotHeight == 0) Scree_iDotHeight = 1;
            #endregion 行数
                //   Scree_Stant_Distance = Scree_iAllCols * Scree_iDotWithmm_X;
        }
        /// <summary>
        /// 画刻度 ：初始化、右边出图、满屏时操作
        /// </summary>
        public void Plant_Ruler_C(System.Windows.Forms.PictureBox PicArea, int iScreenNo)
        {
            if (PicArea == null) return;
            int i_X = 0, i_Y = 0;

            string strT = "";
            //1 画图工具清零
            Chart_Clear_C();
            System.Windows.Forms.PictureBox.CheckForIllegalCrossThreadCalls = false;

            #region 画刻度尺
            Pen Ruler_Pen = new Pen(Brushes.Black);//黑刻度
            Font drawFont = new Font("黑体", (float)7.9, FontStyle.Bold);
            PointF drawPoint;
         //   SolidBrush RulerStr_Brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 0, 0, 0));
            SolidBrush RulerStr_Brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 21, 139, 207));

            float _flStart_X = lst_Screenkd[iScreenNo].Chart_Run_flStart_Distance;//X轴起点
            #region 1 画布准备:初始化、右边出图、满屏时
            m_G_C.image = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
            // 获取背景层
            m_G_C.bg = (Bitmap)PicArea.BackgroundImage;
            // 初始化整个画布
            m_G_C.canvas = new Bitmap(PicArea.ClientSize.Width, PicArea.ClientSize.Height);
            // 初始化图形面板，获取这块内存画布的Graphics的引用
            m_G_C.g = Graphics.FromImage(m_G_C.image);
            m_G_C.gb = Graphics.FromImage(m_G_C.canvas);
            m_G_C.g.Clear(Color.White);
            m_G_C.Buff = new PointF[0];

            int iTop = PicArea.Top;

            int i_S_H = 0, i_S_L = 0;
            float _flEndKd = 0f;

            int _iGs = iRad_Dw == 0 ? 10 : 16;
            #region 1.2 画横轴刻度
            i_S_H = Chart_Ruler_Y_Start - 6; i_S_L = Chart_Ruler_Y_Start - 3;
            for (int i = 1; i <= Scree_iAllCols; i++)
            {
                //1 计算刻度位置和刻度值
                i_X = (int)(Chart_Ruler_X_Start + i * Scree_iDotWith_X);//刻度位置
                _flEndKd = _flStart_X + (i * Scree_iDotWithmm_X);//刻度值
                //2 画刻度线
                if (iRad_Dw == 0 && i % 5 != 0 || iRad_Dw == 1 && i % 4 != 0)
                    m_G_C.g.DrawLine(Ruler_Pen, new Point(i_X, i_S_L), new Point(i_X, Chart_Ruler_Y_Start));
                else
                    m_G_C.g.DrawLine(Ruler_Pen, new Point(i_X, i_S_H), new Point(i_X, Chart_Ruler_Y_Start));
                //3 画刻度值
                if (i % _iGs == 0)
                {
                    #region 标记横轴刻度值
                    if (iRad_Dw == 0)
                    {
                            _flEndKd *= 1000;
                            strT = _flEndKd.ToString("f0") + (i == _iGs ? "mm" : "");
                    }
                    else
                        strT = _flEndKd.ToString("f4") + (i == _iGs ? "in." : "");
                    
                    drawPoint = new PointF(i_X - 15, Chart_Ruler_Word_H);
                    m_G_C.g.DrawString(strT, drawFont, RulerStr_Brush, drawPoint);
                    #endregion 标记横轴刻度值
                }

                if (i_X > PicArea.ClientSize.Width) break;
            }
            m_G_C.g.DrawLine(Ruler_Pen, Chart_Ruler_X_Start, Chart_Ruler_Y_Start, (int)(Chart_Ruler_X_Start + Scree_iAllCols * Scree_iDotWith_X), Chart_Ruler_Y_Start);
            Font drawFont_t = new Font("Arial", (float)8);
            Font drawFont_Y = new Font("黑体", (float)7.9, FontStyle.Bold);//"Arial
            m_G_C.g.DrawString("Dist.", drawFont_Y, RulerStr_Brush, -2, 1);
            m_G_C.g.DrawLine(Ruler_Pen, new Point(Chart_Ruler_X_Start, Chart_Ruler_Y_Start), new Point(Chart_Ruler_X_Start, PicArea.Height ));

            #endregion 1.2 画横轴刻度

            Rectangle _Rect_Kd = new Rectangle(0, 0, PicArea.Width, PicArea.Height);
            if (m_G_C.bg != null)
            {
                try
                {
                    m_G_C.gb.DrawImage(m_G_C.bg, _Rect_Kd);// 先绘制背景层
                }
                catch (Exception de)
                { }
            }
            m_G_C.gb.DrawImage(m_G_C.image, _Rect_Kd); // 再绘制绘画层
            try
            {
                PicArea.BackgroundImage = (Bitmap)m_G_C.canvas.Clone(); // 设置为背景层，就是在内存中建立了一张对应的画布，重新refresh就是再次的加载这个画布
                PicArea.Refresh();
            }
            catch { }
            #endregion 1 画布准备
            #endregion 刻度尺

            Application.DoEvents();
        }

        /// <summary>
        /// 屏幕起止刻度初始化:图像间隔改变时调用此函数,距离范围 0--N
        /// </summary>
        public void Init_ScreenKd(int PicArea_Width, int PicArea_Height)
        {
            //距离以最远距离为准
            if (lst_Screenkd.Count > 0) lst_Screenkd.Clear();
            int _iNo = 0;
            string strDw = iRad_Dw == 0 ? "f3" : "f5";
            //2 计算图像X轴Y轴间隔
            int iScreen_With = PicArea_Width - Chart_Ruler_X_Start;
            int iScreen_Height = PicArea_Height - Chart_Ruler_Y_Start;
            if (Scree_iDotWithmm_X == 0) Scree_iDotWithmm_X = 0.01f;
            //  Scree_iAllCols = iScreen_With / Scree_C_iDotWith_X;
            float    Scree_Stant_Distance = Scree_iAllCols * Scree_iDotWithmm_X;

            Scree_Stant_Distance = Scree_iAllCols * Scree_iDotWithmm_X;
            //3 计算屏幕刻度
            while (true)
            {
                //1 拿数据
                Class_Screen_Kd _kd = new Class_Screen_Kd();

                if (_iNo == 0)
                {
                    _kd.Chart_Run_flStart_Distance = float.Parse((_iNo * (Scree_Stant_Distance + Scree_iDotWithmm_X)).ToString(strDw));
                    _kd.Chart_Run_flEnd_Distance = float.Parse((_iNo * (Scree_Stant_Distance + Scree_iDotWithmm_X) + Scree_Stant_Distance).ToString(strDw));
                }
                else
                {
                    _kd.Chart_Run_flStart_Distance = lst_Screenkd[_iNo - 1].Chart_Run_flEnd_Distance;
                    _kd.Chart_Run_flEnd_Distance = float.Parse((_iNo * (Scree_Stant_Distance) + Scree_Stant_Distance).ToString(strDw));
                }
                //2 添加当前数据
                lst_Screenkd.Add(_kd);
                if (_kd.Chart_Run_flEnd_Distance > flMaxDistance) return;
                //3 下一个数据
                _iNo++;
            }

        }

        /// <summary>
        /// 依据距离获得对应屏幕序号
        /// </summary>
        /// <param name="flDistance"></param>
        /// <returns></returns>
        public int Get_No(float flDistance)
        {
            int iNo = 0;
            for (int i = 0; i < lst_Screenkd.Count; i++)
            {
                if (flDistance >= lst_Screenkd[i].Chart_Run_flStart_Distance && (flDistance < lst_Screenkd[i].Chart_Run_flEnd_Distance))//|| flDistance <= m_flArr_Rul_S[i+1]))
                {
                    iNo = i;
                 //   Chart_Run_flStart_Distance = i;
                    break;
                }
            }
            return iNo;
        }
        /// <summary>
        /// 图形清零
        /// </summary>
        private void Chart_Clear_C()
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
                if (G != null)
                    G.FillRectangle(mybrush, intStar_X, intStar_Y, iWith, iHeight);
            }
            catch (Exception e)
            { }
        }
    }
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
    /// 当前屏幕鼠标位置的信息
    /// 距离
    /// 报文
    /// 是否有伤
    /// 伤最上距离
    /// 伤最下距离
    /// </summary>
    public class Class_Screen_Info
    {
        /// <summary>
        /// 当前距离
        /// </summary>
        public float fl_CurrentDistance = 0;
        /// <summary>
        /// 通讯报文
        /// </summary>
        public int[] btData;
        /// <summary>
        /// 是否有伤点
        /// </summary>
        public bool blMarking = false;

        /// <summary>
        /// 缺陷深度
        /// </summary>
        public int i_Defectdepth = 0;
        /// <summary>
        /// 缺陷长度
        /// </summary>
        public int i_DefectLengt = 0;
    }
    public class TcpClient_UI
    {

        //刷新视频
        // PictureBox m_Pho_Video;
        #region tcp通讯属性

        private bool m_blOut = false;
        /// <summary>
        /// 客户端监听线程
        /// </summary>
        private Thread tcpClientThread = null;

        /// <summary>
        /// 连接服务器的客户端
        /// </summary>
        public TcpClient tcpClient = null;
        /// <summary>
        /// 连接服务器
        /// </summary>
        public bool m_blLinkServe = false;
        /// <summary>
        /// 是否成功接收数据0:失败 1：成功
        /// </summary>
        public int m_iRecevo = 0;
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string m_strServIp = "";
        /// <summary>
        /// 服务器端口号
        /// </summary>
        public string m_strPort = "";
        #endregion

        #region 数据帧格式
        /// <summary>
        /// 接收客户端数据帧头<clie>
        /// </summary>
        public string C_Head = "<clie>";
        /// <summary>
        /// 接收客户端数据帧尾</clie>
        /// </summary>
        public string C_Tail = "</clie>";
        /// <summary>
        /// 服务器发送帧头<main>
        /// </summary>
        public string S_Head = "<main>";
        /// <summary>
        /// 服务器发送帧尾</main>
        /// </summary>
        public string S_Tail = "</main>";
        #endregion
        public TcpClient_UI()//PictureBox _Pic)
        {
            // m_Pho_Video = _Pic;
            tcpClient = null;
        }
        //获取端口
        public string GetLocalEndPoint()
        {

            //this.tcpClient.Client.LocalEndPoint.ToString() = "48000";

            return tcpClient.Client.LocalEndPoint.ToString();
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="strServIp">服务器IP</param>
        /// <param name="strPort">服务器端口</param>
        /// <returns></returns>
        public bool ConnectToServer(string strServIp, string strPort)
        {
            //   csInterface csinfo = new csInterface();
            m_blLinkServe = false;

            if (strServIp == "") strServIp = "127.0.0.1";
            if (strPort == "") strPort = "48100";
            try
            {
                m_strServIp = strServIp;
                m_strPort = strPort;

                if (tcpClient != null) tcpClient.Close();
                tcpClient = new TcpClient(strServIp, int.Parse(strPort));

                //延时操作
                if (tcpClient != null)
                {
                    if (tcpClientThread != null)
                    {
                        if (tcpClientThread.IsAlive)
                        {
                            //关闭线程
                            tcpClientThread.Abort();
                        }
                    }
                    tcpClientThread = new Thread(new ParameterizedThreadStart(ReceiveData));
                    int ID = 0;
                    tcpClientThread.Start((object)ID);
                    tcpClientThread.IsBackground = true;
                }
            }
            catch (Exception ex)
            {
                // throw;
                //  MessageBox.Show("集中控制器没有连接上！"+ex.Message+"请重新连接服务器，确保IP端口正确");
            }
            return m_blLinkServe;
        }


        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="Obj"></param>
        private void ReceiveData(object Obj)
        {
            int iTcp = (int)Obj;
            int num = 0, iCout = 0, iType = -1, i_StartVideo = 1 + 38 + 26;
            string[] _sPara = "".Split(',');
            byte[] _dd = new byte[10];
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_dd, 0);
            try
            {
                if (tcpClient.Connected)
                {
                    NetworkStream ns = tcpClient.GetStream();
                    while (tcpClient.Connected)
                    {
                        m_blLinkServe = true;
                        //if (SysInfo_Xj.m_blSend_Txt_Avrg == false)
                        //{
                        //    //SendData_C(4, SysInfo.m_i_Send_Txt_Avrg.ToString() + "," + SysInfo.m_iStand_AddVal.ToString() + "," + SysInfo.m_i_11_Type +
                        //    //    "," + SysInfo.m_Track_Type.ToString() + "," + (SysInfo.m_bl_MaxLow ? "1" : "0") + "," + SysInfo.m_i_Net_OK);

                        //    SysInfo_Xj.m_Client.SendData_C(4, SysInfo_Xj.m_i_Send_Txt_Avrg + "," + SysInfo_Xj.m_str_FuDu + "," +
                        //                   "0" + "," + SysInfo_Xj.m_Track_Type + "," +
                        //                  (SysInfo_Xj.m_bl_MaxLow ? "1" : "0") + "," + "0");

                        //    SysInfo_Xj.m_blSend_Txt_Avrg = true;

                        //    Application.DoEvents();
                        //}
                        //从网络接收的可供读取的数据字节数据
                        num = tcpClient.Available;

                        byte[] Data = new byte[num];
                        if (num > 1)
                        {
                            iCout = ns.Read(Data, 0, num);
                            ns.Flush();
                        }
                        //  Application.DoEvents();
                        iCout = Data.Length - 1;
                        if (iCout > 0)//数据帧合理就处理
                        {
                            try
                            {
                                //1 解压数据   www
                                // GZip.GZIPDecompress(ref Data);//
                                //2 数据类型
                                iType = Data[0];//
                             
                                byte[] _ArrEnd = new byte[3];
                                string _sT = "";
                                byte[] _GetData;

                                switch (iType)
                                {
                                    case 88://苏州博智惠达激光器
                                            //解析高度和对应属性值
                                            //2个字节表示一个数据，低位在前高温在后
                                        _ArrEnd[0] = Data[iCout - 1];
                                        _ArrEnd[1] = Data[iCout - 2];
                                        _ArrEnd[2] = Data[iCout];
                                        if (!(_ArrEnd[0] == 0xFE && _ArrEnd[1] == 0xFE && _ArrEnd[2] == 0xFE)) break;

                                        byte[] _Arr_Hight = new byte[1000];//高度数据
                                        byte[] _Arr_Attrib = new byte[8];//属性数据
                                        IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_Hight, 0);
                                        Marshal.Copy(Data, 1, IntPtArr, 1000);
                                        IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr_Attrib, 0);
                                        Marshal.Copy(Data, 1001, IntPtArr, 8);
                                        #region 解析数据
                                        Class_Xj_GetData _ClData = new Class_Xj_GetData();
                                        //1 解析高度
                                        byte[] _btArr = new byte[2];
                                        int _iNo = 0;
                                        for (int i = 0; i < 1000; i += 2)//1-1000 测高数据
                                        {
                                            _btArr[0] = _Arr_Hight[i];
                                            _btArr[1] = _Arr_Hight[i + 1];
                                            _ClData.dbArrData[_iNo++] = double.Parse(byteToHexStr(_btArr)) / 100;
                                        }
                                        //2 解析属性 1001-1008 余高 中心点  焊缝开始  焊缝结束
                                        //余高 *100
                                        _iNo = 0;
                                        _btArr[0] = _Arr_Attrib[_iNo++];
                                        _btArr[1] = _Arr_Attrib[_iNo++];
                                        _ClData.dbDepth = double.Parse(byteToHexStr(_btArr)) / 100;
                                        //中心点 
                                        _btArr[0] = _Arr_Attrib[_iNo++];
                                        _btArr[1] = _Arr_Attrib[_iNo++];
                                        _ClData.i_Cent = int.Parse(byteToHexStr(_btArr));
                                        //焊缝开始
                                        _btArr[0] = _Arr_Attrib[_iNo++];
                                        _btArr[1] = _Arr_Attrib[_iNo++];
                                        _ClData.i_Start = int.Parse(byteToHexStr(_btArr));
                                        //焊缝结束
                                        _btArr[0] = _Arr_Attrib[_iNo++];
                                        _btArr[1] = _Arr_Attrib[_iNo++];
                                        _ClData.i_End = int.Parse(byteToHexStr(_btArr));

                                        //串口数据 1009-1034
                                        _GetData = new byte[26];//串口数据
                                        IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_GetData, 0);
                                        Marshal.Copy(Data, 1009, IntPtArr, 26);
                                        _sT = byteToHexStr(_GetData);
                                        _ClData.strFrame = _sT;
                                        #endregion
                                        SysInfo.m_lst_Weld.Add(_ClData);
                                        SysInfo.g_Msg_InterFace.Fun_GetServe_Data1();
                                        break;
                                    case 99://视频数据
                                     /*   _ArrEnd[0] = Data[iCout - 1];
                                        _ArrEnd[1] = Data[iCout - 2];
                                        _ArrEnd[2] = Data[iCout];
                                        if (!(_ArrEnd[0] == 0xFE && _ArrEnd[1] == 0xFE && _ArrEnd[2] == 0xFE)) break;

                                        iCout = iCout - i_StartVideo;// Data.Length帧标识 + 38控制数据-26串口报文  + 视频数据
                                        SysInfo_Xj.m_Data_JC = new byte[iCout];//视频数据大小
                                        IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(SysInfo_Xj.m_Data_JC, 0);
                                        Marshal.Copy(Data, i_StartVideo, IntPtArr, iCout);

                                        //1 控制数据
                                         _GetData = new byte[38];
                                        IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_GetData, 0);
                                        Marshal.Copy(Data, 1, IntPtArr, 38);
                                        string _strT = Encoding.Default.GetString(_GetData, 0, _GetData.Length);
                                        SysInfo_Xj.m_i_Xj_Col = int.Parse(_strT.Split(',')[6]);

                                        if (SysInfo_Xj.m_blShowXunJi == false) break;//窗体没加载不显示

                                        SysInfo_Xj.m_lst_Buf_CtrDat.Add(_strT);
                                        //2 串口数据
                                        _GetData = new byte[26];
                                        IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_GetData, 0);
                                        Marshal.Copy(Data, 39, IntPtArr, 26);
                                         _sT = byteToHexStr(_GetData);// Encoding.Default.GetString(_GetData, 0, _GetData.Length);
                                        SysInfo_Xj.m_lst_Buf_COM.Add(_sT);


                                        #region 3 视频显示
                                        if (SysInfo_Xj.m_Data_JC != null)
                                        {
                                            BitmapImage bi = new BitmapImage();
                                            bi.BeginInit();
                                            bi.StreamSource = new System.IO.MemoryStream(SysInfo_Xj.m_Data_JC);
                                            bi.EndInit();
                                            try
                                            {
                                                SysInfo_Xj.m_lst_Image.Add(bi);//  Image.FromStream(stream));
                                            }
                                            catch (Exception e)
                                            {
                                            }
                                        }
                                        #endregion 
                                        SysInfo_Xj.g_Msg_InterFace.Fun_GetServe_Data_2();
                                      */
                                        break;

                                }
                                if (m_blOut) break;
                            }
                            catch (Exception jx)
                            {
                                if (m_blOut) return;
                            }

                        }
                        Thread.Sleep(20);
                    }
                }

            }
            catch (Exception ex)
            {
                //Application.Exit();
                //this.Close();
            }
            Close();
            //this.Close();
            //Application.Exit();
        }
        #region 字节型转十六进制字符串
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        #endregion


        private void WaitTimess(double dbWait)
        {
            DateTime dtStart = DateTime.Now;
            while (true)
            {
                double dbTime = DateTime.Now.Subtract(dtStart).TotalSeconds;
                if (dbTime >= dbWait) break;
                Thread.Sleep(200);
                Application.DoEvents();
            }
        }

        /// <summary>
        /// 服务器锁
        /// </summary>
        public static object m_lockSend = new object();

        public void SendDatToServe(string strKey, string strDat)
        {
            lock (m_lockSend)
            {
                try
                {

                    string _strSendDat = strKey + "/" + strDat;//发送数据:信息类型/数据
                    m_iRecevo = 0;

                    //2 准备发送
                    if (SendDataToServer(GetSendZhen(_strSendDat)) == false)
                        SendDataToServer(GetSendZhen(_strSendDat));
                }
                catch (Exception e)
                {
                }
            }
        }
        /// <summary>
        /// 将发送字符串组帧压缩
        /// </summary>
        /// <param name="strDat">发送字符串</param>
        /// <returns>压缩后数据</returns>
        public byte[] GetSendZhen(string strDat)
        {
            string strSend = C_Head + "[" + strDat + "[" + C_Tail;
            byte[] sDat = Encoding.Default.GetBytes(strSend);
            //   GZip.GZIPCompress(ref sDat);

            return sDat;
        }
        /// <summary>
        /// 发送控制信息
        /// </summary>
        /// <param name="iType">
        /// 数据类型数据范围：1：PC发送：标准色行号，标准色列号
        ///2：PC接收到：检测标准数据（6）： 视频宽度，视频高度， 中心点Y, 中心点X ,斜率，耗时
        ///3：PC发送：退出系统并断电</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public bool SendData_C(int iType, string strMsg)
        {

            bool _blRet = false;
            if (m_blLinkServe == false) return _blRet;

            strMsg = iType.ToString() + strMsg;
            byte[] Data = Encoding.Default.GetBytes(strMsg);
            //2 压缩
            //  GZip.GZIPCompress(ref Data);
            //3 发送
            SendDataToServer(Data);
            return _blRet;
        }
        /// <summary>
        ///给服务器发送数据
        ///btDat 要写入的数据：字节格式
        /// </summary>
        /// <param name="btSendDat">要写入的数据：字节格式</param>
        public bool SendDataToServer(byte[] btSendDat)
        {
            lock (m_lockSend)
            {
                bool blRet = false;
                try
                {
                    if (tcpClient != null)
                    {
                        if (tcpClient.Connected == true)
                        {
                            NetworkStream ns = tcpClient.GetStream();
                            ns.Write(btSendDat, 0, btSendDat.Length);
                            ns.Flush();
                            blRet = true;
                        }
                        else
                        {
                            ConnectToServer(m_strServIp, m_strPort);

                        }
                    }
                    else
                    {
                        ConnectToServer(m_strServIp, m_strPort);

                    }
                    return blRet;
                }
                catch (Exception e)
                {
                    return blRet;
                }
            }
        }


        /// <summary>
        /// 等待台体返回信息
        /// </summary>
        /// <param name="dbWait">等待时间，单位：秒</param>
        /// <param name="strKey">查询信息类型</param>
        public void WaitTime(double dbWait, string strKey)
        {
            DateTime dtStart = DateTime.Now;
            while (true)
            {
                if (tcpClient != null)//确认设备正常发送出去了
                {
                    if (tcpClient.Connected == true)
                    {
                        if (DateTime.Now.Subtract(dtStart).TotalSeconds > dbWait)//dbWait)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                System.Windows.Forms.Application.DoEvents();
            }
        }
        /// <summary>
        /// 关闭线程和监听
        /// </summary>
        public void Close()
        {
            try
            {
                m_blOut = true;
                try
                {
                    //       Application.ExitThread();
                    if (tcpClientThread != null)
                        if (tcpClientThread.IsAlive) tcpClientThread.Abort();//关闭线程 tcpClientThread
                    tcpClientThread = null;
                }
                catch (Exception e1)
                { }
                try
                {
                    if (tcpClient != null) tcpClient.Close();//关闭与服务器连接
                }
                catch (Exception e2)
                { }

            }
            catch (Exception e)
            {
                //   this.Close();
                Application.Exit();
                if (tcpClient != null) tcpClient.Close();//关闭与服务器连接
            }
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
            //    HLSysInfo.Close();
            try
            {
                System.Environment.Exit(0);
            }
            catch { }
            //  this.Close();
            Application.Exit();
        }
    }
    /// <summary>
    /// 接收测高寻迹数据
    /// </summary>
    public class Class_Xj_GetData
    {
        /// <summary>
        /// 测高数据
        /// </summary>
        public double[] dbArrData = new double[500];
        /// <summary>
        /// 余高
        /// </summary>
        public double  dbDepth=0;
        /// <summary>
        /// 中心点
        /// </summary>
        public int i_Cent = 0;
        /// <summary>
        /// 焊缝开始
        /// </summary>
        public int i_Start = 0;
        /// <summary>
        /// 焊缝结束
        /// </summary>
        public int i_End = 0;
        /// <summary>
        /// 串口数据
        /// </summary>
        public string  strFrame = "";
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

        /// <summary>
        /// 16进制字节变字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string HexToStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    if (i == 0)
                        returnStr += int.Parse(bytes[i].ToString("X2")).ToString();
                    else
                        returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        /// <summary>
        /// 创建当前ID文件夹
        /// </summary>
        /// <param name="ID"></param>
        public void  CreatCurrDir(string ID)
        {
            //1 判断路径是否存在
            //判断是否有Temp
            string strPath = System.Windows.Forms.Application.StartupPath + "\\Temp\\";
            if (System.IO.Directory.Exists(strPath) == false)
            {
                System.IO.Directory.CreateDirectory(strPath);
            }
            strPath = System.Windows.Forms.Application.StartupPath + "\\Temp\\"+ ID+"\\";
            if (System.IO.Directory.Exists(strPath) == false)
            {
                System.IO.Directory.CreateDirectory(strPath);
            }
        }
        /// <summary>
        /// 写检测文件
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="iType">记录长度；0：写 1：不写</param>
        /// <returns></returns>
        public int FileSaveByte(string filename,int iType=0)
        {
            FileStream fi = null;
            int _iRet = 0;
            try
            {
                try
                {
                    fi = new FileStream(filename, FileMode.CreateNew);
                }
                catch (Exception)
                {
                    File.Delete(filename);
                    fi = new FileStream(filename, FileMode.CreateNew);
                }
                #region 文件头保存
                //1 项目名称
                // byte[] ls1 = StrToHex("zkcxus02");
                byte[] ls1 = System.Text.Encoding.Default.GetBytes("zkcxus02");
                fi.Write(ls1, 0, ls1.Length);
                //2 版本日期 2021-9-15
                ls1 = BitConverter.GetBytes(2021);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(9);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(15);
                fi.Write(ls1, 0, ls1.Length);
                //3 参数长度Marshal.

                int filelen = Marshal.SizeOf(typeof(SEmatChanParam));//400  sizeof(SEmatChanParam);
                ls1 = BitConverter.GetBytes(filelen);
                fi.Write(ls1, 0, ls1.Length);
                #endregion
                //4 TOFD参数
                #region 写参数
                /*
                 bool -> System.Boolean (布尔型，其值为 true 或者 false)
                byte -> System.Byte (字节型，占 1 字节，表示 8 位正整数，范围 0 ~ 255)
                sbyte -> System.SByte (带符号字节型，占 1 字节，表示 8 位整数，范围 -128 ~ 127)
                char -> System.Char (字符型，占有两个字节，表示 1 个 Unicode 字符)
                short -> System.Int16 (短整型，占 2 字节，表示 16 位整数，范围 -32,768 ~ 32,767)
                ushort -> System.UInt16 (无符号短整型，占 2 字节，表示 16 位正整数，范围 0 ~ 65,535)
                uint -> System.UInt32 (无符号整型，占 4 字节，表示 32 位正整数，范围 0 ~ 4,294,967,295)
                int -> System.Int32 (整型，占 4 字节，表示 32 位整数，范围 -2,147,483,648 到 2,147,483,647)
                float -> System.Single (单精度浮点型，占 4 个字节)
                ulong -> System.UInt64 (无符号长整型，占 8 字节，表示 64 位正整数，范围 0 ~ 大约 10 的 20 次方)
                long -> System.Int64 (长整型，占 8 字节，表示 64 位整数，范围大约 -(10 的 19) 次方 到 10 的 19 次方)
                double -> System.Double (双精度浮点型，占8 个字节)
                 */
                #endregion
                #region TOFD参数  C#将值类型变量转换为字节数组时，只需调用BitConverter.GetBytes()方法即可。
                int _iNo = SysInfo.m_Tofd_DLL.m_icurChan;
                 ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_idB);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iRange);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iZeroTime);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iParallelTime);
                fi.Write(ls1, 0, ls1.Length);

                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iPulWidthCode);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = new Byte[1];
                ls1[0] =(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iDemodulation_Flag);//1
                fi.Write(ls1, 0, ls1.Length);
                ls1[0] = (SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iRepeatFreq);//1
                fi.Write(ls1, 0, ls1.Length);
                ls1[0] = (SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iWorkMode);//1
                fi.Write(ls1, 0, ls1.Length);
              
                ls1[0] = (SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iSecBandWidthF);//1
                fi.Write(ls1, 0, ls1.Length);
                ls1[0] = (SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iImpedanceF);//1
                fi.Write(ls1, 0, ls1.Length);
                ls1[0] = (SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iVolt);//1
                fi.Write(ls1, 0, ls1.Length);
                ls1 = new Byte[4];
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_dSpeed);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = new Byte[1];
                ls1[0] = (SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iForword);//1
                fi.Write(ls1, 0, ls1.Length);

                ls1[0] = (SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iPcsType);//1
                fi.Write(ls1, 0, ls1.Length);
                ls1[0] = (SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iPcsMode);//1
                fi.Write(ls1, 0, ls1.Length);
                ls1[0] = (SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_BScanMode);//1
                fi.Write(ls1, 0, ls1.Length);
                ls1[0] = (SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iCurEn);//1
                fi.Write(ls1, 0, ls1.Length);
                ls1[0] = (SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iEnPos);//1
                fi.Write(ls1, 0, ls1.Length);
                if (SysInfo.m_Tofd_DLL.m_pSparam[_iNo].u8bk == null) SysInfo.m_Tofd_DLL.m_pSparam[_iNo].u8bk = new byte[318];
                fi.Write(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].u8bk, 0, SysInfo.m_Tofd_DLL.m_pSparam[_iNo].u8bk.Length);
                ls1 = new Byte[8];
                SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_BRecordLen = Tofd.m_lstTofd.Count;
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_BRecordLen);
                fi.Write(ls1, 0, ls1.Length);

                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iPcsLW);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iPcsBW);
                fi.Write(ls1, 0, ls1.Length);

                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsLen);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsArc);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsChord);
                fi.Write(ls1, 0, ls1.Length);

                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsDia);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsAngle);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsStart);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsEnd);
                fi.Write(ls1, 0, ls1.Length);

                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fEnStep);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fEnRatio[0]);
                fi.Write(ls1, 0, ls1.Length);
                ls1 = BitConverter.GetBytes(SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fEnRatio[1]);
                fi.Write(ls1, 0, ls1.Length);
                #endregion
                //记录个数
                //if (iType == 0)
                //{
                //    ls1 = BitConverter.GetBytes(Tofd.m_lstTofd.Count);
                //    fi.Write(ls1, 0, ls1.Length);
                //}

                #region 数据值保存
                for (int i = 0; i < Tofd.m_lstTofd.Count; i++)
                {
                    //波峰
                    fi.Write(Tofd.m_lstTofd[i].ChannelBuf, 0, Tofd.m_lstTofd[i].ChannelBuf.Length);
                    //波谷
                    fi.Write(Tofd.m_lstTofd[i].ValueBuf, 0, Tofd.m_lstTofd[i].ValueBuf.Length);
                }
                #endregion


            }
            catch
            {
                _iRet = -1;
            }
            fi.Close();
            fi.Dispose();
            return _iRet;
        }
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="_TofdData"></param>
        /// <param name="iType"></param>
        /// <returns></returns>
        public List<Tofd_Arr> FileReadByte(string filename, ref SEmatChanParam _TofdData,int iType=0)
        {
            List<Tofd_Arr> _LstTofd = new List<Tofd_Arr>();
            FileStream fi = null;
            /*
             string转byte[]:

            byte[] byteArray = System.Text.Encoding.Default.GetBytes ( str );
            byte[]转string：

            string str = System.Text.Encoding.Default.GetString ( byteArray );
            string转ASCII byte[]:

            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes ( str );
            ASCII byte[]转string:

            string str = System.Text.Encoding.ASCII.GetString ( byteArray );
             */
            fi = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            #region 1 读取文件头
           byte[] _Lst = new byte[8];
            //"zkcxus02"
            fi.Read(_Lst, 0, 8);
            string _strT = System.Text.Encoding.Default.GetString(_Lst);
            fi.Read(_Lst, 0, 4);//2021
           int _T = BitConverter.ToInt32(_Lst, 0);
            fi.Read(_Lst, 0, 4);//9
             _T = BitConverter.ToInt32(_Lst, 0);
            fi.Read(_Lst, 0, 4);//15
            _T = BitConverter.ToInt32(_Lst, 0);
            fi.Read(_Lst, 0, 4);//SEmatChanParam 参数长度
            _T = BitConverter.ToInt32(_Lst, 0);

            int _iNo=SysInfo.m_Tofd_DLL.m_icurChan;
            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_idB= BitConverter.ToInt32(_Lst, 0);
            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iRange = BitConverter.ToInt32(_Lst, 0);
            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iZeroTime = BitConverter.ToInt32(_Lst, 0);
            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iParallelTime = BitConverter.ToInt32(_Lst, 0);

            fi.Read(_Lst, 0, 2);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iPulWidthCode = BitConverter.ToUInt16(_Lst, 0);
            fi.Read(_Lst, 0, 1);//1
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iDemodulation_Flag = _Lst[0];

            fi.Read(_Lst, 0, 1);//1
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iRepeatFreq = _Lst[0];

            fi.Read(_Lst, 0, 1);//1
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iWorkMode = _Lst[0];

            fi.Read(_Lst, 0, 1);//1
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iSecBandWidthF = _Lst[0];

            fi.Read(_Lst, 0, 1);//1
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iImpedanceF = _Lst[0];

            fi.Read(_Lst, 0, 1);//1
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iVolt = _Lst[0];

            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_dSpeed = BitConverter.ToInt32(_Lst, 0);
            fi.Read(_Lst, 0, 1);//1
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iForword = _Lst[0];

            fi.Read(_Lst, 0, 1);//1
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iPcsType = _Lst[0];
            fi.Read(_Lst, 0, 1);//1
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iPcsMode = _Lst[0];
            fi.Read(_Lst, 0, 1);//1
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_BScanMode = _Lst[0];
            fi.Read(_Lst, 0, 1);//1
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iCurEn = _Lst[0];
            fi.Read(_Lst, 0, 1);//1
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iEnPos = _Lst[0];
            _Lst = new byte[318];
            fi.Read(_Lst, 0, 318);
            _Lst = new byte[4];
            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_BRecordLen = BitConverter.ToInt32(_Lst, 0);

            fi.Read(_Lst, 0, 2);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iPcsLW = BitConverter.ToUInt16(_Lst, 0);
            fi.Read(_Lst, 0, 2);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_iPcsBW = BitConverter.ToUInt16(_Lst, 0);

            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsLen = BitConverter.ToSingle(_Lst, 0);
            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsArc = BitConverter.ToSingle(_Lst, 0);
            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsChord = BitConverter.ToSingle(_Lst, 0);

            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsDia = BitConverter.ToSingle(_Lst, 0);
            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsAngle = BitConverter.ToSingle(_Lst, 0);
            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsStart = BitConverter.ToSingle(_Lst, 0);
            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fPcsEnd = BitConverter.ToSingle(_Lst, 0);

            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fEnStep = BitConverter.ToSingle(_Lst, 0);
            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fEnRatio[0] = BitConverter.ToSingle(_Lst, 0);
            fi.Read(_Lst, 0, 4);
            SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_fEnRatio[1] = BitConverter.ToSingle(_Lst, 0);

            #endregion
            #region  2 读取TOFD参数
            //fi.Read(_Lst, 0, 4);//记录个数
            //  _T = BitConverter.ToInt32(_Lst, 0);
            #endregion
            _T = SysInfo.m_Tofd_DLL.m_pSparam[_iNo].m_BRecordLen;
            #region 3 读取检测数据
            var IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Lst, 0);
            for (int i = 0; i < _T; i++)
            {
                try
                {
                    //波峰数据
                    Tofd_Arr _Arr = new Tofd_Arr();
                    _Lst = new byte[Tofd_Arr.UTS_DATA_WIDTH];
                    fi.Read(_Lst, 0, Tofd_Arr.UTS_DATA_WIDTH);

                    IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr.ChannelBuf, 0);
                    Marshal.Copy(_Lst, 0, IntPtArr, _Lst.Length);

                    _Lst = new byte[Tofd_Arr.UTS_DATA_WIDTH];
                    fi.Read(_Lst, 0, Tofd_Arr.UTS_DATA_WIDTH);

                    IntPtArr = Marshal.UnsafeAddrOfPinnedArrayElement(_Arr.ValueBuf, 0);
                    Marshal.Copy(_Lst, 0, IntPtArr, _Lst.Length);

                    _LstTofd.Add(_Arr);
                }
                catch { }
            }
            fi.Close();
            fi.Dispose();
           
            #endregion 
            return _LstTofd;
        }
        /// <summary>
        /// 字符串转字节数组
        /// </summary>
        /// <param name="strHex"></param>
        /// <returns></returns>
        public static byte[] StrToHex(string strHex)
        {
            //清空格
            strHex = strHex.Replace(" ", "");
            if ((strHex.Length % 2) != 0)
                strHex = strHex.Insert(0, "0");
            //数组
            byte[] returnBytes = new byte[strHex.Length / 2];
            //转换
            try
            {
                for (int i = 0; i < returnBytes.Length; i++)
                    returnBytes[i] = Convert.ToByte(strHex.Substring(i * 2, 2), 16);
            }
            catch (Exception e)
            {
                return new byte[strHex.Length / 2];
                //throw e;
            }
            return returnBytes;
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
    
    public class FrameTime
    {

        /// <summary>
        /// 帧序号
        /// </summary>
        public int iFrameNo;
        /// <summary>
        /// 时间ID DateTime.Now.ToString("HHmmssf")
        /// </summary>
        public string strTimeID;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]//设定对齐粒度为一个字节
    [System .Serializable ]
    public struct SEmatChanParam
    {
        /// <summary>
        /// 8 增益 0-1100，显示0-110.0dB   
        /// </summary>
        public Int32 m_idB;
        /// <summary>
        /// 7 范围 10-1000mm   
        /// </summary>
        public Int32 m_iRange;
        /// <summary>
        /// 9 零偏 0-100us    
        /// </summary>
        public Int32 m_iZeroTime;
        /// <summary>
        /// 10 平移 0-100us     
        /// </summary>
        public Int32 m_iParallelTime;

        /// <summary>
        ///11  脉冲个数 1-101，显示(data-1)*5ns
        /// </summary>
        public System.UInt16 m_iPulWidthCode;
        /// <summary>
        /// 1 检波方式	正/负/射/全         
        /// </summary>
        public System.Byte m_iDemodulation_Flag;
        /// <summary>
        /// 重复频率 15/30/60/100/200/300/400/500HZ
        /// </summary>
        public System.Byte m_iRepeatFreq;           //13 重复频率 15/30/60/100/200/300/400/500HZ    
        /// <summary>
        /// 工作模式 自发自收/一发一收   
        /// </summary>
        public System.Byte m_iWorkMode;         // 4 工作模式 自发自收/一发一收   
        /// <summary>
        ///  2 带宽选择2-8M、0.5-4M、1-30M和5-15M 
        /// </summary>
        public System.Byte m_iSecBandWidthF;        // 2 带宽选择 2-8M、0.5-4M、1-30M和5-15M
        /// <summary>
        /// 3 阻抗匹配 48/500欧       
        /// </summary>
        public System.Byte m_iImpedanceF;           // 3 阻抗匹配 48/500欧       
        /// <summary>
        /// 12 高压调节 400/200/300V  
        /// </summary>
        public System.Byte m_iVolt;             // 12 高压调节 400/200/300V  
        /// <summary>
        /// 声速
        /// </summary>
        public int m_dSpeed;               // 6 声速   
        /// <summary>
        /// 前放开关
        /// </summary>
        public System.Byte m_iForword;              //5 前放开关    



        /// <summary>
        /// 0-类型一，1-类型2
        /// </summary>
        public byte m_iPcsType;
        /// <summary>
        /// 0-平板，1-圆弧外壁，2-圆弧内壁
        /// </summary>
        public byte m_iPcsMode;
        /// <summary>
        /// 定位方式：0-模拟，1-编码器 2：测试
        /// </summary>
        public byte m_BScanMode;

        /// <summary>
        /// 当前编码器：0-A，1-B
        /// </summary>
        public byte m_iCurEn;
        /// <summary>
        /// 编码器方向：0-反向，1-正向
        /// </summary>
        public byte m_iEnPos;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 318)]
        public System.Byte[] u8bk;//=new byte[318];//371
        /// <summary>
        /// Tofd记录长度
        /// </summary>
        public int m_BRecordLen;

        public System.UInt16 m_iPcsLW;
        public System.UInt16 m_iPcsBW;

        /// <summary>
        /// 探头中心间距
        /// </summary>
        public float m_fPcsLen;
        /// <summary>
        /// 弧长
        /// </summary>
        public float m_fPcsArc;
        /// <summary>
        /// 弦高
        /// </summary>
        public float m_fPcsChord;

        /// <summary>
        /// 外壁的外径或内壁的内径
        /// </summary>
        public float m_fPcsDia;
        /// <summary>
        /// 楔块角度
        /// </summary>
        public float m_fPcsAngle;
        /// <summary>
        /// 分层起点
        /// </summary>
        public float m_fPcsStart;
        /// <summary>
        /// 分层终点
        /// </summary>
        public float m_fPcsEnd;
        /// <summary>
        /// 步进精度
        /// </summary>
        public float m_fEnStep;

        [MarshalAs(UnmanagedType.ByValArray , SizeConst = 2)]
        /// <summary>
        /// 编码器精度 0:A编码器 1：B编码器
        /// </summary>
        public float[] m_fEnRatio;

        //---------------------------
        ///// <summary>
        ///// 编码器读值
        ///// </summary>
        //public int [] m_iEnPul;        
        ///// <summary>
        ///// 实际位置
        ///// </summary>
        //public float [] m_fEnReal;
        ///// <summary>
        ///// 楔块探头延时时间 2t0
        ///// </summary>
        //public float T0;
        ///// <summary>
        ///// 直通波在楔块运行时间 L0
        ///// </summary>
        //public float L0;

        ///// <summary>
        ///// 直通波在楔块距离
        ///// </summary>
        //public float L0_Distan;
    }
    public struct Emat_Real
    {
        /// <summary>
        /// 编码器读值
        /// </summary>
        public int[] m_iEnPul;
        /// <summary>
        /// 实际位置
        /// </summary>
        public float[] m_fEnReal;
        /// <summary>
        /// 楔块探头延时时间 2t0
        /// </summary>
        public float T0;
        /// <summary>
        /// 直通波在楔块运行时间 L0
        /// </summary>
        public float L0;

        /// <summary>
        /// 直通波在楔块距离
        /// </summary>
        public float L0_Distan;
    }
        /// <summary>
        /// 设置GridView双缓冲
        /// </summary>
        public static class DublGrid
    {
        /// <summary>
        /// 将给定的DataGridView设置双缓冲
        /// </summary>
        /// <param name="dgv">给定的DataGridView</param>
        /// <param name="b">设置为ture即打开双缓冲</param>
        public static void SetDoubleBuffered(this DataGridView dgv, bool b)
        {
            var dgvType = dgv.GetType();
            var pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, b, null);
        }
    }
}
