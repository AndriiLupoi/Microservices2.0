using AutoMapper;
using Rewiews.Domain.Entities;


namespace Reviews.Application.Common.Models;

public class LookupDto
{
    public int Id { get; init; }

    public string? Title { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Product, LookupDto>();
            CreateMap<UserProfile, LookupDto>();
            CreateMap<Review, LookupDto>();
        }
    }
}
