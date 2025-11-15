using Rewiews.Application.Common.DTOs;
using Rewiews.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoUserProfile.UserQueries.GetUser
{
    public class GetUserProfilesListQuery : IQuery<IReadOnlyCollection<UserProfileDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchText { get; set; }
        public string? CursorId { get; set; }
        public string? SortBy { get; set; }
        public bool SortDesc { get; set; } = false;
    }
}
