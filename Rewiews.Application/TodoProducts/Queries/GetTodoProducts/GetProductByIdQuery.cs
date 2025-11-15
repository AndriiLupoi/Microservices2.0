using Rewiews.Application.Common.DTOs;
using Rewiews.Application.Common.Interfaces;
using Rewiews.Domain.Entities;

namespace Rewiews.Application.TodoProducts.Queries.GetTodoProducts
{
    public class GetProductByIdQuery : IQuery<ProductDto>
    {
        public string Id { get; set; } = null!;

        public GetProductByIdQuery(string productId)
        {
            Id = productId;
        }
    }
}
