using System.Threading.Tasks;

namespace InvoiceBuilder.ReportBuilder
{
    public interface IReportBuilder
    {
        Task<byte[]> Build(object o);
    }
}