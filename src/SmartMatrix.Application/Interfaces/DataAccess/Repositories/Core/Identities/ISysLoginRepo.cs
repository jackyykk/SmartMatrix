using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities
{
    public interface ISysLoginRepo : IDbConnectionChangeable, ITransactionableRepo
    {
        IQueryable<SysLogin> SysLogins { get; }

        // Get
        Task<List<SysLogin>> GetListAsync(string partitionKey);
        Task<SysLogin?> GetFirstByRefreshTokenAsync(string partitionKey, string refreshToken);
        Task<SysLogin?> GetFirstByOneTimeTokenAsync(string partitionKey, string oneTimeToken);

        // Update
        Task UpdateSecretAsync(int id, string password, string passwordHash, string passwordSalt);
        Task UpdateRefreshTokenAsync(int id, string refreshToken, DateTime? refreshTokenExpires);
        Task UpdateTokensAsync(int id, string refreshToken, DateTime? refreshTokenExpires, string oneTimeToken, DateTime? oneTimeTokenExpires);

        // Insert
        Task<SysLogin> InsertAsync(SysLogin entity);
    }
}