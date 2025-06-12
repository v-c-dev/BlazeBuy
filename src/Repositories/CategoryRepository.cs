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

        public Task<Category?> GetCategoryByIdAsync(int id, CancellationToken ct = default) =>
            _db.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);

        public Task<bool> CategoryExistsAsync(int id, CancellationToken ct = default) =>
            _db.Categories.AnyAsync(c => c.Id == id, ct);

        public async Task CreateCategoryAsync(Category entity, CancellationToken ct = default) =>
            await _db.Categories.AddAsync(entity, ct);

        public Task UpdateCategoryAsync(Category entity)
        {
            _db.Categories.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteCategoryAsync(Category entity)
        {
            _db.Categories.Remove(entity);
            return Task.CompletedTask;
        }
    }
}
