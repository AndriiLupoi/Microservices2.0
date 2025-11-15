using FluentValidation;
using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoProducts.Queries.GetTodoProducts
{
    public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator(IProductRepository productRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ProductId is required.")

                .Must(IsValidObjectIdFormat).WithMessage("Invalid ProductId format.");
        }


        private static bool IsValidObjectIdFormat(string id)
        {
            return !string.IsNullOrEmpty(id) && Regex.IsMatch(id, "^[a-fA-F0-9]{24}$");
        }
    }
}
