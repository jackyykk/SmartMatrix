using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SmartMatrix.Domain.Entities.Identities;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbContexts
{
    public interface IAppDbContext : IDisposable
    {
        DbContext DbContext { get; }
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
        
        bool HasChanges { get; }        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        // DbSet
        public DbSet<AppUser> AppUsers { get; set; }
    }
}