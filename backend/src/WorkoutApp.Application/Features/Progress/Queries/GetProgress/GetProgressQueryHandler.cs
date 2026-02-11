using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutApp.Application.Common.Interfaces;

namespace WorkoutApp.Application.Features.Progress.Queries.GetProgress;

public class GetProgressQueryHandler : IRequestHandler<GetProgressQuery, ProgressDto?>
{
    private readonly IAppDbContext _context;

    public GetProgressQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ProgressDto?> Handle(GetProgressQuery request, CancellationToken cancellationToken)
    {
        var progress = await _context.UserProgresses
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (progress is null)
            return null;

        return new ProgressDto(
            progress.Id,
            progress.UserId,
            progress.CurrentDayNumber,
            progress.StartedAt,
            progress.CompletedAt,
            progress.LastWorkoutCompletedAt,
            progress.IsCompleted
        );
    }
}
