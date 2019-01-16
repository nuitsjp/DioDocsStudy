using System.Collections.Generic;
using System.IO;
using System.Linq;
using InvoiceService.Repository;
using ReportService;
using Mapster;
using ReportService.DioDocs;

namespace InvoiceService.UseCase.Impl
{
    public class BuildInvoice : IBuildInvoice
    {
        private readonly ISalesOrderRepository _salesOrderRepository;

        private readonly IInvoiceRepository _invoiceRepository;

        private readonly IReportBuilderFactory _reportBuilderFactory;

        private readonly ITemplateProvider _templateProvider;

        public BuildInvoice(ISalesOrderRepository salesOrderRepository, IInvoiceRepository invoiceRepository, IReportBuilderFactory reportBuilderFactory, ITemplateProvider templateProvider)
        {
            _salesOrderRepository = salesOrderRepository;
            _invoiceRepository = invoiceRepository;
            _reportBuilderFactory = reportBuilderFactory;
            _templateProvider = templateProvider;
        }

        public IList<SalesOrder> GetSalesOrders()
        {
            return _salesOrderRepository.Get().Select(x => x.Adapt<SalesOrder>()).ToList();
        }

        public byte[] Build(int salesOrderId)
        {
            var invoice = _invoiceRepository.Get(salesOrderId);
            using (var stream = new MemoryStream(_templateProvider.Get()))
            {
                var reportBuilder =
                    _reportBuilderFactory.Create<InvoiceDetail>(stream)
                        // 単一項目のSetterを設定
                        .AddSetter("$SalesOrderId", cell => cell.Value = invoice.SalesOrderId)
                        .AddSetter("$OrderDate", cell => cell.Value = invoice.OrderDate)
                        .AddSetter("$CompanyName", cell => cell.Value = invoice.CompanyName)
                        .AddSetter("$Name", cell => cell.Value = invoice.Name)
                        .AddSetter("$Address", cell => cell.Value = invoice.Address)
                        .AddSetter("$PostalCode", cell => cell.Value = invoice.PostalCode)
                        // テーブルのセルに対するSetterを設定
                        .AddTableSetter("$ProductName", (cell, detail) => cell.Value = detail.ProductName)
                        .AddTableSetter("$UnitPrice", (cell, detail) => cell.Value = detail.UnitPrice)
                        .AddTableSetter("$OrderQuantity", (cell, detail) => cell.Value = detail.OrderQuantity);
                return reportBuilder.Build(invoice.InvoiceDetails);
            }
        }
    }
}