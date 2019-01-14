﻿using System;

namespace InvoiceService.Repository
{
    public class SalesOrder
    {
        public int SalesOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CompanyName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DetailCount { get; set; }
        public double TotalPrice { get; set; }
    }
}
