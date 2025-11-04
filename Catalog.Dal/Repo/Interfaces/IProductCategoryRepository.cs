using Catalog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Dal.Repo.Interfaces
{
    public interface IProductCategoryRepository : IAsyncRepository<ProductCategory> 
    {
        IQueryable<ProductCategory> Query();
    }
}
