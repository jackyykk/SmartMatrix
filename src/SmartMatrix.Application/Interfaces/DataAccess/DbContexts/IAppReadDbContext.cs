using Microsoft.EntityFrameworkCore;
using SmartMatrix.Domain.Apps.SimpleNoteApp.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbContexts
{
    public interface IAppReadDbContext : IBaseDbContext
    {        
        // DbSet
        DbSet<SimpleNote> SimpleNotes { get; set; }
    }
}