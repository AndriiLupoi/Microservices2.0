using Catalog.Dal.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Dal.Repo.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IBrandRepository Brands { get; }
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IProductCategoryRepository ProductCategories { get; }

        Task<int> SaveChangesAsync();
    }
}