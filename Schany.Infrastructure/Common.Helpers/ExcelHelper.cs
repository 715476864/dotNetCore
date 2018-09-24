using Microsoft.AspNetCore.Hosting;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Schany.Infrastructure.Common.Helpers
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 将excel文件内容读取到DataTable数据表中
        /// </summary>
        /// <param name="fileName">文件完整路径名</param>
        /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名：true=是，false=否</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable ReadExcelToDataTable(string fileName,
            string sheetName = null,
            bool isFirstRowColumn = true)
        {
            //定义要返回的datatable对象
            DataTable data = new DataTable();
            //excel工作表
            ISheet sheet = null;
            //数据开始行(排除标题行)
            int startRow = 0;
            try
            {
                if (!File.Exists(fileName))
                {
                    return null;
                }
                //根据指定路径读取文件
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //根据文件流创建excel数据结构
                IWorkbook workbook = WorkbookFactory.Create(fs);
                //如果有指定工作表名称
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    //如果没有指定的sheetName，则尝试获取第一个sheet
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    //一行最后一个cell的编号 即总的列数
                    int cellCount = firstRow.LastCellNum;
                    //如果第一行是标题列名
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                //读完删除文件
                File.Delete(fileName);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将DataTable数据表内容读取到Excel导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="baseUri"></param>
        /// <param name="hostingEnvironment"></param>
        /// <returns></returns>
        public static Notification ExportDataTableToExcel(DataTable dt, 
            string baseUri, 
            IHostingEnvironment hostingEnvironment)
        {
            string fileName = Guid.NewGuid().ToString() + ".xlsx";
            string fileDir = hostingEnvironment.WebRootPath + "\\Content\\TempFiles\\";
            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }
            string filePath = "/Content/TempFiles/" + fileName;

            IWorkbook workbook = new NPOI.XSSF.UserModel.XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet1");
            IRow Title = sheet.CreateRow(0);
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                Title.CreateCell(k).SetCellValue(dt.Columns[k].ColumnName);
            }

            IRow rows = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                rows = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    rows.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            using (var stream = new FileStream(fileDir + fileName, FileMode.Create))
            {
                try
                {
                    workbook.Write(stream);
                    return new Notification(NotifyType.Success, "导出成功", new
                    {
                        Name = fileName,
                        Url = baseUri + filePath,
                        _Url = filePath
                    });
                }
                catch (Exception ex)
                {
                    return new Notification(NotifyType.Error, string.Format("导出成功：{0}。", ex.Message));
                }
            }
        }

    }
}
