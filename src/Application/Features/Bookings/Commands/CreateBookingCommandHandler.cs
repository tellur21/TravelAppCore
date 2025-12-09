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

namespace Application.Features.Bookings.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, ApiResponse<Guid>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ITravelPackageRepository _packageRepository;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository, ITravelPackageRepository packageRepository)
        {
            _bookingRepository = bookingRepository;
            _packageRepository = packageRepository;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var package = await _packageRepository.GetByIdAsync(request.TravelPackageId);
                if (package == null)
                    return ApiResponse<Guid>.FailureResult("Travel package not found");

                if (package.AvailableSlots < request.NumberOfTravelers)
                    return ApiResponse<Guid>.FailureResult("Not enough available slots");

                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    TravelPackageId = request.TravelPackageId,
                    UserId = request.UserId,
                    BookingDate = DateTime.UtcNow,
                    NumberOfTravelers = request.NumberOfTravelers,
                    TotalAmount = package.Price * request.NumberOfTravelers,
                    Status = BookingStatus.Pending,
                    SpecialRequests = request.SpecialRequests,
                    CreatedAt = DateTime.UtcNow,
                    Travelers = request.Travelers.Select(t => new BookingTraveler
                    {
                        Id = Guid.NewGuid(),
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        DateOfBirth = t.DateOfBirth,
                        PassportNumber = t.PassportNumber,
                        PassportExpiry = t.PassportExpiry
                    }).ToList()
                };

                var bookingId = await _bookingRepository.AddAsync(booking);

                // Update available slots
                package.AvailableSlots -= request.NumberOfTravelers;
                await _packageRepository.UpdateAsync(package);

                return ApiResponse<Guid>.SuccessResult(bookingId, "Booking created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<Guid>.FailureResult("Failed to create booking", new List<string> { ex.Message });
            }
        }
    }
}
