using Catalog.Bll.Interfaces;
using Catalog.Common.DTO;
using Catalog.Common.DTO.ProductCategoryDto_s;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    /// <summary>
    /// Контролер для керування зв'язками продуктів та категорій.
    /// </summary>
    [Route("api/product-categories")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _service;

        public ProductCategoryController(IProductCategoryService service)
        {
            _service = service;
        }

        /// <summary>Отримати всі зв'язки продуктів та категорій.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductCategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllProductsCategoryAsync();
            return Ok(list);
        }

        /// <summary>Отримати зв'язок продукту та категорії за ідентифікатором продукту.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _service.GetProductCategoryByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        /// <summary>Створити зв'язок продукту та категорії.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProductCategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] ProductCategoryCreateDto dto)
        {
            try
            {
                await _service.AddProductCategoryAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dto.ProductId }, dto);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>Оновити зв'язок продукту та категорії.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCategoryDto dto)
        {
            if (id != dto.ProductId) return BadRequest();
            try
            {
                await _service.UpdateProductCategoryAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>Видалити зв'язок продукту та категорії за ідентифікатором продукту.</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteProductCategoryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] int? productId = null, [FromQuery] int? categoryId = null,
                                          [FromQuery] string? sortBy = null, [FromQuery] string sortDir = "asc")
        {
            var result = await _service.GetPagedProductsCategoryAsync(page, pageSize, productId, categoryId, sortBy, sortDir);
            return Ok(result);
        }

    }
}
