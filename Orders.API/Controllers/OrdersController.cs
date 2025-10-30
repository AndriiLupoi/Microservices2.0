using Common.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Orders.Bll.Exceptions;
using Orders.Bll.Interfaces;

namespace Orders.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _service;

        public OrdersController(IOrdersService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all orders.
        /// </summary>
        /// <returns>List of orders.</returns>
        /// <response code="200">Returns list of orders.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdersDTO>>> GetAll()
        {
            var orders = await _service.GetAllAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Get a specific order by ID.
        /// </summary>
        /// <param name="id">Order ID.</param>
        /// <returns>Order details.</returns>
        /// <response code="200">Returns the order.</response>
        /// <response code="404">Order not found.</response>
        [HttpGet("{id:int}", Name = "GetOrder")]
        public async Task<ActionResult<OrdersDTO>> Get(int id)
        {
            try
            {
                var order = await _service.GetByIdAsync(id);
                return Ok(order);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        /// <param name="dto">Order DTO.</param>
        /// <returns>Created order.</returns>
        /// <response code="201">Order created successfully.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="409">Business conflict.</response>
        /// <response code="422">Validation failed.</response>
        [HttpPost]
        public async Task<ActionResult<OrdersDTO>> Create([FromBody] OrdersDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.AddAsync(dto);
                return CreatedAtRoute("GetOrder", new { id = dto.OrderId }, dto);
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
        /// Update an existing order.
        /// </summary>
        /// <param name="id">Order ID.</param>
        /// <param name="dto">Order DTO.</param>
        /// <response code="204">Order updated successfully.</response>
        /// <response code="400">Invalid input or ID mismatch.</response>
        /// <response code="404">Order not found.</response>
        /// <response code="409">Business conflict.</response>
        /// <response code="422">Validation failed.</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrdersDTO dto)
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
        /// Delete an order by ID.
        /// </summary>
        /// <param name="id">Order ID.</param>
        /// <response code="204">Order deleted successfully.</response>
        /// <response code="404">Order not found.</response>
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
