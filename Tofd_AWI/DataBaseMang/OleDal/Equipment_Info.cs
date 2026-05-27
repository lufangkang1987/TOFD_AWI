/*
 * Copyright(C) 2 2017 郑州金润高科电子有限公司
 * 文件名: Equipment_Info.cs
 * 文件功能描述: 检测设备信息数据操作
 * 目的：用户信息数据库文件
 * 创建标识: 陈大伟 2017-1
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
using DataBaseMang.DBUtility;//操作数据库

namespace FrameWork.DataBaseMang.OleDal
{
    public class Manage_Test_Item
    {
        /// <summary>
        /// 操作的数据表名称
        /// </summary>
        private static string _DbTableName = "EquipmentInfo";
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

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <returns></returns>
        public bool Creat_Table()
        {
            bool _blRet = false;
            string strSQL = "", strFileName = ""; ;
            try
            {
                #region 字段表
                strFileName = " ID varchar(20)     NOT NULL," +//ID号
                             " strSetName    varchar(50)  NULL , " +//受检设备名称
                             " strZjsb    varchar(50)  NULL , " +//主检设备
                             " strAdd_Dz    varchar(50)  NULL , " +//设备地址
                             " strDwmc    varchar(50)  NULL , " +//单位名称
                             " strYt    varchar(50)   NULL, " +//设备用途
                              " strArae    varchar(250)  NULL , " +//同一工件不同检验区域标注

                             " strAdd_Sheng    varchar(10)  NULL , " +//省
                             " strAdd_Shi    varchar(20)  NULL , " +//市

                             " strTemperature    varchar(10)  NULL , " +//温度
                             " strJyrq    varchar(18)  NULL , " +//检验日期
                             " strBgrq    varchar(18)  NULL , " +//报告日期
                             " strBgbh    varchar(50)  NULL , " +//报告编号

                             " strJyy    varchar(50)  NULL , " +//检验员
                             " strHyy    varchar(50)   NULL, " +//核验员
                             " strPhone    varchar(17)  NULL , " +//联系电话
                             " strSp    varchar(50)  NULL , " +//审批

                             " strCcbh    varchar(30)   NULL, " +//出厂编号
                             " strZsbh    varchar(50)  NULL , " +//使用登记证编号
                             " strJyyj    varchar(50)  NULL , " +//检验依据
                             " strSynx    varchar(5)  NULL , " +//已经使用年限
                             " strYxsyYl    varchar(10)  NULL , " +//工作压力压力MPa
                             
                             " strSjdw    varchar(50)  NULL , " +//设计单位
                             " strZzdw    varchar(50)  NULL , " +//制造单位
                             " strSjYl    varchar(10)  NULL , " +// 设计压力 MPa
                             " strZtcl    varchar(10)  NULL , " +// 主体材质
                             " strCcrj    varchar(10)  NULL , " +//尺寸/容积
" strTyrq    varchar(17)  NULL , " +//投用日期
" strCcJZ    varchar(30)  NULL , " +//储存介质
(_iDataBase_Type == 0 ? " iFirstTest    Integer  NULL , " : "iFirstTest tinyint  NULL, ") +//是否首次检定 1:首次0：周检
" strRegistNo    varchar(30)  NULL , " +//注册代码
" strEquipmentNo    varchar(20)  NULL , " +//设备代码
(_iDataBase_Type == 0 ? " flNormal_Thickness    Single  NULL , " : "flNormal_Thickness float  NULL, ") +//公称厚度
(_iDataBase_Type == 0 ? " flNormal_Thickness_2    Single  NULL , " : "flNormal_Thickness_2 float  NULL, ") +//公称厚度2
(_iDataBase_Type == 0 ? " flThick_Min    Single  NULL , " : "flThick_Min float  NULL, ") +//最小厚度
(_iDataBase_Type == 0 ? " flPosition_Min    Single  NULL , " : "flPosition_Min float  NULL, ") +//最小厚度对应位置
(_iDataBase_Type == 0 ? " iThick_Min_Row    Single  NULL , " : "iThick_Min_Row float  NULL, ") +//最小厚度对应行

(_iDataBase_Type == 0 ? " flPosition_Min_Y    Single  NULL , " : "flPosition_Min_Y float  NULL, ") +//最小厚度对应位置

(_iDataBase_Type == 0 ? " iChn_Min    Integer  NULL , " : "iChn_Min tinyint  NULL, ") +//最小厚度对应通道号1-N
(_iDataBase_Type == 0 ? " flWc_Min    Single  NULL , " : "flWc_Min float  NULL, ") +//最小厚度对应 最大偏差(mm/%)

(_iDataBase_Type == 0 ? " flThick_Max    Single  NULL , " : "flThick_Max float  NULL, ") +//最大厚度
(_iDataBase_Type == 0 ? " flPosition_Max    Single  NULL , " : "flPosition_Max float  NULL, ") +//最大厚度对应深度
(_iDataBase_Type == 0 ? " iThick_Max_Row    Single  NULL , " : "iThick_Max_Row float  NULL, ") +//最小厚度对应行

(_iDataBase_Type == 0 ? " flPosition_Max_Y    Single  NULL , " : "flPosition_Max_Y float  NULL, ") +//最大厚度对应深度

(_iDataBase_Type == 0 ? " iChn_Max    Integer  NULL , " : "iChn_Max tinyint  NULL, ") +//最大厚度对应通道号1-N
(_iDataBase_Type == 0 ? " flWc_Max    Single  NULL , " : "flWc_Max float  NULL, ") +//最大厚度对应误差值

(_iDataBase_Type == 0 ? " flThick_Average_Max    Single  NULL , " : "flThick_Average_Max float  NULL, ") +//平均厚度
(_iDataBase_Type == 0 ? " flWc_Average_Max    Single  NULL , " : "flWc_Average_Max float  NULL, ") +//平均偏差(mm/%)

" strMeasure_Type    varchar(20)  NULL , " +//测量仪器型号
" strMeasure_Number    varchar(20)  NULL , " +//测量仪器编号
" strMeasure_Precision    varchar(10)  NULL , " +//测量仪器精度

" strQiKuai_Sd    varchar(10)  NULL , " +//楔块速度m/s
" strJt_Sd    varchar(10)  NULL , " +//井筒速度m/s
" strSc_Sd    varchar(10)  NULL , " +//水层速度m/s

" strNum_Limit_Low_Type    varchar(10)  NULL , " +//色标类型
" strWc_Limit    varchar(250)  NULL , " +//现在存放当前测量的尺子信息
" strWc_Col    varchar(248)  NULL , " +//误差段对应的RGB值
" strThickAlarm    varchar(15)  NULL , " +//厚度报警误差限
" strMinThickLimit    varchar(15)  NULL , " +//最小厚度限

(_iDataBase_Type == 0 ? " iChnns    Integer  NULL , " : "iChnns tinyint  NULL, ") +//通道数量
(_iDataBase_Type == 0 ? " iHaveY    Integer  NULL , " : "iHaveY tinyint  NULL, ") +//0:没有Y轴编码器 1：有Y轴编码器


" strOther_1    varchar(20)  NULL , " +//备用1
" strOther_2    varchar(20)  NULL , " +//备用2
" strOther_3    varchar(100)  NULL , " +//备用3
" strOther_4    varchar(20)  NULL , " +//备用4
" strOther_5    varchar(20)  NULL , " +//备用5

" ReMark    varchar(200)  NULL "; //备注

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
            { System.Windows.Forms.MessageBox.Show("Manage_Test_Item.cs的 Creat_Table error：" + e.Message + e.StackTrace + " " + strSQL); }
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

        public int UpData_Data(string strDwmc,string strSetName,string strAdd_Dz,string strJyrq,string strArae, ref Class_Equipment_Info _Infor)
        {
            int iRet = 0;
            string strSQL = "";
            string strAdd_Val = "";//增加Sql值
            string strInsert_Add_Zd = "";//插入字段表
            string strUpdata_Add_Zd = "";//更新字符串
            try
            {
                //1 查询
                //单位名称  受检设备名称  设备地址
                strSQL = "SELECT * from " + _DbTableName + " where strDwmc= '" + strDwmc + "'  and strSetName='" + strSetName + "'" +
                          "  and strAdd_Dz='" + strAdd_Dz + "'" + " and strJyrq='" + strJyrq + "'" + " and strArae='" + strArae + "'" ;

                #region 插入字符串
                strInsert_Add_Zd = "ID,strSetName,strZjsb,strAdd_Dz," +
                             "strDwmc,strYt,strAdd_Sheng,strAdd_Shi,strTemperature,strJyrq,strBgrq,strBgbh,strJyy,strHyy,strPhone,strSp," +
                             "strCcbh,strZsbh,strJyyj,strSynx,strYxsyYl," +
                             "strSjdw,strZzdw,strSjYl,strZtcl,strCcrj,strTyrq,strCcJZ," +
                             "iFirstTest,strRegistNo,strEquipmentNo,flNormal_Thickness,flNormal_Thickness_2,flThick_Min,flPosition_Min,flPosition_Min_Y,iChn_Min,flWc_Min,flThick_Max,flPosition_Max,flPosition_Max_Y,iChn_Max,flWc_Max," +
                             "flThick_Average_Max,flWc_Average_Max," +
                             "strMeasure_Type,strMeasure_Number,strMeasure_Precision," +
                             "strQiKuai_Sd,strJt_Sd,strSc_Sd," +
                             "strNum_Limit_Low_Type,strWc_Limit,strWc_Col,strThickAlarm,strMinThickLimit," +
                             "iChnns,iHaveY,strOther_1,strOther_2,strOther_3,strOther_4,strOther_5,ReMark,iThick_Min_Row,iThick_Max_Row,strArae";

                // if (_Infor.ID=="")
                _Infor.ID = _Infor.strJyrq.Replace("_", "");// DateTime.Now.ToString("yyMMddHHmmss");
                strAdd_Val = "'" + _Infor.ID + "'";
                strAdd_Val += ",'" + _Infor.strSetName + "'";
                strAdd_Val += ",'" + _Infor.strZjsb + "'";
                strAdd_Val += ",'" + _Infor.strAdd_Dz + "'";
                strAdd_Val += ",'" + _Infor.strDwmc + "'";
                strAdd_Val += ",'" + _Infor.strYt + "'";
                strAdd_Val += ",'" + _Infor.strAdd_Sheng + "'";
                strAdd_Val += ",'" + _Infor.strAdd_Shi + "'";
                strAdd_Val += ",'" + _Infor.strTemperature + "'";

                strAdd_Val += ",'" + _Infor.strJyrq + "'";
                strAdd_Val += ",'" + _Infor.strBgrq + "'";
                strAdd_Val += ",'" + _Infor.strBgbh + "'";
                strAdd_Val += ",'" + _Infor.strJyy + "'";
                strAdd_Val += ",'" + _Infor.strHyy + "'";
                strAdd_Val += ",'" + _Infor.strPhone + "'";
                strAdd_Val += ",'" + _Infor.strSp + "'";

                strAdd_Val += ",'" + _Infor.strCcbh + "'";
                strAdd_Val += ",'" + _Infor.strZsbh + "'";
                strAdd_Val += ",'" + _Infor.strJyyj + "'";
                strAdd_Val += ",'" + _Infor.strSynx + "'";
                strAdd_Val += ",'" + _Infor.strYxsyYl + "'";

                strAdd_Val += ",'" + _Infor.strSjdw + "'";
                strAdd_Val += ",'" + _Infor.strZzdw + "'";
                strAdd_Val += ",'" + _Infor.strSjYl + "'";
                strAdd_Val += ",'" + _Infor.strZtcl + "'";
                strAdd_Val += ",'" + _Infor.strCcrj + "'";
                strAdd_Val += ",'" + _Infor.strTyrq + "'";
                strAdd_Val += ",'" + _Infor.strCcJZ + "'";

                strAdd_Val += "," + _Infor.iFirstTest;
                strAdd_Val += ",'" + _Infor.strRegistNo + "'";
                strAdd_Val += ",'" + _Infor.strEquipmentNo + "'";
                strAdd_Val += "," + _Infor.flNormal_Thickness;
                strAdd_Val += "," + _Infor.flNormal_Thickness_2;
                strAdd_Val += "," + _Infor.flThick_Min;
                strAdd_Val += "," + _Infor.flPosition_Min;
                strAdd_Val += "," + _Infor.flPosition_Min_Y;
                strAdd_Val += "," + _Infor.iChn_Min;
                strAdd_Val += "," + _Infor.flWc_Min;
                strAdd_Val += "," + _Infor.flThick_Max;
                strAdd_Val += "," + _Infor.flPosition_Max;
                strAdd_Val += "," + _Infor.flPosition_Max_Y;
                strAdd_Val += "," + _Infor.iChn_Max;
                strAdd_Val += "," + _Infor.flWc_Max;

                strAdd_Val += "," + _Infor.flThick_Average_Max;
                strAdd_Val += "," + _Infor.flWc_Average_Max;

                strAdd_Val += ",'" + _Infor.strMeasure_Type + "'";
                strAdd_Val += ",'" + _Infor.strMeasure_Number + "'";
                strAdd_Val += ",'" + _Infor.strMeasure_Precision + "'";

                strAdd_Val += ",'" + _Infor.strQiKuai_Sd + "'";
                strAdd_Val += ",'" + _Infor.strJt_Sd + "'";
                strAdd_Val += ",'" + _Infor.strSc_Sd + "'";

                strAdd_Val += ",'" + _Infor.strNum_Limit_Low_Type + "'";
                strAdd_Val += ",'" + _Infor.strWc_Limit + "'";
                strAdd_Val += ",'" + _Infor.strWc_Col + "'";
                strAdd_Val += ",'" + _Infor.strThickAlarm + "'";
                strAdd_Val += ",'" + _Infor.strMinThickLimit + "'";

                strAdd_Val += "," + _Infor.iChnns;
                strAdd_Val += "," + _Infor.iHaveY;

                strAdd_Val += ",'" + _Infor.strOther_1 + "'";
                strAdd_Val += ",'" + _Infor.strOther_2 + "'";
                strAdd_Val += ",'" + _Infor.strOther_3 + "'";
                strAdd_Val += ",'" + _Infor.strOther_4 + "'";
                strAdd_Val += ",'" + _Infor.strOther_5 + "'";
                strAdd_Val += ",'" + _Infor.ReMark + "'";

                strAdd_Val += "," + _Infor.iThick_Min_Row;
                strAdd_Val += "," + _Infor.iThick_Max_Row;
                strAdd_Val += ",'" + _Infor.strArae + "'";
               
                #endregion 插入字符串

                #region 更新字符串
                if (_Infor.ID == "")
                {
                    _Infor.ID = DateTime.Now.ToString("yyMMddHHmmss");
                    iRet = 2;
                }
                //3 插入数据
                strUpdata_Add_Zd = "SET ID='" + _Infor.ID + "'";
                strUpdata_Add_Zd += ",strSetName='" + _Infor.strSetName + "'";
                strUpdata_Add_Zd += ",strZjsb='" + _Infor.strZjsb + "'";
                strUpdata_Add_Zd += ",strAdd_Dz='" + _Infor.strAdd_Dz + "'";
                strUpdata_Add_Zd += ",strDwmc='" + _Infor.strDwmc + "'";
                strUpdata_Add_Zd += ",strYt='" + _Infor.strYt + "'";
                strUpdata_Add_Zd += ",strAdd_Sheng='" + _Infor.strAdd_Sheng + "'";
                strUpdata_Add_Zd += ",strAdd_Shi='" + _Infor.strAdd_Shi + "'";
                strUpdata_Add_Zd += ",strTemperature='" + _Infor.strTemperature + "'";

                strUpdata_Add_Zd += ",strJyrq='" + _Infor.strJyrq + "'";
                strUpdata_Add_Zd += ",strBgrq='" + _Infor.strBgrq + "'";
                strUpdata_Add_Zd += ",strBgbh='" + _Infor.strBgbh + "'";
                strUpdata_Add_Zd += ",strJyy='" + _Infor.strJyy + "'";
                strUpdata_Add_Zd += ",strHyy='" + _Infor.strHyy + "'";
                strUpdata_Add_Zd += ",strPhone='" + _Infor.strPhone + "'";
                strUpdata_Add_Zd += ",strSp='" + _Infor.strSp + "'";

                strUpdata_Add_Zd += ",strCcbh='" + _Infor.strCcbh + "'";
                strUpdata_Add_Zd += ",strZsbh='" + _Infor.strZsbh + "'";
                strUpdata_Add_Zd += ",strJyyj='" + _Infor.strJyyj + "'";
                strUpdata_Add_Zd += ",strSynx='" + _Infor.strSynx + "'";
                strUpdata_Add_Zd += ",strYxsyYl='" + _Infor.strYxsyYl + "'";

                strUpdata_Add_Zd += ",strSjdw='" + _Infor.strSjdw + "'";
                strUpdata_Add_Zd += ",strZzdw='" + _Infor.strZzdw + "'";
                strUpdata_Add_Zd += ",strSjYl='" + _Infor.strSjYl + "'";
                strUpdata_Add_Zd += ",strZtcl='" + _Infor.strZtcl + "'";
                strUpdata_Add_Zd += ",strCcrj='" + _Infor.strCcrj + "'";
                strUpdata_Add_Zd += ",strTyrq='" + _Infor.strTyrq + "'";
                strUpdata_Add_Zd += ",strCcJZ='" + _Infor.strCcJZ + "'";

                strUpdata_Add_Zd += ",iFirstTest=" + _Infor.iFirstTest;
                strUpdata_Add_Zd += ",strRegistNo='" + _Infor.strRegistNo + "'";
                strUpdata_Add_Zd += ",strEquipmentNo='" + _Infor.strEquipmentNo + "'";
                strUpdata_Add_Zd += ",flNormal_Thickness=" + _Infor.flNormal_Thickness;
                strUpdata_Add_Zd += ",flNormal_Thickness_2=" + _Infor.flNormal_Thickness_2;
                strUpdata_Add_Zd += ",flThick_Min=" + _Infor.flThick_Min;
                strUpdata_Add_Zd += ",flPosition_Min=" + _Infor.flPosition_Min;
                strUpdata_Add_Zd += ",iThick_Min_Row=" + _Infor.iThick_Min_Row ;
                strUpdata_Add_Zd += ",flPosition_Min_Y=" + _Infor.flPosition_Min_Y;
                strUpdata_Add_Zd += ",iChn_Min=" + _Infor.iChn_Min;
                strUpdata_Add_Zd += ",flWc_Min=" + _Infor.flWc_Min;
                strUpdata_Add_Zd += ",flThick_Max=" + _Infor.flThick_Max;
                strUpdata_Add_Zd += ",flPosition_Max=" + _Infor.flPosition_Max;
                strUpdata_Add_Zd += ",iThick_Max_Row=" + _Infor.iThick_Max_Row ;
                strUpdata_Add_Zd += ",flPosition_Max_Y=" + _Infor.flPosition_Max_Y;
                strUpdata_Add_Zd += ",iChn_Max=" + _Infor.iChn_Max;
                strUpdata_Add_Zd += ",flWc_Max=" + _Infor.flWc_Max;

                strUpdata_Add_Zd += ",flThick_Average_Max=" + _Infor.flThick_Average_Max;
                strUpdata_Add_Zd += ",flWc_Average_Max=" + _Infor.flWc_Average_Max;

                strUpdata_Add_Zd += ",strMeasure_Type='" + _Infor.strMeasure_Type + "'";
                strUpdata_Add_Zd += ",strMeasure_Number='" + _Infor.strMeasure_Number + "'";
                strUpdata_Add_Zd += ",strMeasure_Precision='" + _Infor.strMeasure_Precision + "'";

                strUpdata_Add_Zd += ",strQiKuai_Sd='" + _Infor.strQiKuai_Sd + "'";
                strUpdata_Add_Zd += ",strJt_Sd='" + _Infor.strJt_Sd + "'";
                strUpdata_Add_Zd += ",strSc_Sd='" + _Infor.strSc_Sd + "'";

                strUpdata_Add_Zd += ",strNum_Limit_Low_Type='" + _Infor.strNum_Limit_Low_Type + "'";
                strUpdata_Add_Zd += ",strWc_Limit='" + _Infor.strWc_Limit + "'";
                strUpdata_Add_Zd += ",strWc_Col='" + _Infor.strWc_Col + "'";
                strUpdata_Add_Zd += ",strThickAlarm='" + _Infor.strThickAlarm + "'";
                strUpdata_Add_Zd += ",strMinThickLimit='" + _Infor.strMinThickLimit + "'";


                strUpdata_Add_Zd += ",iChnns=" + _Infor.iChnns;
                strUpdata_Add_Zd += ",iHaveY=" + _Infor.iHaveY;

                strUpdata_Add_Zd += ",strOther_1='" + _Infor.strOther_1 + "'";
                strUpdata_Add_Zd += ",strOther_2='" + _Infor.strOther_2 + "'";
                strUpdata_Add_Zd += ",strOther_3='" + _Infor.strOther_3 + "'";
                strUpdata_Add_Zd += ",strOther_4='" + _Infor.strOther_4 + "'";
                strUpdata_Add_Zd += ",strOther_5='" + _Infor.strOther_5 + "'";
                strUpdata_Add_Zd += ",ReMark='" + _Infor.ReMark + "'";

                strUpdata_Add_Zd += ",strArae='" + _Infor.strArae + "'";
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
                                    strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  strDwmc= '" + strDwmc +
                                                                                               "'  and strSetName='" + strSetName + 
                                                                                           "'" + "  and strAdd_Dz='" + strAdd_Dz +
                                                                                           "'" + " and   strJyrq = '" + strJyrq + "'" + " and   strArae = '" + strArae + "'";

                                    if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                        iRet = 0;
                                    else
                                        iRet = 1;
                                }
                                return iRet;
                            }

                        }
                        catch { }
                        //   else
                        {
                            #region 字符串
                            //3 插入数据
                            // _Infor.ID = DateTime.Now.ToString("yyMMddHHmmss");
                            #endregion

                            //strSQL = "insert into " + _DbTableName + "  (" + strInsert_Add_Zd + ") values(" + strAdd_Val + ")";
                            //if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                iRet = 0;
                            //else
                            //    iRet = 2;
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
                                strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  strDwmc= '" + strDwmc +
                                                                                           "'  and strSetName='" + strSetName +
                                                                                       "'" + "  and strAdd_Dz='" + strAdd_Dz +
                                                                                       "'" + " and   strJyrq = '" + strJyrq + "'" + 
                                                                                       " and   strArae = '" + strArae + "'";

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
                            //_Infor.ID = DateTime.Now.ToString("yyMMddHHmmss");

                           #endregion

                            //strSQL = "insert into " + _DbTableName + "  (" + strInsert_Add_Zd + ") values(" + strAdd_Val + ")";
                            //if (SqlDbHelper.ExecuteNonQuery(SqlDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                iRet = 0;
                            //else
                            //    iRet = 2;
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
        public int SaveData(ref Class_Test_Item _Infor)
        {
            int iRet = 0;
            string strSQL = "";
            string strAdd_Val = "";//增加Sql值
            string strInsert_Add_Zd = "";//插入字段表
            string strUpdata_Add_Zd = "";//更新字符串
            try
            {
                //1 查询
                //单位名称  受检设备名称  设备地址
                strSQL = "SELECT * from " + _DbTableName + " where strDwmc= '" + _Infor.strDwmc + "'  and strSetName='" + _Infor.strSetName + "'" +
                          "  and strAdd_Dz='" + _Infor.strAdd_Dz + "'" + " and strJyrq='"+ _Infor.strJyrq +"'" + " and strArae='" + _Infor.strArae + "'";

            
                #region 插入字符串
                strInsert_Add_Zd = "ID,strSetName,strZjsb,strAdd_Dz," +
                             "strDwmc,strYt,strAdd_Sheng,strAdd_Shi,strTemperature,strJyrq,strBgrq,strBgbh,strJyy,strHyy,strPhone,strSp," +
                             "strCcbh,strZsbh,strJyyj,strSynx,strYxsyYl," +
                             "strSjdw,strZzdw,strSjYl,strZtcl,strCcrj,strTyrq,strCcJZ," +
                             "iFirstTest,strRegistNo,strEquipmentNo,flNormal_Thickness,flNormal_Thickness_2,flThick_Min,flPosition_Min,flPosition_Min_Y,iChn_Min,flWc_Min,flThick_Max,flPosition_Max,flPosition_Max_Y,iChn_Max,flWc_Max," +
                             "flThick_Average_Max,flWc_Average_Max," +
                             "strMeasure_Type,strMeasure_Number,strMeasure_Precision," +
                             "strQiKuai_Sd,strJt_Sd,strSc_Sd," +
                             "strNum_Limit_Low_Type,strWc_Limit,strWc_Col,strThickAlarm,strMinThickLimit," +
                             "iChnns,iHaveY,strOther_1,strOther_2,strOther_3,strOther_4,strOther_5,ReMark,iThick_Min_Row,iThick_Max_Row,strArae";


                // if (_Infor.ID=="")
                _Infor.ID = _Infor.strJyrq.Replace("_","" );// DateTime.Now.ToString("yyMMddHHmmss");
                strAdd_Val = "'" + _Infor.ID + "'";
                strAdd_Val += ",'" + _Infor.strSetName + "'";
                strAdd_Val += ",'" + _Infor.strZjsb + "'";
                strAdd_Val += ",'" + _Infor.strAdd_Dz + "'";
                strAdd_Val += ",'" + _Infor.strDwmc + "'";
                strAdd_Val += ",'" + _Infor.strYt + "'";
                strAdd_Val += ",'" + _Infor.strAdd_Sheng + "'";
                strAdd_Val += ",'" + _Infor.strAdd_Shi + "'";
                strAdd_Val += ",'" + _Infor.strTemperature + "'";

                strAdd_Val += ",'" + _Infor.strJyrq + "'";
                strAdd_Val += ",'" + _Infor.strBgrq + "'";
                strAdd_Val += ",'" + _Infor.strBgbh + "'";
                strAdd_Val += ",'" + _Infor.strJyy + "'";
                strAdd_Val += ",'" + _Infor.strHyy + "'";
                strAdd_Val += ",'" + _Infor.strPhone + "'";
                strAdd_Val += ",'" + _Infor.strSp + "'";

                strAdd_Val += ",'" + _Infor.strCcbh + "'";
                strAdd_Val += ",'" + _Infor.strZsbh + "'";
                strAdd_Val += ",'" + _Infor.strJyyj + "'";
                strAdd_Val += ",'" + _Infor.strSynx + "'";
                strAdd_Val += ",'" + _Infor.strYxsyYl + "'";

                strAdd_Val += ",'" + _Infor.strSjdw + "'";
                strAdd_Val += ",'" + _Infor.strZzdw + "'";
                strAdd_Val += ",'" + _Infor.strSjYl + "'";
                strAdd_Val += ",'" + _Infor.strZtcl + "'";
                strAdd_Val += ",'" + _Infor.strCcrj + "'";
                strAdd_Val += ",'" + _Infor.strTyrq + "'";
                strAdd_Val += ",'" + _Infor.strCcJZ + "'";

                strAdd_Val += "," + _Infor.iFirstTest;
                strAdd_Val += ",'" + _Infor.strRegistNo + "'";
                strAdd_Val += ",'" + _Infor.strEquipmentNo + "'";
                strAdd_Val += "," + _Infor.flNormal_Thickness;
                strAdd_Val += "," + _Infor.flNormal_Thickness_2;
                strAdd_Val += "," + _Infor.flThick_Min;
                strAdd_Val += "," + _Infor.flPosition_Min;
                strAdd_Val += "," + _Infor.flPosition_Min_Y;
                strAdd_Val += "," + _Infor.iChn_Min;
                strAdd_Val += "," + _Infor.flWc_Min;
                strAdd_Val += "," + _Infor.flThick_Max;
                strAdd_Val += "," + _Infor.flPosition_Max;
                strAdd_Val += "," + _Infor.flPosition_Max_Y;
                strAdd_Val += "," + _Infor.iChn_Max;
                strAdd_Val += "," + _Infor.flWc_Max;

                strAdd_Val += "," + _Infor.flThick_Average_Max;
                strAdd_Val += "," + _Infor.flWc_Average_Max;

                strAdd_Val += ",'" + _Infor.strMeasure_Type + "'";
                strAdd_Val += ",'" + _Infor.strMeasure_Number + "'";
                strAdd_Val += ",'" + _Infor.strMeasure_Precision + "'";

                strAdd_Val += ",'" + _Infor.strQiKuai_Sd + "'";
                strAdd_Val += ",'" + _Infor.strJt_Sd + "'";
                strAdd_Val += ",'" + _Infor.strSc_Sd + "'";

                strAdd_Val += ",'" + _Infor.strNum_Limit_Low_Type + "'";
                strAdd_Val += ",'" + _Infor.strWc_Limit + "'";
                strAdd_Val += ",'" + _Infor.strWc_Col + "'";
                strAdd_Val += ",'" + _Infor.strThickAlarm + "'"; 
                strAdd_Val += ",'" + _Infor.strMinThickLimit + "'";

                strAdd_Val += "," + _Infor.iChnns;
                strAdd_Val += "," + _Infor.iHaveY;

                strAdd_Val += ",'" + _Infor.strOther_1 + "'";
                strAdd_Val += ",'" + _Infor.strOther_2 + "'";
                strAdd_Val += ",'" + _Infor.strOther_3 + "'";
                strAdd_Val += ",'" + _Infor.strOther_4 + "'";
                strAdd_Val += ",'" + _Infor.strOther_5 + "'";
                strAdd_Val += ",'" + _Infor.ReMark + "'";
                strAdd_Val += "," + _Infor.iThick_Min_Row;
                strAdd_Val += "," + _Infor.iThick_Max_Row ;

                strAdd_Val += ",'" + _Infor.strArae + "'";

                #endregion 插入字符串

                #region 更新字符串
                if (_Infor.ID == "")
                {
                    _Infor.ID = DateTime.Now.ToString("yyMMddHHmmss");
                    iRet = 2;
                }
                //3 插入数据
                strUpdata_Add_Zd = "SET ID='" + _Infor.ID + "'";
                strUpdata_Add_Zd += ",strSetName='" + _Infor.strSetName + "'";
                strUpdata_Add_Zd += ",strZjsb='" + _Infor.strZjsb + "'";
                strUpdata_Add_Zd += ",strAdd_Dz='" + _Infor.strAdd_Dz + "'";
                strUpdata_Add_Zd += ",strDwmc='" + _Infor.strDwmc + "'";
                strUpdata_Add_Zd += ",strYt='" + _Infor.strYt + "'";
                strUpdata_Add_Zd += ",strAdd_Sheng='" + _Infor.strAdd_Sheng + "'";
                strUpdata_Add_Zd += ",strAdd_Shi='" + _Infor.strAdd_Shi + "'";
                strUpdata_Add_Zd += ",strTemperature='" + _Infor.strTemperature + "'";

                strUpdata_Add_Zd += ",strJyrq='" + _Infor.strJyrq + "'";
                strUpdata_Add_Zd += ",strBgrq='" + _Infor.strBgrq + "'";
                strUpdata_Add_Zd += ",strBgbh='" + _Infor.strBgbh + "'";
                strUpdata_Add_Zd += ",strJyy='" + _Infor.strJyy + "'";
                strUpdata_Add_Zd += ",strHyy='" + _Infor.strHyy + "'";
                strUpdata_Add_Zd += ",strPhone='" + _Infor.strPhone + "'";
                strUpdata_Add_Zd += ",strSp='" + _Infor.strSp + "'";

                strUpdata_Add_Zd += ",strCcbh='" + _Infor.strCcbh + "'";
                strUpdata_Add_Zd += ",strZsbh='" + _Infor.strZsbh + "'";
                strUpdata_Add_Zd += ",strJyyj='" + _Infor.strJyyj + "'";
                strUpdata_Add_Zd += ",strSynx='" + _Infor.strSynx + "'";
                strUpdata_Add_Zd += ",strYxsyYl='" + _Infor.strYxsyYl + "'";

                strUpdata_Add_Zd += ",strSjdw='" + _Infor.strSjdw + "'";
                strUpdata_Add_Zd += ",strZzdw='" + _Infor.strZzdw + "'";
                strUpdata_Add_Zd += ",strSjYl='" + _Infor.strSjYl + "'";
                strUpdata_Add_Zd += ",strZtcl='" + _Infor.strZtcl + "'";
                strUpdata_Add_Zd += ",strCcrj='" + _Infor.strCcrj + "'";
                strUpdata_Add_Zd += ",strTyrq='" + _Infor.strTyrq + "'";
                strUpdata_Add_Zd += ",strCcJZ='" + _Infor.strCcJZ + "'";

                strUpdata_Add_Zd += ",iFirstTest=" + _Infor.iFirstTest;
                strUpdata_Add_Zd += ",strRegistNo='" + _Infor.strRegistNo + "'";
                strUpdata_Add_Zd += ",strEquipmentNo='" + _Infor.strEquipmentNo + "'";
                strUpdata_Add_Zd += ",flNormal_Thickness=" + _Infor.flNormal_Thickness;
                strUpdata_Add_Zd += ",flNormal_Thickness_2=" + _Infor.flNormal_Thickness_2;
                strUpdata_Add_Zd += ",flThick_Min=" + _Infor.flThick_Min;
                strUpdata_Add_Zd += ",iThick_Min_Row=" + _Infor.iThick_Min_Row ;

                strUpdata_Add_Zd += ",flPosition_Min=" + _Infor.flPosition_Min;
                strUpdata_Add_Zd += ",flPosition_Min_Y=" + _Infor.flPosition_Min_Y;
                strUpdata_Add_Zd += ",iChn_Min=" + _Infor.iChn_Min;
                strUpdata_Add_Zd += ",flWc_Min=" + _Infor.flWc_Min;
                strUpdata_Add_Zd += ",flThick_Max=" + _Infor.flThick_Max;
                strUpdata_Add_Zd += ",flPosition_Max=" + _Infor.flPosition_Max;
                strUpdata_Add_Zd += ",iThick_Max_Row=" + _Infor.iThick_Max_Row;
                strUpdata_Add_Zd += ",flPosition_Max_Y=" + _Infor.flPosition_Max_Y;
                strUpdata_Add_Zd += ",iChn_Max=" + _Infor.iChn_Max;
                strUpdata_Add_Zd += ",flWc_Max=" + _Infor.flWc_Max;

                strUpdata_Add_Zd += ",flThick_Average_Max=" + _Infor.flThick_Average_Max;
                strUpdata_Add_Zd += ",flWc_Average_Max=" + _Infor.flWc_Average_Max;

                strUpdata_Add_Zd += ",strMeasure_Type='" + _Infor.strMeasure_Type + "'";
                strUpdata_Add_Zd += ",strMeasure_Number='" + _Infor.strMeasure_Number + "'";
                strUpdata_Add_Zd += ",strMeasure_Precision='" + _Infor.strMeasure_Precision + "'";

                strUpdata_Add_Zd += ",strQiKuai_Sd='" + _Infor.strQiKuai_Sd + "'";
                strUpdata_Add_Zd += ",strJt_Sd='" + _Infor.strJt_Sd + "'";
                strUpdata_Add_Zd += ",strSc_Sd='" + _Infor.strSc_Sd + "'";

                strUpdata_Add_Zd += ",strNum_Limit_Low_Type='" + _Infor.strNum_Limit_Low_Type + "'";
                strUpdata_Add_Zd += ",strWc_Limit='" + _Infor.strWc_Limit + "'";
                strUpdata_Add_Zd += ",strWc_Col='" + _Infor.strWc_Col + "'";
                strUpdata_Add_Zd += ",strThickAlarm='" + _Infor.strThickAlarm + "'";
                strUpdata_Add_Zd += ",strMinThickLimit='" + _Infor.strMinThickLimit + "'";


                strUpdata_Add_Zd += ",iChnns=" + _Infor.iChnns;
                strUpdata_Add_Zd += ",iHaveY=" + _Infor.iHaveY;

                strUpdata_Add_Zd += ",strOther_1='" + _Infor.strOther_1 + "'";
                strUpdata_Add_Zd += ",strOther_2='" + _Infor.strOther_2 + "'";
                strUpdata_Add_Zd += ",strOther_3='" + _Infor.strOther_3 + "'";
                strUpdata_Add_Zd += ",strOther_4='" + _Infor.strOther_4 + "'";
                strUpdata_Add_Zd += ",strOther_5='" + _Infor.strOther_5 + "'";
                strUpdata_Add_Zd += ",ReMark='" + _Infor.ReMark + "'";

                strUpdata_Add_Zd += ",strArae='" + _Infor.strArae + "'";

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
                                    strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  strDwmc= '" + _Infor.strDwmc +
                                                              "'  and strSetName='" + _Infor.strSetName + "'" +
                                                               "  and strAdd_Dz='" + _Infor.strAdd_Dz + "'" + " and   strJyrq = '" + _Infor.strJyrq + "'" + " and   strArae = '" + _Infor.strArae +"'";

                                    if (OleDbHelper.ExecuteNonQuery(OleDbHelper.DbConnString_DT, CommandType.Text, strSQL, null) == 0)
                                        iRet = 0;
                                    else
                                        iRet = 1;
                                }
                                return iRet;
                            }

                        }
                        catch { }
                     //   else
                        {
                            #region 字符串
                            //3 插入数据
                           // _Infor.ID = DateTime.Now.ToString("yyMMddHHmmss");
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
                                strSQL = "Update " + _DbTableName + " " + strUpdata_Add_Zd + " Where  strDwmc= " + _Infor.strDwmc + 
                                    "  and strSetName='" + _Infor.strSetName + "'" + "  and strAdd_Dz='" + _Infor.strAdd_Dz + "'" + 
                                     " and   strJyrq = '" + _Infor.strJyrq + "'" + " and   strArae = '" + _Infor.strArae + "'";

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
                          //  _Infor.ID = DateTime.Now.ToString("yyMMddHHmmss");

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
            { System.Windows.Forms.MessageBox.Show("Manage_Test_Item.cs的SaveData_Tmperror：" + e.Message + e.StackTrace + " " + strSQL); }

            return iRet;
        }

        /// <summary>
        /// 根据单位名称、受检设备名、设备地址查询
        /// </summary>
        /// <param name="_Infor"></param>
        /// <returns></returns>
        public bool GetData(ref Class_Equipment_Info _Infor)
        {
            bool _blRet = false;
            try
            {
                string strSQL = "SELECT * from " + _DbTableName + " where strDwmc= '" + _Infor.strDwmc + "'" +
                                                                "  and strSetName='" + _Infor.strSetName + "'" +
                                                                "  and strAdd_Dz='" + _Infor.strAdd_Dz + "'" +
                                                                " and strJyrq='" + _Infor.strJyrq + "'" +
                                                                 " and strArae='" + _Infor.strArae + "'";

                
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
                                _Infor.strSetName = DBConvert.ToString(dr["strSetName"]);
                                _Infor.strZjsb = DBConvert.ToString(dr["strZjsb"]);
                                _Infor.strAdd_Dz = DBConvert.ToString(dr["strAdd_Dz"]);
                                _Infor.strDwmc = DBConvert.ToString(dr["strDwmc"]);
                                _Infor.strYt = DBConvert.ToString(dr["strYt"]);
                                _Infor.strAdd_Sheng = DBConvert.ToString(dr["strAdd_Sheng"]);
                                _Infor.strAdd_Shi = DBConvert.ToString(dr["strAdd_Shi"]);
                                _Infor.strTemperature = DBConvert.ToString(dr["strTemperature"]);

                                _Infor.strArae = DBConvert.ToString(dr["strArae"]);

                                _Infor.strJyrq = DBConvert.ToString(dr["strJyrq"]);
                                _Infor.strBgrq = DBConvert.ToString(dr["strBgrq"]);
                                _Infor.strBgbh = DBConvert.ToString(dr["strBgbh"]);
                                _Infor.strJyy = DBConvert.ToString(dr["strJyy"]);
                                _Infor.strHyy = DBConvert.ToString(dr["strHyy"]);
                                _Infor.strPhone = DBConvert.ToString(dr["strPhone"]);
                                _Infor.strSp = DBConvert.ToString(dr["strSp"]);

                                _Infor.strCcbh = DBConvert.ToString(dr["strCcbh"]);
                                _Infor.strZsbh = DBConvert.ToString(dr["strZsbh"]);
                                _Infor.strJyyj = DBConvert.ToString(dr["strJyyj"]);
                                _Infor.strSynx = DBConvert.ToString(dr["strSynx"]);
                                _Infor.strYxsyYl = DBConvert.ToString(dr["strYxsyYl"]);

                                _Infor.strSjdw = DBConvert.ToString(dr["strSjdw"]);
                                _Infor.strZzdw = DBConvert.ToString(dr["strZzdw"]);
                                _Infor.strSjYl = DBConvert.ToString(dr["strSjYl"]);
                                _Infor.strZtcl = DBConvert.ToString(dr["strZtcl"]);
                                _Infor.strCcrj = DBConvert.ToString(dr["strCcrj"]);
                                _Infor.strTyrq = DBConvert.ToString(dr["strTyrq"]);
                                _Infor.strCcJZ = DBConvert.ToString(dr["strCcJZ"]);

                                _Infor.iFirstTest = DBConvert.ToInt32(dr["iFirstTest"]);
                                _Infor.strRegistNo = DBConvert.ToString(dr["strRegistNo"]);
                                _Infor.strEquipmentNo = DBConvert.ToString(dr["strEquipmentNo"]);
                                _Infor.flNormal_Thickness = float.Parse(DBConvert.ToString(dr["flNormal_Thickness"]));
                                try
                                {
                                    _Infor.flNormal_Thickness_2 = float.Parse(DBConvert.ToString(dr["flNormal_Thickness_2"]));
                                }
                                catch { }
                                _Infor.flThick_Min = float.Parse(DBConvert.ToString(dr["flThick_Min"]));
                                _Infor.flPosition_Min = float.Parse(DBConvert.ToString(dr["flPosition_Min"]));
                                try
                                {
                                    _Infor.iThick_Min_Row = DBConvert.ToInt32(dr["iThick_Min_Row"]);
                                }
                                catch { }
                                _Infor.flPosition_Min_Y = float.Parse(DBConvert.ToString(dr["flPosition_Min_Y"]));

                                _Infor.iChn_Min = DBConvert.ToInt32(dr["iChn_Min"]);
                                _Infor.flWc_Min = float.Parse(DBConvert.ToString(dr["flWc_Min"]));
                                _Infor.flThick_Max = float.Parse(DBConvert.ToString(dr["flThick_Max"]));
                                _Infor.flPosition_Max = float.Parse(DBConvert.ToString(dr["flPosition_Max"]));
                                try
                                {
                                    _Infor.iThick_Max_Row = DBConvert.ToInt32(dr["iThick_Max_Row"]);
                                }
                                catch { }
                                _Infor.flPosition_Max_Y = float.Parse(DBConvert.ToString(dr["flPosition_Max_Y"]));

                                _Infor.iChn_Max = DBConvert.ToInt32(dr["iChn_Max"]);
                                _Infor.flWc_Max = float.Parse(DBConvert.ToString(dr["flWc_Max"]));

                                _Infor.flThick_Average_Max = float.Parse(DBConvert.ToString(dr["flThick_Average_Max"]));
                                _Infor.flWc_Average_Max = float.Parse(DBConvert.ToString(dr["flWc_Average_Max"]));

                                _Infor.strMeasure_Type = DBConvert.ToString(dr["strMeasure_Type"]);
                                _Infor.strMeasure_Number = DBConvert.ToString(dr["strMeasure_Number"]);
                                _Infor.strMeasure_Precision = DBConvert.ToString(dr["strMeasure_Precision"]);

                                _Infor.strQiKuai_Sd = DBConvert.ToString(dr["strQiKuai_Sd"]);
                                _Infor.strJt_Sd = DBConvert.ToString(dr["strJt_Sd"]);
                                _Infor.strSc_Sd = DBConvert.ToString(dr["strSc_Sd"]);

                                _Infor.strNum_Limit_Low_Type = DBConvert.ToString(dr["strNum_Limit_Low_Type"]);
                                if (_Infor.strNum_Limit_Low_Type == "") _Infor.strNum_Limit_Low_Type = "100";

                                _Infor.strWc_Limit = DBConvert.ToString(dr["strWc_Limit"]);
                                _Infor.strWc_Col = DBConvert.ToString(dr["strWc_Col"]);
                                _Infor.strThickAlarm = DBConvert.ToString(dr["strThickAlarm"]);
                                _Infor.strMinThickLimit = DBConvert.ToString(dr["strMinThickLimit"]);

                                _Infor.iChnns = DBConvert.ToInt32(dr["iChnns"]);
                                _Infor.iHaveY = DBConvert.ToInt32(dr["iHaveY"]);

                                _Infor.strOther_1 = DBConvert.ToString(dr["strOther_1"]);
                                _Infor.strOther_2 = DBConvert.ToString(dr["strOther_2"]);
                                _Infor.strOther_5 = DBConvert.ToString(dr["strOther_5"]);
                                _Infor.strOther_3 = DBConvert.ToString(dr["strOther_3"])+ DBConvert.ToString(dr["strOther_5"]);
                                _Infor.strOther_4 = DBConvert.ToString(dr["strOther_4"]);
                                
                                _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);

                                try
                                {
                                    _Infor.strArae = DBConvert.ToString(dr["strArae"]);
                                }
                                catch { }
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
                                _Infor.strSetName = DBConvert.ToString(dr["strSetName"]);
                                _Infor.strZjsb = DBConvert.ToString(dr["strZjsb"]);
                                _Infor.strAdd_Dz = DBConvert.ToString(dr["strAdd_Dz"]);
                                _Infor.strDwmc = DBConvert.ToString(dr["strDwmc"]);
                                _Infor.strYt = DBConvert.ToString(dr["strYt"]);
                                _Infor.strAdd_Sheng = DBConvert.ToString(dr["strAdd_Sheng"]);
                                _Infor.strAdd_Shi = DBConvert.ToString(dr["strAdd_Shi"]);
                                _Infor.strTemperature = DBConvert.ToString(dr["strTemperature"]);

                                _Infor.strArae = DBConvert.ToString(dr["strArae"]);

                                _Infor.strJyrq = DBConvert.ToString(dr["strJyrq"]);
                                _Infor.strBgrq = DBConvert.ToString(dr["strBgrq"]);
                                _Infor.strBgbh = DBConvert.ToString(dr["strBgbh"]);
                                _Infor.strJyy = DBConvert.ToString(dr["strJyy"]);
                                _Infor.strHyy = DBConvert.ToString(dr["strHyy"]);
                                _Infor.strPhone = DBConvert.ToString(dr["strPhone"]);
                                _Infor.strSp = DBConvert.ToString(dr["strSp"]);

                                _Infor.strCcbh = DBConvert.ToString(dr["strCcbh"]);
                                _Infor.strZsbh = DBConvert.ToString(dr["strZsbh"]);
                                _Infor.strJyyj = DBConvert.ToString(dr["strJyyj"]);
                                _Infor.strSynx = DBConvert.ToString(dr["strSynx"]);
                                _Infor.strYxsyYl = DBConvert.ToString(dr["strYxsyYl"]);

                                _Infor.strSjdw = DBConvert.ToString(dr["strSjdw"]);
                                _Infor.strZzdw = DBConvert.ToString(dr["strZzdw"]);
                                _Infor.strSjYl = DBConvert.ToString(dr["strSjYl"]);
                                _Infor.strZtcl = DBConvert.ToString(dr["strZtcl"]);
                                _Infor.strCcrj = DBConvert.ToString(dr["strCcrj"]);
                                _Infor.strTyrq = DBConvert.ToString(dr["strTyrq"]);
                                _Infor.strCcJZ = DBConvert.ToString(dr["strCcJZ"]);

                                _Infor.iFirstTest = DBConvert.ToInt32(dr["iFirstTest"]);
                                _Infor.strRegistNo = DBConvert.ToString(dr["strRegistNo"]);
                                _Infor.strEquipmentNo = DBConvert.ToString(dr["strEquipmentNo"]);
                                _Infor.flNormal_Thickness = float.Parse(DBConvert.ToString(dr["flNormal_Thickness"]));
                                try
                                {
                                    _Infor.flNormal_Thickness_2 = float.Parse(DBConvert.ToString(dr["flNormal_Thickness_2"]));
                                }
                                catch { }
                                _Infor.flThick_Min = float.Parse(DBConvert.ToString(dr["flThick_Min"]));
                                _Infor.flPosition_Min = float.Parse(DBConvert.ToString(dr["flPosition_Min"]));
                                try
                                {
                                    _Infor.iThick_Min_Row = DBConvert.ToInt32(dr["iThick_Min_Row"]);
                                }
                                catch { }
                                _Infor.flPosition_Min_Y = float.Parse(DBConvert.ToString(dr["flPosition_Min_Y"]));

                                _Infor.iChn_Min = DBConvert.ToInt32(dr["iChn_Min"]);
                                _Infor.flWc_Min = float.Parse(DBConvert.ToString(dr["flWc_Min"]));
                                _Infor.flThick_Max = float.Parse(DBConvert.ToString(dr["flThick_Max"]));
                                _Infor.flPosition_Max = float.Parse(DBConvert.ToString(dr["flPosition_Max"]));
                                try
                                {
                                    _Infor.iThick_Max_Row = DBConvert.ToInt32(dr["iThick_Max_Row"]);
                                }
                                catch { }
                                _Infor.flPosition_Max_Y = float.Parse(DBConvert.ToString(dr["flPosition_Max_Y"]));

                                _Infor.iChn_Max = DBConvert.ToInt32(dr["iChn_Max"]);
                                _Infor.flWc_Max = float.Parse(DBConvert.ToString(dr["flWc_Max"]));

                                _Infor.flThick_Average_Max = float.Parse(DBConvert.ToString(dr["flThick_Average_Max"]));
                                _Infor.flWc_Average_Max = float.Parse(DBConvert.ToString(dr["flWc_Average_Max"]));

                                _Infor.strMeasure_Type = DBConvert.ToString(dr["strMeasure_Type"]);
                                _Infor.strMeasure_Number = DBConvert.ToString(dr["strMeasure_Number"]);
                                _Infor.strMeasure_Precision = DBConvert.ToString(dr["strMeasure_Precision"]);

                                _Infor.strQiKuai_Sd = DBConvert.ToString(dr["strQiKuai_Sd"]);
                                _Infor.strJt_Sd = DBConvert.ToString(dr["strJt_Sd"]);
                                _Infor.strSc_Sd = DBConvert.ToString(dr["strSc_Sd"]);

                                _Infor.strNum_Limit_Low_Type = DBConvert.ToString(dr["strNum_Limit_Low_Type"]);
                                if (_Infor.strNum_Limit_Low_Type == "") _Infor.strNum_Limit_Low_Type = "100";

                                _Infor.strWc_Limit = DBConvert.ToString(dr["strWc_Limit"]);
                                _Infor.strWc_Col = DBConvert.ToString(dr["strWc_Col"]);
                                _Infor.strThickAlarm = DBConvert.ToString(dr["strThickAlarm"]);
                                _Infor.strMinThickLimit = DBConvert.ToString(dr["strMinThickLimit"]);

                                _Infor.iChnns = DBConvert.ToInt32(dr["iChnns"]);
                                _Infor.iHaveY = DBConvert.ToInt32(dr["iHaveY"]);

                                _Infor.strOther_1 = DBConvert.ToString(dr["strOther_1"]);
                                _Infor.strOther_2 = DBConvert.ToString(dr["strOther_2"]);
                                _Infor.strOther_3 = DBConvert.ToString(dr["strOther_3"]) + DBConvert.ToString(dr["strOther_5"]);
                                _Infor.strOther_4 = DBConvert.ToString(dr["strOther_4"]);
                                _Infor.strOther_5 = DBConvert.ToString(dr["strOther_5"]);
                                _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);

                                try
                                {
                                    _Infor.strArae = DBConvert.ToString(dr["strArae"]);
                                }
                                catch { }
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
        public List<Class_Equipment_Info> GetData(string strWhere)
        {
            List<Class_Equipment_Info> _lst_Info = new List<Struct.Class_Equipment_Info>();
            string strSQL = "SELECT * from " + _DbTableName + " where " + strWhere;
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
                            Class_Equipment_Info _Infor = new Class_Equipment_Info();
                            #region 字段赋值
                            _Infor.ID = DBConvert.ToString(dr["ID"]);
                            _Infor.strSetName = DBConvert.ToString(dr["strSetName"]);
                            _Infor.strZjsb = DBConvert.ToString(dr["strZjsb"]);
                            _Infor.strAdd_Dz = DBConvert.ToString(dr["strAdd_Dz"]);
                            _Infor.strDwmc = DBConvert.ToString(dr["strDwmc"]);
                            _Infor.strYt = DBConvert.ToString(dr["strYt"]);
                            _Infor.strAdd_Sheng = DBConvert.ToString(dr["strAdd_Sheng"]);
                            _Infor.strAdd_Shi = DBConvert.ToString(dr["strAdd_Shi"]);
                            _Infor.strTemperature = DBConvert.ToString(dr["strTemperature"]);

                            _Infor.strArae = DBConvert.ToString(dr["strArae"]);
                            //strArae
                            _Infor.strJyrq = DBConvert.ToString(dr["strJyrq"]);
                            _Infor.strBgrq = DBConvert.ToString(dr["strBgrq"]);
                            _Infor.strBgbh = DBConvert.ToString(dr["strBgbh"]);
                            _Infor.strJyy = DBConvert.ToString(dr["strJyy"]);
                            _Infor.strHyy = DBConvert.ToString(dr["strHyy"]);
                            _Infor.strPhone = DBConvert.ToString(dr["strPhone"]);
                            _Infor.strSp = DBConvert.ToString(dr["strSp"]);

                            _Infor.strCcbh = DBConvert.ToString(dr["strCcbh"]);
                            _Infor.strZsbh = DBConvert.ToString(dr["strZsbh"]);
                            _Infor.strJyyj = DBConvert.ToString(dr["strJyyj"]);
                            _Infor.strSynx = DBConvert.ToString(dr["strSynx"]);
                            _Infor.strYxsyYl = DBConvert.ToString(dr["strYxsyYl"]);

                            _Infor.strSjdw = DBConvert.ToString(dr["strSjdw"]);
                            _Infor.strZzdw = DBConvert.ToString(dr["strZzdw"]);
                            _Infor.strSjYl = DBConvert.ToString(dr["strSjYl"]);
                            _Infor.strZtcl = DBConvert.ToString(dr["strZtcl"]);
                            _Infor.strCcrj = DBConvert.ToString(dr["strCcrj"]);
                            _Infor.strTyrq = DBConvert.ToString(dr["strTyrq"]);
                            _Infor.strCcJZ = DBConvert.ToString(dr["strCcJZ"]);

                            _Infor.iFirstTest = DBConvert.ToInt32(dr["iFirstTest"]);
                            _Infor.strRegistNo = DBConvert.ToString(dr["strRegistNo"]);
                            _Infor.strEquipmentNo = DBConvert.ToString(dr["strEquipmentNo"]);
                            _Infor.flNormal_Thickness = float.Parse(DBConvert.ToString(dr["flNormal_Thickness"]));
                            try
                            {
                                _Infor.flNormal_Thickness_2 = float.Parse(DBConvert.ToString(dr["flNormal_Thickness_2"]));
                            }
                            catch { }
                            _Infor.flThick_Min = float.Parse(DBConvert.ToString(dr["flThick_Min"]));
                            _Infor.flPosition_Min = float.Parse(DBConvert.ToString(dr["flPosition_Min"]));
                            try
                            {
                                _Infor.iThick_Min_Row = DBConvert.ToInt32(dr["iThick_Min_Row"]);
                            }
                            catch { }
                            _Infor.flPosition_Min_Y = float.Parse(DBConvert.ToString(dr["flPosition_Min_Y"]));
                            _Infor.iChn_Min = DBConvert.ToInt32(dr["iChn_Min"]);
                            _Infor.flWc_Min = float.Parse(DBConvert.ToString(dr["flWc_Min"]));
                            _Infor.flThick_Max = float.Parse(DBConvert.ToString(dr["flThick_Max"]));
                            _Infor.flPosition_Max = float.Parse(DBConvert.ToString(dr["flPosition_Max"]));
                            try
                            {
                                _Infor.iThick_Max_Row = DBConvert.ToInt32(dr["iThick_Max_Row"]);
                            }
                            catch { }
                            _Infor.flPosition_Max_Y = float.Parse(DBConvert.ToString(dr["flPosition_Max_Y"]));
                            _Infor.iChn_Max = DBConvert.ToInt32(dr["iChn_Max"]);
                            _Infor.flWc_Max = float.Parse(DBConvert.ToString(dr["flWc_Max"]));

                            _Infor.flThick_Average_Max = float.Parse(DBConvert.ToString(dr["flThick_Average_Max"]));
                            _Infor.flWc_Average_Max = float.Parse(DBConvert.ToString(dr["flWc_Average_Max"]));

                            _Infor.strMeasure_Type = DBConvert.ToString(dr["strMeasure_Type"]);
                            _Infor.strMeasure_Number = DBConvert.ToString(dr["strMeasure_Number"]);
                            _Infor.strMeasure_Precision = DBConvert.ToString(dr["strMeasure_Precision"]);

                            _Infor.strQiKuai_Sd = DBConvert.ToString(dr["strQiKuai_Sd"]);
                            _Infor.strJt_Sd = DBConvert.ToString(dr["strJt_Sd"]);
                            _Infor.strSc_Sd = DBConvert.ToString(dr["strSc_Sd"]);

                            _Infor.strNum_Limit_Low_Type = DBConvert.ToString(dr["strNum_Limit_Low_Type"]);
                            if (_Infor.strNum_Limit_Low_Type == "") _Infor.strNum_Limit_Low_Type = "100";
                            _Infor.strWc_Limit = DBConvert.ToString(dr["strWc_Limit"]);
                            _Infor.strWc_Col = DBConvert.ToString(dr["strWc_Col"]);
                            _Infor.strThickAlarm = DBConvert.ToString(dr["strThickAlarm"]);
                            _Infor.strMinThickLimit = DBConvert.ToString(dr["strMinThickLimit"]);

                            _Infor.iChnns = DBConvert.ToInt32(dr["iChnns"]);
                            _Infor.iHaveY = DBConvert.ToInt32(dr["iHaveY"]);

                            _Infor.strOther_1 = DBConvert.ToString(dr["strOther_1"]);
                            _Infor.strOther_2 = DBConvert.ToString(dr["strOther_2"]);
                            _Infor.strOther_3 = DBConvert.ToString(dr["strOther_3"]) + DBConvert.ToString(dr["strOther_5"]);
                            _Infor.strOther_4 = DBConvert.ToString(dr["strOther_4"]);
                            _Infor.strOther_5 = DBConvert.ToString(dr["strOther_5"]);
                            _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);

                            try
                            {
                                _Infor.strArae = DBConvert.ToString(dr["strArae"]);
                            }
                            catch { }
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
                            Class_Equipment_Info _Infor = new Class_Equipment_Info();
                            #region 字段赋值
                            _Infor.ID = DBConvert.ToString(dr["ID"]);
                            _Infor.strSetName = DBConvert.ToString(dr["strSetName"]);
                            _Infor.strZjsb = DBConvert.ToString(dr["strZjsb"]);
                            _Infor.strAdd_Dz = DBConvert.ToString(dr["strAdd_Dz"]);
                            _Infor.strDwmc = DBConvert.ToString(dr["strDwmc"]);
                            _Infor.strYt = DBConvert.ToString(dr["strYt"]);
                            _Infor.strAdd_Sheng = DBConvert.ToString(dr["strAdd_Sheng"]);
                            _Infor.strAdd_Shi = DBConvert.ToString(dr["strAdd_Shi"]);
                            _Infor.strTemperature = DBConvert.ToString(dr["strTemperature"]);

                            _Infor.strArae = DBConvert.ToString(dr["strArae"]);

                            _Infor.strJyrq = DBConvert.ToString(dr["strJyrq"]);
                            _Infor.strBgrq = DBConvert.ToString(dr["strBgrq"]);
                            _Infor.strBgbh = DBConvert.ToString(dr["strBgbh"]);
                            _Infor.strJyy = DBConvert.ToString(dr["strJyy"]);
                            _Infor.strHyy = DBConvert.ToString(dr["strHyy"]);
                            _Infor.strPhone = DBConvert.ToString(dr["strPhone"]);
                            _Infor.strSp = DBConvert.ToString(dr["strSp"]);

                            _Infor.strCcbh = DBConvert.ToString(dr["strCcbh"]);
                            _Infor.strZsbh = DBConvert.ToString(dr["strZsbh"]);
                            _Infor.strJyyj = DBConvert.ToString(dr["strJyyj"]);
                            _Infor.strSynx = DBConvert.ToString(dr["strSynx"]);
                            _Infor.strYxsyYl = DBConvert.ToString(dr["strYxsyYl"]);

                            _Infor.strSjdw = DBConvert.ToString(dr["strSjdw"]);
                            _Infor.strZzdw = DBConvert.ToString(dr["strZzdw"]);
                            _Infor.strSjYl = DBConvert.ToString(dr["strSjYl"]);
                            _Infor.strZtcl = DBConvert.ToString(dr["strZtcl"]);
                            _Infor.strCcrj = DBConvert.ToString(dr["strCcrj"]);
                            _Infor.strTyrq = DBConvert.ToString(dr["strTyrq"]);
                            _Infor.strCcJZ = DBConvert.ToString(dr["strCcJZ"]);

                            _Infor.iFirstTest = DBConvert.ToInt32(dr["iFirstTest"]);
                            _Infor.strRegistNo = DBConvert.ToString(dr["strRegistNo"]);
                            _Infor.strEquipmentNo = DBConvert.ToString(dr["strEquipmentNo"]);
                            _Infor.flNormal_Thickness = float.Parse(DBConvert.ToString(dr["flNormal_Thickness"]));
                            try
                            {
                                _Infor.flNormal_Thickness_2 = float.Parse(DBConvert.ToString(dr["flNormal_Thickness_2"]));
                            }
                            catch { }
                            _Infor.flThick_Min = float.Parse(DBConvert.ToString(dr["flThick_Min"]));
                            _Infor.flPosition_Min = float.Parse(DBConvert.ToString(dr["flPosition_Min"]));
                            try
                            {
                                _Infor.iThick_Min_Row = DBConvert.ToInt32(dr["iThick_Min_Row"]);
                            }
                            catch { }
                            _Infor.flPosition_Min_Y = float.Parse(DBConvert.ToString(dr["flPosition_Min_Y"]));
                            _Infor.iChn_Min = DBConvert.ToInt32(dr["iChn_Min"]);
                            _Infor.flWc_Min = float.Parse(DBConvert.ToString(dr["flWc_Min"]));
                            _Infor.flThick_Max = float.Parse(DBConvert.ToString(dr["flThick_Max"]));
                            _Infor.flPosition_Max = float.Parse(DBConvert.ToString(dr["flPosition_Max"]));
                            try
                            {
                                _Infor.iThick_Max_Row = DBConvert.ToInt32(dr["iThick_Max_Row"]);
                            }
                            catch { }
                            _Infor.flPosition_Max_Y = float.Parse(DBConvert.ToString(dr["flPosition_Max_Y"]));
                            _Infor.iChn_Max = DBConvert.ToInt32(dr["iChn_Max"]);
                            _Infor.flWc_Max = float.Parse(DBConvert.ToString(dr["flWc_Max"]));

                            _Infor.flThick_Average_Max = float.Parse(DBConvert.ToString(dr["flThick_Average_Max"]));
                            _Infor.flWc_Average_Max = float.Parse(DBConvert.ToString(dr["flWc_Average_Max"]));

                            _Infor.strMeasure_Type = DBConvert.ToString(dr["strMeasure_Type"]);
                            _Infor.strMeasure_Number = DBConvert.ToString(dr["strMeasure_Number"]);
                            _Infor.strMeasure_Precision = DBConvert.ToString(dr["strMeasure_Precision"]);

                            _Infor.strQiKuai_Sd = DBConvert.ToString(dr["strQiKuai_Sd"]);
                            _Infor.strJt_Sd = DBConvert.ToString(dr["strJt_Sd"]);
                            _Infor.strSc_Sd = DBConvert.ToString(dr["strSc_Sd"]);

                            _Infor.strNum_Limit_Low_Type = DBConvert.ToString(dr["strNum_Limit_Low_Type"]);
                            if (_Infor.strNum_Limit_Low_Type == "") _Infor.strNum_Limit_Low_Type = "100";
                            _Infor.strWc_Limit = DBConvert.ToString(dr["strWc_Limit"]);
                            _Infor.strWc_Col = DBConvert.ToString(dr["strWc_Col"]);
                            _Infor.strThickAlarm = DBConvert.ToString(dr["strThickAlarm"]);
                            _Infor.strMinThickLimit = DBConvert.ToString(dr["strMinThickLimit"]);

                            _Infor.iChnns = DBConvert.ToInt32(dr["iChnns"]);
                            _Infor.iHaveY = DBConvert.ToInt32(dr["iHaveY"]);

                            _Infor.strOther_1 = DBConvert.ToString(dr["strOther_1"]);
                            _Infor.strOther_2 = DBConvert.ToString(dr["strOther_2"]);
                            _Infor.strOther_3 = DBConvert.ToString(dr["strOther_3"]) + DBConvert.ToString(dr["strOther_5"]);
                            _Infor.strOther_4 = DBConvert.ToString(dr["strOther_4"]);
                            _Infor.strOther_5 = DBConvert.ToString(dr["strOther_5"]);
                            _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);

                            try
                            {
                                _Infor.strArae = DBConvert.ToString(dr["strArae"]);
                            }
                            catch { }
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
        public Class_Equipment_Info GetData_One(string strSourcePathFileName)
        {
            string strOld_DT = "";
            Class_Equipment_Info _Infor = new Class_Equipment_Info();
            string strSQL = "SELECT * from " + _DbTableName +" Where 1=1";
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
                            _Infor.strSetName = DBConvert.ToString(dr["strSetName"]);
                            _Infor.strZjsb = DBConvert.ToString(dr["strZjsb"]);
                            _Infor.strAdd_Dz = DBConvert.ToString(dr["strAdd_Dz"]);
                            _Infor.strDwmc = DBConvert.ToString(dr["strDwmc"]);
                            _Infor.strYt = DBConvert.ToString(dr["strYt"]);
                            _Infor.strAdd_Sheng = DBConvert.ToString(dr["strAdd_Sheng"]);
                            _Infor.strAdd_Shi = DBConvert.ToString(dr["strAdd_Shi"]);
                            _Infor.strTemperature = DBConvert.ToString(dr["strTemperature"]);

                            _Infor.strArae = DBConvert.ToString(dr["strArae"]);

                            _Infor.strJyrq = DBConvert.ToString(dr["strJyrq"]);
                            _Infor.strBgrq = DBConvert.ToString(dr["strBgrq"]);
                            _Infor.strBgbh = DBConvert.ToString(dr["strBgbh"]);
                            _Infor.strJyy = DBConvert.ToString(dr["strJyy"]);
                            _Infor.strHyy = DBConvert.ToString(dr["strHyy"]);
                            _Infor.strPhone = DBConvert.ToString(dr["strPhone"]);
                            _Infor.strSp = DBConvert.ToString(dr["strSp"]);

                            _Infor.strCcbh = DBConvert.ToString(dr["strCcbh"]);
                            _Infor.strZsbh = DBConvert.ToString(dr["strZsbh"]);
                            _Infor.strJyyj = DBConvert.ToString(dr["strJyyj"]);
                            _Infor.strSynx = DBConvert.ToString(dr["strSynx"]);
                            _Infor.strYxsyYl = DBConvert.ToString(dr["strYxsyYl"]);

                            _Infor.strSjdw = DBConvert.ToString(dr["strSjdw"]);
                            _Infor.strZzdw = DBConvert.ToString(dr["strZzdw"]);
                            _Infor.strSjYl = DBConvert.ToString(dr["strSjYl"]);
                            _Infor.strZtcl = DBConvert.ToString(dr["strZtcl"]);
                            _Infor.strCcrj = DBConvert.ToString(dr["strCcrj"]);
                            _Infor.strTyrq = DBConvert.ToString(dr["strTyrq"]);
                            _Infor.strCcJZ = DBConvert.ToString(dr["strCcJZ"]);

                            _Infor.iFirstTest = DBConvert.ToInt32(dr["iFirstTest"]);
                            _Infor.strRegistNo = DBConvert.ToString(dr["strRegistNo"]);
                            _Infor.strEquipmentNo = DBConvert.ToString(dr["strEquipmentNo"]);
                            _Infor.flNormal_Thickness = float.Parse(DBConvert.ToString(dr["flNormal_Thickness"]));
                            try
                            {
                                _Infor.flNormal_Thickness_2 = float.Parse(DBConvert.ToString(dr["flNormal_Thickness_2"]));
                            }
                            catch { }
                            _Infor.flThick_Min = float.Parse(DBConvert.ToString(dr["flThick_Min"]));
                            _Infor.flPosition_Min = float.Parse(DBConvert.ToString(dr["flPosition_Min"]));
                            try
                            {
                                _Infor.iThick_Min_Row = DBConvert.ToInt32(dr["iThick_Min_Row"]);
                            }
                            catch { }
                            _Infor.flPosition_Min_Y = float.Parse(DBConvert.ToString(dr["flPosition_Min_Y"]));
                            _Infor.iChn_Min = DBConvert.ToInt32(dr["iChn_Min"]);
                            _Infor.flWc_Min = float.Parse(DBConvert.ToString(dr["flWc_Min"]));
                            _Infor.flThick_Max = float.Parse(DBConvert.ToString(dr["flThick_Max"]));
                            _Infor.flPosition_Max = float.Parse(DBConvert.ToString(dr["flPosition_Max"]));
                            _Infor.flPosition_Max_Y = float.Parse(DBConvert.ToString(dr["flPosition_Max_Y"]));
                            try
                            { 
                            _Infor.iThick_Max_Row = DBConvert.ToInt32(dr["iThick_Max_Row"]);
                            }
                            catch { }
                            _Infor.iChn_Max = DBConvert.ToInt32(dr["iChn_Max"]);
                            _Infor.flWc_Max = float.Parse(DBConvert.ToString(dr["flWc_Max"]));

                            _Infor.flThick_Average_Max = float.Parse(DBConvert.ToString(dr["flThick_Average_Max"]));
                            _Infor.flWc_Average_Max = float.Parse(DBConvert.ToString(dr["flWc_Average_Max"]));

                            _Infor.strMeasure_Type = DBConvert.ToString(dr["strMeasure_Type"]);
                            _Infor.strMeasure_Number = DBConvert.ToString(dr["strMeasure_Number"]);
                            _Infor.strMeasure_Precision = DBConvert.ToString(dr["strMeasure_Precision"]);

                            _Infor.strQiKuai_Sd = DBConvert.ToString(dr["strQiKuai_Sd"]);
                            _Infor.strJt_Sd = DBConvert.ToString(dr["strJt_Sd"]);
                            _Infor.strSc_Sd = DBConvert.ToString(dr["strSc_Sd"]);

                            _Infor.strNum_Limit_Low_Type = DBConvert.ToString(dr["strNum_Limit_Low_Type"]);
                            if (_Infor.strNum_Limit_Low_Type == "") _Infor.strNum_Limit_Low_Type = "100";
                            _Infor.strWc_Limit = DBConvert.ToString(dr["strWc_Limit"]);
                            _Infor.strWc_Col = DBConvert.ToString(dr["strWc_Col"]);
                            _Infor.strThickAlarm = DBConvert.ToString(dr["strThickAlarm"]);
                            _Infor.strMinThickLimit = DBConvert.ToString(dr["strMinThickLimit"]);

                            _Infor.iChnns = DBConvert.ToInt32(dr["iChnns"]);
                            _Infor.iHaveY = DBConvert.ToInt32(dr["iHaveY"]);

                            _Infor.strOther_1 = DBConvert.ToString(dr["strOther_1"]);
                            _Infor.strOther_2 = DBConvert.ToString(dr["strOther_2"]);
                            _Infor.strOther_3 = DBConvert.ToString(dr["strOther_3"]) + DBConvert.ToString(dr["strOther_5"]);
                            _Infor.strOther_4 = DBConvert.ToString(dr["strOther_4"]);
                            _Infor.strOther_5 = DBConvert.ToString(dr["strOther_5"]);
                            _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);

                            try
                            {
                                _Infor.strArae = DBConvert.ToString(dr["strArae"]);
                            }
                            catch { }
                            #endregion 字段赋值
                            break;
                        }
                    }
                    #endregion 0
                    OleDbHelper.DbConnString_DT= strOld_DT;
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
                            _Infor.strSetName = DBConvert.ToString(dr["strSetName"]);
                            _Infor.strZjsb = DBConvert.ToString(dr["strZjsb"]);
                            _Infor.strAdd_Dz = DBConvert.ToString(dr["strAdd_Dz"]);
                            _Infor.strDwmc = DBConvert.ToString(dr["strDwmc"]);
                            _Infor.strYt = DBConvert.ToString(dr["strYt"]);
                            _Infor.strAdd_Sheng = DBConvert.ToString(dr["strAdd_Sheng"]);
                            _Infor.strAdd_Shi = DBConvert.ToString(dr["strAdd_Shi"]);
                            _Infor.strTemperature = DBConvert.ToString(dr["strTemperature"]);

                            _Infor.strArae = DBConvert.ToString(dr["strArae"]);

                            _Infor.strJyrq = DBConvert.ToString(dr["strJyrq"]);
                            _Infor.strBgrq = DBConvert.ToString(dr["strBgrq"]);
                            _Infor.strBgbh = DBConvert.ToString(dr["strBgbh"]);
                            _Infor.strJyy = DBConvert.ToString(dr["strJyy"]);
                            _Infor.strHyy = DBConvert.ToString(dr["strHyy"]);
                            _Infor.strPhone = DBConvert.ToString(dr["strPhone"]);
                            _Infor.strSp = DBConvert.ToString(dr["strSp"]);

                            _Infor.strCcbh = DBConvert.ToString(dr["strCcbh"]);
                            _Infor.strZsbh = DBConvert.ToString(dr["strZsbh"]);
                            _Infor.strJyyj = DBConvert.ToString(dr["strJyyj"]);
                            _Infor.strSynx = DBConvert.ToString(dr["strSynx"]);
                            _Infor.strYxsyYl = DBConvert.ToString(dr["strYxsyYl"]);

                            _Infor.strSjdw = DBConvert.ToString(dr["strSjdw"]);
                            _Infor.strZzdw = DBConvert.ToString(dr["strZzdw"]);
                            _Infor.strSjYl = DBConvert.ToString(dr["strSjYl"]);
                            _Infor.strZtcl = DBConvert.ToString(dr["strZtcl"]);
                            _Infor.strCcrj = DBConvert.ToString(dr["strCcrj"]);
                            _Infor.strTyrq = DBConvert.ToString(dr["strTyrq"]);
                            _Infor.strCcJZ = DBConvert.ToString(dr["strCcJZ"]);

                            _Infor.iFirstTest = DBConvert.ToInt32(dr["iFirstTest"]);
                            _Infor.strRegistNo = DBConvert.ToString(dr["strRegistNo"]);
                            _Infor.strEquipmentNo = DBConvert.ToString(dr["strEquipmentNo"]);
                            _Infor.flNormal_Thickness = float.Parse(DBConvert.ToString(dr["flNormal_Thickness"]));
                            try
                            {
                                _Infor.flNormal_Thickness_2 = float.Parse(DBConvert.ToString(dr["flNormal_Thickness_2"]));
                            }
                            catch { }
                            _Infor.flThick_Min = float.Parse(DBConvert.ToString(dr["flThick_Min"]));
                            _Infor.flPosition_Min = float.Parse(DBConvert.ToString(dr["flPosition_Min"]));
                            try
                            {
                                _Infor.iThick_Min_Row = DBConvert.ToInt32(dr["iThick_Min_Row"]);
                            }
                            catch { }
                            _Infor.flPosition_Min_Y = float.Parse(DBConvert.ToString(dr["flPosition_Min_Y"]));
                            _Infor.iChn_Min = DBConvert.ToInt32(dr["iChn_Min"]);
                            _Infor.flWc_Min = float.Parse(DBConvert.ToString(dr["flWc_Min"]));
                            _Infor.flThick_Max = float.Parse(DBConvert.ToString(dr["flThick_Max"]));
                            _Infor.flPosition_Max = float.Parse(DBConvert.ToString(dr["flPosition_Max"]));
                            _Infor.flPosition_Max_Y = float.Parse(DBConvert.ToString(dr["flPosition_Max_Y"]));
                            try
                            {
                                _Infor.iThick_Max_Row = DBConvert.ToInt32(dr["iThick_Max_Row"]);
                            }
                            catch { }
                            _Infor.iChn_Max = DBConvert.ToInt32(dr["iChn_Max"]);
                            _Infor.flWc_Max = float.Parse(DBConvert.ToString(dr["flWc_Max"]));

                            _Infor.flThick_Average_Max = float.Parse(DBConvert.ToString(dr["flThick_Average_Max"]));
                            _Infor.flWc_Average_Max = float.Parse(DBConvert.ToString(dr["flWc_Average_Max"]));

                            _Infor.strMeasure_Type = DBConvert.ToString(dr["strMeasure_Type"]);
                            _Infor.strMeasure_Number = DBConvert.ToString(dr["strMeasure_Number"]);
                            _Infor.strMeasure_Precision = DBConvert.ToString(dr["strMeasure_Precision"]);

                            _Infor.strQiKuai_Sd = DBConvert.ToString(dr["strQiKuai_Sd"]);
                            _Infor.strJt_Sd = DBConvert.ToString(dr["strJt_Sd"]);
                            _Infor.strSc_Sd = DBConvert.ToString(dr["strSc_Sd"]);

                            _Infor.strNum_Limit_Low_Type = DBConvert.ToString(dr["strNum_Limit_Low_Type"]);
                            if (_Infor.strNum_Limit_Low_Type == "") _Infor.strNum_Limit_Low_Type = "100";

                            _Infor.strWc_Limit = DBConvert.ToString(dr["strWc_Limit"]);
                            _Infor.strWc_Col = DBConvert.ToString(dr["strWc_Col"]);
                            _Infor.strThickAlarm = DBConvert.ToString(dr["strThickAlarm"]);
                            _Infor.strMinThickLimit = DBConvert.ToString(dr["strMinThickLimit"]);

                            _Infor.iChnns = DBConvert.ToInt32(dr["iChnns"]);
                            _Infor.iHaveY = DBConvert.ToInt32(dr["iHaveY"]);

                            _Infor.strOther_1 = DBConvert.ToString(dr["strOther_1"]);
                            _Infor.strOther_2 = DBConvert.ToString(dr["strOther_2"]);
                            _Infor.strOther_3 = DBConvert.ToString(dr["strOther_3"]) + DBConvert.ToString(dr["strOther_5"]);
                            _Infor.strOther_4 = DBConvert.ToString(dr["strOther_4"]);
                            _Infor.strOther_5 = DBConvert.ToString(dr["strOther_5"]);
                            _Infor.ReMark = DBConvert.ToString(dr["ReMark"]);

                            try
                            {
                                _Infor.strArae = DBConvert.ToString(dr["strArae"]);
                            }
                            catch { }
                            #endregion 字段赋值

                            break;
                        }
                    }
                    #endregion 1 
                    SqlDbHelper.DbConnString_DT= strOld_DT;
                    break;
            }
            return _Infor;
        }
        /// <summary>
        /// 获得字段值集合
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public List<string > GetData_ByMySel(string strName)
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
                            _lst_Info.Add(strDat );
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
        public List<string> GetData_ByMySel_Where(string strName,string strZdName,string strVal)
        {
            List<string> _lst_Info = new List<string>();
            string strSQL = "SELECT DISTINCT " + strName + " from " + _DbTableName + " where " + strZdName + " = '"+ strVal +"'";
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
