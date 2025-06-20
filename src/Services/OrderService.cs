using BlazeBuy.Data;
using BlazeBuy.Models.Enums;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using BlazeBuy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazeBuy.Services
{
    internal sealed class OrderService(
        ApplicationDbContext db,
        IOrderRepository orderRepo,
        ILogger<OrderService> log) : IOrderService
    {
        public async Task<Order> CreateOrderAsync(Order order, CancellationToken ct = default)
        {
            order.Status = order.Status == 0 ? OrderStatus.Pending : order.Status;
            order.CreatedUtc = order.CreatedUtc == default ? DateTime.UtcNow : order.CreatedUtc;
            order.Total = order.Items.Sum(i => i.UnitPrice * i.Quantity);

            await orderRepo.CreateOrderAsync(order, ct);
            await db.SaveChangesAsync(ct);

            // Optional: send confirmation e-mail here
            // await emailSvc.SendOrderConfirmationAsync(order, ct);

            log.LogInformation("New order #{OrderId} created for user {UserId}", order.Id, order.UserId);
            return order;
        }

        public Task<Order?> GetOrderByIdAsync(int id, CancellationToken ct = default) =>
            orderRepo.GetOrderByIdAsync(id, ct);

        public Task<Order?> GetOrderBySessionIdAsync(string sessionId, CancellationToken ct = default) =>
            orderRepo.GetOrderBySessionIdAsync(sessionId, ct);

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(string? userId = null, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(userId))
                return await db.Orders.Include(o => o.Items)
                                      .AsNoTracking()
                                      .ToListAsync(ct);

            return await db.Orders.Include(o => o.Items)
                                  .Where(o => o.UserId == userId)
                                  .AsNoTracking()
                                  .ToListAsync(ct);
        }

        public async Task UpdateOrderStatusAsync(
            int orderId,
            OrderStatus status,
            string? paymentIntentId,
            CancellationToken ct = default)
        {
            await orderRepo.UpdateOrderStatusAsync(orderId, status, paymentIntentId, ct);
            await db.SaveChangesAsync(ct);

            log.LogInformation(
                "Order #{OrderId} status updated to {Status} (PaymentIntentId: {Intent})",
                orderId, status, paymentIntentId ?? "n/a");
        }

        public async Task UpdateOrderAsync(Order order, CancellationToken ct = default)
        {
            await orderRepo.UpdateOrderAsync(order, ct);
            await db.SaveChangesAsync(ct);
        }

        public Task<int> CountOrderByUserIdAsync(string userId, CancellationToken ct = default) =>
            orderRepo.CountOrderByUserIdAsync(userId, ct);
    }
}
