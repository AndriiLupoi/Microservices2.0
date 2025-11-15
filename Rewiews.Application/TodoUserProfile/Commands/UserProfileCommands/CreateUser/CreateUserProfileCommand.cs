using Rewiews.Application.Common.Interfaces;

namespace Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.CreateUser
{
    public class CreateUserProfileCommand : ICommand<string>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
