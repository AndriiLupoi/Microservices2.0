using FluentValidation;
using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.CreateUser
{
    public class CreateUserProfileCommandValidator
        : AbstractValidator<CreateUserProfileCommand>
    {
        public CreateUserProfileCommandValidator(IUserProfileRepository repository)
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                .MustAsync(async (username, cancellation) =>
                    !await repository.ExistsByUsernameAsync(username, cancellation))
                .WithMessage("User with the same username already exists.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(async (email, cancellation) =>
                    !await repository.ExistsByEmailAsync(email, cancellation))
                .WithMessage("User with the same email already exists.");
        }
    }
}
