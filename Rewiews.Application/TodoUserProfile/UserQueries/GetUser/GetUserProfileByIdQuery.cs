using Rewiews.Application.Common.DTOs;
using Rewiews.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoUserProfile.UserQueries.GetUser
{
    public class GetUserProfileByIdQuery : IQuery<UserProfileDto>
    {
        public string Id { get; set; } = null!;

        public GetUserProfileByIdQuery(string id)
        {
            Id = id;
        }
    }
}
