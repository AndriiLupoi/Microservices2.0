using FluentValidation;
using Orders.Bll.Interfaces;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{

    public ProductDtoValidator()
    {
        // 🔹 Ціна: позитивна, не може бути занадто низькою або надто високою
        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Ціна повинна бути більшою за 0")
            .LessThan(100000).WithMessage("Ціна не може перевищувати 100000");


        // Приклад складної бізнес-правила:
        RuleFor(p => p.SKU)
            .Must(sku => !sku.Contains("INVALID"))
            .WithMessage("SKU не може містити слово INVALID");
    }
}
