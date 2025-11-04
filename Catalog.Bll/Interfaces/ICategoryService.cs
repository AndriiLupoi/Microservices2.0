using Catalog.Common.DTO;
using Catalog.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Bll.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategorysAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(CategoryDto categoryDto);
        Task UpdateCategoryAsync(CategoryDto categoryDto);
        Task DeleteCategoryAsync(int id);

        Task<PagedResultDto<CategoryDto>> GetPagedCategoriesAsync(
            int page = 1,
            int pageSize = 20,
            string? sortBy = null,
            string sortDir = "asc"
        );
    }
}
