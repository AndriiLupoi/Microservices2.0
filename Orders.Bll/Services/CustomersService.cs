using AutoMapper;
using Common.DTO_s;
using Microsoft.Extensions.Logging;
using Orders.Bll.Exceptions;
using Orders.Bll.Interfaces;
using Orders.Dal.Repo.Interfaces;

namespace Orders.Bll.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomersService> _logger;

        public CustomersService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CustomersService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CustomersDTO>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all customers");
            var customers = await _unitOfWork.Customers.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<CustomersDTO>>(customers);
            _logger.LogInformation("Fetched {Count} customers", dtos.Count());
            return dtos;
        }

        public async Task<CustomersDTO?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching customer by Id: {Id}", id);
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Customer with Id {Id} not found", id);
                throw new NotFoundException($"Customer with ID={id} not found.");
            }

            var dto = _mapper.Map<CustomersDTO>(customer);
            _logger.LogInformation("Customer with Id {Id} retrieved successfully", id);
            return dto;
        }

        public async Task AddAsync(CustomersDTO dto)
        {
            _logger.LogInformation("Adding customer with Email: {Email}", dto.Email);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (await _unitOfWork.Customers.ExistsByEmailAsync(dto.Email))
                {
                    _logger.LogWarning("Customer with Email '{Email}' already exists", dto.Email);
                    throw new BusinessConflictException($"Customer with email '{dto.Email}' already exists.");
                }

                var entity = _mapper.Map<Domain.Entity.Customers>(dto);
                await _unitOfWork.Customers.AddAsync(entity);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Customer with Email '{Email}' added successfully", dto.Email);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("Error occurred while adding customer with Email '{Email}'", dto.Email);
                throw;
            }
        }

        public async Task UpdateAsync(CustomersDTO dto)
        {
            _logger.LogInformation("Updating customer Id: {Id}", dto.CustomerId);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.Customers.GetByIdAsync(dto.CustomerId);
                if (existing == null)
                {
                    _logger.LogWarning("Customer with Id {Id} not found", dto.CustomerId);
                    throw new NotFoundException($"Customer with ID={dto.CustomerId} not found.");
                }

                if (existing.Email != dto.Email && await _unitOfWork.Customers.ExistsByEmailAsync(dto.Email))
                {
                    _logger.LogWarning("Another customer with Email '{Email}' already exists", dto.Email);
                    throw new BusinessConflictException($"Another customer with email '{dto.Email}' already exists.");
                }

                var entity = _mapper.Map<Domain.Entity.Customers>(dto);
                await _unitOfWork.Customers.UpdateAsync(entity);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Customer Id {Id} updated successfully", dto.CustomerId);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("Error occurred while updating customer Id {Id}", dto.CustomerId);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting customer Id: {Id}", id);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.Customers.GetByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Customer with Id {Id} not found", id);
                    throw new NotFoundException($"Customer with ID={id} not found.");
                }

                await _unitOfWork.Customers.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Customer Id {Id} deleted successfully", id);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError("Error occurred while deleting customer Id {Id}", id);
                throw;
            }
        }
    }
}
