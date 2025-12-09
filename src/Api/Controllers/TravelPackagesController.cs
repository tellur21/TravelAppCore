using Application.Features.TravelPackages.Commands;
using Application.Features.TravelPackages.Queries;
using Application.DTOs;
using Domain.Enums;
using Application.UseCases;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TravelPackagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TravelPackagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetTravelPackages([FromQuery] GetTravelPackagesQuery query)
    {
        var user = HttpContext.User;

        // For non-admin/agent users, ensure we only show published packages.
        if (user.Identity?.IsAuthenticated == false || !(user.IsInRole("Admin") || user.IsInRole("Agent")))
        {
            query.IncludeNonPublished = false;
        }

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTravelPackage(Guid id)
    {
        var query = new GetTravelPackageByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result.Success)
            return Ok(result);

        return NotFound(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> CreateTravelPackage([FromBody] CreateTravelPackageCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.Success)
            return CreatedAtAction(nameof(GetTravelPackage), new { id = result.Data }, result);

        return BadRequest(result);
    }

    [HttpPatch("{id}/publish")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> PublishTravelPackage(Guid id)
    {
        var command = new UpdateTravelPackageStatusCommand { Id = id, Status = PackageStatus.Published };
        var result = await _mediator.Send(command);

        if (result.Success)
            return NoContent();

        return BadRequest(result);
    }

    [HttpPatch("{id}/suspend")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> SuspendTravelPackage(Guid id)
    {
        var command = new UpdateTravelPackageStatusCommand { Id = id, Status = PackageStatus.Suspended };
        var result = await _mediator.Send(command);

        if (result.Success)
            return NoContent();

        return BadRequest(result);
    }
}