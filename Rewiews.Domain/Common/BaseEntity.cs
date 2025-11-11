namespace Rewiews.Domain.Common;

public abstract class BaseEntity
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    protected void Touch() => UpdatedAt = DateTime.UtcNow;
}
