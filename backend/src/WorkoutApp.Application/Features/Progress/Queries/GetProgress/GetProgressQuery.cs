using MediatR;

namespace WorkoutApp.Application.Features.Progress.Queries.GetProgress;

public record GetProgressQuery(Guid UserId) : IRequest<ProgressDto?>;
