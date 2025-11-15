using MongoDB.Driver;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using Rewiews.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Rewiews.Infrastructure.Repositories
{
    public class UserProfileRepository : MongoRepository<UserProfile>, IUserProfileRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<UserProfile> _collection;

        public UserProfileRepository(MongoDbContext context)
            : base(context.Users)
        {
            _context = context;
            _collection = _context.Users;
        }

        public async Task<UserProfile?> GetByUsernameAsync(string username)
        {
            return await _collection.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken token = default)
        {
            var filter = Builders<UserProfile>.Filter.Eq(p => p.Username, username);
            var count = await _collection.CountDocumentsAsync(filter, cancellationToken: token);
            return count > 0;
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken token = default)
        {
            var filter = Builders<UserProfile>.Filter.Eq(p => p.email.Value, email);
            var count = await _collection.CountDocumentsAsync(filter, cancellationToken: token);
            return count > 0;
        }
    }
}
