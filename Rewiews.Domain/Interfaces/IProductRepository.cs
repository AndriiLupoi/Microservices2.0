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

        // NoSQL специфіка: пошук за критеріями
        Task<IReadOnlyCollection<Product>> SearchByTextAsync(string searchText);

        // Aggregation: приклад для MongoDB-like pipeline
        Task<IReadOnlyCollection<TResult>> AggregateAsync<TResult>(Func<IQueryable<Product>, IQueryable<TResult>> pipeline);

        // Optimistic concurrency: повертає true, якщо оновлення пройшло
        Task<bool> UpdateWithConcurrencyCheckAsync(Product product, DateTime originalUpdatedAt);
    }
}
