using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutApp.Application.Common.Interfaces;

namespace WorkoutApp.Application.Features.Exercises.Queries.GetExercises;

public class GetExercisesQueryHandler : IRequestHandler<GetExercisesQuery, List<ExerciseDto>>
{
    private readonly IAppDbContext _context;

    public GetExercisesQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExerciseDto>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
    {
        var exercises = await _context.Exercises
            .AsNoTracking()
            .OrderBy(e => e.Name)
            .Select(e => new ExerciseDto(
                e.Id,
                e.Name,
                e.Description,
                e.CreatedAt))
            .ToListAsync(cancellationToken);

        return exercises;
    }
}
