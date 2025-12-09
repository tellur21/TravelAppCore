using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace TravelAppUI.Pages
{
    public class PaymentModel : PageModel
    {
        public string PublishableKey { get; }

        public PaymentModel(IConfiguration configuration)
        {
            PublishableKey = configuration["Stripe:PublishableKey"] ?? "";
        }
        public void OnGet() { }
    }
}
