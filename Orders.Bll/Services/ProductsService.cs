using AutoMapper;
using Common.DTO_s;
using Orders.Bll.Exceptions;
using Orders.Bll.Interfaces;
using Orders.Dal.Repo.Interfaces;
using Orders.Domain.Entity;

namespace Orders.Bll.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductsDTO>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductsDTO>>(products);
        }

        public async Task<ProductsDTO?> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException($"Product with ID={id} not found.");

            return _mapper.Map<ProductsDTO>(product);
        }

        public async Task AddAsync(ProductsDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("Product name cannot be empty.");
            if (dto.Price < 0)
                throw new ValidationException("Price cannot be negative.");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<Products>(dto);
                await _unitOfWork.Products.AddAsync(entity);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(ProductsDTO dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existing = await _unitOfWork.Products.GetByIdAsync(dto.Id);
                if (existing == null)
                    throw new NotFoundException($"Product with ID={dto.Id} not found.");

                var entity = _mapper.Map<Products>(dto);
                await _unitOfWork.Products.UpdateAsync(entity);
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
                var existing = await _unitOfWork.Products.GetByIdAsync(id);
                if (existing == null)
                    throw new NotFoundException($"Product with ID={id} not found.");

                await _unitOfWork.Products.DeleteAsync(id);
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
