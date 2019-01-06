using System.Data;

namespace InvoiceBuilder.Transaction
{
    public interface ITransactionContext
    {
        IDbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }
    }
}
