using System.Threading.Tasks;

namespace InvoiceBuilder.Report
{
    public interface IReportService
    {
        byte[] Build(Invoice invoice);
    }
}