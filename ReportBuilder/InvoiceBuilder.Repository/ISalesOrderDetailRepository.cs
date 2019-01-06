using System.Collections.Generic;
using InvoiceBuilder.UseCase;

namespace InvoiceBuilder.Repository
{
    public interface ISalesOrderDetailRepository
    {
        IEnumerable<SalesOrderDetail> Get(int salesOrderId);
    }
}