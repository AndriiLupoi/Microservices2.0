using FluentValidation;
using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoReviews.Commands.ReviewsCommands.UptadeReviews
{
    public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewCommandValidator(IReviewRepository reviewRepository)
        {
            // Перевірка Id
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ReviewId is required.")
                .Must(IsValidObjectIdFormat).WithMessage("Invalid ReviewId format.")
                .MustAsync(async (id, cancellation) =>
                {
                    var exists = await reviewRepository.ExistsAsync(id, cancellation);
                    return exists;
                }).WithMessage("Review with the given Id does not exist.");

            // Rating — якщо вказано, 1-5
            When(x => x.Rating.HasValue, () =>
            {
                RuleFor(x => x.Rating.Value)
                    .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
            });

            // Comment — якщо вказано, максимум 500 символів
            When(x => !string.IsNullOrWhiteSpace(x.Comment), () =>
            {
                RuleFor(x => x.Comment)
                    .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.");
            });
        }

        private static bool IsValidObjectIdFormat(string id)
        {
            return !string.IsNullOrEmpty(id) && Regex.IsMatch(id, "^[a-fA-F0-9]{24}$");
        }
    }
}
