using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Dal.EntityConfig
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.SKU)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.HasIndex(p => p.SKU).IsUnique();

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(p => p.Brand)
                   .WithMany(b => b.Products)
                   .HasForeignKey(p => p.BrandId)
                   .OnDelete(DeleteBehavior.Cascade);

            //builder.HasData(
            //    new ProductCategory { Id = 1, ProductId = 1, CategoryId = 1 }, // Свічки Bosch → Свічки запалювання
            //    new ProductCategory { Id = 2, ProductId = 2, CategoryId = 2 }, // Фільтр Valeo → Фільтри
            //    new ProductCategory { Id = 3, ProductId = 3, CategoryId = 3 }  // Колодки NGK → Гальмівні колодки
            //);
        }
    }
}
