using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Common.DTO.CategoryDto_s
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Назва категорії обов'язкова")]
        [MaxLength(100, ErrorMessage = "Назва не може бути довшою за 100 символів")]
        public string Name { get; set; } = string.Empty;
    }
}
