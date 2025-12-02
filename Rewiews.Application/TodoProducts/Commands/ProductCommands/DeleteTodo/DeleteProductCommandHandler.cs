using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.DeleteTodo;
using Rewiews.Domain.Interfaces;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, string>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(IProductRepository productRepository, ILogger<DeleteProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<string> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting product with Id: {Id}", request.Id);
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product == null)
        {
            _logger.LogWarning("Product with Id {Id} not found", request.Id);
            throw new NotFoundException("Product", request.Id);
        }

        await _productRepository.DeleteAsync(request.Id);
        _logger.LogInformation("Product with Id {Id} deleted successfully", request.Id);

        return $"Product '{request.Id}' deleted successfully.";
    }
}
