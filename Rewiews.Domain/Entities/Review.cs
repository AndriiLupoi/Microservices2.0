
using Rewiews.Domain.Common;

namespace Rewiews.Domain.Entities;

public class Review : BaseEntity
{
    public required string UserId { get; set; } // reference на UserProfile
    public int Rating { get; set; }
    public required string Comment { get; set; }
    public required string ProductId { get; set; }

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
