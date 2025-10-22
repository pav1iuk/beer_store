using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BeerStore.Api.Data.Generic;
public interface IDapperRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    Task<T?> GetByIdAsync(object id, CancellationToken ct = default);
}
