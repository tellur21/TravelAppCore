using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Commands
{
    public class CreateBookingCommand : IRequest<ApiResponse<Guid>>
    {
        public Guid TravelPackageId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int NumberOfTravelers { get; set; }
        public string SpecialRequests { get; set; } = string.Empty;
        public List<BookingTravelerDto> Travelers { get; set; } = new();
    }
}
