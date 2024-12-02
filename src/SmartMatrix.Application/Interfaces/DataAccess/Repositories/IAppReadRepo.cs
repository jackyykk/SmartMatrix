using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Application.Interfaces.DataAccess.Specifications;
using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories
{
    public interface IAppReadRepo<T, in TId> : IDbConnectionChangeable where T : class, IEntity<TId>
    {
        IAppReadDbContext AppReadDb { get; }        
        IQueryable<T> Entities { get; }

        Task<T?> GetByIdAsync(TId id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetPagedAsync(int pageNumber, int pageSize);
        
        Task<List<T>> FindAsync(ISpec<T> spec);
        Task<List<T>> FindPagedAsync(int pageNumber, int pageSize, ISpec<T> spec);                
    }
}