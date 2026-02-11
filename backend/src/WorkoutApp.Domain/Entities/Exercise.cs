namespace WorkoutApp.Domain.Entities;

public class Exercise : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    // EF Core needs a parameterless constructor (can be private)
    private Exercise() { }

    public Exercise(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
