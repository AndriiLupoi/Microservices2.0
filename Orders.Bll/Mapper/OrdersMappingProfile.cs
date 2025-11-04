using AutoMapper;
using Common.DTO_s;
using Orders.Domain.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Orders.Bll.Mappers
{
    public class OrdersMappingProfile : Profile
    {
        public OrdersMappingProfile()
        {
            CreateMap<Customers, CustomersDTO>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId));

            CreateMap<OrderItems, OrderItemsDTO>()
                .ReverseMap();

            CreateMap<Order, OrdersDTO>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.OrderDate));

            CreateMap<Products, ProductsDTO>()
                .ReverseMap();
        }
    }
}
