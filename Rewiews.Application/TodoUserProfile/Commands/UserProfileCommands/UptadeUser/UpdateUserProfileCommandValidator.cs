using FluentValidation;
using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.UptadeUser
{
    public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileCommandValidator(IUserProfileRepository userRepository)
        {
            // Перевірка Id
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("UserProfileId is required.")
                .Must(IsValidObjectIdFormat).WithMessage("Invalid UserProfileId format.")
                .MustAsync(async (id, cancellation) =>
                {
                    var exists = await userRepository.ExistsAsync(id, cancellation);
                    return exists;
                }).WithMessage("UserProfile with the given Id does not exist.");

            // Username — якщо вказано, мінімум 3 символи
            When(x => !string.IsNullOrWhiteSpace(x.Username), () =>
            {
                RuleFor(x => x.Username)
                    .MinimumLength(3).WithMessage("Username must be at least 3 characters.");
            });

            // Email — якщо вказано, має бути валідним
            When(x => x.Email is not null, () =>
            {
                RuleFor(x => x.Email!.Value)
                    .NotEmpty().WithMessage("Email cannot be empty.")
                    .EmailAddress().WithMessage("Invalid email format.");
            });
        }

        private static bool IsValidObjectIdFormat(string id)
        {
            return !string.IsNullOrEmpty(id) && Regex.IsMatch(id, "^[a-fA-F0-9]{24}$");
        }
    }
}
