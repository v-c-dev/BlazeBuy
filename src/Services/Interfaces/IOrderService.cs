using BlazeBuy.Models.Enums;
using BlazeBuy.Models;

namespace BlazeBuy.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order, CancellationToken ct = default);

        Task<Order?> GetOrderByIdAsync(int id, CancellationToken ct = default);

        Task<Order?> GetOrderBySessionIdAsync(string sessionId, CancellationToken ct = default);

        Task<IEnumerable<Order>> GetAllOrdersAsync(string? userId = null, CancellationToken ct = default);

        Task UpdateOrderAsync(Order order, CancellationToken ct = default);

        Task UpdateOrderStatusAsync(
            int orderId,
            OrderStatus status,
            string? paymentIntentId,
            CancellationToken ct = default);

        Task<int> CountOrderByUserIdAsync(string userId, CancellationToken ct = default);
    }
}
