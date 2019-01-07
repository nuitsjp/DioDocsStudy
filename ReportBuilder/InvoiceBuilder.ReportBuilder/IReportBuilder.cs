namespace InvoiceBuilder.ReportBuilder
{
    public interface IReportBuilder
    {
        byte[] Build(object o);
    }
}