using Application.DTOs;
using Domain.Enums;
using MediatR;
using System;

namespace Application.Features.TravelPackages.Commands
{
    public class UpdateTravelPackageStatusCommand : IRequest<ApiResponse<bool>>
    {
        public Guid Id { get; set; }
        public PackageStatus Status { get; set; }
    }
}