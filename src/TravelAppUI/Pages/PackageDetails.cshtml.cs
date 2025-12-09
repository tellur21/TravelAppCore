using Application.DTOs;
using Application.Features.Bookings.Commands;
using TravelAppUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TravelAppUI.Pages
{
    public class PackageDetailsModel : PageModel
    {
        private readonly ITravelPackageService _travelPackageService;
        private readonly IBookingService _bookingService;
        private readonly ILogger<PackageDetailsModel> _logger;

        public PackageDetailsModel(
            ITravelPackageService travelPackageService,
            IBookingService bookingService,
            ILogger<PackageDetailsModel> logger)
        {
            _travelPackageService = travelPackageService;
            _bookingService = bookingService;
            _logger = logger;
        }

        public TravelPackageDto? TravelPackage { get; set; }
        
        [BindProperty]
        public CreateBookingCommand createBookingCommand { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                TravelPackage = await _travelPackageService.GetTravelPackageByIdAsync(id);
                
                if (TravelPackage == null)
                {
                    return NotFound();
                }
                
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading travel package {Id}", id);
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reload the package data for the form
                TravelPackage = await _travelPackageService.GetTravelPackageByIdAsync(createBookingCommand.TravelPackageId);
                return Page();
            }

            try
            {
                var result = await _bookingService.CreateBookingAsync(createBookingCommand);
                
                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Your booking has been submitted successfully! We'll contact you soon to confirm the details.";
                    return RedirectToPage("/Packages");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    // Reload the package data for the form
                    TravelPackage = await _travelPackageService.GetTravelPackageByIdAsync(createBookingCommand.TravelPackageId);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating booking");
                ModelState.AddModelError("", "An error occurred while processing your booking. Please try again.");
                
                // Reload the package data for the form
                TravelPackage = await _travelPackageService.GetTravelPackageByIdAsync(createBookingCommand.TravelPackageId);
                return Page();
            }
        }
    }
}

