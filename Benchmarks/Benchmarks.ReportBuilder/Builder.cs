using System;
using System.IO;
using GrapeCity.Documents.Excel;

namespace Benchmarks.ReportBuilder
{
    public class Builder
    {
        static Builder()
        {
            Workbook.SetLicenseKey(Secrets.DioDocsKey);
        }

        public static void Build(Stream input)
        {
            var workbook = new Workbook();
            workbook.Open(input);
            workbook.Save(Stream.Null, SaveFileFormat.Pdf);
        }

    }
}
