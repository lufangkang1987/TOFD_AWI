/*
 * Copyright(C) 2 2017 郑州金润高科电子有限公司
 * 文件名: DbGlobal.cs
 * 文件功能描述: 数据接口层静态对象
 * 目的：操作数据库文件
 * 创建标识: 陈大伟 2017-1
 * 修改标识: 
 * 修改描述:
 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using FrameWork.Struct;
using ClassLib_TestData;
namespace ClassLib_DataMang.DataBaseMang.OleDal
{
    /// <summary>
    ///  Access数据接口层静态对象类
    /// </summary>
    public static class DbGlobal
    {
        #region =========== 数据接口层静态对象 ===========
        /// <summary>
        /// 中英文 0：中文 1：英文
        /// </summary>
        public static int m_iLanguage = 0;
        /// <summary>
        /// 创建数据库
        /// </summary>
        public static CreatDataBase ImCreatDataBase = new CreatDataBase();
        /// <summary>
        /// 1 设备信息数据管理类接口
        /// </summary>
        public static Test_Item ImTest_Item = new Test_Item();
       
        /// <summary>
        /// 2 焊缝数据管理类接口
        /// </summary>
        public static Test_Parts ImTest_Parts = new Test_Parts();

        /// <summary>
        /// 3 原始记录数据管理类接口
        /// </summary>
        public static Test_Records ImTest_Records = new Test_Records();
        /// <summary>
        /// 4 异常报告数据管理类接口
        /// </summary>
        public static Test_Statistical_Report ImStatistical_Report = new Test_Statistical_Report();
 /// <summary>
        /// 5 用户信息数据库管理类接口
        /// </summary>
        public static Manage_User ImUser = new Manage_User();

        /// 6 异常信息数据库管理类接口
        /// </summary>
        public static Test_Alarm ImAlarm = new Test_Alarm();
        /// <summary>
        /// 重建数据库 0：已经有数据库就不重建  1：重建
        /// </summary>
        /// <param name="blMainSub_DataBase">true:主表 false:子表</param>
        /// <param name="iType">0：已经有数据库就不重建  1：重建</param>
        /// <param name="strSubDataBaseName">子表数据库名称:用户名 + 受检设备名 + 地址 + 检定时间</param>
        /// <returns></returns>
        public static bool CreatDataBase_New( bool blMainSub_DataBase,int iType=0,string strSubDataBaseName="")
        {
            bool _blRet = false, _blR_Jg = false; 
            string _0 = m_iLanguage == 0 ? "      您   是   否   重   建   数   据   库 ?" : "      Do you want to rebuild the database ?";
            DbGlobal.ImCreatDataBase.m_iDataBase_Type = 0;

            string strConn = "", strMsg = "",strMsg_Cj= _0+"\r\n\r\n\r\n";

            _blR_Jg = DbGlobal.ImCreatDataBase.Jg_DataBase( ref strConn, blMainSub_DataBase, strSubDataBaseName);
            if (_blR_Jg == false)
            {
                System.Threading. Thread.Sleep(50);
          //      _blR_Jg = DbGlobal.ImCreatDataBase.Jg_DataBase(ref strConn, blMainSub_DataBase, strSubDataBaseName);
            }
            if (_blR_Jg && iType == 0) return true;//已经有数据库，不重建
            string _1 = m_iLanguage == 0 ? "连接数据库失败，系统准备自建，请您检查" : "Connection to database failed, system ready to build, please check";
            string _2= m_iLanguage == 0 ? "1 电脑是否安装了" : "1 is the computer installed";
            string _3 = m_iLanguage == 0 ? "2 连接串是否正确:" : "2 is the connection string correct:";
            string _4 = m_iLanguage == 0 ? " 系统准备自建，请您检查 " : " The system is ready for self-construction, please check ";
            string _5 = m_iLanguage == 0 ? "1 连接串是否正确:" : "1. Whether the connection string is correct:";
            string _6 = m_iLanguage == 0 ? "警示" : "Warning";

            string _7 = m_iLanguage == 0 ? "用户表:" : "user table:";
            string _8 = m_iLanguage == 0 ? "信息表:" : "information table:";
            string _9 = m_iLanguage == 0 ? "距离厚度表" : "Distance and thickness table";
            string _10 = m_iLanguage == 0 ? " 添加用户名Dellon: " : " Add user name Dellon: ";
            string _11 = m_iLanguage == 0 ? "成功" : "Success";
            string _12 = m_iLanguage == 0 ? "失败" : "Failure";

            if (_blR_Jg == false)
                strMsg = _1+" \r\n\r\n" + _2+//"1 电脑是否安装了" +
                   (DbGlobal.ImCreatDataBase.m_iDataBase_Type==0?"Access":" SQL Server")+
                    " \r\n" + _3 + strConn + "\r\n\r\n\r\n" + strMsg_Cj;
            else
                strMsg = _4 +"\r\n\r\n" + _5 + strConn + "\r\n\r\n\r\n" + strMsg_Cj;

            if (blMainSub_DataBase==false || System.Windows.Forms.MessageBox.Show(strMsg, _6, System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                string strCreat_JL = "";
                if (blMainSub_DataBase == false) DbGlobal.ImCreatDataBase.m_strSubDataBaseName = strSubDataBaseName;
                if (DbGlobal.ImCreatDataBase.Creat_DataBase(blMainSub_DataBase))
                {
                    #region 创建表
                    if (blMainSub_DataBase)//主表：主铭牌信息表、用户名称表
                    {
                        //1 用户表
                        strCreat_JL = "1 "+ _7+":";
                        if (DbGlobal.ImUser.Creat_Table())
                        {
                            strCreat_JL += _11 +"，";
                            #region 1 添加用户初始记录
                            strCreat_JL += _10;// " 添加用户名Dellon: ";
                            Class_User _User = new Class_User();
                            _User.Name = "Dellon"; _User.Pass = "123"; _User.strLevel = "0";

                            if (DbGlobal.ImUser.SaveData(ref _User) == 0)
                                strCreat_JL += _12 + ",";// "失败，";
                            else
                                strCreat_JL += _11 + ",";// "成功，";
                            #endregion 添加初始记录
                        }
                        else
                            strCreat_JL += _12 + ",";//"失败，";
                        //2 铭牌表
                        strCreat_JL += "2 " + _8 + "：";// 信息表：";
                        if (DbGlobal.ImTest_Item.Creat_Table())
                            strCreat_JL += _11 + ",";// "成功，";
                        else
                            strCreat_JL += _12 + ",";// "失败，";

                        //3 波形表
                        //strCreat_JL += "3 波形表：";
                        //if (DbGlobal.ImWave.Creat_Table())
                        //    strCreat_JL += "成功";
                        //else
                        //    strCreat_JL += "失败";
                    }
                    else//子表：铭牌信息表、距离厚度表、波形表
                    {
                        //1 铭牌表
                        strCreat_JL = "1 " + _8;// 信息表：";
                        if (DbGlobal.ImTest_Item.Creat_Table())
                            strCreat_JL += _11 + ",";// "成功，";
                        else
                            strCreat_JL += _12 + ",";// "失败，";
                        //2 焊缝表
                        strCreat_JL += "2 " + _9 + ": ";// 距离厚度表：";
                        if (DbGlobal.ImTest_Parts .Creat_Table( ))
                            strCreat_JL += _11 + ",";// "成功，";
                        else
                            strCreat_JL += _12 + ",";// "失败，";
                        //3 波形表
                        strCreat_JL += "3 波形表：";
                        if (DbGlobal.ImTest_Records .Creat_Table())
                            strCreat_JL += "成功";
                        else
                            strCreat_JL += "失败";

                        strCreat_JL += "4 异常表：";
                        if (DbGlobal.ImAlarm .Creat_Table())
                            strCreat_JL += "成功";
                        else
                            strCreat_JL += "失败";
                    }
                    #endregion 创建表

                    if (blMainSub_DataBase)
                        System.Windows.Forms.MessageBox.Show(strCreat_JL);
                    _blRet = strCreat_JL.IndexOf(_12) > -1 ? false : true;
                }
            }

            return _blRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blMainSub_DataBase">true:主表 false:子表</param>
        /// <param name="iType">0：已经有数据库就不重建  1：重建</param>
        /// <param name="strSubDataBaseName">数据表_用户名 + 受检设备名 + 地址 + 检定时间</param>
        /// <returns></returns>
        public static bool CreatDataBase_New_1(bool blMainSub_DataBase, int iType = 0, string strSubDataBaseName = "")
        {
            bool _blRet = false, _blR_Jg = false;
            string _0 = m_iLanguage == 0 ? "      您   是   否   重   建   数   据   库 ?" : "      Do you want to rebuild the database ?";

            string strConn = "", strMsg = "", strMsg_Cj = _0 + "\r\n\r\n\r\n";

            _blR_Jg = DbGlobal.ImCreatDataBase.Jg_DataBase(ref strConn, blMainSub_DataBase, strSubDataBaseName);
            if (_blR_Jg == false && DbGlobal.ImCreatDataBase.m_iDataBase_Type==0)
            {
                System.Threading.Thread.Sleep(50);
                _blR_Jg = DbGlobal.ImCreatDataBase.Jg_DataBase(ref strConn, blMainSub_DataBase, strSubDataBaseName);
            }
            if (_blR_Jg && iType == 0) return true;//已经有数据库，不重建
            string _1 = m_iLanguage == 0 ? "连接数据库失败，系统准备自建，请您检查" : "Connection to database failed, system ready to build, please check";
            string _2 = m_iLanguage == 0 ? "1 电脑是否安装了" : "1 is the computer installed";
            string _3 = m_iLanguage == 0 ? "2 连接串是否正确:" : "2 is the connection string correct:";
            string _4 = m_iLanguage == 0 ? " 系统准备自建，请您检查 " : " The system is ready for self-construction, please check ";
            string _5 = m_iLanguage == 0 ? "1 连接串是否正确:" : "1. Whether the connection string is correct:";
            string _6 = m_iLanguage == 0 ? "警示" : "Warning";

            string _7 = m_iLanguage == 0 ? "用户表:" : "user table:";
            string _8 = m_iLanguage == 0 ? "信息表:" : "information table:";
            string _9 = m_iLanguage == 0 ? "距离厚度表" : "Distance and thickness table";
            string _10 = m_iLanguage == 0 ? " 添加用户名Dellon: " : " Add user name Dellon: ";
            string _11 = m_iLanguage == 0 ? "成功" : "Success";
            string _12 = m_iLanguage == 0 ? "失败" : "Failure";

            if (_blR_Jg == false)
                strMsg = _1 + " \r\n\r\n" + _2 +//"1 电脑是否安装了" +
                   (DbGlobal.ImCreatDataBase.m_iDataBase_Type == 0 ? "Access" : " SQL Server") +
                    " \r\n" + _3 + strConn + "\r\n\r\n\r\n" + strMsg_Cj;
            else
                strMsg = _4 + "\r\n\r\n" + _5 + strConn + "\r\n\r\n\r\n" + strMsg_Cj;

            if (blMainSub_DataBase == false || System.Windows.Forms.MessageBox.Show(strMsg, _6, System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                string strCreat_JL = "";
                if (blMainSub_DataBase == false) DbGlobal.ImCreatDataBase.m_strSubDataBaseName = strSubDataBaseName;
                if (blMainSub_DataBase==false ||   DbGlobal.ImCreatDataBase.Creat_DataBase(blMainSub_DataBase))
                {
                    #region 创建表
                    if (blMainSub_DataBase)//主表：主铭牌信息表、用户名称表
                    {
                        DateTime dtStar = DateTime.Now;
                        while (true)
                        {
                            try
                            {
                                System.Windows.Forms. Application.DoEvents();
                                if (DateTime.Now.Subtract(dtStar).TotalSeconds > 1) break;
                                System.Threading. Thread.Sleep(10);
                            }
                            catch { break; }
                        }
                        //1 用户表
                        strCreat_JL = "1 " + _7 + ":";
                        if (DbGlobal.ImUser.Creat_Table())
                        {
                            strCreat_JL += _11 + "，";
                            #region 1 添加用户初始记录
                            strCreat_JL += _10;// " 添加用户名Dellon: ";
                            Class_User _User = new Class_User();
                            _User.Name = "Dellon"; _User.Pass = "123"; _User.strLevel = "0";

                            if (DbGlobal.ImUser.SaveData(ref _User) == 0)
                                strCreat_JL += _12 + ",";// "失败，";
                            else
                                strCreat_JL += _11 + ",";// "成功，";
                            #endregion 添加初始记录
                        }
                        else
                            strCreat_JL += _12 + ",";//"失败，";
                        //2 铭牌表
                        strCreat_JL += "2 " + _8 + "：";// 信息表：";
                        if (DbGlobal.ImTest_Item.Creat_Table())
                            strCreat_JL += _11 + ",";// "成功，";
                        else
                            strCreat_JL += _12 + ",";// "失败，";

                        ////3 波形表
                        //strCreat_JL += "3 异常标注表：";
                        //if (DbGlobal.ImAlarm.Creat_Table())
                        //    strCreat_JL += "成功";
                        //else
                        //    strCreat_JL += "失败";
                    }
                    else//子表：铭牌信息表、距离厚度表、波形表
                    {
                        //1 铭牌表
                        strCreat_JL = "1 " + _8;// 信息表：";
                        if (DbGlobal.ImTest_Item.Creat_Table())
                            strCreat_JL += _11 + ",";// "成功，";
                        else
                            strCreat_JL += _12 + ",";// "失败，";
                        //2 焊缝表
                        strCreat_JL += "2 " + _9 + ": ";// 距离厚度表：";
                        if (DbGlobal.ImTest_Parts.Creat_Table())
                            strCreat_JL += _11 + ",";// "成功，";
                        else
                            strCreat_JL += _12 + ",";// "失败，";
                        //3 波形表
                        strCreat_JL += "3 波形表：";
                        if (DbGlobal.ImTest_Records.Creat_Table())
                            strCreat_JL += "成功";
                        else
                            strCreat_JL += "失败";

                        strCreat_JL += "4 异常表：";
                        if (DbGlobal.ImStatistical_Report.Creat_Table())
                            strCreat_JL += "成功";
                        else
                            strCreat_JL += "失败";
                    }
                    #endregion 创建表


                    //#region 创建表
                    //if (blMainSub_DataBase)//主表：主铭牌信息表、用户名称表
                    //{
                    //    //1 用户表
                    //    strCreat_JL = "1 " + _7 + ":";
                    //    if (DbGlobal.ImUser.Creat_Table())
                    //    {
                    //        strCreat_JL += _11 + "，";
                    //        #region 1 添加用户初始记录
                    //        strCreat_JL += _10;// " 添加用户名Dellon: ";
                    //        Class_User _User = new Class_User();
                    //        _User.Name = "Dellon"; _User.Pass = "123"; _User.strLevel = "0";

                    //        if (DbGlobal.ImUser.SaveData(ref _User) == 0)
                    //            strCreat_JL += _12 + ",";// "失败，";
                    //        else
                    //            strCreat_JL += _11 + ",";// "成功，";
                    //        #endregion 添加初始记录
                    //    }
                    //    else
                    //        strCreat_JL += _12 + ",";//"失败，";
                    //    //2 铭牌表
                    //    strCreat_JL += "2 " + _8 + "：";// 信息表：";
                    //    if (DbGlobal.ImEquipment_Info.Creat_Table())
                    //        strCreat_JL += _11 + ",";// "成功，";
                    //    else
                    //        strCreat_JL += _12 + ",";// "失败，";
                    //}
                    //else//子表：铭牌信息表、距离厚度表、波形表
                    //{
                    //    //1 铭牌表
                    //    //strCreat_JL += "1 " + _8;// 信息表：";
                    //    //if (DbGlobal.ImEquipment_Info.Creat_Table())
                    //    //    strCreat_JL += _11 + ",";// "成功，";
                    //    //else
                    //    //    strCreat_JL += _12 + ",";// "失败，";
                    //    //1 距离厚度表
                    //    strCreat_JL += "1 " + _9 + ": ";// 距离厚度表：";
                    //    DbGlobal.ImDistanceThick.DbTableName = "DistanceThick" + "_" + strSubDataBaseName;
                    //    if (DbGlobal.ImDistanceThick.Creat_Table(DbGlobal.ImDistanceThick.m_iChnns))
                    //        strCreat_JL += _11 + ",";// "成功，";
                    //    else
                    //        strCreat_JL += _12 + ",";// "失败，";
                    //    //3 波形表
                    //    //strCreat_JL += "3 波形表：";
                    //    //if (DbGlobal.ImWave.Creat_Table())
                    //    //    strCreat_JL += "成功";
                    //    //else
                    //    //    strCreat_JL += "失败";
                    //}
                    //#endregion 创建表

                    if (blMainSub_DataBase)
                        System.Windows.Forms.MessageBox.Show(strCreat_JL);
                    _blRet = strCreat_JL.IndexOf(_12) > -1 ? false : true;
                }
            }

            return _blRet;
        }

        /// <summary>
        /// 创建临时表
        /// </summary>
        /// <param name="iType"></param>
        /// <param name="strSubDataBaseName"></param>
        /// <returns></returns>
        public static bool CreatDataBase_NewTmp(int iType = 0, string strSubDataBaseName = "")
        {
            bool _blRet = false, _blR_Jg = false; ;

            string strConn = "";

            _blR_Jg = DbGlobal.ImCreatDataBase.Jg_Tmp_DataBase(ref strConn, false, strSubDataBaseName);
            if (_blR_Jg && iType == 0) return true;//已经有数据库，不重建

            string strCreat_JL = "";
            DbGlobal.ImCreatDataBase.m_strSubDataBaseName = strSubDataBaseName;
            if (DbGlobal.ImCreatDataBase.Creat_TmpDataBase(false))
            {
                #region 创建表
                string _7 = m_iLanguage == 0 ? "用户表:" : "user table:";
                string _8 = m_iLanguage == 0 ? "信息表:" : "information table:";
                string _9 = m_iLanguage == 0 ? "距离厚度表" : "Distance and thickness table";
                string _10 = m_iLanguage == 0 ? " 添加用户名Dellon: " : " Add user name Dellon: ";
                string _11 = m_iLanguage == 0 ? "成功" : "Success";
                string _12 = m_iLanguage == 0 ? "失败" : "Failure";

                //子表：铭牌信息表、距离厚度表、波形表
                {
                    //1 铭牌表
                    strCreat_JL = "1 " + _8;// 信息表：";
                    if (DbGlobal.ImTest_Item.Creat_Table())
                        strCreat_JL += _11 + ",";// "成功，";
                    else
                        strCreat_JL += _12 + ",";// "失败，";
                                                 //2 焊缝表
                    strCreat_JL += "2 " + _9 + ": ";// 距离厚度表：";
                    if (DbGlobal.ImTest_Parts.Creat_Table())
                        strCreat_JL += _11 + ",";// "成功，";
                    else
                        strCreat_JL += _12 + ",";// "失败，";
                                                 //3 波形表
                    strCreat_JL += "3 波形表：";
                    if (DbGlobal.ImTest_Records.Creat_Table())
                        strCreat_JL += "成功";
                    else
                        strCreat_JL += "失败";

                    strCreat_JL += "4 异常表：";
                    if (DbGlobal.ImStatistical_Report.Creat_Table())
                        strCreat_JL += "成功";
                    else
                        strCreat_JL += "失败";
                }
                #endregion 创建表
                _blRet = strCreat_JL.IndexOf("失败") > -1 ? false : true;
            }

            return _blRet;
        }
        #endregion =======
    }
}
