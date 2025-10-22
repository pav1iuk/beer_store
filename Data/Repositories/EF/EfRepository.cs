namespace BeerStore.Api.Data.Repositories.EF
{
    using Ardalis.Specification;
    using Ardalis.Specification.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class EfRepository<T> : RepositoryBase<T>, IAsyncRepository<T> where T : class
    {
        public EfRepository(DbContext db) : base(db) { }

        async Task<T> IAsyncRepository<T>.AddAsync(T entity, CancellationToken ct)
        {
            await DbContext.Set<T>().AddAsync(entity, ct);
            await DbContext.SaveChangesAsync(ct);
            return entity;
        }

        async Task IAsyncRepository<T>.DeleteAsync(T entity, CancellationToken ct)
        {
            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync(ct);
        }

        async Task<T?> IAsyncRepository<T>.GetByIdAsync(long id, CancellationToken ct)
        {
            return await DbContext.Set<T>()
        .FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id, ct);
        }

        async Task<IReadOnlyList<T>> IAsyncRepository<T>.ListAsync(CancellationToken ct)
        {
            return await DbContext.Set<T>().ToListAsync(ct); ;
        }

        async Task IAsyncRepository<T>.UpdateAsync(T entity, CancellationToken ct)
        {
            DbContext.Set<T>().Update(entity);
            await DbContext.SaveChangesAsync(ct);
        }

        async Task<IReadOnlyList<T>> IAsyncRepository<T>.ListAsync(ISpecification<T> spec, CancellationToken ct)
        {
            return await ApplySpecification(spec).ToListAsync(ct);
        }
        async Task<T?> IAsyncRepository<T>.FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken ct)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync(ct);
        }
    }

}
