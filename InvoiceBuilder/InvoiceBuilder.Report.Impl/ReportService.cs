using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrapeCity.Documents.Excel;

namespace InvoiceBuilder.Report.Impl
{
    public class ReportService : IReportService
    {
        private readonly ITemplateService _templateService;

        public ReportService(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        public byte[] Build(Invoice invoice)
        {
            var reportBuilder = 
                new ReportBuilder<InvoiceDetail>("InvoiceDetails")
                    .AddSetter("$SalesOrderId", cell => cell.Value = invoice.SalesOrderId)
                    .AddSetter("$OrderDate", cell => cell.Value = invoice.OrderDate)
                    .AddSetter("$CompanyName", cell => cell.Value = invoice.CompanyName)
                    .AddSetter("$Name", cell => cell.Value = invoice.FirstName + " " + invoice.LastName)
                    .AddSetter("$Address", cell => cell.Value = invoice.AddressLine1 + " " + invoice.AddressLine2 + " " + invoice.City + " " + invoice.State)
                    .AddSetter("$PostalCode", cell => cell.Value = invoice.PostalCode)
                    .AddTableSetter("$ProductName", (range, detail) => range.Value = detail.ProductName)
                    .AddTableSetter("$UnitPrice", (range, detail) => range.Value = detail.UnitPrice)
                    .AddTableSetter("$OrderQuantity", (range, detail) => range.Value = detail.OrderQuantity);
            using (var stream = new MemoryStream(_templateService.Get()))
            {
                return reportBuilder.Build(stream, invoice.InvoiceDetails);
            }
        }
    }
}