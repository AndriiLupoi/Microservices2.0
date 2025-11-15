using AutoMapper;
using MediatR;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.UptadeUser
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, string>
    {
        private readonly IUserProfileRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserProfileCommandHandler(IUserProfileRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new NotFoundException("UserProfile", request.Id);

            _mapper.Map(request, user);

            await _userRepository.UpdateAsync(user);

            return $"UserProfile '{user.Id}' updated successfully.";
        }
    }
}
