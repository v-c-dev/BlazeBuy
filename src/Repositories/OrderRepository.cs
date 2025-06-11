using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazeBuy.Repositories
{
    internal sealed class OrderRepository(ApplicationDbContext db) : IOrderRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task CreateOrderAsync(Order order, CancellationToken ct = default)
        {
            await _db.Orders.AddAsync(order, ct);
        }

        public Task UpdateOrderAsync(Order order)
        {
            _db.Orders.Update(order);
            return Task.CompletedTask;
        }

        public Task<Order?> GetOrderAsync(int id, CancellationToken ct = default) =>
            _db.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id, ct);

        public Task<Order?> GetOrderBySessionIdAsync(string sessionId, CancellationToken ct = default) =>
            _db.Orders
                .Include(o => o.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.SessionId == sessionId, ct);

        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(
            string userId, int pageNumber, int pageSize, CancellationToken ct = default) =>
            await _db.Orders
                .Include(o => o.Items)
                .AsNoTracking()
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedUtc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

        public Task<int> CountOrderForUserAsync(string userId, CancellationToken ct = default) =>
            _db.Orders.CountAsync(o => o.UserId == userId, ct);
    }
}
