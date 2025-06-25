using BlazeBuy.Models;
using BlazeBuy.Models.Enums;
using BlazeBuy.Repositories.Interfaces;
using BlazeBuy.Services.Interfaces;

namespace BlazeBuy.Services
{
    internal sealed class CouponService(ICouponRepository repo) : ICouponService
    {
        public Task<Coupon?> GetByCodeAsync(string code, CancellationToken ct = default) =>
            repo.GetCouponByCodeAsync(code, ct);

        public Task<List<Coupon>> GetAllCouponsAsync(CancellationToken ct = default) =>
            repo.GetAllCouponsAsync(ct);

        public Task<Coupon> CreateCouponAsync(Coupon coupon, CancellationToken ct = default) =>
            repo.CreateAsync(coupon, ct);

        public Task UpdateCouponAsync(Coupon coupon, CancellationToken ct = default) =>
            repo.UpdateAsync(coupon, ct);

        public async Task<bool> DeleteCouponAsync(int id, CancellationToken ct = default)
        {
            var cpn = await repo.GetAllCouponsAsync(ct)
                                .ContinueWith(t => t.Result.FirstOrDefault(c => c.Id == id), ct);

            if (cpn is null) return false;

            await repo.DeleteAsync(cpn, ct);
            return true;
        }

        public Task<decimal> CalculateDiscountAsync(Coupon c, Order order)
        {
            decimal applicableTotal = 0m;

            if (c.Scope == CouponScope.Order)
            {
                applicableTotal = order.Items.Sum(i => i.Quantity * i.UnitPrice);
            }
            else
            {
                var ids = c.Products.Select(p => p.ProductId).ToHashSet();
                applicableTotal = order.Items
                    .Where(i => ids.Contains(i.ProductId))
                    .Sum(i => i.Quantity * i.UnitPrice);
            }

            decimal discount = c.Type == DiscountType.Percentage
                               ? applicableTotal * (c.Value / 100m)
                               : Math.Min(applicableTotal, c.Value);

            return Task.FromResult(decimal.Round(discount, 2));
        }
    }
}
