using AutoMapper;
using MediatR;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.TodoUserProfile.UserQueries.GetUser;
using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoUserProfile.UserQueries.GetUserHandler
{
    public class GetUserProfilesListQueryHandler : IRequestHandler<GetUserProfilesListQuery, IReadOnlyCollection<UserProfileDto>>
    {
        private readonly IUserProfileRepository _repository;
        private readonly IMapper _mapper;

        public GetUserProfilesListQueryHandler(IUserProfileRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<UserProfileDto>> Handle(GetUserProfilesListQuery request, CancellationToken cancellationToken)
        {
            var users = await _repository.ListAllAsync();
            return _mapper.Map<IReadOnlyCollection<UserProfileDto>>(users);
        }
    }
}
