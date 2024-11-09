using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;

namespace SmartMatrix.Application.Interfaces.DataAccess.Transactions
{
    public interface ICoreUnitOfWork : IBaseUnitOfWork
    {
        ICoreWriteDbContext WriteDbContext { get; }
    }
}