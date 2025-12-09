using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _db;

        public PaymentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> AddAsync(Payment payment)
        {
            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();
            return payment.Id;
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await _db.Payments
                .Include(p => p.Booking)
                .ThenInclude(b => b.TravelPackage)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Payment>> GetByBookingIdAsync(Guid bookingId)
        {
            return await _db.Payments
                .Where(p => p.BookingId == bookingId)
                .OrderByDescending(p => p.ProcessedAt)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetByUserIdAsync(string userId)
        {
            return await _db.Payments
                .Include(p => p.Booking)
                .ThenInclude(b => b.TravelPackage)
                .Where(p => p.Booking.UserId == userId)
                .OrderByDescending(p => p.ProcessedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _db.Payments.Update(payment);
            await _db.SaveChangesAsync();
        }
    }
}
