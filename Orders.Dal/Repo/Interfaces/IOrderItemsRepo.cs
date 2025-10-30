using Orders.Domain.Entity;

namespace Orders.Dal.Repo.Interfaces
{
    public interface IOrderItemsRepo
    {
        Task<IEnumerable<OrderItems>> GetAllAsync();
        Task<OrderItems?> GetByIdAsync(int id);
        Task<int> AddAsync(OrderItems item);
        Task<int> UpdateAsync(OrderItems item);
        Task<int> DeleteAsync(int id);
    }
}
