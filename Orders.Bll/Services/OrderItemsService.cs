using AutoMapper;
using Common.DTO_s;
using Microsoft.Extensions.Logging;
using Orders.Bll.Exceptions;
using Orders.Bll.Interfaces;
using Orders.Dal.Repo.Interfaces;
using Orders.Domain.Entity;
using System.Linq;

namespace Orders.Bll.Services
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderItemsService> _logger;

        public OrderItemsService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderItemsService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderItemsDTO>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all order items");
            var items = await _unitOfWork.OrderItems.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<OrderItemsDTO>>(items);
            _logger.LogInformation("Fetched {Count} order items", dtos.Count());
            return dtos;
        }

        public async Task<OrderItemsDTO?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching order item by Id: {Id}", id);
            var item = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (item == null)
            {
                _logger.LogWarning("OrderItem with Id {Id} not found", id);
                throw new NotFoundException($"OrderItem with ID={id} not found.");
            }

            var dto = _mapper.Map<OrderItemsDTO>(item);
            _logger.LogInformation("OrderItem with Id {Id} retrieved successfully", id);
            return dto;
        }

        public async Task AddAsync(OrderItemsDTO dto)
        {
            _logger.LogInformation("Adding order item for OrderId {OrderId}, ProductId {ProductId}", dto.OrderId, dto.ProductId);

            if (dto.Quantity <= 0)
            {
                _logger.LogWarning("Invalid Quantity {Quantity} for OrderId {OrderId}", dto.Quantity, dto.OrderId);
                throw new ValidationException("Quantity must be greater than zero.");
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<OrderItems>(dto);
                await _unitOfWork.OrderItems.AddAsync(entity);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("OrderItem added successfully: Id {OrderId}", dto.OrderId);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("Error occurred while adding OrderItem for OrderId {OrderId}", dto.OrderId);
                throw;
            }
        }

        public async Task UpdateAsync(OrderItemsDTO dto)
        {
            _logger.LogInformation("Updating OrderItem Id: {OrderId}", dto.OrderId);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.OrderItems.GetByIdAsync(dto.OrderId);
                if (existing == null)
                {
                    _logger.LogWarning("OrderItem with Id {OrderId} not found", dto.OrderId);
                    throw new NotFoundException($"OrderItem with ID={dto.OrderId} not found.");
                }

                var entity = _mapper.Map<OrderItems>(dto);
                await _unitOfWork.OrderItems.UpdateAsync(entity);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("OrderItem Id {OrderId} updated successfully", dto.OrderId);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("Error occurred while updating OrderItem Id {OrderId}", dto.OrderId);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting OrderItem Id: {Id}", id);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.OrderItems.GetByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("OrderItem with Id {Id} not found", id);
                    throw new NotFoundException($"OrderItem with ID={id} not found.");
                }

                await _unitOfWork.OrderItems.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("OrderItem Id {Id} deleted successfully", id);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("Error occurred while deleting OrderItem Id {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<OrderItemsDTO>> GetByOrderIdAsync(int orderId)
        {
            _logger.LogInformation("Fetching order items by OrderId: {OrderId}", orderId);
            var items = await _unitOfWork.OrderItems.GetByOrderIdAsync(orderId);
            var dtos = items.Select(i => new OrderItemsDTO
            {
                OrderId = i.OrderId,
                ProductId = i.ProductId,
                Quantity = i.Quantity
            });
            _logger.LogInformation("Fetched {Count} order items for OrderId {OrderId}", dtos.Count(), orderId);
            return dtos;
        }
    }
}
