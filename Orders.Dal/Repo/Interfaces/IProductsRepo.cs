using Orders.Domain.Entity;

namespace Orders.Dal.Repo.Interfaces
{
    public interface IProductsRepo
    {
        Task<IEnumerable<Products>> GetAllAsync();
        Task<Products?> GetByIdAsync(int id);
        Task<int> AddAsync(Products product);
        Task<bool> UpdateAsync(Products product);
        Task<bool> DeleteAsync(int id);
    }
}
