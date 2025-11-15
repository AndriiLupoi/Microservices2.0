using MongoDB.Driver;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using Rewiews.Infrastructure.Context;

namespace Rewiews.Infrastructure.Repositories
{
    public class ReviewRepository : MongoRepository<Review>, IReviewRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<Review> _collection;
        private readonly IMongoCollection<Product> _productCollection;

        public ReviewRepository(MongoDbContext context)
            : base(context.Reviews)
        {
            _context = context;
            _collection = _context.Reviews;
            _productCollection = _context.Products;
        }

        // Додаємо відгук у продукт (embedded document)
        public async Task AddReviewAsync(Product product, Review review)
        {
            product.AddReview(review);
            product.UpdatedAt = DateTime.UtcNow;

            await _productCollection.ReplaceOneAsync(
                p => p.Id == product.Id,
                product,
                new ReplaceOptions { IsUpsert = false }
            );
        }

        // Середній рейтинг продукту
        public async Task<double> GetAverageRatingAsync(Product product)
        {
            if (product.Reviews.Count == 0)
                return 0.0;

            return product.Reviews.Average(r => r.Rating);
        }

        // Всі відгуки конкретного користувача
        public async Task<IReadOnlyCollection<Review>> GetReviewsByUserAsync(string userId)
        {
            var filter = Builders<Product>.Filter.ElemMatch(p => p.Reviews, r => r.UserId == userId);
            var products = await _productCollection.Find(filter).ToListAsync();

            var reviews = products.SelectMany(p => p.Reviews.Where(r => r.UserId == userId)).ToList();
            return reviews.AsReadOnly();
        }

        // Текстовий пошук по відгуках
        public async Task<IReadOnlyCollection<Review>> SearchReviewsByTextAsync(string searchText)
        {
            var filter = Builders<Product>.Filter.Text(searchText);
            var products = await _productCollection.Find(filter).ToListAsync();

            var reviews = products.SelectMany(p => p.Reviews).ToList();
            return reviews.AsReadOnly();
        }


        public async Task<IReadOnlyCollection<Review>> ListByProductAsync(string productId)
        {
            var filter = Builders<Review>.Filter.Eq(r => r.ProductId, productId);
            var reviews = await _collection.Find(filter).ToListAsync();
            return reviews.AsReadOnly();
        }

    }
}
