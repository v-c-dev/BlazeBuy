using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazeBuy.Repositories
{
    internal sealed class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;

        public CouponRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Coupon?> GetCouponByCodeAsync(string code, CancellationToken ct = default)
        {
            return await _db.Coupons
                .Include(c => c.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Code == code && c.IsActive, ct);
        }

        public Task<Coupon?> GetCouponByIdAsync(int id, CancellationToken ct = default) =>
            _db.Coupons
                .Include(c => c.Products)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<List<Coupon>> GetAllCouponsAsync(CancellationToken ct = default)
        {
            return await _db.Coupons
                .Include(c => c.Products)
                .AsNoTracking()
                .OrderBy(c => c.Code)
                .ToListAsync(ct);
        }
            

        public async Task<Coupon> CreateAsync(Coupon coupon, CancellationToken ct = default)
        {
            await _db.Coupons.AddAsync(coupon, ct);
            await _db.SaveChangesAsync(ct);
            return coupon;
        }

        public async Task UpdateAsync(Coupon coupon, CancellationToken ct = default)
        {
            var existing = await _db.Coupons
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == coupon.Id, ct);

            if (existing == null)
                throw new InvalidOperationException("Coupon not found");

            var toRemove = existing.Products
                .Where(cp => !coupon.Products.Any(np => np.ProductId == cp.ProductId))
                .ToList();
            foreach (var r in toRemove)
                _db.Remove(r);

            var toAdd = coupon.Products
                .Where(np => !existing.Products.Any(cp => cp.ProductId == np.ProductId))
                .Select(np => new CouponProduct { CouponId = coupon.Id, ProductId = np.ProductId })
                .ToList();
            foreach (var a in toAdd)
                existing.Products.Add(a);

            _db.Entry(existing).CurrentValues.SetValues(coupon);

            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Coupon coupon, CancellationToken ct = default)
        {
            _db.Coupons.Remove(coupon);
            await _db.SaveChangesAsync(ct);
        }
    }
}
