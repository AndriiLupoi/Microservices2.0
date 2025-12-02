namespace Aggregator.API.DTOs.OrderFullDetails
{
    public class ProductFromOrderDbDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

}
