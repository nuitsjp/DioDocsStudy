using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using GrapeCity.Documents.Excel;

namespace TemplateStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            var workbook = new Workbook(Secrets.Key);
            workbook.Open("Invoice.xlsx");

            using var invoiceStream = File.OpenRead("invoice.xml");
            
            var serializer = new XmlSerializer(typeof(Invoice));
            var invoice = (Invoice)serializer.Deserialize(invoiceStream);

            workbook.AddDataSource("Invoice", invoice);
            workbook.ProcessTemplate();

            workbook.Save("AppliedTemplate.xlsx");
        }
    }

    public class Invoice
    {
        public int SalesOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; } = new List<InvoiceDetail>();
    }

    public class InvoiceDetail
    {
        public int OrderQuantity { get; set; }
        public int UnitPrice { get; set; }
        public string ProductName { get; set; }
    }
}
