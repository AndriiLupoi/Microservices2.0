using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoProducts.Queries.GetTodoProducts;
using Rewiews.Domain.Interfaces;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProductByIdQueryHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching product by Id: {Id}", request.Id);
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product == null)
        {
            _logger.LogWarning("Product with Id {Id} not found", request.Id);
            throw new NotFoundException($"Product with ID '{request.Id}' not found.");
        }

        _logger.LogInformation("Product with Id {Id} retrieved successfully", request.Id);
        return _mapper.Map<ProductDto>(product);
    }
}
