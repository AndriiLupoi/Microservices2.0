using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Common.DTO.ProductCategoryDto_s
{
    public class ProductCategoryCreateDto
    {

        [Required(ErrorMessage = "ProductId обов'язковий")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId має бути більшим за 0")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "CategoryId обов'язковий")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId має бути більшим за 0")]
        public int CategoryId { get; set; }
    }
}
