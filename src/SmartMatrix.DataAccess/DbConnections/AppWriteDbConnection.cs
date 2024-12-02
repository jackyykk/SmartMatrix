using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SmartMatrix.Application.Interfaces.DataAccess.Common;
using SmartMatrix.Application.Interfaces.DataAccess.DbConnections;
using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.DataAccess.Common;

namespace SmartMatrix.DataAccess.DbConnections
{
    public class AppWriteDbConnection : IAppWriteDbConnection
{
    private IDbConnection connection;

    public void SetConnection(string connectionString)
    {
        if (connection != null)
        {
            connection.Close();
            connection.Dispose();
        }
        connection = new SqlConnection(connectionString);
    }

    public AppWriteDbConnection(IConfiguration configuration)
    {
        connection = new SqlConnection(configuration.GetConnectionString("AppWriteConnection"));
    }

    public async Task<int> ExecuteAsync(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default)
    {
        return await connection.ExecuteAsync(sql, param, transaction);
    }

    public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default)
    {
        return (await connection.QueryAsync<T>(sql, param, transaction)).AsList();
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default)
    {
        return await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
    }

    public async Task<T> QuerySingleAsync<T>(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default)
    {
        return await connection.QuerySingleAsync<T>(sql, param, transaction);
    }

    public async Task<IGridReader> QueryMultipleAsync(string sql, object param, IDbTransaction transaction, CancellationToken cancellationToken = default)
    {
        var gridReader = await connection.QueryMultipleAsync(sql, param, transaction);
        return new DapperGridReader(gridReader);    
    }
}
}