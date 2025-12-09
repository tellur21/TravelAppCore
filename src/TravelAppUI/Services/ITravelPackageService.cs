
using Application.DTOs;

namespace TravelAppUI.Services
{
    public interface ITravelPackageService
    {
        Task<PagedResult<TravelPackageDto>> GetTravelPackagesAsync(int page = 1, int pageSize = 10);
        Task<TravelPackageDto?> GetTravelPackageByIdAsync(Guid id);
    }
}

