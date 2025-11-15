using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.DeleteUser
{
    public class DeleteUserProfileCommand : IRequest<string>
    {
        public DeleteUserProfileCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; } = string.Empty;
    }
}
