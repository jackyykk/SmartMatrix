using Microsoft.EntityFrameworkCore;
using SmartMatrix.Domain.Entities.Demos;
using SmartMatrix.Domain.Entities.Identities;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbContexts
{
    public interface IDemoDbContext : IBaseDbContext
    {        
        // DbSet
        public DbSet<SimpleNote> SimpleNotes { get; set; }
    }
}