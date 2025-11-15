using AutoMapper;
using MediatR;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.TodoProducts.Queries.GetTodoProducts;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoProducts.Queries.GetTodoProductsHandler
{
    public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, IReadOnlyCollection<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductsListQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<ProductDto>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetProductsAsync(
                cursorId: request.CursorId,
                pageSize: request.PageSize,
                searchText: request.SearchText,
                sortBy: request.SortBy,
                sortDesc: request.SortDesc
            );
            return _mapper.Map<IReadOnlyCollection<ProductDto>>(products);
        }
    }
}
