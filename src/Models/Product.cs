using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlazeBuy.Models
{
    public sealed class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Name { get; set; } = default!;

        [Required, Range(0.01, 10_000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(2_000)]
        public string? Description { get; set; }

        public string? SpecialTag { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; } = default!;

        public string? ImageUrl { get; set; }
    }
}
