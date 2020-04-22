using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Dapper;
using GrapeCity.Documents.Excel;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using TemplateStudy;

namespace DataSourceBenckmark
{
    public class Template
    {
        private byte[] WorkbookBytes { get; } = File.ReadAllBytes("Invoice.xlsx");

        private DataSet InvoiceDataSet { get; set; }

        private Invoice Invoice { get; set; }

        [Benchmark]
        public void DataSet()
        {
            var workbook = new Workbook(Secrets.Key);
            workbook.Open("Invoice.xlsx");


            workbook.AddDataSource("Invoice", InvoiceDataSet);
            workbook.ProcessTemplate();
        }

        [Benchmark]
        public void CustomObject()
        {
            var workbook = new Workbook(Secrets.Key);
            workbook.Open("Invoice.xlsx");


            workbook.AddDataSource("Invoice", Invoice);
            workbook.ProcessTemplate();
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            LoadDataSet();
            LoadCustomObject();
        }

        private void LoadDataSet()
        {
            var connectionStringBuilder =
                new SqlConnectionStringBuilder
                {
                    DataSource = "localhost",
                    UserID = "sa",
                    Password = "P@ssw0rd!"
                };
            using var connection = new SqlConnection(connectionStringBuilder.ToString());
            connection.Open();

            InvoiceDataSet = new DataSet();

            var headerDataTable = InvoiceDataSet.Tables.Add("InvoiceHeader");

            using var headerCommand =
                new SqlCommand(
                    File.ReadAllText("SelectInvoiceHeader.sql"),
                    connection);
            headerDataTable.Load(headerCommand.ExecuteReader());

            var dataTable = InvoiceDataSet.Tables.Add("InvoiceDetails");

            using var command =
                new SqlCommand(
                    File.ReadAllText("SelectInvoiceDetails.sql"),
                    connection);
            dataTable.Load(command.ExecuteReader());
        }

        private void LoadCustomObject()
        {
            var connectionStringBuilder =
                new SqlConnectionStringBuilder
                {
                    DataSource = "localhost",
                    UserID = "sa",
                    Password = "P@ssw0rd!"
                };
            using var connection = new SqlConnection(connectionStringBuilder.ToString());
            connection.Open();

            Invoice = new Invoice();
            Invoice.InvoiceHeader = connection.QuerySingle<InvoiceHeader>(File.ReadAllText("SelectInvoiceHeader.sql"));
            Invoice.InvoiceDetails =
                connection.Query<InvoiceDetail>(File.ReadAllText("SelectInvoiceDetails.sql")).ToList();
        }


        [GlobalCleanup]
        public void GlobalCleanup()
        {
            InvoiceDataSet.Dispose();
        }
    }

    public class Invoice
    {
        public InvoiceHeader InvoiceHeader { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; set; }
    }

    public class InvoiceHeader
    {
        public int SalesOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }

    public class InvoiceDetail
    {
        public int OrderQuantity { get; set; }
        public int UnitPrice { get; set; }
        public string ProductName { get; set; }
    }

}