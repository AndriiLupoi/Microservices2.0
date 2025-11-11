using MediatR;
using Rewiews.Application.TodoProducts.Queries.GetTodoProducts;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;

namespace Rewiews.Application.TodoProducts.Queries.GetTodoProductsWithPagination
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetByIdAsync(request.ProductId);
        }
    }
}
