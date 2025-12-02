using Common.DTO_s;
using Orders.Domain.Entity;

namespace Orders.Bll.Interfaces
{
    public interface IOrderItemsService
    {
        Task<IEnumerable<OrderItemsDTO>> GetAllAsync();
        Task<OrderItemsDTO?> GetByIdAsync(int id);
        Task AddAsync(OrderItemsDTO dto);
        Task UpdateAsync(OrderItemsDTO dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<OrderItemsDTO>> GetByOrderIdAsync(int orderId);
    }
}
