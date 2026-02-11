using MediatR;

namespace WorkoutApp.Application.Features.Workouts.Queries.GetWorkoutByDay;

public record GetWorkoutByDayQuery(int DayNumber) : IRequest<WorkoutDto?>;
