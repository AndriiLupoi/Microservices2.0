using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoUserProfile.UserQueries.GetUser;
using Rewiews.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, UserProfileDto>
{
    private readonly IUserProfileRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserProfileByIdQueryHandler> _logger;

    public GetUserProfileByIdQueryHandler(IUserProfileRepository repository, IMapper mapper, ILogger<GetUserProfileByIdQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserProfileDto> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching user profile Id: {Id}", request.Id);

        var user = await _repository.GetByIdAsync(request.Id);
        if (user == null)
        {
            _logger.LogWarning("UserProfile Id: {Id} not found", request.Id);
            throw new NotFoundException("UserProfile", request.Id);
        }

        _logger.LogInformation("UserProfile Id: {Id} retrieved successfully", request.Id);
        return _mapper.Map<UserProfileDto>(user);
    }
}
