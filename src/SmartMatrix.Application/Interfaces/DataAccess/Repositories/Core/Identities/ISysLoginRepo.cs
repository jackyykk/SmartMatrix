using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities
{
    public interface ISysLoginRepo : IDbConnectionChangeable, ITransactionableRepo
    {
        IQueryable<SysLogin> SysLogins { get; }

        // Get
        Task<List<SysLogin>> GetListAsync(string partitionKey);
        Task<SysLogin?> GetFirstByRefreshTokenAsync(string partitionKey, string refreshToken);

        // Update
        Task UpdateSecretAsync(int id, string password, string passwordHash, string passwordSalt);
        Task UpdateRefreshTokenAsync(int id, string refreshToken, DateTime? refreshTokenExpires);

        // Insert
        Task<SysLogin> InsertAsync(SysLogin entity);
    }
}