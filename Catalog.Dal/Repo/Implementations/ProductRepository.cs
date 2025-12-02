using Catalog.Dal.Context;
using Catalog.Dal.Repo.Interfaces;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Dal.Repo.Implementations
{
    public class ProductRepository : AsyncRepository<Product>, IProductRepository
    {

        private readonly CatalogDbContext _db;
        public ProductRepository(CatalogDbContext context) : base(context) { 
            _db = context;
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return await _db.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public IQueryable<Product> Query() => _db.Products.AsQueryable();
    }
}
