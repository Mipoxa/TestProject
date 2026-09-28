using Microsoft.AspNetCore.Mvc;
using TestProject.ClassLogic;

namespace TestProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomUseCases _roomUseCases;

    public RoomsController(IRoomUseCases roomUseCases)
    {
        _roomUseCases = roomUseCases;
    }
    [HttpPost]
    public async Task<ActionResult<RoomResponseDto>> CreateRoom([FromBody] CreateRoomRequest request, CancellationToken ct)
    {
        RoomResponseDto result = await _roomUseCases.CreateRoomAsync(request, ct);
        return CreatedAtAction(nameof(GetAvailableRooms), new { id = result.Id }, result);
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomRequest request, CancellationToken ct)
    {
        await _roomUseCases.UpdateRoomAsync(id, request, ct);
        return NoContent();
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRoom(Guid id, CancellationToken ct)
    {
        await _roomUseCases.DeleteRoomAsync(id, ct);
        return NoContent();
    }
    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAvailableRooms([FromQuery] SearchAvailableRoomsRequest request, CancellationToken ct)
    {
        IEnumerable<RoomResponseDto> result = await _roomUseCases.GetAvailableRoomsAsync(request, ct);
        return Ok(result);
    }
}