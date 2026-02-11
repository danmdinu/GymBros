using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutApp.Application.Common.Interfaces;

namespace WorkoutApp.Application.Features.Workouts.Queries.GetWorkoutByDay;

public class GetWorkoutByDayQueryHandler : IRequestHandler<GetWorkoutByDayQuery, WorkoutDto?>
{
    private readonly IAppDbContext _context;

    public GetWorkoutByDayQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<WorkoutDto?> Handle(GetWorkoutByDayQuery request, CancellationToken cancellationToken)
    {
        var workout = await _context.Workouts
            .AsNoTracking()
            .Include(w => w.Sections.OrderBy(s => s.Order))
                .ThenInclude(s => s.Exercises.OrderBy(e => e.Order))
                    .ThenInclude(e => e.Exercise)
            .FirstOrDefaultAsync(w => w.DayNumber == request.DayNumber, cancellationToken);

        if (workout is null)
            return null;

        return new WorkoutDto(
            workout.Id,
            workout.DayNumber,
            workout.Name,
            workout.Description,
            workout.Sections.Select(s => new WorkoutSectionDto(
                s.Id,
                s.Order,
                s.Type.ToString(),
                s.Name,
                s.Exercises.Select(e => new WorkoutExerciseDto(
                    e.Id,
                    e.Order,
                    e.SupersetGroup,
                    e.Sets,
                    e.Metric,
                    e.Notes,
                    e.Exercise.Name,
                    e.Exercise.Description
                )).ToList()
            )).ToList()
        );
    }
}
