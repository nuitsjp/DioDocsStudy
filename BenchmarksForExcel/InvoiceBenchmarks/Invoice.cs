using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceBenchmarks
{
    public class Invoice
    {
        public int No { get; set; }
        public DateTime BillingDate { get; set; }
        public DateTime PaymentDeadline { get; set; }
        public Customer Customer { get; set; }
        public IList<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
