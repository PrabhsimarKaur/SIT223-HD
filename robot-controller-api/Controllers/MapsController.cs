

using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;

namespace robot_controller_api.Controllers;

/// <summary>
/// Provides endpoints to manage and validate robot maps.
/// </summary>
[ApiController]
[Route("api/maps")]
public class MapsController : ControllerBase
{
    private static readonly List<Map> _maps = new()
    {
        new Map { Id = 1, Name = "MOON", Columns = 10, Rows = 10 },
        new Map { Id = 2, Name = "MOON2", Columns = 5, Rows = 5 },
        new Map { Id = 3, Name = "MOON3", Columns = 8, Rows = 8 }
    };

    /// <summary>
    /// Retrieves all available maps.
    /// </summary>
    /// <returns>A list of maps.</returns>
    /// <response code="200">Returns all maps.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public IEnumerable<Map> GetAllMaps()
    {
        return _maps;
    }

    /// <summary>
    /// Checks whether a given coordinate (x, y) exists within a specific map.
    /// </summary>
    /// <param name="id">The ID of the map.</param>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    /// <returns>True if the coordinate is within the map boundaries; otherwise false.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/maps/1/3-4
    ///
    /// </remarks>
    /// <response code="200">Returns true or false.</response>
    /// <response code="400">If x or y is negative.</response>
    /// <response code="404">If the map is not found.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}/{x}-{y}")]
    public IActionResult CheckCoordinate(int id, int x, int y)
    {
        if (x < 0 || y < 0)
            return BadRequest();

        var map = _maps.FirstOrDefault(m => m.Id == id);

        if (map == null)
            return NotFound();

        bool isOnMap = x < map.Columns && y < map.Rows;

        return Ok(isOnMap);
    }
}