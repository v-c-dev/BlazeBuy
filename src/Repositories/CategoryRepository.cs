using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazeBuy.Repositories
{
    internal sealed class CategoryRepository(ApplicationDbContext db) : ICategoryRepository
    {
        private readonly ApplicationDbContext _db = db;

        public async Task<IReadOnlyList<Category>> GetAllCategoriesAsync(CancellationToken ct = default) =>
            await _db.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync(ct);

        public async Task<Category?> GetCategoryByIdAsync(int id, CancellationToken ct = default) =>
            await _db.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);

        public async Task<bool> CategoryExistsAsync(int id, CancellationToken ct = default) =>
            await _db.Categories.AnyAsync(c => c.Id == id, ct);

        public async Task<Category> CreateCategoryAsync(Category entity, CancellationToken ct = default)
        {
            _db.Categories.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task UpdateCategoryAsync(Category entity, CancellationToken ct = default)
        {
            _db.Categories.Update(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteCategoryAsync(Category entity, CancellationToken ct = default)
        {
            _db.Categories.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }
    }
}
