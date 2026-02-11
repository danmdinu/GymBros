namespace WorkoutApp.Domain.Entities;

public class WorkoutPlan : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    
    private readonly List<Workout> _workouts = new();
    public IReadOnlyCollection<Workout> Workouts => _workouts.AsReadOnly();

    private WorkoutPlan() { }

    public WorkoutPlan(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }

    public void AddWorkout(Workout workout)
    {
        _workouts.Add(workout);
    }
}
