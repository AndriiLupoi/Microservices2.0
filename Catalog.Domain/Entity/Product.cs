using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entity
{
    public class Product
    {
        // PK
        public int ProductId { get; set; }

        // Унікальний SKU (NOT NULL, MAX 50)
        public string SKU { get; set; } = string.Empty;

        // NOT NULL, MAX 200
        public string Name { get; set; } = string.Empty;

        // Ціна (NOT NULL, з точністю 18,2)
        public decimal Price { get; set; }

        // FK → Brand
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        // Навігаційні властивості
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
