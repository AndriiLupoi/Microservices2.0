using Rewiews.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rewiews.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task AddReviewAsync(Product product, Review review);
        Task<Review?> GetByIdAsync(string id);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(string id);

        // Пошук відгуків по користувачу
        Task<IReadOnlyCollection<Review>> GetReviewsByUserAsync(string userId);

        // Текстовий пошук коментарів
        Task<IReadOnlyCollection<Review>> SearchReviewsByTextAsync(string searchText);

        // Підрахунок середнього рейтингу
        Task<double> GetAverageRatingAsync(Product product);
        Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<Review>> ListByProductAsync(string productId);
    }
}
