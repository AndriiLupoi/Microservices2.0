using AutoMapper;
using MediatR;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoUserProfile.UserQueries.GetUser;
using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoUserProfile.UserQueries.GetUserHandler
{
    public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, UserProfileDto>
    {
        private readonly IUserProfileRepository _repository;
        private readonly IMapper _mapper;

        public GetUserProfileByIdQueryHandler(IUserProfileRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id);

            if (user == null)
                throw new NotFoundException("UserProfile", request.Id);

            return _mapper.Map<UserProfileDto>(user);
        }
    }
}
