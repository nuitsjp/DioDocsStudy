using System;
using System.Collections.Generic;
using System.Text;

namespace ReportService.DioDocs
{
    public class Range : IRange
    {
        internal GrapeCity.Documents.Excel.IRange DioDocsRange { private get; set; }

        public object Value
        {
            set => DioDocsRange.Value = value;
        }
    }
}
