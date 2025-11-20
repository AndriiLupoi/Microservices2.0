using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.CreateUser;
using Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.DeleteUser;
using Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.UptadeUser;
using Rewiews.Application.TodoUserProfile.UserQueries.GetUser;

namespace Rewiews.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetUserProfileByIdQuery(id));
            if (result == null)
                return NotFound(new { message = $"User '{id}' not found." });

            return Ok(result);
        }

        // POST: api/users
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Create([FromBody] CreateUserProfileCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserProfileCommand command)
        {
            command.Id = id;

            if (Request.Headers.TryGetValue("If-Match", out var etag))
                command.Version = int.Parse(etag);

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteUserProfileCommand(id));
            return NoContent();
        }
    }
}