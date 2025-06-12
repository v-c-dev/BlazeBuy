using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using BlazeBuy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazeBuy.Services
{
    internal sealed class ShoppingCartService(ApplicationDbContext db, IProductRepository productRepo,
        ILogger<ShoppingCartService> log) : IShoppingCartService
    {
        public async Task AddCartItemAsync(string userId, int productId, int qty = 1, CancellationToken ct = default)
        {
            var existing = await db.ShoppingCarts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId, ct);

            if (existing is null)
            {
                db.ShoppingCarts.Add(new ShoppingCart { UserId = userId, ProductId = productId, Quantity = qty });
            }
            else
            {
                existing.Quantity += qty;
            }

            await db.SaveChangesAsync(ct);
        }

        public async Task RemoveCartItemAsync(string userId, int productId, CancellationToken ct = default)
        {
            var existing = await db.ShoppingCarts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId, ct);
            if (existing is null) return;

            db.ShoppingCarts.Remove(existing);
            await db.SaveChangesAsync(ct);
        }

        public async Task ClearCartAsync(string userId, CancellationToken ct = default)
        {
            db.ShoppingCarts.RemoveRange(db.ShoppingCarts.Where(c => c.UserId == userId));
            await db.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<ShoppingCart>> GetCartAsync(string userId, CancellationToken ct = default)
        {
            return await db.ShoppingCarts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
