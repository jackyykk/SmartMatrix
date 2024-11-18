using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities
{
    public interface ISysUserCacheRepo
    {        
        Task<SysUser?> GetFirstByUserNameAsync(string userName);
    }
}