using MongoDB.Bson;
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

        public async Task<IReadOnlyCollection<Product>> GetProductsAsync(
            string? cursorId = null,
            int pageSize = 10,
            string? searchText = null,
            string? sortBy = null,
            bool sortDesc = false)
        {
            var filter = Builders<Product>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(searchText))
                filter &= Builders<Product>.Filter.Text(searchText);

            if (!string.IsNullOrEmpty(cursorId) && ObjectId.TryParse(cursorId, out var cursorObjectId))
                filter &= Builders<Product>.Filter.Gt(p => p.Id, cursorObjectId.ToString());

            var find = _collection.Find(filter);

            if (!string.IsNullOrEmpty(sortBy))
            {
                var sortDefinition = sortDesc
                    ? Builders<Product>.Sort.Descending(sortBy)
                    : Builders<Product>.Sort.Ascending(sortBy);
                find = find.Sort(sortDefinition);
            }
            else
                find = find.Sort(Builders<Product>.Sort.Ascending(p => p.Id));

            var results = await find.Limit(pageSize).ToListAsync();
            return results.AsReadOnly();
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            return count > 0;
        }

    }
}
