using AutoMapper;
using MongoDB.Bson;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.CreateTodo;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.UptadeTodo;
using Rewiews.Application.TodoReviews.Commands.ReviewsCommands.CreateReviews;
using Rewiews.Application.TodoReviews.Commands.ReviewsCommands.UptadeReviews;
using Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.CreateUser;
using Rewiews.Application.TodoUserProfile.Commands.UserProfileCommands.UptadeUser;
using Rewiews.Domain.Entities;
using Rewiews.Domain.ValueObjects;

namespace Rewiews.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ============================================================
            // PRODUCT MAPPINGS
            // ============================================================

            // Product <-> ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.price.Currency))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews))
                .ForSourceMember(src => src.Version, opt => opt.DoNotValidate())
                .ReverseMap()
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => new Money(src.Price, src.Currency)))
                .ForMember(dest => dest.Version, opt => opt.Ignore());

            // CreateProductCommand -> Product
            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id генерується в Handler через IIdGenerator
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => new Money(src.Price, src.Currency)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore());

            // UpdateProductCommand -> Product
            CreateMap<UpdateProductCommand, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.price, opt => opt.Condition(src => src.Price.HasValue))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src =>
                    new Money(src.Price ?? 0, src.Currency ?? "USD")))
                .ForMember(dest => dest.Name, opt => opt.Condition((src, _) => src.Name != null))
                .ForMember(dest => dest.Description, opt => opt.Condition((src, _) => src.Description != null))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore());

            // ============================================================
            // REVIEW MAPPINGS
            // ============================================================

            // Review <-> ReviewDto
            CreateMap<Review, ReviewDto>().ReverseMap();

            // CreateReviewCommand -> Review
            CreateMap<CreateReviewCommand, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id генерується в Handler
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore());

            // UpdateReviewCommand -> Review
            CreateMap<UpdateReviewCommand, Review>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore()) // ProductId не змінюється
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // UserId не змінюється
                .ForMember(dest => dest.Rating, opt => opt.Condition(src => src.Rating.HasValue))
                .ForMember(dest => dest.Comment, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Comment)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore());

            // ============================================================
            // USERPROFILE MAPPINGS
            // ============================================================

            // UserProfile <-> UserProfileDto
            CreateMap<UserProfile, UserProfileDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email.Value))
                .ReverseMap()
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => new Email(src.Email)));

            // CreateUserProfileCommand -> UserProfile
            CreateMap<CreateUserProfileCommand, UserProfile>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id генерується в Handler
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore());

            // UpdateUserProfileCommand -> UserProfile
            CreateMap<UpdateUserProfileCommand, UserProfile>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Username, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Username)))
                .ForMember(dest => dest.email, opt => opt.Condition(src => src.Email != null))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore());

            // ============================================================
            // VALUE OBJECTS & UTILITIES
            // ============================================================

            // ObjectId conversions
            CreateMap<ObjectId, string>().ConvertUsing(oid => oid.ToString());
            CreateMap<string, ObjectId>().ConvertUsing(s => ParseObjectId(s));
        }

        private static ObjectId ParseObjectId(string s)
        {
            return ObjectId.TryParse(s, out var oid) ? oid : ObjectId.Empty;
        }
    }
}