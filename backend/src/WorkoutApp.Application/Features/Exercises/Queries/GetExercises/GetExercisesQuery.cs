using MediatR;

namespace WorkoutApp.Application.Features.Exercises.Queries.GetExercises;

public record GetExercisesQuery : IRequest<List<ExerciseDto>>;
