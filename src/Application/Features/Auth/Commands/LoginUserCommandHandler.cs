using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ApiResponse<AuthResult>>
    {
        private readonly IIdentityService _identityService;

        public LoginUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ApiResponse<AuthResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _identityService.LoginAsync(request.Email, request.Password);
                return ApiResponse<AuthResult>.SuccessResult(result, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<AuthResult>.FailureResult("Login failed", new List<string> { ex.Message });
            }
        }
    }
}
