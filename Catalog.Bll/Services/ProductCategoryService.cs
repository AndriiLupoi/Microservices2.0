using AutoMapper;
using Catalog.Bll.Exceptions;
using Catalog.Bll.Interfaces;
using Catalog.Common.DTO;
using Catalog.Common.DTO.ProductCategoryDto_s;
using Catalog.Common.Pagination;
using Catalog.Dal.Repo.UOW;
using Catalog.Dal.Specifications;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Bll.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductCategoryService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetAllProductsCategoryAsync()
        {
            var productCategories = await _uow.ProductCategories.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductCategoryDto>>(productCategories);
        }

        public async Task<ProductCategoryDto> GetProductCategoryByIdAsync(int id)
        {
            var productCategory = await _uow.ProductCategories.GetByIdAsync(id);
            if (productCategory == null)
                throw new EntityNotFoundException($"ProductCategory with id {id} not found.");

            return _mapper.Map<ProductCategoryDto>(productCategory);
        }

        public async Task AddProductCategoryAsync(ProductCategoryCreateDto productCategoryDto)
        {
            var product = await _uow.Products.GetByIdAsync(productCategoryDto.ProductId);
            if (product == null)
                throw new ValidationCustomException($"Product with id {productCategoryDto.ProductId} does not exist.");

            var category = await _uow.Categories.GetByIdAsync(productCategoryDto.CategoryId);
            if (category == null)
                throw new ValidationCustomException($"Category with id {productCategoryDto.CategoryId} does not exist.");

            var exists = await _uow.ProductCategories.Query()
                .AnyAsync(pc => pc.ProductId == productCategoryDto.ProductId && pc.CategoryId == productCategoryDto.CategoryId);
            if (exists)
                throw new ConflictException($"ProductCategory with ProductId {productCategoryDto.ProductId} and CategoryId {productCategoryDto.CategoryId} already exists.");

            var productCategory = _mapper.Map<ProductCategory>(productCategoryDto);
            await _uow.ProductCategories.AddAsync(productCategory);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateProductCategoryAsync(ProductCategoryDto productCategoryDto)
        {
            var productCategory = await _uow.ProductCategories.GetByIdAsync(productCategoryDto.Id);
            if (productCategory == null)
                throw new EntityNotFoundException($"ProductCategory with id {productCategoryDto.Id} not found.");

            _mapper.Map(productCategoryDto, productCategory);
            _uow.ProductCategories.Update(productCategory);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteProductCategoryAsync(int id)
        {
            var productCategory = await _uow.ProductCategories.GetByIdAsync(id);
            if (productCategory == null)
                throw new EntityNotFoundException($"ProductCategory with id {id} not found.");

            _uow.ProductCategories.Delete(productCategory);
            await _uow.SaveChangesAsync();
        }

        public async Task<PagedResultDto<ProductCategoryDto>> GetPagedProductsCategoryAsync(
            int page = 1, int pageSize = 20,
            int? productId = null, int? categoryId = null,
            string? sortBy = null, string sortDir = "asc")
        {
            pageSize = Math.Min(pageSize, 100);

            var spec = new ProductCategoryFilterSpecification(
                productId: productId,
                categoryId: categoryId,
                sortBy: sortBy,
                sortDir: sortDir,
                skip: (page - 1) * pageSize,
                take: pageSize);

            var countSpec = new ProductCategoryFilterSpecification(
                productId: productId,
                categoryId: categoryId,
                sortBy: sortBy,
                sortDir: sortDir);


            var totalCount = await _uow.ProductCategories.CountAsync(countSpec);
            var productCategories = await _uow.ProductCategories.ListAsync(spec);

            return new PagedResultDto<ProductCategoryDto>
            {
                Items = _mapper.Map<List<ProductCategoryDto>>(productCategories),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

    }
}
