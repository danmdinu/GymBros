using WorkoutApp.Domain.Enums;

namespace WorkoutApp.Domain.Entities;

public class WorkoutSection : BaseEntity
{
    public int Order { get; private set; }
    public SectionType Type { get; private set; }
    public string Name { get; private set; } = string.Empty;
    
    public Guid WorkoutId { get; private set; }
    
    private readonly List<WorkoutExercise> _exercises = new();
    public IReadOnlyCollection<WorkoutExercise> Exercises => _exercises.AsReadOnly();

    private WorkoutSection() { }

    public WorkoutSection(int order, SectionType type, string name)
    {
        Order = order;
        Type = type;
        Name = name;
    }

    public void AddExercise(WorkoutExercise exercise)
    {
        _exercises.Add(exercise);
    }
}
