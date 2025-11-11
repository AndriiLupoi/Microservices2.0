using FluentValidation;
using Rewiews.Domain.Interfaces;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoProducts.Commands.ProductCommands.UptadeTodo
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator(IProductRepository productRepository)
        {
            // Перевірка ProductId
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.")
                .Must(IsValidObjectIdFormat).WithMessage("Invalid ProductId format.")
                .MustAsync(async (id, cancellation) =>
                {
                    var exists = await productRepository.ExistsAsync(id, cancellation);
                    return exists;
                }).WithMessage("Product with the given id does not exist.");

            // Name — якщо вказано, мінімум 3 символи
            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name)
                    .MinimumLength(3).WithMessage("Name must be at least 3 characters.");
            });

            // Description — якщо вказано, максимум 500 символів
            When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
            {
                RuleFor(x => x.Description)
                    .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
            });

            // Price — якщо вказано, має бути > 0
            When(x => x.Price.HasValue, () =>
            {
                RuleFor(x => x.Price.Value)
                    .GreaterThan(0).WithMessage("Price must be greater than 0.");
            });

            // Currency — якщо вказано, має бути один з дозволених
            When(x => !string.IsNullOrWhiteSpace(x.Currency), () =>
            {
                RuleFor(x => x.Currency)
                    .Must(c => c is "USD" or "EUR" or "UAH")
                    .WithMessage("Currency must be USD, EUR, or UAH.");
            });
        }

        private static bool IsValidObjectIdFormat(string id)
        {
            return !string.IsNullOrEmpty(id) && Regex.IsMatch(id, "^[a-fA-F0-9]{24}$");
        }
    }
}
