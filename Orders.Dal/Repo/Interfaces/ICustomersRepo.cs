using Orders.Domain.Entity;

namespace Orders.Dal.Repo.Interfaces
{
    public interface ICustomersRepo
    {
        Task<IEnumerable<Customers>> GetAllAsync();
        Task<Customers?> GetByIdAsync(int id);
        Task<int> AddAsync(Customers customer);
        Task<bool> UpdateAsync(Customers customer);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByEmailAsync(string email);
    }
}
