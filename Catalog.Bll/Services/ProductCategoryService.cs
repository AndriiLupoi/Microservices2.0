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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Bll.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductCategoryService> _logger;

        public ProductCategoryService(IUnitOfWork uow, IMapper mapper, ILogger<ProductCategoryService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetAllProductsCategoryAsync()
        {
            _logger.LogInformation("Fetching all product categories");
            var productCategories = await _uow.ProductCategories.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ProductCategoryDto>>(productCategories);
            _logger.LogInformation("Fetched {Count} product categories", dtos.Count());
            return dtos;
        }

        public async Task<ProductCategoryDto> GetProductCategoryByIdAsync(int id)
        {
            _logger.LogInformation("Fetching product category with Id: {Id}", id);
            var productCategory = await _uow.ProductCategories.GetByIdAsync(id);
            if (productCategory == null)
            {
                _logger.LogWarning("ProductCategory with Id {Id} not found", id);
                throw new EntityNotFoundException($"ProductCategory with id {id} not found.");
            }

            var dto = _mapper.Map<ProductCategoryDto>(productCategory);
            _logger.LogInformation("ProductCategory with Id {Id} retrieved successfully", id);
            return dto;
        }

        public async Task AddProductCategoryAsync(ProductCategoryCreateDto productCategoryDto)
        {
            _logger.LogInformation("Adding product category: ProductId {ProductId}, CategoryId {CategoryId}",
                productCategoryDto.ProductId, productCategoryDto.CategoryId);

            var product = await _uow.Products.GetByIdAsync(productCategoryDto.ProductId);
            if (product == null)
            {
                _logger.LogWarning("Product with Id {ProductId} does not exist", productCategoryDto.ProductId);
                throw new ValidationCustomException($"Product with id {productCategoryDto.ProductId} does not exist.");
            }

            var category = await _uow.Categories.GetByIdAsync(productCategoryDto.CategoryId);
            if (category == null)
            {
                _logger.LogWarning("Category with Id {CategoryId} does not exist", productCategoryDto.CategoryId);
                throw new ValidationCustomException($"Category with id {productCategoryDto.CategoryId} does not exist.");
            }

            var exists = await _uow.ProductCategories.Query()
                .AnyAsync(pc => pc.ProductId == productCategoryDto.ProductId && pc.CategoryId == productCategoryDto.CategoryId);
            if (exists)
            {
                _logger.LogWarning("ProductCategory already exists for ProductId {ProductId} and CategoryId {CategoryId}",
                    productCategoryDto.ProductId, productCategoryDto.CategoryId);
                throw new ConflictException($"ProductCategory with ProductId {productCategoryDto.ProductId} and CategoryId {productCategoryDto.CategoryId} already exists.");
            }

            var productCategory = _mapper.Map<ProductCategory>(productCategoryDto);
            await _uow.ProductCategories.AddAsync(productCategory);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("ProductCategory added successfully: Id {Id}", productCategory.Id);
        }

        public async Task UpdateProductCategoryAsync(ProductCategoryDto productCategoryDto)
        {
            _logger.LogInformation("Updating product category Id: {Id}", productCategoryDto.Id);

            var productCategory = await _uow.ProductCategories.GetByIdAsync(productCategoryDto.Id);
            if (productCategory == null)
            {
                _logger.LogWarning("ProductCategory with Id {Id} not found", productCategoryDto.Id);
                throw new EntityNotFoundException($"ProductCategory with id {productCategoryDto.Id} not found.");
            }

            _mapper.Map(productCategoryDto, productCategory);
            _uow.ProductCategories.Update(productCategory);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("ProductCategory Id {Id} updated successfully", productCategoryDto.Id);
        }

        public async Task DeleteProductCategoryAsync(int id)
        {
            _logger.LogInformation("Deleting product category Id: {Id}", id);

            var productCategory = await _uow.ProductCategories.GetByIdAsync(id);
            if (productCategory == null)
            {
                _logger.LogWarning("ProductCategory with Id {Id} not found", id);
                throw new EntityNotFoundException($"ProductCategory with id {id} not found.");
            }

            _uow.ProductCategories.Delete(productCategory);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("ProductCategory Id {Id} deleted successfully", id);
        }

        public async Task<PagedResultDto<ProductCategoryDto>> GetPagedProductsCategoryAsync(
            int page = 1, int pageSize = 20,
            int? productId = null, int? categoryId = null,
            string? sortBy = null, string sortDir = "asc")
        {
            _logger.LogInformation("Fetching paged product categories: Page {Page}, PageSize {PageSize}", page, pageSize);

            pageSize = Math.Min(pageSize, 100);

            var spec = new ProductCategoryFilterSpecification(
                productId, categoryId, sortBy, sortDir, (page - 1) * pageSize, pageSize);

            var countSpec = new ProductCategoryFilterSpecification(productId, categoryId, sortBy, sortDir);
            var totalCount = await _uow.ProductCategories.CountAsync(countSpec);
            var productCategories = await _uow.ProductCategories.ListAsync(spec);
            var dtos = _mapper.Map<List<ProductCategoryDto>>(productCategories);

            _logger.LogInformation("Paged product categories fetched successfully: TotalCount {TotalCount}", totalCount);

            return new PagedResultDto<ProductCategoryDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
