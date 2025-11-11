using Rewiews.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rewiews.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task AddReviewAsync(Product product, Review review);

        // Пошук відгуків по користувачу
        Task<IReadOnlyCollection<Review>> GetReviewsByUserAsync(string userId);

        // Текстовий пошук коментарів
        Task<IReadOnlyCollection<Review>> SearchReviewsByTextAsync(string searchText);

        // Підрахунок середнього рейтингу
        Task<double> GetAverageRatingAsync(Product product);
    }
}
