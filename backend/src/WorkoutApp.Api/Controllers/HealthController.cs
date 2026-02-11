using Microsoft.AspNetCore.Mvc;

namespace WorkoutApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        var response = new HealthResponse(
            Status: "healthy",
            Timestamp: DateTime.UtcNow
        );
        
        return Ok(response);
    }
}

public record HealthResponse(string Status, DateTime Timestamp);
