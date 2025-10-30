using System.ComponentModel.DataAnnotations;

namespace Common.DTO_s
{
    public class OrdersDTO
    {
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;
    }
}
