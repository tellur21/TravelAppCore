using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TravelPackages.Queries
{
    public class GetTravelPackagesQueryHandler : IRequestHandler<GetTravelPackagesQuery, ApiResponse<PagedResult<TravelPackageDto>>>
    {
        private readonly ITravelPackageRepository _repository;

        public GetTravelPackagesQueryHandler(ITravelPackageRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<PagedResult<TravelPackageDto>>> Handle(GetTravelPackagesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // If IncludeNonPublished is false, we only get 'Published' packages. Otherwise, we get all.
                var statusToFilter = request.IncludeNonPublished ? (PackageStatus?)null : PackageStatus.Published;

                var (items, totalCount) = await _repository.GetPagedAsync(request.Page, request.Size, request.Destination, request.MinPrice, request.MaxPrice, statusToFilter);

                var dtos = items.Select(p => new TravelPackageDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Destination = p.Destination,
                    Price = p.Price,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    MaxCapacity = p.MaxCapacity,
                    AvailableSlots = p.AvailableSlots,
                    Status = p.Status.ToString(),
                    ImageUrl = p.ImageUrl
                }).ToList();

                var result = new PagedResult<TravelPackageDto>
                {
                    Items = dtos,
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.Size
                };

                return ApiResponse<PagedResult<TravelPackageDto>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResult<TravelPackageDto>>.FailureResult("Failed to retrieve travel packages", new List<string> { ex.Message });
            }
        }
    }
}
