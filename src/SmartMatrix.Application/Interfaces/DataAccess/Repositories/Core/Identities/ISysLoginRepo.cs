using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities
{
    public interface ISysLoginRepo : IDbConnectionChangeable, ITransactionableRepo
    {
        Task<List<SysLogin>> GetListAsync(string partitionKey);
        Task UpdateSecretAsync(SysLogin entity);
    }
}