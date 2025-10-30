using Common.DTO_s;

namespace Orders.Bll.Interfaces
{
    public interface IOrdersService
    {
        Task<IEnumerable<OrdersDTO>> GetAllAsync();
        Task<OrdersDTO?> GetByIdAsync(int id);
        Task AddAsync(OrdersDTO dto);
        Task UpdateAsync(OrdersDTO dto);
        Task DeleteAsync(int id);
    }
}
