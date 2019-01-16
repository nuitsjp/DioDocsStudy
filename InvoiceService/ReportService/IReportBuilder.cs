using System;
using System.Collections.Generic;

namespace ReportService
{
    public interface IReportBuilder<TReportRow>
    {
        IReportBuilder<TReportRow> AddSetter(object key, Action<IField> setter);

        IReportBuilder<TReportRow> AddTableSetter(string key, Action<IField, TReportRow> setter);

        byte[] Build(IList<TReportRow> rows);
    }
}