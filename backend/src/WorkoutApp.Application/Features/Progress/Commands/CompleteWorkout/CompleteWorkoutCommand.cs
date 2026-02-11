using MediatR;

namespace WorkoutApp.Application.Features.Progress.Commands.CompleteWorkout;

public record CompleteWorkoutCommand(Guid UserId) : IRequest<ProgressDto?>;
