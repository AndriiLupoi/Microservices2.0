using MongoDB.Driver;
using Rewiews.Domain.Entities;
using Rewiews.Domain.ValueObjects;
using MongoDB.Bson.Serialization;

namespace Rewiews.Infrastructure.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;

        public MongoDbContext(string connectionString, string databaseName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);

            ConfigureMappings();
            MongoIndexes.Configure(_database);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<UserProfile> Users => _database.GetCollection<UserProfile>("Users");

        public IMongoCollection<Review> Reviews => _database.GetCollection<Review>("Reviews");

        public IClientSessionHandle StartSession() => _client.StartSession();

        private void ConfigureMappings()
        {
            BsonClassMap.RegisterClassMap<Money>(cm =>
            {
                cm.AutoMap();
                cm.MapCreator(m => new Money(m.Amount, m.Currency));
            });

            BsonClassMap.RegisterClassMap<Email>(cm =>
            {
                cm.AutoMap();
                cm.MapCreator(e => new Email(e.Value));
            });
        }
    }
}
