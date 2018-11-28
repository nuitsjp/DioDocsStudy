using System;
using System.Collections.Generic;

namespace InvoiceMaker
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
