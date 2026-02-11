namespace WorkoutApp.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? FirebaseUid { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
}
