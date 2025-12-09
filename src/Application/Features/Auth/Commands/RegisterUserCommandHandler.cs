using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApiResponse<AuthResult>>
    {
        private readonly IIdentityService _identityService;
        private readonly IUserProfileRepository _profileRepository;

        public RegisterUserCommandHandler(IIdentityService identityService, IUserProfileRepository profileRepository)
        {
            _identityService = identityService;
            _profileRepository = profileRepository;
        }

        public async Task<ApiResponse<AuthResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var authResult = await _identityService.RegisterAsync(request.Email, request.Password);

                if (authResult.Success)
                {
                    // Create user profile
                    var profile = new UserProfile
                    {
                        UserId = authResult.UserId,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        PhoneNumber = request.PhoneNumber,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _profileRepository.AddAsync(profile);
                }

                return ApiResponse<AuthResult>.SuccessResult(authResult, "User registered successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AuthResult>.FailureResult("Registration failed", new List<string> { ex.Message });
            }
        }
    }
}
