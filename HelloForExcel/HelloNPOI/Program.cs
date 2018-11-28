using System;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HelloNPOI
{
    class Program
    {
        static void Main(string[] args)
        {
            var workbook = new XSSFWorkbook();
            var worksheet = workbook.CreateSheet("Sheet1");
            worksheet.CreateRow(2).CreateCell(2).SetCellValue("Hello World!");
            using (var stream = new FileStream("Result.xlsx", FileMode.Create))
            {
                workbook.Write(stream);
            }
        }
    }
}
