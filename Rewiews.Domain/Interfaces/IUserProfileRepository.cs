using Rewiews.Domain.Entities;

namespace Rewiews.Domain.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByIdAsync(string id);
        Task<UserProfile?> GetByUsernameAsync(string username);
        Task<IReadOnlyCollection<UserProfile>> ListAllAsync();
        Task AddAsync(UserProfile userProfile);
        Task UpdateAsync(UserProfile userProfile);
        Task DeleteAsync(string id);
        Task<bool> ExistsByUsernameAsync(string username, CancellationToken token);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken token);
        Task<bool> ExistsAsync(string id, CancellationToken cancellation);
    }
}
