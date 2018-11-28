using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using DioDocsStudy.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using Workbook = GrapeCity.Documents.Excel.Workbook;

namespace Benchmarks
{
    [ClrJob(baseline: true), CoreJob]
    public class Benchmark
    {
        //[Params(10000)]
        [Params(1000)]
        public int N;

        [GlobalSetup]
        public void Setup() => ExcelActivator.Activate();

        private const int ColumnNum = 100;
        private const int RowNum = 100;

        [Benchmark]
        public void DioDocs()
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

            workbook.Save(Stream.Null);
        }

        [Benchmark]
        public void ClosedXML()
        {
            using (var workbook = new XLWorkbook())
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

                workbook.SaveAs(Stream.Null);
            }
        }


        [Benchmark]
        public void OpenXml()
        {
            using (var spreadsheetDocument =
                SpreadsheetDocument.Create(Stream.Null, SpreadsheetDocumentType.Workbook))
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
                package.SaveAs(Stream.Null);
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
            workbook.Write(Stream.Null);
        }
    }
}