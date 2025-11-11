using Rewiews.Application.Common.Interfaces;
using Rewiews.Domain.Entities;

namespace Rewiews.Application.TodoProducts.Queries.GetTodoProducts
{
    public class GetProductByIdQuery : IQuery<Product?>
    {
        public string ProductId { get; set; } = string.Empty;
    }
}
