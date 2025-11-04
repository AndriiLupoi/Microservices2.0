using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Catalog.Domain.Entity;

namespace Catalog.Dal.EntityConfig
{
    public class BrandConfig : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");

            builder.HasKey(b => b.BrandId);

            builder.Property(b => b.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(b => b.Name)
                   .IsUnique();

            //builder.HasData(
            //    new Brand { BrandId = 1, Name = "Bosch" },
            //    new Brand { BrandId = 2, Name = "Valeo" },
            //    new Brand { BrandId = 3, Name = "NGK" }
            //);
        }
    }
}
