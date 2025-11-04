using Catalog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Dal.Repo.Interfaces
{
    public interface ICategoryRepository : IAsyncRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();
        Task<IEnumerable<Product>> GetProductsForCategoryAsync(int categoryId);

        IQueryable<Category> Query();

    }
}
