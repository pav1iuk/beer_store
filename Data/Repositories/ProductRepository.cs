using BeerStore.Api.Models;
using Dapper;
using MySqlConnector;
using System.Data;

namespace BeerStore.Api.Data.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly MySqlConnection _connection;
    private readonly IDbTransaction? _transaction;

    public ProductRepository(MySqlConnection connection, IDbTransaction? transaction = null)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default)
    {
        var sql = "SELECT ProductID as ProductId, Name, SKU as Sku, Price, StockQuantity FROM Products LIMIT 100;";
        return await _connection.QueryAsync<Product>(new CommandDefinition(sql, cancellationToken: ct, transaction: _transaction));
    }

    public async Task<Product?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        var sql = "SELECT ProductID as ProductId, Name, SKU as Sku, Price, StockQuantity FROM Products WHERE ProductID = @Id LIMIT 1;";
        return await _connection.QueryFirstOrDefaultAsync<Product>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct, transaction: _transaction));
    }

    public async Task<long> AddAsync(Product product, CancellationToken ct = default)
    {
        var sql = @"INSERT INTO Products (Name, SKU, Price, StockQuantity) 
                    VALUES (@Name, @Sku, @Price, @StockQuantity);
                    SELECT LAST_INSERT_ID();";
        var id = await _connection.ExecuteScalarAsync<long>(new CommandDefinition(sql, new { product.Name, product.Sku, product.Price, product.StockQuantity }, cancellationToken: ct, transaction: _transaction));
        return id;
    }
}
