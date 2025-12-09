using Application.DTOs;
using MediatR;

namespace Application.Features.Admin.Commands;
public class AssignRoleToUserCommand : IRequest<ApiResponse<bool>>
{
    public required string UserId { get; set; }
    public required string RoleName { get; set; }
}