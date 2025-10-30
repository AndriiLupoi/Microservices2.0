using Common.DTO_s;

namespace Orders.Bll.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductsDTO>> GetAllAsync();
        Task<ProductsDTO?> GetByIdAsync(int id);
        Task AddAsync(ProductsDTO dto);
        Task UpdateAsync(ProductsDTO dto);
        Task DeleteAsync(int id);
    }
}
