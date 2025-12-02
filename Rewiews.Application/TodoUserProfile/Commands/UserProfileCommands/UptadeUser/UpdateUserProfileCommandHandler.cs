using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.UptadeUser;
using Rewiews.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, string>
{
    private readonly IUserProfileRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

    public UpdateUserProfileCommandHandler(IUserProfileRepository userRepository, IMapper mapper, ILogger<UpdateUserProfileCommandHandler> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating user profile Id: {Id}", request.Id);

        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            _logger.LogWarning("UserProfile Id: {Id} not found", request.Id);
            throw new NotFoundException("UserProfile", request.Id);
        }

        _mapper.Map(request, user);
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation("UserProfile Id: {Id} updated successfully", user.Id);
        return $"UserProfile '{user.Id}' updated successfully.";
    }
}
