using System.Collections.Generic;

namespace InvoiceBuilder.Repository
{
    public interface ISalesOrderRepository
    {
        IEnumerable<SalesOrder> Get();
    }
}