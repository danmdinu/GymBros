namespace WorkoutApp.Domain.Entities;

public class User : BaseEntity
{
    public string FirebaseUid { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? DisplayName { get; private set; }
    public string? PhotoUrl { get; private set; }

    // Navigation
    public ICollection<UserProgress> Progress { get; } = [];

    private User() { }

    public User(string firebaseUid, string email, string? displayName = null, string? photoUrl = null)
    {
        FirebaseUid = firebaseUid;
        Email = email;
        DisplayName = displayName;
        PhotoUrl = photoUrl;
    }

    public void UpdateProfile(string? displayName, string? photoUrl)
    {
        DisplayName = displayName;
        PhotoUrl = photoUrl;
    }

    public void UpdateEmail(string email)
    {
        Email = email;
    }
}
