using BeerStore.Api.Models;
using MySqlConnector;
using System.Data;

namespace BeerStore.Api.Data.Repositories;
public class OrderRepository : IOrderRepository
{
    private readonly MySqlConnection _connection;
    private readonly IDbTransaction? _transaction;

    public OrderRepository(MySqlConnection connection, IDbTransaction? transaction = null)
    {
        _connection = connection;
        _transaction = transaction;
    }

    // Example: create order and items within existing transaction (if provided)
    public async Task<long> CreateOrderAsync(Order order, CancellationToken ct = default)
    {
        // Parameterized queries to avoid SQL injection
        var sqlOrder = @"INSERT INTO `Order` (CustomerId, OrderNumber, TotalAmount, Currency, Status) 
                         VALUES (@CustomerId, @OrderNumber, @TotalAmount, 'USD', @Status);
                         SELECT LAST_INSERT_ID();";

        using var cmd = _connection.CreateCommand();
        cmd.Transaction = (MySqlTransaction?)_transaction;
        cmd.CommandText = sqlOrder;
        cmd.CommandType = CommandType.Text;

        cmd.Parameters.Add(new MySqlParameter("@CustomerId", order.CustomerId));
        cmd.Parameters.Add(new MySqlParameter("@OrderNumber", order.OrderNumber));
        cmd.Parameters.Add(new MySqlParameter("@TotalAmount", order.TotalAmount));
        cmd.Parameters.Add(new MySqlParameter("@Status", order.Status));

        // Execute scalar to get last id
        var scalar = await cmd.ExecuteScalarAsync(ct);
        var newOrderId = Convert.ToInt64(scalar);

        // Insert items
        foreach (var item in order.Items)
        {
            var sqlItem = @"INSERT INTO OrderItem (OrderId, ProductId, ProductSku, ProductName, Quantity, UnitPrice)
                            VALUES (@OrderId, @ProductId, @ProductSku, @ProductName, @Quantity, @UnitPrice);";
            using var itemCmd = _connection.CreateCommand();
            itemCmd.Transaction = (MySqlTransaction?)_transaction;
            itemCmd.CommandText = sqlItem;
            itemCmd.CommandType = CommandType.Text;
            itemCmd.Parameters.Add(new MySqlParameter("@OrderId", newOrderId));
            itemCmd.Parameters.Add(new MySqlParameter("@ProductId", item.ProductId));
            itemCmd.Parameters.Add(new MySqlParameter("@ProductSku", item.ProductSku));
            itemCmd.Parameters.Add(new MySqlParameter("@ProductName", item.ProductName));
            itemCmd.Parameters.Add(new MySqlParameter("@Quantity", item.Quantity));
            itemCmd.Parameters.Add(new MySqlParameter("@UnitPrice", item.UnitPrice));
            await itemCmd.ExecuteNonQueryAsync(ct);
        }

        return newOrderId;
    }

    public async Task<Order?> GetOrderByIdAsync(long orderId, CancellationToken ct = default)
    {
        var sql = @"SELECT o.OrderId, o.CustomerId, o.OrderNumber, o.TotalAmount, o.Status
                    FROM `Order` o WHERE o.OrderId = @OrderId;";

        using var cmd = _connection.CreateCommand();
        cmd.Transaction = (MySqlTransaction?)_transaction;
        cmd.CommandText = sql;
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.Add(new MySqlParameter("@OrderId", orderId));

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct))
            return null;

        var order = new Order
        {
            OrderId = reader.GetInt64("OrderId"),
            CustomerId = reader.GetInt64("CustomerId"),
            OrderNumber = reader.GetString("OrderNumber"),
            TotalAmount = reader.GetDecimal("TotalAmount"),
            Status = reader.GetString("Status"),
            Items = new List<OrderItem>()
        };

        // read next resultset: items
        await reader.NextResultAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            // omitted for brevity — alternative: separate query for items
        }

        // Simpler: run a separate query for items
        reader.Close();
        var sqlItems = "SELECT OrderItemId, ProductId, ProductSku, ProductName, Quantity, UnitPrice FROM OrderItem WHERE OrderId = @OrderId;";
        using var itemCmd = _connection.CreateCommand();
        itemCmd.Transaction = (MySqlTransaction?)_transaction;
        itemCmd.CommandText = sqlItems;
        itemCmd.Parameters.Add(new MySqlParameter("@OrderId", orderId));
        using var itemReader = await itemCmd.ExecuteReaderAsync(ct);
        while (await itemReader.ReadAsync(ct))
        {
            order.Items.Add(new OrderItem
            {
                OrderItemId = itemReader.GetInt64("OrderItemId"),
                ProductId = itemReader.GetInt64("ProductId"),
                ProductSku = itemReader.GetString("ProductSku"),
                ProductName = itemReader.GetString("ProductName"),
                Quantity = itemReader.GetInt32("Quantity"),
                UnitPrice = itemReader.GetDecimal("UnitPrice")
            });
        }

        return order;
    }
}
