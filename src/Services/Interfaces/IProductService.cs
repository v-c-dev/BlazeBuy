using BlazeBuy.Models;

namespace BlazeBuy.Services.Interfaces
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync(CancellationToken ct = default);
        Task<Product?> GetProductByIdAsync(int id, CancellationToken ct = default);
        Task<Product> CreateProductAsync(Product product, CancellationToken ct = default);
        Task UpdateProductAsync(Product product, CancellationToken ct = default);
        Task<bool> DeleteProductAsync(int id, CancellationToken ct = default);
        Task<bool> AdjustQuantityAsync(int productId, int delta, CancellationToken ct = default);
    }
}
