using BlazeBuy.Models.Enums;
using BlazeBuy.Models;

namespace BlazeBuy.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string userId, CancellationToken ct = default);
        Task<Order?> GetOrderByIdAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<Order>> GetOrderByUserIdAsync(string userId, CancellationToken ct = default);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status, CancellationToken ct = default);
        Task<IEnumerable<Order>> GetAllOrdersAsync(string? userId = null);
    }
}
