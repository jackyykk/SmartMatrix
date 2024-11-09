using SmartMatrix.Domain.Interfaces.DataAccess.DbContexts;

namespace SmartMatrix.Domain.Interfaces.DataAccess.Transactions
{
    public interface IAppUnitOfWork : IBaseUnitOfWork
    {
        IAppDbContext DbContext { get; }
    }
}