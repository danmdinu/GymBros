using WorkoutApp.Application.Common.Interfaces;

namespace WorkoutApp.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
