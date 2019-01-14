namespace ReportService
{
    public interface IReportBuilderFactory
    {
        IReportBuilder<TReport, TReportRow> Create<TReport, TReportRow>(string tableName);
    }
}