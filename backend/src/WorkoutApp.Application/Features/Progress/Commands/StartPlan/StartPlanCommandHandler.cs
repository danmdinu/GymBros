using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutApp.Application.Common.Interfaces;
using WorkoutApp.Domain.Entities;

namespace WorkoutApp.Application.Features.Progress.Commands.StartPlan;

public class StartPlanCommandHandler : IRequestHandler<StartPlanCommand, ProgressDto>
{
    private readonly IAppDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public StartPlanCommandHandler(IAppDbContext context, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ProgressDto> Handle(StartPlanCommand request, CancellationToken cancellationToken)
    {
        // Check if progress already exists for this user
        var existingProgress = await _context.UserProgresses
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (existingProgress is not null)
        {
            // Return existing progress instead of creating a new one
            return new ProgressDto(
                existingProgress.Id,
                existingProgress.UserId,
                existingProgress.CurrentDayNumber,
                existingProgress.StartedAt,
                existingProgress.CompletedAt,
                existingProgress.LastWorkoutCompletedAt,
                existingProgress.IsCompleted
            );
        }

        // Create new progress
        var progress = new UserProgress(request.UserId, _dateTimeProvider.UtcNow);
        _context.UserProgresses.Add(progress);
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
