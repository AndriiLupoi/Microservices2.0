using Common.DTO_s;
using Orders.Domain.Entity;

namespace Orders.Bll.Mappers
{
    public static class OrdersMapper
    {
        public static OrdersDTO ToDto(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            return new OrdersDTO
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.CreatedAt,
                Status = order.Status
            };
        }

        public static Order ToEntity(OrdersDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Order
            {
                Id = dto.OrderId,
                CustomerId = dto.CustomerId,
                CreatedAt = dto.OrderDate,
                Status = dto.Status
            };
        }
    }
}
