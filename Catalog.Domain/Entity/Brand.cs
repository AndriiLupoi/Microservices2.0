using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entity
{
    public class Brand
    {
        // PK
        public int BrandId { get; set; }

        // NOT NULL, UNIQUE, MAX 100 chars
        public string Name { get; set; } = string.Empty;

        // Навігаційна властивість
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
