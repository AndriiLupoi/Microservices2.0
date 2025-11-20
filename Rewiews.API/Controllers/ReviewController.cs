using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        public async Task<IActionResult> GetReviews([FromQuery] string productId)
        {
            var result = await _mediator.Send(new GetReviewsListQuery { ProductId = productId });
            return Ok(result);
        }

        // GET: api/reviews/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(string id)
        {
            var review = await _mediator.Send(new GetReviewByIdQuery(id));
            if (review == null)
                return NotFound(new { message = $"Review '{id}' not found." });

            return Ok(review);
        }

        // POST: api/reviews
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Create([FromBody] CreateReviewCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        // PUT: api/reviews/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateReviewCommand command)
        {
            command.Id = id;

            if (Request.Headers.TryGetValue("If-Match", out var etag))
                command.Version = int.Parse(etag);

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/reviews/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteReviewCommand { Id = id });
            return NoContent();
        }
    }
}