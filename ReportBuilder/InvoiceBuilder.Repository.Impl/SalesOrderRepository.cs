﻿using System.Collections.Generic;
using Dapper;
using InvoiceBuilder.Transaction;
using InvoiceBuilder.UseCase;

namespace InvoiceBuilder.Repository.Impl
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly ITransactionContext _transactionContext;

        public SalesOrderRepository(ITransactionContext transactionContext)
        {
            _transactionContext = transactionContext;
        }

        public IEnumerable<SalesOrder> Get()
        {
            return _transactionContext
                .Connection
                .Query<SalesOrder>(
                    Properties.Resources.SalesOrderGet,
                    transaction:_transactionContext.Transaction);
        }
    }
}