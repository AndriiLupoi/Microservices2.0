using Catalog.Dal.Context;
using Catalog.Dal.Repo.Interfaces;
using Catalog.Domain.Entity;

namespace Catalog.Dal.Repo.Implementations
{
    public class ProductRepository : AsyncRepository<Product>, IProductRepository
    {

        private readonly CatalogDbContext _db;
        public ProductRepository(CatalogDbContext context) : base(context) { 
            _db = context;
        }

        public IQueryable<Product> Query() => _db.Products.AsQueryable();
    }
}
