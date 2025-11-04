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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _uow.Products.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null)
                throw new EntityNotFoundException($"Product with id {id} not found.");

            return _mapper.Map<ProductDto>(product);
        }

        public async Task AddProductAsync(ProductDto productDto)
        {
            var existing = await _uow.Products.Query()
                .FirstOrDefaultAsync(p => p.SKU == productDto.SKU);

            if (existing != null)
                throw new ConflictException($"Product with SKU '{productDto.SKU}' already exists.");

            var brand = await _uow.Brands.GetByIdAsync(productDto.BrandId);
            if (brand == null)
                throw new ValidationCustomException($"Brand with id {productDto.BrandId} does not exist.");

            var product = _mapper.Map<Product>(productDto);
            await _uow.Products.AddAsync(product);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(int id, ProductDto productDto)
        {
            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null)
                throw new EntityNotFoundException($"Product with id {id} not found.");

            _mapper.Map(productDto, product);

            if (productDto.BrandId > 0)
            {
                var brand = await _uow.Brands.GetByIdAsync(productDto.BrandId);
                product.Brand = brand;
            }

            _uow.Products.Update(product);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _uow.Products.GetByIdAsync(id);
            if (product == null)
                throw new EntityNotFoundException($"Product with id {id} not found.");

            var hasRelations = await _uow.ProductCategories.Query()
                .AnyAsync(pc => pc.ProductId == id);

            if (hasRelations)
                throw new ConflictException($"Product with id {id} cannot be deleted because it has linked categories.");

            _uow.Products.Delete(product);
            await _uow.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByBrandIdAsync(int brandId)
        {
            var products = await _uow.Products.GetProductsByBrandIdAsync(brandId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<PagedResultDto<ProductDto>> GetPagedProductsAsync(
            int page = 1, int pageSize = 20,
            int? brandId = null, int? categoryId = null,
            string? sortBy = null, string sortDir = "asc")
        {
            pageSize = Math.Min(pageSize, 100);

            var spec = new ProductFilterSpecification(
                brandId, categoryId, sortBy, sortDir,
                skip: (page - 1) * pageSize, take: pageSize);

            var countSpec = new ProductFilterSpecification(brandId, categoryId); // без пагінації
            var totalCount = await _uow.Products.CountAsync(countSpec);

            var products = await _uow.Products.ListAsync(spec);

            return new PagedResultDto<ProductDto>
            {
                Items = _mapper.Map<List<ProductDto>>(products),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }


    }
}
