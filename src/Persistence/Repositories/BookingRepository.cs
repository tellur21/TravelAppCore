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
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _db;

        public BookingRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> AddAsync(Booking booking)
        {
            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();
            return booking.Id;
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _db.Bookings
                .Include(b => b.TravelPackage)
                .Include(b => b.Travelers)
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Booking>> GetByUserIdAsync(string userId)
        {
            return await _db.Bookings
                .Include(b => b.TravelPackage)
                .Include(b => b.Travelers)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(Booking booking)
        {
            _db.Bookings.Update(booking);
            await _db.SaveChangesAsync();
        }

        public async Task<(List<Booking> Items, int TotalCount)> GetPagedAsync(int page, int size, string? userId = null)
        {
            var query = _db.Bookings
                .Include(b => b.TravelPackage)
                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(b => b.UserId == userId);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * size)
                .Take(size)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
