using AutoMapper;
using MediatR;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using Rewiews.Domain.ValueObjects;

namespace Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.CreateUser
{
    public class CreateUserProfileCommandHandler
    : IRequestHandler<CreateUserProfileCommand, string>
    {
        private readonly IUserProfileRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserProfileCommandHandler(
            IUserProfileRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userEmail = new Email(request.Email);

            var user = new UserProfile
            {
                Username = request.Username,
                email = userEmail
            };

            await _userRepository.AddAsync(user);

            return user.Id!;
        }
    }
}
