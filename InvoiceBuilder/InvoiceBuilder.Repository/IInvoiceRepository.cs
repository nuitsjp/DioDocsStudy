namespace InvoiceBuilder.Repository
{
    public interface IInvoiceRepository
    {
        Invoice Get(int salesOrderId);
    }
}