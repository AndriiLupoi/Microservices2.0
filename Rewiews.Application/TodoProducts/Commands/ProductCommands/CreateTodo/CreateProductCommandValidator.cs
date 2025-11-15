using FluentValidation;
using Rewiews.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.CreateTodo
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator(IProductRepository productRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MustAsync(async (name, cancellation) =>
                    !await productRepository.ExistsByNameAsync(name, cancellation))
                .WithMessage("Product with the same name already exists.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .Must(c => c is "USD" or "EUR" or "UAH")
                .WithMessage("Currency must be USD, EUR, or UAH.");
        }
    }
}
