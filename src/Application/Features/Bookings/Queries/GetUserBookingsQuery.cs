using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Queries
{
    public class GetUserBookingsQuery : IRequest<ApiResponse<PagedResult<BookingDto>>>
    {
        public string UserId { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
