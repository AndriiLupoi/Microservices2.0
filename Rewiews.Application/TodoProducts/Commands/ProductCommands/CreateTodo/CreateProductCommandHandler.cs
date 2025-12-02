using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.CreateTodo;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, string>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<CreateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating product: {Name}", request.Name);
        var product = _mapper.Map<Product>(request);

        await _productRepository.AddAsync(product);

        _logger.LogInformation("Product created successfully with Id: {Id}", product.Id);
        return product.Id!;
    }
}
