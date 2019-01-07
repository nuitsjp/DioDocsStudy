using System;
using System.Collections.Generic;
using System.Linq;
using InvoiceBuilder.Repository;
using Mapster;

namespace InvoiceBuilder.UseCase.Impl
{
    public class BuildInvoice : IBuildInvoice
    {
        private readonly ISalesOrderRepository _salesOrderRepository;

        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;

        public BuildInvoice(ISalesOrderRepository salesOrderRepository, ISalesOrderDetailRepository salesOrderDetailRepository)
        {
            _salesOrderRepository = salesOrderRepository;
            _salesOrderDetailRepository = salesOrderDetailRepository;
        }

        public IEnumerable<SalesOrder> GetSalesOrders()
        {
            return _salesOrderRepository.Get().ToList();
        }

        public byte[] Build(SalesOrder salesOrder)
        {
            var salesOrderDetails = _salesOrderDetailRepository.Get(salesOrder.SalesOrderId);
            var invoice = salesOrder.Adapt<Invoice>();
            foreach (var salesOrderDetail in salesOrderDetails)
            {
                invoice.InvoiceDetails.Add(salesOrderDetail.Adapt<InvoiceDetail>());
            }
            return null;
        }

        private class Invoice
        {
            public int SalesOrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public string StoreName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
            public IList<InvoiceDetail> InvoiceDetails { get; } = new List<InvoiceDetail>();
        }

        private class InvoiceDetail
        {
            public int SalesOrderDetailId { get; set; }
            public int OrderQuantity { get; set; }
            public int UnitPrice { get; set; }
            public string ProductName { get; set; }
        }
    }
}