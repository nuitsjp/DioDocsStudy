using System;
using GrapeCity.Documents.Pdf;

namespace DioDocsStudy.Pdf
{
    public static class PdfActivator
    {
        public static void Activate()
        {
            GcPdfDocument.SetLicenseKey("Your License Key");
        }
    }
}
