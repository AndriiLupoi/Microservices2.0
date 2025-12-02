namespace Aggregator.API.DTOs.OrderFullDetails
{
    public record OrderItemDetailsDto
    {
        public int ProductId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public string SKU { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public decimal Price { get; init; }
        public BrandDetailsDto Brand { get; init; } = null!;
    }
}
