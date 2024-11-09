using Microsoft.EntityFrameworkCore;
using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;
using SmartMatrix.Application.Interfaces.DataAccess.Specifications;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.DataAccess.DbContexts;
using SmartMatrix.DataAccess.Specifications;

namespace SmartMatrix.DataAccess.Repositories
{
    public class DemoWriteRepo<T, TId> : IDemoWriteRepo<T, TId> where T : AuditableEntity<TId>
    {
        const double DefaultCommandTimeoutInSeconds = 60;

        private readonly DemoWriteDbContext _writeDbContext;        
        public IDemoWriteDbContext DemoWriteDb => _writeDbContext;
        public IQueryable<T> Entities => _writeDbContext.Set<T>();

        public void SetConnection(string connectionString)
        {
            _writeDbContext.SetConnection(connectionString);
        }

        public DemoWriteRepo(IDemoReadDbContext readDbContext, IDemoWriteDbContext writeDbContext)
        {            
            _writeDbContext = (DemoWriteDbContext)writeDbContext;
            _writeDbContext.Database.SetCommandTimeout(TimeSpan.FromSeconds(DefaultCommandTimeoutInSeconds));
        }        

        public async Task<T?> GetByIdAsync(TId id)
        {
            return await _writeDbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _writeDbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<List<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _writeDbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<List<T>> FindAsync(ISpec<T> spec)
        {
            return await SpecEvaluator<T>.GetQuery(_writeDbContext.Set<T>().AsQueryable(), spec).ToListAsync();
        }

        public async Task<List<T>> FindPagedAsync(int pageNumber, int pageSize, ISpec<T> spec)
        {
            return await SpecEvaluator<T>.GetQuery(_writeDbContext.Set<T>().AsQueryable(), spec)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> InsertAsync(T entity)
        {
            await _writeDbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(T entity)
        {
            _writeDbContext.Entry(entity).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            _writeDbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }        
    }
}