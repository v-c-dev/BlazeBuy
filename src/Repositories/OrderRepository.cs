using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Models.Enums;
using BlazeBuy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazeBuy.Repositories
{
    internal sealed class OrderRepository(ApplicationDbContext db) : IOrderRepository
    {
        public async Task<Order> CreateOrderAsync(Order order, CancellationToken ct = default)
        {
            await db.Orders.AddAsync(order, ct);
            await db.SaveChangesAsync(ct);
            return order;
        }

        public Task UpdateOrderAsync(Order order, CancellationToken ct = default)
        {
            db.Orders.Update(order);
            return Task.CompletedTask;
        }

        public Task<Order?> GetOrderByIdAsync(int id, CancellationToken ct = default) =>
            db.Orders
               .Include(o => o.Items)
               .ThenInclude(oi => oi.Product) 
               .AsNoTracking()
               .FirstOrDefaultAsync(o => o.Id == id, ct);

        public Task<Order?> GetOrderBySessionIdAsync(string sessionId, CancellationToken ct = default) =>
            db.Orders
               .Include(o => o.Items)
               .ThenInclude(oi => oi.Product)
               .AsNoTracking()
               .FirstOrDefaultAsync(o => o.SessionId == sessionId, ct);

        public Task UpdateOrderStatusAsync(int id, OrderStatus status, string? paymentIntentId,
                                           CancellationToken ct = default)
        {
            return db.Orders
                     .Where(o => o.Id == id)
                     .ExecuteUpdateAsync(setters => setters
                         .SetProperty(o => o.Status, status)
                         .SetProperty(o => o.PaymentIntentId, paymentIntentId),
                         ct);
        }
        
        public Task<int> CountOrderByUserIdAsync(string userId, CancellationToken ct = default) =>
            db.Orders.CountAsync(o => o.UserId == userId, ct);
    }
}
