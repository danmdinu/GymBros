namespace WorkoutApp.Domain.Entities;

public class Workout : BaseEntity
{
    public int DayNumber { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    
    public Guid WorkoutPlanId { get; private set; }
    
    private readonly List<WorkoutSection> _sections = new();
    public IReadOnlyCollection<WorkoutSection> Sections => _sections.AsReadOnly();

    private Workout() { }

    public Workout(int dayNumber, string name, string? description = null)
    {
        DayNumber = dayNumber;
        Name = name;
        Description = description;
    }

    public void AddSection(WorkoutSection section)
    {
        _sections.Add(section);
    }
}
