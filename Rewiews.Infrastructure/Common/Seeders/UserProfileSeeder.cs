using MongoDB.Bson;
using MongoDB.Driver;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using Rewiews.Domain.ValueObjects;
using Rewiews.Infrastructure.Context;

namespace Rewiews.Infrastructure.Common.Seeders
{
    public class UserProfileSeeder : IDataSeeder
    {
        private readonly IMongoCollection<UserProfile> _users;

        public UserProfileSeeder(MongoDbContext database)
        {
            _users = database.Users;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            var existingCount = await _users.CountDocumentsAsync(FilterDefinition<UserProfile>.Empty, cancellationToken: cancellationToken);
            if (existingCount > 0) return; // Ідемпотентність

            var seedData = new List<UserProfile>
        {
            new UserProfile {Id = ObjectId.GenerateNewId().ToString(), Username = "john_doe", email = new Email("john@example.com") },
            new UserProfile {Id = ObjectId.GenerateNewId().ToString(), Username = "jane_smith", email = new Email("jane@example.com") }
        };

            await _users.InsertManyAsync(seedData, cancellationToken: cancellationToken);
        }
    }
}
