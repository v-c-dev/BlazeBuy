using BlazeBuy.Models;

namespace BlazeBuy.Services.Interfaces
{
    public interface ICouponService
    {
        Task<Coupon?> GetByCodeAsync(string code, CancellationToken ct = default);
        Task<List<Coupon>> GetAllCouponsAsync(CancellationToken ct = default);
        Task<Coupon> CreateCouponAsync(Coupon coupon, CancellationToken ct = default);
        Task UpdateCouponAsync(Coupon coupon, CancellationToken ct = default);
        Task<bool> DeleteCouponAsync(int id, CancellationToken ct = default);

        Task<decimal> CalculateDiscountAsync(Coupon coupon, Order draftOrder);
    }
}
