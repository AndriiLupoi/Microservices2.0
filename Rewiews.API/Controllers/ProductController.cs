using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.CreateTodo;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.DeleteTodo;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.UptadeTodo;
using Rewiews.Application.TodoProducts.Queries.GetTodoProducts;

namespace Rewiews.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/products
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetProductsListQuery());
            return Ok(result);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product == null)
                return NotFound(new { message = $"Product '{id}' not found." });

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return UnprocessableEntity(new { message = ex.Message });
            }
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProductCommand command)
        {
            command.Id = id;

            if (Request.Headers.TryGetValue("If-Match", out var etag))
                command.Version = int.Parse(etag);

            var result = await _mediator.Send(command);

            if (result.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return NotFound(new { message = $"Product '{id}' not found." });

            if (result.Contains("conflict", StringComparison.OrdinalIgnoreCase))
                return Conflict(new { message = $"Version conflict while updating product '{id}'." });

            return Ok(result);
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.Send(new DeleteProductCommand { Id = id });

            if (result.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return NotFound(new { message = $"Product '{id}' not found." });

            return NoContent();
        }
    }
}
