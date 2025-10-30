using Common.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Orders.Bll.Exceptions;
using Orders.Bll.Interfaces;

namespace Orders.Api.Controllers
{
    /// <summary>
    /// Controller for managing customers.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersService _service;

        public CustomersController(ICustomersService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all customers.
        /// </summary>
        /// <returns>List of customers.</returns>
        /// <response code="200">Returns list of customers.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomersDTO>>> GetAll()
        {
            var customers = await _service.GetAllAsync();
            return Ok(customers);
        }

        /// <summary>
        /// Get customer by ID.
        /// </summary>
        /// <param name="id">Customer ID.</param>
        /// <returns>Customer details.</returns>
        /// <response code="200">Returns the customer.</response>
        /// <response code="404">Customer not found.</response>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomersDTO>> Get(int id)
        {
            try
            {
                var customer = await _service.GetByIdAsync(id);
                return Ok(customer);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create a new customer.
        /// </summary>
        /// <param name="dto">Customer data.</param>
        /// <returns>Created customer.</returns>
        /// <response code="201">Customer created successfully.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="409">Email conflict.</response>
        /// <response code="422">Validation failed.</response>
        [HttpPost]
        public async Task<ActionResult<CustomersDTO>> Create([FromBody] CustomersDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.AddAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = dto.CustomerId }, dto);
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
        /// Update an existing customer.
        /// </summary>
        /// <param name="id">Customer ID.</param>
        /// <param name="dto">Customer data.</param>
        /// <response code="204">Customer updated successfully.</response>
        /// <response code="400">Invalid input or ID mismatch.</response>
        /// <response code="404">Customer not found.</response>
        /// <response code="409">Email conflict.</response>
        /// <response code="422">Validation failed.</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomersDTO dto)
        {
            if (id != dto.CustomerId)
                return BadRequest(new { message = "ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
        /// Delete a customer by ID.
        /// </summary>
        /// <param name="id">Customer ID.</param>
        /// <response code="204">Customer deleted successfully.</response>
        /// <response code="404">Customer not found.</response>
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
