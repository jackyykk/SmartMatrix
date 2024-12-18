using System.Data;
using SmartMatrix.Application.Interfaces.DataAccess.Common;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;

namespace SmartMatrix.Application.Interfaces.DataAccess.DbConnections
{
    public interface IBaseReadDbConnection : IDbConnectionChangeable
    {
        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default);

        Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default);

        Task<T> QuerySingleAsync<T>(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default);

        Task<IGridReader> QueryMultipleAsync(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default);
    }
}