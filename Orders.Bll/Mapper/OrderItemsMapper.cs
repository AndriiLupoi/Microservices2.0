using Common.DTO_s;
using Orders.Domain.Entity;

namespace Orders.Bll.Mappers
{
    public static class OrderItemsMapper
    {
        public static OrderItemsDTO ToDto(OrderItems item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return new OrderItemsDTO
            {
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
        }

        public static OrderItems ToEntity(OrderItemsDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new OrderItems
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };
        }
    }
}
