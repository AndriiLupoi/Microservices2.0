using System;
using System.Data;
using System.Threading.Tasks;

namespace Orders.Dal.Repo.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IOrdersRepo Orders { get; }
        IOrderItemsRepo OrderItems { get; }
        ICustomersRepo Customers { get; }
        IProductsRepo Products { get; }

        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task CommitAsync();
        Task RollbackAsync();
    }
}
