using Common.DTO_s;

namespace Orders.Bll.Interfaces
{
    public interface ICustomersService
    {
        Task<IEnumerable<CustomersDTO>> GetAllAsync();
        Task<CustomersDTO?> GetByIdAsync(int id);
        Task AddAsync(CustomersDTO dto);
        Task UpdateAsync(CustomersDTO dto);
        Task DeleteAsync(int id);
    }
}
