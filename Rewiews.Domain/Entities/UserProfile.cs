using Rewiews.Domain.Common;
using Rewiews.Domain.ValueObjects;

namespace Rewiews.Domain.Entities;
public class UserProfile : BaseEntity
{
    public string Username { get; set; }
    public Email email { get; set; }

    public void UpdateUsername(string username)
    {
        Username = username;
        Touch();
    }

    public void UpdateEmail(Email newEmail)
    {
        email = newEmail;
    }
}