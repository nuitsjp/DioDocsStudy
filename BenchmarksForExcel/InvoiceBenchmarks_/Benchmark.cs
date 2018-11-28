using System;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using DioDocsStudy.Excel;
using GrapeCity.Documents.Excel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;

namespace InvoiceBenchmarks
{
    [ClrJob(baseline: true)]//, CoreJob]
    public class Benchmark
    {
        //[Params(1)]
        [Params(1000)]
        public int N;

        [GlobalSetup]
        public void Setup() => ExcelActivator.Activate();

        private Invoice Invoice { get; } =
            new Invoice
            {
                No = 1234567890,
                BillingDate = DateTime.Today,
                PaymentDeadline = DateTime.Today.Add(TimeSpan.FromDays(30)),
                Customer = new Customer
                {
                    ZipCode = "108-0023",
                    Address = "東京都港区芝浦3-4-1 グランパークタワー",
                    Name = "ニュイ工房",
                    Staff = "中村 充志"
                },
                InvoiceDetails = new List<InvoiceDetail>
                {
                    new InvoiceDetail{ Name = "Item1", UnitPrice = 1000, Count = 1, Unit = "本"},
                    new InvoiceDetail{ Name = "Item2", UnitPrice = 2000, Count = 2, Unit = "冊"},
                    new InvoiceDetail{ Name = "Item3", UnitPrice = 3000, Count = 3, Unit = "枚"},
                    new InvoiceDetail{ Name = "Item4", UnitPrice = 4000, Count = 4, Unit = "㌢"},
                    new InvoiceDetail{ Name = "Item5", UnitPrice = 5000, Count = 5, Unit = "㌕"},
                    new InvoiceDetail{ Name = "Item6", UnitPrice = 6000, Count = 6, Unit = "年"},
                    new InvoiceDetail{ Name = "Item7", UnitPrice = 7000, Count = 7, Unit = "個"},
                    new InvoiceDetail{ Name = "Item8", UnitPrice = 8000, Count = 8, Unit = "個"},
                    new InvoiceDetail{ Name = "Item9", UnitPrice = 9000, Count = 9, Unit = "個"},
                    new InvoiceDetail{ Name = "Item10", UnitPrice = 10000, Count = 10, Unit = "個"},
                }
            };

        [Benchmark]
        public void DioDocs()
        {
            var workbook = new Workbook();
            workbook.Open("Invoice.xlsx");
            var worksheet = workbook.ActiveSheet;

            worksheet.Range["N3"].Value = Invoice.No;
            worksheet.Range["N4"].Value = Invoice.BillingDate;
            worksheet.Range["O13"].Value = Invoice.PaymentDeadline;
            worksheet.Range["B3"].Value = Invoice.Customer.ZipCode;
            worksheet.Range["A4"].Value = Invoice.Customer.Address;
            worksheet.Range["A5"].Value = Invoice.Customer.Name;
            worksheet.Range["C6"].Value = Invoice.Customer.Staff;

            for (var i = 0; i < Invoice.InvoiceDetails.Count; i++)
            {
                worksheet.Range[15 + i, 1].Value = Invoice.InvoiceDetails[i].Name;
                worksheet.Range[15 + i, 9].Value = Invoice.InvoiceDetails[i].UnitPrice;
                worksheet.Range[15 + i, 12].Value = Invoice.InvoiceDetails[i].Count;
                worksheet.Range[15 + i, 14].Value = Invoice.InvoiceDetails[i].Unit;
            }

            workbook.Save("Result.xlsx");
        }

        [Benchmark]
        public void ClosedXML()
        {
            using (var workbook = new XLWorkbook(new FileStream("Invoice.xlsx", FileMode.Open)))
            {
                var worksheet = workbook.Worksheet(1);

                worksheet.Range("N3").Value = Invoice.No;
                worksheet.Range("N4").Value = Invoice.BillingDate;
                worksheet.Range("O13").Value = Invoice.PaymentDeadline;
                worksheet.Range("B3").Value = Invoice.Customer.ZipCode;
                worksheet.Range("A4").Value = Invoice.Customer.Address;
                worksheet.Range("A5").Value = Invoice.Customer.Name;
                worksheet.Range("C6").Value = Invoice.Customer.Staff;

                for (var i = 0; i < Invoice.InvoiceDetails.Count; i++)
                {
                    worksheet.Cell(16 + i, 2).Value = Invoice.InvoiceDetails[i].Name;
                    worksheet.Cell(16 + i, 10).Value = Invoice.InvoiceDetails[i].UnitPrice;
                    worksheet.Cell(16 + i, 13).Value = Invoice.InvoiceDetails[i].Count;
                    worksheet.Cell(16 + i, 14).Value = Invoice.InvoiceDetails[i].Unit;
                }

                workbook.SaveAs("Result.xlsx");
            }
        }

        [Benchmark]
        public void EPPlus()
        {
            using (var package = new ExcelPackage(new FileInfo("Invoice.xlsx")))
            {
                var worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells.Style.Font.Name = "游ゴシック";

                worksheet.Cells["N3"].Value = Invoice.No;
                worksheet.Cells["N4"].Value = Invoice.BillingDate;
                worksheet.Cells["O13"].Value = Invoice.PaymentDeadline;
                worksheet.Cells["B3"].Value = Invoice.Customer.ZipCode;
                worksheet.Cells["A4"].Value = Invoice.Customer.Address;
                worksheet.Cells["A5"].Value = Invoice.Customer.Name;
                worksheet.Cells["C6"].Value = Invoice.Customer.Staff;

                for (var i = 0; i < Invoice.InvoiceDetails.Count; i++)
                {
                    worksheet.Cells[15 + i, 1].Value = Invoice.InvoiceDetails[i].Name;
                    worksheet.Cells[15 + i, 9].Value = Invoice.InvoiceDetails[i].UnitPrice;
                    worksheet.Cells[15 + i, 12].Value = Invoice.InvoiceDetails[i].Count;
                    worksheet.Cells[15 + i, 14].Value = Invoice.InvoiceDetails[i].Unit;
                }


                package.SaveAs(new FileInfo("Result.xlsx"));
            }
        }

        [Benchmark]
        public void NPOI()
        {
            var workbook = new XSSFWorkbook("Invoice.xlsx");

            var worksheet = workbook.GetSheetAt(0);
            worksheet.GetRow(2).GetCell(13).SetCellValue(Invoice.No);
            worksheet.GetRow(3).CreateCell(13).SetCellValue(Invoice.BillingDate);
            worksheet.GetRow(12).CreateCell(14).SetCellValue(Invoice.PaymentDeadline);
            worksheet.GetRow(2).CreateCell(1).SetCellValue(Invoice.Customer.ZipCode);
            worksheet.GetRow(3).CreateCell(0).SetCellValue(Invoice.Customer.Address);
            worksheet.GetRow(4).CreateCell(0).SetCellValue(Invoice.Customer.Name);
            worksheet.GetRow(5).CreateCell(2).SetCellValue(Invoice.Customer.Staff);

            for (var i = 0; i < Invoice.InvoiceDetails.Count; i++)
            {
                worksheet.GetRow(15 + i).GetCell(1).SetCellValue(Invoice.InvoiceDetails[i].Name);
                worksheet.GetRow(15 + i).GetCell(9).SetCellValue(Invoice.InvoiceDetails[i].UnitPrice);
                worksheet.GetRow(15 + i).GetCell(12).SetCellValue(Invoice.InvoiceDetails[i].Count);
                worksheet.GetRow(15 + i).GetCell(14).SetCellValue(Invoice.InvoiceDetails[i].Unit);
            }

            workbook.Write(new FileStream("Result.xlsx", FileMode.Create));
        }
    }
}
