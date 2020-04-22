using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using GrapeCity.Documents.Excel;
using Microsoft.Data.SqlClient;
using TemplateStudy;

namespace TemplateStudyFromDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            var workbook = new Workbook(Secrets.Key);
            workbook.Open("Invoice.xlsx");


            var connectionStringBuilder = 
                new SqlConnectionStringBuilder
                {
                    DataSource = "localhost",
                    UserID = "sa",
                    Password = "P@ssw0rd!"
                };
            using var connection = new SqlConnection(connectionStringBuilder.ToString());
            connection.Open();

            ProcessTemplate(connection, workbook);
        }

        private static void ProcessTemplate(SqlConnection connection, Workbook workbook)
        {
            using var command =
                new SqlCommand(
                    File.ReadAllText("SelectInvoices.sql"),
                    connection);
            using var dataTable = new DataTable();
            dataTable.Load(command.ExecuteReader());

            workbook.AddDataSource("Invoice", dataTable);
            workbook.ProcessTemplate();

            workbook.Save("AppliedTemplate.pdf", SaveFileFormat.Pdf);
        }
    }

    public class InvoiceHeader
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