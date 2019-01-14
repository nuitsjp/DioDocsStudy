namespace ReportService.DioDocs
{
    public class ReportBuilderFactory : IReportBuilderFactory
    {
        private readonly ITemplateProvider _templateProvider;

        public ReportBuilderFactory(ITemplateProvider templateProvider)
        {
            _templateProvider = templateProvider;
        }

        public IReportBuilder<TReport, TReportRow> Create<TReport, TReportRow>(string tableName)
        {
            return new ReportBuilder<TReport, TReportRow>(tableName, _templateProvider);
        }
    }
}