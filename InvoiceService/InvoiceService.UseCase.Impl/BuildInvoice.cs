using System.Collections.Generic;
using System.Linq;
using InvoiceService.Repository;
using ReportService;

namespace InvoiceService.UseCase.Impl
{
    public class BuildInvoice : IBuildInvoice
    {
        private readonly ISalesOrderRepository _salesOrderRepository;

        private readonly IInvoiceRepository _invoiceRepository;

        private readonly IReportBuilderFactory _reportBuilderFactory;

        public BuildInvoice(ISalesOrderRepository salesOrderRepository, IInvoiceRepository invoiceRepository, IReportBuilderFactory reportBuilderFactory)
        {
            _salesOrderRepository = salesOrderRepository;
            _invoiceRepository = invoiceRepository;
            _reportBuilderFactory = reportBuilderFactory;
        }

        public IList<SalesOrder> GetSalesOrders()
        {
            return _salesOrderRepository.Get().ToList();
        }

        public byte[] Build(int salesOrderId)
        {
            var invoice = _invoiceRepository.Get(salesOrderId);
            var reportBuilder = 
                _reportBuilderFactory.Create<Invoice, InvoiceDetail>("InvoiceDetails")
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
            return reportBuilder.Build(invoice.InvoiceDetails);
        }
    }
}