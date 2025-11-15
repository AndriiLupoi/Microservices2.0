using Rewiews.Application.Common.DTOs;
using Rewiews.Application.Common.Interfaces;

namespace Rewiews.Application.TodoProducts.Queries.GetTodoProducts
{
    public class GetProductsListQuery : IQuery<IReadOnlyCollection<ProductDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchText { get; set; }

        public string? CursorId { get; set; }
        public string? SortBy { get; set; }        // Поле для сортування
        public bool SortDesc { get; set; } = false; // Напрям сортування
    }
}
