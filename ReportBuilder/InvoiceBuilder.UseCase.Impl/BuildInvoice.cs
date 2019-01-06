using System.Collections.Generic;
using System.Linq;
using InvoiceBuilder.Repository;

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
            return null;
        }
    }
}