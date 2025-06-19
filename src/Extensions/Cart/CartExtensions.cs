using BlazeBuy.Models;

namespace BlazeBuy.Extensions.Cart
{
    public static class CartExtensions
    {
        public static List<OrderItem> ToOrderItems(IEnumerable<ShoppingCart> cart) 
            => cart.Select(c => new OrderItem
            {
                ProductId = c.ProductId,
                ProductName = c.Product.Name,
                Quantity = c.Quantity,
                UnitPrice = c.Product.Price
            }).ToList();
    }
}
