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
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly AppDbContext _db;

        public UserProfileRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<UserProfile?> GetByUserIdAsync(string userId)
        {
            return await _db.UserProfiles.FindAsync(userId);
        }

        public async Task AddAsync(UserProfile profile)
        {
            _db.UserProfiles.Add(profile);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserProfile profile)
        {
            profile.UpdatedAt = DateTime.UtcNow;
            _db.UserProfiles.Update(profile);
            await _db.SaveChangesAsync();
        }

        public async Task<(List<UserProfile> Items, int TotalCount)> GetPagedAsync(int page, int size)
        {
            var totalCount = await _db.UserProfiles.CountAsync();
            var items = await _db.UserProfiles
                .Skip((page - 1) * size)
                .Take(size)
                .OrderBy(p => p.FirstName)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
