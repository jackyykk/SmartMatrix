using Microsoft.EntityFrameworkCore;
using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;
using SmartMatrix.Application.Interfaces.DataAccess.Specifications;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.DataAccess.DbContexts;
using SmartMatrix.DataAccess.Specifications;

namespace SmartMatrix.DataAccess.Repositories
{
    public class DemoRepo<T, TId> : IDemoRepo<T, TId> where T : AuditableEntity<TId>
    {
        const double DefaultCommandTimeoutInSeconds = 60;

        private readonly DemoDbContext _dbContext;
        public IDemoDbContext DemoDb => _dbContext;
        public DemoRepo(IDemoDbContext dbContext)
        {
            _dbContext = (DemoDbContext)dbContext;
            _dbContext.Database.SetCommandTimeout(TimeSpan.FromSeconds(DefaultCommandTimeoutInSeconds));
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T?> GetByIdAsync(TId id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<List<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<List<T>> FindAsync(ISpec<T> spec)
        {
            return await SpecEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec).ToListAsync();
        }

        public async Task<List<T>> FindPagedAsync(int pageNumber, int pageSize, ISpec<T> spec)
        {
            return await SpecEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }        
    }
}