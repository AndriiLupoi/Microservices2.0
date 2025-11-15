using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoReviews.Commands.ReviewsCommands.UptadeReviews
{
    public class UpdateReviewCommand : IRequest<string>
    {
        public string Id { get; set; } = string.Empty;
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public int? Version { get; set; }
        public string ProductId { get; internal set; }
    }
}
