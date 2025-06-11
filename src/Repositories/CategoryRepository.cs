using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazeBuy.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return (await _db.Categories.FirstOrDefaultAsync(u => u.Id == id));
        }


        public async Task<Category> CreateCategoryAsync(Category obj)
        {
            await _db.Categories.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<Category> UpdateCategoryAsync(Category obj)
        {
            var objFormDb = await _db.Categories.FirstOrDefaultAsync(u => u.Id == obj.Id);
            if (objFormDb is not null)
            {
                objFormDb.Name = obj.Name;
                _db.Categories.Update(objFormDb);
                await _db.SaveChangesAsync();
                return objFormDb;
            }

            return obj;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var obj = await _db.Categories.FirstOrDefaultAsync(u => u.Id == id);
            if (obj == null)
            {
                return false;
            }

            _db.Categories.Remove(obj);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
