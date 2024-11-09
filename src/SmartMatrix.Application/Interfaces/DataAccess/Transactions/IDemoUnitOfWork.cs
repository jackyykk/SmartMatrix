using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;

namespace SmartMatrix.Application.Interfaces.DataAccess.Transactions
{
    public interface IDemoUnitOfWork : IBaseUnitOfWork
    {
        IDemoWriteDbContext WriteDbContext { get; }
    }
}