using System.Collections.Generic;
using InvoiceBuilder.UseCase;

namespace InvoiceBuilder.Repository
{
    public interface ISalesOrderRepository
    {
        IEnumerable<SalesOrder> Get();
    }
}