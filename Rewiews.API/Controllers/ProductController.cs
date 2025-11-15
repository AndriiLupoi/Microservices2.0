using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.CreateTodo;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.DeleteTodo;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.UptadeTodo;
using Rewiews.Application.TodoProducts.Queries.GetTodoProducts;

namespace Rewiews.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetProductsListQuery());
            return Ok(result);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product == null)
                throw new NotFoundException("Product", id);

            return Ok(product);

        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProductCommand command)
        {
            command.Id = id;

            if (Request.Headers.TryGetValue("If-Match", out var etag))
                command.Version = int.Parse(etag);

            var result = await _mediator.Send(command);

            if (result.Contains("not found", StringComparison.OrdinalIgnoreCase))
                throw new NotFoundException("Product", id);

            if (result.Contains("conflict", StringComparison.OrdinalIgnoreCase))
                throw new ConflictException($"Version conflict while updating product '{id}'.");

            return Ok(result);
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.Send(new DeleteProductCommand { Id = id });

            if (result.Contains("not found", StringComparison.OrdinalIgnoreCase))
                throw new NotFoundException("Product", id);

            return NoContent();
        }
    }
}
