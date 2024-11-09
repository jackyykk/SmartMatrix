using System.Data;

namespace SmartMatrix.Domain.Interfaces.DataAccess.Transactions
{
    public interface IBaseUnitOfWork : ITransactionableUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void Open();
        void Close();
    }
}