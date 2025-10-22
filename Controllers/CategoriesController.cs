using BeerStore.Api.Data.Repositories;
using BeerStore.Api.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categories;
    public CategoriesController(ICategoryRepository categories) => _categories = categories;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var list = await _categories.GetAllAsync(ct);
        return Ok(list);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category category, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(category.Name)) return BadRequest("Name required");
        var id = await _categories.AddAsync(category, ct);
        category.CategoryId = id;
        return CreatedAtAction(null, new { id }, category);
    }
}
