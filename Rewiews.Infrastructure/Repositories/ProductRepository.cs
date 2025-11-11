using MongoDB.Driver;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using Rewiews.Infrastructure.Context;

namespace Rewiews.Infrastructure.Repositories
{
    public class ProductRepository : MongoRepository<Product>, IProductRepository
    {
        public ProductRepository(MongoDbContext context)
            : base(context.Products) { }

        // 🔹 Текстовий пошук
        public async Task<IReadOnlyCollection<Product>> SearchByTextAsync(string text)
        {
            var filter = Builders<Product>.Filter.Text(text);
            var results = await _collection.Find(filter).ToListAsync();
            return results.AsReadOnly();
        }

        // 🔹 Aggregation pipeline
        public async Task<IReadOnlyCollection<TResult>> AggregateAsync<TResult>(
            Func<IQueryable<Product>, IQueryable<TResult>> pipeline)
        {
            var queryable = _collection.AsQueryable();
            var projected = pipeline(queryable);
            return projected.ToList().AsReadOnly();
        }

        // 🔹 Optimistic concurrency
        public async Task<bool> UpdateWithConcurrencyCheckAsync(Product product, DateTime originalUpdatedAt)
        {
            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(p => p.Id, product.Id),
                Builders<Product>.Filter.Eq(p => p.UpdatedAt, originalUpdatedAt)
            );

            product.UpdatedAt = DateTime.UtcNow;

            var result = await _collection.ReplaceOneAsync(filter, product);
            return result.ModifiedCount > 0;
        }
    }
}
