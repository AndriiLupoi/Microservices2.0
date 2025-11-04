using Catalog.Bll.Interfaces;
using Catalog.Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    /// <summary>
    /// Контролер для керування категоріями товарів.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>Отримати всі категорії.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategorysAsync();
            return Ok(categories);
        }

        /// <summary>Отримати категорію за ідентифікатором.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        /// <summary>Створити нову категорію.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
        {
            try
            {
                await _categoryService.AddCategoryAsync(categoryDto);
                return CreatedAtAction(nameof(GetById), new { id = categoryDto.CategoryId }, categoryDto);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>Оновити категорію за ідентифікатором.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto categoryDto)
        {
            if (id != categoryDto.CategoryId) return BadRequest();
            try
            {
                await _categoryService.UpdateCategoryAsync(categoryDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>Видалити категорію за ідентифікатором.</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? sortBy = null, [FromQuery] string sortDir = "asc")
        {
            var result = await _categoryService.GetPagedCategoriesAsync(page, pageSize, sortBy, sortDir);
            return Ok(result);
        }

    }
}
