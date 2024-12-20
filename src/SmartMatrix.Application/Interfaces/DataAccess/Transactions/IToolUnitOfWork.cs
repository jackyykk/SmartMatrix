using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;

namespace SmartMatrix.Application.Interfaces.DataAccess.Transactions
{
    public interface IToolUnitOfWork : IBaseUnitOfWork
    {
        IToolWriteDbContext WriteDbContext { get; }
    }
}