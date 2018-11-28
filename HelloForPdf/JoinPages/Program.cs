using System;
using System.IO;
using System.Linq;
using DioDocsStudy.Pdf;
using GrapeCity.Documents.Pdf;

namespace JoinPages
{
    class Program
    {
        static void Main(string[] args)
        {
            PdfActivator.Activate();

            using (var outputStream = new FileStream("Result.pdf", FileMode.Create))
            {
                var newDoc = new GcPdfDocument();
                newDoc.StartDoc(outputStream);

                for (var i = 0; i < 100; i++)
                {
                    var invoice = new GcPdfDocument();
                    using (var inputStream = new FileStream("Invoice.pdf", FileMode.Open))
                    {
                        invoice.Load(inputStream);
                        var page = invoice.Pages.Single();
                        invoice.Pages.Remove(page);
                        newDoc.Pages.Add(page);
                    }
                }
                
                newDoc.EndDoc();
            }

            Console.WriteLine("Completed! Please press any key.");
            Console.ReadKey();
        }
    }
}
