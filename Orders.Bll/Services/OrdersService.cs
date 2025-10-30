using Common.DTO_s;
using Orders.Bll.Exceptions;
using Orders.Bll.Interfaces;
using Orders.Bll.Mappers;
using Orders.Dal.Repo.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace Orders.Bll.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrdersDTO>> GetAllAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return orders.Select(OrdersMapper.ToDto);
        }

        public async Task<OrdersDTO?> GetByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                throw new NotFoundException($"Order with ID={id} not found.");
            return OrdersMapper.ToDto(order);
        }

        public async Task AddAsync(OrdersDTO dto)
        {
            if (dto.CustomerId <= 0)
                throw new ValidationException("Invalid CustomerId.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = OrdersMapper.ToEntity(dto);
                await _unitOfWork.Orders.AddAsync(entity);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(OrdersDTO dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.Orders.GetByIdAsync(dto.OrderId);
                if (existing == null)
                    throw new NotFoundException($"Order with ID={dto.OrderId} not found.");

                var entity = OrdersMapper.ToEntity(dto);
                await _unitOfWork.Orders.UpdateAsync(entity);
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
                var existing = await _unitOfWork.Orders.GetByIdAsync(id);
                if (existing == null)
                    throw new NotFoundException($"Order with ID={id} not found.");

                await _unitOfWork.Orders.DeleteAsync(id);
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
