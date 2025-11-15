using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoReviews.Commands.ReviewsCommands.CreateReviews;
using Rewiews.Application.TodoReviews.Commands.ReviewsCommands.DeleteReviews;
using Rewiews.Application.TodoReviews.Commands.ReviewsCommands.UptadeReviews;
using Rewiews.Application.TodoReviews.ReviewsQueries.GetReviews;

namespace Rewiews.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/reviews?productId={productId}
        [HttpGet]
        public async Task<IActionResult> GetReviews([FromQuery] string productId)
        {
            var query = new GetReviewsListQuery { ProductId = productId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // GET: api/reviews/{reviewId}
        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetReviewById(string reviewId)
        {
            var query = new GetReviewByIdQuery(reviewId);
            var result = await _mediator.Send(query);

            if (result == null)
                throw new NotFoundException("Review", reviewId);

            return Ok(result);
        }

        // POST: api/reviews
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewCommand command)
        {
            var reviewId = await _mediator.Send(command);

            return Created($"/api/reviews/{reviewId}", new { reviewId });
        }

        // PUT: api/reviews/{reviewId}
        [HttpPut("{reviewId}")]
        public async Task<IActionResult> UpdateReview(
            string reviewId,
            [FromBody] UpdateReviewCommand command)
        {
            command.Id = reviewId;

            if (Request.Headers.TryGetValue("If-Match", out var etag))
                command.Version = int.Parse(etag);

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/reviews/{reviewId}
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(string reviewId)
        {
            var command = new DeleteReviewCommand { Id = reviewId };
            await _mediator.Send(command);

            return NoContent();
        }
    }
}