using Catalog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Dal.Repo.Interfaces
{
    public interface IBrandRepository : IAsyncRepository<Brand>
    {
        Task<Brand> GetBrandWithProductsExplicitAsync(int brandId);

        IQueryable<Brand> Query();
    }
}
