using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;

namespace InvoiceMaker.ClosedXml
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

            using (var workbook = new XLWorkbook(new FileStream("Invoice.xlsx", FileMode.Open)))
            {
                var worksheet = workbook.Worksheet(1);

                worksheet.Range("N3").Value = invoice.No;
                worksheet.Range("N4").Value = invoice.BillingDate;
                worksheet.Range("O13").Value = invoice.PaymentDeadline;
                worksheet.Range("B3").Value = invoice.Customer.ZipCode;
                worksheet.Range("A4").Value = invoice.Customer.Address;
                worksheet.Range("A5").Value = invoice.Customer.Name;
                worksheet.Range("C6").Value = invoice.Customer.Staff;

                for (var i = 0; i < invoice.InvoiceDetails.Count; i++)
                {
                    worksheet.Cell(16 + i, 2).Value = invoice.InvoiceDetails[i].Name;
                    worksheet.Cell(16 + i, 10).Value = invoice.InvoiceDetails[i].UnitPrice;
                    worksheet.Cell(16 + i, 13).Value = invoice.InvoiceDetails[i].Count;
                    worksheet.Cell(16 + i, 14).Value = invoice.InvoiceDetails[i].Unit;
                }

                workbook.SaveAs(Stream.Null);
            }
        }
    }
}
