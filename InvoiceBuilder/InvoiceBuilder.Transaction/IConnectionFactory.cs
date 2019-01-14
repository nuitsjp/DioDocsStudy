using System.Data;

namespace InvoiceBuilder.Transaction
{
    public interface IConnectionFactory
    {
        IDbConnection Create();
    }
}