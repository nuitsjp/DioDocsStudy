﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using Dapper;
using GrapeCity.Documents.Excel;
using ReportService.DioDocs;

namespace InvoiceService.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Workbook.SetLicenseKey(Properties.Settings.Default.DioDocsForExcelKey);

            var settings = ConfigurationManager.ConnectionStrings["DioDocs"];
            var factory = DbProviderFactories.GetFactory(settings.ProviderName);
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = settings.ConnectionString;
                connection.Open();

                const int salesOrderId = 71936;
                var invoice = connection
                    .Query<Invoice>(
                        Properties.Resources.SelectInvoice,
                        new { SalesOrderId = salesOrderId })
                    .Single();
                invoice.InvoiceDetails.AddRange(
                    connection
                        .Query<InvoiceDetail>(
                            Properties.Resources.SelectInvoiceDetail,
                            new { SalesOrderId = salesOrderId }));

                using (var stream = new MemoryStream(Properties.Resources.Invoice))
                {
                    var reportBuilder =
                        new ReportBuilder<InvoiceDetail>(stream)
                            // 単一項目のSetterを設定
                            .AddSetter("$SalesOrderId", cell => cell.Value = invoice.SalesOrderId)
                            .AddSetter("$OrderDate", cell => cell.Value = invoice.OrderDate)
                            .AddSetter("$CompanyName", cell => cell.Value = invoice.CompanyName)
                            .AddSetter("$Name", cell => cell.Value = invoice.FirstName + " " + invoice.LastName)
                            .AddSetter("$Address", cell => cell.Value = invoice.AddressLine1 + " " + invoice.AddressLine2 + " " + invoice.City + " " + invoice.State)
                            .AddSetter("$PostalCode", cell => cell.Value = invoice.PostalCode)
                            // テーブルのセルに対するSetterを設定
                            .AddTableSetter("$ProductName", (range, detail) => range.Value = detail.ProductName)
                            .AddTableSetter("$UnitPrice", (range, detail) => range.Value = detail.UnitPrice)
                            .AddTableSetter("$OrderQuantity", (range, detail) => range.Value = detail.OrderQuantity);
                    var report = reportBuilder.Build(invoice.InvoiceDetails);

                    File.WriteAllBytes("result.pdf", report);
                }

            }
        }
    }

    public class Invoice
    {
        public int SalesOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; } = new List<InvoiceDetail>();
    }

    public class InvoiceDetail
    {
        public int OrderQuantity { get; set; }
        public int UnitPrice { get; set; }
        public string ProductName { get; set; }
    }
}