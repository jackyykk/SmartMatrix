using System.Data;

namespace SmartMatrix.Application.Interfaces.DataAccess.Transactions
{
    public interface ITransactionableUnitOfWork : IDisposable
    {
        IDbTransaction BeginTransaction();
    }
}