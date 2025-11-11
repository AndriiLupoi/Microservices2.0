using System.Collections.Generic;
using Rewiews.Domain.Common;
using Rewiews.Domain.ValueObjects;

namespace Rewiews.Domain.Entities;

public class Product : BaseEntity
{

    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Description { get; set; }
    public Money price { get; set; }

    private readonly List<Review> _reviews = new();
    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

    public void AddReview(Review review)
    {
        _reviews.Add(review);
        Touch();
    }

    public void UpdatePrice(Money newPrice)
    {
        price = newPrice;
        Touch();
    }

    public void UpdateName(string newName)
    {
        Name = newName;
        Touch();
    }

    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
        Touch();
    }
}
