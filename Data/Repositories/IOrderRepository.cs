using BeerStore.Api.Models;
using System.Threading;
using System.Threading.Tasks;

namespace BeerStore.Api.Data.Repositories;
public interface IOrderRepository
{
    Task<long> CreateOrderAsync(Order order, CancellationToken ct = default);
    Task<Order?> GetOrderByIdAsync(long orderId, CancellationToken ct = default);
}
