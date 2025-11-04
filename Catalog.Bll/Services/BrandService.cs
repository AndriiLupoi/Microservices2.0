using AutoMapper;
using Catalog.Bll.Exceptions;
using Catalog.Bll.Interfaces;
using Catalog.Common.DTO;
using Catalog.Common.Pagination;
using Catalog.Dal.Repo.UOW;
using Catalog.Dal.Specifications;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Bll.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var brands = await _uow.Brands.GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDto>>(brands);
        }

        public async Task<BrandDto> GetBrandByIdAsync(int id)
        {
            var brand = await _uow.Brands.GetByIdAsync(id);
            if (brand == null)
                throw new EntityNotFoundException($"Brand with id {id} not found.");

            return _mapper.Map<BrandDto>(brand);
        }

        public async Task AddBrandAsync(BrandDto brandDto)
        {
            var exists = await _uow.Brands.Query()
                .AnyAsync(b => b.Name == brandDto.Name);
            if (exists)
                throw new ConflictException($"Brand with name '{brandDto.Name}' already exists.");

            var brand = _mapper.Map<Brand>(brandDto);
            await _uow.Brands.AddAsync(brand);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateBrandAsync(BrandDto brandDto)
        {
            var brand = await _uow.Brands.GetByIdAsync(brandDto.BrandId);
            if (brand == null)
                throw new EntityNotFoundException($"Brand with id {brandDto.BrandId} not found.");

            var exists = await _uow.Brands.Query()
                .AnyAsync(b => b.Name == brandDto.Name && b.BrandId != brandDto.BrandId);
            if (exists)
                throw new ConflictException($"Brand with name '{brandDto.Name}' already exists.");

            _mapper.Map(brandDto, brand);
            _uow.Brands.Update(brand);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteBrandAsync(int id)
        {
            var brand = await _uow.Brands.GetByIdAsync(id);
            if (brand == null)
                throw new EntityNotFoundException($"Brand with id {id} not found.");

            var hasRelations = await _uow.Products.Query()
                .AnyAsync(p => p.BrandId == id);
            if (hasRelations)
                throw new ConflictException($"Brand with id {id} cannot be deleted because it has linked products.");

            _uow.Brands.Delete(brand);
            await _uow.SaveChangesAsync();
        }

        public async Task<PagedResultDto<BrandDto>> GetPagedBrandsAsync(int page, int pageSize, string? sortBy, string sortDir)
        {
            var spec = new BrandFilterSpecification(
                null, // search
                sortBy,
                sortDir,
                (page - 1) * pageSize,
                pageSize);

            var totalCount = await _uow.Brands.CountAsync(spec);
            var brands = await _uow.Brands.ListAsync(spec);

            return new PagedResultDto<BrandDto>
            {
                Items = _mapper.Map<List<BrandDto>>(brands),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }


    }
}
