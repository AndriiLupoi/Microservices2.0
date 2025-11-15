using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.Interfaces;

namespace Rewiews.Application.Common.Behaviors;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public LoggingBehaviour(ILogger<TRequest> logger, IUser user, IIdentityService identityService)
    {
        _logger = logger;
        _user = user;
        _identityService = identityService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _user.Id ?? string.Empty;
        var userName = string.Empty;
        var roles = _user.Role ?? new List<string>();

        if (!string.IsNullOrEmpty(userId))
        {
            userName = await _identityService.GetUserNameAsync(userId);
        }

        _logger.LogInformation("Rewiews Request: {Name} {@UserId} {@UserName} {@Request} {@Roles}",
            requestName, userId, userName, request, roles);
    }
}
