namespace WorkoutApp.Domain.Entities;

public class UserProgress : BaseEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    
    public int CurrentDayNumber { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? LastWorkoutCompletedAt { get; private set; }

    private UserProgress() { }

    public UserProgress(Guid userId, DateTime startedAt)
    {
        UserId = userId;
        CurrentDayNumber = 1;
        StartedAt = startedAt;
    }

    public void CompleteCurrentWorkout(DateTime completedAt)
    {
        LastWorkoutCompletedAt = completedAt;
        
        if (CurrentDayNumber >= 30)
        {
            CompletedAt = completedAt;
        }
        else
        {
            CurrentDayNumber++;
        }
    }

    public bool IsCompleted => CompletedAt.HasValue;
}
