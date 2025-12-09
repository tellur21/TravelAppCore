using Application.DTOs;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TravelPackages.Queries
{
    public class GetTravelPackageByIdQueryHandler : IRequestHandler<GetTravelPackageByIdQuery, ApiResponse<TravelPackageDto>>
    {
        private readonly ITravelPackageRepository _repository;

        public GetTravelPackageByIdQueryHandler(ITravelPackageRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<TravelPackageDto>> Handle(GetTravelPackageByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var package = await _repository.GetByIdAsync(request.Id);
                if (package == null)
                    return ApiResponse<TravelPackageDto>.FailureResult("Travel package not found");

                var dto = new TravelPackageDto
                {
                    Id = package.Id,
                    Name = package.Name,
                    Destination = package.Destination,
                    Price = package.Price,
                    Description = package.Description,
                    StartDate = package.StartDate,
                    EndDate = package.EndDate,
                    MaxCapacity = package.MaxCapacity,
                    AvailableSlots = package.AvailableSlots,
                    Status = package.Status.ToString(),
                    ImageUrl = package.ImageUrl
                };

                return ApiResponse<TravelPackageDto>.SuccessResult(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<TravelPackageDto>.FailureResult("Failed to retrieve travel package", new List<string> { ex.Message });
            }
        }
    }
}
