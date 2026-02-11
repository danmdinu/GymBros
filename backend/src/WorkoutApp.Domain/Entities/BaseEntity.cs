namespace WorkoutApp.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    public void SetCreatedAt(DateTime dateTime)
    {
        CreatedAt = dateTime;
    }

    public void SetUpdatedAt(DateTime dateTime)
    {
        UpdatedAt = dateTime;
    }
}
