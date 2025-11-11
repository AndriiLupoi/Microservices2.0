using Rewiews.Application.Common.Interfaces;
using Rewiews.Domain.Entities;

namespace Rewiews.Application.TodoProducts.Queries.GetTodoProducts
{
    public class GetProductsListQuery : IQuery<IReadOnlyCollection<Product>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchText { get; set; }
    }
}
