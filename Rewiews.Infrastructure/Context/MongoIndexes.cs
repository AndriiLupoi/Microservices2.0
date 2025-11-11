using MongoDB.Driver;
using Rewiews.Domain.Entities;
using System;

namespace Rewiews.Infrastructure.Context
{
    public static class MongoIndexes
    {
        public static void Configure(IMongoDatabase database)
        {
            var products = database.GetCollection<Product>("Products");
            var users = database.GetCollection<UserProfile>("Users");
            var reviews = database.GetCollection<Review>("Reviews");

            // ---------------------------
            // Product indexes
            // ---------------------------

            // Text index для пошуку по Name та Description
            var productTextIndex = Builders<Product>.IndexKeys
                .Text(p => p.Name)
                .Text(p => p.Description);
            products.Indexes.CreateOne(new CreateIndexModel<Product>(productTextIndex));

            // Compound index для швидкого пошуку по ціні і оновленню
            var productCompoundIndex = Builders<Product>.IndexKeys
                .Ascending(p => p.price.Amount)
                .Descending(p => p.UpdatedAt);
            products.Indexes.CreateOne(new CreateIndexModel<Product>(productCompoundIndex));

            // ---------------------------
            // UserProfile indexes
            // ---------------------------

            // Унікальний індекс по Username
            var userIndex = Builders<UserProfile>.IndexKeys.Ascending(u => u.Username);
            users.Indexes.CreateOne(new CreateIndexModel<UserProfile>(userIndex, new CreateIndexOptions
            {
                Unique = true
            }));

            // ---------------------------
            // Review indexes
            // ---------------------------

            // Text index для Comment
            var reviewTextIndex = Builders<Review>.IndexKeys.Text(r => r.Comment);
            reviews.Indexes.CreateOne(new CreateIndexModel<Review>(reviewTextIndex));

            // Compound index для швидкого пошуку по UserId та CreatedAt
            var reviewCompoundIndex = Builders<Review>.IndexKeys
                .Ascending(r => r.UserId)
                .Descending(r => r.CreatedAt);
            reviews.Indexes.CreateOne(new CreateIndexModel<Review>(reviewCompoundIndex));

            // TTL index: видалення відгуків старших ніж 90 днів (опційно)
            var ttlIndex = Builders<Review>.IndexKeys.Ascending(r => r.CreatedAt);
            reviews.Indexes.CreateOne(new CreateIndexModel<Review>(ttlIndex, new CreateIndexOptions
            {
                ExpireAfter = TimeSpan.FromDays(90)
            }));
        }
    }
}
