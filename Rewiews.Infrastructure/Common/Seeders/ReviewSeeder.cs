using MongoDB.Bson;
using MongoDB.Driver;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using Rewiews.Infrastructure.Context;

namespace Rewiews.Infrastructure.Common.Seeders
{
    public class ReviewSeeder : IDataSeeder
    {
        private readonly IMongoCollection<Review> _reviews;
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<UserProfile> _users;

        public ReviewSeeder(MongoDbContext database)
        {
            _reviews = database.Reviews;
            _products = database.Products;
            _users = database.Users;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            var existingCount = await _reviews.CountDocumentsAsync(FilterDefinition<Review>.Empty, cancellationToken: cancellationToken);
            if (existingCount > 0) return; 

            var products = await _products.Find(FilterDefinition<Product>.Empty).Limit(3).ToListAsync(cancellationToken);
            var users = await _users.Find(FilterDefinition<UserProfile>.Empty).Limit(2).ToListAsync(cancellationToken);

            if (!products.Any() || !users.Any()) return;

            var seedData = new List<Review>
            {
                new Review {Id = ObjectId.GenerateNewId().ToString(), UserId = users[0].Id!, ProductId = products[0].Id!, Rating = 5, Comment = $"Brake pads fit perfectly on {products[0].Name}" },
                new Review {Id = ObjectId.GenerateNewId().ToString(), UserId = users[1].Id!, ProductId = products[1].Id!, Rating = 4, Comment = $"Oil filter {products[1].Name} works fine, but delivery delayed" },
                new Review {Id = ObjectId.GenerateNewId().ToString(), UserId = users[0].Id!, ProductId = products[2].Id !, Rating = 3, Comment = $"Average quality for {products[2].Name}" },
                new Review {Id = ObjectId.GenerateNewId().ToString(),  UserId = users[0].Id!, ProductId = products[0].Id !, Rating = 5, Comment = $"Engine belt {products[0].Name} is excellent" },
                new Review {Id = ObjectId.GenerateNewId().ToString(),  UserId = users[1].Id!, ProductId = products[2].Id !, Rating = 2, Comment = $"Brake pads {products[2].Name} worn out quickly" }
            };

            await _reviews.InsertManyAsync(seedData, cancellationToken: cancellationToken);
        }
    }
}
