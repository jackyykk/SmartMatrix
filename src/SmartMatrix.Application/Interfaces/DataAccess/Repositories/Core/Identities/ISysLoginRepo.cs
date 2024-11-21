using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities
{
    public interface ISysLoginRepo : IDbConnectionChangeable, ITransactionableRepo
    {
        Task<List<SysLogin>> GetListAsync(string partitionKey);
        Task<SysLogin?> GetFirstByRefreshTokenAsync(string partitionKey, string refreshToken);
        Task UpdateSecretAsync(SysLogin entity);
        Task UpdateRefreshTokenAsync(SysLogin entity);
    }
}