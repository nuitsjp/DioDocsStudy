using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace HelloOpenXml
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var spreadsheetDocument =
                SpreadsheetDocument.Create("Result.xlsx", SpreadsheetDocumentType.Workbook))
            {
                // Add a WorkbookPart to the document.
                var workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

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
                sheetData.Append(new Row());
                sheetData.Append(new Row());

                // Row 3
                var row = new Row();
                var cell = new Cell
                {
                    DataType = CellValues.String,
                    CellReference = "C3",
                    CellValue = new CellValue("Hello World!")
                };
                row.Append(cell);
                sheetData.Append(row);


                workbookpart.Workbook.Save();

                // Close the document.
                spreadsheetDocument.Close();
            }
        }
    }
}
