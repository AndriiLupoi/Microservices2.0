using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Rewiews.Infrastructure.Repositories
{
    public class MongoRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;
        private readonly PropertyInfo _idProperty;

        public MongoRepository(IMongoCollection<T> collection)
        {
            _collection = collection;

            // Шукаємо властивість Id (string)
            _idProperty = typeof(T).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance)
                          ?? throw new InvalidOperationException($"Type {typeof(T).Name} does not have a public 'Id' property.");
        }

        private string GetId(T entity) =>
            _idProperty.GetValue(entity)?.ToString()
            ?? throw new InvalidOperationException("Entity Id cannot be null.");

        public async Task AddAsync(T entity) => await _collection.InsertOneAsync(entity);

        public async Task UpdateAsync(T entity) =>
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("Id", GetId(entity)), entity);

        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));

        public async Task<T?> GetByIdAsync(string id) =>
            await _collection.Find(Builders<T>.Filter.Eq("Id", id)).FirstOrDefaultAsync();

        public async Task<IReadOnlyCollection<T>> ListAllAsync()
        {
            var list = await _collection.Find(_ => true).ToListAsync();
            return list.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<T>> ListAsync(Expression<Func<T, bool>> filter)
        {
            var list = await _collection.Find(filter).ToListAsync();
            return list.AsReadOnly();
        }

        public async Task<bool> ExistsAsync(string id, CancellationToken cancellation = default)
        {
            var count = await _collection.CountDocumentsAsync(Builders<T>.Filter.Eq("Id", id), cancellationToken: cancellation);
            return count > 0;
        }
    }
}
