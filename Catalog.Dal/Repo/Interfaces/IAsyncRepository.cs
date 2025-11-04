using Ardalis.Specification;
using Catalog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.Dal.Repo.Interfaces
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<Product>> GetProductsByBrandIdAsync(int brandId);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> SaveChangesAsync();
        Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids);

        Task<IEnumerable<T>> ListAsync(ISpecification<T> spec);
        Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);


    }
}
