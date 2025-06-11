using BlazeBuy.Models;

namespace BlazeBuy.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync(CancellationToken ct = default);
        Task<Product?> GetProductAsync(int id, CancellationToken ct = default);

        Task CreateProductAsync(Product entity, CancellationToken ct = default);
        Task UpdateProductAsync(Product entity);
        Task DeleteProductAsync(Product entity);
    }
}
