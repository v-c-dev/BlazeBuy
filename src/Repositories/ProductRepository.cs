using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BlazeBuy.Repositories
{
    internal sealed class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync(CancellationToken ct = default) =>
            await _db.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync(ct);

        public Task<Product?> GetProductByIdAsync(int id, CancellationToken ct = default) =>
            _db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, ct);


        public async Task<Product> CreateProductAsync(Product entity, CancellationToken ct = default)
        {
            await _db.Products.AddAsync(entity, ct);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateProductAsync(Product entity, CancellationToken ct = default)
        {
            entity.Category = null!;
            _db.Products.Update(entity);
            await _db.SaveChangesAsync(ct);

        }

        public async Task DeleteProductAsync(Product entity, CancellationToken ct = default)
        {
            entity.Category = null!;

            _db.Products.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
    }
}
