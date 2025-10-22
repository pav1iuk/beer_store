using BeerStore.Api.Models;
using Dapper;
using MySqlConnector;
using System.Data;

namespace BeerStore.Api.Data.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly MySqlConnection _connection;
    private readonly IDbTransaction? _transaction;

    public CategoryRepository(MySqlConnection connection, IDbTransaction? transaction = null)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default)
    {
        var sql = "SELECT CategoryID as CategoryId, Name FROM Categories;";
        return await _connection.QueryAsync<Category>(new CommandDefinition(sql, cancellationToken: ct, transaction: _transaction));
    }

    public async Task<long> AddAsync(Category category, CancellationToken ct = default)
    {
        var sql = @"INSERT INTO Categories (Name) VALUES (@Name); SELECT LAST_INSERT_ID();";
        return await _connection.ExecuteScalarAsync<long>(new CommandDefinition(sql, new { category.Name }, cancellationToken: ct, transaction: _transaction));
    }
}
