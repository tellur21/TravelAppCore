using Application.DTOs;
using MediatR;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TravelPackages.Queries
{
    public class GetTravelPackagesQuery : IRequest<ApiResponse<PagedResult<TravelPackageDto>>>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public string? Destination { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool IncludeNonPublished { get; set; } = false;
    }
}
