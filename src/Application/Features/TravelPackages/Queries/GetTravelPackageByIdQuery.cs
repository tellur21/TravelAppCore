using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TravelPackages.Queries
{
    public class GetTravelPackageByIdQuery : IRequest<ApiResponse<TravelPackageDto>>
    {
        public Guid Id { get; set; }
    }
}
