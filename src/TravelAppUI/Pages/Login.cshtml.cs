using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace TravelAppUI.Pages
{
    public class LoginModel : PageModel
    {
        public string GoogleClientId { get; }
        public string ApiBaseUrl { get; }

        public LoginModel(IConfiguration configuration)
        {
            // It's better to get this from a specific section, e.g., "GoogleAuth:ClientId"
            GoogleClientId = configuration["GoogleAuth:ClientId"] ?? "";
            // Get the API base URL from configuration, with a fallback for production
            ApiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://api..travel";
        }

        public void OnGet()
        {
        }
    }
}

