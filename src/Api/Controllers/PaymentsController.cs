using Application.Features.Payments.Commands;
using Application.Features.Payments.Queries;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.Success)
                return Ok(result.Data);

            return BadRequest(result);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetPaymentHistory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetPaymentHistoryQuery { UserId = userId! };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
