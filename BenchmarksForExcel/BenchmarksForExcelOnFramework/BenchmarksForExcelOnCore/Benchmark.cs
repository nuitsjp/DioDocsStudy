using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Workbook = GrapeCity.Documents.Excel.Workbook;

namespace BenchmarksForExcelOnFramework
{
    public class Benchmark
    {
        //[Params(1000, 10000)]
        //[Params(1000)]
        [Params(2, 4, 6, 8, 16, 32)]
        //[Params(32)]
        public int N;

        [GlobalSetup]
        public void Setup() => ExcelActivator.Activate();

        [IterationSetup]
        public void IterationSetup()
        {
            if (File.Exists("Result.xlsx"))
                File.Delete("Result.xlsx");
        }

        private const int ColumnNum = 100;
        private const int RowNum = 100;

        private const int TotalExecCount = 32 * 10;

        private void Run(Action action)
        {
            if (32 < N)
            {
                action();
            }
            else
            {
                var tasks = new List<Task>();
                var threadCount = 32 / N;
                var execCount = TotalExecCount / threadCount;
                for (var i = 0; i < threadCount; i++)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        for (var j = 0; j < execCount; j++)
                        {
                            action();
                        }
                    }));
                }

                Task.WaitAll(tasks.ToArray());
            }
        }

        [Benchmark]
        public void ClosedXML()
        {
            Run(() =>
            {
                using (var workbook = new XLWorkbook())
                using(var stream = new MemoryStream())
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
            });
        }

        [Benchmark]
        public void DioDocs()
        {
            Run(() =>
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
            });
        }

        [Benchmark]
        public void OpenXml()
        {
            Run(() =>
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
            });
        }
    }
}