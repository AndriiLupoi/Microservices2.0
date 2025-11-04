using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Dal.EntityConfig
{
    public class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategories");

            builder.HasKey(pc => new { pc.ProductId, pc.CategoryId, pc.Id});

            builder.HasOne(pc => pc.Product)
                   .WithMany(p => p.ProductCategories)
                   .HasForeignKey(pc => pc.ProductId);

            builder.HasOne(pc => pc.Category)
                   .WithMany(c => c.ProductCategories)
                   .HasForeignKey(pc => pc.CategoryId);

            //builder.HasData(
            //    new Product { ProductId = 1, Name = "Свічка Bosch Super", SKU = "BOSCH-SPARK-001", Price = 150, BrandId = 1 },
            //    new Product { ProductId = 2, Name = "Фільтр повітря Valeo", SKU = "VALEO-AIR-001", Price = 200, BrandId = 2 },
            //    new Product { ProductId = 3, Name = "Колодки гальмівні NGK", SKU = "NGK-BRAKE-001", Price = 400, BrandId = 3 }
            //);
        }
    }
}
