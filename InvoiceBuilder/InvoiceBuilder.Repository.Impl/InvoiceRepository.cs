﻿using System.Linq;
using Dapper;
using InvoiceBuilder.Transaction;

namespace InvoiceBuilder.Repository.Impl
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ITransactionContext _transactionContext;

        public InvoiceRepository(ITransactionContext transactionContext)
        {
            _transactionContext = transactionContext;
        }

        public Invoice Get(int salesOrderId)
        {
            var invoice = _transactionContext
                .Connection
                .Query<Invoice>(
                    Properties.Resources.InvoiceGetBySalesOrderId,
                    new { SalesOrderId = salesOrderId },
                    transaction: _transactionContext.Transaction)
                .SingleOrDefault();
            var invoiceDetails = _transactionContext
                .Connection
                .Query<InvoiceDetail>(
                    Properties.Resources.InvoiceDetailGetBySalesOrderId,
                    new {SalesOrderId = salesOrderId},
                    transaction: _transactionContext.Transaction);
            foreach (var invoiceDetail in invoiceDetails)
            {
                invoice.InvoiceDetails.Add(invoiceDetail);
            }
            return invoice;
        }
    }
}