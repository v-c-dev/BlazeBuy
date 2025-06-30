using BlazeBuy.Models;
using BlazeBuy.Models.Enums;
using BlazeBuy.Repositories.Interfaces;
using BlazeBuy.Services.Interfaces;
using Stripe;
using LocalCoupon = BlazeBuy.Models.Coupon;

namespace BlazeBuy.Services
{
    internal sealed class CouponService : ICouponService
    {
        private readonly ICouponRepository _repo;
        private readonly Stripe.CouponService _stripeCouponService;
        private readonly Stripe.PromotionCodeService _promotionCodeService;

        public CouponService(
            ICouponRepository repo,
            Stripe.CouponService stripeCouponService,
            Stripe.PromotionCodeService promotionCodeService)
        {
            _repo = repo;
            _stripeCouponService = stripeCouponService;
            _promotionCodeService = promotionCodeService;
        }

        public async Task<LocalCoupon?> GetByCodeAsync(string code, CancellationToken ct = default) =>
            await _repo.GetCouponByCodeAsync(code, ct);

        public async Task<LocalCoupon?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await _repo.GetCouponByIdAsync(id, ct);

        public async Task<List<LocalCoupon>> GetAllCouponsAsync(CancellationToken ct = default) =>
            await _repo.GetAllCouponsAsync(ct);

        public async Task<LocalCoupon> CreateCouponAsync(LocalCoupon c, CancellationToken ct = default)
        {
            var stripeOpts = new CouponCreateOptions
            {
                Name = c.Code,
                AmountOff = c.Type == DiscountType.Fixed
                    ? (long?)(c.Value * 100m)
                    : null,
                Currency = "usd",
                PercentOff = c.Type == DiscountType.Percentage
                    ? (decimal?)c.Value
                    : null,
                Duration = "once"
            };
            var stripeCoupon = await _stripeCouponService.CreateAsync(stripeOpts, null, ct);

            var promoOpts = new PromotionCodeCreateOptions
            {
                Coupon = stripeCoupon.Id,
                Code = c.Code
            };
            var stripePromo = await _promotionCodeService.CreateAsync(promoOpts, null, ct);

            c.StripeCouponId = stripeCoupon.Id;
            c.StripePromotionCodeId = stripePromo.Id;
            return await _repo.CreateAsync(c, ct);
        }

        public async Task UpdateCouponAsync(LocalCoupon coupon, CancellationToken ct = default)
        {
            await _repo.UpdateAsync(coupon, ct);
        }

        public async Task<bool> DeleteCouponAsync(int id, CancellationToken ct = default)
        {
            var existing = await _repo.GetCouponByIdAsync(id, ct);
            if (existing is null)
                return false;

            // Optional expire/delete in Stripe
            // await _stripeCouponService.DeleteAsync(existing.StripeCouponId, null, ct);

            await _repo.DeleteAsync(existing, ct);
            return true;
        }

        public Task<decimal> CalculateDiscountAsync(LocalCoupon c, Order order)
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
