using System;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style.XmlAccess;

namespace HelloEPPlus
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells.Style.Font.Name = "游ゴシック";
                worksheet.Cells["C3"].Value = "Hello World!";
                package.SaveAs(new FileInfo("Result.xlsx"));
            }
        }
    }
}
