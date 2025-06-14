using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using BlazeBuy.Services.Interfaces;

namespace BlazeBuy.Services
{
    internal sealed class CategoryService(ApplicationDbContext db, ICategoryRepository repo,
        IProductRepository productRepo) : ICategoryService
    {
        public Task<IReadOnlyList<Category>> GetAllCategoriesAsync(CancellationToken ct = default) =>
            repo.GetAllCategoriesAsync(ct);

        public Task<Category?> GetCategoryByIdAsync(int id, CancellationToken ct = default) =>
            repo.GetCategoryByIdAsync(id, ct);

        public Task<Category> CreateCategoryAsync(Category category, CancellationToken ct = default)
            => repo.CreateCategoryAsync(category, ct);

        public Task UpdateCategoryAsync(Category category, CancellationToken ct = default)
            => repo.UpdateCategoryAsync(category, ct);

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
