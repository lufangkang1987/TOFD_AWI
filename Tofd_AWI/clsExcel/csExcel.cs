using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Text;
using System.Drawing;

namespace clsExcel
{
    public class csExcel
    {
        private Microsoft.Office.Interop.Excel. _Workbook _workBook = null;//Excel文档页面对象
        private Microsoft.Office.Interop.Excel._Worksheet _workSheet = null;//Excel文档单元格对象
        private Microsoft.Office.Interop.Excel.Application  _excelApplicatin = null;//Excel接口

        #region 网络拷贝方法
        /*
         using System;
using System.Data;
using System.Configuration;
using System.Web;
using Microsoft.Office.Interop;
using Microsoft.Office.Core;


namespace Microsoft.Office.Interop.ExcelEdit
{
    /// <SUMMARY>
    /// Microsoft.Office.Interop.ExcelEdit 的摘要说明
    /// </SUMMARY>
    public class ExcelEdit
    {
        public string mFilename;
        public Microsoft.Office.Interop.Excel.Application app;
        public Microsoft.Office.Interop.Excel.Workbooks wbs;
        public Microsoft.Office.Interop.Excel.Workbook wb;
        public Microsoft.Office.Interop.Excel.Worksheets wss;
        public Microsoft.Office.Interop.Excel.Worksheet ws;
        public ExcelEdit()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public void Create()//创建一个Microsoft.Office.Interop.Excel对象
        {
            app = new Microsoft.Office.Interop.Excel.Application();
            wbs = app.Workbooks;
            wb = wbs.Add(true);
        }
        public void Open(string FileName)//打开一个Microsoft.Office.Interop.Excel文件
        {
            app = new Microsoft.Office.Interop.Excel.Application();
            wbs = app.Workbooks;
            wb = wbs.Add(FileName);
            //wb = wbs.Open(FileName, 0, true, 5,"", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "t", false, false, 0, true,Type.Missing,Type.Missing);
            //wb = wbs.Open(FileName,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Microsoft.Office.Interop.Excel.XlPlatform.xlWindows,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing);
            mFilename = FileName;
        }
        public Microsoft.Office.Interop.Excel.Worksheet GetSheet(string SheetName)
        //获取一个工作表
        {
            Microsoft.Office.Interop.Excel.Worksheet s = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[SheetName];
            return s;
        }
        public Microsoft.Office.Interop.Excel.Worksheet AddSheet(string SheetName)
        //添加一个工作表
        {
            Microsoft.Office.Interop.Excel.Worksheet s = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            s.Name = SheetName;
            return s;
        }

        public void DelSheet(string SheetName)//删除一个工作表
        {
            ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[SheetName]).Delete();
        }
        public Microsoft.Office.Interop.Excel.Worksheet ReNameSheet(string OldSheetName, string NewSheetName)//重命名一个工作表一
        {
            Microsoft.Office.Interop.Excel.Worksheet s = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[OldSheetName];
            s.Name = NewSheetName;
            return s;
        }

        public Microsoft.Office.Interop.Excel.Worksheet ReNameSheet(Microsoft.Office.Interop.Excel.Worksheet Sheet, string NewSheetName)//重命名一个工作表二
        {

            Sheet.Name = NewSheetName;

            return Sheet;
        }

        public void SetCellValue(Microsoft.Office.Interop.Excel.Worksheet ws, int x, int y, object value)
        //ws：要设值的工作表     X行Y列     value   值
        {
            ws.Cells[x, y] = value;
        }
        public void SetCellValue(string ws, int x, int y, object value)
        //ws：要设值的工作表的名称 X行Y列 value 值
        {

            GetSheet(ws).Cells[x, y] = value;
        }

        public void SetCellProperty(Microsoft.Office.Interop.Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, int size, string name, Microsoft.Office.Interop.Excel.Constants color, Microsoft.Office.Interop.Excel.Constants HorizontalAlignment)
        //设置一个单元格的属性   字体，   大小，颜色   ，对齐方式
        {
            name = "宋体";
            size = 12;
            color = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
            HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlRight;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Name = name;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Size = size;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Color = color;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        }

        public void SetCellProperty(string wsn, int Startx, int Starty, int Endx, int Endy, int size, string name, Microsoft.Office.Interop.Excel.Constants color, Microsoft.Office.Interop.Excel.Constants HorizontalAlignment)
        {
            //name = "宋体";
            //size = 12;
            //color = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
            //HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlRight;

            Microsoft.Office.Interop.Excel.Worksheet ws = GetSheet(wsn);
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Name = name;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Size = size;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Color = color;

            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        }


        public void UniteCells(Microsoft.Office.Interop.Excel.Worksheet ws, int x1, int y1, int x2, int y2)
        //合并单元格
        {
            ws.get_Range(ws.Cells[x1, y1], ws.Cells[x2, y2]).Merge(Type.Missing);
        }

        public void UniteCells(string ws, int x1, int y1, int x2, int y2)
        //合并单元格
        {
            GetSheet(ws).get_Range(GetSheet(ws).Cells[x1, y1], GetSheet(ws).Cells[x2, y2]).Merge(Type.Missing);

        }


        public void InsertTable(System.Data.DataTable dt, string ws, int startX, int startY)
//将内存中数据表格插入到Microsoft.Office.Interop.Excel指定工作表的指定位置 为在使用模板时控制格式时使用一
        {

            for (int i = 0; i  <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j  <= dt.Columns.Count - 1; j++)
                {
                    GetSheet(ws).Cells[startX+i, j + startY] = dt.Rows[i][j].ToString();

                }

            }

        }
        public void InsertTable(System.Data.DataTable dt, Microsoft.Office.Interop.Excel.Worksheet ws, int startX, int startY)
//将内存中数据表格插入到Microsoft.Office.Interop.Excel指定工作表的指定位置二
        {

            for (int i = 0; i  <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j  <= dt.Columns.Count - 1; j++)
                {

                    ws.Cells[startX+i, j + startY] = dt.Rows[i][j];

                }

            }

        }


        public void AddTable(System.Data.DataTable dt, string ws, int startX, int startY)
//将内存中数据表格添加到Microsoft.Office.Interop.Excel指定工作表的指定位置一
        {

            for (int i = 0; i  <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j  <= dt.Columns.Count - 1; j++)
                {

                    GetSheet(ws).Cells[i + startX, j + startY] = dt.Rows[i][j];

                }

            }

        }
        public void AddTable(System.Data.DataTable dt, Microsoft.Office.Interop.Excel.Worksheet ws, int startX, int startY)
//将内存中数据表格添加到Microsoft.Office.Interop.Excel指定工作表的指定位置二
        {


            for (int i = 0; i  <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j  <= dt.Columns.Count - 1; j++)
                {

                    ws.Cells[i + startX, j + startY] = dt.Rows[i][j];

                }
            }

        }
        public void InsertPictures(string Filename, string ws)
        //插入图片操作一
        {
            GetSheet(ws).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
            //后面的数字表示位置
        }

        //public void InsertPictures(string Filename, string ws, int Height, int Width)
        //插入图片操作二
        //{
        //    GetSheet(ws).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).Height = Height;
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).Width = Width;
        //}
        //public void InsertPictures(string Filename, string ws, int left, int top, int Height, int Width)
        //插入图片操作三
        //{

        //    GetSheet(ws).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).IncrementLeft(left);
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).IncrementTop(top);
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).Height = Height;
        //    GetSheet(ws).Shapes.get_Range(Type.Missing).Width = Width;
        //}

        public void InsertActiveChart(Microsoft.Office.Interop.Excel.XlChartType ChartType, string ws, int DataSourcesX1, int DataSourcesY1, int DataSourcesX2, int DataSourcesY2, Microsoft.Office.Interop.Excel.XlRowCol ChartDataType)
        //插入图表操作
        {
            ChartDataType = Microsoft.Office.Interop.Excel.XlRowCol.xlColumns;
            wb.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            {
                wb.ActiveChart.ChartType = ChartType;
                wb.ActiveChart.SetSourceData(GetSheet(ws).get_Range(GetSheet(ws).Cells[DataSourcesX1, DataSourcesY1], GetSheet(ws).Cells[DataSourcesX2, DataSourcesY2]), ChartDataType);
                wb.ActiveChart.Location(Microsoft.Office.Interop.Excel.XlChartLocation.xlLocationAsObject, ws);
            }
        }
        public bool Save()
        //保存文档
        {
            if (mFilename == "")
            {
                return false;
            }
            else
            {
                try
                {
                    wb.Save();
                    return true;
                }

                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public bool SaveAs(object FileName)
        //文档另存为
        {
            try
            {
                wb.SaveAs(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                return true;

            }

            catch (Exception ex)
            {
                return false;

            }
        }
        public void Close()
        //关闭一个Microsoft.Office.Interop.Excel对象，销毁对象
        {
            //wb.Save();
            wb.Close(Type.Missing, Type.Missing, Type.Missing);
            wbs.Close();
            app.Quit();
            wb = null;
            wbs = null;
            app = null;
            GC.Collect();
        }
    }
}
         */

        /*
Js操作Excel常用方法
Js操作Excel常用方法
1.创建一个新Excel表格
var XLObj = new ActiveXObject("Excel.Application");
var xlBook = XLObj.Workbooks.Add;                         //新增工作簿
var ExcelSheet = xlBook.Worksheets(1);                   //创建工作表
2.保存表格
ExcelSheet.SaveAs("C:\\TEST.XLS");
3.使 Excel 通过Application 对象可见
ExcelSheet.Application.Visible = true;
4.打印
xlBook.PrintOut;
或者:
ExcelSheet.PrintOut;
5.关闭
xlBook.Close(savechanges=false);
或者:
ExcelSheet.Close(savechanges=false);
6.结束进程
ExcelSheet.Application.Quit();
或者:
XLObj.Quit();
XLObj=null;
function endXlsObj(){    
oXL.UserControl = true;   
oXL=null;   
oWB=null;   
activeSheet=null;   
//结束excel进程，退出完成
idTmr = window.setInterval("Cleanup();",1);
}
function Cleanup() {
window.clearInterval(idTmr);
CollectGarbage();
} 
7.页面设置
ExcelSheet.ActiveSheet.PageSetup.Orientation = 2;
ExcelSheet.ActiveSheet.PageSetup.LeftMargin= 2/0.035;         //页边距 左2厘米
ExcelSheet.ActiveSheet.PageSetup.RightMargin = 3/0.035;      //页边距 右3厘米，
ExcelSheet.ActiveSheet.PageSetup.TopMargin = 4/0.035;        //页边距 上4厘米，
ExcelSheet.ActiveSheet.PageSetup.BottomMargin = 5/0.035;   //页边距 下5厘米
ExcelSheet.ActiveSheet.PageSetup.HeaderMargin = 1/0.035;   //页边距 页眉1厘米
ExcelSheet.ActiveSheet.PageSetup.FooterMargin = 2/0.035;    //页边距 页脚2厘米
ExcelSheet.ActiveSheet.PageSetup.CenterHeader = "页眉中部内容";
ExcelSheet.ActiveSheet.PageSetup.LeftHeader = "页眉左部内容";
ExcelSheet.ActiveSheet.PageSetup.RightHeader = "页眉右部内容";
ExcelSheet.ActiveSheet.PageSetup.LeftFooter = "页脚左部内容";
ExcelSheet.ActiveSheet.PageSetup.RightFooter = "页脚右部内容";            ExcelSheet.ActiveSheet.PageSetup.CenterHeader = "&\"宋体,加粗\"&18长天公司" + date1 + "至" + date2 + "(施工图)项目进度检查表";
ExcelSheet.ActiveSheet.PageSetup.RightHeader = "&D";
ExcelSheet.ActiveSheet.PageSetup.PrintGridlines = true;
ExcelSheet.ActiveSheet.PageSetup.PrintTitleRows = "$1:$1";
ExcelSheet.ActiveSheet.PageSetup.Zoom = 75;
8.对单元格操作，带*部分对于行，列，区域都有相应属性
ExcelSheet.ActiveSheet.Cells(row,col).Value = "内容";                //设置单元格内容
ExcelSheet.ActiveSheet.Cells(row,col).Borders.Weight = 1;        //设置单元格边框*()
ExcelSheet.ActiveSheet.Cells(row,col).Interior.ColorIndex = 1;    //设置单元格底色*(1-黑色，
2-白色，3-红色，4-绿色，5-蓝色，6-黄色，7-粉红色，8-天蓝色，9-酱土色..可以多做尝试)
ExcelSheet.ActiveSheet.Cells(row,col).Interior.Pattern = 1;         //设置单元格背景样式*(1-无，
                                            2-细网格，3-粗网格，4-斑点，5-横线，6-竖线..可以多做尝试)
ExcelSheet.ActiveSheet.Cells(row,col).Font.ColorIndex = 1;        //设置字体颜色*(与上相同)
ExcelSheet.ActiveSheet.Cells(row,col).Font.Size = 10;                //设置为10号字*
ExcelSheet.ActiveSheet.Cells(row,col).Font.Name = "黑体";        //设置为黑体*
ExcelSheet.ActiveSheet.Cells(row,col).Font.Italic = true;             //设置为斜体*
ExcelSheet.ActiveSheet.Cells(row,col).Font.Bold = true;             //设置为粗体*
ExcelSheet.ActiveSheet.Cells(row,col).ClearContents;                //清除内容*
ExcelSheet.ActiveSheet.Cells(row,col).WrapText=true;               //设置为自动换行*
ExcelSheet.ActiveSheet.Cells(row,col).HorizontalAlignment = 3; //水平对齐方式枚举* (1-常规，
                           2-靠左，3-居中，4-靠右，5-填充 6-两端对齐，7-跨列居中，8-分散对齐)
ExcelSheet.ActiveSheet.Cells(row,col).VerticalAlignment = 2;      //垂直对齐方式枚举*(1-靠上，
                                                                         2-居中，3-靠下，4-两端对齐，5-分散对齐)
//行，列有相应操作:
ExcelSheet.ActiveSheet.Rows(row).
ExcelSheet.ActiveSheet.Columns(col).
ExcelSheet.ActiveSheet.Rows(startrow+":"+endrow).                  //如Rows("1:5")即1到5行
ExcelSheet.ActiveSheet.Columns(startcol+":"+endcol).               //如Columns("1:5")即1到5列
//区域有相应操作:
XLObj.Range(startcell+":"+endcell).Select;
//如Range("A2:H8")即A列第2格至H列第8格的整个区域
XLObj.Selection.
//合并单元格
XLObj.Range(startcell+":"+endcell).MergeCells = true;
//如Range("A2:H8")即将A列第2格至H列第8格的整个区域合并为一个单元格
或者:
XLObj.Range("A2",XLObj.Cells(8, 8)).MergeCells = true;
9.设置行高与列宽
ExcelSheet.ActiveSheet.Columns(startcol+":"+endcol).ColumnWidth = 22;
//设置从firstcol到stopcol列的宽度为22
ExcelSheet.ActiveSheet.Rows(startrow+":"+endrow).RowHeight = 22;
//设置从firstrow到stoprow行的宽度为22
分类: 技术&提高&收集




            代码操作Excel（插入Excel列、设置单元格背景色、赋值、设置字体） (2012-04-23 09:03:26)转载▼
标签： it	分类： 代码操作Excel、Pdf
 //在前面插入两列主要,次要关键字列表（动态）(Excel中插入两列)
                objRange = objWorkSheet.Columns[1];
                objRange.Insert(Shift: XlDirection.xlDown);
                objRange.Insert(Shift: XlDirection.xlDown);
                objRange = objWorkSheet.get_Range("A6");//定位Excel中的单元格
                objRange.FormulaR1C1 = strParetoCellKey.Split(',')[0].ToString()             == "AK" ? "Rot" : strParetoCellKey.Split(',')[0].ToString();//主要关键字，excel的单元格赋值
                objRange.Interior.ColorIndex = 3;//红色，excel单元格赋予背景色
                objRange.Font.Size = 14;//excel单元格字体设置大小
                //同上理
                objRange = objWorkSheet.get_Range("B6");
                objRange.FormulaR1C1 = strParetoCellKey.Split(',')[1].ToString() == "BK" ? "Gelb" : strParetoCellKey.Split(',')[1].ToString();//次要关键字
                objRange.Interior.ColorIndex = 6;//黄色
                objRange.Font.Size = 14;
*/
        #endregion 网络拷贝



        /// <summary>
        /// //Excel文档单元格对象
        /// </summary>
        public _Worksheet WorkSheet
        {
            get { return _workSheet; }
            set { _workSheet = value; }
        }

        /// <summary>
        /// //Excel页面对象
        /// </summary>
        public _Workbook WorkBook
        {
            get { return _workBook; }
            set { _workBook = value; }
        }


        /// <summary>
        /// //Excel接口
        /// </summary>
        public Application ExcelApplicatin
        {
            get { return _excelApplicatin; }
            set { _excelApplicatin = value; }
        }


        /// <summary>
        /// 通过模板创建新文档
        /// string filePath：模板路径
        /// </summary>
        /// <param name="filePath">文件模板路径</param>
        public void CreateNewDocument(string filePath)
        {
            killWinWordProcess();
            _excelApplicatin =    new Application();//ApplicationClass();//
            _excelApplicatin.Visible = false;//执行Open函数时，不显示Excel文档
            _excelApplicatin.DisplayAlerts = true;//文件存在时是否提示替换
            object missing = System.Reflection.Missing.Value;

            //打开已存在的Excel
            _workBook = _excelApplicatin.Workbooks.Open(filePath, missing, missing,
                      missing, missing, missing, missing, missing,
                      missing, missing, missing, missing, missing, missing, missing);

        }
        public void CreateNewDocument_My(string filePath)
        {
            killWinWordProcess();
            _excelApplicatin =  new Application();// ApplicationClass();// new Application();
                                                     //  Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.ApplicationClass();
            _excelApplicatin.Visible = false;//执行Open函数时，不显示Excel文档
            _excelApplicatin.DisplayAlerts = true;//文件存在时是否提示替换
            object missing = System.Reflection.Missing.Value;

            //打开已存在的Excel
            _workBook = _excelApplicatin.Workbooks.Open(filePath, missing, missing,
                      missing, missing, missing, missing, missing,
                      missing, missing, missing, missing, missing, missing, missing);

        }
        public int GetRows(string SheetName)
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetName];
           int iR= _workSheet.UsedRange.Cells.Rows .Count;
            return _workSheet.UsedRange.Rows.Count ;
        }
        public bool ModySheetName(int iNo)
        {
            bool blRet = false;
            try
            {
              //  MSExcel.Worksheet ws = _excelApplicatin.Worksheets.get_Item(1);
               

                blRet = true;
            }
            catch { }

            return blRet;
        }
        public int GetCols()
        {
            _workSheet = (Worksheet)_workBook.Sheets[""];
            return _workSheet.UsedRange.Cells.Columns .Count ;
        }
        public void Close()
        {
            object RouteWorkBook = false;
            object missing = System.Reflection.Missing.Value;
            _workBook.Close(missing, missing, RouteWorkBook);
            _excelApplicatin.Quit();
            _excelApplicatin = null;

            killWinWordProcess();

        }
        /// <summary>
        /// 保存新文件
        /// </summary>
        /// <param name="filePath">文件保存路径</param>
        public void SaveDocument(string filePath)
        {
            object fileName = filePath;

            XlSaveAsAccessMode AcessMode = Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange;
            object miss = System.Reflection.Missing.Value;
            _workBook.SaveAs(fileName, miss, miss,
                        miss, miss, miss, AcessMode,
                        miss, miss, miss, miss,
                        miss);



            object missing = System.Reflection.Missing.Value;
            object RouteWorkBook = false;



            //#region 用于预览
            //ExcelApplicatin.Visible = true;//用于显示页面
            //_workBook.PrintPreview(true);//预览。


            //#endregion



            _workBook.Close(missing, missing, RouteWorkBook);
            _excelApplicatin.Quit();
            _excelApplicatin = null;

            killWinWordProcess();


        }
        public void SaveDocument_Old(string filePath)
        {
            object fileName = filePath;
            //_excelApplicatin.DisplayAlerts = false;
            //_excelApplicatin.AlertBeforeOverwriting = false;

            object miss = System.Reflection.Missing.Value;
            XlSaveAsAccessMode AcessMode = Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange;

            _workBook.SaveAs(fileName, miss, miss,
                        miss, miss, miss, AcessMode,
                        miss, miss, miss, miss,
                        miss);



            object missing = System.Reflection.Missing.Value;
            object RouteWorkBook = false;



            //#region 用于预览
            //ExcelApplicatin.Visible = true;//用于显示页面
          // _workBook.PrintPreview(true);//预览。


            //#endregion



            _workBook.Close(missing, missing, RouteWorkBook);
            _excelApplicatin.Quit();
            _excelApplicatin = null;

            killWinWordProcess();


        }
        /// <summary>
        /// 设定列宽带
        /// </summary>
        /// <param name="strStartColName">开始列</param>
        /// <param name="strEndColName">结束列</param>
        /// <param name="Width"></param>
        /// <param name="SheetsName"></param>
        public void Set_ColunWidth(string strStartColName, string strEndColName, int Width, string SheetsName = "Sheet1")
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];

            if (strStartColName == "") return;
            if (strEndColName == "") return;

            strStartColName = strStartColName.ToUpper();
            strEndColName = strEndColName.ToUpper();

            ///ColumnWidth "A:B"表示第一列和第二列, "A:A"表示第一列
            ((Range)_workSheet.Columns[strStartColName + ":" + strEndColName, System.Type.Missing]).ColumnWidth = Width;

            //Range allColumn = _workSheet.Columns;
            //allColumn.AutoFit();

        }
        /// <summary>
        /// 设定行高度
        /// </summary>
        /// <param name="iStartRowName">开始行</param>
        /// <param name="iEndRowName">开始列</param>
        /// <param name="Width"></param>
        /// <param name="SheetsName"></param>
        public void Set_RowHeight(int iStartRowName, int iEndRowName, int RowHeight, string SheetsName = "Sheet1")
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];

            if (iStartRowName < 1) return;
            if (iEndRowName < 1) return;

            string strStartRowName = "", strEndRowName = "";
            strStartRowName = iStartRowName.ToString();
            strEndRowName = iEndRowName.ToString();
            //指定开始结束列
            ((Range)_workSheet.Rows[strStartRowName + ":" + strEndRowName, System.Type.Missing]).RowHeight = RowHeight;
            //Range allColumn = _workSheet.Rows ;
            //allColumn.AutoFit();
        }
        /// <summary>
        /// Excel中对单元格赋值
        /// int row 第几行
        /// int col 第几列
        /// string strValue 赋值内容
        /// string SheetsName 页面名称
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="strValue"></param>
        public void Set_Cells(int row, int col, string strValue, string SheetsName = "Sheet1")
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];

            _workSheet.Cells[row, col] = strValue;
        }
        public void Set_Cells(int row, int col, Color _color, string strValue, string SheetsName = "Sheet1")
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
            //Range excelRange = (Range)_workSheet.get_Range(_workSheet.Cells[row, col], _workSheet.Cells[row, col]);
            Range excelRange = (Range)_workSheet.Cells[row, col];
            _workSheet.Cells[row, col] = strValue;
            excelRange.Interior.Color = _color;
        }
        /// <summary>
        /// Excel中对单元格赋值
        /// int row 第几行
        /// int colName 列名称  列名称A B C D<
        /// string strValue 赋值内容
        /// string SheetsName 页面名称
        /// </summary>
        /// <param name="row"></param>
        /// <param name="colName">列名称</param>
        /// <param name="strValue"></param>
        public void Set_Cells(int row, string colName, string strValue, string SheetsName = "Sheet1")
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];

            int col = GetStrIndex(colName);

            _workSheet.Cells[row, col] = strValue;
        }
      
        /// <summary>
        /// 成块设置excel颜色
        /// </summary>
        /// <param name="ArrColor"></param>
        /// <param name="SheetsName"></param>
        /// <returns></returns>
        public bool  Set_Black_Color(object[,] ArrColor, string SheetsName = "Sheet1")
        {
            bool _blRet = false;
            try
            {
                _workSheet = (Worksheet)_workBook.Sheets[SheetsName];

                _workSheet.get_Range("A1", _workSheet.Cells[_workSheet.UsedRange.Rows.Count, _workSheet.UsedRange.Cells.Columns.Count]).Interior.Color = ArrColor;
                _blRet = true ;
            }
            catch (Exception e)
            { }
            return _blRet;
        }
        /// <summary>
        /// 设置单元格颜色
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <param name="_color">颜色</param>
        /// <param name="SheetsName">属性页</param>
        public void Set_Cells_Color(int row, int col, Color _color, string SheetsName = "Sheet1")
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
            Range excelRange;
            string strRow;
            try
            {
                excelRange = (Range)_workSheet.get_Range(_workSheet.Cells[row, col], _workSheet.Cells[row, col]);
            }
            catch (Exception de)
            {
                strRow = IntToMoreChar(col) + row.ToString();
                excelRange = (Range)_workSheet.get_Range(strRow, strRow);
            }

            excelRange.Interior.Color = Color.FromArgb(_color.R, _color.G, _color.B);// _color.F;
        }
        private string IntToMoreChar(int value)
          {
              string rtn = string.Empty;
              List<int> iList = new List<int>();
  
              //To single Int
              while (value / 26 != 0 || value % 26 != 0)
              {
                  iList.Add(value % 26);
                 value /= 26;
             }
 
             //Change 0 To 26
            for (int j = 0; j<iList.Count - 1; j++)
             {
                 if (iList[j] == 0)
                 {
                     iList[j + 1] -= 1;
                     iList[j] = 26;
                 }
             }
 
             //Remove 0 at last
             if (iList[iList.Count - 1] == 0)
             {
                 iList.Remove(iList[iList.Count - 1]);
             }
 
             //To String
             for (int j = iList.Count - 1; j >= 0; j--)
             {
                 char c = (char)(iList[j] + 64);
                 rtn += c.ToString();
             }
 
             return rtn;
         }
        /// <summary>
        /// 指定行位置插入行
        /// </summary>
        /// <param name="iRow"></param>
        /// <param name="SheetsName"></param>
        public void InsertRow(int iRow, string SheetsName = "Sheet1")
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
            Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)_workSheet.Rows[iRow, Type.Missing];

            range.EntireRow.Insert(Microsoft.Office.Interop.Excel.XlDirection.xlDown,
                Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
        }
        /// <summary>
        /// 读指定行列数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="SheetsName"></param>
        /// <returns></returns>
        public string ReadData(int row, int col, string SheetsName = "Sheet1")
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
            Range excelRange = (Range)_workSheet.get_Range(_workSheet.Cells[row, col], _workSheet.Cells[row, col]);

            return excelRange.Text.ToString();
        }
        /*
         网址 https://technet.microsoft.com/zh-cn/library/ff198302 
         
         本示例向 myDocument 中添加由文件"Music.bmp"创建的图片。插入的图片链接到创建该图片的文件，并与 myDocument 一起保存。
            VBA
            Set myDocument = Worksheets(1) 
            myDocument.Shapes.AddPicture _ 
                "c:\microsoft office\clipart\music.bmp", _ 
                True, True, 100, 100, 70, 70
         */
        /// <summary>
        /// 从现有文件创建图片。返回表示此新图片的形状对象。  以下示例在活动文档中新创建的绘图画布上添加一幅图片。
        /// </summary>
        /// <param name="Pic_Name">要在其中创建 OLE 对象的文件。</param>
        /// <param name="Left">图片左上角相对于文档左上角的位置（以磅为单位）。</param>
        /// <param name="Top">图片左上角相对于文档顶部的位置（以磅为单位）。</param>
        /// <param name="Width">图片，以磅为单位的宽度 (输入-1 来保留现有文件的宽度)。</param>
        /// <param name="Height">图片，以磅为单位的高度 (输入-1 来保留现有文件的高度)。</param>
        /// <param name="SheetsName"></param>
        public void Set_Picture(string Pic_Name, float Left, float Top, float Width, float Height, string SheetsName = "Sheet1")
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];

            _workSheet.Shapes.AddPicture(Pic_Name, //图片的路径和文件名。
                                           MsoTriState.msoFalse,//True:要将图片链接到创建它的文件。False使图片文件的独立副本。默认值为False。
                                         Microsoft.Office.Core.MsoTriState.msoTrue,//真要随文档一起保存的链接的图片。默认值为False。
                                           Left, Top, Width, Height);
        }
        /// <summary>
        /// 将图片插入到指定的单元格位置，并设置图片的宽度和高度
        /// </summary>
        /// <param name="row">第几行</param>
        /// <param name="col">第几列</param>
        /// <param name="Pic_PathName">图片必须是绝对物理路径</param>
        /// <param name="SheetsName"></param>
        public void Set_Picture(int row, int iSartcol,int iEndCol, string Pic_PathName, string SheetsName = "Sheet1")
        {
            //功能：将图片插入到指定的单元格位置，并设置图片的宽度和高度
            //图片必须是绝对物理路径 

            //1 确定单元格
            try
            {
                _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
                Range _Range = (Range)_workSheet.get_Range(_workSheet.Cells[row, iSartcol], _workSheet.Cells[row, iEndCol]);
                //2 确定边界
                float Left = 0, Top = 0, Width = 0, Height = 0;
                _Range.Select();
                Left = Convert.ToSingle(_Range.Left);
                Top = Convert.ToSingle(_Range.Top);
                Width = Convert.ToSingle(_Range.Width);
                Height = Convert.ToSingle(_Range.Height);

                //3 添加图片
                _workSheet.Shapes.AddPicture(Pic_PathName,  //图片路径
                                             Microsoft.Office.Core.MsoTriState.msoFalse,//是否链接到文件 
                                             Microsoft.Office.Core.MsoTriState.msoTrue,  //图片插入时是否随文档一起保存
                                             Left, Top, //图片在文档中的坐标位置 坐标
                                             Width, Height); //图片显示的宽度和高度
            }
            catch (Exception dd)
            { }
        }
        //GetStrIndex_I_To_S(col) + row.ToString()
        public void Set_Picture_I_To_S(int row, int iSartcol, int iEndCol, string Pic_PathName, string SheetsName = "Sheet1")
        {
            //功能：将图片插入到指定的单元格位置，并设置图片的宽度和高度
            //图片必须是绝对物理路径 

            //1 确定单元格
            try
            {
                iEndCol--;
                   _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
                Range _Range = (Range)_workSheet.get_Range(GetStrIndex_I_To_S(iSartcol) + row.ToString(), GetStrIndex_I_To_S(iEndCol) + row.ToString());




                 //   _workSheet.Cells[row, iSartcol], _workSheet.Cells[row, iEndCol]);
                //2 确定边界
                float Left = 0, Top = 0, Width = 0, Height = 0;
                _Range.Select();
                Left = Convert.ToSingle(_Range.Left);
                Top = Convert.ToSingle(_Range.Top);
                Width = Convert.ToSingle(_Range.Width);
                Height = Convert.ToSingle(_Range.Height);

                //3 添加图片
                _workSheet.Shapes.AddPicture(Pic_PathName,  //图片路径
                                             Microsoft.Office.Core.MsoTriState.msoFalse,//是否链接到文件 
                                             Microsoft.Office.Core.MsoTriState.msoTrue,  //图片插入时是否随文档一起保存
                                             Left, Top, //图片在文档中的坐标位置 坐标
                                             Width, Height); //图片显示的宽度和高度
            }
            catch (Exception dd)
            { }
        }
        /// <summary>
        /// 将图片插入到指定的单元格位置，并设置图片的宽度和高度
        /// </summary>
        /// <param name="row">第几行</param>
        /// <param name="colName">列名称  列名称A B C D</param>
        /// <param name="Pic_PathName">图片必须是绝对物理路径</param>
        /// <param name="SheetsName"></param>
        public void Set_Picture(int row, string colName, string Pic_PathName, string SheetsName = "Sheet1")
        {
            //功能：将图片插入到指定的单元格位置，并设置图片的宽度和高度
            //图片必须是绝对物理路径 

            //1 确定单元格
            int col = GetStrIndex(colName);
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
            Range _Range = (Range)_workSheet.get_Range(_workSheet.Cells[row, col], _workSheet.Cells[row, col]);

            //2 确定边界
            float Left = 0, Top = 0, Width = 0, Height = 0;
            _Range.Select();
            Left = Convert.ToSingle(_Range.Left);
            Top = Convert.ToSingle(_Range.Top);
            Width = Convert.ToSingle(_Range.Width);
            Height = Convert.ToSingle(_Range.Height);

            //3 添加图片
            _workSheet.Shapes.AddPicture(Pic_PathName,  //图片路径
                                         MsoTriState.msoFalse,//是否链接到文件 
                                         Microsoft.Office.Core.MsoTriState.msoTrue,  //图片插入时是否随文档一起保存
                                         Left, Top, //图片在文档中的坐标位置 坐标
                                         Width, Height); //图片显示的宽度和高度
        }
        /*
      在word文档中任意指定位置插入 其他Excel表数据和图片   https://wenku.baidu.com/view/de1462da50e2524de5187e0e.html
         方法一：
        /// 将图片插入到指定的单元格位置，并设置图片的宽度和高度。
        /// 注意：图片必须是绝对物理路径
        /// </summary>
        /// <param name="RangeName">单元格名称，例如：B4</param>
        /// <param name="PicturePath">要插入图片的绝对路径。</param>
        public void InsertPicture(string RangeName, Excel._Worksheet sheet, string PicturePath)
        {
         Excel.Range rng = (Excel.Range)sheet.get_Range(RangeName, Type.Missing);
        rng.Select();
        float PicLeft, PicTop, PicWidth, PicHeight;　　　　//距离左边距离，顶部距离，图片宽度、高度
        PicTop = Convert.ToSingle(rng.Top) ;
        PicWidth = Convert.ToSingle(rng.MergeArea.Width);
        PicHeight = Convert.ToSingle(rng.Height);
        PicWidth = Convert.ToSingle(rng.Width);
        PicLeft = Convert.ToSingle(rng.Left);//+ (Convert.ToSingle(rng.MergeArea.Width) - PicWidth) / 2;
        try
        {
        Excel.Pictures pics = (Excel.Pictures)sheet.Pictures(Type.Missing);
        pics.Insert(PicturePath, Type.Missing);
        pics.Left = (double)rng.Left;
        pics.Top = (double)rng.Top;
        pics.Width = (double)rng.Width;
        pics.Height = (double)rng.Height;

        }
        catch
        {
        }
        //sheet.Shapes.AddPicture(PicturePath, Microsoft.Office.Core.MsoTriState.msoFalse,
        // Microsoft.Office.Core.MsoTriState.msoTrue, PicLeft, PicTop, PicWidth, PicHeight);
        }

        如果是要在某个区域插入，改区域没有命名的话，直接传入选中区域

        Cell1 = SourceSheet.Cells[第几行, 第几列];
        Cell2 = SourceSheet.Cells[Row , Column];

        SourceRange = SourceSheet.get_Range(Cell1, Cell2);

        然后把上面的这句去掉 Excel.Range rng = (Excel.Range)sheet.get_Range(RangeName, Type.Missing);

        把rng换成SourceRange
         */

        /// <summary>
        /// 里面写了一些常用Excel例子操作
        /// </summary>
        private void ok()
        {
            _excelApplicatin = new Application();
            _excelApplicatin.Visible = true;
            _excelApplicatin.DisplayAlerts = true;

            _workBook = _excelApplicatin.Workbooks.Add(XlSheetType.xlWorksheet);//添加一页(Sheet1)
            _workSheet = (Worksheet)_workBook.ActiveSheet;
            _workSheet.Name = "workSheetName";// 设置

            //打开已存在的Excel
            string strExcelPathName = AppDomain.CurrentDomain.BaseDirectory + "excelSheetName.xls";
            Workbook workBook = _excelApplicatin.Workbooks.Open(strExcelPathName, Type.Missing, Type.Missing,
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                      Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //读取已打开的Excel
            Worksheet workSheet1 = (Worksheet)workBook.Sheets["SheetName1"];
            Worksheet workSheet2 = (Worksheet)workBook.Sheets["SheetName2"];

            //添加一个workSheet
            Worksheet workSheet = (Worksheet)workBook.Worksheets.Add(System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing);

            //RowHeight   "1:1"表示第一行, "1:2"表示,第一行和第二行 
            ((Range)_workSheet.Rows["1:1", System.Type.Missing]).RowHeight = 100;

            //ColumnWidth "A:B"表示第一列和第二列, "A:A"表示第一列
            ((Range)_workSheet.Columns["A:B", System.Type.Missing]).ColumnWidth = 10;

            // EXCEL操作(需要冻结的字段 按住ALT+W 再按F)
            //     Range excelRange = _workSheet.get_Range(_workSheet.Cells[10, 5], _workSheet.Cells[10, 5]);
            //excelRange.Select();
            //_excelApplicatin.ActiveWindow.FreezePanes = true;

            //Borders.LineStyle 单元格边框线
            Range excelRange = _workSheet.get_Range(_workSheet.Cells[2, 2], _workSheet.Cells[4, 6]);
            //单元格边框线类型(线型,虚线型)
            excelRange.Borders.LineStyle = 1;
            excelRange.Borders.get_Item(XlBordersIndex.xlEdgeTop).LineStyle = XlLineStyle.xlContinuous;
            //指定单元格下边框线粗细,和色彩
            excelRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium;

            excelRange.Borders.get_Item(XlBordersIndex.xlEdgeBottom).ColorIndex = 3;

            //设置字体大小
            excelRange.Font.Size = 15;
            //设置字体是否有下划线
            excelRange.Font.Underline = true;

            //设置字体在单元格内的对其方式
            excelRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //设置单元格的宽度
            excelRange.ColumnWidth = 15;
            //设置单元格的背景色
            excelRange.Cells.Interior.Color = System.Drawing.Color.FromArgb(255, 204, 153).ToArgb();
            // 给单元格加边框
            excelRange.BorderAround(XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlThick,
                                               Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());
            //自动调整列宽
            excelRange.EntireColumn.AutoFit();
            // 文本水平居中方式
            excelRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //文本自动换行
            excelRange.WrapText = true;
            //填充颜色为淡紫色
            excelRange.Interior.ColorIndex = 39;

            //合并单元格
            excelRange.Merge(excelRange.MergeCells);
            _workSheet.get_Range("A15", "B15").Merge(_workSheet.get_Range("A15", "B15").MergeCells);
        }

        /// <summary>
        /// 根据列名称得到列索引 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>
        public  int GetStrIndex(string colName)
        {
            int col = 0;
            if (colName.Length < 2)
            {
                colName = colName.PadLeft(2, '0');
            }

            string[] strValue ={"A","B","C","D","E","F","G","H","I","J","K",
                                   "L","M","N","O","P","Q","R","S","T","U",
                                   "V","W","X","Y","Z"};

            string CC1 = colName.Substring(0, 1);

            int num1 = 0;
            for (int i = 0; i < strValue.Length; i++)
            {
                if (CC1.ToUpper() == strValue[i])
                {
                    num1 = i + 1;
                    break;
                }
            }
            col = num1 * 26;


            string CC2 = colName.Substring(1, 1);

            int num2 = 0;
            for (int i = 0; i < strValue.Length; i++)
            {
                if (CC2.ToUpper() == strValue[i])
                {
                    num2 = i + 1;
                    break;
                }
            }
            col = col + num2;
            return col;
        }


        public string GetStrIndex_I_To_S(int iCol)
        {
            int col = 0;
            string colName = iCol.ToString();

            string[] strValue ={"A","B","C","D","E","F","G","H","I","J","K",
                                   "L","M","N","O","P","Q","R","S","T","U",
                                   "V","W","X","Y","Z"};
            if (iCol > 0 && iCol < 27)
                return strValue[iCol - 1];


            if (colName.Length < 2)
            {
                colName = colName.PadLeft(2, '0');
            }


            string CC1 = "";
            int _iCC1 = int.Parse(colName.Substring(0, 1));
            if (_iCC1 > 0)
                CC1 = strValue[_iCC1 - 1];

            string CC2 = colName.Substring(1, 1);

            int _iCC2 = int.Parse(colName.Substring(1, 1));
            if (_iCC2 > 0)
                CC2 = strValue[_iCC2 - 1];

            return CC1 + CC2;
        }
        /// <summary>
        /// 设置单元格字体颜色
        /// int row 第几行
        /// int col 第几列
        /// Color color 颜色
        /// string SheetsName 页面名称
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="colorIndex"></param>
        public void SetCellsFontColor(int row, int col, Color color, string SheetsName)
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
            ((Range)_workSheet.Cells[row, col]).Font.Color = color.ToArgb();
        }

        /// <summary>
        /// 设置单元格背景颜色
        /// int row 第几行
        /// int col 第几列
        /// Color color 颜色
        /// string SheetsName 页面名称
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="colorIndex"></param>
        public void SetCellsBackColor(int row, int col, Color color, string SheetsName)
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
            Range excelRange = (Range)_workSheet.get_Range(_workSheet.Cells[row, col], _workSheet.Cells[row, col]);
            excelRange.Cells.Interior.Color = color.ToArgb();
        }

        /// <summary>
        /// 设置单元格字体样式
        /// int ColWidth 单元格宽度
        /// int row 第几行
        /// int col 第几列
        /// int size 字体大小
        /// bool IsUnderline 是否有下划线 true--是,false--否
        /// bool TextAlignment--文本是否水平居中 true--是，false--否
        /// bool wrapText--文本是否自动换行  true--是，false--否
        /// bool AutoFit--是否自动调整列宽度 ture--是，false--否
        /// string SheetsName 页面名称
        /// </summary>
        /// <param name="ColWidth"></param>
        /// <param name="size"></param>
        /// <param name="IsUnderline"></param>
        /// <param name="TextAlignment"></param>
        /// <param name="wrapText"></param>
        public void SetCellsFontStyle(int row, int col, int ColWidth, int size, bool IsUnderline, bool TextAlignment, bool wrapText, bool AutoFit, string SheetsName)
        {

            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
            Range excelRange = (Range)_workSheet.get_Range(_workSheet.Cells[row, col], _workSheet.Cells[row, col]);

            //设置字体大小
            excelRange.Font.Size = size;
            //设置字体是否有下划线
            excelRange.Font.Underline = IsUnderline;

            if (TextAlignment == true)
            {
                //文本水平居中方式
                excelRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            }

            //设置单元格的宽度
            excelRange.ColumnWidth = ColWidth;

            if (AutoFit == true)
            {
                //自动调整列宽
                excelRange.EntireColumn.AutoFit();
            }
            if (wrapText == true)
            {
                //文本自动换行
                excelRange.WrapText = true;
            }
        }


        /// <summary>
        /// 合并列
        /// int row  第几行
        /// int col  第几列
        /// int columsSum  合并的列数
        /// string SheetsName 页面名称
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="columsSum"></param>
        public void MergeCellsColums(int row, int col, int columsSum, string SheetsName = "Sheet1")
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
            _workSheet.get_Range(_workSheet.Cells[row, col], _workSheet.Cells[row, columsSum + col]);//.MergeCells = true;
        }
        public void MergeCellsColums_I_To_S(int row, int col, int columsSum, string SheetsName = "Sheet1")
        {
            columsSum--;
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
            _workSheet.get_Range(GetStrIndex_I_To_S(col) + row.ToString(), GetStrIndex_I_To_S(col + columsSum) + row.ToString())
                    .Merge(_workSheet.get_Range(GetStrIndex_I_To_S(col) + row.ToString(), GetStrIndex_I_To_S(col + columsSum) + row.ToString()).MergeCells);
        }
        public void MergeCells(string A,string B,int row,  string SheetsName = "Sheet1")
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
            _workSheet.get_Range(A+row .ToString (), B+row .ToString ()).MergeCells = true;
            _workSheet.get_Range(A + row.ToString(), B + row.ToString()).Borders.LineStyle  = 1;

        }

        /// <summary>
        /// 合并行
        /// int row  第几行
        /// int col  第几列
        /// int rowsSum  合并的行数数
        /// string SheetsName 页面名称
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="columsSum"></param>
        public void MergeCellsRows(int row, int col, int rowsSum, string SheetsName)
        {
            _workSheet = (Worksheet)_workBook.Sheets[SheetsName];
            _workSheet.get_Range(_workSheet.Cells[row, col], _workSheet.Cells[row + rowsSum, col]).MergeCells = true;
        }

        /// <summary>
        /// 杀掉EXCEL.exe进程
        /// </summary>
        public void killWinWordProcess()
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("EXCEL");
            foreach (System.Diagnostics.Process process in processes)
            {

                if (string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    process.Kill();
                }
            }
        }








    }
}
