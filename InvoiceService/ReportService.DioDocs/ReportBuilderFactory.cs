﻿using System.IO;

namespace ReportService.DioDocs
{
    public class ReportBuilderFactory : IReportBuilderFactory
    {
        public IReportBuilder<TReportRow> Create<TReportRow>(Stream excel)
        {
            return new ReportBuilder<TReportRow>(excel);
        }
    }
}