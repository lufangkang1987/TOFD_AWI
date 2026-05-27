/*文档说明
 * Copyright(C) 2 2017 郑州金润高科电子有限公司
 * 文件名: ClassHt200.cs
 * 文件功能描述: 报表输出函数库
 * 目的：报表
 * 创建标识: 陈大伟 2017-12
 * 修改标识: 
 * 修改描述:
 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using clsExcel;
using System.Drawing;

using System.Windows.Forms;


using ClassLib_TestData;

namespace ReportDLL
{
    public class ClsReport
    {
        #region 变量
        /// <summary>
        /// 打印进行进度
        /// </summary>
        public int m_iPrintRunState = 0;
        /// <summary>
        /// 退出系统
        /// </summary>
        public   bool m_blOut = false;
        /// <summary>
        /// 输出原始数据按照间隔输出
        /// </summary>
        public bool m_bl_Print_ByJG = false;
        /// <summary>
        /// EXCEL文件缓存
        /// </summary>
        public     csExcel m_MyExcel = new csExcel();
        /// <summary>
        /// 模板文件路径名
        /// </summary>
        /// <summary>
        /// 生成报表路径文件名
        private string m_strResult_Rpt_FileName = "";
        #endregion 变量

        #region 方法

        /// <summary>
        /// 读美国电磁超声Excel
        /// </summary>
        /// <param name="strExcel_FileName">不带后缀</param>
        /// <param name="strCols"></param>
        /// <returns></returns>
        public List<float[]> ReadExcel_C_R(int iMaxCol, int iJg, string strExcel_FileName)
        {
            List<float[]> _RetList_Out = new List<float[]>();//多行数据
            List<float> _RetList = new List<float>();//单列数据
            m_MyExcel = new csExcel();
            int iRows = 0;//总行数
            bool blAddDat = false;//是否添加数据了
            int iRow_No_Old = -1;//上一行
            int iRow_No = 0;//行号 0开始
            int iCol_No = 0;//列号 0开始
            float flData = 0;//厚度数据

            string[] _sPara = strExcel_FileName.Split('.');
            _sPara = _sPara[0].Split('\\');
            string SheetNane = _sPara[_sPara.Length -1];
            if (SheetNane == "") return _RetList_Out;
            SheetNane = "Sheet1";
            int iArrCout = (int)(iMaxCol / iJg)+1;
            float[] ArrData = new float[iArrCout];//每行的固定数据,根据列号将数据填写在指定列的位置

            //0 数据检查
            if (strExcel_FileName == "") return _RetList_Out;
            try
            {
                //1 读模板文件
                m_MyExcel.CreateNewDocument(strExcel_FileName);
                iRows = m_MyExcel.GetRows(SheetNane);//2443
           //     iRows = 200;
                for (int iRow = 2; iRow < iRows; iRow++)
                {
                    //1 行号、列号、厚度值           
                    iCol_No = int.Parse(m_MyExcel.ReadData(iRow, 4, SheetNane)) / iJg;//列号 0-M
                    iRow_No = int.Parse(m_MyExcel.ReadData(iRow, 5, SheetNane)) / iJg - 1;//行号 1-N
                    flData = float.Parse(m_MyExcel.ReadData(iRow, 6, SheetNane));//厚度数据
                    Application.DoEvents();                                                         
                    //2 换行跟新缓存
                    if (iRow_No_Old != iRow_No)
                    {
                        //3 如果添加数据了就将数据添加到_RetList_Out
                        if (blAddDat)
                            _RetList_Out.Add(ArrData);
                        //4 清空缓存
                        ArrData = new float[iArrCout];
                        blAddDat = false;
                        iRow_No_Old= iRow_No;
                    }
                    //5 符合条件将数据添加到列缓存
                    if (flData > 0 && iCol_No > -1 && iCol_No < iArrCout && iRow_No > -1)
                    {
                        ArrData[iCol_No] = flData;
                        blAddDat = true;
                    }
                }
            }
            catch (Exception e)
            { MessageBox.Show(e.Message); }
            m_MyExcel.Close();
            return _RetList_Out;
        }
        public bool  Set_Ysjl_Excel_Color(string[,] ArrColor, string strExcel_FileName)
        {
            bool  _blRet = false ;
            m_MyExcel = new csExcel();

            //0 数据检查
            if (strExcel_FileName == "" ) return _blRet;

            //1 读模板文件
            m_MyExcel.CreateNewDocument(strExcel_FileName);

            _blRet= m_MyExcel.Set_Black_Color(ArrColor);

            m_MyExcel. SaveDocument(strExcel_FileName);
            return _blRet;
        }
        private int GetCurrPoint(float _flDistance, float Scree_iDotWithmm_X)
        {
            float _flPoint = _flDistance / Scree_iDotWithmm_X;
            _flPoint = float.Parse(_flPoint.ToString("f3"));
            return (int)_flPoint;
        }
        /// <summary>
        /// 2轮Excel原始数据填充
        /// </summary>
        /// <param name="LstBuff_Distance_WallThick_Show">原始数据</param>
        /// <param name="g_Info">铭牌信息</param>
        /// <param name="strMb_FileName">模板文件路径及名称</param>
        /// <param name="strResult_Rpt_FileName">声程报表文件路径及名称</param>
        /// <param name="strSheetName">属性页名称</param>
        /// <returns></returns>
        public int  Set_Ysjl_Excel_2(Class_Test_Item m_SysBuff, string Xkjd,string PCS,                                  
                                     string strMb_FileName,string strResult_Rpt_FileName)
        {
            int _iRet = 0;
            int _iExceRow = 0,iNo=1;
            string strT = "";//临时变量
            float _flT = 0.0F;//临时
            int _iT = 0;//临时
            m_MyExcel = new csExcel();
            string strPcdw = "f1";//偏差%小数个数
            Color _color;
            m_iPrintRunState = 0;

            //0 数据检查
            //  strSheetName = strSheetName==""? "测厚原始记录" : strSheetName;
            if (strMb_FileName == ""|| strResult_Rpt_FileName=="") return _iRet;
            m_strResult_Rpt_FileName = strResult_Rpt_FileName;
            bool _blRaid_In = strMb_FileName.ToLower().IndexOf("record_inch") >0;//英寸

            //1 读模板文件
            m_MyExcel.CreateNewDocument(strMb_FileName);
            m_iPrintRunState++;
            //2 添加公共信息
            m_MyExcel.Set_Cells(2, "B", m_SysBuff.Dwmc );//, strSheetName);//1客户名称
            m_iPrintRunState++;
            m_MyExcel.Set_Cells(3, "B", m_SysBuff.ItemName);//项目名称
            m_MyExcel.Set_Cells(5, "B", m_SysBuff.Sbbh);//设备编号
            m_MyExcel.Set_Cells(8, "F", m_SysBuff.Testing_Standard);//检测标准
            m_MyExcel.Set_Cells(10, "F",Xkjd );//楔块角度
            m_MyExcel.Set_Cells(10, "G", PCS );//探头中心间距
            m_iPrintRunState++;

            strT = m_SysBuff.ID.Replace("_", "");
            if (strT.Length > 6)
                strT = "20" + strT.Substring(0, 2) + "-" + strT.Substring(2, 2) + "-" + strT.Substring(4, 2);
            m_MyExcel.Set_Cells(17, "D", strT);//20检测日期
            m_MyExcel.Set_Cells(17, "B", m_SysBuff.strJyy);//公称厚度
         
            _iRet = _iExceRow;//数据下一行号，用于添加图片其他数据
            m_iPrintRunState++;
            return _iRet;
        }
        public void InsertRow(int iRow)
        {
            m_MyExcel.InsertRow(iRow);
        }
        /// <summary>
        /// 合并行
        /// </summary>
        /// <param name="iStartRow"></param>
        /// <param name="iStartCol"></param>
        /// <param name="iCols"></param>
        public void  Merge_Colums(int iStartRow, int iStartCol,int iCols)
        {
            try
            {
              //  m_MyExcel.MergeCellsColums(iStartRow, iStartCol, iCols);
                m_MyExcel.MergeCellsColums_I_To_S(iStartRow, iStartCol, iCols);
                //   m_MyExcel.MergeCellsColums_I_To_S("A", "B", iStartRow);
            }
            catch (Exception ee)
            { }
        }
        /// <summary>
        /// 指定行列设定字符
        /// </summary>
        /// <param name="iStartRow"></param>
        /// <param name="strDat"></param>
        public void Set_Txt(int iStartRow,string strDat)
        {
            m_MyExcel.Set_Cells(iStartRow, "A", strDat);//9 闸门2处的声程(mm)

        }
        /// <summary>
        /// 根据列明设置指定位置值
        /// </summary>
        /// <param name="iRow"></param>
        /// <param name="strColName"></param>
        /// <param name="strDat"></param>
        public void Set_Txt(int iRow,string strColName, string strDat)
        {
            m_MyExcel.Set_Cells(iRow, strColName, strDat);//9 闸门2处的声程(mm)

        }
        public void MergeCells(string strStart, string strEnd, int iRow)
        {
            m_MyExcel.MergeCells(strStart, strEnd, iRow);//合并

        }
        /// <summary>
        /// 指定行设定高度
        /// </summary>
        /// <param name="iStartRow"></param>
        /// <param name="iEndRow"></param>
        /// <param name="iHeight"></param>
        public void Set_RowHeight(int iStartRow,int iEndRow,int iHeight)
        {
            m_MyExcel.Set_RowHeight(iStartRow, iEndRow, iHeight);
        }
        /// <summary>
        /// 指定行列设置图片
        /// </summary>
        /// <param name="iRow"></param>
        /// <param name="iCol"></param>
        /// <param name="strPath"></param>
        public void Set_Picture(int iRow, int iStartCol, int iEndCol, string strPath)
        {
            //  m_MyExcel.Set_Picture(iRow, iStartCol, iEndCol, strPath);
            m_MyExcel.Set_Picture_I_To_S(iRow, iStartCol, iEndCol, strPath);
        }
        /// <summary>
        /// 报表缓存转存到指定EXCEL输出文件
        /// </summary>
        /// <returns></returns>
        public bool Save_Ysjl_ExcelFile()
        {
            bool _blRet = false;
            try
            {
                m_MyExcel.SaveDocument(m_strResult_Rpt_FileName);//生成该文件
                _blRet = true;
            }
            catch { }
            
            return _blRet;
        }
        public bool Save_ExcelFile(string strFileName)
        {
            bool _blRet = false;
            try
            {
                m_MyExcel.SaveDocument(strFileName);//生成该文件
                _blRet = true;
            }
            catch { }
            return _blRet;
        }
        public void  KillEx()
        {
            m_MyExcel.killWinWordProcess();
        }
        /// <summary>
        /// 设置excel文件行列色标值
        /// </summary>
        /// <param name="ArrColor"></param>
        /// <param name="strExcel_FileName"></param>
        /// <returns></returns>
        public bool Set_Color(string[,] ArrColor, string [,] ArrThick,string [] ArrDistanc, string strS_FileName, string strExcel_FileName)
        {
            bool blRet = false;
            m_MyExcel = new csExcel();

            //0 数据检查
            if (strExcel_FileName == "") return blRet;
            try
            {
                //1 打开模板文件
                string strSheet = "";
                string[] _sPara = strExcel_FileName.Split('\\');

                if (_sPara.Length > 1)
                {
                    strSheet = _sPara[_sPara.Length - 1];
                    _sPara = strSheet.Split('.');
                    strSheet =  _sPara[0];
                }
                int iRows = ArrColor.GetLength(0);
                int iCols = ArrColor.GetLength(1);

                _sPara = "".Split(',');
                string strDat = "";
                Color _color;
                int iNoTitl = 1;
                m_MyExcel.CreateNewDocument_My(strS_FileName);
                for (int iRow = 0; iRow < iRows; iRow++)
                {
                    for (int iCol = 0; iCol < iCols; iCol++)
                    {
                        if (iRow == 0)//1 设置距离标题头
                            m_MyExcel.Set_Cells(1, iCol + 2, ArrDistanc[iCol ] + "mm".ToString());
                        if (iCol == 0)//2 设置行号
                            m_MyExcel.Set_Cells(iRow + 2,  1, (iRow+1).ToString() );

                        if (ArrThick[iRow, iCol] != null && ArrColor[iRow, iCol]!=null )
                        {
                            strDat = ArrColor[iRow, iCol];
                            if (strDat != "")
                            {
                                _sPara = strDat.Split('/');//3 有厚度数据就设置
                              
                                if (_sPara.Length == 3 &&  ArrThick[iRow, iCol].Length >0)
                                {
                                    _color = System.Drawing.Color.FromArgb(int.Parse(_sPara[0]), int.Parse(_sPara[1]), int.Parse(_sPara[2]));
                                    m_MyExcel.Set_Cells_Color(iRow + 2, iCol + 2, _color);//字体颜色
                                    m_MyExcel.Set_Cells(iRow + 2, iCol + 2, ArrThick[iRow, iCol].ToString());//厚度值

                                    //   m_MyExcel.Set_Cells(iRow + 3, iCol + 2, iCol.ToString (), strSheet);
                                }
                            }
                        }
                        else
                        { }
                        Application.DoEvents();
                    }
                }
                blRet = true;
            }
            catch(Exception E)
            { }
           
            return blRet;
        }
        public bool Set_Color_4(string[,] ArrColor, string[,] ArrThick, string[] ArrDistanc, string strS_FileName,
            string strExcel_FileName,ref int iRowStart,float fl_Ymm=5 )
        {
            bool blRet = false;
            if (iRowStart == 0)
                m_MyExcel = new csExcel();

            //0 数据检查
            if (strExcel_FileName == "") return blRet;
            try
            {
                //1 打开模板文件
                string strSheet = "";
                string[] _sPara = strExcel_FileName.Split('\\');

                if (_sPara.Length > 1)
                {
                    strSheet = _sPara[_sPara.Length - 1];
                    _sPara = strSheet.Split('.');
                    strSheet = _sPara[0];
                }
                int iRows = ArrColor.GetLength(0);
                int iCols = ArrColor.GetLength(1);

                _sPara = "".Split(',');
                string strDat = "";
                Color _color;
                int iNoTitl = 1;
                m_iPrintRunState = 0;
                int _iStart_Row = iRowStart;
                if (iRowStart ==0)
                m_MyExcel.CreateNewDocument_My(strS_FileName);
                for (int iRow = 0; iRow < iRows; iRow++)
                {
                    for (int iCol = 0; iCol < iCols; iCol++)
                    {
                        m_iPrintRunState++;
                        if (iRow == 0)//1 设置距离标题头
                            m_MyExcel.Set_Cells(_iStart_Row + 1, iCol + 2, ArrDistanc[iCol] + "mm".ToString());
                        if (iCol == 0)//2 设置行号
                            m_MyExcel.Set_Cells(_iStart_Row + iRow + 2, 1, ((iRow + 0)* fl_Ymm).ToString()+"mm");

                        if (ArrThick[iRow, iCol] != null && ArrColor[iRow, iCol] != null)
                        {
                            strDat = ArrColor[iRow, iCol];
                            if (strDat != "")
                            {
                                _sPara = strDat.Split('/');//3 有厚度数据就设置

                                if (_sPara.Length == 3 && ArrThick[iRow, iCol].Length > 0)
                                {
                                    _color = System.Drawing.Color.FromArgb(int.Parse(_sPara[0]), int.Parse(_sPara[1]), int.Parse(_sPara[2]));
                                    m_MyExcel.Set_Cells_Color(_iStart_Row + iRow + 2, iCol + 2, _color);//字体颜色
                                    iRowStart = _iStart_Row + iRow + 2;
                                    m_MyExcel.Set_Cells(iRowStart, iCol + 2, ArrThick[iRow, iCol].ToString());//厚度值
                                }
                            }
                        }
                        else
                        { }
                        Application.DoEvents();
                    }
                }
                blRet = true;
                iRowStart += 2;
            }
            catch (Exception E)
            { }

            return blRet;
        }
        public bool Set_Color(string[,] ArrColor, float [,] ArrThick, string[] ArrDistanc, string strS_FileName, string strExcel_FileName)
        {
            bool blRet = false;
            m_MyExcel = new csExcel();

            //0 数据检查
            if (strExcel_FileName == "") return blRet;
            try
            {
                //1 打开模板文件
                string strSheet = "";
                string[] _sPara = strExcel_FileName.Split('\\');

                if (_sPara.Length > 1)
                {
                    strSheet = _sPara[_sPara.Length - 1];
                    _sPara = strSheet.Split('.');
                    strSheet = _sPara[0];
                }
                int iRows = ArrColor.GetLength(0);
                int iCols = ArrColor.GetLength(1);

                _sPara = "".Split(',');
                string strDat = "";
                Color _color;
                int iNoTitl = 1;
                m_MyExcel.CreateNewDocument_My(strS_FileName);
                for (int iRow = 0; iRow < iRows; iRow++)
                {
                    for (int iCol = 0; iCol < iCols; iCol++)
                    {
                        if (iRow == 0)//1 设置距离标题头
                            m_MyExcel.Set_Cells(1, iCol + 2, ArrDistanc[iCol] + "mm".ToString());
                        if (iCol == 0)//2 设置行号
                            m_MyExcel.Set_Cells(iRow + 2, 1, (iRow + 1).ToString());

                        if (  ArrColor[iRow, iCol] != null)
                        {
                            strDat = ArrColor[iRow, iCol];
                            if (strDat != "")
                            {
                                _sPara = strDat.Split('/');//3 有厚度数据就设置

                                if (_sPara.Length == 3 && ArrThick[iRow, iCol] >= 0)
                                {
                                    _color = System.Drawing.Color.FromArgb(int.Parse(_sPara[0]), int.Parse(_sPara[1]), int.Parse(_sPara[2]));
                                    m_MyExcel.Set_Cells_Color(iRow + 2, iCol + 2, _color);//字体颜色
                                    m_MyExcel.Set_Cells(iRow + 2, iCol + 2, ArrThick[iRow, iCol].ToString());//厚度值

                                    //   m_MyExcel.Set_Cells(iRow + 3, iCol + 2, iCol.ToString (), strSheet);
                                }
                            }
                        }
                        else
                        { }
                        Application.DoEvents();
                    }
                }
                blRet = true;
            }
            catch (Exception E)
            { }

            return blRet;
        }
        #endregion 方法
    }
}
