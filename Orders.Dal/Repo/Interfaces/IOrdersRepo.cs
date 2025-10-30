using Orders.Domain.Entity;
using System.Collections.Generic;

namespace Orders.Dal.Repo.Interfaces
{
    public interface IOrdersRepo
    {
        Task<Order?> GetByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<int> AddAsync(Order order);
        Task<bool> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int orderId);
    }
}
