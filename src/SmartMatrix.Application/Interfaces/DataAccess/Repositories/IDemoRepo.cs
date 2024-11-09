using SmartMatrix.Domain.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Domain.Interfaces.DataAccess.Specifications;
using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Domain.Interfaces.DataAccess.Repositories
{
    public interface IDemoRepo<T, in TId> where T : class, IEntity<TId>
    {
        IDemoDbContext DemoDb { get; }
        IQueryable<T> Entities { get; }

        Task<T?> GetByIdAsync(TId id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetPagedAsync(int pageNumber, int pageSize);
        
        Task<List<T>> FindAsync(ISpec<T> spec);
        Task<List<T>> FindPagedAsync(int pageNumber, int pageSize, ISpec<T> spec);
        
        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}