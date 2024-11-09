using Microsoft.EntityFrameworkCore;
using SmartMatrix.Domain.Core.Identities.Entities;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbContexts
{
    public interface IAppDbContext : IBaseDbContext
    {        
        // DbSet
        public DbSet<AppUser> AppUsers { get; set; }
    }
}