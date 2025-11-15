using Rewiews.Application.Common.DTOs;
using Rewiews.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoReviews.ReviewsQueries.GetReviews
{
    public class GetReviewByIdQuery : IQuery<ReviewDto>
    {
        public string Id { get; set; } = null!;
        public string ProductId { get; internal set; }

        public GetReviewByIdQuery(string id)
        {
            Id = id;
        }
    }
}
