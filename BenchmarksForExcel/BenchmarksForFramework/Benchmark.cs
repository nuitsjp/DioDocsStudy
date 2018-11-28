using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using DioDocsStudy.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Excel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using Sheets = DocumentFormat.OpenXml.Spreadsheet.Sheets;
using Workbook = GrapeCity.Documents.Excel.Workbook;
using Worksheet = DocumentFormat.OpenXml.Spreadsheet.Worksheet;

namespace BenchmarksForFramework
{
    public class Benchmark
    {
        //[Params(1000, 10000)]
        [Params(10)]
        public int N;

        [GlobalSetup]
        public void Setup() => ExcelActivator.Activate();

        public const int ColumnNum = 100;
        public const int RowNum = 100;

        [Benchmark]
        public void OfficeInteropExcel()
        {
            InteropExcel.Run();
        }

        [Benchmark]
        public void ClosedXML()
        {
            using (var workbook = new XLWorkbook())
            using (var stream = new MemoryStream())
            {
                workbook.Style.Font.FontName = "游ゴシック";
                var worksheet = workbook.Worksheets.Add("Sheet1");
                for (var i = 1; i <= ColumnNum; i++)
                {
                    for (var j = 1; j <= RowNum; j++)
                    {
                        worksheet.Cell(i, j).Value = "Hello World!";
                    }
                }

                workbook.SaveAs(stream);
            }
        }

        [Benchmark]
        public void DioDocs()
        {
            using (var stream = new MemoryStream())
            {
                var workbook = new Workbook();
                var worksheet = workbook.ActiveSheet;
                for (var i = 1; i <= ColumnNum; i++)
                {
                    for (var j = 1; j <= RowNum; j++)
                    {
                        worksheet.Range[i, j].Value = "Hello World!";
                    }
                }

                workbook.Save(stream);
            }
        }

        [Benchmark]
        public void OpenXml()
        {
            using (var stream = new MemoryStream())
            using (var spreadsheetDocument =
                SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                // Add a WorkbookPart to the document.
                var workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                // Add a WorksheetPart to the WorkbookPart.
                var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                // Add Sheets to the Workbook.
                var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

                // Append a new worksheet and associate it with the workbook.
                var sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Sheet1"
                };
                sheets.Append(sheet);

                // Append Row 1 and 2.
                for (var i = 1; i <= ColumnNum; i++)
                {
                    for (var j = 1; j <= RowNum; j++)
                    {
                        var row = new Row();
                        var cell = new Cell
                        {
                            DataType = CellValues.String,
                            CellValue = new CellValue("Hello World!")
                        };
                        row.Append(cell);
                        sheetData.Append(row);
                    }
                }

                workbookpart.Workbook.Save();

                // Close the document.
                spreadsheetDocument.Close();
            }
        }

        [Benchmark]
        public void EPPlus()
        {
            using (var package = new ExcelPackage())
            using (var stream = new MemoryStream())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.Style.Font.Name = "游ゴシック";
                for (var i = 1; i <= ColumnNum; i++)
                {
                    for (var j = 1; j <= RowNum; j++)
                    {
                        worksheet.Cells[i, j].Value = "Hello World!";
                    }
                }
                package.SaveAs(stream);
            }
        }

        [Benchmark]
        public void NPOI()
        {
            var workbook = new XSSFWorkbook();
            var worksheet = workbook.CreateSheet("Sheet1");
            for (var i = 1; i <= ColumnNum; i++)
            {
                for (var j = 1; j <= RowNum; j++)
                {
                    worksheet.CreateRow(i).CreateCell(j).SetCellValue("Hello World!");
                }
            }
            using (var stream = new MemoryStream())
            {
                workbook.Write(stream);
            }

        }
    }
}
