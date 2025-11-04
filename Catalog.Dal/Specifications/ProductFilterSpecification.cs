using Ardalis.Specification;
using Catalog.Domain.Entity;

namespace Catalog.Dal.Specifications
{
    public class ProductFilterSpecification : Specification<Product>
    {
        public ProductFilterSpecification(
            int? brandId = null,
            int? categoryId = null,
            string? sortBy = null,
            string sortDir = "asc",
            int skip = 0,
            int take = 20)
        {
            Query.Include(p => p.Brand)
                 .Include(p => p.ProductCategories)
                     .ThenInclude(pc => pc.Category);

            if (brandId.HasValue)
                Query.Where(p => p.BrandId == brandId.Value);

            if (categoryId.HasValue)
                Query.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId.Value));

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        if (sortDir.Equals("asc", StringComparison.OrdinalIgnoreCase))
                            Query.OrderBy(p => p.Name);
                        else
                            Query.OrderByDescending(p => p.Name);
                        break;

                    case "price":
                        if (sortDir.Equals("asc", StringComparison.OrdinalIgnoreCase))
                            Query.OrderBy(p => p.Price);
                        else
                            Query.OrderByDescending(p => p.Price);
                        break;

                    default:
                        Query.OrderBy(p => p.ProductId);
                        break;
                }
            }
            else
            {
                Query.OrderBy(p => p.ProductId);
            }

            Query.Skip(skip).Take(take);
        }
    }
}
