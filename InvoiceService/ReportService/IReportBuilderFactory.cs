using System.IO;

namespace ReportService
{
    public interface IReportBuilderFactory
    {
        IReportBuilder<TReportRow> Create<TReportRow>(Stream excel);
    }
}