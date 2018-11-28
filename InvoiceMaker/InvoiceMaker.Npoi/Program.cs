using System;
using System.Collections.Generic;
using System.IO;
using NPOI.XSSF.UserModel;

namespace InvoiceMaker.Npoi
{
    class Program
    {
        static void Main(string[] args)
        {

            var invoice = new Invoice
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
            var workbook = new XSSFWorkbook("Invoice.xlsx");

            var worksheet = workbook.GetSheetAt(0);
            worksheet.GetRow(2).GetCell(13).SetCellValue(invoice.No);
            worksheet.GetRow(3).CreateCell(13).SetCellValue(invoice.BillingDate);
            worksheet.GetRow(12).CreateCell(14).SetCellValue(invoice.PaymentDeadline);
            worksheet.GetRow(2).CreateCell(1).SetCellValue(invoice.Customer.ZipCode);
            worksheet.GetRow(3).CreateCell(0).SetCellValue(invoice.Customer.Address);
            worksheet.GetRow(4).CreateCell(0).SetCellValue(invoice.Customer.Name);
            worksheet.GetRow(5).CreateCell(2).SetCellValue(invoice.Customer.Staff);

            for (var i = 0; i < invoice.InvoiceDetails.Count; i++)
            {
                worksheet.GetRow(15 + i).GetCell(1).SetCellValue(invoice.InvoiceDetails[i].Name);
                worksheet.GetRow(15 + i).GetCell(9).SetCellValue(invoice.InvoiceDetails[i].UnitPrice);
                worksheet.GetRow(15 + i).GetCell(12).SetCellValue(invoice.InvoiceDetails[i].Count);
                worksheet.GetRow(15 + i).GetCell(14).SetCellValue(invoice.InvoiceDetails[i].Unit);
            }

            workbook.Write(new FileStream("Result.xlsx", FileMode.Create));

            Console.WriteLine("Completed! Please press any key.");
            Console.ReadKey();
        }
    }
}
