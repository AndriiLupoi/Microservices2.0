using Catalog.Dal.Context;
using Catalog.Dal.Repo.Interfaces;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Dal.Repo.Implementations
{
    public class CategoryRepository : AsyncRepository<Category>, ICategoryRepository
    {

        private readonly CatalogDbContext _db;
        public CategoryRepository(CatalogDbContext context) : base(context)
        {
            _db = context;
        }

        // Eager Loading через проміжну таблицю
        public async Task<IEnumerable<Category>> GetCategoriesWithProductsAsync()
        {
            return await _dbSet
                .Include(c => c.ProductCategories)
                    .ThenInclude(pc => pc.Product)
                .ToListAsync();
        }

        // LINQ to Entities
        public async Task<IEnumerable<Product>> GetProductsForCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
                .Include(p => p.Brand)
                .ToListAsync();
        }
        public new async Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _dbSet.Where(c => ids.Contains(c.CategoryId)).ToListAsync();
        }

        public IQueryable<Category> Query() => _db.Categories.AsQueryable();
    }
}
