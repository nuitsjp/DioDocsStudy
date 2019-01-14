using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceBuilder.UseCase
{
    public interface IBuildInvoice
    {
        IList<SalesOrder> GetSalesOrders();

        byte[] Build(int salesOrderId);
    }
}
