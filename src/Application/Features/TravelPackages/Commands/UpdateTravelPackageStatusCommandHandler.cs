using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TravelPackages.Commands
{
    public class UpdateTravelPackageStatusCommandHandler : IRequestHandler<UpdateTravelPackageStatusCommand, ApiResponse<bool>>
    {
        private readonly ITravelPackageRepository _travelPackageRepository;

        public UpdateTravelPackageStatusCommandHandler(ITravelPackageRepository travelPackageRepository)
        {
            _travelPackageRepository = travelPackageRepository;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateTravelPackageStatusCommand request, CancellationToken cancellationToken)
        {
            var travelPackage = await _travelPackageRepository.GetByIdAsync(request.Id);
            if (travelPackage == null)
            {
                return ApiResponse<bool>.FailureResult("Travel package not found.");
            }

            // Here you could add more business logic, e.g., checking if a status transition is valid.

            travelPackage.Status = request.Status;
            travelPackage.UpdatedAt = DateTime.UtcNow;

            await _travelPackageRepository.UpdateAsync(travelPackage);

            return ApiResponse<bool>.SuccessResult(true, $"Travel package status updated to {request.Status}.");
        }
    }
}