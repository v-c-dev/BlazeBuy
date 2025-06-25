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
            var local = _db.Coupons.Local.FirstOrDefault(c => c.Id == coupon.Id);
            if (local is not null) _db.Entry(local).State = EntityState.Detached;

            _db.Coupons.Update(coupon);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Coupon coupon, CancellationToken ct = default)
        {
            _db.Coupons.Remove(coupon);
            await _db.SaveChangesAsync(ct);
        }
    }
}
