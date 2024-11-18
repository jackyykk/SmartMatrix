using Microsoft.EntityFrameworkCore;
using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbContexts
{
    public interface ICoreWriteDbContext : IBaseDbContext
    {        
        // DbSet
        DbSet<SysUser> SysUsers { get; set; }
    }
}