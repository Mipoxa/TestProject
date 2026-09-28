using Microsoft.AspNetCore.Mvc;
using TestProject.ClassLogic;

namespace TestProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingUseCases _bookingUseCases;
    public BookingsController(IBookingUseCases bookingUseCases)
    {
        _bookingUseCases = bookingUseCases;
    }
    [HttpPost("calculate-price")]
    public async Task<ActionResult<decimal>> CalculatePrice([FromBody] CalculatePriceRequest request, CancellationToken ct)
    {
        decimal price = await _bookingUseCases.CalculatePriceAsync(request, ct);
        return Ok(price);
    }
    [HttpPost]
    public async Task<ActionResult<BookingResponseDto>> CreateBooking([FromBody] CreateBookingRequest request, CancellationToken ct)
    {
        BookingResponseDto result = await _bookingUseCases.CreateBookingAsync(request, ct);
        return CreatedAtAction(nameof(CreateBooking), new { id = result.BookingId }, result);
    }
}