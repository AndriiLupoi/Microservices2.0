using Aggregator.API.DTOs.OrderFullDetails;

namespace Aggregator.API.DTOs.ProductWithReviews
{
    public record ProductWithReviewsDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; init; } = string.Empty;
        public string SKU { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public BrandDetailsDto Brand { get; set; } = null!;
        public ReviewStatisticsDto ReviewStatistics { get; init; } = null!;
        public List<ReviewDetailsDto> Reviews { get; init; } = new();
    }
}
