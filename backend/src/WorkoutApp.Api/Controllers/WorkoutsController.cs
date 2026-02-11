using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Application.Features.Workouts;
using WorkoutApp.Application.Features.Workouts.Queries.GetWorkoutByDay;

namespace WorkoutApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutsController : ControllerBase
{
    private readonly ISender _mediator;

    public WorkoutsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("day/{dayNumber:int}")]
    [ProducesResponseType(typeof(WorkoutDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByDay(int dayNumber)
    {
        var workout = await _mediator.Send(new GetWorkoutByDayQuery(dayNumber));
        
        if (workout is null)
            return NotFound();
            
        return Ok(workout);
    }
}
