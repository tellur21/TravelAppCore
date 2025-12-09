using Application.Features.Admin.Commands; // Assuming the command will be in this namespace
using Application.Features.Admin.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var query = new GetDashboardAnalyticsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("health")]
        public async Task<IActionResult> GetSystemHealth()
        {
            var query = new GetSystemHealthQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("users/assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleToUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.Success)
                return NoContent();

            return BadRequest(result);
        }
    }
}
