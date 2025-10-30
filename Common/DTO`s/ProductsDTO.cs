using System.ComponentModel.DataAnnotations;

namespace Common.DTO_s
{
    public class ProductsDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative.")]
        public decimal Price { get; set; }
    }
}
