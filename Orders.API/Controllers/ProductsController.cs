using Common.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Orders.Bll.Exceptions;
using Orders.Bll.Interfaces;

namespace Orders.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _service;

        public ProductsController(IProductsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        /// <returns>List of products.</returns>
        /// <response code="200">Returns list of products.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsDTO>>> GetAll()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        /// <summary>
        /// Get a specific product by ID.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <returns>Product details.</returns>
        /// <response code="200">Returns the product.</response>
        /// <response code="404">Product not found.</response>
        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductsDTO>> Get(int id)
        {
            try
            {
                var product = await _service.GetByIdAsync(id);
                return Ok(product);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create a new product.
        /// </summary>
        /// <param name="dto">Product DTO.</param>
        /// <returns>Created product.</returns>
        /// <response code="201">Product created successfully.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="409">Business conflict.</response>
        /// <response code="422">Validation failed.</response>
        [HttpPost]
        public async Task<ActionResult<ProductsDTO>> Create([FromBody] ProductsDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.AddAsync(dto);
                return CreatedAtRoute("GetProduct", new { id = dto.Id }, dto);
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
        /// Update an existing product.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <param name="dto">Product DTO.</param>
        /// <response code="204">Product updated successfully.</response>
        /// <response code="400">Invalid input or ID mismatch.</response>
        /// <response code="404">Product not found.</response>
        /// <response code="409">Business conflict.</response>
        /// <response code="422">Validation failed.</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductsDTO dto)
        {
            if (id != dto.Id) return BadRequest(new { message = "ID mismatch." });
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
        /// Delete a product by ID.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <response code="204">Product deleted successfully.</response>
        /// <response code="404">Product not found.</response>
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
