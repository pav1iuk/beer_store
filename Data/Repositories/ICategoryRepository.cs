using BeerStore.Api.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BeerStore.Api.Data.Repositories;
public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default);
    Task<long> AddAsync(Category category, CancellationToken ct = default);
}
