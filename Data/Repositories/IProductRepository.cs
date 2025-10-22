using BeerStore.Api.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BeerStore.Api.Data.Repositories;
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default);
    Task<Product?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<long> AddAsync(Product product, CancellationToken ct = default);
}
