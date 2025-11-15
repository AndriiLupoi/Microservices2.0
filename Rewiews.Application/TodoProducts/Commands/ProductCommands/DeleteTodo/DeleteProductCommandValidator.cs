using FluentValidation;
using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.DeleteTodo
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator(IProductRepository productRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ProductId is required.")

                .Must(IsValidObjectIdFormat).WithMessage("Invalid ProductId format.")

                .MustAsync(async (id, cancellation) =>
                {
                    var exists = await productRepository.ExistsAsync(id, cancellation);
                    return exists;
                }).WithMessage("Product with the given id does not exist.");
        }

        // Простий метод для перевірки формату ObjectId без Mongo пакету
        private static bool IsValidObjectIdFormat(string id)
        {
            return !string.IsNullOrEmpty(id) && Regex.IsMatch(id, "^[a-fA-F0-9]{24}$");
        }
    }
}
