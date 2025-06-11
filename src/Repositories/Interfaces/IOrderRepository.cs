using BlazeBuy.Models;

namespace BlazeBuy.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task CreateOrderAsync(Order order, CancellationToken ct = default);

        public Task UpdateOrderAsync(Order order);

        Task<Order?> GetOrderByIdAsync(int id, CancellationToken ct = default);
        Task<Order?> GetOrderBySessionIdAsync(string sessionId, CancellationToken ct = default);

        Task<IReadOnlyList<Order>> GetOrderForUserAsync(
            string userId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<int> CountOrderForUserAsync(string userId, CancellationToken ct = default);
    }
}
