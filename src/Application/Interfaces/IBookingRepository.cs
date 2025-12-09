using Domain.Entities;

namespace Application.Interfaces;

public interface IBookingRepository
{
    Task<Guid> AddAsync(Booking booking);
    Task<Booking?> GetByIdAsync(Guid id);
    Task<List<Booking>> GetByUserIdAsync(string userId);
    Task UpdateAsync(Booking booking);
    Task<(List<Booking> Items, int TotalCount)> GetPagedAsync(int page, int size, string? userId = null);
}