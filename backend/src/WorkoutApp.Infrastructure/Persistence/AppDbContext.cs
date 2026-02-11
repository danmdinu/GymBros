using Microsoft.EntityFrameworkCore;
using WorkoutApp.Application.Common.Interfaces;
using WorkoutApp.Domain.Entities;

namespace WorkoutApp.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public AppDbContext(DbContextOptions<AppDbContext> options, IDateTimeProvider dateTimeProvider) 
        : base(options)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<WorkoutSection> WorkoutSections => Set<WorkoutSection>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public DbSet<UserProgress> UserProgresses => Set<UserProgress>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreatedAt(_dateTimeProvider.UtcNow);
                    break;
                case EntityState.Modified:
                    entry.Entity.SetUpdatedAt(_dateTimeProvider.UtcNow);
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}
