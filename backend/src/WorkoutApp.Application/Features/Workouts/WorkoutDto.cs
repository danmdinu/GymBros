using WorkoutApp.Domain.ValueObjects;

namespace WorkoutApp.Application.Features.Workouts;

public record WorkoutDto(
    Guid Id,
    int DayNumber,
    string Name,
    string? Description,
    List<WorkoutSectionDto> Sections
);

public record WorkoutSectionDto(
    Guid Id,
    int Order,
    string Type,
    string Name,
    List<WorkoutExerciseDto> Exercises
);

public record WorkoutExerciseDto(
    Guid Id,
    int Order,
    string? SupersetGroup,
    int Sets,
    ExerciseMetric Metric,
    string? Notes,
    string ExerciseName,
    string? ExerciseDescription
);
