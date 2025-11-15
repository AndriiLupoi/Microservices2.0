using MediatR;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.DeleteUser
{
    public class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, string>
    {
        private readonly IUserProfileRepository _userRepository;

        public DeleteUserProfileCommandHandler(IUserProfileRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new NotFoundException("UserProfile", request.Id);

            await _userRepository.DeleteAsync(request.Id);

            return $"UserProfile '{request.Id}' deleted successfully.";
        }
    }
}
