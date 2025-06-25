using System.ComponentModel.DataAnnotations;
using BlazeBuy.Models.Enums;

namespace BlazeBuy.Models
{
    public sealed class Coupon
    {
        public int Id { get; set; }

        [MaxLength(32)]
        [Required]
        public string Code { get; set; } = default!;
        public CouponScope Scope { get; set; }
        public DiscountType Type { get; set; }
        [Range(0, 100_000)]
        public decimal Value { get; set; } 
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;

        public decimal? MinimumOrderTotal { get; set; }

        public ICollection<CouponProduct> Products { get; set; } = [];
    }
}
