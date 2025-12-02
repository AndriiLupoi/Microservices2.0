using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.UptadeTodo;
using Rewiews.Domain.Interfaces;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, string>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper, ILogger<UpdateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating product with Id: {Id}", request.Id);
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product == null)
        {
            _logger.LogWarning("Product with Id {Id} not found", request.Id);
            throw new NotFoundException("Product", request.Id);
        }

        _mapper.Map(request, product);
        await _productRepository.UpdateAsync(product);

        _logger.LogInformation("Product with Id {Id} updated successfully", product.Id);
        return $"Product '{product.Id}' updated successfully.";
    }
}
