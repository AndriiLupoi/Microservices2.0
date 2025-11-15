using MediatR;
using Rewiews.Application.Common.Exceptions;
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
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
                throw new NotFoundException("Product", request.Id);

            await _productRepository.DeleteAsync(request.Id);

            return $"Product '{request.Id}' deleted successfully.";
        }
    }
}
