using Microsoft.EntityFrameworkCore;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Entities;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbContexts
{
    public interface IDemoReadDbContext : IBaseDbContext
    {        
        // DbSet
        DbSet<SimpleNote> SimpleNotes { get; set; }
    }
}