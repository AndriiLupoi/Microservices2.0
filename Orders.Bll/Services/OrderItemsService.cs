using AutoMapper;
using Common.DTO_s;
using Orders.Bll.Exceptions;
using Orders.Bll.Interfaces;
using Orders.Dal.Repo.Interfaces;
using Orders.Domain.Entity;

namespace Orders.Bll.Services
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderItemsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderItemsDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.OrderItems.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderItemsDTO>>(items);
        }

        public async Task<OrderItemsDTO?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (item == null)
                throw new NotFoundException($"OrderItem with ID={id} not found.");

            return _mapper.Map<OrderItemsDTO>(item);
        }

        public async Task AddAsync(OrderItemsDTO dto)
        {
            if (dto.Quantity <= 0)
                throw new ValidationException("Quantity must be greater than zero.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<OrderItems>(dto);
                await _unitOfWork.OrderItems.AddAsync(entity);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(OrderItemsDTO dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.OrderItems.GetByIdAsync(dto.OrderId);
                if (existing == null)
                    throw new NotFoundException($"OrderItem with ID={dto.OrderId} not found.");

                var entity = _mapper.Map<OrderItems>(dto);
                await _unitOfWork.OrderItems.UpdateAsync(entity);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.OrderItems.GetByIdAsync(id);
                if (existing == null)
                    throw new NotFoundException($"OrderItem with ID={id} not found.");

                await _unitOfWork.OrderItems.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
