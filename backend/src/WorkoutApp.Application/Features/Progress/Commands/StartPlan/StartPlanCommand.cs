using MediatR;

namespace WorkoutApp.Application.Features.Progress.Commands.StartPlan;

public record StartPlanCommand(Guid UserId) : IRequest<ProgressDto>;
