
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;
using Microsoft.AspNetCore.Authorization;

namespace robot_controller_api.Controllers;

/// <summary>
/// Provides endpoints to manage robot commands.
/// </summary>
[ApiController]
[Route("api/robot-commands")]
public class RobotCommandsController : ControllerBase
{
    private readonly RobotCommandRepository _repository = new();

    /// <summary>
    /// Retrieves all robot commands.
    /// </summary>
    /// <returns>A list of robot commands.</returns>
    /// <response code="200">Returns all robot commands.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public IEnumerable<RobotCommand> GetAllRobotCommands()
    {
        return _repository.GetRobotCommands();
    }

    /// <summary>
    /// Retrieves a robot command by its ID.
    /// </summary>
    /// <param name="id">The ID of the robot command.</param>
    /// <returns>The requested robot command.</returns>
    /// <response code="200">Returns the robot command.</response>
    /// <response code="404">If the robot command is not found.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public IActionResult GetRobotCommandById(int id)
    {
        var command = _repository.GetRobotCommandById(id);

        if (command == null)
            return NotFound();

        return Ok(command);
    }
}