using BlazeBuy.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazeBuy.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
