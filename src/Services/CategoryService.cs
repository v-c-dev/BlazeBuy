using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using BlazeBuy.Services.Interfaces;

namespace BlazeBuy.Services
{
    internal sealed class CategoryService(ApplicationDbContext db, ICategoryRepository repo,
        IProductRepository productRepo, ILogger<CategoryService> log) : ICategoryService
    {
        public Task<IReadOnlyList<Category>> GetAllCategoriesAsync(CancellationToken ct = default) =>
            repo.GetAllCategoriesAsync(ct);

        public Task<Category?> GetCategoryByIdAsync(int id, CancellationToken ct = default) =>
            repo.GetCategoryByIdAsync(id, ct);

        public async Task<Category> CreateCategoryAsync(Category category, CancellationToken ct = default)
        {
            await repo.CreateCategoryAsync(category, ct);
            await db.SaveChangesAsync(ct);
            return category;
        }

        public async Task UpdateCategoryAsync(Category category, CancellationToken ct = default)
        {
            await repo.UpdateCategoryAsync(category);
            await db.SaveChangesAsync(ct);
        }

        public async Task<bool> DeleteCategoryAsync(int id, CancellationToken ct = default)
        {
            if (await productRepo.GetAllProductsAsync(ct).ContinueWith(t => t.Result.Any(p => p.CategoryId == id), ct))
                return false;

            var category = await repo.GetCategoryByIdAsync(id, ct);
            if (category is null) return false;

            await repo.DeleteCategoryAsync(category);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }
}
