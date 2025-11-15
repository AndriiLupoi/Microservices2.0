using MediatR;
using Rewiews.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.UptadeUser
{
    public class UpdateUserProfileCommand : IRequest<string>
    {
        public string Id { get; set; } = string.Empty;
        public string? Username { get; set; }
        public Email? Email { get; set; }
        public int? Version { get; set; }
    }
}
