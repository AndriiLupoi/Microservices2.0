using MongoDB.Bson;
using MongoDB.Driver;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using Rewiews.Domain.ValueObjects;
using Rewiews.Infrastructure.Context;

namespace Rewiews.Infrastructure.Common.Seeders
{ 
    public class ProductSeeder : IDataSeeder
    {
        private readonly IMongoCollection<Product> _products;

        public ProductSeeder(MongoDbContext context)
        {
            _products = context.Products;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            var existingCount = await _products.CountDocumentsAsync(FilterDefinition<Product>.Empty, cancellationToken: cancellationToken);
            if (existingCount > 0) return; 

            var seedData = new List<Product>
        {
            new Product {Id = ObjectId.GenerateNewId().ToString(), Name = "Brake Pad", Description = "Front brake pads for sedan", price = new Money(30, "USD") },
            new Product {Id = ObjectId.GenerateNewId().ToString(),  Name = "Oil Filter", Description = "Engine oil filter", price = new Money(15, "USD") },
            new Product {Id = ObjectId.GenerateNewId().ToString(),  Name = "Spark Plug", Description = "Standard spark plug", price = new Money(8, "USD") }
        };

            await _products.InsertManyAsync(seedData, cancellationToken: cancellationToken);
        }
    }

}
