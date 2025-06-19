using BlazeBuy.Models;
using BlazeBuy.Models.Enums;

namespace BlazeBuy.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order, CancellationToken ct = default);

        Task UpdateOrderAsync(Order order, CancellationToken ct = default);

        Task UpdateOrderStatusAsync(
            int orderId,
            OrderStatus status,
            string? paymentIntentId,
            CancellationToken ct = default);

        Task<Order?> GetOrderByIdAsync(int id, CancellationToken ct = default);

        Task<Order?> GetOrderBySessionIdAsync(string sessionId, CancellationToken ct = default);

        Task<int> CountOrderByUserIdAsync(string userId, CancellationToken ct = default);
    }
}
