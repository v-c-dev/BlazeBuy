using BlazeBuy.Data;
using BlazeBuy.Models.Enums;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using BlazeBuy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazeBuy.Services
{
    internal sealed class OrderService(ApplicationDbContext db, IShoppingCartService cartSvc,
    IProductRepository productRepo, IOrderRepository orderRepo, IEmailService emailSvc,
    ILogger<OrderService> log) : IOrderService
    {
        public async Task<Order> CreateOrderAsync(string userId, CancellationToken ct = default)
        {
            await using var tx = await db.Database.BeginTransactionAsync(ct);

            var cart = await cartSvc.GetCartAsync(userId, ct);
            if (!cart.Any()) throw new InvalidOperationException("Cart empty.");

            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                Name = cart.First().User.UserName ?? userId,
                Email = cart.First().User.Email ?? string.Empty,
                PhoneNumber = string.Empty,
                Total = 0m, // updated below
                Items = cart.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    ProductName = c.Product.Name,
                    Quantity = c.Quantity,
                    UnitPrice = c.Product.Price
                }).ToList()
            };
            order.Total = order.Items.Sum(i => i.UnitPrice * i.Quantity);

            await orderRepo.CreateOrderAsync(order, ct);

            //  optional - reserve stock here
            foreach (var item in order.Items)
                await productRepo.UpdateProductAsync(new Product { Id = item.ProductId, /* ... */ });

            await db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            await cartSvc.ClearCartAsync(userId, ct);
            await emailSvc.SendOrderConfirmationAsync(order, ct);

            return order;
        }

        public Task<Order?> GetOrderByIdAsync(int id, CancellationToken ct = default) =>
            orderRepo.GetOrderByIdAsync(id, ct);

        public async Task<IReadOnlyList<Order>> GetOrderByUserIdAsync(string userId, CancellationToken ct = default) =>
            await db.Orders
                    .Include(o => o.Items)
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.CreatedUtc)
                    .AsNoTracking()
                    .ToListAsync(ct);

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status, CancellationToken ct = default)
        {
            var order = await orderRepo.GetOrderByIdAsync(orderId, ct) ?? throw new KeyNotFoundException();
            order.Status = status;
            await orderRepo.UpdateOrderAsync(order);
            await db.SaveChangesAsync(ct);
        }
    }
}
