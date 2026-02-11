using WorkoutApp.Domain.ValueObjects;

namespace WorkoutApp.Domain.Entities;

public class WorkoutExercise : BaseEntity
{
    public int Order { get; private set; }
    public string? SupersetGroup { get; private set; }
    public int Sets { get; private set; }
    public ExerciseMetric Metric { get; private set; } = null!;
    public string? Notes { get; private set; }
    
    public Guid WorkoutSectionId { get; private set; }
    public Guid ExerciseId { get; private set; }
    public Exercise Exercise { get; private set; } = null!;

    private WorkoutExercise() { }

    public WorkoutExercise(
        int order,
        Guid exerciseId,
        int sets,
        ExerciseMetric metric,
        string? supersetGroup = null,
        string? notes = null)
    {
        Order = order;
        ExerciseId = exerciseId;
        Sets = sets;
        Metric = metric;
        SupersetGroup = supersetGroup;
        Notes = notes;
    }
}
