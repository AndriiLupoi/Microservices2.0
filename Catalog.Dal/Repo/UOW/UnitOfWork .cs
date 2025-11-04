using Catalog.Dal.Context;
using Catalog.Dal.Repo.Implementations;
using Catalog.Dal.Repo.Interfaces;

namespace Catalog.Dal.Repo.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatalogDbContext _context;
        public IBrandRepository Brands { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }
        public IProductCategoryRepository ProductCategories { get; private set; }

        public UnitOfWork(CatalogDbContext context)
        {
            _context = context;

            // Ініціалізація репозиторіїв
            Brands = new BrandRepository(_context);
            Categories = new CategoryRepository(_context);
            Products = new ProductRepository(_context);
            ProductCategories = new ProductCategoryRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
