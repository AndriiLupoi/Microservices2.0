using Common.DTO_s;
using Orders.Bll.Exceptions;
using Orders.Bll.Interfaces;
using Orders.Bll.Mappers;
using Orders.Dal.Repo.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace Orders.Bll.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CustomersDTO>> GetAllAsync()
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();
            return customers.Select(CustomersMapper.ToDto);
        }

        public async Task<CustomersDTO?> GetByIdAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
                throw new NotFoundException($"Customer with ID={id} not found.");
            return CustomersMapper.ToDto(customer);
        }

        public async Task AddAsync(CustomersDTO dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (await _unitOfWork.Customers.ExistsByEmailAsync(dto.Email))
                    throw new BusinessConflictException($"Customer with email '{dto.Email}' already exists.");

                var entity = CustomersMapper.ToEntity(dto);
                await _unitOfWork.Customers.AddAsync(entity);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(CustomersDTO dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.Customers.GetByIdAsync(dto.CustomerId);
                if (existing == null)
                    throw new NotFoundException($"Customer with ID={dto.CustomerId} not found.");

                if (existing.Email != dto.Email && await _unitOfWork.Customers.ExistsByEmailAsync(dto.Email))
                    throw new BusinessConflictException($"Another customer with email '{dto.Email}' already exists.");

                var entity = CustomersMapper.ToEntity(dto);
                await _unitOfWork.Customers.UpdateAsync(entity);
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
                var existing = await _unitOfWork.Customers.GetByIdAsync(id);
                if (existing == null)
                    throw new NotFoundException($"Customer with ID={id} not found.");

                await _unitOfWork.Customers.DeleteAsync(id);
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
