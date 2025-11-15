using MongoDB.Bson;
using MongoDB.Driver;
using Rewiews.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Rewiews.Infrastructure.Repositories
{
    public class MongoRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public virtual async Task AddAsync(T entity)
        {
            // ✅ Генеруємо Id якщо він null або порожній
            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = ObjectId.GenerateNewId().ToString();
            }

            // Встановлюємо дати
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _collection.InsertOneAsync(entity);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;

            await _collection.ReplaceOneAsync(
                e => e.Id == entity.Id,
                entity,
                new ReplaceOptions { IsUpsert = false }
            );
        }

        public virtual async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(e => e.Id == id);
        }

        public virtual async Task<T?> GetByIdAsync(string id)
        {
            return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<IReadOnlyCollection<T>> ListAllAsync()
        {
            var list = await _collection.Find(_ => true).ToListAsync();
            return list.AsReadOnly();
        }

        public virtual async Task<IReadOnlyCollection<T>> ListAsync(Expression<Func<T, bool>> filter)
        {
            var list = await _collection.Find(filter).ToListAsync();
            return list.AsReadOnly();
        }

        public virtual async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(e => e.Id == id).AnyAsync(cancellationToken);
        }
    }
}