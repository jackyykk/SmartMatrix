using Microsoft.EntityFrameworkCore;
using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbContexts
{
    public interface ICoreReadDbContext : IBaseDbContext
    {        
        // DbSet
        DbSet<SysUser> SysUsers { get; set; }
        DbSet<SysUserRole> SysUserRoles { get; set; }
        DbSet<SysLogin> SysLogins { get; set; }
    }
}