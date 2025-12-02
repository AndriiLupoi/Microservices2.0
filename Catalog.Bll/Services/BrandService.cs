using AutoMapper;
using Catalog.Bll.Exceptions;
using Catalog.Bll.Interfaces;
using Catalog.Common.DTO;
using Catalog.Common.Pagination;
using Catalog.Dal.Repo.UOW;
using Catalog.Dal.Specifications;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.Bll.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<BrandService> _logger;

        public BrandService(IUnitOfWork uow, IMapper mapper, ILogger<BrandService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            _logger.LogInformation("Fetching all brands");
            var brands = await _uow.Brands.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<BrandDto>>(brands);
            _logger.LogInformation("Fetched {Count} brands", dtos.Count());
            return dtos;
        }

        public async Task<BrandDto> GetBrandByIdAsync(int id)
        {
            _logger.LogInformation("Fetching brand with Id: {Id}", id);
            var brand = await _uow.Brands.GetByIdAsync(id);
            if (brand == null)
            {
                _logger.LogWarning("Brand with Id {Id} not found", id);
                throw new EntityNotFoundException($"Brand with id {id} not found.");
            }

            var dto = _mapper.Map<BrandDto>(brand);
            _logger.LogInformation("Brand with Id {Id} retrieved successfully", id);
            return dto;
        }

        public async Task AddBrandAsync(BrandCreateDto brandDto)
        {
            _logger.LogInformation("Adding new brand: {Name}", brandDto.Name);
            var exists = await _uow.Brands.Query().AnyAsync(b => b.Name == brandDto.Name);
            if (exists)
            {
                _logger.LogWarning("Brand with name '{Name}' already exists", brandDto.Name);
                throw new ConflictException($"Brand with name '{brandDto.Name}' already exists.");
            }

            var brand = _mapper.Map<Brand>(brandDto);
            await _uow.Brands.AddAsync(brand);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("Brand '{Name}' added successfully with Id {Id}", brand.Name, brand.BrandId);
        }

        public async Task UpdateBrandAsync(BrandDto brandDto)
        {
            _logger.LogInformation("Updating brand Id: {Id}", brandDto.BrandId);
            var brand = await _uow.Brands.GetByIdAsync(brandDto.BrandId);
            if (brand == null)
            {
                _logger.LogWarning("Brand with Id {Id} not found", brandDto.BrandId);
                throw new EntityNotFoundException($"Brand with id {brandDto.BrandId} not found.");
            }

            var exists = await _uow.Brands.Query()
                .AnyAsync(b => b.Name == brandDto.Name && b.BrandId != brandDto.BrandId);
            if (exists)
            {
                _logger.LogWarning("Brand with name '{Name}' already exists", brandDto.Name);
                throw new ConflictException($"Brand with name '{brandDto.Name}' already exists.");
            }

            _mapper.Map(brandDto, brand);
            _uow.Brands.Update(brand);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("Brand Id {Id} updated successfully", brandDto.BrandId);
        }

        public async Task DeleteBrandAsync(int id)
        {
            _logger.LogInformation("Deleting brand Id: {Id}", id);
            var brand = await _uow.Brands.GetByIdAsync(id);
            if (brand == null)
            {
                _logger.LogWarning("Brand with Id {Id} not found", id);
                throw new EntityNotFoundException($"Brand with id {id} not found.");
            }

            var hasRelations = await _uow.Products.Query().AnyAsync(p => p.BrandId == id);
            if (hasRelations)
            {
                _logger.LogWarning("Brand Id {Id} cannot be deleted, it has linked products", id);
                throw new ConflictException($"Brand with id {id} cannot be deleted because it has linked products.");
            }

            _uow.Brands.Delete(brand);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("Brand Id {Id} deleted successfully", id);
        }

        public async Task<PagedResultDto<BrandDto>> GetPagedBrandsAsync(int page, int pageSize, string? sortBy, string sortDir)
        {
            _logger.LogInformation("Fetching paged brands: Page {Page}, PageSize {PageSize}", page, pageSize);
            var spec = new BrandFilterSpecification(
                null,
                sortBy,
                sortDir,
                (page - 1) * pageSize,
                pageSize);

            var totalCount = await _uow.Brands.CountAsync(spec);
            var brands = await _uow.Brands.ListAsync(spec);

            var result = new PagedResultDto<BrandDto>
            {
                Items = _mapper.Map<List<BrandDto>>(brands),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
            _logger.LogInformation("Paged brands fetched successfully: TotalCount {TotalCount}", totalCount);
            return result;
        }
    }
}
