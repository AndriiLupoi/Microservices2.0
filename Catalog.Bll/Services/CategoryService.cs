using AutoMapper;
using Catalog.Bll.Exceptions;
using Catalog.Bll.Interfaces;
using Catalog.Common.DTO;
using Catalog.Common.Pagination;
using Catalog.Dal.Repo.UOW;
using Catalog.Dal.Specifications;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Bll.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategorysAsync()
        {
            var categories = await _uow.Categories.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null)
                throw new EntityNotFoundException($"Category with id {id} not found.");

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task AddCategoryAsync(CategoryDto categoryDto)
        {
            var exists = await _uow.Categories.Query()
                .AnyAsync(c => c.Name == categoryDto.Name);
            if (exists)
                throw new ConflictException($"Category with name '{categoryDto.Name}' already exists.");

            var category = _mapper.Map<Category>(categoryDto);
            await _uow.Categories.AddAsync(category);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = await _uow.Categories.GetByIdAsync(categoryDto.CategoryId);
            if (category == null)
                throw new EntityNotFoundException($"Category with id {categoryDto.CategoryId} not found.");

            var exists = await _uow.Categories.Query()
                .AnyAsync(c => c.Name == categoryDto.Name && c.CategoryId != categoryDto.CategoryId);
            if (exists)
                throw new ConflictException($"Category with name '{categoryDto.Name}' already exists.");

            _mapper.Map(categoryDto, category);
            _uow.Categories.Update(category);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _uow.Categories.GetByIdAsync(id);
            if (category == null)
                throw new EntityNotFoundException($"Category with id {id} not found.");

            var hasRelations = await _uow.ProductCategories.Query()
                .AnyAsync(pc => pc.CategoryId == id);
            if (hasRelations)
                throw new ConflictException($"Category with id {id} cannot be deleted because it has linked products.");

            _uow.Categories.Delete(category);
            await _uow.SaveChangesAsync();
        }

        public async Task<PagedResultDto<CategoryDto>> GetPagedCategoriesAsync(
            int page = 1, int pageSize = 20,
            string? sortBy = null, string sortDir = "asc")
        {
            pageSize = Math.Min(pageSize, 100);

            var spec = new CategoryFilterSpecification(
                null,
                sortBy,
                sortDir,
                skip: (page - 1) * pageSize,
                take: pageSize);
            var countSpec = new CategoryFilterSpecification();

            var totalCount = await _uow.Categories.CountAsync(spec);
            var categories = await _uow.Categories.ListAsync(spec);

            return new PagedResultDto<CategoryDto>
            {
                Items = _mapper.Map<List<CategoryDto>>(categories),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

    }
}
