using Rewiews.Application.Common.Interfaces;

namespace Rewiews.Application.TodoReviews.Commands.ReviewsCommands.CreateReviews
{
    public class CreateReviewCommand : ICommand<string>
    {
        public string UserId { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string ProductId { get; set; }
    }
}