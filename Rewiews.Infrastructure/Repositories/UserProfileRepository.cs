using MongoDB.Driver;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using Rewiews.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<UserProfile> _collection;

        public UserProfileRepository(MongoDbContext context)
        {
            _context = context;
            _collection = _context.Users;
        }

        public async Task AddAsync(UserProfile userProfile)
        {
            await _collection.InsertOneAsync(userProfile);
        }

        public async Task UpdateAsync(UserProfile userProfile)
        {
            await _collection.ReplaceOneAsync(
                u => u.Id == userProfile.Id,
                userProfile,
                new ReplaceOptions { IsUpsert = false }
            );
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(u => u.Id == id);
        }

        public async Task<UserProfile?> GetByIdAsync(string id)
        {
            return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<UserProfile?> GetByUsernameAsync(string username)
        {
            return await _collection.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<UserProfile>> ListAllAsync()
        {
            var users = await _collection.Find(_ => true).ToListAsync();
            return users.AsReadOnly();
        }
    }
}
