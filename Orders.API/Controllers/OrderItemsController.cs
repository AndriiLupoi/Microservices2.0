using Common.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Orders.Bll.Exceptions;
using Orders.Bll.Interfaces;

namespace Orders.Api.Controllers
{
    [Route("api/order-items")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemsService _service;

        public OrderItemsController(IOrderItemsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all order items.
        /// </summary>
        /// <returns>List of order items.</returns>
        /// <response code="200">Returns list of order items.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemsDTO>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        /// <summary>
        /// Get a specific order item by ID.
        /// </summary>
        /// <param name="id">OrderItem ID.</param>
        /// <returns>OrderItem details.</returns>
        /// <response code="200">Returns the order item.</response>
        /// <response code="404">Order item not found.</response>
        [HttpGet("{id:int}", Name = "GetOrderItem")]
        public async Task<ActionResult<OrderItemsDTO>> Get(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);
                return Ok(item);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create a new order item.
        /// </summary>
        /// <param name="dto">OrderItem DTO.</param>
        /// <returns>Created OrderItem.</returns>
        /// <response code="201">Order item created successfully.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="409">Business conflict (duplicate).</response>
        /// <response code="422">Validation failed (e.g., quantity ≤ 0).</response>
        [HttpPost]
        public async Task<ActionResult<OrderItemsDTO>> Create([FromBody] OrderItemsDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.AddAsync(dto);
                return CreatedAtRoute("GetOrderItem", new { id = dto.OrderId }, dto);
            }
            catch (BusinessConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return UnprocessableEntity(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing order item.
        /// </summary>
        /// <param name="id">OrderItem ID.</param>
        /// <param name="dto">OrderItem DTO.</param>
        /// <response code="204">Order item updated successfully.</response>
        /// <response code="400">Invalid input or ID mismatch.</response>
        /// <response code="404">Order item not found.</response>
        /// <response code="409">Business conflict.</response>
        /// <response code="422">Validation failed.</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderItemsDTO dto)
        {
            if (id != dto.OrderId) return BadRequest(new { message = "ID mismatch." });
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _service.UpdateAsync(dto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return UnprocessableEntity(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete an order item by ID.
        /// </summary>
        /// <param name="id">OrderItem ID.</param>
        /// <response code="204">Order item deleted successfully.</response>
        /// <response code="404">Order item not found.</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
