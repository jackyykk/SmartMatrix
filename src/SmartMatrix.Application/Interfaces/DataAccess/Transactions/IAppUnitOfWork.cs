using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;

namespace SmartMatrix.Application.Interfaces.DataAccess.Transactions
{
    public interface IAppUnitOfWork : IBaseUnitOfWork
    {
        IAppDbContext DbContext { get; }
    }
}