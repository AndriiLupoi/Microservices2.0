using Catalog.Common.DTO;
using Catalog.Common.Pagination;

namespace Catalog.Bll.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductDto productDto);
        Task UpdateProductAsync(int id, ProductDto productDto);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductsByBrandIdAsync(int brandId);

        Task<PagedResultDto<ProductDto>> GetPagedProductsAsync(
            int page = 1,
            int pageSize = 20,
            int? brandId = null,
            int? categoryId = null,
            string? sortBy = null,
            string sortDir = "asc");
    }
}
