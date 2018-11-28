using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace BenchmarksForFramework
{
    public class InteropExcel
    {
        public static void Run()
        {
            Application application = null;
            Workbooks workbooks = null;
            Workbook workbook = null;
            Worksheet worksheet = null;
            try
            {
                application = new Application { Visible = false };

                workbooks = application.Workbooks;
                workbook = workbooks.Add();
                worksheet = (Worksheet)workbook.ActiveSheet;

                for (var i = 1; i <= Benchmark.ColumnNum; i++)
                {
                    for (var j = 1; j <= Benchmark.RowNum; j++)
                    {
                        Range range = null;
                        try
                        {
                            range = worksheet.Cells[i, j];
                            range.Value = "Hello World!";
                        }
                        finally
                        {
                            ReleaseComObject(range);
                        }
                    }
                }
                var file = Path.GetTempFileName();
                try
                {
                    workbook.SaveAs(file);
                    using (var fileStream = new FileStream(file, FileMode.Open))
                    using (var memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                    }
                    workbook.Close();
                    application.Quit();
                }
                finally
                {
                    File.Delete(file);
                }
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                ReleaseComObject(worksheet);
                ReleaseComObject(workbook);
                ReleaseComObject(workbooks);
                ReleaseComObject(application);
            }
        }

        private static void ReleaseComObject(object o)
        {
            if (o != null)
                Marshal.ReleaseComObject(o);
        }
    }
}
