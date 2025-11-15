using FluentValidation;
using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoReviews.Commands.ReviewsCommands.DeleteReviews
{
    public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
    {
        public DeleteReviewCommandValidator(IReviewRepository reviewRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ReviewId is required.")

                .Must(IsValidObjectIdFormat)
                .WithMessage("Invalid ReviewId format.")

                .MustAsync(async (id, cancellation) =>
                {
                    var exists = await reviewRepository.ExistsAsync(id, cancellation);
                    return exists;
                })
                .WithMessage("Review with the given Id does not exist.");
        }

        private static bool IsValidObjectIdFormat(string id)
        {
            return !string.IsNullOrEmpty(id) && Regex.IsMatch(id, "^[a-fA-F0-9]{24}$");
        }
    }
}
