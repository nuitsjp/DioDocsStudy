using System.Collections.Generic;
using Dapper;
using InvoiceBuilder.Transaction;
using InvoiceBuilder.UseCase;

namespace InvoiceBuilder.Repository.Impl
{
    public class SalesOrderDetailRepository : ISalesOrderDetailRepository
    {
        private readonly ITransactionContext _transactionContext;

        public SalesOrderDetailRepository(ITransactionContext transactionContext)
        {
            _transactionContext = transactionContext;
        }

        public IEnumerable<SalesOrderDetail> Get(int salesOrderId)
        {
            return _transactionContext
                .Connection
                .Query<SalesOrderDetail>(
                    Properties.Resources.SalesOrderDetailGet,
                    new { SalesOrderId = salesOrderId },
                    transaction: _transactionContext.Transaction);
        }
    }
}
