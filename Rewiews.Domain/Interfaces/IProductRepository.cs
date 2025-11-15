using Rewiews.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rewiews.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(string id);
        Task<IReadOnlyCollection<Product>> ListAllAsync();
        Task<IReadOnlyCollection<Product>> ListAsync(Expression<Func<Product, bool>> filter);

        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(string id);

        Task<bool> ExistsAsync(string id, CancellationToken cancellation);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<Product>> GetProductsAsync(
                    string? cursorId = null,
                    int pageSize = 10,
                    string? searchText = null,
                    string? sortBy = null,
                    bool sortDesc = false
                );
    }
}
