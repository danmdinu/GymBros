namespace WorkoutApp.Application.Features.Progress;

public record ProgressDto(
    Guid Id,
    Guid UserId,
    int CurrentDayNumber,
    DateTime StartedAt,
    DateTime? CompletedAt,
    DateTime? LastWorkoutCompletedAt,
    bool IsCompleted
);
