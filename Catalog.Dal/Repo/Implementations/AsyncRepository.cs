using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Catalog.Dal.Context;
using Catalog.Dal.Repo.Interfaces;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Dal.Repo.Implementations
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly CatalogDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public AsyncRepository(CatalogDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _dbSet
                .Where(e => ids.Contains(EF.Property<int>(e, "Id")))
                .ToListAsync();
        }

        // ❌ тимчасово не використовується, бо тільки для Product
        Task<IEnumerable<Product>> IAsyncRepository<T>.GetProductsByBrandIdAsync(int brandId)
        {
            throw new NotImplementedException();
        }

        // ✅ реалізація через SpecificationEvaluator
        public async Task<IEnumerable<T>> ListAsync(ISpecification<T> spec)
        {
            var query = ApplySpecification(spec);
            return await query.ToListAsync();
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            var query = ApplySpecification(spec);
            return await query.FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator.Default.GetQuery(_dbSet.AsQueryable(), spec);
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            var query = ApplySpecification(spec);
            return await query.CountAsync();
        }

    }
}
