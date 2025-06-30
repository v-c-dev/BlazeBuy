using BlazeBuy.Data;
using BlazeBuy.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazeBuy.Models
{
    public sealed class Order
    {
        public int Id { get; set; }

        
        [Required]
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Order Total")]
        public decimal Total { get; set; }

        public DateTimeOffset CreatedUtc { get; set; } = DateTimeOffset.UtcNow;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string? SessionId { get; set; }   // for Stripe Checkout
        public string? PaymentIntentId { get; set; }   // for Stripe PaymentIntent

        [Required, Display(Name = "Name")]
        public string Name { get; set; } = default!;

        [Required, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = default!;

        [Required, EmailAddress, Display(Name = "Email")]
        public string Email { get; set; } = default!;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public int? CouponId { get; set; }
        public Coupon? Coupon { get; set; }
        public decimal? DiscountAmount { get; set; }

        [NotMapped]
        public decimal FinalTotal => DiscountAmount.HasValue ? Math.Max(0, Total - DiscountAmount.Value) : Total;
    }
}
