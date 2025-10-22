using BeerStore.Api.Models;

namespace BeerStore.Api.Data.Repositories.EF
{
    public interface IProductRepository : IAsyncRepository<Product>
    {
        Task<Product?> GetWithDetailsAsync(long id, CancellationToken ct = default); 
        Task<IReadOnlyList<Product>> GetByCategoryAsync(long categoryId, CancellationToken ct = default); 
        Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default);
    }
}
