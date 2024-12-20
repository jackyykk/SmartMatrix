using Microsoft.EntityFrameworkCore;
using SmartMatrix.Domain.Tools.SimpleNoteTool.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbContexts
{
    public interface IToolReadDbContext : IBaseDbContext
    {        
        // DbSet
        DbSet<SimpleNote> SimpleNotes { get; set; }
    }
}