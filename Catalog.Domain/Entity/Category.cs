using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entity
{
    public class Category
    {
        // PK
        public int CategoryId { get; set; }

        // NOT NULL, UNIQUE, MAX 100 chars
        public string Name { get; set; } = string.Empty;

        // Багато-багато через проміжну таблицю ProductCategory
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
