using AutoMapper;
using Catalog.Bll.Exceptions;
using Catalog.Bll.Interfaces;
using Catalog.Common.DTO;
using Catalog.Common.DTO.CategoryDto_s;
using Catalog.Common.Pagination;
using Catalog.Dal.Repo.UOW;
using Catalog.Dal.Specifications;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Bll.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitOfWork uow, IMapper mapper, ILogger<CategoryService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategorysAsync()
        {
            _logger.LogInformation("Fetching all categories");
            var categories = await _uow.Categories.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            _logger.LogInformation("Fetched {Count} categories", dtos.Count());
            return dtos;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            _logger.LogInformation("Fetching category with Id: {Id}", id);
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category with Id {Id} not found", id);
                throw new EntityNotFoundException($"Category with id {id} not found.");
            }

            var dto = _mapper.Map<CategoryDto>(category);
            _logger.LogInformation("Category with Id {Id} retrieved successfully", id);
            return dto;
        }

        public async Task AddCategoryAsync(CategoryCreateDto categoryDto)
        {
            _logger.LogInformation("Adding category with Name: {Name}", categoryDto.Name);

            var exists = await _uow.Categories.Query()
                .AnyAsync(c => c.Name == categoryDto.Name);
            if (exists)
            {
                _logger.LogWarning("Category with Name '{Name}' already exists", categoryDto.Name);
                throw new ConflictException($"Category with name '{categoryDto.Name}' already exists.");
            }

            var category = _mapper.Map<Category>(categoryDto);
            await _uow.Categories.AddAsync(category);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("Category '{Name}' added successfully", category.Name);
        }

        public async Task UpdateCategoryAsync(CategoryDto categoryDto)
        {
            _logger.LogInformation("Updating category Id: {Id}", categoryDto.CategoryId);

            var category = await _uow.Categories.GetByIdAsync(categoryDto.CategoryId);
            if (category == null)
            {
                _logger.LogWarning("Category with Id {Id} not found", categoryDto.CategoryId);
                throw new EntityNotFoundException($"Category with id {categoryDto.CategoryId} not found.");
            }

            var exists = await _uow.Categories.Query()
                .AnyAsync(c => c.Name == categoryDto.Name && c.CategoryId != categoryDto.CategoryId);
            if (exists)
            {
                _logger.LogWarning("Category with Name '{Name}' already exists", categoryDto.Name);
                throw new ConflictException($"Category with name '{categoryDto.Name}' already exists.");
            }

            _mapper.Map(categoryDto, category);
            _uow.Categories.Update(category);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("Category Id {Id} updated successfully", categoryDto.CategoryId);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            _logger.LogInformation("Deleting category Id: {Id}", id);

            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning("Category with Id {Id} not found", id);
                throw new EntityNotFoundException($"Category with id {id} not found.");
            }

            var hasRelations = await _uow.ProductCategories.Query()
                .AnyAsync(pc => pc.CategoryId == id);
            if (hasRelations)
            {
                _logger.LogWarning("Category Id {Id} cannot be deleted, linked products exist", id);
                throw new ConflictException($"Category with id {id} cannot be deleted because it has linked products.");
            }

            _uow.Categories.Delete(category);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("Category Id {Id} deleted successfully", id);
        }

        public async Task<PagedResultDto<CategoryDto>> GetPagedCategoriesAsync(
            int page = 1, int pageSize = 20,
            string? sortBy = null, string sortDir = "asc")
        {
            _logger.LogInformation("Fetching paged categories: Page {Page}, PageSize {PageSize}", page, pageSize);

            pageSize = Math.Min(pageSize, 100);

            var spec = new CategoryFilterSpecification(
                null,
                sortBy,
                sortDir,
                skip: (page - 1) * pageSize,
                take: pageSize);

            var totalCount = await _uow.Categories.CountAsync(spec);
            var categories = await _uow.Categories.ListAsync(spec);
            var dtos = _mapper.Map<List<CategoryDto>>(categories);

            _logger.LogInformation("Paged categories fetched successfully: TotalCount {TotalCount}", totalCount);

            return new PagedResultDto<CategoryDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
