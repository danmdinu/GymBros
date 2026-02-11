using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorkoutApp.Application.Common.Interfaces;
using WorkoutApp.Infrastructure.Persistence;
using WorkoutApp.Infrastructure.Services;

namespace WorkoutApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register services
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Register SQLite database
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=workoutapp.db"));

        // Register interface for dependency injection
        services.AddScoped<IAppDbContext>(provider => 
            provider.GetRequiredService<AppDbContext>());

        return services;
    }
}
