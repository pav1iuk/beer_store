using BeerStore.Api.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace BeerStore.Api.Data;
public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }

    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
