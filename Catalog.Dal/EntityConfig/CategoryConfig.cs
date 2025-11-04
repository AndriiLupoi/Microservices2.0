using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Dal.EntityConfig
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(c => c.Name)
                   .IsUnique();

            //builder.HasData(
            //    new Category { CategoryId = 1, Name = "Свічки запалювання" },
            //    new Category { CategoryId = 2, Name = "Фільтри" },
            //    new Category { CategoryId = 3, Name = "Гальмівні колодки" }
            //);
        }
    }
}
