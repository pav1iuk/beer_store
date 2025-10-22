using BeerStore.Api.Data.Repositories;
using BeerStore.Api.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _products;

    public ProductsController(IProductRepository products)
    {
        _products = products;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var list = await _products.GetAllAsync(ct);
        return Ok(list);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id, CancellationToken ct)
    {
        var p = await _products.GetByIdAsync(id, ct);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product, CancellationToken ct)
    {
        if (product == null) return BadRequest();

        var newId = await _products.AddAsync(product, ct);
        product.ProductId = newId;
        return CreatedAtAction(nameof(Get), new { id = newId }, product);
    }
}
