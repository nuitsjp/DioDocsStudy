using System;
using System.Collections.Generic;

namespace ReportService
{
    public interface IReportBuilder<TReport, TReportRow>
    {
        IReportBuilder<TReport, TReportRow> AddSetter(object key, Action<IField> setter);

        IReportBuilder<TReport, TReportRow> AddTableSetter(string key, Action<IField, TReportRow> setter);

        byte[] Build(IList<TReportRow> rows);
    }
}