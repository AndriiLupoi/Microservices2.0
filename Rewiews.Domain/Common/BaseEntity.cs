namespace Rewiews.Domain.Common;

public abstract class BaseEntity
{
    public string? Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int Version { get; protected set; } = 0;

    protected void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
        Version++;
    }
}
