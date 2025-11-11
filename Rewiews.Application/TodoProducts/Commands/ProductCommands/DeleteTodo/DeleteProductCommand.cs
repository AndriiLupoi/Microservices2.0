using Rewiews.Application.Common.Interfaces;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.DeleteTodo
{
    public class DeleteProductCommand : ICommand
    {
        public string ProductId { get; set; } = string.Empty;
    }
}

