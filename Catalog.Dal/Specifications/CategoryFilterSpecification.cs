using Ardalis.Specification;
using Catalog.Domain.Entity;

namespace Catalog.Dal.Specifications
{
    public class CategoryFilterSpecification : Specification<Category>
    {
        public CategoryFilterSpecification(
            string? search = null,
            string? sortBy = null,
            string sortDir = "asc",
            int skip = 0,
            int take = 20)

        {
            Query.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
                Query.Where(c => c.Name.Contains(search));

            if (!string.IsNullOrEmpty(sortBy) && sortBy.ToLower() != "null")
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        if (sortDir.Equals("asc", StringComparison.OrdinalIgnoreCase))
                            Query.OrderBy(c => c.Name);
                        else
                            Query.OrderByDescending(c => c.Name);
                        break;

                    default:
                        Query.OrderBy(c => c.CategoryId);
                        break;
                }
            }
            else
            {
                Query.OrderBy(c => c.CategoryId);
            }

            Query.Skip(skip).Take(take);
        }
    }
}
