using System.Data;
using SmartMatrix.Domain.Interfaces.DataAccess.Common;

namespace SmartMatrix.Domain.Interfaces.DataAccess.DbConnections
{
    public interface IBaseReadDbConnection
    {
        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default);

        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default);

        Task<T> QuerySingleAsync<T>(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default);

        Task<IGridReader> QueryMultipleAsync(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default);
    }
}