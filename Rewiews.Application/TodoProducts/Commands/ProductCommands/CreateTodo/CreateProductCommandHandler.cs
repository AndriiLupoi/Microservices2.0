using AutoMapper;
using MediatR;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.CreateTodo;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, string>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request);

        await _productRepository.AddAsync(product);

        return product.Id!;
    }
}
