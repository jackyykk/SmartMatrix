using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities
{
    public interface ISysUserRepo : IDbConnectionChangeable, ITransactionableRepo
    {
        IQueryable<SysUser> SysUsers { get; }

        // Get
        Task<SysUser?> GetByIdAsync(string partitionKey, int id);
        Task<SysUser?> GetFirstByTypeAsync(string partitionKey, string type);
        Task<SysUser?> GetFirstByUserNameAsync(string partitionKey, string userName);        
        Task<SysUser?> GetFirstByLoginNameAsync(string partitionKey, string LoginName);
                
        // Insert
        Task<SysUser> InsertAsync(SysUser entity);
    }
}