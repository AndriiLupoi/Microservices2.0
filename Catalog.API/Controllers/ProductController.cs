using Catalog.Bll.Interfaces;
using Catalog.Common.DTO;
using Catalog.Common.DTO.ProductDto_s;
using Catalog.Common.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>Отримати всі товари.</summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>Отримати товар за ідентифікатором.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>Створити новий товар.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto productDto)
        {
            try
            {
                await _productService.AddProductAsync(productDto);
                return CreatedAtAction(nameof(GetById), new { id = productDto.Name }, productDto);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>Оновити товар за ідентифікатором.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.ProductId) return BadRequest();
            try
            {
                await _productService.UpdateProductAsync(id, productDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>Видалити товар за ідентифікатором.</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Повертає товари з пагінацією, фільтрацією та сортуванням.
        /// </summary>
        /// <param name="page">Номер сторінки (починається з 1)</param>
        /// <param name="pageSize">Розмір сторінки (дефолт 20, максимум 100)</param>
        /// <param name="brandId">Фільтр по бренду (опційно)</param>
        /// <param name="categoryId">Фільтр по категорії (опційно)</param>
        /// <param name="sortBy">Поле для сортування: name або price</param>
        /// <param name="sortDir">Напрям сортування: asc або desc</param>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] int? brandId = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string sortDir = "asc")
        {
            var result = await _productService.GetPagedProductsAsync(page, pageSize, brandId, categoryId, sortBy, sortDir);
            return Ok(result);
        }
    }
}
