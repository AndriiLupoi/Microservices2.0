using MediatR;
using Rewiews.Domain.Interfaces;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.DeleteTodo
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, string>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<string> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                return $"Product with id {request.ProductId} not found";

            await _productRepository.DeleteAsync(request.ProductId);

            return $"Product {request.ProductId} deleted successfully";
        }
    }
}