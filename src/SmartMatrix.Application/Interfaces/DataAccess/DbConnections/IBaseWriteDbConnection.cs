using System.Data;

namespace SmartMatrix.Domain.Interfaces.DataAccess.DbConnections
{
    public interface IBaseWriteDbConnection : IBaseReadDbConnection
    {
        Task<int> ExecuteAsync(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default);
    }
}