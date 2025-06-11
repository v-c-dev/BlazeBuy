using BlazeBuy.Models;

namespace BlazeBuy.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<Product> CreateProductAsync(Product obj);
        public Task<Product> UpdateProductAsync(Product obj);
        public Task<bool> DeleteProductAsync(int id);
        public Task<Product> GetProductByIdAsync(int id);
        public Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
