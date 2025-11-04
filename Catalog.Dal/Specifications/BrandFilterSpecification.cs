using Ardalis.Specification;
using Catalog.Domain.Entity;

namespace Catalog.Dal.Specifications
{
    public class BrandFilterSpecification : Specification<Brand>
    {
        public BrandFilterSpecification(
            string? search = null,
            string? sortBy = null,
            string sortDir = "asc",
            int skip = 0,
            int take = 20)
        {
            Query.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                Query.Where(b => b.Name.Contains(search));

            // 🔹 Сортування
            if (!string.IsNullOrEmpty(sortBy) && sortBy.ToLower() != "null")
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        if (sortDir.Equals("asc", StringComparison.OrdinalIgnoreCase))
                            Query.OrderBy(b => b.Name);
                        else
                            Query.OrderByDescending(b => b.Name);
                        break;

                    default:
                        Query.OrderBy(b => b.BrandId);
                        break;
                }
            }
            else
            {
                Query.OrderBy(b => b.BrandId);
            }

            if (take > 0)
                Query.Skip(Math.Max(skip, 0)).Take(take);
        }
    }
}
