namespace Aggregator.API.DTOs.ProductWithReviews
{

    public record ReviewDetailsDto
    {
        public string ReviewId { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public int Rating { get; init; }
        public string Comment { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
