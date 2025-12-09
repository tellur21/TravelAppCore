using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, ApiResponse<bool>>
    {
        private readonly IUserProfileRepository _repository;

        public UpdateUserProfileCommandHandler(IUserProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var profile = await _repository.GetByUserIdAsync(request.UserId);
                if (profile == null)
                    return ApiResponse<bool>.FailureResult("User profile not found");

                profile.FirstName = request.FirstName;
                profile.LastName = request.LastName;
                profile.DateOfBirth = request.DateOfBirth;
                profile.PhoneNumber = request.PhoneNumber;
                profile.Address = request.Address;
                profile.City = request.City;
                profile.Country = request.Country;
                profile.PassportNumber = request.PassportNumber;
                profile.PassportExpiry = request.PassportExpiry;

                await _repository.UpdateAsync(profile);
                return ApiResponse<bool>.SuccessResult(true, "Profile updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResult("Failed to update profile", new List<string> { ex.Message });
            }
        }
    }
}
