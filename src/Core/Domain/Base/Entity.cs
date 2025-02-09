using Dawn;

namespace Core.Domain.Base;

public abstract class Entity
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; private set; }

    protected Entity()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public void SetUpdatedAt(DateTime? updatedAt = default)
    {
        UpdatedAt = updatedAt ?? DateTime.UtcNow;
    }

    public void SetId(Guid id)
    {
        Guard.Argument(id, nameof(id)).NotDefault();

        Id = id;
    }
}