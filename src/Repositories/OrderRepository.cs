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
               .ThenInclude(oi => oi.Product)                         // eager-load products :contentReference[oaicite:0]{index=0}
               .AsNoTracking()                                        // read-only boost :contentReference[oaicite:1]{index=1}
               .FirstOrDefaultAsync(o => o.Id == id, ct);             // single row :contentReference[oaicite:2]{index=2}

        public Task<Order?> GetOrderBySessionIdAsync(string sessionId, CancellationToken ct = default) =>
            db.Orders
               .Include(o => o.Items)
               .ThenInclude(oi => oi.Product)
               .AsNoTracking()
               .FirstOrDefaultAsync(o => o.SessionId == sessionId, ct);

        public Task UpdateOrderStatusAsync(int id, OrderStatus status, string? paymentIntentId,
                                           CancellationToken ct = default)
        {
            // bulk update without tracking :contentReference[oaicite:3]{index=3}
            return db.Orders
                     .Where(o => o.Id == id)
                     .ExecuteUpdateAsync(setters => setters
                         .SetProperty(o => o.Status, status)
                         .SetProperty(o => o.PaymentIntentId, paymentIntentId),
                         ct);
        }
        
        public Task<int> CountOrderByUserIdAsync(string userId, CancellationToken ct = default) =>
            db.Orders.CountAsync(o => o.UserId == userId, ct);         // async count :contentReference[oaicite:5]{index=5}
    }
}
