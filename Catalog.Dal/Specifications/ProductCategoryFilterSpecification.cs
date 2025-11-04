using Ardalis.Specification;
using Catalog.Domain.Entity;
using System;

namespace Catalog.Dal.Specifications
{
    public class ProductCategoryFilterSpecification : Specification<ProductCategory>
    {
        public ProductCategoryFilterSpecification(
            int? productId = null,
            int? categoryId = null,
            string? sortBy = null,
            string sortDir = "asc",
            int skip = 0,
            int take = 50)
        {
            Query.AsNoTracking()
                 .Include(pc => pc.Product)
                 .Include(pc => pc.Category);

            if (productId.HasValue)
                Query.Where(pc => pc.ProductId == productId.Value);

            if (categoryId.HasValue)
                Query.Where(pc => pc.CategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(sortBy) && sortBy.ToLower() != "null")
            {
                switch (sortBy.Trim().ToLower())
                {
                    case "productid":
                        if (sortDir.Equals("desc", StringComparison.OrdinalIgnoreCase))
                            Query.OrderByDescending(pc => pc.ProductId);
                        else
                            Query.OrderBy(pc => pc.ProductId);
                        break;

                    case "categoryid":
                        if (sortDir.Equals("desc", StringComparison.OrdinalIgnoreCase))
                            Query.OrderByDescending(pc => pc.CategoryId);
                        else
                            Query.OrderBy(pc => pc.CategoryId);
                        break;

                    default:
                        Query.OrderBy(pc => pc.Id);
                        break;
                }
            }
            else
            {
                Query.OrderBy(pc => pc.Id);
            }

            if (take > 0)
                Query.Skip(Math.Max(skip, 0)).Take(take);
        }
    }
}
