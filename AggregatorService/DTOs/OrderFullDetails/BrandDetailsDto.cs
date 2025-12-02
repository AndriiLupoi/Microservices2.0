namespace Aggregator.API.DTOs.OrderFullDetails
{
    public record BrandDetailsDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; init; } = string.Empty;
    }
}
