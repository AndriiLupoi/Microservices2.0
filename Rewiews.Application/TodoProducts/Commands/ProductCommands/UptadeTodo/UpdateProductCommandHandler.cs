using AutoMapper;
using MediatR;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Domain.Interfaces;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.UptadeTodo
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, string>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
                throw new NotFoundException("Product", request.Id);

            _mapper.Map(request, product);

            await _productRepository.UpdateAsync(product);

            return $"Product '{product.Id}' updated successfully.";
        }
    }
}
