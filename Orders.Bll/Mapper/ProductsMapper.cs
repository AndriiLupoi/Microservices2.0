using Common.DTO_s;
using Orders.Domain.Entity;

namespace Orders.Bll.Mappers
{
    public static class ProductsMapper
    {
        public static ProductsDTO ToDto(Products product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            return new ProductsDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }

        public static Products ToEntity(ProductsDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Products
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price
            };
        }
    }
}
