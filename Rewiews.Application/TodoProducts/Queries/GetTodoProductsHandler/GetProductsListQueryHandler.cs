using MediatR;
using Rewiews.Application.TodoProducts.Queries.GetTodoProducts;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;

namespace Rewiews.Application.TodoProducts.Queries.GetTodoProductsHandler
{
    public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, IReadOnlyCollection<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsListQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IReadOnlyCollection<Product>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var allProducts = string.IsNullOrWhiteSpace(request.SearchText)
                ? await _productRepository.ListAllAsync()
                : await _productRepository.SearchByTextAsync(request.SearchText);

            // Пагінація
            var pagedProducts = allProducts
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList()
                .AsReadOnly();

            return pagedProducts;
        }
    }
}
