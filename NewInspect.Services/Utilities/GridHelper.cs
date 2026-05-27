using System;
using System.Reflection;
using System.Windows.Forms;
using ClassLib_TestData;
using ClassLibrary_Interface;

// ============================================================
// 文件: GridHelper.cs
// 位置: NewInspect.Services/Utilities/
// 命名空间: NewInspect.Services.Utilities
// 职责: DataGridView 辅助方法 — 从 SysInfo.cs 迁移
//       - Grd_AddData 所有重载 (8 个)
//       - Grd_IniGrid_ShowZdName (DataGridView 初始化)
//       - DublGrid (双缓冲扩展)
// ============================================================

namespace NewInspect.Services.Utilities
{
    /// <summary>
    /// DataGridView 辅助类 — 替代 SysInfo 中的列表操作方法
    /// </summary>
    public static class GridHelper
    {
        private static readonly ClassLibrary_Interface.ClassInterFace _ini = new ClassLibrary_Interface.ClassInterFace();
        private static readonly string HardConfigPath = Application.StartupPath + "\\database\\HardConfig.ini";

        // ========================================================
        // Grd_AddData — 检测项目 (Class_Test_Item)
        // ========================================================
        public static bool GrdAddData(DataGridView grid, int iR, Class_Test_Item testItem)
        {
            try
            {
                int iRow = iR + 1;
                int iCol = 0;
                if (iRow + 1 > grid.RowCount) return false;
                if (iCol + 1 > grid.ColumnCount) return false;
                if (iCol < 0 || iRow < 1) return false;

                CenterAlignAll(grid, iRow);
                grid[iCol++, iRow].Value = iRow.ToString();
                grid[iCol++, iRow].Value = testItem.ID;
                grid[iCol++, iRow].Value = testItem.Dwmc;
                grid[iCol++, iRow].Value = testItem.ItemName;
                grid[iCol++, iRow].Value = testItem.Sbbh;
                grid[iCol++, iRow].Value = testItem.Testblock;
                grid[iCol++, iRow].Value = testItem.Testing_Standard;
                grid[iCol++, iRow].Value = testItem.strJyy;
                grid.FirstDisplayedScrollingRowIndex = iRow;
                return true;
            }
            catch { return false; }
        }

        // ========================================================
        // Grd_AddData — 焊缝记录 (Class_Test_Parts)
        // ========================================================
        public static bool GrdAddData(DataGridView grid, int iR, Class_Test_Parts testPart)
        {
            try
            {
                int iRow = iR + 1;
                int iCol = 0;
                if (iRow + 1 > grid.RowCount) return false;
                if (iCol + 1 > grid.ColumnCount) return false;
                if (iCol < 0 || iRow < 1) return false;

                CenterAlignAll(grid, iRow);
                grid[iCol, iRow].Value = iRow.ToString(); iCol++;
                grid[iCol, iRow].Value = testPart.Sub_ID; iCol++;
                grid[iCol, iRow].Value = testPart.Part_No; iCol++;
                grid[iCol, iRow].Value = testPart.DetectionSite == 0 ? "外壁" : "内壁"; iCol++;
                grid[iCol, iRow].Value = testPart.flThicknise; iCol++;
                grid[iCol, iRow].Value = testPart.ProbeSpacing; iCol++;
                grid[iCol, iRow].Value = testPart.ReMark;
                return true;
            }
            catch { return false; }
        }

        // ========================================================
        // Grd_AddData — 报警/缺陷 (Class_Test_AlarmArea)
        // ========================================================
        public static bool GrdAddData(DataGridView grid, int iR, Class_Test_AlarmArea alarm)
        {
            try
            {
                int iRow = iR + 1;
                int iCol = 0;
                if (iRow + 1 > grid.RowCount) return false;
                if (iCol + 1 > grid.ColumnCount) return false;
                if (iCol < 0 || iRow < 1) return false;

                CenterAlignAll(grid, iRow);
                grid[iCol++, iRow].Value = iRow.ToString();
                grid[iCol++, iRow].Value = alarm.ID;
                grid[iCol++, iRow].Value = alarm.Sub_ID;
                grid[iCol++, iRow].Value = alarm.Part_No;
                grid[iCol++, iRow].Value = alarm.strType.ToString();
                grid[iCol++, iRow].Value = alarm.flLen.ToString();
                grid[iCol++, iRow].Value = alarm.flLen_S.ToString();
                grid[iCol++, iRow].Value = alarm.flLen_E.ToString();
                grid[iCol++, iRow].Value = alarm.iX_No;
                grid[iCol++, iRow].Value = alarm.iY_No;
                grid[iCol++, iRow].Value = alarm.flHeight;
                grid[iCol++, iRow].Value = alarm.flHeight_S;
                grid[iCol++, iRow].Value = alarm.flHeight_E;
                grid.FirstDisplayedScrollingRowIndex = iRow;
                return true;
            }
            catch { return false; }
        }

        // ========================================================
        // Grd_AddData — 打印项目 (ClPrintItem)
        // ========================================================
        public static bool GrdAddData(DataGridView grid, int iR, ClassLibrary_Interface.Models.ClPrintItem printItem)
        {
            try
            {
                int iRow = iR + 1;
                int iCol = 0;
                if (iRow > grid.RowCount) return false;
                if (iCol + 1 > grid.ColumnCount) return false;
                if (iCol < 0 || iRow < 1) return false;

                CenterAlignAll(grid, iRow);
                grid[iCol++, iRow].Value = iRow.ToString();
                grid[iCol++, iRow].Value = printItem.ItemName;
                grid.FirstDisplayedScrollingRowIndex = iRow;
                return true;
            }
            catch { return false; }
        }

        // ========================================================
        // Grd_AddData — 打印记录 (ClPrintRecord)
        // ========================================================
        public static bool GrdAddData(DataGridView grid, int iR, ClassLibrary_Interface.Models.ClPrintRecord printRecord)
        {
            try
            {
                int iRow = iR + 1;
                int iCol = 0;
                if (iRow + 1 > grid.RowCount) return false;
                if (iCol + 1 > grid.ColumnCount) return false;
                if (iCol < 0 || iRow < 1) return false;

                CenterAlignAll(grid, iRow);
                grid[iCol++, iRow].Value = iRow.ToString();
                grid[iCol++, iRow].Value = printRecord.IsChecked;
                grid[iCol++, iRow].Value = printRecord.RecordName;
                grid.FirstDisplayedScrollingRowIndex = iRow;
                return true;
            }
            catch { return false; }
        }

        // ========================================================
        // Grd_AddData — 报表参数 (Cls_Report_P)
        // ========================================================
        public static bool GrdAddData(DataGridView grid, int iR, Cls_Report_P report)
        {
            try
            {
                int iRow = iR + 1;
                int iCol = 0;
                if (iRow + 1 > grid.RowCount) return false;
                if (iCol + 1 > grid.ColumnCount) return false;
                if (iCol < 0 || iRow < 1) return false;

                CenterAlignAll(grid, iRow);
                grid[iCol++, iRow].Value = iRow.ToString();
                grid[iCol++, iRow].Value = report.Txt_Wtdw;
                grid[iCol++, iRow].Value = report.Txt_Gcmc;
                grid[iCol++, iRow].Value = report.Txt_Gjmc;
                grid[iCol++, iRow].Value = report.Txt_Gjbh;
                grid[iCol++, iRow].Value = report.Txt_Jcff;
                grid[iCol++, iRow].Value = report.Txt_Jcrq;
                grid[iCol++, iRow].Value = report.Txt_Jcjgmc;
                grid[iCol++, iRow].Value = report.Txt_Jcjgdz;
                grid[iCol++, iRow].Value = report.Txt_Yb;
                grid[iCol++, iRow].Value = report.Txt_Bg_Bgbh;
                grid[iCol++, iRow].Value = report.Txt_Bz;
                grid[iCol++, iRow].Value = report.Txt_Bg_Jlbh;
                grid[iCol++, iRow].Value = report.Txt_Jcr;
                grid[iCol++, iRow].Value = report.Txt_Shr;
                grid[iCol++, iRow].Value = report.Txt_Gg;
                grid[iCol++, iRow].Value = report.Txt_CL;
                grid[iCol++, iRow].Value = report.Txt_Hjff;
                grid[iCol++, iRow].Value = report.Txt_Pkxs;
                grid[iCol++, iRow].Value = report.Txt_Rclzt;
                grid[iCol++, iRow].Value = report.Txt_Bmzt;
                grid[iCol++, iRow].Value = report.Txt_Jcbw;
                grid[iCol++, iRow].Value = report.Txt_Jcsj;
                grid[iCol++, iRow].Value = report.Txt_Bmwd;
                grid[iCol++, iRow].Value = report.Txt_Cysblb;
                grid[iCol++, iRow].Value = report.Txt_Jcbl;
                grid[iCol++, iRow].Value = report.Txt_Jcbz;
                grid[iCol++, iRow].Value = report.Txt_Hgjb;
                grid[iCol++, iRow].Value = report.Txt_Jsdj;
                grid[iCol++, iRow].Value = report.Txt_CzZdsbh;
                grid[iCol++, iRow].Value = report.Txt_Yqmc;
                grid[iCol++, iRow].Value = report.Txt_Yqxh;
                grid[iCol++, iRow].Value = report.Txt_Yqbh;
                grid[iCol++, iRow].Value = report.Txt_Sczz;
                grid[iCol++, iRow].Value = report.Txt_Sk;
                grid[iCol++, iRow].Value = report.Txt_Ohj;
                grid[iCol++, iRow].Value = report.Txt_Wd;
                grid[iCol++, iRow].Value = report.Txt_Jcm;
                grid[iCol++, iRow].Value = report.Txt_Jcqy;
                grid[iCol++, iRow].Value = report.Txt_Tt_Td;
                grid[iCol++, iRow].Value = report.Txt_Tt_Xh;
                grid[iCol++, iRow].Value = report.Txt_Tt_Bh;
                grid[iCol++, iRow].Value = report.Txt_Tt_Lmd;
                grid[iCol++, iRow].Value = report.Txt_Tt_Sjck;
                grid[iCol++, iRow].Value = report.Txt_PL;
                grid[iCol++, iRow].Value = report.Txt_Jpcc;
                grid[iCol++, iRow].Value = report.Txt_Xkjd;
                grid[iCol++, iRow].Value = report.Txt_PCS;
                grid[iCol++, iRow].Value = report.Txt_Scbj;
                grid[iCol++, iRow].Value = report.Txt_Scfs;
                grid.FirstDisplayedScrollingRowIndex = iRow;
                return true;
            }
            catch { return false; }
        }

        // ========================================================
        // Grd_AddData — 报警区域(带序号) (Class_Test_AlarmArea + iNo)
        // ========================================================
        public static bool GrdAddData(DataGridView grid, int iR, int iNo, Class_Test_AlarmArea alarm)
        {
            try
            {
                int iRow = iR + 1;
                int iCol = 0;
                if (iRow + 1 > grid.RowCount) return false;
                if (iCol + 1 > grid.ColumnCount) return false;
                if (iCol < 0 || iRow < 1) return false;

                CenterAlignAll(grid, iRow);
                grid[iCol++, iRow].Value = iRow.ToString();
                grid[iCol++, iRow].Value = alarm.Sub_ID;
                grid[iCol++, iRow].Value = alarm.flLen_S;
                grid[iCol++, iRow].Value = "";
                grid[iCol++, iRow].Value = alarm.flLen_S;
                grid[iCol++, iRow].Value = alarm.flLen;
                grid[iCol++, iRow].Value = alarm.flDepth;
                grid[iCol++, iRow].Value = alarm.flHeight;
                grid[iCol++, iRow].Value = alarm.strType;
                grid[iCol++, iRow].Value = alarm.strZldj;
                grid[iCol++, iRow].Value = alarm.strFileName;
                grid[iCol++, iRow].Value = alarm.Remark;
                grid.FirstDisplayedScrollingRowIndex = iRow;
                return true;
            }
            catch { return false; }
        }

        // ========================================================
        // Grd_AddData_Print — 打印报警 (Class_Test_AlarmArea)
        // ========================================================
        public static bool GrdAddDataPrint(DataGridView grid, int iR, Class_Test_AlarmArea alarm)
        {
            try
            {
                int iRow = iR + 1;
                int iCol = 0;
                if (iRow + 1 > grid.RowCount) return false;
                if (iCol + 1 > grid.ColumnCount) return false;
                if (iCol < 0 || iRow < 1) return false;

                CenterAlignAll(grid, iRow);
                grid[iCol++, iRow].Value = iRow.ToString();
                grid[iCol++, iRow].Value = alarm.ID;
                grid[iCol++, iRow].Value = alarm.Sub_ID;
                grid[iCol++, iRow].Value = alarm.Part_No;
                grid[iCol++, iRow].Value = alarm.strType.ToString();
                grid[iCol++, iRow].Value = alarm.flLen.ToString();
                grid[iCol++, iRow].Value = alarm.flLen_S.ToString();
                grid.FirstDisplayedScrollingRowIndex = iRow;
                return true;
            }
            catch { return false; }
        }

        // ========================================================
        // Grd_IniGrid_ShowZdName — 初始化 DataGridView 列头
        // ========================================================
        /// <summary>
        /// 从 INI 配置初始化 DataGridView 列 — 替代 SysInfo.Grd_IniGrid_ShowZdName()
        /// iType: 0=项目记录 1=焊缝记录 2=异常数据标注
        /// </summary>
        public static void InitGridColumns(DataGridView grid, int iType)
        {
            try
            {
                string strSection;
                switch (iType)
                {
                    case 0: strSection = "IniGrid_Item";    break;
                    case 1: strSection = "IniGrid_Weld";    break;
                    case 2: strSection = "IniGrid_TOFD_Bz"; break;
                    default: return;
                }

                int iMax = int.Parse(_ini.INIReadValue(strSection, "sum", "0", HardConfigPath));
                if (iMax == 0) return;

                // 按 P0, P1, P2... 读取列定义: 字段名|标题名|控件类型(0=文本框,1=下拉框)|是否显示(0/1)|列宽度
                grid.Columns.Clear();
                for (int i = 0; i < iMax; i++)
                {
                    string para = _ini.INIReadValue(strSection, "P" + i, "", HardConfigPath);
                    if (string.IsNullOrEmpty(para)) continue;

                    string[] arr = para.Split('|');
                    if (arr.Length < 5) continue;

                    string colName  = arr[0];  // 字段名
                    string colTitle = arr[1];  // 标题
                    int    ctrlType = int.Parse(arr[2]); // 0=TextBox 1=ComboBox
                    int    visible  = int.Parse(arr[3]); // 0=隐藏 1=显示
                    int    colWidth = int.Parse(arr[4]); // 列宽

                    DataGridViewColumn col;
                    if (ctrlType == 0)
                        col = new DataGridViewTextBoxColumn();
                    else
                        col = new DataGridViewComboBoxColumn();

                    col.Name = colName;
                    col.HeaderText = colTitle;
                    col.Width = colWidth;
                    col.Visible = visible == 1;
                    grid.Columns.Add(col);
                }
            }
            catch { }
        }

        // ========================================================
        // 内部辅助
        // ========================================================
        private static void CenterAlignAll(DataGridView grid, int row)
        {
            try
            {
                for (int i = 0; i < grid.ColumnCount; i++)
                    grid[i, row].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch { }
        }
    }

    // ========================================================
    // DublGrid — DataGridView 双缓冲扩展
    // ========================================================
    /// <summary>
    /// DataGridView 双缓冲扩展 — 替代 SysInfo.DublGrid
    /// </summary>
    public static class DublGrid
    {
        /// <summary>启用/禁用 DataGridView 双缓冲</summary>
        public static void SetDoubleBuffered(this DataGridView dgv, bool enabled)
        {
            var dgvType = dgv.GetType();
            var pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi?.SetValue(dgv, enabled, null);
        }
    }
}
