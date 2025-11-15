using MediatR;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.DeleteTodo
{
    public class DeleteProductCommand : IRequest<string>
    {
        public string Id { get; set; } = string.Empty;
    }
}

