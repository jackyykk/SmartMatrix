using System.Data;

namespace SmartMatrix.Domain.Interfaces.DataAccess.Transactions
{
    public interface ITransactionableUnitOfWork : IDisposable
    {
        IDbTransaction BeginTransaction();
    }
}