using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Application.Common.Interfaces;

namespace WorkoutApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(ICurrentUserService currentUserService, IAppDbContext context) : ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId is null)
        {
            return Unauthorized();
        }

        var user = await context.Users.FindAsync(currentUserService.UserId.Value);
        if (user is null)
        {
            return Unauthorized();
        }

        return Ok(new UserDto(
            user.Id,
            user.Email,
            user.DisplayName,
            user.PhotoUrl
        ));
    }
}

public record UserDto(
    Guid Id,
    string Email,
    string? DisplayName,
    string? PhotoUrl
);
