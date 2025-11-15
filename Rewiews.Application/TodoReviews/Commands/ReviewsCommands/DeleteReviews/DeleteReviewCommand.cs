using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoReviews.Commands.ReviewsCommands.DeleteReviews
{
    public class DeleteReviewCommand : IRequest<string>
    {
        public string Id { get; set; } = string.Empty;
        public string ProductId { get; internal set; }
    }
}
