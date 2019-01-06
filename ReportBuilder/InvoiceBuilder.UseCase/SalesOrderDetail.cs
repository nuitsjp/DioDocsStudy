namespace InvoiceBuilder.UseCase
{
    public class SalesOrderDetail
    {
        public int SalesOrderDetailId { get; set; }
        public int OrderQuantity { get; set; }
        public int UnitPrice { get; set; }
        public string ProductName { get; set; }
    }
}
