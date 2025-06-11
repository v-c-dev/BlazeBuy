using BlazeBuy.Data;
using BlazeBuy.Models;
using BlazeBuy.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BlazeBuy.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Product> CreateProductAsync(Product obj)
        {
            await _db.Products.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }
        public async Task<Product> UpdateProductAsync(Product obj)
        {
            var objFromDb = await _db.Products.FirstOrDefaultAsync(u => u.Id == obj.Id);
            if (objFromDb is not null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.SpecialTag = obj.SpecialTag;
                objFromDb.ImageUrl = obj.ImageUrl;
                _db.Products.Update(objFromDb);
                await _db.SaveChangesAsync();
                return objFromDb;
            }
            return obj;
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(u => u.Id == id);
            if (obj == null)
            {
                return false;
            }
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('/'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
            _db.Products.Remove(obj);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return (await _db.Products.FirstOrDefaultAsync(u => u.Id == id));
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _db.Products.Include(u => u.Category).ToListAsync();
        }
    }
}
