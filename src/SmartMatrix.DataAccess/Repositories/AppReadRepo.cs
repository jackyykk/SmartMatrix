using Microsoft.EntityFrameworkCore;
using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;
using SmartMatrix.Application.Interfaces.DataAccess.Specifications;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.DataAccess.DbContexts;
using SmartMatrix.DataAccess.Specifications;

namespace SmartMatrix.DataAccess.Repositories
{
    public class AppReadRepo<T, TId> : IAppReadRepo<T, TId> where T : AuditableEntity<TId>
    {
        const double DefaultCommandTimeoutInSeconds = 60;

        private AppReadDbContext _readDbContext;        
        public IAppReadDbContext AppReadDb => _readDbContext;        
        public IQueryable<T> Entities => _readDbContext.Set<T>();

        public AppReadRepo(IAppReadDbContext readDbContext)
        {
            _readDbContext = (AppReadDbContext)readDbContext;
            _readDbContext.Database.SetCommandTimeout(TimeSpan.FromSeconds(DefaultCommandTimeoutInSeconds));
        }

        public void SetConnection(string connectionString)
        {
            _readDbContext.SetConnection(connectionString);
        }

        public async Task<T?> GetByIdAsync(TId id)
        {
            return await _readDbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _readDbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<List<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _readDbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<List<T>> FindAsync(ISpec<T> spec)
        {
            return await SpecEvaluator<T>.GetQuery(_readDbContext.Set<T>().AsQueryable(), spec).ToListAsync();
        }

        public async Task<List<T>> FindPagedAsync(int pageNumber, int pageSize, ISpec<T> spec)
        {
            return await SpecEvaluator<T>.GetQuery(_readDbContext.Set<T>().AsQueryable(), spec)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }               
    }
}