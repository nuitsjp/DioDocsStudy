using System;
using GrapeCity.Documents.Excel;

namespace DioDocsStudy.Excel
{
    public static class ExcelActivator
    {
        public static void Activate()
        {
            Workbook.SetLicenseKey("Your License Key");
        }
    }
}
