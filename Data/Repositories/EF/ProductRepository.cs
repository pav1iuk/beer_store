namespace BeerStore.Api.Data.Repositories.EF
{
    using BeerStore.Api.Data.EfCatalog;
    using BeerStore.Api.Models;
    using Microsoft.EntityFrameworkCore;
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        private readonly CatalogDbContext _db;
        public ProductRepository(CatalogDbContext db) : base(db) { _db = db; }

        public async Task<Product?> GetWithDetailsAsync(long id, CancellationToken ct = default)
        {
            return await _db.Products
                .Include(p => p.Detail)
                .Include(p => p.Images)
                .Include(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id, ct);
        }

        public async Task<IReadOnlyList<Product>> GetByCategoryAsync(long categoryId, CancellationToken ct = default)
        {
            return await _db.Products
                .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
                .Include(p => p.Images)
                .ToListAsync(ct);
        }

        public async Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
        {
            var query = _db.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search) || p.Sku.Contains(search));

            var total = await query.CountAsync(ct);
            var items = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<Product>(items, total, page, pageSize);
        }
    }
}
