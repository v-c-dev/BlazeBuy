using BlazeBuy.Models;

namespace BlazeBuy.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IReadOnlyList<Category>> GetAllCategoriesAsync(CancellationToken ct = default);
        Task<Category?> GetCategoryByIdAsync(int id, CancellationToken ct = default);
        Task<bool> CategoryExistsAsync(int id, CancellationToken ct = default);

        Task<Category> CreateCategoryAsync(Category entity, CancellationToken ct = default);
        Task UpdateCategoryAsync(Category entity, CancellationToken ct = default);
        Task DeleteCategoryAsync(Category entity, CancellationToken ct = default);
    }
}
