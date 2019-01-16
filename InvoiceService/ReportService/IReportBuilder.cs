using System;
using System.Collections.Generic;

namespace ReportService
{
    public interface IReportBuilder<TReportRow>
    {
        IReportBuilder<TReportRow> AddSetter(object key, Action<IRange> setter);

        IReportBuilder<TReportRow> AddTableSetter(string key, Action<IRange, TReportRow> setter);

        byte[] Build(IList<TReportRow> rows);
    }
}