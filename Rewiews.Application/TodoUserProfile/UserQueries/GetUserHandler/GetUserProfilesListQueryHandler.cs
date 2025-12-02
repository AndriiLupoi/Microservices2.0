using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.TodoUserProfile.UserQueries.GetUser;
using Rewiews.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GetUserProfilesListQueryHandler : IRequestHandler<GetUserProfilesListQuery, IReadOnlyCollection<UserProfileDto>>
{
    private readonly IUserProfileRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserProfilesListQueryHandler> _logger;

    public GetUserProfilesListQueryHandler(IUserProfileRepository repository, IMapper mapper, ILogger<GetUserProfilesListQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<UserProfileDto>> Handle(GetUserProfilesListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all user profiles");

        var users = await _repository.ListAllAsync();
        var dtos = _mapper.Map<IReadOnlyCollection<UserProfileDto>>(users);

        _logger.LogInformation("Fetched {Count} user profiles", dtos.Count);
        return dtos;
    }
}
