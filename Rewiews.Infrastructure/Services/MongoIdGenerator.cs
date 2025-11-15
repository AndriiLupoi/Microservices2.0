
using MongoDB.Bson;
using Rewiews.Domain.Interfaces;

namespace Rewiews.Infrastructure.Services
{
    public class MongoIdGenerator : IIdGenerator
    {
        public string GenerateId()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}