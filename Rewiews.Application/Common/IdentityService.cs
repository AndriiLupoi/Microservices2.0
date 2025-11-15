using Rewiews.Application.Common.Interfaces;
using Rewiews.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.Common
{
    public class IdentityService : IIdentityService
    {
        public Task<string?> GetUserNameAsync(string userId)
        {
            return Task.FromResult<string?>("TestUser");
        }

        public Task<List<string>> GetRolesAsync(string userId)
        {
            return Task.FromResult(new List<string> { "User" });
        }

        public Task<bool> IsInRoleAsync(string userId, string role)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            throw new NotImplementedException();
        }

        public Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
