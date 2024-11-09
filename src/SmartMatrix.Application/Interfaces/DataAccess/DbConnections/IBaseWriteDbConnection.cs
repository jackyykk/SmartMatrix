using System.Data;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbConnections
{
    public interface IBaseWriteDbConnection : IBaseReadDbConnection
    {
        Task<int> ExecuteAsync(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default);
    }
}