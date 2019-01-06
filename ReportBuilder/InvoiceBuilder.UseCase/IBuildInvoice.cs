using System.Collections.Generic;

namespace InvoiceBuilder.UseCase
{
    public interface IBuildInvoice
    {
        IEnumerable<SalesOrder> GetSalesOrders();

        byte[] Build(SalesOrder salesOrder);
    }
}