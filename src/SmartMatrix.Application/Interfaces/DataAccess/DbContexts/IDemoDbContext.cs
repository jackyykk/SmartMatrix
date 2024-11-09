using Microsoft.EntityFrameworkCore;
using SmartMatrix.Domain.Demos.SimpleNotes.Entities;

namespace SmartMatrix.Domain.Interfaces.DataAccess.DbContexts
{
    public interface IDemoDbContext : IBaseDbContext
    {        
        // DbSet
        public DbSet<SimpleNote> SimpleNotes { get; set; }
    }
}