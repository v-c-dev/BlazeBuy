using BlazeBuy.Models;

namespace BlazeBuy.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetAllCategoriesAsync(CancellationToken ct = default);
        Task<Category?> GetCategoryByIdAsync(int id, CancellationToken ct = default);
        public Task<Category> CreateCategoryAsync(Category category, CancellationToken ct = default);
        Task UpdateCategoryAsync(Category category, CancellationToken ct = default);
        Task<bool> DeleteCategoryAsync(int id, CancellationToken ct = default); 
    }
}
