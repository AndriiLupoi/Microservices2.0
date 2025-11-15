using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.CreateUser;
using Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.DeleteUser;
using Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.UptadeUser;
using Rewiews.Application.TodoUserProfile.UserQueries.GetUser;

namespace Rewiews.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/UserProfile/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetUserProfileByIdQuery(id));

            if (result == null)
                throw new NotFoundException("UserProfile", id);

            return Ok(result);
        }

        // POST: api/UserProfile
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserProfileCommand command)
        {
            var id = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { id }
            );
        }

        // PUT: api/UserProfile/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserProfileCommand command)
        {
            command.Id = id;

            if (Request.Headers.TryGetValue("If-Match", out var etag))
                command.Version = int.Parse(etag);

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/UserProfile/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteUserProfileCommand(id));
            return NoContent();
        }
    }
}