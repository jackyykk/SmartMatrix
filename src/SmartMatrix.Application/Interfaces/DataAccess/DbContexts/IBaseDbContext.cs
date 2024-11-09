using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbContexts
{
    public interface IBaseDbContext : IDbConnectionChangeable, IDisposable
    {
        DbContext DbContext { get; }
        IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
        
        bool HasChanges { get; }        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}