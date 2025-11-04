using Catalog.Common.DTO;
using Catalog.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Bll.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
        Task<BrandDto> GetBrandByIdAsync(int id);
        Task AddBrandAsync(BrandDto brandDto);
        Task UpdateBrandAsync(BrandDto brandDto);
        Task DeleteBrandAsync(int id);

        Task<PagedResultDto<BrandDto>> GetPagedBrandsAsync(
            int page = 1,
            int pageSize = 20,
            string? sortBy = null,
            string sortDir = "asc"
        );
    }
}
