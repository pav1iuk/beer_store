using Dapper;
using MySqlConnector;
using System.Data;

namespace BeerStore.Api.Data.Generic;
public abstract class DapperRepositoryBase<T> : IDapperRepository<T> where T : class
{
    protected readonly MySqlConnection _connection;
    protected readonly IDbTransaction? _transaction;

    public DapperRepositoryBase(MySqlConnection connection, IDbTransaction? transaction = null)
    {
        _connection = connection;
        _transaction = transaction;
    }

    protected abstract string TableName { get; }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
    {
        var sql = $"SELECT * FROM {TableName} LIMIT 1000;";
        return await _connection.QueryAsync<T>(new CommandDefinition(sql, cancellationToken: ct, transaction: _transaction));
    }

    public virtual async Task<T?> GetByIdAsync(object id, CancellationToken ct = default)
    {
        var sql = $"SELECT * FROM {TableName} WHERE {GetPkName()} = @Id LIMIT 1;";
        return await _connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct, transaction: _transaction));
    }

    protected virtual string GetPkName() => $"{TableName}Id";
}
