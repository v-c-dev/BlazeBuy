using BlazeBuy.Models;

namespace BlazeBuy.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetAllCategoriesAsync();
        public Task<Category> GetCategoryByIdAsync(int id);
        public Task<Category> CreateCategoryAsync(Category obj);
        public Task<Category> UpdateCategoryAsync(Category obj);
        public Task<bool> DeleteCategoryAsync(int id);
    }
}
