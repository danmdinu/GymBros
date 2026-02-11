using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Application.Features.Exercises;
using WorkoutApp.Application.Features.Exercises.Queries.GetExercises;

namespace WorkoutApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly ISender _mediator;

    public ExercisesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ExerciseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var exercises = await _mediator.Send(new GetExercisesQuery());
        return Ok(exercises);
    }
}
