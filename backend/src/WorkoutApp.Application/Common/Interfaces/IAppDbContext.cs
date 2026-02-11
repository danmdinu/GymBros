using Microsoft.EntityFrameworkCore;
using WorkoutApp.Domain.Entities;

namespace WorkoutApp.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<Exercise> Exercises { get; }
    DbSet<WorkoutPlan> WorkoutPlans { get; }
    DbSet<Workout> Workouts { get; }
    DbSet<WorkoutSection> WorkoutSections { get; }
    DbSet<WorkoutExercise> WorkoutExercises { get; }
    DbSet<UserProgress> UserProgresses { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
