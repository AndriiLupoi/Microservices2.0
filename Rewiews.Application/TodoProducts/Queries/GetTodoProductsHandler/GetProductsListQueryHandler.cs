using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.TodoProducts.Queries.GetTodoProducts;
using Rewiews.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, IReadOnlyCollection<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductsListQueryHandler> _logger;

    public GetProductsListQueryHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProductsListQueryHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<ProductDto>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching products list. PageSize: {PageSize}, CursorId: {CursorId}, SearchText: {SearchText}",
            request.PageSize, request.CursorId, request.SearchText);

        var products = await _productRepository.GetProductsAsync(
            cursorId: request.CursorId,
            pageSize: request.PageSize,
            searchText: request.SearchText,
            sortBy: request.SortBy,
            sortDesc: request.SortDesc
        );

        var dtos = _mapper.Map<IReadOnlyCollection<ProductDto>>(products);
        _logger.LogInformation("Fetched {Count} products", dtos.Count);
        return dtos;
    }
}
