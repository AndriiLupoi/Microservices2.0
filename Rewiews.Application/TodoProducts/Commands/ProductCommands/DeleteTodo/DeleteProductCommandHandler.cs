using MediatR;
using Rewiews.Domain.Interfaces;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.DeleteTodo
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await _productRepository.DeleteAsync(request.ProductId);
            return Unit.Value;
        }
    }
}