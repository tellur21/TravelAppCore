using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class TravelPackageRepository : ITravelPackageRepository
{
    private readonly AppDbContext _db;

    public TravelPackageRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(TravelPackage package)
    {
        _db.TravelPackages.Add(package);
        await _db.SaveChangesAsync();
    }

    public async Task<List<TravelPackage>> GetAllAsync()
    {
        return await _db.TravelPackages.ToListAsync();
    }

    public async Task<TravelPackage?> GetByIdAsync(Guid id)
    {
        return await _db.TravelPackages
            .Include(p => p.Bookings)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateAsync(TravelPackage package)
    {
        package.UpdatedAt = DateTime.UtcNow;
        _db.TravelPackages.Update(package);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var package = await _db.TravelPackages.FindAsync(id);
        if (package != null)
        {
            _db.TravelPackages.Remove(package);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<TravelPackage>> SearchAsync(string query)
    {
        return await _db.TravelPackages
            .Where(p => p.Name.Contains(query) || p.Destination.Contains(query))
            .ToListAsync();
    }

    public async Task<(List<TravelPackage> Items, int TotalCount)> GetPagedAsync(int page, int size, string? destination = null, decimal? minPrice = null, decimal? maxPrice = null, PackageStatus? statusToFilter = null)
    {
        var query = _db.TravelPackages.AsQueryable();

        if (!string.IsNullOrEmpty(destination))
            query = query.Where(p => p.Destination.Contains(destination));

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);
        
        if (statusToFilter.HasValue)
            query = query.Where(p => p.Status == statusToFilter.Value);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (items, totalCount);
    }
}
