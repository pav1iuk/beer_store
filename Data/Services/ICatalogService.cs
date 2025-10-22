using BeerStore.Api.Data.DTOs;
using BeerStore.Api.Data.Repositories.EF;

namespace BeerStore.Api.Data.Services
{
    public interface ICatalogService
    {
        Task<PagedResult<ProductDto>> GetProductsAsync(int page, int pageSize, string? q, CancellationToken ct);
        Task<ProductDto?> GetProductAsync(long id, CancellationToken ct);
        Task<ProductDto> CreateProductAsync(ProductCreateDto dto, CancellationToken ct);
    }
}
