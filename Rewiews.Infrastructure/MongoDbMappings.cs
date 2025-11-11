using MongoDB.Bson.Serialization;
using Rewiews.Domain.Entities;
using Rewiews.Infrastructure.Common.Serializers;

namespace Rewiews.Infrastructure
{
    public static class MongoDbMappings
    {
        public static void RegisterClassMaps()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Product)))
            {
                BsonClassMap.RegisterClassMap<Product>(cm =>
                {
                    cm.AutoMap();
                    cm.MapProperty(p => p.Reviews).SetElementName("reviews");
                    cm.MapProperty(p => p.price)
                        .SetSerializer(new MoneySerializer());
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Review)))
            {
                BsonClassMap.RegisterClassMap<Review>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(UserProfile)))
            {
                BsonClassMap.RegisterClassMap<UserProfile>(cm =>
                {
                    cm.AutoMap();
                    cm.MapProperty(u => u.email)
                        .SetSerializer(new EmailSerializer());
                    cm.SetIgnoreExtraElements(true);
                });
            }

        }
    }
}
