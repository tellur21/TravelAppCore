using Application.DTOs;
using Application.Features.Bookings.Commands;

namespace TravelAppUI.Services
{
    public interface IBookingService
    {
        Task<ApiResponse<object>> CreateBookingAsync(CreateBookingCommand createBookingCommand);
    }
}

