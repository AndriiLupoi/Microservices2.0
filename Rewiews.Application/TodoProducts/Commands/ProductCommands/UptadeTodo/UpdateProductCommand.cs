using MediatR;
using Rewiews.Application.Common.Interfaces;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.UptadeTodo
{
    public class UpdateProductCommand : IRequest<string>
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public int? Version { get; set; }
    }
}
