using Domain.Entities;

namespace Application.Interfaces;

public interface IPaymentRepository
{
    Task<Guid> AddAsync(Payment payment);
    Task<Payment?> GetByIdAsync(Guid id);
    Task<List<Payment>> GetByBookingIdAsync(Guid bookingId);
    Task<List<Payment>> GetByUserIdAsync(string userId);
    Task UpdateAsync(Payment payment);
}