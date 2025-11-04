using Catalog.Dal.Context;
using Catalog.Dal.Repo.Interfaces;
using Catalog.Domain.Entity;

namespace Catalog.Dal.Repo.Implementations
{
    public class ProductCategoryRepository : AsyncRepository<ProductCategory>, IProductCategoryRepository
    {

        private readonly CatalogDbContext _db;
        public ProductCategoryRepository(CatalogDbContext context) : base(context)
        { 
            _db = context;
        }

        public IQueryable<ProductCategory> Query() => _db.ProductCategories.AsQueryable();
    }
}
