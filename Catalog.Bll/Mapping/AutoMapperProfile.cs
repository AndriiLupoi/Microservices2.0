using AutoMapper;
using Catalog.Common.DTO;
using Catalog.Common.DTO.CategoryDto_s;
using Catalog.Common.DTO.ProductCategoryDto_s;
using Catalog.Common.DTO.ProductDto_s;
using Catalog.Domain.Entity;
using System.Linq;

namespace Catalog.Bll.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Brand, BrandCreateDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryCreateDto>().ReverseMap();
            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.BrandId,
                           opt => opt.MapFrom(src => src.BrandId));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());

            CreateMap<Product, ProductCreateDto>()
                .ForMember(dest => dest.BrandId,
                           opt => opt.MapFrom(src => src.BrandId));

            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());
        }
    }
}
