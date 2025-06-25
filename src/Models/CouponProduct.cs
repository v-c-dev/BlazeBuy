namespace BlazeBuy.Models
{
    public sealed class CouponProduct
    {
        public int CouponId { get; set; }
        public int ProductId { get; set; }
        public Coupon Coupon { get; set; } = default!;
        public Product Product { get; set; } = default!;
    }
}
