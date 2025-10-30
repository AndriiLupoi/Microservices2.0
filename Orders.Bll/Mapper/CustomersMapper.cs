using Common.DTO_s;
using Orders.Domain.Entity;

namespace Orders.Bll.Mappers
{
    public static class CustomersMapper
    {
        public static CustomersDTO ToDto(Customers customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            return new CustomersDTO
            {
                CustomerId = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            };
        }

        public static Customers ToEntity(CustomersDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Customers
            {
                Id = dto.CustomerId,
                Name = dto.Name,
                Email = dto.Email
            };
        }
    }
}
