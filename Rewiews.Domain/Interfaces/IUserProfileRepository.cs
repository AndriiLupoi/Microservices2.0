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
    }
}
