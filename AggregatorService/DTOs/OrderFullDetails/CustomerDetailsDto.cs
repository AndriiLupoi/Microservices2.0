namespace Aggregator.API.DTOs.OrderFullDetails
{
    public record CustomerDetailsDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

}
