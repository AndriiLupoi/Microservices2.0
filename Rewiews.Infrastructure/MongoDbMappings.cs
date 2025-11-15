using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Rewiews.Domain.Common;
using Rewiews.Domain.Entities;

public static class MongoDbMappings
{
    public static void RegisterClassMaps()
    {
        // ----------------- BaseEntity -----------------
        if (!BsonClassMap.IsClassMapRegistered(typeof(BaseEntity)))
        {
            BsonClassMap.RegisterClassMap<BaseEntity>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                  .SetIdGenerator(StringObjectIdGenerator.Instance)
                  .SetSerializer(new StringSerializer(BsonType.ObjectId));
                cm.SetIgnoreExtraElements(true);
            });
        }

        // ----------------- Product -----------------
        if (!BsonClassMap.IsClassMapRegistered(typeof(Product)))
        {
            BsonClassMap.RegisterClassMap<Product>(cm =>
            {
                cm.AutoMap();
                cm.MapProperty(p => p.Reviews).SetElementName("reviews");
                cm.SetIgnoreExtraElements(true);
            });
        }

        // ----------------- Review -----------------
        if (!BsonClassMap.IsClassMapRegistered(typeof(Review)))
        {
            BsonClassMap.RegisterClassMap<Review>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }

        // ----------------- UserProfile -----------------
        if (!BsonClassMap.IsClassMapRegistered(typeof(UserProfile)))
        {
            BsonClassMap.RegisterClassMap<UserProfile>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
