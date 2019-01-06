using System;
using System.Data;
using Castle.DynamicProxy;

namespace InvoiceBuilder.Transaction
{
    public class TransactionInterceptor : IInterceptor
    {
        private readonly ITransactionContext _transactionContext;
        private readonly Func<IDbConnection> _createConnection;

        public TransactionInterceptor(ITransactionContext transactionContext, Func<IDbConnection> createConnection)
        {
            _transactionContext = transactionContext;
            _createConnection = createConnection;
        }

        public void Intercept(IInvocation invocation)
        {
            using (var connection = _createConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    _transactionContext.Connection = connection;
                    _transactionContext.Transaction = transaction;

                    invocation.Proceed();

                    transaction.Commit();
                }
            }
        }
    }
}