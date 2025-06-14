﻿using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using BlazeBuy.Services.Interfaces;

namespace BlazeBuy.Services
{
    internal sealed class ProductService(ApplicationDbContext _db, IProductRepository _repo, 
        ILogger<ProductService> log) : IProductService
    {
        public Task<IReadOnlyList<Product>> GetAllProductsAsync(CancellationToken ct = default) =>
            _repo.GetAllProductsAsync(ct);

        public Task<Product?> GetProductByIdAsync(int id, CancellationToken ct = default) =>
            _repo.GetProductByIdAsync(id, ct);

        public async Task<Product> CreateProductAsync(Product product, CancellationToken ct = default)
        {
            await _repo.CreateProductAsync(product, ct);
            await _db.SaveChangesAsync(ct);
            return product;
        }

        public Task UpdateProductAsync(Product product, CancellationToken ct = default)
            => _repo.UpdateProductAsync(product, ct);

        public async Task<bool> DeleteProductAsync(int id, CancellationToken ct = default)
        {
            var product = await _repo.GetProductByIdAsync(id, ct);
            if (product is null) return false;

            await _repo.DeleteProductAsync(product, ct);
            return true;
        }
    }
}
