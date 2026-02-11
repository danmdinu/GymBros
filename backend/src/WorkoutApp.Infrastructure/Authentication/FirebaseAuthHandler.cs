using System.Security.Claims;
using System.Text.Encodings.Web;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkoutApp.Domain.Entities;
using WorkoutApp.Infrastructure.Persistence;

namespace WorkoutApp.Infrastructure.Authentication;

public class FirebaseAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly AppDbContext _dbContext;

    public FirebaseAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        AppDbContext dbContext)
        : base(options, logger, encoder)
    {
        _dbContext = dbContext;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check if Firebase is initialized
        if (FirebaseApp.DefaultInstance == null)
        {
            return AuthenticateResult.NoResult();
        }

        // Get the Authorization header
        if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValue))
        {
            return AuthenticateResult.NoResult();
        }

        var authHeader = authHeaderValue.FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return AuthenticateResult.NoResult();
        }

        var token = authHeader["Bearer ".Length..];

        try
        {
            // Verify the Firebase ID token
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

            // Find or create user in database
            var user = await FindOrCreateUserAsync(decodedToken);

            // Create claims
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new("firebase_uid", decodedToken.Uid),
                new(ClaimTypes.Email, user.Email)
            };

            if (!string.IsNullOrEmpty(user.DisplayName))
            {
                claims.Add(new Claim(ClaimTypes.Name, user.DisplayName));
            }

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (FirebaseAuthException ex)
        {
            Logger.LogWarning(ex, "Firebase token validation failed");
            return AuthenticateResult.Fail("Invalid token");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Authentication error");
            return AuthenticateResult.Fail("Authentication error");
        }
    }

    private async Task<User> FindOrCreateUserAsync(FirebaseToken decodedToken)
    {
        var firebaseUid = decodedToken.Uid;
        var email = decodedToken.Claims.TryGetValue("email", out var emailClaim) 
            ? emailClaim?.ToString() ?? "" 
            : "";
        var name = decodedToken.Claims.TryGetValue("name", out var nameClaim) 
            ? nameClaim?.ToString() 
            : null;
        var picture = decodedToken.Claims.TryGetValue("picture", out var pictureClaim) 
            ? pictureClaim?.ToString() 
            : null;

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.FirebaseUid == firebaseUid);

        if (user is not null)
        {
            // Update profile if changed
            if (user.DisplayName != name || user.PhotoUrl != picture)
            {
                user.UpdateProfile(name, picture);
                await _dbContext.SaveChangesAsync();
            }
            return user;
        }

        // Create new user
        user = new User(firebaseUid, email, name, picture);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }
}
