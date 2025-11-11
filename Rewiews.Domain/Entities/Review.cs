
namespace Rewiews.Domain.Entities;

public class Review
{

    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string UserId { get; private set; } // reference на UserProfile
    public int Rating { get; private set; }
    public string Comment { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public void UpdateRating(int newRating)
    {
        Rating = newRating;
    }

    public void UpdateComment(string newComment)
    {
        Comment = newComment;
    }

    public void SetUser(string userId)
    {
        UserId = userId;
    }
}
