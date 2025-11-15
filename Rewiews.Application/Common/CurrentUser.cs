using Microsoft.AspNetCore.Http;
using Rewiews.Application.Common.Interfaces;

namespace Rewiews.Application.Common
{
    public class CurrentUser : IUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
        public string? Username => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        public List<string>? Role
        {
            get
            {
                var roles = _httpContextAccessor.HttpContext?.User?.FindAll("role");
                if (roles == null) return null;
                return roles.Select(r => r.Value).ToList();
            }
        }
    }
}
