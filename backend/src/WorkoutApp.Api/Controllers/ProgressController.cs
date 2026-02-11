using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Application.Common.Interfaces;
using WorkoutApp.Application.Features.Progress;
using WorkoutApp.Application.Features.Progress.Commands.CompleteWorkout;
using WorkoutApp.Application.Features.Progress.Commands.StartPlan;
using WorkoutApp.Application.Features.Progress.Queries.GetProgress;

namespace WorkoutApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProgressController(ISender mediator, ICurrentUserService currentUserService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ProgressDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProgress()
    {
        if (currentUserService.UserId is null)
            return Unauthorized();

        var progress = await mediator.Send(new GetProgressQuery(currentUserService.UserId.Value));
        
        if (progress is null)
            return NotFound();
            
        return Ok(progress);
    }

    [HttpPost("start")]
    [ProducesResponseType(typeof(ProgressDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> StartPlan()
    {
        if (currentUserService.UserId is null)
            return Unauthorized();

        var progress = await mediator.Send(new StartPlanCommand(currentUserService.UserId.Value));
        return Ok(progress);
    }

    [HttpPost("complete")]
    [ProducesResponseType(typeof(ProgressDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CompleteWorkout()
    {
        if (currentUserService.UserId is null)
            return Unauthorized();

        var progress = await mediator.Send(new CompleteWorkoutCommand(currentUserService.UserId.Value));
        
        if (progress is null)
            return NotFound();
            
        return Ok(progress);
    }
}
