namespace WorkoutApp.Application.Features.Exercises;

public record ExerciseDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt
);
