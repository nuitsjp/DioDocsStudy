using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using GrapeCity.Documents.Excel;
using Microsoft.Data.SqlClient;

namespace TemplateStudyFromDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            var workbook = new Workbook();
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

            using var dataSet = new DataSet();

            using var headerDataTable = dataSet.Tables.Add("InvoiceHeader");
            using var headerCommand = 
                new SqlCommand(
                    File.ReadAllText("SelectInvoiceHeader.sql"),
                    connection);
            headerDataTable.Load(headerCommand.ExecuteReader());

            using var dataTable = dataSet.Tables.Add("InvoiceDetails");
            using var command =
                new SqlCommand(
                    File.ReadAllText("SelectInvoiceDetails.sql"),
                    connection);
            dataTable.Load(command.ExecuteReader());


            workbook.AddDataSource("Invoice", dataSet);
            workbook.ProcessTemplate();

            workbook.Save("AppliedTemplate.xlsx");
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