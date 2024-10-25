using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Application.Interfaces.DataAccess.Specifications;
using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories
{
    public interface IDemoRepo<T, in TId> where T : class, IEntity<TId>
    {
        IDemoDbContext DemoDb { get; }
        IQueryable<T> Entities { get; }

        Task<T> GetByIdAsync(TId id);
        Task<T> GetAllAsync();
        Task<List<T>> GetPagedAsync(int pageNumber, int pageSize);
        
        Task<T> FindAsync(ISpec<T> spec);
        Task<List<T>> FindPagedAsync(int pageNumber, int pageSize, ISpec<T> spec);
        
        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}