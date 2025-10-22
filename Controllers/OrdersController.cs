using BeerStore.Api.Data;
using BeerStore.Api.Data.Repositories;
using BeerStore.Api.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public OrdersController(IUnitOfWork uow) => _uow = uow;

    // Create order and optionally update catalog stock in one UnitOfWork transaction (note: same DB required)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Order order, CancellationToken ct)
    {
        if (order == null || order.Items.Count == 0) return BadRequest("Invalid order");

        try
        {
            // Begin transaction for Catalog DB if you plan to update products stock there
            await _uow.BeginTransactionAsync();

            // Create order in Orders DB (no distributed tx)
            var newOrderId = await _uow.Orders.CreateOrderAsync(order, ct);

            // Update stock in Catalog DB using Dapper repo (transaction scoped)
            foreach (var it in order.Items)
            {
                // parameterized update
                var sql = "UPDATE Products SET StockQuantity = StockQuantity - @Qty WHERE ProductID = @ProductId AND StockQuantity >= @Qty;";
                var affected = await (_uow.Products as dynamic)._connection.ExecuteAsync(sql, new { Qty = it.Quantity, ProductId = it.ProductId }, transaction: (_uow as dynamic)._transaction);
                // Note: the above uses dynamic to access internal connection — you may expose specific methods in repo instead.
            }

            await _uow.CommitAsync();
            return CreatedAtAction(nameof(Get), new { id = newOrderId }, new { OrderId = newOrderId });
        }
        catch (Exception ex)
        {
            await _uow.RollbackAsync();
            return StatusCode(500, new { message = "Could not create order", detail = ex.Message });
        }
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id, CancellationToken ct)
    {
        var order = await _uow.Orders.GetOrderByIdAsync(id, ct);
        if (order == null) return NotFound();
        return Ok(order);
    }
}
