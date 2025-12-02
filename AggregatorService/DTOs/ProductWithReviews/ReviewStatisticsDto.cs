namespace Aggregator.API.DTOs.ProductWithReviews
{
    public record ReviewStatisticsDto
    {
        public int TotalReviews { get; init; }
        public double AverageRating { get; init; }
    }
}
