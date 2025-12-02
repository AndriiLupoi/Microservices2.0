using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoProducts.Queries.GetTodoProducts;
using Rewiews.Domain.Interfaces;

public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductByNameQueryHandler> _logger;

    public GetProductByNameQueryHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProductByNameQueryHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching product by Name: {Name}", request.Name);
        var product = await _productRepository.GetByNameAsync(request.Name);

        if (product == null)
        {
            _logger.LogWarning("Product with Name {Name} not found", request.Name);
            throw new NotFoundException($"Product with Name '{request.Name}' not found.");
        }

        _logger.LogInformation("Product with Name {Name} retrieved successfully", request.Name);
        return _mapper.Map<ProductDto>(product);
    }
}
