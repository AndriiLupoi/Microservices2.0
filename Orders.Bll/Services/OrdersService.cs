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
    public class OrdersService : IOrdersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersService> _logger;

        public OrdersService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrdersService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<OrdersDTO>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all orders");
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<OrdersDTO>>(orders);
            _logger.LogInformation("Fetched {Count} orders", dtos.Count());
            return dtos;
        }

        public async Task<OrdersDTO?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching order by Id: {Id}", id);
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order with Id {Id} not found", id);
                throw new NotFoundException($"Order with ID={id} not found.");
            }

            var dto = _mapper.Map<OrdersDTO>(order);
            _logger.LogInformation("Order with Id {Id} retrieved successfully", id);
            return dto;
        }

        public async Task AddAsync(OrdersDTO dto)
        {
            _logger.LogInformation("Adding order for CustomerId {CustomerId}", dto.CustomerId);
            if (dto.CustomerId <= 0)
            {
                _logger.LogWarning("Invalid CustomerId {CustomerId}", dto.CustomerId);
                throw new ValidationException("Invalid CustomerId.");
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<Order>(dto);
                await _unitOfWork.Orders.AddAsync(entity);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Order added successfully for CustomerId {CustomerId}", dto.CustomerId);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("Error occurred while adding order for CustomerId {CustomerId}", dto.CustomerId);
                throw;
            }
        }

        public async Task UpdateAsync(OrdersDTO dto)
        {
            _logger.LogInformation("Updating order Id: {OrderId}", dto.OrderId);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.Orders.GetByIdAsync(dto.OrderId);
                if (existing == null)
                {
                    _logger.LogWarning("Order with Id {OrderId} not found", dto.OrderId);
                    throw new NotFoundException($"Order with ID={dto.OrderId} not found.");
                }

                var entity = _mapper.Map<Order>(dto);
                await _unitOfWork.Orders.UpdateAsync(entity);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Order Id {OrderId} updated successfully", dto.OrderId);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("Error occurred while updating order Id {OrderId}", dto.OrderId);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting order Id: {Id}", id);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.Orders.GetByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Order with Id {Id} not found", id);
                    throw new NotFoundException($"Order with ID={id} not found.");
                }

                await _unitOfWork.Orders.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Order Id {Id} deleted successfully", id);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("Error occurred while deleting order Id {Id}", id);
                throw;
            }
        }
    }
}
