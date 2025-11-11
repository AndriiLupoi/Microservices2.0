using MediatR;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using Rewiews.Domain.ValueObjects;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.CreateTodo
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, string>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                price = new Money(request.Price, request.Currency)
            };

            await _productRepository.AddAsync(product);
            return product.Id;
        }
    }
}