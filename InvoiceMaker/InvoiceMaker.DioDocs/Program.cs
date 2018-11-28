using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DioDocsStudy.Excel;
using DioDocsStudy.Pdf;
using GrapeCity.Documents.Excel;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Pdf.AcroForms;

namespace InvoiceMaker.DioDocs
{
    class Program
    {
        static void Main(string[] args)
        {
            ExcelActivator.Activate();

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

            var workbook = new Workbook();
            workbook.Open("Invoice.xlsx");
            var worksheet = workbook.ActiveSheet;
            worksheet.Range["N3"].Value = invoice.No;
            worksheet.Range["N4"].Value = invoice.BillingDate;
            worksheet.Range["O13"].Value = invoice.PaymentDeadline;
            worksheet.Range["B3"].Value = invoice.Customer.ZipCode;
            worksheet.Range["A4"].Value = invoice.Customer.Address;
            worksheet.Range["A5"].Value = invoice.Customer.Name;
            worksheet.Range["C6"].Value = invoice.Customer.Staff;

            for (var i = 0; i < invoice.InvoiceDetails.Count; i++)
            {
                worksheet.Range[15 + i, 1].Value = invoice.InvoiceDetails[i].Name;
                worksheet.Range[15 + i, 9].Value = invoice.InvoiceDetails[i].UnitPrice;
                worksheet.Range[15 + i, 12].Value = invoice.InvoiceDetails[i].Count;
                worksheet.Range[15 + i, 14].Value = invoice.InvoiceDetails[i].Unit;
            }

            workbook.Save("Result.pdf", SaveFileFormat.Pdf);
            //workbook.Save("Result.xlsx");

            PdfActivator.Activate();

            var doc = new GcPdfDocument();
            using (var fileStream = new FileStream("Result.pdf", FileMode.Open, FileAccess.Read))
            {
                doc.Load(fileStream);

                const string fontname = "Yu Gothic";

                var cert = new X509Certificate2(File.ReadAllBytes("diodocs.cer"), "diodocs");
                var sp = new SignatureProperties();
                sp.Certificate = cert;
                sp.Location = "DioDocs for PDF サンプルブラウザ";
                sp.SignerName = "DioDocs";

                // 署名を保持する署名フィールドを初期化
                SignatureField sf = new SignatureField();
                sf.Widget.Rect = new RectangleF(72, 72 * 2, 72 * 4, 36);
                sf.Widget.Page = doc.Pages.First();
                sf.Widget.BackColor = Color.LightSeaGreen;
                sf.Widget.TextFormat.FontName = fontname;
                sf.Widget.ButtonAppearance.Caption = $"署名者: {sp.SignerName}\r\n所属: {sp.Location}";
                // ドキュメントに署名フィールドを追加
                doc.AcroForm.Fields.Add(sf);

                // 署名フィールドと署名を結びつけ
                sp.SignatureField = sf;

                doc.Sign(sp, "Signed.pdf");
            }

            Console.WriteLine("Completed! Please press any key.");
            Console.ReadKey();
        }
    }
}
