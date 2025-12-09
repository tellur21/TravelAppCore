using Application.DTOs;
using TravelAppUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TravelAppUI.Pages
{
    public class PackagesModel : PageModel
    {
        private readonly ITravelPackageService _travelPackageService;
        private readonly ILogger<PackagesModel> _logger;

        public PackagesModel(ITravelPackageService travelPackageService, ILogger<PackagesModel> logger)
        {
            _travelPackageService = travelPackageService;
            _logger = logger;
        }

        public PagedResult<TravelPackageDto>? TravelPackages { get; set; }

        public async Task OnGetAsync(int page = 1, int pageSize = 9)
        {
            try
            {
                TravelPackages = await _travelPackageService.GetTravelPackagesAsync(page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading travel packages");
                TravelPackages = new PagedResult<TravelPackageDto>();
            }
        }
    }
}

