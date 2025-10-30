using Common.DTO_s;

namespace Orders.Bll.Interfaces
{
    public interface IOrderItemsService
    {
        Task<IEnumerable<OrderItemsDTO>> GetAllAsync();
        Task<OrderItemsDTO?> GetByIdAsync(int id);
        Task AddAsync(OrderItemsDTO dto);
        Task UpdateAsync(OrderItemsDTO dto);
        Task DeleteAsync(int id);
    }
}
