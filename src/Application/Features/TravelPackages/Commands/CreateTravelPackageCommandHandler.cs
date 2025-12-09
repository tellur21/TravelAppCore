using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TravelPackages.Commands
{
    public class CreateTravelPackageCommandHandler : IRequestHandler<CreateTravelPackageCommand, ApiResponse<Guid>>
    {
        private readonly ITravelPackageRepository _repository;

        public CreateTravelPackageCommandHandler(ITravelPackageRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateTravelPackageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var package = new TravelPackage
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Destination = request.Destination,
                    Price = request.Price,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    MaxCapacity = request.MaxCapacity,
                    AvailableSlots = request.MaxCapacity,
                    Status = PackageStatus.Draft,
                    ImageUrl = request.ImageUrl,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _repository.AddAsync(package);
                return ApiResponse<Guid>.SuccessResult(package.Id, "Travel package created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<Guid>.FailureResult("Failed to create travel package", new List<string> { ex.Message });
            }
        }
    }
}
