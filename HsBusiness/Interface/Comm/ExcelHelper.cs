
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace HsBusiness.Interface.Comm
{
    public static class ExcelHelper
    {
        /**
下载excel代码。
DataTable table = new DataTable();
MemoryStream ms = DataTableRenderToExcel.RenderDataTableToExcel(table) as MemoryStream;
Response.AddHeader("Content-Disposition", string.Format("attachment; filename=Download.xls"));
Response.BinaryWrite(ms.ToArray());
ms.Close();
ms.Dispose();
上传excel取数据用代码
if (this.fuUpload.HasFile)
{
    // 讀取 Excel 資料流並轉換成 DataTable。
    DataTable table = DataTableRenderToExcel.RenderDataTableFromExcel(this.fuUpload.FileContent, 1, 0);
    this.gvExcel.DataSource = table;
    this.gvExcel.DataBind();
}
         * **/

        public static Stream RowsToExcel(IEnumerable<string[]> rows)
        {
            var workbook = new HSSFWorkbook();
            var ms = new MemoryStream();

            ISheet sheet = workbook.CreateSheet();

            // handling value.
            int rowIndex = 0;

            foreach (var row in rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex++);
                for (int i = 0; i < row.Length; i++)
                {
                    dataRow.CreateCell(i).SetCellValue(row[i]);
                }
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            workbook = null;

            return ms;
        }

        /// <summary>
        /// 衡水驾驶协会   身份证等信息特殊处理
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static Stream RowsToExcelHSJS(IEnumerable<string[]> rows)
        {
            var workbook = new HSSFWorkbook();
            var ms = new MemoryStream();

            ISheet sheet = workbook.CreateSheet();

            // handling value.
            int rowIndex = 0;
            int rowlength = 0;

            foreach (var row in rows)
            {
                rowlength = row.Length;
                IRow dataRow = sheet.CreateRow(rowIndex++);
                for (int i = 0; i < row.Length; i++)
                {
                    //创建第i个单元格     
                    ICell cell = dataRow.CreateCell(i);
                    //dataRow.CreateCell(i).SetCellValue(row[i]);
                    cell.SetCellValue(row[i]);

                    ICellStyle cellStyle2 = workbook.CreateCellStyle();
                    IDataFormat format = workbook.CreateDataFormat();
                    cellStyle2.DataFormat = format.GetFormat("@");
                    cell.CellStyle = cellStyle2;
                }
            }

            for (int row = 0; row < 230; row++)
            {
                IRow dataRow = sheet.CreateRow(rowIndex++);
                for (int i = 0; i < rowlength; i++)
                {
                    //创建第i个单元格     
                    ICell cell = dataRow.CreateCell(i);
                    //dataRow.CreateCell(i).SetCellValue(row[i]);

                    ICellStyle cellStyle2 = workbook.CreateCellStyle();
                    IDataFormat format = workbook.CreateDataFormat();
                    cellStyle2.DataFormat = format.GetFormat("@");
                    cell.CellStyle = cellStyle2;
                }
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            workbook = null;

            return ms;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SourceTable"></param>
        /// <returns></returns>
        public static Stream RenderDataTableToExcel(DataTable SourceTable)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // handling value.
            int rowIndex = 1;

            foreach (DataRow row in SourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

        public static void RenderDataTableToExcel(DataTable SourceTable, string FileName)
        {
            MemoryStream ms = RenderDataTableToExcel(SourceTable) as MemoryStream;
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            byte[] data = ms.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();

            data = null;
            ms = null;
            fs = null;
        }

        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, string SheetName, int HeaderRowIndex)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            ISheet sheet = workbook.GetSheet(SheetName);

            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                    dataRow[j] = row.GetCell(j).ToString();
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        public static DataTable RenderDataTableFromExcel(Stream ExcelFileStream, int SheetIndex, int HeaderRowIndex)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(ExcelFileStream);
            ISheet sheet = workbook.GetSheetAt(SheetIndex);

            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(HeaderRowIndex);
            int cellCount = headerRow.LastCellNum;

            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            int rowCount = sheet.LastRowNum;

            for (int i = (sheet.FirstRowNum + 1); i < sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                table.Rows.Add(dataRow);
            }

            ExcelFileStream.Close();
            workbook = null;
            sheet = null;
            return table;
        }

        /// <summary>读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="path">excel文档路径</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(string path, int sheetindex)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfworkbook.GetSheetAt(sheetindex);// (sheetname);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        public static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();

                data = null;
            }
        }

        public static void RenderToBrowser(MemoryStream ms, HttpContext context, string fileName)
        {

            fileName = HttpUtility.UrlEncode(fileName);
            context.Response.AddHeader("Content-Disposition", "attachment;fileName=" + fileName);
            context.Response.BinaryWrite(ms.ToArray());
        }

        public static IEnumerable<string[]> ReadExcelToString(string path, int sheetIndex, int startRow)
        {
            IEnumerable<string[]> rows;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var workbook = new HSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                rows = sheet.ToStringRows(startRow).ToArray();
                sheet = null;
                workbook = null;

                fs.Close();
            }
            return rows;
        }

        /// <summary>
        /// 读取指定区域的excel 开始行,开始列,结束列
        /// </summary>
        public static IEnumerable<string[]> ReadExcelToString(string path, int sheetIndex, int startRow, int startCol, int endCol)
        {
            IEnumerable<string[]> dt = null;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var workbook = new HSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                dt = sheet.ToStringRows(startRow, startCol, endCol);
                workbook = null;
                sheet = null;
                fs.Close();

            }
            return dt;
        }

        #region function

        public static IEnumerable<ISheet> GetSheets(this IWorkbook book)
        {
            for (int i = 0; i < book.NumberOfSheets; i++)
            {
                yield return book.GetSheetAt(i);
            }
        }

        public static DataSet ToDataSet(IEnumerable<ISheet> sheets)
        {
            DataSet ds = new DataSet("dataset");
            foreach (ISheet sheet in sheets)
            {
                ds.Tables.Add(sheet.ToDataTable());
            }
            return ds;
        }

        public static DataTable ToDataTable(this ISheet sheet, bool HDR = true)
        {
            DataTable dt = new DataTable(sheet.SheetName);

            int start, end;
            IRow headerRow = sheet.GetRow(0);//第一行为标题行
            start = headerRow.FirstCellNum;
            end = headerRow.LastCellNum;

            IEnumerable<string> names;
            if (HDR == true)
                names = GetColumns(headerRow);
            else
                names = GetDefaultColumns(end - start);

            foreach (string name in names)
            {
                dt.Columns.Add(name, typeof(string));
            }

            for (int i = HDR ? (sheet.FirstRowNum + 1) : sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                var dataRow = dt.NewRow();

                if (row != null)
                {
                    for (int j = start; j < end; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = row.GetCell(j).StringCellValue;
                    }
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        public static IEnumerable<string[]> ToStringRows(this ISheet sheet, int startRow)
        {
            IRow headerRow = sheet.GetRow(startRow);//第一行为起始行 or 标题行
            if (headerRow == null) headerRow = sheet.GetRow(sheet.FirstRowNum);
            int start = headerRow.FirstCellNum;
            int end = headerRow.LastCellNum;

            for (int i = startRow; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null)
                {
                    var cells = new string[end - start + 1];
                    for (int j = start; j < end; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell != null) cells[j] = cell.ToString();
                    }
                    yield return cells;
                }
            }
        }

        public static DataTable ToDataTable(this ISheet sheet, int startRow, bool HDR = true)
        {
            DataTable dt = new DataTable(sheet.SheetName);

            int start, end;
            IRow headerRow = sheet.GetRow(startRow);//第一行为起始行 or 标题行
            start = headerRow.FirstCellNum;
            end = headerRow.LastCellNum;

            IEnumerable<string> names;
            if (HDR == true)
                names = GetColumns(headerRow);
            else
                names = GetDefaultColumns(end - start);

            foreach (string name in names)
            {
                dt.Columns.Add(name, typeof(string));
            }

            for (int i = HDR ? (startRow + 1) : startRow; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                var dataRow = dt.NewRow();

                if (row != null)
                {
                    for (int j = start; j < end; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null) continue;
                        switch (cell.CellType)
                        {
                            case CellType.String:
                                dataRow[j - start] = cell.StringCellValue;
                                break;
                            case CellType.Numeric:
                                dataRow[j - start] = row.GetCell(j).NumericCellValue;
                                break;
                            case CellType.Formula:
                                throw new Exception("Execl格式错误! 只能为数值或字符串!");
                            default:
                                dataRow[j - start] = row.GetCell(j).ToString();
                                break;
                        }
                    }
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        public static DataTable ToDataTable(this ISheet sheet, int startRow, int columnCount)
        {
            DataTable dt = new DataTable(sheet.SheetName);

            int start, end;
            IRow headerRow = sheet.GetRow(startRow);//第一行为起始行 or 标题行
            start = headerRow.FirstCellNum;
            end = columnCount + headerRow.FirstCellNum;

            var names = GetDefaultColumns(end - start);

            foreach (string name in names)
            {
                dt.Columns.Add(name, typeof(string));
            }

            for (int i = startRow; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                var dataRow = dt.NewRow();

                if (row != null)
                {
                    for (int j = start; j < end; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null) continue;
                        switch (cell.CellType)
                        {
                            case CellType.String:
                                dataRow[j - start] = cell.StringCellValue;
                                break;
                            case CellType.Numeric:
                                dataRow[j - start] = row.GetCell(j).NumericCellValue;
                                break;
                            case CellType.Formula:
                                throw new Exception("Execl格式错误! 只能为数值或字符串!");
                            default:
                                dataRow[j - start] = row.GetCell(j).ToString();
                                break;
                        }
                    }
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        public static DataTable ToDataTable(this ISheet sheet, int startRow, int startCol, int endCol)
        {
            var dt = new DataTable(sheet.SheetName);

            var names = GetDefaultColumns(endCol - startCol + 1);
            foreach (string name in names)
            {
                dt.Columns.Add(name, typeof(string));
            }

            IRow firstRow = sheet.GetRow(startRow);
            try
            {

                for (int i = startRow; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    var dataRow = dt.NewRow();

                    for (int j = startCol; j <= endCol; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null) continue;

                        switch (cell.CellType)
                        {
                            case CellType.String:
                                dataRow[j - startCol] = cell.StringCellValue;
                                break;
                            case CellType.Numeric:
                                dataRow[j - startCol] = row.GetCell(j).NumericCellValue;
                                break;
                            case CellType.Formula:
                                throw new Exception("Execl格式错误! 只能为数值或字符串!");
                            default:
                                dataRow[j - startCol] = row.GetCell(j).ToString();
                                break;
                        }
                    }
                    dt.Rows.Add(dataRow);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public static IEnumerable<string[]> ToStringRows(this ISheet sheet, int startRow, int startCol, int endCol)
        {
            IRow firstRow = sheet.GetRow(startRow);

            for (int i = startRow; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                string[] array = new string[endCol - startCol + 1];

                for (int j = startCol; j <= endCol; j++)
                {
                    var cell = row.GetCell(j);
                    if (cell == null) continue;

                    switch (cell.CellType)
                    {
                        case CellType.String:
                            array[j - startCol] = cell.StringCellValue;
                            break;
                        case CellType.Numeric:
                            array[j - startCol] = row.GetCell(j).NumericCellValue.ToString();
                            break;
                        case CellType.Formula:
                            throw new Exception("Execl格式错误! 只能为数值或字符串!");
                        default:
                            array[j - startCol] = row.GetCell(j).ToString();
                            break;
                    }
                }
                yield return array;
            }
        }

        #endregion

        #region column

        public static IEnumerable<string> GetColumns(IRow row)
        {
            int count = row.LastCellNum - row.FirstCellNum;
            List<string> list = new List<string>(count);//list 当字典用
            for (int i = row.FirstCellNum; i < row.LastCellNum; i++)
            {
                string name = row.GetCell(i).ToString();
                if (!list.Contains(name))
                    list.Add(name);
                else
                    return GetDefaultColumns(count);
            }
            return list.ToArray();
        }

        public static IEnumerable<string> GetDefaultColumns(int count, string name = "Column")
        {
            for (int i = 0; i < count; i++)
            {
                yield return name + i;
            }
        }

        #endregion

        #region Validate

        /// <summary>
        /// 验证数组
        /// </summary>
        /// <param name="columns">要验证的列</param>
        /// <param name="names">真实列</param>
        /// <returns></returns>
        public static bool ValidateColumn(string[] columns, string[] names)
        {
            if (columns.Length < names.Length) return false;
            for (int i = 0; i < columns.Length && i < names.Length; i++)
            {
                if (columns[i].Trim() != names[i].Trim())
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}