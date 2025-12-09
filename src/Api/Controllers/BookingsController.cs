using Application.Features.Bookings.Commands;
using Application.Features.Bookings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetUserBookingsQuery { UserId = userId!, Page = page, Size = size };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(Guid id)
        {
            var query = new GetBookingDetailsQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
        {
            command.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var result = await _mediator.Send(command);

            if (result.Success)
                return CreatedAtAction(nameof(GetBooking), new { id = result.Data }, result);

            return BadRequest(result);
        }
    }
}
