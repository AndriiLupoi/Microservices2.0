using Rewiews.Domain.Common;
using Rewiews.Domain.ValueObjects;

namespace Rewiews.Domain.Entities;
public class UserProfile : BaseEntity
{
    public string Username { get; private set; }
    public Email email { get; private set; }

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