using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Common.DTO.ProductDto_s
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "SKU обов'язковий")]
        [StringLength(50, ErrorMessage = "SKU не може бути довшим за 50 символів")]
        public string SKU { get; set; } = string.Empty;

        [Required(ErrorMessage = "Назва обов'язкова")]
        [StringLength(200, ErrorMessage = "Назва не може бути довшою за 200 символів")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "BrandId обов'язковий")]
        public int BrandId { get; set; }
    }
}
