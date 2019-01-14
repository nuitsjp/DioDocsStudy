using System;
using System.Collections.Generic;
using System.Linq;
using InvoiceBuilder.Report;
using InvoiceBuilder.Repository;

namespace InvoiceBuilder.UseCase.Impl
{
    public class BuildInvoice : IBuildInvoice
    {
        private readonly ISalesOrderRepository _salesOrderRepository;

        private readonly IInvoiceRepository _invoiceRepository;

        private readonly IReportService _reportService;

        public BuildInvoice(ISalesOrderRepository salesOrderRepository, IInvoiceRepository invoiceRepository, IReportService reportService)
        {
            _salesOrderRepository = salesOrderRepository;
            _invoiceRepository = invoiceRepository;
            _reportService = reportService;
        }

        public IList<SalesOrder> GetSalesOrders()
        {
            return _salesOrderRepository.Get().ToList();
        }

        public byte[] Build(int salesOrderId)
        {
            var invoice = _invoiceRepository.Get(salesOrderId);
            return _reportService.Build(invoice);
        }
    }
}