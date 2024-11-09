using Microsoft.EntityFrameworkCore;
using SmartMatrix.Domain.Fundamental.Identities.Entities;

namespace SmartMatrix.Domain.Interfaces.DataAccess.DbContexts
{
    public interface IAppDbContext : IBaseDbContext
    {        
        // DbSet
        public DbSet<AppUser> AppUsers { get; set; }
    }
}