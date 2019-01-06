using System;

namespace InvoiceBuilder.UseCase
{
    public class SalesOrder
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
    }
}
