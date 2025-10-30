using Common.DTO_s;
using Orders.Bll.Exceptions;
using Orders.Bll.Interfaces;
using Orders.Bll.Mappers;
using Orders.Dal.Repo.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace Orders.Bll.Services
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderItemsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrderItemsDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.OrderItems.GetAllAsync();
            return items.Select(OrderItemsMapper.ToDto);
        }

        public async Task<OrderItemsDTO?> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (item == null)
                throw new NotFoundException($"OrderItem with ID={id} not found.");
            return OrderItemsMapper.ToDto(item);
        }

        public async Task AddAsync(OrderItemsDTO dto)
        {
            if (dto.Quantity <= 0)
                throw new ValidationException("Quantity must be greater than zero.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = OrderItemsMapper.ToEntity(dto);
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

                var entity = OrderItemsMapper.ToEntity(dto);
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
