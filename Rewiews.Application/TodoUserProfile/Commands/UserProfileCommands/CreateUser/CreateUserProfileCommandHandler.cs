using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.CreateUser;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using Rewiews.Domain.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

public class CreateUserProfileCommandHandler
    : IRequestHandler<CreateUserProfileCommand, string>
{
    private readonly IUserProfileRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateUserProfileCommandHandler> _logger;

    public CreateUserProfileCommandHandler(
        IUserProfileRepository userRepository,
        IMapper mapper,
        ILogger<CreateUserProfileCommandHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating user profile for Email: {Email}", request.Email);

        var userEmail = new Email(request.Email);
        var user = new UserProfile
        {
            Username = request.Username,
            email = userEmail
        };

        await _userRepository.AddAsync(user);

        _logger.LogInformation("UserProfile created successfully with Id: {Id}", user.Id);
        return user.Id!;
    }
}
