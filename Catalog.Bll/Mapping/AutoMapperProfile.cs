using AutoMapper;
using Catalog.Common.DTO;
using Catalog.Domain.Entity;
using System.Linq;

namespace Catalog.Bll.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.BrandId,
                           opt => opt.MapFrom(src => src.BrandId));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());
        }
    }
}
