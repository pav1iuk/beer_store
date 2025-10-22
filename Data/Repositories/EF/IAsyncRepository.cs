namespace BeerStore.Api.Data.Repositories.EF
{
    using Ardalis.Specification;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IAsyncRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(long id, CancellationToken ct = default);
        Task<IReadOnlyList<T>> ListAsync(CancellationToken ct = default);
        Task<T> AddAsync(T entity, CancellationToken ct = default);
        Task UpdateAsync(T entity, CancellationToken ct = default);
        Task DeleteAsync(T entity, CancellationToken ct = default);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken ct = default);
        Task<T?> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken ct = default);
    }

}
