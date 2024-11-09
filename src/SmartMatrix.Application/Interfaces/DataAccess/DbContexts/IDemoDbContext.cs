using Microsoft.EntityFrameworkCore;
using SmartMatrix.Domain.Demos.SimpleNotes.Entities;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbContexts
{
    public interface IDemoDbContext : IBaseDbContext
    {        
        // DbSet
        public DbSet<SimpleNote> SimpleNotes { get; set; }
    }
}