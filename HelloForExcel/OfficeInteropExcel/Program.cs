using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace OfficeInteropExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            //var application = new Application { Visible = false };

            //var workbook = application.Workbooks.Add();
            //var worksheet = workbook.ActiveSheet as Worksheet;
            //worksheet.Range["C3"].Value = "Hello World!";
            //workbook.SaveAs(Path.Combine(Environment.CurrentDirectory, "Result.xlsx"));
            //workbook.Close();
            //application.Quit();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Application application = null;
            Workbooks workbooks = null;
            Workbook workbook = null;
            Worksheet worksheet = null;
            //Range range = null;
            try
            {
                application = new Application { Visible = false };

                workbooks = application.Workbooks;
                workbook = workbooks.Add();
                worksheet = (Worksheet)workbook.ActiveSheet;
                for (var i = 1; i <= 100; i++)
                {
                    for (var j = 1; j <= 100; j++)
                    {
                        Range range = null;
                        try
                        {
                            range = worksheet.Cells[i, j];
                            range.Value = "Hello World!";
                        }
                        finally
                        {
                            if (range != null) Marshal.ReleaseComObject(range);
                        }
                    }
                }                
                //range = worksheet.Range["C3"];
                //range.Value = "Hello World!";
                workbook.SaveAs(Path.Combine(Environment.CurrentDirectory, "Result.xlsx"));
                workbook.Close();
                application.Quit();
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //if (range != null) Marshal.ReleaseComObject(range);
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                if (workbook != null) Marshal.ReleaseComObject(workbook);
                if (workbooks != null) Marshal.ReleaseComObject(workbooks);
                if (application != null) Marshal.ReleaseComObject(application);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
            Console.ReadKey();
        }
    }
}
