using Rewiews.Domain.ValueObjects;
using MediatR;
using Rewiews.Application.Common.Interfaces;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.CreateTodo
{
    public class CreateProductCommand : ICommand<string>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "USD";
    }
}
