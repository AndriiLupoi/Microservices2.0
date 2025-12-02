namespace Aggregator.API.DTOs.OrderFullDetails
{
    public record OrderFullDetailsDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public CustomerDetailsDto Customer { get; init; } = null!;
        public List<OrderItemDetailsDto> Items { get; init; } = new();
    }
}
