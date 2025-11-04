using Catalog.Common.DTO;
using Catalog.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Bll.Interfaces
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<ProductCategoryDto>> GetAllProductsCategoryAsync();
        Task<ProductCategoryDto> GetProductCategoryByIdAsync(int id);
        Task AddProductCategoryAsync(ProductCategoryDto productCategoryDto);
        Task UpdateProductCategoryAsync(ProductCategoryDto productCategoryDto);
        Task DeleteProductCategoryAsync(int id);

        Task<PagedResultDto<ProductCategoryDto>> GetPagedProductsCategoryAsync(
            int page = 1,
            int pageSize = 20,
            int? productId = null,
            int? categoryId = null,
            string? sortBy = null,
            string sortDir = "asc"
        );

    }
}
