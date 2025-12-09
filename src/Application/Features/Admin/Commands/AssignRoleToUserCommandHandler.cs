using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Admin.Commands
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, ApiResponse<bool>>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AssignRoleToUserCommandHandler(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApiResponse<bool>> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return ApiResponse<bool>.FailureResult("User not found.");


            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExists)
                return ApiResponse<bool>.FailureResult($"Role '{request.RoleName}' not found.");


            var result = await _userManager.AddToRoleAsync(user, request.RoleName);

            if (result.Succeeded)

                return ApiResponse<bool>.SuccessResult(true);


            return ApiResponse<bool>.FailureResult(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}