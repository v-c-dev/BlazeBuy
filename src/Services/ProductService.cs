using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using BlazeBuy.Services.Interfaces;

namespace BlazeBuy.Services
{
    internal sealed class ProductService(ApplicationDbContext db, IProductRepository repo, 
        ILogger<ProductService> log) : IProductService
    {
        public Task<IReadOnlyList<Product>> GetAllProductsAsync(CancellationToken ct = default) =>
            repo.GetAllProductsAsync(ct);

        public Task<Product?> GetProductByIdAsync(int id, CancellationToken ct = default) =>
            repo.GetProductAsync(id, ct);

        public async Task<Product> CreateProductAsync(Product product, CancellationToken ct = default)
        {
            await repo.CreateProductAsync(product, ct);
            await db.SaveChangesAsync(ct);
            return product;
        }

        public async Task UpdateProductAsync(Product product, CancellationToken ct = default)
        {
            await repo.UpdateProductAsync(product);
            await db.SaveChangesAsync(ct);
        }

        public async Task<bool> DeleteProductAsync(int id, CancellationToken ct = default)
        {
            var product = await repo.GetProductAsync(id, ct);
            if (product is null) return false;

            await repo.DeleteProductAsync(product);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }
}
