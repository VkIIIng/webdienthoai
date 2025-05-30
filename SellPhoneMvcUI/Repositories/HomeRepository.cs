using Microsoft.EntityFrameworkCore;
using SellPhoneMvcUI.Interfaces;
namespace SellPhoneMvcUI.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;
        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await this._db.Categories.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsAsync(string sTerm = "", int category = 0)
        {
            var query = this._db.Products
                .Include(p => p.Category) 
                .AsQueryable();

            if (!string.IsNullOrEmpty(sTerm))
            {
                sTerm = sTerm.ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(sTerm) ||
                                        p.Brand.ToLower().Contains(sTerm) ||
                                        p.Description.ToLower().Contains(sTerm));
            }

            if (category > 0)
            {
                query = query.Where(p => p.CategoryId == category);
            }

            var products = await query.ToListAsync();

            return products;
        }

        public async Task<Product> GetProductById(int id)
        {
            var product = await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
            return product;
        }
    }
}
