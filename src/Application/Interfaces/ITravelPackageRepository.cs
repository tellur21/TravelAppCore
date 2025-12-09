using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces;

public interface ITravelPackageRepository
{
    Task AddAsync(TravelPackage package);
    Task<List<TravelPackage>> GetAllAsync();
    Task<TravelPackage?> GetByIdAsync(Guid id);
    Task UpdateAsync(TravelPackage package);
    Task DeleteAsync(Guid id);
    Task<List<TravelPackage>> SearchAsync(string query);
    Task<(List<TravelPackage> Items, int TotalCount)> GetPagedAsync(int page, int size, string? destination = null, decimal? minPrice = null, decimal? maxPrice = null, PackageStatus? statusToFilter = null);
}
