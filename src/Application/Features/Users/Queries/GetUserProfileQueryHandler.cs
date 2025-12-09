using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Queries
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, ApiResponse<UserProfileDto>>
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly IIdentityService _identityService;

        public GetUserProfileQueryHandler(IUserProfileRepository profileRepository, IIdentityService identityService)
        {
            _profileRepository = profileRepository;
            _identityService = identityService;
        }

        public async Task<ApiResponse<UserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var profile = await _profileRepository.GetByUserIdAsync(request.UserId);
                var user = await _identityService.GetUserByIdAsync(request.UserId);

                if (profile == null || user == null)
                    return ApiResponse<UserProfileDto>.FailureResult("User profile not found");

                var dto = new UserProfileDto
                {
                    UserId = profile.UserId,
                    Email = user.Email!,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    DateOfBirth = profile.DateOfBirth,
                    PhoneNumber = profile.PhoneNumber,
                    Address = profile.Address,
                    City = profile.City,
                    Country = profile.Country,
                    PassportNumber = profile.PassportNumber,
                    PassportExpiry = profile.PassportExpiry,
                    PreferredLanguage = profile.PreferredLanguage,
                    AvatarUrl = profile.AvatarUrl
                };

                return ApiResponse<UserProfileDto>.SuccessResult(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserProfileDto>.FailureResult("Failed to retrieve user profile", new List<string> { ex.Message });
            }
        }
    }
}
