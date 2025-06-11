using BlazeBuy.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazeBuy.Models
{
    public sealed class ShoppingCart
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        [Required, Range(1, 100)]
        public int Quantity { get; set; }
    }
}
