using MediatR;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.DeleteTodo
{
    public class DeleteProductCommand : IRequest<string>
    {
        public string ProductId { get; set; } = string.Empty;
    }
}

