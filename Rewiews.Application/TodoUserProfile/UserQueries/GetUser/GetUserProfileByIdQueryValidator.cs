using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoUserProfile.UserQueries.GetUser
{
    public class GetUserProfileByIdQueryValidator : AbstractValidator<GetUserProfileByIdQuery>
    {
        public GetUserProfileByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("UserProfileId is required.")
                .Must(IsValidObjectIdFormat).WithMessage("Invalid UserProfileId format.");
        }

        private static bool IsValidObjectIdFormat(string id)
        {
            return !string.IsNullOrEmpty(id) && Regex.IsMatch(id, "^[a-fA-F0-9]{24}$");
        }
    }
}
