using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities
{
    public interface ISysUserRepo : IDbConnectionChangeable, ITransactionableRepo
    {
        IQueryable<SysUser> SysUsers { get; }
        Task<SysUser?> GetFirstByUserNameAsync(string userName);
        Task<SysUser?> GetFirstByLoginNameAsync(string LoginName);
    }
}