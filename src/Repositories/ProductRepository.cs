using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BlazeBuy.Repositories
{
    internal sealed class ProductRepository(ApplicationDbContext db) : IProductRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync(CancellationToken ct = default) =>
            await _db.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync(ct);

        public Task<Product?> GetProductAsync(int id, CancellationToken ct = default) =>
            _db.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, ct);


        public async Task CreateProductAsync(Product entity, CancellationToken ct = default) =>
            await _db.Products.AddAsync(entity, ct);

        public Task UpdateProductAsync(Product entity)
        {
            _db.Products.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteProductAsync(Product entity)
        {
            _db.Products.Remove(entity);
            return Task.CompletedTask;
        }
    }
}
