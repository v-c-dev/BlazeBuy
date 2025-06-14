using BlazeBuy.Models;

namespace BlazeBuy.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync(CancellationToken ct = default);
        Task<Product?> GetProductByIdAsync(int id, CancellationToken ct = default);

        Task<Product> CreateProductAsync(Product entity, CancellationToken ct = default);
        Task UpdateProductAsync(Product entity, CancellationToken ct = default);
        Task DeleteProductAsync(Product entity, CancellationToken ct = default);
    }
}
