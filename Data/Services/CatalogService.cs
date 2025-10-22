namespace BeerStore.Api.Data.Services
{
    using AutoMapper;
    using BeerStore.Api.Data.DTOs;
    using BeerStore.Api.Data.EfCatalog;
    using BeerStore.Api.Data.Repositories.EF;
    using BeerStore.Api.Models;

    public class CatalogService : ICatalogService
    {
        private readonly IProductRepository _productRepo;
        private readonly IAsyncRepository<Category> _categoryRepo;
        private readonly IMapper _mapper;
        private readonly CatalogDbContext _db;

        public CatalogService(IProductRepository productRepo, IAsyncRepository<Category> categoryRepo, IMapper mapper, CatalogDbContext db)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _db = db;
        }

        public async Task<PagedResult<ProductDto>> GetProductsAsync(int page, int pageSize, string? q, CancellationToken ct)
        {
            var paged = await _productRepo.GetPagedAsync(page, pageSize, q, ct);
            var dtos = _mapper.Map<IReadOnlyList<ProductDto>>(paged.Items);
            return new PagedResult<ProductDto>(dtos.ToList(), paged.TotalCount, page, pageSize);
        }

        public async Task<ProductDto?> GetProductAsync(long id, CancellationToken ct)
        {
            var p = await _productRepo.GetWithDetailsAsync(id, ct);
            if (p == null) return null;
            return _mapper.Map<ProductDto>(p);
        }

        public async Task<ProductDto> CreateProductAsync(ProductCreateDto dto, CancellationToken ct)
        {
            var product = _mapper.Map<Product>(dto);
            product.CreatedAt = DateTime.UtcNow;
            await _productRepo.AddAsync(product, ct);
            return _mapper.Map<ProductDto>(product);
        }
    }
}
