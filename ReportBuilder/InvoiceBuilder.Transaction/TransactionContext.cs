using System.Data;

namespace InvoiceBuilder.Transaction
{
    public class TransactionContext: ITransactionContext
    {
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
    }
}