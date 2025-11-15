using FluentValidation;
using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.DeleteUser
{
    public class DeleteUserProfileCommandValidator : AbstractValidator<DeleteUserProfileCommand>
    {
        public DeleteUserProfileCommandValidator(IUserProfileRepository userRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("UserProfileId is required.")

                .Must(IsValidObjectIdFormat)
                .WithMessage("Invalid UserProfileId format.")

                .MustAsync(async (id, cancellation) =>
                {
                    var exists = await userRepository.ExistsAsync(id, cancellation);
                    return exists;
                })
                .WithMessage("UserProfile with the given Id does not exist.");
        }

        private static bool IsValidObjectIdFormat(string id)
        {
            return !string.IsNullOrEmpty(id) && Regex.IsMatch(id, "^[a-fA-F0-9]{24}$");
        }
    }
}
