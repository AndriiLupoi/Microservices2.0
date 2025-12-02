using AutoMapper;
using Common.DTO_s;
using Microsoft.Extensions.Logging;
using Orders.Bll.Exceptions;
using Orders.Bll.Interfaces;
using Orders.Dal.Repo.Interfaces;
using Orders.Domain.Entity;
using System.Collections.Generic;

namespace Orders.Bll.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsService> _logger;

        public ProductsService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductsService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductsDTO>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all products");
            var products = await _unitOfWork.Products.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ProductsDTO>>(products);
            _logger.LogInformation("Fetched {Count} products", dtos.Count());
            return dtos;
        }

        public async Task<ProductsDTO?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching product by Id: {Id}", id);
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with Id {Id} not found", id);
                throw new NotFoundException($"Product with ID={id} not found.");
            }

            var dto = _mapper.Map<ProductsDTO>(product);
            _logger.LogInformation("Product with Id {Id} retrieved successfully", id);
            return dto;
        }

        public async Task AddAsync(ProductsDTO dto)
        {
            _logger.LogInformation("Adding product: {Name}", dto.Name);
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                _logger.LogWarning("Product name cannot be empty");
                throw new ValidationException("Product name cannot be empty.");
            }
            if (dto.Price < 0)
            {
                _logger.LogWarning("Invalid product price {Price}", dto.Price);
                throw new ValidationException("Price cannot be negative.");
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<Products>(dto);
                await _unitOfWork.Products.AddAsync(entity);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Product '{Name}' added successfully", dto.Name);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("Error occurred while adding product '{Name}'", dto.Name);
                throw;
            }
        }

        public async Task UpdateAsync(ProductsDTO dto)
        {
            _logger.LogInformation("Updating product Id: {Id}", dto.Id);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.Products.GetByIdAsync(dto.Id);
                if (existing == null)
                {
                    _logger.LogWarning("Product with Id {Id} not found", dto.Id);
                    throw new NotFoundException($"Product with ID={dto.Id} not found.");
                }

                var entity = _mapper.Map<Products>(dto);
                await _unitOfWork.Products.UpdateAsync(entity);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Product Id {Id} updated successfully", dto.Id);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("Error occurred while updating product Id {Id}", dto.Id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting product Id: {Id}", id);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.Products.GetByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Product with Id {Id} not found", id);
                    throw new NotFoundException($"Product with ID={id} not found.");
                }

                await _unitOfWork.Products.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Product Id {Id} deleted successfully", id);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("Error occurred while deleting product Id {Id}", id);
                throw;
            }
        }
    }
}
