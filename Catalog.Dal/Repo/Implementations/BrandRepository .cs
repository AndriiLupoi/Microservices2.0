using Catalog.Dal.Context;
using Catalog.Dal.Repo.Interfaces;
using Catalog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Dal.Repo.Implementations
{
    public class BrandRepository : AsyncRepository<Brand>, IBrandRepository
    {

        private readonly CatalogDbContext _db;
        public BrandRepository(CatalogDbContext context) : base(context) {
            _db = context;
        }

        // Explicit Loading
        public async Task<Brand> GetBrandWithProductsExplicitAsync(int brandId)
        {
            var brand = await _dbSet.FindAsync(brandId);
            if (brand != null)
            {
                await _context.Entry(brand)
                    .Collection(b => b.Products)
                    .LoadAsync();
            }
            return brand;
        }

        public IQueryable<Brand> Query() => _db.Brands.AsQueryable();
    }
}
