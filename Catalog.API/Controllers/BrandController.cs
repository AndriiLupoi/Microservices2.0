using Catalog.Bll.Interfaces;
using Catalog.Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    /// <summary>
    /// Контролер для керування брендами товарів у каталозі.
    /// </summary>
    /// <remarks>
    /// Містить CRUD-операції для створення, перегляду, оновлення та видалення брендів.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        /// <summary>
        /// Отримати список усіх брендів.
        /// </summary>
        /// <returns>Колекція об’єктів <see cref="BrandDto"/>.</returns>
        /// <response code="200">Успішно. Повертає список брендів.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BrandDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(brands);
        }

        /// <summary>
        /// Отримати бренд за його ідентифікатором.
        /// </summary>
        /// <param name="id">Ідентифікатор бренду.</param>
        /// <returns>Об’єкт <see cref="BrandDto"/> або код помилки.</returns>
        /// <response code="200">Знайдено. Повертає дані бренду.</response>
        /// <response code="404">Не знайдено. Бренд із таким ідентифікатором відсутній.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BrandDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null) return NotFound();
            return Ok(brand);
        }

        /// <summary>
        /// Створити новий бренд.
        /// </summary>
        /// <param name="brandDto">Об’єкт бренду для створення.</param>
        /// <returns>Створений бренд.</returns>
        /// <response code="201">Успішно створено. Повертає новий бренд.</response>
        /// <response code="409">Конфлікт. Бренд із такими даними вже існує.</response>
        [HttpPost]
        [ProducesResponseType(typeof(BrandDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] BrandDto brandDto)
        {
            try
            {
                await _brandService.AddBrandAsync(brandDto);
                return CreatedAtAction(nameof(GetById), new { id = brandDto.BrandId }, brandDto);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// Оновити існуючий бренд.
        /// </summary>
        /// <param name="id">Ідентифікатор бренду.</param>
        /// <param name="brandDto">Оновлені дані бренду.</param>
        /// <response code="204">Успішно оновлено.</response>
        /// <response code="400">Невірний запит. Ідентифікатори не збігаються.</response>
        /// <response code="404">Не знайдено. Вказаний бренд не існує.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] BrandDto brandDto)
        {
            if (id != brandDto.BrandId) return BadRequest("Id у URL не відповідає Id у тілі запиту.");

            try
            {
                await _brandService.UpdateBrandAsync(brandDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Видалити бренд за ідентифікатором.
        /// </summary>
        /// <param name="id">Ідентифікатор бренду.</param>
        /// <response code="204">Успішно видалено.</response>
        /// <response code="404">Не знайдено. Бренд не існує.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _brandService.DeleteBrandAsync(id);
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
            var result = await _brandService.GetPagedBrandsAsync(page, pageSize, sortBy, sortDir);
            return Ok(result);
        }

    }
}
