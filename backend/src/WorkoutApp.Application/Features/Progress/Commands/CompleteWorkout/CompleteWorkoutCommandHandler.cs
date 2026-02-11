using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutApp.Application.Common.Interfaces;

namespace WorkoutApp.Application.Features.Progress.Commands.CompleteWorkout;

public class CompleteWorkoutCommandHandler : IRequestHandler<CompleteWorkoutCommand, ProgressDto?>
{
    private readonly IAppDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CompleteWorkoutCommandHandler(IAppDbContext context, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ProgressDto?> Handle(CompleteWorkoutCommand request, CancellationToken cancellationToken)
    {
        var progress = await _context.UserProgresses
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (progress is null)
            return null;

        // Already completed the entire plan
        if (progress.IsCompleted)
        {
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

        progress.CompleteCurrentWorkout(_dateTimeProvider.UtcNow);
        await _context.SaveChangesAsync(cancellationToken);

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
