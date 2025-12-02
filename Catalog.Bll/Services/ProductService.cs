using AutoMapper;
using Catalog.Bll.Exceptions;
using Catalog.Bll.Interfaces;
using Catalog.Common.DTO;
using Catalog.Common.DTO.ProductDto_s;
using Catalog.Common.Pagination;
using Catalog.Dal.Repo.UOW;
using Catalog.Dal.Specifications;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.Bll.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IUnitOfWork uow, IMapper mapper, ILogger<ProductService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            _logger.LogInformation("Fetching all products");
            var products = await _uow.Products.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            _logger.LogInformation("Fetched {Count} products", dtos.Count());
            return dtos;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            _logger.LogInformation("Fetching product with Id: {Id}", id);
            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with Id {Id} not found", id);
                throw new EntityNotFoundException($"Product with id {id} not found.");
            }

            var dto = _mapper.Map<ProductDto>(product);
            _logger.LogInformation("Product with Id {Id} retrieved successfully", id);
            return dto;
        }

        public async Task<ProductDto?> GetProductByNameAsync(string name)
        {
            _logger.LogInformation("Fetching product by name: {Name}", name);
            var product = await _uow.Products.GetByNameAsync(name);
            if (product == null)
            {
                _logger.LogWarning("Product with name {Name} not found", name);
                throw new EntityNotFoundException($"Product with name {name} not found.");
            }

            var dto = _mapper.Map<ProductDto>(product);
            _logger.LogInformation("Product with name {Name} retrieved successfully", name);
            return dto;
        }

        public async Task AddProductAsync(ProductCreateDto productDto)
        {
            _logger.LogInformation("Adding product with SKU: {SKU}", productDto.SKU);

            var existing = await _uow.Products.Query()
                .FirstOrDefaultAsync(p => p.SKU == productDto.SKU);
            if (existing != null)
            {
                _logger.LogWarning("Product with SKU '{SKU}' already exists", productDto.SKU);
                throw new ConflictException($"Product with SKU '{productDto.SKU}' already exists.");
            }

            var brand = await _uow.Brands.GetByIdAsync(productDto.BrandId);
            if (brand == null)
            {
                _logger.LogWarning("Brand with Id {BrandId} does not exist", productDto.BrandId);
                throw new ValidationCustomException($"Brand with id {productDto.BrandId} does not exist.");
            }

            var product = _mapper.Map<Product>(productDto);
            await _uow.Products.AddAsync(product);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("Product with SKU '{SKU}' added successfully", product.SKU);
        }

        public async Task UpdateProductAsync(int id, ProductDto productDto)
        {
            _logger.LogInformation("Updating product Id: {Id}", id);

            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with Id {Id} not found", id);
                throw new EntityNotFoundException($"Product with id {id} not found.");
            }

            _mapper.Map(productDto, product);

            if (productDto.BrandId > 0)
            {
                var brand = await _uow.Brands.GetByIdAsync(productDto.BrandId);
                product.Brand = brand;
            }

            _uow.Products.Update(product);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("Product Id {Id} updated successfully", id);
        }

        public async Task DeleteProductAsync(int id)
        {
            _logger.LogInformation("Deleting product Id: {Id}", id);

            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with Id {Id} not found", id);
                throw new EntityNotFoundException($"Product with id {id} not found.");
            }

            var hasRelations = await _uow.ProductCategories.Query()
                .AnyAsync(pc => pc.ProductId == id);
            if (hasRelations)
            {
                _logger.LogWarning("Product Id {Id} cannot be deleted, linked categories exist", id);
                throw new ConflictException($"Product with id {id} cannot be deleted because it has linked categories.");
            }

            _uow.Products.Delete(product);
            await _uow.SaveChangesAsync();
            _logger.LogInformation("Product Id {Id} deleted successfully", id);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByBrandIdAsync(int brandId)
        {
            _logger.LogInformation("Fetching products by BrandId: {BrandId}", brandId);
            var products = await _uow.Products.GetProductsByBrandIdAsync(brandId);
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            _logger.LogInformation("Fetched {Count} products for BrandId {BrandId}", dtos.Count(), brandId);
            return dtos;
        }

        public async Task<PagedResultDto<ProductDto>> GetPagedProductsAsync(
            int page = 1, int pageSize = 20,
            int? brandId = null, int? categoryId = null,
            string? sortBy = null, string sortDir = "asc")
        {
            _logger.LogInformation("Fetching paged products: Page {Page}, PageSize {PageSize}", page, pageSize);

            pageSize = Math.Min(pageSize, 100);

            var spec = new ProductFilterSpecification(
                brandId, categoryId, sortBy, sortDir,
                skip: (page - 1) * pageSize, take: pageSize);

            var countSpec = new ProductFilterSpecification(brandId, categoryId);
            var totalCount = await _uow.Products.CountAsync(countSpec);

            var products = await _uow.Products.ListAsync(spec);
            var dtos = _mapper.Map<List<ProductDto>>(products);

            _logger.LogInformation("Paged products fetched successfully: TotalCount {TotalCount}", totalCount);
            return new PagedResultDto<ProductDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
