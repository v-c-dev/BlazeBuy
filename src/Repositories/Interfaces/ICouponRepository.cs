using BlazeBuy.Models;

namespace BlazeBuy.Repositories.Interfaces
{
    public interface ICouponRepository
    {
        Task<Coupon?> GetCouponByCodeAsync(string code, CancellationToken ct = default);
        Task<List<Coupon>> GetAllCouponsAsync(CancellationToken ct = default);
        Task<Coupon> CreateAsync(Coupon coupon, CancellationToken ct = default);
        Task UpdateAsync(Coupon coupon, CancellationToken ct = default);
        Task DeleteAsync(Coupon coupon, CancellationToken ct = default);
    }
}
