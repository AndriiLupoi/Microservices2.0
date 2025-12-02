using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.DeleteUser;
using Rewiews.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, string>
{
    private readonly IUserProfileRepository _userRepository;
    private readonly ILogger<DeleteUserProfileCommandHandler> _logger;

    public DeleteUserProfileCommandHandler(IUserProfileRepository userRepository, ILogger<DeleteUserProfileCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<string> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting user profile Id: {Id}", request.Id);

        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            _logger.LogWarning("UserProfile Id: {Id} not found", request.Id);
            throw new NotFoundException("UserProfile", request.Id);
        }

        await _userRepository.DeleteAsync(request.Id);
        _logger.LogInformation("UserProfile Id: {Id} deleted successfully", request.Id);

        return $"UserProfile '{request.Id}' deleted successfully.";
    }
}
