using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbContexts
{
    public interface IBaseDbContext : IDisposable
    {
        DbContext DbContext { get; }
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
        
        bool HasChanges { get; }        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}