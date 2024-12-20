using Microsoft.EntityFrameworkCore;
using SmartMatrix.Domain.Tools.SimpleNoteTool.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbContexts
{
    public interface IToolWriteDbContext : IBaseDbContext
    {        
        // DbSet
        DbSet<SimpleNote> SimpleNotes { get; set; }
    }
}