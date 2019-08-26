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

        public static void Build(Stream input, Stream output)
        {
            var workbook = new Workbook();
            workbook.Open(input);
            workbook.Save(output, SaveFileFormat.Pdf);
        }

    }
}
