namespace Rewiews.Application.Common.Interfaces;

public interface IUser
{
    string? Id { get; }
    string? Username { get; }
    List<string>? Role { get; }

}
