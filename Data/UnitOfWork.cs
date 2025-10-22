using BeerStore.Api.Data.Repositories;
using MySqlConnector;
using System.Data;

namespace BeerStore.Api.Data;
public class UnitOfWork : IUnitOfWork
{
    private readonly string _ordersConnString;
    private readonly string _catalogConnString;

    // We'll keep two connections: orders (ADO.NET) and catalog (Dapper)
    private readonly MySqlConnection _ordersConnection;
    private readonly MySqlConnection _catalogConnection;
    private IDbTransaction? _transaction;

    public IOrderRepository Orders { get; private set; }
    public IProductRepository Products { get; private set; }
    public ICategoryRepository Categories { get; private set; }

    public UnitOfWork(string ordersConn, string catalogConn)
    {
        _ordersConnString = ordersConn;
        _catalogConnString = catalogConn;

        _ordersConnection = new MySqlConnection(_ordersConnString);
        _catalogConnection = new MySqlConnection(_catalogConnString);

        // open connections immediately (or lazy open in BeginTransaction)
        _ordersConnection.Open();
        _catalogConnection.Open();

        // initially no transaction
        Orders = new OrderRepository(_ordersConnection, _transaction); // can pass null; repo handles it
        Products = new ProductRepository(_catalogConnection, _transaction);
        Categories = new CategoryRepository(_catalogConnection, _transaction);
    }

    public async Task BeginTransactionAsync()
    {
        // create a transaction on the catalog connection and reuse for both repos if same DB,
        // but here Orders and Catalog are different DBs: real distributed TX would need saga/2PC.
        // We'll start a transaction on the Catalog DB for operations that need atomicity within that DB.
        _transaction = await _catalogConnection.BeginTransactionAsync();
        // Recreate repositories with transaction
        Products = new ProductRepository(_catalogConnection, _transaction);
        Categories = new CategoryRepository(_catalogConnection, _transaction);
        // For Orders (different DB), optionally create a transaction too:
        // var ordersTx = await _ordersConnection.BeginTransactionAsync();
        // Orders = new OrderRepository(_ordersConnection, ordersTx);
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            _transaction.Commit();
            await Task.CompletedTask;
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
            await Task.CompletedTask;
            _transaction = null;
        }
    }

    public void Dispose()
    {
        try { _transaction?.Dispose(); } catch { }
        try { _catalogConnection?.Dispose(); } catch { }
        try { _ordersConnection?.Dispose(); } catch { }
    }
}
