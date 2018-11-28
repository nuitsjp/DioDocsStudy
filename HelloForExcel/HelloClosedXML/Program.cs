using System;
using ClosedXML.Excel;

namespace HelloClosedXML
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var workbook = new XLWorkbook())
            {
                workbook.Style.Font.FontName = "游ゴシック";
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cell("C3").Value = "Hello World!";
                workbook.SaveAs("Result.xlsx");
            }
        }
    }
}
