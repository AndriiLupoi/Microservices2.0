using MediatR;
using Rewiews.Domain.Interfaces;
using Rewiews.Domain.ValueObjects;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.UptadeTodo
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null) return Unit.Value;

            if (request.Name != null) product.UpdateName(request.Name);
            if (request.Description != null) product.UpdateDescription(request.Description);
            if (request.Price.HasValue && request.Currency != null)
                product.UpdatePrice(new Money(request.Price.Value, request.Currency));

            await _productRepository.UpdateAsync(product);
            return Unit.Value;
        }
    }
}
